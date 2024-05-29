Imports System.IO
Imports System.Net.Sockets
Imports System.Net
Imports System.Text
Imports System.ComponentModel
Imports Microsoft.Win32
Imports System.Text.RegularExpressions
Imports System.Reflection
Imports System.Configuration

Public Class Form1
    Private boolMaximizedBeforeMinimize As Boolean
    Private boolDoneLoading As Boolean = False
    Private lockObject As New Object
    Private longNumberOfIgnoredLogs As Long = 0
    Private IgnoredLogs As New List(Of MyDataGridViewRow)
    Private regexCache As New Dictionary(Of String, Regex)
    Private intSortColumnIndex As Integer = 0 ' Define intColumnNumber at class level
    Private sortOrder As SortOrder = SortOrder.Ascending ' Define soSortOrder at class level
    Private ReadOnly dataGridLockObject As New Object
    Private ReadOnly IgnoredLogsLockObject As New Object
    Private Const strPayPal As String = "https://paypal.me/trparky"
    Private serverThread As Threading.Thread

    Private Function MakeDataGridRow(dateObject As Date, strTime As String, strSourceAddress As String, strLog As String, boolAlerted As Boolean, ByRef dataGrid As DataGridView) As MyDataGridViewRow
        Dim MyDataGridViewRow As New MyDataGridViewRow

        With MyDataGridViewRow
            .CreateCells(dataGrid)
            .Cells(0).Value = strTime
            .Cells(0).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Cells(1).Value = strSourceAddress
            .Cells(1).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Cells(2).Value = strLog
            .Cells(3).Value = If(boolAlerted, "Yes", "No")
            .Cells(3).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .DateObject = dateObject
            .BoolAlerted = boolAlerted
        End With

        Return MyDataGridViewRow
    End Function

    Private Sub ChkStartAtUserStartup_Click(sender As Object, e As EventArgs) Handles ChkEnableStartAtUserStartup.Click
        Using registryKey As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Run", True)
            If ChkEnableStartAtUserStartup.Checked Then
                registryKey.SetValue("Free Syslog", $"""{strEXEPath}"" /background")
            Else
                registryKey.DeleteValue("Free Syslog", False)
            End If
        End Using
    End Sub

    Private Function DoesStartupEntryExist() As Boolean
        Using registryKey As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Run", False)
            Return registryKey.GetValue("Free Syslog") IsNot Nothing
        End Using
    End Function

    Private Sub BtnMoveLogFile_Click(sender As Object, e As EventArgs) Handles BtnMoveLogFile.Click
        SyncLock lockObject
            Using SaveFileDialog As New SaveFileDialog()
                SaveFileDialog.Filter = "JSON Data File|*.json"

                Do
                    If SaveFileDialog.ShowDialog() = DialogResult.OK Then
                        If File.Exists(SaveFileDialog.FileName) Then File.Delete(SaveFileDialog.FileName)
                        File.Move(My.Settings.logFileLocation, SaveFileDialog.FileName)
                        My.Settings.logFileLocation = SaveFileDialog.FileName
                        My.Settings.Save()
                        Exit Do
                    End If
                Loop While True
            End Using
        End SyncLock
    End Sub

    Private Sub WriteLogsToDisk()
        SyncLock lockObject
            Dim collectionOfSavedData As New List(Of SavedData)
            Dim myItem As MyDataGridViewRow

            SyncLock dataGridLockObject
                For Each item As DataGridViewRow In Logs.Rows
                    If Not String.IsNullOrWhiteSpace(item.Cells(0).Value) Then
                        myItem = DirectCast(item, MyDataGridViewRow)

                        collectionOfSavedData.Add(New SavedData With {
                                            .time = myItem.Cells(0).Value,
                                            .ip = myItem.Cells(1).Value,
                                            .log = myItem.Cells(2).Value,
                                            .DateObject = myItem.DateObject,
                                            .BoolAlerted = myItem.BoolAlerted
                                          })
                    End If
                Next
            End SyncLock

            Try
                Using fileStream As New StreamWriter(My.Settings.logFileLocation & ".new")
                    fileStream.Write(Newtonsoft.Json.JsonConvert.SerializeObject(collectionOfSavedData))
                End Using

                File.Delete(My.Settings.logFileLocation)
                File.Move(My.Settings.logFileLocation & ".new", My.Settings.logFileLocation)
            Catch ex As Exception
                MsgBox("A critical error occurred while writing log data to disk. The old data had been saved to prevent data corruption.", MsgBoxStyle.Critical, Text)
                Process.GetCurrentProcess.Kill()
            End Try

            LblLogFileSize.Text = $"Log File Size: {FileSizeToHumanSize(New FileInfo(My.Settings.logFileLocation).Length)}"

            BtnSaveLogsToDisk.Enabled = False
        End SyncLock
    End Sub

    Private Sub Form1_ResizeEnd(sender As Object, e As EventArgs) Handles Me.ResizeEnd
        My.Settings.mainWindowSize = Size
        Threading.Thread.Sleep(100)

        If ChkEnableAutoScroll.Checked And Logs.Rows.Count > 0 And intSortColumnIndex = 0 Then
            Logs.FirstDisplayedScrollingRowIndex = If(sortOrder = SortOrder.Ascending, Logs.Rows.Count - 1, 0)
        End If
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
            ShowInTaskbar = WindowState <> FormWindowState.Minimized
        End If
    End Sub

    Private Sub ChkAutoSave_Click(sender As Object, e As EventArgs) Handles ChkEnableAutoSave.Click
        SaveTimer.Enabled = ChkEnableAutoSave.Checked
        ChangeLogAutosaveIntervalToolStripMenuItem.Visible = ChkEnableAutoSave.Checked
        LblAutoSaved.Visible = ChkEnableAutoSave.Checked
    End Sub

    Private Sub SaveTimer_Tick(sender As Object, e As EventArgs) Handles SaveTimer.Tick
        WriteLogsToDisk()
        LblAutoSaved.Text = $"Last Auto-Saved At: {Date.Now:h:mm:ss tt}"
    End Sub

    Private Function FileSizeToHumanSize(size As Long, Optional roundToNearestWholeNumber As Boolean = False) As String
        Dim result As String
        Dim shortRoundNumber As Short = If(roundToNearestWholeNumber, 0, 2)

        If size <= (2 ^ 10) Then
            result = $"{size} Bytes"
        ElseIf size > (2 ^ 10) And size <= (2 ^ 20) Then
            result = $"{MyRoundingFunction(size / (2 ^ 10), shortRoundNumber)} KBs"
        ElseIf size > (2 ^ 20) And size <= (2 ^ 30) Then
            result = $"{MyRoundingFunction(size / (2 ^ 20), shortRoundNumber)} MBs"
        ElseIf size > (2 ^ 30) And size <= (2 ^ 40) Then
            result = $"{MyRoundingFunction(size / (2 ^ 30), shortRoundNumber)} GBs"
        ElseIf size > (2 ^ 40) And size <= (2 ^ 50) Then
            result = $"{MyRoundingFunction(size / (2 ^ 40), shortRoundNumber)} TBs"
        ElseIf size > (2 ^ 50) And size <= (2 ^ 60) Then
            result = $"{MyRoundingFunction(size / (2 ^ 50), shortRoundNumber)} PBs"
        ElseIf size > (2 ^ 60) And size <= (2 ^ 70) Then
            result = $"{MyRoundingFunction(size / (2 ^ 50), shortRoundNumber)} EBs"
        Else
            result = "(None)"
        End If

        Return result
    End Function

    Private Function MyRoundingFunction(value As Double, digits As Integer) As String
        If digits < 0 Then Throw New ArgumentException("The number of digits must be non-negative.", NameOf(digits))

        If digits = 0 Then
            Return Math.Round(value, digits).ToString
        Else
            Return Math.Round(value, digits).ToString("0." & New String("0", digits))
        End If
    End Function

    Private Function ProcessReplacements(input As String) As String
        For Each item As ReplacementsClass In replacementsList
            Try
                input = GetCachedRegex(If(item.BoolRegex, item.StrReplace, Regex.Escape(item.StrReplace)), item.BoolCaseSensitive).Replace(input, item.StrReplaceWith)
            Catch ex As Exception
            End Try
        Next

        Return input
    End Function

    Public Sub RestoreWindow()
        If boolMaximizedBeforeMinimize Then
            WindowState = FormWindowState.Maximized
        ElseIf My.Settings.boolMaximized Then
            WindowState = FormWindowState.Maximized
        Else
            WindowState = FormWindowState.Normal
        End If

        If ChkEnableAutoScroll.Checked And Logs.Rows.Count > 0 And intSortColumnIndex = 0 Then
            Logs.FirstDisplayedScrollingRowIndex = If(sortOrder = SortOrder.Ascending, Logs.Rows.Count - 1, 0)
        End If
    End Sub

    Private Sub NotifyIcon_DoubleClick(sender As Object, e As EventArgs) Handles NotifyIcon.DoubleClick
        RestoreWindow()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ChangeLogAutosaveIntervalToolStripMenuItem.Text = $"Change Log Autosave Interval ({My.Settings.autoSaveMinutes} Minutes)"
        ChangeSyslogServerPortToolStripMenuItem.Text = $"Change Syslog Server Port (Port Number {My.Settings.sysLogPort})"

        ColTime.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
        ColTime.HeaderCell.Style.Padding = New Padding(0, 0, 1, 0)
        ColIPAddress.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
        ColIPAddress.HeaderCell.Style.Padding = New Padding(0, 0, 2, 0)

        ColTime.HeaderCell.SortGlyphDirection = SortOrder.Ascending

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
        StopServerStripMenuItem.Visible = boolDoWeOwnTheMutex
        ChkEnableStartAtUserStartup.Checked = DoesStartupEntryExist()
        Icon = Icon.ExtractAssociatedIcon(strEXEPath)
        Location = VerifyWindowLocation(My.Settings.windowLocation, Me)
        If My.Settings.boolMaximized Then WindowState = FormWindowState.Maximized
        NotifyIcon.Icon = Icon
        NotifyIcon.Text = "Free SysLog"

        If My.Application.CommandLineArgs.Count > 0 AndAlso My.Application.CommandLineArgs(0).Trim.Equals("/background", StringComparison.OrdinalIgnoreCase) Then
            WindowState = FormWindowState.Minimized
            ShowInTaskbar = False
        End If

        Dim flags As BindingFlags = BindingFlags.NonPublic Or BindingFlags.Instance Or BindingFlags.SetProperty
        Dim propInfo As PropertyInfo = GetType(DataGridView).GetProperty("DoubleBuffered", flags)
        propInfo?.SetValue(Logs, True, Nothing)

        Dim rowStyle As New DataGridViewCellStyle() With {.BackColor = My.Settings.searchColor}
        Logs.AlternatingRowsDefaultCellStyle = rowStyle

        Dim tempReplacementsClass As ReplacementsClass
        Dim tempSysLogProxyServer As SysLogProxyServer
        Dim tempIgnoredClass As IgnoredClass
        Dim tempAlertsClass As AlertsClass

        If My.Settings.replacements IsNot Nothing AndAlso My.Settings.replacements.Count > 0 Then
            For Each strJSONString As String In My.Settings.replacements
                tempReplacementsClass = Newtonsoft.Json.JsonConvert.DeserializeObject(Of ReplacementsClass)(strJSONString)
                If tempReplacementsClass.BoolEnabled Then replacementsList.Add(tempReplacementsClass)
                tempReplacementsClass = Nothing
            Next
        End If

        If My.Settings.ServersToSendTo IsNot Nothing AndAlso My.Settings.ServersToSendTo.Count > 0 Then
            For Each strJSONString As String In My.Settings.ServersToSendTo
                tempSysLogProxyServer = Newtonsoft.Json.JsonConvert.DeserializeObject(Of SysLogProxyServer)(strJSONString)
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
                tempIgnoredClass = Newtonsoft.Json.JsonConvert.DeserializeObject(Of IgnoredClass)(strJSONString)
                If tempIgnoredClass.BoolEnabled Then ignoredList.Add(tempIgnoredClass)
                tempIgnoredClass = Nothing
            Next
        End If

        If My.Settings.alerts IsNot Nothing AndAlso My.Settings.alerts.Count > 0 Then
            For Each strJSONString As String In My.Settings.alerts
                tempAlertsClass = Newtonsoft.Json.JsonConvert.DeserializeObject(Of AlertsClass)(strJSONString)
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
        ColIPAddress.Width = My.Settings.columnIPSize
        ColLog.Width = My.Settings.columnLogSize

        boolDoneLoading = True

        If String.IsNullOrWhiteSpace(My.Settings.logFileLocation) Then
            Using SaveFileDialog As New SaveFileDialog()
                SaveFileDialog.Filter = "JSON Data File|*.json"

                Do
                    If SaveFileDialog.ShowDialog() = DialogResult.OK Then
                        My.Settings.logFileLocation = SaveFileDialog.FileName
                        My.Settings.Save()
                        Exit Do
                    Else
                        If MsgBox("You must set a location to save the syslog data file to. Do you want to continue?", MsgBoxStyle.Question Or MsgBoxStyle.YesNo + MsgBoxStyle.SystemModal, Text) = MsgBoxResult.No Then Process.GetCurrentProcess.Kill()
                    End If
                Loop While True
            End Using
        End If

        Dim worker As New BackgroundWorker()
        AddHandler worker.DoWork, AddressOf LoadDataFile
        AddHandler worker.RunWorkerCompleted, AddressOf RunWorkerCompleted
        worker.RunWorkerAsync()
    End Sub

    Private Sub RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs)
        serverThread = New Threading.Thread(AddressOf SysLogThread) With {.Name = "UDP Server Thread", .Priority = Threading.ThreadPriority.Normal}
        serverThread.Start()
    End Sub

    Private Sub LoadDataFile(sender As Object, e As DoWorkEventArgs)
        If File.Exists(My.Settings.logFileLocation) Then
            Try
                Invoke(Sub()
                           Logs.Rows.Add(MakeDataGridRow(Now, Nothing, Nothing, "Loading data and populating data grid... Please Wait.", False, Logs))
                           LblLogFileSize.Text = $"Log File Size: {FileSizeToHumanSize(New FileInfo(My.Settings.logFileLocation).Length)}"
                       End Sub)

                Dim collectionOfSavedData As New List(Of SavedData)

                Using fileStream As New StreamReader(My.Settings.logFileLocation)
                    collectionOfSavedData = Newtonsoft.Json.JsonConvert.DeserializeObject(Of List(Of SavedData))(fileStream.ReadToEnd.Trim, JSONDecoderSettings)
                End Using

                Dim listOfLogEntries As New List(Of MyDataGridViewRow)

                For Each item As SavedData In collectionOfSavedData
                    listOfLogEntries.Add(item.MakeDataGridRow(Logs))
                Next

                listOfLogEntries.Add(MakeDataGridRow(Now, Now.ToString, IPAddress.Loopback.ToString, "Free SysLog Server Started.", False, Logs))

                SyncLock dataGridLockObject
                    Invoke(Sub()
                               Logs.Rows.Clear()
                               Logs.Rows.AddRange(listOfLogEntries.ToArray)

                               If ChkEnableAutoScroll.Checked And Logs.Rows.Count > 0 And intSortColumnIndex = 0 Then
                                   Logs.FirstDisplayedScrollingRowIndex = If(sortOrder = SortOrder.Ascending, Logs.Rows.Count - 1, 0)
                               End If

                               Logs.SelectedRows(0).Selected = False
                               UpdateLogCount()
                           End Sub)
                End SyncLock
            Catch ex As Newtonsoft.Json.JsonSerializationException
                File.Copy(My.Settings.logFileLocation, $"{My.Settings.logFileLocation}.bad", True)
                File.WriteAllText(My.Settings.logFileLocation, "{}")
                LblLogFileSize.Text = $"Log File Size: {FileSizeToHumanSize(New FileInfo(My.Settings.logFileLocation).Length)}"

                SyncLock dataGridLockObject
                    Invoke(Sub()
                               Logs.Rows.Clear()

                               Dim listOfLogEntries As New List(Of MyDataGridViewRow) From {
                                   MakeDataGridRow(Now, Now.ToString, IPAddress.Loopback.ToString, "Free SysLog Server Started.", False, Logs),
                                   MakeDataGridRow(Now, Now.ToString, IPAddress.Loopback.ToString, "There was an error while decoing the JSON data, existing data was copied to another file and the log file was reset.", False, Logs)
                               }

                               Logs.Rows.AddRange(listOfLogEntries.ToArray)
                               UpdateLogCount()
                           End Sub)
                End SyncLock
            End Try
        End If
    End Sub

    Private Sub BtnOpenLogLocation_Click(sender As Object, e As EventArgs) Handles BtnOpenLogLocation.Click
        SelectFileInWindowsExplorer(My.Settings.logFileLocation)
    End Sub

    Private Function GetCachedRegex(pattern As String, Optional boolCaseInsensitive As Boolean = True) As Regex
        If Not regexCache.ContainsKey(pattern) Then regexCache(pattern) = New Regex(pattern, If(boolCaseInsensitive, RegexOptions.Compiled Or RegexOptions.IgnoreCase, RegexOptions.Compiled))
        Return regexCache(pattern)
    End Function

    Private Sub ProcessIncomingLog(strLogText As String, strSourceIP As String)
        Try
            If Not String.IsNullOrWhiteSpace(strLogText) And Not String.IsNullOrWhiteSpace(strSourceIP) Then
                Dim boolIgnored As Boolean = False

                strLogText = strLogText.Replace(vbCr, vbCrLf) ' Converts from UNIX to DOS/Windows.
                strLogText = strLogText.Replace(vbCrLf, Nothing)
                strLogText = Mid(strLogText, InStr(strLogText, ">") + 1, Len(strLogText))
                strLogText = strLogText.Trim

                If ignoredList IsNot Nothing AndAlso ignoredList.Count > 0 Then
                    For Each ignoredClassInstance As IgnoredClass In ignoredList
                        If GetCachedRegex(If(ignoredClassInstance.BoolRegex, ignoredClassInstance.StrIgnore, Regex.Escape(ignoredClassInstance.StrIgnore)), ignoredClassInstance.BoolCaseSensitive).IsMatch(strLogText) Then
                            Invoke(Sub()
                                       longNumberOfIgnoredLogs += 1

                                       If Not ChkEnableRecordingOfIgnoredLogs.Checked Then
                                           ZerooutIgnoredLogsCounterToolStripMenuItem.Enabled = True
                                       End If

                                       LblNumberOfIgnoredIncomingLogs.Text = $"Number of ignored incoming logs: {longNumberOfIgnoredLogs:N0}"
                                   End Sub)

                            boolIgnored = True
                            Exit For
                        End If
                    Next
                End If

                Dim boolAlerted As Boolean = False

                If replacementsList IsNot Nothing AndAlso replacementsList.Count > 0 Then strLogText = ProcessReplacements(strLogText)
                If alertsList IsNot Nothing AndAlso alertsList.Count > 0 Then boolAlerted = ProcessAlerts(strLogText)

                AddToLogList(strSourceIP, strLogText, boolIgnored, boolAlerted)
            End If
        Catch ex As Exception
            AddToLogList("local", $"{ex.Message} -- {ex.StackTrace}", False, False)
        End Try
    End Sub

    Private Function ProcessAlerts(strLogText As String) As Boolean
        Dim ToolTipIcon As ToolTipIcon = ToolTipIcon.None
        Dim RegExObject As Regex
        Dim strAlertText As String
        Dim regExGroupCollection As GroupCollection

        For Each alert As AlertsClass In alertsList
            RegExObject = GetCachedRegex(If(alert.BoolRegex, alert.StrLogText, Regex.Escape(alert.StrLogText)), alert.BoolCaseSensitive)

            If RegExObject.IsMatch(strLogText) Then
                If alert.alertType = AlertType.Warning Then
                    ToolTipIcon = ToolTipIcon.Warning
                ElseIf alert.alertType = AlertType.ErrorMsg Then
                    ToolTipIcon = ToolTipIcon.Error
                ElseIf alert.alertType = AlertType.Info Then
                    ToolTipIcon = ToolTipIcon.Info
                End If

                strAlertText = If(String.IsNullOrWhiteSpace(alert.StrAlertText), strLogText, alert.StrAlertText)

                If alert.BoolRegex And Not String.IsNullOrWhiteSpace(alert.StrAlertText) Then
                    regExGroupCollection = RegExObject.Match(strLogText).Groups

                    If regExGroupCollection.Count > 0 Then
                        For index As Integer = 0 To regExGroupCollection.Count - 1
                            strAlertText = GetCachedRegex(Regex.Escape($"${index}"), False).Replace(strAlertText, regExGroupCollection(index).Value)
                        Next

                        For Each item As Group In regExGroupCollection
                            strAlertText = GetCachedRegex(Regex.Escape($"$({item.Name})"), True).Replace(strAlertText, regExGroupCollection(item.Name).Value)
                        Next
                    End If
                End If

                NotifyIcon.ShowBalloonTip(1, "Log Alert", strAlertText, ToolTipIcon)
                Return True
            End If
        Next

        Return False
    End Function

    Private Sub AddToLogList(strSourceIP As String, strLogText As String, boolIgnored As Boolean, boolAlerted As Boolean)
        Dim currentDate As Date = Now.ToLocalTime

        If Not boolIgnored Then
            Invoke(Sub()
                       SyncLock dataGridLockObject
                           Logs.Rows.Add(MakeDataGridRow(currentDate, currentDate.ToString, strSourceIP, strLogText, boolAlerted, Logs))
                           If intSortColumnIndex = 0 And sortOrder = SortOrder.Descending Then SortLogsByDateObjectNoLocking(intSortColumnIndex, SortOrder.Descending)
                       End SyncLock

                       NotifyIcon.Text = $"Free SysLog{vbCrLf}Last log received at {currentDate}."
                       UpdateLogCount()
                       BtnSaveLogsToDisk.Enabled = True

                       If ChkEnableAutoScroll.Checked And Logs.Rows.Count > 0 And intSortColumnIndex = 0 Then
                           Logs.FirstDisplayedScrollingRowIndex = If(sortOrder = SortOrder.Ascending, Logs.Rows.Count - 1, 0)
                       End If
                   End Sub)
        ElseIf boolIgnored And ChkEnableRecordingOfIgnoredLogs.Checked Then
            SyncLock IgnoredLogsLockObject
                Dim NewIgnoredItem As MyDataGridViewRow = MakeDataGridRow(currentDate, currentDate.ToString, strSourceIP, strLogText, False, Logs)
                IgnoredLogs.Add(NewIgnoredItem)
                If IgnoredLogsAndSearchResultsInstance IsNot Nothing Then IgnoredLogsAndSearchResultsInstance.AddIgnoredDatagrid(NewIgnoredItem, ChkEnableAutoScroll.Checked)
                Invoke(Sub() ClearIgnoredLogsToolStripMenuItem.Enabled = True)
            End SyncLock
        End If
    End Sub

    Private Sub OpenLogViewerWindow()
        If Logs.Rows.Count > 0 Then
            Dim selectedRow As MyDataGridViewRow = Logs.Rows(Logs.SelectedCells(0).RowIndex)

            Using LogViewerInstance As New LogViewer With {.strLogText = selectedRow.Cells(2).Value, .StartPosition = FormStartPosition.CenterParent, .Icon = Icon}
                LogViewerInstance.LblLogDate.Text = $"Log Date: {selectedRow.Cells(0).Value}"
                LogViewerInstance.LblSource.Text = $"Source IP Address: {selectedRow.Cells(1).Value}"
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

                For Each item As DataGridViewRow In Logs.SelectedRows
                    Logs.Rows.Remove(item)
                Next

                Logs.Rows.Add(MakeDataGridRow(Now, Now.ToString, IPAddress.Loopback.ToString, $"The user deleted {intNumberOfLogsDeleted:N0} log {If(intNumberOfLogsDeleted = 1, "entry", "entries")}.", False, Logs))

                If ChkEnableAutoScroll.Checked And Logs.Rows.Count > 0 And intSortColumnIndex = 0 Then
                    Logs.FirstDisplayedScrollingRowIndex = If(sortOrder = SortOrder.Ascending, Logs.Rows.Count - 1, 0)
                End If
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

    Private Sub UpdateLogCount()
        BtnClearLog.Enabled = Logs.Rows.Count <> 0
        NumberOfLogs.Text = $"Number of Log Entries: {Logs.Rows.Count:N0}"
    End Sub

    Private Sub ChkAutoScroll_Click(sender As Object, e As EventArgs) Handles ChkEnableAutoScroll.Click
        My.Settings.autoScroll = ChkEnableAutoScroll.Checked
    End Sub

    Private Sub Logs_ColumnWidthChanged(sender As Object, e As DataGridViewColumnEventArgs) Handles Logs.ColumnWidthChanged
        If boolDoneLoading Then
            My.Settings.columnTimeSize = ColTime.Width
            My.Settings.columnIPSize = ColIPAddress.Width
            My.Settings.columnLogSize = ColLog.Width
        End If
    End Sub

    Private Sub BtnClearAllLogs_Click(sender As Object, e As EventArgs) Handles BtnClearAllLogs.Click
        If MsgBox("Are you sure you want to clear the logs?", MsgBoxStyle.Question + MsgBoxStyle.YesNo + vbDefaultButton2, Text) = MsgBoxResult.Yes Then
            SyncLock dataGridLockObject
                Dim intOldCount As Integer = Logs.Rows.Count
                Logs.Rows.Clear()
                Logs.Rows.Add(MakeDataGridRow(Now, Now.ToString, IPAddress.Loopback.ToString, $"The user deleted {intOldCount:N0} log {If(intOldCount = 1, "entry", "entries")}.", False, Logs))

                If ChkEnableAutoScroll.Checked And Logs.Rows.Count > 0 And intSortColumnIndex = 0 Then
                    Logs.FirstDisplayedScrollingRowIndex = If(sortOrder = SortOrder.Ascending, Logs.Rows.Count - 1, 0)
                End If
            End SyncLock

            UpdateLogCount()
            SaveLogsToDiskSub()
        End If
    End Sub

    Private Sub SaveLogsToDiskSub()
        WriteLogsToDisk()
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
        WriteLogsToDisk()

        If boolDoWeOwnTheMutex Then SendMessageToSysLogServer("terminate", My.Settings.sysLogPort)

        Try
            mutex.ReleaseMutex()
        Catch ex As ApplicationException
        End Try
    End Sub

    Private Sub BtnSearch_Click(sender As Object, e As EventArgs) Handles BtnSearch.Click
        If BtnSearch.Text = "Search" Then
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
                                                          strLogText = MyDataGridRowItem.Cells(2).Value

                                                          If regexCompiledObject.IsMatch(strLogText) Then
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
                                                          Dim searchResultsWindow As New IgnoredLogsAndSearchResults(Me) With {.Icon = Icon, .LogsToBeDisplayed = listOfSearchResults, .Text = "Search Results", .WindowDisplayMode = IgnoreOrSearchWindowDisplayMode.search}
                                                          searchResultsWindow.ShowDialog(Me)
                                                      Else
                                                          MsgBox("Search terms not found.", MsgBoxStyle.Information, Text)
                                                      End If

                                                      Invoke(Sub() BtnSearch.Enabled = True)
                                                  End Sub

            worker.RunWorkerAsync()
        End If
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

    Private Sub SortLogsByDateObjectNoLocking(columnIndex As Integer, order As SortOrder)
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
            regexCache.Clear()
        End Using
    End Sub

    Private Sub ViewIgnoredLogsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ViewIgnoredLogsToolStripMenuItem.Click
        If IgnoredLogsAndSearchResultsInstance Is Nothing Then
            IgnoredLogsAndSearchResultsInstance = New IgnoredLogsAndSearchResults(Me) With {.Icon = Icon, .LogsToBeDisplayed = IgnoredLogs, .Text = "Ignored Logs", .WindowDisplayMode = IgnoreOrSearchWindowDisplayMode.ignored}
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

    Private Sub Form1_LocationChanged(sender As Object, e As EventArgs) Handles Me.LocationChanged
        If boolDoneLoading Then My.Settings.windowLocation = Location
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
                    Dim dateChosenDate As Date = ClearLogsOlderThanInstance.dateChosenDate.AddDays(-1)

                    SyncLock dataGridLockObject
                        Logs.AllowUserToOrderColumns = False
                        Logs.Enabled = False

                        Dim intOldCount As Integer = Logs.Rows.Count

                        For i As Integer = Logs.Rows.Count - 1 To 0 Step -1
                            Dim item As MyDataGridViewRow = CType(Logs.Rows(i), MyDataGridViewRow)

                            If item.DateObject.Date <= dateChosenDate.Date Then
                                Logs.Rows.RemoveAt(i)
                            End If
                        Next

                        Logs.Enabled = True
                        Logs.AllowUserToOrderColumns = True

                        Dim intCountDifference As Integer = intOldCount - Logs.Rows.Count
                        Logs.Rows.Add(MakeDataGridRow(Now, Now.ToString, IPAddress.Loopback.ToString, $"The user deleted {intCountDifference:N0} log {If(intCountDifference = 1, "entry", "entries")}.", False, Logs))

                        If ChkEnableAutoScroll.Checked And Logs.Rows.Count > 0 And intSortColumnIndex = 0 Then
                            Logs.FirstDisplayedScrollingRowIndex = If(sortOrder = SortOrder.Ascending, Logs.Rows.Count - 1, 0)
                        End If
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
            regexCache.Clear()
        End Using
    End Sub

    Private Sub ConfigureAlternatingColorToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ChangeAlternatingColorToolStripMenuItem.Click
        Using ColorDialog As New ColorDialog()
            If ColorDialog.ShowDialog() = DialogResult.OK Then
                My.Settings.searchColor = ColorDialog.Color

                Dim rowStyle As New DataGridViewCellStyle() With {.BackColor = ColorDialog.Color}
                Logs.AlternatingRowsDefaultCellStyle = rowStyle
            End If
        End Using
    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
#If DEBUG Then
        MsgBox($"Free SysLog.{vbCrLf}{vbCrLf}Version {checkForUpdates.versionString} (Debug Build)", MsgBoxStyle.Information, Text)
#Else
        MsgBox($"Free SysLog.{vbCrLf}{vbCrLf}Version {checkForUpdates.versionString}", MsgBoxStyle.Information, Text)
#End If
    End Sub

    Private Sub ExportToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExportToolStripMenuItem.Click
        Using SaveFileDialog As New SaveFileDialog()
            SaveFileDialog.Title = "Safe Program Settings..."
            SaveFileDialog.Filter = "JSON File|*.json"

            If SaveFileDialog.ShowDialog() = DialogResult.OK Then
                Try
                    SaveApplicationSettingsToFile(SaveFileDialog.FileName)
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

            If OpenFileDialog.ShowDialog = DialogResult.OK AndAlso LoadApplicationSettingsFromFile(OpenFileDialog.FileName, Text) Then
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
            Dim dateChosenDate As Date = Date.Today.AddDays(-daysToKeep)

            SyncLock dataGridLockObject
                Logs.AllowUserToOrderColumns = False
                Logs.Enabled = False

                Dim intOldCount As Integer = Logs.Rows.Count

                For i As Integer = Logs.Rows.Count - 1 To 0 Step -1
                    Dim item As MyDataGridViewRow = CType(Logs.Rows(i), MyDataGridViewRow)

                    If item.DateObject.Date <= dateChosenDate.Date Then
                        Logs.Rows.RemoveAt(i)
                    End If
                Next

                Logs.Enabled = True
                Logs.AllowUserToOrderColumns = True

                Dim intCountDifference As Integer = intOldCount - Logs.Rows.Count
                Logs.Rows.Add(MakeDataGridRow(Now, Now.ToString, IPAddress.Loopback.ToString, $"The user deleted {intCountDifference:N0} log {If(intCountDifference = 1, "entry", "entries")}.", False, Logs))

                If ChkEnableAutoScroll.Checked And Logs.Rows.Count > 0 And intSortColumnIndex = 0 Then
                    Logs.FirstDisplayedScrollingRowIndex = If(sortOrder = SortOrder.Ascending, Logs.Rows.Count - 1, 0)
                End If
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
        CopyTextToWindowsClipboard(Logs.SelectedRows(0).Cells(2).Value)
    End Sub

    Private Function CopyTextToWindowsClipboard(strTextToBeCopiedToClipboard As String) As Boolean
        Try
            Clipboard.SetDataObject(strTextToBeCopiedToClipboard, True, 5, 200)
            Return True
        Catch ex As Exception
            MsgBox("Unable to open Windows Clipboard to copy text to it.", MsgBoxStyle.Critical, Text)
            Return False
        End Try
    End Function

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

            For Each item As DataGridViewRow In Logs.SelectedRows
                Logs.Rows.Remove(item)
            Next

            Logs.Rows.Add(MakeDataGridRow(Now, Now.ToString, IPAddress.Loopback.ToString, $"The user deleted {intNumberOfLogsDeleted:N0} log {If(intNumberOfLogsDeleted = 1, "entry", "entries")}.", False, Logs))

            If ChkEnableAutoScroll.Checked And Logs.Rows.Count > 0 And intSortColumnIndex = 0 Then
                Logs.FirstDisplayedScrollingRowIndex = If(sortOrder = SortOrder.Ascending, Logs.Rows.Count - 1, 0)
            End If
        End SyncLock

        UpdateLogCount()
        SaveLogsToDiskSub()
    End Sub

    Private Sub ExportAllLogsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExportAllLogsToolStripMenuItem.Click
        SyncLock dataGridLockObject
            Dim saveFileDialog As New SaveFileDialog With {.Title = "Export Data...", .Filter = "CSV (Comma Separated Value)|*.csv|JSON File|*.json|XML File|*.xml"}

            If saveFileDialog.ShowDialog() = DialogResult.OK Then
                Dim fileInfo As New FileInfo(saveFileDialog.FileName)

                Dim collectionOfSavedData As New List(Of SavedData)
                Dim myItem As MyDataGridViewRow
                Dim csvStringBuilder As New StringBuilder
                Dim strTime, strSourceIP, strLogText, strAlerted As String

                If fileInfo.Extension.Equals(".csv", StringComparison.OrdinalIgnoreCase) Then csvStringBuilder.AppendLine("Time,Source IP,Log Text,Alerted")

                For Each item As DataGridViewRow In Logs.Rows
                    If Not String.IsNullOrWhiteSpace(item.Cells(0).Value) Then
                        myItem = DirectCast(item, MyDataGridViewRow)

                        If fileInfo.Extension.Equals(".csv", StringComparison.OrdinalIgnoreCase) Then
                            With myItem
                                strTime = SanitizeForCSV(.Cells(0).Value)
                                strSourceIP = SanitizeForCSV(.Cells(1).Value)
                                strLogText = SanitizeForCSV(.Cells(2).Value)
                                strAlerted = If(.BoolAlerted, "Yes", "No")
                            End With

                            csvStringBuilder.AppendLine($"{strTime},{strSourceIP},{strLogText},{strAlerted}")
                        Else
                            collectionOfSavedData.Add(New SavedData With {
                                                    .time = myItem.Cells(0).Value,
                                                    .ip = myItem.Cells(1).Value,
                                                    .log = myItem.Cells(2).Value,
                                                    .DateObject = myItem.DateObject,
                                                    .BoolAlerted = myItem.BoolAlerted
                                                  })
                        End If
                    End If
                Next

                Using fileStream As New StreamWriter(saveFileDialog.FileName)
                    If fileInfo.Extension.Equals(".xml", StringComparison.OrdinalIgnoreCase) Then
                        Dim xmlSerializerObject As New Xml.Serialization.XmlSerializer(collectionOfSavedData.GetType)
                        xmlSerializerObject.Serialize(fileStream, collectionOfSavedData)
                    ElseIf fileInfo.Extension.Equals(".json", StringComparison.OrdinalIgnoreCase) Then
                        fileStream.Write(Newtonsoft.Json.JsonConvert.SerializeObject(collectionOfSavedData))
                    ElseIf fileInfo.Extension.Equals(".csv", StringComparison.OrdinalIgnoreCase) Then
                        fileStream.Write(csvStringBuilder.ToString.Trim)
                    End If
                End Using

                If MsgBox($"Data exported to ""{saveFileDialog.FileName}"" successfully.{vbCrLf}{vbCrLf}Do you want to open Windows Explorer to the location of the file?", MsgBoxStyle.Question + MsgBoxStyle.YesNo + vbDefaultButton2, Text) = MsgBoxResult.Yes Then
                    SelectFileInWindowsExplorer(saveFileDialog.FileName)
                End If
            End If
        End SyncLock
    End Sub

    Private Sub ExportsLogsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExportsLogsToolStripMenuItem.Click
        SyncLock dataGridLockObject
            Dim saveFileDialog As New SaveFileDialog With {.Title = "Export Data...", .Filter = "CSV (Comma Separated Value)|*.csv|JSON File|*.json|XML File|*.xml"}

            If saveFileDialog.ShowDialog() = DialogResult.OK Then
                Dim fileInfo As New FileInfo(saveFileDialog.FileName)

                Dim collectionOfSavedData As New List(Of SavedData)
                Dim myItem As MyDataGridViewRow
                Dim csvStringBuilder As New StringBuilder
                Dim strTime, strSourceIP, strLogText, strAlerted As String

                If fileInfo.Extension.Equals(".csv", StringComparison.OrdinalIgnoreCase) Then csvStringBuilder.AppendLine("Time,Source IP,Log Text,Alerted")

                For Each item As DataGridViewRow In Logs.SelectedRows
                    If Not String.IsNullOrWhiteSpace(item.Cells(0).Value) Then
                        myItem = DirectCast(item, MyDataGridViewRow)

                        If fileInfo.Extension.Equals(".csv", StringComparison.OrdinalIgnoreCase) Then
                            With myItem
                                strTime = SanitizeForCSV(.Cells(0).Value)
                                strSourceIP = SanitizeForCSV(.Cells(1).Value)
                                strLogText = SanitizeForCSV(.Cells(2).Value)
                                strAlerted = If(.BoolAlerted, "Yes", "No")
                            End With

                            csvStringBuilder.AppendLine($"{strTime},{strSourceIP},{strLogText},{strAlerted}")
                        Else
                            collectionOfSavedData.Add(New SavedData With {
                                                    .time = myItem.Cells(0).Value,
                                                    .ip = myItem.Cells(1).Value,
                                                    .log = myItem.Cells(2).Value,
                                                    .DateObject = myItem.DateObject,
                                                    .BoolAlerted = myItem.BoolAlerted
                                                  })
                        End If
                    End If
                Next

                Using fileStream As New StreamWriter(saveFileDialog.FileName)
                    If fileInfo.Extension.Equals(".xml", StringComparison.OrdinalIgnoreCase) Then
                        Dim xmlSerializerObject As New Xml.Serialization.XmlSerializer(collectionOfSavedData.GetType)
                        xmlSerializerObject.Serialize(fileStream, collectionOfSavedData)
                    ElseIf fileInfo.Extension.Equals(".json", StringComparison.OrdinalIgnoreCase) Then
                        fileStream.Write(Newtonsoft.Json.JsonConvert.SerializeObject(collectionOfSavedData))
                    ElseIf fileInfo.Extension.Equals(".csv", StringComparison.OrdinalIgnoreCase) Then
                        fileStream.Write(csvStringBuilder.ToString.Trim)
                    End If
                End Using

                If MsgBox($"Data exported to ""{saveFileDialog.FileName}"" successfully.{vbCrLf}{vbCrLf}Do you want to open Windows Explorer to the location of the file?", MsgBoxStyle.Question + MsgBoxStyle.YesNo + vbDefaultButton2, Text) = MsgBoxResult.Yes Then
                    SelectFileInWindowsExplorer(saveFileDialog.FileName)
                End If
            End If
        End SyncLock
    End Sub

    Private Sub DonationStripMenuItem_Click(sender As Object, e As EventArgs) Handles DonationStripMenuItem.Click
        Process.Start(strPayPal)
    End Sub

    Private Sub StopServerStripMenuItem_Click(sender As Object, e As EventArgs) Handles StopServerStripMenuItem.Click
        If StopServerStripMenuItem.Text = "Stop Server" Then
            SendMessageToSysLogServer("terminate", My.Settings.sysLogPort)
            StopServerStripMenuItem.Text = "Start Server"
        ElseIf StopServerStripMenuItem.Text = "Start Server" Then
            serverThread = New Threading.Thread(AddressOf SysLogThread) With {.Name = "UDP Server Thread", .Priority = Threading.ThreadPriority.Normal}
            serverThread.Start()

            SyncLock dataGridLockObject
                Logs.Rows.Add(MakeDataGridRow(Now, Now.ToString, IPAddress.Loopback.ToString, "Free SysLog Server Started.", False, Logs))

                If ChkEnableAutoScroll.Checked And Logs.Rows.Count > 0 And intSortColumnIndex = 0 Then
                    Logs.FirstDisplayedScrollingRowIndex = If(sortOrder = SortOrder.Ascending, Logs.Rows.Count - 1, 0)
                End If

                UpdateLogCount()
            End SyncLock

            StopServerStripMenuItem.Text = "Stop Server"
        End If
    End Sub

    Private Sub ConfigureAlertsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConfigureAlertsToolStripMenuItem.Click
        Using Alerts As New Alerts With {.Icon = Icon, .StartPosition = FormStartPosition.CenterParent}
            Alerts.ShowDialog(Me)
            regexCache.Clear()
        End Using
    End Sub

    Private Sub OpenWindowsExplorerToAppConfigFile_Click(sender As Object, e As EventArgs) Handles OpenWindowsExplorerToAppConfigFile.Click
        MsgBox("Modifying the application XML configuration file by hand may cause the program to malfunction. Caution is advised.", MsgBoxStyle.Information, Text)
        SelectFileInWindowsExplorer(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath)
    End Sub

    Private Sub CreateAlertToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CreateAlertToolStripMenuItem.Click
        Using AddAlert As New AddAlert With {.StartPosition = FormStartPosition.CenterParent, .Icon = Icon, .Text = "Add Alert"}
            Dim strLogText As String = Logs.SelectedRows(0).Cells(2).Value
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
        Using IntegerInputForm As New IntegerInputForm With {.Icon = Icon, .Text = "Change Syslog Server Port", .StartPosition = FormStartPosition.CenterParent, .intMax = 65535, .intMin = 1}
            With IntegerInputForm
                .lblSetting.Text = "Server Port"
                .TxtSetting.Text = My.Settings.sysLogPort

                .ShowDialog(Me)

                If .boolSuccess Then
                    If .intResult < 1 Or .intResult > 65535 Then
                        MsgBox("The port number must be in the range of 1 - 65535.", MsgBoxStyle.Critical, Text)
                    Else
                        If boolDoWeOwnTheMutex Then SendMessageToSysLogServer("terminate", My.Settings.sysLogPort)

                        ChangeSyslogServerPortToolStripMenuItem.Text = $"Change Syslog Server Port (Port Number {IntegerInputForm.intResult})"

                        My.Settings.sysLogPort = .intResult
                        My.Settings.Save()

                        If serverThread.IsAlive Then serverThread.Abort()

                        serverThread = New Threading.Thread(AddressOf SysLogThread) With {.Name = "UDP Server Thread", .Priority = Threading.ThreadPriority.Normal}
                        serverThread.Start()

                        SyncLock dataGridLockObject
                            Logs.Rows.Add(MakeDataGridRow(Now, Now.ToString, IPAddress.Loopback.ToString, "Free SysLog Server Started.", False, Logs))

                            If ChkEnableAutoScroll.Checked And Logs.Rows.Count > 0 And intSortColumnIndex = 0 Then
                                Logs.FirstDisplayedScrollingRowIndex = If(sortOrder = SortOrder.Ascending, Logs.Rows.Count - 1, 0)
                            End If

                            UpdateLogCount()
                        End SyncLock

                        MsgBox("Done.", MsgBoxStyle.Information, Text)
                    End If
                End If
            End With
        End Using
    End Sub

    Private Sub ChangeAutosaveIntervalToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ChangeLogAutosaveIntervalToolStripMenuItem.Click
        Using IntegerInputForm As New IntegerInputForm With {.Icon = Icon, .Text = "Change Log Autosave Interval", .StartPosition = FormStartPosition.CenterParent, .intMax = 20, .intMin = 1}
            With IntegerInputForm
                .lblSetting.Text = "Auto Save (In Minutes)"
                .TxtSetting.Text = My.Settings.autoSaveMinutes

                .ShowDialog(Me)

                If .boolSuccess Then
                    ChangeLogAutosaveIntervalToolStripMenuItem.Text = $"Change Log Autosave Interval ({IntegerInputForm.intResult} Minutes)"
                    SaveTimer.Interval = TimeSpan.FromMinutes(.intResult).TotalMilliseconds
                    My.Settings.autoSaveMinutes = .intResult

                    MsgBox("Done.", MsgBoxStyle.Information, Text)
                End If
            End With
        End Using
    End Sub

    Private Sub CreateIgnoredLogToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CreateIgnoredLogToolStripMenuItem.Click
        Using AddIgnored As New AddIgnored With {.StartPosition = FormStartPosition.CenterParent, .Icon = Icon, .Text = "Add Ignored String"}
            Dim strLogText As String = Logs.SelectedRows(0).Cells(2).Value
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
            Dim strLogText As String = Logs.SelectedRows(0).Cells(2).Value
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

            If ConfigureSysLogMirrorServers.boolSuccess Then
                MsgBox("Done", MsgBoxStyle.Information, Text)
            End If
        End Using
    End Sub

#Region "-- SysLog Server Code --"
    Sub SysLogThread()
        Try
            Dim ipEndPoint As New IPEndPoint(IPAddress.Any, 0)

            Using udpServer As New UdpClient(My.Settings.sysLogPort)
                Dim strReceivedData, strSourceIP As String
                Dim byteReceivedData() As Byte
                Dim boolDoServerLoop As Boolean = True
                Dim ProxiedSysLogData As ProxiedSysLogData

                While boolDoServerLoop
                    byteReceivedData = udpServer.Receive(ipEndPoint)
                    strReceivedData = Encoding.UTF8.GetString(byteReceivedData)
                    strSourceIP = ipEndPoint.Address.ToString

                    If strReceivedData.Trim.Equals("restore", StringComparison.OrdinalIgnoreCase) Then
                        Invoke(Sub() RestoreWindowAfterReceivingRestoreCommand())
                    ElseIf strReceivedData.Trim.Equals("terminate", StringComparison.OrdinalIgnoreCase) Then
                        boolDoServerLoop = False
                    ElseIf strReceivedData.Trim.StartsWith("proxied", StringComparison.OrdinalIgnoreCase) Then
                        Try
                            strReceivedData = strReceivedData.Replace("proxied|", "", StringComparison.OrdinalIgnoreCase)
                            ProxiedSysLogData = Newtonsoft.Json.JsonConvert.DeserializeObject(Of ProxiedSysLogData)(strReceivedData, JSONDecoderSettings)
                            ProcessIncomingLog(ProxiedSysLogData.log, ProxiedSysLogData.ip)
                        Catch ex As Newtonsoft.Json.JsonSerializationException
                        End Try
                    Else
                        If serversList.Count > 0 Then
                            Threading.ThreadPool.QueueUserWorkItem(Sub()
                                                                       ProxiedSysLogData = New ProxiedSysLogData() With {.ip = strSourceIP, .log = strReceivedData}

                                                                       For Each item As SysLogProxyServer In serversList
                                                                           SendMessageToSysLogServer("proxied|" & Newtonsoft.Json.JsonConvert.SerializeObject(ProxiedSysLogData), item.ip, item.port)
                                                                       Next

                                                                       ProxiedSysLogData = Nothing
                                                                   End Sub)
                        End If

                        ProcessIncomingLog(strReceivedData, strSourceIP)
                    End If

                    strReceivedData = Nothing
                    strSourceIP = Nothing
                End While
            End Using
        Catch ex As Threading.ThreadAbortException
            ' Does nothing
        Catch e As Exception
            Invoke(Sub() MsgBox("Unable to start syslog server, perhaps another instance of this program is running on your system.", MsgBoxStyle.Critical + MsgBoxStyle.ApplicationModal, Text))
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

        If ChkEnableAutoScroll.Checked And Logs.Rows.Count > 0 And intSortColumnIndex = 0 Then
            Logs.FirstDisplayedScrollingRowIndex = If(sortOrder = SortOrder.Ascending, Logs.Rows.Count - 1, 0)
        End If
    End Sub

    Private Sub ChkShowAlertedColumn_Click(sender As Object, e As EventArgs) Handles ChkShowAlertedColumn.Click
        My.Settings.boolShowAlertedColumn = ChkShowAlertedColumn.Checked
        ColAlerts.Visible = ChkShowAlertedColumn.Checked
    End Sub
#End Region
End Class