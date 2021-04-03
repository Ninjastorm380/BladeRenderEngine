Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports Blade

Public Class Main : Inherits Form
    Private Test As New Viewport With {.Dock = DockStyle.Fill}
    Public Sub New()
        Me.SuspendLayout()
        Me.Size = New Size(1024, 768)
        Me.Controls.Add(Test)
        Me.ResumeLayout()
        Me.StartPosition = FormStartPosition.CenterScreen
    End Sub
End Class