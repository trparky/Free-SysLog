Public Class AddReplacement
    Public boolRegex As Boolean
    Public strReplace, strReplaceWith As String
    Public boolSuccess As Boolean = False

    Private Sub BtnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        boolRegex = chkRegex.Checked
        strReplace = txtReplace.Text
        strReplaceWith = txtReplaceWith.Text
        boolSuccess = True
        Close()
    End Sub
End Class