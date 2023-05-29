Imports System.IO
Imports System.Net.Sockets
Imports System.Net
Imports System.Text

Public Class Form1
    Private sysLogThreadInstance As Threading.Thread
    Private boolDoneLoading As Boolean = False

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        My.Settings.mainWindowSize = Size
        Dim collectionOfSavedData As New List(Of SavedData)

        For Each listViewItem As ListViewItem In logs.Items
            collectionOfSavedData.Add(New SavedData With {
                                        .time = listViewItem.SubItems(0).Text,
                                        .type = listViewItem.SubItems(1).Text,
                                        .ip = listViewItem.SubItems(2).Text,
                                        .log = listViewItem.SubItems(3).Text
                                      })
        Next

        Using fileStream As New StreamWriter(My.Settings.logFileLocation)
            fileStream.Write(Newtonsoft.Json.JsonConvert.SerializeObject(collectionOfSavedData))
        End Using

        Try
            sysLogThreadInstance.Abort()
        Catch ex As Exception
            ' Does nothing.
        End Try
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        chkAutoScroll.Checked = My.Settings.autoScroll
        Size = My.Settings.mainWindowSize

        Time.Width = My.Settings.columnTimeSize
        Type.Width = My.Settings.columnTypeSize
        IPAddressCol.Width = My.Settings.columnIPSize
        Log.Width = My.Settings.columnLogSize

        boolDoneLoading = True

        If String.IsNullOrWhiteSpace(My.Settings.logFileLocation) Then
            MsgBox("You must set a location to save the syslog data to.", MsgBoxStyle.Information, Text)
askAgain:
            SaveFileDialog.ShowDialog()

            If String.IsNullOrWhiteSpace(SaveFileDialog.FileName) Then
                MsgBox("You must set a location to save the syslog data to.", MsgBoxStyle.Information, Text)
                GoTo askAgain
            End If

            My.Settings.logFileLocation = SaveFileDialog.FileName
            My.Settings.Save()
        End If

        pathToLogFiles = My.Settings.logFileLocation

        If File.Exists(My.Settings.logFileLocation) Then
            Dim collectionOfSavedData As New List(Of SavedData)

            Using fileStream As New StreamReader(My.Settings.logFileLocation)
                collectionOfSavedData = Newtonsoft.Json.JsonConvert.DeserializeObject(Of List(Of SavedData))(fileStream.ReadToEnd.Trim)
            End Using

            Dim listOfLogEntries As New List(Of ListViewItem)
            Dim listViewItem As ListViewItem

            For Each item As SavedData In collectionOfSavedData
                listViewItem = New ListViewItem(item.time)
                listViewItem.SubItems.Add(item.type)
                listViewItem.SubItems.Add(item.ip)
                listViewItem.SubItems.Add(item.log)
                listOfLogEntries.Add(listViewItem)
            Next

            Invoke(Sub()
                       logs.Items.AddRange(listOfLogEntries.ToArray())
                       UpdateLogCount()
                   End Sub)
        End If

        sysLogThreadInstance = New Threading.Thread(AddressOf SysLogThread) With {
            .Name = "SysLog Thread",
            .Priority = Threading.ThreadPriority.Lowest
        }
        sysLogThreadInstance.Start()
    End Sub

    Sub SysLogThread()
        Try
            ListenForSyslogs()
        Catch ex As Threading.ThreadAbortException
            ' Does nothing
        End Try
    End Sub

    Private Sub BtnOpenLogLocation_Click(sender As Object, e As EventArgs) Handles btnOpenLogLocation.Click
        Process.Start("Explorer.exe", Chr(34) & My.Settings.logFileLocation & Chr(34))
    End Sub

    Private Sub BtnServerController_Click(sender As Object, e As EventArgs) Handles btnServerController.Click
        If btnServerController.Text = "Stop SysLog Server" Then
            sysLogThreadInstance.Abort()
            btnServerController.Text = "Start SysLog Server"
            sysLogThreadInstance = Nothing
        Else
            btnServerController.Text = "Stop SysLog Server"

            sysLogThreadInstance = New Threading.Thread(AddressOf SysLogThread) With {
                .Name = "SysLog Thread",
                .Priority = Threading.ThreadPriority.Lowest,
                .IsBackground = True
            }
            sysLogThreadInstance.Start()
        End If
    End Sub

#Region "-- SysLog Server Code --"
    Public pathToLogFiles As String

    Public Sub ListenForSyslogs()
        Dim ipeRemoteIpEndPoint As New IPEndPoint(IPAddress.Any, 0)
        Dim udpcUDPClient As New UdpClient(514)
        Dim sDataRecieve As String
        Dim bBytesRecieved() As Byte
        Dim sFromIP As String

        Try
            While True
                bBytesRecieved = udpcUDPClient.Receive(ipeRemoteIpEndPoint)
                sDataRecieve = Encoding.ASCII.GetString(bBytesRecieved)
                sFromIP = ipeRemoteIpEndPoint.Address.ToString

                FillLog(sDataRecieve, sFromIP)

                sDataRecieve = ""
            End While
        Catch e As Exception
            ' just ignore for now
        End Try
    End Sub

    Private Function GetSyslogPriority(sSyslog As String) As String
        If sSyslog.Contains("L0") Then
            Return "Emergency (0)"
        ElseIf sSyslog.Contains("L1") Then
            Return "Alert (1)"
        ElseIf sSyslog.Contains("L2") Then
            Return "Critical (2)"
        ElseIf sSyslog.Contains("L3") Then
            Return "Error (3)"
        ElseIf sSyslog.Contains("L4") Then
            Return "Warning (4)"
        ElseIf sSyslog.Contains("L5") Then
            Return "Notice (5)"
        ElseIf sSyslog.Contains("L6") Then
            Return "Info (6)"
        ElseIf sSyslog.Contains("L7") Then
            Return "Debug (7)"
        Else
            Return "UNKNOWN"
        End If
    End Function

    Private Sub FillLog(sSyslog As String, sFromIp As String)
        Try
            Dim sPriority As String

            sSyslog = sSyslog.Replace(vbCr, vbCrLf) ' Converts from UNIX to DOS/Windows.
            sSyslog = sSyslog.Replace(vbCrLf, "")
            sSyslog = Mid(sSyslog, InStr(sSyslog, ">") + 1, Len(sSyslog))
            sSyslog = sSyslog.Trim

            sPriority = GetSyslogPriority(sSyslog)

            If sSyslog.Contains("dsldevice") And sSyslog.Contains("dhcpd") Then
                Exit Sub
            End If

            Dim logFileName As String = String.Format("syslog {0}.txt", Now.ToShortDateString.ToString.Replace("/", "-"))

            addToLogList(sPriority, sFromIp, sSyslog)
        Catch ex As Exception
            addToLogList("Error (3)", "local", $"{ex.Message} -- {ex.StackTrace}")
        End Try
    End Sub

    Private Sub addToLogList(sPriority As String, sFromIp As String, sSyslog As String)
        Dim listViewItem As New ListViewItem(Now.ToLocalTime.ToString)
        listViewItem.SubItems.Add(sPriority)
        listViewItem.SubItems.Add(sFromIp)
        listViewItem.SubItems.Add(sSyslog)

        Invoke(Sub()
                   logs.Items.Add(listViewItem)
                   UpdateLogCount()
                   If chkAutoScroll.Checked Then logs.EnsureVisible(logs.Items.Count - 1)
               End Sub)

        listViewItem = Nothing
    End Sub

    Private Sub OpenLogViewerWindow()
        Dim LogViewer As New Log_Viewer With {.strLogText = logs.SelectedItems(0).SubItems(3).Text, .StartPosition = FormStartPosition.CenterParent}
        LogViewer.ShowDialog()
    End Sub

    Private Sub Logs_DoubleClick(sender As Object, e As EventArgs) Handles logs.DoubleClick
        OpenLogViewerWindow()
    End Sub

    Private Sub Logs_KeyUp(sender As Object, e As KeyEventArgs) Handles logs.KeyUp
        If e.KeyValue = Keys.Enter Then OpenLogViewerWindow()
    End Sub

    Private Sub UpdateLogCount()
        NumberOfLogs.Text = "Number of Log Entries: " & logs.Items.Count.ToString("N0")
    End Sub

    Private Sub chkAutoScroll_Click(sender As Object, e As EventArgs) Handles chkAutoScroll.Click
        My.Settings.autoScroll = chkAutoScroll.Checked
    End Sub

    Private Sub logs_ColumnWidthChanged(sender As Object, e As ColumnWidthChangedEventArgs) Handles logs.ColumnWidthChanged
        If boolDoneLoading Then
            My.Settings.columnTimeSize = Time.Width
            My.Settings.columnTypeSize = Type.Width
            My.Settings.columnIPSize = IPAddressCol.Width
            My.Settings.columnLogSize = Log.Width
        End If
    End Sub

    Private Sub btnClearLog_Click(sender As Object, e As EventArgs) Handles btnClearLog.Click
        logs.Items.Clear()
    End Sub
#End Region
End Class

Public Class SavedData
    Public time, type, ip, log As String
End Class