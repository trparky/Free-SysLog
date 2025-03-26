using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Free_SysLog
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class Alerts_History : Form
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
            AlertHistoryList = new DataGridView();
            AlertHistoryList.DoubleClick += new EventHandler(AlertHistoryList_DoubleClick);
            colTime = new DataGridViewTextBoxColumn();
            colAlertType = new DataGridViewTextBoxColumn();
            colAlert = new DataGridViewTextBoxColumn();
            BtnRefresh = new Button();
            BtnRefresh.Click += new EventHandler(BtnRefresh_Click);
            StatusStrip1 = new StatusStrip();
            lblNumberOfAlerts = new ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)AlertHistoryList).BeginInit();
            StatusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // AlertHistoryList
            // 
            AlertHistoryList.AllowUserToAddRows = false;
            AlertHistoryList.AllowUserToOrderColumns = true;
            AlertHistoryList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            AlertHistoryList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            AlertHistoryList.Columns.AddRange(new DataGridViewColumn[] { colTime, colAlertType, colAlert });
            AlertHistoryList.Location = new Point(12, 12);
            AlertHistoryList.Name = "AlertHistoryList";
            AlertHistoryList.ReadOnly = true;
            AlertHistoryList.RowHeadersVisible = false;
            AlertHistoryList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            AlertHistoryList.Size = new Size(1227, 381);
            AlertHistoryList.TabIndex = 0;
            // 
            // colTime
            // 
            colTime.HeaderText = "Time";
            colTime.Name = "colTime";
            colTime.ReadOnly = true;
            // 
            // colAlertType
            // 
            colAlertType.HeaderText = "Type";
            colAlertType.Name = "colAlertType";
            colAlertType.ReadOnly = true;
            // 
            // colAlert
            // 
            colAlert.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colAlert.HeaderText = "Alert Text";
            colAlert.Name = "colAlert";
            colAlert.ReadOnly = true;
            // 
            // BtnRefresh
            // 
            BtnRefresh.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BtnRefresh.Location = new Point(1139, 402);
            BtnRefresh.Name = "BtnRefresh";
            BtnRefresh.Size = new Size(100, 23);
            BtnRefresh.TabIndex = 1;
            BtnRefresh.Text = "&Refresh (F5)";
            BtnRefresh.UseVisualStyleBackColor = true;
            // 
            // StatusStrip1
            // 
            StatusStrip1.Items.AddRange(new ToolStripItem[] { lblNumberOfAlerts });
            StatusStrip1.Location = new Point(0, 428);
            StatusStrip1.Name = "StatusStrip1";
            StatusStrip1.Size = new Size(1251, 22);
            StatusStrip1.TabIndex = 2;
            StatusStrip1.Text = "StatusStrip1";
            // 
            // lblNumberOfAlerts
            // 
            lblNumberOfAlerts.Name = "lblNumberOfAlerts";
            lblNumberOfAlerts.Size = new Size(101, 17);
            lblNumberOfAlerts.Text = "Number of Alerts:";
            // 
            // Alerts_History
            // 
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1251, 450);
            Controls.Add(StatusStrip1);
            Controls.Add(BtnRefresh);
            Controls.Add(AlertHistoryList);
            KeyPreview = true;
            MinimumSize = new Size(1267, 489);
            Name = "Alerts_History";
            Text = "Alerts History";
            ((System.ComponentModel.ISupportInitialize)AlertHistoryList).EndInit();
            StatusStrip1.ResumeLayout(false);
            StatusStrip1.PerformLayout();
            Load += new EventHandler(Alerts_History_Load);
            ResizeEnd += new EventHandler(Alerts_History_ResizeEnd);
            KeyUp += new KeyEventHandler(Alerts_History_KeyUp);
            FormClosing += new FormClosingEventHandler(Alerts_History_FormClosing);
            ResumeLayout(false);
            PerformLayout();

        }

        internal DataGridView AlertHistoryList;
        internal DataGridViewTextBoxColumn colTime;
        internal DataGridViewTextBoxColumn colAlertType;
        internal DataGridViewTextBoxColumn colAlert;
        internal Button BtnRefresh;
        internal StatusStrip StatusStrip1;
        internal ToolStripStatusLabel lblNumberOfAlerts;
    }
}