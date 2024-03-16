Imports System.ComponentModel
Imports System.IO
Imports System.Reflection
Imports System.Xml.Serialization

Public Class Ignored_Logs_and_Search_Results
    Public LogsToBeDisplayed As List(Of MyDataGridViewRow)
    Private m_SortingColumn1, m_SortingColumn2 As ColumnHeader
    Private boolDoneLoading As Boolean = False

    Private intColumnNumber As Integer ' Define intColumnNumber at class level
    Private sortOrder As SortOrder = SortOrder.Descending ' Define soSortOrder at class level
    Private ReadOnly dataGridLockObject As New Object

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

    Private Sub Logs_DoubleClick(sender As Object, e As EventArgs) Handles logs.DoubleClick
        OpenLogViewerWindow()
    End Sub

    Private Sub Ignored_Logs_and_Search_Results_ResizeEnd(sender As Object, e As EventArgs) Handles Me.ResizeEnd
        My.Settings.ignoredWindowSize = Size
    End Sub

    Private Sub Ignored_Logs_and_Search_Results_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Size = My.Settings.ignoredWindowSize
        Location = VerifyWindowLocation(My.Settings.ignoredWindowLocation, Me)
        colTime.Width = My.Settings.columnTimeSize
        colType.Width = My.Settings.columnTypeSize
        colIPAddress.Width = My.Settings.columnIPSize
        colLog.Width = My.Settings.columnLogSize

        Dim flags As BindingFlags = BindingFlags.NonPublic Or BindingFlags.Instance Or BindingFlags.SetProperty
        Dim propInfo As PropertyInfo = GetType(DataGridView).GetProperty("DoubleBuffered", flags)
        propInfo?.SetValue(logs, True, Nothing)

        Dim rowStyle As New DataGridViewCellStyle() With {.BackColor = My.Settings.searchColor}
        logs.AlternatingRowsDefaultCellStyle = rowStyle

        logs.Rows.AddRange(LogsToBeDisplayed.ToArray())

        boolDoneLoading = True
    End Sub

    Private Sub Ignored_Logs_and_Search_Results_LocationChanged(sender As Object, e As EventArgs) Handles Me.LocationChanged
        If boolDoneLoading Then My.Settings.ignoredWindowLocation = Location
    End Sub

    Private Function SanitizeForCSV(input As String) As String
        If input.Contains(Chr(34)) Then input = input.Replace(Chr(34), Chr(34) & Chr(34))
        If input.Contains(",") Then input = $"{Chr(34)}{input}{Chr(34)}"
        Return input
    End Function

    Private Sub BtnExport_Click(sender As Object, e As EventArgs) Handles BtnExport.Click
        SaveFileDialog.Filter = "XML File|*.xml|JSON File|*.json|CSV (Comma Separated Value)|*.csv"
        SaveFileDialog.Title = "Export..."

        If SaveFileDialog.ShowDialog() = DialogResult.OK Then
            Dim fileInfo As New IO.FileInfo(SaveFileDialog.FileName)

            Dim collectionOfSavedData As New List(Of SavedData)
            Dim myItem As MyDataGridViewRow
            Dim csvStringBuilder As New Text.StringBuilder
            Dim strTime, strType, strSourceIP, strLogText As String

            If fileInfo.Extension.Equals(".csv", StringComparison.OrdinalIgnoreCase) Then csvStringBuilder.AppendLine("Time,Type,Source IP,Log Text")

            For Each item As DataGridViewRow In logs.Rows
                If Not String.IsNullOrWhiteSpace(item.Cells(0).Value) Then
                    myItem = DirectCast(item, MyDataGridViewRow)

                    If fileInfo.Extension.Equals(".csv", StringComparison.OrdinalIgnoreCase) Then
                        With myItem
                            strTime = SanitizeForCSV(.Cells(0).Value)
                            strType = SanitizeForCSV(.Cells(1).Value)
                            strSourceIP = SanitizeForCSV(.Cells(2).Value)
                            strLogText = SanitizeForCSV(.Cells(3).Value)
                        End With

                        csvStringBuilder.AppendLine($"{strTime},{strType},{strSourceIP},{strLogText}")
                    Else
                        collectionOfSavedData.Add(New SavedData With {
                                                .time = myItem.Cells(0).Value,
                                                .type = myItem.Cells(1).Value,
                                                .ip = myItem.Cells(2).Value,
                                                .log = myItem.Cells(3).Value,
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

    Private Sub Ignored_Logs_and_Search_Results_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        ignoredLogsWindow = Nothing
    End Sub
End Class