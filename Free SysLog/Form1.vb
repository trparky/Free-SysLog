Imports System.IO
Imports System.Net.Sockets
Imports System.Net
Imports System.Text
Imports System.ComponentModel
Imports Microsoft.Win32
Imports System.Text.RegularExpressions

Public Class Form1
    Private sysLogThreadInstance As Threading.Thread
    Private boolDoneLoading As Boolean = False
    Private lockObject As New Object
    Private intPreviousSearchIndex As Integer = -1
    Private m_SortingColumn1, m_SortingColumn2 As ColumnHeader
    Private longNumberOfIgnoredLogs As Long = 0
    Private IgnoredLogs As New List(Of MyListViewItem)

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
        SaveFileDialog.Filter = "JSON Data File|*.json"

        Do
            If SaveFileDialog.ShowDialog() = DialogResult.OK Then
                SyncLock lockObject
                    If File.Exists(SaveFileDialog.FileName) Then File.Delete(SaveFileDialog.FileName)
                    File.Move(My.Settings.logFileLocation, SaveFileDialog.FileName)
                    My.Settings.logFileLocation = SaveFileDialog.FileName
                    My.Settings.Save()
                    Exit Do
                End SyncLock
            End If
        Loop While True
    End Sub

    Private Sub WriteLogsToDisk()
        SyncLock lockObject
            Dim collectionOfSavedData As New List(Of SavedData)

            For Each listViewItem As MyListViewItem In logs.Items
                collectionOfSavedData.Add(New SavedData With {
                                            .time = listViewItem.SubItems(0).Text,
                                            .type = listViewItem.SubItems(1).Text,
                                            .ip = listViewItem.SubItems(2).Text,
                                            .log = listViewItem.SubItems(3).Text,
                                            .DateObject = listViewItem.DateObject
                                          })
            Next

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

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        WriteLogsToDisk()

        Try
            sysLogThreadInstance.Abort()
        Catch ex As Exception
            ' Does nothing.
        End Try

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
                        input = Regex.Replace(input, item.StrReplace, item.StrReplaceWith)
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

        Time.Width = My.Settings.columnTimeSize
        Type.Width = My.Settings.columnTypeSize
        IPAddressCol.Width = My.Settings.columnIPSize
        Log.Width = My.Settings.columnLogSize

        boolDoneLoading = True
        SaveFileDialog.Filter = "JSON Data File|*.json"

        If String.IsNullOrWhiteSpace(My.Settings.logFileLocation) Then
            Do
                If SaveFileDialog.ShowDialog() = DialogResult.OK Then
                    My.Settings.logFileLocation = SaveFileDialog.FileName
                    My.Settings.Save()
                    Exit Do
                Else
                    MsgBox("You must set a location to save the syslog data file to.", MsgBoxStyle.Information, Text)
                End If
            Loop While True
        End If

        If File.Exists(My.Settings.logFileLocation) Then
            Try
                lblLogFileSize.Text = $"Log File Size: {FileSizeToHumanSize(New FileInfo(My.Settings.logFileLocation).Length)}"

                Dim collectionOfSavedData As New List(Of SavedData)

                Using fileStream As New StreamReader(My.Settings.logFileLocation)
                    collectionOfSavedData = Newtonsoft.Json.JsonConvert.DeserializeObject(Of List(Of SavedData))(fileStream.ReadToEnd.Trim)
                End Using

                Dim listOfLogEntries As New List(Of MyListViewItem)

                For Each item As SavedData In collectionOfSavedData
                    listOfLogEntries.Add(item.ToListViewItem())
                Next

                logs.Items.AddRange(listOfLogEntries.ToArray())
                If logs.Items.Count > 0 Then logs.Items.Item(logs.Items.Count - 1).EnsureVisible()
                UpdateLogCount()
            Catch ex As Exception
            End Try
        End If

        sysLogThreadInstance = New Threading.Thread(AddressOf SysLogThread) With {
            .Name = "SysLog Thread",
            .Priority = Threading.ThreadPriority.Lowest
        }
        sysLogThreadInstance.Start()
    End Sub

    Sub SysLogThread()
        Try
            ListenForSyslogs()
        Catch ex As Threading.ThreadAbortException
            ' Does nothing
        End Try
    End Sub

    Private Sub BtnOpenLogLocation_Click(sender As Object, e As EventArgs) Handles btnOpenLogLocation.Click
        SelectFileInWindowsExplorer(My.Settings.logFileLocation)
    End Sub

    Private Sub SelectFileInWindowsExplorer(strFullPath As String)
        If Not String.IsNullOrEmpty(strFullPath) AndAlso IO.File.Exists(strFullPath) Then
            Dim pidlList As IntPtr = NativeMethod.NativeMethods.ILCreateFromPathW(strFullPath)

            If Not pidlList.Equals(IntPtr.Zero) Then
                Try
                    NativeMethod.NativeMethods.SHOpenFolderAndSelectItems(pidlList, 0, IntPtr.Zero, 0)
                Finally
                    NativeMethod.NativeMethods.ILFree(pidlList)
                End Try
            End If
        End If
    End Sub

    Private Sub BtnServerController_Click(sender As Object, e As EventArgs) Handles btnServerController.Click
        If btnServerController.Text = "Stop SysLog Server" Then
            sysLogThreadInstance.Abort()
            btnServerController.Text = "Start SysLog Server"
            sysLogThreadInstance = Nothing
        Else
            btnServerController.Text = "Stop SysLog Server"

            sysLogThreadInstance = New Threading.Thread(AddressOf SysLogThread) With {
                .Name = "SysLog Thread",
                .Priority = Threading.ThreadPriority.Lowest,
                .IsBackground = True
            }
            sysLogThreadInstance.Start()
        End If
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
                    If sSyslog.CaseInsensitiveContains(word) Then

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

        Dim listViewItem As New MyListViewItem(currentDate.ToString)
        listViewItem.SubItems.Add(sPriority)
        listViewItem.SubItems.Add(sFromIp)
        listViewItem.SubItems.Add(ProcessReplacements(sSyslog))
        listViewItem.SubItems.Add("")
        listViewItem.DateObject = currentDate

        If Not boolIgnored Then
            Invoke(Sub()
                       logs.Items.Add(listViewItem)
                       UpdateLogCount()
                       If chkAutoScroll.Checked Then logs.EnsureVisible(logs.Items.Count - 1)
                       btnSaveLogsToDisk.Enabled = True
                   End Sub)
        ElseIf boolIgnored And chkRecordIgnoredLogs.Checked Then
            IgnoredLogs.Add(listViewItem)
        End If

        listViewItem = Nothing
    End Sub

    Private Sub OpenLogViewerWindow()
        If logs.SelectedItems.Count > 0 Then
            Using LogViewer As New Log_Viewer With {.strLogText = logs.SelectedItems(0).SubItems(3).Text, .StartPosition = FormStartPosition.CenterParent, .Icon = Icon}
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
            logs.BeginUpdate()

            For Each item As MyListViewItem In logs.SelectedItems
                logs.Items.Remove(item)
            Next

            logs.EndUpdate()

            UpdateLogCount()
            SaveLogsToDiskSub()
        End If
    End Sub

    Private Sub UpdateLogCount()
        btnClearLog.Enabled = logs.Items.Count <> 0
        NumberOfLogs.Text = $"Number of Log Entries: {logs.Items.Count:N0}"
    End Sub

    Private Sub ChkAutoScroll_Click(sender As Object, e As EventArgs) Handles chkAutoScroll.Click
        My.Settings.autoScroll = chkAutoScroll.Checked
    End Sub

    Private Sub Logs_ColumnWidthChanged(sender As Object, e As ColumnWidthChangedEventArgs) Handles logs.ColumnWidthChanged
        If boolDoneLoading Then
            My.Settings.columnTimeSize = Time.Width
            My.Settings.columnTypeSize = Type.Width
            My.Settings.columnIPSize = IPAddressCol.Width
            My.Settings.columnLogSize = Log.Width
        End If
    End Sub

    Private Sub BtnClearAllLogs_Click(sender As Object, e As EventArgs) Handles btnClearAllLogs.Click
        If MsgBox("Are you sure you want to clear the logs?", MsgBoxStyle.Question + MsgBoxStyle.YesNo + vbDefaultButton2, Text) = MsgBoxResult.Yes Then
            logs.Items.Clear()
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

                    Try
                        sysLogThreadInstance.Abort()
                    Catch ex As Exception
                        ' Does nothing.
                    End Try

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
            Dim boolFound As Boolean = False
            Dim listOfSearchResults As New List(Of MyListViewItem)
            Dim regexCompiledObject As Regex = Nothing

            Try
                If chkRegExSearch.Checked Then
                    If chkRegexCaseInsensitive.Enabled Then
                        regexCompiledObject = New Regex(txtSearchTerms.Text, RegexOptions.Compiled + RegexOptions.IgnoreCase)
                    Else
                        regexCompiledObject = New Regex(txtSearchTerms.Text, RegexOptions.Compiled)
                    End If
                End If

                For Each item As MyListViewItem In logs.Items
                    strLogText = item.SubItems(3).Text

                    If chkRegExSearch.Checked Then
                        If regexCompiledObject.IsMatch(strLogText) Then
                            boolFound = True
                            listOfSearchResults.Add(item.Clone())
                            Debug.WriteLine("found")
                        End If
                    Else
                        If strLogText.CaseInsensitiveContains(txtSearchTerms.Text) And item.Index > intPreviousSearchIndex Then
                            boolFound = True
                            listOfSearchResults.Add(item.Clone())
                            Debug.WriteLine("found")
                        End If
                    End If
                Next

                If boolFound Then
                    Dim searchResultsWindow As New Ignored_Logs_and_Search_Results With {.Icon = Icon, .LogsToBeDisplayed = listOfSearchResults, .Text = "Search Results"}
                    searchResultsWindow.lblCount.Text = $"Number of search results: {listOfSearchResults.Count:N0}"
                    searchResultsWindow.ShowDialog(Me)
                Else
                    MsgBox("Search terms not found.", MsgBoxStyle.Information, Text)
                End If
            Catch ex As ArgumentException
                MsgBox("Malformed RegEx pattern detected, search aborted.", MsgBoxStyle.Critical, Text)
            End Try
        End If
    End Sub

    Private Sub TxtSearchTerms_KeyUp(sender As Object, e As KeyEventArgs) Handles txtSearchTerms.KeyUp
        If e.KeyCode = Keys.Enter Then btnSearch.PerformClick()
    End Sub

    Private Sub Logs_ColumnClick(sender As Object, e As ColumnClickEventArgs) Handles logs.ColumnClick
        Dim new_sorting_column As ColumnHeader = logs.Columns(e.Column)

        ' Figure out the new sorting order.
        Dim sort_order As SortOrder

        If m_SortingColumn2 Is Nothing Then
            ' New column. Sort ascending.
            sort_order = SortOrder.Ascending
        Else
            ' See if this is the same column.
            If new_sorting_column.Equals(m_SortingColumn2) Then
                ' Same column. Switch the sort order.
                sort_order = If(m_SortingColumn2.Text.StartsWith("> "), SortOrder.Descending, SortOrder.Ascending)
            Else
                ' New column. Sort ascending.
                sort_order = SortOrder.Ascending
            End If

            ' Remove the old sort indicator.
            m_SortingColumn2.Text = m_SortingColumn2.Text.Substring(2)
        End If

        ' Display the new sort order.
        m_SortingColumn2 = new_sorting_column
        m_SortingColumn2.Text = If(sort_order = SortOrder.Ascending, $"> {m_SortingColumn2.Text}", $"< {m_SortingColumn2.Text}")

        ' Create a comparer.
        logs.ListViewItemSorter = New ListViewComparer(e.Column, sort_order)

        ' Sort.
        logs.Sort()
    End Sub

    Private Sub IgnoredWordsAndPhrasesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles IgnoredWordsAndPhrasesToolStripMenuItem.Click
        Using ignored As New Ignored_Words_and_Phrases With {.Icon = Icon}
            ignored.ShowDialog()
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
            clearLogsOlderThanObject.ShowDialog(Me)

            If clearLogsOlderThanObject.boolSuccess Then
                Dim dateChosenDate As Date = clearLogsOlderThanObject.dateChosenDate.AddDays(-1)

                logs.BeginUpdate()

                For Each item As MyListViewItem In logs.Items
                    If item.DateObject.Date < dateChosenDate Then
                        item.Remove()
                    End If
                Next

                logs.EndUpdate()

                UpdateLogCount()
                SaveLogsToDiskSub()
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
        End Using
    End Sub

    Private Sub chkRegExSearch_Click(sender As Object, e As EventArgs) Handles chkRegExSearch.Click
        chkRegexCaseInsensitive.Enabled = chkRegExSearch.Checked
        chkRegexCaseInsensitive.Checked = False
    End Sub

#Region "-- SysLog Server Code --"
    Public Sub ListenForSyslogs()
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
        Catch e As Exception
            MsgBox("Unable to start syslog server, perhaps another instance of this program is running on your system.", MsgBoxStyle.Critical, Text)
        End Try
    End Sub
#End Region
End Class

Public Class SavedData
    Public time, type, ip, log As String
    Public DateObject As Date

    Public Function ToListViewItem() As MyListViewItem
        Dim listViewItem As New MyListViewItem(time)
        listViewItem.SubItems.Add(type)
        listViewItem.SubItems.Add(ip)
        listViewItem.SubItems.Add(log)
        listViewItem.SubItems.Add("")
        listViewItem.DateObject = DateObject
        Return listViewItem
    End Function
End Class