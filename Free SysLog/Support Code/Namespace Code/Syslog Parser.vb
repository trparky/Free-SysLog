Imports System.Text.RegularExpressions
Imports Free_SysLog.SupportCode

Namespace SyslogParser
    Public Module SyslogParser
        Private ParentForm As Form1

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
                serverDate = ParseTimestamp(strTimeStampFromServer)
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

        Private Function ParseTimestamp(timestamp As String) As Date
            Dim parsedDate As Date

            If timestamp.EndsWith("Z") Then
                ' RFC 3339/ISO 8601 format with UTC timezone ("yyyy-MM-ddTHH:mm:ssZ")
                parsedDate = Date.ParseExact(timestamp, "yyyy-MM-ddTHH:mm:ssZ", Globalization.CultureInfo.InvariantCulture, Globalization.DateTimeStyles.AdjustToUniversal)
            ElseIf timestamp.Contains("+") OrElse timestamp.Contains("-") Then
                ' RFC 3339/ISO 8601 format with timezone offset ("yyyy-MM-ddTHH:mm:sszzz")
                Dim parsedDateOffset As DateTimeOffset = DateTimeOffset.ParseExact(timestamp, "yyyy-MM-ddTHH:mm:sszzz", Globalization.CultureInfo.InvariantCulture)
                parsedDate = parsedDateOffset.DateTime
            ElseIf timestamp.Length >= 15 AndAlso Char.IsLetter(timestamp(0)) Then
                ' "MMM dd HH:mm:ss" format (like "Sep  4 22:39:12")
                timestamp = timestamp.Replace("  ", " 0") ' Handle single-digit day (e.g., "Sep  4" becomes "Sep 04")
                parsedDate = Date.ParseExact(timestamp, "MMM dd HH:mm:ss", Globalization.CultureInfo.InvariantCulture)

                ' If you need to add the current year to the date:
                parsedDate = parsedDate.AddYears(Date.Now.Year - parsedDate.Year)
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

        Public Sub ProcessIncomingLog(strRawLogText As String, strSourceIP As String)
            Try
                If Not String.IsNullOrWhiteSpace(strRawLogText) AndAlso Not String.IsNullOrWhiteSpace(strSourceIP) Then
                    Dim boolIgnored As Boolean = False
                    Dim header As String = String.Empty
                    Dim priorityObject As (Facility As String, Severity As String) = Nothing
                    Dim version, procId, structuredData, msgId, priority As String
                    Dim message As String = Nothing
                    Dim timestamp As String = Nothing
                    Dim hostname As String = Nothing
                    Dim appName As String = Nothing

                    ' Step 1: Use Regex to extract the RFC 5424 header and the message

                    Dim rfc5424TransformRegexMatch As Match = ParentForm.rfc5424TransformRegex.Match(strRawLogText)

                    If rfc5424TransformRegexMatch.Success Then
                        ' Handling the transformation to RFC 5424 format
                        priority = If(String.IsNullOrWhiteSpace(rfc5424TransformRegexMatch.Groups(1).Value), "", rfc5424TransformRegexMatch.Groups(1).Value)
                        timestamp = If(String.IsNullOrWhiteSpace(rfc5424TransformRegexMatch.Groups(2).Value), "", rfc5424TransformRegexMatch.Groups(2).Value)
                        hostname = If(String.IsNullOrWhiteSpace(rfc5424TransformRegexMatch.Groups(3).Value), "", rfc5424TransformRegexMatch.Groups(3).Value)
                        appName = If(String.IsNullOrWhiteSpace(rfc5424TransformRegexMatch.Groups(4).Value), "", rfc5424TransformRegexMatch.Groups(4).Value)
                        message = If(String.IsNullOrWhiteSpace(rfc5424TransformRegexMatch.Groups(5).Value), "", rfc5424TransformRegexMatch.Groups(5).Value)

                        priorityObject = GetSeverityAndFacility(priority)

                        ' Transform to RFC 5424 format (Version 1 assumed)
                        'finalMessage = $"{priority}1 {timestamp} {hostname} {appName} - - {message}"
                    Else
                        ' Match against RFC 5424 formatted logs
                        Dim rfc5424RegexMatch As Match = ParentForm.rfc5424Regex.Match(strRawLogText)

                        If rfc5424RegexMatch.Success Then
                            ' Use the correct match object
                            priority = If(String.IsNullOrWhiteSpace(rfc5424RegexMatch.Groups(1).Value), "", rfc5424RegexMatch.Groups(1).Value)
                            version = If(String.IsNullOrWhiteSpace(rfc5424RegexMatch.Groups(2).Value), "", rfc5424RegexMatch.Groups(2).Value)
                            timestamp = If(String.IsNullOrWhiteSpace(rfc5424RegexMatch.Groups(3).Value), "", rfc5424RegexMatch.Groups(3).Value)
                            hostname = If(String.IsNullOrWhiteSpace(rfc5424RegexMatch.Groups(4).Value), "", rfc5424RegexMatch.Groups(4).Value)
                            appName = If(String.IsNullOrWhiteSpace(rfc5424RegexMatch.Groups(5).Value), "", rfc5424RegexMatch.Groups(5).Value)
                            procId = If(String.IsNullOrWhiteSpace(rfc5424RegexMatch.Groups(6).Value), "", rfc5424RegexMatch.Groups(6).Value)
                            msgId = If(String.IsNullOrWhiteSpace(rfc5424RegexMatch.Groups(7).Value), "", rfc5424RegexMatch.Groups(7).Value)
                            structuredData = If(String.IsNullOrWhiteSpace(rfc5424RegexMatch.Groups(8).Value), "", rfc5424RegexMatch.Groups(8).Value)

                            priorityObject = GetSeverityAndFacility(priority)

                            ' Reconstruct the header using the correct match data
                            header = $"{priority}{version} {timestamp} {hostname} {appName} {procId} {msgId} {structuredData}".Trim()
                            'finalMessage = If(String.IsNullOrWhitespace(rfc5424RegexMatch.Groups(9).Value), "", rfc5424RegexMatch.Groups(9).Value).Trim() ' Remaining part is the message
                        Else
                            ' If it doesn't match, treat the whole log as the message
                            'finalMessage = strLogText.Trim()
                        End If
                    End If

                    ' Step 2: Process the log message (previous processing logic)
                    message = ConvertLineFeeds(message)
                    strRawLogText = ConvertLineFeeds(strRawLogText)

                    ' Step 3: Handle the ignored logs and alerts
                    If ignoredList IsNot Nothing AndAlso ignoredList.Count > 0 Then
                        For Each ignoredClassInstance As IgnoredClass In ignoredList
                            If GetCachedRegex(If(ignoredClassInstance.BoolRegex, ignoredClassInstance.StrIgnore, $".*{Regex.Escape(ignoredClassInstance.StrIgnore)}.*"), ignoredClassInstance.BoolCaseSensitive).IsMatch(message) Then
                                ParentForm.Invoke(Sub()
                                                      ParentForm.longNumberOfIgnoredLogs += 1
                                                      If Not ParentForm.ChkEnableRecordingOfIgnoredLogs.Checked Then
                                                          ParentForm.ZerooutIgnoredLogsCounterToolStripMenuItem.Enabled = True
                                                      End If
                                                      ParentForm.LblNumberOfIgnoredIncomingLogs.Text = $"Number of ignored incoming logs: {ParentForm.longNumberOfIgnoredLogs:N0}"
                                                  End Sub)
                                boolIgnored = True
                                Exit For
                            End If
                        Next
                    End If

                    Dim boolAlerted As Boolean = False
                    If replacementsList IsNot Nothing AndAlso replacementsList.Count > 0 Then message = ProcessReplacements(message)
                    If alertsList IsNot Nothing AndAlso alertsList.Count > 0 Then boolAlerted = ProcessAlerts(message)

                    ' Step 4: Add to log list, separating header and message
                    AddToLogList(timestamp, strSourceIP, hostname, appName, message, boolIgnored, boolAlerted, priorityObject, strRawLogText)
                End If
            Catch ex As Exception
                AddToLogList(Nothing, "local", $"{ex.Message} -- {ex.StackTrace}{vbCrLf}Data from Server: {strRawLogText}")
            End Try
        End Sub

        Private Sub AddToLogList(strTimeStampFromServer As String, strSourceIP As String, strHostname As String, strRemoteProcess As String, strLogText As String, boolIgnored As Boolean, boolAlerted As Boolean, priority As (Facility As String, Severity As String), strRawLogText As String)
            Dim currentDate As Date = Now.ToLocalTime
            Dim serverDate As Date

            If String.IsNullOrWhiteSpace(strTimeStampFromServer) Then
                serverDate = currentDate
            Else
                serverDate = ParseTimestamp(strTimeStampFromServer)
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

                                      If ParentForm.ChkEnableAutoScroll.Checked And ParentForm.Logs.Rows.Count > 0 And ParentForm.intSortColumnIndex = 0 Then
                                          ParentForm.Logs.FirstDisplayedScrollingRowIndex = If(ParentForm.sortOrder = SortOrder.Ascending, ParentForm.Logs.Rows.Count - 1, 0)
                                      End If
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