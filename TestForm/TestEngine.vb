Imports System.Drawing
Imports System.Windows.Forms
Imports Blade

Public Class TestEngine
    Inherits Blade.EngineBase
    Dim TE As New TestEntity
    Public Overrides Sub LoadEngine(LogicState As LogicState, RenderState As RenderState)
        ResolutionMode = ResolutionMode.Virtual
        Resolution = New SizeF(800, 600)
        ParentForm.WindowState = FormWindowState.Maximized
        ParentForm.Text = "Bouncing squares demo"
        LogicState.Add(TE)
        RenderState.Add(TE)
    End Sub

    Public Overrides Sub UnloadEngine(LogicState As LogicState, RenderState As RenderState)
        LogicState.Remove(TE)
        RenderState.Remove(TE)
    End Sub
End Class