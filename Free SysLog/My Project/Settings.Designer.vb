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
     Global.System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.9.0.0"),  _
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
         Global.System.Configuration.DefaultSettingValueAttribute("110")>  _
        Public Property columnTypeSize() As Integer
            Get
                Return CType(Me("columnTypeSize"),Integer)
            End Get
            Set
                Me("columnTypeSize") = value
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
