<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Hostnames
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
        Me.ListHostnames = New System.Windows.Forms.ListView()
        Me.colIP = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colHostname = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ipAddressLabel = New System.Windows.Forms.Label()
        Me.BtnDelete = New System.Windows.Forms.Button()
        Me.txtIP = New System.Windows.Forms.TextBox()
        Me.txtHostname = New System.Windows.Forms.TextBox()
        Me.hostnameLabel = New System.Windows.Forms.Label()
        Me.BtnAddSave = New System.Windows.Forms.Button()
        Me.BtnEdit = New System.Windows.Forms.Button()
        Me.lblAddEditHostNameLabel = New System.Windows.Forms.Label()
        Me.BtnDown = New System.Windows.Forms.Button()
        Me.BtnUp = New System.Windows.Forms.Button()
        Me.BtnImport = New System.Windows.Forms.Button()
        Me.BtnExport = New System.Windows.Forms.Button()
        Me.SeparatingLine = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'ListHostnames
        '
        Me.ListHostnames.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListHostnames.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colIP, Me.colHostname})
        Me.ListHostnames.FullRowSelect = True
        Me.ListHostnames.HideSelection = False
        Me.ListHostnames.Location = New System.Drawing.Point(12, 12)
        Me.ListHostnames.Name = "ListHostnames"
        Me.ListHostnames.Size = New System.Drawing.Size(566, 191)
        Me.ListHostnames.TabIndex = 0
        Me.ListHostnames.UseCompatibleStateImageBehavior = False
        Me.ListHostnames.View = System.Windows.Forms.View.Details
        '
        'colIP
        '
        Me.colIP.Text = "IP Address"
        Me.colIP.Width = 180
        '
        'colHostname
        '
        Me.colHostname.Text = "Hostname"
        Me.colHostname.Width = 360
        '
        'ipAddressLabel
        '
        Me.ipAddressLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ipAddressLabel.AutoSize = True
        Me.ipAddressLabel.Location = New System.Drawing.Point(12, 275)
        Me.ipAddressLabel.Name = "ipAddressLabel"
        Me.ipAddressLabel.Size = New System.Drawing.Size(58, 13)
        Me.ipAddressLabel.TabIndex = 1
        Me.ipAddressLabel.Text = "IP Address"
        '
        'BtnDelete
        '
        Me.BtnDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnDelete.Enabled = False
        Me.BtnDelete.Location = New System.Drawing.Point(12, 209)
        Me.BtnDelete.Name = "BtnDelete"
        Me.BtnDelete.Size = New System.Drawing.Size(75, 23)
        Me.BtnDelete.TabIndex = 2
        Me.BtnDelete.Text = "Delete"
        Me.BtnDelete.UseVisualStyleBackColor = True
        '
        'txtIP
        '
        Me.txtIP.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtIP.Location = New System.Drawing.Point(76, 272)
        Me.txtIP.Name = "txtIP"
        Me.txtIP.Size = New System.Drawing.Size(526, 20)
        Me.txtIP.TabIndex = 3
        '
        'txtHostname
        '
        Me.txtHostname.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtHostname.Location = New System.Drawing.Point(76, 298)
        Me.txtHostname.Name = "txtHostname"
        Me.txtHostname.Size = New System.Drawing.Size(526, 20)
        Me.txtHostname.TabIndex = 4
        '
        'hostnameLabel
        '
        Me.hostnameLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.hostnameLabel.AutoSize = True
        Me.hostnameLabel.Location = New System.Drawing.Point(12, 301)
        Me.hostnameLabel.Name = "hostnameLabel"
        Me.hostnameLabel.Size = New System.Drawing.Size(55, 13)
        Me.hostnameLabel.TabIndex = 5
        Me.hostnameLabel.Text = "Hostname"
        '
        'BtnAddSave
        '
        Me.BtnAddSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnAddSave.Location = New System.Drawing.Point(15, 326)
        Me.BtnAddSave.Name = "BtnAddSave"
        Me.BtnAddSave.Size = New System.Drawing.Size(75, 23)
        Me.BtnAddSave.TabIndex = 6
        Me.BtnAddSave.Text = "Add"
        Me.BtnAddSave.UseVisualStyleBackColor = True
        '
        'BtnEdit
        '
        Me.BtnEdit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnEdit.Enabled = False
        Me.BtnEdit.Location = New System.Drawing.Point(93, 209)
        Me.BtnEdit.Name = "BtnEdit"
        Me.BtnEdit.Size = New System.Drawing.Size(75, 23)
        Me.BtnEdit.TabIndex = 7
        Me.BtnEdit.Text = "Edit"
        Me.BtnEdit.UseVisualStyleBackColor = True
        '
        'lblAddEditHostNameLabel
        '
        Me.lblAddEditHostNameLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblAddEditHostNameLabel.AutoSize = True
        Me.lblAddEditHostNameLabel.Location = New System.Drawing.Point(12, 251)
        Me.lblAddEditHostNameLabel.Name = "lblAddEditHostNameLabel"
        Me.lblAddEditHostNameLabel.Size = New System.Drawing.Size(140, 13)
        Me.lblAddEditHostNameLabel.TabIndex = 8
        Me.lblAddEditHostNameLabel.Text = "Add New Custom Hostname"
        '
        'BtnDown
        '
        Me.BtnDown.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnDown.Location = New System.Drawing.Point(584, 171)
        Me.BtnDown.Name = "BtnDown"
        Me.BtnDown.Size = New System.Drawing.Size(24, 23)
        Me.BtnDown.TabIndex = 21
        Me.BtnDown.Text = "▼"
        Me.BtnDown.UseVisualStyleBackColor = True
        '
        'BtnUp
        '
        Me.BtnUp.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnUp.Location = New System.Drawing.Point(584, 12)
        Me.BtnUp.Name = "BtnUp"
        Me.BtnUp.Size = New System.Drawing.Size(24, 23)
        Me.BtnUp.TabIndex = 20
        Me.BtnUp.Text = "▲"
        Me.BtnUp.UseVisualStyleBackColor = True
        '
        'BtnImport
        '
        Me.BtnImport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnImport.Location = New System.Drawing.Point(533, 209)
        Me.BtnImport.Name = "BtnImport"
        Me.BtnImport.Size = New System.Drawing.Size(75, 23)
        Me.BtnImport.TabIndex = 22
        Me.BtnImport.Text = "Import"
        Me.BtnImport.UseVisualStyleBackColor = True
        '
        'BtnExport
        '
        Me.BtnExport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnExport.Location = New System.Drawing.Point(452, 209)
        Me.BtnExport.Name = "BtnExport"
        Me.BtnExport.Size = New System.Drawing.Size(75, 23)
        Me.BtnExport.TabIndex = 23
        Me.BtnExport.Text = "Export"
        Me.BtnExport.UseVisualStyleBackColor = True
        '
        'SeparatingLine
        '
        Me.SeparatingLine.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SeparatingLine.BackColor = System.Drawing.Color.Black
        Me.SeparatingLine.Location = New System.Drawing.Point(-1, 240)
        Me.SeparatingLine.Name = "SeparatingLine"
        Me.SeparatingLine.Size = New System.Drawing.Size(618, 1)
        Me.SeparatingLine.TabIndex = 24
        '
        'Hostnames
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(614, 356)
        Me.Controls.Add(Me.SeparatingLine)
        Me.Controls.Add(Me.BtnExport)
        Me.Controls.Add(Me.BtnImport)
        Me.Controls.Add(Me.BtnDown)
        Me.Controls.Add(Me.BtnUp)
        Me.Controls.Add(Me.lblAddEditHostNameLabel)
        Me.Controls.Add(Me.BtnEdit)
        Me.Controls.Add(Me.BtnAddSave)
        Me.Controls.Add(Me.hostnameLabel)
        Me.Controls.Add(Me.txtHostname)
        Me.Controls.Add(Me.txtIP)
        Me.Controls.Add(Me.BtnDelete)
        Me.Controls.Add(Me.ipAddressLabel)
        Me.Controls.Add(Me.ListHostnames)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(630, 395)
        Me.Name = "Hostnames"
        Me.Text = "Configure Custom Hostnames"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ListHostnames As ListView
    Friend WithEvents colIP As ColumnHeader
    Friend WithEvents colHostname As ColumnHeader
    Friend WithEvents ipAddressLabel As Label
    Friend WithEvents BtnDelete As Button
    Friend WithEvents txtIP As TextBox
    Friend WithEvents txtHostname As TextBox
    Friend WithEvents hostnameLabel As Label
    Friend WithEvents BtnAddSave As Button
    Friend WithEvents BtnEdit As Button
    Friend WithEvents lblAddEditHostNameLabel As Label
    Friend WithEvents BtnDown As Button
    Friend WithEvents BtnUp As Button
    Friend WithEvents BtnImport As Button
    Friend WithEvents BtnExport As Button
    Friend WithEvents SeparatingLine As Label
End Class
