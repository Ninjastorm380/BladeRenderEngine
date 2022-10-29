Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Text
Imports System.Threading
Imports System.Windows.Forms
Imports Un4seen.Bass

Public Mustinherit Class EngineBase 
    Inherits UserControl
    Implements IDisposable
    
    Private Surface As Graphics = Nothing
    Private SurfaceHandle As IntPtr = Nothing
    Private SurfaceBounds As Rectangle = Nothing
    Private BufferedSurface As BufferedGraphics = Nothing
    Private BufferedSurfaceContext As BufferedGraphicsContext 
    Private EngineRunning As Boolean = False
    Private IsResizing As Boolean = False
    Private WidthScale As Double = 1.0
    Private HeightScale As Double = 1.0
    Private WidthScaleInverse As Double = 1.0
    Private HeightScaleInverse As Double = 1.0
    Private RenderLock As Object = New Object
    Private KeyLock As Object = New Object
    Private BaseResolution As SizeF = New SizeF(1920, 1080)
    Private RS As new RenderState
    Private LS As new LogicState
    Private KS As New KeyboardState
    Private MS AS New MouseState
    Private LastKeyUp As Int32
    Private LastKeyDown As Int32
    Private UsingMono As Boolean = False
    Private ScrollDir As Int32 = 0
    Private TitleHeight As Int32 = 0
    Private BaseResolutionMode As ResolutionMode = ResolutionMode.Screen
    Private TmpWindowHeight As Int32
    Private TmpWindowHeight2 As Int32
    Private FormLocation As Point
    Private MousePos As Point
    Private W32MouseState As MouseButtons
    Private WorkaroundKeyPressed As Boolean = False
    Private FormClientBounds As New RectangleF
    
    
    Public Property ResolutionMode As ResolutionMode
        Get
            Return BaseResolutionMode
        End Get
        Set
            BaseResolutionMode = Value
            OnResize(Nothing)
        End Set
    End Property

    
    
    Public Property Resolution As SizeF
        Get
            Select Case ResolutionMode
                Case = ResolutionMode.Screen
                    Return CType(SurfaceBounds.Size, SizeF)
                Case = ResolutionMode.Virtual
                    Return BaseResolution
            End Select
        End Get
        Set
            BaseResolution = Value
            OnResize(Nothing)
        End Set
    End Property
    
    
    Public Overrides Function PreProcessMessage(ByRef msg As Message) As Boolean
        Select Case msg.Msg
            Case Else
                Return False
        End Select
    End Function
    
    Protected Overrides Sub WndProc(Byref M As Message)
        
        Select Case M.Msg
            Case 260 'WM_SYSKEYDOWN
                
                Dim LPARAM As Int64 = M.LParam
                
                Dim SCANCODE As Int64 = LPARAM >> 16 And 255
                If SCANCODE = 135 Then
                    LastKeyDown = 135
                    Return
                End If
                If LastKeyDown <> SCANCODE
                    LastKeyDown = SCANCODE
                    KS.Buttons += KeyboardState.ScanToHex(SCANCODE)
                End If

                M.Result = 0
            Case 261 'WM_SYSKEYUP
                Dim LPARAM As Int64 = M.LParam
                Dim SCANCODE As Int64 = (LPARAM >> 16) And 255
                If SCANCODE = 135 Then
                    LastKeyDown = 0
                    Return
                End If
                If LastKeyDown = SCANCODE Then LastKeyDown = 0
                KS.Buttons -= KeyboardState.ScanToHex(SCANCODE)
                
                If KS.Buttons < 0 Then KS.Buttons = 0
                M.Result = 0
            Case 256 'WM_KEYDOWN
                Dim LPARAM As Int64 = M.LParam
                Dim SCANCODE As Int64 = (LPARAM >> 16) And 255
                If SCANCODE = 135 Then
                    LastKeyDown = 135
                    Return
                End If
                
                If LastKeyDown <> SCANCODE
                    LastKeyDown = SCANCODE
                    KS.Buttons += KeyboardState.ScanToHex(SCANCODE)
                End If
                

                M.Result = 0
            Case 257 'WM_KEYUP
                Dim LPARAM As Int64 = M.LParam
                Dim SCANCODE As Int64 = (LPARAM >> 16) And 255
                If SCANCODE = 135 Then
                    LastKeyDown = 0
                    Return
                End If
                If LastKeyDown = SCANCODE Then LastKeyDown = 0
                
                KS.Buttons -= KeyboardState.ScanToHex(SCANCODE)
                
                If KS.Buttons < 0 Then KS.Buttons = 0

                M.Result = 0
            Case Else
                MyBase.WndProc(M)
        End Select

    End Sub

    Public Sub New(StartingSet As Entity())
        Dim T As Type = Type.GetType("Mono.Runtime")
        If T = Nothing Then UsingMono = False Else UsingMono = True
        SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        SetStyle(ControlStyles.UserPaint, True)
        SetStyle(ControlStyles.Opaque, True)
        UpdateStyles()
        CreateHandle()
        LS.Entities.AddRange(StartingSet)
        RS.Entities.AddRange(StartingSet)
        Dim AsyncBootstrapper As New Thread(AddressOf AsyncEngineBootstrap)
        AsyncBootstrapper.Start()
    End Sub
    
    Public Sub New()
        Dim T As Type = Type.GetType("Mono.Runtime")
        If T = Nothing Then UsingMono = False Else UsingMono = True
        SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        SetStyle(ControlStyles.UserPaint, True)
        SetStyle(ControlStyles.Opaque, True)
        UpdateStyles()
        CreateHandle()
        Dim AsyncBootstrapper As New Thread(AddressOf AsyncEngineBootstrap)
        AsyncBootstrapper.Start()
    End Sub
    
    Private Sub AsyncEngineBootstrap
        Dim LogicThread As New Thread(AddressOf LogicMethod)
        Dim RenderThread As New Thread(AddressOf RenderMethod)
        Dim MessagePumpThread As New Thread(Addressof MessageMethod)
        Do Until ParentForm IsNot Nothing
            
        Loop
        EngineRunning = True
        
        LoadMethod()
        
        MessagePumpThread.Start()
        LogicThread.Start()
        RenderThread.Start()
    End Sub
    
    Private Sub LoadMethod()
            do until ParentForm.Created = True
            
            Loop
            do until Created = True
            
            Loop
        LoadAudioEngine()
        LoadEngine(LS, RS)
        
        For Index = 0 To LS.Entities.Count - 1
            LS.Entities(Index).Load(LS, RS)
        Next

    End Sub
    
    Private Sub UnloadMethod()
        
        Do Until LS.Entities.Count = 0
            Dim Ref As Entity = LS.Entities(0)
            Ref.Unload(LS, RS)
            
            If LS.Entities.Count > 0 AndAlso Ref Is LS.Entities(0) Then
                LS.Remove(Ref)
                RS.Remove(Ref)
            End If
        Loop
        
        Do Until AudioStream.RefList.Count = 0
            Dim Refrence As AudioStream = AudioStream.RefList(0)
            If Refrence.IsDisposed = False Then
                Refrence.StopAsync()
                Refrence.Dispose()
            End If
        Loop
        
        Do Until AudioData.RefList.Count = 0
            Dim Refrence As AudioData = AudioData.RefList(0)
            If Refrence.IsDisposed = False Then
                Refrence.Dispose()
            End If
        Loop
        
        Do Until AudioFXWriter.RefList.Count = 0
            Dim Refrence As AudioFXWriter = AudioFXWriter.RefList(0)
            If Refrence.IsDisposed = False Then
                Refrence.Dispose()
            End If
        Loop
        
        Do Until Sprite.RefList.Count = 0
            Dim Refrence As Sprite = Sprite.RefList(0)
            If Refrence.IsDisposed = False Then
                Refrence.Dispose()
            End If
        Loop
        
        For Each Item In GetInputDevices()
            Item.Dispose()
        Next
        For Each Item In GetOutputDevices()
            Item.Dispose()
        Next
        UnloadEngine(LS, RS)
        UnloadAudioEngine()
    End Sub
    
    Private Sub LogicMethod()
        Dim WorkaroundLaunched As Boolean = False
        Do While EngineRunning = True
            TmpWindowHeight = ParentForm.Height
            TmpWindowHeight2 = ParentForm.ClientRectangle.Height
            FormClientBounds = ParentForm.ClientRectangle
            FormLocation = Parentform.Location
            MousePos = MousePosition
            W32MouseState = MouseButtons
            TitleHeight = TmpWindowHeight - TmpWindowHeight2
            Dim TmpPoint As Point = New Point((MousePos.X - FormLocation.X) * WidthScaleInverse, ((MousePos.Y - FormLocation.Y - TitleHeight) * HeightScaleInverse))
            If (TmpPoint.X >= 0 And TmpPoint.X <= Resolution.Width) And (TmpPoint.Y >= 0 And TmpPoint.Y <= Resolution.Height)
                MS.Location = TmpPoint
            End If

            MS.Buttons = 0
            If (W32MouseState And MouseButtons.Left) <> 0 Then
                MS.Buttons += MouseState.H00
            End If
            If (W32MouseState And MouseButtons.Middle) <> 0 Then
                MS.Buttons += MouseState.H01
            End If
            If (W32MouseState And MouseButtons.Right) <> 0 Then
                MS.Buttons += MouseState.H02
            End If
            If (W32MouseState And MouseButtons.XButton1) <> 0 Then
                MS.Buttons += MouseState.H03
            End If
            If (W32MouseState And MouseButtons.XButton2) <> 0 Then
                MS.Buttons += MouseState.H04
            End If
            
            If ScrollDir = 1
                MS.Buttons += MouseState.H05
                ScrollDir = 0
            ElseIf ScrollDir = -1
                MS.Buttons += MouseState.H06
                ScrollDir = 0
            End If
            

            LS.Mouse = MS
            LS.Keyboard = KS
            LS.LogicGovernor.Rate = LS.LPSTarget
            LS.Resolution = Resolution
            LS.Parent = ParentForm
            
            If WorkaroundState = 1 Then
                If LS.Keyboard.Buttons > 0 And WorkaroundFinished = True And WorkaroundLaunched = True Then WorkaroundKeyPressed = True
                If LS.Keyboard.Buttons = 0 And WorkaroundFinished = True And WorkaroundLaunched = True And WorkaroundKeyPressed = True Then
                    WorkaroundKeyPressed = False
                    ParentForm.Close()
                End If
            Else
                If CustomFontsLoaded = True And WorkaroundState = 0 And WorkaroundLaunched = False Then
                    Synclock RenderLock
                        CheckMonoFontsWorkaroundBegin()
                        If WorkaroundState = 1 Then
                            ParentForm.WindowState = FormWindowState.Maximized
                            ResolutionMode = ResolutionMode.Screen
                            UnloadMethod()
                        End If
                        
                        WorkaroundLaunched = True
                    End SyncLock
                Else
                    Synclock LS.EntityLock
                        For Index = 0 To LS.Entities.Count - 1
                            If Index < LS.Entities.Count AndAlso LS.Entities(Index).Active = True Then LS.Entities(Index).Logic(LS)
                        Next
                    End SyncLock
                End If
            End If
            LS.LogicGovernor.Limit()
        Loop
        If WorkaroundState <> 1 Then
            UnloadMethod()
            CheckMonoFontsWorkaroundEnd()
        End If
        
        
    End Sub
    
    Private Sub RenderMethod()
        Do While EngineRunning = True
            do until ParentForm.Created = True
            
            Loop
            do until Created = True
            
            Loop
            Synclock RenderLock
                RS.RenderGovernor.Rate = RS.FPSTarget
                
                 If SurfaceBounds <> Bounds Then
                        If Surface IsNot Nothing Then Surface.Dispose()
                        If BufferedSurface IsNot Nothing Then BufferedSurface.Dispose()
            
                        Invoke(Sub()SurfaceHandle = Handle)
                        Invoke(Sub()SurfaceBounds = Bounds)
                    
                        Surface = Graphics.FromHwnd(SurfaceHandle)
                        Surface.CompositingMode = CompositingMode.SourceOver
                        Surface.CompositingQuality = CompositingQuality.AssumeLinear
                        Surface.InterpolationMode = InterpolationMode.NearestNeighbor
                        Surface.SmoothingMode = SmoothingMode.None
                        Surface.PixelOffsetMode = PixelOffsetMode.None
                        Surface.TextRenderingHint = TextRenderingHint.AntiAlias
                        Surface.Clip = New Region(SurfaceBounds)
        
                        BufferedSurfaceContext = BufferedGraphicsManager.Current
                        If SurfaceBounds.Size.Width > Size.Empty.Width Then BufferedSurfaceContext.MaximumBuffer = SurfaceBounds.Size
                        If Surface IsNot Nothing And SurfaceBounds <> Rectangle.Empty
                            BufferedSurface = BufferedSurfaceContext.Allocate(Surface, SurfaceBounds)
                            BufferedSurface.Graphics.Clear(Color.Black)
                            BufferedSurface.Graphics.ResetTransform()
                            WidthScale = SurfaceBounds.Width / Resolution.Width
                            HeightScale = SurfaceBounds.Height / Resolution.Height
                            WidthScaleInverse =  Resolution.Width / SurfaceBounds.Width
                            HeightScaleInverse = Resolution.Height / SurfaceBounds.Height
                            BufferedSurface.Graphics.ScaleTransform(WidthScale,HeightScale)
                        End If
                    End If
                
                If WorkaroundState = 1 Then
                    BufferedSurface.Graphics.Clear(Color.Black)
                    If WorkaroundFinished = True Then
                        BufferedSurface.Graphics.DrawString(
                            "Blade engine has detected that you are using mono for linux." + vbcrlf +
                            "The program font files have been temporarily installed to work around a bug in mono for linux, and they will be uninstalled the next time this program is run and closed normally."+vbcrlf+
                            "If you wish to avoid seeing this message again, simply remove or rename the workaround configuration file  '~/blade.monofontsworkaround.cfg' before relaunching this program."+vbcrlf+vbcrlf+
                            "Press any key to close...",
                            new font(SystemFonts.DefaultFont.Name, 14), Brushes.White, 0, 0)
                    Else
                        BufferedSurface.Graphics.DrawString(
                            "Blade engine has detected that you are using mono for linux." + vbcrlf +
                            "The program font files have been temporarily installed to work around a bug in mono for linux, and they will be uninstalled the next time this program is run and closed normally."+vbcrlf+
                            "If you wish to avoid seeing this message again, simply remove or rename the workaround configuration file '~/blade.monofontsworkaround.cfg' before relaunching this program.",
                            new font(SystemFonts.DefaultFont.Name, 14), Brushes.White, 0, 0)
                    End If
                Else
                    RS.Surface = BufferedSurface.Graphics
                    
                    Synclock RS.EntityLock
                        For Index = 0 To RS.Entities.Count - 1
                            If RS.Entities(Index).Visible = True Then RS.Entities(Index).Render(RS)
                        Next
                    End SyncLock
                End If

                BufferedSurface.Render()
                
            End SyncLock
            RS.RenderGovernor.Limit()
        Loop
        RS.Entities.Clear()
    End Sub
    
    Private Sub MessageMethod()
        Dim Governor As New Governor(RS.FPSTarget)
        Do While EngineRunning = True
            Governor.Rate = RS.FPSTarget
            Application.DoEvents()
            Governor.Limit()
        Loop

    End Sub
    
    
    Public Shadows Sub Dispose()
        EngineRunning = False
        MyBase.Dispose()
    End Sub
    Public Shadows Sub Dispose(NonmanagedToo As Boolean)
        Dispose()
        MyBase.Dispose(NonmanagedToo)
    End Sub

    Protected Overrides Sub OnMouseWheel(e As MouseEventArgs)
        If e.Delta > 0 Then 
            ScrollDir = 1
        ElseIf e.Delta < 0 Then
            ScrollDir = -1
        Else
            ScrollDir = 0
        End If
    End Sub

    Public MustOverride Sub LoadEngine(LogicState As LogicState, RenderState As RenderState)
    Public MustOverride Sub UnloadEngine(State As LogicState, RenderState As RenderState)
End Class