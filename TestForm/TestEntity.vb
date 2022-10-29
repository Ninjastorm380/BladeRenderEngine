
Imports System.Drawing
Imports Blade

Public Class TestEntity
    Inherits Blade.Entity
    Private OutputDevice As Blade.AudioDevice
    Private OutputStream As Blade.AudioStream
    Private SFXData As Blade.AudioData
    Private Player As Blade.AudioFXWriter
    Private BackgroundMusic As Blade.AudioData
    Private ClickCoordinates As Drawing.Point
    Private CurrentCoordinates As Drawing.Point
    Private ReleaseCoordinates As Drawing.Point
    Private IsSettingCourse As Boolean = False
    Private BouncySquares As New List(Of BouncySquare)
    Private BSXVEL, BSYVEL As Single
    Private BSXPOL, BSYPOL As Boolean
    Private BitFontFamily As FontFamily()
    Private BitFont As Font
    Private FirstClicked As Boolean = False

    Public Overrides Sub Render(State As Blade.RenderState)
        State.Surface.Clear(Drawing.Color.Blue)
        For Index = 0 To BouncySquares.Count - 1
            If BouncySquares(Index).Visible = True Then BouncySquares(Index).Render(State)
        Next
        If IsSettingCourse = True
            State.Surface.DrawLine(Pens.Gold,ClickCoordinates,CurrentCoordinates)
        End If
        
        If FirstClicked = False
            State.Surface.DrawString("Bouncing squares - blade engine demo by demon4hire!", BitFont, Brushes.Black, 2 , 2)
            State.Surface.DrawString("Instructions: click and drag with the left mouse button", BitFont, Brushes.Black, 2 , 17)
            State.Surface.DrawString("to fire noisy and bouncy white squares!", BitFont, Brushes.Black, 2 , 32)
            State.Surface.DrawString("Click the 'X' in the upper right corner to close!", BitFont, Brushes.Black, 2 , 47)
            State.Surface.DrawString("Left click anywhere with the mouse to continue!", BitFont, Brushes.Green, 2 , 62)
        End If

    End Sub

    Public Overrides Sub Logic(State As Blade.LogicState)
        If IsSettingCourse = False And ((State.Mouse.Buttons And Blade.MouseState.H00) <> 0)
            ClickCoordinates = State.Mouse.Location
            FirstClicked = True
            IsSettingCourse = True
        End If
        
        If IsSettingCourse = True And ((State.Mouse.Buttons And Blade.MouseState.H00) <> 0)
            CurrentCoordinates = State.Mouse.Location
        End If
        
        If IsSettingCourse = True And ((State.Mouse.Buttons And Blade.MouseState.H00) = 0)
            ReleaseCoordinates = State.Mouse.Location
            BSXVEL = ReleaseCoordinates.X - ClickCoordinates.X
            BSYVEL = ReleaseCoordinates.Y - ClickCoordinates.Y
            If BSXVEL < 0 Then
                BSXVEL *= -1
                BSXPOL = True
            Else
                BSXPOL = False
            End If
            If BSYVEL < 0 Then
                BSYVEL *= -1
                BSYPOL = True
            Else
                BSYPOL = False
            End If
            Dim BS As New BouncySquare(ClickCoordinates.X,ClickCoordinates.Y,BSXVEL/100,BSYVEL/100,BSXPOL,BSYPOL, Player, SFXData)
            BS.Load(State, Nothing)
            BouncySquares.Add(BS)
            IsSettingCourse = False
        End If
        
        
        For Index = 0 To BouncySquares.Count - 1
            If BouncySquares(Index).Active = True Then BouncySquares(Index).Logic(State)
        Next
        
    End Sub

    Public Overrides Sub Load(LogicState As LogicState, RenderState As RenderState)
        Visible = True
        Active = True
        OutputDevice = Blade.Audio.GetOutputDevices()(0)
        OutputStream = New AudioStream(OutputDevice)
        
        Player = New AudioFXWriter(512, OutputDevice)
        Player.Volume = 0.6
        
        SFXData = New AudioData("./Resources/chord.wav")
        BackgroundMusic = New AudioData("./Resources/Soar.mp3")
        BitFontFamily = Blade.LoadFont("./Resources/BitFont.ttf")
        BitFont = New Font(BitFontFamily(0), 9)
        OutputStream.PlayAsync(BackgroundMusic, True)
    End Sub

    Public Overrides Sub Unload(LogicState As LogicState, Renderstate As RenderState)
        
        
        
        
        
        For Index = 0 To BouncySquares.Count - 1
            BouncySquares(Index).Unload(LogicState, RenderState)
        Next
        Dim BCCount AS Int32 = BouncySquares.Count
        For X = 0 To BCCount - 1
            BouncySquares.RemoveAt(0)
        Next
        BitFont.Dispose()
        Blade.UnloadFont(BitFontFamily)
        Player.Dispose()
        BackgroundMusic.Dispose()
        OutputStream.StopAsync()
        OutputStream.Dispose()
    End Sub
End Class