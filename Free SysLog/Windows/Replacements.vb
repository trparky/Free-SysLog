Imports System.ComponentModel

Public Class Replacements
    Private boolDoneLoading As Boolean = False

    Private Function CheckForExistingItem(strReplace As String, strReplaceWith As String) As Boolean
        Return ReplacementsListView.Items.Cast(Of MyReplacementsListViewItem).Any(Function(item As MyReplacementsListViewItem)
                                                                                      Return item.SubItems(0).Text.Equals(strReplace, StringComparison.OrdinalIgnoreCase) And item.SubItems(1).Text.Equals(strReplaceWith, StringComparison.OrdinalIgnoreCase)
                                                                                  End Function)
    End Function

    Private Sub BtnAdd_Click(sender As Object, e As EventArgs) Handles BtnAdd.Click
        Using AddReplacement As New AddReplacement With {.StartPosition = FormStartPosition.CenterParent, .Icon = Icon, .Text = "Add Replacement"}
            AddReplacement.ShowDialog(Me)

            If AddReplacement.boolSuccess Then
                If CheckForExistingItem(AddReplacement.strReplace, AddReplacement.strReplaceWith) Then
                    MsgBox("A similar item has already been found in your replacements list.", MsgBoxStyle.Critical, Text)
                    Exit Sub
                End If

                Dim MyReplacementsListViewItem As New MyReplacementsListViewItem(AddReplacement.strReplace)

                With MyReplacementsListViewItem
                    .SubItems.Add(AddReplacement.strReplaceWith)
                    .SubItems.Add(AddReplacement.boolRegex.ToString)
                    .SubItems.Add(AddReplacement.boolCaseSensitive.ToString)
                    .SubItems.Add(AddReplacement.boolEnabled.ToString)
                    .BoolRegex = AddReplacement.boolRegex
                    .BoolCaseSensitive = AddReplacement.boolCaseSensitive
                    .BoolEnabled = AddIgnored.boolEnabled
                End With

                ReplacementsListView.Items.Add(MyReplacementsListViewItem)
            End If
        End Using
    End Sub

    Private Sub Ignored_Words_and_Phrases_LocationChanged(sender As Object, e As EventArgs) Handles Me.LocationChanged
        If boolDoneLoading Then My.Settings.replacementsLocation = Location
    End Sub

    Private Sub Replacements_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Location = VerifyWindowLocation(My.Settings.replacementsLocation, Me)
        Dim listOfReplacementsToAdd As New List(Of MyReplacementsListViewItem)

        If My.Settings.replacements IsNot Nothing AndAlso My.Settings.replacements.Count > 0 Then
            For Each strJSONString As String In My.Settings.replacements
                listOfReplacementsToAdd.Add(Newtonsoft.Json.JsonConvert.DeserializeObject(Of ReplacementsClass)(strJSONString).ToListViewItem())
            Next
        End If

        ReplacementsListView.Items.AddRange(listOfReplacementsToAdd.ToArray())

        boolDoneLoading = True
    End Sub

    Private Sub Replacements_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        replacementsList.Clear()

        Dim replacementsClass As ReplacementsClass
        Dim tempReplacements As New Specialized.StringCollection()

        For Each item As MyReplacementsListViewItem In ReplacementsListView.Items
            replacementsClass = New ReplacementsClass With {.BoolRegex = item.BoolRegex, .StrReplace = item.SubItems(0).Text, .StrReplaceWith = item.SubItems(1).Text, .BoolCaseSensitive = item.BoolCaseSensitive, .BoolEnabled = item.BoolEnabled}
            replacementsList.Add(replacementsClass)
            tempReplacements.Add(Newtonsoft.Json.JsonConvert.SerializeObject(replacementsClass))
        Next

        My.Settings.replacements = tempReplacements
        My.Settings.Save()
    End Sub

    Private Sub ReplacementsListView_KeyUp(sender As Object, e As KeyEventArgs) Handles ReplacementsListView.KeyUp
        If ReplacementsListView.SelectedItems.Count > 0 Then
            If e.KeyCode = Keys.Delete Then
                ReplacementsListView.Items.Remove(ReplacementsListView.SelectedItems(0))
                BtnDelete.Enabled = False
                BtnEdit.Enabled = False
            ElseIf e.KeyCode = Keys.Enter Then
                EditItem()
            End If
        End If
    End Sub

    Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles BtnDelete.Click
        ReplacementsListView.Items.Remove(ReplacementsListView.SelectedItems(0))
    End Sub

    Private Sub EditItem()
        Using AddReplacement As New AddReplacement With {.StartPosition = FormStartPosition.CenterParent, .Icon = Icon, .boolEditMode = True, .Text = "Edit Replacement"}
            Dim selectedItemObject As MyReplacementsListViewItem = DirectCast(ReplacementsListView.SelectedItems(0), MyReplacementsListViewItem)

            With AddReplacement
                .strReplace = selectedItemObject.SubItems(0).Text
                .strReplaceWith = selectedItemObject.SubItems(1).Text
                .boolRegex = selectedItemObject.BoolRegex
                .boolCaseSensitive = selectedItemObject.BoolCaseSensitive
                .boolEnabled = selectedItemObject.BoolEnabled
            End With

            AddReplacement.ShowDialog(Me)

            If AddReplacement.boolSuccess Then
                With selectedItemObject
                    .SubItems(0).Text = AddReplacement.strReplace
                    .SubItems(1).Text = AddReplacement.strReplaceWith
                    .SubItems(2).Text = AddReplacement.boolRegex.ToString
                    .SubItems(3).Text = AddReplacement.boolCaseSensitive.ToString
                    .SubItems(4).Text = AddReplacement.boolEnabled.ToString
                    .BoolRegex = AddReplacement.boolRegex
                    .BoolCaseSensitive = AddReplacement.boolCaseSensitive
                    .BoolEnabled = AddReplacement.boolEnabled
                End With
            End If
        End Using
    End Sub

    Private Sub BtnEdit_Click(sender As Object, e As EventArgs) Handles BtnEdit.Click
        EditItem()
    End Sub

    Private Sub ReplacementsListView_Click(sender As Object, e As EventArgs) Handles ReplacementsListView.Click
        If ReplacementsListView.SelectedItems.Count > 0 Then
            BtnDelete.Enabled = True
            BtnEdit.Enabled = True
        Else
            BtnDelete.Enabled = False
            BtnEdit.Enabled = False
        End If
    End Sub

    Private Sub ReplacementsListView_DoubleClick(sender As Object, e As EventArgs) Handles ReplacementsListView.DoubleClick
        EditItem()
    End Sub
End Class