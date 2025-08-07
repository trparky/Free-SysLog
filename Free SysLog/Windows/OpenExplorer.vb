﻿Public Class OpenExplorer
    Public Property MyParentForm As Form

    Private Sub OpenExplorer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Media.SystemSounds.Asterisk.Play()
        PictureBox1.Image = SystemIcons.Question.ToBitmap()
        ChkAskEveryTime.Checked = My.Settings.AskOpenExplorer
    End Sub

    Private Sub BtnYes_Click(sender As Object, e As EventArgs) Handles BtnYes.Click
        DialogResult = DialogResult.Yes
        Close()
    End Sub

    Private Sub BtnNo_Click(sender As Object, e As EventArgs) Handles BtnNo.Click
        DialogResult = DialogResult.No
        Close()
    End Sub

    Private Sub CloseFreeSysLogDialog_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.Y Then
            BtnYes.PerformClick()
        ElseIf e.KeyCode = Keys.N Then
            BtnNo.PerformClick()
        End If
    End Sub

    Private Sub ChkAskEveryTime_Click(sender As Object, e As EventArgs) Handles ChkAskEveryTime.Click
        SupportCode.AskOpenExplorer = ChkAskEveryTime.Checked
    End Sub
End Class