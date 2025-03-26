using System;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace Free_SysLog
{

    public partial class LogViewer
    {
        public string strLogText, strRawLogText;
        public Form1 MyParentForm;

        public LogViewer()
        {
            InitializeComponent();
        }

        private void Log_Viewer_Load(object sender, EventArgs e)
        {
            if (My.MySettingsProperty.Settings.font is not null)
                LogText.Font = My.MySettingsProperty.Settings.font;

            ChkShowRawLog.Checked = My.MySettingsProperty.Settings.boolShowRawLogOnLogViewer;
            Size = My.MySettingsProperty.Settings.logViewerWindowSize;
            LogText.Text = ChkShowRawLog.Checked ? strRawLogText : strLogText;
            LogText.Select(0, 0);
        }

        private void Log_Viewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            My.MySettingsProperty.Settings.logViewerWindowSize = Size;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Log_Viewer_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
                Close();
        }

        private void ChkShowRawLog_Click(object sender, EventArgs e)
        {
            My.MySettingsProperty.Settings.boolShowRawLogOnLogViewer = ChkShowRawLog.Checked;
            LogText.Text = ChkShowRawLog.Checked ? strRawLogText.Replace("{newline}", Constants.vbCrLf, StringComparison.OrdinalIgnoreCase) : strLogText;
            if (MyParentForm is not null)
                MyParentForm.ShowRawLogOnLogViewer.Checked = ChkShowRawLog.Checked;
        }
    }
}