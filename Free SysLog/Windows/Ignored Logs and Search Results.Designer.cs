using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Free_SysLog
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class IgnoredLogsAndSearchResults : Form
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

        private new object parentForm;

        public IgnoredLogsAndSearchResults(object parentForm, SupportCode.IgnoreOrSearchWindowDisplayMode WindowDisplayMode)
        {
            InitializeComponent();
            this.parentForm = parentForm;

            _WindowDisplayMode = WindowDisplayMode;
            if (WindowDisplayMode == SupportCode.IgnoreOrSearchWindowDisplayMode.search)
                ChkColLogsAutoFill.Location = new Point(12, 382);
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
            Logs = new DataGridView();
            Logs.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(Logs_ColumnHeaderMouseClick);
            Logs.DoubleClick += new EventHandler(Logs_DoubleClick);
            Logs.ColumnWidthChanged += new DataGridViewColumnEventHandler(Logs_ColumnWidthChanged);
            Logs.MouseDown += new MouseEventHandler(Logs_MouseDown);
            Logs.MouseUp += new MouseEventHandler(Logs_MouseUp);
            colServerTime = new DataGridViewTextBoxColumn();
            ColTime = new DataGridViewTextBoxColumn();
            ColIPAddress = new DataGridViewTextBoxColumn();
            ColLog = new DataGridViewTextBoxColumn();
            ColAlerts = new DataGridViewTextBoxColumn();
            ColRemoteProcess = new DataGridViewTextBoxColumn();
            StatusStrip1 = new StatusStrip();
            LblCount = new ToolStripStatusLabel();
            ColFileName = new DataGridViewTextBoxColumn();
            BtnExport = new Button();
            BtnExport.Click += new EventHandler(BtnExport_Click);
            SaveFileDialog = new SaveFileDialog();
            BtnClearIgnoredLogs = new Button();
            BtnClearIgnoredLogs.Click += new EventHandler(BtnClearIgnoredLogs_Click);
            BtnViewMainWindow = new Button();
            BtnViewMainWindow.Click += new EventHandler(BtnViewMainWindow_Click);
            CreateAlertToolStripMenuItem = new ToolStripMenuItem();
            CreateAlertToolStripMenuItem.Click += new EventHandler(CreateAlertToolStripMenuItem_Click);
            LogsContextMenu = new ContextMenuStrip(components);
            LogsContextMenu.Opening += new System.ComponentModel.CancelEventHandler(LogsContextMenu_Opening);
            CopyLogTextToolStripMenuItem = new ToolStripMenuItem();
            CopyLogTextToolStripMenuItem.Click += new EventHandler(CopyLogTextToolStripMenuItem_Click);
            ChkCaseInsensitiveSearch = new CheckBox();
            ChkColLogsAutoFill = new CheckBox();
            ChkColLogsAutoFill.Click += new EventHandler(ChkColLogsAutoFill_Click);
            ChkRegExSearch = new CheckBox();
            BtnSearch = new Button();
            BtnSearch.Click += new EventHandler(BtnSearch_Click);
            TxtSearchTerms = new TextBox();
            TxtSearchTerms.KeyDown += new KeyEventHandler(TxtSearchTerms_KeyDown);
            LblSearchLabel = new Label();
            ExportSelectedLogsToolStripMenuItem = new ToolStripMenuItem();
            ExportSelectedLogsToolStripMenuItem.Click += new EventHandler(ExportSelectedLogsToolStripMenuItem_Click);
            colLogType = new DataGridViewTextBoxColumn();
            ColHostname = new DataGridViewTextBoxColumn();
            OpenLogFileForViewingToolStripMenuItem = new ToolStripMenuItem();
            OpenLogFileForViewingToolStripMenuItem.Click += new EventHandler(OpenLogFileForViewingToolStripMenuItem_Click);
            LogsLoadedInLabel = new ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)Logs).BeginInit();
            LogsContextMenu.SuspendLayout();
            StatusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // ColHostname
            // 
            ColHostname.HeaderText = "Hostname/Device Name";
            ColHostname.Name = "ColHostname";
            ColHostname.ReadOnly = true;
            ColHostname.SortMode = DataGridViewColumnSortMode.Programmatic;
            ColHostname.Width = 150;
            // 
            // colLogType
            // 
            colLogType.HeaderText = "Log Type";
            colLogType.Name = "colLogType";
            colLogType.ReadOnly = true;
            colLogType.Width = 200;
            // 
            // StatusStrip1
            // 
            StatusStrip1.Items.AddRange(new ToolStripItem[] { LblCount, LogsLoadedInLabel });
            StatusStrip1.Location = new Point(0, 403);
            StatusStrip1.Name = "StatusStrip1";
            StatusStrip1.Size = new Size(1152, 22);
            StatusStrip1.TabIndex = 5;
            StatusStrip1.Text = "StatusStrip1";
            // 
            // LblCount
            // 
            LblCount.Name = "LblCount";
            LblCount.Size = new Size(53, 17);
            LblCount.Text = "lblCount";
            // 
            // Logs
            // 
            Logs.AllowUserToAddRows = false;
            Logs.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            Logs.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            Logs.Columns.AddRange(new DataGridViewColumn[] { ColTime, colServerTime, colLogType, ColIPAddress, ColHostname, ColRemoteProcess, ColLog, ColAlerts, ColFileName });
            Logs.ContextMenuStrip = LogsContextMenu;
            Logs.Location = new Point(12, 12);
            Logs.Name = "Logs";
            Logs.ReadOnly = true;
            Logs.RowHeadersVisible = false;
            Logs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            Logs.Size = new Size(1128, 359);
            Logs.TabIndex = 19;
            // 
            // colServerTime
            // 
            colServerTime.HeaderText = "Server Time";
            colServerTime.Name = "colServerTime";
            colServerTime.ReadOnly = true;
            colServerTime.SortMode = DataGridViewColumnSortMode.Programmatic;
            colServerTime.ToolTipText = "The time on the server at which the log entry came in.";
            // 
            // ColTime
            // 
            ColTime.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            ColTime.HeaderCell.Style.Padding = new Padding(0, 0, 1, 0);
            ColTime.HeaderText = "Time";
            ColTime.Name = "ColTime";
            ColTime.ReadOnly = true;
            ColTime.SortMode = DataGridViewColumnSortMode.Programmatic;
            ColTime.ToolTipText = "The time at which the log entry came in.";
            // 
            // ColIPAddress
            // 
            ColIPAddress.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            ColIPAddress.HeaderCell.Style.Padding = new Padding(0, 0, 2, 0);
            ColIPAddress.HeaderText = "IP Address";
            ColIPAddress.Name = "ColIPAddress";
            ColIPAddress.ReadOnly = true;
            ColIPAddress.SortMode = DataGridViewColumnSortMode.Programmatic;
            ColIPAddress.ToolTipText = "The IP address of the system from which the log came from.";
            // 
            // ColFileName
            // 
            ColFileName.HeaderText = "File Name";
            ColFileName.Name = "ColFileName";
            ColFileName.ReadOnly = true;
            ColFileName.SortMode = DataGridViewColumnSortMode.Programmatic;
            ColFileName.Visible = false;
            ColFileName.Width = 150;
            // 
            // ColLog
            // 
            ColLog.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            ColLog.HeaderText = "Log";
            ColLog.Name = "ColLog";
            ColLog.ReadOnly = true;
            ColLog.SortMode = DataGridViewColumnSortMode.Programmatic;
            ColLog.ToolTipText = "The text contents of the log.";
            // 
            // ColAlerts
            // 
            ColAlerts.HeaderText = "Alerted";
            ColAlerts.Name = "ColAlerts";
            ColAlerts.ReadOnly = true;
            ColAlerts.SortMode = DataGridViewColumnSortMode.Programmatic;
            ColAlerts.ToolTipText = "True or False. Indicates if the log entry triggered an alert from this program.";
            ColAlerts.Width = 50;
            // 
            // ColRemoteProcess
            // 
            ColRemoteProcess.HeaderText = "Remote Process";
            ColRemoteProcess.Name = "ColRemoteProcess";
            ColRemoteProcess.ReadOnly = true;
            ColRemoteProcess.SortMode = DataGridViewColumnSortMode.Programmatic;
            ColRemoteProcess.Width = 150;
            // 
            // BtnExport
            // 
            BtnExport.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BtnExport.Location = new Point(1065, 377);
            BtnExport.Name = "BtnExport";
            BtnExport.Size = new Size(75, 23);
            BtnExport.TabIndex = 20;
            BtnExport.Text = "Export";
            BtnExport.UseVisualStyleBackColor = true;
            // 
            // BtnClearIgnoredLogs
            // 
            BtnClearIgnoredLogs.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BtnClearIgnoredLogs.Location = new Point(923, 377);
            BtnClearIgnoredLogs.Name = "BtnClearIgnoredLogs";
            BtnClearIgnoredLogs.Size = new Size(136, 23);
            BtnClearIgnoredLogs.TabIndex = 21;
            BtnClearIgnoredLogs.Text = "Clear Ignored Logs";
            BtnClearIgnoredLogs.UseVisualStyleBackColor = true;
            BtnClearIgnoredLogs.Visible = false;
            // 
            // LogsContextMenu
            // 
            LogsContextMenu.Items.AddRange(new ToolStripItem[] { CopyLogTextToolStripMenuItem, CreateAlertToolStripMenuItem, OpenLogFileForViewingToolStripMenuItem, ExportSelectedLogsToolStripMenuItem });
            LogsContextMenu.Name = "LogsContextMenu";
            LogsContextMenu.Size = new Size(211, 92);
            // 
            // CopyLogTextToolStripMenuItem
            // 
            CopyLogTextToolStripMenuItem.Name = "CopyLogTextToolStripMenuItem";
            CopyLogTextToolStripMenuItem.Size = new Size(210, 22);
            CopyLogTextToolStripMenuItem.Text = "Copy Log Text";
            // 
            // BtnViewMainWindow
            // 
            BtnViewMainWindow.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            BtnViewMainWindow.Enabled = false;
            BtnViewMainWindow.Location = new Point(12, 377);
            BtnViewMainWindow.Name = "BtnViewMainWindow";
            BtnViewMainWindow.Size = new Size(117, 23);
            BtnViewMainWindow.TabIndex = 22;
            BtnViewMainWindow.Text = "View Main Window";
            BtnViewMainWindow.UseVisualStyleBackColor = true;
            BtnViewMainWindow.Visible = false;
            // 
            // CreateAlertToolStripMenuItem
            // 
            CreateAlertToolStripMenuItem.Name = "CreateAlertToolStripMenuItem";
            CreateAlertToolStripMenuItem.Size = new Size(210, 22);
            CreateAlertToolStripMenuItem.Text = "Create Alert";
            // 
            // ChkCaseInsensitiveSearch
            // 
            ChkCaseInsensitiveSearch.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            ChkCaseInsensitiveSearch.AutoSize = true;
            ChkCaseInsensitiveSearch.Checked = true;
            ChkCaseInsensitiveSearch.CheckState = CheckState.Checked;
            ChkCaseInsensitiveSearch.Location = new Point(431, 382);
            ChkCaseInsensitiveSearch.Name = "ChkCaseInsensitiveSearch";
            ChkCaseInsensitiveSearch.Size = new Size(109, 17);
            ChkCaseInsensitiveSearch.TabIndex = 28;
            ChkCaseInsensitiveSearch.Text = "Case Insensitive?";
            ChkCaseInsensitiveSearch.UseVisualStyleBackColor = true;
            ChkCaseInsensitiveSearch.Visible = false;
            // 
            // ChkColLogsAutoFill
            // 
            ChkColLogsAutoFill.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            ChkColLogsAutoFill.AutoSize = true;
            ChkColLogsAutoFill.Location = new Point(597, 382);
            ChkColLogsAutoFill.Name = "ChkColLogsAutoFill";
            ChkColLogsAutoFill.Size = new Size(124, 17);
            ChkColLogsAutoFill.TabIndex = 29;
            ChkColLogsAutoFill.Text = "Logs Column AutoFill";
            ChkColLogsAutoFill.UseVisualStyleBackColor = true;
            // 
            // ChkRegExSearch
            // 
            ChkRegExSearch.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            ChkRegExSearch.AutoSize = true;
            ChkRegExSearch.Location = new Point(362, 382);
            ChkRegExSearch.Name = "ChkRegExSearch";
            ChkRegExSearch.Size = new Size(63, 17);
            ChkRegExSearch.TabIndex = 27;
            ChkRegExSearch.Text = "Regex?";
            ChkRegExSearch.UseVisualStyleBackColor = true;
            ChkRegExSearch.Visible = false;
            // 
            // BtnSearch
            // 
            BtnSearch.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            BtnSearch.Location = new Point(539, 378);
            BtnSearch.Name = "BtnSearch";
            BtnSearch.Size = new Size(52, 23);
            BtnSearch.TabIndex = 26;
            BtnSearch.Text = "Search";
            BtnSearch.UseVisualStyleBackColor = true;
            BtnSearch.Visible = false;
            // 
            // TxtSearchTerms
            // 
            TxtSearchTerms.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            TxtSearchTerms.Location = new Point(208, 379);
            TxtSearchTerms.Name = "TxtSearchTerms";
            TxtSearchTerms.Size = new Size(148, 20);
            TxtSearchTerms.TabIndex = 25;
            TxtSearchTerms.Visible = false;
            // 
            // LblSearchLabel
            // 
            LblSearchLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            LblSearchLabel.AutoSize = true;
            LblSearchLabel.Location = new Point(135, 382);
            LblSearchLabel.Name = "LblSearchLabel";
            LblSearchLabel.Size = new Size(67, 13);
            LblSearchLabel.TabIndex = 24;
            LblSearchLabel.Text = "Search Logs";
            LblSearchLabel.Visible = false;
            // 
            // OpenLogFileForViewingToolStripMenuItem
            // 
            OpenLogFileForViewingToolStripMenuItem.Name = "OpenLogFileForViewingToolStripMenuItem";
            OpenLogFileForViewingToolStripMenuItem.Size = new Size(210, 22);
            OpenLogFileForViewingToolStripMenuItem.Text = "Open Log File for Viewing";
            OpenLogFileForViewingToolStripMenuItem.Visible = false;
            // 
            // ExportSelectedLogsToolStripMenuItem
            // 
            ExportSelectedLogsToolStripMenuItem.Name = "ExportSelectedLogsToolStripMenuItem";
            ExportSelectedLogsToolStripMenuItem.Size = new Size(210, 22);
            ExportSelectedLogsToolStripMenuItem.Text = "Export Selected Logs";
            // 
            // LogsLoadedInLabel
            // 
            LogsLoadedInLabel.Margin = new Padding(50, 3, 0, 2);
            LogsLoadedInLabel.Name = "LogsLoadedInLabel";
            LogsLoadedInLabel.Size = new Size(90, 17);
            LogsLoadedInLabel.Text = "Logs Loaded In:";
            LogsLoadedInLabel.Visible = false;
            // 
            // IgnoredLogsAndSearchResults
            // 
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1152, 425);
            Controls.Add(ChkColLogsAutoFill);
            Controls.Add(BtnViewMainWindow);
            Controls.Add(BtnExport);
            Controls.Add(Logs);
            Controls.Add(StatusStrip1);
            Controls.Add(BtnClearIgnoredLogs);
            Controls.Add(ChkCaseInsensitiveSearch);
            Controls.Add(ChkRegExSearch);
            Controls.Add(BtnSearch);
            Controls.Add(TxtSearchTerms);
            Controls.Add(LblSearchLabel);
            MaximizeBox = false;
            MinimumSize = new Size(1168, 464);
            Name = "IgnoredLogsAndSearchResults";
            Text = "Ignored Logs";
            StatusStrip1.ResumeLayout(false);
            StatusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)Logs).EndInit();
            LogsContextMenu.ResumeLayout(false);
            ResizeBegin += new EventHandler(Ignored_Logs_and_Search_Results_ResizeBegin);
            ResizeEnd += new EventHandler(Ignored_Logs_and_Search_Results_ResizeEnd);
            Closing += new System.ComponentModel.CancelEventHandler(Ignored_Logs_and_Search_Results_Closing);
            Load += new EventHandler(Ignored_Logs_and_Search_Results_Load);
            ResumeLayout(false);
            PerformLayout();

        }
        internal StatusStrip StatusStrip1;
        internal ToolStripStatusLabel LblCount;
        internal DataGridView Logs;
        internal DataGridViewTextBoxColumn ColTime;
        internal DataGridViewTextBoxColumn colServerTime;
        internal DataGridViewTextBoxColumn ColIPAddress;
        internal DataGridViewTextBoxColumn ColLog;
        internal Button BtnExport;
        internal SaveFileDialog SaveFileDialog;
        internal Button BtnClearIgnoredLogs;
        internal Button BtnViewMainWindow;
        internal ContextMenuStrip LogsContextMenu;
        internal ToolStripMenuItem CopyLogTextToolStripMenuItem;
        internal ToolStripMenuItem CreateAlertToolStripMenuItem;
        internal DataGridViewTextBoxColumn ColAlerts;
        internal DataGridViewTextBoxColumn ColRemoteProcess;
        internal DataGridViewTextBoxColumn ColFileName;
        internal CheckBox ChkCaseInsensitiveSearch;
        internal CheckBox ChkRegExSearch;
        internal Button BtnSearch;
        internal TextBox TxtSearchTerms;
        internal Label LblSearchLabel;
        internal ToolStripMenuItem OpenLogFileForViewingToolStripMenuItem;
        internal DataGridViewTextBoxColumn colLogType;
        internal DataGridViewTextBoxColumn ColHostname;
        internal ToolStripMenuItem ExportSelectedLogsToolStripMenuItem;
        internal ToolStripStatusLabel LogsLoadedInLabel;
        internal CheckBox ChkColLogsAutoFill;
    }
}