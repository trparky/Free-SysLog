using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Free_SysLog.SyslogParser
{
    public static class SyslogParser
    {
        private static Form1 ParentForm;
        private readonly static Regex rfc5424Regex = new Regex(@"<(?<priority>[0-9]+)>(?:\d ){0,1}(?<timestamp>[0-9]{4}[-.](?:1[0-2]|0[1-9])[-.](?:3[01]|[12][0-9]|0[1-9])T(?:2[0-3]|[01][0-9]):[0-5][0-9]:[0-5][0-9]\.[0-9]+Z)(?: -){0,1} (?<hostname>(?:\\.|[^\n\r ])+) (?:\d+ ){0,1}(?<appname>(?:\\.|[^\n\r:]+?)(?: \d*){0,1}):{0,1} (?:- - %% ){0,1}(?<message>.+?)(?=\s*<\d+>|$)", RegexOptions.Compiled); // PERFECT!
        private readonly static Regex rfc5424TransformRegex = new Regex(@"<(?<priority>[0-9]+)>(?<timestamp>(?:Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec) {1,2}[0-9]{1,2} [0-2][0-9]:[0-5][0-9]:[0-5][0-9]) (?<hostname>(?:\\.|[^\n\r ])+)(?: \:){0,1} (?<appname>(?:\\.|[^\n\r:]+)): (?<message>.+?)(?=\s*<\d+>|$)", RegexOptions.Compiled); // PERFECT!

        private readonly static Regex NumberRemovingRegex = new Regex(@"([A-Za-z-]*)\[[0-9]*\]", RegexOptions.Compiled);
        private readonly static Regex SyslogPreProcessor1 = new Regex(@"\d+ (<\d+>)", RegexOptions.Compiled);
        private readonly static Regex SyslogPreProcessor2 = new Regex(@"(<\d+>)", RegexOptions.Compiled);

        private const string strNewLine = "{newline}";

        private static NotificationLimiter.NotificationLimiter NotificationLimiter;

        public static Form1 SetParentForm
        {
            set
            {
                ParentForm = value;
                NotificationLimiter = new NotificationLimiter.NotificationLimiter();
            }
        }

        public static MyDataGridViewRow MakeLocalDataGridRowEntry(string strLogText, ref DataGridView dataGrid, string strLogType = "Informational, Local")
        {
            var MyDataGridViewRow = MakeDataGridRow(serverTimeStamp: DateTime.Now, dateObject: DateTime.Now, strTime: DateTime.Now.ToString(), strSourceAddress: IPAddress.Loopback.ToString(), strHostname: "Local", strRemoteProcess: null, strLog: strLogText, strLogType: strLogType, boolAlerted: false, strRawLogText: null, strAlertText: null, AlertType: AlertType.None, dataGrid: ref dataGrid);

            MyDataGridViewRow.DefaultCellStyle.Padding = new Padding(0, 2, 0, 2);
            return MyDataGridViewRow;
        }

        public static MyDataGridViewRow MakeDataGridRow(DateTime serverTimeStamp, DateTime dateObject, string strTime, string strSourceAddress, string strHostname, string strRemoteProcess, string strLog, string strLogType, bool boolAlerted, string strRawLogText, string strAlertText, AlertType AlertType, ref DataGridView dataGrid)
        {
            using (var MyDataGridViewRow = new MyDataGridViewRow())
            {
                MyDataGridViewRow.CreateCells(dataGrid);
                MyDataGridViewRow.Cells[SupportCode.SupportCode.ColumnIndex_ComputedTime].Value = strTime;
                MyDataGridViewRow.Cells[SupportCode.SupportCode.ColumnIndex_ComputedTime].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                MyDataGridViewRow.Cells[SupportCode.SupportCode.ColumnIndex_LogType].Value = string.IsNullOrWhiteSpace(strLogType) ? "" : strLogType;
                MyDataGridViewRow.Cells[SupportCode.SupportCode.ColumnIndex_IPAddress].Value = strSourceAddress;
                MyDataGridViewRow.Cells[SupportCode.SupportCode.ColumnIndex_RemoteProcess].Value = string.IsNullOrWhiteSpace(strRemoteProcess) ? "" : strRemoteProcess;
                MyDataGridViewRow.Cells[SupportCode.SupportCode.ColumnIndex_Hostname].Value = string.IsNullOrWhiteSpace(strHostname) ? "" : strHostname;
                MyDataGridViewRow.Cells[SupportCode.SupportCode.ColumnIndex_ServerTime].Value = SupportCode.SupportCode.ToIso8601Format(serverTimeStamp);
                MyDataGridViewRow.Cells[SupportCode.SupportCode.ColumnIndex_LogText].Value = strLog;
                MyDataGridViewRow.Cells[SupportCode.SupportCode.ColumnIndex_Alerted].Value = boolAlerted ? "Yes" : "No";
                MyDataGridViewRow.Cells[SupportCode.SupportCode.ColumnIndex_Alerted].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                MyDataGridViewRow.Cells[SupportCode.SupportCode.ColumnIndex_Alerted].Style.WrapMode = DataGridViewTriState.True;
                MyDataGridViewRow.DateObject = dateObject;
                MyDataGridViewRow.BoolAlerted = boolAlerted;
                MyDataGridViewRow.ServerDate = serverTimeStamp;
                MyDataGridViewRow.RawLogData = strRawLogText;
                MyDataGridViewRow.AlertText = strAlertText;
                MyDataGridViewRow.alertType = AlertType;

                if (My.MySettingsProperty.Settings.font is not null)
                {
                    MyDataGridViewRow.Cells[SupportCode.SupportCode.ColumnIndex_ComputedTime].Style.Font = My.MySettingsProperty.Settings.font;
                    MyDataGridViewRow.Cells[SupportCode.SupportCode.ColumnIndex_LogType].Style.Font = My.MySettingsProperty.Settings.font;
                    MyDataGridViewRow.Cells[SupportCode.SupportCode.ColumnIndex_IPAddress].Style.Font = My.MySettingsProperty.Settings.font;
                    MyDataGridViewRow.Cells[SupportCode.SupportCode.ColumnIndex_RemoteProcess].Style.Font = My.MySettingsProperty.Settings.font;
                    MyDataGridViewRow.Cells[SupportCode.SupportCode.ColumnIndex_Hostname].Style.Font = My.MySettingsProperty.Settings.font;
                    MyDataGridViewRow.Cells[SupportCode.SupportCode.ColumnIndex_LogText].Style.Font = My.MySettingsProperty.Settings.font;
                    MyDataGridViewRow.Cells[SupportCode.SupportCode.ColumnIndex_Alerted].Style.Font = My.MySettingsProperty.Settings.font;
                    MyDataGridViewRow.Cells[SupportCode.SupportCode.ColumnIndex_ServerTime].Style.Font = My.MySettingsProperty.Settings.font;
                }

                MyDataGridViewRow.DefaultCellStyle.Padding = new Padding(0, 2, 0, 2);

                return MyDataGridViewRow;
            }
        }

        public static void AddToLogList(string strTimeStampFromServer, string strSourceIP, string strLogText)
        {
            if (strLogText.CaseInsensitiveContains(strNewLine))
                strLogText = strLogText.Replace(strNewLine, Constants.vbCrLf).Trim();

            var currentDate = DateTime.Now.ToLocalTime();
            DateTime serverDate;

            if (string.IsNullOrWhiteSpace(strTimeStampFromServer))
            {
                serverDate = currentDate;
            }
            else
            {
                try
                {
                    serverDate = ParseTimestamp(strTimeStampFromServer);
                }
                catch (FormatException)
                {
                    serverDate = currentDate;
                    AddToLogList(null, "local", $"Unable to parse timestamp {SupportCode.SupportCode.strQuote}{strTimeStampFromServer.Trim()}{SupportCode.SupportCode.strQuote}.");
                }
            }

            ParentForm.Invoke(new Action(() =>
            {
                lock (ParentForm.dataGridLockObject)
                {
                    MyDataGridViewRow localMakeLocalDataGridRowEntry() { var argdataGrid = ParentForm.Logs; var ret = MakeLocalDataGridRowEntry(strLogText, ref argdataGrid); ParentForm.Logs = argdataGrid; return ret; }

                    ParentForm.Logs.Rows.Add(localMakeLocalDataGridRowEntry());
                    ParentForm.UpdateLogCount();
                    ParentForm.SelectLatestLogEntry();
                    ParentForm.BtnSaveLogsToDisk.Enabled = true;
                    if (ParentForm.intSortColumnIndex == 0 & ParentForm.sortOrder == SortOrder.Descending)
                        ParentForm.SortLogsByDateObjectNoLocking(ParentForm.intSortColumnIndex, SortOrder.Descending);
                }

                ParentForm.NotifyIcon.Text = $"Free SysLog{Constants.vbCrLf}Last log received at {currentDate}.";
                ParentForm.UpdateLogCount();
                ParentForm.BtnSaveLogsToDisk.Enabled = true;

                if (ParentForm.ChkEnableAutoScroll.Checked & ParentForm.Logs.Rows.Count > 0 & ParentForm.intSortColumnIndex == 0)
                {
                    try
                    {
                        SupportCode.SupportCode.boolIsProgrammaticScroll = true;
                        ParentForm.Logs.FirstDisplayedScrollingRowIndex = ParentForm.sortOrder == SortOrder.Ascending ? ParentForm.Logs.Rows.Count - 1 : 0;
                    }
                    finally
                    {
                        SupportCode.SupportCode.boolIsProgrammaticScroll = false;
                    }
                }
            }));
        }

        private static (string Facility, string Severity) GetSeverityAndFacility(string strPriority)
        {
            strPriority = strPriority.Replace("<", "").Replace(">", "").Trim();

            int priorityNumber;

            if (int.TryParse(strPriority, out priorityNumber))
            {
                // Define the severity levels as per RFC 5424
                string[] severityLevels = new string[] { "Emergency", "Alert", "Critical", "Error", "Warning", "Notice", "Informational", "Debug" };

                // Define the facility levels as per RFC 5424
                string[] facilityLevels = new string[] { "Kernel messages", "User-level messages", "Mail system", "System daemons", "Security/authorization messages", "Messages generated internally by syslogd", "Line printer subsystem", "Network news subsystem", "UUCP subsystem", "Clock daemon", "Security/authorization messages", "FTP daemon", "NTP subsystem", "Log audit", "Log alert", "Clock daemon", "Local use 0", "Local use 1", "Local use 2", "Local use 3", "Local use 4", "Local use 5", "Local use 6", "Local use 7" };

                // Calculate facility and severity
                int facility = priorityNumber / 8;
                int severity = priorityNumber % 8;

                // Get facility and severity descriptions
                string facilityDescription = facility >= 0 & facility < facilityLevels.Length ? facilityLevels[facility] : "Unknown Facility";
                string severityDescription = severity >= 0 & severity < severityLevels.Length ? severityLevels[severity] : "Unknown Severity";

                return (facilityDescription, severityDescription);
            }
            else
            {
                return default;
            }
        }

        /// <summary>Parses a date timestamp in String format to a Date Object.</summary>
        /// <param name="timestamp">A date timestamp in String format.</param>
        /// <returns>A Date Object.</returns>
        /// <exception cref="FormatException">Throws a FormatException if the function can't parse the input.</exception>
        public static DateTime ParseTimestamp(string timestamp)
        {
            DateTime parsedDate;
            var userCulture = System.Globalization.CultureInfo.CurrentCulture;
            bool isEuropeanDateFormat = userCulture.DateTimeFormat.ShortDatePattern.StartsWith("d");

            if (timestamp.EndsWith("Z"))
            {
                // RFC 3339/ISO 8601 format with UTC timezone and optional milliseconds
                if (timestamp.Contains(":") && timestamp.Count(c => Conversions.ToString(c) == ":") == 3)
                {
                    // Handle timestamp with extra colon before milliseconds (e.g., "yyyy-MM-ddTHH:mm:ss:fffZ")
                    timestamp = timestamp.Remove(timestamp.LastIndexOf(":"), 1).Insert(timestamp.LastIndexOf(":"), ".");
                }

                if (timestamp.Contains("."))
                {
                    // Handle timestamp with milliseconds
                    parsedDate = DateTime.ParseExact(timestamp, "yyyy-MM-ddTHH:mm:ss.fffZ", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AdjustToUniversal);
                }
                else
                {
                    // Handle timestamp without milliseconds
                    parsedDate = DateTime.ParseExact(timestamp, "yyyy-MM-ddTHH:mm:ssZ", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AdjustToUniversal);
                }
            }
            else if (timestamp.Contains("+") || timestamp.Contains("-"))
            {
                DateTimeOffset parsedDateOffset;

                // RFC 3339/ISO 8601 format with timezone offset and optional milliseconds
                if (timestamp.Contains("."))
                {
                    // Handle timestamp with milliseconds
                    parsedDateOffset = DateTimeOffset.ParseExact(timestamp, "yyyy-MM-ddTHH:mm:ss.fffzzz", System.Globalization.CultureInfo.InvariantCulture);
                    parsedDate = parsedDateOffset.DateTime;
                }
                else
                {
                    // Handle timestamp without milliseconds
                    parsedDateOffset = DateTimeOffset.ParseExact(timestamp, "yyyy-MM-ddTHH:mm:sszzz", System.Globalization.CultureInfo.InvariantCulture);
                    parsedDate = parsedDateOffset.DateTime;
                }
            }
            else if (timestamp.Length >= 15 && char.IsLetter(timestamp[0]))
            {
                // "MMM dd HH:mm:ss" format (like "Sep  4 22:39:12")
                timestamp = timestamp.Replace("  ", " 0"); // Handle single-digit day (e.g., "Sep  4" becomes "Sep 04")
                parsedDate = DateTime.ParseExact(timestamp, "MMM dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

                // If you need to add the current year to the date:
                parsedDate = parsedDate.AddYears(DateTime.Now.Year - parsedDate.Year);
            }
            else if (timestamp.Contains("/"))
            {
                if (timestamp.EndsWith("PM", StringComparison.OrdinalIgnoreCase) | timestamp.EndsWith("AM", StringComparison.OrdinalIgnoreCase))
                {
                    // Handle both American and European formats based on localization
                    if (isEuropeanDateFormat)
                    {
                        // European format "d/M/yyyy HH:mm:ss"
                        parsedDate = DateTime.ParseExact(timestamp, "d/M/yyyy H:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        // American format "M/d/yyyy h:mm:ss tt"
                        parsedDate = DateTime.ParseExact(timestamp, "M/d/yyyy h:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture);
                    }
                }
                // Handle both American and European formats based on localization
                else if (isEuropeanDateFormat)
                {
                    // European format "d/M/yyyy HH:mm:ss"
                    parsedDate = DateTime.ParseExact(timestamp, "d/M/yyyy H:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                }
                else
                {
                    // American format "M/d/yyyy h:mm:ss"
                    parsedDate = DateTime.ParseExact(timestamp, "M/d/yyyy h:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            else
            {
                throw new FormatException("Unknown timestamp format.");
            }

            return parsedDate;
        }

        public static string ConvertLineFeeds(string strInput)
        {
            strInput = strInput.Replace(Constants.vbCrLf, Constants.vbLf); // Temporarily replace all CRLF with LF
            strInput = strInput.Replace(Constants.vbCr, Constants.vbLf);   // Convert standalone CR to LF
            strInput = strInput.Replace(Constants.vbLf, Constants.vbCrLf); // Finally, replace all LF with CRLF
            return strInput.Trim();
        }

        private static bool IsRegexMatch(Regex regex, string strRawLogText, ref Match match)
        {
            match = regex.Match(strRawLogText);
            return match.Success;
        }

        private static bool TrySplitLogEntries(Regex regex, string input, ref string[] matches)
        {
            Match argmatch = null;
            if (IsRegexMatch(regex, input, ref argmatch))
            {
                matches = regex.Split(input);
                return true;
            }

            return false;
        }

        public static void ProcessIncomingLog(string strRawLogText, string strSourceIP)
        {
            if (!string.IsNullOrWhiteSpace(strRawLogText) && !string.IsNullOrWhiteSpace(strSourceIP))
            {
                // Convert all linefeeds.
                strRawLogText = ConvertLineFeeds(strRawLogText);

                // Do some pre-processing so that we can separate out the data more easily.
                strRawLogText = strRawLogText.Replace(Constants.vbCrLf, strNewLine).Replace(Constants.vbLf, strNewLine);
                strRawLogText = SyslogPreProcessor1.Replace(strRawLogText, "$1");
                strRawLogText = SyslogPreProcessor2.Replace(strRawLogText, Constants.vbCrLf + "$1");

                // Split off each syslog entry by using vbCrLf as a delimiter.
                string[] matches = strRawLogText.Split(Conversions.ToChar(Constants.vbCrLf));

                // Check to see if we have anything to work with before we go into the loop.
                if (matches.Any())
                {
                    // Now work with each syslog entry.
                    foreach (string strMatch in matches)
                    {
                        if (!string.IsNullOrWhiteSpace(strMatch))
                        {
                            ProcessIncomingLog_SubRoutine(strMatch, strSourceIP);
                        }
                    }
                }
                else
                {
                    // Nope, log it as unable to be parsed.
                    AddToLogList(null, "local", $"Unable to parse log {SupportCode.SupportCode.strQuote}{strRawLogText}{SupportCode.SupportCode.strQuote}.");
                }
            }
        }

        public static void ProcessIncomingLog_SubRoutine(string strRawLogText, string strSourceIP)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(strRawLogText) && !string.IsNullOrWhiteSpace(strSourceIP))
                {
                    bool boolIgnored = false;
                    bool boolAlerted = false;
                    (string Facility, string Severity) priorityObject = default;
                    string priority = null;
                    string message = null;
                    string timestamp = null;
                    string hostname = null;
                    string customHostname = null;
                    string appName = null;
                    string strAlertText = null;
                    var AlertType = Free_SysLog.AlertType.None;

                    // Step 1: Use Regex to extract the RFC 5424 header and the message
                    Match match = null;

                    if (IsRegexMatch(rfc5424TransformRegex, strRawLogText, ref match) || IsRegexMatch(rfc5424Regex, strRawLogText, ref match))
                    {
                        // Handling the transformation to RFC 5424 format
                        priority = string.IsNullOrWhiteSpace(match.Groups["priority"].Value) ? "" : match.Groups["priority"].Value;
                        timestamp = string.IsNullOrWhiteSpace(match.Groups["timestamp"].Value) ? "" : match.Groups["timestamp"].Value;
                        hostname = string.IsNullOrWhiteSpace(match.Groups["hostname"].Value) ? "" : match.Groups["hostname"].Value;
                        appName = string.IsNullOrWhiteSpace(match.Groups["appname"].Value) ? "" : match.Groups["appname"].Value;
                        message = string.IsNullOrWhiteSpace(match.Groups["message"].Value) ? "" : match.Groups["message"].Value;

                        if (SupportCode.SupportCode.hostnames.TryGetValue(strSourceIP, out customHostname))
                            hostname = customHostname;

                        priorityObject = GetSeverityAndFacility(priority);
                    }
                    else
                    {
                        timestamp = DateTime.Now.ToString();
                        hostname = "";
                        appName = "";
                        priorityObject = ("Local", "Error");
                        message = $"An error occured while attempting to parse the log entry. Below is the log entry that failed...{Constants.vbCrLf}{strRawLogText}";
                    } // Something went wrong, we couldn't parse the entry so we're going to just pass the raw log entry to the program.

                    if (!string.IsNullOrWhiteSpace(message) && message.CaseInsensitiveContains(strNewLine))
                        message = message.Replace(strNewLine, Constants.vbCrLf).Trim();

                    if (My.MySettingsProperty.Settings.RemoveNumbersFromRemoteApp)
                        appName = NumberRemovingRegex.Replace(appName, "$1");

                    // Step 3: Handle the ignored logs and alerts
                    if (SupportCode.SupportCode.ignoredList is not null && SupportCode.SupportCode.ignoredList.Any())
                        boolIgnored = ProcessIgnoredLogPreferences(message);
                    if (SupportCode.SupportCode.replacementsList is not null && SupportCode.SupportCode.replacementsList.Any())
                        message = ProcessReplacements(message);
                    if (SupportCode.SupportCode.alertsList is not null && SupportCode.SupportCode.alertsList.Any())
                        boolAlerted = ProcessAlerts(message, ref strAlertText, DateTime.Now.ToString(), strSourceIP, strRawLogText, ref AlertType);

                    // Step 4: Add to log list, separating header and message
                    AddToLogList(timestamp, strSourceIP, hostname, appName, message, boolIgnored, boolAlerted, priorityObject, strRawLogText, strAlertText, AlertType);
                }
            }
            catch (Exception ex)
            {
                AddToLogList(null, "local", $"{ex.Message} -- {ex.StackTrace}{Constants.vbCrLf}Data from Server: {strRawLogText}");
            }
        }

        private static bool ProcessIgnoredLogPreferences(string message)
        {
            foreach (IgnoredClass ignoredClassInstance in SupportCode.SupportCode.ignoredList)
            {
                if (GetCachedRegex(ignoredClassInstance.BoolRegex ? ignoredClassInstance.StrIgnore : $".*{Regex.Escape(ignoredClassInstance.StrIgnore)}.*", ignoredClassInstance.BoolCaseSensitive).IsMatch(message))
                {
                    ParentForm.Invoke(new Action(() =>
                    {
                        ParentForm.longNumberOfIgnoredLogs += 1L;
                        if (!ParentForm.ChkEnableRecordingOfIgnoredLogs.Checked)
                        {
                            ParentForm.ZerooutIgnoredLogsCounterToolStripMenuItem.Enabled = true;
                        }
                        ParentForm.LblNumberOfIgnoredIncomingLogs.Text = $"Number of ignored incoming logs: {ParentForm.longNumberOfIgnoredLogs:N0}";
                    }));
                    return true;
                }
            }

            return false;
        }

        private static void AddToLogList(string strTimeStampFromServer, string strSourceIP, string strHostname, string strRemoteProcess, string strLogText, bool boolIgnored, bool boolAlerted, (string Facility, string Severity) priority, string strRawLogText, string strAlertText, AlertType alertType)
        {
            var currentDate = DateTime.Now.ToLocalTime();
            DateTime serverDate;

            if (string.IsNullOrWhiteSpace(strTimeStampFromServer))
            {
                serverDate = currentDate;
            }
            else
            {
                try
                {
                    serverDate = ParseTimestamp(strTimeStampFromServer);
                }
                catch (FormatException)
                {
                    serverDate = currentDate;
                    AddToLogList(null, "local", $"Unable to parse timestamp {SupportCode.SupportCode.strQuote}{strTimeStampFromServer.Trim()}{SupportCode.SupportCode.strQuote}.");
                }
            }

            if (!boolIgnored)
            {
                ParentForm.Invoke(new Action(() =>
                {
                    lock (ParentForm.dataGridLockObject)
                    {
                        MyDataGridViewRow localMakeDataGridRow() { var argdataGrid1 = ParentForm.Logs; var ret = MakeDataGridRow(serverTimeStamp: serverDate, dateObject: currentDate, strTime: currentDate.ToString(), strSourceAddress: strSourceIP, strHostname: strHostname, strRemoteProcess: strRemoteProcess, strLog: strLogText, strLogType: $"{priority.Severity}, {priority.Facility}", boolAlerted: boolAlerted, strRawLogText: strRawLogText.Trim(), strAlertText: strAlertText, AlertType: alertType, dataGrid: ref argdataGrid1); ParentForm.Logs = argdataGrid1; return ret; }
                        ParentForm.Logs.Rows.Add(localMakeDataGridRow());
                        if (ParentForm.intSortColumnIndex == 0 & ParentForm.sortOrder == SortOrder.Descending)
                            ParentForm.SortLogsByDateObjectNoLocking(ParentForm.intSortColumnIndex, SortOrder.Descending);
                    }

                    ParentForm.NotifyIcon.Text = $"Free SysLog{Constants.vbCrLf}Last log received at {currentDate}.";
                    ParentForm.UpdateLogCount();
                    ParentForm.BtnSaveLogsToDisk.Enabled = true;

                    ParentForm.SelectLatestLogEntry();
                }));
            }
            else if (boolIgnored & ParentForm.ChkEnableRecordingOfIgnoredLogs.Checked)
            {
                lock (ParentForm.IgnoredLogsLockObject)
                {
                    var argdataGrid = ParentForm.Logs;
                    var NewIgnoredItem = MakeDataGridRow(serverTimeStamp: serverDate, dateObject: currentDate, strTime: currentDate.ToString(), strSourceAddress: strSourceIP, strHostname: strHostname, strRemoteProcess: strRemoteProcess, strLog: strLogText, strLogType: $"{priority.Severity}, {priority.Facility}", boolAlerted: boolAlerted, strRawLogText: strRawLogText.Trim(), strAlertText: strAlertText, AlertType: alertType, dataGrid: ref argdataGrid);
                    ParentForm.Logs = argdataGrid;
                    ParentForm.IgnoredLogs.Add(NewIgnoredItem);
                    if (SupportCode.SupportCode.IgnoredLogsAndSearchResultsInstance is not null)
                        SupportCode.SupportCode.IgnoredLogsAndSearchResultsInstance.AddIgnoredDatagrid(NewIgnoredItem, ParentForm.ChkEnableAutoScroll.Checked);
                    ParentForm.Invoke(new Action(() => ParentForm.ClearIgnoredLogsToolStripMenuItem.Enabled = true));
                }
            }
        }

        private static string ProcessReplacements(string input)
        {
            foreach (ReplacementsClass item in SupportCode.SupportCode.replacementsList)
            {
                try
                {
                    input = GetCachedRegex(item.BoolRegex ? item.StrReplace : Regex.Escape(item.StrReplace), item.BoolCaseSensitive).Replace(input, item.StrReplaceWith);
                }
                catch (Exception)
                {
                }
            }

            return input;
        }

        private static Regex GetCachedRegex(string pattern, bool boolCaseSensitive = true)
        {
            if (!ParentForm.regexCache.ContainsKey(pattern))
                ParentForm.regexCache[pattern] = new Regex(pattern, boolCaseSensitive ? RegexOptions.Compiled : RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return ParentForm.regexCache[pattern];
        }

        private static bool ProcessAlerts(string strLogText, ref string strOutgoingAlertText, string strLogDate, string strSourceIP, string strRawLogText, ref AlertType alertTypeAsAlertType)
        {
            var ToolTipIcon = System.Windows.Forms.ToolTipIcon.None;
            Regex RegExObject;
            string strAlertText;
            GroupCollection regExGroupCollection;

            foreach (AlertsClass alert in SupportCode.SupportCode.alertsList)
            {
                RegExObject = GetCachedRegex(alert.BoolRegex ? alert.StrLogText : Regex.Escape(alert.StrLogText), alert.BoolCaseSensitive);

                if (RegExObject.IsMatch(strLogText))
                {
                    if (alert.alertType == AlertType.Warning)
                    {
                        ToolTipIcon = System.Windows.Forms.ToolTipIcon.Warning;
                        alertTypeAsAlertType = AlertType.Warning;
                    }
                    else if (alert.alertType == AlertType.ErrorMsg)
                    {
                        ToolTipIcon = System.Windows.Forms.ToolTipIcon.Error;
                        alertTypeAsAlertType = AlertType.ErrorMsg;
                    }
                    else if (alert.alertType == AlertType.Info)
                    {
                        ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
                        alertTypeAsAlertType = AlertType.Info;
                    }

                    strAlertText = string.IsNullOrWhiteSpace(alert.StrAlertText) ? strLogText : alert.StrAlertText;

                    if (alert.BoolRegex & !string.IsNullOrWhiteSpace(alert.StrAlertText))
                    {
                        regExGroupCollection = RegExObject.Match(strLogText).Groups;

                        if (regExGroupCollection.Count > 0)
                        {
                            for (int index = 0, loopTo = regExGroupCollection.Count - 1; index <= loopTo; index++)
                                strAlertText = GetCachedRegex(Regex.Escape($"${index}"), false).Replace(strAlertText, regExGroupCollection[index].Value);

                            foreach (Group item in regExGroupCollection)
                                strAlertText = GetCachedRegex(Regex.Escape($"$({item.Name})"), true).Replace(strAlertText, regExGroupCollection[item.Name].Value);
                        }
                    }

                    if (alert.BoolLimited)
                    {
                        NotificationLimiter.ShowNotification(strAlertText, ToolTipIcon, strLogText, strLogDate, strSourceIP, strRawLogText);
                    }
                    else
                    {
                        SupportCode.SupportCode.ShowToastNotification(strAlertText, ToolTipIcon, strLogText, strLogDate, strSourceIP, strRawLogText);
                    }

                    strOutgoingAlertText = strAlertText;
                    return true;
                }
            }

            return false;
        }
    }
}