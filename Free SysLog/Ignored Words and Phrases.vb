Public Class Ignored_Words_and_Phrases
    Private Sub BtnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Dim input As String = InputBox("Provide a word or phrase that Free SysLog will ignore...", "Add Ignored Word or Phrase")

        If Not String.IsNullOrWhiteSpace(input) Then
            listOfWords.Items.Add(input)
        End If
    End Sub

    Private Sub Ignored_Words_and_Phrases_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Dim strings As New Specialized.StringCollection()

        For Each item As ListViewItem In listOfWords.Items
            strings.Add(item.Text)
        Next

        My.Settings.ignored = strings
    End Sub

    Private Sub Ignored_Words_and_Phrases_Load(sender As Object, e As EventArgs) Handles Me.Load
        If My.Settings.ignored IsNot Nothing Then
            For Each word As String In My.Settings.ignored
                listOfWords.Items.Add(word)
            Next
        End If
    End Sub

    Private Sub ListOfWords_KeyUp(sender As Object, e As KeyEventArgs) Handles listOfWords.KeyUp
        If e.KeyCode = Keys.Delete Then listOfWords.Items.Remove(listOfWords.SelectedItems(0))
    End Sub
End Class