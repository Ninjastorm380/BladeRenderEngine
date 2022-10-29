
    Public Partial Class Governor
        Private BaseRate As Double
        Private BaseTimeConstant As Int64
        Private BaseDelta As Int64
        Private ReadOnly BaseGovernorWatch As Stopwatch
        Private ReadOnly BaseSleepOffsetConstant As Int64
        Private BaseSleepTarget As TimeSpan
    End Class

