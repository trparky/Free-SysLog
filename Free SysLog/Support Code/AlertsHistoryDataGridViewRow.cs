using System;
using System.Windows.Forms;

namespace Free_SysLog
{
    public class AlertsHistoryDataGridViewRow : DataGridViewRow, ICloneable
    {
        public string strIP { get; set; }
        public string strLog { get; set; }
        public string strRawLog { get; set; }
        public string strTime { get; set; }
        public string strAlertText { get; set; }

        public override object Clone()
        {
            var AlertsHistoryDataGridViewRow = new AlertsHistoryDataGridViewRow();

            AlertsHistoryDataGridViewRow.CreateCells(DataGridView);
            AlertsHistoryDataGridViewRow.strRawLog = strRawLog;
            AlertsHistoryDataGridViewRow.strLog = strLog;
            AlertsHistoryDataGridViewRow.strIP = strIP;
            AlertsHistoryDataGridViewRow.strTime = strTime;
            AlertsHistoryDataGridViewRow.strAlertText = strAlertText;

            for (short index = 0, loopTo = (short)(Cells.Count - 1); index <= loopTo; index++)
            {
                AlertsHistoryDataGridViewRow.Cells[index].Value = Cells[index].Value;
                AlertsHistoryDataGridViewRow.Cells[index].Style.Alignment = Cells[index].Style.Alignment;
                AlertsHistoryDataGridViewRow.Cells[index].Style.Font = Cells[index].Style.Font;
                AlertsHistoryDataGridViewRow.MinimumHeight = MinimumHeight;
            }

            return AlertsHistoryDataGridViewRow;
        }
    }
}