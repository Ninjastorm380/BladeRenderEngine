Imports System.Runtime.InteropServices
Imports Un4seen.Bass

Public Class AudioData : Implements IDisposable
    Friend BackBuffer As Byte()
    Friend Length As Int32
    
    Friend IsDisposed As Boolean = False
    Shared Friend RefList As New List(Of AudioData)
    
    Public Sub New(Path As String)
        Bass.BASS_Init(0, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero)
        Dim StreamHandle As Int32 = Bass.BASS_StreamCreateFile(Path,0,0,BASSFlag.BASS_STREAM_DECODE)
        Length = Bass.BASS_ChannelGetLength(StreamHandle, BASSMode.BASS_POS_BYTE)
        ReDim BackBuffer(Length - 1)
        Bass.BASS_ChannelGetData(StreamHandle, BackBuffer, Length)
        
        Bass.BASS_SetDevice(0)
        Bass.BASS_StreamFree(StreamHandle)
        Bass.BASS_Free()
        RefList.Add(Me)
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        If IsDisposed = False Then
            BackBuffer = Nothing
            RefList.Remove(Me)
            IsDisposed = True
        End If

    End Sub
End Class