using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Free_SysLog
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class Replacements : Form
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
            ReplacementsListView = new ListView();
            ReplacementsListView.KeyUp += new KeyEventHandler(ReplacementsListView_KeyUp);
            ReplacementsListView.Click += new EventHandler(ReplacementsListView_Click);
            ReplacementsListView.DoubleClick += new EventHandler(ReplacementsListView_DoubleClick);
            ReplacementsListView.ColumnWidthChanged += new ColumnWidthChangedEventHandler(ReplacementsListView_ColumnWidthChanged);
            Replace = new ColumnHeader();
            ReplaceWith = new ColumnHeader();
            Regex = new ColumnHeader();
            CaseSensitive = new ColumnHeader();
            ColEnabled = new ColumnHeader();
            ListViewMenu = new ContextMenuStrip(components);
            ListViewMenu.Opening += new System.ComponentModel.CancelEventHandler(ListViewMenu_Opening);
            EnableDisableToolStripMenuItem = new ToolStripMenuItem();
            EnableDisableToolStripMenuItem.Click += new EventHandler(EnableDisableToolStripMenuItem_Click);
            BtnAdd = new Button();
            BtnAdd.Click += new EventHandler(BtnAdd_Click);
            BtnDelete = new Button();
            BtnDelete.Click += new EventHandler(BtnDelete_Click);
            BtnEdit = new Button();
            BtnEdit.Click += new EventHandler(BtnEdit_Click);
            BtnEnableDisable = new Button();
            BtnEnableDisable.Click += new EventHandler(BtnEnableDisable_Click);
            BtnExport = new Button();
            BtnExport.Click += new EventHandler(BtnExport_Click);
            BtnImport = new Button();
            BtnImport.Click += new EventHandler(BtnImport_Click);
            btnDeleteAll = new Button();
            btnDeleteAll.Click += new EventHandler(btnDeleteAll_Click);
            BtnDown = new Button();
            BtnDown.Click += new EventHandler(BtnDown_Click);
            BtnUp = new Button();
            BtnUp.Click += new EventHandler(BtnUp_Click);
            SeparatingLine = new Label();
            ChkEnabled = new CheckBox();
            ChkCaseSensitive = new CheckBox();
            ChkRegex = new CheckBox();
            TxtReplaceWith = new TextBox();
            Label2 = new Label();
            TxtReplace = new TextBox();
            Label1 = new Label();
            Label3 = new Label();
            BtnCancel = new Button();
            BtnCancel.Click += new EventHandler(BtnCancel_Click);
            ListViewMenu.SuspendLayout();
            SuspendLayout();
            // 
            // ReplacementsListView
            // 
            ReplacementsListView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            ReplacementsListView.Columns.AddRange(new ColumnHeader[] { Replace, ReplaceWith, Regex, CaseSensitive, ColEnabled });
            ReplacementsListView.ContextMenuStrip = ListViewMenu;
            ReplacementsListView.FullRowSelect = true;
            ReplacementsListView.HideSelection = false;
            ReplacementsListView.Location = new Point(12, 12);
            ReplacementsListView.Name = "ReplacementsListView";
            ReplacementsListView.Size = new Size(915, 363);
            ReplacementsListView.TabIndex = 4;
            ReplacementsListView.UseCompatibleStateImageBehavior = false;
            ReplacementsListView.View = View.Details;
            // 
            // Replace
            // 
            Replace.Text = "Replace";
            Replace.Width = 345;
            // 
            // ReplaceWith
            // 
            ReplaceWith.Text = "With";
            ReplaceWith.Width = 345;
            // 
            // Regex
            // 
            Regex.Text = "Regex";
            // 
            // CaseSensitive
            // 
            CaseSensitive.Text = "Case Sensitive";
            CaseSensitive.Width = 91;
            // 
            // ColEnabled
            // 
            ColEnabled.Text = "Enabled";
            // 
            // ListViewMenu
            // 
            ListViewMenu.Items.AddRange(new ToolStripItem[] { EnableDisableToolStripMenuItem });
            ListViewMenu.Name = "ContextMenuStrip1";
            ListViewMenu.Size = new Size(153, 26);
            // 
            // EnableDisableToolStripMenuItem
            // 
            EnableDisableToolStripMenuItem.Name = "EnableDisableToolStripMenuItem";
            EnableDisableToolStripMenuItem.Size = new Size(152, 22);
            EnableDisableToolStripMenuItem.Text = "Enable/Disable";
            // 
            // BtnAdd
            // 
            BtnAdd.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            BtnAdd.Location = new Point(12, 521);
            BtnAdd.Name = "BtnAdd";
            BtnAdd.Size = new Size(75, 23);
            BtnAdd.TabIndex = 5;
            BtnAdd.Text = "Add";
            BtnAdd.UseVisualStyleBackColor = true;
            // 
            // BtnDelete
            // 
            BtnDelete.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            BtnDelete.Enabled = false;
            BtnDelete.Location = new Point(12, 381);
            BtnDelete.Name = "BtnDelete";
            BtnDelete.Size = new Size(75, 23);
            BtnDelete.TabIndex = 6;
            BtnDelete.Text = "Delete";
            BtnDelete.UseVisualStyleBackColor = true;
            // 
            // BtnEdit
            // 
            BtnEdit.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            BtnEdit.Enabled = false;
            BtnEdit.Location = new Point(93, 381);
            BtnEdit.Name = "BtnEdit";
            BtnEdit.Size = new Size(75, 23);
            BtnEdit.TabIndex = 7;
            BtnEdit.Text = "Edit";
            BtnEdit.UseVisualStyleBackColor = true;
            // 
            // BtnEnableDisable
            // 
            BtnEnableDisable.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            BtnEnableDisable.Enabled = false;
            BtnEnableDisable.Location = new Point(174, 381);
            BtnEnableDisable.Name = "BtnEnableDisable";
            BtnEnableDisable.Size = new Size(75, 23);
            BtnEnableDisable.TabIndex = 9;
            BtnEnableDisable.Text = "Disable";
            BtnEnableDisable.UseVisualStyleBackColor = true;
            // 
            // BtnExport
            // 
            BtnExport.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BtnExport.Location = new Point(801, 381);
            BtnExport.Name = "BtnExport";
            BtnExport.Size = new Size(75, 23);
            BtnExport.TabIndex = 14;
            BtnExport.Text = "Export";
            BtnExport.UseVisualStyleBackColor = true;
            // 
            // BtnImport
            // 
            BtnImport.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BtnImport.Location = new Point(882, 381);
            BtnImport.Name = "BtnImport";
            BtnImport.Size = new Size(75, 23);
            BtnImport.TabIndex = 13;
            BtnImport.Text = "Import";
            BtnImport.UseVisualStyleBackColor = true;
            // 
            // btnDeleteAll
            // 
            btnDeleteAll.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnDeleteAll.Location = new Point(255, 381);
            btnDeleteAll.Name = "btnDeleteAll";
            btnDeleteAll.Size = new Size(75, 23);
            btnDeleteAll.TabIndex = 17;
            btnDeleteAll.Text = "Delete All";
            btnDeleteAll.UseVisualStyleBackColor = true;
            // 
            // BtnDown
            // 
            BtnDown.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            BtnDown.Location = new Point(933, 352);
            BtnDown.Name = "BtnDown";
            BtnDown.Size = new Size(24, 23);
            BtnDown.TabIndex = 19;
            BtnDown.Text = "▼";
            BtnDown.UseVisualStyleBackColor = true;
            // 
            // BtnUp
            // 
            BtnUp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            BtnUp.Location = new Point(933, 12);
            BtnUp.Name = "BtnUp";
            BtnUp.Size = new Size(24, 23);
            BtnUp.TabIndex = 18;
            BtnUp.Text = "▲";
            BtnUp.UseVisualStyleBackColor = true;
            // 
            // SeparatingLine
            // 
            SeparatingLine.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            SeparatingLine.BackColor = Color.Black;
            SeparatingLine.Location = new Point(-1, 413);
            SeparatingLine.Name = "SeparatingLine";
            SeparatingLine.Size = new Size(971, 1);
            SeparatingLine.TabIndex = 26;
            // 
            // ChkEnabled
            // 
            ChkEnabled.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            ChkEnabled.AutoSize = true;
            ChkEnabled.Checked = true;
            ChkEnabled.CheckState = CheckState.Checked;
            ChkEnabled.Location = new Point(714, 498);
            ChkEnabled.Name = "ChkEnabled";
            ChkEnabled.Size = new Size(71, 17);
            ChkEnabled.TabIndex = 33;
            ChkEnabled.Text = "Enabled?";
            ChkEnabled.UseVisualStyleBackColor = true;
            // 
            // ChkCaseSensitive
            // 
            ChkCaseSensitive.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            ChkCaseSensitive.AutoSize = true;
            ChkCaseSensitive.Location = new Point(539, 498);
            ChkCaseSensitive.Name = "ChkCaseSensitive";
            ChkCaseSensitive.Size = new Size(102, 17);
            ChkCaseSensitive.TabIndex = 32;
            ChkCaseSensitive.Text = "Case Sensitive?";
            ChkCaseSensitive.UseVisualStyleBackColor = true;
            // 
            // ChkRegex
            // 
            ChkRegex.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            ChkRegex.AutoSize = true;
            ChkRegex.Location = new Point(12, 498);
            ChkRegex.Name = "ChkRegex";
            ChkRegex.Size = new Size(478, 17);
            ChkRegex.TabIndex = 31;
            ChkRegex.Text = "Regex? (Be careful with Regex, a broken regex pattern could cause the program to " + "malfunction)";
            ChkRegex.UseVisualStyleBackColor = true;
            // 
            // TxtReplaceWith
            // 
            TxtReplaceWith.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            TxtReplaceWith.Location = new Point(62, 472);
            TxtReplaceWith.Name = "TxtReplaceWith";
            TxtReplaceWith.Size = new Size(895, 20);
            TxtReplaceWith.TabIndex = 30;
            // 
            // Label2
            // 
            Label2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            Label2.AutoSize = true;
            Label2.Location = new Point(27, 475);
            Label2.Name = "Label2";
            Label2.Size = new Size(29, 13);
            Label2.TabIndex = 29;
            Label2.Text = "With";
            // 
            // TxtReplace
            // 
            TxtReplace.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            TxtReplace.Location = new Point(62, 446);
            TxtReplace.Name = "TxtReplace";
            TxtReplace.Size = new Size(895, 20);
            TxtReplace.TabIndex = 28;
            // 
            // Label1
            // 
            Label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            Label1.AutoSize = true;
            Label1.Location = new Point(9, 449);
            Label1.Name = "Label1";
            Label1.Size = new Size(47, 13);
            Label1.TabIndex = 27;
            Label1.Text = "Replace";
            // 
            // Label3
            // 
            Label3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            Label3.AutoSize = true;
            Label3.Location = new Point(17, 424);
            Label3.Name = "Label3";
            Label3.Size = new Size(92, 13);
            Label3.TabIndex = 34;
            Label3.Text = "Add Replacement";
            // 
            // BtnCancel
            // 
            BtnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            BtnCancel.Location = new Point(93, 521);
            BtnCancel.Name = "BtnCancel";
            BtnCancel.Size = new Size(75, 23);
            BtnCancel.TabIndex = 46;
            BtnCancel.Text = "Cancel";
            BtnCancel.UseVisualStyleBackColor = true;
            // 
            // Replacements
            // 
            AutoScaleDimensions = new SizeF(6.0f, 13.0f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(969, 553);
            Controls.Add(BtnCancel);
            Controls.Add(Label3);
            Controls.Add(ChkEnabled);
            Controls.Add(ChkCaseSensitive);
            Controls.Add(ChkRegex);
            Controls.Add(TxtReplaceWith);
            Controls.Add(Label2);
            Controls.Add(TxtReplace);
            Controls.Add(Label1);
            Controls.Add(SeparatingLine);
            Controls.Add(BtnDown);
            Controls.Add(BtnUp);
            Controls.Add(btnDeleteAll);
            Controls.Add(BtnExport);
            Controls.Add(BtnImport);
            Controls.Add(BtnEnableDisable);
            Controls.Add(BtnEdit);
            Controls.Add(BtnDelete);
            Controls.Add(BtnAdd);
            Controls.Add(ReplacementsListView);
            KeyPreview = true;
            MinimumSize = new Size(985, 592);
            Name = "Replacements";
            Text = "Replacements";
            ListViewMenu.ResumeLayout(false);
            Load += new EventHandler(Replacements_Load);
            Closing += new System.ComponentModel.CancelEventHandler(Replacements_Closing);
            ResizeEnd += new EventHandler(Replacements_ResizeEnd);
            KeyUp += new KeyEventHandler(Replacements_KeyUp);
            ResumeLayout(false);
            PerformLayout();

        }

        internal ListView ReplacementsListView;
        internal ColumnHeader Replace;
        internal ColumnHeader ReplaceWith;
        internal ColumnHeader Regex;
        internal Button BtnAdd;
        internal Button BtnDelete;
        internal ColumnHeader CaseSensitive;
        internal Button BtnEdit;
        internal ColumnHeader ColEnabled;
        internal ContextMenuStrip ListViewMenu;
        internal ToolStripMenuItem EnableDisableToolStripMenuItem;
        internal Button BtnEnableDisable;
        internal Button BtnExport;
        internal Button BtnImport;
        internal Button btnDeleteAll;
        internal Button BtnDown;
        internal Button BtnUp;
        internal Label SeparatingLine;
        internal CheckBox ChkEnabled;
        internal CheckBox ChkCaseSensitive;
        internal CheckBox ChkRegex;
        internal TextBox TxtReplaceWith;
        internal Label Label2;
        internal TextBox TxtReplace;
        internal Label Label1;
        internal Label Label3;
        internal Button BtnCancel;
    }
}