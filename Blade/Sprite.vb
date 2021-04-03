
Option Explicit On
Option Strict On

Imports System
Imports System.Collections.Generic
Imports System.Windows.Forms
Imports System.Drawing


Public MustInherit Class Sprite
    Public Property Parent As View
    Public Property Active As Boolean = False
    Public Property Visible As Boolean = False

    Public ReadOnly Property LastLogicDeltaTime As Double
        Get
            Return Parent.LastLogicDeltaTime
        End Get
    End Property
    Public ReadOnly Property LastRenderDeltaTime As Double
        Get
            Return Parent.LastRenderDeltaTime
        End Get
    End Property
    Public Property BorderLessFullscreen As Boolean
        Get
            Return Parent.BorderLessFullscreen
        End Get
        Set(Value As Boolean)
            Parent.BorderLessFullscreen = Value
        End Set
    End Property

    Public Sub GetBufferedSurface(ByRef Output As Graphics)
        Parent.GetBufferedSurface(Output)
    End Sub
    Public Sub GetKeyBuffer(ByRef Output As KeyBuffer)
        Parent.GetKeyBuffer(Output)
    End Sub
    Public Sub GetMouseBuffer(ByRef Output As MouseBuffer)
        Parent.GetMouseBuffer(Output)
    End Sub
    Public Sub GetHostForm(ByRef Output As Form)
        Parent.GetHostForm(Output)
    End Sub
    Public sub GetInputDevices(Byref Output as InputDevice())
        Parent.GetInputDevices(Output)
    End Sub
    Public sub GetOutputDevices(Byref Output as OutputDevice())
        Parent.GetOutputDevices(Output)
    End Sub
    Public Sub AddView(ByRef V As View)
        Parent.AddView(V)
    End Sub
    Public Sub RemoveView(ByRef V As View)
        Parent.RemoveView(V)
    End Sub
    Public Sub AddSprite(ByRef S As Sprite)
        Parent.AddSprite(S)
    End Sub
    Public Sub RemoveSprite(ByRef S As Sprite)
        Parent.RemoveSprite(S)
    End Sub

    Public MustOverride Sub Load()
    Public MustOverride Sub Unload()
    Public MustOverride Sub Logic()
    Public MustOverride Sub Render()
End Class
