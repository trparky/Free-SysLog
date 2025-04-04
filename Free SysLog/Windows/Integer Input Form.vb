Public Class IntegerInputForm
    Public intResult As Integer
    Private intMin, intMax As Integer

    Public Sub New(intInputMin As Integer, intInputMax As Integer)
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        intMin = intInputMin
        intMax = intInputMax
    End Sub

    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        If Integer.TryParse(TxtSetting.Text, intResult) Then
            DialogResult = DialogResult.OK
            Close()
        Else
            MsgBox("Invalid user input!", MsgBoxStyle.Critical, Text)
        End If
    End Sub

    Private Sub BtnUp_Click(sender As Object, e As EventArgs) Handles BtnUp.Click
        Dim myInteger As Integer

        If Integer.TryParse(TxtSetting.Text, myInteger) Then
            If myInteger = intMax Then Exit Sub
            myInteger += 1
            TxtSetting.Text = myInteger.ToString
        End If
    End Sub

    Private Sub BtnDown_Click(sender As Object, e As EventArgs) Handles BtnDown.Click
        Dim myInteger As Integer

        If Integer.TryParse(TxtSetting.Text, myInteger) Then
            If myInteger = intMin Then Exit Sub
            myInteger -= 1
            TxtSetting.Text = myInteger.ToString
        End If
    End Sub

    Private Sub BtnCancel_Click(sender As Object, e As EventArgs) Handles BtnCancel.Click
        DialogResult = DialogResult.Cancel
        Close()
    End Sub

    Private Sub IntegerInputForm_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyData = Keys.Escape Then
            DialogResult = DialogResult.Cancel
            Close()
        ElseIf e.KeyData = Keys.Enter Then
            DialogResult = DialogResult.OK
            Close()
        End If
    End Sub
End Class