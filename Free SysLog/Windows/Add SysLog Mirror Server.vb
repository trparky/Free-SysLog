﻿Imports System.Net

Public Class AddSysLogMirrorServer
    Public strIP As String, intPort As Integer
    Public boolEditMode As Boolean = False
    Public boolSuccess As Boolean = False

    Private Sub BtnAddServer_Click(sender As Object, e As EventArgs) Handles BtnAddServer.Click
        If Integer.TryParse(txtPort.Text, intPort) Then
            Dim tempIP As IPAddress = Nothing

            If Not IPAddress.TryParse(txtIP.Text, tempIP) Then
                MsgBox("You must input a valid IP address.", MsgBoxStyle.Critical, Text)
                Exit Sub
            End If

            strIP = txtIP.Text
            boolSuccess = True
            Close()
        Else
            MsgBox("You must input a numerical value.", MsgBoxStyle.Critical, Text)
        End If
    End Sub

    Private Sub AddSysLogMirrorServer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If boolEditMode Then
            BtnAddServer.Text = "Edit Server"
            txtIP.Text = strIP
            txtPort.Text = intPort.ToString
        Else
            txtPort.Text = My.Settings.sysLogPort.ToString
        End If
    End Sub
End Class