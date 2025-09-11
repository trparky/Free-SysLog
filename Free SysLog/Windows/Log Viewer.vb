Imports Windows.UI.Xaml.Controls.Maps

Public Class LogViewer
    Public strLogText, strRawLogText As String
    Public alertType As AlertType = AlertType.None
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

    Private Sub HideTheImageBox()
        TableLayoutPanel1.Controls.Remove(lblAlertText)
        TableLayoutPanel1.Controls.Add(lblAlertText, 0, 1)

        TableLayoutPanel1.Controls.Remove(txtAlertText)
        TableLayoutPanel1.Controls.Add(txtAlertText, 0, 2)

        TableLayoutPanel1.SetColumnSpan(txtAlertText, 2)

        lblAlertType.Visible = False
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
            IconImageBox.Visible = False
            TableLayoutPanel1.SetRowSpan(LogText, 3)
        Else
            AdjustScrollBars(txtAlertText)

            If alertType = AlertType.None Then
                HideTheImageBox()
            Else
                Dim strIconPath As String = Nothing

                If alertType = ToolTipIcon.Error Then
                    lblAlertType.Text = "Alert Type: Error"
                    strIconPath = IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "error.png")
                    ToolTip1.SetToolTip(IconImageBox, "Error alert type is used for critical issues." & vbCrLf & "It indicates a serious problem that needs immediate attention.")
                ElseIf alertType = ToolTipIcon.Warning Then
                    lblAlertType.Text = "Alert Type: Warning"
                    strIconPath = IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "warning.png")
                    ToolTip1.SetToolTip(IconImageBox, "Warning alert type is used for non-critical issues." & vbCrLf & "It indicates a potential problem that may need attention.")
                ElseIf alertType = ToolTipIcon.Info Then
                    lblAlertType.Text = "Alert Type: Information"
                    strIconPath = IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "info.png")
                    ToolTip1.SetToolTip(IconImageBox, "Information alert type is used for general information alerts." & vbCrLf & "It does not indicate any problem.")
                End If

                If Not String.IsNullOrWhiteSpace(strIconPath) AndAlso IO.File.Exists(strIconPath) Then
                    IconImageBox.Image = Image.FromFile(strIconPath)
                Else
                    HideTheImageBox()
                End If
            End If
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