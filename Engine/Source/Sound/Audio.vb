Imports System.Threading
Imports Un4seen.Bass

Public Module Audio
    Private AudioStop As Boolean = False
    Public Function GetInputDevices As AudioDevice()
         Dim Result As New List(Of AudioDevice)
        
         For InputDeviceIndex As Integer = -1 To Bass.BASS_RecordGetDeviceCount - 1
             Dim Info As BASS_DEVICEINFO = Bass.BASS_RecordGetDeviceInfo(InputDeviceIndex)

             Dim Device As New AudioDevice
             Device.BaseType = AudioDeviceType.Input
             Device.ID = InputDeviceIndex
             Bass.BASS_RecordInit(Device.ID)
             If Info IsNot Nothing Then
                 Select Case Info.name.ToLower()
                     Case "default"
                     Case "no sound"
                     Case Else
                         Device.BaseName = Info.name
                         Select Case CByte(Info.status)
                             Case 1 'Enabled
                                 Device.BaseAvailable = True 
                             Case 5 'Enabled and Initialized
                                 Device.BaseAvailable = True
                             Case 3 'Enabled and Default
                                 Device.BaseAvailable = True
                             Case 7 'Enabled, Initialized, and Default
                                 Device.BaseAvailable = True
                             Case Else 'Dead device, mark as not available.
                                 Device.BaseAvailable = False
                         End Select
                         Result.Add(Device) 
                 End Select
             Else
                 If Device.ID = -1 Then 
                     Device.BaseName = "Default input device"
                     Device.BaseAvailable = True
                 Else
                     Device.BaseName = "Unknown input device"
                     Device.BaseAvailable = False
                 End If
                 Result.Add(Device)
             End If
         Next
         Return Result.ToArray()
    End Function
    
    Public Function GetOutputDevices As AudioDevice()
        Dim Result As New List(Of AudioDevice)
        
        For InputDeviceIndex As Integer = -1 To Bass.BASS_GetDeviceCount - 1
            Dim Info As BASS_DEVICEINFO = Bass.BASS_GetDeviceInfo(InputDeviceIndex)
            Dim Device As New AudioDevice
            Device.BaseType = AudioDeviceType.Output
            Device.ID = InputDeviceIndex
            Bass.BASS_Init(Device.ID, 48000, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero)
            If Info IsNot Nothing Then
                Select Case Info.name.ToLower()
                    Case "default"
                    Case "no sound"
                    Case Else
                        Device.BaseName = Info.name
                        Select Case CByte(Info.status)
                            Case 1 'Enabled
                                Device.BaseAvailable = True 
                            Case 5 'Enabled and Initialized
                                Device.BaseAvailable = True
                            Case 3 'Enabled and Default
                                Device.BaseAvailable = True
                            Case 7 'Enabled, Initialized, and Default
                                Device.BaseAvailable = True
                            Case Else 'Dead device, mark as not available.
                                Device.BaseAvailable = False
                        End Select
                        Result.Add(Device) 
                End Select
            Else
                If Device.ID = -1 Then 
                    Device.BaseName = "Default output device"
                    Device.BaseAvailable = True
                Else
                    Device.BaseName = "Unknown output device"
                    Device.BaseAvailable = False
                End If
                Result.Add(Device)
            End If
        Next
        Return Result.ToArray()
    End Function
    
    Friend Sub LoadAudioEngine
        AudioStop = False
        Dim Governor As new Governor(1000)
        Dim AsyncThread As New Thread(
            Sub()
                Do While AudioStop = False
                    Bass.BASS_Update(150)
                    If AudioStop = True Then Exit Do
                    Governor.Limit()
                Loop
            End Sub)
        AsyncThread.Start()
    End Sub
    Friend Sub UnloadAudioEngine
        AudioStop = True
    End Sub
    Public Property CurrentOutputDevice As AudioDevice = GetOutputDevices(0)
    Public Property CurrentInputDevice As AudioDevice = GetInputDevices(0)
End Module