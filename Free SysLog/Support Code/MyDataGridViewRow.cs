using System;
using System.Windows.Forms;

namespace Free_SysLog
{
    public class MyDataGridViewRow : DataGridViewRow, ICloneable
    {
        public DateTime DateObject { get; set; }
        public DateTime ServerDate { get; set; }
        public bool BoolAlerted { get; set; } = false;
        public string RawLogData { get; set; }
        public string AlertText { get; set; }
        public AlertType alertType { get; set; }

        public override object Clone()
        {
            var newDataGridRow = new MyDataGridViewRow();
            newDataGridRow.CreateCells(DataGridView);
            newDataGridRow.DateObject = DateObject;
            newDataGridRow.BoolAlerted = BoolAlerted;
            newDataGridRow.AlertText = AlertText;
            newDataGridRow.RawLogData = RawLogData;
            newDataGridRow.alertType = alertType;

            for (short index = 0, loopTo = (short)(Cells.Count - 1); index <= loopTo; index++)
            {
                newDataGridRow.Cells[index].Value = Cells[index].Value;
                newDataGridRow.Cells[index].Style.Alignment = Cells[index].Style.Alignment;
                newDataGridRow.Cells[index].Style.Font = Cells[index].Style.Font;
                newDataGridRow.MinimumHeight = MinimumHeight;
            }

            newDataGridRow.DefaultCellStyle.Padding = new Padding(0, 2, 0, 2);
            return newDataGridRow;
        }
    }
}