Imports System.Drawing.Drawing2D

''' <summary>
''' A balloon overlay that lives ON the form as a child control (no separate window).
''' It is added to the form's Controls, brought to front, and clipped to a balloon shape
''' via Region, so the controls underneath show through around the tail and corners.
''' </summary>
Public Class BalloonControl
    Inherits Control

    Private Const TailHeight As Integer = 12
    Private Const TailWidth As Integer = 20
    Private Const CornerRadius As Integer = 10
    Private Const Pad As Integer = 10
    Private Const MaxTextWidth As Integer = 260
    Private Const BorderWidth As Single = 2.0F

    Private ReadOnly _timer As New Timer()
    Private _tailX As Integer
    Private _tailOnTop As Boolean   ' True = balloon sits below the target, tail on top

    Public Sub New(message As String, Optional autoCloseMs As Integer = 4000)
        SetStyle(ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.ResizeRedraw, True)

        Text = message
        Font = SystemFonts.MessageBoxFont
        BackColor = Color.FromArgb(255, 255, 225)
        ForeColor = Color.Black
        Visible = False

        Dim textSize = TextRenderer.MeasureText(Text, Font, New Size(MaxTextWidth, 0), TextFormatFlags.WordBreak Or TextFormatFlags.NoPadding)
        Size = New Size(textSize.Width + Pad * 2, textSize.Height + Pad * 2 + TailHeight)

        If autoCloseMs > 0 Then
            _timer.Interval = autoCloseMs
            AddHandler _timer.Tick, Sub() Dismiss()
        End If
    End Sub

    ''' <summary>
    ''' Show the balloon pointing at <paramref name="target"/>. Prefers above the control;
    ''' flips below if there isn't room. Stays inside the form's client area.
    ''' </summary>
    Public Sub ShowFor(target As Control)
        Dim host As Form = target.FindForm()
        If host Is Nothing Then Return

        ' Where the tail tip should land, in the host form's client coordinates
        Dim tipAbove As Point = host.PointToClient(target.PointToScreen(New Point(target.Width \ 2, 0)))
        Dim tipBelow As Point = host.PointToClient(target.PointToScreen(New Point(target.Width \ 2, target.Height)))

        Place(host, tipAbove, tipBelow)
    End Sub

    ''' <summary>
    ''' Show the balloon next to the mouse cursor, with the tail pointing at it.
    ''' Prefers above the cursor; flips below if there isn't room.
    ''' </summary>
    Public Sub ShowAtCursor(host As Form)
        Dim pt As Point = host.PointToClient(MousePosition)
        ' The arrow's hotspot is its top-left corner and the image extends ~22px below it,
        ' so the "below" tip is offset to clear the cursor graphic.
        Place(host, New Point(pt.X, pt.Y - 2), New Point(pt.X, pt.Y + 22))
    End Sub

    ' tipAbove/tipBelow: client-coordinate points the tail should touch if the balloon
    ' sits above / below the anchor. Everything is clamped to stay inside the host form.
    Private Sub Place(host As Form, tipAbove As Point, tipBelow As Point)
        _tailOnTop = tipAbove.Y - Height < 0          ' not enough room above -> go below
        Dim tip As Point = If(_tailOnTop, tipBelow, tipAbove)

        ' Center on the target, but clamp horizontally inside the form
        Dim left As Integer = tip.X - Width \ 2
        left = Math.Max(0, Math.Min(left, host.ClientSize.Width - Width))

        Dim minTail As Integer = CornerRadius + TailWidth \ 2
        _tailX = Math.Max(minTail, Math.Min(tip.X - left, Width - minTail))

        Location = New Point(left, If(_tailOnTop, tip.Y, tip.Y - Height))

        UpdateShape()
        host.Controls.Add(Me)    ' added to the FORM so it isn't clipped by a GroupBox/Panel
        BringToFront()

        Visible = True
        If _timer.Interval > 0 Then _timer.Start()
    End Sub

    Public Sub Dismiss()
        _timer.Stop()
        Parent?.Controls.Remove(Me)
        Dispose()
    End Sub

    Private Function BuildPath() As GraphicsPath
        Dim r As Integer = CornerRadius
        Dim w As Integer = Width - 1
        Dim top As Integer = If(_tailOnTop, TailHeight, 0)
        Dim bottom As Integer = top + (Height - TailHeight) - 1
        Dim half As Integer = TailWidth \ 2
        Dim path As New GraphicsPath()

        path.AddArc(0, top, r * 2, r * 2, 180, 90)                     ' top-left

        If _tailOnTop Then
            path.AddLine(_tailX - half, top, _tailX, 0)
            path.AddLine(_tailX, 0, _tailX + half, top)
        End If

        path.AddArc(w - r * 2, top, r * 2, r * 2, 270, 90)             ' top-right
        path.AddArc(w - r * 2, bottom - r * 2, r * 2, r * 2, 0, 90)    ' bottom-right

        If Not _tailOnTop Then
            path.AddLine(_tailX + half, bottom, _tailX, bottom + TailHeight)
            path.AddLine(_tailX, bottom + TailHeight, _tailX - half, bottom)
        End If

        path.AddArc(0, bottom - r * 2, r * 2, r * 2, 90, 90)           ' bottom-left
        path.CloseFigure()

        Return path
    End Function

    Private Sub UpdateShape()
        Using path = BuildPath()
            Region = New Region(path)
        End Using
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias

        Using path = BuildPath(), fill As New SolidBrush(BackColor), border As New Pen(Color.FromArgb(118, 118, 118), BorderWidth * 2)
            border.LineJoin = LineJoin.Round   ' avoids a spiky miter at the tail tip
            e.Graphics.FillPath(fill, path)
            e.Graphics.DrawPath(border, path)
        End Using

        Dim top As Integer = If(_tailOnTop, TailHeight, 0) + Pad
        Dim textRect As New Rectangle(Pad, top, Width - Pad * 2, Height - TailHeight - Pad * 2)

        TextRenderer.DrawText(e.Graphics, Text, Font, textRect, ForeColor, TextFormatFlags.WordBreak Or TextFormatFlags.NoPadding)
    End Sub

    Protected Overrides Sub OnClick(e As EventArgs)
        MyBase.OnClick(e)
        Dismiss()   ' click the balloon to dismiss it
    End Sub

    Protected Overrides Sub Dispose(disposing As Boolean)
        If disposing Then _timer.Dispose()
        MyBase.Dispose(disposing)
    End Sub
End Class