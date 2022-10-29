Imports Un4seen.Bass

Public Structure AudioDevice
    Friend ID As Int32
    Friend BaseName As String
    Friend BaseType As AudioDeviceType
    Friend BaseAvailable As Boolean
    
    Public Readonly Property Name As String
        Get
            Return BaseName
        End Get
    End Property
    
    Public Readonly Property Available As Boolean
        Get
            Return BaseAvailable
        End Get
    End Property
    
    Public Readonly Property Type As AudioDeviceType
        Get
            Return BaseType
        End Get
    End Property
    
    Public Overrides Function ToString() As String
        Return "{Name: " + BaseName.ToString() + ", Type: " + BaseType.ToString() + ", Ready: " + BaseAvailable.ToString() + "}"
    End Function
    
    Public Sub Dispose()
        If Type = AudioDeviceType.Input
            Bass.BASS_RecordSetDevice(ID)
            Bass.BASS_RecordFree()
        ElseIf Type = AudioDeviceType.Output
            Bass.BASS_SetDevice(ID)
            Bass.BASS_Free()
        End If
    End Sub
End Structure