using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Free_SysLog
{
    public class MyDataGridViewFileRowComparer : IComparer<MyDataGridViewFileRow>
    {

        private readonly int intColumnNumber;
        private readonly SortOrder soSortOrder;

        public MyDataGridViewFileRowComparer(int columnNumber, SortOrder sortOrder)
        {
            intColumnNumber = columnNumber;
            soSortOrder = sortOrder;
        }

        public int Compare(MyDataGridViewFileRow row1, MyDataGridViewFileRow row2)
        {
            // Compare them.
            if (intColumnNumber == 1)
            {
                var date1 = row1.fileDate;
                var date2 = row2.fileDate;

                return soSortOrder == SortOrder.Ascending ? DateTime.Compare(date1, date2) : DateTime.Compare(date2, date1);
            }
            else if (intColumnNumber == 2)
            {
                long size1 = row1.fileSize;
                long size2 = row2.fileSize;

                return soSortOrder == SortOrder.Ascending ? size1.CompareTo(size2) : size2.CompareTo(size1);
            }
            else if (intColumnNumber == 3)
            {
                long entry1 = row1.entryCount;
                long entry2 = row2.entryCount;

                return soSortOrder == SortOrder.Ascending ? entry1.CompareTo(entry2) : entry2.CompareTo(entry1);
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