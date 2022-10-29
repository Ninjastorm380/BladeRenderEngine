Imports System.Drawing

Public Class RenderState
    Public Property FPSTarget As Double = 60.0
    Friend RenderGovernor As New Governor(FPSTarget)
    Friend Entities As New List(Of Entity)
    Friend EntityLock As New Object
    Public Property Surface As Graphics
    Public Sub Add(Entity As Entity)
        SyncLock EntityLock
            Entities.Add(Entity)
        End SyncLock

    End Sub
    Public Sub Remove(Entity As Entity)
        SyncLock EntityLock
            Entities.Remove(Entity)
        End SyncLock
    End Sub
End Class