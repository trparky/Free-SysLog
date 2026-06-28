Public Class Color_Picker
    Public Property ChosenColor As Color

    Private Sub ColorGrid1_Click(sender As Object, e As EventArgs) Handles ColorGrid1.Click
        ColorEditor1.Color = ColorGrid1.Color
        ColorWheel1.Color = ColorGrid1.Color
        lblColorShower.BackColor = ColorGrid1.Color
    End Sub

    Private Sub ColorEditor1_ColorChanged(sender As Object, e As EventArgs) Handles ColorEditor1.ColorChanged
        lblColorShower.BackColor = ColorEditor1.Color
        ColorWheel1.Color = ColorEditor1.Color
    End Sub

    Private Sub ColorWheel1_ColorChanged(sender As Object, e As EventArgs) Handles ColorWheel1.ColorChanged
        lblColorShower.BackColor = ColorWheel1.Color
    End Sub

    Private Sub Color_Picker_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadCustomColors()

        ColorEditor1.Color = ChosenColor
        ColorWheel1.Color = ChosenColor
        ColorGrid1.Color = ChosenColor
        lblColorShower.BackColor = ChosenColor
    End Sub

    Private Sub Color_Picker_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        My.Settings.ColorPickerDialogSize = Size
        SaveCustomColors()
    End Sub

    Private Sub btnChooseColor_Click(sender As Object, e As EventArgs) Handles btnChooseColor.Click
        ChosenColor = lblColorShower.BackColor
        Close()
    End Sub

    Private Sub btnSaveColorToCustomColors_Click(sender As Object, e As EventArgs) Handles btnSaveColorToCustomColors.Click
        AddCustomColor(lblColorShower.BackColor)
    End Sub

    Private Sub AddCustomColor(c As Color)
        Dim colors As Cyotek.Windows.Forms.ColorCollection = ColorGrid1.CustomColors

        'Avoid duplicates
        For i = 0 To colors.Count - 1
            If colors(i).ToArgb() = c.ToArgb() Then
                ColorGrid1.Color = c
                Return
            End If
        Next

        'Find an empty/default slot
        For i = 0 To colors.Count - 1
            If colors(i).IsEmpty OrElse colors(i).ToArgb() = Color.White.ToArgb() Then
                colors(i) = c
                ColorGrid1.Color = c
                ColorGrid1.Invalidate()
                Return
            End If
        Next

        'No empty slots; shift older colors down and insert at front
        For i = colors.Count - 1 To 1 Step -1
            colors(i) = colors(i - 1)
        Next

        If colors.Count = 0 Then
            colors.Add(c)
        Else
            colors(0) = c
        End If

        ColorGrid1.Color = c
        ColorGrid1.Invalidate()
    End Sub

    Private Sub SaveCustomColors()
        If My.Settings.CustomColors Is Nothing Then My.Settings.CustomColors = New Specialized.StringCollection()
        My.Settings.CustomColors.Clear()

        For Each c As Color In ColorGrid1.CustomColors
            My.Settings.CustomColors.Add(c.ToArgb().ToString())
        Next

        My.Settings.Save()
    End Sub

    Private Function IsEmptyCustomColor(c As Color) As Boolean
        Return c.IsEmpty OrElse c.ToArgb() = Color.White.ToArgb()
    End Function

    Private Sub LoadCustomColors()
        If My.Settings.CustomColors Is Nothing OrElse My.Settings.CustomColors.Count = 0 Then Exit Sub

        Dim argb As Integer
        Dim color As Color

        For i As Integer = 0 To Math.Min(ColorGrid1.CustomColors.Count - 1, My.Settings.CustomColors.Count - 1)
            If Integer.TryParse(My.Settings.CustomColors(i), argb) Then
                color = Color.FromArgb(argb)
                If Not IsEmptyCustomColor(color) Then ColorGrid1.CustomColors(i) = color
            End If
        Next

        ColorGrid1.Invalidate()
    End Sub

    Private Sub btnClearCustomColors_Click(sender As Object, e As EventArgs) Handles btnClearCustomColors.Click
        My.Settings.CustomColors.Clear()
        ColorGrid1.CustomColors.Clear()
        ColorGrid1.Invalidate()
    End Sub

    Private Sub Color_Picker_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.Enter Then
            ChosenColor = lblColorShower.BackColor
            Close()
        ElseIf e.KeyCode = Keys.Escape Then
            Close()
        End If
    End Sub
End Class