﻿Public Class LogViewer
    Public strLogText, strRawLogText As String
    Public MyParentForm As Form1

    Private Sub Log_Viewer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If My.Settings.font IsNot Nothing Then LogText.Font = My.Settings.font

        ChkShowRawLog.Checked = My.Settings.boolShowRawLogOnLogViewer
        Size = My.Settings.logViewerWindowSize
        LogText.Text = If(ChkShowRawLog.Checked, strRawLogText, strLogText)
        LogText.Select(0, 0)

        If String.IsNullOrWhiteSpace(txtAlertText.Text) Then txtAlertText.Text = "(This log entry has no alert text associated with it)"
    End Sub

    Private Sub Log_Viewer_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        My.Settings.logViewerWindowSize = Size
    End Sub

    Private Sub BtnClose_Click(sender As Object, e As EventArgs) Handles BtnClose.Click
        Close()
    End Sub

    Private Sub Log_Viewer_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyData = Keys.Escape Then Close()
    End Sub

    Private Sub ChkShowRawLog_Click(sender As Object, e As EventArgs) Handles ChkShowRawLog.Click
        My.Settings.boolShowRawLogOnLogViewer = ChkShowRawLog.Checked
        LogText.Text = If(ChkShowRawLog.Checked, strRawLogText.Replace("{newline}", vbCrLf, StringComparison.OrdinalIgnoreCase), strLogText)
        If MyParentForm IsNot Nothing Then MyParentForm.ShowRawLogOnLogViewer.Checked = ChkShowRawLog.Checked
    End Sub
End Class