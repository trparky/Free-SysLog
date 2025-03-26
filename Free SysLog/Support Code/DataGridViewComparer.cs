using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Free_SysLog
{
    public class DataGridViewComparer : IComparer<MyDataGridViewRow>
    {

        private readonly int intColumnNumber;
        private readonly SortOrder soSortOrder;

        public DataGridViewComparer(int columnNumber, SortOrder sortOrder)
        {
            intColumnNumber = columnNumber;
            soSortOrder = sortOrder;
        }

        public int Compare(MyDataGridViewRow row1, MyDataGridViewRow row2)
        {
            // Compare them.
            if (intColumnNumber == 0)
            {
                var date1 = row1.DateObject;
                var date2 = row2.DateObject;

                return soSortOrder == SortOrder.Ascending ? DateTime.Compare(date1, date2) : DateTime.Compare(date2, date1);
            }
            else
            {
                string strFirstString = row1.Cells.Count <= intColumnNumber ? "" : (row1.Cells[intColumnNumber].Value?.ToString());
                string strSecondString = row2.Cells.Count <= intColumnNumber ? "" : (row2.Cells[intColumnNumber].Value?.ToString());

                return soSortOrder == SortOrder.Descending ? string.Compare(strFirstString, strSecondString) : string.Compare(strSecondString, strFirstString);
            }

            return 0;
        }
    }
}