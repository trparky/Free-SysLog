using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Free_SysLog
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class ViewLogBackups : Form
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
            FileList = new DataGridView();
            FileList.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(Logs_ColumnHeaderMouseClick);
            FileList.DoubleClick += new EventHandler(FileList_DoubleClick);
            FileList.Click += new EventHandler(FileList_Click);
            FileList.ColumnWidthChanged += new DataGridViewColumnEventHandler(FileList_ColumnWidthChanged);
            FileList.MouseDown += new MouseEventHandler(FileList_MouseDown);
            FileList.MouseUp += new MouseEventHandler(FileList_MouseUp);
            ColFileName = new DataGridViewTextBoxColumn();
            ColFileDate = new DataGridViewTextBoxColumn();
            ColFileSize = new DataGridViewTextBoxColumn();
            ContextMenuStrip1 = new ContextMenuStrip(components);
            ContextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(ContextMenuStrip1_Opening);
            DeleteToolStripMenuItem = new ToolStripMenuItem();
            DeleteToolStripMenuItem.Click += new EventHandler(DeleteToolStripMenuItem_Click);
            ViewToolStripMenuItem = new ToolStripMenuItem();
            ViewToolStripMenuItem.Click += new EventHandler(ViewToolStripMenuItem_Click);
            BtnView = new Button();
            BtnView.Click += new EventHandler(BtnView_Click);
            BtnDelete = new Button();
            BtnDelete.Click += new EventHandler(BtnDelete_Click);
            BtnRefresh = new Button();
            BtnRefresh.Click += new EventHandler(BtnRefresh_Click);
            StatusStrip1 = new StatusStrip();
            lblNumberOfFiles = new ToolStripStatusLabel();
            ChkCaseInsensitiveSearch = new CheckBox();
            ChkRegExSearch = new CheckBox();
            BtnSearch = new Button();
            BtnSearch.Click += new EventHandler(BtnSearch_Click);
            TxtSearchTerms = new TextBox();
            TxtSearchTerms.KeyDown += new KeyEventHandler(TxtSearchTerms_KeyDown);
            LblSearchLabel = new Label();
            lblTotalNumberOfLogs = new ToolStripStatusLabel();
            ChkShowHidden = new CheckBox();
            ChkShowHidden.Click += new EventHandler(ChkShowHidden_Click);
            colHidden = new DataGridViewTextBoxColumn();
            ChkShowHiddenAsGray = new CheckBox();
            ChkShowHiddenAsGray.Click += new EventHandler(ChkShowHiddenAsGray_Click);
            colEntryCount = new DataGridViewTextBoxColumn();
            HideToolStripMenuItem = new ToolStripMenuItem();
            HideToolStripMenuItem.Click += new EventHandler(HideToolStripMenuItem_Click);
            UnhideToolStripMenuItem = new ToolStripMenuItem();
            UnhideToolStripMenuItem.Click += new EventHandler(UnhideToolStripMenuItem_Click);
            ToolTip = new ToolTip(components);
            ChkIgnoreSearchResultsLimits = new CheckBox();
            ChkIgnoreSearchResultsLimits.Click += new EventHandler(ChkIgnoreSearchResultsLimits_Click);
            ChkLogFileDeletions = new CheckBox();
            ChkLogFileDeletions.Click += new EventHandler(ChkLogFileDeletions_Click);
            lblNumberOfHiddenFiles = new ToolStripStatusLabel();
            lblTotalNumberOfHiddenLogs = new ToolStripStatusLabel();
            LblTotalDiskSpace = new ToolStripStatusLabel();
            ContextMenuStrip1.SuspendLayout();
            StatusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)FileList).BeginInit();
            SuspendLayout();
            // 
            // FileList
            // 
            FileList.AllowUserToAddRows = false;
            FileList.AllowUserToDeleteRows = false;
            FileList.AllowUserToOrderColumns = true;
            FileList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            FileList.BackgroundColor = SystemColors.ControlLightLight;
            FileList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            FileList.Columns.AddRange(new DataGridViewColumn[] { ColFileName, ColFileDate, ColFileSize, colEntryCount, colHidden });
            FileList.ContextMenuStrip = ContextMenuStrip1;
            FileList.Location = new Point(13, 12);
            FileList.Name = "FileList";
            FileList.ReadOnly = true;
            FileList.RowHeadersVisible = false;
            FileList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            FileList.Size = new Size(929, 272);
            FileList.TabIndex = 36;
            // 
            // ColFileName
            // 
            ColFileName.HeaderText = "File Name";
            ColFileName.Name = "ColFileName";
            ColFileName.ReadOnly = true;
            ColFileName.Width = 240;
            // 
            // ColFileDate
            // 
            ColFileDate.HeaderText = "Creation Date";
            ColFileDate.Name = "ColFileDate";
            ColFileDate.ReadOnly = true;
            ColFileDate.Width = 240;
            // 
            // ColFileSize
            // 
            ColFileSize.HeaderText = "File Size";
            ColFileSize.Name = "ColFileSize";
            ColFileSize.ReadOnly = true;
            ColFileSize.Width = 240;
            // 
            // colEntryCount
            // 
            colEntryCount.HeaderText = "Entry Count";
            colEntryCount.Name = "colEntryCount";
            colEntryCount.ReadOnly = true;
            colEntryCount.Width = 125;
            // 
            // ContextMenuStrip1
            // 
            ContextMenuStrip1.Items.AddRange(new ToolStripItem[] { DeleteToolStripMenuItem, ViewToolStripMenuItem, HideToolStripMenuItem, UnhideToolStripMenuItem });
            ContextMenuStrip1.Name = "ContextMenuStrip1";
            ContextMenuStrip1.Size = new Size(147, 92);
            // 
            // DeleteToolStripMenuItem
            // 
            DeleteToolStripMenuItem.Name = "DeleteToolStripMenuItem";
            DeleteToolStripMenuItem.Size = new Size(146, 22);
            DeleteToolStripMenuItem.Text = "&Delete";
            // 
            // ViewToolStripMenuItem
            // 
            ViewToolStripMenuItem.Name = "ViewToolStripMenuItem";
            ViewToolStripMenuItem.Size = new Size(146, 22);
            ViewToolStripMenuItem.Text = "&View";
            // 
            // HideToolStripMenuItem
            // 
            HideToolStripMenuItem.Name = "HideToolStripMenuItem";
            HideToolStripMenuItem.Size = new Size(146, 22);
            HideToolStripMenuItem.Text = "Hide";
            // 
            // UnhideToolStripMenuItem
            // 
            UnhideToolStripMenuItem.Name = "UnhideToolStripMenuItem";
            UnhideToolStripMenuItem.Size = new Size(146, 22);
            UnhideToolStripMenuItem.Text = "Unhide/Show";
            // 
            // BtnView
            // 
            BtnView.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            BtnView.Enabled = false;
            BtnView.Location = new Point(14, 290);
            BtnView.Name = "BtnView";
            BtnView.Size = new Size(75, 23);
            BtnView.TabIndex = 2;
            BtnView.Text = "&View";
            BtnView.UseVisualStyleBackColor = true;
            // 
            // BtnDelete
            // 
            BtnDelete.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            BtnDelete.Enabled = false;
            BtnDelete.Location = new Point(95, 290);
            BtnDelete.Name = "BtnDelete";
            BtnDelete.Size = new Size(75, 23);
            BtnDelete.TabIndex = 3;
            BtnDelete.Text = "&Delete";
            BtnDelete.UseVisualStyleBackColor = true;
            // 
            // BtnRefresh
            // 
            BtnRefresh.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            BtnRefresh.Location = new Point(176, 290);
            BtnRefresh.Name = "BtnRefresh";
            BtnRefresh.Size = new Size(95, 23);
            BtnRefresh.TabIndex = 4;
            BtnRefresh.Text = "Re&fresh (F5)";
            BtnRefresh.UseVisualStyleBackColor = true;
            // 
            // StatusStrip1
            // 
            StatusStrip1.Items.AddRange(new ToolStripItem[] { lblNumberOfFiles, lblNumberOfHiddenFiles, lblTotalNumberOfLogs, lblTotalNumberOfHiddenLogs, LblTotalDiskSpace });
            StatusStrip1.Location = new Point(0, 342);
            StatusStrip1.Name = "StatusStrip1";
            StatusStrip1.Size = new Size(954, 22);
            StatusStrip1.TabIndex = 5;
            StatusStrip1.Text = "StatusStrip1";
            // 
            // lblNumberOfFiles
            // 
            lblNumberOfFiles.Name = "lblNumberOfFiles";
            lblNumberOfFiles.Size = new Size(94, 17);
            lblNumberOfFiles.Text = "Number of Files:";
            // 
            // ChkCaseInsensitiveSearch
            // 
            ChkCaseInsensitiveSearch.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            ChkCaseInsensitiveSearch.AutoSize = true;
            ChkCaseInsensitiveSearch.Checked = true;
            ChkCaseInsensitiveSearch.CheckState = CheckState.Checked;
            ChkCaseInsensitiveSearch.Location = new Point(390, 321);
            ChkCaseInsensitiveSearch.Name = "ChkCaseInsensitiveSearch";
            ChkCaseInsensitiveSearch.Size = new Size(109, 17);
            ChkCaseInsensitiveSearch.TabIndex = 33;
            ChkCaseInsensitiveSearch.Text = "Case Insensitive?";
            ChkCaseInsensitiveSearch.UseVisualStyleBackColor = true;
            // 
            // ChkRegExSearch
            // 
            ChkRegExSearch.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            ChkRegExSearch.AutoSize = true;
            ChkRegExSearch.Location = new Point(321, 321);
            ChkRegExSearch.Name = "ChkRegExSearch";
            ChkRegExSearch.Size = new Size(63, 17);
            ChkRegExSearch.TabIndex = 32;
            ChkRegExSearch.Text = "Regex?";
            ChkRegExSearch.UseVisualStyleBackColor = true;
            // 
            // BtnSearch
            // 
            BtnSearch.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            BtnSearch.Location = new Point(498, 317);
            BtnSearch.Name = "BtnSearch";
            BtnSearch.Size = new Size(52, 23);
            BtnSearch.TabIndex = 31;
            BtnSearch.Text = "&Search";
            BtnSearch.UseVisualStyleBackColor = true;
            // 
            // TxtSearchTerms
            // 
            TxtSearchTerms.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            TxtSearchTerms.Location = new Point(95, 319);
            TxtSearchTerms.Name = "TxtSearchTerms";
            TxtSearchTerms.Size = new Size(220, 20);
            TxtSearchTerms.TabIndex = 30;
            // 
            // LblSearchLabel
            // 
            LblSearchLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            LblSearchLabel.AutoSize = true;
            LblSearchLabel.Location = new Point(8, 322);
            LblSearchLabel.Name = "LblSearchLabel";
            LblSearchLabel.Size = new Size(81, 13);
            LblSearchLabel.TabIndex = 29;
            LblSearchLabel.Text = "Search All Logs";
            // 
            // lblTotalNumberOfLogs
            // 
            lblTotalNumberOfLogs.Margin = new Padding(25, 3, 0, 2);
            lblTotalNumberOfLogs.Name = "lblTotalNumberOfLogs";
            lblTotalNumberOfLogs.Size = new Size(124, 17);
            lblTotalNumberOfLogs.Text = "Total Number of Logs:";
            // 
            // ChkShowHidden
            // 
            ChkShowHidden.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            ChkShowHidden.AutoSize = true;
            ChkShowHidden.Location = new Point(277, 294);
            ChkShowHidden.Name = "ChkShowHidden";
            ChkShowHidden.Size = new Size(114, 17);
            ChkShowHidden.TabIndex = 34;
            ChkShowHidden.Text = "Show Hidden Files";
            ChkShowHidden.UseVisualStyleBackColor = true;
            // 
            // colHidden
            // 
            colHidden.HeaderText = "Hidden?";
            colHidden.Name = "colHidden";
            colHidden.ReadOnly = true;
            colHidden.Width = 60;
            // 
            // ChkShowHiddenAsGray
            // 
            ChkShowHiddenAsGray.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            ChkShowHiddenAsGray.AutoSize = true;
            ChkShowHiddenAsGray.Location = new Point(397, 294);
            ChkShowHiddenAsGray.Name = "ChkShowHiddenAsGray";
            ChkShowHiddenAsGray.Size = new Size(153, 17);
            ChkShowHiddenAsGray.TabIndex = 35;
            ChkShowHiddenAsGray.Text = "Show Hidden Files as Gray";
            ChkShowHiddenAsGray.UseVisualStyleBackColor = true;
            // 
            // lblNumberOfHiddenFiles
            // 
            lblNumberOfHiddenFiles.Margin = new Padding(25, 3, 0, 2);
            lblNumberOfHiddenFiles.Name = "lblNumberOfHiddenFiles";
            lblNumberOfHiddenFiles.Size = new Size(136, 17);
            lblNumberOfHiddenFiles.Text = "Number of Hidden Files:";
            lblNumberOfHiddenFiles.Visible = false;
            // 
            // lblTotalNumberOfHiddenLogs
            // 
            lblTotalNumberOfHiddenLogs.Margin = new Padding(25, 3, 0, 2);
            lblTotalNumberOfHiddenLogs.Name = "lblTotalNumberOfHiddenLogs";
            lblTotalNumberOfHiddenLogs.Size = new Size(138, 17);
            lblTotalNumberOfHiddenLogs.Text = "Number of Hidden Logs:";
            lblTotalNumberOfHiddenLogs.Visible = false;
            // 
            // LblTotalDiskSpace
            // 
            LblTotalDiskSpace.Margin = new Padding(25, 3, 0, 2);
            LblTotalDiskSpace.Name = "LblTotalDiskSpace";
            LblTotalDiskSpace.Size = new Size(123, 17);
            LblTotalDiskSpace.Text = "Total Disk Space Used:";
            // 
            // ChkIgnoreSearchResultsLimits
            // 
            ChkIgnoreSearchResultsLimits.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            ChkIgnoreSearchResultsLimits.AutoSize = true;
            ChkIgnoreSearchResultsLimits.Location = new Point(556, 321);
            ChkIgnoreSearchResultsLimits.Name = "ChkIgnoreSearchResultsLimits";
            ChkIgnoreSearchResultsLimits.Size = new Size(160, 17);
            ChkIgnoreSearchResultsLimits.TabIndex = 37;
            ChkIgnoreSearchResultsLimits.Text = "Ignore Search Results Limits";
            ToolTip.SetToolTip(ChkIgnoreSearchResultsLimits, "Warning: Enabling this could cause performance issues.");
            ChkIgnoreSearchResultsLimits.UseVisualStyleBackColor = true;
            // 
            // ChkLogFileDeletions
            // 
            ChkLogFileDeletions.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            ChkLogFileDeletions.AutoSize = true;
            ChkLogFileDeletions.Location = new Point(556, 294);
            ChkLogFileDeletions.Name = "ChkLogFileDeletions";
            ChkLogFileDeletions.Size = new Size(127, 17);
            ChkLogFileDeletions.TabIndex = 38;
            ChkLogFileDeletions.Text = "Log Deletions of Files";
            ChkLogFileDeletions.UseVisualStyleBackColor = true;
            // 
            // ViewLogBackups
            // 
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(954, 364);
            Controls.Add(ChkIgnoreSearchResultsLimits);
            Controls.Add(ChkShowHiddenAsGray);
            Controls.Add(ChkShowHidden);
            Controls.Add(ChkCaseInsensitiveSearch);
            Controls.Add(ChkRegExSearch);
            Controls.Add(BtnSearch);
            Controls.Add(TxtSearchTerms);
            Controls.Add(LblSearchLabel);
            Controls.Add(StatusStrip1);
            Controls.Add(BtnRefresh);
            Controls.Add(BtnDelete);
            Controls.Add(BtnView);
            Controls.Add(FileList);
            Controls.Add(ChkLogFileDeletions);
            KeyPreview = true;
            MaximizeBox = false;
            MinimizeBox = false;
            MinimumSize = new Size(970, 403);
            Name = "ViewLogBackups";
            Text = "View Log Backups";
            ContextMenuStrip1.ResumeLayout(false);
            StatusStrip1.ResumeLayout(false);
            StatusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)FileList).EndInit();
            Load += new EventHandler(ViewLogBackups_Load);
            KeyUp += new KeyEventHandler(ViewLogBackups_KeyUp);
            ResizeEnd += new EventHandler(ViewLogBackups_ResizeEnd);
            FormClosing += new FormClosingEventHandler(ViewLogBackups_FormClosing);
            ResumeLayout(false);
            PerformLayout();

        }

        internal DataGridView FileList;
        internal DataGridViewTextBoxColumn ColFileName;
        internal DataGridViewTextBoxColumn ColFileDate;
        internal DataGridViewTextBoxColumn ColFileSize;
        internal Button BtnView;
        internal Button BtnDelete;
        internal Button BtnRefresh;
        internal StatusStrip StatusStrip1;
        internal ToolStripStatusLabel lblNumberOfFiles;
        internal CheckBox ChkCaseInsensitiveSearch;
        internal CheckBox ChkRegExSearch;
        internal Button BtnSearch;
        internal TextBox TxtSearchTerms;
        internal Label LblSearchLabel;
        internal ContextMenuStrip ContextMenuStrip1;
        internal ToolStripMenuItem DeleteToolStripMenuItem;
        internal ToolStripMenuItem ViewToolStripMenuItem;
        internal ToolStripStatusLabel lblTotalNumberOfLogs;
        internal CheckBox ChkShowHidden;
        internal ToolStripMenuItem HideToolStripMenuItem;
        internal ToolStripMenuItem UnhideToolStripMenuItem;
        internal DataGridViewTextBoxColumn colHidden;
        internal CheckBox ChkShowHiddenAsGray;
        internal ToolStripStatusLabel lblNumberOfHiddenFiles;
        internal ToolStripStatusLabel lblTotalNumberOfHiddenLogs;
        internal ToolStripStatusLabel LblTotalDiskSpace;
        internal DataGridViewTextBoxColumn colEntryCount;
        internal ToolTip ToolTip;
        internal CheckBox ChkIgnoreSearchResultsLimits;
        internal CheckBox ChkLogFileDeletions;
    }
}