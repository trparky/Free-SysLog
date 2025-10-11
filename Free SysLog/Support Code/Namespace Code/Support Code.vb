Imports System.Collections.Concurrent
Imports System.Net
Imports System.Net.NetworkInformation
Imports System.Net.Sockets
Imports System.Text
Imports System.Text.RegularExpressions
Imports Microsoft.Toolkit.Uwp.Notifications

Namespace SupportCode
    Public Enum IgnoreOrSearchWindowDisplayMode As Byte
        ignored
        search
        viewer
    End Enum

    Public Class uniqueObjectsClass
        Public logTypes As HashSet(Of String)
        Public processes As HashSet(Of String)
        Public hostNames As HashSet(Of String)
        Public ipAddresses As HashSet(Of String)

        Public Sub New()
            logTypes = New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
            processes = New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
            hostNames = New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
            ipAddresses = New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
        End Sub

        Public Sub Clear()
            logTypes.Clear()
            processes.Clear()
            hostNames.Clear()
            ipAddresses.Clear()
        End Sub

        Public Sub Merge(other As uniqueObjectsClass)
            If other Is Nothing Then Exit Sub

            logTypes.UnionWith(other.logTypes)
            processes.UnionWith(other.processes)
            hostNames.UnionWith(other.hostNames)
            ipAddresses.UnionWith(other.ipAddresses)
        End Sub
    End Class

    Module SupportCode
        Public ParentForm As Form1

        Public AlertsRegexCache As New Dictionary(Of String, Regex)
        Public AlertsRegexCacheLockingObject As New Object
        Public ReplacementsRegexCache As New Dictionary(Of String, Regex)
        Public ReplacementsRegexCacheLockingObject As New Object
        Public IgnoredRegexCache As New Dictionary(Of String, Regex)
        Public IgnoredRegexCacheLockingObject As New Object()
        Public IgnoredHits As New ConcurrentDictionary(Of String, Integer)

        Public boolIsProgrammaticScroll As Boolean = False
        Public IgnoredLogsAndSearchResultsInstance As IgnoredLogsAndSearchResults = Nothing
        Public replacementsList As New List(Of ReplacementsClass)
        Public ignoredList As New List(Of IgnoredClass)
        Public ignoredListLockingObject As New Object()
        Public alertsList As New List(Of AlertsClass)
        Public serversList As New List(Of SysLogProxyServer)
        Public hostnames As New Dictionary(Of String, String)(StringComparison.OrdinalIgnoreCase)
        Public Const strMutexName As String = "Free SysLog Server"
        Public mutex As Threading.Mutex
        Public strEXEPath As String = Process.GetCurrentProcess.MainModule.FileName
        Public boolDoWeOwnTheMutex As Boolean = False
        Public JSONDecoderSettingsForLogFiles As New Newtonsoft.Json.JsonSerializerSettings With {.MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore}
        Public JSONDecoderSettingsForSettingsFiles As New Newtonsoft.Json.JsonSerializerSettings With {.MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Error}
        Public strPathToDataFolder As String = IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Free SysLog")
        Public strPathToDataBackupFolder As String = IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Free SysLog", "Backup")
        Public strPathToDataFile As String = IO.Path.Combine(strPathToDataFolder, "log.json")
        Public Const strProxiedString As String = "proxied|"
        Public Const strQuote As String = Chr(34)
        Public Const strViewLog As String = "viewlog"
        Public Const strOpenSysLog As String = "opensyslog"
        Public Const strRestore As String = "restore"
        Public Const strTerminate As String = "terminate"

        Public Const ColumnIndex_ComputedTime As Integer = 0
        Public Const ColumnIndex_ServerTime As Integer = 1
        Public Const ColumnIndex_LogType As Integer = 2
        Public Const ColumnIndex_IPAddress As Integer = 3
        Public Const ColumnIndex_Hostname As Integer = 4
        Public Const ColumnIndex_RemoteProcess As Integer = 5
        Public Const ColumnIndex_LogText As Integer = 6
        Public Const ColumnIndex_Alerted As Integer = 7
        Public Const ColumnIndex_FileName As Integer = 8

        Public Const strUpdaterEXE As String = "updater.exe"
        Public Const strUpdaterPDB As String = "updater.pdb"

        Public allUniqueObjects As uniqueObjectsClass
        Public recentUniqueObjects As uniqueObjectsClass
        Public ReadOnly recentUniqueObjectsLock As New Object()
        Public ReadOnly IgnoredLogsAndSearchResultsInstanceLockObject As New Object()

        Public WriteOnly Property AskOpenExplorer As Boolean
            Set(value As Boolean)
                My.Settings.AskOpenExplorer = value
                If ParentForm IsNot Nothing Then ParentForm.AskToOpenExplorerWhenSavingData.Checked = value
            End Set
        End Property

#If DEBUG Then
        Public Const boolDebugBuild As Boolean = True
#Else
        Public Const boolDebugBuild As Boolean = False
#End If

        Public Function SaveColumnOrders(columns As DataGridViewColumnCollection) As Specialized.StringCollection
            Try
                Dim SpecializedStringCollection As New Specialized.StringCollection

                For Each column As DataGridViewColumn In columns
                    If TypeOf column Is DataGridViewTextBoxColumn Then
                        SpecializedStringCollection.Add(Newtonsoft.Json.JsonConvert.SerializeObject(New ColumnOrder With {.ColumnName = column.Name, .ColumnIndex = column.DisplayIndex}))
                    End If
                Next

                Return SpecializedStringCollection
            Catch ex As Exception
                Return New Specialized.StringCollection
            End Try
        End Function

        Public Sub SortByClickedColumn(ByRef ListView As ListView, intColumn As Integer, ByRef m_SortingColumn As ColumnHeader)
            ' Get the new sorting column.
            Dim new_sorting_column As ColumnHeader = ListView.Columns(intColumn)

            ' Figure out the new sorting order.
            Dim sort_order As SortOrder
            If m_SortingColumn Is Nothing Then
                ' New column. Sort ascending.
                sort_order = SortOrder.Ascending
            Else
                ' See if this is the same column.
                If new_sorting_column.Equals(m_SortingColumn) Then
                    ' Same column. Switch the sort order.
                    If m_SortingColumn.Text.StartsWith("> ") Then
                        sort_order = SortOrder.Descending
                    Else
                        sort_order = SortOrder.Ascending
                    End If
                Else
                    ' New column. Sort ascending.
                    sort_order = SortOrder.Ascending
                End If

                ' Remove the old sort indicator.
                m_SortingColumn.Text = m_SortingColumn.Text.Substring(2)
            End If

            ' Display the new sort order.
            m_SortingColumn = new_sorting_column
            m_SortingColumn.Text = If(sort_order = SortOrder.Ascending, "> " & m_SortingColumn.Text, "< " & m_SortingColumn.Text)

            ' Create a comparer.
            ListView.ListViewItemSorter = New listViewSorter.ListViewComparer(intColumn, sort_order)

            ' Sort.
            ListView.Sort()
        End Sub

        Public Function GetProcessUsingPort(port As Integer, protocolType As ProtocolType) As Process
            Dim startInfo As New ProcessStartInfo() With {
                .FileName = "netstat",
                .UseShellExecute = False,
                .RedirectStandardOutput = True,
                .CreateNoWindow = True
            }

            Dim strOutput, strLocalAddress, strPID, strPort, strProtocolType As String
            Dim strArrayParts As String()
            Dim intPID As Integer

            If protocolType = ProtocolType.Tcp Then
                startInfo.Arguments = "-ano -p tcp"
                strProtocolType = "TCP"
            Else
                startInfo.Arguments = "-ano -p udp"
                strProtocolType = "UDP"
            End If

            Using proc As Process = Process.Start(startInfo)
                strOutput = proc.StandardOutput.ReadToEnd()
                proc.WaitForExit()
            End Using

            Dim strArrayLines As String() = strOutput.Split({Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)

            For Each strLine As String In strArrayLines
                strLine = strLine.Trim()

                If strLine.StartsWith(strProtocolType, StringComparison.OrdinalIgnoreCase) Then
                    ' Normalize spaces, then split
                    strArrayParts = Regex.Split(strLine, "\s+")

                    If strArrayParts.Length >= 4 Then
                        strLocalAddress = strArrayParts(1)
                        strPID = strArrayParts(3)

                        ' Match port
                        strPort = ":" & port.ToString()

                        If strLocalAddress.EndsWith(strPort) Then
                            If Integer.TryParse(strPID, intPID) Then
                                Try
                                    Return Process.GetProcessById(intPID)
                                Catch ex As Exception
                                    ' Process may have exited
                                    Return Nothing
                                End Try
                            End If
                        End If
                    End If
                End If
            Next

            Return Nothing
        End Function

        Public Sub ShowToastNotification(tipText As String, tipIcon As ToolTipIcon, strLogText As String, strLogDate As String, strSourceIP As String, strRawLogText As String, alertType As AlertType)
            Dim strIconPath As String = Nothing
            Dim notification As New ToastContentBuilder()

            notification.AddText(tipText)
            notification.SetToastDuration(If(My.Settings.NotificationLength = 0, ToastDuration.Short, ToastDuration.Long))

            If tipIcon = ToolTipIcon.Error Then
                strIconPath = IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "error.png")
            ElseIf tipIcon = ToolTipIcon.Warning Then
                strIconPath = IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "warning.png")
            ElseIf tipIcon = ToolTipIcon.Info Then
                strIconPath = IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "info.png")
            End If

            If My.Settings.IncludeButtonsOnNotifications Then
                Dim strNotificationPacket As String = Newtonsoft.Json.JsonConvert.SerializeObject(New NotificationDataPacket With {.alerttext = tipText, .logdate = strLogDate, .logtext = strLogText, .sourceip = strSourceIP, .rawlogtext = strRawLogText, .alertType = alertType})

                notification.AddButton(New ToastButton().SetContent("View Log").AddArgument("action", strViewLog).AddArgument("datapacket", strNotificationPacket))
                notification.AddButton(New ToastButton().SetContent("Open SysLog").AddArgument("action", strOpenSysLog))
                If My.Settings.ShowCloseButtonOnNotifications Then notification.AddButton(New ToastButton().SetContent("Close").SetDismissActivation())
            Else
                notification.AddArgument("action", strOpenSysLog)
            End If

            If Not String.IsNullOrWhiteSpace(strIconPath) AndAlso IO.File.Exists(strIconPath) Then notification.AddAppLogoOverride(New Uri(strIconPath), ToastGenericAppLogoCrop.Circle)

            notification.Show()
        End Sub

        Public Sub LoadColumnOrders(ByRef columns As DataGridViewColumnCollection, ByRef specializedStringCollection As Specialized.StringCollection)
            Try
                Dim columnOrder As ColumnOrder
                Dim jsonSerializerSettings As New Newtonsoft.Json.JsonSerializerSettings With {.MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Error}

                If specializedStringCollection IsNot Nothing AndAlso specializedStringCollection.Count <> 0 Then
                    Try
                        For Each item As String In specializedStringCollection
                            columnOrder = Newtonsoft.Json.JsonConvert.DeserializeObject(Of ColumnOrder)(item, jsonSerializerSettings)
                            columns(columnOrder.ColumnName).DisplayIndex = columnOrder.ColumnIndex
                        Next
                    Catch ex As Newtonsoft.Json.JsonSerializationException
                        specializedStringCollection = Nothing
                    End Try
                End If
            Catch ex As Exception
            End Try
        End Sub

        Public Function ConvertListOfStringsToString(input As List(Of String), Optional boolUseOnlyOneLine As Boolean = False) As String
            If input.Count = 1 Then Return input(0)

            If boolUseOnlyOneLine OrElse input.Count >= 6 Then
                If input.Count = 2 Then
                    Return $"{input(0)} and {input(1)}"
                Else
                    Return $"{String.Join(", ", input.Take(input.Count - 1))}, and {input.Last()}"
                End If
            Else
                ' For lists with more than 6 items, and if not using only one line
                Dim stringBuilder As New StringBuilder()

                For Each item As String In input
                    stringBuilder.AppendLine(item)
                Next

                Return stringBuilder.ToString().Trim()
            End If
        End Function

        Public Function CopyTextToWindowsClipboard(strTextToBeCopiedToClipboard As String, strErrorMessageTitle As String) As Boolean
            Try
                Clipboard.SetDataObject(strTextToBeCopiedToClipboard, True, 5, 200)
                Return True
            Catch ex As Exception
                MsgBox("Unable to open Windows Clipboard to copy text to it.", MsgBoxStyle.Critical, strErrorMessageTitle)
                Return False
            End Try
        End Function

        Public Function ToIso8601Format(dateTime As Date) As String
            ' Ensure the DateTime is in UTC
            Dim utcDateTime As Date = dateTime.ToUniversalTime()

            ' Convert to ISO 8601 format with UTC time zone designator (Z)
            Return utcDateTime.ToString("yyyy-MM-ddTHH:mm:ss:fffZ", Globalization.CultureInfo.InvariantCulture)
        End Function

        Public Function GetIPv4Address(ipv6Address As IPAddress) As IPAddress
            If ipv6Address.AddressFamily = AddressFamily.InterNetworkV6 AndAlso ipv6Address.IsIPv4MappedToIPv6 Then Return ipv6Address.MapToIPv4()
            Return ipv6Address
        End Function

        Public Function GetGoodTextColorBasedUponBackgroundColor(input As Color) As Color
            Dim intCombinedTotal As Short = Integer.Parse(input.R.ToString) + Integer.Parse(input.G.ToString) + Integer.Parse(input.B.ToString)
            Return If((intCombinedTotal / 3) < 128, Color.White, Color.Black)
        End Function

        Public Function FileSizeToHumanSize(size As Long, Optional roundToNearestWholeNumber As Boolean = False) As String
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

        Public Function MyRoundingFunction(value As Double, digits As Integer) As String
            If digits < 0 Then Throw New ArgumentException("The number of digits must be non-negative.", NameOf(digits))

            If digits = 0 Then
                Return Math.Round(value, digits).ToString
            Else
                Return Math.Round(value, digits).ToString("0." & New String("0", digits))
            End If
        End Function

        Public Sub CenterFormOverParent(parent As Form, child As Form)
            Dim parentCenterX As Integer = parent.Left + (parent.Width \ 2)
            Dim parentCenterY As Integer = parent.Top + (parent.Height \ 2)

            Dim childLeft As Integer = parentCenterX - (child.Width \ 2)
            Dim childTop As Integer = parentCenterY - (child.Height \ 2)

            child.Location = New Point(childLeft, childTop)
        End Sub

        Public Function IsRegexPatternValid(pattern As String) As Boolean
            Try
                Dim regex As New Regex(pattern)
                Return True
            Catch ex As ArgumentException
                Return False
            End Try
        End Function

        Private Function GetLocalIPAddress() As IPAddress
            Dim networkInterfaces As NetworkInterface() = NetworkInterface.GetAllNetworkInterfaces()

            If networkInterfaces IsNot Nothing AndAlso networkInterfaces.Any() Then
                For Each ni As NetworkInterface In networkInterfaces
                    If ni IsNot Nothing AndAlso ni.OperationalStatus = OperationalStatus.Up AndAlso ni.NetworkInterfaceType <> NetworkInterfaceType.Loopback AndAlso ni.NetworkInterfaceType <> NetworkInterfaceType.Tunnel Then
                        If ni.GetIPProperties().UnicastAddresses.Any() Then
                            For Each ip As UnicastIPAddressInformation In ni.GetIPProperties().UnicastAddresses
                                If ip.Address.AddressFamily = AddressFamily.InterNetwork Then Return ip.Address
                            Next
                        End If
                    End If
                Next
            End If

            Throw New Exception("No active network adapter with a matching IP address found.")
        End Function

        Public Sub SendMessageToSysLogServer(strMessage As String, intPort As Integer)
            Try
                Using udpClient As New UdpClient()
                    udpClient.Connect(GetLocalIPAddress(), intPort)
                    Dim data As Byte() = Encoding.UTF8.GetBytes(strMessage)
                    udpClient.Send(data, data.Length)
                End Using
            Catch ex As Exception
            End Try
        End Sub

        Public Sub SendMessageToTCPSysLogServer(strMessage As String, intPort As Integer)
            Try
                Using tcpClient As New TcpClient()
                    tcpClient.Connect(GetLocalIPAddress(), intPort)

                    Using networkStream As NetworkStream = tcpClient.GetStream()
                        Dim data As Byte() = Encoding.UTF8.GetBytes(strMessage)
                        networkStream.Write(data, 0, data.Length)
                    End Using
                End Using
            Catch ex As Exception
            End Try
        End Sub

        Public Sub SendMessageToSysLogServer(strMessage As String, strDestinationIP As String, intPort As Integer)
            Try
                Using udpClient As New UdpClient()
                    udpClient.Connect(strDestinationIP, intPort)
                    Dim data As Byte() = Encoding.UTF8.GetBytes(strMessage)
                    udpClient.Send(data, data.Length)
                End Using
            Catch ex As SocketException
            End Try
        End Sub

        Public Function SanitizeForCSV(input As String) As String
            If String.IsNullOrWhiteSpace(input) Then
                Return ""
            Else
                If input.Contains(strQuote) Then input = input.Replace(strQuote, strQuote & strQuote)
                If input.Contains(",") Then input = $"{strQuote}{input}{strQuote}"
                input = input.Replace(vbCrLf, "\n")
                Return input
            End If
        End Function

        Public Function VerifyWindowLocation(point As Point, ByRef window As Form) As Point
            Dim screen As Screen = Screen.FromPoint(point) ' Get the screen based on the new window location

            Dim windowBounds As New Rectangle(point.X, point.Y, window.Width, window.Height)
            Dim screenBounds As Rectangle = screen.WorkingArea

            ' Ensure the window is at least partially on the screen
            If windowBounds.IntersectsWith(screenBounds) Then
                Return point
            Else
                ' Adjust the window to a default location if it is completely off-screen
                Return New Point(screenBounds.Left, screenBounds.Top)
            End If
        End Function

        Public Sub SelectFileInWindowsExplorer(strFullPath As String)
            If Not String.IsNullOrEmpty(strFullPath) AndAlso IO.File.Exists(strFullPath) Then
                Dim pidlList As IntPtr = NativeMethod.NativeMethods.ILCreateFromPathW(strFullPath)

                If Not pidlList.Equals(IntPtr.Zero) Then
                    Try
                        NativeMethod.NativeMethods.SHOpenFolderAndSelectItems(pidlList, 0, IntPtr.Zero, 0)
                    Finally
                        NativeMethod.NativeMethods.ILFree(pidlList)
                    End Try
                End If
            End If
        End Sub
    End Module
End Namespace