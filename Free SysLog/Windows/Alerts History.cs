using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Free_SysLog
{

    public partial class Alerts_History
    {
        public List<AlertsHistory> DataToLoad { get; set; }
        private new Form1 ParentForm;
        private bool boolDoneLoading = false;

        public Form1 SetParentForm
        {
            set
            {
                ParentForm = value;
            }
        }

        public Alerts_History()
        {
            InitializeComponent();
        }

        private void OpenLogViewerWindow(string strLogText, string strAlertText, string strLogDate, string strSourceIP, string strRawLogText)
        {
            strRawLogText = strRawLogText.Replace("{newline}", Constants.vbCrLf, StringComparison.OrdinalIgnoreCase);

            using (var LogViewerInstance = new LogViewer() { strRawLogText = strRawLogText, strLogText = strLogText, StartPosition = FormStartPosition.CenterParent, Icon = Icon })
            {
                LogViewerInstance.LblLogDate.Text = $"Log Date: {strLogDate}";
                LogViewerInstance.LblSource.Text = $"Source IP Address: {strSourceIP}";
                LogViewerInstance.TopMost = true;
                LogViewerInstance.lblAlertText.Text = $"Alert Text: {strAlertText}";

                LogViewerInstance.ShowDialog();
            }
        }

        private void Alerts_History_Load(object sender, EventArgs e)
        {
            if (My.MySettingsProperty.Settings.font is not null)
            {
                AlertHistoryList.DefaultCellStyle.Font = My.MySettingsProperty.Settings.font;
                AlertHistoryList.ColumnHeadersDefaultCellStyle.Font = My.MySettingsProperty.Settings.font;
            }

            var flags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty;
            var propInfo = typeof(DataGridView).GetProperty("DoubleBuffered", flags);
            propInfo?.SetValue(AlertHistoryList, true, null);

            AlertHistoryList.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle() { BackColor = My.MySettingsProperty.Settings.searchColor, ForeColor = SupportCode.SupportCode.GetGoodTextColorBasedUponBackgroundColor(My.MySettingsProperty.Settings.searchColor) };
            Size = My.MySettingsProperty.Settings.AlertHistorySize;
            Form argwindow = this;
            Location = SupportCode.SupportCode.VerifyWindowLocation(My.MySettingsProperty.Settings.AlertHistoryLocation, ref argwindow);

            colTime.Width = My.MySettingsProperty.Settings.columnTimeSize;

            var argcolumns = AlertHistoryList.Columns;
            var argspecializedStringCollection = My.MySettingsProperty.Settings.alertsHistoryColumnOrder;
            SupportCode.SupportCode.LoadColumnOrders(ref argcolumns, ref argspecializedStringCollection);
            My.MySettingsProperty.Settings.alertsHistoryColumnOrder = argspecializedStringCollection;

            if (DataToLoad is not null && DataToLoad.Any())
            {
                lblNumberOfAlerts.Text = $"Number of Alerts: {DataToLoad.Count:N0}";
                var listOfDataRows = new List<AlertsHistoryDataGridViewRow>();

                foreach (AlertsHistory item in DataToLoad)
                {
                    AlertsHistoryDataGridViewRow localMakeDataGridRow() { var argdataGrid = AlertHistoryList; var ret = item.MakeDataGridRow(ref argdataGrid); AlertHistoryList = argdataGrid; return ret; }

                    listOfDataRows.Add(localMakeDataGridRow());
                }

                AlertHistoryList.SuspendLayout();
                AlertHistoryList.Rows.AddRange(listOfDataRows.ToArray());
                AlertHistoryList.ResumeLayout();
            }

            boolDoneLoading = true;
        }

        private void Alerts_History_ResizeEnd(object sender, EventArgs e)
        {
            if (boolDoneLoading)
                My.MySettingsProperty.Settings.AlertHistorySize = Size;
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_EXITSIZEMOVE = 0x232;

            base.WndProc(ref m);

            if (m.Msg == WM_EXITSIZEMOVE && boolDoneLoading)
            {
                Form argwindow = this;
                Location = SupportCode.SupportCode.VerifyWindowLocation(Location, ref argwindow);
                My.MySettingsProperty.Settings.AlertHistoryLocation = Location;
            }
        }

        private void AlertHistoryList_DoubleClick(object sender, EventArgs e)
        {
            if (AlertHistoryList.SelectedRows.Count > 0)
            {
                AlertsHistoryDataGridViewRow AlertsHistoryDataGridViewRow = (AlertsHistoryDataGridViewRow)AlertHistoryList.SelectedRows[0];

                OpenLogViewerWindow(AlertsHistoryDataGridViewRow.strLog, AlertsHistoryDataGridViewRow.strAlertText, AlertsHistoryDataGridViewRow.strTime, AlertsHistoryDataGridViewRow.strIP, AlertsHistoryDataGridViewRow.strRawLog);
            }
        }

        private void Alerts_History_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
                BtnRefresh.PerformClick();
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            if (ParentForm is not null)
            {
                {
                    ref var withBlock = ref ParentForm;
                    var data = new List<AlertsHistory>();

                    lock (ParentForm.dataGridLockObject)
                    {
                        foreach (MyDataGridViewRow item in ParentForm.Logs.Rows)
                        {
                            if (item.BoolAlerted)
                            {
                                data.Add(new AlertsHistory()
                                {
                                    strTime = Conversions.ToString(item.Cells[SupportCode.SupportCode.ColumnIndex_ComputedTime].Value),
                                    alertType = item.alertType,
                                    strAlertText = item.AlertText,
                                    strIP = Conversions.ToString(item.Cells[SupportCode.SupportCode.ColumnIndex_IPAddress].Value),
                                    strLog = Conversions.ToString(item.Cells[SupportCode.SupportCode.ColumnIndex_LogText].Value),
                                    strRawLog = item.RawLogData
                                });
                            }
                        }
                    }

                    if (data.Any())
                    {
                        lblNumberOfAlerts.Text = $"Number of Alerts: {data.Count:N0}";
                        var listOfDataRows = new List<AlertsHistoryDataGridViewRow>();

                        foreach (AlertsHistory item in data)
                        {
                            AlertsHistoryDataGridViewRow localMakeDataGridRow() { var argdataGrid1 = AlertHistoryList; var ret = item.MakeDataGridRow(ref argdataGrid1); AlertHistoryList = argdataGrid1; return ret; }

                            listOfDataRows.Add(localMakeDataGridRow());
                        }

                        AlertHistoryList.SuspendLayout();
                        AlertHistoryList.Rows.Clear();
                        AlertHistoryList.Rows.AddRange(listOfDataRows.ToArray());
                        AlertHistoryList.ResumeLayout();
                    }
                }
            }
        }

        private void Alerts_History_FormClosing(object sender, FormClosingEventArgs e)
        {
            My.MySettingsProperty.Settings.alertsHistoryColumnOrder = SupportCode.SupportCode.SaveColumnOrders(AlertHistoryList.Columns);
        }
    }
}