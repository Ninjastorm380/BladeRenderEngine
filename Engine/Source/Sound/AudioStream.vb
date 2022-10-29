Imports System.Linq.Expressions
Imports System.Runtime.InteropServices
Imports System.Threading
Imports Un4seen.Bass

Public Class AudioStream : Implements IDisposable
    
    Private Device As AudioDevice
    Friend IsDisposed As Boolean = False
    Shared Friend RefList As New List(Of AudioStream)

    Private ReadTempBuffer(1048575) As Byte
    Private ReadBuffer As New QueueStream(Of Byte)
    Private ReadLock As New Object
    
    Dim WriteTempBuffer(1048575) As Byte
    Private WriteBuffer As New QueueStream(Of Byte)
    Private WriteLock As New Object
    
    Private ReadHandle As Integer
    Private ReadMethodHandle As RECORDPROC 'audio input
    
    Private WriteHandle As Integer
    Private WriteMethodHandle As STREAMPROC 'audio output
    
    Private NeedsInit As Boolean = True
    Private DoLoop As Boolean = False
    
    Public Sub New(Device As AudioDevice)
        Me.Device = Device
        Dim AsyncRecreate As New Threading.Thread(AddressOF RecreateStream)
        AsyncRecreate.start
        RefList.Add(Me)
    End Sub
    
    Friend Sub RecreateStream()
        If Device.Type = AudioDeviceType.Input
            Bass.BASS_RecordSetDevice(Device.ID)
            Bass.BASS_ChannelStop(ReadHandle)
            ReadMethodHandle = New RECORDPROC(AddressOf StreamReader)
            ReadHandle = Bass.BASS_RecordStart(44100, 2, BASSFlag.BASS_RECORD_PAUSE And BASSFlag.BASS_SAMPLE_FLOAT, ReadMethodHandle, IntPtr.Zero)
            Bass.BASS_ChannelPlay(ReadHandle, True)
        ElseIf Device.Type = AudioDeviceType.Output
            Bass.BASS_SetDevice(Device.ID)
            Bass.BASS_ChannelStop(WriteHandle)
            WriteBuffer.Clear()
            Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_UPDATEPERIOD, 0)
            WriteMethodHandle = New STREAMPROC(AddressOf StreamWriter)
            WriteHandle = Bass.BASS_StreamCreate(44100, 2, BASSFlag.BASS_DEFAULT, WriteMethodHandle, IntPtr.Zero)
            Bass.BASS_ChannelPlay(WriteHandle, True)
        End If
    End Sub
    
    Private Function StreamReader(handle As Integer, buffer As IntPtr, length As Integer, user As IntPtr) As Boolean
        SyncLock ReadLock
        If length > 0
            Bass.BASS_RecordSetDevice(Device.ID)
            If ReadTempBuffer.Length < length Then Redim ReadTempBuffer(length - 1)
            Marshal.Copy(buffer, ReadTempBuffer, 0, length)
            ReadBuffer.Write(ReadTempBuffer, 0, length)
        End If
        Return True
        End SyncLock
    End Function 
    
    Dim SWLength As Integer
    Private Function StreamWriter(ByVal handle As Integer, ByVal buffer As IntPtr, ByVal length As Integer, ByVal user As IntPtr) As Integer
        SWLength = 0
        SyncLock WriteLock
        If WriteBuffer.Length > 0
            Bass.BASS_SetDevice(Device.ID)
            If WriteTempBuffer.Length < length Then Redim WriteTempBuffer(length - 1)
            If WriteBuffer.Length <= length Then
                WriteBuffer.Read(WriteTempBuffer, 0, 0, WriteBuffer.Length)
                Marshal.Copy(WriteTempBuffer, 0, buffer, WriteBuffer.Length)
                SWLength = WriteBuffer.Length
                WriteBuffer.Shift(WriteBuffer.Length)
            Else
                WriteBuffer.Read(WriteTempBuffer, 0, 0, length)
                Marshal.Copy(WriteTempBuffer, 0, buffer, length)
                SWLength = length
                WriteBuffer.Shift(length)
            End If
        End If
        End SyncLock
        Return SWLength
    End Function
    
    Public Sub Write(ByRef AudioData as Byte())
        If Device.Type = AudioDeviceType.Input Then Return
        SyncLock WriteLock
            If AudioData IsNot Nothing Then 
                WriteBuffer.Write(AudioData, 0, AudioData.Length)
            End If
        End SyncLock
    End Sub
    
    Public Sub Read(ByRef AudioData as Byte())
        If Device.Type = AudioDeviceType.Output Then Return
        SyncLock ReadLock
            ReadBuffer.Read(AudioData, 0, 0, AudioData.Count())
            ReadBuffer.Shift(AudioData.Count())
        End SyncLock
    End Sub
    Public ReadOnly Property Length As Int32
        Get
            If Device.Type = AudioDeviceType.Output Then
                SyncLock WriteLock
                    Return WriteBuffer.Length
                End SyncLock
            ElseIf Device.Type = AudioDeviceType.Input
                SyncLock ReadLock
                    Return ReadBuffer.Length
                End SyncLock
            End If
            Return 0
        End Get
    End Property
    
    Public Property Volume As Single
        Get
            Dim Result As Single = 0
            If Device.Type = AudioDeviceType.Output
                Bass.BASS_ChannelGetAttribute(WriteHandle, BASSAttribute.BASS_ATTRIB_VOL, Result)
            End If
            Return Result
        End Get
        Set
            If Device.Type = AudioDeviceType.Output
                Bass.BASS_ChannelSetAttribute(WriteHandle, BASSAttribute.BASS_ATTRIB_VOL, Value)
            End If
        End Set
    End Property

    Private ThreadLock As New Object
    Public Sub PlayAsync(Data As AudioData, Optional [Loop] As Boolean = False)
        If Device.Type = AudioDeviceType.Input Then Return
        Synclock ThreadLock
            DoLoop = [Loop]
            If NeedsInit = True Then
                RecreateStream()
                NeedsInit = False
            End If
            Write(Data.BackBuffer)
            If DoLoop = False Then Return
            Dim AsyncWatcher As New Thread(
                Sub()
                    Synclock ThreadLock
                        Dim Governor As New Governor(30)
                        Do
                            If DoLoop = False Then Exit Do
                            If Length = 0 And Data IsNot Nothing Then
                                Write(Data.BackBuffer)
                            End If
                            If DoLoop = False Then Exit Do
                            Governor.Limit()
                        Loop While DoLoop = True
                        RecreateStream()
                    End SyncLock
                End sub)
            AsyncWatcher.Start()
        End SyncLock
    End Sub
    
    Public Sub StopAsync()
        DoLoop = False
    End Sub
    
    Public Sub Dispose() Implements IDisposable.Dispose
        If IsDisposed = False Then
            If Device.Type = AudioDeviceType.Input
                Bass.BASS_RecordSetDevice(Device.ID)
                Bass.BASS_ChannelStop(ReadHandle)
            ElseIf Device.Type = AudioDeviceType.Output
                Bass.BASS_SetDevice(Device.ID)
                Bass.BASS_ChannelStop(WriteHandle)
                WriteBuffer.Clear()
            End If
            RefList.Remove(Me)
            IsDisposed = True
        End If
    End Sub
End Class