using System;
using System.Windows.Forms;

namespace Free_SysLog
{
    // This class extends the ListViewItem so that I can add more properties to it for my purposes.
    public class MyReplacementsListViewItem : ListViewItem, ICloneable
    {
        public bool BoolRegex { get; set; }
        public bool BoolCaseSensitive { get; set; }
        public bool BoolEnabled { get; set; }

        public MyReplacementsListViewItem(string strInput)
        {
            Text = strInput;
        }
    }

    // This class extends the ListViewItem so that I can add more properties to it for my purposes.
    public class MyIgnoredListViewItem : ListViewItem, ICloneable
    {
        public bool BoolRegex { get; set; }
        public bool BoolCaseSensitive { get; set; }
        public bool BoolEnabled { get; set; }

        public MyIgnoredListViewItem(string strInput)
        {
            Text = strInput;
        }
    }

    public class AlertsListViewItem : ListViewItem, ICloneable
    {
        public bool BoolRegex { get; set; }
        public bool BoolCaseSensitive { get; set; }
        public string StrLogText { get; set; }
        public string StrAlertText { get; set; }
        public AlertType AlertType { get; set; }
        public bool BoolEnabled { get; set; }
        public bool BoolLimited { get; set; }

        public AlertsListViewItem(string strInput)
        {
            Text = strInput;
        }
    }

    public class ServerListViewItem : ListViewItem, ICloneable
    {
        public bool BoolEnabled { get; set; }
        public string StrName { get; set; } = null;

        public ServerListViewItem(string strInput)
        {
            Text = strInput;
        }
    }
}