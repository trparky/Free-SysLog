Imports System.IO
Imports System.Text.RegularExpressions
Imports System.ComponentModel
Imports System.Threading.Tasks
Imports Free_SysLog.SupportCode
Imports System.Threading

Public Class ViewLogBackups
    Public MyParentForm As Form1
    Public currentLogs As List(Of SavedData)
    Private boolDoneLoading As Boolean = False
    Public intSortColumnIndex As Integer = 0 ' Define intColumnNumber at class level
    Public sortOrder As SortOrder = SortOrder.Ascending ' Define soSortOrder at class level

    Private Sub Logs_ColumnHeaderMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles FileList.ColumnHeaderMouseClick
        If e.Button = MouseButtons.Left Then
            ' Disable user sorting
            FileList.AllowUserToOrderColumns = False

            Dim column As DataGridViewColumn = FileList.Columns(e.ColumnIndex)
            intSortColumnIndex = e.ColumnIndex

            If sortOrder = SortOrder.Descending Then
                sortOrder = SortOrder.Ascending
            ElseIf sortOrder = SortOrder.Ascending Then
                sortOrder = SortOrder.Descending
            Else
                sortOrder = SortOrder.Ascending
            End If

            colHidden.HeaderCell.SortGlyphDirection = SortOrder.None
            ColFileName.HeaderCell.SortGlyphDirection = SortOrder.None
            ColFileSize.HeaderCell.SortGlyphDirection = SortOrder.None

            FileList.Columns(e.ColumnIndex).HeaderCell.SortGlyphDirection = sortOrder

            SortLogsByDateObject(column.Index, sortOrder)
        End If
    End Sub

    Private Sub SortLogsByDateObject(columnIndex As Integer, order As SortOrder)
        SortLogsByDateObjectNoLocking(columnIndex, order)
    End Sub

    Public Sub SortLogsByDateObjectNoLocking(columnIndex As Integer, order As SortOrder)
        FileList.AllowUserToOrderColumns = False
        FileList.Enabled = False

        Dim comparer As New MyDataGridViewFileRowComparer(columnIndex, order)
        Dim rows As MyDataGridViewFileRow() = FileList.Rows.Cast(Of DataGridViewRow).OfType(Of MyDataGridViewFileRow)().ToArray()

        Array.Sort(rows, Function(row1 As MyDataGridViewFileRow, row2 As MyDataGridViewFileRow) comparer.Compare(row1, row2))

        FileList.Rows.Clear()
        FileList.Rows.AddRange(rows)
        FileList.Enabled = True
        FileList.AllowUserToOrderColumns = True
    End Sub

    Private Function GetEntryCount(strFileName As String) As Integer
        Try
            Using fileStream As New StreamReader(strFileName)
                Return Newtonsoft.Json.JsonConvert.DeserializeObject(Of List(Of SavedData))(fileStream.ReadToEnd.Trim, JSONDecoderSettingsForLogFiles).Count
            End Using
        Catch ex As Exception
            Return -1
        End Try
    End Function

    Private Sub LoadFileList()
        Dim filesInDirectory As FileInfo()

        If ChkShowHidden.Checked Then
            filesInDirectory = New DirectoryInfo(strPathToDataBackupFolder).GetFiles()
        Else
            filesInDirectory = New DirectoryInfo(strPathToDataBackupFolder).GetFiles().Where(Function(fileinfo As FileInfo) (fileinfo.Attributes And FileAttributes.Hidden) <> FileAttributes.Hidden).ToArray
        End If

        Dim listOfDataGridViewRows As New List(Of DataGridViewRow)
        Dim intHiddenTotalLogCount, intFileCount, intHiddenFileCount As Integer
        Dim longTotalLogCount, longUsedDiskSpace As Long

        Parallel.ForEach(filesInDirectory, Sub(file As FileInfo)
                                               Dim boolIsHidden As Boolean = (file.Attributes And FileAttributes.Hidden) = FileAttributes.Hidden
                                               Dim intCount As Integer = GetEntryCount(file.FullName)

                                               If intCount <> -1 Then
                                                   ' Accumulate counts and totals
                                                   Interlocked.Add(longUsedDiskSpace, file.Length)

                                                   If boolIsHidden Then
                                                       Interlocked.Increment(intHiddenFileCount)
                                                       Interlocked.Add(intHiddenTotalLogCount, intCount)
                                                   Else
                                                       Interlocked.Increment(intFileCount)
                                                       Interlocked.Add(longTotalLogCount, intCount)
                                                   End If

                                                   Dim row As New MyDataGridViewFileRow()

                                                   With row
                                                       .CreateCells(FileList)
                                                       .fileDate = file.CreationTime
                                                       .fileSize = file.Length
                                                       .entryCount = intCount
                                                       .Cells(0).Value = file.Name
                                                       .Cells(0).Style.Alignment = DataGridViewContentAlignment.MiddleLeft

                                                       .Cells(1).Value = $"{file.CreationTime.ToLongDateString} {file.CreationTime.ToLongTimeString}"
                                                       .Cells(2).Style.Alignment = DataGridViewContentAlignment.MiddleLeft

                                                       .Cells(2).Value = FileSizeToHumanSize(file.Length)
                                                       .Cells(2).Style.Alignment = DataGridViewContentAlignment.MiddleLeft

                                                       .Cells(3).Value = $"{intCount:N0}"
                                                       .Cells(3).Style.Alignment = DataGridViewContentAlignment.MiddleLeft

                                                       .Cells(4).Value = If(boolIsHidden, "Yes", "No")
                                                       .Cells(4).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                                                   End With


                                                   For Each cell As DataGridViewCell In row.Cells
                                                       cell.Style.Font = My.Settings.font
                                                       If boolIsHidden AndAlso ChkShowHiddenAsGray.Checked Then cell.Style.ForeColor = Color.Gray
                                                   Next

                                                   ' Thread-safe add to list
                                                   SyncLock listOfDataGridViewRows
                                                       listOfDataGridViewRows.Add(row)
                                                   End SyncLock
                                               End If
                                           End Sub)

        Invoke(Sub()
                   listOfDataGridViewRows = listOfDataGridViewRows.OrderBy(Function(row As MyDataGridViewFileRow) row.fileDate).ToList()

                   If My.Settings.font IsNot Nothing Then
                       FileList.DefaultCellStyle.Font = My.Settings.font
                       FileList.ColumnHeadersDefaultCellStyle.Font = My.Settings.font
                   End If

                   FileList.Rows.Clear()
                   FileList.Rows.AddRange(listOfDataGridViewRows.ToArray())
                   lblNumberOfFiles.Text = $"Number of Files: {intFileCount:N0}"
                   LblTotalDiskSpace.Text = $"Total Disk Space Used: {FileSizeToHumanSize(longUsedDiskSpace)}"
                   lblTotalNumberOfLogs.Text = $"Total Number of Logs: {longTotalLogCount:N0}"

                   lblNumberOfHiddenFiles.Visible = intHiddenFileCount > 0
                   lblTotalNumberOfHiddenLogs.Visible = intHiddenFileCount > 0
                   lblNumberOfHiddenFiles.Text = $"Number of Hidden Files: {intHiddenFileCount:N0}"
                   lblTotalNumberOfHiddenLogs.Text = $"Number of Hidden Logs: {intHiddenTotalLogCount:N0}"
               End Sub)
    End Sub

    Private Sub ViewLogBackups_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim flags As Reflection.BindingFlags = Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance Or Reflection.BindingFlags.SetProperty
        Dim propInfo As Reflection.PropertyInfo = GetType(DataGridView).GetProperty("DoubleBuffered", flags)
        propInfo?.SetValue(FileList, True, Nothing)

        FileList.AlternatingRowsDefaultCellStyle = New DataGridViewCellStyle() With {.BackColor = My.Settings.searchColor}
        FileList.DefaultCellStyle = New DataGridViewCellStyle() With {.WrapMode = DataGridViewTriState.True}

        ColFileDate.Width = My.Settings.ColViewLogBackupsFileDate
        ColFileName.Width = My.Settings.ColViewLogBackupsFileName
        ColFileSize.Width = My.Settings.ColViewLogBackupsFileSize
        colEntryCount.Width = My.Settings.viewLogBackupsEntryCountColumnSize
        colHidden.Width = My.Settings.viewLogBackupsHiddenColumnSize

        LoadColumnOrders(FileList.Columns, My.Settings.fileListColumnOrder)

        ColFileDate.HeaderCell.SortGlyphDirection = SortOrder.Ascending

        colHidden.Visible = My.Settings.boolShowHiddenFilesOnViewLogBackyupsWindow
        ChkShowHidden.Checked = My.Settings.boolShowHiddenFilesOnViewLogBackyupsWindow
        ChkShowHiddenAsGray.Checked = My.Settings.boolShowHiddenAsGray
        ChkShowHiddenAsGray.Enabled = ChkShowHidden.Checked
        Size = My.Settings.ViewLogBackupsSize
        CenterFormOverParent(MyParentForm, Me)
        ThreadPool.QueueUserWorkItem(AddressOf LoadFileList)
        boolDoneLoading = True
    End Sub

    Private Sub FileList_DoubleClick(sender As Object, e As EventArgs) Handles FileList.DoubleClick
        BtnView.PerformClick()
    End Sub

    Private Sub BtnView_Click(sender As Object, e As EventArgs) Handles BtnView.Click
        If FileList.SelectedRows.Count > 0 Then
            Dim fileName As String = Path.Combine(strPathToDataBackupFolder, FileList.SelectedRows(0).Cells(0).Value)

            If File.Exists(fileName) Then
                Dim searchResultsWindow As New IgnoredLogsAndSearchResults(Me, IgnoreOrSearchWindowDisplayMode.viewer) With {.MainProgramForm = MyParentForm, .Icon = Icon, .Text = "Log Viewer", .strFileToLoad = fileName, .boolLoadExternalData = True}
                searchResultsWindow.ChkColLogsAutoFill.Checked = My.Settings.colLogAutoFill
                searchResultsWindow.ShowDialog(Me)
            End If
        End If
    End Sub

    Private Sub FileList_Click(sender As Object, e As EventArgs) Handles FileList.Click
        If FileList.SelectedRows.Count > 0 Then
            BtnDelete.Enabled = True
            BtnView.Enabled = FileList.SelectedRows.Count <= 1
        Else
            BtnDelete.Enabled = False
            BtnView.Enabled = False
        End If
    End Sub

    Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles BtnDelete.Click
        If FileList.SelectedRows.Count > 0 Then
            If FileList.SelectedRows.Count = 1 Then
                Dim fileName As String = Path.Combine(strPathToDataBackupFolder, FileList.SelectedRows(0).Cells(0).Value)

                If MsgBox($"Are you sure you want to delete the file named ""{FileList.SelectedRows(0).Cells(0).Value}""?", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2, Text) = MsgBoxResult.Yes Then
                    File.Delete(fileName)
                    SyslogParser.AddToLogList(Nothing, Net.IPAddress.Loopback.ToString, $"The user deleted ""{FileList.SelectedRows(0).Cells(0).Value}"" from the log backups folder.")
                    ThreadPool.QueueUserWorkItem(AddressOf LoadFileList)
                End If
            Else
                Dim msgBoxText As String = "Are you sure you want to delete the following files?" & vbCrLf & vbCrLf
                Dim listOfFilesThatAreToBeDeleted As New List(Of String)

                For Each item As MyDataGridViewFileRow In FileList.SelectedRows
                    listOfFilesThatAreToBeDeleted.Add(item.Cells(0).Value)
                Next

                Dim listOfFilesThatAreToBeDeletedInHumanReadableFormat As String = ConvertListOfStringsToString(listOfFilesThatAreToBeDeleted, True)

                msgBoxText &= listOfFilesThatAreToBeDeletedInHumanReadableFormat

                If MsgBox(msgBoxText.Trim, MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2, Text) = MsgBoxResult.Yes Then
                    Dim strDeletedFilesLog As String = $"The user deleted the following {FileList.SelectedRows.Count} files from the log backups folder..."

                    For Each item As MyDataGridViewFileRow In FileList.SelectedRows
                        File.Delete(Path.Combine(strPathToDataBackupFolder, item.Cells(0).Value))
                    Next

                    strDeletedFilesLog &= vbCrLf & listOfFilesThatAreToBeDeletedInHumanReadableFormat

                    SyslogParser.AddToLogList(Nothing, Net.IPAddress.Loopback.ToString, strDeletedFilesLog.ToString)

                    ThreadPool.QueueUserWorkItem(AddressOf LoadFileList)
                End If
            End If
        End If
    End Sub

    Private Sub ViewLogBackups_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F5 Then
            ThreadPool.QueueUserWorkItem(AddressOf LoadFileList)
        ElseIf e.KeyCode = Keys.Delete Then
            BtnDelete.PerformClick()
        End If
    End Sub

    Private Sub BtnRefresh_Click(sender As Object, e As EventArgs) Handles BtnRefresh.Click
        ThreadPool.QueueUserWorkItem(AddressOf LoadFileList)
    End Sub

    Private Sub ViewLogBackups_ResizeEnd(sender As Object, e As EventArgs) Handles Me.ResizeEnd
        If boolDoneLoading Then My.Settings.ViewLogBackupsSize = Size
    End Sub

    Private Sub BtnSearch_Click(sender As Object, e As EventArgs) Handles BtnSearch.Click
        If String.IsNullOrWhiteSpace(TxtSearchTerms.Text) Then
            MsgBox("You must provide something to search for.", MsgBoxStyle.Critical, Text)
            Exit Sub
        End If

        Dim listOfSearchResults As New HashSet(Of MyDataGridViewRow)()
        Dim listOfSearchResults2 As New List(Of MyDataGridViewRow)
        Dim regexCompiledObject As Regex = Nothing
        Dim searchResultsWindow As New IgnoredLogsAndSearchResults(Me, IgnoreOrSearchWindowDisplayMode.search) With {.MainProgramForm = MyParentForm, .Icon = Icon, .Text = "Search Results"}
        searchResultsWindow.ChkColLogsAutoFill.Checked = My.Settings.colLogAutoFill

        If My.Settings.font IsNot Nothing Then searchResultsWindow.Logs.DefaultCellStyle.Font = My.Settings.font

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

                                          Dim filesInDirectory As FileInfo()

                                          If ChkShowHidden.Checked Then
                                              filesInDirectory = New DirectoryInfo(strPathToDataBackupFolder).GetFiles()
                                          Else
                                              filesInDirectory = New DirectoryInfo(strPathToDataBackupFolder).GetFiles().Where(Function(fileinfo As FileInfo) (fileinfo.Attributes And FileAttributes.Hidden) <> FileAttributes.Hidden).ToArray
                                          End If

                                          Dim dataFromFile As List(Of SavedData)
                                          Dim myDataGridRow As MyDataGridViewRow

                                          Parallel.ForEach(filesInDirectory, Sub(file As FileInfo)
                                                                                 Using fileStream As New StreamReader(file.FullName)
                                                                                     dataFromFile = Newtonsoft.Json.JsonConvert.DeserializeObject(Of List(Of SavedData))(fileStream.ReadToEnd.Trim, JSONDecoderSettingsForLogFiles)

                                                                                     For Each item As SavedData In dataFromFile
                                                                                         If regexCompiledObject.IsMatch(item.log) Then
                                                                                             myDataGridRow = item.MakeDataGridRow(searchResultsWindow.Logs, GetMinimumHeight(item.log, searchResultsWindow.Logs.DefaultCellStyle.Font, My.Settings.columnLogSize, searchResultsWindow.Logs))
                                                                                             myDataGridRow.Cells(ColumnIndex_FileName).Value = file.Name
                                                                                             If My.Settings.font IsNot Nothing Then myDataGridRow.Cells(ColumnIndex_FileName).Style.Font = My.Settings.font
                                                                                             SyncLock listOfSearchResults ' Ensure thread safety
                                                                                                 listOfSearchResults.Add(myDataGridRow)
                                                                                             End SyncLock
                                                                                         End If
                                                                                     Next
                                                                                 End Using
                                                                             End Sub)

                                          For Each item As SavedData In currentLogs
                                              If regexCompiledObject.IsMatch(item.log) Then
                                                  myDataGridRow = item.MakeDataGridRow(searchResultsWindow.Logs, GetMinimumHeight(item.log, searchResultsWindow.Logs.DefaultCellStyle.Font, My.Settings.columnLogSize, searchResultsWindow.Logs))
                                                  myDataGridRow.Cells(ColumnIndex_FileName).Value = "Current Log Data"
                                                  listOfSearchResults.Add(myDataGridRow)
                                                  myDataGridRow = Nothing
                                              End If
                                          Next

                                          listOfSearchResults2 = listOfSearchResults.Distinct().ToList().OrderBy(Function(row) row.Cells(ColumnIndex_LogText).Value.ToString()).ThenBy(Function(row) row.Cells(ColumnIndex_ComputedTime).Value.ToString()).ToList()
                                      Catch ex As ArgumentException
                                          MsgBox("Malformed RegEx pattern detected, search aborted.", MsgBoxStyle.Critical, Text)
                                      End Try
                                  End Sub

        AddHandler worker.RunWorkerCompleted, Sub()
                                                  If listOfSearchResults2.Count > 0 Then
                                                      searchResultsWindow.LogsToBeDisplayed = listOfSearchResults2
                                                      searchResultsWindow.ColFileName.Visible = True
                                                      searchResultsWindow.OpenLogFileForViewingToolStripMenuItem.Visible = True
                                                      searchResultsWindow.ShowDialog(Me)
                                                  Else
                                                      MsgBox("Search terms not found.", MsgBoxStyle.Information, Text)
                                                  End If

                                                  Invoke(Sub() BtnSearch.Enabled = True)
                                              End Sub

        worker.RunWorkerAsync()
    End Sub

    Private Sub TxtSearchTerms_KeyDown(sender As Object, e As KeyEventArgs) Handles TxtSearchTerms.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            BtnSearch.PerformClick()
        End If
    End Sub

    Private Sub ContextMenuStrip1_Opening(sender As Object, e As CancelEventArgs) Handles ContextMenuStrip1.Opening
        If FileList.SelectedRows.Count > 0 Then
            DeleteToolStripMenuItem.Enabled = True
            ViewToolStripMenuItem.Enabled = FileList.SelectedRows.Count <= 1

            Dim fileName As String = Path.Combine(strPathToDataBackupFolder, FileList.SelectedRows(0).Cells(0).Value)

            If (New FileInfo(fileName).Attributes And FileAttributes.Hidden) = FileAttributes.Hidden Then
                UnhideToolStripMenuItem.Visible = True
                HideToolStripMenuItem.Visible = False
            Else
                UnhideToolStripMenuItem.Visible = False
                HideToolStripMenuItem.Visible = True
            End If
        Else
            DeleteToolStripMenuItem.Enabled = False
            ViewToolStripMenuItem.Enabled = False
        End If
    End Sub

    Private Sub DeleteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteToolStripMenuItem.Click
        BtnDelete.PerformClick()
    End Sub

    Private Sub ViewToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ViewToolStripMenuItem.Click
        BtnView.PerformClick()
    End Sub

    Private Sub ChkShowHidden_Click(sender As Object, e As EventArgs) Handles ChkShowHidden.Click
        My.Settings.boolShowHiddenFilesOnViewLogBackyupsWindow = ChkShowHidden.Checked
        ChkShowHiddenAsGray.Enabled = ChkShowHidden.Checked
        colHidden.Visible = ChkShowHidden.Checked
        BtnRefresh.PerformClick()
    End Sub

    Private Sub UnhideToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UnhideToolStripMenuItem.Click
        Dim fileName As String

        If FileList.SelectedRows.Count > 1 Then
            For Each item As DataGridViewRow In FileList.SelectedRows
                fileName = Path.Combine(strPathToDataBackupFolder, item.Cells(0).Value)
                UnhideFile(fileName)
            Next
        Else
            fileName = Path.Combine(strPathToDataBackupFolder, FileList.SelectedRows(0).Cells(0).Value)
            UnhideFile(fileName)
        End If

        BtnRefresh.PerformClick()
    End Sub

    Private Sub HideToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HideToolStripMenuItem.Click
        Dim fileName As String

        If FileList.SelectedRows.Count > 1 Then
            For Each item As DataGridViewRow In FileList.SelectedRows
                fileName = Path.Combine(strPathToDataBackupFolder, item.Cells(0).Value)
                HideFile(fileName)
            Next
        Else
            fileName = Path.Combine(strPathToDataBackupFolder, FileList.SelectedRows(0).Cells(0).Value)
            HideFile(fileName)
        End If

        BtnRefresh.PerformClick()
    End Sub

    Private Sub HideFile(fileName As String)
        If File.Exists(fileName) Then
            Dim attributes As FileAttributes = File.GetAttributes(fileName)
            attributes = attributes Or FileAttributes.Hidden
            File.SetAttributes(fileName, attributes)
        End If
    End Sub

    Private Sub UnhideFile(fileName As String)
        If File.Exists(fileName) Then
            Dim attributes As FileAttributes = File.GetAttributes(fileName)
            attributes = attributes And Not FileAttributes.Hidden
            File.SetAttributes(fileName, attributes)
        End If
    End Sub

    Private Sub ChkShowHiddenAsGray_Click(sender As Object, e As EventArgs) Handles ChkShowHiddenAsGray.Click
        My.Settings.boolShowHiddenAsGray = ChkShowHiddenAsGray.Checked
        BtnRefresh.PerformClick()
    End Sub

    Private Sub FileList_ColumnWidthChanged(sender As Object, e As DataGridViewColumnEventArgs) Handles FileList.ColumnWidthChanged
        If boolDoneLoading Then
            My.Settings.ColViewLogBackupsFileDate = ColFileDate.Width
            My.Settings.ColViewLogBackupsFileName = ColFileName.Width
            My.Settings.ColViewLogBackupsFileSize = ColFileSize.Width
            My.Settings.viewLogBackupsEntryCountColumnSize = colEntryCount.Width
            My.Settings.viewLogBackupsHiddenColumnSize = colHidden.Width
        End If
    End Sub

    Private Sub ViewLogBackups_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        My.Settings.fileListColumnOrder = SaveColumnOrders(FileList.Columns)
    End Sub
End Class