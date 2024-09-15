Public Class Confirm_Delete
    Private intNumberOfLogsToBeDeleted As Integer
    Public choice As UserChoice

    Public Enum UserChoice As Integer
        YesDeleteNoBackup
        YesDeleteYesBackup
        NoDelete
    End Enum

    Public Sub New(_intNumberOfLogsToBeDeleted As Integer)
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        intNumberOfLogsToBeDeleted = _intNumberOfLogsToBeDeleted
    End Sub

    Private Sub Delete_Confirm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Media.SystemSounds.Asterisk.Play()
        IconPictureBox.Image = SystemIcons.Question.ToBitmap()
        BtnDontDelete.Select()
    End Sub

    Private Sub BtnDontDelete_Click(sender As Object, e As EventArgs) Handles BtnDontDelete.Click
        choice = UserChoice.NoDelete
        Close()
    End Sub

    Private Sub BtnYesDeleteYesBackup_Click(sender As Object, e As EventArgs) Handles BtnYesDeleteYesBackup.Click
        choice = UserChoice.YesDeleteYesBackup
        Close()
    End Sub

    Private Sub BtnYesDeleteNoBackup_Click(sender As Object, e As EventArgs) Handles BtnYesDeleteNoBackup.Click
        choice = UserChoice.YesDeleteNoBackup
        Close()
    End Sub
End Class