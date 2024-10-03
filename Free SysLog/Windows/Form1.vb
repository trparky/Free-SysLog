﻿Imports System.IO
Imports System.Net.Sockets
Imports System.Net
Imports System.Text
Imports System.ComponentModel
Imports Microsoft.Win32
Imports System.Text.RegularExpressions
Imports System.Reflection
Imports System.Configuration
Imports Microsoft.Win32.TaskScheduler
Imports Free_SysLog.SupportCode

Public Class Form1
    Private boolMaximizedBeforeMinimize As Boolean
    Private boolDoneLoading As Boolean = False
    Public lockObject As New Object
    Public longNumberOfIgnoredLogs As Long = 0
    Public IgnoredLogs As New List(Of MyDataGridViewRow)
    Public regexCache As New Dictionary(Of String, Regex)
    Public intSortColumnIndex As Integer = 0 ' Define intColumnNumber at class level
    Public sortOrder As SortOrder = SortOrder.Ascending ' Define soSortOrder at class level
    Public ReadOnly dataGridLockObject As New Object
    Public ReadOnly IgnoredLogsLockObject As New Object
    Private Const strPayPal As String = "https://paypal.me/trparky"
    Private serverThread As Threading.Thread
    Private SyslogTcpServer As SyslogTcpServer.SyslogTcpServer
    Private boolServerRunning As Boolean = False

#Region "--== Midnight Timer Code ==--"
    ' This implementation is based on code found at https://www.codeproject.com/Articles/18201/Midnight-Timer-A-Way-to-Detect-When-it-is-Midnight.
    ' I have rewritten the code to ensure that I fully understand it and to avoid blatantly copying someone else's work.
    ' Using their code as-is without making an effort to learn from it or to create my own implementation doesn't sit well with me.

    Private MyMidnightTimer As Timers.Timer

    Private Sub CreateNewMidnightTimer()
        If MyMidnightTimer IsNot Nothing Then
            MyMidnightTimer.Stop()
            MyMidnightTimer.Dispose()
            MyMidnightTimer = Nothing
        End If

        ' Calculate the time span until midnight
        Dim ts As TimeSpan = GetMidnight(1).Subtract(Date.Now)
        Dim tsMidnight As New TimeSpan(ts.Hours, ts.Minutes, ts.Seconds)

        ' Create and start the new timer
        MyMidnightTimer = New Timers.Timer(tsMidnight.TotalMilliseconds)

        AddHandler MyMidnightTimer.Elapsed, AddressOf MidnightEvent
        AddHandler SystemEvents.TimeChanged, AddressOf WindowsTimeChangeHandler

        MyMidnightTimer.Start()
    End Sub

    Private Sub MidnightEvent(sender As Object, e As Timers.ElapsedEventArgs)
        If Logs.InvokeRequired Then
            Logs.Invoke(New Action(Of Object, Timers.ElapsedEventArgs)(AddressOf MidnightEvent), sender, e)
            Return
        End If

        SyncLock dataGridLockObject
            If My.Settings.BackupOldLogsAfterClearingAtMidnight Then MakeLogBackup()

            Dim oldLogCount As Integer = Logs.Rows.Count
            Logs.Rows.Clear()

            Logs.Rows.Add(SyslogParser.MakeDataGridRow(Now, Now, Now.ToString, IPAddress.Loopback.ToString, Nothing, Nothing, $"The program deleted {oldLogCount:N0} log {If(oldLogCount = 1, "entry", "entries")} at midnight.", "Informational", False, Nothing, Logs))

            SelectLatestLogEntry()

            NumberOfLogs.Text = $"Number of Log Entries: {Logs.Rows.Count:N0}"
        End SyncLock

        CreateNewMidnightTimer()
    End Sub

    Private Sub WindowsTimeChangeHandler(sender As Object, e As EventArgs)
        CreateNewMidnightTimer()
    End Sub

    Private Function GetMidnight(minutesAfterMidnight As Integer) As Date
        Dim tomorrow As Date = Date.Now.AddDays(1)
        Return New Date(tomorrow.Year, tomorrow.Month, tomorrow.Day, 0, minutesAfterMidnight, 0)
    End Function

    Private Sub BackupOldLogsAfterClearingAtMidnight_Click(sender As Object, e As EventArgs) Handles BackupOldLogsAfterClearingAtMidnight.Click
        My.Settings.DeleteOldLogsAtMidnight = BackupOldLogsAfterClearingAtMidnight.Checked
    End Sub

    Private Sub DeleteOldLogsAtMidnight_Click(sender As Object, e As EventArgs) Handles DeleteOldLogsAtMidnight.Click
        My.Settings.DeleteOldLogsAtMidnight = DeleteOldLogsAtMidnight.Checked
        BackupOldLogsAfterClearingAtMidnight.Enabled = DeleteOldLogsAtMidnight.Checked

        If Not DeleteOldLogsAtMidnight.Checked Then
            DeleteOldLogsAtMidnight.Checked = False
            My.Settings.DeleteOldLogsAtMidnight = False
        End If

        If DeleteOldLogsAtMidnight.Checked Then
            CreateNewMidnightTimer()
        Else
            If MyMidnightTimer IsNot Nothing Then
                MyMidnightTimer.Stop()
                MyMidnightTimer.Dispose()
                MyMidnightTimer = Nothing
            End If
        End If
    End Sub
#End Region

    Private Function GetDateStringBasedOnUserPreference(dateObject As Date) As String
        Select Case My.Settings.DateFormat
            Case 1
                Return dateObject.ToLongDateString.Replace("/", "-").Replace("\", "-")
            Case 2
                Return dateObject.ToShortDateString.Replace("/", "-").Replace("\", "-")
            Case 3
                Return dateObject.ToString(My.Settings.CustomDateFormat).Replace("/", "-").Replace("\", "-")
            Case Else
                Return dateObject.ToLongDateString().Replace("/", "-").Replace("\", "-")
        End Select
    End Function

    Private Sub MakeLogBackup()
        DataHandling.WriteLogsToDisk()
        File.Copy(strPathToDataFile, GetUniqueFileName(Path.Combine(strPathToDataBackupFolder, $"{GetDateStringBasedOnUserPreference(Now.AddDays(-1))} Backup.json")))
    End Sub

    Private Sub ChkStartAtUserStartup_Click(sender As Object, e As EventArgs) Handles ChkEnableStartAtUserStartup.Click
        If ChkEnableStartAtUserStartup.Checked Then
            TaskHandling.CreateTask()
            StartUpDelay.Enabled = True
            StartUpDelay.Text = "        Startup Delay (60 Seconds)"
        Else
            Using taskService As New TaskService
                taskService.RootFolder.DeleteTask($"Free SysLog for {Environment.UserName}")
            End Using

            StartUpDelay.Enabled = False
        End If
    End Sub

    Public Sub SelectLatestLogEntry()
        If ChkEnableAutoScroll.Checked AndAlso Logs.Rows.Count > 0 AndAlso intSortColumnIndex = 0 Then
            Logs.FirstDisplayedScrollingRowIndex = If(sortOrder = SortOrder.Ascending, Logs.Rows.Count - 1, 0)
        End If
    End Sub

    Private Sub Form1_ResizeEnd(sender As Object, e As EventArgs) Handles Me.ResizeEnd
        My.Settings.mainWindowSize = Size
        Threading.Thread.Sleep(100)
        SelectLatestLogEntry()
    End Sub

    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        If boolDoneLoading Then
            If WindowState = FormWindowState.Minimized Then
                If My.Settings.boolDeselectItemsWhenMinimizing Then
                    Logs.ClearSelection()
                    LblItemsSelected.Visible = False
                End If

                boolMaximizedBeforeMinimize = WindowState = FormWindowState.Maximized
            Else
                My.Settings.boolMaximized = WindowState = FormWindowState.Maximized
            End If

            If IgnoredLogsAndSearchResultsInstance IsNot Nothing Then IgnoredLogsAndSearchResultsInstance.BtnViewMainWindow.Enabled = WindowState = FormWindowState.Minimized
            If MinimizeToClockTray.Checked Then ShowInTaskbar = WindowState <> FormWindowState.Minimized

            For Each item As MyDataGridViewRow In Logs.Rows
                item.Height = GetMinimumHeight(item.Cells(ColumnIndex_IPAddress).Value, Logs.DefaultCellStyle.Font, ColLog.Width)
            Next

            Logs.Invalidate()
            Logs.Refresh()
        End If
    End Sub

    Private Sub ChkAutoSave_Click(sender As Object, e As EventArgs) Handles ChkEnableAutoSave.Click
        SaveTimer.Enabled = ChkEnableAutoSave.Checked
        ChangeLogAutosaveIntervalToolStripMenuItem.Visible = ChkEnableAutoSave.Checked
        LblAutoSaved.Visible = ChkEnableAutoSave.Checked
    End Sub

    Private Sub SaveTimer_Tick(sender As Object, e As EventArgs) Handles SaveTimer.Tick
        DataHandling.WriteLogsToDisk()
        LblAutoSaved.Text = $"Last Auto-Saved At: {Date.Now:h:mm:ss tt}"
    End Sub

    Public Sub RestoreWindow()
        If boolMaximizedBeforeMinimize Then
            WindowState = FormWindowState.Maximized
        ElseIf My.Settings.boolMaximized Then
            WindowState = FormWindowState.Maximized
        Else
            WindowState = FormWindowState.Normal
        End If

        TopMost = True
        BringToFront()
        TopMost = False

        SelectLatestLogEntry()
    End Sub

    Private Sub NotifyIcon_DoubleClick(sender As Object, e As EventArgs) Handles NotifyIcon.DoubleClick
        RestoreWindow()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SyslogParser.SetParentForm = Me
        DataHandling.SetParentForm = Me
        TaskHandling.SetParentForm = Me

        TaskHandling.ConvertRegistryRunCommandToTask()

        If My.Settings.boolCheckForUpdates Then Threading.ThreadPool.QueueUserWorkItem(Sub()
                                                                                           Dim checkForUpdatesClassObject As New checkForUpdates.CheckForUpdatesClass(Me)
                                                                                           checkForUpdatesClassObject.CheckForUpdates(False)
                                                                                       End Sub)
        If My.Settings.DeleteOldLogsAtMidnight Then CreateNewMidnightTimer()

        ChangeLogAutosaveIntervalToolStripMenuItem.Text = $"        Change Log Autosave Interval ({My.Settings.autoSaveMinutes} Minutes)"
        ChangeSyslogServerPortToolStripMenuItem.Text = $"Change Syslog Server Port (Port Number {My.Settings.sysLogPort})"

        ColTime.HeaderCell.Style.Padding = New Padding(0, 0, 1, 0)
        ColIPAddress.HeaderCell.Style.Padding = New Padding(0, 0, 2, 0)

        ColTime.HeaderCell.SortGlyphDirection = SortOrder.Ascending

        AutomaticallyCheckForUpdates.Checked = My.Settings.boolCheckForUpdates
        ChkDeselectItemAfterMinimizingWindow.Checked = My.Settings.boolDeselectItemsWhenMinimizing
        ChkEnableRecordingOfIgnoredLogs.Checked = My.Settings.recordIgnoredLogs
        IgnoredLogsToolStripMenuItem.Visible = ChkEnableRecordingOfIgnoredLogs.Checked
        ZerooutIgnoredLogsCounterToolStripMenuItem.Visible = Not ChkEnableRecordingOfIgnoredLogs.Checked
        ChkEnableAutoScroll.Checked = My.Settings.autoScroll
        ChkEnableAutoSave.Checked = My.Settings.autoSave
        ChkEnableConfirmCloseToolStripItem.Checked = My.Settings.boolConfirmClose
        LblAutoSaved.Visible = ChkEnableAutoSave.Checked
        ColAlerts.Visible = My.Settings.boolShowAlertedColumn
        ChkShowAlertedColumn.Checked = My.Settings.boolShowAlertedColumn
        MinimizeToClockTray.Checked = My.Settings.MinimizeToClockTray
        StopServerStripMenuItem.Visible = boolDoWeOwnTheMutex
        ChkEnableStartAtUserStartup.Checked = TaskHandling.DoesTaskExist()
        DeleteOldLogsAtMidnight.Checked = My.Settings.DeleteOldLogsAtMidnight
        BackupOldLogsAfterClearingAtMidnight.Enabled = My.Settings.DeleteOldLogsAtMidnight
        BackupOldLogsAfterClearingAtMidnight.Checked = My.Settings.BackupOldLogsAfterClearingAtMidnight
        ViewLogBackups.Visible = BackupOldLogsAfterClearingAtMidnight.Checked
        ChkEnableTCPSyslogServer.Checked = My.Settings.EnableTCPServer
        Icon = Icon.ExtractAssociatedIcon(strEXEPath)
        Location = VerifyWindowLocation(My.Settings.windowLocation, Me)
        If My.Settings.boolMaximized Then WindowState = FormWindowState.Maximized
        NotifyIcon.Icon = Icon
        NotifyIcon.Text = "Free SysLog"
        ColHostname.Visible = My.Settings.boolShowHostnameColumn
        ChkShowHostnameColumn.Checked = My.Settings.boolShowHostnameColumn
        colServerTime.Visible = My.Settings.boolShowServerTimeColumn
        ChkShowServerTimeColumn.Checked = My.Settings.boolShowServerTimeColumn
        colLogType.Visible = My.Settings.boolShowLogTypeColumn
        ChkShowLogTypeColumn.Checked = My.Settings.boolShowLogTypeColumn
        RemoveNumbersFromRemoteApp.Checked = My.Settings.RemoveNumbersFromRemoteApp
        IPv6Support.Checked = My.Settings.IPv6Support

        Dim flags As BindingFlags = BindingFlags.NonPublic Or BindingFlags.Instance Or BindingFlags.SetProperty
        Dim propInfo As PropertyInfo = GetType(DataGridView).GetProperty("DoubleBuffered", flags)
        propInfo?.SetValue(Logs, True, Nothing)

        Logs.AlternatingRowsDefaultCellStyle = New DataGridViewCellStyle() With {.BackColor = My.Settings.searchColor}
        Logs.DefaultCellStyle = New DataGridViewCellStyle() With {.WrapMode = DataGridViewTriState.True}
        ColLog.DefaultCellStyle = New DataGridViewCellStyle() With {.WrapMode = DataGridViewTriState.True}

        Dim tempReplacementsClass As ReplacementsClass
        Dim tempSysLogProxyServer As SysLogProxyServer
        Dim tempIgnoredClass As IgnoredClass
        Dim tempAlertsClass As AlertsClass

        If My.Settings.replacements IsNot Nothing AndAlso My.Settings.replacements.Count > 0 Then
            For Each strJSONString As String In My.Settings.replacements
                tempReplacementsClass = Newtonsoft.Json.JsonConvert.DeserializeObject(Of ReplacementsClass)(strJSONString, JSONDecoderSettingsForSettingsFiles)
                If tempReplacementsClass.BoolEnabled Then replacementsList.Add(tempReplacementsClass)
                tempReplacementsClass = Nothing
            Next
        End If

        If My.Settings.ServersToSendTo IsNot Nothing AndAlso My.Settings.ServersToSendTo.Count > 0 Then
            For Each strJSONString As String In My.Settings.ServersToSendTo
                tempSysLogProxyServer = Newtonsoft.Json.JsonConvert.DeserializeObject(Of SysLogProxyServer)(strJSONString, JSONDecoderSettingsForSettingsFiles)
                If tempSysLogProxyServer.boolEnabled Then serversList.Add(tempSysLogProxyServer)
                tempSysLogProxyServer = Nothing
            Next
        End If

        If My.Settings.ignored2 Is Nothing Then
            My.Settings.ignored2 = New Specialized.StringCollection()

            If My.Settings.ignored IsNot Nothing AndAlso My.Settings.ignored.Count > 0 Then
                For Each strIgnoredString As String In My.Settings.ignored
                    My.Settings.ignored2.Add(Newtonsoft.Json.JsonConvert.SerializeObject(New IgnoredClass With {.BoolCaseSensitive = False, .BoolRegex = False, .StrIgnore = strIgnoredString}))
                Next

                My.Settings.ignored.Clear()
                My.Settings.Save()
            End If
        End If

        If My.Settings.ignored2 IsNot Nothing AndAlso My.Settings.ignored2.Count > 0 Then
            For Each strJSONString As String In My.Settings.ignored2
                tempIgnoredClass = Newtonsoft.Json.JsonConvert.DeserializeObject(Of IgnoredClass)(strJSONString, JSONDecoderSettingsForSettingsFiles)
                If tempIgnoredClass.BoolEnabled Then ignoredList.Add(tempIgnoredClass)
                tempIgnoredClass = Nothing
            Next
        End If

        If My.Settings.alerts IsNot Nothing AndAlso My.Settings.alerts.Count > 0 Then
            For Each strJSONString As String In My.Settings.alerts
                tempAlertsClass = Newtonsoft.Json.JsonConvert.DeserializeObject(Of AlertsClass)(strJSONString, JSONDecoderSettingsForSettingsFiles)
                If tempAlertsClass.BoolEnabled Then alertsList.Add(tempAlertsClass)
                tempAlertsClass = Nothing
            Next
        End If

        If My.Settings.autoSave Then
            SaveTimer.Interval = TimeSpan.FromMinutes(My.Settings.autoSaveMinutes).TotalMilliseconds
            SaveTimer.Enabled = True
        End If

        Size = My.Settings.mainWindowSize

        ColTime.Width = My.Settings.columnTimeSize
        colServerTime.Width = My.Settings.ServerTimeWidth
        colLogType.Width = My.Settings.LogTypeWidth
        ColIPAddress.Width = My.Settings.columnIPSize
        ColHostname.Width = My.Settings.HostnameWidth
        ColRemoteProcess.Width = My.Settings.RemoteProcessHeaderSize
        ColLog.Width = My.Settings.columnLogSize

        boolDoneLoading = True

        Dim worker As New BackgroundWorker()
        AddHandler worker.DoWork, AddressOf LoadDataFile
        AddHandler worker.RunWorkerCompleted, AddressOf RunWorkerCompleted
        worker.RunWorkerAsync()
    End Sub

    Private Async Sub StartTCPServer()
        SyslogTcpServer = New SyslogTcpServer.SyslogTcpServer(Sub(strReceivedData As String, strSourceIP As String) SyslogParser.ProcessIncomingLog(strReceivedData, strSourceIP), My.Settings.sysLogPort)
        Await SyslogTcpServer.StartAsync()
    End Sub

    Private Sub RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs)
        serverThread = New Threading.Thread(AddressOf SysLogThread) With {.Name = "UDP Server Thread", .Priority = Threading.ThreadPriority.Normal}
        serverThread.Start()

        If My.Settings.EnableTCPServer Then StartTCPServer()

        boolServerRunning = True
    End Sub

    Private Sub LoadDataFile(sender As Object, e As DoWorkEventArgs)
        If File.Exists(strPathToDataFile) Then
            Try
                Invoke(Sub()
                           Logs.Rows.Add(SyslogParser.MakeDataGridRow(Now, Now, Nothing, Nothing, Nothing, Nothing, "Loading data and populating data grid... Please Wait.", "Informational, Local", False, Nothing, Logs))
                           LblLogFileSize.Text = $"Log File Size: {FileSizeToHumanSize(New FileInfo(strPathToDataFile).Length)}"
                       End Sub)

                Dim collectionOfSavedData As New List(Of SavedData)

                Using fileStream As New StreamReader(strPathToDataFile)
                    collectionOfSavedData = Newtonsoft.Json.JsonConvert.DeserializeObject(Of List(Of SavedData))(fileStream.ReadToEnd.Trim, JSONDecoderSettingsForLogFiles)
                End Using

                Dim listOfLogEntries As New List(Of MyDataGridViewRow)
                Dim stopwatch As Stopwatch = Stopwatch.StartNew

                If collectionOfSavedData.Count > 0 Then
                    Dim intProgress As Integer = 0

                    Invoke(Sub() LoadingProgressBar.Visible = True)

                    For Each item As SavedData In collectionOfSavedData
                        listOfLogEntries.Add(item.MakeDataGridRow(Logs, GetMinimumHeight(item.log, Logs.DefaultCellStyle.Font, ColLog.Width)))
                        intProgress += 1
                        Invoke(Sub() LoadingProgressBar.Value = intProgress / collectionOfSavedData.Count * 100)
                    Next

                    Invoke(Sub() LoadingProgressBar.Visible = False)
                End If

                listOfLogEntries.Add(SyslogParser.MakeDataGridRow(Now, Now, Now.ToString, IPAddress.Loopback.ToString, Nothing, Nothing, $"Free SysLog Server Started. Data loaded in {MyRoundingFunction(stopwatch.Elapsed.TotalMilliseconds / 1000, 2)} seconds.", "Informational, Local", False, Nothing, Logs))

                SyncLock dataGridLockObject
                    Invoke(Sub()
                               Logs.Rows.Clear()
                               Logs.Rows.AddRange(listOfLogEntries.ToArray)

                               SelectLatestLogEntry()

                               Logs.SelectedRows(0).Selected = False
                               UpdateLogCount()
                           End Sub)
                End SyncLock
            Catch ex As Newtonsoft.Json.JsonSerializationException
                HandleLogFileLoadException(ex)
            Catch ex As Newtonsoft.Json.JsonReaderException
                HandleLogFileLoadException(ex)
            End Try
        End If
    End Sub

    Private Sub HandleLogFileLoadException(ex As Exception)
        If File.Exists($"{strPathToDataFile}.bad") Then
            File.Copy(strPathToDataFile, GetUniqueFileName($"{strPathToDataFile}.bad"))
        Else
            File.Copy(strPathToDataFile, $"{strPathToDataFile}.bad")
        End If

        File.WriteAllText(strPathToDataFile, "[]")
        LblLogFileSize.Text = $"Log File Size: {FileSizeToHumanSize(New FileInfo(strPathToDataFile).Length)}"

        SyncLock dataGridLockObject
            Invoke(Sub()
                       Logs.Rows.Clear()

                       Dim listOfLogEntries As New List(Of MyDataGridViewRow) From {
                           SyslogParser.MakeDataGridRow(Now, Now, Now.ToString, IPAddress.Loopback.ToString, Nothing, Nothing, "Free SysLog Server Started.", "Informational, Local", False, Nothing, Logs),
                           SyslogParser.MakeDataGridRow(Now, Now, Now.ToString, IPAddress.Loopback.ToString, Nothing, Nothing, "There was an error while decoing the JSON data, existing data was copied to another file and the log file was reset.", "Informational, Local", False, Nothing, Logs),
                           SyslogParser.MakeDataGridRow(Now, Now, Now.ToString, IPAddress.Loopback.ToString, Nothing, Nothing, $"Exception Type: {ex.GetType}{vbCrLf}Exception Message: {ex.Message}{vbCrLf}{vbCrLf}Exception Stack Trace{vbCrLf}{ex.StackTrace}", "Informational, Local", False, Nothing, Logs)
                       }

                       Logs.Rows.AddRange(listOfLogEntries.ToArray)
                       UpdateLogCount()
                   End Sub)
        End SyncLock
    End Sub

    Private Function GetUniqueFileName(fileName As String) As String
        Dim fileInfo As New FileInfo(fileName)

        Dim strDirectory As String = fileInfo.DirectoryName
        Dim strFileBase As String = Path.GetFileNameWithoutExtension(fileInfo.Name)
        Dim strFileExtension As String = fileInfo.Extension

        If String.IsNullOrWhiteSpace(strDirectory) Then strDirectory = Directory.GetCurrentDirectory

        Dim strNewFileName As String = Path.Combine(strDirectory, fileInfo.Name)
        Dim intCount As Integer = 1

        While File.Exists(strNewFileName)
            strNewFileName = Path.Combine(strDirectory, $"{strFileBase} ({intCount}){strFileExtension}")
            intCount += 1
        End While

        Return strNewFileName
    End Function

    Private Sub BtnOpenLogLocation_Click(sender As Object, e As EventArgs) Handles BtnOpenLogLocation.Click
        SelectFileInWindowsExplorer(strPathToDataFile)
    End Sub

    Private Sub OpenLogViewerWindow()
        If Logs.Rows.Count > 0 And Logs.SelectedCells.Count > 0 Then
            Dim selectedRow As MyDataGridViewRow = Logs.Rows(Logs.SelectedCells(0).RowIndex)
            Dim strLogText As String = If(String.IsNullOrWhiteSpace(selectedRow.RawLogData), selectedRow.Cells(ColumnIndex_LogText).Value, selectedRow.RawLogData)

            Using LogViewerInstance As New LogViewer With {.strLogText = strLogText, .StartPosition = FormStartPosition.CenterParent, .Icon = Icon}
                LogViewerInstance.LblLogDate.Text = $"Log Date: {selectedRow.Cells(ColumnIndex_ComputedTime).Value}"
                LogViewerInstance.LblSource.Text = $"Source IP Address: {selectedRow.Cells(ColumnIndex_IPAddress).Value}"
                LogViewerInstance.ShowDialog(Me)
            End Using
        End If
    End Sub

    Private Sub Logs_DoubleClick(sender As Object, e As EventArgs) Handles Logs.DoubleClick
        OpenLogViewerWindow()
    End Sub

    Private Sub Logs_KeyUp(sender As Object, e As KeyEventArgs) Handles Logs.KeyUp
        If e.KeyValue = Keys.Enter Then
            OpenLogViewerWindow()
        ElseIf e.KeyValue = Keys.Delete Then
            SyncLock dataGridLockObject
                Dim intNumberOfLogsDeleted As Integer = Logs.SelectedRows.Count
                Dim choice As Confirm_Delete.UserChoice

                Using Confirm_Delete As New Confirm_Delete(intNumberOfLogsDeleted) With {.Icon = Icon, .StartPosition = FormStartPosition.CenterParent}
                    Confirm_Delete.lblMainLabel.Text = $"Are you sure you want to delete the {intNumberOfLogsDeleted} selected {If(intNumberOfLogsDeleted = 1, "log", "logs")}?"
                    Confirm_Delete.ShowDialog(Me)
                    choice = Confirm_Delete.choice
                End Using

                If choice = Confirm_Delete.UserChoice.NoDelete Then
                    MsgBox("Logs not deleted.", MsgBoxStyle.Information, Text)
                    Exit Sub
                ElseIf choice = Confirm_Delete.UserChoice.YesDeleteYesBackup Then
                    MakeLogBackup()
                End If

                For Each item As DataGridViewRow In Logs.SelectedRows
                    Logs.Rows.Remove(item)
                Next

                Logs.Rows.Add(SyslogParser.MakeDataGridRow(Now, Now, Now.ToString, IPAddress.Loopback.ToString, Nothing, Nothing, $"The user deleted {intNumberOfLogsDeleted:N0} log {If(intNumberOfLogsDeleted = 1, "entry", "entries")}.", "Informational, Local", False, Nothing, Logs))

                SelectLatestLogEntry()
            End SyncLock

            UpdateLogCount()
            SaveLogsToDiskSub()
        End If
    End Sub

    Private Sub Logs_KeyDown(sender As Object, e As KeyEventArgs) Handles Logs.KeyDown
        If e.KeyCode = Keys.Enter Then e.Handled = True
    End Sub

    Private Sub Logs_UserDeletingRow(sender As Object, e As DataGridViewRowCancelEventArgs) Handles Logs.UserDeletingRow
        e.Cancel = True
    End Sub

    Public Sub UpdateLogCount()
        BtnClearLog.Enabled = Logs.Rows.Count <> 0
        NumberOfLogs.Text = $"Number of Log Entries: {Logs.Rows.Count:N0}"
    End Sub

    Private Sub ChkAutoScroll_Click(sender As Object, e As EventArgs) Handles ChkEnableAutoScroll.Click
        My.Settings.autoScroll = ChkEnableAutoScroll.Checked
    End Sub

    Private Sub Logs_ColumnWidthChanged(sender As Object, e As DataGridViewColumnEventArgs) Handles Logs.ColumnWidthChanged
        If boolDoneLoading Then
            My.Settings.columnTimeSize = ColTime.Width
            My.Settings.ServerTimeWidth = colServerTime.Width
            My.Settings.LogTypeWidth = colLogType.Width
            My.Settings.columnIPSize = ColIPAddress.Width
            My.Settings.HostnameWidth = ColHostname.Width
            My.Settings.RemoteProcessHeaderSize = ColRemoteProcess.Width
            My.Settings.columnLogSize = ColLog.Width
        End If
    End Sub

    Private Sub BtnClearAllLogs_Click(sender As Object, e As EventArgs) Handles BtnClearAllLogs.Click
        If MsgBox("Are you sure you want to clear the logs?", MsgBoxStyle.Question + MsgBoxStyle.YesNo + vbDefaultButton2, Text) = MsgBoxResult.Yes Then
            SyncLock dataGridLockObject
                If MsgBox("Do you want to make a backup of the logs before deleting them?", MsgBoxStyle.Question + MsgBoxStyle.YesNo + vbDefaultButton2, Text) = MsgBoxResult.Yes Then MakeLogBackup()

                Dim intOldCount As Integer = Logs.Rows.Count
                Logs.Rows.Clear()
                Logs.Rows.Add(SyslogParser.MakeDataGridRow(Now, Now, Now.ToString, IPAddress.Loopback.ToString, Nothing, Nothing, $"The user deleted {intOldCount:N0} log {If(intOldCount = 1, "entry", "entries")}.", "Informational, Local", False, Nothing, Logs))

                SelectLatestLogEntry()
            End SyncLock

            UpdateLogCount()
            SaveLogsToDiskSub()
        End If
    End Sub

    Private Sub SaveLogsToDiskSub()
        DataHandling.WriteLogsToDisk()
        LblAutoSaved.Text = $"Last Saved At: {Date.Now:h:mm:ss tt}"
        SaveTimer.Enabled = False
        SaveTimer.Enabled = True
    End Sub

    Private Sub BtnSaveLogsToDisk_Click(sender As Object, e As EventArgs) Handles BtnSaveLogsToDisk.Click
        SaveLogsToDiskSub()
    End Sub

    Private Sub BtnCheckForUpdates_Click(sender As Object, e As EventArgs) Handles BtnCheckForUpdates.Click
        SaveLogsToDiskSub()

        Threading.ThreadPool.QueueUserWorkItem(Sub()
                                                   Dim checkForUpdatesClassObject As New checkForUpdates.CheckForUpdatesClass(Me)
                                                   checkForUpdatesClassObject.CheckForUpdates()
                                               End Sub)
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If e.CloseReason = CloseReason.UserClosing AndAlso My.Settings.boolConfirmClose AndAlso MsgBox("Are you sure you want to close Free SysLog?", MsgBoxStyle.YesNo + MsgBoxStyle.Question + vbDefaultButton2, Text) = MsgBoxResult.No Then
            e.Cancel = True
            Exit Sub
        End If

        My.Settings.Save()
        DataHandling.WriteLogsToDisk()

        If boolDoWeOwnTheMutex Then
            SendMessageToSysLogServer("terminate", My.Settings.sysLogPort)
            If My.Settings.EnableTCPServer Then SendMessageToTCPSysLogServer("terminate", My.Settings.sysLogPort)
        End If

        Try
            mutex.ReleaseMutex()
        Catch ex As ApplicationException
        End Try
    End Sub

    Private Sub BtnSearch_Click(sender As Object, e As EventArgs) Handles BtnSearch.Click
        If String.IsNullOrWhiteSpace(TxtSearchTerms.Text) Then
            MsgBox("You must provide something to search for.", MsgBoxStyle.Critical, Text)
            Exit Sub
        End If

        Dim strLogText As String
        Dim listOfSearchResults As New List(Of MyDataGridViewRow)
        Dim regexCompiledObject As Regex = Nothing
        Dim MyDataGridRowItem As MyDataGridViewRow

        BtnSearch.Enabled = False

        Dim worker As New BackgroundWorker()

        AddHandler worker.DoWork, Sub()
                                      Try
                                          Dim regExOptions As RegexOptions = If(ChkCaseInsensitiveSearch.Checked, RegexOptions.Compiled + RegexOptions.IgnoreCase, RegexOptions.Compiled)

                                          If ChkRegExSearch.Checked Then
                                              regexCompiledObject = New Regex(TxtSearchTerms.Text, regExOptions)
                                          Else
                                              regexCompiledObject = New Regex(Regex.Escape(TxtSearchTerms.Text), regExOptions)
                                          End If

                                          SyncLock dataGridLockObject
                                              For Each item As DataGridViewRow In Logs.Rows
                                                  MyDataGridRowItem = TryCast(item, MyDataGridViewRow)

                                                  If MyDataGridRowItem IsNot Nothing Then
                                                      strLogText = MyDataGridRowItem.Cells(ColumnIndex_LogText).Value

                                                      If Not String.IsNullOrWhiteSpace(strLogText) AndAlso regexCompiledObject.IsMatch(strLogText) Then
                                                          listOfSearchResults.Add(MyDataGridRowItem.Clone())
                                                      End If
                                                  End If
                                              Next
                                          End SyncLock
                                      Catch ex As ArgumentException
                                          MsgBox("Malformed RegEx pattern detected, search aborted.", MsgBoxStyle.Critical, Text)
                                      End Try
                                  End Sub

        AddHandler worker.RunWorkerCompleted, Sub()
                                                  If listOfSearchResults.Count > 0 Then
                                                      Dim searchResultsWindow As New IgnoredLogsAndSearchResults(Me) With {.MainProgramForm = Me, .Icon = Icon, .LogsToBeDisplayed = listOfSearchResults, .Text = "Search Results", .WindowDisplayMode = IgnoreOrSearchWindowDisplayMode.search}
                                                      searchResultsWindow.ShowDialog(Me)
                                                  Else
                                                      MsgBox("Search terms not found.", MsgBoxStyle.Information, Text)
                                                  End If

                                                  Invoke(Sub() BtnSearch.Enabled = True)
                                              End Sub

        worker.RunWorkerAsync()
    End Sub

    Private Sub TxtSearchTerms_KeyDown(sender As Object, e As KeyEventArgs) Handles TxtSearchTerms.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            BtnSearch.PerformClick()
        End If
    End Sub

    Private Sub Logs_ColumnHeaderMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles Logs.ColumnHeaderMouseClick
        ' Disable user sorting
        Logs.AllowUserToOrderColumns = False

        Dim column As DataGridViewColumn = Logs.Columns(e.ColumnIndex)
        intSortColumnIndex = e.ColumnIndex

        If sortOrder = SortOrder.Descending Then
            sortOrder = SortOrder.Ascending
        ElseIf sortOrder = SortOrder.Ascending Then
            sortOrder = SortOrder.Descending
        Else
            sortOrder = SortOrder.Ascending
        End If

        ColAlerts.HeaderCell.SortGlyphDirection = SortOrder.None
        ColIPAddress.HeaderCell.SortGlyphDirection = SortOrder.None
        ColLog.HeaderCell.SortGlyphDirection = SortOrder.None
        ColTime.HeaderCell.SortGlyphDirection = SortOrder.None

        Logs.Columns(e.ColumnIndex).HeaderCell.SortGlyphDirection = sortOrder

        SortLogsByDateObject(column.Index, sortOrder)
    End Sub

    Private Sub SortLogsByDateObject(columnIndex As Integer, order As SortOrder)
        SyncLock dataGridLockObject
            SortLogsByDateObjectNoLocking(columnIndex, order)
        End SyncLock
    End Sub

    Public Sub SortLogsByDateObjectNoLocking(columnIndex As Integer, order As SortOrder)
        Logs.AllowUserToOrderColumns = False
        Logs.Enabled = False

        Dim comparer As New DataGridViewComparer(columnIndex, order)
        Dim rows As MyDataGridViewRow() = Logs.Rows.Cast(Of DataGridViewRow).OfType(Of MyDataGridViewRow)().ToArray()

        Array.Sort(rows, Function(row1 As MyDataGridViewRow, row2 As MyDataGridViewRow) comparer.Compare(row1, row2))

        Logs.Rows.Clear()
        Logs.Rows.AddRange(rows)

        Logs.Enabled = True
        Logs.AllowUserToOrderColumns = True
    End Sub

    Private Sub IgnoredWordsAndPhrasesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConfigureIgnoredWordsAndPhrasesToolStripMenuItem.Click
        Using IgnoredWordsAndPhrasesOrAlertsInstance As New IgnoredWordsAndPhrases With {.Icon = Icon, .StartPosition = FormStartPosition.CenterParent}
            IgnoredWordsAndPhrasesOrAlertsInstance.ShowDialog(Me)
            If IgnoredWordsAndPhrasesOrAlertsInstance.boolChanged Then regexCache.Clear()
        End Using
    End Sub

    Private Sub ViewIgnoredLogsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ViewIgnoredLogsToolStripMenuItem.Click
        If IgnoredLogsAndSearchResultsInstance Is Nothing Then
            IgnoredLogsAndSearchResultsInstance = New IgnoredLogsAndSearchResults(Me) With {.MainProgramForm = Me, .Icon = Icon, .LogsToBeDisplayed = IgnoredLogs, .Text = "Ignored Logs", .WindowDisplayMode = IgnoreOrSearchWindowDisplayMode.ignored}
            IgnoredLogsAndSearchResultsInstance.Show()
        Else
            IgnoredLogsAndSearchResultsInstance.WindowState = FormWindowState.Normal
            IgnoredLogsAndSearchResultsInstance.BringToFront()
        End If
    End Sub

    Private Sub ClearIgnoredLogsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ClearIgnoredLogsToolStripMenuItem.Click
        If MsgBox("Are you sure you want to clear the ignored logs stored in system memory?", MsgBoxStyle.Question + MsgBoxStyle.YesNo + vbDefaultButton2, Text) = MsgBoxResult.Yes Then ClearIgnoredLogs()
    End Sub

    Public Sub ClearIgnoredLogs()
        SyncLock IgnoredLogsLockObject
            IgnoredLogs.Clear()
            longNumberOfIgnoredLogs = 0
            ClearIgnoredLogsToolStripMenuItem.Enabled = False
            LblNumberOfIgnoredIncomingLogs.Text = $"Number of ignored incoming logs: {longNumberOfIgnoredLogs:N0}"
        End SyncLock
    End Sub

    Protected Overrides Sub WndProc(ByRef m As Message)
        Const WM_EXITSIZEMOVE As Integer = &H232

        MyBase.WndProc(m)

        If m.Msg = WM_EXITSIZEMOVE AndAlso boolDoneLoading Then
            Location = VerifyWindowLocation(Location, Me)
            My.Settings.windowLocation = Location
        End If
    End Sub

    Private Sub ChkRecordIgnoredLogs_Click(sender As Object, e As EventArgs) Handles ChkEnableRecordingOfIgnoredLogs.Click
        My.Settings.recordIgnoredLogs = ChkEnableRecordingOfIgnoredLogs.Checked
        IgnoredLogsToolStripMenuItem.Visible = ChkEnableRecordingOfIgnoredLogs.Checked
        ZerooutIgnoredLogsCounterToolStripMenuItem.Visible = Not ChkEnableRecordingOfIgnoredLogs.Checked

        If Not ChkEnableRecordingOfIgnoredLogs.Checked Then
            IgnoredLogs.Clear()
            LblNumberOfIgnoredIncomingLogs.Text = "Number of ignored incoming logs: 0"
        End If
    End Sub

    Private Sub LogsOlderThanToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LogsOlderThanToolStripMenuItem.Click
        Using ClearLogsOlderThanInstance As New ClearLogsOlderThan With {.Icon = Icon, .StartPosition = FormStartPosition.CenterParent}
            ClearLogsOlderThanInstance.LblLogCount.Text = $"Number of Log Entries: {Logs.Rows.Count:N0}"
            ClearLogsOlderThanInstance.ShowDialog(Me)

            If ClearLogsOlderThanInstance.boolSuccess Then
                Try
                    Dim dateChosenDate As Date = ClearLogsOlderThanInstance.dateChosenDate

                    SyncLock dataGridLockObject
                        Logs.AllowUserToOrderColumns = False
                        Logs.Enabled = False

                        Dim intOldCount As Integer = Logs.Rows.Count
                        Dim newListOfLogs As New List(Of MyDataGridViewRow)

                        For Each item As MyDataGridViewRow In Logs.Rows
                            If item.DateObject.Date >= dateChosenDate.Date Then
                                newListOfLogs.Add(item.Clone())
                            End If
                        Next

                        Logs.Enabled = True
                        Logs.AllowUserToOrderColumns = True

                        If MsgBox("Do you want to make a backup of the logs before deleting them?", MsgBoxStyle.Question + MsgBoxStyle.YesNo + vbDefaultButton2, Text) = MsgBoxResult.Yes Then MakeLogBackup()

                        Logs.Rows.Clear()
                        Logs.Rows.AddRange(newListOfLogs.ToArray)

                        Dim intCountDifference As Integer = intOldCount - Logs.Rows.Count
                        Logs.Rows.Add(SyslogParser.MakeDataGridRow(Now, Now, Now.ToString, IPAddress.Loopback.ToString, Nothing, Nothing, $"The user deleted {intCountDifference:N0} log {If(intCountDifference = 1, "entry", "entries")}.", "Informational, Local", False, Nothing, Logs))

                        SelectLatestLogEntry()
                    End SyncLock

                    UpdateLogCount()
                    SaveLogsToDiskSub()
                Catch ex As ArgumentOutOfRangeException
                End Try
            End If
        End Using
    End Sub

    Private Sub ZerooutIgnoredLogsCounterToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ZerooutIgnoredLogsCounterToolStripMenuItem.Click
        longNumberOfIgnoredLogs = 0
        LblNumberOfIgnoredIncomingLogs.Text = $"Number of ignored incoming logs: {longNumberOfIgnoredLogs:N0}"
        ZerooutIgnoredLogsCounterToolStripMenuItem.Enabled = False
    End Sub

    Private Sub ConfigureReplacementsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConfigureReplacementsToolStripMenuItem.Click
        Using ReplacementsInstance As New Replacements With {.Icon = Icon, .StartPosition = FormStartPosition.CenterParent}
            ReplacementsInstance.ShowDialog(Me)
            If ReplacementsInstance.boolChanged Then regexCache.Clear()
        End Using
    End Sub

    Private Sub ConfigureAlternatingColorToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ChangeAlternatingColorToolStripMenuItem.Click
        Using ColorDialog As New ColorDialog()
            If ColorDialog.ShowDialog() = DialogResult.OK Then
                My.Settings.searchColor = ColorDialog.Color

                Dim rowStyle As New DataGridViewCellStyle() With {.BackColor = ColorDialog.Color, .ForeColor = GetGoodTextColorBasedUponBackgroundColor(ColorDialog.Color)}
                Logs.AlternatingRowsDefaultCellStyle = rowStyle
            End If
        End Using
    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
#If DEBUG Then
        MsgBox($"Free SysLog.{vbCrLf}{vbCrLf}Version {checkForUpdates.versionString} (Debug Build){vbCrLf}{vbCrLf}Copyright Thomas Parkison © 2023-2025", MsgBoxStyle.Information, Text)
#Else
        MsgBox($"Free SysLog.{vbCrLf}{vbCrLf}Version {checkForUpdates.versionString}{vbcrlf}{vbcrlf}Copyright Thomas Parkison © 2023-2025", MsgBoxStyle.Information, Text)
#End If
    End Sub

    Private Sub ExportToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExportToolStripMenuItem.Click
        Using SaveFileDialog As New SaveFileDialog()
            SaveFileDialog.Title = "Safe Program Settings..."
            SaveFileDialog.Filter = "JSON File|*.json"

            If SaveFileDialog.ShowDialog() = DialogResult.OK Then
                Try
                    SaveAppSettings.SaveApplicationSettingsToFile(SaveFileDialog.FileName)
                    If MsgBox("Application settings have been saved to disk. Do you want to open Windows Explorer to the location of the file?", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2, Text) = MsgBoxResult.Yes Then SelectFileInWindowsExplorer(SaveFileDialog.FileName)
                Catch ex As Exception
                    MsgBox("There was an issue saving your exported settings to disk, export failed.", MsgBoxStyle.Critical, Text)
                End Try
            End If
        End Using
    End Sub

    Private Sub ImportToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ImportToolStripMenuItem.Click
        Using OpenFileDialog As New OpenFileDialog()
            OpenFileDialog.Title = "Import Program Settings..."
            OpenFileDialog.Filter = "JSON File|*.json"

            If OpenFileDialog.ShowDialog = DialogResult.OK AndAlso SaveAppSettings.LoadApplicationSettingsFromFile(OpenFileDialog.FileName, Text) Then
                My.Settings.Save()

                Threading.Thread.Sleep(500)

                MsgBox("Free SysLog will now close and restart itself for the imported settings to take effect.", MsgBoxStyle.Information, Text)
                Process.Start(strEXEPath)

                Try
                    mutex.ReleaseMutex()
                Catch ex As ApplicationException
                End Try

                Process.GetCurrentProcess.Kill()
            End If
        End Using
    End Sub

    Private Sub ClearLogsOlderThan(daysToKeep As Integer)
        Try
            SyncLock dataGridLockObject
                Logs.AllowUserToOrderColumns = False
                Logs.Enabled = False

                Dim intOldCount As Integer = Logs.Rows.Count
                Dim newListOfLogs As New List(Of MyDataGridViewRow)
                Dim dateChosenDate As Date = Now.AddDays(daysToKeep * -1)

                For Each item As MyDataGridViewRow In Logs.Rows
                    If item.DateObject.Date >= dateChosenDate Then
                        newListOfLogs.Add(item.Clone())
                    End If
                Next

                If MsgBox("Do you want to make a backup of the logs before deleting them?", MsgBoxStyle.Question + MsgBoxStyle.YesNo + vbDefaultButton2, Text) = MsgBoxResult.Yes Then MakeLogBackup()

                Logs.Enabled = True
                Logs.AllowUserToOrderColumns = True

                Logs.Rows.Clear()
                Logs.Rows.AddRange(newListOfLogs.ToArray)

                Dim intCountDifference As Integer = intOldCount - Logs.Rows.Count
                Logs.Rows.Add(SyslogParser.MakeDataGridRow(Now, Now, Now.ToString, IPAddress.Loopback.ToString, Nothing, Nothing, $"The user deleted {intCountDifference:N0} log {If(intCountDifference = 1, "entry", "entries")}.", "Informational, Local", False, Nothing, Logs))

                SelectLatestLogEntry()
            End SyncLock

            UpdateLogCount()
            SaveLogsToDiskSub()
        Catch ex As ArgumentOutOfRangeException
            ' Handle exception if necessary
        End Try
    End Sub

    Private Sub OlderThan1DayToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OlderThan1DayToolStripMenuItem.Click
        ClearLogsOlderThan(1)
    End Sub

    Private Sub OlderThan2DaysToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OlderThan2DaysToolStripMenuItem.Click
        ClearLogsOlderThan(2)
    End Sub

    Private Sub OlderThan3DaysToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OlderThan3DaysToolStripMenuItem.Click
        ClearLogsOlderThan(3)
    End Sub

    Private Sub OlderThanAWeekToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OlderThanAWeekToolStripMenuItem.Click
        ClearLogsOlderThan(7)
    End Sub

    Private Sub ChkConfirmCloseToolStripItem_Click(sender As Object, e As EventArgs) Handles ChkEnableConfirmCloseToolStripItem.Click
        My.Settings.boolConfirmClose = ChkEnableConfirmCloseToolStripItem.Checked
    End Sub

    Private Sub LogsMenu_Opening(sender As Object, e As CancelEventArgs) Handles LogsMenu.Opening
        If Logs.SelectedRows.Count = 1 Then
            CopyLogTextToolStripMenuItem.Visible = True
            OpenLogViewerToolStripMenuItem.Visible = True
            CreateAlertToolStripMenuItem.Visible = True
            CreateReplacementToolStripMenuItem.Visible = True
            CreateIgnoredLogToolStripMenuItem.Visible = True
        Else
            CopyLogTextToolStripMenuItem.Visible = False
            OpenLogViewerToolStripMenuItem.Visible = False
            CreateAlertToolStripMenuItem.Visible = False
            CreateReplacementToolStripMenuItem.Visible = False
            CreateIgnoredLogToolStripMenuItem.Visible = False
        End If

        DeleteLogsToolStripMenuItem.Text = If(Logs.SelectedRows.Count = 1, "Delete Selected Log", "Delete Selected Logs")
        ExportsLogsToolStripMenuItem.Text = If(Logs.SelectedRows.Count = 1, "Export Selected Log", "Export Selected Logs")
    End Sub

    Private Sub CopyLogTextToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyLogTextToolStripMenuItem.Click
        CopyTextToWindowsClipboard(Logs.SelectedRows(0).Cells(ColumnIndex_LogText).Value, Text)
    End Sub

    Private Sub Logs_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles Logs.CellMouseClick
        If e.Button = MouseButtons.Right AndAlso e.RowIndex >= 0 AndAlso e.ColumnIndex >= 0 Then
            Logs.ClearSelection()
            Logs.Rows(e.RowIndex).Cells(e.ColumnIndex).Selected = True
        End If
    End Sub

    Private Sub Logs_MouseDown(sender As Object, e As MouseEventArgs) Handles Logs.MouseDown
        If e.Button = MouseButtons.Right Then
            Dim currentMouseOverRow As Integer = Logs.HitTest(e.X, e.Y).RowIndex

            If currentMouseOverRow >= 0 Then
                If Logs.SelectedRows.Count <= 1 Then
                    Logs.ClearSelection()
                    Logs.Rows(currentMouseOverRow).Selected = True
                End If
            End If
        End If
    End Sub

    Private Sub OpenLogViewerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenLogViewerToolStripMenuItem.Click
        OpenLogViewerWindow()
    End Sub

    Private Sub DeleteLogsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteLogsToolStripMenuItem.Click
        SyncLock dataGridLockObject
            Dim intNumberOfLogsDeleted As Integer = Logs.SelectedRows.Count
            Dim choice As Confirm_Delete.UserChoice

            Using Confirm_Delete As New Confirm_Delete(intNumberOfLogsDeleted) With {.Icon = Icon, .StartPosition = FormStartPosition.CenterParent}
                Confirm_Delete.lblMainLabel.Text = $"Are you sure you want to delete the {intNumberOfLogsDeleted} selected {If(intNumberOfLogsDeleted = 1, "log", "logs")}?"
                Confirm_Delete.ShowDialog(Me)
                choice = Confirm_Delete.choice
            End Using

            If choice = Confirm_Delete.UserChoice.NoDelete Then
                MsgBox("Logs not deleted.", MsgBoxStyle.Information, Text)
                Exit Sub
            ElseIf choice = Confirm_Delete.UserChoice.YesDeleteYesBackup Then
                MakeLogBackup()
            End If

            For Each item As DataGridViewRow In Logs.SelectedRows
                Logs.Rows.Remove(item)
            Next

            Logs.Rows.Add(SyslogParser.MakeDataGridRow(Now, Now, Now.ToString, IPAddress.Loopback.ToString, Nothing, Nothing, $"The user deleted {intNumberOfLogsDeleted:N0} log {If(intNumberOfLogsDeleted = 1, "entry", "entries")}.", "Informational, Local", False, Nothing, Logs))

            SelectLatestLogEntry()
        End SyncLock

        UpdateLogCount()
        SaveLogsToDiskSub()
    End Sub

    Private Sub ExportAllLogsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExportAllLogsToolStripMenuItem.Click
        DataHandling.ExportAllLogs()
    End Sub

    Private Sub ExportsLogsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExportsLogsToolStripMenuItem.Click
        DataHandling.ExportSelectedLogs()
    End Sub

    Private Sub DonationStripMenuItem_Click(sender As Object, e As EventArgs) Handles DonationStripMenuItem.Click
        Process.Start(strPayPal)
    End Sub

    Private Sub StopServerStripMenuItem_Click(sender As Object, e As EventArgs) Handles StopServerStripMenuItem.Click
        If StopServerStripMenuItem.Text = "Stop Server" Then
            SendMessageToSysLogServer("terminate", My.Settings.sysLogPort)
            If My.Settings.EnableTCPServer Then SendMessageToTCPSysLogServer("terminate", My.Settings.sysLogPort)
            StopServerStripMenuItem.Text = "Start Server"
            boolServerRunning = False
        ElseIf StopServerStripMenuItem.Text = "Start Server" Then
            boolServerRunning = True
            serverThread = New Threading.Thread(AddressOf SysLogThread) With {.Name = "UDP Server Thread", .Priority = Threading.ThreadPriority.Normal}
            serverThread.Start()

            If My.Settings.EnableTCPServer Then StartTCPServer()

            SyncLock dataGridLockObject
                Logs.Rows.Add(SyslogParser.MakeDataGridRow(Now, Now, Now.ToString, IPAddress.Loopback.ToString, Nothing, Nothing, "Free SysLog Server Started.", "Informational, Local", False, Nothing, Logs))
                SelectLatestLogEntry()
                UpdateLogCount()
            End SyncLock

            StopServerStripMenuItem.Text = "Stop Server"
        End If
    End Sub

    Private Sub ConfigureAlertsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConfigureAlertsToolStripMenuItem.Click
        Using Alerts As New Alerts With {.Icon = Icon, .StartPosition = FormStartPosition.CenterParent}
            Alerts.ShowDialog(Me)
            If Alerts.boolChanged Then regexCache.Clear()
        End Using
    End Sub

    Private Sub OpenWindowsExplorerToAppConfigFile_Click(sender As Object, e As EventArgs) Handles OpenWindowsExplorerToAppConfigFile.Click
        MsgBox("Modifying the application XML configuration file by hand may cause the program to malfunction. Caution is advised.", MsgBoxStyle.Information, Text)
        SelectFileInWindowsExplorer(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath)
    End Sub

    Private Sub CreateAlertToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CreateAlertToolStripMenuItem.Click
        Using AddAlert As New AddAlert With {.StartPosition = FormStartPosition.CenterParent, .Icon = Icon, .Text = "Add Alert"}
            Dim strLogText As String = Logs.SelectedRows(0).Cells(ColumnIndex_LogText).Value
            AddAlert.TxtLogText.Text = strLogText

            AddAlert.ShowDialog(Me)

            If AddAlert.boolSuccess Then
                Dim boolExistCheck As Boolean = alertsList.Any(Function(item As AlertsClass)
                                                                   Return item.StrLogText.Equals(strLogText, StringComparison.OrdinalIgnoreCase)
                                                               End Function)

                If boolExistCheck Then
                    MsgBox("A similar item has already been found in your alerts list.", MsgBoxStyle.Critical, Text)
                    Exit Sub
                End If

                Dim AlertsClass As New AlertsClass() With {.StrLogText = AddAlert.strLogText, .StrAlertText = AddAlert.strAlertText, .BoolCaseSensitive = AddAlert.boolCaseSensitive, .BoolRegex = AddAlert.boolRegex, .alertType = AddAlert.AlertType}
                alertsList.Add(AlertsClass)
                My.Settings.alerts.Add(Newtonsoft.Json.JsonConvert.SerializeObject(AlertsClass))

                MsgBox("Done", MsgBoxStyle.Information, Text)
            End If
        End Using
    End Sub

    Private Sub ChangeSyslogServerPortToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ChangeSyslogServerPortToolStripMenuItem.Click
        Using IntegerInputForm As New IntegerInputForm(1, 65535) With {.Icon = Icon, .Text = "Change Syslog Server Port", .StartPosition = FormStartPosition.CenterParent}
            IntegerInputForm.lblSetting.Text = "Server Port"
            IntegerInputForm.TxtSetting.Text = My.Settings.sysLogPort

            IntegerInputForm.ShowDialog(Me)

            If IntegerInputForm.boolSuccess Then
                If IntegerInputForm.intResult < 1 Or IntegerInputForm.intResult > 65535 Then
                    MsgBox("The port number must be in the range of 1 - 65535.", MsgBoxStyle.Critical, Text)
                Else
                    If boolDoWeOwnTheMutex Then
                        SendMessageToSysLogServer("terminate", My.Settings.sysLogPort)
                        If My.Settings.EnableTCPServer Then SendMessageToTCPSysLogServer("terminate", My.Settings.sysLogPort)
                    End If

                    ChangeSyslogServerPortToolStripMenuItem.Text = $"Change Syslog Server Port (Port Number {IntegerInputForm.intResult})"

                    My.Settings.sysLogPort = IntegerInputForm.intResult
                    My.Settings.Save()

                    If serverThread.IsAlive Then serverThread.Abort()

                    serverThread = New Threading.Thread(AddressOf SysLogThread) With {.Name = "UDP Server Thread", .Priority = Threading.ThreadPriority.Normal}
                    serverThread.Start()

                    SyncLock dataGridLockObject
                        Logs.Rows.Add(SyslogParser.MakeDataGridRow(Now, Now, Now.ToString, IPAddress.Loopback.ToString, Nothing, Nothing, "Free SysLog Server Started.", "Informational, Local", False, Nothing, Logs))
                        SelectLatestLogEntry()
                        UpdateLogCount()
                    End SyncLock

                    MsgBox("Done.", MsgBoxStyle.Information, Text)
                End If
            End If
        End Using
    End Sub

    Private Sub ChangeAutosaveIntervalToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ChangeLogAutosaveIntervalToolStripMenuItem.Click
        Using IntegerInputForm As New IntegerInputForm(1, 20) With {.Icon = Icon, .Text = "Change Log Autosave Interval", .StartPosition = FormStartPosition.CenterParent}
            IntegerInputForm.lblSetting.Text = "Auto Save (In Minutes)"
            IntegerInputForm.TxtSetting.Text = My.Settings.autoSaveMinutes

            IntegerInputForm.ShowDialog(Me)

            If IntegerInputForm.boolSuccess Then
                ChangeLogAutosaveIntervalToolStripMenuItem.Text = $"Change Log Autosave Interval ({IntegerInputForm.intResult} Minutes)"
                SaveTimer.Interval = TimeSpan.FromMinutes(IntegerInputForm.intResult).TotalMilliseconds
                My.Settings.autoSaveMinutes = IntegerInputForm.intResult

                MsgBox("Done.", MsgBoxStyle.Information, Text)
            End If
        End Using
    End Sub

    Private Sub CreateIgnoredLogToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CreateIgnoredLogToolStripMenuItem.Click
        Using AddIgnored As New AddIgnored With {.StartPosition = FormStartPosition.CenterParent, .Icon = Icon, .Text = "Add Ignored String"}
            Dim strLogText As String = Logs.SelectedRows(0).Cells(ColumnIndex_LogText).Value
            AddIgnored.TxtIgnored.Text = strLogText

            AddIgnored.ShowDialog(Me)

            If AddIgnored.boolSuccess Then
                Dim boolExistCheck As Boolean = ignoredList.Cast(Of IgnoredClass).Any(Function(item As IgnoredClass)
                                                                                          Return item.StrIgnore.Equals(strLogText, StringComparison.OrdinalIgnoreCase)
                                                                                      End Function)

                If boolExistCheck Then
                    MsgBox("A similar item has already been found in your ignored list.", MsgBoxStyle.Critical, Text)
                    Exit Sub
                End If

                Dim IgnoredClass As New IgnoredClass() With {.StrIgnore = AddIgnored.TxtIgnored.Text, .BoolCaseSensitive = AddIgnored.boolCaseSensitive, .BoolRegex = AddIgnored.boolRegex}
                ignoredList.Add(IgnoredClass)
                My.Settings.ignored2.Add(Newtonsoft.Json.JsonConvert.SerializeObject(IgnoredClass))

                MsgBox("Done", MsgBoxStyle.Information, Text)
            End If
        End Using
    End Sub

    Private Sub CreateReplacementToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CreateReplacementToolStripMenuItem.Click
        Using AddReplacement As New AddReplacement With {.StartPosition = FormStartPosition.CenterParent, .Icon = Icon, .Text = "Add Ignored String"}
            Dim strLogText As String = Logs.SelectedRows(0).Cells(ColumnIndex_LogText).Value
            AddReplacement.TxtReplace.Text = strLogText

            AddReplacement.ShowDialog(Me)

            If AddReplacement.boolSuccess Then
                Dim boolExistCheck As Boolean = replacementsList.Cast(Of ReplacementsClass).Any(Function(item As ReplacementsClass)
                                                                                                    Return item.StrReplace.Equals(strLogText, StringComparison.OrdinalIgnoreCase)
                                                                                                End Function)

                If boolExistCheck Then
                    MsgBox("A similar item has already been found in your ignored list.", MsgBoxStyle.Critical, Text)
                    Exit Sub
                End If

                Dim ReplacementClass As New ReplacementsClass() With {.StrReplace = AddReplacement.TxtReplace.Text, .StrReplaceWith = AddReplacement.TxtReplaceWith.Text, .BoolCaseSensitive = AddReplacement.boolCaseSensitive, .BoolRegex = AddReplacement.boolRegex}
                replacementsList.Add(ReplacementClass)
                My.Settings.replacements.Add(Newtonsoft.Json.JsonConvert.SerializeObject(ReplacementClass))

                MsgBox("Done", MsgBoxStyle.Information, Text)
            End If
        End Using
    End Sub

    Private Sub Logs_SelectionChanged(sender As Object, e As EventArgs) Handles Logs.SelectionChanged
        LblItemsSelected.Visible = Logs.SelectedRows.Count > 1
        LblItemsSelected.Text = $"Selected Logs: {Logs.SelectedRows.Count:N0}"
    End Sub

    Private Sub ChkDeselectItemAfterMinimizingWindow_Click(sender As Object, e As EventArgs) Handles ChkDeselectItemAfterMinimizingWindow.Click
        My.Settings.boolDeselectItemsWhenMinimizing = ChkDeselectItemAfterMinimizingWindow.Checked
    End Sub

    Private Sub ConfigureSysLogMirrorServers_Click(sender As Object, e As EventArgs) Handles ConfigureSysLogMirrorServers.Click
        Using ConfigureSysLogMirrorServers As New ConfigureSysLogMirrorServers With {.StartPosition = FormStartPosition.CenterParent, .Icon = Icon}
            ConfigureSysLogMirrorServers.ShowDialog(Me)
            If ConfigureSysLogMirrorServers.boolSuccess Then MsgBox("Done", MsgBoxStyle.Information, Text)
        End Using
    End Sub

    Private Sub ChkShowAlertedColumn_Click(sender As Object, e As EventArgs) Handles ChkShowAlertedColumn.Click
        My.Settings.boolShowAlertedColumn = ChkShowAlertedColumn.Checked
        ColAlerts.Visible = ChkShowAlertedColumn.Checked
    End Sub

    Private Sub MinimizeToClockTray_Click(sender As Object, e As EventArgs) Handles MinimizeToClockTray.Click
        My.Settings.MinimizeToClockTray = MinimizeToClockTray.Checked
    End Sub

    Private Sub BtnOpenLogForViewing_Click(sender As Object, e As EventArgs) Handles BtnOpenLogForViewing.Click
        Using OpenFileDialog As New OpenFileDialog With {.Title = "Open Log File", .Filter = "JSON File|*.json"}
            If OpenFileDialog.ShowDialog() = DialogResult.OK Then
                Dim logFileViewer As New IgnoredLogsAndSearchResults(Me) With {.MainProgramForm = Me, .Icon = Icon, .Text = "Log File Viewer", .WindowDisplayMode = IgnoreOrSearchWindowDisplayMode.viewer, .strFileToLoad = OpenFileDialog.FileName, .boolLoadExternalData = True}
                logFileViewer.Show(Me)
            End If
        End Using
    End Sub

    Private Sub AutomaticallyCheckForUpdates_Click(sender As Object, e As EventArgs) Handles AutomaticallyCheckForUpdates.Click
        My.Settings.boolCheckForUpdates = AutomaticallyCheckForUpdates.Checked
    End Sub

    Private Sub BackupFileNameDateFormatChooser_Click(sender As Object, e As EventArgs) Handles BackupFileNameDateFormatChooser.Click
        Dim DateFormatChooser As New DateFormatChooser() With {.Icon = Icon, .StartPosition = FormStartPosition.CenterParent}
        DateFormatChooser.Show(Me)
    End Sub

    Private Sub ViewLogBackups_Click(sender As Object, e As EventArgs) Handles ViewLogBackups.Click
        Dim collectionOfSavedData As New List(Of SavedData)

        SyncLock dataGridLockObject
            Dim myItem As MyDataGridViewRow

            For Each item As DataGridViewRow In Logs.Rows
                If Not String.IsNullOrWhiteSpace(item.Cells(ColumnIndex_ComputedTime).Value) Then
                    myItem = DirectCast(item, MyDataGridViewRow)

                    collectionOfSavedData.Add(New SavedData With {
                                            .time = myItem.Cells(ColumnIndex_ComputedTime).Value,
                                            .ip = myItem.Cells(ColumnIndex_LogType).Value,
                                            .log = myItem.Cells(ColumnIndex_IPAddress).Value,
                                            .DateObject = myItem.DateObject,
                                            .BoolAlerted = myItem.BoolAlerted
                                          })
                End If
            Next
        End SyncLock

        Dim ViewLogBackups As New ViewLogBackups With {.Icon = Icon, .MyParentForm = Me, .currentLogs = collectionOfSavedData}
        ViewLogBackups.Show(Me)
    End Sub

    Private Sub CloseMe_Click(sender As Object, e As EventArgs) Handles CloseMe.Click
        Close()
    End Sub

    Private Sub StartUpDelay_Click(sender As Object, e As EventArgs) Handles StartUpDelay.Click
        Dim dblSeconds As Double = 0

        Using taskService As New TaskService
            Dim task As Task = Nothing

            If TaskHandling.GetTaskObject(taskService, $"Free SysLog for {Environment.UserName}", task) Then
                If task.Definition.Triggers.Count > 0 Then
                    Dim trigger As Trigger = task.Definition.Triggers(0)
                    If trigger.TriggerType = TaskTriggerType.Logon Then dblSeconds = DirectCast(trigger, LogonTrigger).Delay.TotalSeconds
                End If
            End If
        End Using

        Using IntegerInputForm As New IntegerInputForm(1, 300) With {.Icon = Icon, .Text = "Change Startup Delay", .StartPosition = FormStartPosition.CenterParent}
            IntegerInputForm.lblSetting.Text = "Time in Seconds"
            IntegerInputForm.TxtSetting.Text = dblSeconds.ToString

            IntegerInputForm.ShowDialog(Me)

            If IntegerInputForm.boolSuccess Then
                If IntegerInputForm.intResult < 1 Or IntegerInputForm.intResult > 300 Then
                    MsgBox("The time in seconds must be in the range of 1 - 300.", MsgBoxStyle.Critical, Text)
                Else
                    Using taskService As New TaskService
                        Dim task As Task = Nothing

                        If TaskHandling.GetTaskObject(taskService, $"Free SysLog for {Environment.UserName}", task) Then
                            If task.Definition.Triggers.Count > 0 Then
                                Dim trigger As Trigger = task.Definition.Triggers(0)

                                If trigger.TriggerType = TaskTriggerType.Logon Then
                                    DirectCast(trigger, LogonTrigger).Delay = New TimeSpan(0, 0, IntegerInputForm.intResult)
                                    task.RegisterChanges()
                                End If
                            End If
                        End If
                    End Using

                    StartUpDelay.Text = $"        Startup Delay ({IntegerInputForm.intResult} {If(IntegerInputForm.intResult = 1, "Second", "Seconds")})"
                    MsgBox("Done.", MsgBoxStyle.Information, Text)
                End If
            End If
        End Using
    End Sub

    Private Sub ChkEnableTCPSyslogServer_Click(sender As Object, e As EventArgs) Handles ChkEnableTCPSyslogServer.Click
        My.Settings.EnableTCPServer = ChkEnableTCPSyslogServer.Checked

        If ChkEnableTCPSyslogServer.Checked Then
            StartTCPServer()
        Else
            SendMessageToTCPSysLogServer("terminate", My.Settings.sysLogPort)
        End If
    End Sub

    Private Sub ChkShowHostnameColumn_Click(sender As Object, e As EventArgs) Handles ChkShowHostnameColumn.Click
        My.Settings.boolShowHostnameColumn = ChkShowHostnameColumn.Checked
        ColHostname.Visible = My.Settings.boolShowHostnameColumn
    End Sub

    Private Sub ChkShowLogTypeColumn_Click(sender As Object, e As EventArgs) Handles ChkShowLogTypeColumn.Click
        My.Settings.boolShowLogTypeColumn = ChkShowLogTypeColumn.Checked
        colLogType.Visible = My.Settings.boolShowLogTypeColumn
    End Sub

    Private Sub ChkShowServerTimeColumn_Click(sender As Object, e As EventArgs) Handles ChkShowServerTimeColumn.Click
        My.Settings.boolShowServerTimeColumn = ChkShowServerTimeColumn.Checked
        colServerTime.Visible = My.Settings.boolShowServerTimeColumn
    End Sub

    Private Sub RemoveNumbersFromRemoteApp_Click(sender As Object, e As EventArgs) Handles RemoveNumbersFromRemoteApp.Click
        My.Settings.RemoveNumbersFromRemoteApp = RemoveNumbersFromRemoteApp.Checked
    End Sub

    Private Sub IPv6Support_Click(sender As Object, e As EventArgs) Handles IPv6Support.Click
        My.Settings.IPv6Support = IPv6Support.Checked
        My.Settings.Save()

        If boolServerRunning AndAlso MsgBox("Changing this setting will require a reset of the Syslog Client. Do you want to restart the Syslog Client now?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, Text) = MsgBoxResult.Yes Then
            Threading.ThreadPool.QueueUserWorkItem(Sub()
                                                       SendMessageToSysLogServer("terminate", My.Settings.sysLogPort)
                                                       If My.Settings.EnableTCPServer Then SendMessageToTCPSysLogServer("terminate", My.Settings.sysLogPort)

                                                       Threading.Thread.Sleep(5000)

                                                       serverThread = New Threading.Thread(AddressOf SysLogThread) With {.Name = "UDP Server Thread", .Priority = Threading.ThreadPriority.Normal}
                                                       serverThread.Start()

                                                       If My.Settings.EnableTCPServer Then StartTCPServer()
                                                   End Sub)
        End If
    End Sub

#Region "-- SysLog Server Code --"
    Sub SysLogThread()
        Try
            Using socket As New Socket(AddressFamily.InterNetworkV6, SocketType.Dgram, ProtocolType.Udp) With {.DualMode = My.Settings.IPv6Support}
                socket.Bind(New IPEndPoint(IPAddress.IPv6Any, My.Settings.sysLogPort))

                Dim boolDoServerLoop As Boolean = True
                Dim buffer(4095) As Byte
                Dim remoteEndPoint As EndPoint = New IPEndPoint(IPAddress.IPv6Any, 0)
                Dim ProxiedSysLogData As ProxiedSysLogData

                While boolDoServerLoop
                    Dim bytesReceived As Integer = socket.ReceiveFrom(buffer, remoteEndPoint)
                    Dim strReceivedData As String = Encoding.UTF8.GetString(buffer, 0, bytesReceived)
                    Dim strSourceIP As String = GetIPv4Address(CType(remoteEndPoint, IPEndPoint).Address).ToString()

                    If strReceivedData.Trim.Equals("restore", StringComparison.OrdinalIgnoreCase) Then
                        Invoke(Sub() RestoreWindowAfterReceivingRestoreCommand())
                    ElseIf strReceivedData.Trim.Equals("terminate", StringComparison.OrdinalIgnoreCase) Then
                        boolDoServerLoop = False
                    ElseIf strReceivedData.Trim.StartsWith("proxied", StringComparison.OrdinalIgnoreCase) Then
                        Try
                            strReceivedData = strReceivedData.Replace(strProxiedString, "", StringComparison.OrdinalIgnoreCase)
                            ProxiedSysLogData = Newtonsoft.Json.JsonConvert.DeserializeObject(Of ProxiedSysLogData)(strReceivedData, JSONDecoderSettingsForLogFiles)
                            SyslogParser.ProcessIncomingLog(ProxiedSysLogData.log, ProxiedSysLogData.ip)
                        Catch ex As Newtonsoft.Json.JsonSerializationException
                        End Try
                    Else
                        If serversList.Count > 0 AndAlso Not strReceivedData.StartsWith(strNoProxyString, StringComparison.OrdinalIgnoreCase) Then
                            Threading.ThreadPool.QueueUserWorkItem(Sub()
                                                                       ProxiedSysLogData = New ProxiedSysLogData() With {.ip = strSourceIP, .log = strReceivedData}

                                                                       For Each item As SysLogProxyServer In serversList
                                                                           SendMessageToSysLogServer(strProxiedString & Newtonsoft.Json.JsonConvert.SerializeObject(ProxiedSysLogData), item.ip, item.port)
                                                                       Next

                                                                       ProxiedSysLogData = Nothing
                                                                   End Sub)
                        End If

                        If strReceivedData.StartsWith(strNoProxyString) Then strReceivedData = strReceivedData.Replace(strNoProxyString, "", StringComparison.OrdinalIgnoreCase)
                        SyslogParser.ProcessIncomingLog(strReceivedData, strSourceIP)
                    End If

                    strReceivedData = Nothing
                    strSourceIP = Nothing
                End While
            End Using
        Catch ex As Threading.ThreadAbortException
            ' Does nothing
        Catch e As Exception
            Invoke(Sub()
                       SyncLock dataGridLockObject
                           Logs.Rows.Add(SyslogParser.MakeDataGridRow(Now, Now, Now.ToString, IPAddress.Loopback.ToString, Nothing, Nothing, $"Exception Type: {e.GetType}{vbCrLf}Exception Message: {e.Message}{vbCrLf}{vbCrLf}Exception Stack Trace{vbCrLf}{e.StackTrace}", "Error", False, Nothing, Logs))
                           SelectLatestLogEntry()
                           UpdateLogCount()
                       End SyncLock

                       MsgBox("Unable to start syslog server, perhaps another instance of this program is running on your system.", MsgBoxStyle.Critical + MsgBoxStyle.ApplicationModal, Text)
                   End Sub)
        End Try
    End Sub

    Private Async Sub RestoreWindowAfterReceivingRestoreCommand()
        If boolMaximizedBeforeMinimize Then
            WindowState = FormWindowState.Maximized
        ElseIf My.Settings.boolMaximized Then
            WindowState = FormWindowState.Maximized
        Else
            WindowState = FormWindowState.Normal
        End If

        Await Threading.Tasks.Task.Delay(100)

        SelectLatestLogEntry()
    End Sub
#End Region
End Class