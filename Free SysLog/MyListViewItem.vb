﻿' This class extends the ListViewItem so that I can add more properties to it for my purposes.
Public Class MyReplacementsListViewItem
    Inherits ListViewItem
    Implements ICloneable
    Public Property BoolRegex As Boolean
    Public Property BoolCaseSensitive As Boolean

    Public Sub New(strInput As String)
        Me.Text = strInput
    End Sub
End Class

Public Class MyDataGridViewRow
    Inherits DataGridViewRow
    Implements ICloneable
    Public Property DateObject As Date
End Class