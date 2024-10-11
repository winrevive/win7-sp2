Public Class RemovalForm

#Region "Classic Theme"
    <DllImport("uxtheme.dll", CharSet:=CharSet.Auto)> _
    Public Shared Sub SetThemeAppProperties(ByVal Flags As Integer)
    End Sub
    <DllImport("uxtheme.dll", CharSet:=CharSet.Auto)> _
    Public Shared Function SetWindowTheme(hWnd As IntPtr, pszSubAppName As String, pszSubIdList As String) As Integer
    End Function
#End Region

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        SetThemeAppProperties(0)
        SetWindowTheme(Me.Handle, "", Nothing) 'Using this to send a WM_THEMECHANGED event
        SetWindowTheme(Me.ProgressBar1.Handle, "", Nothing) 'The progressbar otherwise doesn't use the proper theme
        RemovalBG.Show()

        If HKLMKey32.OpenSubKey("Software\Win8To7").GetValue("currentphase") = 69 Then
            Dim jobthread As New Thread(AddressOf Phase2)
            jobthread.Start()
        Else
            End
        End If
    End Sub

    Private Sub Form1_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        MsgBox("This operation cannot be cancelled.", MsgBoxStyle.Exclamation, "Win8To7 Transformation Pack Backend") 'lool Startup Repair reference
        e.Cancel = True 'Instead of closing, the Form just gets out-right ended anyway.
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

#Region "Label status changes"
    Private Sub SetProgress(ByVal value As String)
        'This bit here makes it work despite being called via a VB.NET Thread
        If Me.InvokeRequired Then
            Dim args As String = value
            Me.Invoke(New Action(Of String)(AddressOf SetProgress), args)
            Return
        End If

        ProgressText.Text = value
    End Sub
#End Region

#Region "Important calls"
    Private Sub ErrorOccurred(ByVal status As String)
        'This bit here makes it work despite being called via a VB.NET Thread
        If Me.InvokeRequired Then
            Dim args() As String = {status}
            Me.Invoke(New Action(Of String)(AddressOf ErrorOccurred), args)
            Return
        End If

        'Go back into Windows as an emergency precaution as well
        Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v CmdLine /t REG_SZ /f", AppWinStyle.Hide, True)
        Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v OOBEInProgress /t REG_DWORD /d 0 /f", AppWinStyle.Hide, True)
        Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SetupPhase /t REG_DWORD /d 0 /f", AppWinStyle.Hide, True)
        Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SetupType /t REG_DWORD /d 0 /f", AppWinStyle.Hide, True)
        Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SystemSetupInProgress /t REG_DWORD /d 0 /f", AppWinStyle.Hide, True)

        ErrorScreen.setStrings("removing", "Remove backed up pre-modification files and settings and uninstall", status) 'flavour text - refer to Form1 for comments
        ErrorScreen.Show()
        Me.Hide()
    End Sub
#End Region

#Region "Stage 2 of 2"
    Sub Phase2()
        Try
            'Make sure Windows is still in Setup Mode, in case an unexpected shutdown suddenly occurs
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v CmdLine /d " + windir + "\Temp\win8to7uninstall.exe" + " /t REG_SZ /f", AppWinStyle.Hide, True)
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v OOBEInProgress /t REG_DWORD /d 1 /f", AppWinStyle.Hide, True)
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SetupPhase /t REG_DWORD /d 4 /f", AppWinStyle.Hide, True)
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SetupType /t REG_DWORD /d 2 /f", AppWinStyle.Hide, True)
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SystemSetupInProgress /t REG_DWORD /d 1 /f", AppWinStyle.Hide, True)

            'DELETE REGISTRY BACKUPS
            SetProgress("Deleting Windows System Registry backups...")
            ' SYSTEM-WIDE
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg DELETE ""HKLM\SOFTWARE\Win8To7"" /f", AppWinStyle.Hide, True)
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg DELETE ""HKLM\SOFTWARE\Wow6432Node\Win8To7"" /f", AppWinStyle.Hide, True)

            ' USER-WIDE
            SetProgress("Preparing to delete User Registry backups...")
            Dim dirArr As IO.DirectoryInfo() = New IO.DirectoryInfo(windrive + "Users").GetDirectories() 'Required for getting list of directories in directory to loop through
            Dim loopdirinfo As IO.DirectoryInfo 'for the loop below
            Dim tries As Integer 'for file/folder deletion attempts

            'Unload HKLM\UserConfig first if loaded
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg unload ""HKLM\UserConfig""", AppWinStyle.Hide, True)
            Thread.Sleep(400)

            'For each Windows user, load NTUSER.DAT, delete backups, and unload their NTUSER.DAT
            For Each loopdirinfo In dirArr
                If Not IO.File.Exists(loopdirinfo.FullName + "\NTUSER.DAT") Then
                    Continue For 'Skip folder if not a Windows user or Default User Skeleton
                End If
                If loopdirinfo.Name = "All Users" Or loopdirinfo.Name = "Default User" Then
                    Continue For 'Skip symlinks
                End If
                SetProgress("Deleting Registry backups for user USERNAME".Replace("USERNAME", loopdirinfo.Name))

                'Load the NTUSER.DAT file to HKLM\UserConfig
                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg load ""HKLM\UserConfig"" """ + loopdirinfo.FullName + "\NTUSER.DAT""", AppWinStyle.Hide, True)

                'Delete the user's backups
                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg DELETE ""HKLM\UserConfig\Software\Win8To7"" /f", AppWinStyle.Hide, True)
                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg DELETE ""HKLM\UserConfig\Software\Wow6432Node\Win8To7"" /f", AppWinStyle.Hide, True)
                Thread.Sleep(400)

                'Finally, unload HKLM\UserConfig once again
                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg unload ""HKLM\UserConfig""", AppWinStyle.Hide, True)

                '...and delete our backups
                SetProgress("Deleting Win8To7 backups and files for user USERNAME...".Replace("USERNAME", loopdirinfo.Name))
                tries = 0
                While Not tries = 10
                    Shell(windir + "\" + sysprefix + "\cmd.exe /c del """ + loopdirinfo.FullName + storagelocationuser + """ /s /q /f /a", AppWinStyle.Hide, True)
                    Shell(windir + "\" + sysprefix + "\cmd.exe /c rd """ + loopdirinfo.FullName + storagelocationuser + """ /s /q", AppWinStyle.Hide, True)
                    If Not IO.Directory.Exists(loopdirinfo.FullName + storagelocationuser) Then
                        Exit While
                    End If
                    tries += 1
                End While
            Next

            'DELETE Win8To7 SYSTEM-WIDE FOLDER
            SetProgress("Deleting Win8To7 backups and files...")
            tries = 0
            While Not tries = 10
                Shell(windir + "\" + sysprefix + "\cmd.exe /c del """ + storagelocation + """ /s /q /f /a", AppWinStyle.Hide, True)
                Shell(windir + "\" + sysprefix + "\cmd.exe /c rd """ + storagelocation + """ /s /q", AppWinStyle.Hide, True)
                If Not IO.Directory.Exists(storagelocation) Then
                    Exit While
                End If
                tries += 1
            End While

            SetProgress("Completing uninstallation...")

            'Remove uninstaller from control
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg DELETE ""HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Win8To7TransformationPack"" /f", AppWinStyle.Hide, True)

            'Go back into Windows now it's all complete.
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v CmdLine /t REG_SZ /f", AppWinStyle.Hide, True)
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v OOBEInProgress /t REG_DWORD /d 0 /f", AppWinStyle.Hide, True)
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SetupPhase /t REG_DWORD /d 0 /f", AppWinStyle.Hide, True)
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SetupType /t REG_DWORD /d 0 /f", AppWinStyle.Hide, True)
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SystemSetupInProgress /t REG_DWORD /d 0 /f", AppWinStyle.Hide, True)

            SetProgress("Restarting Windows...")
            RestartTime("")
        Catch ex As Exception
            ErrorOccurred(ex.ToString())
            Exit Sub
        End Try
    End Sub
#End Region

End Class
