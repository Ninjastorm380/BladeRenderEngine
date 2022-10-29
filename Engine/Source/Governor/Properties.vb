    Public Partial Class Governor
        Public Property Rate As Double
            Get
                Return BaseRate
            End Get
            Set(ByVal value As Double)
                BaseRate = value
                BaseTimeConstant = TimeSpan.TicksPerMillisecond * CLng((1000.0 / BaseRate))
                BaseSleepTarget = New TimeSpan(BaseTimeConstant - BaseSleepOffsetConstant)
            End Set
        End Property

        Public ReadOnly Property Delta As Double
            Get
                Return BaseDelta / CDbl(BaseTimeConstant)
            End Get
        End Property

        Public ReadOnly Property DeltaInverse As Double
            Get
                Return CDbl(BaseTimeConstant) / BaseDelta
            End Get
        End Property

        Public ReadOnly Property IterationElapsed As Double
            Get
                Return (1000.0 / BaseDelta)
            End Get
        End Property

        Public ReadOnly Property IterationProjected As Double
            Get
                Return (1000.0 / BaseRate)
            End Get
        End Property
    End Class