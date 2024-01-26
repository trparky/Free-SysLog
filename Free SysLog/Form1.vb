Imports System.IO
Imports System.Net.Sockets
Imports System.Net
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.ComponentModel
Imports Microsoft.Win32

Public Class Form1
    Private sysLogThreadInstance As Threading.Thread
    Private boolDoneLoading As Boolean = False
    Private lockObject As New Object

    Private Sub ChkStartAtUserStartup_Click(sender As Object, e As EventArgs) Handles chkStartAtUserStartup.Click
        Using registryKey As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Run", True)
            If chkStartAtUserStartup.Checked Then
                registryKey.SetValue("Free Syslog", $"""{Application.ExecutablePath}"" /background")
            Else
                registryKey.DeleteValue("Free Syslog", False)
            End If
        End Using
    End Sub

    Private Function DoesStartupEntryExist() As Boolean
        Using registryKey As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Run", False)
            Return registryKey.GetValue("Free Syslog") IsNot Nothing
        End Using
    End Function

    Private Sub WriteLogsToDisk()
        SyncLock lockObject
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

            lblLogFileSize.Text = $"Log File Size: {FileSizeToHumanSize(New FileInfo(My.Settings.logFileLocation).Length)}"

            btnSaveLogsToDisk.Enabled = False
        End SyncLock
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        My.Settings.mainWindowSize = Size
        WriteLogsToDisk()

        Try
            sysLogThreadInstance.Abort()
        Catch ex As Exception
            ' Does nothing.
        End Try

        Process.GetCurrentProcess.Kill()
    End Sub

    Private Sub ChkAutoSave_Click(sender As Object, e As EventArgs) Handles chkAutoSave.Click
        SaveTimer.Enabled = chkAutoSave.Checked
        NumericUpDown.Visible = chkAutoSave.Checked
        lblAutoSaveLabel.Visible = chkAutoSave.Checked
        lblAutoSaved.Visible = chkAutoSave.Checked
    End Sub

    Private Sub SaveTimer_Tick(sender As Object, e As EventArgs) Handles SaveTimer.Tick
        WriteLogsToDisk()
        lblAutoSaved.Text = $"Last Auto-Saved At: {Date.Now:h:mm:ss tt}"
    End Sub

    Private Sub NumericUpDown_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown.ValueChanged
        If boolDoneLoading Then
            SaveTimer.Interval = TimeSpan.FromMinutes(NumericUpDown.Value).TotalMilliseconds
            My.Settings.autoSaveMinutes = NumericUpDown.Value
        End If
    End Sub

    Private Function FileSizeToHumanSize(size As Long, Optional roundToNearestWholeNumber As Boolean = False) As String
        Dim result As String
        Dim shortRoundNumber As Short = If(roundToNearestWholeNumber, 0, 2)

        If size <= (2 ^ 10) Then
            result = $"{size} Bytes"
        ElseIf size > (2 ^ 10) And size <= (2 ^ 20) Then
            result = $"{MyRoundingFunction(size / (2 ^ 10), shortRoundNumber)} KBs"
        ElseIf size > (2 ^ 20) And size <= (2 ^ 30) Then
            result = $"{MyRoundingFunction(size / (2 ^ 20), shortRoundNumber)} MBs"
        ElseIf size > (2 ^ 30) And size <= (2 ^ 40) Then
            result = $"{MyRoundingFunction(size / (2 ^ 30), shortRoundNumber)} GBs"
        ElseIf size > (2 ^ 40) And size <= (2 ^ 50) Then
            result = $"{MyRoundingFunction(size / (2 ^ 40), shortRoundNumber)} TBs"
        ElseIf size > (2 ^ 50) And size <= (2 ^ 60) Then
            result = $"{MyRoundingFunction(size / (2 ^ 50), shortRoundNumber)} PBs"
        ElseIf size > (2 ^ 60) And size <= (2 ^ 70) Then
            result = $"{MyRoundingFunction(size / (2 ^ 50), shortRoundNumber)} EBs"
        Else
            result = "(None)"
        End If

        Return result
    End Function

    Private Function MyRoundingFunction(value As Double, digits As Integer) As String
        If digits = 0 Then
            Return Math.Round(value, digits).ToString
        Else
            Dim strFormatString As String = "{0:0." & New String("0", digits) & "}"
            Return String.Format(strFormatString, Math.Round(value, digits))
        End If
    End Function

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If My.Application.CommandLineArgs.Count > 0 AndAlso My.Application.CommandLineArgs(0).Trim.Equals("/background", StringComparison.OrdinalIgnoreCase) Then WindowState = FormWindowState.Minimized

        chkAutoScroll.Checked = My.Settings.autoScroll
        chkAutoSave.Checked = My.Settings.autoSave
        NumericUpDown.Value = My.Settings.autoSaveMinutes
        NumericUpDown.Visible = chkAutoSave.Checked
        lblAutoSaveLabel.Visible = chkAutoSave.Checked
        lblAutoSaved.Visible = chkAutoSave.Checked
        chkStartAtUserStartup.Checked = DoesStartupEntryExist()
        Icon = Icon.ExtractAssociatedIcon(AppDomain.CurrentDomain.FriendlyName)

        If My.Settings.autoSave Then
            SaveTimer.Interval = TimeSpan.FromMinutes(My.Settings.autoSaveMinutes).TotalMilliseconds
            SaveTimer.Enabled = True
        End If

        Size = My.Settings.mainWindowSize

        Time.Width = My.Settings.columnTimeSize
        Type.Width = My.Settings.columnTypeSize
        IPAddressCol.Width = My.Settings.columnIPSize
        Log.Width = My.Settings.columnLogSize

        boolDoneLoading = True
        SaveFileDialog.Filter = "JSON Data File|*.json"

		If String.IsNullOrWhiteSpace(My.Settings.logFileLocation) Then
		    Do
		        SaveFileDialog.ShowDialog()

		        If String.IsNullOrWhiteSpace(SaveFileDialog.FileName) Then
		            MsgBox("You must set a location to save the syslog data to.", MsgBoxStyle.Information, Text)
		        Else
		            My.Settings.logFileLocation = SaveFileDialog.FileName
		            My.Settings.Save()
		            Exit Do
		        End If
		    Loop While True
		End If

        pathToLogFiles = My.Settings.logFileLocation

        If File.Exists(My.Settings.logFileLocation) Then
            Try
                lblLogFileSize.Text = $"Log File Size: {FileSizeToHumanSize(New FileInfo(My.Settings.logFileLocation).Length)}"

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
            Catch ex As Exception
            End Try
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
        Try
            Dim ipeRemoteIpEndPoint As New IPEndPoint(IPAddress.Any, 0)
            Dim udpcUDPClient As New UdpClient(514)
            Dim sDataRecieve As String
            Dim bBytesRecieved() As Byte
            Dim sFromIP As String

            While True
                bBytesRecieved = udpcUDPClient.Receive(ipeRemoteIpEndPoint)
                sDataRecieve = Encoding.ASCII.GetString(bBytesRecieved)
                sFromIP = ipeRemoteIpEndPoint.Address.ToString

                FillLog(sDataRecieve, sFromIP)

                sDataRecieve = ""
            End While
        Catch e As Exception
            MsgBox("Unable to start syslog server, perhaps another instance of this program is running on your system.", MsgBoxStyle.Critical, Text)
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
            Return ""
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

            sSyslog = Regex.Replace(sSyslog, "[./0-9-]{5,10}T[.0-9:]{5,8}\.[0-9]+-[0-9]+:[0-9]+ L[0-9]+ ", "").Trim
            addToLogList(sPriority, sFromIp, sSyslog)
        Catch ex As Exception
            addToLogList("Error (3)", "local", $"{ex.Message} -- {ex.StackTrace}")
        End Try
    End Sub

    Private Sub AddToLogList(sPriority As String, sFromIp As String, sSyslog As String)
        Dim listViewItem As New ListViewItem(Now.ToLocalTime.ToString)
        listViewItem.SubItems.Add(sPriority)
        listViewItem.SubItems.Add(sFromIp)
        listViewItem.SubItems.Add(sSyslog)

        Invoke(Sub()
                   logs.Items.Add(listViewItem)
                   UpdateLogCount()
                   If chkAutoScroll.Checked Then logs.EnsureVisible(logs.Items.Count - 1)
                   btnSaveLogsToDisk.Enabled = True
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
        NumberOfLogs.Text = $"Number of Log Entries: {logs.Items.Count:N0}"
    End Sub

    Private Sub ChkAutoScroll_Click(sender As Object, e As EventArgs) Handles chkAutoScroll.Click
        My.Settings.autoScroll = chkAutoScroll.Checked
    End Sub

    Private Sub Logs_ColumnWidthChanged(sender As Object, e As ColumnWidthChangedEventArgs) Handles logs.ColumnWidthChanged
        If boolDoneLoading Then
            My.Settings.columnTimeSize = Time.Width
            My.Settings.columnTypeSize = Type.Width
            My.Settings.columnIPSize = IPAddressCol.Width
            My.Settings.columnLogSize = Log.Width
        End If
    End Sub

    Private Sub BtnClearLog_Click(sender As Object, e As EventArgs) Handles btnClearLog.Click
        logs.Items.Clear()
        UpdateLogCount()
        WriteLogsToDisk()
    End Sub

    Private Sub BtnSaveLogsToDisk_Click(sender As Object, e As EventArgs) Handles btnSaveLogsToDisk.Click
        WriteLogsToDisk()
        lblAutoSaved.Text = $"Last Saved At: {Date.Now:h:mm:ss tt}"
        SaveTimer.Enabled = False
        SaveTimer.Enabled = True
    End Sub

    Private Sub BtnCheckForUpdates_Click(sender As Object, e As EventArgs) Handles btnCheckForUpdates.Click
        Threading.ThreadPool.QueueUserWorkItem(Sub()
                                                   Dim checkForUpdatesClassObject As New checkForUpdates.CheckForUpdatesClass(Me)
                                                   checkForUpdatesClassObject.CheckForUpdates()
                                               End Sub)
    End Sub

    Private Sub Form1_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        My.Settings.Save()
        WriteLogsToDisk()
        Process.GetCurrentProcess.Kill()
    End Sub
#End Region
End Class

Public Class SavedData
    Public time, type, ip, log As String
End Class