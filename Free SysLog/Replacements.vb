Imports System.ComponentModel

Public Class Replacements
    Private Function CheckForExistingItem(strReplace As String, strReplaceWith As String) As Boolean
        Return replacementsListView.Items.Cast(Of MyReplacementsListViewItem).Any(Function(item As MyReplacementsListViewItem)
                                                                                      Return item.SubItems(0).Text.Equals(strReplace, StringComparison.OrdinalIgnoreCase) And item.SubItems(1).Text.Equals(strReplaceWith, StringComparison.OrdinalIgnoreCase)
                                                                                  End Function)
    End Function

    Private Sub BtnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Using AddReplacement As New AddReplacement With {.StartPosition = FormStartPosition.CenterParent, .Icon = Icon, .Text = "Add Replacement"}
            AddReplacement.ShowDialog(Me)

            If AddReplacement.boolSuccess Then
                If CheckForExistingItem(AddReplacement.strReplace, AddReplacement.strReplaceWith) Then
                    MsgBox("A similar item has already been found in your replacements list.", MsgBoxStyle.Critical, Text)
                    Exit Sub
                End If

                Dim MyReplacementsListViewItem As New MyReplacementsListViewItem(AddReplacement.strReplace)
                MyReplacementsListViewItem.SubItems.Add(AddReplacement.strReplaceWith)
                MyReplacementsListViewItem.SubItems.Add(AddReplacement.boolRegex.ToString)
                MyReplacementsListViewItem.SubItems.Add(AddReplacement.boolCaseSensitive.ToString)
                MyReplacementsListViewItem.BoolRegex = AddReplacement.boolRegex
                MyReplacementsListViewItem.BoolCaseSensitive = AddReplacement.boolCaseSensitive

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
            replacementsClass = New ReplacementsClass With {.BoolRegex = item.BoolRegex, .StrReplace = item.SubItems(0).Text, .StrReplaceWith = item.SubItems(1).Text, .BoolCaseSensitive = item.BoolCaseSensitive}
            replacementsList.Add(replacementsClass)
            tempReplacements.Add(Newtonsoft.Json.JsonConvert.SerializeObject(replacementsClass))
        Next

        My.Settings.replacements = tempReplacements
        My.Settings.Save()
    End Sub

    Private Sub ReplacementsListView_KeyUp(sender As Object, e As KeyEventArgs) Handles replacementsListView.KeyUp
        If replacementsListView.SelectedItems.Count > 0 Then
            If e.KeyCode = Keys.Delete Then
                replacementsListView.Items.Remove(replacementsListView.SelectedItems(0))
            ElseIf e.KeyCode = Keys.Enter Then
                EditItem()
            End If
        End If
    End Sub

    Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        replacementsListView.Items.Remove(replacementsListView.SelectedItems(0))
    End Sub

    Private Sub EditItem()
        Using AddReplacement As New AddReplacement With {.StartPosition = FormStartPosition.CenterParent, .Icon = Icon, .boolEditMode = True, .Text = "Edit Replacement"}
            Dim selectedItemObject As MyReplacementsListViewItem = DirectCast(replacementsListView.SelectedItems(0), MyReplacementsListViewItem)

            AddReplacement.strReplace = selectedItemObject.SubItems(0).Text
            AddReplacement.strReplaceWith = selectedItemObject.SubItems(1).Text
            AddReplacement.boolRegex = selectedItemObject.BoolRegex
            AddReplacement.boolCaseSensitive = selectedItemObject.BoolCaseSensitive

            AddReplacement.ShowDialog(Me)

            If AddReplacement.boolSuccess Then
                selectedItemObject.SubItems(0).Text = AddReplacement.strReplace
                selectedItemObject.SubItems(1).Text = AddReplacement.strReplaceWith
                selectedItemObject.SubItems(2).Text = AddReplacement.boolRegex.ToString
                selectedItemObject.SubItems(3).Text = AddReplacement.boolCaseSensitive.ToString
                selectedItemObject.BoolRegex = AddReplacement.boolRegex
                selectedItemObject.BoolCaseSensitive = AddReplacement.boolCaseSensitive
            End If
        End Using
    End Sub

    Private Sub BtnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        EditItem()
    End Sub

    Private Sub ReplacementsListView_Click(sender As Object, e As EventArgs) Handles replacementsListView.Click
        If replacementsListView.SelectedItems.Count > 0 Then
            btnDelete.Enabled = True
            btnEdit.Enabled = True
        Else
            btnDelete.Enabled = False
            btnEdit.Enabled = False
        End If
    End Sub

    Private Sub replacementsListView_DoubleClick(sender As Object, e As EventArgs) Handles replacementsListView.DoubleClick
        EditItem()
    End Sub
End Class