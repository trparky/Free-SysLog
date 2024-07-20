Imports System.IO
Imports System.Text.RegularExpressions
Imports System.ComponentModel

Public Class ViewLogBackups
    Public MyParentForm As Form1
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

        lblNumberOfFiles.Text = $"Number of Files: {filesInDirectory.Count:N0}"

        For Each file As FileInfo In filesInDirectory
            listViewItem = New ListViewItem With {.Text = file.Name}
            listViewItem.SubItems.Add(file.CreationTime.ToString)
            listViewItem.SubItems.Add($"{FileSizeToHumanSize(file.Length)} ({GetEntryCount(file.FullName):N0} entries)")
            listOfListViewItems.Add(listViewItem)
        Next

        Invoke(Sub()
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
                Dim searchResultsWindow As New IgnoredLogsAndSearchResults(Me) With {.Icon = Icon, .Text = "Log Viewer", .strFileToLoad = fileName, .WindowDisplayMode = IgnoreOrSearchWindowDisplayMode.viewer, .boolLoadExternalData = True}
                searchResultsWindow.ShowDialog(Me)
            End If
        End If
    End Sub

    Private Sub FileList_Click(sender As Object, e As EventArgs) Handles FileList.Click
        If FileList.SelectedItems.Count > 0 Then
            BtnDelete.Enabled = True
            BtnView.Enabled = True
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
                    SendMessageToSysLogServer($"(NoProxy)The user deleted ""{FileList.SelectedItems(0).SubItems(0).Text}"" from the log backups folder.", My.Settings.sysLogPort)
                    LoadFileList()
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
                        SendMessageToSysLogServer($"(NoProxy)The user deleted ""{item.SubItems(0).Text}"" from the log backups folder.", My.Settings.sysLogPort)
                    Next

                    LoadFileList()
                End If
            End If
        End If
    End Sub

    Private Sub ViewLogBackups_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F5 Then
            LoadFileList()
        ElseIf e.KeyCode = Keys.Delete Then
            BtnDelete.PerformClick()
        ElseIf e.KeyCode = Keys.Space Then
            BtnView.PerformClick()
        End If
    End Sub

    Private Sub BtnRefresh_Click(sender As Object, e As EventArgs) Handles BtnRefresh.Click
        LoadFileList()
    End Sub

    Private Sub ViewLogBackups_ResizeEnd(sender As Object, e As EventArgs) Handles Me.ResizeEnd
        If boolDoneLoading Then My.Settings.ViewLogBackupsSize = Size
    End Sub

    Private Sub BtnSearch_Click(sender As Object, e As EventArgs) Handles BtnSearch.Click
        If BtnSearch.Text = "Search" Then
            If String.IsNullOrWhiteSpace(TxtSearchTerms.Text) Then
                MsgBox("You must provide something to search for.", MsgBoxStyle.Critical, Text)
                Exit Sub
            End If

            Dim listOfSearchResults As New List(Of MyDataGridViewRow)
            Dim regexCompiledObject As Regex = Nothing

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

                                              For Each file As FileInfo In filesInDirectory
                                                  Using fileStream As New StreamReader(file.FullName)
                                                      dataFromFile = Newtonsoft.Json.JsonConvert.DeserializeObject(Of List(Of SavedData))(fileStream.ReadToEnd.Trim, JSONDecoderSettings)

                                                      For Each item As SavedData In dataFromFile
                                                          If regexCompiledObject.IsMatch(item.log) Then listOfSearchResults.Add(item.MakeDataGridRow(MyParentForm.Logs, GetMinimumHeight(item.log, MyParentForm.Logs.DefaultCellStyle.Font)))
                                                      Next

                                                      dataFromFile = Nothing
                                                  End Using
                                              Next
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
        End If
    End Sub

    Private Sub TxtSearchTerms_KeyDown(sender As Object, e As KeyEventArgs) Handles TxtSearchTerms.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            BtnSearch.PerformClick()
        End If
    End Sub
End Class