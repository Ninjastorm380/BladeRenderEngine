Public Class AudioFXWriter : Implements IDisposable
    Private ReadOnly AudioStreamRingBuffer As AudioStream()
    Private ReadOnly AudioStreamRingMax As UInt32 = 0
    Private AudioStreamRingPointer As UInt32 = 0
    Private ReadOnly Device As AudioDevice
    Friend IsDisposed As Boolean = False
    Shared Friend RefList As New List(Of AudioFXWriter)
    Public Sub New(MaxRunningStreams As UInt32, Device As AudioDevice)
        AudioStreamRingMax = MaxRunningStreams
        AudioStreamRingPointer = 0
        Me.Device = Device
        ReDim AudioStreamRingBuffer(AudioStreamRingMax - 1)
        For X = 0 To MaxRunningStreams - 1
            AudioStreamRingBuffer(X) = New AudioStream(Me.Device)
        Next
        RefList.Add(Me)
    End Sub
    
    Public Sub PlayAsync(Sample As AudioData)
        AudioStreamRingBuffer(AudioStreamRingPointer).PlayAsync(Sample, False)
        AudioStreamRingPointer += 1
        If AudioStreamRingPointer >= AudioStreamRingMax Then AudioStreamRingPointer = 0
    End Sub
    
    Public Property Volume As Single
        Get
            If Device.Type = AudioDeviceType.Output
                Return AudioStreamRingBuffer(0).Volume
            Else 
                Return 1.0
            End If
        End Get
        Set
            If Device.Type = AudioDeviceType.Output
                For X = 0 To AudioStreamRingMax - 1
                    AudioStreamRingBuffer(X).Volume = Value
                Next
            End If
        End Set
    End Property

    Public Sub Dispose() Implements IDisposable.Dispose
        If IsDisposed = False Then
            For X = 0 To AudioStreamRingMax - 1
                If AudioStreamRingBuffer(X).IsDisposed = False Then
                    AudioStreamRingBuffer(X).Dispose()
                End If
            Next
            RefList.Remove(Me)
            IsDisposed = True
        End If
    End Sub
End Class