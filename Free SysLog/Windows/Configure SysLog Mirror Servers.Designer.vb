﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ConfigureSysLogMirrorServers
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
        Me.servers = New System.Windows.Forms.ListView()
        Me.colServer = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colPort = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colEnabled = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.EnableDisableToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BtnAddServer = New System.Windows.Forms.Button()
        Me.BtnEditServer = New System.Windows.Forms.Button()
        Me.BtnDeleteServer = New System.Windows.Forms.Button()
        Me.btnEnableDisable = New System.Windows.Forms.Button()
        Me.BtnExport = New System.Windows.Forms.Button()
        Me.BtnImport = New System.Windows.Forms.Button()
        Me.colName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.btnDeleteAll = New System.Windows.Forms.Button()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'servers
        '
        Me.servers.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.servers.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colServer, Me.colPort, Me.colEnabled, Me.colName})
        Me.servers.ContextMenuStrip = Me.ContextMenuStrip1
        Me.servers.FullRowSelect = True
        Me.servers.HideSelection = False
        Me.servers.Location = New System.Drawing.Point(12, 12)
        Me.servers.Name = "servers"
        Me.servers.Size = New System.Drawing.Size(613, 97)
        Me.servers.TabIndex = 0
        Me.servers.UseCompatibleStateImageBehavior = False
        Me.servers.View = System.Windows.Forms.View.Details
        '
        'colServer
        '
        Me.colServer.Text = "Server IP"
        Me.colServer.Width = 250
        '
        'colPort
        '
        Me.colPort.Text = "Server Port"
        Me.colPort.Width = 80
        '
        'colEnabled
        '
        Me.colEnabled.Text = "Enabled"
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EnableDisableToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(153, 26)
        '
        'EnableDisableToolStripMenuItem
        '
        Me.EnableDisableToolStripMenuItem.Name = "EnableDisableToolStripMenuItem"
        Me.EnableDisableToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.EnableDisableToolStripMenuItem.Text = "Enable/Disable"
        '
        'BtnAddServer
        '
        Me.BtnAddServer.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnAddServer.Location = New System.Drawing.Point(12, 115)
        Me.BtnAddServer.Name = "BtnAddServer"
        Me.BtnAddServer.Size = New System.Drawing.Size(75, 23)
        Me.BtnAddServer.TabIndex = 1
        Me.BtnAddServer.Text = "Add Server"
        Me.BtnAddServer.UseVisualStyleBackColor = True
        '
        'BtnEditServer
        '
        Me.BtnEditServer.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnEditServer.Enabled = False
        Me.BtnEditServer.Location = New System.Drawing.Point(93, 115)
        Me.BtnEditServer.Name = "BtnEditServer"
        Me.BtnEditServer.Size = New System.Drawing.Size(75, 23)
        Me.BtnEditServer.TabIndex = 2
        Me.BtnEditServer.Text = "Edit Server"
        Me.BtnEditServer.UseVisualStyleBackColor = True
        '
        'BtnDeleteServer
        '
        Me.BtnDeleteServer.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnDeleteServer.Enabled = False
        Me.BtnDeleteServer.Location = New System.Drawing.Point(174, 115)
        Me.BtnDeleteServer.Name = "BtnDeleteServer"
        Me.BtnDeleteServer.Size = New System.Drawing.Size(82, 23)
        Me.BtnDeleteServer.TabIndex = 3
        Me.BtnDeleteServer.Text = "Delete Server"
        Me.BtnDeleteServer.UseVisualStyleBackColor = True
        '
        'btnEnableDisable
        '
        Me.btnEnableDisable.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnEnableDisable.Enabled = False
        Me.btnEnableDisable.Location = New System.Drawing.Point(262, 115)
        Me.btnEnableDisable.Name = "btnEnableDisable"
        Me.btnEnableDisable.Size = New System.Drawing.Size(75, 23)
        Me.btnEnableDisable.TabIndex = 4
        Me.btnEnableDisable.Text = "Enable"
        Me.btnEnableDisable.UseVisualStyleBackColor = True
        '
        'colName
        '
        Me.colName.Text = "Server Name"
        Me.colName.Width = 180
        '
        'BtnExport
        '
        Me.BtnExport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnExport.Location = New System.Drawing.Point(469, 115)
        Me.BtnExport.Name = "BtnExport"
        Me.BtnExport.Size = New System.Drawing.Size(75, 23)
        Me.BtnExport.TabIndex = 14
        Me.BtnExport.Text = "Export"
        Me.BtnExport.UseVisualStyleBackColor = True
        '
        'BtnImport
        '
        Me.BtnImport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnImport.Location = New System.Drawing.Point(550, 115)
        Me.BtnImport.Name = "BtnImport"
        Me.BtnImport.Size = New System.Drawing.Size(75, 23)
        Me.BtnImport.TabIndex = 13
        Me.BtnImport.Text = "Import"
        Me.BtnImport.UseVisualStyleBackColor = True
        '
        'btnDeleteAll
        '
        Me.btnDeleteAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnDeleteAll.Location = New System.Drawing.Point(343, 115)
        Me.btnDeleteAll.Name = "btnDeleteAll"
        Me.btnDeleteAll.Size = New System.Drawing.Size(75, 23)
        Me.btnDeleteAll.TabIndex = 17
        Me.btnDeleteAll.Text = "Delete All"
        Me.btnDeleteAll.UseVisualStyleBackColor = True
        '
        'ConfigureSysLogMirrorServers
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(637, 147)
        Me.Controls.Add(Me.btnDeleteAll)
        Me.Controls.Add(Me.BtnExport)
        Me.Controls.Add(Me.BtnImport)
        Me.Controls.Add(Me.btnEnableDisable)
        Me.Controls.Add(Me.BtnDeleteServer)
        Me.Controls.Add(Me.BtnEditServer)
        Me.Controls.Add(Me.BtnAddServer)
        Me.Controls.Add(Me.servers)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ConfigureSysLogMirrorServers"
        Me.Text = "Configure SysLog Mirror Servers"
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents servers As ListView
    Friend WithEvents colServer As ColumnHeader
    Friend WithEvents colPort As ColumnHeader
    Friend WithEvents BtnAddServer As Button
    Friend WithEvents BtnEditServer As Button
    Friend WithEvents BtnDeleteServer As Button
    Friend WithEvents colEnabled As ColumnHeader
    Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
    Friend WithEvents EnableDisableToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents btnEnableDisable As Button
    Friend WithEvents colName As ColumnHeader
    Friend WithEvents BtnExport As Button
    Friend WithEvents BtnImport As Button
    Friend WithEvents btnDeleteAll As Button
End Class
