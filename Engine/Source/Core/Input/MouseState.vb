Imports System.Drawing
Imports System.Numerics

Public Class MouseState
    Public Property Location As Point
    Public Property Buttons as New BigInteger
    Public Shared H00 As BigInteger = New BigInteger(2^000)
    Public Shared H01 As BigInteger = New BigInteger(2^001)
    Public Shared H02 As BigInteger = New BigInteger(2^002)
    Public Shared H03 As BigInteger = New BigInteger(2^003)
    Public Shared H04 As BigInteger = New BigInteger(2^004)
    Public Shared H05 As BigInteger = New BigInteger(2^005)
    Public Shared H06 As BigInteger = New BigInteger(2^006)
    Public Shared H07 As BigInteger = New BigInteger(2^007)
    Public Shared H08 As BigInteger = New BigInteger(2^008)
End Class