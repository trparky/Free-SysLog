using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Free_SysLog
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class CloseFreeSysLogDialog : Form
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
            components = new System.ComponentModel.Container();
            Panel1 = new Panel();
            Label1 = new Label();
            PictureBox1 = new PictureBox();
            BtnNo = new Button();
            BtnNo.Click += new EventHandler(BtnNo_Click);
            BtnYes = new Button();
            BtnYes.Click += new EventHandler(BtnYes_Click);
            BtnMinimize = new Button();
            BtnMinimize.Click += new EventHandler(BtnMinimize_Click);
            ToolTip = new ToolTip(components);
            ChkConfirmClose = new CheckBox();
            ChkConfirmClose.Click += new EventHandler(ChkConfirmClose_Click);
            Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PictureBox1).BeginInit();
            SuspendLayout();
            // 
            // Panel1
            // 
            Panel1.BackColor = Color.White;
            Panel1.Controls.Add(Label1);
            Panel1.Controls.Add(PictureBox1);
            Panel1.Location = new Point(0, 0);
            Panel1.Name = "Panel1";
            Panel1.Size = new Size(293, 58);
            Panel1.TabIndex = 2;
            // 
            // Label1
            // 
            Label1.AutoSize = true;
            Label1.Location = new Point(50, 14);
            Label1.Name = "Label1";
            Label1.Size = new Size(220, 13);
            Label1.TabIndex = 3;
            Label1.Text = "Are you sure you want to close Free SysLog?";
            // 
            // PictureBox1
            // 
            PictureBox1.Location = new Point(12, 14);
            PictureBox1.Name = "PictureBox1";
            PictureBox1.Size = new Size(32, 32);
            PictureBox1.TabIndex = 2;
            PictureBox1.TabStop = false;
            // 
            // BtnNo
            // 
            BtnNo.Location = new Point(123, 64);
            BtnNo.Name = "BtnNo";
            BtnNo.Size = new Size(75, 23);
            BtnNo.TabIndex = 0;
            BtnNo.Text = "&No";
            ToolTip.SetToolTip(BtnNo, "Can be activated by pressing the N key.");
            BtnNo.UseVisualStyleBackColor = true;
            // 
            // BtnYes
            // 
            BtnYes.Location = new Point(42, 64);
            BtnYes.Name = "BtnYes";
            BtnYes.Size = new Size(75, 23);
            BtnYes.TabIndex = 2;
            BtnYes.Text = "&Yes";
            ToolTip.SetToolTip(BtnYes, "Can be activated by pressing the Y key.");
            BtnYes.UseVisualStyleBackColor = true;
            // 
            // BtnMinimize
            // 
            BtnMinimize.Location = new Point(204, 64);
            BtnMinimize.Name = "BtnMinimize";
            BtnMinimize.Size = new Size(75, 23);
            BtnMinimize.TabIndex = 1;
            BtnMinimize.Text = "&Minimize";
            ToolTip.SetToolTip(BtnMinimize, "Can be activated by pressing the M key.");
            BtnMinimize.UseVisualStyleBackColor = true;
            // 
            // ChkConfirmClose
            // 
            ChkConfirmClose.AutoSize = true;
            ChkConfirmClose.Location = new Point(12, 93);
            ChkConfirmClose.Name = "ChkConfirmClose";
            ChkConfirmClose.Size = new Size(90, 17);
            ChkConfirmClose.TabIndex = 3;
            ChkConfirmClose.Text = "Confirm Close";
            ChkConfirmClose.UseVisualStyleBackColor = true;
            // 
            // CloseFreeSysLogDialog
            // 
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(291, 116);
            Controls.Add(ChkConfirmClose);
            Controls.Add(BtnMinimize);
            Controls.Add(BtnYes);
            Controls.Add(BtnNo);
            Controls.Add(Panel1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "CloseFreeSysLogDialog";
            Text = "Close Free SysLog?";
            Panel1.ResumeLayout(false);
            Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)PictureBox1).EndInit();
            Load += new EventHandler(Close_Free_SysLog_Load);
            KeyUp += new KeyEventHandler(CloseFreeSysLogDialog_KeyUp);
            ResumeLayout(false);
            PerformLayout();

        }
        internal Panel Panel1;
        internal Label Label1;
        internal PictureBox PictureBox1;
        internal Button BtnNo;
        internal Button BtnYes;
        internal Button BtnMinimize;
        internal ToolTip ToolTip;
        internal CheckBox ChkConfirmClose;
    }
}