Public Class AddAlert
    Public boolRegex, boolCaseSensitive As Boolean
    Public strLogText, strAlertText As String
    Public boolSuccess As Boolean = False
    Public boolEditMode As Boolean = False

    Private Sub BtnAdd_Click(sender As Object, e As EventArgs) Handles BtnAdd.Click
        TxtAlertText.Text = TxtAlertText.Text.Trim

        If Not ChkRegex.Checked Then
            boolRegex = ChkRegex.Checked
            boolCaseSensitive = ChkCaseSensitive.Checked
            strLogText = TxtLogText.Text
            strAlertText = TxtAlertText.Text
            boolSuccess = True
            Close()
        Else
            If IsRegexPatternValid(TxtLogText.Text) Then
                boolRegex = ChkRegex.Checked
                boolCaseSensitive = ChkCaseSensitive.Checked
                strLogText = TxtLogText.Text
                strAlertText = TxtAlertText.Text
                boolSuccess = True
                Close()
            Else
                MsgBox("Invalid regex pattern detected.", MsgBoxStyle.Critical, Text)
            End If
        End If
    End Sub

    Private Sub Add_Alert_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If boolEditMode Then
            TxtLogText.Text = strLogText
            TxtAlertText.Text = strAlertText
            ChkRegex.Checked = boolRegex
            ChkCaseSensitive.Checked = boolCaseSensitive
            BtnAdd.Text = "Save"
        End If
    End Sub

    Private Sub Add_Alert_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.Enter AndAlso Not String.IsNullOrWhiteSpace(TxtLogText.Text) Then BtnAdd.PerformClick()
    End Sub
End Class