Imports Free_SysLog.ThreadSafetyLists

Partial Class Form1
    Private boolMaximizedBeforeMinimize As Boolean
    Private boolDoneLoading As Boolean = False
    Public IgnoredLogs As New ThreadSafeList(Of MyDataGridViewRow)
    Public intSortColumnIndex As Integer = 0 ' Define intColumnNumber at class level
    Public sortOrder As SortOrder = SortOrder.Ascending ' Define soSortOrder at class level
    Public ReadOnly dataGridLockObject As New Object
    Public ReadOnly IgnoredLogsLockObject As New Object
    Private Const strBuyMeACoffee As String = "https://buymeacoffee.com/trparky"
    Private serverThread As Threading.Thread
    Private SyslogTcpServer As SyslogTcpServer.SyslogTcpServer
    Private boolServerRunning As Boolean = False
    Private boolTCPServerRunning As Boolean = False
    Private lastFirstDisplayedRowIndex As Integer = -1
    Private processUptimeTimer As Timer
    Private dateProcessStarted As Date = Process.GetCurrentProcess.StartTime

    Private HostNamesInstance As Hostnames
    Private IgnoredWordsAndPhrasesOrAlertsInstance As IgnoredWordsAndPhrases
    Private ReplacementsInstance As Replacements
    Private AlertsInstance As Alerts
    Private ConfigureSysLogMirrorClientsInstance As ConfigureSysLogMirrorClients
End Class