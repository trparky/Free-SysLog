using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Free_SysLog
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class LogViewer : Form
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
            LogText = new TextBox();
            BtnClose = new Button();
            BtnClose.Click += new EventHandler(BtnClose_Click);
            StatusStrip1 = new StatusStrip();
            LblLogDate = new ToolStripStatusLabel();
            LblSource = new ToolStripStatusLabel();
            lblAlertText = new Label();
            ChkShowRawLog = new CheckBox();
            ChkShowRawLog.Click += new EventHandler(ChkShowRawLog_Click);
            StatusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // LogText
            // 
            LogText.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            LogText.BackColor = SystemColors.Window;
            LogText.Location = new Point(12, 30);
            LogText.Multiline = true;
            LogText.Name = "LogText";
            LogText.ReadOnly = true;
            LogText.Size = new Size(776, 108);
            LogText.TabIndex = 1;
            // 
            // BtnClose
            // 
            BtnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BtnClose.Location = new Point(713, 144);
            BtnClose.Name = "BtnClose";
            BtnClose.Size = new Size(75, 26);
            BtnClose.TabIndex = 0;
            BtnClose.Text = "&Close";
            BtnClose.UseVisualStyleBackColor = true;
            // 
            // StatusStrip1
            // 
            StatusStrip1.Items.AddRange(new ToolStripItem[] { LblLogDate, LblSource });
            StatusStrip1.Location = new Point(0, 173);
            StatusStrip1.Name = "StatusStrip1";
            StatusStrip1.Size = new Size(800, 22);
            StatusStrip1.TabIndex = 4;
            StatusStrip1.Text = "StatusStrip1";
            // 
            // LblLogDate
            // 
            LblLogDate.Name = "LblLogDate";
            LblLogDate.Size = new Size(57, 17);
            LblLogDate.Text = "Log Date:";
            // 
            // LblSource
            // 
            LblSource.Margin = new Padding(100, 3, 0, 2);
            LblSource.Name = "LblSource";
            LblSource.Size = new Size(104, 17);
            LblSource.Text = "Source IP Address:";
            // 
            // lblAlertText
            // 
            lblAlertText.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblAlertText.AutoSize = true;
            lblAlertText.Location = new Point(12, 144);
            lblAlertText.Name = "lblAlertText";
            lblAlertText.Size = new Size(112, 13);
            lblAlertText.TabIndex = 6;
            lblAlertText.Text = "(Alert Text Goes Here)";
            // 
            // ChkShowRawLog
            // 
            ChkShowRawLog.AutoSize = true;
            ChkShowRawLog.Location = new Point(12, 7);
            ChkShowRawLog.Name = "ChkShowRawLog";
            ChkShowRawLog.Size = new Size(123, 17);
            ChkShowRawLog.TabIndex = 7;
            ChkShowRawLog.Text = "Show Raw Log Text";
            ChkShowRawLog.UseVisualStyleBackColor = true;
            // 
            // LogViewer
            // 
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 195);
            Controls.Add(ChkShowRawLog);
            Controls.Add(lblAlertText);
            Controls.Add(StatusStrip1);
            Controls.Add(BtnClose);
            Controls.Add(LogText);
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            MinimumSize = new Size(816, 173);
            Name = "LogViewer";
            Text = "Log Viewer";
            StatusStrip1.ResumeLayout(false);
            StatusStrip1.PerformLayout();
            Load += new EventHandler(Log_Viewer_Load);
            FormClosing += new FormClosingEventHandler(Log_Viewer_FormClosing);
            KeyUp += new KeyEventHandler(Log_Viewer_KeyUp);
            ResumeLayout(false);
            PerformLayout();

        }

        internal TextBox LogText;
        internal Button BtnClose;
        internal StatusStrip StatusStrip1;
        internal ToolStripStatusLabel LblLogDate;
        internal ToolStripStatusLabel LblSource;
        internal Label lblAlertText;
        internal CheckBox ChkShowRawLog;
    }
}