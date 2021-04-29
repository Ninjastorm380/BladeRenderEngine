Public MustInherit Class ViewBase
    Friend Property Parent As Viewport = Nothing
    Protected Friend Property Active As Boolean = False
    Protected Friend Property Visible As Boolean = False
    Friend Property Sprites As New List(Of SpriteBase)
    Public ReadOnly Property SurfaceRenderBounds As Rectangle
        Get
            Return Parent.SurfaceRenderBounds
        End Get
    End Property
    Protected Friend Property BorderlessFullscreen As Boolean
        Get
            Return Parent.BorderlessFullscreen
        End Get
        Set(value As Boolean)
            Parent.BorderlessFullscreen = value
        End Set
    End Property
    Protected Friend ReadOnly Property LogicDelta As Double
        Get
            Return Parent.LogicDelta
        End Get
    End Property
    Protected Friend ReadOnly Property RenderDelta As Double
        Get
            Return Parent.RenderDelta
        End Get
    End Property
    Protected Friend ReadOnly Property AudioInputDevices As InputDevice()
        Get
            Return Parent.AudioInputDevices
        End Get
    End Property
    Protected Friend ReadOnly Property AudioOutputDevices As OutputDevice()
        Get
            Return Parent.AudioOutputDevices
        End Get
    End Property
    Protected Friend ReadOnly Property KeyBuffer As KeyBuffer
        Get
            Return Parent.KeyBuffer
        End Get
    End Property
    Protected Friend ReadOnly Property MouseBuffer As MouseBuffer
        Get
            Return Parent.MouseBuffer
        End Get
    End Property
    Protected Friend ReadOnly Property HostingForm As Form
        Get
            Return Parent.HostingForm
        End Get
    End Property

    Protected Friend Sub GetSurface(ByRef Surface As Graphics)
        Parent.GetSurface(Surface)
    End Sub
    Protected Friend Sub AddView(ByRef View As ViewBase)
        Parent.AddView(View)
    End Sub
    Protected Friend Sub RemoveView(ByRef View As ViewBase)
        Parent.RemoveView(View)
    End Sub
    Protected Friend Sub AddSprite(ByRef Sprite As SpriteBase)
        SyncLock Me
            Sprite.Parent = Me
            Sprite.CallLoad()
            Sprites.Add(Sprite)
        End SyncLock
    End Sub
    Protected Friend Sub RemoveSprite(ByRef Sprite As SpriteBase)
        SyncLock Me
            Sprite.Parent = Nothing
            Sprite.CallUnload()
            Sprites.Remove(Sprite)
        End SyncLock
    End Sub

    Public Sub CallLoad()
        SyncLock Me
            Load()
        End SyncLock
    End Sub
    Public Sub CallUnload()
        SyncLock Me
            Unload()
        End SyncLock
    End Sub
    Public Sub CallLogic()
        SyncLock Me
            Logic()
            Dim count As Integer = Sprites.Count - 1
            For x = 0 To count
                Sprites(x).CallLogic()
            Next
        End SyncLock
    End Sub
    Public Sub CallRender()
        SyncLock Me
            Render()
            Dim count As Integer = Sprites.Count - 1
            For x = 0 To count
                Sprites(x).CallRender()
            Next
        End SyncLock
    End Sub
    Protected MustOverride Sub Load()
    Protected MustOverride Sub Unload()
    Protected MustOverride Sub Logic()
    Protected MustOverride Sub Render()
End Class
