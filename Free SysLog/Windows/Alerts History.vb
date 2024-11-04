Imports System.Reflection

Public Class Alerts_History
    Public Property data As List(Of AlertsHistory)
    Private boolDoneLoading As Boolean = False

    Private Sub Alerts_History_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim flags As BindingFlags = BindingFlags.NonPublic Or BindingFlags.Instance Or BindingFlags.SetProperty
        Dim propInfo As PropertyInfo = GetType(DataGridView).GetProperty("DoubleBuffered", flags)
        propInfo?.SetValue(AlertHistoryList, True, Nothing)

        AlertHistoryList.AlternatingRowsDefaultCellStyle = New DataGridViewCellStyle() With {.BackColor = My.Settings.searchColor, .ForeColor = SupportCode.GetGoodTextColorBasedUponBackgroundColor(My.Settings.searchColor)}
        Size = My.Settings.AlertHistorySize
        Location = SupportCode.VerifyWindowLocation(My.Settings.AlertHistoryLocation, Me)

        colTime.Width = My.Settings.columnTimeSize

        If data IsNot Nothing AndAlso data.Count > 0 Then
            Dim listOfDataRows As New List(Of DataGridViewRow)

            For Each item As AlertsHistory In data
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
End Class