Imports System.ComponentModel
Imports System.IO
Imports System.Reflection
Imports System.Xml.Serialization

Public Class IgnoredLogsAndSearchResults
    Public LogsToBeDisplayed As List(Of MyDataGridViewRow)
    Public WindowDisplayMode As IgnoreOrSearchWindowDisplayMode
    Private m_SortingColumn1, m_SortingColumn2 As ColumnHeader
    Private boolDoneLoading As Boolean = False

    Private intColumnNumber As Integer ' Define intColumnNumber at class level
    Private sortOrder As SortOrder = SortOrder.Descending ' Define soSortOrder at class level
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

    Private Sub Logs_DoubleClick(sender As Object, e As EventArgs) Handles Logs.DoubleClick
        OpenLogViewerWindow()
    End Sub

    Private Sub Ignored_Logs_and_Search_Results_ResizeEnd(sender As Object, e As EventArgs) Handles Me.ResizeEnd
        If WindowDisplayMode = IgnoreOrSearchWindowDisplayMode.ignored Then
            My.Settings.ignoredWindowSize = Size
        Else
            My.Settings.searchWindowSize = Size
        End If
    End Sub

    Private Sub Ignored_Logs_and_Search_Results_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If WindowDisplayMode = IgnoreOrSearchWindowDisplayMode.ignored Then
            BtnClearIgnoredLogs.Visible = True
            BtnViewMainWindow.Visible = True
        End If

        If WindowDisplayMode = IgnoreOrSearchWindowDisplayMode.ignored Then
            Size = My.Settings.ignoredWindowSize
            Location = VerifyWindowLocation(My.Settings.ignoredWindowLocation, Me)
        Else
            Size = My.Settings.searchWindowSize
            Location = VerifyWindowLocation(My.Settings.searchWindowLocation, Me)
        End If

        ColTime.Width = My.Settings.columnTimeSize
        ColIPAddress.Width = My.Settings.columnIPSize
        ColLog.Width = My.Settings.columnLogSize

        Dim flags As BindingFlags = BindingFlags.NonPublic Or BindingFlags.Instance Or BindingFlags.SetProperty
        Dim propInfo As PropertyInfo = GetType(DataGridView).GetProperty("DoubleBuffered", flags)
        propInfo?.SetValue(Logs, True, Nothing)

        Dim rowStyle As New DataGridViewCellStyle() With {.BackColor = My.Settings.searchColor}
        Logs.AlternatingRowsDefaultCellStyle = rowStyle

        Logs.Rows.AddRange(LogsToBeDisplayed.ToArray())

        If WindowDisplayMode = IgnoreOrSearchWindowDisplayMode.ignored Then
            LblCount.Text = $"Number of ignored logs: {LogsToBeDisplayed.Count:N0}"
        Else
            LblCount.Text = $"Number of search results: {LogsToBeDisplayed.Count:N0}"
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
            Else
                My.Settings.searchWindowLocation = Location
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
            Dim fileInfo As New IO.FileInfo(SaveFileDialog.FileName)

            Dim collectionOfSavedData As New List(Of SavedData)
            Dim myItem As MyDataGridViewRow
            Dim csvStringBuilder As New Text.StringBuilder
            Dim strTime, strSourceIP, strLogText As String

            If fileInfo.Extension.Equals(".csv", StringComparison.OrdinalIgnoreCase) Then csvStringBuilder.AppendLine("Time,Source IP,Log Text")

            For Each item As DataGridViewRow In Logs.Rows
                If Not String.IsNullOrWhiteSpace(item.Cells(0).Value) Then
                    myItem = DirectCast(item, MyDataGridViewRow)

                    If fileInfo.Extension.Equals(".csv", StringComparison.OrdinalIgnoreCase) Then
                        With myItem
                            strTime = SanitizeForCSV(.Cells(0).Value)
                            strSourceIP = SanitizeForCSV(.Cells(1).Value)
                            strLogText = SanitizeForCSV(.Cells(2).Value)
                        End With

                        csvStringBuilder.AppendLine($"{strTime},{strSourceIP},{strLogText}")
                    Else
                        collectionOfSavedData.Add(New SavedData With {
                                                .time = myItem.Cells(0).Value,
                                                .ip = myItem.Cells(1).Value,
                                                .log = myItem.Cells(2).Value,
                                                .DateObject = myItem.DateObject
                                              })
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
        End If
    End Sub

    Private Sub LogsContextMenu_Opening(sender As Object, e As CancelEventArgs) Handles LogsContextMenu.Opening
        If Logs.SelectedRows.Count <> 1 Then e.Cancel = True
    End Sub

    Private Sub CopyLogTextToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyLogTextToolStripMenuItem.Click
        Dim selectedRow As MyDataGridViewRow = Logs.Rows(Logs.SelectedCells(0).RowIndex)
        Clipboard.SetText(selectedRow.Cells(2).Value)
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
End Class