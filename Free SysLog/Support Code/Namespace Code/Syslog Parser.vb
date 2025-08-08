Imports System.Net
Imports System.Text.RegularExpressions
Imports Free_SysLog.SupportCode
Imports System.ComponentModel
Imports System.Threading
Imports System.Threading.Tasks

Namespace SyslogParser
    Public Module SyslogParser
        Private ReadOnly rfc5424Regex As New Regex("<(?<priority>[0-9]+)>(?:\d ){0,1}(?<timestamp>[0-9]{4}[-.](?:1[0-2]|0[1-9])[-.](?:3[01]|[12][0-9]|0[1-9])T(?:2[0-3]|[01][0-9]):[0-5][0-9]:[0-5][0-9]\.[0-9]+Z)(?: -){0,1} (?<hostname>(?:\\.|[^\n\r ])+) (?:\d+ ){0,1}(?<appname>(?:\\.|[^\n\r:]+?)(?: \d*){0,1}):{0,1} (?:- - %% ){0,1}(?<message>.+?)(?=\s*<\d+>|$)", RegexOptions.Compiled) ' PERFECT!
        Private ReadOnly rfc5424TransformRegex As New Regex("<{0,1}(?<priority>[0-9]{0,})>{0,1}(?<timestamp>(?:Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec) {1,2}[0-9]{1,2} [0-2][0-9]:[0-5][0-9]:[0-5][0-9]) (?<hostname>(?:\\.|[^\n\r ])+)(?: \:){0,1} \[{0,1}(?<appname>(?:\\.|[^\n\r:]+))\]{0,1}:{0,1} {0,1}(?<message>.+?)(?=\s*<\d+>|$)", RegexOptions.Compiled) ' PERFECT!

        Private ReadOnly NumberRemovingRegex As New Regex("([A-Za-z-]*)\[[0-9]*\]", RegexOptions.Compiled)
        Private ReadOnly SyslogPreProcessor1 As New Regex("\d+ (<\d+>)", RegexOptions.Compiled)
        Private ReadOnly SyslogPreProcessor2 As New Regex("(<\d+>)", RegexOptions.Compiled)

        Private Const strNewLine As String = "{newline}"

        Public Function MakeLocalDataGridRowEntry(strLogText As String, ByRef dataGrid As DataGridView, Optional strLogType As String = "Informational, Local") As MyDataGridViewRow
            Dim MyDataGridViewRow As MyDataGridViewRow = MakeDataGridRow(serverTimeStamp:=Now,
                                   dateObject:=Now,
                                   strTime:=Now.ToString,
                                   strSourceAddress:=IPAddress.Loopback.ToString,
                                   strHostname:="Local",
                                   strRemoteProcess:=Nothing,
                                   strLog:=strLogText,
                                   strLogType:=strLogType,
                                   boolAlerted:=False,
                                   strRawLogText:=Nothing,
                                   strAlertText:=Nothing,
                                   AlertType:=AlertType.None,
                                   dataGrid:=dataGrid)

            MyDataGridViewRow.DefaultCellStyle.Padding = New Padding(0, 2, 0, 2)
            Return MyDataGridViewRow
        End Function

        Public Function MakeDataGridRow(serverTimeStamp As Date, dateObject As Date, strTime As String, strSourceAddress As String, strHostname As String, strRemoteProcess As String, strLog As String, strLogType As String, boolAlerted As Boolean, strRawLogText As String, strAlertText As String, AlertType As AlertType, ByRef dataGrid As DataGridView) As MyDataGridViewRow
            Using MyDataGridViewRow As New MyDataGridViewRow
                With MyDataGridViewRow
                    .CreateCells(dataGrid)
                    .Cells(ColumnIndex_ComputedTime).Value = Date.Parse(strTime)
                    .Cells(ColumnIndex_ComputedTime).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                    .Cells(ColumnIndex_LogType).Value = If(String.IsNullOrWhiteSpace(strLogType), "", strLogType)
                    .Cells(ColumnIndex_IPAddress).Value = strSourceAddress
                    .Cells(ColumnIndex_RemoteProcess).Value = If(String.IsNullOrWhiteSpace(strRemoteProcess), "", strRemoteProcess)
                    .Cells(ColumnIndex_Hostname).Value = If(String.IsNullOrWhiteSpace(strHostname), "", strHostname)
                    .Cells(ColumnIndex_ServerTime).Value = ToIso8601Format(serverTimeStamp)
                    .Cells(ColumnIndex_LogText).Value = strLog
                    .Cells(ColumnIndex_Alerted).Value = If(boolAlerted, "Yes", "No")
                    .Cells(ColumnIndex_Alerted).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                    .Cells(ColumnIndex_Alerted).Style.WrapMode = DataGridViewTriState.True
                    .DateObject = dateObject
                    .BoolAlerted = boolAlerted
                    .ServerDate = serverTimeStamp
                    .RawLogData = strRawLogText
                    .AlertText = strAlertText
                    .alertType = AlertType

                    If My.Settings.font IsNot Nothing Then
                        .Cells(ColumnIndex_ComputedTime).Style.Font = My.Settings.font
                        .Cells(ColumnIndex_LogType).Style.Font = My.Settings.font
                        .Cells(ColumnIndex_IPAddress).Style.Font = My.Settings.font
                        .Cells(ColumnIndex_RemoteProcess).Style.Font = My.Settings.font
                        .Cells(ColumnIndex_Hostname).Style.Font = My.Settings.font
                        .Cells(ColumnIndex_LogText).Style.Font = My.Settings.font
                        .Cells(ColumnIndex_Alerted).Style.Font = My.Settings.font
                        .Cells(ColumnIndex_ServerTime).Style.Font = My.Settings.font
                    End If

                    .DefaultCellStyle.Padding = New Padding(0, 2, 0, 2)
                End With

                Return MyDataGridViewRow
            End Using
        End Function

        Public Sub AddToLogList(strTimeStampFromServer As String, strLogText As String)
            If strLogText.CaseInsensitiveContains(strNewLine) Then strLogText = strLogText.Replace(strNewLine, vbCrLf).Trim

            Dim currentDate As Date = Now.ToLocalTime
            Dim serverDate As Date

            If String.IsNullOrWhiteSpace(strTimeStampFromServer) Then
                serverDate = currentDate
            Else
                Try
                    serverDate = ParseTimestamp(strTimeStampFromServer)
                Catch ex As FormatException
                    serverDate = currentDate
                    AddToLogList(Nothing, $"Unable to parse timestamp {strQuote}{strTimeStampFromServer.Trim}{strQuote}.")
                End Try
            End If

            ParentForm.Invoke(Sub()
                                  SyncLock ParentForm.dataGridLockObject
                                      ParentForm.Logs.Rows.Add(MakeLocalDataGridRowEntry(strLogText, ParentForm.Logs))
                                      ParentForm.UpdateLogCount()
                                      ParentForm.SelectLatestLogEntry()
                                      ParentForm.BtnSaveLogsToDisk.Enabled = True
                                      If ParentForm.intSortColumnIndex = 0 And ParentForm.sortOrder = SortOrder.Descending Then ParentForm.SortLogsByDateObjectNoLocking(ParentForm.intSortColumnIndex, ListSortDirection.Descending)
                                  End SyncLock

                                  ParentForm.NotifyIcon.Text = $"Free SysLog{vbCrLf}Last log received at {currentDate}."
                                  ParentForm.UpdateLogCount()
                                  ParentForm.BtnSaveLogsToDisk.Enabled = True

                                  If ParentForm.ChkEnableAutoScroll.Checked And ParentForm.Logs.Rows.Count > 0 And ParentForm.intSortColumnIndex = 0 Then
                                      Try
                                          boolIsProgrammaticScroll = True
                                          ParentForm.Logs.FirstDisplayedScrollingRowIndex = If(ParentForm.sortOrder = SortOrder.Ascending, ParentForm.Logs.Rows.Count - 1, 0)
                                      Finally
                                          boolIsProgrammaticScroll = False
                                      End Try
                                  End If
                              End Sub)
        End Sub

        Private Function GetSeverityAndFacility(strPriority As String) As (Facility As String, Severity As String)
            strPriority = strPriority.Replace("<", "").Replace(">", "").Trim

            Dim priorityNumber As Integer

            If Integer.TryParse(strPriority, priorityNumber) Then
                ' Define the severity levels as per RFC 5424
                Dim severityLevels() As String = {"Emergency", "Alert", "Critical", "Error", "Warning", "Notice", "Informational", "Debug"}

                ' Define the facility levels as per RFC 5424
                Dim facilityLevels() As String = {"Kernel messages", "User-level messages", "Mail system", "System daemons",
                                               "Security/authorization messages", "Messages generated internally by syslogd",
                                               "Line printer subsystem", "Network news subsystem", "UUCP subsystem",
                                               "Clock daemon", "Security/authorization messages", "FTP daemon",
                                               "NTP subsystem", "Log audit", "Log alert", "Clock daemon",
                                               "Local use 0", "Local use 1", "Local use 2", "Local use 3", "Local use 4",
                                               "Local use 5", "Local use 6", "Local use 7"}

                ' Calculate facility and severity
                Dim facility As Integer = priorityNumber \ 8
                Dim severity As Integer = priorityNumber Mod 8

                ' Get facility and severity descriptions
                Dim facilityDescription As String = If(facility >= 0 And facility < facilityLevels.Length, facilityLevels(facility), "Unknown Facility")
                Dim severityDescription As String = If(severity >= 0 And severity < severityLevels.Length, severityLevels(severity), "Unknown Severity")

                Return (facilityDescription, severityDescription)
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>Parses a date timestamp in String format to a Date Object.</summary>
        ''' <param name="timestamp">A date timestamp in String format.</param>
        ''' <returns>A Date Object.</returns>
        ''' <exception cref="FormatException">Throws a FormatException if the function can't parse the input.</exception>
        Public Function ParseTimestamp(timestamp As String) As Date
            Dim parsedDate As Date
            Dim userCulture As Globalization.CultureInfo = Globalization.CultureInfo.CurrentCulture
            Dim isEuropeanDateFormat As Boolean = userCulture.DateTimeFormat.ShortDatePattern.StartsWith("d")

            If timestamp.EndsWith("Z") Then
                ' RFC 3339/ISO 8601 format with UTC timezone and optional milliseconds
                If timestamp.Contains(":") AndAlso timestamp.Count(Function(c) c = ":") = 3 Then
                    ' Handle timestamp with extra colon before milliseconds (e.g., "yyyy-MM-ddTHH:mm:ss:fffZ")
                    timestamp = timestamp.Remove(timestamp.LastIndexOf(":"), 1).Insert(timestamp.LastIndexOf(":"), ".")
                End If

                If timestamp.Contains(".") Then
                    ' Handle timestamp with milliseconds
                    parsedDate = Date.ParseExact(timestamp, "yyyy-MM-ddTHH:mm:ss.fffZ", Globalization.CultureInfo.InvariantCulture, Globalization.DateTimeStyles.AdjustToUniversal)
                Else
                    ' Handle timestamp without milliseconds
                    parsedDate = Date.ParseExact(timestamp, "yyyy-MM-ddTHH:mm:ssZ", Globalization.CultureInfo.InvariantCulture, Globalization.DateTimeStyles.AdjustToUniversal)
                End If
            ElseIf timestamp.Contains("+") OrElse timestamp.Contains("-") Then
                Dim parsedDateOffset As DateTimeOffset

                ' RFC 3339/ISO 8601 format with timezone offset and optional milliseconds
                If timestamp.Contains(".") Then
                    ' Handle timestamp with milliseconds
                    parsedDateOffset = DateTimeOffset.ParseExact(timestamp, "yyyy-MM-ddTHH:mm:ss.fffzzz", Globalization.CultureInfo.InvariantCulture)
                    parsedDate = parsedDateOffset.DateTime
                Else
                    ' Handle timestamp without milliseconds
                    parsedDateOffset = DateTimeOffset.ParseExact(timestamp, "yyyy-MM-ddTHH:mm:sszzz", Globalization.CultureInfo.InvariantCulture)
                    parsedDate = parsedDateOffset.DateTime
                End If
            ElseIf timestamp.Length >= 15 AndAlso Char.IsLetter(timestamp(0)) Then
                ' "MMM dd HH:mm:ss" format (like "Sep  4 22:39:12")
                timestamp = timestamp.Replace("  ", " 0") ' Handle single-digit day (e.g., "Sep  4" becomes "Sep 04")
                parsedDate = Date.ParseExact(timestamp, "MMM dd HH:mm:ss", Globalization.CultureInfo.InvariantCulture)

                ' If you need to add the current year to the date:
                parsedDate = parsedDate.AddYears(Date.Now.Year - parsedDate.Year)
            ElseIf timestamp.Contains("/") Then
                If timestamp.EndsWith("PM", StringComparison.OrdinalIgnoreCase) Or timestamp.EndsWith("AM", StringComparison.OrdinalIgnoreCase) Then
                    ' Handle both American and European formats based on localization
                    If isEuropeanDateFormat Then
                        ' European format "d/M/yyyy HH:mm:ss"
                        parsedDate = Date.ParseExact(timestamp, "d/M/yyyy H:mm:ss tt", Globalization.CultureInfo.InvariantCulture)
                    Else
                        ' American format "M/d/yyyy h:mm:ss tt"
                        parsedDate = Date.ParseExact(timestamp, "M/d/yyyy h:mm:ss tt", Globalization.CultureInfo.InvariantCulture)
                    End If
                Else
                    ' Handle both American and European formats based on localization
                    If isEuropeanDateFormat Then
                        ' European format "d/M/yyyy HH:mm:ss"
                        parsedDate = Date.ParseExact(timestamp, "d/M/yyyy H:mm:ss", Globalization.CultureInfo.InvariantCulture)
                    Else
                        ' American format "M/d/yyyy h:mm:ss"
                        parsedDate = Date.ParseExact(timestamp, "M/d/yyyy h:mm:ss", Globalization.CultureInfo.InvariantCulture)
                    End If
                End If
            Else
                Throw New FormatException("Unknown timestamp format.")
            End If

            Return parsedDate
        End Function

        Public Function ConvertLineFeeds(strInput As String) As String
            strInput = strInput.Replace(vbCrLf, vbLf) ' Temporarily replace all CRLF with LF
            strInput = strInput.Replace(vbCr, vbLf)   ' Convert standalone CR to LF
            strInput = strInput.Replace(vbLf, vbCrLf) ' Finally, replace all LF with CRLF
            Return strInput.Trim()
        End Function

        Private Function IsRegexMatch(regex As Regex, strRawLogText As String, ByRef match As Match) As Boolean
            match = regex.Match(strRawLogText)
            Return match.Success
        End Function

        Private Function TrySplitLogEntries(regex As Regex, input As String, ByRef matches As String()) As Boolean
            If IsRegexMatch(regex, input, Nothing) Then
                matches = regex.Split(input)
                Return True
            End If

            Return False
        End Function

        Public Sub ProcessIncomingLog(strRawLogText As String, strSourceIP As String)
            If Not String.IsNullOrWhiteSpace(strRawLogText) AndAlso Not String.IsNullOrWhiteSpace(strSourceIP) Then
                ' Convert all linefeeds.
                strRawLogText = ConvertLineFeeds(strRawLogText)

                ' Do some pre-processing so that we can separate out the data more easily.
                strRawLogText = strRawLogText.Replace(vbCrLf, strNewLine).Replace(vbLf, strNewLine)
                strRawLogText = SyslogPreProcessor1.Replace(strRawLogText, "$1")
                strRawLogText = SyslogPreProcessor2.Replace(strRawLogText, vbCrLf & "$1")

                ' Split off each syslog entry by using vbCrLf as a delimiter.
                Dim matches As String() = strRawLogText.Split(vbCrLf)

                ' Check to see if we have anything to work with before we go into the loop.
                If matches.Any() Then
                    ' Now work with each syslog entry.
                    For Each strMatch As String In matches
                        If Not String.IsNullOrWhiteSpace(strMatch) Then
                            ProcessIncomingLog_SubRoutine(strMatch, strSourceIP)
                        End If
                    Next
                Else
                    ' Nope, log it as unable to be parsed.
                    AddToLogList(Nothing, $"Unable to parse log {strQuote}{strRawLogText}{strQuote}.")
                End If
            End If
        End Sub

        Public Sub ProcessIncomingLog_SubRoutine(strRawLogText As String, strSourceIP As String)
            Try
                If Not String.IsNullOrWhiteSpace(strRawLogText) AndAlso Not String.IsNullOrWhiteSpace(strSourceIP) Then
                    Dim boolIgnored As Boolean = False
                    Dim boolAlerted As Boolean = False
                    Dim priorityObject As (Facility As String, Severity As String) = Nothing
                    Dim priority As String = Nothing
                    Dim message As String = Nothing
                    Dim timestamp As String = Nothing
                    Dim hostname As String = Nothing
                    Dim customHostname As String = Nothing
                    Dim appName As String = Nothing
                    Dim strAlertText As String = Nothing
                    Dim AlertType As AlertType = AlertType.None
                    Dim strIgnoredPattern As String = Nothing

                    ' Step 1: Use Regex to extract the RFC 5424 header and the message
                    Dim match As Match = Nothing

                    If IsRegexMatch(rfc5424TransformRegex, strRawLogText, match) OrElse IsRegexMatch(rfc5424Regex, strRawLogText, match) Then
                        ' Handling the transformation to RFC 5424 format
                        priority = If(String.IsNullOrWhiteSpace(match.Groups("priority").Value), "", match.Groups("priority").Value)
                        timestamp = If(String.IsNullOrWhiteSpace(match.Groups("timestamp").Value), "", match.Groups("timestamp").Value)
                        hostname = If(String.IsNullOrWhiteSpace(match.Groups("hostname").Value), "", match.Groups("hostname").Value)
                        appName = If(String.IsNullOrWhiteSpace(match.Groups("appname").Value), "", match.Groups("appname").Value)
                        message = If(String.IsNullOrWhiteSpace(match.Groups("message").Value), "", match.Groups("message").Value)

                        If SupportCode.hostnames.TryGetValue(strSourceIP, customHostname) Then hostname = customHostname

                        priorityObject = GetSeverityAndFacility(priority)
                    Else
                        timestamp = Now.ToString
                        hostname = ""
                        appName = ""
                        priorityObject = ("Local", "Error")
                        message = $"An error occured while attempting to parse the log entry. Below is the log entry that failed...{vbCrLf}{strRawLogText}" ' Something went wrong, we couldn't parse the entry so we're going to just pass the raw log entry to the program.
                    End If

                    If Not String.IsNullOrWhiteSpace(message) AndAlso message.CaseInsensitiveContains(strNewLine) Then message = message.Replace(strNewLine, vbCrLf).Trim

                    If My.Settings.RemoveNumbersFromRemoteApp Then appName = NumberRemovingRegex.Replace(appName, "$1")

                    ' Step 3: Handle the ignored logs and alerts
                    If My.Settings.ProcessReplacementsInSyslogDataFirst Then
                        If replacementsList IsNot Nothing AndAlso replacementsList.Any() Then message = ProcessReplacements(message)
                        If ignoredList IsNot Nothing AndAlso ignoredList.Any() Then boolIgnored = ProcessIgnoredLogPreferences(strRawLogText, appName, strIgnoredPattern)
                    Else
                        If ignoredList IsNot Nothing AndAlso ignoredList.Any() Then boolIgnored = ProcessIgnoredLogPreferences(strRawLogText, appName, strIgnoredPattern)
                        If replacementsList IsNot Nothing AndAlso replacementsList.Any() Then message = ProcessReplacements(message)
                    End If

                    If alertsList IsNot Nothing AndAlso alertsList.Any() Then boolAlerted = ProcessAlerts(message, strAlertText, Now.ToString, strSourceIP, strRawLogText, AlertType)

                    If Not boolIgnored Then
                        Dim strLimitBy As String = Nothing

                        ParentForm.boxLimitBy.Invoke(Sub() strLimitBy = ParentForm.boxLimitBy.Text)

                        Dim logType As String = $"{priorityObject.Severity}, {priorityObject.Facility}"

                        SyncLock recentUniqueObjectsLock
                            With recentUniqueObjects
                                If .logTypes.Add(logType) AndAlso strLimitBy.Equals("Log Type", StringComparison.OrdinalIgnoreCase) Then
                                    ParentForm.boxLimiter.Items.Add(logType)
                                End If

                                If .processes.Add(appName) AndAlso strLimitBy.Equals("Remote Process", StringComparison.OrdinalIgnoreCase) Then
                                    ParentForm.boxLimiter.Items.Add(appName)
                                End If

                                If .hostNames.Add(hostname) AndAlso strLimitBy.Equals("Source Hostname", StringComparison.OrdinalIgnoreCase) Then
                                    ParentForm.boxLimiter.Items.Add(hostname)
                                End If

                                If .ipAddresses.Add(strSourceIP) AndAlso strLimitBy.Equals("Source IP Address", StringComparison.OrdinalIgnoreCase) Then
                                    ParentForm.boxLimiter.Items.Add(strSourceIP)
                                End If
                            End With
                        End SyncLock
                    End If

                    ' Step 4: Add to log list, separating header and message
                    AddToLogList(timestamp, strSourceIP, hostname, appName, message, boolIgnored, boolAlerted, priorityObject, strRawLogText, strAlertText, AlertType, strIgnoredPattern)
                End If
            Catch ex As Exception
                AddToLogList(Nothing, $"{ex.Message} -- {ex.StackTrace}{vbCrLf}Data from Server: {strRawLogText}")
            End Try
        End Sub

        Private Function MatchPattern(strInput As String, strPattern As String, boolUseRegex As Boolean, boolCaseSensitive As Boolean) As Boolean
            If boolUseRegex Then
                Return GetCachedRegex(IgnoredRegexCache, strPattern, boolCaseSensitive).IsMatch(strInput)
            Else
                If boolCaseSensitive Then
                    Return strInput.Contains(strPattern)
                Else
                    Return strInput.CaseInsensitiveContains(strPattern)
                End If
            End If
        End Function

        Private Function ProcessIgnoredLogPreferences(message As String, remoteProcess As String, ByRef strIgnoredPattern As String) As Boolean
            Dim strFailedPattern As String = Nothing
            Dim matchFound As Boolean = False
            Dim _strIgnoredPattern As String = Nothing
            Dim parallelOptions As New ParallelOptions With {.MaxDegreeOfParallelism = Environment.ProcessorCount}

            ' Using a SyncLock to protect the shared ignoredList and prevent race conditions.
            SyncLock ignoredList
                Try
                    ' Using a SyncLock to protect the shared IgnoredRegexCache and prevent race conditions.
                    SyncLock IgnoredRegexCache
                        ' Use a thread-safe flag to stop Parallel.ForEach as soon as a match is found.
                        ' This ensures that only the first match updates the result.
                        Dim lockObj As New Object()

                        ' Parallel loop to check each pattern concurrently
                        Parallel.ForEach(ignoredList, parallelOptions, Sub(ignoredClassInstance As IgnoredClass, state As ParallelLoopState)
                                                                           If Not matchFound Then ' Check this flag to prevent unnecessary checks after a match
                                                                               Dim strRegexPattern As String = ignoredClassInstance.StrIgnore
                                                                               strFailedPattern = strRegexPattern

                                                                               Dim boolDidWeMatch As Boolean = False
                                                                               Dim strInput As String

                                                                               If ignoredClassInstance.IgnoreType = IgnoreType.RemoteApp AndAlso Not String.IsNullOrWhiteSpace(remoteProcess) Then
                                                                                   strInput = remoteProcess
                                                                               Else
                                                                                   strInput = message
                                                                               End If

                                                                               If MatchPattern(strInput, strRegexPattern, ignoredClassInstance.BoolRegex, ignoredClassInstance.BoolCaseSensitive) Then
                                                                                   ' Use lock to safely update shared state (_strIgnoredPattern and ParentForm.longNumberOfIgnoredLogs)
                                                                                   SyncLock lockObj
                                                                                       If Not matchFound Then
                                                                                           _strIgnoredPattern = strRegexPattern
                                                                                           matchFound = True
                                                                                           state.Stop()
                                                                                           If ParentForm IsNot Nothing Then ParentForm.Invoke(Sub() Interlocked.Increment(ParentForm.longNumberOfIgnoredLogs))
                                                                                       End If
                                                                                   End SyncLock
                                                                               End If
                                                                           End If
                                                                       End Sub)
                    End SyncLock

                    If matchFound Then strIgnoredPattern = _strIgnoredPattern
                    Return matchFound
                Catch ex As Exception
                    AddToLogList(Nothing, $"{strQuote}{strFailedPattern}{strQuote} failed to be processed.")
                    Return False
                End Try
            End SyncLock
        End Function

        Private Sub AddToLogList(strTimeStampFromServer As String, strSourceIP As String, strHostname As String, strRemoteProcess As String, strLogText As String, boolIgnored As Boolean, boolAlerted As Boolean, priority As (Facility As String, Severity As String), strRawLogText As String, strAlertText As String, alertType As AlertType, strIgnoredPattern As String)
            Dim currentDate As Date = Now.ToLocalTime
            Dim serverDate As Date

            If String.IsNullOrWhiteSpace(strTimeStampFromServer) Then
                serverDate = currentDate
            Else
                Try
                    serverDate = ParseTimestamp(strTimeStampFromServer)
                Catch ex As FormatException
                    serverDate = currentDate
                    AddToLogList(Nothing, $"Unable to parse timestamp {strQuote}{strTimeStampFromServer.Trim}{strQuote}.")
                End Try
            End If

            If Not boolIgnored Then
                SyncLock ParentForm.dataGridLockObject
                    ParentForm.Logs.Invoke(Sub()
                                               ParentForm.Logs.Rows.Add(MakeDataGridRow(serverTimeStamp:=serverDate,
                                                                                        dateObject:=currentDate,
                                                                                        strTime:=currentDate.ToString,
                                                                                        strSourceAddress:=strSourceIP,
                                                                                        strHostname:=strHostname,
                                                                                        strRemoteProcess:=strRemoteProcess,
                                                                                        strLog:=strLogText,
                                                                                        strLogType:=$"{priority.Severity}, {priority.Facility}",
                                                                                        boolAlerted:=boolAlerted,
                                                                                        strRawLogText:=strRawLogText.Trim,
                                                                                        strAlertText:=strAlertText,
                                                                                        AlertType:=alertType,
                                                                                        dataGrid:=ParentForm.Logs)
                                                                                       )
                                           End Sub)
                    If ParentForm.intSortColumnIndex = 0 And ParentForm.sortOrder = SortOrder.Descending Then ParentForm.SortLogsByDateObjectNoLocking(ParentForm.intSortColumnIndex, ListSortDirection.Descending)
                End SyncLock

                ParentForm.NotifyIcon.Text = $"Free SysLog{vbCrLf}Last log received at {currentDate}."
                ParentForm.UpdateLogCount()

                ParentForm.Invoke(Sub()
                                      ParentForm.BtnSaveLogsToDisk.Enabled = True
                                      ParentForm.SelectLatestLogEntry()
                                  End Sub)
            ElseIf boolIgnored Then
                If ParentForm.ChkEnableRecordingOfIgnoredLogs.Checked Then
                    SyncLock ParentForm.IgnoredLogsLockObject
                        Dim NewIgnoredItem As MyDataGridViewRow = MakeDataGridRow(serverTimeStamp:=serverDate,
                                                                                  dateObject:=currentDate,
                                                                                  strTime:=currentDate.ToString,
                                                                                  strSourceAddress:=strSourceIP,
                                                                                  strHostname:=strHostname,
                                                                                  strRemoteProcess:=strRemoteProcess,
                                                                                  strLog:=strLogText,
                                                                                  strLogType:=$"{priority.Severity}, {priority.Facility}",
                                                                                  boolAlerted:=boolAlerted,
                                                                                  strRawLogText:=strRawLogText.Trim,
                                                                                  strAlertText:=strAlertText,
                                                                                  AlertType:=alertType,
                                                                                  dataGrid:=ParentForm.Logs
                                                                                 )
                        NewIgnoredItem.IgnoredPattern = strIgnoredPattern

                        SyncLock ParentForm.IgnoredLogsLockingObject
                            If ParentForm.IgnoredLogs.Count < My.Settings.LimitNumberOfIgnoredLogs Then
                                ParentForm.IgnoredLogs.Add(NewIgnoredItem)
                            Else
                                While ParentForm.IgnoredLogs.Count >= My.Settings.LimitNumberOfIgnoredLogs
                                    ParentForm.IgnoredLogs.RemoveAt(0)
                                End While

                                ParentForm.IgnoredLogs.Add(NewIgnoredItem)
                            End If

                            If My.Settings.recordIgnoredLogs Then
                                ParentForm.LblNumberOfIgnoredIncomingLogs.Text = $"Number of ignored incoming logs: {ParentForm.IgnoredLogs.Count:N0}"
                            Else
                                ParentForm.ZerooutIgnoredLogsCounterToolStripMenuItem.Enabled = True
                                ParentForm.LblNumberOfIgnoredIncomingLogs.Text = $"Number of ignored incoming logs: {ParentForm.longNumberOfIgnoredLogs:N0}"
                            End If
                        End SyncLock

                        SyncLock IgnoredLogsAndSearchResultsInstanceLockObject
                            If IgnoredLogsAndSearchResultsInstance IsNot Nothing Then IgnoredLogsAndSearchResultsInstance.AddIgnoredDatagrid(NewIgnoredItem)
                        End SyncLock

                        ParentForm.Invoke(Sub() ParentForm.ClearIgnoredLogsToolStripMenuItem.Enabled = True)
                    End SyncLock
                Else
                    Interlocked.Increment(ParentForm.longNumberOfIgnoredLogs)
                    ParentForm.LblNumberOfIgnoredIncomingLogs.Text = $"Number of ignored incoming logs: {ParentForm.longNumberOfIgnoredLogs:N0}"
                End If
            End If
        End Sub

        Private Function ProcessReplacements(input As String) As String
            SyncLock ReplacementsRegexCache
                For Each item As ReplacementsClass In replacementsList
                    Try
                        input = GetCachedRegex(ReplacementsRegexCache, If(item.BoolRegex, item.StrReplace, Regex.Escape(item.StrReplace).Replace("\ ", " ")), item.BoolCaseSensitive).Replace(input, item.StrReplaceWith)
                    Catch ex As Exception
                    End Try
                Next
            End SyncLock

            Return input
        End Function

        Private Function GetCachedRegex(ByRef regexCache As Dictionary(Of String, Regex), pattern As String, Optional boolCaseSensitive As Boolean = True) As Regex
            If Not regexCache.ContainsKey(pattern) Then regexCache(pattern) = New Regex(pattern, If(boolCaseSensitive, RegexOptions.Compiled, RegexOptions.Compiled Or RegexOptions.IgnoreCase))
            Return regexCache(pattern)
        End Function

        Private Function ProcessAlerts(strLogText As String, ByRef strOutgoingAlertText As String, strLogDate As String, strSourceIP As String, strRawLogText As String, ByRef alertTypeAsAlertType As AlertType) As Boolean
            Dim ToolTipIcon As ToolTipIcon = ToolTipIcon.None
            Dim RegExObject As Regex
            Dim strAlertText As String
            Dim regExGroupCollection As GroupCollection

            SyncLock AlertsRegexCache
                For Each alert As AlertsClass In alertsList
                    RegExObject = GetCachedRegex(AlertsRegexCache, If(alert.BoolRegex, alert.StrLogText, Regex.Escape(alert.StrLogText).Replace("\ ", " ")), alert.BoolCaseSensitive)

                    If RegExObject.IsMatch(strLogText) Then
                        If alert.alertType = AlertType.Warning Then
                            ToolTipIcon = ToolTipIcon.Warning
                            alertTypeAsAlertType = AlertType.Warning
                        ElseIf alert.alertType = AlertType.ErrorMsg Then
                            ToolTipIcon = ToolTipIcon.Error
                            alertTypeAsAlertType = AlertType.ErrorMsg
                        ElseIf alert.alertType = AlertType.Info Then
                            ToolTipIcon = ToolTipIcon.Info
                            alertTypeAsAlertType = AlertType.Info
                        End If

                        strAlertText = If(String.IsNullOrWhiteSpace(alert.StrAlertText), strLogText, alert.StrAlertText)

                        If alert.BoolRegex And Not String.IsNullOrWhiteSpace(alert.StrAlertText) Then
                            regExGroupCollection = RegExObject.Match(strLogText).Groups

                            If regExGroupCollection.Count > 0 Then
                                For index As Integer = 0 To regExGroupCollection.Count - 1
                                    ' Handle the indexed group
                                    strAlertText = GetCachedRegex(AlertsRegexCache, Regex.Escape($"${index}"), False).Replace(strAlertText, regExGroupCollection(index).Value)

                                    ' Handle the named group
                                    If Not String.IsNullOrEmpty(regExGroupCollection(index).Name) Then
                                        strAlertText = GetCachedRegex(AlertsRegexCache, Regex.Escape($"$({regExGroupCollection(index).Name})"), True).Replace(strAlertText, regExGroupCollection(regExGroupCollection(index).Name).Value)
                                    End If
                                Next
                            End If
                        End If

                        If alert.BoolLimited Then
                            NotificationLimiter.ShowNotification(strAlertText, ToolTipIcon, strLogText, strLogDate, strSourceIP, strRawLogText)
                        Else
                            ShowToastNotification(strAlertText, ToolTipIcon, strLogText, strLogDate, strSourceIP, strRawLogText)
                        End If

                        strOutgoingAlertText = strAlertText
                        Return True
                    End If
                Next
            End SyncLock

            Return False
        End Function
    End Module
End Namespace