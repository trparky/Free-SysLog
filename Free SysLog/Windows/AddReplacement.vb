Public Class AddReplacement
    Public boolRegex, boolCaseSensitive As Boolean
    Public strReplace, strReplaceWith As String
    Public boolSuccess As Boolean = False
    Public boolEditMode As Boolean = False
    Private boolChanged As Boolean = False

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

    Private Sub AddReplacement_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If boolChanged Then
            If MsgBox("You have changed some data on this window, do you want to save it before closing?", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton1 + MsgBoxStyle.Question, Text) = MsgBoxResult.Yes Then
                BtnAdd.PerformClick()
            Else
                boolChanged = False
                Close()
            End If
        End If
    End Sub

    Private Sub ChkCaseSensitive_Click(sender As Object, e As EventArgs) Handles ChkCaseSensitive.Click
        boolChanged = True
    End Sub

    Private Sub ChkRegex_Click(sender As Object, e As EventArgs) Handles ChkRegex.Click
        boolChanged = True
    End Sub

    Private Sub TxtReplace_KeyUp(sender As Object, e As KeyEventArgs) Handles TxtReplace.KeyUp
        If e.KeyCode <> Keys.Escape Then boolChanged = True
    End Sub

    Private Sub TxtReplaceWith_KeyUp(sender As Object, e As KeyEventArgs) Handles TxtReplaceWith.KeyUp
        If e.KeyCode <> Keys.Escape Then boolChanged = True
    End Sub
End Class