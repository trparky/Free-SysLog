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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.BtnDelete = New System.Windows.Forms.Button()
        Me.txtIP = New System.Windows.Forms.TextBox()
        Me.txtHostname = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.BtnAddSave = New System.Windows.Forms.Button()
        Me.BtnEdit = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'ListHostnames
        '
        Me.ListHostnames.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colIP, Me.colHostname})
        Me.ListHostnames.FullRowSelect = True
        Me.ListHostnames.HideSelection = False
        Me.ListHostnames.Location = New System.Drawing.Point(12, 12)
        Me.ListHostnames.Name = "ListHostnames"
        Me.ListHostnames.Size = New System.Drawing.Size(591, 191)
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
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 281)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(58, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "IP Address"
        '
        'BtnDelete
        '
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
        Me.txtIP.Location = New System.Drawing.Point(76, 278)
        Me.txtIP.Name = "txtIP"
        Me.txtIP.Size = New System.Drawing.Size(526, 20)
        Me.txtIP.TabIndex = 3
        '
        'txtHostname
        '
        Me.txtHostname.Location = New System.Drawing.Point(76, 304)
        Me.txtHostname.Name = "txtHostname"
        Me.txtHostname.Size = New System.Drawing.Size(526, 20)
        Me.txtHostname.TabIndex = 4
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 307)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(55, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Hostname"
        '
        'BtnAddSave
        '
        Me.BtnAddSave.Location = New System.Drawing.Point(15, 332)
        Me.BtnAddSave.Name = "BtnAddSave"
        Me.BtnAddSave.Size = New System.Drawing.Size(75, 23)
        Me.BtnAddSave.TabIndex = 6
        Me.BtnAddSave.Text = "Add"
        Me.BtnAddSave.UseVisualStyleBackColor = True
        '
        'BtnEdit
        '
        Me.BtnEdit.Enabled = False
        Me.BtnEdit.Location = New System.Drawing.Point(93, 209)
        Me.BtnEdit.Name = "BtnEdit"
        Me.BtnEdit.Size = New System.Drawing.Size(75, 23)
        Me.BtnEdit.TabIndex = 7
        Me.BtnEdit.Text = "Edit"
        Me.BtnEdit.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 257)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(49, 13)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "Add/Edit"
        '
        'Hostnames
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(614, 365)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.BtnEdit)
        Me.Controls.Add(Me.BtnAddSave)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtHostname)
        Me.Controls.Add(Me.txtIP)
        Me.Controls.Add(Me.BtnDelete)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ListHostnames)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Hostnames"
        Me.Text = "Hostnames"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ListHostnames As ListView
    Friend WithEvents colIP As ColumnHeader
    Friend WithEvents colHostname As ColumnHeader
    Friend WithEvents Label1 As Label
    Friend WithEvents BtnDelete As Button
    Friend WithEvents txtIP As TextBox
    Friend WithEvents txtHostname As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents BtnAddSave As Button
    Friend WithEvents BtnEdit As Button
    Friend WithEvents Label3 As Label
End Class
