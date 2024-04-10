Public Class Ignored_Words_and_Phrases
    Private boolDoneLoading As Boolean = False

    Private Function CheckForExistingItem(strIgnored As String) As Boolean
        Return IgnoredListView.Items.Cast(Of MyIgnoredListViewItem).Any(Function(item As MyIgnoredListViewItem)
                                                                            Return item.SubItems(0).Text.Equals(strIgnored, StringComparison.OrdinalIgnoreCase)
                                                                        End Function)
    End Function

    Private Sub BtnAdd_Click(sender As Object, e As EventArgs) Handles BtnAdd.Click
        Using AddIgnored As New AddIgnored With {.StartPosition = FormStartPosition.CenterParent, .Icon = Icon, .Text = "Add Ignored String"}
            AddIgnored.ShowDialog(Me)

            If AddIgnored.boolSuccess Then
                If CheckForExistingItem(AddIgnored.strIgnored) Then
                    MsgBox("A similar item has already been found in your replacements list.", MsgBoxStyle.Critical, Text)
                    Exit Sub
                End If

                Dim MyIgnoredListViewItem As New MyIgnoredListViewItem(AddIgnored.strIgnored)
                MyIgnoredListViewItem.SubItems.Add(AddIgnored.boolRegex.ToString)
                MyIgnoredListViewItem.SubItems.Add(AddIgnored.boolCaseSensitive.ToString)
                MyIgnoredListViewItem.BoolRegex = AddIgnored.boolRegex
                MyIgnoredListViewItem.BoolCaseSensitive = AddIgnored.boolCaseSensitive

                IgnoredListView.Items.Add(MyIgnoredListViewItem)
            End If
        End Using
    End Sub

    Private Sub Ignored_Words_and_Phrases_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        ignoredList.Clear()

        Dim ignoredClass As IgnoredClass
        Dim tempIgnored As New Specialized.StringCollection()

        For Each item As MyIgnoredListViewItem In IgnoredListView.Items
            ignoredClass = New IgnoredClass() With {.StrIgnore = item.SubItems(0).Text, .BoolCaseSensitive = item.BoolCaseSensitive, .BoolRegex = item.BoolRegex}
            ignoredList.Add(ignoredClass)
            tempIgnored.Add(Newtonsoft.Json.JsonConvert.SerializeObject(ignoredClass))
        Next

        My.Settings.ignored2 = tempIgnored
        My.Settings.Save()
    End Sub

    Private Sub Ignored_Words_and_Phrases_Load(sender As Object, e As EventArgs) Handles Me.Load
        Location = VerifyWindowLocation(My.Settings.ignoredWordsLocation, Me)

        Dim MyIgnoredListViewItem As New List(Of MyIgnoredListViewItem)

        If My.Settings.ignored2 IsNot Nothing AndAlso My.Settings.ignored2.Count > 0 Then
            For Each strJSONString As String In My.Settings.ignored2
                MyIgnoredListViewItem.Add(Newtonsoft.Json.JsonConvert.DeserializeObject(Of IgnoredClass)(strJSONString).ToListViewItem())
            Next
        End If

        IgnoredListView.Items.AddRange(MyIgnoredListViewItem.ToArray())

        boolDoneLoading = True
    End Sub

    Private Sub IgnoredListView_KeyUp(sender As Object, e As KeyEventArgs) Handles IgnoredListView.KeyUp
        If e.KeyCode = Keys.Delete And IgnoredListView.SelectedItems().Count > 0 Then IgnoredListView.Items.Remove(IgnoredListView.SelectedItems(0))
    End Sub

    Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles BtnDelete.Click
        If IgnoredListView.SelectedItems().Count > 0 Then
            IgnoredListView.Items.Remove(IgnoredListView.SelectedItems(0))
            BtnDelete.Enabled = False
        End If
    End Sub

    Private Sub IgnoredListView_Click(sender As Object, e As EventArgs) Handles IgnoredListView.Click
        BtnDelete.Enabled = IgnoredListView.SelectedItems().Count > 0
    End Sub

    Private Sub Ignored_Words_and_Phrases_LocationChanged(sender As Object, e As EventArgs) Handles Me.LocationChanged
        If boolDoneLoading Then My.Settings.ignoredWordsLocation = Location
    End Sub

    Private Sub EditItem()
        Using AddIgnored As New AddIgnored With {.StartPosition = FormStartPosition.CenterParent, .Icon = Icon, .boolEditMode = True, .Text = "Edit Ignored String"}
            Dim selectedItemObject As MyIgnoredListViewItem = DirectCast(IgnoredListView.SelectedItems(0), MyIgnoredListViewItem)

            AddIgnored.strIgnored = selectedItemObject.SubItems(0).Text
            AddIgnored.boolRegex = selectedItemObject.BoolRegex
            AddIgnored.boolCaseSensitive = selectedItemObject.BoolCaseSensitive

            AddIgnored.ShowDialog(Me)

            If AddIgnored.boolSuccess Then
                selectedItemObject.SubItems(0).Text = AddIgnored.strIgnored
                selectedItemObject.SubItems(1).Text = AddIgnored.boolRegex.ToString
                selectedItemObject.SubItems(2).Text = AddIgnored.boolCaseSensitive.ToString
                selectedItemObject.BoolRegex = AddIgnored.boolRegex
                selectedItemObject.BoolCaseSensitive = AddIgnored.boolCaseSensitive
            End If
        End Using
    End Sub

    Private Sub IgnoredListView_DoubleClick(sender As Object, e As EventArgs) Handles IgnoredListView.DoubleClick
        EditItem()
    End Sub

    Private Sub BtnEdit_Click(sender As Object, e As EventArgs) Handles BtnEdit.Click
        EditItem()
    End Sub
End Class