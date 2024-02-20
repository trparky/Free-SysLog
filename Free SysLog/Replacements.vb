Imports System.ComponentModel

Public Class Replacements
    Private Sub BtnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Using AddReplacement As New AddReplacement With {.StartPosition = FormStartPosition.CenterParent, .Icon = Icon}
            AddReplacement.ShowDialog(Me)

            If AddReplacement.boolSuccess Then
                Dim MyReplacementsListViewItem As New MyReplacementsListViewItem(AddReplacement.strReplace)
                MyReplacementsListViewItem.SubItems.Add(AddReplacement.strReplaceWith)
                MyReplacementsListViewItem.SubItems.Add(AddReplacement.boolRegex.ToString)
                MyReplacementsListViewItem.BoolRegex = AddReplacement.boolRegex

                replacementsListView.Items.Add(MyReplacementsListViewItem)
            End If
        End Using
    End Sub

    Private Sub Replacements_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim listOfReplacementsToAdd As New List(Of MyReplacementsListViewItem)

        If Not String.IsNullOrWhiteSpace(My.Settings.replacements) Then
            For Each item As ReplacementsClass In replacementsList
                listOfReplacementsToAdd.Add(item.ToListViewItem())
            Next
        End If

        replacementsListView.Items.AddRange(listOfReplacementsToAdd.ToArray())
    End Sub

    Private Sub Replacements_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        replacementsList.Clear()

        For Each item As MyReplacementsListViewItem In replacementsListView.Items
            replacementsList.Add(New ReplacementsClass With {.BoolRegex = item.BoolRegex, .StrReplace = item.SubItems(0).Text, .StrReplaceWith = item.SubItems(1).Text})
        Next

        My.Settings.replacements = Newtonsoft.Json.JsonConvert.SerializeObject(replacementsList)
    End Sub

    Private Sub ReplacementsListView_KeyUp(sender As Object, e As KeyEventArgs) Handles replacementsListView.KeyUp
        If e.KeyCode = Keys.Delete AndAlso replacementsListView.SelectedItems.Count > 0 Then replacementsListView.Items.Remove(replacementsListView.SelectedItems(0))
    End Sub

    Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If replacementsListView.SelectedItems.Count > 0 Then replacementsListView.Items.Remove(replacementsListView.SelectedItems(0))
    End Sub
End Class