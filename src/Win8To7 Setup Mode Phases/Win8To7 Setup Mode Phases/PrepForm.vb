Public Class PrepForm

    Private Sub PrepForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'First, error out on incompatible versions
        If Not System.Environment.OSVersion.Version.Major = 6 Or (System.Environment.OSVersion.Version.Major = 6 And (System.Environment.OSVersion.Version.Minor > 3 Or System.Environment.OSVersion.Version.Minor < 2)) Then
            ' Quit silently if incompatible
            Application.Exit()
            End
        End If
        If HKLMKey32.OpenSubKey("SOFTWARE\Win8To7") Is Nothing Then
            ' Key doesn't exist, so we just quit
            Application.Exit()
            End
        End If

        If HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("TransformationFailed") = 1 Then 'If the transformation failed, don't continue running this
            MsgBox("Win8To7 Transformation Pack Backend experienced an unexpected error. To install this transformation pack, restart the installation.")

            'Go back into Windows as an emergency precaution as well
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v CmdLine /t REG_SZ /f", AppWinStyle.Hide, True)
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v OOBEInProgress /t REG_DWORD /d 0 /f", AppWinStyle.Hide, True)
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SetupPhase /t REG_DWORD /d 0 /f", AppWinStyle.Hide, True)
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SetupType /t REG_DWORD /d 0 /f", AppWinStyle.Hide, True)
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SystemSetupInProgress /t REG_DWORD /d 0 /f", AppWinStyle.Hide, True)
            Thread.Sleep(8000) 'Give Windows time to save the changes

            If Environment.UserName = "SYSTEM" Then
                RestartTime("") 'SYSTEM's the user programs use in Setup Mode, and nowhere else intentionally
            Else
                RestartTime("inwin") 'Otherwise restart Windows normally, instead of via API
            End If
        End If

        'Open the correct GUI for the current phase
        If HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("CurrentPhase") < 10 Then 'Normal phases
            Form1.Show()
        ElseIf HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("CurrentPhase") = 10 Then 'Uninstaller GUI
            UninstallForm.Show()
        ElseIf HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("CurrentPhase") = 69 Then 'Removal phase
            RemovalForm.Show()
        ElseIf HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("CurrentPhase") >= 97 And _
            HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("CurrentPhase") <= 99 Then 'Restoration phases
            RestorationForm.Show()
        ElseIf HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("CurrentPhase") = 102 Then 'Customisation stage 2
            forCustomise = True
            RestorationForm.Show()
        ElseIf HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("CurrentPhase") >= 103 Then 'Customisation stage 3+
            forCustomise = True
            Form1.Show()
        End If
        Me.Close()
    End Sub

    Private Sub PrepForm_Minimise(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.SizeChanged, MyBase.Resize
        Me.WindowState = FormWindowState.Minimized
    End Sub
End Class
