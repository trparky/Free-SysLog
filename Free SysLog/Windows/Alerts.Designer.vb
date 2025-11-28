<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Alerts
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Alerts))
        Me.BtnEdit = New System.Windows.Forms.Button()
        Me.AlertsListView = New System.Windows.Forms.ListView()
        Me.AlertLogText = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.AlertText = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Regex = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.CaseSensitive = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.AlertTypeColumn = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colLimit = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColEnabled = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ListViewMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.EnableDisableToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BtnDelete = New System.Windows.Forms.Button()
        Me.BtnAdd = New System.Windows.Forms.Button()
        Me.BtnEnableDisable = New System.Windows.Forms.Button()
        Me.BtnExport = New System.Windows.Forms.Button()
        Me.BtnImport = New System.Windows.Forms.Button()
        Me.BtnDeleteAll = New System.Windows.Forms.Button()
        Me.BtnDown = New System.Windows.Forms.Button()
        Me.BtnUp = New System.Windows.Forms.Button()
        Me.SeparatingLine = New System.Windows.Forms.Label()
        Me.lblRegExBackReferences = New System.Windows.Forms.Label()
        Me.ChkEnabled = New System.Windows.Forms.CheckBox()
        Me.IconPictureBox = New System.Windows.Forms.PictureBox()
        Me.AlertTypeComboBox = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TxtAlertText = New System.Windows.Forms.TextBox()
        Me.ChkCaseSensitive = New System.Windows.Forms.CheckBox()
        Me.ChkRegex = New System.Windows.Forms.CheckBox()
        Me.TxtLogText = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.BtnCancel = New System.Windows.Forms.Button()
        Me.ChkLimited = New System.Windows.Forms.CheckBox()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.ListViewMenu.SuspendLayout()
        CType(Me.IconPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'BtnEdit
        '
        Me.BtnEdit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnEdit.Enabled = False
        Me.BtnEdit.Location = New System.Drawing.Point(83, 248)
        Me.BtnEdit.Name = "BtnEdit"
        Me.BtnEdit.Size = New System.Drawing.Size(75, 23)
        Me.BtnEdit.TabIndex = 12
        Me.BtnEdit.Text = "Edit"
        Me.BtnEdit.UseVisualStyleBackColor = True
        '
        'AlertsListView
        '
        Me.AlertsListView.AllowColumnReorder = True
        Me.AlertsListView.AllowDrop = True
        Me.AlertsListView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AlertsListView.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.AlertLogText, Me.AlertText, Me.Regex, Me.CaseSensitive, Me.AlertTypeColumn, Me.colLimit, Me.ColEnabled})
        Me.AlertsListView.ContextMenuStrip = Me.ListViewMenu
        Me.AlertsListView.FullRowSelect = True
        Me.AlertsListView.HideSelection = False
        Me.AlertsListView.Location = New System.Drawing.Point(12, 12)
        Me.AlertsListView.Name = "AlertsListView"
        Me.AlertsListView.Size = New System.Drawing.Size(933, 230)
        Me.AlertsListView.TabIndex = 11
        Me.AlertsListView.UseCompatibleStateImageBehavior = False
        Me.AlertsListView.View = System.Windows.Forms.View.Details
        '
        'AlertLogText
        '
        Me.AlertLogText.Text = "Alert Log Text"
        Me.AlertLogText.Width = 308
        '
        'AlertText
        '
        Me.AlertText.Text = "Alert Text"
        Me.AlertText.Width = 257
        '
        'Regex
        '
        Me.Regex.Text = "Regex"
        '
        'CaseSensitive
        '
        Me.CaseSensitive.Text = "Case Sensitive"
        Me.CaseSensitive.Width = 91
        '
        'AlertTypeColumn
        '
        Me.AlertTypeColumn.Text = "Alert Type"
        Me.AlertTypeColumn.Width = 90
        '
        'colLimit
        '
        Me.colLimit.DisplayIndex = 6
        Me.colLimit.Text = "Limited"
        '
        'ColEnabled
        '
        Me.ColEnabled.DisplayIndex = 5
        Me.ColEnabled.Text = "Enabled"
        '
        'ListViewMenu
        '
        Me.ListViewMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EnableDisableToolStripMenuItem})
        Me.ListViewMenu.Name = "ContextMenuStrip1"
        Me.ListViewMenu.Size = New System.Drawing.Size(153, 26)
        '
        'EnableDisableToolStripMenuItem
        '
        Me.EnableDisableToolStripMenuItem.Name = "EnableDisableToolStripMenuItem"
        Me.EnableDisableToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.EnableDisableToolStripMenuItem.Text = "Enable/Disable"
        '
        'BtnDelete
        '
        Me.BtnDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnDelete.Enabled = False
        Me.BtnDelete.Location = New System.Drawing.Point(12, 248)
        Me.BtnDelete.Name = "BtnDelete"
        Me.BtnDelete.Size = New System.Drawing.Size(65, 23)
        Me.BtnDelete.TabIndex = 10
        Me.BtnDelete.Text = "Delete"
        Me.BtnDelete.UseVisualStyleBackColor = True
        '
        'BtnAdd
        '
        Me.BtnAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnAdd.Location = New System.Drawing.Point(12, 424)
        Me.BtnAdd.Name = "BtnAdd"
        Me.BtnAdd.Size = New System.Drawing.Size(65, 23)
        Me.BtnAdd.TabIndex = 9
        Me.BtnAdd.Text = "Add"
        Me.BtnAdd.UseVisualStyleBackColor = True
        '
        'BtnEnableDisable
        '
        Me.BtnEnableDisable.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnEnableDisable.Enabled = False
        Me.BtnEnableDisable.Location = New System.Drawing.Point(164, 248)
        Me.BtnEnableDisable.Name = "BtnEnableDisable"
        Me.BtnEnableDisable.Size = New System.Drawing.Size(75, 23)
        Me.BtnEnableDisable.TabIndex = 13
        Me.BtnEnableDisable.Text = "Disable"
        Me.BtnEnableDisable.UseVisualStyleBackColor = True
        '
        'BtnExport
        '
        Me.BtnExport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnExport.Location = New System.Drawing.Point(819, 248)
        Me.BtnExport.Name = "BtnExport"
        Me.BtnExport.Size = New System.Drawing.Size(75, 23)
        Me.BtnExport.TabIndex = 15
        Me.BtnExport.Text = "Export"
        Me.BtnExport.UseVisualStyleBackColor = True
        '
        'BtnImport
        '
        Me.BtnImport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnImport.Location = New System.Drawing.Point(900, 248)
        Me.BtnImport.Name = "BtnImport"
        Me.BtnImport.Size = New System.Drawing.Size(75, 23)
        Me.BtnImport.TabIndex = 14
        Me.BtnImport.Text = "Import"
        Me.BtnImport.UseVisualStyleBackColor = True
        '
        'BtnDeleteAll
        '
        Me.BtnDeleteAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnDeleteAll.Location = New System.Drawing.Point(245, 248)
        Me.BtnDeleteAll.Name = "BtnDeleteAll"
        Me.BtnDeleteAll.Size = New System.Drawing.Size(75, 23)
        Me.BtnDeleteAll.TabIndex = 16
        Me.BtnDeleteAll.Text = "Delete All"
        Me.BtnDeleteAll.UseVisualStyleBackColor = True
        '
        'BtnDown
        '
        Me.BtnDown.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnDown.Location = New System.Drawing.Point(951, 219)
        Me.BtnDown.Name = "BtnDown"
        Me.BtnDown.Size = New System.Drawing.Size(24, 23)
        Me.BtnDown.TabIndex = 21
        Me.BtnDown.Text = "▼"
        Me.BtnDown.UseVisualStyleBackColor = True
        '
        'BtnUp
        '
        Me.BtnUp.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnUp.Location = New System.Drawing.Point(951, 12)
        Me.BtnUp.Name = "BtnUp"
        Me.BtnUp.Size = New System.Drawing.Size(24, 23)
        Me.BtnUp.TabIndex = 20
        Me.BtnUp.Text = "▲"
        Me.BtnUp.UseVisualStyleBackColor = True
        '
        'SeparatingLine
        '
        Me.SeparatingLine.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SeparatingLine.BackColor = System.Drawing.Color.Black
        Me.SeparatingLine.Location = New System.Drawing.Point(-1, 280)
        Me.SeparatingLine.Name = "SeparatingLine"
        Me.SeparatingLine.Size = New System.Drawing.Size(1000, 1)
        Me.SeparatingLine.TabIndex = 25
        '
        'lblRegExBackReferences
        '
        Me.lblRegExBackReferences.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblRegExBackReferences.AutoSize = True
        Me.lblRegExBackReferences.Location = New System.Drawing.Point(196, 360)
        Me.lblRegExBackReferences.Name = "lblRegExBackReferences"
        Me.lblRegExBackReferences.Size = New System.Drawing.Size(595, 26)
        Me.lblRegExBackReferences.TabIndex = 37
        Me.lblRegExBackReferences.Text = resources.GetString("lblRegExBackReferences.Text")
        '
        'ChkEnabled
        '
        Me.ChkEnabled.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ChkEnabled.AutoSize = True
        Me.ChkEnabled.Checked = True
        Me.ChkEnabled.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkEnabled.Location = New System.Drawing.Point(678, 401)
        Me.ChkEnabled.Name = "ChkEnabled"
        Me.ChkEnabled.Size = New System.Drawing.Size(71, 17)
        Me.ChkEnabled.TabIndex = 36
        Me.ChkEnabled.Text = "Enabled?"
        Me.ChkEnabled.UseVisualStyleBackColor = True
        '
        'IconPictureBox
        '
        Me.IconPictureBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.IconPictureBox.Location = New System.Drawing.Point(158, 363)
        Me.IconPictureBox.Name = "IconPictureBox"
        Me.IconPictureBox.Size = New System.Drawing.Size(32, 32)
        Me.IconPictureBox.TabIndex = 35
        Me.IconPictureBox.TabStop = False
        '
        'AlertTypeComboBox
        '
        Me.AlertTypeComboBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.AlertTypeComboBox.FormattingEnabled = True
        Me.AlertTypeComboBox.Items.AddRange(New Object() {"Warning", "Error", "Information", "None"})
        Me.AlertTypeComboBox.Location = New System.Drawing.Point(73, 363)
        Me.AlertTypeComboBox.Name = "AlertTypeComboBox"
        Me.AlertTypeComboBox.Size = New System.Drawing.Size(79, 21)
        Me.AlertTypeComboBox.TabIndex = 34
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 366)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(55, 13)
        Me.Label3.TabIndex = 33
        Me.Label3.Text = "Alert Type"
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 340)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(52, 13)
        Me.Label2.TabIndex = 32
        Me.Label2.Text = "Alert Text"
        '
        'TxtAlertText
        '
        Me.TxtAlertText.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TxtAlertText.Location = New System.Drawing.Point(67, 337)
        Me.TxtAlertText.Name = "TxtAlertText"
        Me.TxtAlertText.Size = New System.Drawing.Size(908, 20)
        Me.TxtAlertText.TabIndex = 31
        '
        'ChkCaseSensitive
        '
        Me.ChkCaseSensitive.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ChkCaseSensitive.AutoSize = True
        Me.ChkCaseSensitive.Location = New System.Drawing.Point(499, 401)
        Me.ChkCaseSensitive.Name = "ChkCaseSensitive"
        Me.ChkCaseSensitive.Size = New System.Drawing.Size(102, 17)
        Me.ChkCaseSensitive.TabIndex = 30
        Me.ChkCaseSensitive.Text = "Case Sensitive?"
        Me.ChkCaseSensitive.UseVisualStyleBackColor = True
        '
        'ChkRegex
        '
        Me.ChkRegex.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ChkRegex.AutoSize = True
        Me.ChkRegex.Location = New System.Drawing.Point(15, 401)
        Me.ChkRegex.Name = "ChkRegex"
        Me.ChkRegex.Size = New System.Drawing.Size(478, 17)
        Me.ChkRegex.TabIndex = 29
        Me.ChkRegex.Text = "Regex? (Be careful with Regex, a broken regex pattern could cause the program to " &
    "malfunction)"
        Me.ChkRegex.UseVisualStyleBackColor = True
        '
        'TxtLogText
        '
        Me.TxtLogText.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TxtLogText.Location = New System.Drawing.Point(67, 311)
        Me.TxtLogText.Name = "TxtLogText"
        Me.TxtLogText.Size = New System.Drawing.Size(908, 20)
        Me.TxtLogText.TabIndex = 28
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 314)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(49, 13)
        Me.Label1.TabIndex = 27
        Me.Label1.Text = "Log Text"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label4
        '
        Me.Label4.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 291)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(435, 13)
        Me.Label4.TabIndex = 38
        Me.Label4.Text = "Add Alert        Note: All alerts are based upon the text of the log AFTER data r" &
    "eplacements."
        '
        'BtnCancel
        '
        Me.BtnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnCancel.Location = New System.Drawing.Point(83, 424)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.Size = New System.Drawing.Size(75, 23)
        Me.BtnCancel.TabIndex = 39
        Me.BtnCancel.Text = "Cancel"
        Me.BtnCancel.UseVisualStyleBackColor = True
        '
        'ChkLimited
        '
        Me.ChkLimited.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ChkLimited.AutoSize = True
        Me.ChkLimited.Location = New System.Drawing.Point(607, 401)
        Me.ChkLimited.Name = "ChkLimited"
        Me.ChkLimited.Size = New System.Drawing.Size(65, 17)
        Me.ChkLimited.TabIndex = 40
        Me.ChkLimited.Text = "Limited?"
        Me.ToolTip.SetToolTip(Me.ChkLimited, "Tells the program if this alert type should be limited by the notification limite" &
        "r.")
        Me.ChkLimited.UseVisualStyleBackColor = True
        '
        'Alerts
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(987, 458)
        Me.Controls.Add(Me.ChkLimited)
        Me.Controls.Add(Me.BtnCancel)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.lblRegExBackReferences)
        Me.Controls.Add(Me.ChkEnabled)
        Me.Controls.Add(Me.IconPictureBox)
        Me.Controls.Add(Me.AlertTypeComboBox)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.TxtAlertText)
        Me.Controls.Add(Me.ChkCaseSensitive)
        Me.Controls.Add(Me.ChkRegex)
        Me.Controls.Add(Me.TxtLogText)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.SeparatingLine)
        Me.Controls.Add(Me.BtnDown)
        Me.Controls.Add(Me.BtnUp)
        Me.Controls.Add(Me.BtnDeleteAll)
        Me.Controls.Add(Me.BtnExport)
        Me.Controls.Add(Me.BtnImport)
        Me.Controls.Add(Me.BtnEnableDisable)
        Me.Controls.Add(Me.BtnEdit)
        Me.Controls.Add(Me.AlertsListView)
        Me.Controls.Add(Me.BtnDelete)
        Me.Controls.Add(Me.BtnAdd)
        Me.KeyPreview = True
        Me.MinimumSize = New System.Drawing.Size(1003, 497)
        Me.Name = "Alerts"
        Me.Text = "Alerts"
        Me.ListViewMenu.ResumeLayout(False)
        CType(Me.IconPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents BtnEdit As Button
    Friend WithEvents AlertsListView As ListView
    Friend WithEvents AlertLogText As ColumnHeader
    Friend WithEvents Regex As ColumnHeader
    Friend WithEvents CaseSensitive As ColumnHeader
    Friend WithEvents BtnDelete As Button
    Friend WithEvents BtnAdd As Button
    Friend WithEvents AlertText As ColumnHeader
    Friend WithEvents AlertTypeColumn As ColumnHeader
    Friend WithEvents ColEnabled As ColumnHeader
    Friend WithEvents ListViewMenu As ContextMenuStrip
    Friend WithEvents EnableDisableToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents BtnEnableDisable As Button
    Friend WithEvents BtnExport As Button
    Friend WithEvents BtnImport As Button
    Friend WithEvents BtnDeleteAll As Button
    Friend WithEvents BtnDown As Button
    Friend WithEvents BtnUp As Button
    Friend WithEvents SeparatingLine As Label
    Friend WithEvents lblRegExBackReferences As Label
    Friend WithEvents ChkEnabled As CheckBox
    Friend WithEvents IconPictureBox As PictureBox
    Friend WithEvents AlertTypeComboBox As ComboBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents TxtAlertText As TextBox
    Friend WithEvents ChkCaseSensitive As CheckBox
    Friend WithEvents ChkRegex As CheckBox
    Friend WithEvents TxtLogText As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents BtnCancel As Button
    Friend WithEvents colLimit As ColumnHeader
    Friend WithEvents ChkLimited As CheckBox
    Friend WithEvents ToolTip As ToolTip
End Class
