Imports System.ComponentModel
Imports System.IO
Imports System.Reflection
Imports System.Text.RegularExpressions
Imports System.Xml.Serialization
Imports Free_SysLog.SupportCode
Imports System.Threading.Tasks

Public Class IgnoredLogsAndSearchResults
    Public LogsToBeDisplayed As List(Of MyDataGridViewRow)
    Private _WindowDisplayMode As IgnoreOrSearchWindowDisplayMode
    Private m_SortingColumn1, m_SortingColumn2 As ColumnHeader
    Private boolDoneLoading As Boolean = False
    Public MainProgramForm As Form1
    Private logsLockObject As New Object

    Public boolLoadExternalData As Boolean = False
    Public strFileToLoad As String

    Private intColumnNumber As Integer ' Define intColumnNumber at class level
    Private sortOrder As SortOrder = SortOrder.Ascending ' Define soSortOrder at class level
    Private ReadOnly dataGridLockObject As New Object

    Private Const intBatchSize As Integer = 25

    Private Sub OpenLogViewerWindow()
        If Logs.Rows.Count > 0 And Logs.SelectedCells.Count > 0 Then
            Dim selectedRow As MyDataGridViewRow = Logs.Rows(Logs.SelectedCells(0).RowIndex)
            Dim strLogText As String = selectedRow.Cells(ColumnIndex_LogText).Value
            Dim strRawLogText As String = If(String.IsNullOrWhiteSpace(selectedRow.RawLogData), selectedRow.Cells(ColumnIndex_LogText).Value, selectedRow.RawLogData.Replace("{newline}", vbCrLf, StringComparison.OrdinalIgnoreCase))

            Using LogViewerInstance As New LogViewer With {.strRawLogText = strRawLogText, .strLogText = strLogText, .StartPosition = FormStartPosition.CenterParent, .Icon = Icon, .alertType = selectedRow.alertType}
                LogViewerInstance.LblLogDate.Text = $"Log Date: {selectedRow.Cells(ColumnIndex_ComputedTime).Value}"
                LogViewerInstance.LblSource.Text = $"Source IP Address: {selectedRow.Cells(ColumnIndex_IPAddress).Value}"

                If Not String.IsNullOrEmpty(selectedRow.AlertText) Then
                    LogViewerInstance.txtAlertText.Text = selectedRow.AlertText
                End If

                LogViewerInstance.ShowDialog(Me)
            End Using
        End If
    End Sub

    Private Sub Logs_ColumnHeaderMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles Logs.ColumnHeaderMouseClick
        If e.Button = MouseButtons.Left Then
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
        End If
    End Sub

    Private Sub SortLogsByDateObject(columnIndex As Integer, order As SortOrder)
        SyncLock dataGridLockObject
            Logs.Invoke(Sub()
                            Logs.SuspendLayout()
                            Logs.Sort(Logs.Columns(columnIndex), If(order = SortOrder.Ascending, ListSortDirection.Ascending, ListSortDirection.Descending))
                            Logs.ResumeLayout()
                        End Sub)
        End SyncLock
    End Sub

    Private Sub Logs_DoubleClick(sender As Object, e As EventArgs) Handles Logs.DoubleClick
        Dim hitTest As DataGridView.HitTestInfo = Logs.HitTest(Logs.PointToClient(MousePosition).X, Logs.PointToClient(MousePosition).Y)
        If hitTest.Type = DataGridViewHitTestType.Cell And hitTest.RowIndex <> -1 Then OpenLogViewerWindow()
    End Sub

    Private Sub Ignored_Logs_and_Search_Results_ResizeBegin(sender As Object, e As EventArgs) Handles Me.ResizeBegin
        Logs.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
    End Sub

    Private Sub Ignored_Logs_and_Search_Results_ResizeEnd(sender As Object, e As EventArgs) Handles Me.ResizeEnd
        If boolDoneLoading Then
            If _WindowDisplayMode = IgnoreOrSearchWindowDisplayMode.ignored Then
                My.Settings.ignoredWindowSize = Size
            ElseIf _WindowDisplayMode = IgnoreOrSearchWindowDisplayMode.search Then
                My.Settings.searchWindowSize = Size
            ElseIf _WindowDisplayMode = IgnoreOrSearchWindowDisplayMode.viewer Then
                My.Settings.logFileViewerSize = Size
            End If
        End If

        Logs.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders
    End Sub

    Private Sub Logs_ColumnWidthChanged(sender As Object, e As DataGridViewColumnEventArgs) Handles Logs.ColumnWidthChanged
        If boolDoneLoading Then
            My.Settings.columnFileNameSize = ColFileName.Width
        End If
    End Sub

    Private Sub Logs_MouseDown(sender As Object, e As MouseEventArgs) Handles Logs.MouseDown
        Dim hitTest As DataGridView.HitTestInfo = Logs.HitTest(e.X, e.Y)

        If hitTest.Type = DataGridViewHitTestType.ColumnHeader Then
            Logs.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
        End If
    End Sub

    Private Sub Logs_MouseUp(sender As Object, e As MouseEventArgs) Handles Logs.MouseUp
        Dim hitTest As DataGridView.HitTestInfo = Logs.HitTest(e.X, e.Y)

        If hitTest.Type = DataGridViewHitTestType.ColumnHeader Then
            Logs.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders
        End If
    End Sub

    Private Sub Logs_CellPainting(sender As Object, e As DataGridViewCellPaintingEventArgs) Handles Logs.CellPainting
        If e.RowIndex = -1 AndAlso e.ColumnIndex = ColAlerts.Index Then
            e.PaintBackground(e.CellBounds, False)
            TextRenderer.DrawText(e.Graphics, e.FormattedValue.ToString(), e.CellStyle.Font, e.CellBounds, e.CellStyle.ForeColor, TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter)
            e.Handled = True
        End If
    End Sub

    Private Sub Ignored_Logs_and_Search_Results_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If My.Settings.font IsNot Nothing Then
            Logs.DefaultCellStyle.Font = My.Settings.font
            Logs.ColumnHeadersDefaultCellStyle.Font = My.Settings.font
        End If

        ColLog.AutoSizeMode = If(My.Settings.colLogAutoFill, DataGridViewAutoSizeColumnMode.Fill, DataGridViewAutoSizeColumnMode.NotSet)
        Logs.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders

        If _WindowDisplayMode = IgnoreOrSearchWindowDisplayMode.ignored Then
            BtnClearIgnoredLogs.Visible = True
            BtnViewMainWindow.Visible = True
            ChkColLogsAutoFill.Location = New Point(135, 382)
        ElseIf _WindowDisplayMode = IgnoreOrSearchWindowDisplayMode.viewer Then
            BtnExport.Visible = False
            BtnViewMainWindow.Visible = True

            LblSearchLabel.Visible = True
            TxtSearchTerms.Visible = True
            ChkRegExSearch.Visible = True
            ChkCaseInsensitiveSearch.Visible = True
            BtnSearch.Visible = True
            ViewIgnoredLogPatternToolStripMenuItem.Visible = False
        End If

        If _WindowDisplayMode = IgnoreOrSearchWindowDisplayMode.ignored Then
            Size = My.Settings.ignoredWindowSize
            Location = VerifyWindowLocation(My.Settings.ignoredWindowLocation, Me)
        ElseIf _WindowDisplayMode = IgnoreOrSearchWindowDisplayMode.search Then
            Size = My.Settings.searchWindowSize
            Location = VerifyWindowLocation(My.Settings.searchWindowLocation, Me)
        ElseIf _WindowDisplayMode = IgnoreOrSearchWindowDisplayMode.viewer Then
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
        ColAlerts.Width = My.Settings.columnAlertedSize
        ColFileName.Width = My.Settings.columnFileNameSize

        LoadColumnOrders(Logs.Columns, My.Settings.logsColumnOrder)

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

        ChkAutoScroll.Visible = _WindowDisplayMode = IgnoreOrSearchWindowDisplayMode.ignored
        ChkKeepIgnoredLogsPastUserLimit.Visible = _WindowDisplayMode = IgnoreOrSearchWindowDisplayMode.ignored

        If _WindowDisplayMode <> IgnoreOrSearchWindowDisplayMode.viewer Then
            Logs.SuspendLayout()

            If _WindowDisplayMode = IgnoreOrSearchWindowDisplayMode.ignored Then
                ColAlerts.Visible = False
                LblCount.Text = $"Number of ignored logs: {LogsToBeDisplayed.Count:N0}"
            Else
                LblCount.Text = $"Number of search results: {LogsToBeDisplayed.Count:N0}"
            End If

            Threading.ThreadPool.QueueUserWorkItem(Sub()
                                                       SyncLock IgnoredLogsAndSearchResultsInstanceLockObject
                                                           ' Dynamically calculate the batch size based on total logs
                                                           Dim totalLogs As Integer = LogsToBeDisplayed.Count
                                                           Dim intBatchSize As Integer

                                                           ' Adjust batch size based on the number of logs to process
                                                           If totalLogs <= 100 Then
                                                               intBatchSize = 5  ' Small dataset: use smaller batches
                                                           ElseIf totalLogs <= 1000 Then
                                                               intBatchSize = 20 ' Medium dataset: use medium-sized batches
                                                           Else
                                                               intBatchSize = 50 ' Large dataset: use larger batches
                                                           End If

                                                           ' Number of batches based on the dynamically determined batch size
                                                           Dim numBatches As Integer = Math.Ceiling(totalLogs / intBatchSize)

                                                           ' Create ParallelOptions to control the degree of parallelism
                                                           Dim parallelOptions As New ParallelOptions() With {.MaxDegreeOfParallelism = Environment.ProcessorCount}

                                                           ' Parallel loop to process batches
                                                           Parallel.For(0, numBatches, parallelOptions, Sub(batchIndex As Integer)
                                                                                                            ' Calculate the starting and ending index for the current batch
                                                                                                            Dim startIndex As Integer = batchIndex * intBatchSize
                                                                                                            Dim endIndex As Integer = Math.Min(startIndex + intBatchSize, totalLogs)

                                                                                                            ' Get the current batch of rows
                                                                                                            Dim batch As MyDataGridViewRow() = LogsToBeDisplayed.Skip(startIndex).Take(endIndex - startIndex).ToArray()

                                                                                                            ' Ensure thread safety when updating the UI
                                                                                                            SyncLock dataGridLockObject
                                                                                                                Logs.Invoke(Sub() Logs.Rows.AddRange(batch)) ' Invoke to update UI on the main thread
                                                                                                            End SyncLock
                                                                                                        End Sub)


                                                           Invoke(Sub() Logs.ResumeLayout())
                                                       End SyncLock
                                                   End Sub)
        ElseIf _WindowDisplayMode = IgnoreOrSearchWindowDisplayMode.viewer AndAlso boolLoadExternalData AndAlso Not String.IsNullOrEmpty(strFileToLoad) Then
            Threading.ThreadPool.QueueUserWorkItem(Sub() LoadData(strFileToLoad))
        End If

        boolDoneLoading = True
    End Sub

    Public Sub AddIgnoredDatagrid(ItemToAdd As MyDataGridViewRow)
        Invoke(Sub()
                   Try
                       SyncLock logsLockObject
                           If ChkKeepIgnoredLogsPastUserLimit.Checked Then
                               Logs.Rows.Add(ItemToAdd)
                           Else
                               If Logs.Rows.Count < My.Settings.LimitNumberOfIgnoredLogs Then
                                   Logs.Rows.Add(ItemToAdd)
                               Else
                                   While Logs.Rows.Count >= My.Settings.LimitNumberOfIgnoredLogs
                                       Logs.Rows.RemoveAt(0)
                                   End While

                                   Logs.Rows.Add(ItemToAdd)
                               End If
                           End If
                       End SyncLock

                       If ChkAutoScroll.Checked Then Logs.FirstDisplayedScrollingRowIndex = Logs.Rows.Count - 1
                       LblCount.Text = $"Number of ignored logs: {Logs.Rows.Count:N0}"
                   Catch ex As Exception
                   End Try
               End Sub)
    End Sub

    Protected Overrides Sub WndProc(ByRef m As Message)
        Const WM_EXITSIZEMOVE As Integer = &H232

        MyBase.WndProc(m)

        If m.Msg = WM_EXITSIZEMOVE AndAlso boolDoneLoading Then
            If _WindowDisplayMode = IgnoreOrSearchWindowDisplayMode.ignored Then
                My.Settings.ignoredWindowLocation = Location
            ElseIf _WindowDisplayMode = IgnoreOrSearchWindowDisplayMode.search Then
                My.Settings.searchWindowLocation = Location
            ElseIf _WindowDisplayMode = IgnoreOrSearchWindowDisplayMode.viewer Then
                My.Settings.logFileViewerLocation = Location
            End If
        End If
    End Sub

    Private Sub BtnClearIgnoredLogs_Click(sender As Object, e As EventArgs) Handles BtnClearIgnoredLogs.Click
        If TypeOf parentForm Is Form1 AndAlso MsgBox("Are you sure you want to clear the ignored logs stored in system memory?", MsgBoxStyle.Question + MsgBoxStyle.YesNo + vbDefaultButton2, Text) = MsgBoxResult.Yes Then
            Logs.Rows.Clear()
            LblCount.Text = "Number of ignored logs: 0"
            parentForm.IgnoredLogs.Clear()
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

            If My.Settings.AskOpenExplorer Then
                Using OpenExplorer As New OpenExplorer()
                    OpenExplorer.StartPosition = FormStartPosition.CenterParent
                    OpenExplorer.MyParentForm = Me

                    Dim result As DialogResult = OpenExplorer.ShowDialog(Me)

                    If result = DialogResult.No Then
                        Exit Sub
                    ElseIf result = DialogResult.Yes Then
                        SelectFileInWindowsExplorer(SaveFileDialog.FileName)
                    End If
                End Using
            Else
                MsgBox("Data exported successfully.", MsgBoxStyle.Information, SupportCode.ParentForm.Text)
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
        Dim hitTest As DataGridView.HitTestInfo = Logs.HitTest(Logs.PointToClient(MousePosition).X, Logs.PointToClient(MousePosition).Y)

        If hitTest.Type = DataGridViewHitTestType.ColumnHeader Then
            e.Cancel = True
            Exit Sub
        End If

        If _WindowDisplayMode = IgnoreOrSearchWindowDisplayMode.viewer AndAlso boolLoadExternalData AndAlso Not String.IsNullOrEmpty(strFileToLoad) Then
            ExportSelectedLogsToolStripMenuItem.Visible = True
            CopyLogTextToolStripMenuItem.Visible = False
            CreateAlertToolStripMenuItem.Visible = False
            OpenLogFileForViewingToolStripMenuItem.Visible = False
        Else
            If Logs.SelectedRows().Count > 1 Then
                Dim selectedItem As MyDataGridViewRow = TryCast(Logs.SelectedRows(0), MyDataGridViewRow)
                If selectedItem IsNot Nothing Then CopyRawLogTextToolStripMenuItem.Visible = Not String.IsNullOrEmpty(selectedItem.RawLogData)

                ExportSelectedLogsToolStripMenuItem.Visible = True
                CopyLogTextToolStripMenuItem.Visible = False
                CreateAlertToolStripMenuItem.Visible = False
                OpenLogFileForViewingToolStripMenuItem.Visible = False
            Else
                CopyLogTextToolStripMenuItem.Visible = True
                CreateAlertToolStripMenuItem.Visible = True
                OpenLogFileForViewingToolStripMenuItem.Visible = True
            End If
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
                .Cells(ColumnIndex_ComputedTime).Value = Now
                .Cells(ColumnIndex_LogText).Value = strLog
                .DefaultCellStyle.Padding = New Padding(0, 2, 0, 2)
            End With

            Return MyDataGridViewRow
        End Using
    End Function

    Private Sub LoadData(strFileName As String)
        Dim stopWatch As Stopwatch = Stopwatch.StartNew

        Logs.Invoke(Sub() Logs.Rows.Add(MakeDataGridRow(Now, "Loading data and populating data grid... Please Wait.", Logs)))

        Dim collectionOfSavedData As New List(Of SavedData)

        Try
            Using fileStream As New StreamReader(strFileName)
                collectionOfSavedData = Newtonsoft.Json.JsonConvert.DeserializeObject(Of List(Of SavedData))(fileStream.ReadToEnd.Trim, JSONDecoderSettingsForSettingsFiles)
            End Using

            If collectionOfSavedData.Any() Then
                collectionOfSavedData.Sort(Function(x As SavedData, y As SavedData) x.DateObject.CompareTo(y.DateObject))

                Logs.Invoke(Sub()
                                Logs.Rows.Clear()
                                Logs.SuspendLayout()
                                LblCount.Text = $"Number of logs: {collectionOfSavedData.Count:N0}"
                            End Sub)

                Dim index As Integer = 0

                ' Create ParallelOptions to control the degree of parallelism
                Dim parallelOptions As New ParallelOptions() With {.MaxDegreeOfParallelism = Environment.ProcessorCount}

                ' Dynamically calculate the batch size based on total logs
                Dim totalLogs As Integer = collectionOfSavedData.Count
                Dim intBatchSize As Integer

                ' Adjust batch size based on the number of logs to process
                If totalLogs <= 100 Then
                    intBatchSize = 5  ' Small dataset: use smaller batches
                ElseIf totalLogs <= 1000 Then
                    intBatchSize = 20 ' Medium dataset: use medium-sized batches
                Else
                    intBatchSize = 50 ' Large dataset: use larger batches
                End If

                ' Number of batches based on the dynamically determined batch size
                Dim numBatches As Integer = Math.Ceiling(totalLogs / intBatchSize)

                ' Parallel loop to process batches
                Parallel.For(0, numBatches, parallelOptions, Sub(batchIndex As Integer)
                                                                 ' Calculate the starting and ending index for the current batch
                                                                 Dim startIndex As Integer = batchIndex * intBatchSize
                                                                 Dim endIndex As Integer = Math.Min(startIndex + intBatchSize, totalLogs)

                                                                 ' Process each individual item in the batch
                                                                 For i As Integer = startIndex To endIndex - 1
                                                                     Dim intIndex As Integer = i

                                                                     ' Ensure thread safety when updating the UI
                                                                     SyncLock dataGridLockObject
                                                                         Logs.Invoke(Sub() Logs.Rows.Add(collectionOfSavedData(intIndex).MakeDataGridRow(Logs))) ' Invoke to update UI on the main thread
                                                                     End SyncLock
                                                                 Next
                                                             End Sub)

                Logs.Invoke(Sub()
                                Logs.ResumeLayout()
                                LogsLoadedInLabel.Visible = True
                                LogsLoadedInLabel.Text = $"Logs Loaded In: {MyRoundingFunction(stopWatch.Elapsed.TotalMilliseconds / 1000, 2)} seconds"
                                Logs.Sort(Logs.Columns(0), ListSortDirection.Ascending)
                                boolDoneLoading = True
                            End Sub)
            End If
        Catch ex As Newtonsoft.Json.JsonSerializationException
            SyslogParser.AddToLogList(Nothing, $"Exception Type: {ex.GetType}{vbCrLf}Exception Message: {ex.Message}{vbCrLf}{vbCrLf}Exception Stack Trace{vbCrLf}{ex.StackTrace}")
            MsgBox("There was an error decoding JSON data.", MsgBoxStyle.Critical, Text)
        Catch ex As Newtonsoft.Json.JsonReaderException
            SyslogParser.AddToLogList(Nothing, $"Exception Type: {ex.GetType}{vbCrLf}Exception Message: {ex.Message}{vbCrLf}{vbCrLf}Exception Stack Trace{vbCrLf}{ex.StackTrace}")
            MsgBox("There was an error decoding JSON data.", MsgBoxStyle.Critical, Text)
        End Try
    End Sub

    Private Sub CreateAlertToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CreateAlertToolStripMenuItem.Click
        Using Alerts As New Alerts With {.StartPosition = FormStartPosition.CenterParent, .Icon = Icon}
            Alerts.TxtLogText.Text = Logs.SelectedRows(0).Cells(ColumnIndex_LogText).Value
            Alerts.ShowDialog(Me)
        End Using
    End Sub

    Private Sub Ignored_Logs_and_Search_Results_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        SyncLock IgnoredLogsAndSearchResultsInstanceLockObject
            IgnoredLogsAndSearchResultsInstance = Nothing
        End SyncLock
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
                                                  If listOfSearchResults.Any() Then
                                                      Dim searchResultsWindow As New IgnoredLogsAndSearchResults(Me, IgnoreOrSearchWindowDisplayMode.search) With {.MainProgramForm = MainProgramForm, .Icon = Icon, .LogsToBeDisplayed = listOfSearchResults, .Text = "Search Results"}
                                                      searchResultsWindow.ChkColLogsAutoFill.Checked = My.Settings.colLogAutoFill
                                                      searchResultsWindow.ShowDialog(Me)
                                                  Else
                                                      MsgBox("Search terms not found.", MsgBoxStyle.Information, Text)
                                                  End If

                                                  Invoke(Sub() BtnSearch.Enabled = True)
                                              End Sub

        worker.RunWorkerAsync()
    End Sub

    Private Sub OpenLogFileForViewingToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenLogFileForViewingToolStripMenuItem.Click
        If Logs.Rows.Count > 0 And Logs.SelectedCells.Count > 0 Then
            Dim selectedRow As MyDataGridViewRow = Logs.Rows(Logs.SelectedCells(0).RowIndex)
            Dim strLogText As String = selectedRow.Cells(ColumnIndex_LogText).Value
            Dim strRawLogText As String = If(String.IsNullOrWhiteSpace(selectedRow.RawLogData), selectedRow.Cells(ColumnIndex_LogText).Value, selectedRow.RawLogData.Replace("{newline}", vbCrLf, StringComparison.OrdinalIgnoreCase))

            Using LogViewerInstance As New LogViewer With {.strRawLogText = strRawLogText, .strLogText = strLogText, .StartPosition = FormStartPosition.CenterParent, .Icon = Icon, .alertType = selectedRow.alertType}
                LogViewerInstance.LblLogDate.Text = $"Log Date: {selectedRow.Cells(ColumnIndex_ComputedTime).Value}"
                LogViewerInstance.LblSource.Text = $"Source IP Address: {selectedRow.Cells(ColumnIndex_IPAddress).Value}"

                If Not String.IsNullOrEmpty(selectedRow.AlertText) Then
                    LogViewerInstance.txtAlertText.Text = selectedRow.AlertText
                End If

                LogViewerInstance.ShowDialog(Me)
            End Using
        End If
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

    Private Sub ViewIgnoredLogPatternToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ViewIgnoredLogPatternToolStripMenuItem.Click
        If Logs.Rows.Count > 0 And Logs.SelectedCells.Count > 0 Then
            Dim selectedRow As MyDataGridViewRow = Logs.Rows(Logs.SelectedCells(0).RowIndex)
            Dim strIgnoredPattern As String = selectedRow.IgnoredPattern

            Using IgnoredWordsAndPhrasesOrAlertsInstance As New IgnoredWordsAndPhrases With {.Icon = Icon, .StartPosition = FormStartPosition.CenterParent}
                IgnoredWordsAndPhrasesOrAlertsInstance.strIgnoredPattern = strIgnoredPattern
                IgnoredWordsAndPhrasesOrAlertsInstance.ShowDialog(Me)

                If IgnoredWordsAndPhrasesOrAlertsInstance.boolChanged Then
                    IgnoredRegexCache.Clear()
                End If
            End Using
        End If
    End Sub

    Private Sub OpenLogForViewingToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenLogForViewingToolStripMenuItem.Click
        OpenLogViewerWindow()
    End Sub

    Private Sub ChkColLogsAutoFill_Click(sender As Object, e As EventArgs) Handles ChkColLogsAutoFill.Click
        My.Settings.colLogAutoFill = ChkColLogsAutoFill.Checked
        ColLog.AutoSizeMode = If(My.Settings.colLogAutoFill, DataGridViewAutoSizeColumnMode.Fill, DataGridViewAutoSizeColumnMode.NotSet)

        If MainProgramForm IsNot Nothing Then
            MainProgramForm.ColLogsAutoFill.Checked = ChkColLogsAutoFill.Checked
            MainProgramForm.ColLog.AutoSizeMode = If(My.Settings.colLogAutoFill, DataGridViewAutoSizeColumnMode.Fill, DataGridViewAutoSizeColumnMode.NotSet)
        End If
    End Sub

    Private Sub ChkKeepIgnoredLogsPastUserLimit_Click(sender As Object, e As EventArgs) Handles ChkKeepIgnoredLogsPastUserLimit.Click
        If Not ChkKeepIgnoredLogsPastUserLimit.Checked AndAlso Logs.Rows.Count > My.Settings.LimitNumberOfIgnoredLogs Then
            SyncLock logsLockObject
                While Logs.Rows.Count >= My.Settings.LimitNumberOfIgnoredLogs
                    Logs.Rows.RemoveAt(0)
                End While
            End SyncLock
        End If
    End Sub

    Private Sub CopyRawLogTextToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyRawLogTextToolStripMenuItem.Click
        If Logs.SelectedRows().Count <> 0 Then
            Dim selectedItem As MyDataGridViewRow = TryCast(Logs.SelectedRows(0), MyDataGridViewRow)
            If selectedItem IsNot Nothing Then CopyTextToWindowsClipboard(selectedItem.RawLogData, Text)
        End If
    End Sub
End Class