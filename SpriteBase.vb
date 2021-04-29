Public MustInherit Class SpriteBase
    Friend Property Parent As ViewBase = Nothing
    Protected Friend Property Active As Boolean = False
    Protected Friend Property Visible As Boolean = False
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
    Protected Friend ReadOnly Property AudioInputDevices As InputDevice()
        Get
            Return Parent.audioinputdevices
        End Get
    End Property
    Protected Friend ReadOnly Property AudioOutputDevices As OutputDevice()
        Get
            Return Parent.audioOutputDevices
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
        Parent.AddSprite(Sprite)
    End Sub
    Protected Friend Sub RemoveSprite(ByRef Sprite As SpriteBase)
        Parent.RemoveSprite(Sprite)
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
        End SyncLock
    End Sub
    Public Sub CallRender()
        SyncLock Me
            Render()
        End SyncLock
    End Sub
    Protected MustOverride Sub Load()
    Protected MustOverride Sub Unload()
    Protected MustOverride Sub Logic()
    Protected MustOverride Sub Render()
End Class
