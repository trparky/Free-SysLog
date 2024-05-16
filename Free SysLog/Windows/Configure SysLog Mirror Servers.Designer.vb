<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
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
        Me.servers = New System.Windows.Forms.ListView()
        Me.colServer = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colPort = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.BtnAddServer = New System.Windows.Forms.Button()
        Me.BtnEditServer = New System.Windows.Forms.Button()
        Me.BtnDeleteServer = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'servers
        '
        Me.servers.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colServer, Me.colPort})
        Me.servers.HideSelection = False
        Me.servers.Location = New System.Drawing.Point(12, 12)
        Me.servers.Name = "servers"
        Me.servers.Size = New System.Drawing.Size(415, 97)
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
        'BtnAddServer
        '
        Me.BtnAddServer.Location = New System.Drawing.Point(12, 115)
        Me.BtnAddServer.Name = "BtnAddServer"
        Me.BtnAddServer.Size = New System.Drawing.Size(75, 23)
        Me.BtnAddServer.TabIndex = 1
        Me.BtnAddServer.Text = "Add Server"
        Me.BtnAddServer.UseVisualStyleBackColor = True
        '
        'BtnEditServer
        '
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
        Me.BtnDeleteServer.Enabled = False
        Me.BtnDeleteServer.Location = New System.Drawing.Point(174, 115)
        Me.BtnDeleteServer.Name = "BtnDeleteServer"
        Me.BtnDeleteServer.Size = New System.Drawing.Size(82, 23)
        Me.BtnDeleteServer.TabIndex = 3
        Me.BtnDeleteServer.Text = "Delete Server"
        Me.BtnDeleteServer.UseVisualStyleBackColor = True
        '
        'ConfigureSysLogMirrorServers
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(439, 147)
        Me.Controls.Add(Me.BtnDeleteServer)
        Me.Controls.Add(Me.BtnEditServer)
        Me.Controls.Add(Me.BtnAddServer)
        Me.Controls.Add(Me.servers)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "ConfigureSysLogMirrorServers"
        Me.Text = "Configure SysLog Mirror Servers"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents servers As ListView
    Friend WithEvents colServer As ColumnHeader
    Friend WithEvents colPort As ColumnHeader
    Friend WithEvents BtnAddServer As Button
    Friend WithEvents BtnEditServer As Button
    Friend WithEvents BtnDeleteServer As Button
End Class
