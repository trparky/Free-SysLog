Imports System.Net
Imports System.Net.Sockets
Imports System.Text

Namespace SupportCode
    Public Enum IgnoreOrSearchWindowDisplayMode As Byte
        ignored
        search
        viewer
    End Enum

    Module SupportCode
        Public IgnoredLogsAndSearchResultsInstance As IgnoredLogsAndSearchResults = Nothing
        Public replacementsList As New List(Of ReplacementsClass)
        Public ignoredList As New List(Of IgnoredClass)
        Public alertsList As New List(Of AlertsClass)
        Public serversList As New List(Of SysLogProxyServer)
        Public Const strMutexName As String = "Free SysLog Server"
        Public mutex As Threading.Mutex
        Public strEXEPath As String = Process.GetCurrentProcess.MainModule.FileName
        Public boolDoWeOwnTheMutex As Boolean = False
        Public JSONDecoderSettingsForLogFiles As New Newtonsoft.Json.JsonSerializerSettings With {.MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore}
        Public JSONDecoderSettingsForSettingsFiles As New Newtonsoft.Json.JsonSerializerSettings With {.MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Error}
        Public strPathToDataFolder As String = IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Free SysLog")
        Public strPathToDataBackupFolder As String = IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Free SysLog", "Backup")
        Public strPathToDataFile As String = IO.Path.Combine(strPathToDataFolder, "log.json")
        Public Const strNoProxyString As String = "noproxy|"
        Public Const strProxiedString As String = "proxied|"
        Public Const strQuote As String = Chr(34)

        Public Const ColumnIndex_ComputedTime As Integer = 0
        Public Const ColumnIndex_ServerTime As Integer = 1
        Public Const ColumnIndex_LogType As Integer = 2
        Public Const ColumnIndex_IPAddress As Integer = 3
        Public Const ColumnIndex_Hostname As Integer = 4
        Public Const ColumnIndex_RemoteProcess As Integer = 5
        Public Const ColumnIndex_LogText As Integer = 6
        Public Const ColumnIndex_Alerted As Integer = 7
        Public Const ColumnIndex_FileName As Integer = 8

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

        Public Function GetMinimumHeight(strInput As String, font As Font, maxWidth As Integer) As Integer
            Dim textSize As Size = TextRenderer.MeasureText(strInput, font, New Size(maxWidth, Integer.MaxValue), TextFormatFlags.WordBreak)
            Return textSize.Height + 10
        End Function

        Public Function IsRegexPatternValid(pattern As String) As Boolean
            Try
                Dim regex As New RegularExpressions.Regex(pattern)
                Return True
            Catch ex As ArgumentException
                Return False
            End Try
        End Function

        Public Sub SendMessageToSysLogServer(strMessage As String, intPort As Integer)
            Try
                Using udpClient As New UdpClient()
                    udpClient.Connect(Net.IPAddress.Loopback, intPort)
                    Dim data As Byte() = Encoding.UTF8.GetBytes(strMessage)
                    udpClient.Send(data, data.Length)
                End Using
            Catch ex As SocketException
            End Try
        End Sub

        Public Sub SendMessageToTCPSysLogServer(strMessage As String, intPort As Integer)
            Try
                Using tcpClient As New TcpClient()
                    tcpClient.Connect(Net.IPAddress.Loopback, intPort)

                    Using networkStream As NetworkStream = tcpClient.GetStream()
                        Dim data As Byte() = Encoding.UTF8.GetBytes(strMessage)
                        networkStream.Write(data, 0, data.Length)
                    End Using
                End Using
            Catch ex As SocketException
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
            If input.Contains(strQuote) Then input = input.Replace(strQuote, strQuote & strQuote)
            If input.Contains(",") Then input = $"{strQuote}{input}{strQuote}"
            input = input.Replace(vbCrLf, "\n")
            Return input
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