Public Class Form1

#Region "Variables"
    Private StatusText As String = <a>Transforming Windows... Stage PHASE of 4 - PROGRESS% complete.
Do not turn off your computer.</a>
    Private StatusTextCustomise As String = <a>Updating transformation... Stage PHASE of 4 - PROGRESS% complete.
Do not turn off your computer.</a> 'These are more-so based on how Windows Vista's updates status was layed out, but iirc Windows 7 SP0 shared Vista's updates status

    Private CurrentPhase As Integer = 0
    Private totalTasks As Integer = 0
    Private doneTasks As Integer = 0
#End Region

#Region "HC compatibility"
    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        Const WM_THEMECHANGED = &H31A '31A - refer to https://docs.microsoft.com/en-us/windows/win32/winmsg/wm-themechanged

        If (m.Msg = WM_THEMECHANGED) Then 'This is the most reliable way to react to theme changes
            RefreshHC()
            Me.Refresh() 'Force all controls to rePaint
        End If

        MyBase.WndProc(m) 'Needed - without it, you'll just get an unknown exception when the window loads
    End Sub

    Private Sub RefreshHC(Optional ByVal override As Boolean = False)
        If SystemInformation.HighContrast = True Then 'If HC is on, use Windows 7's High Contrast login design
            Me.BackgroundImage = Nothing ' Doesn't matter, but might as well in case it saves resources doing so
            PictureBox2.BackgroundImage = Nothing ' Same as Me.BackgroundImage, and just in case HC somehow doesn't disable background images
            Me.BackColor = Control.DefaultBackColor ' In Windows 7, High Contrast login uses the text colour and background colour of the current Classic colour scheme
            Me.ForeColor = Control.DefaultForeColor
        Else
            If override = True Then 'Reference Windows 7's "High Contrast" login screen when not high contrast
                Me.BackgroundImage = Nothing 'Windows 7 has a weird glitch where if you have HC in the login screen... but then log into someone with HC off, it never brings back the background
                Me.BackColor = Color.Black 'Thus, the background is left black, so this is a cheeky reference to that obscure glitch
                Me.ForeColor = Color.White 'and non-glitched's white text
            Else
                Me.BackgroundImage = My.Resources.int_SetupBG 'Otherwise, just make it look like it should
                Me.BackColor = Color.Black
                Me.ForeColor = Color.White
            End If
            If HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("Branding") = "win8pro" Then 'Use the appropriate branding as per what the user chose during pre-transforming
                PictureBox2.BackgroundImage = My.Resources.win8probranding
            ElseIf HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("Branding") = "win7pro" Then
                PictureBox2.BackgroundImage = My.Resources.win7probranding
            ElseIf HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("Branding") = "win7ult" Then
                PictureBox2.BackgroundImage = My.Resources.win7ultimatebranding
            ElseIf HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("Branding") = "win7home" Then
                PictureBox2.BackgroundImage = My.Resources.win7homebranding
            ElseIf HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("Branding") = "win8home" Then
                PictureBox2.BackgroundImage = My.Resources.win8homebranding
            ElseIf HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("Branding") = "win7starter" Then
                PictureBox2.BackgroundImage = My.Resources.win7starterbranding
            ElseIf HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("Branding") = "win8starter" Then
                PictureBox2.BackgroundImage = My.Resources.win8starterbranding
            ElseIf HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("Branding") = "win7enterprise" Then
                PictureBox2.BackgroundImage = My.Resources.win7enterprisebranding
            ElseIf HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("Branding") = "win8enterprise" Then
                PictureBox2.BackgroundImage = My.Resources.win8enterprisebranding
            End If
        End If
    End Sub
#End Region

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Cursor.Hide() 'Hide the cursor immediately as Windows 7 shows the cursor slightly later

        Functions.SetErrorOccurred(New Action(Of String)(AddressOf ErrorOccurred)) 'Mark this Form's ErrorOccurred as the one for Functions to call if required

        'High Contrast check
        If HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("HighContrast") = 1 Then 'Do a HC check manually as ThemeChanged only triggers if the theme changes
            RefreshHC(True)
        Else
            RefreshHC()
        End If

        Thread.Sleep(800)
        Cursor.Show() 'Simulate every Windows 7 post-bootscreen startup
        Thread.Sleep(200)
        FakeIntro.Cursor = Cursors.AppStarting
        Thread.Sleep(300)
        FakeIntro.Cursor = Cursors.Arrow
        Thread.Sleep(700)
        FakeIntro.Cursor = Cursors.WaitCursor
        Thread.Sleep(700)
        FakeIntro.Cursor = Cursors.Arrow
        FakeIntro.Hide() 'Alright, we're done showing the intro, let's get on with it
        Cursor.Hide()

        If HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("CurrentPhase") = 2 Then 'Load the code for the appropriate phase, in a separate thread
            Dim jobthread As New Thread(AddressOf Phase2) 'without a thread, it'd freeze the GUI, heck make the program be classed as currently 'Not Responding'
            jobthread.Start()
        ElseIf HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("CurrentPhase") = 3 Then
            Dim jobthread As New Thread(AddressOf Phase3)
            jobthread.Start()
        ElseIf HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("CurrentPhase") = 4 Then
            Dim jobthread As New Thread(AddressOf Phase4)
            jobthread.Start()
        ElseIf HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("CurrentPhase") = 103 Then 'Stage 103 = Stage 2, with slight differences
            Dim jobthread As New Thread(AddressOf Phase2)
            jobthread.Start()
        ElseIf HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("CurrentPhase") = 104 Then 'Stage 104 = Stage 4, with slight differences
            Dim jobthread As New Thread(AddressOf Phase4)
            jobthread.Start()
        Else
            End
        End If
    End Sub

    Private Sub Form1_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        e.Cancel = True 'Instead of closing, the Form just gets out-right ended anyway.
    End Sub

#Region "Label status changes"
    Private Sub AboutToRestart(ByVal status As String)
        'This bit here makes it work despite being called via a VB.NET Thread
        If Me.InvokeRequired Then
            Dim args() As String = {status}
            Me.Invoke(New Action(Of String)(AddressOf AboutToRestart), args)
            Return
        End If

        LabelStatus.Text = "Restarting" 'Set the status text
    End Sub

    Private Sub ChangeProgress(ByVal progress As Integer)
        'This bit here makes it work despite being called via a VB.NET Thread
        If Me.InvokeRequired Then
            Dim args() As String = {progress}
            Me.Invoke(New Action(Of String)(AddressOf ChangeProgress), args)
            Return
        End If

        If forCustomise = False Then
            LabelStatus.Text = StatusText.Replace("PROGRESS", progress.ToString()).Replace("PHASE", CurrentPhase.ToString())
        Else
            LabelStatus.Text = StatusTextCustomise.Replace("PROGRESS", progress.ToString()).Replace("PHASE", CurrentPhase.ToString())
        End If 'Set the label text to the appropriate progress text but with PROGRESS and PHASE changed to the appropriate number for each
    End Sub

    Private Sub Phase4Progress() 'thread that's only for Stage 4 to calculate its current progress based on tasks completed, and update GUI progress
        While True
            ChangeProgress(Math.Floor((doneTasks / totalTasks) * 100))
            Thread.Sleep(80)
        End While
    End Sub
#End Region

#Region "Registry functions"
    Public Function BackupRegistry(ByVal Key As String, Optional ByVal Value As String = "", Optional ByVal Recursive As Boolean = False)
        Dim tempArray As String() = Key.Split("\")
        Dim locationPrefix As String
        Dim errorfilelocation As String = windir + "\TEMP\regchangeerror" + rng.Next(0, 999999999 + 1).ToString() 'Figure out a good file location for reporting any exceptions regchange encounters
        While IO.File.Exists(errorfilelocation) 'Make sure our 'good file location' doesn't already exist
            errorfilelocation = windir + "\TEMP\regchangeerror" + rng.Next(0, 999999999 + 1).ToString()
            If IO.File.Exists(errorfilelocation) Then
                Try 'Anti-softlock for this loop by deleting existing exception-logs if possible
                    IO.File.Delete(errorfilelocation)
                Catch
                End Try
            End If
        End While

        'Add a prefix to the target location according to the data presented
        If Key.StartsWith("HKLM\UserConfig") Then
            locationPrefix = "HKLM\UserConfig\Software\Win8To7\RegBackup\HKCU"
        Else
            locationPrefix = "HKLM\SOFTWARE\Win8To7\RegBackup\" + tempArray.GetValue(0)
        End If

        Shell(storagelocation + "\SetupTools\regchange.exe backup """ + Key + """ """ + Value + """ """ + locationPrefix + """ """ + errorfilelocation + """ " + Recursive.ToString(), AppWinStyle.Hide, True)

        If IO.File.Exists(errorfilelocation) Then 'If the file exists, it means it encountered an error - fail with the error encountered
            ErrorOccurred(IO.File.ReadAllText(errorfilelocation))
            Return False
        End If
        Return True
    End Function
#End Region

#Region "Important calls"
    Private Sub ErrorOccurred(ByVal status As String)
        'This bit here makes it work despite being called via a VB.NET Thread
        If Me.InvokeRequired Then
            Dim args() As String = {status}
            Me.Invoke(New Action(Of String)(AddressOf ErrorOccurred), args)
            Return
        End If

        If forCustomise = True Then
            HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("CurrentPhase", 10)
        End If

        'Mark transformation as failed
        HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("TransformationFailed", 1)

        'Go back into Windows as an emergency precaution as well
        Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v CmdLine /t REG_SZ /f", AppWinStyle.Hide, True)
        Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v OOBEInProgress /t REG_DWORD /d 0 /f", AppWinStyle.Hide, True)
        Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SetupPhase /t REG_DWORD /d 0 /f", AppWinStyle.Hide, True)
        Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SetupType /t REG_DWORD /d 0 /f", AppWinStyle.Hide, True)
        Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SystemSetupInProgress /t REG_DWORD /d 0 /f", AppWinStyle.Hide, True)

        ErrorScreen.setStrings("transforming", "Transforming Windows", status) 'Set the strings for the Error Screen to have fitting flavour text for what this form does
        ErrorScreen.Show()
        Me.Hide()
    End Sub
#End Region

#Region "Stage 2 of 4"
    Sub Phase2()
        Try
            If forCustomise = False Then
                CurrentPhase = 2
            Else
                CurrentPhase = 3
            End If
            ChangeProgress(0)

            Dim deletionsTarget As String = getRandomTEMPfolder()

            If forCustomise = True Then 'If we're updating an existing installation of this transformation pack...
                'Make sure Windows is still in Setup Mode, in case an unexpected shutdown suddenly occurs (only needed for update, as Stage 2 doesn't occur in Setup Mode on first installs)
                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v CmdLine /d " + storagelocation + "\SetupTools\setup.exe" + " /t REG_SZ /f", AppWinStyle.Hide, True)
                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v OOBEInProgress /t REG_DWORD /d 1 /f", AppWinStyle.Hide, True)
                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SetupPhase /t REG_DWORD /d 4 /f", AppWinStyle.Hide, True)
                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SetupType /t REG_DWORD /d 2 /f", AppWinStyle.Hide, True)
                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SystemSetupInProgress /t REG_DWORD /d 1 /f", AppWinStyle.Hide, True)

                'DELETE FILES AND PROGRAM-LEFTOVERS FROM OLD TRANSFORMATION PACK VERSION
                Dim tries As Integer 'Delete and re-create directories, first.
                For Each direc In {storagelocation + "\SetupFiles", windir + "\Temp\Win8To7DiscardedFiles", windir + "\" + sysprefix + "\OldNewExplorer", _
                                   windrive + "Program Files\Microsoft Games", windrive + "Program Files\ClassicTaskmgr", windir + "\" + sysprefix + "\Win8To7", _
                                   windrive + "Program Files (x86)\Windows Sidebar", windrive + "Program Files\Windows Sidebar", windir + "\explorer7", _
                                   windrive + "Program Files (x86)\StartIsBack", windrive + "Program Files\StartIsBack", windrive + "Program Files\Open-Shell"} 'NOTE: At this point, we already uninstalled these programs.
                    tries = 0
                    While Not tries = 10
                        Shell(windir + "\" + sysprefix + "\cmd.exe /c del """ + direc + """ /s /q /f /a", AppWinStyle.Hide, True)
                        Shell(windir + "\" + sysprefix + "\cmd.exe /c rd """ + direc + """ /s /q", AppWinStyle.Hide, True)
                        If Not IO.Directory.Exists(direc) Then
                            Exit While
                        End If
                        tries += 1
                    End While
                Next
                Shell(windir + "\" + sysprefix + "\cmd.exe /c del """ + windrive + "ProgramData\Microsoft\Windows\Start Menu\Programs\Games\GameExplorer.lnk"" /s /q /f /a", AppWinStyle.Hide, True) 'Delete the remaining shortcut Win7Games left
                Shell(windir + "\" + sysprefix + "\cmd.exe /c del """ + storagelocation + "\setupold.exe"" /s /q /f /a", AppWinStyle.Hide, True) 'Delete the old Setup executable, now it's completely unnecessary
            End If

            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SOFTWARE\Win8To7GS"" /v ""Branding"" /t REG_SZ /d """ + HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("Branding") + """ /f", AppWinStyle.Hide, True)

            'Extract everything before running
            If ExtractEverything() = False Then 'Abort if extracting fails
                Exit Sub
            End If
            ChangeProgress(10)

            'First step: Run Installers
            'Install OldNewExplorer
            Shell(windir + "\" + sysprefix + "\cmd.exe /c regsvr32 " + windir + "\System32\OldNewExplorer\OldNewExplorer32.dll /s", AppWinStyle.NormalFocus, True, 2400000) 'OldNewExplorer doesn't have an installer,
            Shell(windir + "\" + sysprefix + "\cmd.exe /c regsvr32 " + windir + "\System32\OldNewExplorer\OldNewExplorer64.dll /s", AppWinStyle.NormalFocus, True, 2400000) 'so we do what ONE does to install it.
            ChangeProgress(20)

            If forCustomise = False Then 'If this is an update installation, then don't install Aero Glass and UXThemePatcher again as Aero Glass is only partially installed if installed in Setup Mode
                '(where we're doing this during an Update install), and UXThemePatcher would be a waste of time to 'install' given it's already installed (Update doesn't uninstall them)
                If Not IO.File.Exists(windrive + "AeroGlass\unins000.exe") Then
                    'Install Glass8
                    Shell(storagelocation + "\SetupFiles\Glass8.exe /VERYSILENT /CLOSEAPPLICATIONS /NORESTART /SUPPRESSMSGBOXES /TASKS=", AppWinStyle.NormalFocus, True, 2400000) 'Run in Quiet/Automatic mode
                    Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{277BA0F1-D0BB-4D73-A2DF-6B60C91E1533}_is1"" /v ""SystemComponent"" /t REG_DWORD /d 1 /f", AppWinStyle.Hide, True)
                    'SystemComponent means that it won't appear in Programs and Features - this is especially important for programs like UXThemePatcher
                End If
                ChangeProgress(35)

                If System.Environment.OSVersion.Version.Minor = 3 And Not IO.File.Exists(windrive + "Program Files\7+ Taskbar Tweaker\uninstall.exe") Then 'Don't need 7TT in 8.0 'cos Show Desktop's the correct size in 8.0
                    'Install 7TaskbarTweaker
                    Shell(storagelocation + "\SetupFiles\7tt.exe /S /D=" + windrive + "Program Files\7+ Taskbar Tweaker", AppWinStyle.NormalFocus, True, 2400000)
                    IO.File.Delete(appdataroaming + "\Microsoft\Windows\Start Menu\Programs\7+ Taskbar Tweaker.lnk")
                    Shell(windir + "\" + sysprefix + "\cmd.exe /c reg DELETE ""HKCU\Software\Microsoft\Windows\CurrentVersion\Uninstall\7 Taskbar Tweaker"" /f", AppWinStyle.Hide, True)
                End If
            End If 'Everything else is free game to reinstall in Update installs since the rest is uninstalled last stage
            ChangeProgress(50)

            If HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("Allow7Games") = "true" And Not IO.File.Exists(windrive + "Program Files\Microsoft Games\unwin7games.exe") Then
                'Install 7GamesPack
                Shell(storagelocation + "\SetupFiles\7GamesInstaller.exe /S", AppWinStyle.NormalFocus, True, 2400000)
                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Win7Games"" /v ""SystemComponent"" /t REG_DWORD /d 1 /f", AppWinStyle.Hide, True)
            End If
            ChangeProgress(60)

            If HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("Allow7TaskManager") = "true" And Not IO.File.Exists(windrive + "Program Files\ClassicTaskmgr\unins000.exe") Then
                'Install Classic Task Manager
                Shell(storagelocation + "\SetupFiles\ClassicTM.exe /VERYSILENT /CLOSEAPPLICATIONS /NORESTART /SUPPRESSMSGBOXES /TASKS=", AppWinStyle.NormalFocus, True, 2400000)
                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Classic Task Manager+msconfig for Win 10 and 8_is1"" /v ""SystemComponent"" /t REG_DWORD /d 1 /f", AppWinStyle.Hide, True)
            End If
            ChangeProgress(70)

            If HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("Allow7Gadgets") = "true" And Not IO.File.Exists(windrive + "Program Files\Windows Sidebar\sidebar.exe") Then
                'Install Windows Desktop Gadgets (using an alternative running method because Shell seems to quit it before it has completed sometimes)
                startInfo = New System.Diagnostics.ProcessStartInfo
                startInfo.FileName = storagelocation + "\SetupFiles\GadgetInstaller.exe"
                startInfo.Arguments = "/VERYSILENT /CLOSEAPPLICATIONS /NORESTART /SUPPRESSMSGBOXES /TASKS="
                startInfo.CreateNoWindow = True
                startInfo.UseShellExecute = True
                startInfo.WindowStyle = ProcessWindowStyle.Hidden

                MyProcess = Process.Start(startInfo)
                MyProcess.WaitForExit()

                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Windows Desktop Gadgets_is1"" /v ""SystemComponent"" /t REG_DWORD /d 1 /f", AppWinStyle.Hide, True)
            End If
            ChangeProgress(80)

            If HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("AllowMediaCenter") = "true" Then
                'Install Windows Media Center
                If Not Microsoft.Win32.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "EditionID", "Professional").ToString.StartsWith("ProfessionalWMC") And _
                    System.Environment.OSVersion.Version.Minor = 3 Then
                    Shell(sysprefix + "\msiexec.exe /i " + storagelocation + "\SetupFiles\MediaCenter.msi /passive", AppWinStyle.NormalFocus, True, 2400000)
                End If
                If IO.File.Exists(windrive + "Users\Public\Desktop\Windows Media Center.lnk") Then
                    Try
                        IO.File.Delete(windrive + "Users\Public\Desktop\Windows Media Center.lnk")
                    Catch 'Ignore this failing if it does somehow
                    End Try
                End If
                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{99CCD11D-435B-4662-A48C-3AC046EC7014}"" /v ""SystemComponent"" /t REG_DWORD /d 1 /f", AppWinStyle.Hide, True)
            End If
            ChangeProgress(90)

            'Install Open Shell, if selected
            If IO.File.Exists(storagelocation + "\SetupFiles\OpenShellSetup.exe") Then
                Shell(storagelocation + "\SetupFiles\OpenShellSetup.exe /qn ADDLOCAL=StartMenu,Update", AppWinStyle.NormalFocus, True, 2400000)
                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{F4B6EE58-F183-4B0D-930B-4480673C0F5B}"" /v ""SystemComponent"" /t REG_DWORD /d 1 /f", AppWinStyle.Hide, True)
            End If
            'Install Ex7ForWin8, if selected
            If IO.File.Exists(storagelocation + "\SetupFiles\ex7forw8.exe") And Not IO.File.Exists(windir + "\explorer7\ex7forw8.exe") Then
                Shell(storagelocation + "\SetupFiles\ex7forw8.exe /silent", AppWinStyle.NormalFocus, True, 2400000)
                If Environment.Is64BitOperatingSystem = True Then
                    Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\Ex7forW8"" /v ""SystemComponent"" /t REG_DWORD /d 1 /f", AppWinStyle.Hide, True)
                Else
                    Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Ex7forW8"" /v ""SystemComponent"" /t REG_DWORD /d 1 /f", AppWinStyle.Hide, True)
                End If
            End If
            'Install StartIsBack, if selected
            If IO.File.Exists(storagelocation + "\SetupFiles\StartIsBack.exe") And HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("Start") = "startisback" Then
                Shell(storagelocation + "\SetupFiles\StartIsBack.exe /elevated /silent", AppWinStyle.NormalFocus, True, 2400000)
                If Environment.Is64BitOperatingSystem = True Then
                    Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\StartIsBack"" /v ""SystemComponent"" /t REG_DWORD /d 1 /f", AppWinStyle.Hide, True)
                Else
                    Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\StartIsBack"" /v ""SystemComponent"" /t REG_DWORD /d 1 /f", AppWinStyle.Hide, True)
                End If
            End If
            ChangeProgress(100)

            If forCustomise = False Then
                'Boot into installer for Phase 3 with the Setup Mode if this is the first ever transformation done, and not an update
                HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("CurrentPhase", 3)
                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v CmdLine /d " + storagelocation + "\SetupTools\setup.exe" + " /t REG_SZ /f", AppWinStyle.Hide, True)
                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v OOBEInProgress /t REG_DWORD /d 1 /f", AppWinStyle.Hide, True)
                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SetupPhase /t REG_DWORD /d 4 /f", AppWinStyle.Hide, True)
                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SetupType /t REG_DWORD /d 2 /f", AppWinStyle.Hide, True)
                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SystemSetupInProgress /t REG_DWORD /d 1 /f", AppWinStyle.Hide, True)
            Else
                'Boot into installer once again with the Setup Mode if this is a transformation update, however:
                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v CmdLine /d " + storagelocation + "\SetupTools\setup.exe" + " /t REG_SZ /f", AppWinStyle.Hide, True)
                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v OOBEInProgress /t REG_DWORD /d 1 /f", AppWinStyle.Hide, True)
                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SetupPhase /t REG_DWORD /d 4 /f", AppWinStyle.Hide, True)
                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SetupType /t REG_DWORD /d 2 /f", AppWinStyle.Hide, True)
                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SystemSetupInProgress /t REG_DWORD /d 1 /f", AppWinStyle.Hide, True)
                'Go to Stage 104 (Customisation Stage 4)
                HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("CurrentPhase", 104)
            End If

            Thread.Sleep(8000) 'Grant more time for programs to actually finish their installations

            AboutToRestart("")
            If forCustomise = False Then 'If this is the first transformation, and not an update of a prior transformation...
                If SystemInformation.HighContrast = True Then 'If we're in High Contrast, remember that we are for an easter egg to occur for the rest of the transformation process
                    HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("HighContrast", 1)
                Else
                    HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("HighContrast", 0)
                End If
            End If
            RestartTime("inwin") 'Restart Windows once complete
        Catch ex As Exception
            ErrorOccurred(ex.ToString())
            Exit Sub
        End Try
    End Sub
#End Region
#Region "Stage 3 of 4"
    Sub Phase3()
        Try
            CurrentPhase = 3
            ChangeProgress(0)

            'Make sure Windows is still in Setup Mode, in case an unexpected shutdown suddenly occurs
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v CmdLine /d " + storagelocation + "\SetupTools\setup.exe" + " /t REG_SZ /f", AppWinStyle.Hide, True)
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v OOBEInProgress /t REG_DWORD /d 1 /f", AppWinStyle.Hide, True)
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SetupPhase /t REG_DWORD /d 4 /f", AppWinStyle.Hide, True)
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SetupType /t REG_DWORD /d 2 /f", AppWinStyle.Hide, True)
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SystemSetupInProgress /t REG_DWORD /d 1 /f", AppWinStyle.Hide, True)

            If HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("AllowUXThemePatcher") = "true" And IO.File.Exists(storagelocation + "\SetupFiles\UXThemePatcher.exe") Then
                'Install UXThemePatcher
                Shell(storagelocation + "\SetupFiles\UXThemePatcher.exe /S", AppWinStyle.NormalFocus, True, 2400000)
                ' Backend NOTE: Newer versions of UXThemePatcher no longer respect /S - if you need a modern version of UXThemePatcher, good luck I guess.
                If Environment.Is64BitOperatingSystem = True Then
                    Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\UltraUXThemePatcher"" /v ""SystemComponent"" /t REG_DWORD /d 1 /f", AppWinStyle.Hide, True)
                Else
                    Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\UltraUXThemePatcher"" /v ""SystemComponent"" /t REG_DWORD /d 1 /f", AppWinStyle.Hide, True)
                End If
                Try
                    IO.File.Delete(System.Environment.GetEnvironmentVariable("USERPROFILE") + "\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\UltraUXThemePatcher\Uninstall.lnk")
                    'Make sure it's as hard as possible to uninstall UXThemePatcher as doing so WILL effectively BRICK Windows (unless done during restoration, that's the only exception)
                Catch 'Ignore this failing if it does somehow
                End Try
            End If

            'Delete now-pointless Setup Files
            Shell(windir + "\" + sysprefix + "\cmd.exe /c del /s /q /a /f """ + storagelocation + "\SetupFiles""")
            ChangeProgress(25)

            'Now copy shell32.dll to shell32a.dll, as part of the anti-bricking setup
            If Not IO.File.Exists(windir + "\" + sysprefix + "\shell32a.dll") Then
                Try
                    IO.File.Copy(windir + "\" + sysprefix + "\shell32.dll", windir + "\" + sysprefix + "\shell32a.dll", True)
                Catch ex As Exception
                    ErrorOccurred("Failed to back up shell32.dll in advance to prevent bricking your Windows installation: " + ex.ToString())
                    Exit Sub
                End Try
            End If
            'Now that is done, change SHELL32.DLL dependency to be shell32a.dll to prevent Windows getting bricked by the next phase
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\ControlSet001\Control\Session Manager\KnownDLLs"" /v SHELL32 /t REG_SZ /d shell32a.dll /f", AppWinStyle.Hide, True)
            ChangeProgress(50)

            Thread.Sleep(3000) 'This is done to make sure it doesn't seem like all it's doing is restarting for no reason - +3 seconds of 3/4

            'Boot into installer once again for Phase 4 with the Setup Mode
            HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("CurrentPhase", 4)
            ChangeProgress(75)
            ChangeProgress(100)
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v CmdLine /d " + storagelocation + "\SetupTools\setup.exe" + " /t REG_SZ /f", AppWinStyle.Hide, True)
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v OOBEInProgress /t REG_DWORD /d 1 /f", AppWinStyle.Hide, True)
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SetupPhase /t REG_DWORD /d 4 /f", AppWinStyle.Hide, True)
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SetupType /t REG_DWORD /d 2 /f", AppWinStyle.Hide, True)
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SystemSetupInProgress /t REG_DWORD /d 1 /f", AppWinStyle.Hide, True)
            AboutToRestart("")
            RestartTime("")
        Catch ex As Exception
            ErrorOccurred(ex.ToString())
            Exit Sub
        End Try
    End Sub
#End Region
#Region "Stage 4 of 4"
    Sub Phase4()
        Try
            CurrentPhase = 4
            ChangeProgress(0)

            'Make sure Windows is still in Setup Mode, in case an unexpected shutdown suddenly occurs
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v CmdLine /d " + storagelocation + "\SetupTools\setup.exe" + " /t REG_SZ /f", AppWinStyle.Hide, True)
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v OOBEInProgress /t REG_DWORD /d 1 /f", AppWinStyle.Hide, True)
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SetupPhase /t REG_DWORD /d 4 /f", AppWinStyle.Hide, True)
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SetupType /t REG_DWORD /d 2 /f", AppWinStyle.Hide, True)
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SystemSetupInProgress /t REG_DWORD /d 1 /f", AppWinStyle.Hide, True)

            Dim directoriesList As List(Of String)

            Dim fiArr As IO.FileInfo()
            Dim loopfileinfo As IO.FileInfo

            Dim targetPath As String

            Dim MultiSZFileChanges As New List(Of String) 'We'll use this to store our changed files in Registry via multiSZ
            Dim MultiSZUserFileChanges As New List(Of String) 'same, but for user-specific file changes
            Dim MultiSZFolderChanges As New List(Of String) 'We'll use this to store our intentionally created folders in Registry via multiSZ
            Dim MultiSZUserFolderChanges As New List(Of String) 'same, but for user-specific folder creations

            'First, count the tasks to do
            ' 1: Files/Folders to delete
            totalTasks += cfgs.itemsToDelete.Count
            ' 2: Files to replace
            directoriesList = ListDirectory(storagelocation + "\FileReplacements")
            For Each item In directoriesList
                fiArr = New IO.DirectoryInfo(item).GetFiles() 'Get the files in this directory
                For Each loopfileinfo In fiArr
                    totalTasks += 1
                Next
            Next
            ' 3: ResFiles to patch
            directoriesList = ListDirectory(storagelocation + "\ResFiles")
            For Each item In directoriesList
                fiArr = New IO.DirectoryInfo(item).GetFiles() 'Get the files in this directory
                For Each loopfileinfo In fiArr
                    totalTasks += 1
                Next
            Next
            totalTasks += cfgs.itemsToMove.Count ' 3: Files/Folders to move
            totalTasks += 2 ' 4: Deleting Swatches and setting 7++ autostart
            ' 5: Registry keys
            For Each key In regtweaks.SystemTweaks.Item("Delete")
                totalTasks += 1
            Next
            For Each key In regtweaks.SystemTweaks.Item("DWORD")
                totalTasks += 1
            Next
            For Each key In regtweaks.SystemTweaks.Item("Binary")
                totalTasks += 1
            Next
            For Each key In regtweaks.SystemTweaks.Item("String")
                totalTasks += 1
            Next
            For Each key In regtweaks.SystemTweaks.Item("MultiString")
                totalTasks += 1
            Next
            ' 6: User-wide changes, multiplied by each user
            Dim dirArr As IO.DirectoryInfo() = New IO.DirectoryInfo(windrive + "Users").GetDirectories() 'Required for getting list of directories in directory to loop through
            Dim loopdirinfo As IO.DirectoryInfo 'for the loop below
            For Each loopdirinfo In dirArr
                If Not IO.File.Exists(loopdirinfo.FullName + "\NTUSER.DAT") Then
                    Continue For 'Skip folder if not a Windows user or Default User Skeleton
                End If
                If loopdirinfo.Name = "All Users" Or loopdirinfo.Name = "Default User" Then
                    Continue For 'Skip symlinks
                End If
                totalTasks += cfgs.userItemsToDelete.Count
                directoriesList = ListDirectory(storagelocation + "\UserFileReplacements")
                For Each item In directoriesList
                    fiArr = New IO.DirectoryInfo(item).GetFiles() 'Get the files in this directory
                    For Each loopfileinfo In fiArr
                        totalTasks += 1
                    Next
                Next
                totalTasks += cfgs.userItemsToMove.Count
                For Each key In regtweaks.UserTweaks.Item("Delete")
                    totalTasks += 1
                Next
                For Each key In regtweaks.UserTweaks.Item("DWORD")
                    totalTasks += 1
                Next
                For Each key In regtweaks.UserTweaks.Item("Binary")
                    totalTasks += 1
                Next
                For Each key In regtweaks.UserTweaks.Item("String")
                    totalTasks += 1
                Next
                For Each key In regtweaks.UserTweaks.Item("MultiString")
                    totalTasks += 1
                Next
                totalTasks += 1 '7: Cleanup of Icons Caches
            Next
            totalTasks += 3 '8: Cleanup stuff
            totalTasks += My.Settings.itemsToDelete.Count '9: Files to delete

            'Start the background thread for changing the progress over time in this stage
            Dim jobthread As New Thread(AddressOf Phase4Progress)
            jobthread.Start() 'launch the progress thingy


            'SYSTEM-WIDE
            ' FILE AND FOLDER DELETIONS
            For Each item In cfgs.itemsToDelete
                If IO.Directory.Exists(ToNative(windrive + item)) Then
                    directoriesList = ListDirectory(windrive + item) 'Get all the folders in the directory recursively
                    For Each item2 In directoriesList
                        fiArr = New IO.DirectoryInfo(item2).GetFiles() 'Get all the files in that directory
                        For Each loopfileinfo In fiArr
                            If Not MultiSZUserFileChanges.Contains(loopfileinfo.FullName) Then
                                MultiSZFileChanges.Add(loopfileinfo.FullName) 'Add contents to file changes
                            End If
                        Next
                    Next
                    'Move original folder to backups area
                    If MoveFolder(windrive + item, storagelocation + "\Backups\" + item, "folder deletions backups", False) = False Then
                        Exit Sub 'It failed, and we can't just exit sub via the function, so this is done.
                    End If
                ElseIf IO.File.Exists(ToNative(windrive + item)) Then
                    'Move original file to backups area
                    If MoveFile(windrive + item, storagelocation + "\Backups\" + item, "file deletions backups", False) = False Then
                        Exit Sub
                    End If
                    If Not MultiSZFileChanges.Contains(windrive + item) Then
                        MultiSZFileChanges.Add(windrive + item)
                    End If
                End If
            Next

            ' FILE AND FOLDER MOVEMENTS
            Dim changedFiles As New List(Of String)
            For Each key In cfgs.itemsToMove
                If IO.Directory.Exists(windrive + key.Value.Item(0)) And Not IO.Directory.Exists(windrive + key.Value.Item(1)) Then
                    If Not MultiSZFolderChanges.Contains(windrive + key.Value.Item(1)) Then 'Keep track of folders created from movements, so that during restoration we delete them
                        MultiSZFolderChanges.Add(windrive + key.Value.Item(1))
                    End If
                End If

                changedFiles = MoveUnknown(windrive + key.Value.Item(0), windrive + key.Value.Item(1), "file and folder movements", True) 'This is a call for calling MoveFile or MoveFolder depending on item typing, only for instances where we don't know if it's a file or folder being acted on currently
                If changedFiles.Contains("FAILURE:") Then
                    Exit Sub 'This means the function failed, and since we can't just Exit Sub there, this is done to compensate
                End If
                For Each item In changedFiles
                    If Not MultiSZFileChanges.Contains(item) Then
                        MultiSZFileChanges.Add(item)
                    End If
                Next
                doneTasks += 1
            Next

            ' FILE REPLACEMENTS
            '  Before we do that, let's get the directories
            directoriesList = ListDirectory(storagelocation + "\FileReplacements")
            For Each item In directoriesList
                fiArr = New IO.DirectoryInfo(item).GetFiles() 'Get the files in this directory
                For Each loopfileinfo In fiArr
                    'Move original file to backups area
                    If MoveFile(loopfileinfo.FullName.Replace(storagelocation + "\FileReplacements\", windrive), loopfileinfo.FullName.Replace(storagelocation + "\FileReplacements\", storagelocation + "\Backups\"), _
                                "file replacements backups", False) = False Then
                        Exit Sub
                    End If
                    'Move new file to its intended location
                    If MoveFile(loopfileinfo.FullName, loopfileinfo.FullName.Replace(storagelocation + "\FileReplacements\", windrive), "file replacements replacements", False) = False Then
                        Exit Sub
                    End If

                    doneTasks += 1 'Add 1 to doneTasks
                    If Not MultiSZFileChanges.Contains(loopfileinfo.FullName.Replace(storagelocation + "\FileReplacements\", windrive)) Then
                        MultiSZFileChanges.Add(loopfileinfo.FullName.Replace(storagelocation + "\FileReplacements\", windrive)) 'Add contents to file changes
                    End If
                Next
            Next

            ' RES FILE-PATCHES
            '  Before we do that, let's get the directories
            directoriesList = ListDirectory(storagelocation + "\ResFiles")
            Dim reslessname As String 'We use this to store the target file's full path

            Dim startInfo As New System.Diagnostics.ProcessStartInfo
            Dim MyProcess As Process

            For Each item In directoriesList
                fiArr = New IO.DirectoryInfo(item).GetFiles() 'Get the files in this directory
                For Each loopfileinfo In fiArr
                    'Get the .res extension outta here
                    reslessname = loopfileinfo.FullName.Replace(storagelocation + "\ResFiles\", windrive) 'Replace ResFiles path with C:\ so that we can approximate where on the Windows install this goes
                    reslessname = reslessname.Substring(0, reslessname.Length - 4) 'cuts out '.res' from the end of the filename

                    If Not IO.File.Exists(ToNative(reslessname)) Then
                        doneTasks += 1
                        Continue For 'Skip non-existent target files
                    End If

                    'Copy original file to backups area if it hasn't yet been backed up
                    If Not IO.File.Exists(reslessname.Replace(windrive, storagelocation + "\Backups\")) Then
                        If MoveFile(reslessname, reslessname.Replace(windrive, storagelocation + "\Backups\"), "file patches backups", True) = False Then
                            Exit Sub
                        End If
                    End If
                    reslessname = ToNative(reslessname) 'WOW32 countermeasure - System32 (x64) = SysNative in WOW32 - for ResHacker

                    'Second, run Resource Hacker to patch the file
                    startInfo = New System.Diagnostics.ProcessStartInfo
                    startInfo.FileName = storagelocation + "\SetupTools\ResourceHacker.exe"
                    startInfo.Arguments = "-action addoverwrite -open """ + reslessname + """ -save """ + reslessname + """ -resource """ + loopfileinfo.FullName + """"
                    startInfo.CreateNoWindow = True
                    startInfo.UseShellExecute = True
                    startInfo.WindowStyle = ProcessWindowStyle.Hidden

                    MyProcess = Process.Start(startInfo)
                    MyProcess.WaitForExit(2400000) '2400000 milisecond time limit should be long enough given Resource Hacker takes a long time on imageres.dll and such
                    If (MyProcess.ExitCode <> 0) Then
                        ErrorOccurred("Resource Hacker failed to patch file: " + reslessname)
                        Exit Sub
                    End If
                    doneTasks += 1

                    Try
                        IO.File.Delete(loopfileinfo.FullName) 'Delete the file now we're done with it
                    Catch
                    End Try
                    If Not MultiSZFileChanges.Contains(loopfileinfo.FullName.Replace(storagelocation + "\ResFiles\", windrive)) Then
                        MultiSZFileChanges.Add(loopfileinfo.FullName.Replace(storagelocation + "\ResFiles\", windrive))
                    End If
                Next
            Next


            ' REGISTRY CHANGES - SYSTEM-WIDE
            '  Back up Swatches first
            If BackupRegistry("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches", "", True) = False Then 'Backup first
                Exit Sub
            End If
            '  Delete Swatches entirely first
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg DELETE ""HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches"" /f", AppWinStyle.Hide, True) 'and then make changes
            doneTasks += 1
            '  Then tackle remaining deletions
            For Each key In regtweaks.SystemTweaks.Item("Delete")
                If BackupRegistry(key.Item(0), key.Item(1), False) = False Then 'Backup first
                    Exit Sub
                End If
                regtweaks.DeleteKey(key.Item(0), key.Item(1), True) 'and then make changes
                doneTasks += 1
            Next
            For Each key In regtweaks.SystemTweaks.Item("DWORD")
                If BackupRegistry(key.Item(0), key.Item(1), False) = False Then
                    Exit Sub
                End If
                regtweaks.SetDWORD(key.Item(0), key.Item(1), regtweaks.translate(key.Item(2)), True)
                doneTasks += 1
            Next
            For Each key In regtweaks.SystemTweaks.Item("Binary")
                If BackupRegistry(key.Item(0), key.Item(1), False) = False Then
                    Exit Sub
                End If
                regtweaks.SetBinary(key.Item(0), key.Item(1), regtweaks.translate(key.Item(2)), True)
                doneTasks += 1
            Next
            For Each key In regtweaks.SystemTweaks.Item("String")
                If key.Item(1) = "/ve" Then 'If for (Default) value...
                    If BackupRegistry(key.Item(0), "(Default)", False) = False Then 'Default Value needs alt. treatment due to how RegistryTweaks was designed
                        Exit Sub
                    End If
                    regtweaks.SetDefaultStr(key.Item(0), regtweaks.translate(key.Item(2)), True)
                Else
                    If BackupRegistry(key.Item(0), key.Item(1), False) = False Then
                        Exit Sub
                    End If
                    regtweaks.SetSZ(key.Item(0), key.Item(1), regtweaks.translate(key.Item(2)), True)
                End If
                doneTasks += 1
            Next
            For Each key In regtweaks.SystemTweaks.Item("MultiString")
                If BackupRegistry(key.Item(0), key.Item(1), False) = False Then
                    Exit Sub
                End If
                regtweaks.SetMultiSZ(key.Item(0), key.Item(1), regtweaks.translate(key.Item(2)), True)
                doneTasks += 1
            Next

            'Extra registry tweaks that are too tedious to add to RegistryTweaks directly:
            If System.Environment.OSVersion.Version.Minor = 3 Then 'Don't need 7TT in 8.0 'cos Show Desktop's the correct size in 8.0
                'Run 7+ Taskbar Tweaker on login
                If BackupRegistry("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Run", "7 Taskbar Tweaker", False) = False Then
                    Exit Sub
                End If
                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Run"" /v ""7 Taskbar Tweaker"" /t REG_SZ /d ""\""" + windrive + "Program Files\7+ Taskbar Tweaker\7+ Taskbar Tweaker.exe\"" -hidewnd"" /f", AppWinStyle.Hide, True)
            End If
            doneTasks += 1


            ' USER-WIDE
            '  Before we do anything, let's get the directories
            directoriesList = ListDirectory(storagelocation + "\UserFileReplacements")
            dirArr = New IO.DirectoryInfo(windrive + "Users").GetDirectories() 'Required for getting list of directories in directory to loop through

            ' ...and unload HKLM\UserConfig first if loaded
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg unload ""HKLM\UserConfig""", AppWinStyle.Hide, True)
            Thread.Sleep(400)

            '  Now loop the following between users:
            For Each loopdirinfo In dirArr
                If Not IO.File.Exists(loopdirinfo.FullName + "\NTUSER.DAT") Then
                    Continue For 'Skip folder if not a Windows user or Default User Skeleton
                End If
                If loopdirinfo.Name = "All Users" Or loopdirinfo.Name = "Default User" Then
                    Continue For 'Skip symlinks
                End If

                ' FILE AND FOLDER DELETIONS
                For Each item In cfgs.userItemsToDelete
                    If IO.Directory.Exists(loopdirinfo.FullName + "\" + item) Then
                        directoriesList = ListDirectory(loopdirinfo.FullName + "\" + item)
                        For Each item2 In directoriesList
                            fiArr = New IO.DirectoryInfo(item2).GetFiles() 'Add contents to file changes
                            For Each loopfileinfo In fiArr
                                If Not MultiSZUserFileChanges.Contains(loopfileinfo.FullName) Then
                                    MultiSZUserFileChanges.Add(loopfileinfo.FullName)
                                End If
                            Next
                        Next
                        'Move original folder to backups area
                        If MoveFolder(loopdirinfo.FullName + "\" + item, loopdirinfo.FullName + storagelocationuser + "\Backups\" + item, "user folder deletions backups", False) = False Then
                            Exit Sub 'It failed, and we can't just exit sub via the function, so this is done.
                        End If
                    ElseIf IO.File.Exists(loopdirinfo.FullName + "\" + item) Then
                        'Move original file to backups area
                        If MoveFile(loopdirinfo.FullName + "\" + item, loopdirinfo.FullName + storagelocationuser + "\Backups\" + item, "user file deletions backups", False) = False Then
                            Exit Sub 'It failed, and we can't just exit sub via the function, so this is done.
                        End If
                        If Not MultiSZUserFileChanges.Contains(windrive + item) Then
                            MultiSZUserFileChanges.Add(windrive + item)
                        End If
                    End If
                Next

                ' FILE AND FOLDER MOVEMENTS
                changedFiles = New List(Of String)
                For Each key In cfgs.userItemsToMove
                    If IO.Directory.Exists(loopdirinfo.FullName + "\" + key.Value.Item(0)) And Not IO.Directory.Exists(loopdirinfo.FullName + "\" + key.Value.Item(1)) Then
                        If Not MultiSZUserFolderChanges.Contains(key.Value.Item(1)) Then
                            MultiSZUserFolderChanges.Add(key.Value.Item(1))
                        End If
                    End If

                    changedFiles = MoveUnknown(loopdirinfo.FullName + "\" + key.Value.Item(0), loopdirinfo.FullName + "\" + key.Value.Item(1), "file and folder movements", True, loopdirinfo.FullName + "\", loopdirinfo.FullName + storagelocationuser + "\Backups\")
                    If changedFiles.Contains("FAILURE:") Then
                        Exit Sub 'It failed, and we can't just exit sub via the function, so this is done.
                    End If
                    For Each item In changedFiles
                        If Not MultiSZUserFileChanges.Contains(item.Replace(loopdirinfo.FullName + "\", "")) Then
                            MultiSZUserFileChanges.Add(item.Replace(loopdirinfo.FullName + "\", ""))
                        End If
                    Next
                    doneTasks += 1
                Next

                ' FILE REPLACEMENTS
                directoriesList = ListDirectory(storagelocation + "\UserFileReplacements")
                For Each item In directoriesList
                    fiArr = New IO.DirectoryInfo(item).GetFiles() 'Get the files in this directory
                    For Each loopfileinfo In fiArr
                        'Move original file to backups area
                        If MoveFile(loopfileinfo.FullName.Replace(storagelocation + "\UserFileReplacements\", loopdirinfo.FullName + "\"), _
                                    loopfileinfo.FullName.Replace(storagelocation + "\UserFileReplacements\", loopdirinfo.FullName + storagelocationuser + "\Backups\"), _
                                    "user file replacements backups", False) = False Then
                            Exit Sub
                        End If

                        'Move new file to its intended location
                        If MoveFile(loopfileinfo.FullName, loopfileinfo.FullName.Replace(storagelocation + "\UserFileReplacements\", loopdirinfo.FullName + "\"), _
                                    "user file replacements replacements", True) = False Then
                            Exit Sub
                        End If
                        doneTasks += 1
                        If Not MultiSZUserFileChanges.Contains(loopfileinfo.FullName.Replace(storagelocation + "\UserFileReplacements\", "")) Then
                            MultiSZUserFileChanges.Add(loopfileinfo.FullName.Replace(storagelocation + "\UserFileReplacements\", ""))
                        End If
                    Next
                Next

                ' REGISTRY CHANGES
                '  Load the NTUSER.DAT file to HKLM\UserConfig
                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg load ""HKLM\UserConfig"" """ + loopdirinfo.FullName + "\NTUSER.DAT""", AppWinStyle.Hide, True)

                '  Then tackle remaining deletions
                For Each key In regtweaks.UserTweaks.Item("Delete")
                    If BackupRegistry(key.Item(0).Replace("HKCU\", "HKLM\UserConfig\"), key.Item(1), False) = False Then
                        Exit Sub
                    End If
                    regtweaks.DeleteKey(key.Item(0).Replace("HKCU\", "HKLM\UserConfig\"), key.Item(1), True)
                    doneTasks += 1
                Next
                For Each key In regtweaks.UserTweaks.Item("DWORD")
                    If BackupRegistry(key.Item(0).Replace("HKCU\", "HKLM\UserConfig\"), key.Item(1), False) = False Then
                        Exit Sub
                    End If
                    regtweaks.SetDWORD(key.Item(0).Replace("HKCU\", "HKLM\UserConfig\"), key.Item(1), regtweaks.translate(key.Item(2)), True)
                    doneTasks += 1
                Next
                For Each key In regtweaks.UserTweaks.Item("Binary")
                    If BackupRegistry(key.Item(0).Replace("HKCU\", "HKLM\UserConfig\"), key.Item(1), False) = False Then
                        Exit Sub
                    End If
                    regtweaks.SetBinary(key.Item(0).Replace("HKCU\", "HKLM\UserConfig\"), key.Item(1), regtweaks.translate(key.Item(2)), True)
                    doneTasks += 1
                Next
                For Each key In regtweaks.UserTweaks.Item("String")
                    If key.Item(1) = "/ve" Then 'If for (Default) value...
                        If BackupRegistry(key.Item(0).Replace("HKCU\", "HKLM\UserConfig\"), "(Default)", False) = False Then
                            Exit Sub
                        End If
                        regtweaks.SetDefaultStr(key.Item(0).Replace("HKCU\", "HKLM\UserConfig\"), regtweaks.translate(key.Item(2)), True)
                    Else
                        If BackupRegistry(key.Item(0).Replace("HKCU\", "HKLM\UserConfig\"), key.Item(1), False) = False Then
                            Exit Sub
                        End If
                        regtweaks.SetSZ(key.Item(0).Replace("HKCU\", "HKLM\UserConfig\"), key.Item(1), regtweaks.translate(key.Item(2)), True)
                    End If
                    doneTasks += 1
                Next
                For Each key In regtweaks.UserTweaks.Item("MultiString")
                    If BackupRegistry(key.Item(0).Replace("HKCU\", "HKLM\UserConfig\"), key.Item(1), False) = False Then
                        Exit Sub
                    End If
                    regtweaks.SetMultiSZ(key.Item(0).Replace("HKCU\", "HKLM\UserConfig\"), key.Item(1), regtweaks.translate(key.Item(2)), True)
                    doneTasks += 1
                Next
                Thread.Sleep(400)

                '  Extra Registry changes
                '   Delete 7+ Taskbar Tweaker user-wide autostart (moved to system-wide autostart)
                regtweaks.DeleteKey("HKLM\UserConfig\Software\Microsoft\Windows\CurrentVersion\Run", "7 Taskbar Tweaker", True)

                '  CLEANUP
                '   Delete Icon Caches
                If IO.Directory.Exists(loopdirinfo.FullName + "\AppData\Local\Microsoft\Windows\Explorer") Then
                    fiArr = New IO.DirectoryInfo(loopdirinfo.FullName + "\AppData\Local\Microsoft\Windows\Explorer").GetFiles() 'Get the files in this directory
                    For Each loopfileinfo In fiArr
                        If loopfileinfo.Name.StartsWith("iconcache") Then
                            Try
                                IO.File.Delete(loopfileinfo.FullName)
                            Catch
                                Shell(windir + "\" + sysprefix + "\cmd.exe /c del """ + loopfileinfo.FullName + """ /s /q /f /a", AppWinStyle.Hide, True)
                            End Try
                        End If
                    Next
                End If
                doneTasks += 1

                '  Finally, unload HKLM\UserConfig once again
                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg unload ""HKLM\UserConfig""", AppWinStyle.Hide, True)
            Next

            'CLEANUP
            ' Delete VSCache for AEROs (aka move to backup)
            targetPath = "Windows\Resources\Themes\aero\VSCache"
            Try
                CreateDir(storagelocation + "\VSCaches")
            Catch ex As Exception
                ErrorOccurred("Failed to create directory to back up Visual Style Cache files to: " + ex.ToString())
                Exit Sub
            End Try
            fiArr = New IO.DirectoryInfo(windrive + targetPath).GetFiles() 'Get the files in this directory
            For Each loopfileinfo In fiArr
                Try
                    If Not IO.File.Exists(storagelocation + "\VSCaches\" + loopfileinfo.Name) Then
                        IO.File.Move(loopfileinfo.FullName, storagelocation + "\VSCaches\" + loopfileinfo.Name)
                    Else
                        IO.File.Delete(loopfileinfo.FullName) 'prioritise older VSCache backup, delete the newer one
                    End If
                Catch ex As Exception
                    ErrorOccurred("Failed to move " + loopfileinfo.FullName + " to the VSCache backups folder: " + ex.ToString())
                    Exit Sub
                End Try
            Next
            doneTasks += 1
            ' Delete lock screen background cache
            Shell(windir + "\" + sysprefix + "\cmd.exe /c del " + windrive + "ProgramData\Microsoft\Windows\SystemData\* /s /q /f", AppWinStyle.Hide, True)
            doneTasks += 1
            ' Delete Resource Hacker files, it had its use completely done
            Dim tries As Integer
            For Each fil In {storagelocation + "\SetupTools\ResourceHacker.exe", storagelocation + "\SetupTools\ResourceHacker.ini"}
                tries = 0
                While Not tries = 10
                    Shell(windir + "\" + sysprefix + "\cmd.exe /c del """ + fil + """ /s /q /f /a", AppWinStyle.Hide, True)
                    If Not IO.File.Exists(fil) Then
                        Exit While
                    End If
                    tries += 1
                End While
            Next
            ' Delete UserFileReplacements
            directoriesList = ListDirectory(storagelocation + "\UserFileReplacements")
            For Each item In directoriesList
                fiArr = New IO.DirectoryInfo(item).GetFiles()
                For Each loopfileinfo In fiArr
                    Shell(windir + "\" + sysprefix + "\cmd.exe /c del """ + loopfileinfo.FullName + """ /s /q /f /a", AppWinStyle.Hide, True)
                Next
            Next
            'Add installer to control
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Win8To7TransformationPack"" /v ""DisplayName"" /t REG_SZ /d ""Win8To7 Transformation Pack Backend"" /f", AppWinStyle.Hide, True)
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Win8To7TransformationPack"" /v ""DisplayVersion"" /t REG_SZ /d ""3.1"" /f", AppWinStyle.Hide, True)
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Win8To7TransformationPack"" /v ""DisplayIcon"" /t REG_SZ /d """ + storagelocation + "\SetupTools\setup.exe"" /f", AppWinStyle.Hide, True)
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Win8To7TransformationPack"" /v ""InstallLocation"" /t REG_SZ /d """ + storagelocation + "\SetupTools""\ /f", AppWinStyle.Hide, True)
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Win8To7TransformationPack"" /v ""UninstallString"" /t REG_SZ /d """ + storagelocation + "\SetupTools\setup.exe"" /f", AppWinStyle.Hide, True)
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Win8To7TransformationPack"" /v ""NoModify"" /t REG_DWORD /d 1 /f", AppWinStyle.Hide, True)
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Win8To7TransformationPack"" /v ""NoRepair"" /t REG_SZ /d 1 /f", AppWinStyle.Hide, True)
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Win8To7TransformationPack"" /v ""VersionMajor"" /t REG_SZ /d 3 /f", AppWinStyle.Hide, True)
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Win8To7TransformationPack"" /v ""VersionMinor"" /t REG_SZ /d 1 /f", AppWinStyle.Hide, True)
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Win8To7TransformationPack"" /v ""Publisher"" /t REG_SZ /d ""ImSwordKing and co."" /f", AppWinStyle.Hide, True)
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Win8To7TransformationPack"" /v ""Comments"" /t REG_SZ /d ""Uninstall Win8To7 Transformation Pack Backend, or revert Windows to how it was before transforming"" /f", AppWinStyle.Hide, True)
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Win8To7TransformationPack"" /v ""URLInfoAbout"" /t REG_SZ /d ""https://forum.eclectic4un.me/viewtopic.php?f=29&t=107"" /f", AppWinStyle.Hide, True)

            'End the progress thread, and change the progress to 100
            jobthread.Abort()
            ChangeProgress(100)

            'Go back into Windows now it's all complete.
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v CmdLine /t REG_SZ /f", AppWinStyle.Hide, True)
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v OOBEInProgress /t REG_DWORD /d 0 /f", AppWinStyle.Hide, True)
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SetupPhase /t REG_DWORD /d 0 /f", AppWinStyle.Hide, True)
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SetupType /t REG_DWORD /d 0 /f", AppWinStyle.Hide, True)
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SystemSetupInProgress /t REG_DWORD /d 0 /f", AppWinStyle.Hide, True)
            'Mark install as Done ('Phase 10')
            HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("CurrentPhase", 10)
            'Delete the HighContrast value, so it doesn't get reused in future installs/uninstalls while HC is off
            HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).DeleteValue("HighContrast", False)
            'Save our file changes lists to Registry
            HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("FilesChanged", MultiSZFileChanges.ToArray, Microsoft.Win32.RegistryValueKind.MultiString)
            HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("UserFilesChanged", MultiSZUserFileChanges.ToArray, Microsoft.Win32.RegistryValueKind.MultiString)
            HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("FoldersChanged", MultiSZFolderChanges.ToArray, Microsoft.Win32.RegistryValueKind.MultiString)
            HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("UserFoldersChanged", MultiSZUserFolderChanges.ToArray, Microsoft.Win32.RegistryValueKind.MultiString)

            AboutToRestart("")
            RestartTime("") 'End this Setup program, to finish things off
        Catch ex As Exception
            ErrorOccurred(ex.ToString())
            Exit Sub
        End Try
    End Sub
#End Region

End Class
