Public Class IgnoredWordsAndPhrases
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
                    MsgBox("A similar item has already been found in your ignored list.", MsgBoxStyle.Critical, Text)
                    Exit Sub
                End If

                Dim IgnoredListViewItem As New MyIgnoredListViewItem(AddIgnored.strIgnored)

                With IgnoredListViewItem
                    .SubItems.Add(AddIgnored.boolRegex.ToString)
                    .SubItems.Add(AddIgnored.boolCaseSensitive.ToString)
                    .SubItems.Add(AddIgnored.boolEnabled.ToString)
                    .BoolRegex = AddIgnored.boolRegex
                    .BoolCaseSensitive = AddIgnored.boolCaseSensitive
                    .BoolEnabled = AddIgnored.boolEnabled
                End With

                IgnoredListView.Items.Add(IgnoredListViewItem)
            End If
        End Using
    End Sub

    Private Sub IgnoredWordsAndPhrases_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        ignoredList.Clear()

        Dim ignoredClass As IgnoredClass
        Dim tempIgnored As New Specialized.StringCollection()

        For Each item As MyIgnoredListViewItem In IgnoredListView.Items
            ignoredClass = New IgnoredClass() With {.StrIgnore = item.SubItems(0).Text, .BoolCaseSensitive = item.BoolCaseSensitive, .BoolRegex = item.BoolRegex, .BoolEnabled = item.BoolEnabled}
            ignoredList.Add(ignoredClass)
            tempIgnored.Add(Newtonsoft.Json.JsonConvert.SerializeObject(ignoredClass))
        Next

        My.Settings.ignored2 = tempIgnored
        My.Settings.Save()
    End Sub

    Private Sub IgnoredWordsAndPhrases_Load(sender As Object, e As EventArgs) Handles Me.Load
        Location = VerifyWindowLocation(My.Settings.ignoredWordsLocation, Me)
        Dim MyIgnoredListViewItem As New List(Of MyIgnoredListViewItem)

        If My.Settings.ignored2 IsNot Nothing AndAlso My.Settings.ignored2.Count > 0 Then
            For Each strJSONString As String In My.Settings.ignored2
                MyIgnoredListViewItem.Add(Newtonsoft.Json.JsonConvert.DeserializeObject(Of IgnoredClass)(strJSONString).ToListViewItem())
            Next
        End If

        IgnoredListView.Items.AddRange(MyIgnoredListViewItem.ToArray())

        Replace.Width = My.Settings.colIgnoredReplace
        Regex.Width = My.Settings.colIgnoredRegex
        CaseSensitive.Width = My.Settings.colIgnoredCaseSensitive
        ColEnabled.Width = My.Settings.colIgnoredEnabled

        boolDoneLoading = True
    End Sub

    Private Sub IgnoredListView_KeyUp(sender As Object, e As KeyEventArgs) Handles IgnoredListView.KeyUp
        If e.KeyCode = Keys.Delete And IgnoredListView.SelectedItems().Count > 0 Then
            IgnoredListView.Items.Remove(IgnoredListView.SelectedItems(0))
            BtnDelete.Enabled = False
            BtnEdit.Enabled = False
        End If
    End Sub

    Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles BtnDelete.Click
        If IgnoredListView.SelectedItems().Count > 0 Then
            IgnoredListView.Items.Remove(IgnoredListView.SelectedItems(0))
            BtnDelete.Enabled = False
            BtnEdit.Enabled = False
        End If
    End Sub

    Private Sub IgnoredListView_Click(sender As Object, e As EventArgs) Handles IgnoredListView.Click
        If IgnoredListView.SelectedItems.Count > 0 Then
            BtnDelete.Enabled = True
            BtnEdit.Enabled = True
            BtnEnableDisable.Enabled = True

            BtnEnableDisable.Text = If(DirectCast(IgnoredListView.SelectedItems(0), MyIgnoredListViewItem).BoolEnabled, "Disable", "Enable")
        Else
            BtnDelete.Enabled = False
            BtnEdit.Enabled = False
            BtnEnableDisable.Enabled = False
        End If
    End Sub

    Private Sub Ignored_Words_and_Phrases_LocationChanged(sender As Object, e As EventArgs) Handles Me.LocationChanged
        If boolDoneLoading Then My.Settings.ignoredWordsLocation = Location
    End Sub

    Private Sub EditItem()
        Using AddIgnored As New AddIgnored With {.StartPosition = FormStartPosition.CenterParent, .Icon = Icon, .boolEditMode = True}
            Dim selectedItemObject As MyIgnoredListViewItem = DirectCast(IgnoredListView.SelectedItems(0), MyIgnoredListViewItem)

            With AddIgnored
                .strIgnored = selectedItemObject.SubItems(0).Text
                .boolRegex = selectedItemObject.BoolRegex
                .boolCaseSensitive = selectedItemObject.BoolCaseSensitive
                .boolEnabled = selectedItemObject.BoolEnabled
            End With

            AddIgnored.ShowDialog(Me)

            If AddIgnored.boolSuccess Then
                With selectedItemObject
                    .SubItems(0).Text = AddIgnored.strIgnored
                    .SubItems(1).Text = AddIgnored.boolRegex.ToString
                    .SubItems(2).Text = AddIgnored.boolCaseSensitive.ToString
                    .SubItems(3).Text = AddIgnored.boolEnabled.ToString
                    .BoolRegex = AddIgnored.boolRegex
                    .BoolCaseSensitive = AddIgnored.boolCaseSensitive
                    .BoolEnabled = AddIgnored.boolEnabled
                End With
            End If
        End Using
    End Sub

    Private Sub IgnoredListView_DoubleClick(sender As Object, e As EventArgs) Handles IgnoredListView.DoubleClick
        EditItem()
    End Sub

    Private Sub BtnEdit_Click(sender As Object, e As EventArgs) Handles BtnEdit.Click
        EditItem()
    End Sub

    Private Sub ListViewMenu_Opening(sender As Object, e As ComponentModel.CancelEventArgs) Handles ListViewMenu.Opening
        If IgnoredListView.SelectedItems.Count = 0 And IgnoredListView.SelectedItems.Count > 1 Then
            e.Cancel = True
            Exit Sub
        Else
            Dim selectedItem As MyIgnoredListViewItem = IgnoredListView.SelectedItems(0)
            EnableDisableToolStripMenuItem.Text = If(selectedItem.BoolEnabled, "Disable", "Enable")
        End If
    End Sub

    Private Sub DisableEnableItem()
        Dim selectedItem As MyIgnoredListViewItem = IgnoredListView.SelectedItems(0)

        If selectedItem.BoolEnabled Then
            selectedItem.BoolEnabled = False
            selectedItem.SubItems(3).Text = "False"
            BtnEnableDisable.Text = "Enable"
        Else
            selectedItem.BoolEnabled = True
            selectedItem.SubItems(3).Text = "True"
            BtnEnableDisable.Text = "Disable"
        End If
    End Sub

    Private Sub BtnEnableDisable_Click(sender As Object, e As EventArgs) Handles BtnEnableDisable.Click
        DisableEnableItem()
    End Sub

    Private Sub EnableDisableToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EnableDisableToolStripMenuItem.Click
        DisableEnableItem()
    End Sub

    Private Sub IgnoredListView_ColumnWidthChanged(sender As Object, e As ColumnWidthChangedEventArgs) Handles IgnoredListView.ColumnWidthChanged
        If boolDoneLoading Then
            My.Settings.colIgnoredReplace = Replace.Width
            My.Settings.colIgnoredRegex = Regex.Width
            My.Settings.colIgnoredCaseSensitive = CaseSensitive.Width
            My.Settings.colIgnoredEnabled = ColEnabled.Width
        End If
    End Sub
End Class