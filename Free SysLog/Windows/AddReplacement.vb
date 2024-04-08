Public Class AddReplacement
    Public boolRegex, boolCaseSensitive As Boolean
    Public strReplace, strReplaceWith As String
    Public boolSuccess As Boolean = False
    Public boolEditMode As Boolean = False

    Private Sub BtnAdd_Click(sender As Object, e As EventArgs) Handles BtnAdd.Click
        If Not ChkRegex.Checked Then
            boolRegex = ChkRegex.Checked
            boolCaseSensitive = ChkCaseSensitive.Checked
            strReplace = TxtReplace.Text
            strReplaceWith = TxtReplaceWith.Text
            boolSuccess = True
            Close()
        Else
            If IsRegexPatternValid(TxtReplace.Text) Then
                boolRegex = ChkRegex.Checked
                boolCaseSensitive = ChkCaseSensitive.Checked
                strReplace = TxtReplace.Text
                strReplaceWith = TxtReplaceWith.Text
                boolSuccess = True
                Close()
            Else
                MsgBox("Invalid regex pattern detected.", MsgBoxStyle.Critical, Text)
            End If
        End If
    End Sub

    Private Sub AddReplacement_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If boolEditMode Then
            TxtReplace.Text = strReplace
            TxtReplaceWith.Text = strReplaceWith
            ChkRegex.Checked = boolRegex
            ChkCaseSensitive.Checked = boolCaseSensitive
            BtnAdd.Text = "Save"
        End If
    End Sub

    Private Sub AddReplacement_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.Enter AndAlso Not String.IsNullOrWhiteSpace(TxtReplace.Text) Then BtnAdd.PerformClick()
    End Sub
End Class