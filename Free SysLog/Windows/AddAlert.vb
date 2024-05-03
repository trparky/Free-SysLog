Public Class AddAlert
    Public boolRegex, boolCaseSensitive As Boolean
    Public strLogText, strAlertText As String
    Public boolSuccess As Boolean = False
    Public boolEditMode As Boolean = False
    Public AlertType As AlertType
    Public boolEnabled As Boolean = True
    Private boolChanged As Boolean = False
    Private boolDoneLoading As Boolean = False

    Private Function GetToolTipIconImage(icon As ToolTipIcon) As Image
        Select Case icon
            Case ToolTipIcon.None
                Return Nothing
            Case ToolTipIcon.Info
                Return SystemIcons.Information.ToBitmap()
            Case ToolTipIcon.Warning
                Return SystemIcons.Warning.ToBitmap()
            Case ToolTipIcon.Error
                Return SystemIcons.Error.ToBitmap()
            Case Else
                Return Nothing
        End Select
    End Function

    Private Sub BtnAdd_Click(sender As Object, e As EventArgs) Handles BtnAdd.Click
        TxtAlertText.Text = TxtAlertText.Text.Trim

        If AlertTypeComboBox.SelectedIndex = 0 Then
            AlertType = AlertType.Warning
        ElseIf AlertTypeComboBox.SelectedIndex = 1 Then
            AlertType = AlertType.ErrorMsg
        ElseIf AlertTypeComboBox.SelectedIndex = 2 Then
            AlertType = AlertType.Info
        ElseIf AlertTypeComboBox.SelectedIndex = 3 Then
            AlertType = AlertType.None
        End If

        If Not ChkRegex.Checked Then
            boolRegex = ChkRegex.Checked
            boolCaseSensitive = ChkCaseSensitive.Checked
            strLogText = TxtLogText.Text
            strAlertText = TxtAlertText.Text
            boolEnabled = ChkEnabled.Checked
            boolSuccess = True
            boolChanged = False
            Close()
        Else
            If IsRegexPatternValid(TxtLogText.Text) Then
                boolRegex = ChkRegex.Checked
                boolCaseSensitive = ChkCaseSensitive.Checked
                strLogText = TxtLogText.Text
                strAlertText = TxtAlertText.Text
                boolEnabled = ChkEnabled.Checked
                boolSuccess = True
                boolChanged = False
                Close()
            Else
                MsgBox("Invalid regex pattern detected.", MsgBoxStyle.Critical, Text)
            End If
        End If
    End Sub

    Private Sub Add_Alert_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If boolEditMode Then
            If AlertType = AlertType.Warning Then
                AlertTypeComboBox.SelectedIndex = 0
            ElseIf AlertType = AlertType.ErrorMsg Then
                AlertTypeComboBox.SelectedIndex = 1
            ElseIf AlertType = AlertType.Info Then
                AlertTypeComboBox.SelectedIndex = 2
            ElseIf AlertType = AlertType.None Then
                AlertTypeComboBox.SelectedIndex = 3
            End If

            TxtLogText.Text = strLogText
            TxtAlertText.Text = strAlertText
            ChkRegex.Checked = boolRegex
            ChkCaseSensitive.Checked = boolCaseSensitive
            ChkEnabled.Checked = boolEnabled
            BtnAdd.Text = "Save"
        End If

        boolDoneLoading = True
    End Sub

    Private Sub Add_Alert_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.Enter AndAlso Not String.IsNullOrWhiteSpace(TxtLogText.Text) Then
            BtnAdd.PerformClick()
        ElseIf e.KeyCode = Keys.Escape Then
            Close()
        End If
    End Sub

    Private Sub BtnAdd_KeyUp(sender As Object, e As KeyEventArgs) Handles BtnAdd.KeyUp
        BtnAdd.PerformClick()
    End Sub

    Private Sub TxtAlertText_KeyUp(sender As Object, e As KeyEventArgs) Handles TxtAlertText.KeyUp
        If e.KeyCode <> Keys.Escape Then boolChanged = True
    End Sub

    Private Sub TxtLogText_KeyUp(sender As Object, e As KeyEventArgs) Handles TxtLogText.KeyUp
        If e.KeyCode <> Keys.Escape Then boolChanged = True
    End Sub

    Private Sub AlertTypeComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles AlertTypeComboBox.SelectedIndexChanged
        If AlertTypeComboBox.SelectedIndex = 0 Then
            IconPictureBox.Image = GetToolTipIconImage(ToolTipIcon.Warning)
        ElseIf AlertTypeComboBox.SelectedIndex = 1 Then
            IconPictureBox.Image = GetToolTipIconImage(ToolTipIcon.Error)
        ElseIf AlertTypeComboBox.SelectedIndex = 2 Then
            IconPictureBox.Image = GetToolTipIconImage(ToolTipIcon.Info)
        ElseIf AlertTypeComboBox.SelectedIndex = 3 Then
            IconPictureBox.Image = Nothing
        End If

        If boolDoneLoading Then boolChanged = True
    End Sub

    Private Sub AddAlert_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If boolChanged Then
            If MsgBox("You have changed some data on this window, do you want to save it before closing?", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton1 + MsgBoxStyle.Question, Text) = MsgBoxResult.Yes Then
                BtnAdd.PerformClick()
            Else
                boolChanged = False
                Close()
            End If
        End If
    End Sub

    Private Sub ChkCaseSensitive_Click(sender As Object, e As EventArgs) Handles ChkCaseSensitive.Click
        boolChanged = True
    End Sub

    Private Sub ChkRegex_Click(sender As Object, e As EventArgs) Handles ChkRegex.Click
        boolChanged = True
    End Sub

    Private Sub ChkEnabled_Click(sender As Object, e As EventArgs) Handles ChkEnabled.Click
        boolChanged = True
    End Sub
End Class