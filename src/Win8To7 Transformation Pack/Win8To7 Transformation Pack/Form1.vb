Public Class Form1
#Region "Variables"
    Private windrive = IO.Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.System))
    Private windir As String = System.Environment.GetEnvironmentVariable("WINDIR")
    Private storagelocation = windir + "\Win8To7"
    Private sysdir As String
    Public HKLMKey32 As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.LocalMachine
    Public isStarter As Boolean = Microsoft.Win32.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "EditionID", "Professional").ToString.StartsWith("Starter")
    Public isHome As Boolean = Microsoft.Win32.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "EditionID", "Professional").ToString.StartsWith("Core")
    Public isEnterprise As Boolean = Microsoft.Win32.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "EditionID", "Professional").ToString.StartsWith("Enterprise")

    Public isInstalled As Boolean = False 'has already been installed?
    Public firefoxInstalled As Boolean = IO.File.Exists(windrive + "Program Files\Mozilla Firefox\firefox.exe") Or IO.File.Exists(windrive + "Program Files\LibreWolf\librewolf.exe") Or IO.File.Exists(windrive + "Program Files\Waterfox\waterfox.exe")
#End Region
#Region "HC compatibility"
    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        Const WM_THEMECHANGED = &H31A

        If (m.Msg = WM_THEMECHANGED) Then
            RefreshHC()
            Me.Refresh()
        End If

        MyBase.WndProc(m)
    End Sub

    Public Sub RefreshHC()
        If SystemInformation.HighContrast = True Then
            Me.BackColor = Control.DefaultBackColor
            Me.BackgroundImage = Nothing
            Me.ForeColor = Control.DefaultForeColor
            Next1.FlatStyle = FlatStyle.System
            LinkLabel1.ForeColor = Control.DefaultForeColor
            LinkLabel1.LinkColor = Control.DefaultForeColor
            LinkLabel1.ActiveLinkColor = Control.DefaultForeColor
            LinkLabel1.VisitedLinkColor = Control.DefaultForeColor
            LinkLabel2.ForeColor = Control.DefaultForeColor
            LinkLabel2.LinkColor = Control.DefaultForeColor
            LinkLabel2.ActiveLinkColor = Control.DefaultForeColor
            LinkLabel2.VisitedLinkColor = Control.DefaultForeColor
        Else
            Me.BackColor = Color.Black
            Me.BackgroundImage = My.Resources.int_SetupBG
            Me.ForeColor = Color.White
            Next1.FlatStyle = FlatStyle.Flat
            LinkLabel1.ForeColor = Color.White
            LinkLabel1.LinkColor = Color.White
            LinkLabel1.ActiveLinkColor = Color.White
            LinkLabel1.VisitedLinkColor = Color.White
            LinkLabel2.ForeColor = Color.White
            LinkLabel2.LinkColor = Color.White
            LinkLabel2.ActiveLinkColor = Color.White
            LinkLabel2.VisitedLinkColor = Color.White
        End If
    End Sub
#End Region

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Check System32 directory
        If System.IO.File.Exists(windir + "\SysNative\LogonUI.exe") Then
            sysdir = "SysNative"
        Else
            sysdir = "System32"
        End If
    End Sub

    Private Sub Form1_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If PageTransform.Visible = True Then
            MsgBox("Installation of Win8To7 Transformation Pack Backend cannot be cancelled, as cancelling the transformation can risk rendering Windows unusable.", MsgBoxStyle.Exclamation, "Win8To7 Transformation Pack Backend")
            e.Cancel = True
        End If
    End Sub

#Region "Label status changes"
    Private Sub AboutToRestart(ByVal status As String)
        'This bit here makes it work despite being called via a VB.NET Thread
        If Me.InvokeRequired Then
            Dim args() As String = {status}
            Me.Invoke(New Action(Of String)(AddressOf AboutToRestart), args)
            Exit Sub
        End If

        ProgressBar1.Style = ProgressBarStyle.Marquee
        LabelStatus.Text = "Windows will restart in a few moments to continue the transformation process..."
    End Sub

    Private Sub RestartTime(ByVal status As String)
        'This bit here makes it work despite being called via a VB.NET Thread
        If Me.InvokeRequired Then
            Dim args() As String = {status}
            Me.Invoke(New Action(Of String)(AddressOf RestartTime), args)
            Exit Sub
        End If

        PageTransform.Visible = False
        End
    End Sub

    Private Sub ChangeProgress(ByVal progress As Integer)
        'This bit here makes it work despite being called via a VB.NET Thread
        If Me.InvokeRequired Then
            Dim args() As String = {progress}
            Me.Invoke(New Action(Of String)(AddressOf ChangeProgress), args)
            Exit Sub
        End If

        ProgressBar1.Value = progress
    End Sub

    Private Sub ChangeProgressStyle(ByVal style As ProgressBarStyle)
        'This bit here makes it work despite being called via a VB.NET Thread
        If Me.InvokeRequired Then
            Dim args() As String = {style}
            Me.Invoke(New Action(Of String)(AddressOf ChangeProgressStyle), args)
            Return
        End If

        ProgressBar1.Style = style
    End Sub

    Private Sub ChangeStatus(ByVal status As String)
        'This bit here makes it work despite being called via a VB.NET Thread
        If Me.InvokeRequired Then
            Dim args() As String = {status}
            Me.Invoke(New Action(Of String)(AddressOf ChangeStatus), args)
            Exit Sub
        End If

        LabelStatus.Text = "Status: " + status
    End Sub
#End Region

#Region "First page - new installs"
    Private Sub Next1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Next1.Click
        BackArea.Visible = True
        BackButton.Enabled = True
        Page2.Visible = True
        Page1.Visible = False
    End Sub
    Private Sub Next1_Hover(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Next1.MouseEnter, Next1.MouseUp
        Next1.BackgroundImage = My.Resources.int_ButtonHover
        Next1.Image = My.Resources.int_ForwardHover
    End Sub
    Private Sub Next1_MouseLeave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Next1.MouseLeave
        Next1.BackgroundImage = Nothing
        Next1.Image = My.Resources.int_ForwardNormal
    End Sub
    Private Sub Next1_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Next1.MouseDown
        Next1.BackgroundImage = My.Resources.int_ButtonPressed
        Next1.Image = My.Resources.int_ForwardPressed
    End Sub

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Credits.Show()
        Credits.Focus()
    End Sub
#End Region
#Region "First page - update mode"
    Private Sub ManageOptionConfigure_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles ManageOptionConfigure.CheckedChanged
        If ManageOptionConfigure.Checked = True Then
            NextManage.Text = "Next"
        Else
            NextManage.Text = "Launch Uninstaller"
        End If
    End Sub

    Private Sub NextManage_Click(sender As System.Object, e As System.EventArgs) Handles NextManage.Click
        If ManageOptionConfigure.Checked = True Then
            Page3.Visible = True
            PageOptions.Visible = False
            BackButton.Enabled = True
        Else
            Shell(storagelocation + "\SetupTools\setup.exe", AppWinStyle.NormalFocus, False) 'Go over to uninstaller
            End
        End If
    End Sub
#End Region
#Region "Second page - Ts&Cs"
    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        Next2.Enabled = CheckBox1.Checked
    End Sub

    Private Sub Next2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Next2.Click
        Page3.Visible = True
        Page2.Visible = False
        BackButton.Enabled = True
    End Sub
#End Region
#Region "Third page"
    Private Sub Next3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Next3.Click
        Page4.Visible = True
        Page3.Visible = False
    End Sub
#End Region
#Region "Fourth page"
    Private Sub Next4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Next4.Click
        Page5.Visible = True
        Page4.Visible = False
    End Sub
#End Region
#Region "Fifth page"
    Private Sub Next5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Next5.Click
        'Branding
        If Branding8Pro.Checked = True Then
            OverviewBranding.Text = "Branding: Windows 8 Professional"
        ElseIf Branding7Pro.Checked = True Then
            OverviewBranding.Text = "Branding: Windows 7 Professional"
        ElseIf Branding7Ult.Checked = True Then
            OverviewBranding.Text = "Branding: Windows 7 Ultimate"
        End If
        'Ribbon
        If RibbonWin7.Checked = True Then
            OverviewRibbon.Text = "Ribbon: Windows 7's Ribbon"
        ElseIf RibbonWin8.Checked = True Then
            OverviewRibbon.Text = "Ribbon: Windows 8 DP/ConP's Ribbon"
        End If
        'Start Menu
        If SMEx7ForWin8.Checked = True Then
            OverviewSM.Text = "Start Menu: Ex7ForWin8"
        ElseIf SMStartIsBack.Checked = True Then
            OverviewSM.Text = "Start Menu: StartIsBack"
        ElseIf SMOpenShell.Checked = True Then
            OverviewSM.Text = "Start Menu: Open Shell"
        End If
        'Glass Style
        If ThemeUseShine.Checked = True Then
            OverviewShine.Text = "Glass Style: Shiny"
        ElseIf ThemeNoUseShine.Checked = True Then
            OverviewShine.Text = "Glass Style: Flat"
        End If
        'Window Colour and Appearance
        If KeepAutomaticColor.Checked = True And KeepChocolate.Checked = True Then
            OverviewSwatches.Text = "Window Colors: Automatic and Chocolate kept (will add a third row)"
        ElseIf KeepAutomaticColor.Checked = True Then
            OverviewSwatches.Text = "Window Colors: Automatic kept and Chocolate removed"
        ElseIf KeepChocolate.Checked = True Then
            OverviewSwatches.Text = "Window Colors: Chocolate kept and Automatic removed"
        End If

        PageConfirm.Visible = True
        Page5.Visible = False
    End Sub
#End Region
#Region "Confirmation page"
    Private Sub NextConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NextConfirm.Click
        If firefoxInstalled = True Then 'Warn about the limitations
            MsgBox("We have detected an installation of Firefox or a Firefox-based browser on your computer. Unfortunately, Mozilla decided to break Firefox in Windows 7, and subsequently in this transformation pack, when introducing the 'Proton' redesign." + Environment.NewLine + Environment.NewLine + "Transformation will continue, but will lack key changes like the Windows 7 titlebar button widths to prevent issues from occurring." + Environment.NewLine + "Press OK to begin the transformation.", MsgBoxStyle.Exclamation + MsgBoxStyle.ApplicationModal, "Win8To7 Transformation Pack Backend")
        End If

        BackArea.Visible = False
        PageTransform.Visible = True
        PageConfirm.Visible = False

        Dim jobthread As New Thread(AddressOf DoTheJob)
        jobthread.Start()
    End Sub

    Private Sub CheckBox5_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles AllowUXThemePatcher.CheckedChanged
        If AllowUXThemePatcher.Checked = False Then
            If MsgBox("Not patching your system with UXThemePatcher SHOULD ONLY BE DONE if you have already patched your system to be able to use custom themes already. If not patched, the transformation will brick your current Windows installation. Are you SURE you want to continue without patching your system with UXThemePatcher?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2, "Win8To7 Transformation Pack Backend") = MsgBoxResult.No Then
                AllowUXThemePatcher.Checked = True
            End If
        End If
    End Sub
#End Region

#Region "Important calls"
    Private Sub ErrorOccurred(ByVal status As String)
        'This bit here makes it work despite being called via a VB.NET Thread
        If Me.InvokeRequired Then
            Dim args() As String = {status}
            Me.Invoke(New Action(Of String)(AddressOf ErrorOccurred), args)
            Exit Sub
        End If

        If status = "" Then
            MsgBox("Something happened", "Something happened")
        Else
            MsgBox(status, MsgBoxStyle.OkOnly, "Something happened")
        End If
        End
    End Sub
#End Region

#Region "Back Button"
    Private Sub BackButton_EnabledChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BackButton.EnabledChanged
        If BackButton.Enabled = True Then
            BackButton.Image = My.Resources.int_BackNormal
        Else
            BackButton.Image = My.Resources.int_BackDisabled
        End If
    End Sub
    Private Sub BackButton_Hover(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BackButton.MouseEnter, BackButton.MouseUp
        If BackButton.Enabled = True Then
            BackButton.Image = My.Resources.int_BackHover
        End If
    End Sub
    Private Sub BackButton_MouseLeave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BackButton.MouseLeave
        If BackButton.Enabled = True Then
            BackButton.Image = My.Resources.int_BackNormal
        End If
    End Sub
    Private Sub BackButton_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles BackButton.MouseDown
        If BackButton.Enabled = True Then
            BackButton.Image = My.Resources.int_BackPressed
        End If
    End Sub

    Private Sub BackButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BackButton.Click
        'Page 2 -> 1
        If Page2.Visible = True Then
            Page1.Visible = True
            Page2.Visible = False
            BackArea.Visible = False
        End If
        'Page 3 -> 2
        If Page3.Visible = True Then
            If isInstalled = True Then
                PageOptions.Visible = True
                BackButton.Enabled = False
            Else
                Page2.Visible = True
            End If
            Page3.Visible = False
        End If
        'Page 4 -> 3
        If Page4.Visible = True Then
            Page3.Visible = True
            Page4.Visible = False
        End If
        'Page 5 -> 4
        If Page5.Visible = True Then
            Page4.Visible = True
            Page5.Visible = False
        End If
        'Page Confirm -> 5
        If PageConfirm.Visible = True Then
            Page5.Visible = True
            PageConfirm.Visible = False
        End If
    End Sub
#End Region
#Region "Credits button"
    Private Sub LinkLabel2_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        Credits.Show()
    End Sub
#End Region

#Region "Transformation begins"
    Sub DoTheJob()
        'Get sysprefix value first
        Dim sysprefix As String
        If System.IO.File.Exists(windir + "\SysNative\LogonUI.exe") Then
            sysprefix = windir + "\SysNative"
        Else
            sysprefix = windir + "\System32"
        End If

        'Create a Restore Point if this is the first installation
        If isInstalled = False Then
            ChangeStatus("Creating a restore point...")
            ChangeProgress(1)
            ChangeProgressStyle(ProgressBarStyle.Marquee)
            Shell(sysprefix + "\cmd.exe /c reg add ""HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore"" /v ""SystemRestorePointCreationFrequency"" /t REG_DWORD /d 0 /f", AppWinStyle.Hide, True)
            Shell(sysprefix + "\cmd.exe /c wmic.exe /Namespace:\\root\default Path SystemRestore Call CreateRestorePoint ""Before installing Win8To7 Transformation Pack Backend"", 100, 12", AppWinStyle.Hide, True)
            Shell(sysprefix + "\cmd.exe /c reg del ""HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore"" /v ""SystemRestorePointCreationFrequency"" /f", AppWinStyle.Hide, True)
            ChangeProgressStyle(ProgressBarStyle.Continuous)

            '...and create the pack's directory
            ChangeStatus("Creating directory for transformation pack files...")
            ChangeProgress(2)
            'First, make the location a thing
            If IO.Directory.Exists(storagelocation) Then
                Try
                    IO.Directory.Delete(storagelocation, True)
                Catch ex As Exception
                    ErrorOccurred("An error occurred clearing room for the transformation pack directory.")
                    Exit Sub
                End Try
            End If
            If Not IO.Directory.Exists(storagelocation) Then
                Try
                    IO.Directory.CreateDirectory(storagelocation)
                Catch ex As Exception
                    ErrorOccurred(ex.Message)
                    Exit Sub
                End Try
            End If
        End If

        'Now extract the files
        ChangeStatus("Extracting files ready for patching...")
        ChangeProgress(3)

        If isInstalled = True Then
            Try 'Move the old transformation pack's version of Setup Mode Phases to a temporary spot
                IO.File.Move(storagelocation + "\SetupTools\setup.exe", storagelocation + "\setupold.exe")
            Catch ex As Exception
                ErrorOccurred("Couldn't move the old version of Setup for usage in Stage 2 of customisation")
                Exit Sub
            End Try
        End If

        If isInstalled = True And IO.File.Exists(storagelocation + "\SetupFiles\regchange.exe") Then '3.0 update-path support 1/2
            Try 'Move the old transformation pack's version of Registry Changes Executable to a temporary spot
                IO.File.Move(storagelocation + "\SetupFiles\regchange.exe", storagelocation + "\regchangeold.exe")
            Catch ex As Exception
                ErrorOccurred("Couldn't move the old version of Registry Changes Executable for usage in Stage 2 of customisation")
                Exit Sub
            End Try
        End If

        Dim tries As Integer 'Delete and recreate directories
        For Each direc In {storagelocation + "\FileReplacements", storagelocation + "\UserFileReplacements", storagelocation + "\ResFiles", storagelocation + "\SetupFiles", windir + "\Temp\Win8To7DiscardedFiles"}
            tries = 0
            While Not tries = 10
                Shell(sysprefix + "\cmd.exe /c del """ + direc + """ /s /q /f /a", AppWinStyle.Hide, True)
                Shell(sysprefix + "\cmd.exe /c rd """ + direc + """ /s /q", AppWinStyle.Hide, True)
                If Not IO.Directory.Exists(direc) Then
                    Exit While
                End If
                tries += 1
            End While
        Next
        For Each direc In {storagelocation + "\Backups", storagelocation + "\FileReplacements", storagelocation + "\UserFileReplacements", storagelocation + "\ResFiles", storagelocation + "\SetupTools", _
                           windir + "\Temp\Win8To7DiscardedFiles", storagelocation + "\SetupFiles"}
            If Not IO.Directory.Exists(direc) Then
                Try
                    IO.Directory.CreateDirectory(direc)
                Catch ex As Exception
                    ErrorOccurred(ex.Message)
                    Exit Sub
                End Try
            End If
        Next

        If isInstalled = True And IO.File.Exists(storagelocation + "\regchangeold.exe") Then '3.0 update-path support 2/2
            Try 'Move the old transformation pack's version of Registry Changes Executable to a temporary spot
                IO.File.Move(storagelocation + "\regchangeold.exe", storagelocation + "\SetupFiles\regchange.exe")
            Catch ex As Exception
                ErrorOccurred("Couldn't move the old version of Registry Changes Executable back to original placement for usage in Stage 2 of customisation")
                Exit Sub
            End Try
        End If

        Dim targetPath As String
        Dim tempArray As String()

        'Extract files from resources
        Dim startInfo As New System.Diagnostics.ProcessStartInfo
        Dim MyProcess As Process

        'First run: Only setupfiles and setuptools
        For Each dictEntry In My.Resources.ResourceManager.GetResourceSet(System.Globalization.CultureInfo.CurrentCulture, True, True)
            If dictEntry.Key.StartsWith("int_") Then 'Skip internal resources
                Continue For
            End If
            If dictEntry.Key.StartsWith("i386_") And Environment.Is64BitOperatingSystem = True Then 'Skip incompatible architectures - 64-Bit
                Continue For
            End If
            If dictEntry.Key.StartsWith("amd64_") And Environment.Is64BitOperatingSystem = False Then 'Skip incompatible architectures - 32-Bit
                Continue For
            End If

            ' Strip architecture identifier from id for next steps
            tempArray = dictEntry.Key.Split("_")
            tempArray = tempArray.Skip(1).ToArray()
            targetPath = String.Join("_", tempArray.ToArray)

            If targetPath.StartsWith("9200_") And System.Environment.OSVersion.Version.Minor = 3 Then 'Skip 8.1-incompatible files
                Continue For
            End If
            If targetPath.StartsWith("9600_") And System.Environment.OSVersion.Version.Minor = 2 Then 'Skip 8.0-incompatible files
                Continue For
            End If

            ' Strip build identifier from id for next steps
            tempArray = targetPath.Split("_")
            tempArray = tempArray.Skip(1).ToArray()
            targetPath = String.Join("_", tempArray.ToArray)


            If targetPath.StartsWith("NotStarter_") And isStarter = True Then 'Skip files not for Starter if Starter is in use
                Continue For
            End If
            If targetPath.StartsWith("NotPro_") And isStarter = False And isHome = False And isEnterprise = False Then 'Skip files not for Pro if Pro is in use
                Continue For
            End If
            If targetPath.StartsWith("NotEnterprise_") And isEnterprise = True Then 'Skip files not for Enterprise if Enterprise is in use
                Continue For
            End If
            If targetPath.StartsWith("Starter_") And isStarter = False Then 'Skip files only for Starter if Starter is not in use
                Continue For
            End If
            If targetPath.StartsWith("Core_") And isHome = False Then 'Skip files only for Core/Home if it's is not in use
                Continue For
            End If
            If targetPath.StartsWith("Pro_") And (isStarter = True Or isHome = True Or isEnterprise = True) Then 'Skip files only for Pro if Pro is not in use
                Continue For
            End If
            If targetPath.StartsWith("Enterprise_") And isEnterprise = False Then 'Skip files only for Enterprise if Enterprise is not in use
                Continue For
            End If

            ' Strip SKU identifier from id for next steps
            tempArray = targetPath.Split("_")
            tempArray = tempArray.Skip(1).ToArray()
            targetPath = String.Join("_", tempArray.ToArray)


            ' Now skip depending on Branding styling
            If targetPath.StartsWith("Branding7Pro_") And (Branding7Pro.Checked = False Or isStarter = True Or isHome = True Or isEnterprise = True) Then 'Skip 7 Professional Branding if not chosen or isn't Pro
                Continue For
            End If
            If targetPath.StartsWith("Branding7Starter_") And (Branding7Pro.Checked = False Or Not isStarter = True) Then 'Skip 7 Starter Branding if not chosen or isn't Starter
                Continue For
            End If
            If targetPath.StartsWith("Branding7Home_") And (Branding7Pro.Checked = False Or Not isHome = True) Then 'Skip 7 Home Branding if not chosen or isn't Home
                Continue For
            End If
            If targetPath.StartsWith("Branding7Enterprise_") And (Branding7Pro.Checked = False Or Not isEnterprise = True) Then 'Skip 7 Enterprise Branding if not chosen or isn't Enterprise
                Continue For
            End If
            If targetPath.StartsWith("Branding7Ult_") And (Branding7Ult.Checked = False) Then 'Skip 7 Ultimate Branding if not chosen
                Continue For
            End If
            If targetPath.StartsWith("Branding8Pro_") And (Branding8Pro.Checked = False Or isStarter = True Or isHome = True) Then 'Skip 8 Professional Branding if not chosen or isn't Pro
                Continue For
            End If
            If targetPath.StartsWith("Branding8Starter_") And (Branding8Pro.Checked = False Or Not isStarter = True) Then 'Skip 8 Starter Branding if not chosen or isn't Starter
                Continue For
            End If
            If targetPath.StartsWith("Branding8Home_") And (Branding8Pro.Checked = False Or Not isHome = True) Then 'Skip 8 Home Branding if not chosen or isn't Home
                Continue For
            End If
            If targetPath.StartsWith("Branding8Enterprise_") And (Branding8Pro.Checked = False Or Not isEnterprise = True) Then 'Skip 8 Enterprise Branding if not chosen or isn't Enterprise
                Continue For
            End If
            ' Now skip depending on Ribbon styling
            If targetPath.StartsWith("DPRibbon_") And RibbonWin8.Checked = False Then 'Skip DP Ribbon if not chosen
                Continue For
            End If
            If targetPath.StartsWith("7Ribbon_") And RibbonWin7.Checked = False Then 'Skip 7 Ribbon if not chosen
                Continue For
            End If
            ' Now skip depending on Start Menu
            If targetPath.StartsWith("SMEx7ForWin8_") And SMEx7ForWin8.Checked = False Then 'Skip Ex7ForWin8 files if not chosen
                Continue For
            End If
            If targetPath.StartsWith("SMStartIsBack_") And SMStartIsBack.Checked = False Then 'Skip StartIsBack files if not chosen
                Continue For
            End If
            If targetPath.StartsWith("SMOpenShell_") And SMOpenShell.Checked = False Then 'Skip Open Shell if not chosen
                Continue For
            End If
            ' Now skip depending on Glass styling
            If targetPath.StartsWith("ShinelessTheme_") And ThemeNoUseShine.Checked = False Then 'Skip Shineless Theme if not chosen
                Continue For
            End If
            If targetPath.StartsWith("ShineTheme_") And ThemeUseShine.Checked = False Then 'Skip Shiny Theme if not chosen
                Continue For
            End If
            ' Now skip depending on shell32 preference
            If targetPath.StartsWith("AllowShell32_") And AllowShell32.Checked = False Then 'Skip shell32 replacement if not chosen
                Continue For
            End If
            ' Now skip depending on usercpl preference
            If targetPath.StartsWith("AllowUserCPL_") And AllowUserCPL.Checked = False Then 'Skip usercpl replacement if not chosen
                Continue For
            End If
            ' Now skip depending on remaining preferences
            If targetPath.StartsWith("AllowUXThemePatcher_") And AllowUXThemePatcher.Checked = False Then 'Skip UXThemePatcher stuff if not chosen
                Continue For
            End If
            If targetPath.StartsWith("Allow7Games_") And Allow7Games.Checked = False Then 'Skip Win7's Games if not chosen
                Continue For
            End If
            If targetPath.StartsWith("Allow7Gadgets_") And Allow7Gadgets.Checked = False Then 'Skip Win7's Gadgets if not chosen
                Continue For
            End If
            If targetPath.StartsWith("AllowMediaCenter_") And AllowMediaCenter.Checked = False And Not IO.Directory.Exists(windir + "\ehome") Then 'Skip Media Center if not chosen, and if not on 8.x +MediaCenter SKU
                Continue For
            End If
            If targetPath.StartsWith("Allow7TaskManager_") And Allow7TaskManager.Checked = False Then 'Skip Task Manager if not chosen
                Continue For
            End If
            If targetPath.StartsWith("ReduceWinX_") And ReduceWinX.Checked = False Then 'Skip alt. Win+X if not chosen
                Continue For
            End If
            If targetPath.StartsWith("NotReduceWinX_") And ReduceWinX.Checked = True Then 'Skip not alt. Win+X if chosen
                Continue For
            End If
            ' Now skip depending on...
            If targetPath.StartsWith("Foxless_") And firefoxInstalled = True Then 'Skip 'foxless' if Firefox or Firefox-based browsers were detected
                Continue For
            End If

            ' Strip setting identifier from id for next steps
            tempArray = targetPath.Split("_")
            tempArray = tempArray.Skip(1).ToArray()
            targetPath = String.Join("_", tempArray.ToArray)


            'This is where you'd skip depending on Aesthetics not being selected, but no other aesthetic options are pre-coded in this, so... this part is empty.

            ' Strip aesthetic identifier from id for next steps
            tempArray = targetPath.Split("_")
            tempArray = tempArray.Skip(1).ToArray()
            targetPath = String.Join("_", tempArray.ToArray)


            'First, deal with Setup files
            If targetPath.StartsWith("setupfile:") Then
                'Create directory for extraction
                targetPath = targetPath.Replace("setupfile:", "")

                'Now copy the new file into the temporary path
                If Not IO.File.Exists(storagelocation + "\SetupFiles\" + targetPath) Then
                    If WriteFileFromResources(dictEntry.Key.ToString(), storagelocation + "\SetupFiles\" + targetPath) = False Then
                        ErrorOccurred("Failed to extract files")
                        Exit Sub
                    End If
                End If
            End If

            'Second, deal with Setup tools
            If targetPath.StartsWith("setuptool:") Then
                'Create directory for extraction
                targetPath = targetPath.Replace("setuptool:", "")

                'Now copy the new file into the temporary path
                If Not IO.File.Exists(storagelocation + "\SetupTools\" + targetPath) Then
                    If WriteFileFromResources(dictEntry.Key.ToString(), storagelocation + "\SetupTools\" + targetPath) = False Then
                        ErrorOccurred("Failed to extract files")
                        Exit Sub
                    End If
                End If
            End If
        Next

        'Second run: Remaining files
        For Each dictEntry In My.Resources.ResourceManager.GetResourceSet(System.Globalization.CultureInfo.CurrentCulture, True, True)
            If dictEntry.Key.StartsWith("int_") Then 'Skip internal resources
                Continue For
            End If
            If dictEntry.Key.StartsWith("i386_") And Environment.Is64BitOperatingSystem = True Then 'Skip incompatible architectures - 64-Bit
                Continue For
            End If
            If dictEntry.Key.StartsWith("amd64_") And Environment.Is64BitOperatingSystem = False Then 'Skip incompatible architectures - 32-Bit
                Continue For
            End If

            ' Strip architecture identifier from id for next steps
            tempArray = dictEntry.Key.Split("_")
            tempArray = tempArray.Skip(1).ToArray()
            targetPath = String.Join("_", tempArray.ToArray)

            If targetPath.StartsWith("9200_") And System.Environment.OSVersion.Version.Minor = 3 Then 'Skip 8.1-incompatible files
                Continue For
            End If
            If targetPath.StartsWith("9600_") And System.Environment.OSVersion.Version.Minor = 2 Then 'Skip 8.0-incompatible files
                Continue For
            End If

            ' Strip build identifier from id for next steps
            tempArray = targetPath.Split("_")
            tempArray = tempArray.Skip(1).ToArray()
            targetPath = String.Join("_", tempArray.ToArray)


            If targetPath.StartsWith("NotStarter_") And isStarter = True Then 'Skip files not for Starter if Starter is in use
                Continue For
            End If
            If targetPath.StartsWith("NotPro_") And isStarter = False And isHome = False And isEnterprise = False Then 'Skip files not for Pro if Pro is in use
                Continue For
            End If
            If targetPath.StartsWith("NotEnterprise_") And isEnterprise = True Then 'Skip files not for Enterprise if Enterprise is in use
                Continue For
            End If
            If targetPath.StartsWith("Starter_") And isStarter = False Then 'Skip files only for Starter if Starter is not in use
                Continue For
            End If
            If targetPath.StartsWith("Core_") And isHome = False Then 'Skip files only for Core/Home if it's is not in use
                Continue For
            End If
            If targetPath.StartsWith("Pro_") And (isStarter = True Or isHome = True Or isEnterprise = True) Then 'Skip files only for Pro if Pro is not in use
                Continue For
            End If
            If targetPath.StartsWith("Enterprise_") And isEnterprise = False Then 'Skip files only for Enterprise if Enterprise is not in use
                Continue For
            End If

            ' Strip SKU identifier from id for next steps
            tempArray = targetPath.Split("_")
            tempArray = tempArray.Skip(1).ToArray()
            targetPath = String.Join("_", tempArray.ToArray)


            ' Now skip depending on Branding styling
            If targetPath.StartsWith("Branding7Pro_") And (Branding7Pro.Checked = False Or isStarter = True Or isHome = True Or isEnterprise = True) Then 'Skip 7 Professional Branding if not chosen or isn't Pro
                Continue For
            End If
            If targetPath.StartsWith("Branding7Starter_") And (Branding7Pro.Checked = False Or Not isStarter = True) Then 'Skip 7 Starter Branding if not chosen or isn't Starter
                Continue For
            End If
            If targetPath.StartsWith("Branding7Home_") And (Branding7Pro.Checked = False Or Not isHome = True) Then 'Skip 7 Home Branding if not chosen or isn't Home
                Continue For
            End If
            If targetPath.StartsWith("Branding7Enterprise_") And (Branding7Pro.Checked = False Or Not isEnterprise = True) Then 'Skip 7 Enterprise Branding if not chosen or isn't Enterprise
                Continue For
            End If
            If targetPath.StartsWith("Branding7Ult_") And (Branding7Ult.Checked = False) Then 'Skip 7 Ultimate Branding if not chosen
                Continue For
            End If
            If targetPath.StartsWith("Branding8Pro_") And (Branding8Pro.Checked = False Or isStarter = True Or isHome = True) Then 'Skip 8 Professional Branding if not chosen or isn't Pro
                Continue For
            End If
            If targetPath.StartsWith("Branding8Starter_") And (Branding8Pro.Checked = False Or Not isStarter = True) Then 'Skip 8 Starter Branding if not chosen or isn't Starter
                Continue For
            End If
            If targetPath.StartsWith("Branding8Home_") And (Branding8Pro.Checked = False Or Not isHome = True) Then 'Skip 8 Home Branding if not chosen or isn't Home
                Continue For
            End If
            If targetPath.StartsWith("Branding8Enterprise_") And (Branding8Pro.Checked = False Or Not isEnterprise = True) Then 'Skip 8 Enterprise Branding if not chosen or isn't Enterprise
                Continue For
            End If
            ' Now skip depending on Ribbon styling
            If targetPath.StartsWith("DPRibbon_") And RibbonWin8.Checked = False Then 'Skip DP Ribbon if not chosen
                Continue For
            End If
            If targetPath.StartsWith("7Ribbon_") And RibbonWin7.Checked = False Then 'Skip 7 Ribbon if not chosen
                Continue For
            End If
            ' Now skip depending on Start Menu
            If targetPath.StartsWith("SMEx7ForWin8_") And SMEx7ForWin8.Checked = False Then 'Skip Ex7ForWin8 files if not chosen
                Continue For
            End If
            If targetPath.StartsWith("SMStartIsBack_") And SMStartIsBack.Checked = False Then 'Skip StartIsBack files if not chosen
                Continue For
            End If
            If targetPath.StartsWith("SMOpenShell_") And SMOpenShell.Checked = False Then 'Skip Open Shell if not chosen
                Continue For
            End If
            ' Now skip depending on Glass styling
            If targetPath.StartsWith("ShinelessTheme_") And ThemeNoUseShine.Checked = False Then 'Skip Shineless Theme if not chosen
                Continue For
            End If
            If targetPath.StartsWith("ShineTheme_") And ThemeUseShine.Checked = False Then 'Skip Shiny Theme if not chosen
                Continue For
            End If
            ' Now skip depending on shell32 preference
            If targetPath.StartsWith("AllowShell32_") And AllowShell32.Checked = False Then 'Skip shell32 replacement if not chosen
                Continue For
            End If
            ' Now skip depending on usercpl preference
            If targetPath.StartsWith("AllowUserCPL_") And AllowUserCPL.Checked = False Then 'Skip usercpl replacement if not chosen
                Continue For
            End If
            ' Now skip depending on remaining preferences
            If targetPath.StartsWith("AllowUXThemePatcher_") And AllowUXThemePatcher.Checked = False Then 'Skip UXThemePatcher stuff if not chosen
                Continue For
            End If
            If targetPath.StartsWith("Allow7Games_") And Allow7Games.Checked = False Then 'Skip Win7's Games if not chosen
                Continue For
            End If
            If targetPath.StartsWith("Allow7Gadgets_") And Allow7Gadgets.Checked = False Then 'Skip Win7's Gadgets if not chosen
                Continue For
            End If
            If targetPath.StartsWith("AllowMediaCenter_") And AllowMediaCenter.Checked = False And Not IO.Directory.Exists(windir + "\ehome") Then 'Skip Media Center if not chosen, and if not on 8.x +MediaCenter SKU
                Continue For
            End If
            If targetPath.StartsWith("Allow7TaskManager_") And Allow7TaskManager.Checked = False Then 'Skip Task Manager if not chosen
                Continue For
            End If
            If targetPath.StartsWith("ReduceWinX_") And ReduceWinX.Checked = False Then 'Skip alt. Win+X if not chosen
                Continue For
            End If
            If targetPath.StartsWith("NotReduceWinX_") And ReduceWinX.Checked = True Then 'Skip not alt. Win+X if chosen
                Continue For
            End If
            ' Now skip depending on...
            If targetPath.StartsWith("Foxless_") And firefoxInstalled = True Then 'Skip 'foxless' if Firefox or Firefox-based browsers were detected
                Continue For
            End If

            ' Strip setting identifier from id for next steps
            tempArray = targetPath.Split("_")
            tempArray = tempArray.Skip(1).ToArray()
            targetPath = String.Join("_", tempArray.ToArray)


            'This is where you'd skip depending on Aesthetics not being selected, but no other aesthetic options are pre-coded in this, so... this part is empty.

            ' Strip aesthetic identifier from id for next steps
            tempArray = targetPath.Split("_")
            tempArray = tempArray.Skip(1).ToArray()
            targetPath = String.Join("_", tempArray.ToArray)


            'Skip extracted Setup files and tools
            If targetPath.StartsWith("setupfile:") Or targetPath.StartsWith("setuptool:") Then
                Continue For
            End If

            'Now, get on with extracting these archives
            'First, deal with ones that are just straight-up file replacements
            If targetPath = "location" Then
                If WriteFileFromResources(dictEntry.Key.ToString(), storagelocation + "\TEMPfiles.7z") = False Then
                    ErrorOccurred("Failed to extract files")
                    Exit Sub
                End If

                startInfo = New System.Diagnostics.ProcessStartInfo
                startInfo.FileName = storagelocation + "\SetupTools\7za.exe"
                startInfo.Arguments = "x " + storagelocation + "\TEMPfiles.7z -o" + storagelocation + "\FileReplacements * -r"
                startInfo.CreateNoWindow = True
                startInfo.UseShellExecute = True
                startInfo.WindowStyle = ProcessWindowStyle.Hidden

                MyProcess = Process.Start(startInfo)
                MyProcess.WaitForExit()
                IO.File.Delete(storagelocation + "\TEMPfiles.7z")
                If (MyProcess.ExitCode <> 0) Then
                    ErrorOccurred("Failed to extract files")
                    Exit Sub
                End If
            End If

            'Second, deal with ones that are straight-up file replacements... for users.
            If targetPath = "userlocation" Then
                If WriteFileFromResources(dictEntry.Key.ToString(), storagelocation + "\TEMPuserfiles.7z") = False Then
                    ErrorOccurred("Failed to extract files")
                    Exit Sub
                End If

                startInfo = New System.Diagnostics.ProcessStartInfo
                startInfo.FileName = storagelocation + "\SetupTools\7za.exe"
                startInfo.Arguments = "x " + storagelocation + "\TEMPuserfiles.7z -o" + storagelocation + "\UserFileReplacements * -r"
                startInfo.CreateNoWindow = True
                startInfo.UseShellExecute = True
                startInfo.WindowStyle = ProcessWindowStyle.Hidden

                MyProcess = Process.Start(startInfo)
                MyProcess.WaitForExit()
                IO.File.Delete(storagelocation + "\TEMPuserfiles.7z")
                If (MyProcess.ExitCode <> 0) Then
                    ErrorOccurred("Failed to extract files")
                    Exit Sub
                End If
            End If

            'Second, deal with ones that are .res files
            If targetPath = "res" Then
                If WriteFileFromResources(dictEntry.Key.ToString(), storagelocation + "\TEMPres.7z") = False Then
                    ErrorOccurred("Failed to extract files")
                    Exit Sub
                End If

                startInfo = New System.Diagnostics.ProcessStartInfo
                startInfo.FileName = storagelocation + "\SetupTools\7za.exe"
                startInfo.Arguments = "x " + storagelocation + "\TEMPres.7z -o" + storagelocation + "\ResFiles * -r"
                startInfo.CreateNoWindow = True
                startInfo.UseShellExecute = True
                startInfo.WindowStyle = ProcessWindowStyle.Hidden

                MyProcess = Process.Start(startInfo)
                MyProcess.WaitForExit()
                IO.File.Delete(storagelocation + "\TEMPres.7z")
                If (MyProcess.ExitCode <> 0) Then
                    ErrorOccurred("Failed to extract files")
                    Exit Sub
                End If
            End If
        Next

        'Replace percentage paths with their actual paths
        Try
            ' STARTISBACK -> StartIsBack directory
            If Environment.Is64BitOperatingSystem = True Then
                'Make sure that StartIsBack folder is moved to correct place in both patching folders
                For Each directory In {storagelocation + "\FileReplacements", storagelocation + "\ResFiles"}
                    If IO.Directory.Exists(directory + "\STARTISBACK") Then
                        'Make the parent directories if they don't exist yet
                        For Each item In {directory + "\Program Files (x86)"}
                            If Not IO.Directory.Exists(item) Then
                                IO.Directory.CreateDirectory(item)
                            End If
                        Next
                        'Now move the folder, and rename it, to intended parent directory
                        My.Computer.FileSystem.CopyDirectory(directory + "\STARTISBACK", directory + "\Program Files (x86)\StartIsBack", True)
                        Shell(sysprefix + "\cmd.exe /c del """ + directory + "\STARTISBACK" + """ /s /q /f /a", AppWinStyle.Hide, True) 'Delete the old directory now it is copied
                        Shell(sysprefix + "\cmd.exe /c rd """ + directory + "\STARTISBACK" + """ /s /q", AppWinStyle.Hide, True)
                        Shell(sysprefix + "\cmd.exe /c del """ + directory + "\STARTISBACK" + """ /s /q /f /a", AppWinStyle.Hide, True) 'Just to be sure, since deleting is tempremental
                        Shell(sysprefix + "\cmd.exe /c rd """ + directory + "\STARTISBACK" + """ /s /q", AppWinStyle.Hide, True)
                    End If
                Next
            Else
                For Each directory In {storagelocation + "\FileReplacements", storagelocation + "\ResFiles"}
                    If IO.Directory.Exists(directory + "\STARTISBACK") Then
                        For Each item In {directory + "\Program Files"}
                            If Not IO.Directory.Exists(item) Then
                                IO.Directory.CreateDirectory(item)
                            End If
                        Next
                        My.Computer.FileSystem.CopyDirectory(directory + "\STARTISBACK", directory + "\Program Files\StartIsBack", True)
                        Shell(sysprefix + "\cmd.exe /c del """ + directory + "\STARTISBACK" + """ /s /q /f /a", AppWinStyle.Hide, True)
                        Shell(sysprefix + "\cmd.exe /c rd """ + directory + "\STARTISBACK" + """ /s /q", AppWinStyle.Hide, True)
                        Shell(sysprefix + "\cmd.exe /c del """ + directory + "\STARTISBACK" + """ /s /q /f /a", AppWinStyle.Hide, True)
                        Shell(sysprefix + "\cmd.exe /c rd """ + directory + "\STARTISBACK" + """ /s /q", AppWinStyle.Hide, True)
                    End If
                Next
            End If
        Catch ex As Exception
            ErrorOccurred("There was an error moving the StartIsBack files to their correct locations")
            Exit Sub
        End Try
        Try
            ' OPENSHELL -> Open Shell directory
            For Each directory In {storagelocation + "\FileReplacements", storagelocation + "\ResFiles"}
                If IO.Directory.Exists(directory + "\OPENSHELL") Then
                    For Each item In {directory + "\Program Files"}
                        If Not IO.Directory.Exists(item) Then
                            IO.Directory.CreateDirectory(item)
                        End If
                    Next
                    My.Computer.FileSystem.CopyDirectory(directory + "\OPENSHELL", directory + "\Program Files\Open-Shell", True)
                    Shell(sysprefix + "\cmd.exe /c del """ + directory + "\OPENSHELL" + """ /s /q /f /a", AppWinStyle.Hide, True)
                    Shell(sysprefix + "\cmd.exe /c rd """ + directory + "\OPENSHELL" + """ /s /q", AppWinStyle.Hide, True)
                    Shell(sysprefix + "\cmd.exe /c del """ + directory + "\OPENSHELL" + """ /s /q /f /a", AppWinStyle.Hide, True)
                    Shell(sysprefix + "\cmd.exe /c rd """ + directory + "\OPENSHELL" + """ /s /q", AppWinStyle.Hide, True)
                End If
            Next
        Catch ex As Exception
            ErrorOccurred("There was an error moving the Open Shell files to their correct locations")
            Exit Sub
        End Try
        Try
            ' MEDIACENTER -> Windows Media Center directory
            If AllowMediaCenter.Checked = True Then
                'Make sure that Windows Media Center folder is moved to correct place in both patching folders
                For Each directory In {storagelocation + "\FileReplacements", storagelocation + "\ResFiles"}
                    If IO.Directory.Exists(directory + "\MEDIACENTER") Then
                        For Each item In {directory + "\Program Files", directory + "\Program Files\Windows Media Center"}
                            If Not IO.Directory.Exists(item) Then
                                IO.Directory.CreateDirectory(item)
                            End If
                        Next
                        My.Computer.FileSystem.CopyDirectory(directory + "\MEDIACENTER", directory + "\Program Files\Windows Media Center\ehome", True)
                        Shell(sysprefix + "\cmd.exe /c del """ + directory + "\MEDIACENTER" + """ /s /q /f /a", AppWinStyle.Hide, True)
                        Shell(sysprefix + "\cmd.exe /c rd """ + directory + "\MEDIACENTER" + """ /s /q", AppWinStyle.Hide, True)
                        Shell(sysprefix + "\cmd.exe /c del """ + directory + "\MEDIACENTER" + """ /s /q /f /a", AppWinStyle.Hide, True)
                        Shell(sysprefix + "\cmd.exe /c rd """ + directory + "\MEDIACENTER" + """ /s /q", AppWinStyle.Hide, True)
                    End If
                Next
            Else
                For Each directory In {storagelocation + "\FileReplacements", storagelocation + "\ResFiles"}
                    If IO.Directory.Exists(directory + "\MEDIACENTER") Then
                        'Make the parent directories if they don't exist yet
                        For Each item In {directory + "\Windows"}
                            If Not IO.Directory.Exists(item) Then
                                IO.Directory.CreateDirectory(item)
                            End If
                        Next
                        'Now move the folder, and rename it, to intended parent directory
                        My.Computer.FileSystem.CopyDirectory(directory + "\MEDIACENTER", directory + "\Windows\ehome", True)
                        Shell(sysprefix + "\cmd.exe /c del """ + directory + "\MEDIACENTER" + """ /s /q /f /a", AppWinStyle.Hide, True)
                        Shell(sysprefix + "\cmd.exe /c rd """ + directory + "\MEDIACENTER" + """ /s /q", AppWinStyle.Hide, True)
                        Shell(sysprefix + "\cmd.exe /c del """ + directory + "\MEDIACENTER" + """ /s /q /f /a", AppWinStyle.Hide, True)
                        Shell(sysprefix + "\cmd.exe /c rd """ + directory + "\MEDIACENTER" + """ /s /q", AppWinStyle.Hide, True)
                    End If
                Next
            End If
        Catch ex As Exception
            ErrorOccurred("There was an error moving the Windows Media Center files to their correct locations")
            Exit Sub
        End Try
        Try
            ' SIDEBAR -> Windows Desktop Gadgets directory
            For Each directory In {storagelocation + "\FileReplacements", storagelocation + "\ResFiles"}
                If IO.Directory.Exists(directory + "\SIDEBAR") Then
                    For Each item In {directory + "\Program Files"}
                        If Not IO.Directory.Exists(item) Then
                            IO.Directory.CreateDirectory(item)
                        End If
                    Next
                    My.Computer.FileSystem.CopyDirectory(directory + "\SIDEBAR", directory + "\Program Files\Windows Sidebar", True)
                    Shell(sysprefix + "\cmd.exe /c del """ + directory + "\SIDEBAR" + """ /s /q /f /a", AppWinStyle.Hide, True)
                    Shell(sysprefix + "\cmd.exe /c rd """ + directory + "\SIDEBAR" + """ /s /q", AppWinStyle.Hide, True)
                    Shell(sysprefix + "\cmd.exe /c del """ + directory + "\SIDEBAR" + """ /s /q /f /a", AppWinStyle.Hide, True)
                    Shell(sysprefix + "\cmd.exe /c rd """ + directory + "\SIDEBAR" + """ /s /q", AppWinStyle.Hide, True)
                End If
            Next
        Catch ex As Exception
            ErrorOccurred("There was an error moving the Sidebar files to their correct locations")
            Exit Sub
        End Try

        Try
            If isStarter = True Then 'Make some extra adjustments to files if Starter is detected
                ' Delete aero.msstyles, copy aerostandard.msstyles to aero.msstyles, since Starter uses Basic
                IO.File.Delete(storagelocation + "\FileReplacements\Windows\Resources\Themes\aero\aero.msstyles")
                IO.File.Copy(storagelocation + "\FileReplacements\Windows\Resources\Themes\aero\aerostandard.msstyles", storagelocation + "\FileReplacements\Windows\Resources\Themes\aero\aero.msstyles", True)
            End If
        Catch ex As Exception
            ErrorOccurred("Failed to make Starter-specific changes")
            Exit Sub
        End Try

        Try
            If SMStartIsBack.Checked = True And ReduceWinX.Checked = True Then 'Add 'Properties' to Win+X if StartIsBack isn't selected
                IO.File.Delete(storagelocation + "\UserFileReplacements\AppData\Local\Microsoft\Windows\WinX\Group1\2 - Properties.lnk")
            End If
        Catch ex As Exception
            ErrorOccurred("Failed to add Properties to Win+X reduction edits")
            Exit Sub
        End Try

        'Clean up 7z files - it's no longer needed
        IO.File.Delete(storagelocation + "\SetupTools\7z License.txt")
        IO.File.Delete(storagelocation + "\SetupTools\7zxa.dll")
        IO.File.Delete(storagelocation + "\SetupTools\7za.dll")
        IO.File.Delete(storagelocation + "\SetupTools\7za.exe")

        ChangeProgress(4)
        'Configure the Registry options
        ChangeStatus("Configuring Registry values...")

        HKLMKey32.CreateSubKey("SOFTWARE\Win8To7")
        'Branding
        If Branding7Pro.Checked = True Then
            If isHome = False And isStarter = False And isEnterprise = False Then
                HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("Branding", "win7pro")
            ElseIf isHome = True Then
                HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("Branding", "win7home")
            ElseIf isStarter = True Then
                HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("Branding", "win7starter")
            ElseIf isEnterprise = True Then
                HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("Branding", "win7enterprise")
            End If
        ElseIf Branding7Ult.Checked = True Then
            HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("Branding", "win7ult")
        ElseIf Branding8Pro.Checked = True Then
            If isHome = False And isStarter = False And isEnterprise = False Then
                HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("Branding", "win8pro")
            ElseIf isHome = True Then
                HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("Branding", "win8home")
            ElseIf isStarter = True Then
                HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("Branding", "win8starter")
            ElseIf isEnterprise = True Then
                HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("Branding", "win8enterprise")
            End If
        End If
        'Ribbon
        If RibbonWin7.Checked = True Then
            HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("Ribbon", "win7")
        ElseIf RibbonWin8.Checked = True Then
            HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("Ribbon", "win8")
        End If
        'Start Menu
        If SMEx7ForWin8.Checked = True Then
            HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("Start", "ex7forwin8")
        ElseIf SMStartIsBack.Checked = True Then
            HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("Start", "startisback")
        ElseIf SMOpenShell.Checked = True Then
            HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("Start", "openshell")
        End If
        'Glass Style
        If ThemeUseShine.Checked = True Then
            HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("GlassShine", "true")
        ElseIf ThemeNoUseShine.Checked = True Then
            HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("GlassShine", "false")
        End If
        'Window Colors
        If KeepAutomaticColor.Checked = True Then
            HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("KeepAutomatic", "true")
        Else
            HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("KeepAutomatic", "false")
        End If
        If KeepChocolate.Checked = True Then
            HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("KeepChocolate", "true")
        Else
            HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("KeepChocolate", "false")
        End If
        'Remaining preferences
        If AllowUXThemePatcher.Checked = True Then
            HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("AllowUXThemePatcher", "true")
        Else
            HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("AllowUXThemePatcher", "false")
        End If
        If AllowShell32.Checked = True Then
            HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("AllowShell32", "true")
        Else
            HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("AllowShell32", "false")
        End If
        If AllowUserCPL.Checked = True Then
            HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("AllowUserCPL", "true")
        Else
            HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("AllowUserCPL", "false")
        End If
        If Allow7Games.Checked = True Then
            HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("Allow7Games", "true")
        Else
            HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("Allow7Games", "false")
        End If
        If Allow7Gadgets.Checked = True Then
            HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("Allow7Gadgets", "true")
        Else
            HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("Allow7Gadgets", "false")
        End If
        If AllowMediaCenter.Checked = True Or IO.Directory.Exists(windir + "\ehome") Then
            HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("AllowMediaCenter", "true")
        Else
            HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("AllowMediaCenter", "false")
        End If
        If Allow7TaskManager.Checked = True Then
            HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("Allow7TaskManager", "true")
        Else
            HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("Allow7TaskManager", "false")
        End If
        If ReduceWinX.Checked = True Then
            HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("ReduceWinX", "true")
        Else
            HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("ReduceWinX", "false")
        End If
        If DisableLockScreen.Checked = True Then
            HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("DisableLockScreen", "true")
        Else
            HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("DisableLockScreen", "false")
        End If

        'Remove transformation failure indicator if the transformation failed last time
        HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).DeleteValue("TransformationFailed", False)

        If isInstalled = False Then
            'Set Phase to 2
            HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("CurrentPhase", 2)

            ChangeProgress(5)
            ChangeProgressStyle(ProgressBarStyle.Marquee)
            ChangeStatus("Windows will begin transformation in a few moments...")
            Thread.Sleep(8000)

            Shell("taskkill /f /im explorer.exe", AppWinStyle.Hide, False)
            Shell(storagelocation + "\SetupTools\setup.exe", AppWinStyle.MaximizedFocus, False)
            RestartTime("")
        Else
            'Set Phase to 102 (Configuration Phase 2)
            HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("CurrentPhase", 102)

            ChangeProgress(5)
            ChangeProgressStyle(ProgressBarStyle.Marquee)
            ChangeStatus("Windows will restart in a few moments...")
            Thread.Sleep(8000)

            'Switch to progress form
            If SystemInformation.HighContrast = True Then
                HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("HighContrast", 1)
            Else
                HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("HighContrast", 0)
            End If
            Shell(sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v CmdLine /d " + storagelocation + "\setupold.exe" + " /t REG_SZ /f", AppWinStyle.Hide, True)
            Shell(sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v OOBEInProgress /t REG_DWORD /d 1 /f", AppWinStyle.Hide, True)
            Shell(sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SetupPhase /t REG_DWORD /d 4 /f", AppWinStyle.Hide, True)
            Shell(sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SetupType /t REG_DWORD /d 2 /f", AppWinStyle.Hide, True)
            Shell(sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SystemSetupInProgress /t REG_DWORD /d 1 /f", AppWinStyle.Hide, True)
            Shell(sysprefix + "\cmd.exe /c bcdedit /deletevalue {current} safeboot", AppWinStyle.Hide, True)
            Shell(sysprefix + "\cmd.exe /c shutdown /r /t 0 /f", AppWinStyle.Hide, False)
            RestartTime("")
        End If
    End Sub

    Function WriteFileFromResources(ByVal resourceID As String, ByVal targetPath As String)
        Dim audioStream As IO.MemoryStream
        Try
            If targetPath.EndsWith(".png") Then
                My.Resources.ResourceManager.GetObject(resourceID).Save(targetPath, System.Drawing.Imaging.ImageFormat.Png)
            ElseIf targetPath.EndsWith(".bmp") Then
                My.Resources.ResourceManager.GetObject(resourceID).Save(targetPath, System.Drawing.Imaging.ImageFormat.Bmp)
            ElseIf targetPath.EndsWith(".jpg") Then
                My.Resources.ResourceManager.GetObject(resourceID).Save(targetPath, System.Drawing.Imaging.ImageFormat.Jpeg)
            ElseIf targetPath.EndsWith(".gif") Then
                My.Resources.ResourceManager.GetObject(resourceID).Save(targetPath, System.Drawing.Imaging.ImageFormat.Gif)
            ElseIf targetPath.EndsWith(".wav") Then
                audioStream = My.Resources.ResourceManager.GetObject(resourceID)
                My.Computer.FileSystem.WriteAllBytes(targetPath, audioStream.ToArray, False)
            Else
                IO.File.WriteAllBytes(targetPath, My.Resources.ResourceManager.GetObject(resourceID))
            End If
            Return True
        Catch ex As Exception
            ErrorOccurred("Couldn't write " + targetPath + ". Make sure support for it is implemented. Exception message: " + ex.Message.ToString)
            Return False
        End Try
    End Function
#End Region

#Region "Shhhh..."
    Private Sub secret_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles secret.KeyDown
        If e.KeyCode = Keys.Space Then
            Next1.Focus() 'Pretend they pressed Next
            Next1_Click(sender, e)
        End If
    End Sub

    Private Sub secret_TextChanged(sender As System.Object, e As System.EventArgs) Handles secret.TextChanged
        If secret.Text.ToUpper = "AMOGUS" Then
            WriteFileFromResources("int_amogus", System.Environment.GetEnvironmentVariable("TEMP") + "\amogus.png")
            Process.Start(System.Environment.GetEnvironmentVariable("TEMP") + "\amogus.png")
        ElseIf secret.Text.ToUpper = "WUT" Then
            WriteFileFromResources("int_wut", System.Environment.GetEnvironmentVariable("TEMP") + "\GET HELP.png")
            Process.Start(System.Environment.GetEnvironmentVariable("TEMP") + "\GET HELP.png")
        ElseIf secret.Text.ToUpper = "BAKAGEDDON" Then
            PrepForm.SetThemeAppProperties(0)
            MsgBox("no.", MsgBoxStyle.Exclamation + MsgBoxStyle.MsgBoxRight, "")
            PrepForm.SetThemeAppProperties(3)
        ElseIf secret.Text.ToUpper = "IMBLUE" Then
            If SystemInformation.HighContrast = False Then
                Me.BackColor = Color.FromArgb(67, 149, 209)
                Me.BackgroundImage = Nothing
            End If
        Else
            Exit Sub
        End If
        secret.Text = ""
    End Sub
#End Region

#Region "Hitbox Expansions"
    Private Sub Branding8ProImg_Click(sender As System.Object, e As System.EventArgs) Handles Branding8ProImg.Click
        Branding8Pro.Checked = Branding8Pro.Enabled
    End Sub

    Private Sub Branding7ProImg_Click(sender As System.Object, e As System.EventArgs) Handles Branding7ProImg.Click
        Branding7Pro.Checked = Branding7Pro.Enabled
    End Sub

    Private Sub Branding7UltImg_Click(sender As System.Object, e As System.EventArgs) Handles Branding7UltImg.Click
        Branding7Ult.Checked = Branding7Ult.Enabled
    End Sub

    Private Sub PictureBox5_Click(sender As System.Object, e As System.EventArgs) Handles PictureBox5.Click
        RibbonWin7.Checked = RibbonWin7.Enabled
    End Sub

    Private Sub PictureBox6_Click(sender As System.Object, e As System.EventArgs) Handles PictureBox6.Click
        RibbonWin8.Checked = RibbonWin8.Enabled
    End Sub

    Private Sub PictureBox8_Click(sender As System.Object, e As System.EventArgs) Handles PictureBox8.Click
        SMEx7ForWin8.Checked = SMEx7ForWin8.Enabled
    End Sub

    Private Sub PictureBox10_Click(sender As System.Object, e As System.EventArgs) Handles PictureBox10.Click
        SMStartIsBack.Checked = SMStartIsBack.Enabled
    End Sub

    Private Sub PictureBox11_Click(sender As System.Object, e As System.EventArgs) Handles PictureBox11.Click
        SMOpenShell.Checked = SMOpenShell.Enabled
    End Sub

    Private Sub PictureBox12_Click(sender As System.Object, e As System.EventArgs) Handles PictureBox12.Click
        ThemeUseShine.Checked = ThemeUseShine.Enabled
    End Sub

    Private Sub PictureBox9_Click(sender As System.Object, e As System.EventArgs) Handles PictureBox9.Click
        ThemeNoUseShine.Checked = ThemeNoUseShine.Enabled
    End Sub

    Private Sub PictureBox13_Click(sender As System.Object, e As System.EventArgs) Handles PictureBox13.Click
        KeepAutomaticColor.Checked = Not KeepAutomaticColor.Checked And KeepAutomaticColor.Enabled
    End Sub

    Private Sub PictureBox14_Click(sender As System.Object, e As System.EventArgs) Handles PictureBox14.Click
        KeepChocolate.Checked = Not KeepChocolate.Checked And KeepChocolate.Enabled
    End Sub
#End Region
End Class
