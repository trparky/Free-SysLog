﻿' This class extends the ListViewItem so that I can add more properties to it for my purposes.
Public Class MyIgnoredListViewItem
    Inherits ListViewItem
    Implements ICloneable
    Public Property BoolRegex As Boolean
    Public Property BoolCaseSensitive As Boolean

    Public Sub New(strInput As String)
        Me.Text = strInput
    End Sub
End Class

Public Class AlertsListViewItem
    Inherits ListViewItem
    Implements ICloneable
    Public Property BoolRegex As Boolean
    Public Property BoolCaseSensitive As Boolean
    Public Property StrLogText As String
    Public Property StrAlertText As String
    Public Property AlertType As AlertType

    Public Sub New(strInput As String)
        Me.Text = strInput
    End Sub
End Class