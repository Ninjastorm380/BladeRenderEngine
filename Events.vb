
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
    Public Property Handled As Boolean = False
End Class
Public Class KeyEvent
    <Flags()> Public Enum KeyModifiers As SByte
        None = 0
        Alt = 1
        Ctrl = 2
        Shift = 4
    End Enum

    Friend Shared Function Initialize(Base As Keys) As KeyEvent
        Dim KA As New KeyEventArgs(Base)
        Dim NewEvent As New KeyEvent
        Dim SByteFlag As SByte = 0
        If KA.Alt = True Then SByteFlag += CType(1, SByte)
        If KA.Control = True Then SByteFlag += CType(2, SByte)
        If KA.Shift = True Then SByteFlag += CType(4, SByte)
        NewEvent.Modifiers = CType(SByteFlag, KeyModifiers)
        NewEvent.KeyCode = KA.KeyCode
        Return NewEvent
    End Function

    Public Property KeyCode As Integer
    Public Property Modifiers As KeyModifiers
    Public Property Handled As Boolean = False
End Class