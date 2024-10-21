Imports System.ComponentModel
Imports System.IO
Imports System.Reflection
Imports System.Text.RegularExpressions
Imports System.Xml.Serialization
Imports Free_SysLog.SupportCode

Public Class IgnoredLogsAndSearchResults
    Public LogsToBeDisplayed As List(Of MyDataGridViewRow)
    Public WindowDisplayMode As IgnoreOrSearchWindowDisplayMode
    Private m_SortingColumn1, m_SortingColumn2 As ColumnHeader
    Private boolDoneLoading As Boolean = False
    Public MainProgramForm As Form1

    Public boolLoadExternalData As Boolean = False
    Public strFileToLoad As String

    Private intColumnNumber As Integer ' Define intColumnNumber at class level
    Private sortOrder As SortOrder = SortOrder.Ascending ' Define soSortOrder at class level
    Private ReadOnly dataGridLockObject As New Object

    Private Sub OpenLogViewerWindow()
        If Logs.Rows.Count > 0 And Logs.SelectedCells.Count > 0 Then
            Dim selectedRow As MyDataGridViewRow = Logs.Rows(Logs.SelectedCells(0).RowIndex)
            Dim strLogText As String = selectedRow.Cells(ColumnIndex_LogText).Value
            Dim strRawLogText As String = If(String.IsNullOrWhiteSpace(selectedRow.RawLogData), selectedRow.Cells(ColumnIndex_LogText).Value, selectedRow.RawLogData)

            Using LogViewerInstance As New LogViewer With {.strRawLogText = strRawLogText, .strLogText = strLogText, .StartPosition = FormStartPosition.CenterParent, .Icon = Icon}
                LogViewerInstance.LblLogDate.Text = $"Log Date: {selectedRow.Cells(ColumnIndex_ComputedTime).Value}"
                LogViewerInstance.LblSource.Text = $"Source IP Address: {selectedRow.Cells(ColumnIndex_IPAddress).Value}"

                If Not String.IsNullOrEmpty(selectedRow.AlertText) Then
                    LogViewerInstance.lblAlertText.Text = $"Alert Text: {selectedRow.AlertText}"
                Else
                    LogViewerInstance.lblAlertText.Visible = False
                End If

                LogViewerInstance.ShowDialog(Me)
            End Using
        End If
    End Sub

    Private Sub Logs_ColumnHeaderMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles Logs.ColumnHeaderMouseClick
        ' Disable user sorting
        Logs.AllowUserToOrderColumns = False

        Dim column As DataGridViewColumn = Logs.Columns(e.ColumnIndex)

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
            Logs.AllowUserToOrderColumns = False
            Logs.Enabled = False

            Dim comparer As New DataGridViewComparer(columnIndex, order)
            Dim rows As MyDataGridViewRow() = Logs.Rows.Cast(Of DataGridViewRow).OfType(Of MyDataGridViewRow)().ToArray()

            Array.Sort(rows, Function(row1 As MyDataGridViewRow, row2 As MyDataGridViewRow) comparer.Compare(row1, row2))

            Logs.Rows.Clear()
            Logs.Rows.AddRange(rows)

            Logs.Enabled = True
            Logs.AllowUserToOrderColumns = True
        End SyncLock
    End Sub

    Private Sub Logs_DoubleClick(sender As Object, e As EventArgs) Handles Logs.DoubleClick
        OpenLogViewerWindow()
    End Sub

    Private Sub Ignored_Logs_and_Search_Results_ResizeEnd(sender As Object, e As EventArgs) Handles Me.ResizeEnd
        If boolDoneLoading Then
            If WindowDisplayMode = IgnoreOrSearchWindowDisplayMode.ignored Then
                My.Settings.ignoredWindowSize = Size
            ElseIf WindowDisplayMode = IgnoreOrSearchWindowDisplayMode.search Then
                My.Settings.searchWindowSize = Size
            ElseIf WindowDisplayMode = IgnoreOrSearchWindowDisplayMode.viewer Then
                My.Settings.logFileViewerSize = Size
            End If
        End If
    End Sub

    Private Sub Ignored_Logs_and_Search_Results_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If WindowDisplayMode = IgnoreOrSearchWindowDisplayMode.ignored Then
            BtnClearIgnoredLogs.Visible = True
            BtnViewMainWindow.Visible = True
        ElseIf WindowDisplayMode = IgnoreOrSearchWindowDisplayMode.viewer Then
            BtnExport.Visible = False
            BtnViewMainWindow.Visible = True

            LblSearchLabel.Visible = True
            TxtSearchTerms.Visible = True
            ChkRegExSearch.Visible = True
            ChkCaseInsensitiveSearch.Visible = True
            BtnSearch.Visible = True
        End If

        If WindowDisplayMode = IgnoreOrSearchWindowDisplayMode.ignored Then
            Size = My.Settings.ignoredWindowSize
            Location = VerifyWindowLocation(My.Settings.ignoredWindowLocation, Me)
        ElseIf WindowDisplayMode = IgnoreOrSearchWindowDisplayMode.search Then
            Size = My.Settings.searchWindowSize
            Location = VerifyWindowLocation(My.Settings.searchWindowLocation, Me)
        ElseIf WindowDisplayMode = IgnoreOrSearchWindowDisplayMode.viewer Then
            Size = My.Settings.logFileViewerSize
            Location = VerifyWindowLocation(My.Settings.logFileViewerLocation, Me)
        End If

        ColTime.Width = My.Settings.columnTimeSize
        colServerTime.Width = My.Settings.ServerTimeWidth
        colLogType.Width = My.Settings.LogTypeWidth
        ColIPAddress.Width = My.Settings.columnIPSize
        ColHostname.Width = My.Settings.HostnameWidth
        ColRemoteProcess.Width = My.Settings.RemoteProcessHeaderSize
        ColLog.Width = My.Settings.columnLogSize

        ColHostname.Visible = My.Settings.boolShowHostnameColumn
        colServerTime.Visible = My.Settings.boolShowServerTimeColumn
        colLogType.Visible = My.Settings.boolShowLogTypeColumn

        ColTime.HeaderCell.SortGlyphDirection = SortOrder.Ascending

        Dim flags As BindingFlags = BindingFlags.NonPublic Or BindingFlags.Instance Or BindingFlags.SetProperty
        Dim propInfo As PropertyInfo = GetType(DataGridView).GetProperty("DoubleBuffered", flags)
        propInfo?.SetValue(Logs, True, Nothing)

        Logs.AlternatingRowsDefaultCellStyle = New DataGridViewCellStyle() With {.BackColor = My.Settings.searchColor}
        Logs.DefaultCellStyle = New DataGridViewCellStyle() With {.WrapMode = DataGridViewTriState.True}
        ColLog.DefaultCellStyle = New DataGridViewCellStyle() With {.WrapMode = DataGridViewTriState.True}

        If WindowDisplayMode <> IgnoreOrSearchWindowDisplayMode.viewer Then
            Logs.Rows.AddRange(LogsToBeDisplayed.ToArray())
            SortLogsByDateObject(0, SortOrder.Ascending)

            If WindowDisplayMode = IgnoreOrSearchWindowDisplayMode.ignored Then
                LblCount.Text = $"Number of ignored logs: {LogsToBeDisplayed.Count:N0}"
            Else
                LblCount.Text = $"Number of search results: {LogsToBeDisplayed.Count:N0}"
            End If
        End If

        If WindowDisplayMode = IgnoreOrSearchWindowDisplayMode.viewer AndAlso boolLoadExternalData AndAlso Not String.IsNullOrEmpty(strFileToLoad) Then
            Dim worker As New BackgroundWorker()
            AddHandler worker.DoWork, Sub() LoadData(strFileToLoad)
            AddHandler worker.RunWorkerCompleted, Sub()
                                                      LblCount.Text = $"Number of logs: {Logs.Rows.Count:N0}"
                                                      boolDoneLoading = True
                                                      SortLogsByDateObject(0, SortOrder.Ascending)
                                                  End Sub
            worker.RunWorkerAsync()
        End If

        boolDoneLoading = True
    End Sub

    Public Sub AddIgnoredDatagrid(ItemToAdd As MyDataGridViewRow, BoolAutoScroll As Boolean)
        Invoke(Sub()
                   Logs.Rows.Add(ItemToAdd)
                   If BoolAutoScroll Then Logs.FirstDisplayedScrollingRowIndex = Logs.Rows.Count - 1
                   LblCount.Text = $"Number of ignored logs: {LogsToBeDisplayed.Count:N0}"
               End Sub)
    End Sub

    Protected Overrides Sub WndProc(ByRef m As Message)
        Const WM_EXITSIZEMOVE As Integer = &H232

        MyBase.WndProc(m)

        If m.Msg = WM_EXITSIZEMOVE AndAlso boolDoneLoading Then
            If WindowDisplayMode = IgnoreOrSearchWindowDisplayMode.ignored Then
                My.Settings.ignoredWindowLocation = Location
            ElseIf WindowDisplayMode = IgnoreOrSearchWindowDisplayMode.search Then
                My.Settings.searchWindowLocation = Location
            ElseIf WindowDisplayMode = IgnoreOrSearchWindowDisplayMode.viewer Then
                My.Settings.logFileViewerLocation = Location
            End If
        End If
    End Sub

    Private Sub BtnClearIgnoredLogs_Click(sender As Object, e As EventArgs) Handles BtnClearIgnoredLogs.Click
        If TypeOf parentForm Is Form1 AndAlso MsgBox("Are you sure you want to clear the ignored logs stored in system memory?", MsgBoxStyle.Question + MsgBoxStyle.YesNo + vbDefaultButton2, Text) = MsgBoxResult.Yes Then
            Logs.Rows.Clear()
            LblCount.Text = "Number of ignored logs: 0"
            parentForm.ClearIgnoredLogs()
        End If
    End Sub

    Private Sub BtnExport_Click(sender As Object, e As EventArgs) Handles BtnExport.Click
        SaveFileDialog.Filter = "CSV (Comma Separated Value)|*.csv|JSON File|*.json|XML File|*.xml"
        SaveFileDialog.Title = "Export Data..."

        If SaveFileDialog.ShowDialog() = DialogResult.OK Then
            Dim fileInfo As New FileInfo(SaveFileDialog.FileName)

            Dim collectionOfSavedData As New List(Of SavedData)
            Dim myItem As MyDataGridViewRow
            Dim csvStringBuilder As New Text.StringBuilder
            Dim savedData As SavedData
            Dim strLogType, strTime, strSourceIP, strHeader, strLogText, strAlerted, strHostname, strRemoteProcess, strServerTime, strFileName As String

            If fileInfo.Extension.Equals(".csv", StringComparison.OrdinalIgnoreCase) Then
                If ColFileName.Visible Then
                    csvStringBuilder.AppendLine("Time,Server Time,Log Type,IP Address,Hostname,Remote Process,Log Text,Alerted,File Name")
                Else
                    csvStringBuilder.AppendLine("Time,Server Time,Log Type,IP Address,Hostname,Remote Process,Log Text,Alerted")
                End If
            End If

            For Each item As DataGridViewRow In Logs.Rows
                If Not String.IsNullOrWhiteSpace(item.Cells(ColumnIndex_ComputedTime).Value) Then
                    myItem = DirectCast(item, MyDataGridViewRow)

                    If fileInfo.Extension.Equals(".csv", StringComparison.OrdinalIgnoreCase) Then
                        With myItem
                            strTime = SanitizeForCSV(.Cells(ColumnIndex_ComputedTime).Value)
                            strLogType = SanitizeForCSV(.Cells(ColumnIndex_LogType).Value)
                            strSourceIP = SanitizeForCSV(.Cells(ColumnIndex_IPAddress).Value)
                            strHeader = SanitizeForCSV(.Cells(ColumnIndex_RemoteProcess).Value)
                            strLogText = SanitizeForCSV(.Cells(ColumnIndex_LogText).Value)
                            strHostname = SanitizeForCSV(.Cells(ColumnIndex_Hostname).Value)
                            strRemoteProcess = SanitizeForCSV(.Cells(ColumnIndex_RemoteProcess).Value)
                            strServerTime = SanitizeForCSV(.Cells(ColumnIndex_ServerTime).Value)
                            strAlerted = If(.BoolAlerted, "Yes", "No")
                        End With

                        If ColFileName.Visible Then
                            strFileName = SanitizeForCSV(myItem.Cells(ColumnIndex_FileName).Value)
                            csvStringBuilder.AppendLine($"{strTime},{strServerTime},{strLogType},{strSourceIP},{strHostname},{strRemoteProcess},{strLogText},{strAlerted},{strFileName}")
                        Else
                            csvStringBuilder.AppendLine($"{strTime},{strServerTime},{strLogType},{strSourceIP},{strHostname},{strRemoteProcess},{strLogText},{strAlerted}")
                        End If
                    Else
                        savedData = New SavedData With {
                                                    .time = myItem.Cells(ColumnIndex_ComputedTime).Value,
                                                    .ServerDate = myItem.Cells(ColumnIndex_ServerTime).Value,
                                                    .logType = myItem.Cells(ColumnIndex_LogType).Value,
                                                    .ip = myItem.Cells(ColumnIndex_IPAddress).Value,
                                                    .hostname = myItem.Cells(ColumnIndex_Hostname).Value,
                                                    .appName = myItem.Cells(ColumnIndex_RemoteProcess).Value,
                                                    .log = myItem.Cells(ColumnIndex_LogText).Value,
                                                    .DateObject = myItem.DateObject,
                                                    .BoolAlerted = myItem.BoolAlerted,
                                                    .rawLogData = myItem.RawLogData
                                              }
                        If ColFileName.Visible Then savedData.fileName = myItem.Cells(ColumnIndex_FileName).Value
                        collectionOfSavedData.Add(savedData)
                    End If
                End If
            Next

            Using fileStream As New StreamWriter(SaveFileDialog.FileName)
                If fileInfo.Extension.Equals(".xml", StringComparison.OrdinalIgnoreCase) Then
                    Dim xmlSerializerObject As New XmlSerializer(collectionOfSavedData.GetType)
                    xmlSerializerObject.Serialize(fileStream, collectionOfSavedData)
                ElseIf fileInfo.Extension.Equals(".json", StringComparison.OrdinalIgnoreCase) Then
                    fileStream.Write(Newtonsoft.Json.JsonConvert.SerializeObject(collectionOfSavedData, Newtonsoft.Json.Formatting.Indented))
                ElseIf fileInfo.Extension.Equals(".csv", StringComparison.OrdinalIgnoreCase) Then
                    fileStream.Write(csvStringBuilder.ToString.Trim)
                End If
            End Using

            If MsgBox($"Data exported to ""{SaveFileDialog.FileName}"" successfully.{vbCrLf}{vbCrLf}Do you want to open Windows Explorer to the location of the file?", MsgBoxStyle.Question + MsgBoxStyle.YesNo + vbDefaultButton2, Text) = MsgBoxResult.Yes Then
                SelectFileInWindowsExplorer(SaveFileDialog.FileName)
            End If
        End If
    End Sub

    Private Sub BtnViewMainWindow_Click(sender As Object, e As EventArgs) Handles BtnViewMainWindow.Click
        If TypeOf parentForm Is Form1 Then
            parentForm.RestoreWindow()
            BtnViewMainWindow.Enabled = False
            BringToFront()
        End If
    End Sub

    Private Sub LogsContextMenu_Opening(sender As Object, e As CancelEventArgs) Handles LogsContextMenu.Opening
        If WindowDisplayMode = IgnoreOrSearchWindowDisplayMode.viewer AndAlso boolLoadExternalData AndAlso Not String.IsNullOrEmpty(strFileToLoad) Then
            ExportSelectedLogsToolStripMenuItem.Visible = True
            CopyLogTextToolStripMenuItem.Visible = False
            CreateAlertToolStripMenuItem.Visible = False
            OpenLogFileForViewingToolStripMenuItem.Visible = False
        Else
            ExportSelectedLogsToolStripMenuItem.Visible = False
            CopyLogTextToolStripMenuItem.Visible = True
            CreateAlertToolStripMenuItem.Visible = True
            OpenLogFileForViewingToolStripMenuItem.Visible = True
        End If
    End Sub

    Private Sub CopyLogTextToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyLogTextToolStripMenuItem.Click
        Dim selectedRow As MyDataGridViewRow = Logs.Rows(Logs.SelectedCells(0).RowIndex)
        CopyTextToWindowsClipboard(selectedRow.Cells(ColumnIndex_LogText).Value, Text)
    End Sub

    Private Function MakeDataGridRow(dateObject As Date, strLog As String, ByRef dataGrid As DataGridView) As MyDataGridViewRow
        Using MyDataGridViewRow As New MyDataGridViewRow
            With MyDataGridViewRow
                .CreateCells(dataGrid)
                .Cells(ColumnIndex_ComputedTime).Value = Now.ToString
                .Cells(ColumnIndex_LogText).Value = strLog
            End With

            Return MyDataGridViewRow
        End Using
    End Function

    Private Sub LoadData(strFileName As String)
        Invoke(Sub() Logs.Rows.Add(MakeDataGridRow(Now, "Loading data and populating data grid... Please Wait.", Logs)))

        Dim collectionOfSavedData As New List(Of SavedData)

        Try
            Using fileStream As New StreamReader(strFileName)
                collectionOfSavedData = Newtonsoft.Json.JsonConvert.DeserializeObject(Of List(Of SavedData))(fileStream.ReadToEnd.Trim, JSONDecoderSettingsForSettingsFiles)
            End Using

            If collectionOfSavedData.Count > 0 Then
                Dim listOfLogEntries As New List(Of MyDataGridViewRow)

                For Each item As SavedData In collectionOfSavedData
                    listOfLogEntries.Add(item.MakeDataGridRow(Logs, GetMinimumHeight(item.log, Logs.DefaultCellStyle.Font, ColLog.Width)))
                Next

                Invoke(Sub()
                           Logs.Rows.Clear()
                           Logs.Rows.AddRange(listOfLogEntries.ToArray)
                       End Sub)
            End If
        Catch ex As Newtonsoft.Json.JsonSerializationException
            SyslogParser.AddToLogList(Nothing, Net.IPAddress.Loopback.ToString, $"Exception Type: {ex.GetType}{vbCrLf}Exception Message: {ex.Message}{vbCrLf}{vbCrLf}Exception Stack Trace{vbCrLf}{ex.StackTrace}")
            MsgBox("There was an error decoding JSON data.", MsgBoxStyle.Critical, Text)
        Catch ex As Newtonsoft.Json.JsonReaderException
            SyslogParser.AddToLogList(Nothing, Net.IPAddress.Loopback.ToString, $"Exception Type: {ex.GetType}{vbCrLf}Exception Message: {ex.Message}{vbCrLf}{vbCrLf}Exception Stack Trace{vbCrLf}{ex.StackTrace}")
            MsgBox("There was an error decoding JSON data.", MsgBoxStyle.Critical, Text)
        End Try
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

    Private Sub Ignored_Logs_and_Search_Results_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        IgnoredLogsAndSearchResultsInstance = Nothing
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
                                                      strLogText = MyDataGridRowItem.Cells(ColumnIndex_RemoteProcess).Value

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
                                                      Dim searchResultsWindow As New IgnoredLogsAndSearchResults(Me) With {.MainProgramForm = MainProgramForm, .Icon = Icon, .LogsToBeDisplayed = listOfSearchResults, .Text = "Search Results", .WindowDisplayMode = IgnoreOrSearchWindowDisplayMode.search}
                                                      searchResultsWindow.ShowDialog(Me)
                                                  Else
                                                      MsgBox("Search terms not found.", MsgBoxStyle.Information, Text)
                                                  End If

                                                  Invoke(Sub() BtnSearch.Enabled = True)
                                              End Sub

        worker.RunWorkerAsync()
    End Sub

    Private Sub OpenLogFileForViewingToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenLogFileForViewingToolStripMenuItem.Click
        Dim logFileViewer As New IgnoredLogsAndSearchResults(Me) With {.MainProgramForm = MainProgramForm, .Icon = Icon, .Text = "Log File Viewer", .WindowDisplayMode = IgnoreOrSearchWindowDisplayMode.viewer, .strFileToLoad = Path.Combine(strPathToDataBackupFolder, Logs.SelectedRows(0).Cells(ColumnIndex_FileName).Value), .boolLoadExternalData = True}
        logFileViewer.Show(Me)
    End Sub

    Private Sub TxtSearchTerms_KeyDown(sender As Object, e As KeyEventArgs) Handles TxtSearchTerms.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            BtnSearch.PerformClick()
        End If
    End Sub

    Private Sub ExportSelectedLogsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExportSelectedLogsToolStripMenuItem.Click
        DataHandling.ExportSelectedLogs(Logs.SelectedRows)
    End Sub

    Private Sub IgnoredLogsAndSearchResults_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        If boolDoneLoading Then
            For Each item As MyDataGridViewRow In Logs.Rows
                item.Height = GetMinimumHeight(item.Cells(ColumnIndex_LogText).Value, Logs.DefaultCellStyle.Font, ColLog.Width)
            Next
        End If
    End Sub
End Class