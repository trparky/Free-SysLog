Imports System.IO
Imports System.Net.Sockets
Imports System.Net
Imports System.Text
Imports System.ComponentModel
Imports Microsoft.Win32
Imports System.Text.RegularExpressions
Imports System.Reflection
Imports System.Configuration
Imports Free_SysLog.SupportCode
Imports Microsoft.Toolkit.Uwp.Notifications
Imports Free_SysLog.SyslogTcpServer.SyslogTcpServer

Public Class Form1
    Private boolMaximizedBeforeMinimize As Boolean
    Private boolDoneLoading As Boolean = False
    Public longNumberOfIgnoredLogs As Long = 0
    Public IgnoredLogs As New List(Of MyDataGridViewRow)
    Public IgnoredLogsLockingObject As New Object
    Public intSortColumnIndex As Integer = 0 ' Define intColumnNumber at class level
    Public sortOrder As SortOrder = SortOrder.Ascending ' Define soSortOrder at class level
    Public ReadOnly dataGridLockObject As New Object
    Public ReadOnly IgnoredLogsLockObject As New Object
    Private Const strPayPal As String = "https://paypal.me/trparky"
    Private serverThread As Threading.Thread
    Private SyslogTcpServer As SyslogTcpServer.SyslogTcpServer
    Private boolServerRunning As Boolean = False
    Private boolTCPServerRunning As Boolean = False
    Private lastFirstDisplayedRowIndex As Integer = -1

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

            Logs.Rows.Add(SyslogParser.MakeLocalDataGridRowEntry($"The program deleted {oldLogCount:N0} log {If(oldLogCount = 1, "entry", "entries")} at midnight.", Logs))

            UpdateLogCount()
            SelectLatestLogEntry()
            BtnSaveLogsToDisk.Enabled = True

            SyncLock recentUniqueObjectsLock
                recentUniqueObjects.Clear()
            End SyncLock

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
            Using taskService As New TaskScheduler.TaskService
                taskService.RootFolder.DeleteTask($"Free SysLog for {Environment.UserName}")
            End Using

            StartUpDelay.Enabled = False
        End If
    End Sub

    Public Sub SelectLatestLogEntry()
        If ChkEnableAutoScroll.Checked AndAlso Logs.Rows.Count > 0 AndAlso intSortColumnIndex = 0 Then
            boolIsProgrammaticScroll = True
            Logs.BeginInvoke(Sub()
                                 Logs.FirstDisplayedScrollingRowIndex = If(sortOrder = SortOrder.Ascending, Logs.Rows.Count - 1, 0)
                             End Sub)
        End If
    End Sub

    Private Sub Form1_ResizeBegin(sender As Object, e As EventArgs) Handles Me.ResizeBegin
        Logs.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
    End Sub

    Private Sub Form1_ResizeEnd(sender As Object, e As EventArgs) Handles Me.ResizeEnd
        My.Settings.mainWindowSize = Size
        Threading.Thread.Sleep(100)
        SelectLatestLogEntry()
        Logs.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders
        boolIsProgrammaticScroll = False
    End Sub

    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        If boolDoneLoading Then
            boolIsProgrammaticScroll = True

            If WindowState = FormWindowState.Minimized Then
                If My.Settings.boolDeselectItemsWhenMinimizing Then
                    Logs.ClearSelection()

                    For Each item As MyDataGridViewRow In Logs.Rows
                        item.UncheckRow()
                    Next

                    LblItemsSelected.Visible = False
                End If

                boolMaximizedBeforeMinimize = WindowState = FormWindowState.Maximized
            Else
                My.Settings.boolMaximized = WindowState = FormWindowState.Maximized
            End If

            SyncLock IgnoredLogsAndSearchResultsInstanceLockObject
                If IgnoredLogsAndSearchResultsInstance IsNot Nothing Then IgnoredLogsAndSearchResultsInstance.BtnViewMainWindow.Enabled = WindowState = FormWindowState.Minimized
            End SyncLock

            If MinimizeToClockTray.Checked Then ShowInTaskbar = WindowState <> FormWindowState.Minimized

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

        BringToFront()
        Activate()

        SelectLatestLogEntry()
    End Sub

    Private Sub NotifyIcon_DoubleClick(sender As Object, e As EventArgs) Handles NotifyIcon.DoubleClick
        RestoreWindow()
    End Sub

    Private Sub LoadCheckboxSettings()
        If My.Settings.NotificationLength = 0 Then
            NotificationLengthShort.Checked = True
            NotificationLengthLong.Checked = False
        Else
            NotificationLengthShort.Checked = False
            NotificationLengthLong.Checked = True
        End If

        ColLog.AutoSizeMode = If(My.Settings.colLogAutoFill, DataGridViewAutoSizeColumnMode.Fill, DataGridViewAutoSizeColumnMode.NotSet)

        SaveIgnoredLogCount.Checked = My.Settings.saveIgnoredLogCount
        AskToOpenExplorerWhenSavingData.Checked = My.Settings.AskOpenExplorer
        ColLogsAutoFill.Checked = My.Settings.colLogAutoFill
        IncludeButtonsOnNotifications.Checked = My.Settings.IncludeButtonsOnNotifications
        AutomaticallyCheckForUpdates.Checked = My.Settings.boolCheckForUpdates
        ChkDeselectItemAfterMinimizingWindow.Checked = My.Settings.boolDeselectItemsWhenMinimizing
        ChkEnableRecordingOfIgnoredLogs.Checked = My.Settings.recordIgnoredLogs
        LimitNumberOfIgnoredLogs.Visible = My.Settings.recordIgnoredLogs
        IgnoredLogsToolStripMenuItem.Visible = ChkEnableRecordingOfIgnoredLogs.Checked
        ZerooutIgnoredLogsCounterToolStripMenuItem.Visible = Not ChkEnableRecordingOfIgnoredLogs.Checked
        ChkEnableAutoScroll.Checked = My.Settings.autoScroll
        ChkDisableAutoScrollUponScrolling.Enabled = ChkEnableAutoScroll.Checked
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
        ColHostname.Visible = My.Settings.boolShowHostnameColumn
        ChkShowHostnameColumn.Checked = My.Settings.boolShowHostnameColumn
        colServerTime.Visible = My.Settings.boolShowServerTimeColumn
        ChkShowServerTimeColumn.Checked = My.Settings.boolShowServerTimeColumn
        colLogType.Visible = My.Settings.boolShowLogTypeColumn
        ChkShowLogTypeColumn.Checked = My.Settings.boolShowLogTypeColumn
        RemoveNumbersFromRemoteApp.Checked = My.Settings.RemoveNumbersFromRemoteApp
        IPv6Support.Checked = My.Settings.IPv6Support
        ChkDisableAutoScrollUponScrolling.Checked = My.Settings.disableAutoScrollUponScrolling
        ChkDebug.Checked = My.Settings.boolDebug
        ConfirmDelete.Checked = My.Settings.ConfirmDelete
        ProcessReplacementsInSyslogDataFirst.Checked = My.Settings.ProcessReplacementsInSyslogDataFirst
        ShowCloseButtonOnNotifications.Checked = My.Settings.ShowCloseButtonOnNotifications
    End Sub

    Private Sub LoadAndDeserializeArrays()
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

        If My.Settings.hostnames IsNot Nothing AndAlso My.Settings.hostnames.Count > 0 Then
            Dim customHostname As CustomHostname

            For Each strJSONString As String In My.Settings.hostnames
                customHostname = Newtonsoft.Json.JsonConvert.DeserializeObject(Of CustomHostname)(strJSONString, JSONDecoderSettingsForSettingsFiles)
                SupportCode.hostnames.Add(customHostname.ip, customHostname.deviceName)
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

        SyncLock ignoredListLockingObject
            If My.Settings.ignored2 IsNot Nothing AndAlso My.Settings.ignored2.Count > 0 Then
                For Each strJSONString As String In My.Settings.ignored2
                    tempIgnoredClass = Newtonsoft.Json.JsonConvert.DeserializeObject(Of IgnoredClass)(strJSONString, JSONDecoderSettingsForSettingsFiles)
                    If tempIgnoredClass.BoolEnabled Then ignoredList.Add(tempIgnoredClass)
                    tempIgnoredClass = Nothing
                Next
            End If
        End SyncLock

        If My.Settings.alerts IsNot Nothing AndAlso My.Settings.alerts.Count > 0 Then
            For Each strJSONString As String In My.Settings.alerts
                tempAlertsClass = Newtonsoft.Json.JsonConvert.DeserializeObject(Of AlertsClass)(strJSONString, JSONDecoderSettingsForSettingsFiles)
                If tempAlertsClass.BoolEnabled Then alertsList.Add(tempAlertsClass)
                tempAlertsClass = Nothing
            Next
        End If
    End Sub

    Private Sub boxLimiter_SelectedValueChanged(sender As Object, e As EventArgs) Handles boxLimiter.SelectedValueChanged
        btnShowLimit.Enabled = Not String.IsNullOrWhiteSpace(boxLimiter.Text)
    End Sub

    Private Sub BoxLimitBy_SelectedValueChanged(sender As Object, e As EventArgs) Handles boxLimitBy.SelectedValueChanged
        boxLimiter.Text = Nothing
        boxLimiter.Items.Clear()
        boxLimiter.Text = "(Not Specified)"
        boxLimiter.Enabled = True

        Dim sortedList As List(Of String)

        SyncLock recentUniqueObjectsLock
            If boxLimitBy.Text.Equals("Log Type", StringComparison.OrdinalIgnoreCase) Then
                sortedList = recentUniqueObjects.logTypes.ToList()
                sortedList.Sort()

                boxLimiter.Items.AddRange(sortedList.ToArray)
            ElseIf boxLimitBy.Text.Equals("Remote Process", StringComparison.OrdinalIgnoreCase) Then
                sortedList = recentUniqueObjects.processes.ToList()
                sortedList.Sort()

                boxLimiter.Items.AddRange(sortedList.ToArray)
            ElseIf boxLimitBy.Text.Equals("Source Hostname", StringComparison.OrdinalIgnoreCase) Then
                sortedList = recentUniqueObjects.hostNames.ToList()
                sortedList.Sort()

                boxLimiter.Items.AddRange(sortedList.ToArray)
            ElseIf boxLimitBy.Text.Equals("Source IP Address", StringComparison.OrdinalIgnoreCase) Then
                sortedList = recentUniqueObjects.ipAddresses.ToList()
                sortedList.Sort()

                boxLimiter.Items.AddRange(sortedList.ToArray)
            Else
                boxLimiter.Text = "(Not Specified)"
                boxLimiter.Enabled = False
            End If
        End SyncLock
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

    Private Sub Logs_CellPainting(sender As Object, e As DataGridViewCellPaintingEventArgs) Handles Logs.CellPainting
        If e.RowIndex = -1 AndAlso e.ColumnIndex = ColAlerts.Index AndAlso e.ColumnIndex = colDelete.Index Then
            e.PaintBackground(e.CellBounds, False)
            TextRenderer.DrawText(e.Graphics, e.FormattedValue.ToString(), e.CellStyle.Font, e.CellBounds, e.CellStyle.ForeColor, TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter)
            e.Handled = True
        End If
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SupportCode.ParentForm = Me

        TaskHandling.ConvertRegistryRunCommandToTask()

        If My.Settings.DeleteOldLogsAtMidnight Then CreateNewMidnightTimer()

        ChangeLogAutosaveIntervalToolStripMenuItem.Text = $"        Change Log Autosave Interval ({My.Settings.autoSaveMinutes} Minutes)"
        ChangeSyslogServerPortToolStripMenuItem.Text = $"Change Syslog Server Port (Port Number {My.Settings.sysLogPort})"
        ConfigureTimeBetweenSameNotifications.Text = $"Configure Time Between Same Notifications ({My.Settings.TimeBetweenSameNotifications} Seconds or {FormatSecondsToReadableTime(My.Settings.TimeBetweenSameNotifications)})"
        LimitNumberOfIgnoredLogs.Text = $"Limit Number of Ignored Logs ({My.Settings.LimitNumberOfIgnoredLogs:N0})"

        ColTime.HeaderCell.Style.Padding = New Padding(0, 0, 1, 0)
        ColIPAddress.HeaderCell.Style.Padding = New Padding(0, 0, 2, 0)

        Logs.DefaultCellStyle.Padding = New Padding(0, 20, 0, 20) ' Left, Top, Right, Bottom

        ColTime.HeaderCell.SortGlyphDirection = SortOrder.Ascending
        Icon = Icon.ExtractAssociatedIcon(strEXEPath)
        Location = VerifyWindowLocation(My.Settings.windowLocation, Me)
        If My.Settings.boolMaximized Then WindowState = FormWindowState.Maximized
        NotifyIcon.Icon = Icon
        NotifyIcon.Text = "Free SysLog"

        LoadCheckboxSettings()

        Dim flags As BindingFlags = BindingFlags.NonPublic Or BindingFlags.Instance Or BindingFlags.SetProperty
        Dim propInfo As PropertyInfo = GetType(DataGridView).GetProperty("DoubleBuffered", flags)
        propInfo?.SetValue(Logs, True, Nothing)

        Logs.AlternatingRowsDefaultCellStyle = New DataGridViewCellStyle() With {.BackColor = My.Settings.searchColor, .ForeColor = GetGoodTextColorBasedUponBackgroundColor(My.Settings.searchColor)}
        Logs.DefaultCellStyle = New DataGridViewCellStyle() With {.WrapMode = DataGridViewTriState.True}
        ColLog.DefaultCellStyle = New DataGridViewCellStyle() With {.WrapMode = DataGridViewTriState.True}

        LoadAndDeserializeArrays()
        LoadColumnOrders(Logs.Columns, My.Settings.logsColumnOrder)

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
        ColAlerts.Width = My.Settings.columnAlertedSize

        LblAutoScrollStatus.Text = $"Auto Scroll Status: {If(ChkEnableAutoScroll.Checked, "Enabled", "Disabled")}"

        If My.Settings.saveIgnoredLogCount Then
            longNumberOfIgnoredLogs = My.Settings.ignoredLogCount
            LblNumberOfIgnoredIncomingLogs.Text = $"Number of ignored incoming logs: {longNumberOfIgnoredLogs:N0}"
        End If

        If My.Settings.font IsNot Nothing Then
            Logs.DefaultCellStyle.Font = My.Settings.font
            Logs.ColumnHeadersDefaultCellStyle.Font = My.Settings.font
        End If

        boolDoneLoading = True

        Dim worker As New BackgroundWorker()
        AddHandler worker.DoWork, Sub() LoadDataFile()
        AddHandler worker.RunWorkerCompleted, AddressOf RunWorkerCompleted
        worker.RunWorkerAsync()

        AddNotificationActionHandler()
    End Sub

    Private Sub AddNotificationActionHandler()
        AddHandler ToastNotificationManagerCompat.OnActivated, Sub(args)
                                                                   ' Parse arguments
                                                                   Dim argsDictionary As New Dictionary(Of String, String)
                                                                   Dim itemSplit As String()

                                                                   For Each item As String In args.Argument.Split(";")
                                                                       If Not String.IsNullOrWhiteSpace(item) Then
                                                                           itemSplit = item.Split("=")

                                                                           If itemSplit.Length = 2 AndAlso Not String.IsNullOrWhiteSpace(itemSplit(0)) Then
                                                                               argsDictionary(itemSplit(0)) = itemSplit(1)
                                                                           End If
                                                                       End If
                                                                   Next

                                                                   If argsDictionary.ContainsKey("action") Then
                                                                       If argsDictionary("action").ToString.Equals(strOpenSysLog, StringComparison.OrdinalIgnoreCase) Then
                                                                           Invoke(Sub() RestoreWindow())
                                                                       ElseIf argsDictionary("action").ToString.Equals(strViewLog, StringComparison.OrdinalIgnoreCase) AndAlso argsDictionary.ContainsKey("datapacket") Then
                                                                           Try
                                                                               Dim NotificationDataPacket As NotificationDataPacket = Newtonsoft.Json.JsonConvert.DeserializeObject(Of NotificationDataPacket)(argsDictionary("datapacket").ToString, JSONDecoderSettingsForSettingsFiles)

                                                                               OpenLogViewerWindow(NotificationDataPacket.logtext, NotificationDataPacket.alerttext, NotificationDataPacket.logdate, NotificationDataPacket.sourceip, NotificationDataPacket.rawlogtext, NotificationDataPacket.alertType)
                                                                           Catch ex As Exception
                                                                           End Try
                                                                       End If
                                                                   End If
                                                               End Sub
    End Sub

    Private Async Sub StartTCPServer()
        Dim subRoutine As New SyslogMessageHandlerDelegate(Sub(strReceivedData As String, strSourceIP As String)
                                                               SyslogParser.ProcessIncomingLog(strReceivedData, strSourceIP)
                                                           End Sub)

        SyslogTcpServer = New SyslogTcpServer.SyslogTcpServer(subRoutine, My.Settings.sysLogPort)
        Await SyslogTcpServer.StartAsync()
    End Sub

    Private Sub RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs)
        If Not boolDebugBuild AndAlso My.Settings.boolCheckForUpdates Then Threading.ThreadPool.QueueUserWorkItem(Sub()
                                                                                                                      Dim checkForUpdatesClassObject As New checkForUpdates.CheckForUpdatesClass(Me)
                                                                                                                      checkForUpdatesClassObject.CheckForUpdates(False)
                                                                                                                  End Sub)

        If boolDoWeOwnTheMutex Then
            serverThread = New Threading.Thread(AddressOf SysLogThread) With {.Name = "UDP Server Thread", .Priority = Threading.ThreadPriority.Normal}
            serverThread.Start()

            If My.Settings.EnableTCPServer Then
                StartTCPServer()
                boolTCPServerRunning = True
            End If

            boolServerRunning = True
        Else
            Dim process As Process = GetProcessUsingPort(My.Settings.sysLogPort, ProtocolType.Udp)

            If process Is Nothing Then
                MsgBox("Unable to start syslog server, perhaps another instance of this program is running on your system.", MsgBoxStyle.Critical + MsgBoxStyle.ApplicationModal, Text)
            Else
                Dim strLogText As String = $"Unable to start syslog server. A process with a PID of {process.Id} already has the port open."

                SyncLock dataGridLockObject
                    Logs.Rows.Add(SyslogParser.MakeLocalDataGridRowEntry(strLogText, Logs))
                End SyncLock

                MsgBox(strLogText, MsgBoxStyle.Critical + MsgBoxStyle.ApplicationModal, Text)
            End If
        End If
    End Sub

    Private Sub LoadDataFile(Optional boolShowDataLoadedEvent As Boolean = True)
        If File.Exists(strPathToDataFile) Then
            Try
                Invoke(Sub()
                           Logs.Rows.Add(SyslogParser.MakeLocalDataGridRowEntry("Loading data and populating data grid... Please Wait.", Logs))
                           LblLogFileSize.Text = $"Log File Size: {FileSizeToHumanSize(New FileInfo(strPathToDataFile).Length)}"
                       End Sub)

                Dim collectionOfSavedData As New List(Of SavedData)

                Using fileStream As New StreamReader(strPathToDataFile)
                    collectionOfSavedData = Newtonsoft.Json.JsonConvert.DeserializeObject(Of List(Of SavedData))(fileStream.ReadToEnd.Trim, JSONDecoderSettingsForLogFiles)
                End Using

                Dim listOfLogEntries As New List(Of MyDataGridViewRow)
                Dim stopwatch As Stopwatch = Stopwatch.StartNew

                If collectionOfSavedData.Any() Then
                    Dim intProgress As Integer = 0

                    Invoke(Sub() LoadingProgressBar.Visible = True)

                    For Each item As SavedData In collectionOfSavedData
                        listOfLogEntries.Add(item.MakeDataGridRow(Logs))
                        intProgress += 1
                        Invoke(Sub() LoadingProgressBar.Value = intProgress / collectionOfSavedData.Count * 100)
                    Next

                    Invoke(Sub() LoadingProgressBar.Visible = False)
                End If

                If boolShowDataLoadedEvent Then
                    listOfLogEntries.Add(SyslogParser.MakeLocalDataGridRowEntry($"Free SysLog Server Started. Data loaded in {MyRoundingFunction(stopwatch.Elapsed.TotalMilliseconds / 1000, 2)} seconds.", Logs))
                End If

                SyncLock dataGridLockObject
                    Invoke(Sub()
                               Logs.SuspendLayout()
                               Logs.Rows.Clear()

                               If listOfLogEntries.Count > 2000 Then
                                   Dim intBatchSize As Integer = 250

                                   Threading.Tasks.Task.Run(Sub()
                                                                For index As Integer = 0 To listOfLogEntries.Count - 1 Step intBatchSize
                                                                    Dim batch As MyDataGridViewRow() = listOfLogEntries.Skip(index).Take(intBatchSize).ToArray()
                                                                    Invoke(Sub() Logs.Rows.AddRange(batch)) ' Invoke needed for UI updates
                                                                Next

                                                                Invoke(Sub()
                                                                           SelectLatestLogEntry()

                                                                           Logs.SelectedRows(0).Selected = False
                                                                           UpdateLogCount()
                                                                           Logs.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders
                                                                           Logs.ResumeLayout()
                                                                       End Sub)
                                                            End Sub)
                               Else
                                   Logs.Rows.AddRange(listOfLogEntries.ToArray)
                                   Logs.ResumeLayout()

                                   SelectLatestLogEntry()

                                   Logs.SelectedRows(0).Selected = False
                                   UpdateLogCount()
                                   Logs.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders
                               End If
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
                       Logs.SuspendLayout()
                       Logs.Rows.Clear()

                       Dim listOfLogEntries As New List(Of MyDataGridViewRow) From {
                           SyslogParser.MakeLocalDataGridRowEntry("Free SysLog Server Started.", Logs),
                           SyslogParser.MakeLocalDataGridRowEntry("There was an error while decoing the JSON data, existing data was copied to another file and the log file was reset.", Logs),
                           SyslogParser.MakeLocalDataGridRowEntry($"Exception Type: {ex.GetType}{vbCrLf}Exception Message: {ex.Message}{vbCrLf}{vbCrLf}Exception Stack Trace{vbCrLf}{ex.StackTrace}", Logs)
                       }

                       Logs.Rows.AddRange(listOfLogEntries.ToArray)
                       Logs.ResumeLayout()
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

    Private Sub Logs_DoubleClick(sender As Object, e As EventArgs) Handles Logs.DoubleClick
        Dim hitTest As DataGridView.HitTestInfo = Logs.HitTest(Logs.PointToClient(MousePosition).X, Logs.PointToClient(MousePosition).Y)
        If hitTest.Type = DataGridViewHitTestType.Cell And hitTest.RowIndex <> -1 Then OpenLogViewerWindow()
    End Sub

    Private Sub Logs_KeyUp(sender As Object, e As KeyEventArgs) Handles Logs.KeyUp
        If e.KeyValue = Keys.Delete Then
            SyncLock dataGridLockObject
                Dim intNumberOfCheckedLogs As Integer = Logs.Rows.Cast(Of DataGridViewRow).Where(Function(row As MyDataGridViewRow) row.Cells(colDelete.Index).Value).Count()
                Dim intNumberOfSelectedLogs As Integer = Logs.SelectedRows.Count

                If intNumberOfCheckedLogs <> 0 And intNumberOfCheckedLogs <> 1 And intNumberOfSelectedLogs <> 0 And intNumberOfSelectedLogs <> 1 AndAlso intNumberOfCheckedLogs <> intNumberOfSelectedLogs AndAlso MsgBox("The checked logs does not match the selected logs, do you want to make the checked logs match the selected logs?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, Text) = MsgBoxResult.Yes Then
                    For Each item As MyDataGridViewRow In Logs.SelectedRows
                        item.CheckRow()
                    Next

                    intNumberOfCheckedLogs = Logs.Rows.Cast(Of DataGridViewRow).Where(Function(row As MyDataGridViewRow) row.Cells(colDelete.Index).Value).Count()
                    intNumberOfSelectedLogs = Logs.SelectedRows.Count
                End If

                If ConfirmDelete.Checked Then
                    Dim choice As Confirm_Delete.UserChoice

                    Using Confirm_Delete As New Confirm_Delete(intNumberOfSelectedLogs) With {.Icon = Icon, .StartPosition = FormStartPosition.CenterParent}
                        If intNumberOfCheckedLogs > 0 Then
                            Confirm_Delete.lblMainLabel.Text = $"Are you sure you want to delete the {intNumberOfCheckedLogs} checked {If(intNumberOfCheckedLogs = 1, "log", "logs")}?"
                        Else
                            Confirm_Delete.lblMainLabel.Text = $"Are you sure you want to delete the {intNumberOfSelectedLogs} selected {If(intNumberOfSelectedLogs = 1, "log", "logs")}?"
                        End If

                        Confirm_Delete.ShowDialog(Me)
                        choice = Confirm_Delete.choice
                    End Using

                    If choice = Confirm_Delete.UserChoice.NoDelete Then
                        If intNumberOfCheckedLogs > 0 Then
                            MsgBox($"{If(intNumberOfCheckedLogs = 1, "Log", "Logs")} not deleted.", MsgBoxStyle.Information, Text)
                        Else
                            MsgBox($"{If(intNumberOfSelectedLogs = 1, "Log", "Logs")} not deleted.", MsgBoxStyle.Information, Text)
                        End If

                        Exit Sub
                    ElseIf choice = Confirm_Delete.UserChoice.YesDeleteYesBackup Then
                        MakeLogBackup()
                    End If
                End If

                ' Create a list to store rows that need to be removed
                Dim rowsToDelete As New List(Of MyDataGridViewRow)

                If intNumberOfCheckedLogs > 0 Then
                    ' Loop through the rows in reverse order to avoid index shifting
                    For i As Integer = Logs.Rows.Count - 1 To 0 Step -1
                        If Logs.Rows(i).Cells(colDelete.Index).Value Then rowsToDelete.Add(Logs.Rows(i))
                    Next
                Else
                    ' Loop through the rows in reverse order to avoid index shifting
                    For i As Integer = Logs.SelectedRows.Count - 1 To 0 Step -1
                        rowsToDelete.Add(Logs.SelectedRows(i))
                    Next
                End If

                ' Remove the rows outside the loop
                For Each row As MyDataGridViewRow In rowsToDelete
                    Logs.Rows.Remove(row)
                Next

                Logs.Rows.Add(SyslogParser.MakeLocalDataGridRowEntry($"The user deleted {rowsToDelete.Count:N0} log {If(rowsToDelete.Count = 1, "entry", "entries")}.", Logs))

                BtnSaveLogsToDisk.Enabled = True

                SelectLatestLogEntry()
            End SyncLock

            UpdateLogCount()
            SaveLogsToDiskSub()
        ElseIf e.KeyValue = Keys.Space Then
            e.Handled = True

            If Logs.SelectedCells.Count > 0 Then
                If Logs.SelectedCells.Count = 1 Then
                    Dim selectedRow As MyDataGridViewRow = Logs.Rows(Logs.SelectedCells(0).RowIndex)
                    selectedRow.InvertRowCheckboxStatus()
                Else
                    For Each item As MyDataGridViewRow In Logs.SelectedRows
                        item.InvertRowCheckboxStatus()
                    Next
                End If

                Dim intNumberOfCheckedLogs As Integer = Logs.Rows.Cast(Of DataGridViewRow).Where(Function(row As MyDataGridViewRow) row.Cells(colDelete.Index).Value).Count()

                If intNumberOfCheckedLogs = 0 Then
                    LblItemsSelected.Visible = False
                    LblItemsSelected.Text = Nothing
                Else
                    LblItemsSelected.Visible = True
                    LblItemsSelected.Text = $"Checked Logs: {intNumberOfCheckedLogs:N0}"
                End If
            End If
        End If
    End Sub

    Private Sub Logs_KeyDown(sender As Object, e As KeyEventArgs) Handles Logs.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            OpenLogViewerWindow()
        End If
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
        ChkDisableAutoScrollUponScrolling.Enabled = ChkEnableAutoScroll.Checked
        LblAutoScrollStatus.Text = "Auto Scroll Status: Enabled"
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
            My.Settings.columnAlertedSize = ColAlerts.Width
        End If
    End Sub

    Private Sub BtnClearAllLogs_Click(sender As Object, e As EventArgs) Handles BtnClearAllLogs.Click
        If MsgBox("Are you sure you want to clear the logs?", MsgBoxStyle.Question + MsgBoxStyle.YesNo + vbDefaultButton2, Text) = MsgBoxResult.Yes Then
            SyncLock dataGridLockObject
                If MsgBox("Do you want to make a backup of the logs before deleting them?", MsgBoxStyle.Question + MsgBoxStyle.YesNo + vbDefaultButton2, Text) = MsgBoxResult.Yes Then MakeLogBackup()

                Dim intOldCount As Integer = Logs.Rows.Count
                Logs.Rows.Clear()
                Logs.Rows.Add(SyslogParser.MakeLocalDataGridRowEntry($"The user deleted {intOldCount:N0} log {If(intOldCount = 1, "entry", "entries")}.", Logs))

                SelectLatestLogEntry()
            End SyncLock

            UpdateLogCount()
            SaveLogsToDiskSub()
        End If
    End Sub

    Public Sub SaveLogsToDiskSub()
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
        If e.CloseReason = CloseReason.UserClosing AndAlso My.Settings.boolConfirmClose Then
            Using CloseFreeSysLog As New CloseFreeSysLogDialog()
                CloseFreeSysLog.StartPosition = FormStartPosition.CenterParent
                CloseFreeSysLog.MyParentForm = Me

                Dim result As DialogResult = CloseFreeSysLog.ShowDialog(Me)

                If result = DialogResult.No Then
                    e.Cancel = True
                    Exit Sub
                ElseIf result = DialogResult.Cancel Then
                    WindowState = FormWindowState.Minimized
                    e.Cancel = True
                    Exit Sub
                End If
            End Using
        End If

        If e.CloseReason = CloseReason.WindowsShutDown Then
            SyncLock dataGridLockObject
                Logs.Rows.Add(SyslogParser.MakeLocalDataGridRowEntry("Windows shutdown initiated, Free Syslog is closing.", Logs))
            End SyncLock
        End If

        If My.Settings.saveIgnoredLogCount Then My.Settings.ignoredLogCount = longNumberOfIgnoredLogs
        My.Settings.logsColumnOrder = SaveColumnOrders(Logs.Columns)
        My.Settings.Save()
        DataHandling.WriteLogsToDisk()

        If boolDoWeOwnTheMutex Then
            SendMessageToSysLogServer(strTerminate, My.Settings.sysLogPort)
            If My.Settings.EnableTCPServer Then SendMessageToTCPSysLogServer(strTerminate, My.Settings.sysLogPort)
        End If

        Try
            mutex.ReleaseMutex()
        Catch ex As ApplicationException
        End Try

        Process.GetCurrentProcess.Kill()
    End Sub

    Private Sub BtnSearch_Click(sender As Object, e As EventArgs) Handles BtnSearch.Click
        If String.IsNullOrWhiteSpace(TxtSearchTerms.Text) Then
            MsgBox("You must provide something to search for.", MsgBoxStyle.Critical, Text)
            Exit Sub
        End If

        Dim strLimitBy As String = boxLimitBy.Text
        Dim strLimiter As String = boxLimiter.Text
        Dim boolDoesLogMatchLimitedSearch As Boolean = True
        Dim strLogText As String
        Dim listOfSearchResults As New List(Of MyDataGridViewRow)
        Dim regexCompiledObject As Regex = Nothing
        Dim MyDataGridRowItem As MyDataGridViewRow
        Dim stopWatch As Stopwatch = Stopwatch.StartNew

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

                                                      If strLimitBy.Equals("Log Type", StringComparison.OrdinalIgnoreCase) Then
                                                          boolDoesLogMatchLimitedSearch = String.Equals(MyDataGridRowItem.Cells(ColumnIndex_LogType).Value, strLimiter, StringComparison.OrdinalIgnoreCase)
                                                      ElseIf strLimitBy.Equals("Remote Process", StringComparison.OrdinalIgnoreCase) Then
                                                          boolDoesLogMatchLimitedSearch = String.Equals(MyDataGridRowItem.Cells(ColumnIndex_RemoteProcess).Value, strLimiter, StringComparison.OrdinalIgnoreCase)
                                                      ElseIf strLimitBy.Equals("Source Hostname", StringComparison.OrdinalIgnoreCase) Then
                                                          boolDoesLogMatchLimitedSearch = String.Equals(MyDataGridRowItem.Cells(ColumnIndex_Hostname).Value, strLimiter, StringComparison.OrdinalIgnoreCase)
                                                      ElseIf strLimitBy.Equals("Source IP Address", StringComparison.OrdinalIgnoreCase) Then
                                                          boolDoesLogMatchLimitedSearch = String.Equals(MyDataGridRowItem.Cells(ColumnIndex_IPAddress).Value, strLimiter, StringComparison.OrdinalIgnoreCase)
                                                      Else
                                                          boolDoesLogMatchLimitedSearch = True
                                                      End If

                                                      If boolDoesLogMatchLimitedSearch AndAlso Not String.IsNullOrWhiteSpace(strLogText) AndAlso regexCompiledObject.IsMatch(strLogText) Then
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
                                                  If listOfSearchResults.Any() Then
                                                      Dim searchResultsWindow As New IgnoredLogsAndSearchResults(Me, IgnoreOrSearchWindowDisplayMode.search) With {.MainProgramForm = Me, .Icon = Icon, .LogsToBeDisplayed = listOfSearchResults, .Text = "Search Results"}
                                                      searchResultsWindow.LogsLoadedInLabel.Visible = True
                                                      searchResultsWindow.LogsLoadedInLabel.Text = $"Search took {MyRoundingFunction(stopWatch.Elapsed.TotalMilliseconds / 1000, 2)} seconds"
                                                      searchResultsWindow.ChkColLogsAutoFill.Checked = My.Settings.colLogAutoFill
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
        If e.Button = MouseButtons.Left Then
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

            SortLogsByDateObject(column.Index, If(sortOrder = SortOrder.Ascending, ListSortDirection.Ascending, ListSortDirection.Descending))
        End If
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

    Private Sub IgnoredWordsAndPhrasesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConfigureIgnoredWordsAndPhrasesToolStripMenuItem.Click
        Using IgnoredWordsAndPhrasesOrAlertsInstance As New IgnoredWordsAndPhrases With {.Icon = Icon, .StartPosition = FormStartPosition.CenterParent}
            IgnoredWordsAndPhrasesOrAlertsInstance.ShowDialog(Me)

            If IgnoredWordsAndPhrasesOrAlertsInstance.boolChanged Then
                SyncLock IgnoredRegexCacheLockingObject
                    IgnoredRegexCache.Clear()
                End SyncLock
            End If
        End Using
    End Sub

    Private Sub ViewIgnoredLogsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ViewIgnoredLogsToolStripMenuItem.Click
        SyncLock IgnoredLogsAndSearchResultsInstanceLockObject
            If IgnoredLogsAndSearchResultsInstance Is Nothing Then
                IgnoredLogsAndSearchResultsInstance = New IgnoredLogsAndSearchResults(Me, IgnoreOrSearchWindowDisplayMode.ignored) With {.MainProgramForm = Me, .Icon = Icon, .LogsToBeDisplayed = IgnoredLogs, .Text = "Ignored Logs"}
                IgnoredLogsAndSearchResultsInstance.ChkColLogsAutoFill.Checked = My.Settings.colLogAutoFill
                IgnoredLogsAndSearchResultsInstance.Show()
            Else
                IgnoredLogsAndSearchResultsInstance.WindowState = FormWindowState.Normal
                IgnoredLogsAndSearchResultsInstance.BringToFront()
            End If
        End SyncLock
    End Sub

    Private Sub ClearIgnoredLogsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ClearIgnoredLogsToolStripMenuItem.Click
        If MsgBox("Are you sure you want to clear the ignored logs stored in system memory?", MsgBoxStyle.Question + MsgBoxStyle.YesNo + vbDefaultButton2, Text) = MsgBoxResult.Yes Then
            SyncLock IgnoredLogsLockObject
                For Each item As MyDataGridViewRow In IgnoredLogs
                    item.Dispose()
                Next

                SyncLock IgnoredLogsLockingObject
                    IgnoredLogs.Clear()
                End SyncLock

                GC.Collect()
                GC.WaitForPendingFinalizers()

                longNumberOfIgnoredLogs = 0
                ClearIgnoredLogsToolStripMenuItem.Enabled = False

                If My.Settings.recordIgnoredLogs Then
                    LblNumberOfIgnoredIncomingLogs.Text = $"Number of ignored incoming logs: {IgnoredLogs.Count:N0}"
                Else
                    LblNumberOfIgnoredIncomingLogs.Text = $"Number of ignored incoming logs: {longNumberOfIgnoredLogs:N0}"
                End If
            End SyncLock
        End If
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
        LimitNumberOfIgnoredLogs.Visible = My.Settings.recordIgnoredLogs
        IgnoredLogsToolStripMenuItem.Visible = ChkEnableRecordingOfIgnoredLogs.Checked
        ZerooutIgnoredLogsCounterToolStripMenuItem.Visible = Not ChkEnableRecordingOfIgnoredLogs.Checked
        longNumberOfIgnoredLogs = 0

        If Not ChkEnableRecordingOfIgnoredLogs.Checked Then
            SyncLock IgnoredLogsLockingObject
                IgnoredLogs.Clear()
            End SyncLock

            LblNumberOfIgnoredIncomingLogs.Text = "Number of ignored incoming logs: 0"
        End If
    End Sub

    Private Sub LogsOlderThanToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LogsOlderThanToolStripMenuItem.Click
        Using ClearLogsOlderThanInstance As New ClearLogsOlderThan With {.Icon = Icon, .StartPosition = FormStartPosition.CenterParent}
            ClearLogsOlderThanInstance.LblLogCount.Text = $"Number of Log Entries: {Logs.Rows.Count:N0}"
            ClearLogsOlderThanInstance.ShowDialog(Me)

            If ClearLogsOlderThanInstance.DialogResult = DialogResult.OK Then
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

                        Logs.SuspendLayout()
                        Logs.Rows.Clear()
                        Logs.Rows.AddRange(newListOfLogs.ToArray)

                        Dim intCountDifference As Integer = intOldCount - Logs.Rows.Count
                        Logs.Rows.Add(SyslogParser.MakeLocalDataGridRowEntry($"The user deleted {intCountDifference:N0} log {If(intCountDifference = 1, "entry", "entries")}.", Logs))

                        Logs.ResumeLayout()

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
        IgnoredHits.Clear()
        longNumberOfIgnoredLogs = 0
        LblNumberOfIgnoredIncomingLogs.Text = $"Number of ignored incoming logs: {longNumberOfIgnoredLogs:N0}"
    End Sub

    Private Sub ConfigureReplacementsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConfigureReplacementsToolStripMenuItem.Click
        Using ReplacementsInstance As New Replacements With {.Icon = Icon, .StartPosition = FormStartPosition.CenterParent}
            ReplacementsInstance.ShowDialog(Me)

            SyncLock ReplacementsRegexCacheLockingObject
                If ReplacementsInstance.boolChanged Then
                    ReplacementsRegexCache.Clear()
                End If
            End SyncLock
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
            SaveFileDialog.Title = "Save Program Settings..."
            SaveFileDialog.Filter = "JSON File|*.json"

            If SaveFileDialog.ShowDialog() = DialogResult.OK Then
                Try
                    SaveAppSettings.SaveApplicationSettingsToFile(SaveFileDialog.FileName)

                    If My.Settings.AskOpenExplorer Then
                        Using OpenExplorer As New OpenExplorer()
                            OpenExplorer.StartPosition = FormStartPosition.CenterParent
                            OpenExplorer.MyParentForm = SupportCode.ParentForm

                            Dim result As DialogResult = OpenExplorer.ShowDialog(Me)

                            If result = DialogResult.No Then
                                Exit Sub
                            ElseIf result = DialogResult.Yes Then
                                SelectFileInWindowsExplorer(SaveFileDialog.FileName)
                            End If
                        End Using
                    Else
                        MsgBox("Data exported successfully.", MsgBoxStyle.Information, Text)
                    End If
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

                Logs.SuspendLayout()
                Logs.Rows.Clear()
                Logs.Rows.AddRange(newListOfLogs.ToArray)

                Dim intCountDifference As Integer = intOldCount - Logs.Rows.Count
                Logs.Rows.Add(SyslogParser.MakeLocalDataGridRowEntry($"The user deleted {intCountDifference:N0} log {If(intCountDifference = 1, "entry", "entries")}.", Logs))

                Logs.ResumeLayout()

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
        Dim hitTest As DataGridView.HitTestInfo = Logs.HitTest(Logs.PointToClient(MousePosition).X, Logs.PointToClient(MousePosition).Y)

        If hitTest.Type = DataGridViewHitTestType.ColumnHeader Then
            e.Cancel = True
            Exit Sub
        End If

        If Logs.SelectedRows.Count = 0 Then
            DeleteLogsToolStripMenuItem.Visible = False
            ExportsLogsToolStripMenuItem.Visible = False
            DeleteSimilarLogsToolStripMenuItem.Visible = False
        Else
            DeleteLogsToolStripMenuItem.Visible = True
            ExportsLogsToolStripMenuItem.Visible = True
            DeleteSimilarLogsToolStripMenuItem.Visible = True
        End If

        If Logs.SelectedRows.Count = 1 Then
            CopyLogTextToolStripMenuItem.Visible = True
            OpenLogViewerToolStripMenuItem.Visible = True
            CreateAlertToolStripMenuItem.Visible = True
            CreateReplacementToolStripMenuItem.Visible = True
            CreateIgnoredLogToolStripMenuItem.Visible = True
            DeleteSimilarLogsToolStripMenuItem.Visible = True

            Dim selectedItem As MyDataGridViewRow = TryCast(Logs.SelectedRows(0), MyDataGridViewRow)
            If selectedItem IsNot Nothing Then CopyRawLogTextToolStripMenuItem.Visible = Not String.IsNullOrEmpty(selectedItem.RawLogData)
        Else
            OpenLogViewerToolStripMenuItem.Visible = False
            CreateAlertToolStripMenuItem.Visible = False
            CreateReplacementToolStripMenuItem.Visible = False
            CreateIgnoredLogToolStripMenuItem.Visible = False
            DeleteSimilarLogsToolStripMenuItem.Visible = False
        End If

        DeleteLogsToolStripMenuItem.Text = If(Logs.SelectedRows.Count = 1, "Delete Selected Log", "Delete Selected Logs")
        ExportsLogsToolStripMenuItem.Text = If(Logs.SelectedRows.Count = 1, "Export Selected Log", "Export Selected Logs")
    End Sub

    Private Sub CopyLogTextToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyLogTextToolStripMenuItem.Click
        Dim intLogCount As Integer = Logs.SelectedRows().Count
        Dim boolCopyToClipboardResults As Boolean = False

        If intLogCount <> 0 Then
            If intLogCount = 1 Then
                boolCopyToClipboardResults = CopyTextToWindowsClipboard(Logs.SelectedRows(0).Cells(ColumnIndex_LogText).Value, Text)
            Else
                Dim allSelectedLogs As New StringBuilder()
                Dim selectedItem As MyDataGridViewRow

                For Each item As DataGridViewRow In Logs.SelectedRows
                    selectedItem = TryCast(item, MyDataGridViewRow)
                    If selectedItem IsNot Nothing Then allSelectedLogs.AppendLine(selectedItem.Cells(ColumnIndex_LogText).Value)
                Next

                If allSelectedLogs.Length > 0 Then boolCopyToClipboardResults = CopyTextToWindowsClipboard(allSelectedLogs.ToString().Trim, Text)
            End If

            If boolCopyToClipboardResults Then MsgBox("Data copied to clipboard.", MsgBoxStyle.Information, Text)
        End If
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

        Dim hitTest As DataGridView.HitTestInfo = Logs.HitTest(e.X, e.Y)

        If hitTest.Type = DataGridViewHitTestType.ColumnHeader Then
            Logs.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
        ElseIf hitTest.Type = DataGridViewHitTestType.Cell And hitTest.ColumnIndex = colDelete.Index Then
            Dim cell As DataGridViewCheckBoxCell = DirectCast(Logs.Rows(hitTest.RowIndex).Cells(colDelete.Index), DataGridViewCheckBoxCell)
            cell.Value = Not cell.Value
        End If
    End Sub

    Private Sub Logs_MouseUp(sender As Object, e As MouseEventArgs) Handles Logs.MouseUp
        Dim hitTest As DataGridView.HitTestInfo = Logs.HitTest(e.X, e.Y)

        If hitTest.Type = DataGridViewHitTestType.ColumnHeader Then
            Logs.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders
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
                MsgBox($"{If(intNumberOfLogsDeleted = 1, "Log", "Logs")} not deleted.", MsgBoxStyle.Information, Text)
                Exit Sub
            ElseIf choice = Confirm_Delete.UserChoice.YesDeleteYesBackup Then
                MakeLogBackup()
            End If

            ' Create a list to store rows that need to be removed
            Dim rowsToDelete As New List(Of MyDataGridViewRow)

            ' Loop through the rows in reverse order to avoid index shifting
            For i As Integer = Logs.SelectedRows.Count - 1 To 0 Step -1
                rowsToDelete.Add(Logs.SelectedRows(i))
            Next

            ' Remove the rows outside the loop
            For Each row As MyDataGridViewRow In rowsToDelete
                Logs.Rows.Remove(row)
            Next

            Logs.Rows.Add(SyslogParser.MakeLocalDataGridRowEntry($"The user deleted {rowsToDelete.Count:N0} log {If(rowsToDelete.Count = 1, "entry", "entries")}.", Logs))

            SelectLatestLogEntry()
        End SyncLock

        UpdateLogCount()
        SaveLogsToDiskSub()
    End Sub

    Private Sub ExportAllLogsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExportAllLogsToolStripMenuItem.Click
        DataHandling.ExportAllLogs(Logs.Rows)
    End Sub

    Private Sub ExportsLogsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExportsLogsToolStripMenuItem.Click
        DataHandling.ExportSelectedLogs(Logs.SelectedRows)
    End Sub

    Private Sub DonationStripMenuItem_Click(sender As Object, e As EventArgs) Handles DonationStripMenuItem.Click
        Process.Start(strPayPal)
    End Sub

    Private Sub StopServerStripMenuItem_Click(sender As Object, e As EventArgs) Handles StopServerStripMenuItem.Click
        If StopServerStripMenuItem.Text = "Stop Server" Then
            SendMessageToSysLogServer(strTerminate, My.Settings.sysLogPort)
            If My.Settings.EnableTCPServer Then SendMessageToTCPSysLogServer(strTerminate, My.Settings.sysLogPort)
            StopServerStripMenuItem.Text = "Start Server"
            boolServerRunning = False
        ElseIf StopServerStripMenuItem.Text = "Start Server" And boolDoWeOwnTheMutex Then
            boolServerRunning = True
            serverThread = New Threading.Thread(AddressOf SysLogThread) With {.Name = "UDP Server Thread", .Priority = Threading.ThreadPriority.Normal}
            serverThread.Start()

            If My.Settings.EnableTCPServer Then
                StartTCPServer()
                boolTCPServerRunning = True
            End If

            SyncLock dataGridLockObject
                Logs.Rows.Add(SyslogParser.MakeLocalDataGridRowEntry("Free SysLog Server Started.", Logs))
                SelectLatestLogEntry()
                UpdateLogCount()
            End SyncLock

            StopServerStripMenuItem.Text = "Stop Server"
        End If
    End Sub

    Private Sub ConfigureAlertsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConfigureAlertsToolStripMenuItem.Click
        Using Alerts As New Alerts With {.Icon = Icon, .StartPosition = FormStartPosition.CenterParent}
            Alerts.ShowDialog(Me)

            If Alerts.boolChanged Then
                SyncLock AlertsRegexCacheLockingObject
                    AlertsRegexCache.Clear()
                End SyncLock
            End If
        End Using
    End Sub

    Private Sub OpenWindowsExplorerToAppConfigFile_Click(sender As Object, e As EventArgs) Handles OpenWindowsExplorerToAppConfigFile.Click
        MsgBox("Modifying the application XML configuration file by hand may cause the program to malfunction. Caution is advised.", MsgBoxStyle.Information, Text)
        SelectFileInWindowsExplorer(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath)
    End Sub

    Private Sub CreateAlertToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CreateAlertToolStripMenuItem.Click
        Using Alerts As New Alerts With {.StartPosition = FormStartPosition.CenterParent, .Icon = Icon}
            Alerts.TxtLogText.Text = Logs.SelectedRows(0).Cells(ColumnIndex_LogText).Value
            Alerts.ShowDialog(Me)
        End Using
    End Sub

    Private Sub ChangeSyslogServerPortToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ChangeSyslogServerPortToolStripMenuItem.Click
        If boolDoWeOwnTheMutex Then
            Using IntegerInputForm As New IntegerInputForm(1, 65535) With {.Icon = Icon, .Text = "Change Syslog Server Port", .StartPosition = FormStartPosition.CenterParent}
                IntegerInputForm.lblSetting.Text = "Server Port"
                IntegerInputForm.TxtSetting.Text = My.Settings.sysLogPort

                IntegerInputForm.ShowDialog(Me)

                If IntegerInputForm.DialogResult = DialogResult.OK Then
                    If IntegerInputForm.intResult < 1 Or IntegerInputForm.intResult > 65535 Then
                        MsgBox("The port number must be in the range of 1 - 65535.", MsgBoxStyle.Critical, Text)
                    Else
                        If boolDoWeOwnTheMutex Then
                            SendMessageToSysLogServer(strTerminate, My.Settings.sysLogPort)
                            If My.Settings.EnableTCPServer Then SendMessageToTCPSysLogServer(strTerminate, My.Settings.sysLogPort)
                        End If

                        ChangeSyslogServerPortToolStripMenuItem.Text = $"Change Syslog Server Port (Port Number {IntegerInputForm.intResult})"

                        My.Settings.sysLogPort = IntegerInputForm.intResult
                        My.Settings.Save()

                        If serverThread.IsAlive Then serverThread.Abort()

                        serverThread = New Threading.Thread(AddressOf SysLogThread) With {.Name = "UDP Server Thread", .Priority = Threading.ThreadPriority.Normal}
                        serverThread.Start()

                        SyncLock dataGridLockObject
                            Logs.Rows.Add(SyslogParser.MakeLocalDataGridRowEntry("Free SysLog Server Started.", Logs))
                            SelectLatestLogEntry()
                            UpdateLogCount()
                        End SyncLock

                        MsgBox("Done.", MsgBoxStyle.Information, Text)
                    End If
                End If
            End Using
        End If
    End Sub

    Private Sub ChangeAutosaveIntervalToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ChangeLogAutosaveIntervalToolStripMenuItem.Click
        Using IntegerInputForm As New IntegerInputForm(1, 20) With {.Icon = Icon, .Text = "Change Log Autosave Interval", .StartPosition = FormStartPosition.CenterParent}
            IntegerInputForm.lblSetting.Text = "Auto Save (In Minutes)"
            IntegerInputForm.TxtSetting.Text = My.Settings.autoSaveMinutes

            IntegerInputForm.ShowDialog(Me)

            If IntegerInputForm.DialogResult = DialogResult.OK Then
                ChangeLogAutosaveIntervalToolStripMenuItem.Text = $"Change Log Autosave Interval ({IntegerInputForm.intResult} Minutes)"
                SaveTimer.Interval = TimeSpan.FromMinutes(IntegerInputForm.intResult).TotalMilliseconds
                My.Settings.autoSaveMinutes = IntegerInputForm.intResult

                MsgBox("Done.", MsgBoxStyle.Information, Text)
            End If
        End Using
    End Sub

    Private Sub CreateIgnoredLogToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CreateIgnoredLogToolStripMenuItem.Click
        If Logs.SelectedRows.Count > 0 Then
            Using Ignored As New IgnoredWordsAndPhrases With {.StartPosition = FormStartPosition.CenterParent, .Icon = Icon}
                Dim myItem As MyDataGridViewRow = TryCast(Logs.SelectedRows(0), MyDataGridViewRow)

                If myItem IsNot Nothing Then
                    Ignored.TxtIgnored.Text = myItem.RawLogData
                    Ignored.ShowDialog(Me)
                End If
            End Using
        End If
    End Sub

    Private Sub CreateReplacementToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CreateReplacementToolStripMenuItem.Click
        Using Replacements As New Replacements With {.StartPosition = FormStartPosition.CenterParent, .Icon = Icon}
            Replacements.TxtReplace.Text = Logs.SelectedRows(0).Cells(ColumnIndex_LogText).Value
            Replacements.ShowDialog(Me)
        End Using
    End Sub

    Private Sub Logs_SelectionChanged(sender As Object, e As EventArgs) Handles Logs.SelectionChanged
        Dim intNumberOfCheckedLogs As Integer = Logs.Rows.Cast(Of DataGridViewRow).Where(Function(row As MyDataGridViewRow) row.Cells(colDelete.Index).Value).Count()

        If intNumberOfCheckedLogs = 0 Then
            LblItemsSelected.Visible = Logs.SelectedRows.Count > 1
            LblItemsSelected.Text = $"Selected Logs: {Logs.SelectedRows.Count:N0}"
        End If
    End Sub

    Private Sub ChkDeselectItemAfterMinimizingWindow_Click(sender As Object, e As EventArgs) Handles ChkDeselectItemAfterMinimizingWindow.Click
        My.Settings.boolDeselectItemsWhenMinimizing = ChkDeselectItemAfterMinimizingWindow.Checked
    End Sub

    Private Sub ConfigureSysLogMirrorServers_Click(sender As Object, e As EventArgs) Handles ConfigureSysLogMirrorServers.Click
        Using ConfigureSysLogMirrorClients As New ConfigureSysLogMirrorClients With {.StartPosition = FormStartPosition.CenterParent, .Icon = Icon}
            ConfigureSysLogMirrorClients.ShowDialog(Me)
            If ConfigureSysLogMirrorClients.boolSuccess Then MsgBox("Done", MsgBoxStyle.Information, Text)
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
                Dim logFileViewer As New IgnoredLogsAndSearchResults(Me, IgnoreOrSearchWindowDisplayMode.viewer) With {.MainProgramForm = Me, .Icon = Icon, .Text = "Log File Viewer", .strFileToLoad = OpenFileDialog.FileName, .boolLoadExternalData = True}
                logFileViewer.ChkColLogsAutoFill.Checked = My.Settings.colLogAutoFill
                logFileViewer.Show(Me)
            End If
        End Using
    End Sub

    Private Sub AutomaticallyCheckForUpdates_Click(sender As Object, e As EventArgs) Handles AutomaticallyCheckForUpdates.Click
        My.Settings.boolCheckForUpdates = AutomaticallyCheckForUpdates.Checked
    End Sub

    Private Sub ReplaceStringsInSysLogDataBeforeProcessingIgnoredRules_Click(sender As Object, e As EventArgs) Handles ProcessReplacementsInSyslogDataFirst.Click
        My.Settings.ProcessReplacementsInSyslogDataFirst = ProcessReplacementsInSyslogDataFirst.Checked
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

        Using taskService As New TaskScheduler.TaskService
            Dim task As TaskScheduler.Task = Nothing

            If TaskHandling.GetTaskObject(taskService, $"Free SysLog for {Environment.UserName}", task) Then
                If task.Definition.Triggers.Any() Then
                    Dim trigger As TaskScheduler.Trigger = task.Definition.Triggers(0)
                    If trigger.TriggerType = TaskScheduler.TaskTriggerType.Logon Then dblSeconds = DirectCast(trigger, TaskScheduler.LogonTrigger).Delay.TotalSeconds
                End If
            End If
        End Using

        Using IntegerInputForm As New IntegerInputForm(1, 300) With {.Icon = Icon, .Text = "Change Startup Delay", .StartPosition = FormStartPosition.CenterParent}
            IntegerInputForm.lblSetting.Text = "Time in Seconds"
            IntegerInputForm.TxtSetting.Text = dblSeconds.ToString

            IntegerInputForm.ShowDialog(Me)

            If IntegerInputForm.DialogResult = DialogResult.OK Then
                If IntegerInputForm.intResult < 1 Or IntegerInputForm.intResult > 300 Then
                    MsgBox("The time in seconds must be in the range of 1 - 300.", MsgBoxStyle.Critical, Text)
                Else
                    Using taskService As New TaskScheduler.TaskService
                        Dim task As TaskScheduler.Task = Nothing

                        If TaskHandling.GetTaskObject(taskService, $"Free SysLog for {Environment.UserName}", task) Then
                            If task.Definition.Triggers.Any() Then
                                Dim trigger As TaskScheduler.Trigger = task.Definition.Triggers(0)

                                If trigger.TriggerType = TaskScheduler.TaskTriggerType.Logon Then
                                    DirectCast(trigger, TaskScheduler.LogonTrigger).Delay = New TimeSpan(0, 0, IntegerInputForm.intResult)
                                    task.RegisterChanges()
                                End If
                            End If
                        End If
                    End Using

                    StartUpDelay.Text = $"        Startup Delay ({IntegerInputForm.intResult} {If(IntegerInputForm.intResult = 1, "Second", "Seconds")})"
                    MsgBox("Done.", MsgBoxStyle.Information, Text)
                End If
            ElseIf IntegerInputForm.DialogResult = DialogResult.Cancel Then
                Using taskService As New TaskScheduler.TaskService
                    Dim task As TaskScheduler.Task = Nothing

                    If TaskHandling.GetTaskObject(taskService, $"Free SysLog for {Environment.UserName}", task) Then
                        If task.Definition.Triggers.Any() Then
                            Dim trigger As TaskScheduler.Trigger = task.Definition.Triggers(0)

                            If trigger.TriggerType = TaskScheduler.TaskTriggerType.Logon Then
                                DirectCast(trigger, TaskScheduler.LogonTrigger).Delay = Nothing
                                task.RegisterChanges()
                            End If
                        End If
                    End If
                End Using

                StartUpDelay.Checked = False
                StartUpDelay.Text = $"        Startup Delay"
                MsgBox("Done.", MsgBoxStyle.Information, Text)
            End If
        End Using
    End Sub

    Private Sub ChkEnableTCPSyslogServer_Click(sender As Object, e As EventArgs) Handles ChkEnableTCPSyslogServer.Click
        My.Settings.EnableTCPServer = ChkEnableTCPSyslogServer.Checked

        If ChkEnableTCPSyslogServer.Checked Then
            StartTCPServer()
        Else
            SendMessageToTCPSysLogServer(strTerminate, My.Settings.sysLogPort)
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

        If boolDoWeOwnTheMutex AndAlso boolServerRunning AndAlso MsgBox("Changing this setting will require a reset of the Syslog Client. Do you want to restart the Syslog Client now?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, Text) = MsgBoxResult.Yes Then
            Threading.ThreadPool.QueueUserWorkItem(Sub()
                                                       SendMessageToSysLogServer(strTerminate, My.Settings.sysLogPort)
                                                       If boolTCPServerRunning Then SendMessageToTCPSysLogServer(strTerminate, My.Settings.sysLogPort)

                                                       Threading.Thread.Sleep(5000)

                                                       serverThread = New Threading.Thread(AddressOf SysLogThread) With {.Name = "UDP Server Thread", .Priority = Threading.ThreadPriority.Normal}
                                                       serverThread.Start()

                                                       If My.Settings.EnableTCPServer Then StartTCPServer()
                                                   End Sub)
        End If
    End Sub

    Private Sub ShowRawLogOnLogViewer_Click(sender As Object, e As EventArgs) Handles ShowRawLogOnLogViewer.Click
        My.Settings.boolShowRawLogOnLogViewer = ShowRawLogOnLogViewer.Checked
    End Sub

    Private Sub LblLogFileSize_Click(sender As Object, e As EventArgs) Handles LblLogFileSize.Click
        SelectFileInWindowsExplorer(strPathToDataFile)
    End Sub

    Private Sub ConfigureHostnames_Click(sender As Object, e As EventArgs) Handles ConfigureHostnames.Click
        Using hostnames As New Hostnames() With {.Icon = Icon}
            hostnames.ShowDialog()
        End Using
    End Sub

    Private Sub ChangeFont_Click(sender As Object, e As EventArgs) Handles ChangeFont.Click
        Using FontDialog As New FontDialog
            If My.Settings.font IsNot Nothing Then FontDialog.Font = My.Settings.font

            If FontDialog.ShowDialog() = DialogResult.OK Then
                My.Settings.font = FontDialog.Font

                Logs.DefaultCellStyle.Font = My.Settings.font
                Logs.ColumnHeadersDefaultCellStyle.Font = My.Settings.font

                DataHandling.WriteLogsToDisk()

                SyncLock dataGridLockObject
                    Threading.Tasks.Task.Run(Sub() LoadDataFile(False))
                End SyncLock
            End If
        End Using
    End Sub

    Private Sub ConfigureTimeBetweenSameNotifications_Click(sender As Object, e As EventArgs) Handles ConfigureTimeBetweenSameNotifications.Click
        Using IntegerInputForm As New IntegerInputForm(30, 240) With {.Icon = Icon, .Text = "Configure Time Between Same Notifications", .StartPosition = FormStartPosition.CenterParent}
            IntegerInputForm.lblSetting.Text = "Time Between Same Notifications (In Seconds)"
            IntegerInputForm.TxtSetting.Text = My.Settings.TimeBetweenSameNotifications
            IntegerInputForm.Width += 60

            IntegerInputForm.ShowDialog(Me)

            If IntegerInputForm.DialogResult = DialogResult.OK Then
                If IntegerInputForm.intResult < 30 Or IntegerInputForm.intResult > 240 Then
                    MsgBox("The time in seconds must be in the range of 30 - 240.", MsgBoxStyle.Critical, Text)
                Else
                    ConfigureTimeBetweenSameNotifications.Text = $"Configure Time Between Same Notifications ({IntegerInputForm.intResult} Seconds or {FormatSecondsToReadableTime(IntegerInputForm.intResult)})"

                    My.Settings.TimeBetweenSameNotifications = IntegerInputForm.intResult
                    My.Settings.Save()

                    MsgBox("Done.", MsgBoxStyle.Information, Text)
                End If
            End If
        End Using
    End Sub

    Private Sub IncludeButtonsOnNotifications_Click(sender As Object, e As EventArgs) Handles IncludeButtonsOnNotifications.Click
        My.Settings.IncludeButtonsOnNotifications = IncludeButtonsOnNotifications.Checked
    End Sub

    Private Sub NotificationLengthShort_Click(sender As Object, e As EventArgs) Handles NotificationLengthShort.Click
        NotificationLengthLong.Checked = False
        My.Settings.NotificationLength = 0
    End Sub

    Private Sub NotificationLengthLong_Click(sender As Object, e As EventArgs) Handles NotificationLengthLong.Click
        NotificationLengthShort.Checked = False
        My.Settings.NotificationLength = 1
    End Sub

    Private Sub AlertsHistory_Click(sender As Object, e As EventArgs) Handles AlertsHistory.Click
        If Logs.Rows.Count > 0 Then
            Dim DataToLoad As New List(Of AlertsHistory)

            SyncLock dataGridLockObject
                For Each item As MyDataGridViewRow In Logs.Rows
                    If item.BoolAlerted Then
                        DataToLoad.Add(New AlertsHistory With {
                                 .strTime = item.Cells(ColumnIndex_ComputedTime).Value,
                                 .alertType = item.alertType,
                                 .strAlertText = item.AlertText,
                                 .strIP = item.Cells(ColumnIndex_IPAddress).Value,
                                 .strLog = item.Cells(ColumnIndex_LogText).Value,
                                 .strRawLog = item.RawLogData
                                })
                    End If
                Next
            End SyncLock

            If DataToLoad.Count = 0 Then
                MsgBox("There are no alerts to show in the Alerts History.", MsgBoxStyle.Information, Text)
            Else
                Using Alerts_History As New Alerts_History() With {.Icon = Icon, .DataToLoad = DataToLoad, .StartPosition = FormStartPosition.CenterParent, .SetParentForm = Me}
                    Alerts_History.ShowDialog(Me)
                End Using
            End If
        End If
    End Sub

    Private Sub ColLogsAutoFill_Click(sender As Object, e As EventArgs) Handles ColLogsAutoFill.Click
        My.Settings.colLogAutoFill = ColLogsAutoFill.Checked
        ColLog.AutoSizeMode = If(My.Settings.colLogAutoFill, DataGridViewAutoSizeColumnMode.Fill, DataGridViewAutoSizeColumnMode.NotSet)
    End Sub

    Private Sub Logs_Scroll(sender As Object, e As ScrollEventArgs) Handles Logs.Scroll
        If ChkDisableAutoScrollUponScrolling.Checked AndAlso Not boolIsProgrammaticScroll AndAlso ChkEnableAutoScroll.Checked AndAlso e.NewValue < e.OldValue Then
            My.Settings.autoScroll = False
            ChkEnableAutoScroll.Checked = False
            LblAutoScrollStatus.Text = "Auto Scroll Status: Disabled"
        End If

        If Logs.FirstDisplayedScrollingRowIndex <> lastFirstDisplayedRowIndex Then
            ' The visible area has changed, handle the change
            lastFirstDisplayedRowIndex = Logs.FirstDisplayedScrollingRowIndex
            ' Your custom logic when the first displayed row changes
            boolIsProgrammaticScroll = False
        End If
    End Sub

    Private Sub ChkDisableAutoScrollUponScrolling_Click(sender As Object, e As EventArgs) Handles ChkDisableAutoScrollUponScrolling.Click
        My.Settings.disableAutoScrollUponScrolling = ChkDisableAutoScrollUponScrolling.Checked
    End Sub

    Private Sub ClearNotificationLimits_Click(sender As Object, e As EventArgs) Handles ClearNotificationLimits.Click
        If NotificationLimiter.lastNotificationTime IsNot Nothing AndAlso NotificationLimiter.lastNotificationTime.Any() Then
            NotificationLimiter.lastNotificationTime.Clear()
            MsgBox("Done.", MsgBoxStyle.Information, Text)
        End If
    End Sub

    Private Sub ReOpenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ReOpenToolStripMenuItem.Click
        RestoreWindow()
    End Sub

    Private Sub ChkDebug_Click(sender As Object, e As EventArgs) Handles ChkDebug.Click
        My.Settings.boolDebug = ChkDebug.Checked
    End Sub

    Private Sub ConfirmDelete_Click(sender As Object, e As EventArgs) Handles ConfirmDelete.Click
        My.Settings.ConfirmDelete = ConfirmDelete.Checked
    End Sub

    Private Sub LogFunctionsToolStripMenuItem_DropDownOpening(sender As Object, e As EventArgs) Handles LogFunctionsToolStripMenuItem.DropDownOpening
        SyncLock dataGridLockObject
            AlertsHistory.Enabled = Logs.Rows.Cast(Of MyDataGridViewRow).Any(Function(row As MyDataGridViewRow) Not String.IsNullOrWhiteSpace(row.AlertText))
        End SyncLock
    End Sub

    Private Sub DeleteSimilarLogsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteSimilarLogsToolStripMenuItem.Click
        Dim hitTest As DataGridView.HitTestInfo = Logs.HitTest(Logs.PointToClient(MousePosition).X, Logs.PointToClient(MousePosition).Y)

        If hitTest.Type = DataGridViewHitTestType.Cell And hitTest.RowIndex <> -1 And Logs.Rows.Count > 0 And Logs.SelectedCells.Count > 0 Then
            Dim selectedRow As MyDataGridViewRow = Logs.Rows(Logs.SelectedCells(0).RowIndex)
            Dim strLogText As String = selectedRow.Cells(ColumnIndex_LogText).Value
            Dim strCellValue As String
            Dim item As MyDataGridViewRow

            ' Create a list to store rows that need to be removed
            Dim rowsToDelete As New List(Of MyDataGridViewRow)

            ' Synchronize both the iteration and the removal
            SyncLock dataGridLockObject
                ' Loop through the rows in reverse order to avoid index shifting
                For i As Integer = Logs.Rows.Count - 1 To 0 Step -1
                    item = Logs.Rows(i)
                    strCellValue = item.Cells(ColumnIndex_LogText).Value

                    If Not String.IsNullOrWhiteSpace(strCellValue) AndAlso strCellValue.Equals(strLogText, StringComparison.OrdinalIgnoreCase) Then
                        rowsToDelete.Add(item)
                    End If
                Next

                ' Remove the rows outside the loop
                For Each row As MyDataGridViewRow In rowsToDelete
                    Logs.Rows.Remove(row)
                Next

                Logs.Rows.Add(SyslogParser.MakeLocalDataGridRowEntry($"The program deleted {rowsToDelete.Count:N0} similar log {If(rowsToDelete.Count = 1, "entry", "entries")}.", Logs))
            End SyncLock

            ' Update log count and save changes to disk
            UpdateLogCount()
            SaveLogsToDiskSub()
        End If
    End Sub

    Private Sub btnShowLimit_Click(sender As Object, e As EventArgs) Handles btnShowLimit.Click
        Dim strLimitBy As String = boxLimitBy.Text
        Dim strLimiter As String = boxLimiter.Text
        Dim listOfSearchResults As New List(Of MyDataGridViewRow)
        Dim stopWatch As Stopwatch = Stopwatch.StartNew
        Dim worker As New BackgroundWorker()

        AddHandler worker.DoWork, Sub()
                                      Threading.Tasks.Parallel.ForEach(Logs.Rows.Cast(Of MyDataGridViewRow), Sub(item As MyDataGridViewRow)
                                                                                                                 If strLimitBy.Equals("Log Type", StringComparison.OrdinalIgnoreCase) AndAlso String.Equals(item.Cells(ColumnIndex_LogType).Value, strLimiter, StringComparison.OrdinalIgnoreCase) Then
                                                                                                                     SyncLock listOfSearchResults
                                                                                                                         listOfSearchResults.Add(item.Clone)
                                                                                                                     End SyncLock
                                                                                                                 ElseIf strLimitBy.Equals("Remote Process", StringComparison.OrdinalIgnoreCase) AndAlso String.Equals(item.Cells(ColumnIndex_RemoteProcess).Value, strLimiter, StringComparison.OrdinalIgnoreCase) Then
                                                                                                                     SyncLock listOfSearchResults
                                                                                                                         listOfSearchResults.Add(item.Clone)
                                                                                                                     End SyncLock
                                                                                                                 ElseIf strLimitBy.Equals("Source Hostname", StringComparison.OrdinalIgnoreCase) AndAlso String.Equals(item.Cells(ColumnIndex_Hostname).Value, strLimiter, StringComparison.OrdinalIgnoreCase) Then
                                                                                                                     SyncLock listOfSearchResults
                                                                                                                         listOfSearchResults.Add(item.Clone)
                                                                                                                     End SyncLock
                                                                                                                 ElseIf strLimitBy.Equals("Source IP Address", StringComparison.OrdinalIgnoreCase) AndAlso String.Equals(item.Cells(ColumnIndex_IPAddress).Value, strLimiter, StringComparison.OrdinalIgnoreCase) Then
                                                                                                                     SyncLock listOfSearchResults
                                                                                                                         listOfSearchResults.Add(item.Clone)
                                                                                                                     End SyncLock
                                                                                                                 End If
                                                                                                             End Sub)
                                  End Sub



        AddHandler worker.RunWorkerCompleted, Sub()
                                                  If listOfSearchResults.Any() Then
                                                      Dim searchResultsWindow As New IgnoredLogsAndSearchResults(Me, IgnoreOrSearchWindowDisplayMode.search) With {.MainProgramForm = Me, .Icon = Icon, .LogsToBeDisplayed = listOfSearchResults, .Text = "Search Results"}
                                                      searchResultsWindow.LogsLoadedInLabel.Visible = True
                                                      searchResultsWindow.LogsLoadedInLabel.Text = $"Search took {MyRoundingFunction(stopWatch.Elapsed.TotalMilliseconds / 1000, 2)} seconds"
                                                      searchResultsWindow.ChkColLogsAutoFill.Checked = My.Settings.colLogAutoFill
                                                      searchResultsWindow.ShowDialog(Me)
                                                  Else
                                                      MsgBox("No logs match the limit you set.", MsgBoxStyle.Information, Text)
                                                  End If

                                                  Invoke(Sub() BtnSearch.Enabled = True)
                                              End Sub

        worker.RunWorkerAsync()
    End Sub

    Private Sub LimitNumberOfIgnoredLogs_Click(sender As Object, e As EventArgs) Handles LimitNumberOfIgnoredLogs.Click
        Using IntegerInputForm As New IntegerInputForm(1, 2000) With {.Icon = Icon, .Text = "Limit Number of Ignored Logs", .StartPosition = FormStartPosition.CenterParent}
            IntegerInputForm.lblSetting.Text = "Limit Number of Ignored Logs"
            IntegerInputForm.TxtSetting.Text = My.Settings.LimitNumberOfIgnoredLogs

            IntegerInputForm.ShowDialog(Me)

            If IntegerInputForm.DialogResult = DialogResult.OK Then
                My.Settings.LimitNumberOfIgnoredLogs = IntegerInputForm.intResult
                LimitNumberOfIgnoredLogs.Text = $"Limit Number of Ignored Logs ({My.Settings.LimitNumberOfIgnoredLogs:N0})"

                MsgBox("Done.", MsgBoxStyle.Information, Text)
            End If
        End Using
    End Sub

    Private Sub AskToOpenExplorerWhenSavingData_Click(sender As Object, e As EventArgs) Handles AskToOpenExplorerWhenSavingData.Click
        My.Settings.AskOpenExplorer = AskToOpenExplorerWhenSavingData.Checked
    End Sub

    Private Sub SaveIgnoredLogCount_Click(sender As Object, e As EventArgs) Handles SaveIgnoredLogCount.Click
        My.Settings.saveIgnoredLogCount = SaveIgnoredLogCount.Checked
    End Sub

    Private Sub CopyRawLogTextToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyRawLogTextToolStripMenuItem.Click
        Dim intLogCount As Integer = Logs.SelectedRows().Count
        Dim boolCopyToClipboardResults As Boolean = False

        If intLogCount <> 0 Then
            If intLogCount = 1 Then
                Dim selectedItem As MyDataGridViewRow = TryCast(Logs.SelectedRows(0), MyDataGridViewRow)
                If selectedItem IsNot Nothing Then boolCopyToClipboardResults = CopyTextToWindowsClipboard(selectedItem.RawLogData, Text)
            Else
                Dim allSelectedLogs As New StringBuilder()
                Dim selectedItem As MyDataGridViewRow

                For Each item As DataGridViewRow In Logs.SelectedRows
                    selectedItem = TryCast(item, MyDataGridViewRow)
                    If selectedItem IsNot Nothing Then allSelectedLogs.AppendLine(selectedItem.RawLogData)
                Next

                If allSelectedLogs.Length > 0 Then boolCopyToClipboardResults = CopyTextToWindowsClipboard(allSelectedLogs.ToString().Trim, Text)
            End If

            If boolCopyToClipboardResults Then MsgBox("Data copied to clipboard.", MsgBoxStyle.Information, Text)
        End If
    End Sub

    Private Sub ShowCloseButtonOnNotifications_Click(sender As Object, e As EventArgs) Handles ShowCloseButtonOnNotifications.Click
        My.Settings.ShowCloseButtonOnNotifications = ShowCloseButtonOnNotifications.Checked
    End Sub

#Region "-- SysLog Server Code --"
    Sub SysLogThread()
        Try
            ' These are initialized as IPv4 mode.
            Dim addressFamilySetting As AddressFamily = AddressFamily.InterNetwork
            Dim ipAddressSetting As IPAddress = IPAddress.Any

            If My.Settings.IPv6Support Then
                addressFamilySetting = AddressFamily.InterNetworkV6
                ipAddressSetting = IPAddress.IPv6Any
            End If

            Using socket As New Socket(addressFamilySetting, SocketType.Dgram, ProtocolType.Udp)
                If My.Settings.IPv6Support Then socket.DualMode = True
                socket.Bind(New IPEndPoint(ipAddressSetting, My.Settings.sysLogPort))

                Dim boolDoServerLoop As Boolean = True
                Dim buffer(4095) As Byte
                Dim remoteEndPoint As EndPoint = New IPEndPoint(ipAddressSetting, 0)
                Dim ProxiedSysLogData As ProxiedSysLogData
                Dim bytesReceived As Integer
                Dim strReceivedData, strSourceIP As String

                While boolDoServerLoop
                    bytesReceived = socket.ReceiveFrom(buffer, remoteEndPoint)
                    strReceivedData = Encoding.UTF8.GetString(buffer, 0, bytesReceived)
                    strSourceIP = GetIPv4Address(CType(remoteEndPoint, IPEndPoint).Address).ToString()

                    If strReceivedData.Trim.Equals(strRestore, StringComparison.OrdinalIgnoreCase) Then
                        Invoke(Sub() RestoreWindowAfterReceivingRestoreCommand())
                    ElseIf strReceivedData.Trim.Equals(strTerminate, StringComparison.OrdinalIgnoreCase) Then
                        boolDoServerLoop = False
                    ElseIf strReceivedData.Trim.StartsWith(strProxiedString, StringComparison.OrdinalIgnoreCase) Then
                        Try
                            strReceivedData = strReceivedData.Replace(strProxiedString, "", StringComparison.OrdinalIgnoreCase)
                            ProxiedSysLogData = Newtonsoft.Json.JsonConvert.DeserializeObject(Of ProxiedSysLogData)(strReceivedData, JSONDecoderSettingsForLogFiles)
                            SyslogParser.ProcessIncomingLog(ProxiedSysLogData.log, ProxiedSysLogData.ip)
                        Catch ex As Newtonsoft.Json.JsonSerializationException
                        End Try
                    Else
                        If serversList IsNot Nothing AndAlso serversList.Any() Then
                            Threading.ThreadPool.QueueUserWorkItem(Sub()
                                                                       ProxiedSysLogData = New ProxiedSysLogData() With {.ip = strSourceIP, .log = strReceivedData}
                                                                       Dim strDataToSend As String = strProxiedString & Newtonsoft.Json.JsonConvert.SerializeObject(ProxiedSysLogData)

                                                                       For Each item As SysLogProxyServer In serversList
                                                                           SendMessageToSysLogServer(strDataToSend, item.ip, item.port)
                                                                       Next

                                                                       ProxiedSysLogData = Nothing
                                                                       strDataToSend = Nothing
                                                                   End Sub)
                        End If

                        SyslogParser.ProcessIncomingLog(strReceivedData, strSourceIP)
                    End If

                    strReceivedData = Nothing
                    strSourceIP = Nothing
                    bytesReceived = Nothing
                End While
            End Using
        Catch ex As Threading.ThreadAbortException
            ' Does nothing
        Catch e As Exception
            Invoke(Sub()
                       Dim process As Process = GetProcessUsingPort(My.Settings.sysLogPort, ProtocolType.Udp)

                       If process Is Nothing Then
                           MsgBox("Unable to start syslog server, perhaps another instance of this program is running on your system.", MsgBoxStyle.Critical + MsgBoxStyle.ApplicationModal, Text)
                       Else
                           Dim strLogText As String = $"Unable to start syslog server. A process with a PID of {process.Id} already has the port open."

                           SyncLock dataGridLockObject
                               Logs.Rows.Add(SyslogParser.MakeLocalDataGridRowEntry($"Exception Type: {e.GetType}{vbCrLf}Exception Message: {e.Message}{vbCrLf}{vbCrLf}Exception Stack Trace{vbCrLf}{e.StackTrace}", Logs))
                               Logs.Rows.Add(SyslogParser.MakeLocalDataGridRowEntry(strLogText, Logs))
                               SelectLatestLogEntry()
                               UpdateLogCount()
                           End SyncLock

                           MsgBox(strLogText, MsgBoxStyle.Critical + MsgBoxStyle.ApplicationModal, Text)
                       End If
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

        If boolDebugBuild Or ChkDebug.Checked Then
            Logs.Rows.Add(SyslogParser.MakeLocalDataGridRowEntry("Restore command received.", Logs))
            SelectLatestLogEntry()
            UpdateLogCount()
            BtnSaveLogsToDisk.Enabled = True
        End If

        SelectLatestLogEntry()
    End Sub
#End Region
End Class