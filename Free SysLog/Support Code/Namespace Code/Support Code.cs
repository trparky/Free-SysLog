using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.VisualBasic;

namespace Free_SysLog.SupportCode
{
    public enum IgnoreOrSearchWindowDisplayMode : byte
    {
        ignored,
        search,
        viewer
    }

    static class SupportCode
    {
        public static bool boolIsProgrammaticScroll = false;
        public static IgnoredLogsAndSearchResults IgnoredLogsAndSearchResultsInstance = null;
        public static List<ReplacementsClass> replacementsList = new List<ReplacementsClass>();
        public static List<IgnoredClass> ignoredList = new List<IgnoredClass>();
        public static List<AlertsClass> alertsList = new List<AlertsClass>();
        public static List<SysLogProxyServer> serversList = new List<SysLogProxyServer>();
        public static Dictionary<string, string> hostnames = new Dictionary<string, string>((int)StringComparison.OrdinalIgnoreCase);
        public const string strMutexName = "Free SysLog Server";
        public static System.Threading.Mutex mutex;
        public static string strEXEPath = Process.GetCurrentProcess().MainModule.FileName;
        public static bool boolDoWeOwnTheMutex = false;
        public static Newtonsoft.Json.JsonSerializerSettings JSONDecoderSettingsForLogFiles = new Newtonsoft.Json.JsonSerializerSettings() { MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore };
        public static Newtonsoft.Json.JsonSerializerSettings JSONDecoderSettingsForSettingsFiles = new Newtonsoft.Json.JsonSerializerSettings() { MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Error };
        public static string strPathToDataFolder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Free SysLog");
        public static string strPathToDataBackupFolder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Free SysLog", "Backup");
        public static string strPathToDataFile = System.IO.Path.Combine(strPathToDataFolder, "log.json");
        public const string strProxiedString = "proxied|";
        public const string strQuote = "\"";
        public const string strViewLog = "viewlog";
        public const string strOpenSysLog = "opensyslog";
        public const string strRestore = "restore";
        public const string strTerminate = "terminate";

        public const int ColumnIndex_ComputedTime = 0;
        public const int ColumnIndex_ServerTime = 1;
        public const int ColumnIndex_LogType = 2;
        public const int ColumnIndex_IPAddress = 3;
        public const int ColumnIndex_Hostname = 4;
        public const int ColumnIndex_RemoteProcess = 5;
        public const int ColumnIndex_LogText = 6;
        public const int ColumnIndex_Alerted = 7;
        public const int ColumnIndex_FileName = 8;

        public const string strUpdaterEXE = "updater.exe";
        public const string strUpdaterPDB = "updater.pdb";

#if DEBUG
        public const bool boolDebugBuild = true;
#else
        public const bool boolDebugBuild = false;
#endif

        public static System.Collections.Specialized.StringCollection SaveColumnOrders(DataGridViewColumnCollection columns)
        {
            try
            {
                var SpecializedStringCollection = new System.Collections.Specialized.StringCollection();

                foreach (DataGridViewTextBoxColumn column in columns)
                    SpecializedStringCollection.Add(Newtonsoft.Json.JsonConvert.SerializeObject(new ColumnOrder() { ColumnName = column.Name, ColumnIndex = column.DisplayIndex }));

                return SpecializedStringCollection;
            }
            catch (Exception)
            {
                return new System.Collections.Specialized.StringCollection();
            }
        }

        public static void ShowToastNotification(string tipText, ToolTipIcon tipIcon, string strLogText, string strLogDate, string strSourceIP, string strRawLogText)
        {
            string strIconPath = null;
            var notification = new ToastContentBuilder();

            notification.AddText(tipText);
            notification.SetToastDuration(My.MySettingsProperty.Settings.NotificationLength == 0 ? ToastDuration.Short : ToastDuration.Long);

            if (tipIcon == ToolTipIcon.Error)
            {
                strIconPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "error.png");
            }
            else if (tipIcon == ToolTipIcon.Warning)
            {
                strIconPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "warning.png");
            }
            else if (tipIcon == ToolTipIcon.Info)
            {
                strIconPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "info.png");
            }

            if (My.MySettingsProperty.Settings.IncludeButtonsOnNotifications)
            {
                string strNotificationPacket = Newtonsoft.Json.JsonConvert.SerializeObject(new NotificationDataPacket() { alerttext = tipText, logdate = strLogDate, logtext = strLogText, sourceip = strSourceIP, rawlogtext = strRawLogText });

                notification.AddButton(new ToastButton().SetContent("View Log").AddArgument("action", strViewLog).AddArgument("datapacket", strNotificationPacket));
                notification.AddButton(new ToastButton().SetContent("Open SysLog").AddArgument("action", strOpenSysLog));
            }
            else
            {
                notification.AddArgument("action", strOpenSysLog);
            }

            if (!string.IsNullOrWhiteSpace(strIconPath) && System.IO.File.Exists(strIconPath))
                notification.AddAppLogoOverride(new Uri(strIconPath), ToastGenericAppLogoCrop.Circle);

            notification.Show();
        }

        public static void LoadColumnOrders(ref DataGridViewColumnCollection columns, ref System.Collections.Specialized.StringCollection specializedStringCollection)
        {
            try
            {
                ColumnOrder columnOrder;
                var jsonSerializerSettings = new Newtonsoft.Json.JsonSerializerSettings() { MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Error };

                if (specializedStringCollection is not null && specializedStringCollection.Count != 0)
                {
                    try
                    {
                        foreach (string item in specializedStringCollection)
                        {
                            columnOrder = Newtonsoft.Json.JsonConvert.DeserializeObject<ColumnOrder>(item, jsonSerializerSettings);
                            columns[columnOrder.ColumnName].DisplayIndex = columnOrder.ColumnIndex;
                        }
                    }
                    catch (Newtonsoft.Json.JsonSerializationException)
                    {
                        specializedStringCollection = null;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        public static string ConvertListOfStringsToString(List<string> input, bool boolUseOnlyOneLine = false)
        {
            if (input.Count == 1)
                return input[0];

            if (boolUseOnlyOneLine || input.Count >= 6)
            {
                if (input.Count == 2)
                {
                    return $"{input[0]} and {input[1]}";
                }
                else
                {
                    return $"{string.Join(", ", input.Take(input.Count - 1))}, and {input.Last()}";
                }
            }
            else
            {
                // For lists with more than 6 items, and if not using only one line
                var stringBuilder = new StringBuilder();

                foreach (string item in input)
                    stringBuilder.AppendLine(item);

                return stringBuilder.ToString().Trim();
            }
        }

        public static bool CopyTextToWindowsClipboard(string strTextToBeCopiedToClipboard, string strErrorMessageTitle)
        {
            try
            {
                Clipboard.SetDataObject(strTextToBeCopiedToClipboard, true, 5, 200);
                return true;
            }
            catch (Exception)
            {
                Interaction.MsgBox("Unable to open Windows Clipboard to copy text to it.", MsgBoxStyle.Critical, strErrorMessageTitle);
                return false;
            }
        }

        public static string ToIso8601Format(DateTime dateTime)
        {
            // Ensure the DateTime is in UTC
            var utcDateTime = dateTime.ToUniversalTime();

            // Convert to ISO 8601 format with UTC time zone designator (Z)
            return utcDateTime.ToString("yyyy-MM-ddTHH:mm:ss:fffZ", System.Globalization.CultureInfo.InvariantCulture);
        }

        public static IPAddress GetIPv4Address(IPAddress ipv6Address)
        {
            if (ipv6Address.AddressFamily == AddressFamily.InterNetworkV6 && ipv6Address.IsIPv4MappedToIPv6)
                return ipv6Address.MapToIPv4();
            return ipv6Address;
        }

        public static Color GetGoodTextColorBasedUponBackgroundColor(Color input)
        {
            short intCombinedTotal = (short)(int.Parse(input.R.ToString()) + int.Parse(input.G.ToString()) + int.Parse(input.B.ToString()));
            return intCombinedTotal / 3d < 128d ? Color.White : Color.Black;
        }

        public static string FileSizeToHumanSize(long size, bool roundToNearestWholeNumber = false)
        {
            string result;
            short shortRoundNumber = (short)(roundToNearestWholeNumber ? 0 : 2);

            if (size <= Math.Pow(2d, 10d))
            {
                result = $"{size} Bytes";
            }
            else if (size > Math.Pow(2d, 10d) & size <= Math.Pow(2d, 20d))
            {
                result = $"{MyRoundingFunction(size / Math.Pow(2d, 10d), shortRoundNumber)} KBs";
            }
            else if (size > Math.Pow(2d, 20d) & size <= Math.Pow(2d, 30d))
            {
                result = $"{MyRoundingFunction(size / Math.Pow(2d, 20d), shortRoundNumber)} MBs";
            }
            else if (size > Math.Pow(2d, 30d) & size <= Math.Pow(2d, 40d))
            {
                result = $"{MyRoundingFunction(size / Math.Pow(2d, 30d), shortRoundNumber)} GBs";
            }
            else if (size > Math.Pow(2d, 40d) & size <= Math.Pow(2d, 50d))
            {
                result = $"{MyRoundingFunction(size / Math.Pow(2d, 40d), shortRoundNumber)} TBs";
            }
            else if (size > Math.Pow(2d, 50d) & size <= Math.Pow(2d, 60d))
            {
                result = $"{MyRoundingFunction(size / Math.Pow(2d, 50d), shortRoundNumber)} PBs";
            }
            else if (size > Math.Pow(2d, 60d) & size <= Math.Pow(2d, 70d))
            {
                result = $"{MyRoundingFunction(size / Math.Pow(2d, 50d), shortRoundNumber)} EBs";
            }
            else
            {
                result = "(None)";
            }

            return result;
        }

        public static string MyRoundingFunction(double value, int digits)
        {
            if (digits < 0)
                throw new ArgumentException("The number of digits must be non-negative.", nameof(digits));

            if (digits == 0)
            {
                return Math.Round(value, digits).ToString();
            }
            else
            {
                return Math.Round(value, digits).ToString("0." + new string('0', digits));
            }
        }

        public static void CenterFormOverParent(Form parent, Form child)
        {
            int parentCenterX = parent.Left + parent.Width / 2;
            int parentCenterY = parent.Top + parent.Height / 2;

            int childLeft = parentCenterX - child.Width / 2;
            int childTop = parentCenterY - child.Height / 2;

            child.Location = new Point(childLeft, childTop);
        }

        public static bool IsRegexPatternValid(string pattern)
        {
            try
            {
                var regex = new System.Text.RegularExpressions.Regex(pattern);
                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        private static IPAddress GetLocalIPAddress()
        {
            NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            if (networkInterfaces is not null && networkInterfaces.Any())
            {
                foreach (NetworkInterface ni in networkInterfaces)
                {
                    if (ni is not null && ni.OperationalStatus == OperationalStatus.Up && ni.NetworkInterfaceType != NetworkInterfaceType.Loopback && ni.NetworkInterfaceType != NetworkInterfaceType.Tunnel)
                    {
                        if (ni.GetIPProperties().UnicastAddresses.Any()) {
                            foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                            {
                                if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                                    return ip.Address;
                            }
                        }
                    }
                }
            }

            throw new Exception("No active network adapter with a matching IP address found.");
        }

        public static void SendMessageToSysLogServer(string strMessage, int intPort)
        {
            try
            {
                using (var udpClient = new UdpClient())
                {
                    udpClient.Connect(GetLocalIPAddress(), intPort);
                    byte[] data = Encoding.UTF8.GetBytes(strMessage);
                    udpClient.Send(data, data.Length);
                }
            }
            catch (Exception)
            {
            }
        }

        public static void SendMessageToTCPSysLogServer(string strMessage, int intPort)
        {
            try
            {
                using (var tcpClient = new TcpClient())
                {
                    tcpClient.Connect(GetLocalIPAddress(), intPort);

                    using (var networkStream = tcpClient.GetStream())
                    {
                        byte[] data = Encoding.UTF8.GetBytes(strMessage);
                        networkStream.Write(data, 0, data.Length);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        public static void SendMessageToSysLogServer(string strMessage, string strDestinationIP, int intPort)
        {
            try
            {
                using (var udpClient = new UdpClient())
                {
                    udpClient.Connect(strDestinationIP, intPort);
                    byte[] data = Encoding.UTF8.GetBytes(strMessage);
                    udpClient.Send(data, data.Length);
                }
            }
            catch (SocketException)
            {
            }
        }

        public static string SanitizeForCSV(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return "";
            }
            else
            {
                if (input.Contains(strQuote))
                    input = input.Replace(strQuote, strQuote + strQuote);
                if (input.Contains(","))
                    input = $"{strQuote}{input}{strQuote}";
                input = input.Replace(Constants.vbCrLf, @"\n");
                return input;
            }
        }

        public static Point VerifyWindowLocation(Point point, ref Form window)
        {
            var screen = Screen.FromPoint(point); // Get the screen based on the new window location

            var windowBounds = new Rectangle(point.X, point.Y, window.Width, window.Height);
            var screenBounds = screen.WorkingArea;

            // Ensure the window is at least partially on the screen
            if (windowBounds.IntersectsWith(screenBounds))
            {
                return point;
            }
            else
            {
                // Adjust the window to a default location if it is completely off-screen
                return new Point(screenBounds.Left, screenBounds.Top);
            }
        }

        public static void SelectFileInWindowsExplorer(string strFullPath)
        {
            if (!string.IsNullOrEmpty(strFullPath) && System.IO.File.Exists(strFullPath))
            {
                var pidlList = NativeMethod.NativeMethods.ILCreateFromPathW(strFullPath);

                if (!pidlList.Equals(IntPtr.Zero))
                {
                    try
                    {
                        NativeMethod.NativeMethods.SHOpenFolderAndSelectItems(pidlList, 0U, IntPtr.Zero, 0U);
                    }
                    finally
                    {
                        NativeMethod.NativeMethods.ILFree(pidlList);
                    }
                }
            }
        }
    }
}