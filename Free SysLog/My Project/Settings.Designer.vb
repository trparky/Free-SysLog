﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.42000
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On


Namespace My
    
    <Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(),  _
     Global.System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.11.0.0"),  _
     Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
    Partial Friend NotInheritable Class MySettings
        Inherits Global.System.Configuration.ApplicationSettingsBase
        
        Private Shared defaultInstance As MySettings = CType(Global.System.Configuration.ApplicationSettingsBase.Synchronized(New MySettings()),MySettings)
        
#Region "My.Settings Auto-Save Functionality"
#If _MyType = "WindowsForms" Then
    Private Shared addedHandler As Boolean

    Private Shared addedHandlerLockObject As New Object

    <Global.System.Diagnostics.DebuggerNonUserCodeAttribute(), Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)> _
    Private Shared Sub AutoSaveSettings(sender As Global.System.Object, e As Global.System.EventArgs)
        If My.Application.SaveMySettingsOnExit Then
            My.Settings.Save()
        End If
    End Sub
#End If
#End Region
        
        Public Shared ReadOnly Property [Default]() As MySettings
            Get
                
#If _MyType = "WindowsForms" Then
               If Not addedHandler Then
                    SyncLock addedHandlerLockObject
                        If Not addedHandler Then
                            AddHandler My.Application.Shutdown, AddressOf AutoSaveSettings
                            addedHandler = True
                        End If
                    End SyncLock
                End If
#End If
                Return defaultInstance
            End Get
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property logFileLocation() As String
            Get
                Return CType(Me("logFileLocation"),String)
            End Get
            Set
                Me("logFileLocation") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("816, 173")>  _
        Public Property logViewerWindowSize() As Global.System.Drawing.Size
            Get
                Return CType(Me("logViewerWindowSize"),Global.System.Drawing.Size)
            End Get
            Set
                Me("logViewerWindowSize") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("False")>  _
        Public Property autoScroll() As Boolean
            Get
                Return CType(Me("autoScroll"),Boolean)
            End Get
            Set
                Me("autoScroll") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("1191, 485")>  _
        Public Property mainWindowSize() As Global.System.Drawing.Size
            Get
                Return CType(Me("mainWindowSize"),Global.System.Drawing.Size)
            End Get
            Set
                Me("mainWindowSize") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("196")>  _
        Public Property columnTimeSize() As Integer
            Get
                Return CType(Me("columnTimeSize"),Integer)
            End Get
            Set
                Me("columnTimeSize") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("102")>  _
        Public Property columnIPSize() As Integer
            Get
                Return CType(Me("columnIPSize"),Integer)
            End Get
            Set
                Me("columnIPSize") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("670")>  _
        Public Property columnLogSize() As Integer
            Get
                Return CType(Me("columnLogSize"),Integer)
            End Get
            Set
                Me("columnLogSize") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("True")>  _
        Public Property autoSave() As Boolean
            Get
                Return CType(Me("autoSave"),Boolean)
            End Get
            Set
                Me("autoSave") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("5")>  _
        Public Property autoSaveMinutes() As Short
            Get
                Return CType(Me("autoSaveMinutes"),Short)
            End Get
            Set
                Me("autoSaveMinutes") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("514")>  _
        Public Property sysLogPort() As Integer
            Get
                Return CType(Me("sysLogPort"),Integer)
            End Get
            Set
                Me("sysLogPort") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
        Public Property ignored() As Global.System.Collections.Specialized.StringCollection
            Get
                Return CType(Me("ignored"),Global.System.Collections.Specialized.StringCollection)
            End Get
            Set
                Me("ignored") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("LightBlue")>  _
        Public Property searchColor() As Global.System.Drawing.Color
            Get
                Return CType(Me("searchColor"),Global.System.Drawing.Color)
            End Get
            Set
                Me("searchColor") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("1168, 464")>  _
        Public Property ignoredWindowSize() As Global.System.Drawing.Size
            Get
                Return CType(Me("ignoredWindowSize"),Global.System.Drawing.Size)
            End Get
            Set
                Me("ignoredWindowSize") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("0, 0")>  _
        Public Property windowLocation() As Global.System.Drawing.Point
            Get
                Return CType(Me("windowLocation"),Global.System.Drawing.Point)
            End Get
            Set
                Me("windowLocation") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("0, 0")>  _
        Public Property ignoredWindowLocation() As Global.System.Drawing.Point
            Get
                Return CType(Me("ignoredWindowLocation"),Global.System.Drawing.Point)
            End Get
            Set
                Me("ignoredWindowLocation") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("0, 0")>  _
        Public Property ignoredWordsLocation() As Global.System.Drawing.Point
            Get
                Return CType(Me("ignoredWordsLocation"),Global.System.Drawing.Point)
            End Get
            Set
                Me("ignoredWordsLocation") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("True")>  _
        Public Property recordIgnoredLogs() As Boolean
            Get
                Return CType(Me("recordIgnoredLogs"),Boolean)
            End Get
            Set
                Me("recordIgnoredLogs") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
        Public Property replacements() As Global.System.Collections.Specialized.StringCollection
            Get
                Return CType(Me("replacements"),Global.System.Collections.Specialized.StringCollection)
            End Get
            Set
                Me("replacements") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("False")>  _
        Public Property boolMaximized() As Boolean
            Get
                Return CType(Me("boolMaximized"),Boolean)
            End Get
            Set
                Me("boolMaximized") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("True")>  _
        Public Property boolConfirmClose() As Boolean
            Get
                Return CType(Me("boolConfirmClose"),Boolean)
            End Get
            Set
                Me("boolConfirmClose") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
        Public Property ignored2() As Global.System.Collections.Specialized.StringCollection
            Get
                Return CType(Me("ignored2"),Global.System.Collections.Specialized.StringCollection)
            End Get
            Set
                Me("ignored2") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("0, 0")>  _
        Public Property replacementsLocation() As Global.System.Drawing.Point
            Get
                Return CType(Me("replacementsLocation"),Global.System.Drawing.Point)
            End Get
            Set
                Me("replacementsLocation") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("0, 0")>  _
        Public Property searchWindowLocation() As Global.System.Drawing.Point
            Get
                Return CType(Me("searchWindowLocation"),Global.System.Drawing.Point)
            End Get
            Set
                Me("searchWindowLocation") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("1168, 464")>  _
        Public Property searchWindowSize() As Global.System.Drawing.Size
            Get
                Return CType(Me("searchWindowSize"),Global.System.Drawing.Size)
            End Get
            Set
                Me("searchWindowSize") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
        Public Property alerts() As Global.System.Collections.Specialized.StringCollection
            Get
                Return CType(Me("alerts"),Global.System.Collections.Specialized.StringCollection)
            End Get
            Set
                Me("alerts") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("0, 0")>  _
        Public Property alertsLocation() As Global.System.Drawing.Point
            Get
                Return CType(Me("alertsLocation"),Global.System.Drawing.Point)
            End Get
            Set
                Me("alertsLocation") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("True")>  _
        Public Property boolDeselectItemsWhenMinimizing() As Boolean
            Get
                Return CType(Me("boolDeselectItemsWhenMinimizing"),Boolean)
            End Get
            Set
                Me("boolDeselectItemsWhenMinimizing") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("True")>  _
        Public Property boolShowAlertedColumn() As Boolean
            Get
                Return CType(Me("boolShowAlertedColumn"),Boolean)
            End Get
            Set
                Me("boolShowAlertedColumn") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("345")>  _
        Public Property colIgnoredReplace() As Integer
            Get
                Return CType(Me("colIgnoredReplace"),Integer)
            End Get
            Set
                Me("colIgnoredReplace") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("60")>  _
        Public Property colIgnoredRegex() As Integer
            Get
                Return CType(Me("colIgnoredRegex"),Integer)
            End Get
            Set
                Me("colIgnoredRegex") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("91")>  _
        Public Property colIgnoredCaseSensitive() As Integer
            Get
                Return CType(Me("colIgnoredCaseSensitive"),Integer)
            End Get
            Set
                Me("colIgnoredCaseSensitive") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("60")>  _
        Public Property colIgnoredEnabled() As Integer
            Get
                Return CType(Me("colIgnoredEnabled"),Integer)
            End Get
            Set
                Me("colIgnoredEnabled") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("345")>  _
        Public Property colReplacementsReplace() As Integer
            Get
                Return CType(Me("colReplacementsReplace"),Integer)
            End Get
            Set
                Me("colReplacementsReplace") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("345")>  _
        Public Property colReplacementsReplaceWith() As Integer
            Get
                Return CType(Me("colReplacementsReplaceWith"),Integer)
            End Get
            Set
                Me("colReplacementsReplaceWith") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("60")>  _
        Public Property colReplacementsRegex() As Integer
            Get
                Return CType(Me("colReplacementsRegex"),Integer)
            End Get
            Set
                Me("colReplacementsRegex") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("91")>  _
        Public Property colReplacementsCaseSensitive() As Integer
            Get
                Return CType(Me("colReplacementsCaseSensitive"),Integer)
            End Get
            Set
                Me("colReplacementsCaseSensitive") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("60")>  _
        Public Property colReplacementsEnabled() As Integer
            Get
                Return CType(Me("colReplacementsEnabled"),Integer)
            End Get
            Set
                Me("colReplacementsEnabled") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("308")>  _
        Public Property colAlertsAlertLogText() As Integer
            Get
                Return CType(Me("colAlertsAlertLogText"),Integer)
            End Get
            Set
                Me("colAlertsAlertLogText") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("257")>  _
        Public Property colAlertsAlertText() As Integer
            Get
                Return CType(Me("colAlertsAlertText"),Integer)
            End Get
            Set
                Me("colAlertsAlertText") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("60")>  _
        Public Property colAlertsRegex() As Integer
            Get
                Return CType(Me("colAlertsRegex"),Integer)
            End Get
            Set
                Me("colAlertsRegex") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("91")>  _
        Public Property colAlertsCaseSensitive() As Integer
            Get
                Return CType(Me("colAlertsCaseSensitive"),Integer)
            End Get
            Set
                Me("colAlertsCaseSensitive") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("90")>  _
        Public Property colAlertsType() As Integer
            Get
                Return CType(Me("colAlertsType"),Integer)
            End Get
            Set
                Me("colAlertsType") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("60")>  _
        Public Property colAlertsEnabled() As Integer
            Get
                Return CType(Me("colAlertsEnabled"),Integer)
            End Get
            Set
                Me("colAlertsEnabled") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("670, 304")>  _
        Public Property ConfigureIgnoredSize() As Global.System.Drawing.Size
            Get
                Return CType(Me("ConfigureIgnoredSize"),Global.System.Drawing.Size)
            End Get
            Set
                Me("ConfigureIgnoredSize") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("1051, 489")>  _
        Public Property ConfigureReplacementsSize() As Global.System.Drawing.Size
            Get
                Return CType(Me("ConfigureReplacementsSize"),Global.System.Drawing.Size)
            End Get
            Set
                Me("ConfigureReplacementsSize") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("1003, 322")>  _
        Public Property ConfigureAlertsSize() As Global.System.Drawing.Size
            Get
                Return CType(Me("ConfigureAlertsSize"),Global.System.Drawing.Size)
            End Get
            Set
                Me("ConfigureAlertsSize") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
        Public Property ServersToSendTo() As Global.System.Collections.Specialized.StringCollection
            Get
                Return CType(Me("ServersToSendTo"),Global.System.Collections.Specialized.StringCollection)
            End Get
            Set
                Me("ServersToSendTo") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("True")>  _
        Public Property MinimizeToClockTray() As Boolean
            Get
                Return CType(Me("MinimizeToClockTray"),Boolean)
            End Get
            Set
                Me("MinimizeToClockTray") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("False")>  _
        Public Property DeleteOldLogsAtMidnight() As Boolean
            Get
                Return CType(Me("DeleteOldLogsAtMidnight"),Boolean)
            End Get
            Set
                Me("DeleteOldLogsAtMidnight") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("1168, 464")>  _
        Public Property logFileViewerSize() As Global.System.Drawing.Size
            Get
                Return CType(Me("logFileViewerSize"),Global.System.Drawing.Size)
            End Get
            Set
                Me("logFileViewerSize") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("0, 0")>  _
        Public Property logFileViewerLocation() As Global.System.Drawing.Point
            Get
                Return CType(Me("logFileViewerLocation"),Global.System.Drawing.Point)
            End Get
            Set
                Me("logFileViewerLocation") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("True")>  _
        Public Property BackupOldLogsAfterClearingAtMidnight() As Boolean
            Get
                Return CType(Me("BackupOldLogsAfterClearingAtMidnight"),Boolean)
            End Get
            Set
                Me("BackupOldLogsAfterClearingAtMidnight") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("True")>  _
        Public Property boolCheckForUpdates() As Boolean
            Get
                Return CType(Me("boolCheckForUpdates"),Boolean)
            End Get
            Set
                Me("boolCheckForUpdates") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("0, 0")>  _
        Public Property DateChooserWindowLocation() As Global.System.Drawing.Point
            Get
                Return CType(Me("DateChooserWindowLocation"),Global.System.Drawing.Point)
            End Get
            Set
                Me("DateChooserWindowLocation") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
        Public Property DateFormat() As Byte
            Get
                Return CType(Me("DateFormat"),Byte)
            End Get
            Set
                Me("DateFormat") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property CustomDateFormat() As String
            Get
                Return CType(Me("CustomDateFormat"),String)
            End Get
            Set
                Me("CustomDateFormat") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("816, 403")>  _
        Public Property ViewLogBackupsSize() As Global.System.Drawing.Size
            Get
                Return CType(Me("ViewLogBackupsSize"),Global.System.Drawing.Size)
            End Get
            Set
                Me("ViewLogBackupsSize") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("False")>  _
        Public Property EnableTCPServer() As Boolean
            Get
                Return CType(Me("EnableTCPServer"),Boolean)
            End Get
            Set
                Me("EnableTCPServer") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("150")>  _
        Public Property RFC5424HeaderSize() As Integer
            Get
                Return CType(Me("RFC5424HeaderSize"),Integer)
            End Get
            Set
                Me("RFC5424HeaderSize") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("200")>  _
        Public Property LogTypeWidth() As Integer
            Get
                Return CType(Me("LogTypeWidth"),Integer)
            End Get
            Set
                Me("LogTypeWidth") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("True")>  _
        Public Property boolProcessReplacementsOnHeaderData() As Boolean
            Get
                Return CType(Me("boolProcessReplacementsOnHeaderData"),Boolean)
            End Get
            Set
                Me("boolProcessReplacementsOnHeaderData") = value
            End Set
        End Property
    End Class
End Namespace

Namespace My
    
    <Global.Microsoft.VisualBasic.HideModuleNameAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute()>  _
    Friend Module MySettingsProperty
        
        <Global.System.ComponentModel.Design.HelpKeywordAttribute("My.Settings")>  _
        Friend ReadOnly Property Settings() As Global.Free_SysLog.My.MySettings
            Get
                Return Global.Free_SysLog.My.MySettings.Default
            End Get
        End Property
    End Module
End Namespace
