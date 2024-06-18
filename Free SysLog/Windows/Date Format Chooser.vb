Public Class DateFormatChooser
    Private boolDoneLoading As Boolean = False
    Private boolChanged As Boolean = False

    Private Sub DateFormatChooser_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Location = VerifyWindowLocation(My.Settings.DateChooserWindowLocation, Me)

        DateFormat1.Text = Now.ToLongDateString
        DateFormat2.Text = Now.ToShortDateString.Replace("/", "-").Replace("\", "-")

        Select Case My.Settings.DateFormat
            Case 1
                DateFormat1.Checked = True
                TxtCustom.Enabled = False
            Case 2
                DateFormat2.Checked = True
                TxtCustom.Enabled = False
            Case 3
                DateFormat3.Checked = True
                TxtCustom.Enabled = True
                TxtCustom.Text = My.Settings.CustomDateFormat
        End Select

        lblCustomDateOutput.Text = Now.ToString("MM-dd-yyyy")

        boolDoneLoading = True
    End Sub

    Private Sub DateFormatChooser_LocationChanged(sender As Object, e As EventArgs) Handles Me.LocationChanged
        My.Settings.DateChooserWindowLocation = Location
    End Sub

    Private Sub DateFormat3_CheckedChanged(sender As Object, e As EventArgs) Handles DateFormat3.CheckedChanged
        TxtCustom.Enabled = True
        If boolDoneLoading Then boolChanged = True
    End Sub

    Private Sub DateFormat1_CheckedChanged(sender As Object, e As EventArgs) Handles DateFormat1.CheckedChanged
        TxtCustom.Enabled = False
        If boolDoneLoading Then boolChanged = True
    End Sub

    Private Sub DateFormat2_CheckedChanged(sender As Object, e As EventArgs) Handles DateFormat2.CheckedChanged
        TxtCustom.Enabled = False
        If boolDoneLoading Then boolChanged = True
    End Sub

    Private Sub TxtCustom_KeyUp(sender As Object, e As KeyEventArgs) Handles TxtCustom.KeyUp
        lblCustomDateOutput.Text = Now.ToString(TxtCustom.Text)
        If boolDoneLoading Then boolChanged = True
    End Sub

    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        If DateFormat1.Checked Then
            My.Settings.DateFormat = 1
        ElseIf DateFormat2.Checked Then
            My.Settings.DateFormat = 2
        ElseIf DateFormat3.Checked Then
            My.Settings.DateFormat = 3
            My.Settings.CustomDateFormat = TxtCustom.Text
        End If

        My.Settings.Save()
        boolChanged = False
    End Sub

    Private Sub DateFormatChooser_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If boolChanged AndAlso MsgBox("Preferences have changed, do you want to save them?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, Text) = MsgBoxResult.Yes Then BtnSave.PerformClick()
    End Sub
End Class