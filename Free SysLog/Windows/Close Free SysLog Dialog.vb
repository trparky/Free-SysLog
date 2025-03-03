Public Class CloseFreeSysLogDialog
    Public Property MyParentForm As Form1

    Private Sub Close_Free_SysLog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Media.SystemSounds.Asterisk.Play()
        PictureBox1.Image = SystemIcons.Question.ToBitmap()
        ChkConfirmClose.Checked = My.Settings.boolConfirmClose
    End Sub

    Private Sub BtnYes_Click(sender As Object, e As EventArgs) Handles BtnYes.Click
        DialogResult = DialogResult.Yes
        Close()
    End Sub

    Private Sub BtnNo_Click(sender As Object, e As EventArgs) Handles BtnNo.Click
        DialogResult = DialogResult.No
        Close()
    End Sub

    Private Sub BtnMinimize_Click(sender As Object, e As EventArgs) Handles BtnMinimize.Click
        DialogResult = DialogResult.Cancel
        Close()
    End Sub

    Private Sub CloseFreeSysLogDialog_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.Y Then
            BtnYes.PerformClick()
        ElseIf e.KeyCode = Keys.N Then
            BtnNo.PerformClick()
        ElseIf e.KeyCode = Keys.M Then
            BtnMinimize.PerformClick()
        End If
    End Sub

    Private Sub ChkConfirmClose_Click(sender As Object, e As EventArgs) Handles ChkConfirmClose.Click
        My.Settings.boolConfirmClose = ChkConfirmClose.Checked
        If MyParentForm IsNot Nothing Then MyParentForm.ChkEnableConfirmCloseToolStripItem.Checked = ChkConfirmClose.Checked
    End Sub
End Class