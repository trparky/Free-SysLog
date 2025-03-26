using System;
using System.Drawing;
using System.Windows.Forms;

namespace Free_SysLog
{
    public partial class CloseFreeSysLogDialog
    {
        public Form1 MyParentForm { get; set; }

        public CloseFreeSysLogDialog()
        {
            InitializeComponent();
        }

        private void Close_Free_SysLog_Load(object sender, EventArgs e)
        {
            System.Media.SystemSounds.Asterisk.Play();
            PictureBox1.Image = SystemIcons.Question.ToBitmap();
            ChkConfirmClose.Checked = My.MySettingsProperty.Settings.boolConfirmClose;
        }

        private void BtnYes_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
            Close();
        }

        private void BtnNo_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            Close();
        }

        private void BtnMinimize_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void CloseFreeSysLogDialog_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Y)
            {
                BtnYes.PerformClick();
            }
            else if (e.KeyCode == Keys.N)
            {
                BtnNo.PerformClick();
            }
            else if (e.KeyCode == Keys.M)
            {
                BtnMinimize.PerformClick();
            }
        }

        private void ChkConfirmClose_Click(object sender, EventArgs e)
        {
            My.MySettingsProperty.Settings.boolConfirmClose = ChkConfirmClose.Checked;
            if (MyParentForm is not null)
                MyParentForm.ChkEnableConfirmCloseToolStripItem.Checked = ChkConfirmClose.Checked;
        }
    }
}