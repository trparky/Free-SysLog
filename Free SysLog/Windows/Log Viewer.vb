Public Class LogViewer
    Public strLogText, strRawLogText As String
    Public parentForm As Form1

    Private Sub Log_Viewer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ChkShowRawLog.Checked = My.Settings.boolShowRawLogOnLogViewer
        Size = My.Settings.logViewerWindowSize
        LogText.Text = If(ChkShowRawLog.Checked, strRawLogText, strLogText)
        LogText.Select(0, 0)
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
        LogText.Text = If(ChkShowRawLog.Checked, strRawLogText, strLogText)
        If parentForm IsNot Nothing Then parentForm.ShowRawLogOnLogViewer.Checked = ChkShowRawLog.Checked
    End Sub
End Class