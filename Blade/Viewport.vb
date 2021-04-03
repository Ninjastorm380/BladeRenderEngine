
Option Explicit On
Option Strict On

Imports System
Imports System.Windows.Forms
Imports System.Drawing
Imports System.Diagnostics
Imports System.Collections.Generic
Imports Un4seen.Bass

Public Class Viewport : Inherits UserControl
    'graphics control variables.'
    Private BaseGFX As Graphics
    Private GFXBufferContext As BufferedGraphicsContext = BufferedGraphicsManager.Current()
    Private GFXBuffer As BufferedGraphics
    Private SurfaceHandle As IntPtr

    'Private control flags.'
    Private EngineLoaded As Boolean = False 'True when the engine has completed loading all views, otherwise false.'
    Private IsBorderlessFullscreen As Boolean = False 'internal boolean flag for property "BorderlessFullscreen".'

    'Delta calculation variables.'
    Private LogicDeltaTimer As New Stopwatch 'Measures elapsed time for the current iteration of the logic loop.'
    Private RenderDeltaTimer As New Stopwatch 'Measures elapsed time for the current iteration of the render loop.'
    Public Property LastLogicDeltaTime As Double = 0.0 'Duration of the last logic loop iteration. Measured in milliseconds, collected and stored as a double for precision.'
    Public Property LastRenderDeltaTime As Double = 0.0 'Duration of the last render loop iteration. Measured in milliseconds, collected and stored as a double for precision.' 

    'Engine threaded governors.'
    Private LogicLimiter As New ThreadLimiter(60) 'Logic Governor.'
    Private RenderLimiter As New ThreadLimiter(60) 'Render Governor.'

    'Input buffers for both the keyboard and mouse.'
    Private KeyBuffer As New KeyBuffer 'Keyboard buffer'
    Private MouseBuffer As New MouseBuffer 'Mouse buffer'

    'list to manage all the engine views'
    Private Views As New List(Of View)

    'all audio devices in the enviroment have their refrences stored here in these two arrays'
    Private InputDevices As InputDevice() 'Audio inputs'
    Private OutputDevices As OutputDevice() 'Audio Outputs'

    'High performance return calls for public access by individual views.'
    Public Sub GetBufferedSurface(ByRef Output As Graphics) 'When called, output will contain a graphics object that points to the buffer.' 
        If EngineLoaded = True Then
            Output = GFXBuffer.Graphics
        Else
            Output = Nothing
        End If
    End Sub
    Public Sub GetKeyBuffer(ByRef Output As KeyBuffer) 'When called, output will contain the keyboard buffer.' 
        If EngineLoaded = True Then
            Output = KeyBuffer
        Else
            Output = Nothing
        End If
    End Sub
    Public Sub GetMouseBuffer(ByRef Output As MouseBuffer) 'When called, output will contain the mouse buffer.' 
        If EngineLoaded = True Then
            Output = MouseBuffer
        Else
            Output = Nothing
        End If
    End Sub
    Public Sub GetHostForm(ByRef Output As Form) 'When called, output will contain the parent form that the engine resides inside.' 
        If EngineLoaded = True Then
            Output = ParentForm
        Else
            Output = Nothing
        End If
    End Sub
    Public Sub GetInputDevices(ByRef Output As InputDevice()) 'When called, output will contain all known audio input devices in the current enviroment.' 
        If EngineLoaded = True Then
            Output = InputDevices
        Else
            Output = Nothing
        End If
    End Sub
    Public Sub GetOutputDevices(ByRef Output As OutputDevice()) 'When called, output will contain all known audio output devices in the current enviroment.' 
        If EngineLoaded = True Then
            Output = OutputDevices
        Else
            Output = Nothing
        End If
    End Sub

    'Resets audio device listings. Call this before calling GetInputDevices or GetOutputDevices to get the most recent device lists.'
    Public Sub UpdateAudioDeviceLists()
        Dim InputList As New List(Of InputDevice)
        Dim OutputList As New List(Of OutputDevice)
        For x As Integer = -1 To Bass.BASS_RecordGetDeviceCount - 1
            InputList.Add(New InputDevice(x))
        Next
        For x As Integer = -1 To Bass.BASS_GetDeviceCount - 1
            OutputList.Add(New OutputDevice(x))
        Next
        OutputDevices = OutputList.ToArray
        InputDevices = InputList.ToArray
        OutputList.Clear()
        InputList.Clear()
    End Sub

    'Setting to switch engine between borderless fullscreen and windowed. Setting this value will cause the view to reset if the engine is running. Internally sets flag "IsBorderlessFullscreen".'
    Public Property BorderLessFullscreen As Boolean
        Get
            Return IsBorderlessFullscreen
        End Get
        Set(Value As Boolean)
            IsBorderlessFullscreen = Value
            If EngineLoaded = True Then ResetView()
        End Set
    End Property

    'Various input handlers.'
    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean 'Intercept all keypresses, including command keys.' 
        If EngineLoaded = True Then
            KeyBuffer.Enqueue(KeyEvent.Initialize(keyData))
        End If
        Return True
    End Function
    Protected Overrides Sub OnMouseWheel(e As MouseEventArgs) 'Intercept scroll events.' 
        If EngineLoaded = True Then
            MouseBuffer.Enqueue(MouseEvent.Initialize(e, MouseEvent.EventType.Scroll))
        End If
    End Sub
    Protected Overrides Sub OnMouseUp(e As MouseEventArgs) 'Intercept click events.' 
        If EngineLoaded = True Then
            MouseBuffer.Enqueue(MouseEvent.Initialize(e, MouseEvent.EventType.Up))
        End If
    End Sub
    Protected Overrides Sub OnMouseDown(e As MouseEventArgs) 'Intercept click events.' 
        If EngineLoaded = True Then
            MouseBuffer.Enqueue(MouseEvent.Initialize(e, MouseEvent.EventType.Down))
        End If
    End Sub
    Protected Overrides Sub OnMouseMove(e As MouseEventArgs) 'Intercept mouse movement events.' 
        If EngineLoaded = True Then
            MouseBuffer.Enqueue(MouseEvent.Initialize(e, MouseEvent.EventType.Move))
        End If
    End Sub
    Protected Overrides Sub OnResize(e As EventArgs) 'Intercept and rehandle resize events. This is done so we can reset the view when the engine control is resized.' 
        MyBase.OnResize(e)
        If EngineLoaded = True Then ResetView()
    End Sub

    'Methods for managing view list'
    Public Sub AddView(ByRef V As View) 'Adds the view to the view list, and calls View.Load() if the engine is running.' 
        Views.Add(V)
        V.Parent = Me
        If EngineLoaded = True Then
            Dim OldVStates(1) As Boolean
            OldVStates(0) = V.Active
            OldVStates(1) = V.Visible
            V.Active = False
            V.Visible = False
            SyncLock Me
                V.Load()
            End SyncLock
            If V.Active = False Then V.Active = OldVStates(0)
            If V.Visible = False Then V.Visible = OldVStates(1)
        End If
    End Sub
    Public Sub RemoveView(ByRef V As View) 'Removes the view from the view list, and calls View.Unload() if the engine is running.' 
        Views.Remove(V)
        V.Parent = Nothing
        If EngineLoaded = True Then
            V.Active = False
            V.Visible = False
            SyncLock Me
                V.Unload()
            End SyncLock
        End If
    End Sub

    'Internal logic. These are the heart of the engine.'
    Private Sub EngineLoader() Handles Me.Load 'Kickstarts the engine asynchronously for us automagically.' 
        Dim AsyncLoader As New Threading.thread(AddressOf LoaderMethod)
        AsyncLoader.Start
    End Sub
    Private Sub LoaderMethod() 'Asynchronous start helper.' 
        Do Until (ParentForm Is Nothing) = False : Loop
        SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        SetStyle(ControlStyles.UserPaint, True)
        SetStyle(ControlStyles.Opaque, True)
        UpdateStyles()
        UpdateAudioDeviceLists()
        ResetView()
        SyncLock Me
            For Each X As View In Views
                X.Load()
            Next
        End SyncLock
        EngineLoaded = True
        Dim AsyncLogicThread As New Threading.Thread(AddressOf LogicLoop) : AsyncLogicThread.Start
        Dim AsyncRenderThread As New Threading.Thread(AddressOf RenderLoop) : AsyncRenderThread.Start
    End Sub
    Private Sub RenderLoop() 'Clears surface buffer and handles calling every view for rendering via view.render().' 
        Do Until ParentForm Is Nothing
            SyncLock Me
                Dim Snapshot As TimeSpan = RenderDeltaTimer.Elapsed
                LastRenderDeltaTime = Snapshot.TotalMilliseconds
                RenderDeltaTimer.Restart()
                GFXBuffer.Graphics.Clear(Color.Black)
                For Each X As View In Views
                    If X.Visible = True Then X.Render()
                Next
                GFXBuffer.Render()
            End SyncLock
            RenderLimiter.Limit()
        Loop
    End Sub
    Private Sub LogicLoop() 'Handles calling every view for computation via view.logic(). exiting this thread will call view.unload() for each loaded view.' 
        Do Until ParentForm Is Nothing
            SyncLock Me
                Dim Snapshot As TimeSpan = LogicDeltaTimer.Elapsed
                LastLogicDeltaTime = Snapshot.TotalMilliseconds
                LogicDeltaTimer.Restart()
                For Each X As View In Views
                    If X.Active = True Then X.Logic()
                Next
            End SyncLock
            If MouseBuffer.Length > 0 Then MouseBuffer.Clear()
            If KeyBuffer.Length > 0 Then KeyBuffer.Clear()
            LogicLimiter.Limit()
        Loop
        SyncLock Me
            For Each X As View In Views
                X.Unload()
            Next
        End SyncLock
    End Sub
    Private Sub ResetView() 'Resets everything related to the rendering surface and sets viewport dimensions.' 
        Me.Invoke(Sub()
                      If BorderLessFullscreen = True Then
                          ParentForm.FormBorderStyle = FormBorderStyle.None
                          ParentForm.WindowState = FormWindowState.Maximized
                          ParentForm.TopMost = True
                      Else
                          Dim Check As Boolean = (ParentForm.FormBorderStyle = FormBorderStyle.None)
                          ParentForm.FormBorderStyle = FormBorderStyle.Sizable
                          If Check = True Then ParentForm.WindowState = FormWindowState.Normal
                          ParentForm.TopMost = False
                      End If
                      SurfaceHandle = Me.Handle
                  End Sub)
        SyncLock Me
            If GFXBuffer IsNot Nothing Then GFXBuffer.Dispose()
            If BaseGFX IsNot Nothing Then BaseGFX.Dispose()
            BaseGFX = Graphics.FromHwnd(SurfaceHandle)
            GFXBufferContext.MaximumBuffer = Me.Size
            GFXBuffer = GFXBufferContext.Allocate(BaseGFX, Me.Bounds)
            GFXBuffer.Graphics.CompositingMode = Drawing2D.CompositingMode.SourceOver
            GFXBuffer.Graphics.CompositingQuality = Drawing2D.CompositingQuality.GammaCorrected
            GFXBuffer.Graphics.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
            GFXBuffer.Graphics.SmoothingMode = Drawing2D.SmoothingMode.None
            GFXBuffer.Graphics.PixelOffsetMode = Drawing2D.PixelOffsetMode.None
            GFXBuffer.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
            GFXBuffer.Graphics.Clip = New Region(New RectangleF(0, 0, GFXBufferContext.MaximumBuffer.Width, GFXBufferContext.MaximumBuffer.Height))
        End SyncLock
    End Sub
End Class