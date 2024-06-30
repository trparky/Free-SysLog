Imports System.IO

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
        Dim intEntryCount As Integer

        For Each file As FileInfo In filesInDirectory
            intEntryCount = GetEntryCount(file.FullName)

            listViewItem = New ListViewItem With {.Text = file.Name}
            listViewItem.SubItems.Add(file.CreationTime.ToString)
            listViewItem.SubItems.Add($"{FileSizeToHumanSize(file.Length)} ({intEntryCount:N0} entries)")
            Invoke(Sub() listOfListViewItems.Add(listViewItem))
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
        ElseIf e.KeyCode = Keys.space Then
            BtnView.PerformClick()
        End If
    End Sub

    Private Sub BtnRefresh_Click(sender As Object, e As EventArgs) Handles BtnRefresh.Click
        LoadFileList()
    End Sub

    Private Sub ViewLogBackups_ResizeEnd(sender As Object, e As EventArgs) Handles Me.ResizeEnd
        If boolDoneLoading Then My.Settings.ViewLogBackupsSize = Size
    End Sub
End Class