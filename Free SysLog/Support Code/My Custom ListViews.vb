Public Enum IgnoreType
    MainLog = 0
    RemoteApp = 1
End Enum

' This class extends the ListViewItem so that I can add more properties to it for my purposes.
Public Class MyReplacementsListViewItem
    Inherits ListViewItem
    Implements ICloneable
    Public Property BoolRegex As Boolean
    Public Property BoolCaseSensitive As Boolean
    Public Property BoolEnabled As Boolean

    Public Sub New(strInput As String)
        Me.Text = strInput
    End Sub
End Class

' This class extends the ListViewItem so that I can add more properties to it for my purposes.
Public Class MyIgnoredListViewItem
    Inherits ListViewItem
    Implements ICloneable
    Public Property BoolRegex As Boolean
    Public Property BoolCaseSensitive As Boolean
    Public Property BoolEnabled As Boolean
    Public Property IgnoreType As IgnoreType
    Public Property timeSpanOfLastOccurrence As TimeSpan
    Public Property dateOfLastOccurrence As Date
    Public Property intHits As Integer
    Public dateCreated As Date
    Public strComment As String

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
    Public Property BoolEnabled As Boolean
    Public Property BoolLimited As Boolean

    Public Sub New(strInput As String)
        Me.Text = strInput
    End Sub
End Class

Public Class ServerListViewItem
    Inherits ListViewItem
    Implements ICloneable
    Public Property BoolEnabled As Boolean
    Public Property StrName As String = Nothing

    Public Sub New(strInput As String)
        Me.Text = strInput
    End Sub
End Class