using System;
using System.Windows.Forms;

namespace Free_SysLog
{
    public partial class ClearLogsOlderThan
    {
        public DateTime dateChosenDate;

        public ClearLogsOlderThan()
        {
            InitializeComponent();
        }

        private void Clear_logs_older_than_Load(object sender, EventArgs e)
        {
            DateTimePicker.MinDate = DateTime.Now.AddDays(-15);
            DateTimePicker.MaxDate = DateTime.Now;
        }

        private void BtnClearLogs_Click(object sender, EventArgs e)
        {
            dateChosenDate = DateTimePicker.Value;
            Close();
        }

        private void DateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            BtnClearLogs.Enabled = true;
            DialogResult = DialogResult.OK;
        }

        private void Clear_logs_older_than_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }
    }
}