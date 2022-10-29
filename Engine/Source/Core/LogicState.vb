Imports System.Drawing
Imports System.Windows.Forms

Public Class LogicState
    Public Property LPSTarget As Double = 60.0
    Public Resolution As SizeF
    Public Mouse As MouseState
    Public Keyboard As KeyboardState
    Friend LogicGovernor As New Governor(LPSTarget)
    Friend Entities As New List(Of Entity)
    Friend Parent As Form
    Friend EntityLock As New Object
    Public Readonly Property Delta As Double 
        Get
            Return LogicGovernor.Delta * 10
        End Get
    End Property

    Public Sub [Exit]
        Parent.Invoke(Sub() Parent.Close()) 
    End Sub
    
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