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
    Private intColumnNumber As Integer ' Define intColumnNumber at class level
    Private sortOrder As SortOrder = SortOrder.Descending ' Define soSortOrder at class level
    Private ReadOnly dataGridLockObject As New Object
    Private strMutexName As String = "Free SysLog Server"
    Private mutex As Threading.Mutex

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

    Private Sub ChkStartAtUserStartup_Click(sender As Object, e As EventArgs) Handles ChkStartAtUserStartup.Click
        Using registryKey As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Run", True)
            If ChkStartAtUserStartup.Checked Then
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

            LblLogFileSize.Text = $"Log File Size: {FileSizeToHumanSize(New FileInfo(My.Settings.logFileLocation).Length)}"

            BtnSaveLogsToDisk.Enabled = False
        End SyncLock
    End Sub

    Private Sub Form1_ResizeEnd(sender As Object, e As EventArgs) Handles Me.ResizeEnd
        My.Settings.mainWindowSize = Size
        Threading.Thread.Sleep(100)
        If Logs.Rows.Count > 0 Then Logs.FirstDisplayedScrollingRowIndex = Logs.Rows.Count - 1
    End Sub

    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        If boolDoneLoading Then
            My.Settings.boolMaximized = WindowState = FormWindowState.Maximized
            ShowInTaskbar = WindowState <> FormWindowState.Minimized
        End If
    End Sub

    Private Sub ChkAutoSave_Click(sender As Object, e As EventArgs) Handles ChkAutoSave.Click
        SaveTimer.Enabled = ChkAutoSave.Checked
        NumericUpDown.Visible = ChkAutoSave.Checked
        LblAutoSaveLabel.Visible = ChkAutoSave.Checked
        LblAutoSaved.Visible = ChkAutoSave.Checked
    End Sub

    Private Sub SaveTimer_Tick(sender As Object, e As EventArgs) Handles SaveTimer.Tick
        WriteLogsToDisk()
        LblAutoSaved.Text = $"Last Auto-Saved At: {Date.Now:h:mm:ss tt}"
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
        If digits < 0 Then Throw New ArgumentException("The number of digits must be non-negative.", NameOf(digits))

        If digits = 0 Then
            Return Math.Round(value, digits).ToString
        Else
            Return Math.Round(value, digits).ToString("0." & New String("0", digits))
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

    Private Sub SendMessageToSysLogServer(strMessage As String, intPort As Integer)
        Using udpClient As New UdpClient()
            udpClient.Connect("127.0.0.1", intPort)
            Dim data As Byte() = Encoding.ASCII.GetBytes(strMessage)
            udpClient.Send(data, data.Length)
        End Using
    End Sub

    Private Sub NotifyIcon_DoubleClick(sender As Object, e As EventArgs) Handles NotifyIcon.DoubleClick
        WindowState = FormWindowState.Normal
        If Logs.Rows.Count > 0 Then Logs.FirstDisplayedScrollingRowIndex = Logs.Rows.Count - 1
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        mutex = New Threading.Mutex(initiallyOwned:=False, name:=strMutexName, createdNew:=False)

        If Not mutex.WaitOne(0, False) Then
            SendMessageToSysLogServer("restore", My.Settings.sysLogPort)
            Process.GetCurrentProcess.Kill()
        End If

        ColTime.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
        ColTime.HeaderCell.Style.Padding = New Padding(0, 0, 1, 0)
        ColType.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
        ColType.HeaderCell.Style.Padding = New Padding(0, 0, 2, 0)
        ColIPAddress.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
        ColIPAddress.HeaderCell.Style.Padding = New Padding(0, 0, 2, 0)

        ChkRecordIgnoredLogs.Checked = My.Settings.recordIgnoredLogs
        IgnoredLogsToolStripMenuItem.Visible = ChkRecordIgnoredLogs.Checked
        ZerooutIgnoredLogsCounterToolStripMenuItem.Visible = Not ChkRecordIgnoredLogs.Checked
        ChkAutoScroll.Checked = My.Settings.autoScroll
        ChkAutoSave.Checked = My.Settings.autoSave
        ChkConfirmCloseToolStripItem.Checked = My.Settings.boolConfirmClose
        NumericUpDown.Value = My.Settings.autoSaveMinutes
        NumericUpDown.Visible = ChkAutoSave.Checked
        LblAutoSaveLabel.Visible = ChkAutoSave.Checked
        LblAutoSaved.Visible = ChkAutoSave.Checked
        ChkStartAtUserStartup.Checked = DoesStartupEntryExist()
        Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath)
        TxtSysLogServerPort.Text = My.Settings.sysLogPort.ToString
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

        ColTime.Width = My.Settings.columnTimeSize
        ColType.Width = My.Settings.columnTypeSize
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
                           Logs.Rows.Add(MakeDataGridRow(Now, "", "", "", "Loading data and populating data grid... Please Wait.", Logs))
                           LblLogFileSize.Text = $"Log File Size: {FileSizeToHumanSize(New FileInfo(My.Settings.logFileLocation).Length)}"
                       End Sub)

                Dim collectionOfSavedData As New List(Of SavedData)

                Using fileStream As New StreamReader(My.Settings.logFileLocation)
                    collectionOfSavedData = Newtonsoft.Json.JsonConvert.DeserializeObject(Of List(Of SavedData))(fileStream.ReadToEnd.Trim)
                End Using

                Dim listOfLogEntries As New List(Of MyDataGridViewRow)

                For Each item As SavedData In collectionOfSavedData
                    listOfLogEntries.Add(item.MakeDataGridRow(Logs))
                Next

                listOfLogEntries.Add(MakeDataGridRow(Now, Now.ToString, "", "127.0.0.1", "Free SysLog Server Started.", Logs))

                SyncLock dataGridLockObject
                    Invoke(Sub()
                               Logs.Rows.Clear()
                               Logs.Rows.AddRange(listOfLogEntries.ToArray)
                               If Logs.Rows.Count > 0 Then Logs.FirstDisplayedScrollingRowIndex = Logs.Rows.Count - 1
                               UpdateLogCount()
                           End Sub)
                End SyncLock
            Catch ex As Exception
            End Try
        End If
    End Sub

    Private Sub BtnOpenLogLocation_Click(sender As Object, e As EventArgs) Handles BtnOpenLogLocation.Click
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
        If Not regexCache.ContainsKey(pattern) Then regexCache(pattern) = New Regex(pattern, If(boolCaseInsensitive, RegexOptions.Compiled Or RegexOptions.IgnoreCase, RegexOptions.Compiled))
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

                                   If ChkRecordIgnoredLogs.Checked Then
                                       IgnoredLogsToolStripMenuItem.Enabled = True
                                   Else
                                       ZerooutIgnoredLogsCounterToolStripMenuItem.Enabled = True
                                   End If

                                   LblNumberOfIgnoredIncomingLogs.Text = $"Number of ignored incoming logs: {longNumberOfIgnoredLogs:N0}"
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
                           Logs.Rows.Add(MakeDataGridRow(currentDate, currentDate.ToString, sPriority, sFromIp, ProcessReplacements(sSyslog), Logs))
                       End SyncLock

                       UpdateLogCount()
                       BtnSaveLogsToDisk.Enabled = True

                       If ChkAutoScroll.Checked Then Logs.FirstDisplayedScrollingRowIndex = Logs.Rows.Count - 1
                   End Sub)
        ElseIf boolIgnored And ChkRecordIgnoredLogs.Checked Then
            IgnoredLogs.Add(MakeDataGridRow(currentDate, currentDate.ToString, sPriority, sFromIp, ProcessReplacements(sSyslog), Logs))
        End If
    End Sub

    Private Sub OpenLogViewerWindow()
        If Logs.Rows.Count > 0 Then
            Dim selectedRow As MyDataGridViewRow = Logs.Rows(Logs.SelectedCells(0).RowIndex)

            Using LogViewer As New Log_Viewer With {.strLogText = selectedRow.Cells(3).Value, .StartPosition = FormStartPosition.CenterParent, .Icon = Icon}
                LogViewer.LblLogDate.Text = $"Log Date: {selectedRow.Cells(0).Value}"
                LogViewer.LblSource.Text = $"Source IP Address: {selectedRow.Cells(2).Value}"
                LogViewer.ShowDialog(Me)
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

                Logs.Rows.Add(MakeDataGridRow(Now, Now.ToString, "", "127.0.0.1", $"The user deleted {intNumberOfLogsDeleted} log {If(intNumberOfLogsDeleted = 1, "entry", "entries")}.", Logs))
                If ChkAutoScroll.Checked Then Logs.FirstDisplayedScrollingRowIndex = Logs.Rows.Count - 1
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

    Private Sub ChkAutoScroll_Click(sender As Object, e As EventArgs) Handles ChkAutoScroll.Click
        My.Settings.autoScroll = ChkAutoScroll.Checked
    End Sub

    Private Sub Logs_ColumnWidthChanged(sender As Object, e As DataGridViewColumnEventArgs) Handles Logs.ColumnWidthChanged
        If boolDoneLoading Then
            My.Settings.columnTimeSize = ColTime.Width
            My.Settings.columnTypeSize = ColType.Width
            My.Settings.columnIPSize = ColIPAddress.Width
            My.Settings.columnLogSize = ColLog.Width
        End If
    End Sub

    Private Sub BtnClearAllLogs_Click(sender As Object, e As EventArgs) Handles BtnClearAllLogs.Click
        If MsgBox("Are you sure you want to clear the logs?", MsgBoxStyle.Question + MsgBoxStyle.YesNo + vbDefaultButton2, Text) = MsgBoxResult.Yes Then
            SyncLock dataGridLockObject
                Dim intOldCount As Integer = Logs.Rows.Count
                Logs.Rows.Clear()
                Logs.Rows.Add(MakeDataGridRow(Now, Now.ToString, "", "127.0.0.1", $"The user deleted {intOldCount} log {If(intOldCount = 1, "entry", "entries")}.", Logs))
                If ChkAutoScroll.Checked Then Logs.FirstDisplayedScrollingRowIndex = Logs.Rows.Count - 1
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

    Private Sub Form1_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        If My.Settings.boolConfirmClose AndAlso MsgBox("Are you sure you want to close Free SysLog?", MsgBoxStyle.YesNo + MsgBoxStyle.Question + vbDefaultButton2, Text) = MsgBoxResult.No Then
            e.Cancel = True
            Exit Sub
        End If

        My.Settings.Save()
        WriteLogsToDisk()
        mutex.ReleaseMutex()
        Process.GetCurrentProcess.Kill()
    End Sub

    Private Sub TxtSysLogServerPort_KeyUp(sender As Object, e As KeyEventArgs) Handles TxtSysLogServerPort.KeyUp
        If e.KeyCode = Keys.Enter Then
            Dim newPortNumber As Integer

            If Integer.TryParse(TxtSysLogServerPort.Text, newPortNumber) Then
                If newPortNumber < 1 Or newPortNumber > 65535 Then
                    MsgBox("The port number must be in the range of 1 - 65535.", MsgBoxStyle.Critical, Text)
                Else
                    MsgBox("New port number accepted. The program will need to be restarted in order for the new port number to be used by the program.", MsgBoxStyle.Information, Text)
                    My.Settings.sysLogPort = newPortNumber
                    My.Settings.Save()

                    WriteLogsToDisk()

                    mutex.ReleaseMutex()
                    Process.GetCurrentProcess.Kill()
                End If
            Else
                MsgBox("You must input a valid integer.", MsgBoxStyle.Critical, Text)
            End If
        End If
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
                                                          strLogText = MyDataGridRowItem.Cells(3).Value

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
                                                          Dim searchResultsWindow As New Ignored_Logs_and_Search_Results With {.Icon = Icon, .LogsToBeDisplayed = listOfSearchResults, .Text = "Search Results"}
                                                          searchResultsWindow.LblCount.Text = $"Number of search results: {listOfSearchResults.Count:N0}"
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

        If e.ColumnIndex = 0 Then
            If sortOrder = SortOrder.Descending Then
                sortOrder = SortOrder.Ascending
            ElseIf sortOrder = SortOrder.Ascending Then
                sortOrder = SortOrder.Descending
            Else
                sortOrder = SortOrder.Ascending
            End If

            ColIPAddress.HeaderCell.SortGlyphDirection = SortOrder.None
            ColLog.HeaderCell.SortGlyphDirection = SortOrder.None
            ColType.HeaderCell.SortGlyphDirection = SortOrder.None

            SortLogsByDateObject(column.Index, sortOrder)
        Else
            sortOrder = SortOrder.None
        End If
    End Sub

    Private Sub SortLogsByDateObject(columnIndex As Integer, order As SortOrder)
        SyncLock dataGridLockObject
            Logs.AllowUserToOrderColumns = False
            Logs.Enabled = False

            Dim comparer As New DataGridViewComparer(columnIndex, order)
            Dim rows As MyDataGridViewRow() = Logs.Rows.Cast(Of DataGridViewRow)().OfType(Of MyDataGridViewRow)().ToArray()

            Array.Sort(rows, Function(row1, row2) comparer.Compare(row1, row2))

            Logs.Rows.Clear()
            Logs.Rows.AddRange(rows)

            Logs.Enabled = True
            Logs.AllowUserToOrderColumns = True
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
                ignoredLogsWindow.LblCount.Text = $"Number of ignored logs: {IgnoredLogs.Count:N0}"
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
            LblNumberOfIgnoredIncomingLogs.Text = $"Number of ignored incoming logs: {longNumberOfIgnoredLogs:N0}"
        End If
    End Sub

    Private Sub Form1_LocationChanged(sender As Object, e As EventArgs) Handles Me.LocationChanged
        If boolDoneLoading Then My.Settings.windowLocation = Location
    End Sub

    Private Sub ChkRecordIgnoredLogs_Click(sender As Object, e As EventArgs) Handles ChkRecordIgnoredLogs.Click
        My.Settings.recordIgnoredLogs = ChkRecordIgnoredLogs.Checked
        IgnoredLogsToolStripMenuItem.Visible = ChkRecordIgnoredLogs.Checked
        ZerooutIgnoredLogsCounterToolStripMenuItem.Visible = Not ChkRecordIgnoredLogs.Checked
        If Not ChkRecordIgnoredLogs.Checked Then IgnoredLogs.Clear()
    End Sub

    Private Sub LogsOlderThanToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LogsOlderThanToolStripMenuItem.Click
        Using clearLogsOlderThanObject As New Clear_logs_older_than With {.Icon = Icon, .StartPosition = FormStartPosition.CenterParent}
            clearLogsOlderThanObject.LblLogCount.Text = $"Number of Log Entries: {Logs.Rows.Count:N0}"
            clearLogsOlderThanObject.ShowDialog(Me)

            If clearLogsOlderThanObject.boolSuccess Then
                Try
                    Dim dateChosenDate As Date = clearLogsOlderThanObject.dateChosenDate.AddDays(-1)

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
                        Logs.Rows.Add(MakeDataGridRow(Now, Now.ToString, "", "127.0.0.1", $"The user deleted {intCountDifference} log {If(intCountDifference = 1, "entry", "entries")}.", Logs))
                        If ChkAutoScroll.Checked Then Logs.FirstDisplayedScrollingRowIndex = Logs.Rows.Count - 1
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
        Using ReplacementsWindow As New Replacements With {.Icon = Icon, .StartPosition = FormStartPosition.CenterParent}
            ReplacementsWindow.ShowDialog(Me)
            regexCache.Clear()
        End Using
    End Sub

    Private Sub ConfigureAlternatingColorToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConfigureAlternatingColorToolStripMenuItem.Click
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
                MsgBox("Free SysLog will now close and restart itself for the imported settings to take effect.", MsgBoxStyle.Information, Text)
                Process.Start(Application.ExecutablePath)
                mutex.ReleaseMutex()
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
                Logs.Rows.Add(MakeDataGridRow(Now, Now.ToString, "", "127.0.0.1", $"The user deleted {intCountDifference} log {If(intCountDifference = 1, "entry", "entries")}.", Logs))
                If ChkAutoScroll.Checked Then Logs.FirstDisplayedScrollingRowIndex = Logs.Rows.Count - 1
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

    Private Sub ChkConfirmCloseToolStripItem_Click(sender As Object, e As EventArgs) Handles ChkConfirmCloseToolStripItem.Click
        My.Settings.boolConfirmClose = ChkConfirmCloseToolStripItem.Checked
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
                sDataRecieve = Encoding.UTF8.GetString(bBytesRecieved)
                sFromIP = ipeRemoteIpEndPoint.Address.ToString

                If sDataRecieve.Trim.Equals("restore", StringComparison.OrdinalIgnoreCase) Then
                    Invoke(Sub() RestoreWindowAfterReceivingRestoreCommand())
                Else
                    FillLog(sDataRecieve, sFromIP)
                End If

                sDataRecieve = ""
            End While
        Catch ex As Threading.ThreadAbortException
            ' Does nothing
        Catch e As Exception
            Invoke(Sub() MsgBox("Unable to start syslog server, perhaps another instance of this program is running on your system.", MsgBoxStyle.Critical, Text))
        End Try
    End Sub

    Private Async Sub RestoreWindowAfterReceivingRestoreCommand()
        WindowState = FormWindowState.Normal
        Await Threading.Tasks.Task.Delay(100)
        If Logs.Rows.Count > 0 Then Logs.FirstDisplayedScrollingRowIndex = Logs.Rows.Count - 1
    End Sub
#End Region
End Class