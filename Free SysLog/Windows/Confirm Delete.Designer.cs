using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Free_SysLog
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class Confirm_Delete : Form
    {

        // Form overrides dispose to clean up the component list.
        [DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components is not null)
                {
                    components.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.  
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            lblMainLabel = new Label();
            IconPictureBox = new PictureBox();
            TableLayoutPanel1 = new TableLayoutPanel();
            BtnYesDeleteYesBackup = new Button();
            BtnYesDeleteYesBackup.Click += new EventHandler(BtnYesDeleteYesBackup_Click);
            BtnDontDelete = new Button();
            BtnDontDelete.Click += new EventHandler(BtnDontDelete_Click);
            BtnYesDeleteNoBackup = new Button();
            BtnYesDeleteNoBackup.Click += new EventHandler(BtnYesDeleteNoBackup_Click);
            ((System.ComponentModel.ISupportInitialize)IconPictureBox).BeginInit();
            TableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // lblMainLabel
            // 
            lblMainLabel.AutoSize = true;
            lblMainLabel.Location = new Point(50, 12);
            lblMainLabel.Name = "lblMainLabel";
            lblMainLabel.Size = new Size(81, 13);
            lblMainLabel.TabIndex = 0;
            lblMainLabel.Text = "Fill In Text Here";
            // 
            // IconPictureBox
            // 
            IconPictureBox.Location = new Point(12, 12);
            IconPictureBox.Name = "IconPictureBox";
            IconPictureBox.Size = new Size(32, 32);
            IconPictureBox.TabIndex = 25;
            IconPictureBox.TabStop = false;
            // 
            // TableLayoutPanel1
            // 
            TableLayoutPanel1.ColumnCount = 3;
            TableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33333f));
            TableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33333f));
            TableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33333f));
            TableLayoutPanel1.Controls.Add(BtnYesDeleteYesBackup, 1, 0);
            TableLayoutPanel1.Controls.Add(BtnDontDelete, 0, 0);
            TableLayoutPanel1.Controls.Add(BtnYesDeleteNoBackup, 2, 0);
            TableLayoutPanel1.Location = new Point(12, 50);
            TableLayoutPanel1.Name = "TableLayoutPanel1";
            TableLayoutPanel1.RowCount = 1;
            TableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100.0f));
            TableLayoutPanel1.Size = new Size(433, 29);
            TableLayoutPanel1.TabIndex = 29;
            // 
            // BtnYesDeleteYesBackup
            // 
            BtnYesDeleteYesBackup.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            BtnYesDeleteYesBackup.Location = new Point(147, 3);
            BtnYesDeleteYesBackup.Name = "BtnYesDeleteYesBackup";
            BtnYesDeleteYesBackup.Size = new Size(138, 23);
            BtnYesDeleteYesBackup.TabIndex = 29;
            BtnYesDeleteYesBackup.Text = "&Yes, With Backup";
            BtnYesDeleteYesBackup.UseVisualStyleBackColor = true;
            // 
            // BtnDontDelete
            // 
            BtnDontDelete.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            BtnDontDelete.Location = new Point(3, 3);
            BtnDontDelete.Name = "BtnDontDelete";
            BtnDontDelete.Size = new Size(138, 23);
            BtnDontDelete.TabIndex = 30;
            BtnDontDelete.Text = "&Don't Delete";
            BtnDontDelete.UseVisualStyleBackColor = true;
            // 
            // BtnYesDeleteNoBackup
            // 
            BtnYesDeleteNoBackup.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            BtnYesDeleteNoBackup.Location = new Point(291, 3);
            BtnYesDeleteNoBackup.Name = "BtnYesDeleteNoBackup";
            BtnYesDeleteNoBackup.Size = new Size(139, 23);
            BtnYesDeleteNoBackup.TabIndex = 27;
            BtnYesDeleteNoBackup.Text = "Yes, With &No Backup";
            BtnYesDeleteNoBackup.UseVisualStyleBackColor = true;
            // 
            // Confirm_Delete
            // 
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(456, 90);
            Controls.Add(TableLayoutPanel1);
            Controls.Add(IconPictureBox);
            Controls.Add(lblMainLabel);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Confirm_Delete";
            Text = "Confirm Deletion of Logs?";
            ((System.ComponentModel.ISupportInitialize)IconPictureBox).EndInit();
            TableLayoutPanel1.ResumeLayout(false);
            Load += new EventHandler(Delete_Confirm_Load);
            ResumeLayout(false);
            PerformLayout();

        }

        internal Label lblMainLabel;
        internal PictureBox IconPictureBox;
        internal TableLayoutPanel TableLayoutPanel1;
        internal Button BtnDontDelete;
        internal Button BtnYesDeleteYesBackup;
        internal Button BtnYesDeleteNoBackup;
    }
}