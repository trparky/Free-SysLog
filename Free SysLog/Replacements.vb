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

        If My.Settings.replacements IsNot Nothing AndAlso My.Settings.replacements.Count > 0 Then
            For Each strJSONString As String In My.Settings.replacements
                listOfReplacementsToAdd.Add(Newtonsoft.Json.JsonConvert.DeserializeObject(Of ReplacementsClass)(strJSONString).ToListViewItem())
            Next
        End If

        replacementsListView.Items.AddRange(listOfReplacementsToAdd.ToArray())
    End Sub

    Private Sub Replacements_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        replacementsList.Clear()

        Dim replacementsClass As ReplacementsClass
        Dim tempReplacements As New Specialized.StringCollection()

        For Each item As MyReplacementsListViewItem In replacementsListView.Items
            replacementsClass = New ReplacementsClass With {.BoolRegex = item.BoolRegex, .StrReplace = item.SubItems(0).Text, .StrReplaceWith = item.SubItems(1).Text}
            replacementsList.Add(replacementsClass)
            tempReplacements.Add(Newtonsoft.Json.JsonConvert.SerializeObject(replacementsClass))
        Next

        My.Settings.replacements = tempReplacements
    End Sub

    Private Sub ReplacementsListView_KeyUp(sender As Object, e As KeyEventArgs) Handles replacementsListView.KeyUp
        If e.KeyCode = Keys.Delete AndAlso replacementsListView.SelectedItems.Count > 0 Then replacementsListView.Items.Remove(replacementsListView.SelectedItems(0))
    End Sub

    Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If replacementsListView.SelectedItems.Count > 0 Then replacementsListView.Items.Remove(replacementsListView.SelectedItems(0))
    End Sub
End Class