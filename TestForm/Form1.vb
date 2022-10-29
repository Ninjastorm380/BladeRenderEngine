Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
Imports Blade

Friend Class Form1 
    Inherits Windows.Forms.Form
    
    

    
    Private TestEngine1 As TestEngine
    Sub New()
        TestEngine1 = New TestEngine With {.Dock = DockStyle.Fill}
        Controls.AddRange({TestEngine1})
    End Sub
    
    Private sub IsClosingCheck Handles Me.FormClosing
        TestEngine1.Dispose()
    End sub
End Class