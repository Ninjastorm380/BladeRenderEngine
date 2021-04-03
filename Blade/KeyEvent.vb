
Option Explicit On
Option Strict On

Imports System.Windows.Forms
Imports System.Drawing
Imports System


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
End Class
