
Option Explicit On
Option Strict On

Imports System
Imports Un4seen.Bass

Public Class InputDeviceStream
    Private Device As InputDevice
    Private Handle As Integer
    Private BaseBuffer As Byte()
    Private Initialized As Boolean = False
    Private MethodHandle As New RECORDPROC(AddressOf Recorder)
    Public Sub Load(Device As InputDevice, Optional MaxFrequency As Integer = 44100)
        Me.Device = Device
        If Initialized = False Then
            Bass.BASS_RecordInit(Device.ID)
            Handle = Bass.BASS_RecordStart(MaxFrequency, 2, BASSFlag.BASS_RECORD_PAUSE, MethodHandle, IntPtr.Zero)
            Initialized = True
        Else
            Bass.BASS_RecordSetDevice(Device.ID)
        End If
        Bass.BASS_ChannelPlay(Handle, True)
    End Sub
    Public Sub Unload()
        If Initialized = True Then
            Bass.BASS_ChannelStop(Handle)
            Bass.BASS_RecordSetDevice(Device.ID)
            Bass.BASS_RecordFree()
            Initialized = False
        End If
    End Sub
    Public Sub SetDevice(Device As InputDevice, Optional MaxFrequency As Integer = 44100)
        If Initialized = True Then
            Unload()
            Load(Device, MaxFrequency)
        End If
    End Sub
    Private Function Recorder(handle As Integer, buffer As IntPtr, length As Integer, user As IntPtr) As Boolean
        SyncLock Me
            ReDim BaseBuffer(length - 1)
            Runtime.InteropServices.Marshal.Copy(buffer, BaseBuffer, 0, BaseBuffer.Length)
            Return True
        End SyncLock
    End Function
    Public Function Read() As Byte()
        SyncLock Me
            Return BaseBuffer
        End SyncLock
    End Function
End Class
Public Class OutputDeviceStream
    Private Device As OutputDevice
    Private Handle As Integer
    Private BaseBuffer As Byte()
    Private Initialized As Boolean = False
    Private NewData As Boolean = False
    Private MethodHandle As New STREAMPROC(AddressOf BufferReader)
    Private VolumeBase As Single = 1.0 : Public Property Volume As Integer
        Get
            Return CInt(VolumeBase * 100.0)
        End Get
        Set(value As Integer)
            VolumeBase = CSng(value / 100.0)
            If Initialized = True Then
                Bass.BASS_ChannelSetAttribute(Handle, BASSAttribute.BASS_ATTRIB_VOL, VolumeBase)
            End If
        End Set
    End Property
    Public Sub Load(Device As OutputDevice, Optional MaxFrequency As Integer = 44100)
        Me.Device = Device
        If Initialized = False Then
            Bass.BASS_Init(Device.ID, MaxFrequency, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero)
            Initialized = True
        Else
            Bass.BASS_SetDevice(Device.ID)
        End If
        Handle = Bass.BASS_StreamCreate(MaxFrequency, 2, BASSFlag.BASS_DEFAULT, MethodHandle, IntPtr.Zero)
        Bass.BASS_ChannelSetAttribute(Handle, BASSAttribute.BASS_ATTRIB_VOL, VolumeBase)
        Bass.BASS_ChannelPlay(Handle, True)
    End Sub
    Public Sub Unload()
        If Initialized = True Then
            Bass.BASS_ChannelStop(Handle)
            Bass.BASS_SetDevice(Device.ID)
            Bass.BASS_Free()
            Initialized = False
        End If
    End Sub
    Public Sub SetDevice(Device As OutputDevice, Optional MaxFrequency As Integer = 44100)
        If Initialized = True Then
            Unload()
            Load(Device, MaxFrequency)
        End If
    End Sub
    Private Function BufferReader(ByVal handle As Integer, ByVal buffer As IntPtr, ByVal length As Integer, ByVal user As IntPtr) As Integer
        If BaseBuffer Is Nothing Then Return 0
        SyncLock Me
            If NewData = True Then
                NewData = False
                Runtime.InteropServices.Marshal.Copy(BaseBuffer, 0, buffer, BaseBuffer.Length)
                Return BaseBuffer.Length
            Else
                Return 0
            End If
        End SyncLock

    End Function
    Public Sub Write(ByVal Input As Byte())
        SyncLock Me
            If Initialized = True Then
                BaseBuffer = Input
                NewData = True
            End If
        End SyncLock
    End Sub
End Class
Public Class AudioFileStream
    Private Device As OutputDevice
    Private Handle As Integer
    Private Initialized As Boolean = False
    Private Playing As Boolean = False
    Private Path As String
    Public Event EffectFinished As EventHandler
    Public Property Repeat As Boolean = False
    Private VolumeBase As Single = 1.0 : Public Property Volume As Integer
        Get
            Return CInt(VolumeBase * 100.0)
        End Get
        Set(value As Integer)
            VolumeBase = CSng(value / 100.0)
            If Initialized = True Then
                Bass.BASS_ChannelSetAttribute(Handle, BASSAttribute.BASS_ATTRIB_VOL, VolumeBase)
            End If
        End Set
    End Property
    Sub Load(AbsolutePath As String, Device As OutputDevice, Optional Repeat As Boolean = False)
        Me.Device = Device
        Me.Path = AbsolutePath
        Me.Repeat = Repeat
        If Initialized = False Then
            Bass.BASS_Init(Device.ID, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero)
            Initialized = True
        Else
            Bass.BASS_SetDevice(Device.ID)
        End If
        Handle = Bass.BASS_StreamCreateFile(AbsolutePath, 0, 0, BASSFlag.BASS_DEFAULT)
        Bass.BASS_ChannelSetAttribute(Handle, BASSAttribute.BASS_ATTRIB_VOL, VolumeBase)
    End Sub
    Sub Unload()
        If Initialized = True Then
            [Stop]()
            Bass.BASS_SetDevice(Device.ID)
            Bass.BASS_Free()

            Initialized = False
        End If
    End Sub
    Public Sub SetDevice(Device As OutputDevice)
        If Initialized = True Then
            Unload()
            Load(Path, Device)
        End If
    End Sub
    Public Sub PlayAsync(Optional Restart As Boolean = True)
        If Initialized = True Then Bass.BASS_ChannelPlay(Handle, Restart)
        Dim AsyncThread As New Threading.Thread(AddressOf NewThreadHandler)
        AsyncThread.Start()
    End Sub
    Public Sub Pause()
        If Initialized = True Then Bass.BASS_ChannelPause(Handle)
    End Sub
    Public Sub [Resume]()
        If Initialized = True Then Bass.BASS_ChannelPlay(Handle, False)
    End Sub
    Public Sub [Stop]()
        If Initialized = True Then Bass.BASS_ChannelStop(Handle)
        Repeat = False
        Playing = False
    End Sub
    Private Sub NewThreadHandler()
        Dim Limiter As New ThreadLimiter(10)
        Playing = True
        While Playing = True
            Dim StreamLength As Long = Bass.BASS_ChannelGetLength(Handle)
            Dim StreamPosition As Long = Bass.BASS_ChannelGetPosition(Handle)
            If StreamPosition = StreamLength Then Exit While
            Limiter.Limit()
        End While
        Playing = False
        RaiseEvent EffectFinished(Me, New EventArgs)
        If Repeat = True Then PlayAsync()
    End Sub
End Class
