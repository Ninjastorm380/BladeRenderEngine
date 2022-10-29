Public MustInherit Class Entity
    Public MustOverride Sub Render(State As RenderState)
    Public MustOverride Sub Logic(State As LogicState)
    Public MustOverride Sub Load(LogicState As LogicState, RenderState As RenderState)
    Public MustOverride Sub Unload(LogicState As LogicState, Renderstate As RenderState)
    Public Property Visible As Boolean = False
    Public Property Active As Boolean = False
End Class