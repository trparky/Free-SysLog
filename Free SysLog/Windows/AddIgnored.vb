Public Class AddIgnored
    Public boolRegex, boolCaseSensitive As Boolean
    Public strIgnored As String
    Public boolSuccess As Boolean = False
    Public boolEditMode As Boolean = False

    Private Sub BtnAdd_Click(sender As Object, e As EventArgs) Handles BtnAdd.Click
        If Not ChkRegex.Checked Then
            boolRegex = ChkRegex.Checked
            boolCaseSensitive = ChkCaseSensitive.Checked
            strIgnored = TxtIgnored.Text
            boolSuccess = True
            Close()
        Else
            If IsRegexPatternValid(TxtIgnored.Text) Then
                boolRegex = ChkRegex.Checked
                boolCaseSensitive = ChkCaseSensitive.Checked
                strIgnored = TxtIgnored.Text
                boolSuccess = True
                Close()
            Else
                MsgBox("Invalid regex pattern detected.", MsgBoxStyle.Critical, Text)
            End If
        End If
    End Sub

    Private Sub AddReplacement_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If boolEditMode Then
            TxtIgnored.Text = strIgnored
            ChkRegex.Checked = boolRegex
            ChkCaseSensitive.Checked = boolCaseSensitive
            BtnAdd.Text = "Save"
        End If
    End Sub

    Private Sub AddReplacement_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.Enter AndAlso Not String.IsNullOrWhiteSpace(TxtIgnored.Text) Then BtnAdd.PerformClick()
    End Sub
End Class