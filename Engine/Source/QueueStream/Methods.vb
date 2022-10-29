    Public Partial Class QueueStream(Of T)
        Public Sub New(ByVal Optional Capacity As Int32 = 65536)
            BaseBuffer = New T(Capacity - 1) {}
            Array.Resize(BaseBuffer, Capacity)
        End Sub

        Public Sub Read(ByRef Buffer As T(), ByVal Offset As Int32, ByVal Seek As Int32, ByVal Count As Int32)
            BlockCopy(BaseBuffer, Seek, Buffer, Offset, Count)
        End Sub
        
        Public Sub ReadOne(ByRef Item As T, ByVal Seek As Int32)
            Item = BaseBuffer(Seek)
        End Sub

        Public Sub Shift(ByVal Amount As Int32)
            If WritePointer - Amount > 0 Then BlockCopy(BaseBuffer, Amount, BaseBuffer, 0, WritePointer - Amount)
            WritePointer = WritePointer - Amount
        End Sub

        Public Sub Write(ByRef Buffer As T(), ByVal Offset As Int32, ByVal Count As Int32)
            If BaseBuffer.Length < WritePointer + Count Then Array.Resize(BaseBuffer, WritePointer + Count)
            BlockCopy(Buffer, Offset, BaseBuffer, WritePointer, Count)
            WritePointer = WritePointer + Count
        End Sub
        
        Public Sub WriteOne(ByRef Item As T)
            If BaseBuffer.Length < WritePointer + 1 Then Array.Resize(BaseBuffer, WritePointer + 1)
            BaseBuffer(WritePointer) = Item
            WritePointer = WritePointer + 1
        End Sub
        
        Public Sub Clear()
            Shift(Length)
        End Sub

        Private Sub BlockCopy(ByRef InputBuffer As T(), ByVal InputBufferOffset As Int32, ByRef OutputBuffer As T(), ByVal OutputBufferOffset As Int32, ByVal Count As Int32)
            For Index As Int32 = InputBufferOffset To InputBufferOffset + Count - 1
                OutputBuffer((Index - InputBufferOffset) + OutputBufferOffset) = InputBuffer(Index)
            Next
        End Sub
    End Class