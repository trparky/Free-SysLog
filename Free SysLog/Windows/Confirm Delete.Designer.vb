<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Confirm_Delete
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.lblMainLabel = New System.Windows.Forms.Label()
        Me.IconPictureBox = New System.Windows.Forms.PictureBox()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.BtnYesDeleteYesBackup = New System.Windows.Forms.Button()
        Me.BtnDontDelete = New System.Windows.Forms.Button()
        Me.BtnYesDeleteNoBackup = New System.Windows.Forms.Button()
        CType(Me.IconPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblMainLabel
        '
        Me.lblMainLabel.AutoSize = True
        Me.lblMainLabel.Location = New System.Drawing.Point(50, 12)
        Me.lblMainLabel.Name = "lblMainLabel"
        Me.lblMainLabel.Size = New System.Drawing.Size(81, 13)
        Me.lblMainLabel.TabIndex = 0
        Me.lblMainLabel.Text = "Fill In Text Here"
        '
        'IconPictureBox
        '
        Me.IconPictureBox.Location = New System.Drawing.Point(12, 12)
        Me.IconPictureBox.Name = "IconPictureBox"
        Me.IconPictureBox.Size = New System.Drawing.Size(32, 32)
        Me.IconPictureBox.TabIndex = 25
        Me.IconPictureBox.TabStop = False
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 3
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel1.Controls.Add(Me.BtnYesDeleteYesBackup, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.BtnDontDelete, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.BtnYesDeleteNoBackup, 2, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(12, 50)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(433, 29)
        Me.TableLayoutPanel1.TabIndex = 29
        '
        'BtnYesDeleteYesBackup
        '
        Me.BtnYesDeleteYesBackup.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnYesDeleteYesBackup.Location = New System.Drawing.Point(147, 3)
        Me.BtnYesDeleteYesBackup.Name = "BtnYesDeleteYesBackup"
        Me.BtnYesDeleteYesBackup.Size = New System.Drawing.Size(138, 23)
        Me.BtnYesDeleteYesBackup.TabIndex = 29
        Me.BtnYesDeleteYesBackup.Text = "&Yes, With Backup"
        Me.BtnYesDeleteYesBackup.UseVisualStyleBackColor = True
        '
        'BtnDontDelete
        '
        Me.BtnDontDelete.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnDontDelete.Location = New System.Drawing.Point(3, 3)
        Me.BtnDontDelete.Name = "BtnDontDelete"
        Me.BtnDontDelete.Size = New System.Drawing.Size(138, 23)
        Me.BtnDontDelete.TabIndex = 30
        Me.BtnDontDelete.Text = "&Don't Delete"
        Me.BtnDontDelete.UseVisualStyleBackColor = True
        '
        'BtnYesDeleteNoBackup
        '
        Me.BtnYesDeleteNoBackup.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnYesDeleteNoBackup.Location = New System.Drawing.Point(291, 3)
        Me.BtnYesDeleteNoBackup.Name = "BtnYesDeleteNoBackup"
        Me.BtnYesDeleteNoBackup.Size = New System.Drawing.Size(139, 23)
        Me.BtnYesDeleteNoBackup.TabIndex = 27
        Me.BtnYesDeleteNoBackup.Text = "Yes, With &No Backup"
        Me.BtnYesDeleteNoBackup.UseVisualStyleBackColor = True
        '
        'Confirm_Delete
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(456, 90)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Controls.Add(Me.IconPictureBox)
        Me.Controls.Add(Me.lblMainLabel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Confirm_Delete"
        Me.Text = "Confirm Deletion of Logs?"
        CType(Me.IconPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblMainLabel As Label
    Friend WithEvents IconPictureBox As PictureBox
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents BtnDontDelete As Button
    Friend WithEvents BtnYesDeleteYesBackup As Button
    Friend WithEvents BtnYesDeleteNoBackup As Button
End Class
