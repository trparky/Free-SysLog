using System;
using System.Windows.Forms;

namespace Free_SysLog
{
    public class MyDataGridViewFileRow : DataGridViewRow, ICloneable
    {
        public DateTime fileDate { get; set; }
        public long fileSize { get; set; }
        public long entryCount { get; set; }

        public override object Clone()
        {
            var newDataGridRow = new MyDataGridViewFileRow();
            newDataGridRow.CreateCells(DataGridView);
            newDataGridRow.fileDate = fileDate;
            newDataGridRow.fileSize = fileSize;
            newDataGridRow.entryCount = entryCount;

            for (short index = 0, loopTo = (short)(Cells.Count - 1); index <= loopTo; index++)
            {
                newDataGridRow.Cells[index].Value = Cells[index].Value;
                newDataGridRow.Cells[index].Style.Alignment = Cells[index].Style.Alignment;
                newDataGridRow.Cells[index].Style.Font = Cells[index].Style.Font;
                newDataGridRow.MinimumHeight = MinimumHeight;
            }

            return newDataGridRow;
        }
    }
}