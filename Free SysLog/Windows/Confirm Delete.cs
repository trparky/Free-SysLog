using System;
using System.Drawing;

namespace Free_SysLog
{
    public partial class Confirm_Delete
    {
        private int intNumberOfLogsToBeDeleted;
        public UserChoice choice;

        public enum UserChoice : int
        {
            YesDeleteNoBackup,
            YesDeleteYesBackup,
            NoDelete
        }

        public Confirm_Delete(int _intNumberOfLogsToBeDeleted)
        {
            // This call is required by the designer.
            InitializeComponent();

            // Add any initialization after the InitializeComponent() call.
            intNumberOfLogsToBeDeleted = _intNumberOfLogsToBeDeleted;
        }

        private void Delete_Confirm_Load(object sender, EventArgs e)
        {
            System.Media.SystemSounds.Asterisk.Play();
            IconPictureBox.Image = SystemIcons.Question.ToBitmap();
            BtnDontDelete.Select();
        }

        private void BtnDontDelete_Click(object sender, EventArgs e)
        {
            choice = UserChoice.NoDelete;
            Close();
        }

        private void BtnYesDeleteYesBackup_Click(object sender, EventArgs e)
        {
            choice = UserChoice.YesDeleteYesBackup;
            Close();
        }

        private void BtnYesDeleteNoBackup_Click(object sender, EventArgs e)
        {
            choice = UserChoice.YesDeleteNoBackup;
            Close();
        }
    }
}