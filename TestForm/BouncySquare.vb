Imports System.Drawing
Imports Blade

Public Class BouncySquare
    Inherits Blade.Entity

    Dim X As Single = 0
    Dim Y As Single = 0
    Dim Width As Single = 10
    Dim Height As Single = 10
    Dim XVel As Single = 600
    Dim YVel As Single = 600
    Dim XPol As Boolean = False
    Dim YPol As Boolean = False
    
    Private OutputDevice As Blade.AudioDevice
    Private Player As AudioFXWriter
    
    Private OutputStream1 As Blade.AudioStream
    Private OutputStream2 As Blade.AudioStream
    Private OutputStream3 As Blade.AudioStream
    Private OutputStream4 As Blade.AudioStream

    
    Private BounceSFX As Blade.AudioData
    
    Public sub New(NX As Single, NY As Single, NXVel As Single, NYVel As Single, NXPol As Boolean, NYPol As Boolean, Player As AudioFXWriter, SFX as AudioData)
        X = NX
        Y = NY
        XVel = NXVel
        YVel = NYVel
        XPol = NXPol
        YPol = NYPol
        Me.Player = Player
        BounceSFX = SFX
    End sub
    
    
    Public Overrides Sub Render(State As RenderState)
        State.Surface.FillRectangle(Brushes.White, X, Y, Width, Height)
    End Sub

    Public Overrides Sub Logic(State As LogicState)
        If X >= State.Resolution.Width - Width Then
            XPol = True
            Player.PlayAsync(BounceSFX)
            
        End If
        If Y >= State.Resolution.Height - Height Then
            YPol = True
            Player.PlayAsync(BounceSFX)
        End If
        If X <= 0 Then
            XPol = False
            Player.PlayAsync(BounceSFX)
        End If
        If Y <= 0 Then
            YPol = False
            Player.PlayAsync(BounceSFX)
        End If
        
        
        
        If XPol = False Then
            X += XVel * State.Delta
        Else
            X -= XVel * State.Delta
        End If
        
        If YPol = False Then
            Y += YVel * State.Delta
        Else
            Y -= YVel * State.Delta
        End If
    End Sub

    Public Overrides Sub Load(LogicState As LogicState, RenderState As RenderState)
        Visible = True
        Active = True
    End Sub

    Public Overrides Sub Unload(LogicState As LogicState, Renderstate As RenderState)
    End Sub
End Class