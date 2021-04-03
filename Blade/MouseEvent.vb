
Option Explicit On
Option Strict On

Imports System.Windows.Forms
Imports System.Drawing
Imports System

Public Class MouseEvent
    Public Enum EventType As SByte
        None = -1
        Down = 0
        Move = 1
        Up = 2
        Scroll = 3
    End Enum
    Public Enum EventButton As SByte
        None = -1
        Left = 0
        Middle = 1
        Right = 2
    End Enum

    Friend Shared Function Initialize(Base As MouseEventArgs, Optional Type As EventType = EventType.None) As MouseEvent
        Dim NewEvent As New MouseEvent
        Select Case Base.Button
            Case MouseButtons.Left
                NewEvent.Button = EventButton.Left
            Case MouseButtons.Middle
                NewEvent.Button = EventButton.Middle
            Case MouseButtons.Right
                NewEvent.Button = EventButton.Right
        End Select
        NewEvent.Type = Type
        NewEvent.Delta = Base.Delta
        NewEvent.Location = Base.Location
        Return NewEvent
    End Function

    Public Property Button As EventButton
    Public Property Delta As Integer = 0
    Public Property Location As Point
    Public Property Type As EventType

End Class
