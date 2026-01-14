Imports System.ComponentModel
Imports Free_SysLog.SupportCode

Partial Class Form1
    Public Sub SelectLatestLogEntry()
        If ChkEnableAutoScroll.Checked AndAlso Logs.Rows.Count > 0 AndAlso intSortColumnIndex = 0 Then
            boolIsProgrammaticScroll = True
            Logs.BeginInvoke(Sub()
                                 Logs.FirstDisplayedScrollingRowIndex = If(sortOrder = SortOrder.Ascending, Logs.Rows.Count - 1, 0)
                             End Sub)
        End If
    End Sub

    Public Sub RestoreWindow()
        If boolMaximizedBeforeMinimize Then
            WindowState = FormWindowState.Maximized
        ElseIf My.Settings.boolMaximized Then
            WindowState = FormWindowState.Maximized
        Else
            WindowState = FormWindowState.Normal
        End If

        BringToFront()
        Activate()

        SelectLatestLogEntry()
    End Sub

    Private Function FormatSecondsToReadableTime(input As Integer) As String
        If input < 0 Then Return "0 seconds"

        Dim minutes As Integer = input \ 60
        Dim seconds As Integer = input Mod 60

        Dim parts As New List(Of String)

        If minutes > 0 Then
            parts.Add($"{minutes} minute{If(minutes > 1, "s", "")}")
        End If

        If seconds > 0 OrElse minutes = 0 Then
            parts.Add($"{seconds} second{If(seconds <> 1, "s", "")}")
        End If

        Return String.Join(" and ", parts)
    End Function

    Public Sub UpdateLogCount()
        BtnClearLog.Enabled = Logs.Rows.Count <> 0
        NumberOfLogs.Text = $"Number of Log Entries: {Logs.Rows.Count:N0}"
    End Sub

    Public Sub SaveLogsToDiskSub()
        WriteLogsToDisk()
        LblAutoSaved.Text = $"Last Saved At: {Date.Now:h:mm:ss tt}"
        SaveTimer.Enabled = False
        SaveTimer.Enabled = True
    End Sub

    Private Sub SortLogsByDateObject(columnIndex As Integer, order As ListSortDirection)
        SyncLock dataGridLockObject
            SortLogsByDateObjectNoLocking(columnIndex, order)
        End SyncLock
    End Sub

    Public Sub SortLogsByDateObjectNoLocking(columnIndex As Integer, order As ListSortDirection)
        Logs.SuspendLayout()
        Logs.Sort(Logs.Columns(columnIndex), order)
        Logs.ResumeLayout()
    End Sub

    Public Sub ShowSingleInstanceWindow(Of T As {Form, New})(ByRef instance As T, ownerIcon As Icon)
        If instance IsNot Nothing AndAlso Not instance.IsDisposed Then
            instance.WindowState = FormWindowState.Normal
            instance.BringToFront()
            instance.Activate()
        Else
            instance = New T() With {.Icon = ownerIcon}
            instance.Show()
        End If
    End Sub

    Public Sub ShowSingleInstanceWindow(Of T As Form)(ByRef instance As T, createForm As Func(Of T))
        If instance IsNot Nothing AndAlso Not instance.IsDisposed Then
            instance.WindowState = FormWindowState.Normal
            instance.BringToFront()
            instance.Activate()
        Else
            instance = createForm()
            instance.Show()
        End If
    End Sub

    Private Sub OpenLogViewerWindow(strLogText As String, strAlertText As String, strLogDate As String, strSourceIP As String, strRawLogText As String, alertType As AlertType)
        strRawLogText = strRawLogText.Replace("{newline}", vbCrLf, StringComparison.OrdinalIgnoreCase)

        Using LogViewerInstance As New LogViewer With {.strRawLogText = strRawLogText, .strLogText = strLogText, .StartPosition = FormStartPosition.CenterParent, .Icon = Icon, .alertType = alertType}
            LogViewerInstance.LblLogDate.Text = $"Log Date: {strLogDate}"
            LogViewerInstance.LblSource.Text = $"Source IP Address: {strSourceIP}"
            LogViewerInstance.TopMost = True
            LogViewerInstance.txtAlertText.Text = strAlertText

            LogViewerInstance.ShowDialog()
        End Using
    End Sub

    Private Sub OpenLogViewerWindow()
        If Logs.Rows.Count > 0 And Logs.SelectedCells.Count > 0 Then
            Dim selectedRow As MyDataGridViewRow = Logs.Rows(Logs.SelectedCells(0).RowIndex)
            Dim strLogText As String = selectedRow.Cells(ColumnIndex_LogText).Value
            Dim strRawLogText As String = If(String.IsNullOrWhiteSpace(selectedRow.RawLogData), selectedRow.Cells(ColumnIndex_LogText).Value, selectedRow.RawLogData.Replace("{newline}", vbCrLf, StringComparison.OrdinalIgnoreCase))

            Using LogViewerInstance As New LogViewer With {.strRawLogText = strRawLogText, .strLogText = strLogText, .StartPosition = FormStartPosition.CenterParent, .Icon = Icon, .MyParentForm = Me, .alertType = selectedRow.alertType}
                LogViewerInstance.LblLogDate.Text = $"Log Date: {selectedRow.Cells(ColumnIndex_ComputedTime).Value}"
                LogViewerInstance.LblSource.Text = $"Source IP Address: {selectedRow.Cells(ColumnIndex_IPAddress).Value}"

                If Not String.IsNullOrEmpty(selectedRow.AlertText) Then
                    LogViewerInstance.txtAlertText.Text = selectedRow.AlertText
                End If

                LogViewerInstance.ShowDialog(Me)
            End Using
        End If
    End Sub
End Class