Imports System.IO

Public Class ViewLogBackups
    Public MyParentForm As Form1

    Private Sub LoadFileList()
        Dim filesInDirectory As FileInfo() = New DirectoryInfo(strPathToDataBackupFolder).GetFiles()
        Dim listOfListViewItems As New List(Of ListViewItem)
        Dim listViewItem As ListViewItem

        For Each file As FileInfo In filesInDirectory
            listViewItem = New ListViewItem With {.Text = file.Name}
            listViewItem.SubItems.Add(file.CreationTime.ToString)
            listViewItem.SubItems.Add(FileSizeToHumanSize(file.Length))
            listOfListViewItems.Add(listViewItem)
        Next

        FileList.Items.Clear()
        FileList.Items.AddRange(listOfListViewItems.ToArray)
    End Sub

    Private Sub ViewLogBackups_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CenterFormOverParent(MyParentForm, Me)
        LoadFileList()
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
            Dim fileName As String = Path.Combine(strPathToDataBackupFolder, FileList.SelectedItems(0).SubItems(0).Text)

            If MsgBox($"Are you sure you want to delete the file named ""{FileList.SelectedItems(0).SubItems(0).Text}""?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, Text) = MsgBoxResult.Yes Then
                File.Delete(fileName)
                LoadFileList()
            End If
        End If
    End Sub
End Class