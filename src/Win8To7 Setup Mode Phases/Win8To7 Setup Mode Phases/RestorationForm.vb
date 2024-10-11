Public Class RestorationForm

#Region "Variables"
    Private StatusTextCustomise As String = <a>Updating transformation... Stage PHASE of 4 - PROGRESS% complete.
Do not turn off your computer.</a>

    Private AnimValue As Integer = 0
    Private CurrentPhase As Integer
    Private totalTasks As Integer = 0
    Private doneTasks As Integer = 0
#End Region

#Region "HC compatibility"
    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        Const WM_THEMECHANGED = &H31A 'also refer to Form1 for comments about this

        If (m.Msg = WM_THEMECHANGED) Then
            RefreshHC()
            Me.Refresh()
        End If

        MyBase.WndProc(m)
    End Sub

    Private Sub RefreshHC(Optional ByVal override As Boolean = False)
        If SystemInformation.HighContrast = True Then
            Me.BackgroundImage = Nothing
            Me.BackColor = Control.DefaultBackColor
            Me.ForeColor = Control.DefaultForeColor
        Else
            If override = True Then '...and these as well
                Me.BackgroundImage = Nothing
                Me.BackColor = Color.Black
                Me.ForeColor = Color.White
            Else
                Me.BackgroundImage = My.Resources.int_SetupBG
                Me.BackColor = Color.Black
                Me.ForeColor = Color.White
            End If
            If HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("Branding") = "win8pro" Then
                CustomisingLogo.BackgroundImage = My.Resources.win8probranding
            ElseIf HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("Branding") = "win7pro" Then
                CustomisingLogo.BackgroundImage = My.Resources.win7probranding
            ElseIf HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("Branding") = "win7ult" Then
                CustomisingLogo.BackgroundImage = My.Resources.win7ultimatebranding
            ElseIf HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("Branding") = "win7home" Then
                CustomisingLogo.BackgroundImage = My.Resources.win7homebranding
            ElseIf HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("Branding") = "win8home" Then
                CustomisingLogo.BackgroundImage = My.Resources.win8homebranding
            ElseIf HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("Branding") = "win7starter" Then
                CustomisingLogo.BackgroundImage = My.Resources.win7starterbranding
            ElseIf HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("Branding") = "win8starter" Then
                CustomisingLogo.BackgroundImage = My.Resources.win8starterbranding
            ElseIf HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("Branding") = "win7enterprise" Then
                CustomisingLogo.BackgroundImage = My.Resources.win7enterprisebranding
            ElseIf HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("Branding") = "win8enterprise" Then
                CustomisingLogo.BackgroundImage = My.Resources.win8enterprisebranding
            End If
        End If
    End Sub
#End Region

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Cursor.Hide()

        Functions.SetErrorOccurred(New Action(Of String)(AddressOf ErrorOccurred)) 'Mark this Form's ErrorOccurred as the one for Functions to call if required

        'High Contrast check
        If HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("HighContrast") = 1 Then
            RefreshHC(True)
        Else
            RefreshHC()
        End If

        Thread.Sleep(700)
        FakeIntro.Hide() 'Simulate entering upgrade rollback :P
        Cursor.Hide()

        Dim animthread As New Thread(AddressOf AnimProgress)
        animthread.Start()

        If HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("CurrentPhase") = 97 Then
            Dim jobthread As New Thread(AddressOf Phase2)
            jobthread.Start()
        ElseIf HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("CurrentPhase") = 98 Then
            Dim jobthread As New Thread(AddressOf Phase3)
            jobthread.Start()
        ElseIf HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("CurrentPhase") = 99 Then
            Dim jobthread As New Thread(AddressOf Phase4)
            jobthread.Start()
        ElseIf HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("CurrentPhase") = 102 Then 'Stage 102 = Restoration Stage 3, but abridged
            animthread.Abort() 'Stop the restoration animation, because it'll waste resources
            CustomisingMode.Visible = True 'Switch GUIs to Customising
            forCustomise = True
            Dim jobthread As New Thread(AddressOf Phase3)
            jobthread.Start()
        Else
            End
        End If
    End Sub

    Private Sub Form1_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        e.Cancel = True 'Instead of closing, the Form just gets out-right ended anyway.
    End Sub

#Region "Label status changes - Updating"
    'For customisation mode:
    Private Sub AboutToRestart(ByVal status As String)
        'This bit here makes it work despite being called via a VB.NET Thread
        If Me.InvokeRequired Then
            Dim args() As String = {status}
            Me.Invoke(New Action(Of String)(AddressOf AboutToRestart), args)
            Return
        End If

        CustomisingStatus.Text = "Restarting"
    End Sub

    'For customisation mode:
    Private Sub CustomisationCleanup(ByVal status As String)
        'This bit here makes it work despite being called via a VB.NET Thread
        If Me.InvokeRequired Then
            Dim args() As String = {status}
            Me.Invoke(New Action(Of String)(AddressOf CustomisationCleanup), args)
            Return
        End If

        CustomisingStatus.Text = "Preparing for next stage of transformation..." 'Unique status for when preparing next Stage of updating
    End Sub

    'For customisation mode:
    Private Sub ChangeProgress(ByVal progress As Integer)
        'This bit here makes it work despite being called via a VB.NET Thread
        If Me.InvokeRequired Then
            Dim args() As String = {progress}
            Me.Invoke(New Action(Of String)(AddressOf ChangeProgress), args)
            Return
        End If

        CustomisingStatus.Text = StatusTextCustomise.Replace("PROGRESS", progress.ToString()).Replace("PHASE", CurrentPhase.ToString()) 'refer to Form1's comments
    End Sub

    'For customisation mode:
    Private Sub Phase4Progress()
        While True
            ChangeProgress(Math.Floor((doneTasks / totalTasks) * 100))
            Thread.Sleep(80)
        End While
    End Sub
#End Region
#Region "Progress Animation - Restoration"
    Private Sub SetProgress(ByVal value As Integer)
        'This bit here makes it work despite being called via a VB.NET Thread
        If Me.InvokeRequired Then
            Dim args As Integer = value
            Me.Invoke(New Action(Of Integer)(AddressOf SetProgress), args)
            Return
        End If

        AnimValue = value
        ProgressAnimFill.Width = (ProgressAnim.Width - 6) * (value / 100) 'Maximum width being the size of the progress thing's free space minus the padding present
    End Sub

    Private Sub AnimProgress(ByVal progress As Integer)
        While True
            If AnimValue = 100 Then
                SetProgress(0)
            Else
                SetProgress(AnimValue + 1)
            End If
            Thread.Sleep(40) 'Slow down the animation to make it not too fast
        End While
    End Sub
#End Region

#Region "Registry functions"
    Private Function RestoreRegistry(ByVal Key As String, Optional ByVal Value As String = "", Optional ByVal Recursive As Boolean = False) 'Refer to Form1's BackupRegistry for comments
        Dim tempArray As String() = Key.Split("\")
        Dim locationPrefix As String = ""
        Dim errorfilelocation As String = windir + "\TEMP\regchangeerror" + rng.Next(0, 999999999 + 1).ToString()
        While IO.File.Exists(errorfilelocation)
            errorfilelocation = windir + "\TEMP\regchangeerror" + rng.Next(0, 999999999 + 1).ToString()
            If IO.File.Exists(errorfilelocation) Then
                Try 'Anti-softlock
                    IO.File.Delete(errorfilelocation)
                Catch
                End Try
            End If
        End While

        'Add a prefix to the target location according to the data presented
        If Key.StartsWith("HKLM\UserConfigClasses") Then
            locationPrefix = "HKLM\UserConfig\Software\Win8To7\RegBackup\HKCU"
        ElseIf Key.StartsWith("HKLM\UserConfig") Then
            locationPrefix = "HKLM\UserConfig\Software\Win8To7\RegBackup\HKCU"
        Else
            locationPrefix = "HKLM\SOFTWARE\Win8To7\RegBackup\" + tempArray.GetValue(0)
        End If

        Shell(storagelocation + "\SetupTools\regchange.exe restore """ + Key + """ """ + Value + """ """ + locationPrefix + """ """ + errorfilelocation + """ True", AppWinStyle.Hide, True) 'True is placeholder'd here

        If IO.File.Exists(errorfilelocation) Then 'If the file exists, it means it encountered an error - fail with the error encountered
            ErrorOccurred(IO.File.ReadAllText(errorfilelocation))
            Return False
        End If

        'Also delete the key if it is empty after doing this
        Shell(storagelocation + "\SetupTools\regchange.exe deleteifempty """ + Key + """ """" """" """ + errorfilelocation + """ True", AppWinStyle.Hide, True) 'True is placeholder'd here

        If IO.File.Exists(errorfilelocation) Then 'If the file exists, it means it encountered an error - fail with the error encountered
            ErrorOccurred(IO.File.ReadAllText(errorfilelocation))
            Return False
        End If
        Return True
    End Function

    Private Function RestoreRegistryAll(ByVal Key As String) 'Note: Restore Registry All does NOT delete anything - that's Restore Registry's job
        Dim tempArray As String() = Key.Split("\")
        Dim locationPrefix As String = ""
        Dim errorfilelocation As String = windir + "\TEMP\regchangeerror" + rng.Next(0, 999999999 + 1).ToString()
        While IO.File.Exists(errorfilelocation)
            errorfilelocation = windir + "\TEMP\regchangeerror" + rng.Next(0, 999999999 + 1).ToString()
            If IO.File.Exists(errorfilelocation) Then
                Try 'Anti-softlock
                    IO.File.Delete(errorfilelocation)
                Catch
                End Try
            End If
        End While

        'Add a prefix to the target location according to the data presented
        If Key.StartsWith("HKLM\UserConfigClasses") Then
            locationPrefix = "HKLM\UserConfig\Software\Win8To7\RegBackup\HKCU"
        ElseIf Key.StartsWith("HKLM\UserConfig") Then
            locationPrefix = "HKLM\UserConfig\Software\Win8To7\RegBackup\HKCU"
        Else
            locationPrefix = "HKLM\SOFTWARE\Win8To7\RegBackup\" + tempArray.GetValue(0)
        End If

        Shell(storagelocation + "\SetupTools\regchange.exe restoreall """ + Key + """ """" """ + locationPrefix + """ """ + errorfilelocation + """ True", AppWinStyle.Hide, True)

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

        HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("CurrentPhase", 10)

        'Go back into Windows as an emergency precaution as well
        Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v CmdLine /t REG_SZ /f", AppWinStyle.Hide, True)
        Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v OOBEInProgress /t REG_DWORD /d 0 /f", AppWinStyle.Hide, True)
        Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SetupPhase /t REG_DWORD /d 0 /f", AppWinStyle.Hide, True)
        Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SetupType /t REG_DWORD /d 0 /f", AppWinStyle.Hide, True)
        Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SystemSetupInProgress /t REG_DWORD /d 0 /f", AppWinStyle.Hide, True)

        ErrorScreen.setStrings("restoring", "Restore pre-transformation files and settings and uninstall", status) 'flavour text, like Form1's
        ErrorScreen.Show()
        Me.Hide()
    End Sub
#End Region

#Region "Stage 2 of 4"
    Sub Phase2()
        Try
            'Boot into installer for Phase 3 with the Setup Mode
            HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("CurrentPhase", 98)
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v CmdLine /d " + storagelocation + "\SetupTools\setup.exe" + " /t REG_SZ /f", AppWinStyle.Hide, True)
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v OOBEInProgress /t REG_DWORD /d 1 /f", AppWinStyle.Hide, True)
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SetupPhase /t REG_DWORD /d 4 /f", AppWinStyle.Hide, True)
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SetupType /t REG_DWORD /d 2 /f", AppWinStyle.Hide, True)
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SystemSetupInProgress /t REG_DWORD /d 1 /f", AppWinStyle.Hide, True)

            RestartTime("inwin")
        Catch ex As Exception
            ErrorOccurred(ex.ToString())
            Exit Sub
        End Try
    End Sub
#End Region
#Region "Stage 3 of 4"
    Private Sub Phase3()
        Try
            Dim directoriesList As List(Of String)
            Dim filePath As String

            Dim fiArr As IO.FileInfo()
            Dim loopfileinfo As IO.FileInfo

            Dim tempArray As String()
            Dim tempArray2 As String()

            Dim ModifiedFilesTarget As String = storagelocation + "\FileReplacements\"
            If forCustomise = True Then
                ModifiedFilesTarget = windir + "\Temp\Win8To7DiscardedFiles\" 'Change where we drop the modified files to a temporary folder we delete later on, to make sure it doesn't conflict with the new version's file changes 
            End If

            Dim dirArr As IO.DirectoryInfo() = New IO.DirectoryInfo(windrive + "Users").GetDirectories() 'Required for getting list of directories in directory to loop through
            Dim loopdirinfo As IO.DirectoryInfo 'for the loop below

            'Make sure Windows is still in Setup Mode, in case an unexpected shutdown suddenly occurs
            If forCustomise = False Then
                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v CmdLine /d " + storagelocation + "\SetupTools\setup.exe" + " /t REG_SZ /f", AppWinStyle.Hide, True)
            Else
                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v CmdLine /d " + storagelocation + "\setupold.exe" + " /t REG_SZ /f", AppWinStyle.Hide, True)
            End If
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v OOBEInProgress /t REG_DWORD /d 1 /f", AppWinStyle.Hide, True)
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SetupPhase /t REG_DWORD /d 4 /f", AppWinStyle.Hide, True)
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SetupType /t REG_DWORD /d 2 /f", AppWinStyle.Hide, True)
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SystemSetupInProgress /t REG_DWORD /d 1 /f", AppWinStyle.Hide, True)

            Dim knownFileChanges As Array = HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("FilesChanged")
            Dim knownUserFileChanges As Array = HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("UserFilesChanged")
            Dim knownFolderChanges As Array = HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("FoldersChanged")
            Dim knownUserFolderChanges As Array = HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("UserFoldersChanged")
            Dim jobthread As New Thread(AddressOf Phase4Progress)
            Dim tries As Integer

            If forCustomise = True Then
                CurrentPhase = 2
                ChangeProgress(0)

                'Count things to change, since we show the Customising GUI
                ' Files without backups, to delete
                For Each item In knownFileChanges
                    tempArray = item.Split("\")
                    filePath = Join(tempArray.Skip(1).ToArray(), "\") 'Skip C:\ on its own for directory creations
                    If IO.File.Exists(storagelocation + "\Backups\" + filePath) Then
                        Continue For
                    End If
                    totalTasks += 1
                Next
                ' Files with backups, to restore
                If IO.Directory.Exists(storagelocation + "\Backups") Then 'Safeguard to prevent failure
                    directoriesList = ListDirectory(storagelocation + "\Backups")
                    For Each item In directoriesList
                        fiArr = New IO.DirectoryInfo(item).GetFiles() 'Get the files in this directory
                        For Each loopfileinfo In fiArr
                            tempArray = loopfileinfo.FullName.Split("\")
                            filePath = Join(tempArray.Skip(1).ToArray(), "\") 'Skip C:\ on its own for directory creations
                            If IO.File.Exists(ModifiedFilesTarget + filePath) And Not IO.File.Exists(storagelocation + "\Backups\" + filePath) Then
                                Continue For
                            End If
                            totalTasks += 1
                        Next
                    Next
                End If
                totalTasks += 1 'Deleting Swatches
                ' Registry keys
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
                ' 4: User-wide changes, multiplied by each user
                For Each loopdirinfo In dirArr
                    If Not IO.File.Exists(loopdirinfo.FullName + "\NTUSER.DAT") Then
                        Continue For 'Skip folder if not a Windows user or Default User Skeleton
                    End If
                    If loopdirinfo.Name = "All Users" Or loopdirinfo.Name = "Default User" Then
                        Continue For 'Skip symlinks
                    End If
                    For Each item In knownFileChanges
                        tempArray = item.Split("\")
                        filePath = Join(tempArray.Skip(1).ToArray(), "\") 'Skip C:\ on its own for directory creations
                        If IO.File.Exists(loopdirinfo.FullName + storagelocationuser + "\Backups\" + filePath) Then
                            Continue For
                        End If
                        totalTasks += 1
                    Next
                    If IO.Directory.Exists(loopdirinfo.FullName + storagelocationuser + "\Backups") Then 'Safeguard to prevent failure
                        directoriesList = ListDirectory(loopdirinfo.FullName + storagelocationuser + "\Backups")
                        For Each item In directoriesList
                            fiArr = New IO.DirectoryInfo(item).GetFiles() 'Get the files in this directory
                            For Each loopfileinfo In fiArr
                                tempArray = loopfileinfo.FullName.Split("\")
                                filePath = Join(tempArray.Skip(1).ToArray(), "\") 'Skip C:\ on its own for directory creations
                                If Not IO.File.Exists(loopdirinfo.FullName + storagelocationuser + "\Backups\" + filePath) Then
                                    Continue For
                                End If
                                totalTasks += 1
                            Next
                        Next
                    End If
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
                    If IO.File.Exists(loopdirinfo.FullName + "\AppData\Local\Microsoft\Windows\UsrClass.dat") Then
                        For Each key In regtweaks.UserClassTweaks.Item("Delete")
                            totalTasks += 1
                        Next
                        For Each key In regtweaks.UserClassTweaks.Item("DWORD")
                            totalTasks += 1
                        Next
                        For Each key In regtweaks.UserClassTweaks.Item("Binary")
                            totalTasks += 1
                        Next
                        For Each key In regtweaks.UserClassTweaks.Item("String")
                            totalTasks += 1
                        Next
                        For Each key In regtweaks.UserClassTweaks.Item("MultiString")
                            totalTasks += 1
                        Next
                    End If
                Next

                jobthread.Start() 'launch the progress thingy
            End If


            'DELETE FILES THAT HAVE NO BACKUPS
            For Each item In knownFileChanges
                'Check that there isn't a backup - if there is, we'll handle it later
                If IO.File.Exists(item.Replace(windrive, storagelocation + "\Backups\")) Then
                    Continue For
                End If

                'Move new file to temporary area
                If IO.File.Exists(item) Then
                    If MoveFile(item, item.Replace(windrive, ModifiedFilesTarget), "files with no backups", False) = False Then
                        Exit Sub
                    End If
                ElseIf IO.Directory.Exists(item) Then
                    If MoveFolder(item, item.Replace(windrive, ModifiedFilesTarget), "folders with no backups", False) = False Then
                        Exit Sub
                    End If
                End If

                doneTasks += 1 'Add 1 to doneTasks
            Next


            'RESTORE THE REMAINING FILES
            If IO.Directory.Exists(storagelocation + "\Backups") Then 'Safeguard to prevent failure
                directoriesList = ListDirectory(storagelocation + "\Backups")
                For Each item In directoriesList
                    fiArr = New IO.DirectoryInfo(item).GetFiles() 'Get the files in this directory
                    For Each loopfileinfo In fiArr
                        'Move new file to temporary area
                        If MoveFile(loopfileinfo.FullName.Replace(storagelocation + "\Backups\", windrive), loopfileinfo.FullName.Replace(storagelocation + "\Backups\", ModifiedFilesTarget), _
                                    "remaining files", False) = False Then
                            Exit Sub
                        End If
                        'Move original file to its original location
                        If MoveFile(loopfileinfo.FullName, loopfileinfo.FullName.Replace(storagelocation + "\Backups\", windrive), "remaining files restoration", False) = False Then
                            Exit Sub
                        End If

                        doneTasks += 1 'Add 1 to doneTasks
                    Next
                Next
            End If


            'DELETE FOLDERS INTENTIONALLY CREATED BY THE TRANSFORMATION
            For Each item In knownFolderChanges
                tries = 0
                While Not tries = 10
                    Shell(windir + "\" + sysprefix + "\cmd.exe /c del """ + item + """ /s /q /f /a", AppWinStyle.Hide, True)
                    Shell(windir + "\" + sysprefix + "\cmd.exe /c rd """ + item + """ /s /q", AppWinStyle.Hide, True)
                    If Not IO.Directory.Exists(item) Then
                        Exit While
                    End If
                    tries += 1
                End While
                If IO.Directory.Exists(item) Then
                    ErrorOccurred("Failed to delete directory created by transformation: " + item)
                    Exit Sub
                End If
            Next


            'DELETE EMPTIED FOLDERS CREATED DURING TRANSFORMATION
            For Each loopy In {knownFileChanges, knownFolderChanges}
                tempArray = loopy
                Array.Sort(tempArray)
                Array.Reverse(tempArray) 'Do it reverse alphabetical so that the longest paths happen before the shortest ones

                For Each item In tempArray
                    tempArray2 = item.Split("\").ToArray()
                    If IO.File.Exists(item) Then
                        Array.Resize(tempArray2, tempArray2.Length - 1)
                    End If
                    DeleteDirIfEmpty(Join(tempArray2, "\"))
                Next
            Next


            'REGISTRY RESTORATION STUFF
            ' SYSTEM-WIDE
            ' Delete Swatches entirely again
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg DELETE ""HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches"" /f", AppWinStyle.Hide, True)
            doneTasks += 1

            If forCustomise = False Then
                ' Delete Getting Started data
                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg DELETE ""HKLM\SOFTWARE\Win8To7GS"" /f", AppWinStyle.Hide, True)
            End If

            ' Restore all settings first without deletions
            If RestoreRegistryAll("HKLM") = False Then
                Exit Sub
            End If

            ' Restore registry bit by bit
            For Each key In regtweaks.SystemTweaks.Item("Delete")
                If RestoreRegistry(key.Item(0), key.Item(1), False) = False Then
                    Exit Sub
                End If
                doneTasks += 1
            Next
            For Each key In regtweaks.SystemTweaks.Item("DWORD")
                If RestoreRegistry(key.Item(0), key.Item(1), False) = False Then
                    Exit Sub
                End If
                doneTasks += 1
            Next
            For Each key In regtweaks.SystemTweaks.Item("Binary")
                If RestoreRegistry(key.Item(0), key.Item(1), False) = False Then
                    Exit Sub
                End If
                doneTasks += 1
            Next
            For Each key In regtweaks.SystemTweaks.Item("String")
                If key.Item(1) = "/ve" Then 'If for (Default) value...
                    If RestoreRegistry(key.Item(0), "(Default)", False) = False Then
                        Exit Sub
                    End If
                Else
                    If RestoreRegistry(key.Item(0), key.Item(1), False) = False Then
                        Exit Sub
                    End If
                End If
                doneTasks += 1
            Next
            For Each key In regtweaks.SystemTweaks.Item("MultiString")
                If RestoreRegistry(key.Item(0), key.Item(1), False) = False Then
                    Exit Sub
                End If
                doneTasks += 1
            Next
            If System.Environment.OSVersion.Version.Minor = 3 And forCustomise = False Then 'Refer to Form1's reasoning for not in 8.0.
                'Undo autorun for 7+ Taskbar Tweaker
                If RestoreRegistry("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Run", "7 Taskbar Tweaker", False) = False Then
                    Exit Sub
                End If
            End If


            ' USER-WIDE
            'Unload HKLM\UserConfig first if loaded
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg unload ""HKLM\UserConfigClasses""", AppWinStyle.Hide, True)
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg unload ""HKLM\UserConfig""", AppWinStyle.Hide, True)
            Thread.Sleep(400)

            'For each Windows user, load NTUSER.DAT, undo changes, and unload their NTUSER.DAT
            For Each loopdirinfo In dirArr
                If Not IO.File.Exists(loopdirinfo.FullName + "\NTUSER.DAT") Then
                    Continue For 'Skip folder if not a Windows user or Default User Skeleton
                End If
                If loopdirinfo.Name = "All Users" Or loopdirinfo.Name = "Default User" Then
                    Continue For 'Skip symlinks
                End If


                'DELETE FILES THAT HAVE NO BACKUPS
                For Each item In knownUserFileChanges
                    'Check that there isn't a backup - if there is, we'll handle it later
                    If IO.File.Exists(loopdirinfo.FullName + storagelocationuser + "\Backups\" + item) Then
                        Continue For
                    End If

                    'Delete the new file or folder
                    If IO.File.Exists(loopdirinfo.FullName + "\" + item) Then
                        IO.File.Delete(loopdirinfo.FullName + "\" + item)
                    ElseIf IO.Directory.Exists(loopdirinfo.FullName + "\" + item) Then
                        tries = 0
                        While Not tries = 10
                            Shell(windir + "\" + sysprefix + "\cmd.exe /c del """ + loopdirinfo.FullName + "\" + item + """ /s /q /f /a", AppWinStyle.Hide, True)
                            Shell(windir + "\" + sysprefix + "\cmd.exe /c rd """ + loopdirinfo.FullName + "\" + item + """ /s /q", AppWinStyle.Hide, True)
                            If Not IO.Directory.Exists(loopdirinfo.FullName + "\" + item) Then
                                Exit While
                            End If
                            tries += 1
                        End While
                        If IO.Directory.Exists(loopdirinfo.FullName + "\" + item) Then
                            ErrorOccurred("Failed to delete directory created by transformation: " + item)
                            Exit Sub
                        End If
                    End If

                    doneTasks += 1 'Add 1 to doneTasks
                Next


                'RESTORE THE REMAINING FILES
                If IO.Directory.Exists(loopdirinfo.FullName + storagelocationuser + "\Backups") Then 'Safeguard to prevent failure
                    directoriesList = ListDirectory(loopdirinfo.FullName + storagelocationuser + "\Backups")
                    For Each item In directoriesList
                        fiArr = New IO.DirectoryInfo(item).GetFiles() 'Get the files in this directory
                        For Each loopfileinfo In fiArr
                            If IO.File.Exists(loopfileinfo.FullName.Replace(loopdirinfo.FullName + storagelocationuser + "\Backups\", loopdirinfo.FullName + "\")) Then
                                'Delete the new file
                                IO.File.Delete(loopfileinfo.FullName.Replace(loopdirinfo.FullName + storagelocationuser + "\Backups\", loopdirinfo.FullName + "\"))
                            End If
                            'Move original file to its original location
                            If MoveFile(loopfileinfo.FullName, loopfileinfo.FullName.Replace(loopdirinfo.FullName + storagelocationuser + "\Backups\", loopdirinfo.FullName + "\"), "remaining user files restoration", False) = False Then
                                Exit Sub
                            End If

                            doneTasks += 1 'Add 1 to doneTasks
                        Next
                    Next
                End If


                'DELETE FOLDERS INTENTIONALLY CREATED BY THE TRANSFORMATION
                For Each item In knownUserFolderChanges
                    tries = 0
                    While Not tries = 10
                        Shell(windir + "\" + sysprefix + "\cmd.exe /c del """ + loopdirinfo.FullName + "\" + item + """ /s /q /f /a", AppWinStyle.Hide, True)
                        Shell(windir + "\" + sysprefix + "\cmd.exe /c rd """ + loopdirinfo.FullName + "\" + item + """ /s /q", AppWinStyle.Hide, True)
                        If Not IO.Directory.Exists(loopdirinfo.FullName + "\" + item) Then
                            Exit While
                        End If
                        tries += 1
                    End While
                    If IO.Directory.Exists(loopdirinfo.FullName + "\" + item) Then
                        ErrorOccurred("Failed to delete directory created by transformation: " + item)
                        Exit Sub
                    End If
                Next


                'DELETE EMPTIED FOLDERS CREATED DURING TRANSFORMATION
                For Each loopy In {knownUserFileChanges, knownUserFolderChanges}
                    tempArray = loopy
                    Array.Sort(tempArray)
                    Array.Reverse(tempArray) 'Do it reverse alphabetical so that the longest paths happen before the shortest ones

                    For Each item In tempArray
                        tempArray2 = item.Split("\").ToArray()
                        If IO.File.Exists(loopdirinfo.FullName + "\" + item) Then
                            Array.Resize(tempArray2, tempArray2.Length - 1)
                        End If
                        DeleteDirIfEmpty(loopdirinfo.FullName + "\" + Join(tempArray2, "\"))
                    Next
                Next


                'Load the NTUSER.DAT file to HKLM\UserConfig, and UsrClass.dat file to HKLM\UserConfigClasses too
                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg load ""HKLM\UserConfig"" """ + loopdirinfo.FullName + "\NTUSER.DAT""", AppWinStyle.Hide, True)
                If IO.File.Exists(loopdirinfo.FullName + "\AppData\Local\Microsoft\Windows\UsrClass.dat") Then
                    Shell(windir + "\" + sysprefix + "\cmd.exe /c reg load ""HKLM\UserConfigClasses"" """ + loopdirinfo.FullName + "\AppData\Local\Microsoft\Windows\UsrClass.dat""", AppWinStyle.Hide, True)
                End If

                ' Restore all settings first without deletions
                If RestoreRegistryAll("HKLM\UserConfig") = False Then
                    Exit Sub
                End If
                If IO.File.Exists(loopdirinfo.FullName + "\AppData\Local\Microsoft\Windows\UsrClass.dat") Then
                    If RestoreRegistryAll("HKLM\UserConfigClasses") = False Then
                        Exit Sub
                    End If
                End If

                ' Then tackle remaining deletions
                For Each key In regtweaks.UserTweaks.Item("Delete")
                    If RestoreRegistry(key.Item(0).Replace("HKCU\", "HKLM\UserConfig\"), key.Item(1), False) = False Then
                        Exit Sub
                    End If
                    doneTasks += 1
                Next
                For Each key In regtweaks.UserTweaks.Item("DWORD")
                    If RestoreRegistry(key.Item(0).Replace("HKCU\", "HKLM\UserConfig\"), key.Item(1), False) = False Then
                        Exit Sub
                    End If
                    doneTasks += 1
                Next
                For Each key In regtweaks.UserTweaks.Item("Binary")
                    If RestoreRegistry(key.Item(0).Replace("HKCU\", "HKLM\UserConfig\"), key.Item(1), False) = False Then
                        Exit Sub
                    End If
                    doneTasks += 1
                Next
                For Each key In regtweaks.UserTweaks.Item("String")
                    If key.Item(1) = "/ve" Then 'If for (Default) value...
                        If RestoreRegistry(key.Item(0).Replace("HKCU\", "HKLM\UserConfig\"), "(Default)", False) = False Then
                            Exit Sub
                        End If
                    Else
                        If RestoreRegistry(key.Item(0).Replace("HKCU\", "HKLM\UserConfig\"), key.Item(1), False) = False Then
                            Exit Sub
                        End If
                    End If
                    doneTasks += 1
                Next
                For Each key In regtweaks.UserTweaks.Item("MultiString")
                    If RestoreRegistry(key.Item(0).Replace("HKCU\", "HKLM\UserConfig\"), key.Item(1), False) = False Then
                        Exit Sub
                    End If
                    doneTasks += 1
                Next
                If IO.File.Exists(loopdirinfo.FullName + "\AppData\Local\Microsoft\Windows\UsrClass.dat") Then
                    For Each key In regtweaks.UserClassTweaks.Item("Delete")
                        If RestoreRegistry(key.Item(0).Replace("HKCU\Software\Classes\", "HKLM\UserConfigClasses\"), key.Item(1), False) = False Then
                            Exit Sub
                        End If
                        doneTasks += 1
                    Next
                    For Each key In regtweaks.UserClassTweaks.Item("DWORD")
                        If RestoreRegistry(key.Item(0).Replace("HKCU\Software\Classes\", "HKLM\UserConfigClasses\"), key.Item(1), False) = False Then
                            Exit Sub
                        End If
                        doneTasks += 1
                    Next
                    For Each key In regtweaks.UserClassTweaks.Item("Binary")
                        If RestoreRegistry(key.Item(0).Replace("HKCU\Software\Classes\", "HKLM\UserConfigClasses\"), key.Item(1), False) = False Then
                            Exit Sub
                        End If
                        doneTasks += 1
                    Next
                    For Each key In regtweaks.UserClassTweaks.Item("String")
                        If key.Item(1) = "/ve" Then 'If for (Default) value...
                            If RestoreRegistry(key.Item(0).Replace("HKCU\Software\Classes\", "HKLM\UserConfigClasses\"), "(Default)", False) = False Then
                                Exit Sub
                            End If
                        Else
                            If RestoreRegistry(key.Item(0).Replace("HKCU\Software\Classes\", "HKLM\UserConfigClasses\"), key.Item(1), False) = False Then
                                Exit Sub
                            End If
                        End If
                        doneTasks += 1
                    Next
                    For Each key In regtweaks.UserClassTweaks.Item("MultiString")
                        If RestoreRegistry(key.Item(0).Replace("HKCU\Software\Classes\", "HKLM\UserConfigClasses\"), key.Item(1), False) = False Then
                            Exit Sub
                        End If
                        doneTasks += 1
                    Next

                    'Delete Getting Started configs
                    Shell(windir + "\" + sysprefix + "\cmd.exe /c reg DELETE ""HKLM\UserConfig\Software\Win8To7GS"" /f", AppWinStyle.Hide, True)
                End If
                Thread.Sleep(400)

                If System.Environment.OSVersion.Version.Minor = 3 Then
                    'While we're here, remove the Libraries' Public folders-symlinks (rmdir only removes empty-folders) 
                    Shell(windir + "\" + sysprefix + "\cmd.exe /c rmdir """ + loopdirinfo.FullName + "\Music\Sample Music""", AppWinStyle.Hide, True)
                    Shell(windir + "\" + sysprefix + "\cmd.exe /c rmdir """ + loopdirinfo.FullName + "\Pictures\Sample Pictures""", AppWinStyle.Hide, True)
                    Shell(windir + "\" + sysprefix + "\cmd.exe /c rmdir """ + loopdirinfo.FullName + "\Videos\Sample Videos""", AppWinStyle.Hide, True)
                End If

                'Finally, unload HKLM\UserConfig once again
                If IO.File.Exists(loopdirinfo.FullName + "\AppData\Local\Microsoft\Windows\UsrClass.dat") Then
                    Shell(windir + "\" + sysprefix + "\cmd.exe /c reg unload ""HKLM\UserConfigClasses""", AppWinStyle.Hide, True)
                End If
                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg unload ""HKLM\UserConfig""", AppWinStyle.Hide, True)
            Next
            If forCustomise = True Then
                jobthread.Abort() 'Stop the progress thread
                ChangeProgress(100)
            End If


            'CLEANUP
            'Delete lock screen background cache
            Shell(windir + "\" + sysprefix + "\cmd.exe /c del " + windrive + "ProgramData\Microsoft\Windows\SystemData\* /s /q /f", AppWinStyle.Hide, True)
            'Delete icon caches for all users
            dirArr = New IO.DirectoryInfo(windrive + "Users").GetDirectories() 'Required for getting list of directories in directory to loop through
            For Each loopdirinfo In dirArr
                If Not IO.Directory.Exists(loopdirinfo.FullName + "\AppData\Local\Microsoft\Windows\Explorer") Then
                    Continue For 'Skip folder if not a Windows user or Default User Skeleton
                End If

                fiArr = New IO.DirectoryInfo(loopdirinfo.FullName + "\AppData\Local\Microsoft\Windows\Explorer").GetFiles() 'Get the files in this directory
                For Each loopfileinfo In fiArr
                    If loopfileinfo.Name.StartsWith("iconcache") Then
                        Try
                            IO.File.Delete(loopfileinfo.FullName)
                        Catch ex As Exception
                            Shell(windir + "\" + sysprefix + "\cmd.exe /c del " + loopfileinfo.FullName + " /s /q /f", AppWinStyle.Hide, True)
                        End Try
                    End If
                Next
            Next
            If IO.Directory.Exists(windir + "\Resources\Themes\aero\VSCache") Then
                'Delete the existing VSCaches and restore old ones (MUST BE DONE HERE OR WINDOWS WILL BRICK)
                fiArr = New IO.DirectoryInfo(windir + "\Resources\Themes\aero\VSCache").GetFiles() 'Get current VSCaches
                For Each loopfileinfo In fiArr
                    Try 'Delete modified VS Caches
                        IO.File.Delete(loopfileinfo.FullName)
                    Catch ex As Exception
                        ErrorOccurred("Failed to delete modified Visual Style Cache " + loopfileinfo.FullName + ": " + ex.ToString())
                        Exit Sub
                    End Try
                Next
            Else
                Try
                    IO.Directory.CreateDirectory(windir + "\Resources\Themes\aero\VSCache")
                Catch ex As Exception
                    ErrorOccurred("Failed to make VSCache folder for unmodified Visual Style Caches to move back to: " + ex.ToString())
                End Try
            End If
            fiArr = New IO.DirectoryInfo(storagelocation + "\VSCaches").GetFiles() 'Get original VSCaches
            For Each loopfileinfo In fiArr
                Try 'Restore original VS Caches
                    IO.File.Move(loopfileinfo.FullName, windir + "\Resources\Themes\aero\VSCache\" + loopfileinfo.Name)
                Catch ex As Exception
                    ErrorOccurred("Failed to move original Visual Style Cache " + loopfileinfo.FullName + " to " + windir + "\Resources\Themes\aero\VSCache\" + loopfileinfo.Name + ": " + ex.ToString())
                    Exit Sub
                End Try
            Next


            'Just in case, copy shell32a.dll over shell32.dll for the next step
            If IO.File.Exists(windir + "\Temp\shell32.dll.bak") Then
                Try
                    IO.File.Delete(windir + "\Temp\shell32.dll.bak")
                Catch ex As Exception
                    ErrorOccurred("Failed to make room for moving modified shell32.dll to the Temp folder" + ": " + ex.ToString())
                    Exit Sub
                End Try
            End If
            Try
                IO.File.Move(windir + "\" + sysprefix + "\shell32.dll", windir + "\Temp\shell32.dll.bak")
            Catch ex As Exception
                ErrorOccurred("Failed to move modified file " + windir + "\" + sysprefix + "\shell32.dll" + " to " + windir + "\Temp\shell32.dll.bak" + ": " + ex.ToString())
                Exit Sub
            End Try
            Try
                IO.File.Copy(windir + "\" + sysprefix + "\shell32a.dll", windir + "\" + sysprefix + "\shell32.dll", True)
            Catch ex As Exception
                ErrorOccurred("Failed to copy original file " + windir + "\" + sysprefix + "\shell32a.dll" + " to " + windir + "\" + sysprefix + "\shell32.dll" + ": " + ex.ToString())
                Exit Sub
            End Try

            If forCustomise = False Then
                'Restore SHELL32.DLL dependency defaults
                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\ControlSet001\Control\Session Manager\KnownDLLs"" /v SHELL32 /t REG_SZ /d shell32.dll /f", AppWinStyle.Hide, True)

                'Copy setup executable to TEMP folder for final stage
                Try
                    IO.File.Copy(storagelocation + "\SetupTools\setup.exe", windir + "\Temp\win8to7restore.exe", True)
                Catch ex As Exception
                    ErrorOccurred("Failed to copy Restoration Executable to the Temp folder for the next stage of restoration" + ": " + ex.ToString())
                    Exit Sub
                End Try
            End If

            If forCustomise = True Then
                CustomisationCleanup("") 'Change status to cleanup status
            End If

            'Prevent softlocking as easily by automatically pressing SPACE if needed to push a button
            Dim jobthread2 As Thread

            'Uninstall programs that're installed
            If forCustomise = False Then
                'Uninstall 7TaskbarTweaker (we'll delete the remaining keys, etc. later)
                If IO.File.Exists(windrive + "Program Files\7+ Taskbar Tweaker\uninstall.exe") Then
                    Shell("taskkill /f /im ""7+ Taskbar Tweaker.exe""", AppWinStyle.Hide, True)
                    Shell(windrive + "Program Files\7+ Taskbar Tweaker\uninstall.exe /S /D=" + windrive + "Program Files\7+ Taskbar Tweaker", AppWinStyle.NormalFocus, True, 2400000)
                End If
                If IO.File.Exists(windrive + "AeroGlass\unins000.exe") Then
                    'Uninstall Glass8
                    Shell(windir + "\" + sysprefix + "\cmd.exe /c reg DELETE ""HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{277BA0F1-D0BB-4D73-A2DF-6B60C91E1533}_is1"" /v ""SystemComponent"" /f", AppWinStyle.Hide, True)
                    Shell(windrive + "AeroGlass\unins000.exe /VERYSILENT /CLOSEAPPLICATIONS /NORESTART /SUPPRESSMSGBOXES", AppWinStyle.NormalFocus, True, 2400000)
                End If
            End If
            If IO.Directory.Exists(windir + "\" + sysprefix + "\OldNewExplorer") Then
                'Uninstall OldNewExplorer
                Shell(windir + "\" + sysprefix + "\cmd.exe /c regsvr32 /u " + windir + "\System32\OldNewExplorer\OldNewExplorer32.dll /s", AppWinStyle.NormalFocus, True, 2400000)
                Shell(windir + "\" + sysprefix + "\cmd.exe /c regsvr32 /u " + windir + "\System32\OldNewExplorer\OldNewExplorer64.dll /s", AppWinStyle.NormalFocus, True, 2400000)
            End If
            If IO.File.Exists(windir + "\explorer7\ex7forw8.exe") Then
                'Uninstall Ex7ForWin8, if selected
                If Environment.Is64BitOperatingSystem = True Then
                    Shell(windir + "\" + sysprefix + "\cmd.exe /c reg DELETE ""HKLM\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\Ex7forW8"" /v ""SystemComponent"" /f", AppWinStyle.Hide, True)
                Else
                    Shell(windir + "\" + sysprefix + "\cmd.exe /c reg DELETE ""HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Ex7forW8"" /v ""SystemComponent"" /f", AppWinStyle.Hide, True)
                End If
                Shell(windir + "\explorer7\ex7forw8.exe /uninstall /silent", AppWinStyle.NormalFocus, True, 2400000)
            End If

            'Uninstall Open Shell
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg DELETE ""HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{F4B6EE58-F183-4B0D-930B-4480673C0F5B}"" /v ""SystemComponent"" /f", AppWinStyle.Hide, True)
            Shell(windir + "\" + sysprefix + "\msiexec.exe /X{F4B6EE58-F183-4B0D-930B-4480673C0F5B} /passive /norestart", AppWinStyle.NormalFocus, True, 2400000)

            If IO.File.Exists(windrive + "Program Files\Microsoft Games\unwin7games.exe") Then
                'Uninstall 7GamesPack
                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg DELETE ""HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Win7Games"" /v ""SystemComponent"" /f", AppWinStyle.Hide, True)
                Shell(windrive + "Program Files\Microsoft Games\unwin7games.exe /S", AppWinStyle.NormalFocus, True, 2400000)
            End If
            If IO.File.Exists(windrive + "Program Files\ClassicTaskmgr\unins000.exe") Then
                'Uninstall Classic Task Manager
                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg DELETE ""HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Classic Task Manager+msconfig for Win 10 and 8_is1"" /v ""SystemComponent"" /f", AppWinStyle.Hide, True)
                Shell(windrive + "Program Files\ClassicTaskmgr\unins000.exe /VERYSILENT /CLOSEAPPLICATIONS /NORESTART /SUPPRESSMSGBOXES", AppWinStyle.NormalFocus, True, 2400000)
            End If
            If IO.File.Exists(windir + "\Installer\Desktop Gadgets\unins000.exe") Then
                'Uninstall Windows Desktop Gadgets (using an alternative running method because Shell seems to quit it before it has completed sometimes)
                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg DELETE ""HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Windows Desktop Gadgets_is1"" /v ""SystemComponent"" /f", AppWinStyle.Hide, True)

                startInfo = New System.Diagnostics.ProcessStartInfo
                startInfo.FileName = windir + "\Installer\Desktop Gadgets\unins000.exe"
                startInfo.Arguments = "/VERYSILENT /CLOSEAPPLICATIONS /NORESTART /SUPPRESSMSGBOXES"
                startInfo.CreateNoWindow = True
                startInfo.UseShellExecute = True
                startInfo.WindowStyle = ProcessWindowStyle.Hidden

                MyProcess = Process.Start(startInfo)
                MyProcess.WaitForExit()
            End If
            'Uninstall Windows Media Center
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg DELETE ""HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{99CCD11D-435B-4662-A48C-3AC046EC7014}"" /v ""SystemComponent"" /f", AppWinStyle.Hide, True)
            Shell(windir + "\" + sysprefix + "\msiexec.exe /X{99CCD11D-435B-4662-A48C-3AC046EC7014} /passive /norestart", AppWinStyle.NormalFocus, True, 2400000)

            'Keep Explorer from running after SIB+ and co. is uninstalled
            jobthread2 = New Thread(AddressOf KillExplorer)
            jobthread2.Start()

            'Uninstall StartIsBack
            If Environment.Is64BitOperatingSystem = True Then
                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg DELETE ""HKLM\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\StartIsBack"" /v ""SystemComponent"" /f", AppWinStyle.Hide, True)
            Else
                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg DELETE ""HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\StartIsBack"" /v ""SystemComponent"" /f", AppWinStyle.Hide, True)
            End If
            If IO.File.Exists(windrive + "Program Files (x86)\StartIsBack\StartIsBackCfg.exe") Then
                Shell(windrive + "Program Files (x86)\StartIsBack\StartIsBackCfg.exe /uninstall", AppWinStyle.NormalFocus, False)
            ElseIf IO.File.Exists(windrive + "Program Files\StartIsBack\StartIsBackCfg.exe") Then
                Shell(windrive + "Program Files\StartIsBack\StartIsBackCfg.exe /uninstall", AppWinStyle.NormalFocus, False)
            End If
            Thread.Sleep(4000)
            My.Computer.Keyboard.SendKeys(" ", True)

            If forCustomise = False Then
                'Boot into installer once again for Phase 4 with the Setup Mode
                HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("CurrentPhase", 99)

                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v CmdLine /d " + windir + "\Temp\win8to7restore.exe" + " /t REG_SZ /f", AppWinStyle.Hide, True)
            Else
                'Do some extra cleanup with Phase 4
                Phase4()

                'Go to Stage 103 (Customisation Stage 3)
                HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).SetValue("CurrentPhase", 103)

                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v CmdLine /d " + storagelocation + "\SetupTools\setup.exe" + " /t REG_SZ /f", AppWinStyle.Hide, True)
            End If
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v OOBEInProgress /t REG_DWORD /d 1 /f", AppWinStyle.Hide, True)
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SetupPhase /t REG_DWORD /d 4 /f", AppWinStyle.Hide, True)
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SetupType /t REG_DWORD /d 2 /f", AppWinStyle.Hide, True)
            Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SystemSetupInProgress /t REG_DWORD /d 1 /f", AppWinStyle.Hide, True)
            Thread.Sleep(8000) 'Give time for programs to finish uninstalling
            If forCustomise = True Then
                AboutToRestart("")
            End If
            jobthread2.Abort()
            RestartTime("")
        Catch ex As Exception
            ErrorOccurred(ex.ToString())
            Exit Sub
        End Try
    End Sub
#End Region
#Region "Stage 4 of 4"
    Sub Phase4() 'When done in Customising-mode, this is only partial for partial-uninstalls
        Try
            Dim SMSetting As String = HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("Start")
            Dim UXThemePatcherAllowed As String = HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("AllowUXThemePatcher")
            Dim SevenGamesAllowed As String = HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("Allow7Games")
            Dim SevenTaskManagerAllowed As String = HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("Allow7TaskManager")
            Dim SevenGadgetsAllowed As String = HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("Allow7Gadgets")

            Dim dirArr As IO.DirectoryInfo() = New IO.DirectoryInfo(windrive + "Users").GetDirectories() 'Required for getting list of directories in directory to loop through
            Dim loopdirinfo As IO.DirectoryInfo 'for the loop below
            Dim programdirstodelete As New List(Of String)
            Dim tries As Integer

            If forCustomise = False Then
                'Make sure Windows is still in Setup Mode, in case an unexpected shutdown suddenly occurs
                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v CmdLine /d " + windir + "\Temp\win8to7restore.exe" + " /t REG_SZ /f", AppWinStyle.Hide, True)
                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v OOBEInProgress /t REG_DWORD /d 1 /f", AppWinStyle.Hide, True)
                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SetupPhase /t REG_DWORD /d 4 /f", AppWinStyle.Hide, True)
                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SetupType /t REG_DWORD /d 2 /f", AppWinStyle.Hide, True)
                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SystemSetupInProgress /t REG_DWORD /d 1 /f", AppWinStyle.Hide, True)
            End If

            'Delete StartIsBack leftover thingy
            If HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("Start") = "startisback" Then
                If IO.File.Exists(windrive + "remove.cmd") Then
                    Try
                        IO.File.Delete(windrive + "remove.cmd")
                    Catch
                    End Try
                End If
            End If

            If forCustomise = False Then 'Don't do this portion if updating the transformation
                'Undo the anti-bricking setup, now it's no longer needed
                If IO.File.Exists(windir + "\" + sysprefix + "\shell32a.dll") Then
                    Try
                        IO.File.Delete(windir + "\" + sysprefix + "\shell32a.dll")
                    Catch ex As Exception
                        ErrorOccurred("Failed to delete shell32a.dll - perhaps the workaround is still active even though it shouldn't be?" + ": " + ex.ToString())
                        Exit Sub
                    End Try
                End If
                'Then remove shell32.dll.bak too
                If IO.File.Exists(windir + "\Temp\shell32.dll.bak") Then
                    Try
                        IO.File.Delete(windir + "\Temp\shell32.dll.bak")
                    Catch 'Ignore this failure too
                    End Try
                End If


                If UXThemePatcherAllowed = "true" Then
                    'Uninstall UXThemePatcher
                    If Environment.Is64BitOperatingSystem = True Then
                        Shell(windir + "\" + sysprefix + "\cmd.exe /c reg DELETE ""HKLM\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\UltraUXThemePatcher"" /v ""SystemComponent"" /f", AppWinStyle.Hide, True)
                    Else
                        Shell(windir + "\" + sysprefix + "\cmd.exe /c reg DELETE ""HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\UltraUXThemePatcher"" /v ""SystemComponent"" /f", AppWinStyle.Hide, True)
                    End If
                    If IO.File.Exists(windrive + "Program Files (x86)\UltraUXThemePatcher\Uninstall.exe") Then
                        Shell(windrive + "Program Files (x86)\UltraUXThemePatcher\Uninstall.exe /S /NORESTART", AppWinStyle.NormalFocus, False)
                    ElseIf IO.File.Exists(windrive + "Program Files\UltraUXThemePatcher\Uninstall.exe") Then
                        Shell(windrive + "Program Files\UltraUXThemePatcher\Uninstall.exe /S /NORESTART", AppWinStyle.NormalFocus, False)
                    End If

                    'If UXThemePatcher is installed by us, delete its dropped shortcuts
                    For Each loopdirinfo In dirArr
                        If IO.Directory.Exists(loopdirinfo.FullName + "\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\UltraUXThemePatcher") Then
                            programdirstodelete.Add(loopdirinfo.FullName + "\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\UltraUXThemePatcher")
                        End If
                    Next
                End If
                Thread.Sleep(6000)
                My.Computer.Keyboard.SendKeys(" ", True)
            End If


            'DELETE REGISTRY BACKUPS
            If forCustomise = False Then
                ' SYSTEM-WIDE
                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg DELETE ""HKLM\SOFTWARE\Win8To7"" /f", AppWinStyle.Hide, True)
                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg DELETE ""HKLM\SOFTWARE\Wow6432Node\Win8To7"" /f", AppWinStyle.Hide, True)
            Else
                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg DELETE ""HKLM\SOFTWARE\Win8To7\RegBackup"" /f", AppWinStyle.Hide, True)
                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg DELETE ""HKLM\SOFTWARE\Wow6432Node\Win8To7\RegBackup"" /f", AppWinStyle.Hide, True)
            End If

            ' USER-WIDE
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

                'Load the NTUSER.DAT file to HKLM\UserConfig
                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg load ""HKLM\UserConfig"" """ + loopdirinfo.FullName + "\NTUSER.DAT""", AppWinStyle.Hide, True)

                'Delete the user's backups
                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg DELETE ""HKLM\UserConfig\Software\Win8To7"" /f", AppWinStyle.Hide, True)
                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg DELETE ""HKLM\UserConfig\Software\Wow6432Node\Win8To7"" /f", AppWinStyle.Hide, True)
                Thread.Sleep(400)

                'Finally, unload HKLM\UserConfig once again
                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg unload ""HKLM\UserConfig""", AppWinStyle.Hide, True)

                '...and delete our backups
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

            If forCustomise = False Then 'During customisation, it happens in Stage 3, not here
                'DELETE FILES AND PROGRAM-LEFTOVERS
                programdirstodelete.Add(storagelocation.ToString())
                programdirstodelete.Add(windir + "\System32\OldNewExplorer")
                programdirstodelete.Add(windir + "\System32\Win8To7")
                programdirstodelete.Add(windrive + "AeroGlass")
                programdirstodelete.Add(windrive + "Program Files\7+ Taskbar Tweaker")
                If UXThemePatcherAllowed = "true" Then
                    programdirstodelete.Add(windrive + "Program Files (x86)\UltraUXThemePatcher")
                    programdirstodelete.Add(windrive + "Program Files\UltraUXThemePatcher")
                End If
                If SevenGamesAllowed = "true" Then
                    programdirstodelete.Add(windrive + "Program Files\Microsoft Games")
                    Shell(windir + "\" + sysprefix + "\cmd.exe /c del """ + windrive + "ProgramData\Microsoft\Windows\Start Menu\Programs\Games\GameExplorer.lnk"" /s /q /f /a", AppWinStyle.Hide, True) 'Delete the remaining shortcut it left
                End If
                If SevenTaskManagerAllowed = "true" Then
                    programdirstodelete.Add(windrive + "Program Files\ClassicTaskmgr")
                End If
                If SevenGadgetsAllowed = "true" Then
                    programdirstodelete.Add(windrive + "Program Files (x86)\Windows Sidebar")
                    programdirstodelete.Add(windrive + "Program Files\Windows Sidebar")
                End If
                If SMSetting = "ex7forwin8" Then
                    programdirstodelete.Add(windir + "\explorer7")
                End If
                If SMSetting = "startisback" Then
                    programdirstodelete.Add(windrive + "Program Files (x86)\StartIsBack")
                    programdirstodelete.Add(windrive + "Program Files\StartIsBack")
                End If
                If SMSetting = "openshell" Then
                    programdirstodelete.Add(windrive + "Program Files\Open-Shell")
                End If

                For Each direc In programdirstodelete
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

                'Remove uninstaller from control
                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg DELETE ""HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Win8To7TransformationPack"" /f", AppWinStyle.Hide, True)

                'Go back into Windows now it's all complete.
                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v CmdLine /t REG_SZ /f", AppWinStyle.Hide, True)
                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v OOBEInProgress /t REG_DWORD /d 0 /f", AppWinStyle.Hide, True)
                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SetupPhase /t REG_DWORD /d 0 /f", AppWinStyle.Hide, True)
                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SetupType /t REG_DWORD /d 0 /f", AppWinStyle.Hide, True)
                Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD ""HKLM\SYSTEM\Setup"" /v SystemSetupInProgress /t REG_DWORD /d 0 /f", AppWinStyle.Hide, True)

                RestartTime("")
            Else
                'Delete now redundant regchange executable to prevent mid-customise clashes
                Shell(windir + "\" + sysprefix + "\cmd.exe /c del """ + storagelocation + "\SetupTools\regchange.exe"" /s /q /f /a", AppWinStyle.Hide, True)
            End If
        Catch ex As Exception
            ErrorOccurred(ex.ToString())
            Exit Sub
        End Try
    End Sub
#End Region

End Class
