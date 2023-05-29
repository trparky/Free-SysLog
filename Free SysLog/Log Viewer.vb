Public Class Log_Viewer
    Public strLogText As String

    Private Sub Log_Viewer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Size = My.Settings.logViewerWindowSize
        LogText.Text = strLogText
        LogText.Select(0, 0)
    End Sub

    Private Sub Log_Viewer_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        My.Settings.logViewerWindowSize = Size
    End Sub
End Class