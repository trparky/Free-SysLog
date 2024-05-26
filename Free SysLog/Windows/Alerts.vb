Public Class Alerts
    Private boolDoneLoading As Boolean = False

    Private Function CheckForExistingItem(strIgnored As String) As Boolean
        Return AlertsListView.Items.Cast(Of AlertsListViewItem).Any(Function(item As AlertsListViewItem)
                                                                        Return item.SubItems(0).Text.Equals(strIgnored, StringComparison.OrdinalIgnoreCase)
                                                                    End Function)
    End Function

    Private Sub Alerts_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Location = VerifyWindowLocation(My.Settings.alertsLocation, Me)
        Dim MyIgnoredListViewItem As New List(Of AlertsListViewItem)

        For Each alertsClass In alertsList
            MyIgnoredListViewItem.Add(alertsClass.ToListViewItem)
        Next

        AlertsListView.Items.AddRange(MyIgnoredListViewItem.ToArray)

        AlertLogText.Width = My.Settings.colAlertsAlertLogText
        AlertText.Width = My.Settings.colAlertsAlertText
        Regex.Width = My.Settings.colAlertsRegex
        CaseSensitive.Width = My.Settings.colAlertsCaseSensitive
        AlertTypeColumn.Width = My.Settings.colAlertsType
        ColEnabled.Width = My.Settings.colAlertsEnabled

        Size = My.Settings.ConfigureAlertsSize

        boolDoneLoading = True
    End Sub

    Private Sub AlertsListView_KeyUp(sender As Object, e As KeyEventArgs) Handles AlertsListView.KeyUp
        If e.KeyCode = Keys.Delete And AlertsListView.SelectedItems().Count > 0 Then
            AlertsListView.Items.Remove(AlertsListView.SelectedItems(0))
            BtnDelete.Enabled = False
            BtnEdit.Enabled = False
        End If
    End Sub

    Private Sub AlertsListView_Click(sender As Object, e As EventArgs) Handles AlertsListView.Click
        If AlertsListView.SelectedItems.Count > 0 Then
            BtnDelete.Enabled = True
            BtnEdit.Enabled = True
            BtnEnableDisable.Enabled = True

            BtnEnableDisable.Text = If(DirectCast(AlertsListView.SelectedItems(0), AlertsListViewItem).BoolEnabled, "Disable", "Enable")
        Else
            BtnDelete.Enabled = False
            BtnEdit.Enabled = False
            BtnEnableDisable.Enabled = False
        End If
    End Sub

    Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles BtnDelete.Click
        If AlertsListView.SelectedItems().Count > 0 Then
            AlertsListView.Items.Remove(AlertsListView.SelectedItems(0))
            BtnDelete.Enabled = False
            BtnEdit.Enabled = False
        End If
    End Sub

    Private Sub Alerts_LocationChanged(sender As Object, e As EventArgs) Handles Me.LocationChanged
        If boolDoneLoading Then My.Settings.alertsLocation = Location
    End Sub

    Private Sub EditItem()
        Using AddAlert As New AddAlert With {.StartPosition = FormStartPosition.CenterParent, .Icon = Icon, .boolEditMode = True}
            Dim selectedItemObject As AlertsListViewItem = DirectCast(AlertsListView.SelectedItems(0), AlertsListViewItem)

            With AddAlert
                .strLogText = selectedItemObject.StrLogText
                .strAlertText = selectedItemObject.StrAlertText
                .boolRegex = selectedItemObject.BoolRegex
                .boolCaseSensitive = selectedItemObject.BoolCaseSensitive
                .AlertType = selectedItemObject.AlertType
                .boolEnabled = selectedItemObject.BoolEnabled
            End With

            AddAlert.ShowDialog(Me)

            If AddAlert.boolSuccess Then
                With selectedItemObject
                    .StrLogText = AddAlert.strLogText
                    .StrAlertText = AddAlert.strAlertText
                    .SubItems(0).Text = AddAlert.strLogText
                    .SubItems(1).Text = If(String.IsNullOrWhiteSpace(AddAlert.strAlertText), "(Shows Log Text)", AddAlert.strAlertText)
                    .SubItems(2).Text = If(AddAlert.boolRegex, "Yes", "No")
                    .SubItems(3).Text = If(AddAlert.boolCaseSensitive, "Yes", "No")
                    .BoolRegex = AddAlert.boolRegex
                    .BoolCaseSensitive = AddAlert.boolCaseSensitive
                    .AlertType = AddAlert.AlertType
                    .BoolEnabled = AddAlert.boolEnabled

                    Select Case .AlertType
                        Case AlertType.Warning
                            .SubItems(4).Text = "Warning"
                        Case AlertType.ErrorMsg
                            .SubItems(4).Text = "Error"
                        Case AlertType.Info
                            .SubItems(4).Text = "Information"
                        Case AlertType.None
                            .SubItems(4).Text = "None"
                    End Select

                    .SubItems(5).Text = If(AddAlert.boolEnabled, "Yes", "No")
                End With
            End If
        End Using
    End Sub

    Private Sub AlertsListView_DoubleClick(sender As Object, e As EventArgs) Handles AlertsListView.DoubleClick
        EditItem()
    End Sub

    Private Sub BtnEdit_Click(sender As Object, e As EventArgs) Handles BtnEdit.Click
        EditItem()
    End Sub

    Private Sub BtnAdd_Click(sender As Object, e As EventArgs) Handles BtnAdd.Click
        Using AddAlert As New AddAlert With {.StartPosition = FormStartPosition.CenterParent, .Icon = Icon, .Text = "Add Alert"}
            AddAlert.ShowDialog(Me)

            If AddAlert.boolSuccess Then
                If CheckForExistingItem(AddAlert.strLogText) Then
                    MsgBox("A similar item has already been found in your alerts list.", MsgBoxStyle.Critical, Text)
                    Exit Sub
                End If

                Dim AlertsListViewItem As New AlertsListViewItem(AddAlert.strLogText) With {.StrLogText = AddAlert.strLogText, .StrAlertText = AddAlert.strAlertText}

                With AlertsListViewItem
                    .SubItems.Add(If(String.IsNullOrWhiteSpace(AddAlert.strAlertText), "(Shows Log Text)", AddAlert.strAlertText))
                    .SubItems.Add(If(AddAlert.boolRegex, "Yes", "No"))
                    .SubItems.Add(If(AddAlert.boolCaseSensitive, "Yes", "No"))

                    Select Case .AlertType
                        Case AlertType.Warning
                            .SubItems.Add("Warning")
                        Case AlertType.ErrorMsg
                            .SubItems.Add("Error")
                        Case AlertType.Info
                            .SubItems.Add("Information")
                        Case AlertType.None
                            .SubItems.Add("None")
                    End Select

                    .SubItems.Add(If(AddAlert.boolEnabled, "Yes", "No"))
                    .BoolRegex = AddAlert.boolRegex
                    .BoolCaseSensitive = AddAlert.boolCaseSensitive
                    .AlertType = AddAlert.AlertType
                    .BoolEnabled = AddAlert.boolEnabled
                End With

                AlertsListView.Items.Add(AlertsListViewItem)
            End If
        End Using
    End Sub

    Private Sub Alerts_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        alertsList.Clear()

        Dim AlertsClass As AlertsClass
        Dim tempAlerts As New Specialized.StringCollection()

        For Each item As AlertsListViewItem In AlertsListView.Items
            AlertsClass = New AlertsClass() With {.StrLogText = item.StrLogText, .StrAlertText = item.StrAlertText, .BoolCaseSensitive = item.BoolCaseSensitive, .BoolRegex = item.BoolRegex, .alertType = item.AlertType, .BoolEnabled = item.BoolEnabled}
            If AlertsClass.BoolEnabled Then alertsList.Add(AlertsClass)
            tempAlerts.Add(Newtonsoft.Json.JsonConvert.SerializeObject(AlertsClass))
        Next

        My.Settings.alerts = tempAlerts
        My.Settings.Save()
    End Sub

    Private Sub ListViewMenu_Opening(sender As Object, e As ComponentModel.CancelEventArgs) Handles ListViewMenu.Opening
        If AlertsListView.SelectedItems.Count = 0 And AlertsListView.SelectedItems.Count > 1 Then
            e.Cancel = True
            Exit Sub
        Else
            Dim selectedItem As AlertsListViewItem = AlertsListView.SelectedItems(0)
            EnableDisableToolStripMenuItem.Text = If(selectedItem.BoolEnabled, "Disable", "Enable")
        End If
    End Sub

    Private Sub DisableEnableItem()
        Dim selectedItem As AlertsListViewItem = AlertsListView.SelectedItems(0)

        If selectedItem.BoolEnabled Then
            selectedItem.BoolEnabled = False
            selectedItem.SubItems(5).Text = "False"
            BtnEnableDisable.Text = "Enable"
        Else
            selectedItem.BoolEnabled = True
            selectedItem.SubItems(5).Text = "True"
            BtnEnableDisable.Text = "Disable"
        End If
    End Sub

    Private Sub BtnEnableDisable_Click(sender As Object, e As EventArgs) Handles BtnEnableDisable.Click
        DisableEnableItem()
    End Sub

    Private Sub EnableDisableToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EnableDisableToolStripMenuItem.Click
        DisableEnableItem()
    End Sub

    Private Sub AlertsListView_ColumnWidthChanged(sender As Object, e As ColumnWidthChangedEventArgs) Handles AlertsListView.ColumnWidthChanged
        If boolDoneLoading Then
            My.Settings.colAlertsAlertLogText = AlertLogText.Width
            My.Settings.colAlertsAlertText = AlertText.Width
            My.Settings.colAlertsRegex = Regex.Width
            My.Settings.colAlertsCaseSensitive = CaseSensitive.Width
            My.Settings.colAlertsType = AlertTypeColumn.Width
            My.Settings.colAlertsEnabled = ColEnabled.Width
        End If
    End Sub

    Private Sub Alerts_ResizeEnd(sender As Object, e As EventArgs) Handles Me.ResizeEnd
        If boolDoneLoading Then My.Settings.ConfigureAlertsSize = Size
    End Sub

    Private Sub BtnExport_Click(sender As Object, e As EventArgs) Handles BtnExport.Click
        If AlertsListView.Items.Count() = 0 Then
            MsgBox("There's nothing to export.", MsgBoxStyle.Critical, Text)
            Exit Sub
        End If

        Dim saveFileDialog As New SaveFileDialog() With {.Title = "Export Alerts", .Filter = "JSON File|*.json", .OverwritePrompt = True}
        Dim listOfAlertsClass As New List(Of AlertsClass)

        If saveFileDialog.ShowDialog() = DialogResult.OK Then
            For Each item As AlertsListViewItem In AlertsListView.Items
                listOfAlertsClass.Add(New AlertsClass() With {.StrLogText = item.StrLogText, .StrAlertText = item.StrAlertText, .BoolCaseSensitive = item.BoolCaseSensitive, .BoolRegex = item.BoolRegex, .alertType = item.AlertType, .BoolEnabled = item.BoolEnabled})
            Next

            IO.File.WriteAllText(saveFileDialog.FileName, Newtonsoft.Json.JsonConvert.SerializeObject(listOfAlertsClass))

            MsgBox("Data exported successfully.", MsgBoxStyle.Information, Text)
        End If
    End Sub

    Private Sub BtnImport_Click(sender As Object, e As EventArgs) Handles BtnImport.Click
        Dim openFileDialog As New OpenFileDialog() With {.Title = "Import Alerts", .Filter = "JSON File|*.json"}
        Dim listOfAlertsClass As New List(Of AlertsClass)

        If openFileDialog.ShowDialog() = DialogResult.OK Then
            Try
                listOfAlertsClass = Newtonsoft.Json.JsonConvert.DeserializeObject(Of List(Of AlertsClass))(IO.File.ReadAllText(openFileDialog.FileName), JSONDecoderSettings)

                AlertsListView.Items.Clear()
                alertsList.Clear()

                Dim tempAlerts As New Specialized.StringCollection()

                For Each item As AlertsClass In listOfAlertsClass
                    alertsList.Add(item)
                    tempAlerts.Add(Newtonsoft.Json.JsonConvert.SerializeObject(item))
                    AlertsListView.Items.Add(item.ToListViewItem())
                Next

                My.Settings.alerts = tempAlerts
                My.Settings.Save()

                MsgBox("Data imported successfully.", MsgBoxStyle.Information, Text)
            Catch ex As Newtonsoft.Json.JsonSerializationException
                MsgBox("There was an error decoding the JSON data.", MsgBoxStyle.Critical, Text)
            End Try
        End If
    End Sub

    Private Sub Alerts_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.Escape Then Close()
    End Sub
End Class