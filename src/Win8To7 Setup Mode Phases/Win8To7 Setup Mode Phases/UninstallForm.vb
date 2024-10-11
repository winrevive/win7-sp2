Public Class UninstallForm
#Region "HC compatibility"
    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        Const WM_THEMECHANGED = &H31A

        If (m.Msg = WM_THEMECHANGED) Then
            RefreshHC()
            Me.Refresh()
        End If

        MyBase.WndProc(m)
    End Sub

    Public Sub RefreshHC(Optional ByVal override As Boolean = False)
        If override = True Or SystemInformation.HighContrast = True Then
            Me.BackColor = Control.DefaultBackColor
            Me.BackgroundImage = Nothing
            Me.ForeColor = Control.DefaultForeColor
            LinkLabel1.ForeColor = Control.DefaultForeColor
            LinkLabel1.LinkColor = Control.DefaultForeColor
            LinkLabel1.ActiveLinkColor = Control.DefaultForeColor
            LinkLabel1.VisitedLinkColor = Control.DefaultForeColor
        Else
            Me.BackColor = Color.Black
            Me.BackgroundImage = My.Resources.int_SetupBG
            Me.ForeColor = Color.White
            LinkLabel1.ForeColor = Color.White
            LinkLabel1.LinkColor = Color.White
            LinkLabel1.ActiveLinkColor = Color.White
            LinkLabel1.VisitedLinkColor = Color.White
        End If
    End Sub
#End Region

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        RefreshHC()
    End Sub

    Private Sub UninstallForm_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If PageTransform.Visible = True Then
            MsgBox("This operation cannot be cancelled, as cancelling this operation can risk rendering Windows unusable.", MsgBoxStyle.Exclamation, "Win8To7 Transformation Pack Backend")
            e.Cancel = True
        End If
    End Sub

#Region "Label status changes"
    Private Sub ChangeProgress(ByVal progress As Integer)
        'This bit here makes it work despite being called via a VB.NET Thread
        If Me.InvokeRequired Then
            Dim args() As String = {progress}
            Me.Invoke(New Action(Of String)(AddressOf ChangeProgress), args)
            Return
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
            Return
        End If

        LabelStatus.Text = "Status: " + status 'Change the label for the current job in progress
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

        'Make sure we're NOT in Setup Mode
        Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v CmdLine /t REG_SZ /f", AppWinStyle.Hide, True)
        Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v OOBEInProgress /t REG_DWORD /d 0 /f", AppWinStyle.Hide, True)
        Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SetupPhase /t REG_DWORD /d 0 /f", AppWinStyle.Hide, True)
        Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SetupType /t REG_DWORD /d 0 /f", AppWinStyle.Hide, True)
        Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SystemSetupInProgress /t REG_DWORD /d 0 /f", AppWinStyle.Hide, True)

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
        'Page Confirm (Restore) -> 1
        If PageConfirmRestore.Visible = True Then
            Page1.Visible = True
            PageConfirmRestore.Visible = False
            BackButton.Enabled = False
        End If
        'Page Confirm (Remove) -> 1
        If PageConfirmRemove.Visible = True Then
            Page1.Visible = True
            PageConfirmRemove.Visible = False
            BackButton.Enabled = False
        End If
    End Sub
#End Region
#Region "Buttons"
    Private Sub Next1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Next1.Click
        If OptRestore.Checked = True Then
            PageConfirmRestore.Visible = True
        ElseIf OptRemove.Checked = True Then
            PageConfirmRemove.Visible = True
        End If
        Page1.Visible = False
        BackButton.Enabled = True
    End Sub

    Private Sub NextConfirmRestore_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NextConfirmRestore.Click
        BackArea.Visible = False
        PageTransform.Visible = True
        PageConfirmRestore.Visible = False

        Dim jobthread As New Thread(AddressOf DoTheJobRestore)
        jobthread.Start()
    End Sub

    Private Sub NextConfirmRemoval_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NextConfirmRemoval.Click
        BackArea.Visible = False
        PageTransform.Visible = True
        PageConfirmRemove.Visible = False

        Try
            IO.File.Copy(storagelocation + "\SetupTools\setup.exe", windir + "\Temp\win8to7uninstall.exe", True)
        Catch ex As Exception
            ErrorOccurred("Failed to write Removal Executable: " + ex.Message)
            Exit Sub
        End Try
        Dim jobthread As New Thread(AddressOf DoTheJobRemove)
        jobthread.Start()
    End Sub
#End Region
#Region "Credits button"
    Private Sub LinkLabel1_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Credits.Show()
        Credits.Focus()
    End Sub
#End Region

#Region "Start Restoration"
    Sub DoTheJobRestore()
        ChangeStatus("Creating a restore point...")
        ChangeProgress(1)
        ChangeProgressStyle(ProgressBarStyle.Marquee)
        Shell(windir + "\" + sysprefix + "\cmd.exe /c reg add ""HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore"" /v ""SystemRestorePointCreationFrequency"" /t REG_DWORD /d 0 /f", AppWinStyle.Hide, True)
        Shell(windir + "\" + sysprefix + "\cmd.exe /c wmic.exe /Namespace:\\root\default Path SystemRestore Call CreateRestorePoint ""Before uninstalling Win8To7 Transformation Pack Backend"", 100, 12", AppWinStyle.Hide, True)
        Shell(windir + "\" + sysprefix + "\cmd.exe /c reg del ""HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore"" /v ""SystemRestorePointCreationFrequency"" /f", AppWinStyle.Hide, True)
        ChangeProgressStyle(ProgressBarStyle.Continuous)

        ChangeProgress(2)
        ChangeProgressStyle(ProgressBarStyle.Marquee)
        ChangeStatus("Windows will begin restoration in a few moments...")
        Thread.Sleep(8000)
        EnterRestoration()
    End Sub

    Sub EnterRestoration()
        'This bit here makes it work despite being called via a VB.NET Thread
        If Me.InvokeRequired Then
            Me.Invoke(New Action(AddressOf EnterRestoration))
            Return
        End If

        'Set Phase to 97 (Restoration Stage 1)
        HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("CurrentPhase", 97)
        'Terminate explorer before going into the restoration process
        Shell("taskkill /f /im explorer.exe", AppWinStyle.Hide, True)
        'Switch to Restoration form
        RestorationForm.Show()
        PageTransform.Visible = False
        Me.Close()
    End Sub
#End Region
#Region "Start Removal"
    Sub DoTheJobRemove()
        ChangeStatus("Creating a restore point...")
        ChangeProgress(1)
        ChangeProgressStyle(ProgressBarStyle.Marquee)
        Shell(windir + "\" + sysprefix + "\cmd.exe /c reg add ""HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore"" /v ""SystemRestorePointCreationFrequency"" /t REG_DWORD /d 0 /f", AppWinStyle.Hide, True)
        Shell(windir + "\" + sysprefix + "\cmd.exe /c wmic.exe /Namespace:\\root\default Path SystemRestore Call CreateRestorePoint ""Before deleting Win8To7 Transformation Pack Backend backups"", 100, 12", AppWinStyle.Hide, True)
        Shell(windir + "\" + sysprefix + "\cmd.exe /c reg del ""HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\SystemRestore"" /v ""SystemRestorePointCreationFrequency"" /f", AppWinStyle.Hide, True)
        ChangeProgressStyle(ProgressBarStyle.Continuous)

        ChangeProgress(2)
        ChangeProgressStyle(ProgressBarStyle.Marquee)
        ChangeStatus("Windows will restart in a few moments...")
        Thread.Sleep(8000)
        EnterRemoval()
    End Sub

    Sub EnterRemoval()
        'This bit here makes it work despite being called via a VB.NET Thread
        If Me.InvokeRequired Then
            Me.Invoke(New Action(AddressOf EnterRemoval))
            Return
        End If

        'Set Phase to 69 (Removal Stage)
        HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("CurrentPhase", 69)

        'Switch to progress form
        Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v CmdLine /d " + windir + "\Temp\win8to7uninstall.exe" + " /t REG_SZ /f", AppWinStyle.Hide, True)
        Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v OOBEInProgress /t REG_DWORD /d 1 /f", AppWinStyle.Hide, True)
        Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SetupPhase /t REG_DWORD /d 4 /f", AppWinStyle.Hide, True)
        Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SetupType /t REG_DWORD /d 2 /f", AppWinStyle.Hide, True)
        Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SystemSetupInProgress /t REG_DWORD /d 1 /f", AppWinStyle.Hide, True)
        RestartTime("inwin")
    End Sub
#End Region

End Class
