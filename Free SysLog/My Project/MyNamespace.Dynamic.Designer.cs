using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Free_SysLog.My
{
    internal static partial class MyProject
    {
        internal partial class MyForms
        {

            [EditorBrowsable(EditorBrowsableState.Never)]
            public Alerts m_Alerts;

            public Alerts Alerts
            {
                [DebuggerHidden]
                get
                {
                    m_Alerts = Create__Instance__(m_Alerts);
                    return m_Alerts;
                }
                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_Alerts))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_Alerts);
                }
            }


            [EditorBrowsable(EditorBrowsableState.Never)]
            public Alerts_History m_Alerts_History;

            public Alerts_History Alerts_History
            {
                [DebuggerHidden]
                get
                {
                    m_Alerts_History = Create__Instance__(m_Alerts_History);
                    return m_Alerts_History;
                }
                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_Alerts_History))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_Alerts_History);
                }
            }


            [EditorBrowsable(EditorBrowsableState.Never)]
            public ClearLogsOlderThan m_ClearLogsOlderThan;

            public ClearLogsOlderThan ClearLogsOlderThan
            {
                [DebuggerHidden]
                get
                {
                    m_ClearLogsOlderThan = Create__Instance__(m_ClearLogsOlderThan);
                    return m_ClearLogsOlderThan;
                }
                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_ClearLogsOlderThan))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_ClearLogsOlderThan);
                }
            }


            [EditorBrowsable(EditorBrowsableState.Never)]
            public CloseFreeSysLogDialog m_CloseFreeSysLogDialog;

            public CloseFreeSysLogDialog CloseFreeSysLogDialog
            {
                [DebuggerHidden]
                get
                {
                    m_CloseFreeSysLogDialog = Create__Instance__(m_CloseFreeSysLogDialog);
                    return m_CloseFreeSysLogDialog;
                }
                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_CloseFreeSysLogDialog))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_CloseFreeSysLogDialog);
                }
            }


            [EditorBrowsable(EditorBrowsableState.Never)]
            public ConfigureSysLogMirrorClients m_ConfigureSysLogMirrorClients;

            public ConfigureSysLogMirrorClients ConfigureSysLogMirrorClients
            {
                [DebuggerHidden]
                get
                {
                    m_ConfigureSysLogMirrorClients = Create__Instance__(m_ConfigureSysLogMirrorClients);
                    return m_ConfigureSysLogMirrorClients;
                }
                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_ConfigureSysLogMirrorClients))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_ConfigureSysLogMirrorClients);
                }
            }


            [EditorBrowsable(EditorBrowsableState.Never)]
            public DateFormatChooser m_DateFormatChooser;

            public DateFormatChooser DateFormatChooser
            {
                [DebuggerHidden]
                get
                {
                    m_DateFormatChooser = Create__Instance__(m_DateFormatChooser);
                    return m_DateFormatChooser;
                }
                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_DateFormatChooser))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_DateFormatChooser);
                }
            }


            [EditorBrowsable(EditorBrowsableState.Never)]
            public Form1 m_Form1;

            public Form1 Form1
            {
                [DebuggerHidden]
                get
                {
                    m_Form1 = Create__Instance__(m_Form1);
                    return m_Form1;
                }
                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_Form1))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_Form1);
                }
            }


            [EditorBrowsable(EditorBrowsableState.Never)]
            public Hostnames m_Hostnames;

            public Hostnames Hostnames
            {
                [DebuggerHidden]
                get
                {
                    m_Hostnames = Create__Instance__(m_Hostnames);
                    return m_Hostnames;
                }
                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_Hostnames))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_Hostnames);
                }
            }


            [EditorBrowsable(EditorBrowsableState.Never)]
            public IgnoredWordsAndPhrases m_IgnoredWordsAndPhrases;

            public IgnoredWordsAndPhrases IgnoredWordsAndPhrases
            {
                [DebuggerHidden]
                get
                {
                    m_IgnoredWordsAndPhrases = Create__Instance__(m_IgnoredWordsAndPhrases);
                    return m_IgnoredWordsAndPhrases;
                }
                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_IgnoredWordsAndPhrases))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_IgnoredWordsAndPhrases);
                }
            }


            [EditorBrowsable(EditorBrowsableState.Never)]
            public LogViewer m_LogViewer;

            public LogViewer LogViewer
            {
                [DebuggerHidden]
                get
                {
                    m_LogViewer = Create__Instance__(m_LogViewer);
                    return m_LogViewer;
                }
                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_LogViewer))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_LogViewer);
                }
            }


            [EditorBrowsable(EditorBrowsableState.Never)]
            public Replacements m_Replacements;

            public Replacements Replacements
            {
                [DebuggerHidden]
                get
                {
                    m_Replacements = Create__Instance__(m_Replacements);
                    return m_Replacements;
                }
                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_Replacements))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_Replacements);
                }
            }


            [EditorBrowsable(EditorBrowsableState.Never)]
            public ViewLogBackups m_ViewLogBackups;

            public ViewLogBackups ViewLogBackups
            {
                [DebuggerHidden]
                get
                {
                    m_ViewLogBackups = Create__Instance__(m_ViewLogBackups);
                    return m_ViewLogBackups;
                }
                [DebuggerHidden]
                set
                {
                    if (ReferenceEquals(value, m_ViewLogBackups))
                        return;
                    if (value is not null)
                        throw new ArgumentException("Property can only be set to Nothing");
                    Dispose__Instance__(ref m_ViewLogBackups);
                }
            }

        }


    }
}