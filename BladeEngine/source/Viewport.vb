Public Class Viewport
    Private Surface As Graphics
    Private SurfaceVarsRequireUpdate As Boolean = True
    Private SurfaceHandle As IntPtr
    Private SurfaceBounds As Rectangle
    Private BufferedSurface As BufferedGraphics
    Private BufferedSurfaceContext As BufferedGraphicsContext = BufferedGraphicsManager.Current
    Private IsBorderlessFullscreen As Boolean = False
    Private Views As New List(Of ViewBase)
    Private KeyBufferBase As New KeyBuffer
    Private MouseBufferBase As New MouseBuffer
    Private LogicDeltaTimer As New Stopwatch
    Private RenderDeltaTimer As New Stopwatch
    Private LogicDeltaTime As Double
    Private RenderDeltaTime As Double
    Private LogicThreadExited As Boolean = False
    Private RenderThreadExited As Boolean = False
    Private IsUpdatingSurfaceVars As Boolean = False
    Private CloseEngine As Boolean = False
    Private Delegate Sub ThreadInvoker()
    Private InputDevices As InputDevice()
    Private OutputDevices As OutputDevice()
    Private Const MaxBufferCount As Integer = 5
    Public Property LogicRate As Integer = 60
    Public Property RenderRate As Integer = 60

    Public Property BorderlessFullscreen As Boolean
        Get
            Return IsBorderlessFullscreen
        End Get
        Set(value As Boolean)
            IsBorderlessFullscreen = value
            SurfaceVarsRequireUpdate = True
        End Set
    End Property
    Protected Friend ReadOnly Property KeyBuffer As KeyBuffer
        Get
            Return KeyBufferBase
        End Get
    End Property
    Protected Friend ReadOnly Property MouseBuffer As MouseBuffer
        Get
            Return MouseBufferBase
        End Get
    End Property
    Protected Friend ReadOnly Property AudioInputDevices As InputDevice()
        Get
            Return InputDevices
        End Get
    End Property
    Protected Friend ReadOnly Property AudioOutputDevices As OutputDevice()
        Get
            Return OutputDevices
        End Get
    End Property
    Protected Friend ReadOnly Property LogicDelta As Double
        Get
            Return LogicDeltaTime
        End Get
    End Property
    Protected Friend ReadOnly Property RenderDelta As Double
        Get
            Return RenderDeltaTime
        End Get
    End Property
    Protected Friend ReadOnly Property HostingForm As Form
        Get
            Return ParentForm
        End Get
    End Property
    Public ReadOnly Property SurfaceRenderBounds As Rectangle
        Get
            Return SurfaceBounds
        End Get
    End Property
    Protected Friend Sub GetSurface(ByRef Surface As Graphics)
        Surface = BufferedSurface.Graphics
    End Sub
    Public Sub AddView(ByRef View As ViewBase)
        SyncLock Me
            View.Parent = Me
            View.CallLoad()
            Views.Add(View)
        End SyncLock
    End Sub
    Public Sub RemoveView(ByRef View As ViewBase)
        SyncLock Me
            View.Parent = Nothing
            View.CallUnload()
            Views.Remove(View)
        End SyncLock
    End Sub
    Public Sub LoadEngine()
        SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        SetStyle(ControlStyles.UserPaint, True)
        SetStyle(ControlStyles.Opaque, True)
        UpdateStyles()

        Dim AsyncLoader As New Threading.Thread(AddressOf AsyncEngineLoader)
        AsyncLoader.Start()
    End Sub
    Public Sub UnloadEngine()
        CloseEngine = True
    End Sub
    Public Sub UpdateAudioDeviceLists()
        Dim InputList As New List(Of InputDevice)
        Dim OutputList As New List(Of OutputDevice)
        For x As Integer = -1 To Un4seen.Bass.Bass.BASS_RecordGetDeviceCount - 1
            InputList.Add(New InputDevice(x))
        Next
        For x As Integer = -1 To Un4seen.Bass.Bass.BASS_GetDeviceCount - 1
            OutputList.Add(New OutputDevice(x))
        Next
        OutputDevices = OutputList.ToArray
        InputDevices = InputList.ToArray
        OutputList.Clear()
        InputList.Clear()
    End Sub
    Private Sub AsyncEngineLoader()
        UpdateAudioDeviceLists()
        UpdateSurfaceVars(True)
        LogicThreadExited = False
        RenderThreadExited = False
        CloseEngine = False
        Dim LogicThread As New Threading.Thread(AddressOf Logic)
        Dim RenderThread As New Threading.Thread(AddressOf Render)
        Dim CloseThread As New Threading.Thread(AddressOf Closer)
        LogicThread.Start()
        RenderThread.Start()
        CloseThread.Start()
    End Sub

    Private Sub Logic()
        Dim Limiter As New ThreadLimiter(LogicRate)
        Do Until ParentForm Is Nothing Or CloseEngine = True
            Dim Snapshot As TimeSpan = LogicDeltaTimer.Elapsed
            LogicDeltaTime = Snapshot.TotalMilliseconds
            LogicDeltaTimer.Restart()
            Limiter.IterationsPerSecond = LogicRate
            SyncLock Me
                If Views.Count > 0 Then
                    Dim Count As Integer = Views.Count - 1
                    For x = 0 To Count
                        If Views(x).Active = True Then Views(x).CallLogic()
                    Next
                End If
            End SyncLock
            If MouseBuffer.Length > 0 Then MouseBuffer.Dequeue()
            If KeyBuffer.Length > 0 Then KeyBuffer.Dequeue()
            Limiter.Limit()
        Loop
        LogicThreadExited = True
    End Sub
    Private Sub Render()
        Dim Limiter As New ThreadLimiter(RenderRate)
        Do Until ParentForm Is Nothing Or CloseEngine = True
            Dim Snapshot As TimeSpan = RenderDeltaTimer.Elapsed
            RenderDeltaTime = Snapshot.TotalMilliseconds
            RenderDeltaTimer.Restart()
            Limiter.IterationsPerSecond = RenderRate
            SyncLock Me
                If BufferedSurface IsNot Nothing Then
                    BufferedSurface.Graphics.Clear(Color.Black)
                    If Views.Count > 0 Then
                        Dim Count As Integer = Views.Count - 1
                        For x = 0 To Count
                            If Views(x).Visible = True Then Views(x).CallRender()
                        Next
                    End If
                    BufferedSurface.Render(Surface)
                End If
            End SyncLock
            UpdateSurfaceVars()
            Limiter.Limit()
        Loop
        RenderThreadExited = True
    End Sub
    Private Sub Closer()
        Dim Limiter As New ThreadLimiter(3)
        Do Until ParentForm Is Nothing Or (LogicThreadExited = True And RenderThreadExited = True)
            Limiter.Limit()
        Loop
        If Views.Count > 0 Then
            Dim Count As Integer = Views.Count - 1
            For x = 0 To Count
                Views(x).CallUnload()
            Next
        End If
        Views.Clear()
        RenderDeltaTimer.Stop()
        LogicDeltaTimer.Stop()
    End Sub

    Protected Overrides Sub OnMouseWheel(e As MouseEventArgs)
        MouseBuffer.Enqueue(MouseEvent.Initialize(e, MouseEvent.EventType.Scroll))
        If MouseBuffer.Length > MaxBufferCount Then
            Do Until MouseBuffer.Length = MaxBufferCount
                MouseBuffer.Dequeue()
            Loop
        End If
    End Sub
    Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
        MouseBuffer.Enqueue(MouseEvent.Initialize(e, MouseEvent.EventType.Up))
        If MouseBuffer.Length > MaxBufferCount Then
            Do Until MouseBuffer.Length = MaxBufferCount
                MouseBuffer.Dequeue()
            Loop
        End If
    End Sub
    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MouseBuffer.Enqueue(MouseEvent.Initialize(e, MouseEvent.EventType.Down))
        If MouseBuffer.Length > MaxBufferCount Then
            Do Until MouseBuffer.Length = MaxBufferCount
                MouseBuffer.Dequeue()
            Loop
        End If
    End Sub
    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        MouseBuffer.Enqueue(MouseEvent.Initialize(e, MouseEvent.EventType.Move))
        If MouseBuffer.Length > MaxBufferCount Then
            Do Until MouseBuffer.Length = MaxBufferCount
                MouseBuffer.Dequeue()
            Loop
        End If
    End Sub
    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean 'Intercept all keypresses, including command keys.' 
        KeyBuffer.Enqueue(KeyEvent.Initialize(keyData))
        If KeyBuffer.Length > MaxBufferCount Then
            Do Until KeyBuffer.Length = MaxBufferCount
                KeyBuffer.Dequeue()
            Loop
        End If
        Return True
    End Function

    Private Sub UpdateSurfaceVars(Optional UseCallerThread As Boolean = False)
        If SurfaceVarsRequireUpdate = True OrElse SurfaceBounds.Size <> Size Then
            If IsUpdatingSurfaceVars = False Then
                IsUpdatingSurfaceVars = True
                Dim AsyncThread As New Threading.Thread(AddressOf ControlThreadUpdater)
                AsyncThread.Start()
                If UseCallerThread = True Then
                    Do Until IsUpdatingSurfaceVars = False
                        Threading.Thread.Sleep(100)
                    Loop
                End If
            End If
        End If
    End Sub
    Private Sub ControlThreadUpdater()
        SyncLock Me
            Dim Invoker As New ThreadInvoker(AddressOf ControlThreadInvoker)
            Me.Invoke(Invoker)
            If Surface IsNot Nothing Then Surface.Dispose()
            If BufferedSurface IsNot Nothing Then BufferedSurface.Dispose()
            Surface = Graphics.FromHwnd(SurfaceHandle)
            Surface.CompositingMode = Drawing2D.CompositingMode.SourceOver
            Surface.CompositingQuality = Drawing2D.CompositingQuality.GammaCorrected
            Surface.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
            Surface.SmoothingMode = Drawing2D.SmoothingMode.None
            Surface.PixelOffsetMode = Drawing2D.PixelOffsetMode.None
            Surface.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias
            Surface.Clip = New Region(SurfaceBounds)
            If SurfaceBounds.Size.Width > 0 Then BufferedSurfaceContext.MaximumBuffer = SurfaceBounds.Size
            BufferedSurface = BufferedSurfaceContext.Allocate(Surface, SurfaceBounds)
            SurfaceVarsRequireUpdate = False
            IsUpdatingSurfaceVars = False
        End SyncLock
    End Sub
    Private Sub ControlThreadInvoker()
        If BorderlessFullscreen = True Then
            ParentForm.WindowState = FormWindowState.Normal
            ParentForm.FormBorderStyle = FormBorderStyle.None
            ParentForm.WindowState = FormWindowState.Maximized
            ParentForm.TopMost = True
        Else
            Dim Check As Boolean = (ParentForm.FormBorderStyle = FormBorderStyle.None)
            ParentForm.FormBorderStyle = FormBorderStyle.Sizable
            If Check = True Then ParentForm.WindowState = FormWindowState.Normal
            ParentForm.TopMost = False
        End If
        SurfaceHandle = Handle
        SurfaceBounds = Bounds
    End Sub

End Class
