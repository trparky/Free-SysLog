Public Class LogViewer
    Public strLogText, strRawLogText As String
    Public MyParentForm As Form1

    Private Sub AdjustScrollBars(ByRef textBoxControl As TextBox)
        ' Get the height of the TextBox's visible area
        Dim visibleHeight As Integer = textBoxControl.ClientSize.Height

        ' Check how much space is needed to display the full text
        Dim requiredHeight As Integer = 0

        ' Create a Graphics object to measure the height of the text content
        Using g As Graphics = textBoxControl.CreateGraphics()
            ' Use TextRenderer to measure the required height of the text content
            requiredHeight = TextRenderer.MeasureText(g, textBoxControl.Text, textBoxControl.Font, New Size(textBoxControl.ClientSize.Width, Integer.MaxValue), TextFormatFlags.WordBreak).Height
        End Using

        ' If the text exceeds the visible area, enable the scrollbar
        If requiredHeight > visibleHeight Then
            textBoxControl.ScrollBars = ScrollBars.Vertical
        Else
            textBoxControl.ScrollBars = ScrollBars.None
        End If
    End Sub

    Private Sub Log_Viewer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If My.Settings.font IsNot Nothing Then
            LogText.Font = My.Settings.font
            txtAlertText.Font = My.Settings.font
        End If

        ChkShowRawLog.Checked = My.Settings.boolShowRawLogOnLogViewer
        Size = My.Settings.logViewerWindowSize
        LogText.Text = If(ChkShowRawLog.Checked, strRawLogText, strLogText)
        LogText.Select(0, 0)

        If String.IsNullOrWhiteSpace(txtAlertText.Text) Then
            txtAlertText.Visible = False
            lblAlertText.Visible = False
            TableLayoutPanel1.SetRowSpan(LogText, 3)
        Else
            AdjustScrollBars(txtAlertText)
        End If

        AdjustScrollBars(LogText)
        NativeMethod.NativeMethods.SetForegroundWindow(Handle)
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

    Private Sub LogText_SizeChanged(sender As Object, e As EventArgs) Handles LogText.SizeChanged
        AdjustScrollBars(LogText)
    End Sub

    Private Sub txtAlertText_SizeChanged(sender As Object, e As EventArgs) Handles txtAlertText.SizeChanged
        AdjustScrollBars(txtAlertText)
    End Sub
End Class