Imports System.Reflection

Public Class Alerts_History
    Public Property DataToLoad As List(Of AlertsHistory)
    Private Shadows ParentForm As Form1
    Private boolDoneLoading As Boolean = False

    Public WriteOnly Property SetParentForm As Form1
        Set(value As Form1)
            ParentForm = value
        End Set
    End Property

    Private Sub OpenLogViewerWindow(strLogText As String, strAlertText As String, strLogDate As String, strSourceIP As String, strRawLogText As String)
        Using LogViewerInstance As New LogViewer With {.strRawLogText = strRawLogText, .strLogText = strLogText, .StartPosition = FormStartPosition.CenterParent, .Icon = Icon}
            LogViewerInstance.LblLogDate.Text = $"Log Date: {strLogDate}"
            LogViewerInstance.LblSource.Text = $"Source IP Address: {strSourceIP}"
            LogViewerInstance.TopMost = True
            LogViewerInstance.lblAlertText.Text = $"Alert Text: {strAlertText}"

            LogViewerInstance.ShowDialog()
        End Using
    End Sub

    Private Sub Alerts_History_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim flags As BindingFlags = BindingFlags.NonPublic Or BindingFlags.Instance Or BindingFlags.SetProperty
        Dim propInfo As PropertyInfo = GetType(DataGridView).GetProperty("DoubleBuffered", flags)
        propInfo?.SetValue(AlertHistoryList, True, Nothing)

        AlertHistoryList.AlternatingRowsDefaultCellStyle = New DataGridViewCellStyle() With {.BackColor = My.Settings.searchColor, .ForeColor = SupportCode.GetGoodTextColorBasedUponBackgroundColor(My.Settings.searchColor)}
        Size = My.Settings.AlertHistorySize
        Location = SupportCode.VerifyWindowLocation(My.Settings.AlertHistoryLocation, Me)

        colTime.Width = My.Settings.columnTimeSize

        SupportCode.LoadColumnOrders(AlertHistoryList.Columns, My.Settings.alertsHistoryColumnOrder)

        If DataToLoad IsNot Nothing AndAlso DataToLoad.Count > 0 Then
            lblNumberOfAlerts.Text = $"Number of Alerts: {DataToLoad.Count:N0}"
            Dim listOfDataRows As New List(Of AlertsHistoryDataGridViewRow)

            For Each item As AlertsHistory In DataToLoad
                listOfDataRows.Add(item.MakeDataGridRow(AlertHistoryList, SupportCode.GetMinimumHeight(item.strAlertText, AlertHistoryList.DefaultCellStyle.Font, colAlert.Width)))
            Next

            AlertHistoryList.Rows.AddRange(listOfDataRows.ToArray)
        End If

        boolDoneLoading = True
    End Sub

    Private Sub Alerts_History_ResizeEnd(sender As Object, e As EventArgs) Handles Me.ResizeEnd
        If boolDoneLoading Then My.Settings.AlertHistorySize = Size
    End Sub

    Protected Overrides Sub WndProc(ByRef m As Message)
        Const WM_EXITSIZEMOVE As Integer = &H232

        MyBase.WndProc(m)

        If m.Msg = WM_EXITSIZEMOVE AndAlso boolDoneLoading Then
            Location = SupportCode.VerifyWindowLocation(Location, Me)
            My.Settings.AlertHistoryLocation = Location
        End If
    End Sub

    Private Sub AlertHistoryList_DoubleClick(sender As Object, e As EventArgs) Handles AlertHistoryList.DoubleClick
        If AlertHistoryList.SelectedRows.Count > 0 Then
            Dim AlertsHistoryDataGridViewRow As AlertsHistoryDataGridViewRow = DirectCast(AlertHistoryList.SelectedRows(0), AlertsHistoryDataGridViewRow)

            With AlertsHistoryDataGridViewRow
                OpenLogViewerWindow(.strLog, .strAlertText, .strTime, .strIP, .strRawLog)
            End With
        End If
    End Sub

    Private Sub Alerts_History_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.F5 Then BtnRefresh.PerformClick()
    End Sub

    Private Sub BtnRefresh_Click(sender As Object, e As EventArgs) Handles BtnRefresh.Click
        If ParentForm IsNot Nothing Then
            With ParentForm
                Dim data As New List(Of AlertsHistory)

                SyncLock ParentForm.dataGridLockObject
                    For Each item As MyDataGridViewRow In ParentForm.Logs.Rows
                        If item.BoolAlerted Then
                            data.Add(New AlertsHistory With {
                                     .strTime = item.Cells(SupportCode.ColumnIndex_ComputedTime).Value,
                                     .alertType = item.alertType,
                                     .strAlertText = item.AlertText,
                                     .strIP = item.Cells(SupportCode.ColumnIndex_IPAddress).Value,
                                     .strLog = item.Cells(SupportCode.ColumnIndex_LogText).Value,
                                     .strRawLog = item.RawLogData
                                    })
                        End If
                    Next
                End SyncLock

                If data.Count > 0 Then
                    lblNumberOfAlerts.Text = $"Number of Alerts: {data.Count:N0}"
                    Dim listOfDataRows As New List(Of AlertsHistoryDataGridViewRow)

                    For Each item As AlertsHistory In data
                        listOfDataRows.Add(item.MakeDataGridRow(AlertHistoryList, SupportCode.GetMinimumHeight(item.strAlertText, AlertHistoryList.DefaultCellStyle.Font, colAlert.Width)))
                    Next

                    AlertHistoryList.Rows.Clear()
                    AlertHistoryList.Rows.AddRange(listOfDataRows.ToArray)
                End If
            End With
        End If
    End Sub

    Private Sub Alerts_History_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        My.Settings.alertsHistoryColumnOrder = SupportCode.SaveColumnOrders(AlertHistoryList.Columns)
    End Sub
End Class