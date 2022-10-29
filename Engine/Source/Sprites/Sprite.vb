Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Drawing.Text
Imports System.IO
Imports System.Runtime.InteropServices

Public Class Sprite
    Private BaseCurrentTextureRaw As Bitmap
    Private BaseCurrentTextureScaled As Bitmap
    Private CurrentTexture As Bitmap
    Private BaseTextures As List(Of Bitmap)
    Private BaseHitbox As New List(Of PointF)
    Private BaseHitboxBounds As Rectangle
    Private OldAngle As Int16 = -1
    Private OldSize As Size = Nothing
    Public Property IsCenterpoint As Boolean = True
    Private Offset As Point
    Private OffsetLocation As PointF
    Friend IsDisposed As Boolean = False
    Private Invalidated As Boolean = False
    Private TextureLock As Object
    Private DEBUG_DRAW_HITBOX As Boolean = False
    Private HitboxBitmap As New Bitmap(1, 1, PixelFormat.Format32bppPArgb)
    
    Friend Shared RefList As New List(Of Sprite)
    
    Public ReadOnly Property TextureCount As Int32
        Get
            Return Basetextures.Count
        End Get
    End Property

    
    Public Sub New()
            BaseTextures = New List(Of Bitmap)()
            TextureLock = New Object
            IsDisposed = False
            RefList.Add(Me)
    End Sub
    
    Public Sub Add(Paths As String())
            For Each Item In Paths
                Add(Item)
            Next
    End Sub
    
    Public Sub Add(Path As String)
        Dim TempBitmap As Bitmap = Bitmap.FromFile(Path)
        Dim NewBitmap As Bitmap = New Bitmap(TempBitmap.Width, TempBitmap.Height, PixelFormat.Format32bppPArgb)
        Dim Surface As Graphics = Graphics.FromImage(NewBitmap)
        Surface.CompositingMode = CompositingMode.SourceOver
        Surface.CompositingQuality = CompositingQuality.AssumeLinear
        Surface.InterpolationMode = InterpolationMode.NearestNeighbor
        Surface.SmoothingMode = SmoothingMode.None
        Surface.PixelOffsetMode = PixelOffsetMode.None
        Surface.TextRenderingHint = TextRenderingHint.AntiAlias
        Surface.DrawImage(TempBitmap, 0, 0, TempBitmap.Width, TempBitmap.Height)
        Surface.Dispose()
        TempBitmap.Dispose()
        BaseTextures.Add(NewBitmap)
        If BaseCurrentTextureRaw Is Nothing Then
            SetTexture(0)
            Size = BaseCurrentTextureRaw.Size
        End If
    End Sub
    
    Public Sub SetTexture(Index As Int32)
        SyncLock TextureLock
            BaseCurrentTextureRaw = BaseTextures(Index)
            Invalidated = True
        End SyncLock
    End Sub
    
    Public Sub Dispose()
        If IsDisposed = False Then
            For Each Item In BaseTextures
                If Item IsNot Nothing Then Item.Dispose()
            Next
            BaseTextures.Clear()
            If BaseCurrentTextureRaw IsNot Nothing Then BaseCurrentTextureRaw.Dispose()
            If BaseCurrentTextureScaled IsNot Nothing Then BaseCurrentTextureScaled.Dispose()
            If CurrentTexture IsNot Nothing Then CurrentTexture.Dispose()
            BaseTextures = Nothing
            BaseCurrentTextureRaw = Nothing
            BaseCurrentTextureScaled = Nothing
            CurrentTexture = Nothing
            TextureLock = Nothing
            IsDisposed = True
            RefList.Remove(Me)
        End If
    End Sub
    
    Public Sub Logic()
        SyncLock TextureLock
            ScaleTexture(Size,BaseCurrentTextureRaw, BaseCurrentTextureScaled)
            RotateTexture(Angle,BaseCurrentTextureScaled, CurrentTexture)
            GetHitBox(CurrentTexture, BaseHitbox, BaseHitBoxBounds)
            Invalidated = False
        End SyncLock
    End Sub
    
    Public Sub Render(State As RenderState)
        SyncLock TextureLock
            If CurrentTexture IsNot Nothing Then 
                If IsCenterpoint = True Then
                    State.Surface.DrawImage(CurrentTexture, OffsetLocation)
                Else
                    State.Surface.DrawImage(CurrentTexture, Location)
                End If
                
                If DEBUG_DRAW_HITBOX = true Then
                    If IsCenterpoint = True Then
                        State.Surface.DrawImage(HitboxBitmap, OffsetLocation)
                    Else
                        State.Surface.DrawImage(HitboxBitmap, Location) 
                    End If
                End If
            End If
        End SyncLock
    End Sub
    
    Private BaseLocation As PointF = New PointF(0, 0) 
    Public Property Location As PointF
        Get
            Return BaseLocation
        End Get
        Set(value As PointF)
            BaseLocation = value
            OffsetLocation = New PointF(BaseLocation.X - Offset.X, BaseLocation.Y - Offset.Y)
        End Set
    End Property
    
    Private BaseAngle As Int16 = 0
    Public Property Angle As Int16
        Get
            Return BaseAngle
        End Get
        Set(value As Int16)
            BaseAngle = value Mod 360
            Invalidated = True
        End Set
    End Property
    
    Private BaseSize As Size = New Size(16, 16) 
    Public Property Size As Size
        Get
            Return BaseSize
        End Get
        Set(value As Size)
            BaseSize = value
            Invalidated = True
        End Set
    End Property
    
    Public Sub GetHitBox(ByRef InputTexture As Bitmap, ByRef HitBoxData As List(Of PointF), ByRef HitBoxBounds As Rectangle)
        Dim TexBounds As Rectangle = New Rectangle(0, 0, InputTexture.Width, InputTexture.Height)
        Dim TexAccess As ImageLockMode = ImageLockMode.ReadOnly
        Dim TexType As PixelFormat = InputTexture.PixelFormat
        Dim TexRef As BitmapData = InputTexture.LockBits(TexBounds, TexAccess, TexType)
        Dim TexWidth As Integer = TexRef.Width
        Dim TexHeight As Integer = TexRef.Height
        Dim TexStride As Integer = TexRef.Stride
        Dim TexData((TexStride * TexHeight) - 1) As Byte
        Marshal.Copy(TexRef.Scan0, TexData, 0, TexData.Length)
        InputTexture.UnlockBits(TexRef)
        Offset = New Point(TexWidth / 2, TexHeight / 2)
        Dim X As Integer = 0
        Dim Y As Integer = 0
        If HitBoxData IsNot Nothing Then HitBoxData = New List(Of PointF)
        HitboxData.Clear()
        Dim TexCounterLength As Integer = TexData.Length - 1
        For TexDataIndex = 0 To TexCounterLength Step 4
            If TexData(TexDataIndex + 3) > 0 Then HitboxData.Add(New PointF(X, Y))
            X += 1 : If X = TexWidth Then X = 0
            Y = Math.Floor((TexDataIndex / TexStride))
        Next
        HitboxData.TrimExcess()
        HitboxBounds = TexBounds
    End Sub
    
    Public Shared Sub RotateTexture(ByRef NewAngle As Int16, ByRef Input As Bitmap, ByRef Output As Bitmap)
        Dim Width As Integer = Input.Width
        Dim Height As Integer = Input.Height
        Dim AnglePI As Double = NewAngle * Math.PI / 180
        Dim AngleCosine As Double = Math.Abs(Math.Cos(AnglePI))
        Dim AngleSine As Double = Math.Abs(Math.Sin(AnglePI))
        Dim NormalizedWidth As Integer = CInt((Width * AngleCosine + Height * AngleSine))
        Dim NormalizedHeight As Integer = CInt((Width * AngleSine + Height * AngleCosine))
        If Output IsNot Nothing Then Output.Dispose()
        Output = New Bitmap(NormalizedWidth, NormalizedHeight,  PixelFormat.Format32bppPArgb)
        Dim Surface As Graphics = Graphics.FromImage(Output)
        Surface.CompositingMode = CompositingMode.SourceOver
        Surface.CompositingQuality = CompositingQuality.AssumeLinear
        Surface.InterpolationMode = InterpolationMode.NearestNeighbor
        Surface.SmoothingMode = SmoothingMode.None
        Surface.PixelOffsetMode = PixelOffsetMode.None
        Surface.TextRenderingHint = TextRenderingHint.AntiAlias
        Dim SingleWidth As Single = Width / 2.0
        Dim SingleHeight As Single = Height / 2.0
        Surface.TranslateTransform(CSng((NormalizedWidth - Width)) / 2, CSng((NormalizedHeight - Height)) / 2)
        Surface.TranslateTransform(SingleWidth, SingleHeight)
        Surface.RotateTransform(NewAngle)
        Surface.TranslateTransform(-SingleWidth, -SingleHeight)
        Surface.DrawImage(Input, 0, 0)
        Surface.Dispose()
    End Sub
    
    Public Shared Sub ScaleTexture(ByVal NewSize As Size, ByRef Source As Bitmap, ByRef Destination As Bitmap)
        If Destination IsNot Nothing Then Destination.Dispose()
        Destination = New Bitmap(NewSize.Width, NewSize.Height, PixelFormat.Format32bppPArgb)
        Dim Surface As Graphics = Graphics.FromImage(Destination)
        Surface.CompositingMode = CompositingMode.SourceOver
        Surface.CompositingQuality = CompositingQuality.AssumeLinear
        Surface.InterpolationMode = InterpolationMode.NearestNeighbor
        Surface.SmoothingMode = SmoothingMode.None
        Surface.PixelOffsetMode = PixelOffsetMode.None
        Surface.TextRenderingHint = TextRenderingHint.AntiAlias
        Surface.DrawImage(Source, 0, 0, NewSize.Width, NewSize.Height)
        Surface.Dispose()
    End Sub
    
    Public Function IsCollidingWith(Target As Sprite) As Boolean
        If IsCenterpoint = True And Target.IsCenterpoint = True
            For Each SourcePoint In BaseHitbox
                For Each TargetPoint In Target.BaseHitbox
                    Dim SourceAbsolutePoint As PointF = New PointF((SourcePoint.X - Offset.X) + Location.X, (SourcePoint.Y - Offset.Y) + Location.Y)
                    Dim TargetAbsolutePoint As PointF = New PointF((TargetPoint.X - Target.Offset.X) + Target.Location.X, (TargetPoint.Y - Target.Offset.Y) + Target.Location.Y)
                    If (Math.Abs(SourceAbsolutePoint.X - TargetAbsolutePoint.X) < 1 And Math.Abs(SourceAbsolutePoint.Y - TargetAbsolutePoint.Y) < 1) Then Return True
                Next
            Next
        End If
        
        If IsCenterpoint = False And Target.IsCenterpoint = False
            For Each SourcePoint In BaseHitbox
                For Each TargetPoint In Target.BaseHitbox
                    Dim SourceAbsolutePoint As PointF = New PointF(SourcePoint.X + Location.X, SourcePoint.Y + Location.Y)
                    Dim TargetAbsolutePoint As PointF = New PointF(TargetPoint.X + Target.Location.X, TargetPoint.Y + Target.Location.Y)
                    If (Math.Abs(SourceAbsolutePoint.X - TargetAbsolutePoint.X) < 1 And Math.Abs(SourceAbsolutePoint.Y - TargetAbsolutePoint.Y) < 1) Then Return True
                Next
            Next
        End If
        
        If IsCenterpoint = True And Target.IsCenterpoint = False
            For Each SourcePoint In BaseHitbox
                For Each TargetPoint In Target.BaseHitbox
                    Dim SourceAbsolutePoint As PointF = New PointF((SourcePoint.X - Offset.X) + Location.X, (SourcePoint.Y - Offset.Y) + Location.Y)
                    Dim TargetAbsolutePoint As PointF = New PointF(TargetPoint.X + Target.Location.X, TargetPoint.Y + Target.Location.Y)
                    If (Math.Abs(SourceAbsolutePoint.X - TargetAbsolutePoint.X) < 1 And Math.Abs(SourceAbsolutePoint.Y - TargetAbsolutePoint.Y) < 1) Then Return True
                Next
            Next
        End If
        
        If IsCenterpoint = False And Target.IsCenterpoint = True
            For Each SourcePoint In BaseHitbox
                For Each TargetPoint In Target.BaseHitbox
                    Dim SourceAbsolutePoint As PointF = New PointF(SourcePoint.X + Location.X, SourcePoint.Y + Location.Y)
                    Dim TargetAbsolutePoint As PointF = New PointF((TargetPoint.X - Target.Offset.X) + Target.Location.X, (TargetPoint.Y - Target.Offset.Y) + Target.Location.Y)
                    If (Math.Abs(SourceAbsolutePoint.X - TargetAbsolutePoint.X) < 1 And Math.Abs(SourceAbsolutePoint.Y - TargetAbsolutePoint.Y) < 1) Then Return True
                Next
            Next
        End If
        
        Return False
    End Function
    
    Public Function IsCollidingWith(Target As PointF) As Boolean
        If IsCenterpoint = True
            For Each SourcePoint In BaseHitbox
                Dim SourceAbsolutePoint As PointF = New PointF((SourcePoint.X - Offset.X) + Location.X, (SourcePoint.Y - Offset.Y) + Location.Y)
                Dim TargetAbsolutePoint As PointF = New PointF(Target.X, Target.Y)
                If (Math.Abs(SourceAbsolutePoint.X - TargetAbsolutePoint.X) < 1 And Math.Abs(SourceAbsolutePoint.Y - TargetAbsolutePoint.Y) < 1) Then Return True
            Next
        End If
        
        If IsCenterpoint = False 
            For Each SourcePoint In BaseHitbox
                Dim SourceAbsolutePoint As PointF = New PointF(SourcePoint.X + Location.X, SourcePoint.Y + Location.Y)
                Dim TargetAbsolutePoint As PointF = New PointF(Target.X, Target.Y)
                If (Math.Abs(SourceAbsolutePoint.X - TargetAbsolutePoint.X) < 1 And Math.Abs(SourceAbsolutePoint.Y - TargetAbsolutePoint.Y) < 1) Then Return True
            Next
        End If
        
        Return False
    End Function
    
    Public Function HitBoxIntersectsPoint(Byref HitBox as List(Of Point), Byref HitBoxBounds as Rectangle, PT As Point) As Boolean
        'Dim RelativeOffsetPT As New Point(PT.X - (Location.X - Offset.X), PT.Y - (Location.Y - Offset.Y))
        If HitboxBounds.Contains(PT) AndAlso Hitbox.Contains(PT) Then Return True Else Return False
    End Function
End Class