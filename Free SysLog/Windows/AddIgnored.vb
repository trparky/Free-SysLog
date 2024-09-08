Imports Free_SysLog.SupportCode

Public Class AddIgnored
    Public boolRegex, boolCaseSensitive As Boolean
    Public strIgnored As String
    Public boolSuccess As Boolean = False
    Public boolEditMode As Boolean = False
    Public boolEnabled As Boolean = True
    Private boolChanged As Boolean = False

    Private Sub BtnAdd_Click(sender As Object, e As EventArgs) Handles BtnAdd.Click
        If Not ChkRegex.Checked Then
            boolRegex = ChkRegex.Checked
            boolCaseSensitive = ChkCaseSensitive.Checked
            strIgnored = TxtIgnored.Text
            boolEnabled = ChkEnabled.Checked
            boolSuccess = True
            boolChanged = False
            Close()
        Else
            If IsRegexPatternValid(TxtIgnored.Text) Then
                boolRegex = ChkRegex.Checked
                boolCaseSensitive = ChkCaseSensitive.Checked
                strIgnored = TxtIgnored.Text
                boolEnabled = ChkEnabled.Checked
                boolSuccess = True
                boolChanged = False
                Close()
            Else
                MsgBox("Invalid regex pattern detected.", MsgBoxStyle.Critical, Text)
            End If
        End If
    End Sub

    Private Sub AddIgnoredOrAlert_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If boolEditMode Then
            TxtIgnored.Text = strIgnored
            ChkRegex.Checked = boolRegex
            ChkCaseSensitive.Checked = boolCaseSensitive
            ChkEnabled.Checked = boolEnabled
            BtnAdd.Text = "Save"
        End If
    End Sub

    Private Sub AddReplacement_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.Enter AndAlso Not String.IsNullOrWhiteSpace(TxtIgnored.Text) Then
            BtnAdd.PerformClick()
        ElseIf e.KeyCode = Keys.Escape Then
            Close()
        End If
    End Sub

    Private Sub TxtIgnored_KeyUp(sender As Object, e As KeyEventArgs) Handles TxtIgnored.KeyUp
        If e.KeyCode <> Keys.Escape Then boolChanged = True
    End Sub

    Private Sub ChkCaseSensitive_Click(sender As Object, e As EventArgs) Handles ChkCaseSensitive.Click
        boolChanged = True
    End Sub

    Private Sub ChkRegex_Click(sender As Object, e As EventArgs) Handles ChkRegex.Click
        boolChanged = True
    End Sub

    Private Sub AddIgnored_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If boolChanged Then
            If MsgBox("You have changed some data on this window, do you want to save it before closing?", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton1 + MsgBoxStyle.Question, Text) = MsgBoxResult.Yes Then
                BtnAdd.PerformClick()
            Else
                boolChanged = False
                Close()
            End If
        End If
    End Sub

    Private Sub ChkEnabled_Click(sender As Object, e As EventArgs) Handles ChkEnabled.Click
        boolChanged = True
    End Sub
End Class