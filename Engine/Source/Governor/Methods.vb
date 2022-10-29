    Imports System.Threading

Public Partial Class Governor
        Public Sub New(ByVal Optional Rate As Double = 100.0)
            BaseRate = Rate
            BaseSleepOffsetConstant = TimeSpan.TicksPerMillisecond / 10
            BaseTimeConstant = TimeSpan.TicksPerMillisecond * CLng((1000.0 / BaseRate))
            BaseSleepTarget = New TimeSpan(BaseTimeConstant - BaseSleepOffsetConstant)
            BaseGovernorWatch = Stopwatch.StartNew()
        End Sub
        
        Public Sub Pause()
            BaseGovernorWatch.Stop()
        End Sub
        
        Public Sub [Resume]()
            BaseGovernorWatch.Start()
        End Sub

        Public Sub Limit()
            If BaseSleepTarget.Ticks > 1000 Then
                Thread.Sleep(BaseSleepTarget)
            End If

            Do
            Loop While BaseGovernorWatch.ElapsedTicks < BaseTimeConstant

            BaseDelta = BaseGovernorWatch.ElapsedTicks
            BaseGovernorWatch.Restart()
        End Sub
    End Class