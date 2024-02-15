' This class extends the ListViewItem so that I can add more properties to it for my purposes.
Public Class MyListViewItem
    Inherits ListViewItem
    Implements ICloneable
    Public Property DateObject As Date

    Public Sub New(strInput As String)
        Me.Text = strInput
    End Sub
End Class