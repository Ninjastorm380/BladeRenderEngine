
Option Explicit On
Option Strict On

Imports System
Imports Un4seen.Bass

Public Class InputDevice
    Private BaseID As Integer
    Private BaseName As String
    Private BaseUsable As Boolean = False
    Sub New(Optional ID As Integer = -1)
        BaseID = ID
        If ID = -1 Then
            BaseName = "default"
            BaseUsable = True
            Exit Sub
        End If
        Dim Info As BASS_DEVICEINFO = Bass.BASS_RecordGetDeviceInfo(ID)
        If Info IsNot Nothing Then
            BaseName = Info.name
            Select Case Info.status
                Case BASSDeviceInfo.BASS_DEVICE_ENABLED
                    BaseUsable = True
                Case BASSDeviceInfo.BASS_DEVICE_ENABLED And BASSDeviceInfo.BASS_DEVICE_INIT
                    BaseUsable = True
                Case BASSDeviceInfo.BASS_DEVICE_ENABLED And BASSDeviceInfo.BASS_DEVICE_DEFAULT
                    BaseUsable = True
                Case BASSDeviceInfo.BASS_DEVICE_ENABLED And BASSDeviceInfo.BASS_DEVICE_INIT And BASSDeviceInfo.BASS_DEVICE_DEFAULT
                    BaseUsable = True
                Case Else
                    BaseUsable = False
            End Select
        Else
            BaseName = "unknown"
            BaseUsable = False
        End If

    End Sub
    Public ReadOnly Property ID As Integer
        Get
            Return BaseID
        End Get
    End Property
    Public ReadOnly Property Name As String
        Get
            Return BaseName
        End Get
    End Property
    Public ReadOnly Property Usable As Boolean
        Get
            Return BaseUsable
        End Get
    End Property
End Class
