Imports System.IO
Imports System.Threading.Tasks
Imports Free_SysLog.ThreadSafetyLists

Public Class Alerts_History
    Private Shadows ParentForm As Form1
    Private boolDoneLoading As Boolean = False
    Public sortOrder As SortOrder = SortOrder.Ascending ' Define soSortOrder at class level

    Public WriteOnly Property SetParentForm As Form1
        Set(value As Form1)
            ParentForm = value
        End Set
    End Property

    Private Sub OpenLogViewerWindow(strLogText As String, strAlertText As String, strLogDate As String, strSourceIP As String, strRawLogText As String, alertType As AlertType)
        strRawLogText = strRawLogText.Replace("{newline}", vbCrLf, StringComparison.OrdinalIgnoreCase)

        Using LogViewerInstance As New LogViewer With {.strRawLogText = strRawLogText, .strLogText = strLogText, .StartPosition = FormStartPosition.CenterParent, .Icon = Icon, .alertType = alertType, .Size = My.Settings.logViewerWindowSize}
            LogViewerInstance.LblLogDate.Text = $"Log Date: {strLogDate}"
            LogViewerInstance.LblSource.Text = $"Source IP Address: {strSourceIP}"
            LogViewerInstance.TopMost = True
            LogViewerInstance.txtAlertText.Text = strAlertText

            LogViewerInstance.ShowDialog()
        End Using
    End Sub

    Private Sub Alerts_History_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If My.Settings.font IsNot Nothing Then
            AlertHistoryList.DefaultCellStyle.Font = My.Settings.font
            AlertHistoryList.ColumnHeadersDefaultCellStyle.Font = My.Settings.font
        End If

        SupportCode.SetDoubleBufferingFlag(AlertHistoryList)

        AlertHistoryList.RowsDefaultCellStyle.Padding = New Padding(0, 2, 0, 2)

        AlertHistoryList.AlternatingRowsDefaultCellStyle = New DataGridViewCellStyle() With {.BackColor = My.Settings.searchColor, .ForeColor = SupportCode.GetGoodTextColorBasedUponBackgroundColor(My.Settings.searchColor)}
        Location = SupportCode.VerifyWindowLocation(My.Settings.AlertHistoryLocation, Me)

        colTime.Width = My.Settings.columnTimeSize
        chkShowAlertsFromAllFiles.Checked = My.Settings.ShowAlertsFromAllFiles
        chkIncludeHiddenFiles.Enabled = My.Settings.ShowAlertsFromAllFiles
        chkIncludeHiddenFiles.Checked = My.Settings.ShowAlertsFromAllFilesWithHiddenFiles

        SupportCode.LoadColumnOrders(AlertHistoryList.Columns, My.Settings.alertsHistoryColumnOrder)

        AlertHistoryList.Columns(3).SortMode = DataGridViewColumnSortMode.NotSortable

        RefreshData()

        chkAlertTextColumnAutoFill.Checked = My.Settings.AlertsHistoryAlertColumnFill
        colAlert.AutoSizeMode = If(My.Settings.AlertsHistoryAlertColumnFill, DataGridViewAutoSizeColumnMode.Fill, DataGridViewAutoSizeColumnMode.NotSet)

        colFileName.Width = My.Settings.AlertsHistoryFileNameColumnSize

        boolDoneLoading = True
    End Sub

    Private Sub Alerts_History_ResizeEnd(sender As Object, e As EventArgs) Handles Me.ResizeEnd
        If boolDoneLoading Then My.Settings.AlertHistorySize = Size
    End Sub

    Protected Overrides Sub WndProc(ByRef m As Message)
        Const WM_EXITSIZEMOVE As Integer = &H232

        MyBase.WndProc(m)

        If m.Msg = WM_EXITSIZEMOVE AndAlso boolDoneLoading Then
            Location = SupportCode.VerifyWindowLocation(Location, Me)
            My.Settings.AlertHistoryLocation = Location
        End If
    End Sub

    Private Sub AlertHistoryList_DoubleClick(sender As Object, e As EventArgs) Handles AlertHistoryList.DoubleClick
        Dim hitTest As DataGridView.HitTestInfo = AlertHistoryList.HitTest(AlertHistoryList.PointToClient(MousePosition).X, AlertHistoryList.PointToClient(MousePosition).Y)

        If hitTest.Type = DataGridViewHitTestType.Cell And hitTest.RowIndex <> -1 And AlertHistoryList.SelectedRows.Count > 0 Then
            Dim AlertsHistoryDataGridViewRow As AlertsHistoryDataGridViewRow = DirectCast(AlertHistoryList.SelectedRows(0), AlertsHistoryDataGridViewRow)

            With AlertsHistoryDataGridViewRow
                OpenLogViewerWindow(.strLog, .strAlertText, .strTime, .strIP, .strRawLog, .alertType)
            End With
        End If
    End Sub

    Private Sub Alerts_History_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F5 Then RefreshData()
    End Sub

    Private Sub RefreshData()
        If ParentForm IsNot Nothing Then
            With ParentForm
                Dim stopwatch As Stopwatch = Stopwatch.StartNew()
                Dim data As New ThreadSafeList(Of AlertsHistoryDataGridViewRow)

                SyncLock ParentForm.dataGridLockObject
                    For Each item As MyDataGridViewRow In ParentForm.Logs.Rows
                        If item.BoolAlerted Then
                            data.Add(New AlertsHistory With {
                                     .strTime = item.Cells(SupportCode.ColumnIndex_ComputedTime).Value,
                                     .alertType = item.alertType,
                                     .strAlertText = item.AlertText,
                                     .strIP = item.Cells(SupportCode.ColumnIndex_IPAddress).Value,
                                     .strLog = item.Cells(SupportCode.ColumnIndex_LogText).Value,
                                     .strRawLog = item.RawLogData,
                                     .strFileName = "(Current Logs)",
                                     .alertDate = item.DateObject,
                                     .boolCompressed = False,
                                     .boolHidden = False
                                    }.MakeDataGridRow(AlertHistoryList))
                        End If
                    Next
                End SyncLock

                If chkShowAlertsFromAllFiles.Checked Then
                    Dim filesInDirectory As FileInfo()

                    If chkIncludeHiddenFiles.Checked Then
                        filesInDirectory = New DirectoryInfo(SupportCode.strPathToDataBackupFolder).GetFiles()
                    Else
                        filesInDirectory = New DirectoryInfo(SupportCode.strPathToDataBackupFolder).GetFiles().Where(Function(fileinfo As FileInfo) (fileinfo.Attributes And FileAttributes.Hidden) <> FileAttributes.Hidden).ToArray
                    End If

                    Parallel.ForEach(filesInDirectory, Sub(file As FileInfo)
                                                           Dim dataFromFile As List(Of SavedData)
                                                           Dim boolCompressed As Boolean = False
                                                           Dim boolHidden As Boolean = (file.Attributes And FileAttributes.Hidden) = FileAttributes.Hidden

                                                           If file.Extension.Equals(".gz", StringComparison.OrdinalIgnoreCase) And SupportCode.IsGZipFile(file.FullName) Then
                                                               boolCompressed = True
                                                               dataFromFile = Newtonsoft.Json.JsonConvert.DeserializeObject(Of List(Of SavedData))(SupportCode.GetTextContentsFromGZIPedLogFile(file.FullName), SupportCode.JSONDecoderSettingsForLogFiles)
                                                           Else
                                                               Using fileStream As New StreamReader(file.FullName)
                                                                   dataFromFile = Newtonsoft.Json.JsonConvert.DeserializeObject(Of List(Of SavedData))(fileStream.ReadToEnd.Trim, SupportCode.JSONDecoderSettingsForLogFiles)
                                                               End Using
                                                           End If

                                                           Parallel.ForEach(dataFromFile, Sub(SavedData As SavedData)
                                                                                              If SavedData.BoolAlerted Then
                                                                                                  data.Add(New AlertsHistory With {
                                                                                                     .strTime = SavedData.time,
                                                                                                     .alertType = SavedData.alertType,
                                                                                                     .strAlertText = SavedData.alertText,
                                                                                                     .strIP = SavedData.ip,
                                                                                                     .strLog = SavedData.log,
                                                                                                     .strRawLog = SavedData.rawLogData,
                                                                                                     .strFileName = file.Name,
                                                                                                     .alertDate = SavedData.DateObject,
                                                                                                     .boolHidden = boolHidden,
                                                                                                     .boolCompressed = boolCompressed
                                                                                                  }.MakeDataGridRow(AlertHistoryList))
                                                                                              End If
                                                                                          End Sub)
                                                       End Sub)
                End If

                data.Sort(Function(x As AlertsHistoryDataGridViewRow, y As AlertsHistoryDataGridViewRow) y.alertDate.CompareTo(x.alertDate))

                If data.Any() Then
                    lblNumberOfAlerts.Text = $"Number of Alerts: {data.Count:N0}"

                    AlertHistoryList.SuspendLayout()
                    AlertHistoryList.Rows.Clear()
                    AlertHistoryList.Rows.AddRange(data.GetSnapshot.ToArray)
                    AlertHistoryList.ResumeLayout()
                    AlertHistoryList.AutoResizeRows()
                End If

                lblTimeTakenToLoadData.Text = $"Time Taken to Load Data: {stopwatch.ElapsedMilliseconds}ms"
            End With
        End If
    End Sub

    Private Sub BtnRefresh_Click(sender As Object, e As EventArgs) Handles BtnRefresh.Click
        RefreshData()
    End Sub

    Private Sub Alerts_History_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        My.Settings.alertsHistoryColumnOrder = SupportCode.SaveColumnOrders(AlertHistoryList.Columns)
    End Sub

    Private Sub chkShowAlertsFromAllFiles_CheckedChanged(sender As Object, e As EventArgs) Handles chkShowAlertsFromAllFiles.CheckedChanged
        colFileName.Visible = chkShowAlertsFromAllFiles.Checked
        chkIncludeHiddenFiles.Enabled = chkShowAlertsFromAllFiles.Checked
        If boolDoneLoading Then RefreshData()
    End Sub

    Private Sub chkAlertTextColumnAutoFill_Click(sender As Object, e As EventArgs) Handles chkAlertTextColumnAutoFill.Click
        My.Settings.AlertsHistoryAlertColumnFill = chkAlertTextColumnAutoFill.Checked
        colAlert.AutoSizeMode = If(My.Settings.AlertsHistoryAlertColumnFill, DataGridViewAutoSizeColumnMode.Fill, DataGridViewAutoSizeColumnMode.NotSet)
    End Sub

    Private Sub AlertHistoryList_ColumnWidthChanged(sender As Object, e As DataGridViewColumnEventArgs) Handles AlertHistoryList.ColumnWidthChanged
        If boolDoneLoading Then My.Settings.AlertsHistoryFileNameColumnSize = colFileName.Width
    End Sub

    Private Sub chkShowAlertsFromAllFiles_Click(sender As Object, e As EventArgs) Handles chkShowAlertsFromAllFiles.Click
        My.Settings.ShowAlertsFromAllFiles = chkShowAlertsFromAllFiles.Checked
    End Sub

    Private Sub chkIncludeHiddenFiles_Click(sender As Object, e As EventArgs) Handles chkIncludeHiddenFiles.Click
        My.Settings.ShowAlertsFromAllFilesWithHiddenFiles = chkIncludeHiddenFiles.Checked
        BtnRefresh.PerformClick()
    End Sub

    Private Sub AlertHistoryList_ColumnHeaderMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles AlertHistoryList.ColumnHeaderMouseClick
        If e.ColumnIndex = 3 Then Exit Sub

        If e.Button = MouseButtons.Left Then
            Dim column As DataGridViewColumn = AlertHistoryList.Columns(e.ColumnIndex)

            If sortOrder = SortOrder.Descending Then
                sortOrder = SortOrder.Ascending
            ElseIf sortOrder = SortOrder.Ascending Then
                sortOrder = SortOrder.Descending
            Else
                sortOrder = SortOrder.Ascending
            End If

            colTime.HeaderCell.SortGlyphDirection = SortOrder.None
            colAlertType.HeaderCell.SortGlyphDirection = SortOrder.None
            colAlert.HeaderCell.SortGlyphDirection = SortOrder.None
            colFileName.HeaderCell.SortGlyphDirection = SortOrder.None

            AlertHistoryList.Columns(e.ColumnIndex).HeaderCell.SortGlyphDirection = sortOrder

            SortLogsByDateObjectNoLocking(column.Index, sortOrder)
        End If
    End Sub

    Public Sub SortLogsByDateObjectNoLocking(columnIndex As Integer, order As SortOrder)
        InitializeAlertsHistoryDataGridViewRowComparer.InitializeAlertsHistoryDataGridViewRowComparer(AlertHistoryList, columnIndex, order)
    End Sub
End Class