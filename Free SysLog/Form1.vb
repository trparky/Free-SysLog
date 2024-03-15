Imports System.IO
Imports System.Net.Sockets
Imports System.Net
Imports System.Text
Imports System.ComponentModel
Imports Microsoft.Win32
Imports System.Text.RegularExpressions
Imports System.Reflection

Public Class Form1
    Private boolDoneLoading As Boolean = False
    Private lockObject As New Object
    Private m_SortingColumn1, m_SortingColumn2 As ColumnHeader
    Private longNumberOfIgnoredLogs As Long = 0
    Private IgnoredLogs As New List(Of MyDataGridViewRow)
    Private regexCache As New Dictionary(Of String, Regex)

    Private Function MakeDataGridRow(dateObject As Date, strTime As String, strType As String, strSourceAddress As String, strLog As String, ByRef dataGrid As DataGridView) As MyDataGridViewRow
        Dim MyDataGridViewRow As New MyDataGridViewRow

        With MyDataGridViewRow
            .CreateCells(dataGrid)
            .Cells(0).Value = strTime
            .Cells(0).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Cells(1).Value = strType
            .Cells(1).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Cells(2).Value = strSourceAddress
            .Cells(2).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Cells(3).Value = strLog
            .DateObject = dateObject
        End With

        Return MyDataGridViewRow
    End Function

    Private Sub ChkStartAtUserStartup_Click(sender As Object, e As EventArgs) Handles chkStartAtUserStartup.Click
        Using registryKey As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Run", True)
            If chkStartAtUserStartup.Checked Then
                registryKey.SetValue("Free Syslog", $"""{Application.ExecutablePath}"" /background")
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

    Private Sub BtnMoveLogFile_Click(sender As Object, e As EventArgs) Handles btnMoveLogFile.Click
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
                For Each item As DataGridViewRow In logs.Rows
                    If Not String.IsNullOrWhiteSpace(item.Cells(0).Value) Then
                        myItem = DirectCast(item, MyDataGridViewRow)

                        collectionOfSavedData.Add(New SavedData With {
                                            .time = myItem.Cells(0).Value,
                                            .type = myItem.Cells(1).Value,
                                            .ip = myItem.Cells(2).Value,
                                            .log = myItem.Cells(3).Value,
                                            .DateObject = myItem.DateObject
                                          })
                    End If
                Next
            End SyncLock

            Using fileStream As New StreamWriter(My.Settings.logFileLocation)
                fileStream.Write(Newtonsoft.Json.JsonConvert.SerializeObject(collectionOfSavedData))
            End Using

            lblLogFileSize.Text = $"Log File Size: {FileSizeToHumanSize(New FileInfo(My.Settings.logFileLocation).Length)}"

            btnSaveLogsToDisk.Enabled = False
        End SyncLock
    End Sub

    Private Sub Form1_ResizeEnd(sender As Object, e As EventArgs) Handles Me.ResizeEnd
        My.Settings.mainWindowSize = Size
    End Sub

    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        If boolDoneLoading Then My.Settings.boolMaximized = WindowState = FormWindowState.Maximized
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        WriteLogsToDisk()
        Process.GetCurrentProcess.Kill()
    End Sub

    Private Sub ChkAutoSave_Click(sender As Object, e As EventArgs) Handles chkAutoSave.Click
        SaveTimer.Enabled = chkAutoSave.Checked
        NumericUpDown.Visible = chkAutoSave.Checked
        lblAutoSaveLabel.Visible = chkAutoSave.Checked
        lblAutoSaved.Visible = chkAutoSave.Checked
    End Sub

    Private Sub SaveTimer_Tick(sender As Object, e As EventArgs) Handles SaveTimer.Tick
        WriteLogsToDisk()
        lblAutoSaved.Text = $"Last Auto-Saved At: {Date.Now:h:mm:ss tt}"
    End Sub

    Private Sub NumericUpDown_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown.ValueChanged
        If boolDoneLoading Then
            SaveTimer.Interval = TimeSpan.FromMinutes(NumericUpDown.Value).TotalMilliseconds
            My.Settings.autoSaveMinutes = NumericUpDown.Value
        End If
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
        If digits = 0 Then
            Return Math.Round(value, digits).ToString
        Else
            Dim strFormatString As String = "{0:0." & New String("0", digits) & "}"
            Return String.Format(strFormatString, Math.Round(value, digits))
        End If
    End Function

    Private Function ProcessReplacements(input As String) As String
        If replacementsList.Count > 0 Then
            For Each item As ReplacementsClass In replacementsList
                If item.BoolRegex Then
                    Try
                        input = GetCachedRegex(item.StrReplace, item.BoolCaseSensitive).Replace(input, item.StrReplaceWith)
                    Catch ex As Exception
                    End Try
                Else
                    If item.BoolCaseSensitive Then
                        input = input.Replace(item.StrReplace, item.StrReplaceWith)
                    Else
                        input = input.Replace(item.StrReplace, item.StrReplaceWith, StringComparison.OrdinalIgnoreCase)
                    End If
                End If
            Next
        End If

        Return input
    End Function

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If My.Application.CommandLineArgs.Count > 0 AndAlso My.Application.CommandLineArgs(0).Trim.Equals("/background", StringComparison.OrdinalIgnoreCase) Then WindowState = FormWindowState.Minimized

        colTime.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
        colTime.HeaderCell.Style.Padding = New Padding(0, 0, 1, 0)
        colType.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
        colType.HeaderCell.Style.Padding = New Padding(0, 0, 2, 0)
        colIPAddress.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
        colIPAddress.HeaderCell.Style.Padding = New Padding(0, 0, 2, 0)

        chkRecordIgnoredLogs.Checked = My.Settings.recordIgnoredLogs
        IgnoredLogsToolStripMenuItem.Visible = chkRecordIgnoredLogs.Checked
        ZerooutIgnoredLogsCounterToolStripMenuItem.Visible = Not chkRecordIgnoredLogs.Checked
        chkAutoScroll.Checked = My.Settings.autoScroll
        chkAutoSave.Checked = My.Settings.autoSave
        NumericUpDown.Value = My.Settings.autoSaveMinutes
        NumericUpDown.Visible = chkAutoSave.Checked
        lblAutoSaveLabel.Visible = chkAutoSave.Checked
        lblAutoSaved.Visible = chkAutoSave.Checked
        chkStartAtUserStartup.Checked = DoesStartupEntryExist()
        Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath)
        txtSysLogServerPort.Text = My.Settings.sysLogPort.ToString
        Location = VerifyWindowLocation(My.Settings.windowLocation, Me)
        If My.Settings.boolMaximized Then WindowState = FormWindowState.Maximized

        Dim flags As BindingFlags = BindingFlags.NonPublic Or BindingFlags.Instance Or BindingFlags.SetProperty
        Dim propInfo As PropertyInfo = GetType(DataGridView).GetProperty("DoubleBuffered", flags)
        propInfo?.SetValue(logs, True, Nothing)

        Dim rowStyle As New DataGridViewCellStyle() With {.BackColor = My.Settings.searchColor}
        logs.AlternatingRowsDefaultCellStyle = rowStyle

        If My.Settings.replacements IsNot Nothing AndAlso My.Settings.replacements.Count > 0 Then
            For Each strJSONString As String In My.Settings.replacements
                replacementsList.Add(Newtonsoft.Json.JsonConvert.DeserializeObject(Of ReplacementsClass)(strJSONString))
            Next
        End If

        If My.Settings.autoSave Then
            SaveTimer.Interval = TimeSpan.FromMinutes(My.Settings.autoSaveMinutes).TotalMilliseconds
            SaveTimer.Enabled = True
        End If

        Size = My.Settings.mainWindowSize

        colTime.Width = My.Settings.columnTimeSize
        colType.Width = My.Settings.columnTypeSize
        colIPAddress.Width = My.Settings.columnIPSize
        colLog.Width = My.Settings.columnLogSize

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
                        MsgBox("You must set a location to save the syslog data file to.", MsgBoxStyle.Information, Text)
                    End If
                Loop While True
            End Using
        End If

        Dim worker As New BackgroundWorker()
        AddHandler worker.DoWork, Sub() LoadDataFile()
        AddHandler worker.RunWorkerCompleted, Sub()
                                                  Dim serverThread As New Threading.Thread(AddressOf SysLogThread) With {.Name = "UDP Server Thread", .IsBackground = True, .Priority = Threading.ThreadPriority.Normal}
                                                  serverThread.Start()
                                              End Sub
        worker.RunWorkerAsync()
    End Sub

    Private Sub LoadDataFile()
        If File.Exists(My.Settings.logFileLocation) Then
            Try
                Invoke(Sub()
                           logs.Rows.Add(MakeDataGridRow(Now, "", "", "", "Loading data and populating data grid... Please Wait.", logs))
                           lblLogFileSize.Text = $"Log File Size: {FileSizeToHumanSize(New FileInfo(My.Settings.logFileLocation).Length)}"
                       End Sub)

                Dim collectionOfSavedData As New List(Of SavedData)

                Using fileStream As New StreamReader(My.Settings.logFileLocation)
                    collectionOfSavedData = Newtonsoft.Json.JsonConvert.DeserializeObject(Of List(Of SavedData))(fileStream.ReadToEnd.Trim)
                End Using

                Dim listOfLogEntries As New List(Of MyDataGridViewRow)

                For Each item As SavedData In collectionOfSavedData
                    listOfLogEntries.Add(MakeDataGridRow(item.DateObject, item.time, item.type, item.ip, item.log, logs))
                Next

                SyncLock dataGridLockObject
                    Invoke(Sub()
                               logs.Rows.Clear()
                               logs.Rows.AddRange(listOfLogEntries.ToArray)
                               If logs.Rows.Count > 0 Then logs.FirstDisplayedScrollingRowIndex = logs.Rows.GetLastRow(DataGridViewElementStates.None)
                               UpdateLogCount()
                           End Sub)
                End SyncLock
            Catch ex As Exception
            End Try
        End If
    End Sub

    Private Sub BtnOpenLogLocation_Click(sender As Object, e As EventArgs) Handles btnOpenLogLocation.Click
        SelectFileInWindowsExplorer(My.Settings.logFileLocation)
    End Sub

    Private Function GetSyslogPriority(sSyslog As String) As String
        If sSyslog.Contains("L0") Then
            Return "Emergency (0)"
        ElseIf sSyslog.Contains("L1") Then
            Return "Alert (1)"
        ElseIf sSyslog.Contains("L2") Then
            Return "Critical (2)"
        ElseIf sSyslog.Contains("L3") Then
            Return "Error (3)"
        ElseIf sSyslog.Contains("L4") Then
            Return "Warning (4)"
        ElseIf sSyslog.Contains("L5") Then
            Return "Notice (5)"
        ElseIf sSyslog.Contains("L6") Then
            Return "Info (6)"
        ElseIf sSyslog.Contains("L7") Then
            Return "Debug (7)"
        Else
            Return ""
        End If
    End Function

    Private Function GetCachedRegex(pattern As String, Optional boolCaseInsensitive As Boolean = True) As Regex
        If Not regexCache.ContainsKey(pattern) Then
            Dim options As RegexOptions = If(boolCaseInsensitive, RegexOptions.Compiled Or RegexOptions.IgnoreCase, RegexOptions.Compiled)
            regexCache(pattern) = New Regex(pattern, options)
        End If

        Return regexCache(pattern)
    End Function

    Private Sub FillLog(sSyslog As String, sFromIp As String)
        Try
            Dim sPriority As String
            Dim boolIgnored As Boolean = False

            sSyslog = sSyslog.Replace(vbCr, vbCrLf) ' Converts from UNIX to DOS/Windows.
            sSyslog = sSyslog.Replace(vbCrLf, "")
            sSyslog = Mid(sSyslog, InStr(sSyslog, ">") + 1, Len(sSyslog))
            sSyslog = sSyslog.Trim

            sPriority = GetSyslogPriority(sSyslog)

            If My.Settings.ignored IsNot Nothing AndAlso My.Settings.ignored.Count <> 0 Then
                For Each word As String In My.Settings.ignored
                    If GetCachedRegex(Regex.Escape(word)).IsMatch(sSyslog) Then
                        Invoke(Sub()
                                   longNumberOfIgnoredLogs += 1

                                   If chkRecordIgnoredLogs.Checked Then
                                       IgnoredLogsToolStripMenuItem.Enabled = True
                                   Else
                                       ZerooutIgnoredLogsCounterToolStripMenuItem.Enabled = True
                                   End If

                                   lblNumberOfIgnoredIncomingLogs.Text = $"Number of ignored incoming logs: {longNumberOfIgnoredLogs:N0}"
                               End Sub)

                        boolIgnored = True
                        Exit For
                    End If
                Next
            End If

            AddToLogList(sPriority, sFromIp, sSyslog, boolIgnored)
        Catch ex As Exception
            AddToLogList("Error (3)", "local", $"{ex.Message} -- {ex.StackTrace}", False)
        End Try
    End Sub

    Private Sub AddToLogList(sPriority As String, sFromIp As String, sSyslog As String, boolIgnored As Boolean)
        Dim currentDate As Date = Now.ToLocalTime

        If Not boolIgnored Then
            Invoke(Sub()
                       SyncLock dataGridLockObject
                           logs.Rows.Add(MakeDataGridRow(currentDate, currentDate.ToString, sPriority, sFromIp, ProcessReplacements(sSyslog), logs))
                       End SyncLock

                       UpdateLogCount()
                       btnSaveLogsToDisk.Enabled = True

                       If chkAutoScroll.Checked Then logs.FirstDisplayedScrollingRowIndex = logs.Rows.GetLastRow(DataGridViewElementStates.None)
                   End Sub)
        ElseIf boolIgnored And chkRecordIgnoredLogs.Checked Then
            IgnoredLogs.Add(MakeDataGridRow(currentDate, currentDate.ToString, sPriority, sFromIp, ProcessReplacements(sSyslog), logs))
        End If
    End Sub

    Private Sub OpenLogViewerWindow()
        If logs.Rows.Count > 0 Then
            Dim selectedRow As MyDataGridViewRow = logs.Rows(logs.SelectedCells(0).RowIndex)

            Using LogViewer As New Log_Viewer With {.strLogText = selectedRow.Cells(3).Value, .StartPosition = FormStartPosition.CenterParent, .Icon = Icon}
                LogViewer.lblLogDate.Text = $"Log Date: {selectedRow.Cells(0).Value}"
                LogViewer.lblSource.Text = $"Source IP Address: {selectedRow.Cells(2).Value}"
                LogViewer.ShowDialog(Me)
            End Using
        End If
    End Sub

    Private Sub Logs_DoubleClick(sender As Object, e As EventArgs) Handles logs.DoubleClick
        OpenLogViewerWindow()
    End Sub

    Private Sub Logs_KeyUp(sender As Object, e As KeyEventArgs) Handles logs.KeyUp
        If e.KeyValue = Keys.Enter Then
            OpenLogViewerWindow()
        ElseIf e.KeyValue = Keys.Delete Then
            SyncLock dataGridLockObject
                For Each item As DataGridViewRow In logs.SelectedRows
                    logs.Rows.Remove(item)
                Next
            End SyncLock

            UpdateLogCount()
            SaveLogsToDiskSub()
        End If
    End Sub

    Private Sub Logs_KeyDown(sender As Object, e As KeyEventArgs) Handles logs.KeyDown
        If e.KeyCode = Keys.Enter Then e.Handled = True
    End Sub

    Private Sub Logs_UserDeletingRow(sender As Object, e As DataGridViewRowCancelEventArgs) Handles logs.UserDeletingRow
        e.Cancel = True
    End Sub

    Private Sub UpdateLogCount()
        btnClearLog.Enabled = logs.Rows.Count <> 0
        NumberOfLogs.Text = $"Number of Log Entries: {logs.Rows.Count:N0}"
    End Sub

    Private Sub ChkAutoScroll_Click(sender As Object, e As EventArgs) Handles chkAutoScroll.Click
        My.Settings.autoScroll = chkAutoScroll.Checked
    End Sub

    Private Sub Logs_ColumnWidthChanged(sender As Object, e As DataGridViewColumnEventArgs) Handles logs.ColumnWidthChanged
        If boolDoneLoading Then
            My.Settings.columnTimeSize = colTime.Width
            My.Settings.columnTypeSize = colType.Width
            My.Settings.columnIPSize = colIPAddress.Width
            My.Settings.columnLogSize = colLog.Width
        End If
    End Sub

    Private Sub BtnClearAllLogs_Click(sender As Object, e As EventArgs) Handles btnClearAllLogs.Click
        If MsgBox("Are you sure you want to clear the logs?", MsgBoxStyle.Question + MsgBoxStyle.YesNo + vbDefaultButton2, Text) = MsgBoxResult.Yes Then
            SyncLock dataGridLockObject
                logs.Rows.Clear()
            End SyncLock

            UpdateLogCount()
            SaveLogsToDiskSub()
        End If
    End Sub

    Private Sub SaveLogsToDiskSub()
        WriteLogsToDisk()
        lblAutoSaved.Text = $"Last Saved At: {Date.Now:h:mm:ss tt}"
        SaveTimer.Enabled = False
        SaveTimer.Enabled = True
    End Sub

    Private Sub BtnSaveLogsToDisk_Click(sender As Object, e As EventArgs) Handles btnSaveLogsToDisk.Click
        SaveLogsToDiskSub()
    End Sub

    Private Sub BtnCheckForUpdates_Click(sender As Object, e As EventArgs) Handles btnCheckForUpdates.Click
        SaveLogsToDiskSub()

        Threading.ThreadPool.QueueUserWorkItem(Sub()
                                                   Dim checkForUpdatesClassObject As New checkForUpdates.CheckForUpdatesClass(Me)
                                                   checkForUpdatesClassObject.CheckForUpdates()
                                               End Sub)
    End Sub

    Private Sub Form1_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        My.Settings.Save()
        WriteLogsToDisk()
        Process.GetCurrentProcess.Kill()
    End Sub

    Private Sub TxtSysLogServerPort_KeyUp(sender As Object, e As KeyEventArgs) Handles txtSysLogServerPort.KeyUp
        If e.KeyCode = Keys.Enter Then
            Dim newPortNumber As Integer

            If Integer.TryParse(txtSysLogServerPort.Text, newPortNumber) Then
                If newPortNumber < 1 Or newPortNumber > 65535 Then
                    MsgBox("The port number must be in the range of 1 - 65535.", MsgBoxStyle.Critical, Text)
                Else
                    MsgBox("New port number accepted. The program will need to be restarted in order for the new port number to be used by the program.", MsgBoxStyle.Information, Text)
                    My.Settings.sysLogPort = newPortNumber
                    My.Settings.Save()

                    WriteLogsToDisk()

                    Process.GetCurrentProcess.Kill()
                End If
            Else
                MsgBox("You must input a valid integer.", MsgBoxStyle.Critical, Text)
            End If
        End If
    End Sub

    Private Sub BtnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        If btnSearch.Text = "Search" Then
            If String.IsNullOrWhiteSpace(txtSearchTerms.Text) Then
                MsgBox("You must provide something to search for.", MsgBoxStyle.Critical, Text)
                Exit Sub
            End If

            Dim strLogText As String
            Dim listOfSearchResults As New List(Of MyDataGridViewRow)
            Dim regexCompiledObject As Regex = Nothing
            Dim MyDataGridRowItem As MyDataGridViewRow

            btnSearch.Enabled = False

            Dim worker As New BackgroundWorker()

            AddHandler worker.DoWork, Sub()
                                          Try
                                              If chkRegExSearch.Checked Then
                                                  If chkRegexCaseInsensitive.Enabled Then
                                                      regexCompiledObject = New Regex(txtSearchTerms.Text, RegexOptions.Compiled + RegexOptions.IgnoreCase)
                                                  Else
                                                      regexCompiledObject = New Regex(txtSearchTerms.Text, RegexOptions.Compiled)
                                                  End If
                                              Else
                                                  regexCompiledObject = New Regex(Regex.Escape(txtSearchTerms.Text), RegexOptions.Compiled + RegexOptions.IgnoreCase)
                                              End If

                                              SyncLock dataGridLockObject
                                                  For Each item As DataGridViewRow In logs.Rows
                                                      MyDataGridRowItem = TryCast(item, MyDataGridViewRow)

                                                      If MyDataGridRowItem IsNot Nothing Then
                                                          strLogText = MyDataGridRowItem.Cells(3).Value

                                                          If regexCompiledObject.IsMatch(strLogText) Then
                                                              With MyDataGridRowItem
                                                                  listOfSearchResults.Add(MakeDataGridRow(.DateObject, .Cells(0).Value.ToString, .Cells(1).Value, .Cells(2).Value, .Cells(3).Value, logs))
                                                              End With
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
                                                          Dim searchResultsWindow As New Ignored_Logs_and_Search_Results With {.Icon = Icon, .LogsToBeDisplayed = listOfSearchResults, .Text = "Search Results"}
                                                          searchResultsWindow.lblCount.Text = $"Number of search results: {listOfSearchResults.Count:N0}"
                                                          searchResultsWindow.ShowDialog(Me)
                                                      Else
                                                          MsgBox("Search terms not found.", MsgBoxStyle.Information, Text)
                                                      End If

                                                      Invoke(Sub() btnSearch.Enabled = True)
                                                  End Sub

            worker.RunWorkerAsync()
        End If
    End Sub

    Private Sub TxtSearchTerms_KeyUp(sender As Object, e As KeyEventArgs) Handles txtSearchTerms.KeyUp
        If e.KeyCode = Keys.Enter Then btnSearch.PerformClick()
    End Sub

    Private intColumnNumber As Integer ' Define intColumnNumber at class level
    Private sortOrder As SortOrder = SortOrder.Descending ' Define soSortOrder at class level
    Private ReadOnly dataGridLockObject As New Object

    Private Sub Logs_ColumnHeaderMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles logs.ColumnHeaderMouseClick
        ' Disable user sorting
        logs.AllowUserToOrderColumns = False

        Dim column As DataGridViewColumn = logs.Columns(e.ColumnIndex)

        If e.ColumnIndex = 0 Then
            If sortOrder = SortOrder.Descending Then
                sortOrder = SortOrder.Ascending
            ElseIf sortOrder = SortOrder.Ascending Then
                sortOrder = SortOrder.Descending
            Else
                sortOrder = SortOrder.Ascending
            End If

            colIPAddress.HeaderCell.SortGlyphDirection = SortOrder.None
            colLog.HeaderCell.SortGlyphDirection = SortOrder.None
            colType.HeaderCell.SortGlyphDirection = SortOrder.None

            SortLogsByDateObject(column.Index, sortOrder)
        Else
            sortOrder = SortOrder.None
        End If
    End Sub

    Private Sub SortLogsByDateObject(columnIndex As Integer, order As SortOrder)
        SyncLock dataGridLockObject
            logs.AllowUserToOrderColumns = False
            logs.Enabled = False

            Dim comparer As New DataGridViewComparer(columnIndex, order)
            Dim rows As MyDataGridViewRow() = logs.Rows.Cast(Of DataGridViewRow)().OfType(Of MyDataGridViewRow)().ToArray()

            Array.Sort(rows, Function(row1, row2) comparer.Compare(row1, row2))

            logs.Rows.Clear()
            logs.Rows.AddRange(rows)

            logs.Enabled = True
            logs.AllowUserToOrderColumns = True
        End SyncLock
    End Sub

    Private Sub IgnoredWordsAndPhrasesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles IgnoredWordsAndPhrasesToolStripMenuItem.Click
        Using ignored As New Ignored_Words_and_Phrases With {.Icon = Icon, .StartPosition = FormStartPosition.CenterParent}
            ignored.ShowDialog(Me)
            regexCache.Clear()
        End Using
    End Sub

    Private Sub ViewIgnoredLogsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ViewIgnoredLogsToolStripMenuItem.Click
        If IgnoredLogs.Count = 0 Then
            MsgBox("There are no recorded ignored log entries to be shown.", MsgBoxStyle.Information, Text)
        Else
            If ignoredLogsWindow Is Nothing Then
                ignoredLogsWindow = New Ignored_Logs_and_Search_Results With {.Icon = Icon, .LogsToBeDisplayed = IgnoredLogs, .Text = "Ignored Logs"}
                ignoredLogsWindow.lblCount.Text = $"Number of ignored logs: {IgnoredLogs.Count:N0}"
                ignoredLogsWindow.Show(Me)
            Else
                ignoredLogsWindow.BringToFront()
            End If
        End If
    End Sub

    Private Sub ClearIgnoredLogsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ClearIgnoredLogsToolStripMenuItem.Click
        If MsgBox("Are you sure you want to clear the ignored logs stored in system memory?", MsgBoxStyle.Question + MsgBoxStyle.YesNo + vbDefaultButton2, Text) = MsgBoxResult.Yes Then
            IgnoredLogs.Clear()
            longNumberOfIgnoredLogs = 0
            IgnoredLogsToolStripMenuItem.Enabled = False
            lblNumberOfIgnoredIncomingLogs.Text = $"Number of ignored incoming logs: {longNumberOfIgnoredLogs:N0}"
        End If
    End Sub

    Private Sub Form1_LocationChanged(sender As Object, e As EventArgs) Handles Me.LocationChanged
        If boolDoneLoading Then My.Settings.windowLocation = Location
    End Sub

    Private Sub ChkRecordIgnoredLogs_Click(sender As Object, e As EventArgs) Handles chkRecordIgnoredLogs.Click
        My.Settings.recordIgnoredLogs = chkRecordIgnoredLogs.Checked
        IgnoredLogsToolStripMenuItem.Visible = chkRecordIgnoredLogs.Checked
        ZerooutIgnoredLogsCounterToolStripMenuItem.Visible = Not chkRecordIgnoredLogs.Checked
        If Not chkRecordIgnoredLogs.Checked Then IgnoredLogs.Clear()
    End Sub

    Private Sub LogsOlderThanToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LogsOlderThanToolStripMenuItem.Click
        Using clearLogsOlderThanObject As New Clear_logs_older_than With {.Icon = Icon, .StartPosition = FormStartPosition.CenterParent}
            clearLogsOlderThanObject.lblLogCount.Text = $"Number of Log Entries: {logs.Rows.Count:N0}"
            clearLogsOlderThanObject.ShowDialog(Me)

            If clearLogsOlderThanObject.boolSuccess Then
                Try
                    Dim dateChosenDate As Date = clearLogsOlderThanObject.dateChosenDate.AddDays(-1)

                    SyncLock dataGridLockObject
                        logs.AllowUserToOrderColumns = False
                        logs.Enabled = False

                        For i As Integer = logs.Rows.Count - 1 To 0 Step -1
                            Dim item As MyDataGridViewRow = CType(logs.Rows(i), MyDataGridViewRow)

                            If item.DateObject.Date <= dateChosenDate.Date Then
                                logs.Rows.RemoveAt(i)
                            End If
                        Next

                        logs.Enabled = True
                        logs.AllowUserToOrderColumns = True
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
        lblNumberOfIgnoredIncomingLogs.Text = $"Number of ignored incoming logs: {longNumberOfIgnoredLogs:N0}"
        ZerooutIgnoredLogsCounterToolStripMenuItem.Enabled = False
    End Sub

    Private Sub ConfigureReplacementsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConfigureReplacementsToolStripMenuItem.Click
        Using ReplacementsWindow As New Replacements With {.Icon = Icon, .StartPosition = FormStartPosition.CenterParent}
            ReplacementsWindow.ShowDialog(Me)
            regexCache.Clear()
        End Using
    End Sub

    Private Sub ChkRegExSearch_Click(sender As Object, e As EventArgs) Handles chkRegExSearch.Click
        chkRegexCaseInsensitive.Enabled = chkRegExSearch.Checked
        chkRegexCaseInsensitive.Checked = False
    End Sub

    Private Sub ConfigureAlternatingColorToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConfigureAlternatingColorToolStripMenuItem.Click
        Using ColorDialog As New ColorDialog()
            If ColorDialog.ShowDialog() = DialogResult.OK Then
                My.Settings.searchColor = ColorDialog.Color

                Dim rowStyle As New DataGridViewCellStyle() With {.BackColor = ColorDialog.Color}
                logs.AlternatingRowsDefaultCellStyle = rowStyle
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
                MsgBox("Free SysLog will now close and restart itself for the imported settings to take effect.", MsgBoxStyle.Information, Text)
                Process.Start(Application.ExecutablePath)
                Process.GetCurrentProcess.Kill()
            End If
        End Using
    End Sub

#Region "-- SysLog Server Code --"
    Sub SysLogThread()
        Try
            Dim ipeRemoteIpEndPoint As New IPEndPoint(IPAddress.Any, 0)
            Dim udpcUDPClient As New UdpClient(My.Settings.sysLogPort)
            Dim sDataRecieve As String
            Dim bBytesRecieved() As Byte
            Dim sFromIP As String

            While True
                bBytesRecieved = udpcUDPClient.Receive(ipeRemoteIpEndPoint)
                sDataRecieve = Encoding.ASCII.GetString(bBytesRecieved)
                sFromIP = ipeRemoteIpEndPoint.Address.ToString

                FillLog(sDataRecieve, sFromIP)

                sDataRecieve = ""
            End While
        Catch ex As Threading.ThreadAbortException
            ' Does nothing
        Catch e As Exception
            Invoke(Sub() MsgBox("Unable to start syslog server, perhaps another instance of this program is running on your system.", MsgBoxStyle.Critical, Text))
        End Try
    End Sub
#End Region
End Class

Public Class SavedData
    Public time, type, ip, log As String
    Public DateObject As Date
End Class

Public Class DataGridViewComparer
    Implements IComparer(Of DataGridViewRow)

    Private ReadOnly intColumnNumber As Integer
    Private ReadOnly soSortOrder As SortOrder

    Public Sub New(columnNumber As Integer, sortOrder As SortOrder)
        intColumnNumber = columnNumber
        soSortOrder = sortOrder
    End Sub

    Public Function Compare(row1 As DataGridViewRow, row2 As DataGridViewRow) As Integer Implements IComparer(Of DataGridViewRow).Compare
        Dim strFirstString, strSecondString As String
        Dim date1, date2 As Date

        ' Get the cell values.
        strFirstString = If(row1.Cells.Count <= intColumnNumber, "", row1.Cells(intColumnNumber).Value?.ToString())
        strSecondString = If(row2.Cells.Count <= intColumnNumber, "", row2.Cells(intColumnNumber).Value?.ToString())

        ' Compare them.
        If intColumnNumber = 0 Then
            If TypeOf row1 Is MyDataGridViewRow AndAlso TypeOf row2 Is MyDataGridViewRow Then
                date1 = DirectCast(row1, MyDataGridViewRow).DateObject
                date2 = DirectCast(row2, MyDataGridViewRow).DateObject
                Return If(soSortOrder = SortOrder.Ascending, Date.Compare(date1, date2), Date.Compare(date2, date1))
            End If
        Else
            Return If(soSortOrder = SortOrder.Ascending, String.Compare(strFirstString, strSecondString), String.Compare(strSecondString, strFirstString))
        End If

        Return 0
    End Function
End Class