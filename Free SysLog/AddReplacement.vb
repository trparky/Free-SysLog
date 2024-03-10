Public Class AddReplacement
    Public boolRegex, boolCaseSensitive As Boolean
    Public strReplace, strReplaceWith As String
    Public boolSuccess As Boolean = False
    Public boolEditMode As Boolean = False

    Private Sub BtnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        boolRegex = chkRegex.Checked
        boolCaseSensitive = chkCaseSensitive.Checked
        strReplace = txtReplace.Text
        strReplaceWith = txtReplaceWith.Text
        boolSuccess = True
        Close()
    End Sub

    Private Sub AddReplacement_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If boolEditMode Then
            txtReplace.Text = strReplace
            txtReplaceWith.Text = strReplaceWith
            chkRegex.Checked = boolRegex
            chkCaseSensitive.Checked = boolCaseSensitive
            btnAdd.Text = "Save"
        End If
    End Sub

    Private Sub AddReplacement_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.Enter AndAlso Not String.IsNullOrWhiteSpace(txtReplace.Text) Then btnAdd.PerformClick()
    End Sub
End Class