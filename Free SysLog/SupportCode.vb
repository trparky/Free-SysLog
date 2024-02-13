Module SupportCode
    Public Function VerifyWindowLocation(point As Point) As Point
        Return If(point.X < 0 Or point.Y < 0, New Point(0, 0), point)
    End Function
End Module