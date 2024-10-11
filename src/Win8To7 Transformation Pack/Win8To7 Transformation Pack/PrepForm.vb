Public Class PrepForm
#Region "Classic theme, because Classic theme's funny"
    <DllImport("uxtheme.dll", CharSet:=CharSet.Auto)> _
    Public Shared Sub SetThemeAppProperties(ByVal Flags As Integer)
    End Sub
#End Region

#Region "Variables"
    Private ex7forw8notice1 As String = <a>This option is only available on
Windows 8.0.</a>
    Private ex7forw8notice2 As String = <a>This option breaks if used with
high contrast.</a>
#End Region

    Private Sub PrepForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'First, error out on incompatible versions
        If Not System.Environment.OSVersion.Version.Major = 6 Or (System.Environment.OSVersion.Version.Major = 6 And (System.Environment.OSVersion.Version.Minor > 3 Or System.Environment.OSVersion.Version.Minor < 2)) Then
            Me.Hide()
            SetThemeAppProperties(0)
            MsgBox("This transformation pack is only compatible with Windows 8.0 and Windows 8.1.", MsgBoxStyle.Critical, "Win8To7 Transformation Pack Backend")
            End
        End If

        Me.Show()
        Me.Refresh()

        'Now do 8.1 and 8.0 checks and edit the UI accordingly
        If System.Environment.OSVersion.Version.Minor = 3 Then 'Windows 8.1
            Form1.SMEx7ForWin8.Enabled = False
            Form1.SMWin81Notice.Text = ex7forw8notice1
            If System.Environment.Is64BitOperatingSystem = False Then
                Form1.AllowShell32.Enabled = False
                Form1.AllowShell32.Checked = False
                Form1.Shell32Tooltip.SetToolTip(Form1.AllowShell32, "File Copying dialog is not available in 32-bit Windows.")
                Form1.AllowMediaCenter.Enabled = False
                Form1.AllowMediaCenter.Checked = False
                Form1.MediaCenterTooltip.SetToolTip(Form1.AllowMediaCenter, "Media Center modifications are not available in 32-bit Windows.")
            End If
        Else 'Windows 8.0
            Form1.AllowMediaCenter.Enabled = False
            Form1.AllowMediaCenter.Checked = False
            Form1.MediaCenterTooltip.SetToolTip(Form1.AllowMediaCenter, "Media Center is not available in Windows 8.0 without upgrading to Pro With Media Center.")
            Form1.AllowShell32.Enabled = False
            Form1.AllowShell32.Checked = False
            Form1.Shell32Tooltip.SetToolTip(Form1.AllowShell32, "File Copying dialog is not available in Windows 8.0.")
            Form1.SMWin81Notice.Text = ex7forw8notice2
        End If

        If Microsoft.Win32.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "EditionID", "Professional").ToString.StartsWith("ProfessionalWMC") Then 'Disable Media Center option if it's already present in the SKU itself
            Form1.AllowMediaCenter.Checked = False
            Form1.AllowMediaCenter.Enabled = False
            Form1.MediaCenterTooltip.SetToolTip(Form1.AllowMediaCenter, "Media Center is already installed as part of your version of Windows.")
        End If

        'Hide unavailable options for Starter
        If Form1.isStarter = True Then
            Form1.Branding7Ult.Visible = False
            Form1.Branding7UltImg.Visible = False
            Form1.ColoursPicker.Visible = False
            Form1.Label14.Visible = False
            Form1.Label13.Visible = False
            Form1.AllowMediaCenter.Enabled = False
            Form1.AllowMediaCenter.Checked = False
            Form1.MediaCenterTooltip.SetToolTip(Form1.AllowMediaCenter, "Media Center is not available in Starter.")
            ' Swap out for Starter branding
            Form1.Branding7ProImg.Image = My.Resources.int_win7starterbranding
            Form1.Branding8ProImg.Image = My.Resources.int_win8starterbranding
        ElseIf Form1.isHome = True Then 'Only hide third branding on Home
            Form1.Branding7Ult.Visible = False
            Form1.Branding7UltImg.Visible = False
            'Swap out for Home branding
            Form1.Branding7ProImg.Image = My.Resources.int_win7homebranding
            Form1.Branding8ProImg.Image = My.Resources.int_win8homebranding
        ElseIf Form1.isEnterprise = True Then 'Only hide third branding on Enterprise
            Form1.Branding7Ult.Visible = False
            Form1.Branding7UltImg.Visible = False
            'Swap out for Enterprise branding
            Form1.Branding7ProImg.Image = My.Resources.int_win7enterprisebranding
            Form1.Branding8ProImg.Image = My.Resources.int_win8enterprisebranding
        End If

        Form1.RefreshHC() 'Manually trigger HC check, since it otherwise ONLY gets triggered by the theme changing

        If Not Form1.HKLMKey32.OpenSubKey("SOFTWARE\Win8To7") Is Nothing Then 'We're in a transformed machine?
            Try
                'Branding selecting
                If Form1.HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).GetValue("Branding") = "win8pro" Or _
                Form1.HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).GetValue("Branding") = "win8home" Or _
                Form1.HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).GetValue("Branding") = "win8starter" Or _
                Form1.HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).GetValue("Branding") = "win8enterprise" Then
                    Form1.Branding8Pro.Checked = True
                ElseIf Form1.HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).GetValue("Branding") = "win7pro" Or _
                Form1.HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).GetValue("Branding") = "win7home" Or _
                Form1.HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).GetValue("Branding") = "win7starter" Or _
                Form1.HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).GetValue("Branding") = "win7enterprise" Then
                    Form1.Branding7Pro.Checked = True
                ElseIf Form1.HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).GetValue("Branding") = "win7ult" Then
                    Form1.Branding7Ult.Checked = True
                End If
                ' Make sure third option isn't checked if unavailable
                If Form1.Branding7Ult.Checked = True And (Form1.isStarter = True Or Form1.isHome = True Or Form1.isEnterprise = True) Then
                    Form1.Branding7Pro.Checked = True
                End If
                'Ribbon styling
                If Form1.HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).GetValue("Ribbon") = "win7" Then
                    Form1.RibbonWin7.Checked = True
                ElseIf Form1.HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).GetValue("Ribbon") = "win8" Then
                    Form1.RibbonWin8.Checked = True
                End If
                'Start menu
                If Form1.HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).GetValue("Start") = "ex7forwin8" Then
                    Form1.SMEx7ForWin8.Checked = True
                ElseIf Form1.HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).GetValue("Start") = "startisback" Then
                    Form1.SMStartIsBack.Checked = True
                ElseIf Form1.HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).GetValue("Start") = "openshell" Then
                    Form1.SMOpenShell.Checked = True
                End If
                'Glass styles
                If Form1.HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).GetValue("GlassShine") = "true" Then
                    Form1.ThemeUseShine.Checked = True
                ElseIf Form1.HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).GetValue("GlassShine") = "false" Then
                    Form1.ThemeNoUseShine.Checked = True
                End If
                'Keep Automatic
                Form1.KeepAutomaticColor.Checked = (Form1.HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).GetValue("KeepAutomatic") = "true")
                Form1.KeepChocolate.Checked = (Form1.HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).GetValue("KeepChocolate") = "true")
                'Remaining prefs
                Form1.AllowUXThemePatcher.Checked = (Form1.HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).GetValue("AllowUXThemePatcher") = "true")
                Form1.AllowUXThemePatcher.Enabled = False 'prevent changing its... install-state to prevent a brick
                Form1.AllowShell32.Checked = (Form1.HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).GetValue("AllowShell32") = "true")
                Form1.AllowUserCPL.Checked = (Form1.HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).GetValue("AllowUserCPL") = "true")
                Form1.Allow7Games.Checked = (Form1.HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).GetValue("Allow7Games") = "true")
                Form1.Allow7Gadgets.Checked = (Form1.HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).GetValue("Allow7Gadgets") = "true")
                Form1.AllowMediaCenter.Checked = (Form1.HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).GetValue("AllowMediaCenter") = "true")
                Form1.Allow7TaskManager.Checked = (Form1.HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).GetValue("Allow7TaskManager") = "true")
                Form1.DisableLockScreen.Checked = (Form1.HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).GetValue("DisableLockScreen") = "true")
                If System.Environment.OSVersion.Version.Minor = 3 And Not _
                    Microsoft.Win32.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "EditionID", "Professional").ToString.StartsWith("ProfessionalWMC") Then
                    Form1.AllowMediaCenter.Enabled = True
                    Form1.MediaCenterTooltip.SetToolTip(Form1.AllowMediaCenter, Nothing)
                End If

                If Form1.HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("CurrentPhase") = 10 Then 'Load configuration
                    Form1.PageOptions.Visible = True
                    Form1.Page1.Visible = False
                    Form1.BackArea.Visible = True
                    Form1.isInstalled = True
                    Form1.NextConfirm.Text = "Apply"
                    Form1.Text = "Manage - Win8To7 Transformation Pack Backend"
                    Form1.Label17.Text = "Select extra components"
                    Form1.Label16.Text = "Manage the extra components installed on your computer below. Once done, click Apply to apply your changes."
                    Form1.Label12.Visible = False 'Hide the transformation notice
                    Form1.Label20.Text = "Updating transformation..."
                End If
            Catch
            End Try
        End If

        ProgressBar1.Visible = True
        Dim jobthread As New Thread(AddressOf LoadPack)
        jobthread.Start()
    End Sub

    Private Sub LoadPack()
        If Form1.HKLMKey32.OpenSubKey("SOFTWARE\Win8To7") Is Nothing Then
            ChangeStatus("Checking for updates...")
            'Check updates are pending
            Try
                Dim uSession As Object = CreateObject("Microsoft.Update.Session")
                Dim uSearcher As Object = uSession.CreateUpdateSearcher()
                uSearcher.Online = False
                Dim sResult As Object = uSearcher.Search("IsInstalled=0 And IsHidden=0")

                If sResult.Updates.Count > 0 Then
                    SetThemeAppProperties(0) 'now laugh
                    If MsgBox("Windows Updates are available for your system. You should apply them before transforming, as updating your system with certain years-old-updates while transformed can brick your install of Windows, and lacking certain updates before February 2021 can make the transformation brick Windows entirely." + Environment.NewLine + Environment.NewLine + "KNOWING THIS, would you like to continue?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2 + MsgBoxStyle.SystemModal, "Win8To7 Transformation Pack Backend") = MsgBoxResult.Yes Then
                        SetThemeAppProperties(3)
                    Else
                        End
                    End If
                End If
            Catch ex As Exception
                If MsgBox("We couldn't check your Windows installation for available updates." + Environment.NewLine + "Please be warned that updating your system with certain years-old-updates while transformed can brick your install of Windows, and lacking certain updates before February 2021 can make the transformation brick Windows entirely." + Environment.NewLine + Environment.NewLine + "KNOWING THIS, would you like to continue?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2 + MsgBoxStyle.SystemModal, "Win8To7 Transformation Pack Backend") = MsgBoxResult.Yes Then
                    SetThemeAppProperties(3)
                Else
                    End
                End If
            End Try
        End If
        ChangeStatus("Loading...")
        CloseSplash()
    End Sub

    Private Sub CloseSplash()
        'This bit here makes it work despite being called via a VB.NET Thread
        If Me.InvokeRequired Then
            Me.Invoke(New Action(AddressOf CloseSplash))
            Exit Sub
        End If

        Form1.Show()
        Form1.secret.Focus()
        Me.Close()
    End Sub

    Private Sub ChangeStatus(ByVal status As String)
        'This bit here makes it work despite being called via a VB.NET Thread
        If Me.InvokeRequired Then
            Dim args() As String = {status}
            Me.Invoke(New Action(Of String)(AddressOf ChangeStatus), args)
            Exit Sub
        End If

        Me.Text = status
    End Sub
End Class
