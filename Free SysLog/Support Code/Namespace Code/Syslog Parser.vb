Imports System.Text.RegularExpressions
Imports Free_SysLog.SupportCode

Namespace SyslogParser
    Public Module SyslogParser
        Private ParentForm As Form1
        Private ReadOnly rfc5424Regex As New Regex("\d{0,}(<\d+>)([0-9]{4}[/.-](?:1[0-2]|0[1-9])[/.-](?:3[01]|[12][0-9]|0[1-9])T(?:2[0-3]|[01][0-9])[:.][0-5][0-9][:.][0-5][0-9]\.[0-9]+Z)\s+(\S+)\s+(\S+)\s+(\S+\s+\S+\s+\S+.*)", RegexOptions.Compiled Or RegexOptions.Singleline)
        Private ReadOnly rfc5424TransformRegex As New Regex("(<\d+>)(\w{3} \d{1,2} \d{2}:\d{2}:\d{2}) (\S+) (\S+): (.*)(?:\n|\r\n)", RegexOptions.Compiled Or RegexOptions.Singleline)

        Private ReadOnly rfc5424RegexWithoutGroups As New Regex("(\d{0,}<\d+>[0-9]{4}[/.-](?:1[0-2]|0[1-9])[/.-](?:3[01]|[12][0-9]|0[1-9])T(?:2[0-3]|[01][0-9])[:.][0-5][0-9][:.][0-5][0-9]\.[0-9]+Z\s+\S+\s+\S+\s+\S+\s+\S+\s+\S+.*)", RegexOptions.Compiled Or RegexOptions.Singleline)
        Private ReadOnly rfc5424TransformRegexWithoutGroups As New Regex("(<\d+>\w{3} \d{1,2} \d{2}:\d{2}:\d{2} \S+ \S+: .*(?:\n|\r\n))", RegexOptions.Compiled Or RegexOptions.Singleline)

        Private ReadOnly NumberRemovingRegex As New Regex("([A-Za-z-]*)\[[0-9]*\]", RegexOptions.Compiled)

        Public WriteOnly Property SetParentForm As Form1
            Set(value As Form1)
                ParentForm = value
            End Set
        End Property

        Public Function MakeDataGridRow(serverTimeStamp As Date, dateObject As Date, strTime As String, strSourceAddress As String, strHostname As String, strRemoteProcess As String, strLog As String, strLogType As String, boolAlerted As Boolean, strRawLogText As String, ByRef dataGrid As DataGridView) As MyDataGridViewRow
            Using MyDataGridViewRow As New MyDataGridViewRow
                With MyDataGridViewRow
                    .CreateCells(dataGrid)
                    .Cells(ColumnIndex_ComputedTime).Value = strTime
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
                    .MinimumHeight = GetMinimumHeight(strLog, ParentForm.Logs.DefaultCellStyle.Font, ParentForm.ColLog.Width)
                End With

                Return MyDataGridViewRow
            End Using
        End Function

        Public Sub AddToLogList(strTimeStampFromServer As String, strSourceIP As String, strLogText As String)
            Dim currentDate As Date = Now.ToLocalTime
            Dim serverDate As Date

            If String.IsNullOrWhiteSpace(strTimeStampFromServer) Then
                serverDate = currentDate
            Else
                Try
                    serverDate = ParseTimestamp(strTimeStampFromServer)
                Catch ex As FormatException
                    serverDate = currentDate
                    AddToLogList(Nothing, "local", $"Unable to parse timestamp {strQuote}{strTimeStampFromServer.Trim}{strQuote}.")
                End Try
            End If

            ParentForm.Invoke(Sub()
                                  SyncLock ParentForm.dataGridLockObject
                                      ParentForm.Logs.Rows.Add(MakeDataGridRow(serverDate, currentDate, currentDate.ToString, strSourceIP, Nothing, Nothing, strLogText, "Informational, Local", False, Nothing, ParentForm.Logs))
                                      If ParentForm.intSortColumnIndex = 0 And ParentForm.sortOrder = SortOrder.Descending Then ParentForm.SortLogsByDateObjectNoLocking(ParentForm.intSortColumnIndex, SortOrder.Descending)
                                  End SyncLock

                                  ParentForm.NotifyIcon.Text = $"Free SysLog{vbCrLf}Last log received at {currentDate}."
                                  ParentForm.UpdateLogCount()
                                  ParentForm.BtnSaveLogsToDisk.Enabled = True

                                  If ParentForm.ChkEnableAutoScroll.Checked And ParentForm.Logs.Rows.Count > 0 And ParentForm.intSortColumnIndex = 0 Then
                                      ParentForm.Logs.FirstDisplayedScrollingRowIndex = If(ParentForm.sortOrder = SortOrder.Ascending, ParentForm.Logs.Rows.Count - 1, 0)
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
        Private Function ParseTimestamp(timestamp As String) As Date
            Dim parsedDate As Date
			Dim userCulture As Globalization.CultureInfo = Globalization.CultureInfo.CurrentCulture
			Dim isEuropeanDateFormat As Boolean = userCulture.DateTimeFormat.ShortDatePattern.StartsWith("d")

            If timestamp.EndsWith("Z") Then
                ' RFC 3339/ISO 8601 format with UTC timezone and optional milliseconds ("yyyy-MM-ddTHH:mm:ssZ" or "yyyy-MM-ddTHH:mm:ss.fffZ")
                If timestamp.Contains(".") Then
                    ' Handle timestamp with milliseconds
                    parsedDate = Date.ParseExact(timestamp, "yyyy-MM-ddTHH:mm:ss.fffZ", Globalization.CultureInfo.InvariantCulture, Globalization.DateTimeStyles.AdjustToUniversal)
                Else
                    ' Handle timestamp without milliseconds
                    parsedDate = Date.ParseExact(timestamp, "yyyy-MM-ddTHH:mm:ssZ", Globalization.CultureInfo.InvariantCulture, Globalization.DateTimeStyles.AdjustToUniversal)
                End If
            ElseIf timestamp.Contains("+") OrElse timestamp.Contains("-") Then
                Dim parsedDateOffset As DateTimeOffset

                ' RFC 3339/ISO 8601 format with timezone offset and optional milliseconds ("yyyy-MM-ddTHH:mm:sszzz" or "yyyy-MM-ddTHH:mm:ss.fffzzz")
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
                ' Handle both American and European formats based on localization
                If isEuropeanDateFormat Then
                    ' European format "dd/MM/yyyy HH:mm:ss"
                    parsedDate = Date.ParseExact(timestamp, "dd/MM/yyyy HH:mm:ss", Globalization.CultureInfo.InvariantCulture)
                Else
                    ' American format "MM/dd/yyyy HH:mm:ss"
                    parsedDate = Date.ParseExact(timestamp, "MM/dd/yyyy HH:mm:ss", Globalization.CultureInfo.InvariantCulture)
                End If
            Else
                Throw New FormatException("Unknown timestamp format.")
            End If

            Return parsedDate
        End Function

        Private Function ConvertLineFeeds(strInput As String) As String
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
                Dim matches As String() = Nothing

                If TrySplitLogEntries(rfc5424TransformRegexWithoutGroups, strRawLogText, matches) OrElse TrySplitLogEntries(rfc5424RegexWithoutGroups, strRawLogText, matches) Then
                    matches = rfc5424TransformRegexWithoutGroups.Split(strRawLogText)

                    For Each strMatch As String In matches
                        If Not String.IsNullOrWhiteSpace(strMatch) Then
                            ProcessIncomingLog_SubRoutine(strMatch, strSourceIP)
                        End If
                    Next
                Else
                    AddToLogList(Nothing, "local", $"Unable to parse log {strQuote}{strRawLogText}{strQuote}.")
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
                    Dim appName As String = Nothing

                    ' Step 1: Use Regex to extract the RFC 5424 header and the message
                    Dim match As Match = Nothing

                    If IsRegexMatch(rfc5424TransformRegex, strRawLogText, match) Then
                        ' Handling the transformation to RFC 5424 format
                        priority = If(String.IsNullOrWhiteSpace(match.Groups(1).Value), "", match.Groups(1).Value)
                        timestamp = If(String.IsNullOrWhiteSpace(match.Groups(2).Value), "", match.Groups(2).Value)
                        hostname = If(String.IsNullOrWhiteSpace(match.Groups(3).Value), "", match.Groups(3).Value)
                        appName = If(String.IsNullOrWhiteSpace(match.Groups(4).Value), "", match.Groups(4).Value)
                        message = If(String.IsNullOrWhiteSpace(match.Groups(5).Value), "", match.Groups(5).Value)

                        priorityObject = GetSeverityAndFacility(priority)
                    ElseIf IsRegexMatch(rfc5424Regex, strRawLogText, match) Then
                        ' Match against RFC 5424 formatted logs
                        priority = If(String.IsNullOrWhiteSpace(match.Groups(1).Value), "", match.Groups(1).Value)
                        timestamp = If(String.IsNullOrWhiteSpace(match.Groups(2).Value), "", match.Groups(2).Value)
                        hostname = If(String.IsNullOrWhiteSpace(match.Groups(3).Value), "", match.Groups(3).Value)
                        appName = If(String.IsNullOrWhiteSpace(match.Groups(4).Value), "", match.Groups(4).Value)
                        message = If(String.IsNullOrWhiteSpace(match.Groups(5).Value), "", match.Groups(5).Value)

                        priorityObject = GetSeverityAndFacility(priority)
                    Else
                        timestamp = Now.ToString
                        hostname = ""
                        appName = ""
                        priorityObject = ("Local", "Error")
                        message = $"An error occured while attempting to parse the log entry. Below is the log entry that failed...{vbCrLf}{strRawLogText}" ' Something went wrong, we couldn't parse the entry so we're going to just pass the raw log entry to the program.
                    End If

                    ' Step 2: Process the log message (previous processing logic)
                    message = ConvertLineFeeds(message)
                    strRawLogText = ConvertLineFeeds(strRawLogText)

                    If My.Settings.RemoveNumbersFromRemoteApp Then appName = NumberRemovingRegex.Replace(appName, "$1")

                    ' Step 3: Handle the ignored logs and alerts
                    If ignoredList IsNot Nothing AndAlso ignoredList.Count > 0 Then boolIgnored = ProcessIgnoredLogPreferences(message)
                    If replacementsList IsNot Nothing AndAlso replacementsList.Count > 0 Then message = ProcessReplacements(message)
                    If alertsList IsNot Nothing AndAlso alertsList.Count > 0 Then boolAlerted = ProcessAlerts(message)

                    ' Step 4: Add to log list, separating header and message
                    AddToLogList(timestamp, strSourceIP, hostname, appName, message, boolIgnored, boolAlerted, priorityObject, strRawLogText)
                End If
            Catch ex As Exception
                AddToLogList(Nothing, "local", $"{ex.Message} -- {ex.StackTrace}{vbCrLf}Data from Server: {strRawLogText}")
            End Try
        End Sub

        Private Function ProcessIgnoredLogPreferences(message As String) As Boolean
            For Each ignoredClassInstance As IgnoredClass In ignoredList
                If GetCachedRegex(If(ignoredClassInstance.BoolRegex, ignoredClassInstance.StrIgnore, $".*{Regex.Escape(ignoredClassInstance.StrIgnore)}.*"), ignoredClassInstance.BoolCaseSensitive).IsMatch(message) Then
                    ParentForm.Invoke(Sub()
                                          ParentForm.longNumberOfIgnoredLogs += 1
                                          If Not ParentForm.ChkEnableRecordingOfIgnoredLogs.Checked Then
                                              ParentForm.ZerooutIgnoredLogsCounterToolStripMenuItem.Enabled = True
                                          End If
                                          ParentForm.LblNumberOfIgnoredIncomingLogs.Text = $"Number of ignored incoming logs: {ParentForm.longNumberOfIgnoredLogs:N0}"
                                      End Sub)
                    Return True
                End If
            Next

            Return False
        End Function

        Private Sub AddToLogList(strTimeStampFromServer As String, strSourceIP As String, strHostname As String, strRemoteProcess As String, strLogText As String, boolIgnored As Boolean, boolAlerted As Boolean, priority As (Facility As String, Severity As String), strRawLogText As String)
            Dim currentDate As Date = Now.ToLocalTime
            Dim serverDate As Date

            If String.IsNullOrWhiteSpace(strTimeStampFromServer) Then
                serverDate = currentDate
            Else
                Try
                    serverDate = ParseTimestamp(strTimeStampFromServer)
                Catch ex As FormatException
                    serverDate = currentDate
                    AddToLogList(Nothing, "local", $"Unable to parse timestamp {strQuote}{strTimeStampFromServer.Trim}{strQuote}.")
                End Try
            End If

            If Not boolIgnored Then
                ParentForm.Invoke(Sub()
                                      SyncLock ParentForm.dataGridLockObject
                                          ParentForm.Logs.Rows.Add(MakeDataGridRow(serverDate, currentDate, currentDate.ToString, strSourceIP, strHostname, strRemoteProcess, strLogText, $"{priority.Severity}, {priority.Facility}", boolAlerted, strRawLogText, ParentForm.Logs))
                                          If ParentForm.intSortColumnIndex = 0 And ParentForm.sortOrder = SortOrder.Descending Then ParentForm.SortLogsByDateObjectNoLocking(ParentForm.intSortColumnIndex, SortOrder.Descending)
                                      End SyncLock

                                      ParentForm.NotifyIcon.Text = $"Free SysLog{vbCrLf}Last log received at {currentDate}."
                                      ParentForm.UpdateLogCount()
                                      ParentForm.BtnSaveLogsToDisk.Enabled = True

                                      ParentForm.SelectLatestLogEntry()
                                  End Sub)
            ElseIf boolIgnored And ParentForm.ChkEnableRecordingOfIgnoredLogs.Checked Then
                SyncLock ParentForm.IgnoredLogsLockObject
                    Dim NewIgnoredItem As MyDataGridViewRow = MakeDataGridRow(serverDate, currentDate, currentDate.ToString, strSourceIP, strHostname, strRemoteProcess, strLogText, $"{priority.Severity}, {priority.Facility}", boolAlerted, strRawLogText, ParentForm.Logs)
                    ParentForm.IgnoredLogs.Add(NewIgnoredItem)
                    If IgnoredLogsAndSearchResultsInstance IsNot Nothing Then IgnoredLogsAndSearchResultsInstance.AddIgnoredDatagrid(NewIgnoredItem, ParentForm.ChkEnableAutoScroll.Checked)
                    ParentForm.Invoke(Sub() ParentForm.ClearIgnoredLogsToolStripMenuItem.Enabled = True)
                End SyncLock
            End If
        End Sub

        Private Function ProcessReplacements(input As String) As String
            For Each item As ReplacementsClass In replacementsList
                Try
                    input = GetCachedRegex(If(item.BoolRegex, item.StrReplace, Regex.Escape(item.StrReplace)), item.BoolCaseSensitive).Replace(input, item.StrReplaceWith)
                Catch ex As Exception
                End Try
            Next

            Return input
        End Function

        Private Function GetCachedRegex(pattern As String, Optional boolCaseInsensitive As Boolean = True) As Regex
            If Not ParentForm.regexCache.ContainsKey(pattern) Then ParentForm.regexCache(pattern) = New Regex(pattern, If(boolCaseInsensitive, RegexOptions.Compiled Or RegexOptions.IgnoreCase, RegexOptions.Compiled))
            Return ParentForm.regexCache(pattern)
        End Function

        Private Function ProcessAlerts(strLogText As String) As Boolean
            Dim ToolTipIcon As ToolTipIcon = ToolTipIcon.None
            Dim RegExObject As Regex
            Dim strAlertText As String
            Dim regExGroupCollection As GroupCollection

            For Each alert As AlertsClass In alertsList
                RegExObject = GetCachedRegex(If(alert.BoolRegex, alert.StrLogText, Regex.Escape(alert.StrLogText)), alert.BoolCaseSensitive)

                If RegExObject.IsMatch(strLogText) Then
                    If alert.alertType = AlertType.Warning Then
                        ToolTipIcon = ToolTipIcon.Warning
                    ElseIf alert.alertType = AlertType.ErrorMsg Then
                        ToolTipIcon = ToolTipIcon.Error
                    ElseIf alert.alertType = AlertType.Info Then
                        ToolTipIcon = ToolTipIcon.Info
                    End If

                    strAlertText = If(String.IsNullOrWhiteSpace(alert.StrAlertText), strLogText, alert.StrAlertText)

                    If alert.BoolRegex And Not String.IsNullOrWhiteSpace(alert.StrAlertText) Then
                        regExGroupCollection = RegExObject.Match(strLogText).Groups

                        If regExGroupCollection.Count > 0 Then
                            For index As Integer = 0 To regExGroupCollection.Count - 1
                                strAlertText = GetCachedRegex(Regex.Escape($"${index}"), False).Replace(strAlertText, regExGroupCollection(index).Value)
                            Next

                            For Each item As Group In regExGroupCollection
                                strAlertText = GetCachedRegex(Regex.Escape($"$({item.Name})"), True).Replace(strAlertText, regExGroupCollection(item.Name).Value)
                            Next
                        End If
                    End If

                    ParentForm.NotifyIcon.ShowBalloonTip(1, "Log Alert", strAlertText, ToolTipIcon)
                    Return True
                End If
            Next

            Return False
        End Function
    End Module
End Namespace