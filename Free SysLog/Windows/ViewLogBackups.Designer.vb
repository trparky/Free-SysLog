﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ViewLogBackups
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
        Me.FileList = New System.Windows.Forms.ListView()
        Me.ColFileName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColFileDate = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColFileSize = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.BtnView = New System.Windows.Forms.Button()
        Me.BtnDelete = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'FileList
        '
        Me.FileList.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FileList.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColFileName, Me.ColFileDate, Me.ColFileSize})
        Me.FileList.FullRowSelect = True
        Me.FileList.HideSelection = False
        Me.FileList.Location = New System.Drawing.Point(12, 12)
        Me.FileList.Name = "FileList"
        Me.FileList.Size = New System.Drawing.Size(776, 313)
        Me.FileList.TabIndex = 1
        Me.FileList.UseCompatibleStateImageBehavior = False
        Me.FileList.View = System.Windows.Forms.View.Details
        '
        'ColFileName
        '
        Me.ColFileName.Text = "File Name"
        Me.ColFileName.Width = 240
        '
        'ColFileDate
        '
        Me.ColFileDate.Text = "Date"
        Me.ColFileDate.Width = 240
        '
        'ColFileSize
        '
        Me.ColFileSize.Text = "Size"
        Me.ColFileSize.Width = 240
        '
        'BtnView
        '
        Me.BtnView.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnView.Enabled = False
        Me.BtnView.Location = New System.Drawing.Point(12, 331)
        Me.BtnView.Name = "BtnView"
        Me.BtnView.Size = New System.Drawing.Size(75, 23)
        Me.BtnView.TabIndex = 2
        Me.BtnView.Text = "View"
        Me.BtnView.UseVisualStyleBackColor = True
        '
        'BtnDelete
        '
        Me.BtnDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnDelete.Enabled = False
        Me.BtnDelete.Location = New System.Drawing.Point(93, 331)
        Me.BtnDelete.Name = "BtnDelete"
        Me.BtnDelete.Size = New System.Drawing.Size(75, 23)
        Me.BtnDelete.TabIndex = 3
        Me.BtnDelete.Text = "Delete"
        Me.BtnDelete.UseVisualStyleBackColor = True
        '
        'ViewLogBackups
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 364)
        Me.Controls.Add(Me.BtnDelete)
        Me.Controls.Add(Me.BtnView)
        Me.Controls.Add(Me.FileList)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ViewLogBackups"
        Me.Text = "View Log Backups"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents FileList As ListView
    Friend WithEvents ColFileName As ColumnHeader
    Friend WithEvents ColFileDate As ColumnHeader
    Friend WithEvents ColFileSize As ColumnHeader
    Friend WithEvents BtnView As Button
    Friend WithEvents BtnDelete As Button
End Class
