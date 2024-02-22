' This class extends the ListViewItem so that I can add more properties to it for my purposes.
Public Class MyListViewItem
    Inherits ListViewItem
    Implements ICloneable
    Public Property DateObject As Date

    Public Sub New(strInput As String)
        Me.Text = strInput
    End Sub

    Public Overrides Function Clone() As Object Implements ICloneable.Clone
        Dim newListViewItem As New MyListViewItem(Me.Text)

        For index As Short = 1 To Me.SubItems.Count - 1
            newListViewItem.SubItems.Add(Me.SubItems(index))
        Next

        With newListViewItem
            .DateObject = Me.DateObject
        End With

        Return newListViewItem
    End Function
End Class

' This class extends the ListViewItem so that I can add more properties to it for my purposes.
Public Class MyReplacementsListViewItem
    Inherits ListViewItem
    Implements ICloneable
    Public Property BoolRegex As Boolean
    Public Property BoolCaseSensitive As Boolean

    Public Sub New(strInput As String)
        Me.Text = strInput
    End Sub
End Class