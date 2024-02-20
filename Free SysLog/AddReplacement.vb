Public Class AddReplacement
    Public boolRegex, boolCaseSensitive As Boolean
    Public strReplace, strReplaceWith As String
    Public boolSuccess As Boolean = False

    Private Sub BtnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        boolRegex = chkRegex.Checked
        boolCaseSensitive = chkCaseSensitive.Checked
        strReplace = txtReplace.Text
        strReplaceWith = txtReplaceWith.Text
        boolSuccess = True
        Close()
    End Sub

    Private Sub ChkRegex_Click(sender As Object, e As EventArgs) Handles chkRegex.Click
        chkCaseSensitive.Checked = False
    End Sub

    Private Sub ChkCaseSensitive_Click(sender As Object, e As EventArgs) Handles chkCaseSensitive.Click
        chkRegex.Checked = False
    End Sub
End Class