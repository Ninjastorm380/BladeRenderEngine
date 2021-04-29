Public Class KeyBuffer : Inherits Queue
    Private ItemCount As Integer = 0 : Public ReadOnly Property Length As Integer
        Get
            Return ItemCount
        End Get
    End Property
    Public Shadows Sub Enqueue(ByRef Item As KeyEvent)
        MyBase.Enqueue(Item)
        ItemCount += 1
    End Sub
    Public Shadows Function Dequeue() As KeyEvent
        If ItemCount - 1 < 0 Then Return Nothing
        ItemCount -= 1
        Return CType(MyBase.Dequeue(), KeyEvent)
    End Function
    Public Shadows Sub Clear()
        SyncLock Me
            MyBase.Clear()
            ItemCount = 0
        End SyncLock
    End Sub
End Class
Public Class MouseBuffer : Inherits Queue
    Private ItemCount As Integer = 0 : Public ReadOnly Property Length As Integer
        Get
            Return ItemCount
        End Get
    End Property
    Public Shadows Sub Enqueue(Item As MouseEvent)
        SyncLock Me
            MyBase.Enqueue(Item)
            ItemCount += 1
        End SyncLock
    End Sub
    Public Shadows Function Dequeue() As MouseEvent
        SyncLock Me
            If ItemCount - 1 < 0 Then Return Nothing
            ItemCount -= 1
            Return CType(MyBase.Dequeue(), MouseEvent)
        End SyncLock
    End Function
    Public Shadows Sub Clear()
        SyncLock Me
            MyBase.Clear()
            ItemCount = 0
        End SyncLock
    End Sub
End Class
