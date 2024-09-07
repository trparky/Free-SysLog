Imports System.IO
Imports System.Text.RegularExpressions
Imports System.ComponentModel
Imports System.Threading.Tasks

Public Class ViewLogBackups
    Public MyParentForm As Form1
    Public currentLogs As List(Of SavedData)
    Private boolDoneLoading As Boolean = False

    Private Function GetEntryCount(strFileName As String) As Integer
        Using fileStream As New StreamReader(strFileName)
            Return Newtonsoft.Json.JsonConvert.DeserializeObject(Of List(Of SavedData))(fileStream.ReadToEnd.Trim, JSONDecoderSettings).Count
        End Using
    End Function

    Private Sub LoadFileList()
        Dim filesInDirectory As FileInfo() = New DirectoryInfo(strPathToDataBackupFolder).GetFiles()
        Dim listOfListViewItems As New List(Of ListViewItem)
        Dim listViewItem As ListViewItem
        Dim intCount As Integer
        Dim longTotalLogCount As Long

        lblNumberOfFiles.Text = $"Number of Files: {filesInDirectory.Count:N0}"

        For Each file As FileInfo In filesInDirectory
            intCount = GetEntryCount(file.FullName)
            longTotalLogCount += intCount

            listViewItem = New ListViewItem With {.Text = file.Name}
            listViewItem.SubItems.Add($"{file.CreationTime.ToLongDateString} {file.CreationTime.ToLongTimeString}")
            listViewItem.SubItems.Add($"{FileSizeToHumanSize(file.Length)} ({intCount:N0} entries)")
            listOfListViewItems.Add(listViewItem)
        Next

        Invoke(Sub()
                   lblTotalNumberOfLogs.Text = $"Total Number of Logs: {longTotalLogCount:N0}"
                   FileList.Items.Clear()
                   FileList.Items.AddRange(listOfListViewItems.ToArray)
               End Sub)
    End Sub

    Private Sub ViewLogBackups_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Size = My.Settings.ViewLogBackupsSize
        CenterFormOverParent(MyParentForm, Me)
        Threading.ThreadPool.QueueUserWorkItem(AddressOf LoadFileList)
        boolDoneLoading = True
    End Sub

    Private Sub FileList_DoubleClick(sender As Object, e As EventArgs) Handles FileList.DoubleClick
        BtnView.PerformClick()
    End Sub

    Private Sub BtnView_Click(sender As Object, e As EventArgs) Handles BtnView.Click
        If FileList.SelectedItems.Count > 0 Then
            Dim fileName As String = Path.Combine(strPathToDataBackupFolder, FileList.SelectedItems(0).SubItems(0).Text)

            If File.Exists(fileName) Then
                Dim searchResultsWindow As New IgnoredLogsAndSearchResults(Me) With {.MainProgramForm = MyParentForm, .Icon = Icon, .Text = "Log Viewer", .strFileToLoad = fileName, .WindowDisplayMode = IgnoreOrSearchWindowDisplayMode.viewer, .boolLoadExternalData = True}
                searchResultsWindow.ShowDialog(Me)
            End If
        End If
    End Sub

    Private Sub FileList_Click(sender As Object, e As EventArgs) Handles FileList.Click
        If FileList.SelectedItems.Count > 0 Then
            BtnDelete.Enabled = True
            BtnView.Enabled = FileList.SelectedItems.Count <= 1
        Else
            BtnDelete.Enabled = False
            BtnView.Enabled = False
        End If
    End Sub

    Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles BtnDelete.Click
        If FileList.SelectedItems.Count > 0 Then
            If FileList.SelectedItems.Count = 1 Then
                Dim fileName As String = Path.Combine(strPathToDataBackupFolder, FileList.SelectedItems(0).SubItems(0).Text)

                If MsgBox($"Are you sure you want to delete the file named ""{FileList.SelectedItems(0).SubItems(0).Text}""?", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2, Text) = MsgBoxResult.Yes Then
                    File.Delete(fileName)
                    SyslogParser.AddToLogList(Nothing, Net.IPAddress.Loopback.ToString, $"The user deleted ""{FileList.SelectedItems(0).SubItems(0).Text}"" from the log backups folder.")
                    Threading.ThreadPool.QueueUserWorkItem(AddressOf LoadFileList)
                End If
            Else
                Dim stringBuilder As New Text.StringBuilder()

                stringBuilder.AppendLine("Are you sure you want to delete the following files?")
                stringBuilder.AppendLine()

                For Each item As ListViewItem In FileList.SelectedItems
                    stringBuilder.AppendLine(item.SubItems(0).Text)
                Next

                If MsgBox(stringBuilder.ToString.Trim, MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2, Text) = MsgBoxResult.Yes Then
                    For Each item As ListViewItem In FileList.SelectedItems
                        File.Delete(Path.Combine(strPathToDataBackupFolder, item.SubItems(0).Text))
                        SyslogParser.AddToLogList(Nothing, Net.IPAddress.Loopback.ToString, $"The user deleted ""{item.SubItems(0).Text}"" from the log backups folder.")
                    Next

                    Threading.ThreadPool.QueueUserWorkItem(AddressOf LoadFileList)
                End If
            End If
        End If
    End Sub

    Private Sub ViewLogBackups_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F5 Then
            Threading.ThreadPool.QueueUserWorkItem(AddressOf LoadFileList)
        ElseIf e.KeyCode = Keys.Delete Then
            BtnDelete.PerformClick()
        End If
    End Sub

    Private Sub BtnRefresh_Click(sender As Object, e As EventArgs) Handles BtnRefresh.Click
        Threading.ThreadPool.QueueUserWorkItem(AddressOf LoadFileList)
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
        Dim searchResultsWindow As New IgnoredLogsAndSearchResults(Me) With {.MainProgramForm = MyParentForm, .Icon = Icon, .Text = "Search Results", .WindowDisplayMode = IgnoreOrSearchWindowDisplayMode.search}

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

                                          Dim filesInDirectory As FileInfo() = New DirectoryInfo(strPathToDataBackupFolder).GetFiles()
                                          Dim dataFromFile As List(Of SavedData)
                                          Dim myDataGridRow As MyDataGridViewRow

                                          Parallel.ForEach(filesInDirectory, Sub(file As FileInfo)
                                                                                 Using fileStream As New StreamReader(file.FullName)
                                                                                     dataFromFile = Newtonsoft.Json.JsonConvert.DeserializeObject(Of List(Of SavedData))(fileStream.ReadToEnd.Trim, JSONDecoderSettings)

                                                                                     For Each item As SavedData In dataFromFile
                                                                                         If regexCompiledObject.IsMatch(item.log) Then
                                                                                             myDataGridRow = item.MakeDataGridRow(searchResultsWindow.Logs, GetMinimumHeight(item.log, searchResultsWindow.Logs.DefaultCellStyle.Font, My.Settings.columnLogSize))
                                                                                             myDataGridRow.Cells(ColumnIndex_FileName).Value = file.Name
                                                                                             SyncLock listOfSearchResults ' Ensure thread safety
                                                                                                 listOfSearchResults.Add(myDataGridRow)
                                                                                             End SyncLock
                                                                                         End If
                                                                                     Next
                                                                                 End Using
                                                                             End Sub)

                                          For Each item As SavedData In currentLogs
                                              If regexCompiledObject.IsMatch(item.log) Then
                                                  myDataGridRow = item.MakeDataGridRow(searchResultsWindow.Logs, GetMinimumHeight(item.log, searchResultsWindow.Logs.DefaultCellStyle.Font, My.Settings.columnLogSize))
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
        If FileList.SelectedItems.Count > 0 Then
            DeleteToolStripMenuItem.Enabled = True
            ViewToolStripMenuItem.Enabled = FileList.SelectedItems.Count <= 1
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
End Class