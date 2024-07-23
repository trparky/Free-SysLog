Imports System.ComponentModel
Imports System.IO
Imports System.Reflection
Imports System.Text.RegularExpressions
Imports System.Xml.Serialization

Public Class IgnoredLogsAndSearchResults
    Public LogsToBeDisplayed As List(Of MyDataGridViewRow)
    Public WindowDisplayMode As IgnoreOrSearchWindowDisplayMode
    Private m_SortingColumn1, m_SortingColumn2 As ColumnHeader
    Private boolDoneLoading As Boolean = False

    Public boolLoadExternalData As Boolean = False
    Public strFileToLoad As String

    Private intColumnNumber As Integer ' Define intColumnNumber at class level
    Private sortOrder As SortOrder = SortOrder.Ascending ' Define soSortOrder at class level
    Private ReadOnly dataGridLockObject As New Object

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
        ColIPAddress.Width = My.Settings.columnIPSize
        ColLog.Width = My.Settings.columnLogSize
        ColAlerts.Visible = My.Settings.boolShowAlertedColumn

        ColTime.HeaderCell.SortGlyphDirection = SortOrder.Ascending

        Dim flags As BindingFlags = BindingFlags.NonPublic Or BindingFlags.Instance Or BindingFlags.SetProperty
        Dim propInfo As PropertyInfo = GetType(DataGridView).GetProperty("DoubleBuffered", flags)
        propInfo?.SetValue(Logs, True, Nothing)

        Logs.AlternatingRowsDefaultCellStyle = New DataGridViewCellStyle() With {.BackColor = My.Settings.searchColor}
        Logs.DefaultCellStyle = New DataGridViewCellStyle() With {.WrapMode = DataGridViewTriState.True}
        ColLog.DefaultCellStyle = New DataGridViewCellStyle() With {.WrapMode = DataGridViewTriState.True}

        If WindowDisplayMode <> IgnoreOrSearchWindowDisplayMode.viewer Then
            Logs.Rows.AddRange(LogsToBeDisplayed.ToArray())

            If WindowDisplayMode = IgnoreOrSearchWindowDisplayMode.ignored Then
                LblCount.Text = $"Number of ignored logs: {LogsToBeDisplayed.Count:N0}"
            Else
                LblCount.Text = $"Number of search results: {LogsToBeDisplayed.Count:N0}"
            End If
        End If

        If WindowDisplayMode = IgnoreOrSearchWindowDisplayMode.viewer AndAlso boolLoadExternalData AndAlso Not String.IsNullOrEmpty(strFileToLoad) Then
            LoadData(strFileToLoad)
            LblCount.Text = $"Number of logs: {Logs.Rows.Count:N0}"
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

    Private Sub Ignored_Logs_and_Search_Results_LocationChanged(sender As Object, e As EventArgs) Handles Me.LocationChanged
        If boolDoneLoading Then
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
            Dim strTime, strSourceIP, strLogText, strAlerted, strFileName As String

            If fileInfo.Extension.Equals(".csv", StringComparison.OrdinalIgnoreCase) Then
                If ColFileName.Visible Then
                    csvStringBuilder.AppendLine("Time,Source IP,Log Text,Alerted,File Name")
                Else
                    csvStringBuilder.AppendLine("Time,Source IP,Log Text,Alerted")
                End If
            End If

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

                        If ColFileName.Visible Then
                            strFileName = SanitizeForCSV(myItem.Cells(4).Value)
                            csvStringBuilder.AppendLine($"{strTime},{strSourceIP},{strLogText},{strAlerted},{strFileName}")
                        Else
                            csvStringBuilder.AppendLine($"{strTime},{strSourceIP},{strLogText},{strAlerted}")
                        End If
                    Else
                        savedData = New SavedData With {
                                                .time = myItem.Cells(0).Value,
                                                .ip = myItem.Cells(1).Value,
                                                .log = myItem.Cells(2).Value,
                                                .DateObject = myItem.DateObject,
                                                .BoolAlerted = myItem.BoolAlerted
                                              }
                        If ColFileName.Visible Then savedData.fileName = myItem.Cells(4).Value
                        collectionOfSavedData.Add(savedData)
                    End If
                End If
            Next

            Using fileStream As New StreamWriter(SaveFileDialog.FileName)
                If fileInfo.Extension.Equals(".xml", StringComparison.OrdinalIgnoreCase) Then
                    Dim xmlSerializerObject As New XmlSerializer(collectionOfSavedData.GetType)
                    xmlSerializerObject.Serialize(fileStream, collectionOfSavedData)
                ElseIf fileInfo.Extension.Equals(".json", StringComparison.OrdinalIgnoreCase) Then
                    fileStream.Write(Newtonsoft.Json.JsonConvert.SerializeObject(collectionOfSavedData))
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
        If Logs.SelectedRows.Count <> 1 Then e.Cancel = True
    End Sub

    Private Sub CopyLogTextToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyLogTextToolStripMenuItem.Click
        Dim selectedRow As MyDataGridViewRow = Logs.Rows(Logs.SelectedCells(0).RowIndex)
        Clipboard.SetText(selectedRow.Cells(2).Value)
    End Sub

    Private Sub LoadData(strFileName As String)
        Dim collectionOfSavedData As New List(Of SavedData)

        Try
            Using fileStream As New StreamReader(strFileName)
                collectionOfSavedData = Newtonsoft.Json.JsonConvert.DeserializeObject(Of List(Of SavedData))(fileStream.ReadToEnd.Trim, JSONDecoderSettings)
            End Using

            If collectionOfSavedData.Count > 0 Then
                Dim listOfLogEntries As New List(Of MyDataGridViewRow)

                For Each item As SavedData In collectionOfSavedData
                    listOfLogEntries.Add(item.MakeDataGridRow(Logs, GetMinimumHeight(item.log, Logs.DefaultCellStyle.Font)))
                Next

                Logs.Rows.Clear()
                Logs.Rows.AddRange(listOfLogEntries.ToArray)
            End If
        Catch ex As Newtonsoft.Json.JsonSerializationException
            SendMessageToSysLogServer($"(NoProxy)Exception Type: {ex.GetType}{vbCrLf}Exception Message: {ex.Message}{vbCrLf}{vbCrLf}Exception Stack Trace{vbCrLf}{ex.StackTrace}", My.Settings.sysLogPort)
            MsgBox("There was an error decoding JSON data.", MsgBoxStyle.Critical, Text)
        Catch ex As Newtonsoft.Json.JsonReaderException
            SendMessageToSysLogServer($"(NoProxy)Exception Type: {ex.GetType}{vbCrLf}Exception Message: {ex.Message}{vbCrLf}{vbCrLf}Exception Stack Trace{vbCrLf}{ex.StackTrace}", My.Settings.sysLogPort)
            MsgBox("There was an error decoding JSON data.", MsgBoxStyle.Critical, Text)
        End Try
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
    End Sub

    Private Sub OpenLogFileForViewingToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenLogFileForViewingToolStripMenuItem.Click
        Dim logFileViewer As New IgnoredLogsAndSearchResults(Me) With {.Icon = Icon, .Text = "Log File Viewer", .WindowDisplayMode = IgnoreOrSearchWindowDisplayMode.viewer, .strFileToLoad = Path.Combine(strPathToDataBackupFolder, Logs.SelectedRows(0).Cells(4).Value), .boolLoadExternalData = True}
        logFileViewer.Show(Me)
    End Sub

    Private Sub TxtSearchTerms_KeyDown(sender As Object, e As KeyEventArgs) Handles TxtSearchTerms.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            BtnSearch.PerformClick()
        End If
    End Sub
End Class