using System;
using System.Windows.Forms;

namespace Free_SysLog
{

    public class ColumnOrder
    {
        public string ColumnName;
        public int ColumnIndex;
    }

    public class SavedData
    {
        public string time, ip, log, fileName, logType, hostname, appName, rawLogData, alertText;
        public AlertType alertType = AlertType.None;
        public DateTime DateObject, ServerDate;
        public bool BoolAlerted = false;

        public MyDataGridViewRow MakeDataGridRow(ref DataGridView dataGrid)
        {
            using (var MyDataGridViewRow = new MyDataGridViewRow())
            {
                MyDataGridViewRow.CreateCells(dataGrid);
                MyDataGridViewRow.Cells[SupportCode.SupportCode.ColumnIndex_ComputedTime].Value = time;
                MyDataGridViewRow.Cells[SupportCode.SupportCode.ColumnIndex_ComputedTime].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                MyDataGridViewRow.Cells[SupportCode.SupportCode.ColumnIndex_LogType].Value = string.IsNullOrWhiteSpace(logType) ? "" : logType;
                MyDataGridViewRow.Cells[SupportCode.SupportCode.ColumnIndex_IPAddress].Value = ip;
                MyDataGridViewRow.Cells[SupportCode.SupportCode.ColumnIndex_RemoteProcess].Value = string.IsNullOrWhiteSpace(appName) ? "" : appName;
                MyDataGridViewRow.Cells[SupportCode.SupportCode.ColumnIndex_Hostname].Value = string.IsNullOrWhiteSpace(hostname) ? "" : hostname;
                MyDataGridViewRow.Cells[SupportCode.SupportCode.ColumnIndex_LogText].Value = log;
                MyDataGridViewRow.Cells[SupportCode.SupportCode.ColumnIndex_Alerted].Value = BoolAlerted ? "Yes" : "No";
                MyDataGridViewRow.Cells[SupportCode.SupportCode.ColumnIndex_Alerted].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                MyDataGridViewRow.Cells[SupportCode.SupportCode.ColumnIndex_Alerted].Style.WrapMode = DataGridViewTriState.True;
                MyDataGridViewRow.Cells[SupportCode.SupportCode.ColumnIndex_ServerTime].Value = ServerDate == DateTime.MinValue ? "" : SupportCode.SupportCode.ToIso8601Format(ServerDate);
                MyDataGridViewRow.DateObject = DateObject;
                MyDataGridViewRow.BoolAlerted = BoolAlerted;
                MyDataGridViewRow.RawLogData = rawLogData;
                MyDataGridViewRow.AlertText = alertText;
                MyDataGridViewRow.alertType = alertType;

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
    }

    public class ReplacementsClass
    {
        public bool BoolRegex;
        public bool BoolCaseSensitive;
        public string StrReplace, StrReplaceWith;
        public bool BoolEnabled = true;

        public MyReplacementsListViewItem ToListViewItem()
        {
            var listViewItem = new MyReplacementsListViewItem(StrReplace);
            listViewItem.SubItems.Add(StrReplaceWith);
            listViewItem.SubItems.Add(BoolRegex ? "Yes" : "No");
            listViewItem.SubItems.Add(BoolCaseSensitive ? "Yes" : "No");
            listViewItem.SubItems.Add(BoolEnabled ? "Yes" : "No");
            listViewItem.BoolRegex = BoolRegex;
            listViewItem.BoolCaseSensitive = BoolCaseSensitive;
            listViewItem.BoolEnabled = BoolEnabled;
            if (My.MySettingsProperty.Settings.font is not null)
                listViewItem.Font = My.MySettingsProperty.Settings.font;
            return listViewItem;
        }
    }

    public class AlertsHistory
    {
        public string strTime, strAlertText, strIP, strLog, strRawLog;
        public AlertType alertType;

        public AlertsHistoryDataGridViewRow MakeDataGridRow(ref DataGridView dataGrid)
        {
            using (var AlertsHistoryDataGridViewRow = new AlertsHistoryDataGridViewRow())
            {
                AlertsHistoryDataGridViewRow.CreateCells(dataGrid);
                AlertsHistoryDataGridViewRow.Cells[0].Value = strTime;
                AlertsHistoryDataGridViewRow.Cells[0].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                if (alertType == AlertType.ErrorMsg)
                {
                    AlertsHistoryDataGridViewRow.Cells[1].Value = "Error";
                }
                else if (alertType == AlertType.Warning)
                {
                    AlertsHistoryDataGridViewRow.Cells[1].Value = "Warning";
                }
                else if (alertType == AlertType.Info)
                {
                    AlertsHistoryDataGridViewRow.Cells[1].Value = "Information";
                }
                else
                {
                    AlertsHistoryDataGridViewRow.Cells[1].Value = "";
                }

                AlertsHistoryDataGridViewRow.Cells[1].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                AlertsHistoryDataGridViewRow.Cells[2].Value = strAlertText;

                if (My.MySettingsProperty.Settings.font is not null)
                {
                    AlertsHistoryDataGridViewRow.Cells[0].Style.Font = My.MySettingsProperty.Settings.font;
                    AlertsHistoryDataGridViewRow.Cells[1].Style.Font = My.MySettingsProperty.Settings.font;
                    AlertsHistoryDataGridViewRow.Cells[2].Style.Font = My.MySettingsProperty.Settings.font;
                }

                AlertsHistoryDataGridViewRow.strRawLog = strRawLog;
                AlertsHistoryDataGridViewRow.strIP = strIP;
                AlertsHistoryDataGridViewRow.strLog = strLog;
                AlertsHistoryDataGridViewRow.strTime = strTime;
                AlertsHistoryDataGridViewRow.strAlertText = strAlertText;

                return AlertsHistoryDataGridViewRow;
            }
        }
    }

    public class IgnoredClass
    {
        public bool BoolRegex;
        public bool BoolCaseSensitive;
        public string StrIgnore;
        public bool BoolEnabled = true;

        public MyIgnoredListViewItem ToListViewItem()
        {
            var listViewItem = new MyIgnoredListViewItem(StrIgnore);
            listViewItem.SubItems.Add(BoolRegex ? "Yes" : "No");
            listViewItem.SubItems.Add(BoolCaseSensitive ? "Yes" : "No");
            listViewItem.SubItems.Add(BoolEnabled ? "Yes" : "No");
            listViewItem.BoolRegex = BoolRegex;
            listViewItem.BoolCaseSensitive = BoolCaseSensitive;
            listViewItem.BoolEnabled = BoolEnabled;
            if (My.MySettingsProperty.Settings.font is not null)
                listViewItem.Font = My.MySettingsProperty.Settings.font;
            return listViewItem;
        }
    }

    public enum AlertType : byte
    {
        Warning,
        Info,
        ErrorMsg,
        None
    }

    public class AlertsClass
    {
        public bool BoolRegex;
        public bool BoolCaseSensitive;
        public string StrLogText, StrAlertText;
        public AlertType alertType = AlertType.None;
        public bool BoolEnabled = true;
        public bool BoolLimited = true;

        public AlertsListViewItem ToListViewItem()
        {
            var listViewItem = new AlertsListViewItem(StrLogText) { StrLogText = StrLogText, StrAlertText = StrAlertText };
            listViewItem.SubItems.Add(string.IsNullOrWhiteSpace(StrAlertText) ? "(Shows Log Text)" : StrAlertText);
            listViewItem.SubItems.Add(BoolRegex ? "Yes" : "No");
            listViewItem.SubItems.Add(BoolCaseSensitive ? "Yes" : "No");

            switch (alertType)
            {
                case AlertType.Warning:
                    {
                        listViewItem.SubItems.Add("Warning");
                        break;
                    }
                case AlertType.ErrorMsg:
                    {
                        listViewItem.SubItems.Add("Error");
                        break;
                    }
                case AlertType.Info:
                    {
                        listViewItem.SubItems.Add("Information");
                        break;
                    }
                case AlertType.None:
                    {
                        listViewItem.SubItems.Add("None");
                        break;
                    }
            }

            listViewItem.SubItems.Add(BoolLimited ? "Yes" : "No");
            listViewItem.SubItems.Add(BoolEnabled ? "Yes" : "No");

            listViewItem.BoolRegex = BoolRegex;
            listViewItem.BoolCaseSensitive = BoolCaseSensitive;
            listViewItem.AlertType = alertType;
            listViewItem.BoolEnabled = BoolEnabled;
            listViewItem.BoolLimited = BoolLimited;
            if (My.MySettingsProperty.Settings.font is not null)
                listViewItem.Font = My.MySettingsProperty.Settings.font;
            return listViewItem;
        }
    }

    public class ProxiedSysLogData
    {
        public string ip, log;
    }

    public class NotificationDataPacket
    {
        public string logtext, alerttext, logdate, sourceip, rawlogtext;
    }

    public class CustomHostname
    {
        public string ip, deviceName;

        public ListViewItem ToListViewItem()
        {
            var ListViewItem = new ListViewItem(ip);

            ListViewItem.SubItems.Add(deviceName);

            if (My.MySettingsProperty.Settings.font is not null)
                ListViewItem.Font = My.MySettingsProperty.Settings.font;

            return ListViewItem;
        }
    }

    public class SysLogProxyServer
    {
        public string ip;
        public string name = null;
        public int port;
        public bool boolEnabled = true;

        public ServerListViewItem ToListViewItem()
        {
            var ServerListView = new ServerListViewItem(ip);

            ServerListView.SubItems.Add(port.ToString());
            ServerListView.SubItems.Add(boolEnabled ? "Yes" : "No");
            ServerListView.SubItems.Add(name);
            ServerListView.StrName = name;
            ServerListView.BoolEnabled = boolEnabled;
            if (My.MySettingsProperty.Settings.font is not null)
                ServerListView.Font = My.MySettingsProperty.Settings.font;

            return ServerListView;
        }
    }
}