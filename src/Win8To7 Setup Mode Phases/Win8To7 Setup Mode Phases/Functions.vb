Module Functions
#Region "API calls - Restart Windows"
    'Refer to RestartClass for whole callset for that
    Sub RestartWindows()
        RestartClass.ExitWindows(RestartOptions.Reboot)
    End Sub
#End Region

#Region "Variables"
    Public windrive = IO.Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.System))
    Public windir As String = System.Environment.GetEnvironmentVariable("WINDIR")
    Public appdataroaming As String = System.Environment.GetEnvironmentVariable("APPDATA")
    Public sysprefix As String
    Public storagelocation As String = windir + "\Win8To7"
    Public storagelocationuser = "\AppData\Local\Win8To7"
    Public HKLMKey32 As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.LocalMachine

    Public ErrorOccurred As Action(Of String)
    Public forCustomise As Boolean = False
    Public rng As System.Random = New System.Random()
    Public lingue As Globalization.CultureInfo = Globalization.CultureInfo.CurrentCulture

    Public regtweaks As New RegistryTweaks
    Public cfgs As New Configs

    Public startInfo As New System.Diagnostics.ProcessStartInfo
    Public MyProcess As Process

    Public isStarter As Boolean = Microsoft.Win32.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "EditionID", "Professional").ToString.StartsWith("Starter")
    Public isHome As Boolean = Microsoft.Win32.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "EditionID", "Professional").ToString.StartsWith("Core")
    Public isEnterprise As Boolean = Microsoft.Win32.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "EditionID", "Professional").ToString.StartsWith("Enterprise")
#End Region

#Region "Init and End"
    Sub New()
        If Environment.Is64BitOperatingSystem = True Then
            sysprefix = "SysNative"
        Else
            sysprefix = "System32"
        End If
        regtweaks.init()
        cfgs.populateItems()
    End Sub

    Function getRandomTEMPfolder()
        Dim result As String = windir + "\TEMP\Win8To7-" + rng.Next(0, 999999999 + 1).ToString()
        While IO.File.Exists(result)
            result = windir + "\TEMP\Win8To7-" + rng.Next(0, 999999999 + 1).ToString()
            If IO.File.Exists(result) Then
                Try 'Anti-softlock
                    IO.File.Delete(result)
                Catch
                End Try
            End If
        End While

        Return result
    End Function

    Sub SetErrorOccurred(ByVal NewErrorOccurred As Action(Of String))
        ErrorOccurred = NewErrorOccurred
    End Sub

    Sub RestartTime(ByVal status As String)
        HKLMKey32.Flush() 'Save changes before restarting
        If Not status = "inwin" Then
            RestartWindows() 'Use kernel APIs to restart Windows
        Else
            Shell(windir + "\" + sysprefix + "\cmd.exe /c bcdedit /deletevalue {current} safeboot", AppWinStyle.Hide, True) 'Exit Safe Mode if we're in Safe Mode
            Shell(windir + "\" + sysprefix + "\cmd.exe /c shutdown /r /t 0 /f", AppWinStyle.Hide, True) 'Restart Windows immediately upon exiting
            End 'Exit the whole program without effort
        End If
    End Sub
#End Region

#Region "Directory Creation and Listing"
    Sub CreateDir(ByVal dirpath As String)
        Dim tempArray As String() = dirpath.Split("\")
        Dim pathToCreate As String = ""
        For Each item2 In tempArray
            If pathToCreate = "" Then
                pathToCreate = item2
            Else
                pathToCreate = pathToCreate + "\" + item2
            End If
            If Not IO.Directory.Exists(pathToCreate) Then
                IO.Directory.CreateDirectory(pathToCreate)
            End If
        Next
    End Sub

    Function DeleteDirIfEmpty(ByVal dirpath As String)
        If Not IO.Directory.Exists(dirpath) Then
            Return True 'Don't bother if the directory doesn't exist
        End If

        If Not New IO.DirectoryInfo(dirpath).GetFiles().Count = 0 Then
            Return True
        End If
        If Not New IO.DirectoryInfo(dirpath).GetDirectories().Count = 0 Then
            Return True
        End If

        Dim tries As Integer = 0
        While Not tries = 10
            Shell(windir + "\" + sysprefix + "\cmd.exe /c del /s /q /f /a """ + dirpath + """", AppWinStyle.Hide, True)
            Shell(windir + "\" + sysprefix + "\cmd.exe /c rd /s /q """ + dirpath + """", AppWinStyle.Hide, True)
            If Not IO.Directory.Exists(dirpath) Then
                Exit While
            End If
            tries += 1
        End While
        If IO.Directory.Exists(dirpath) Then
            ErrorOccurred("Failed to delete empty directory: " + dirpath)
            Return False
        End If
        Return True
    End Function

    Function ListDirectory(ByVal directory As String)
        Dim directoriesList As New List(Of String) 'List of all directories in FileReplacements
        directoriesList.Add(directory) 'FileReplacements path for directoriesList to start directories listings from
        Dim newdirectoriesList As New List(Of String) 'We'll use this to check when we're out of directories to list
        Dim directoriesListTemp As New List(Of String) 'REQUIRED to prevent exception during for loop - directoriesList can't be changed mid-forloop
        Dim dirArr As IO.DirectoryInfo() 'Required for getting list of directories in directory to loop through
        Dim loopdirinfo As IO.DirectoryInfo 'Required to obtain FullName


        While True
            If directoriesList.Count = newdirectoriesList.Count Then 'When out of directories, exit loop
                Exit While
            End If

            newdirectoriesList = New List(Of String)
            newdirectoriesList.AddRange(directoriesList) 'Every loop, update newdirectoriesList to current state before updating directoriesList

            directoriesListTemp = New List(Of String)
            directoriesListTemp.AddRange(directoriesList) 'Update directoriesListTemp with current values

            For Each item In directoriesList
                dirArr = New IO.DirectoryInfo(item).GetDirectories() 'Get directories of each item in directoriesList
                For Each loopdirinfo In dirArr
                    If Not directoriesListTemp.Contains(loopdirinfo.FullName) Then
                        directoriesListTemp.Add(loopdirinfo.FullName) 'Add new-found directories to directoriesListTemp
                    End If
                Next
            Next
            directoriesList = New List(Of String)
            directoriesList.AddRange(directoriesListTemp) 'Update directoriesList with new values
        End While

        directoriesList.Sort()
        Return directoriesList
    End Function
#End Region

#Region "File and Directory Manipulation"
    Function MoveFile(ByVal originalpath As String, newpath As String, ByVal jobDescription As String, Optional ByVal copyFile As Boolean = False)
        'e.g.: C:\Windows\System32\imageres.dll, C:\Windows\Win8To7\Backups\Windows\System32\imageres.dll, "file patching - to backups", True
        ' copies the original imageres.dll to backups ready for it being patched in System32 afterwards

        originalpath = ToNative(originalpath) 'WOW32 countermeasure - System32 (x64) = SysNative in WOW32
        newpath = ToNative(newpath) 'WOW32 countermeasure for Restores/Replacements

        Dim tempArray As String()
        tempArray = originalpath.Split("\").ToArray()
        Array.Resize(tempArray, tempArray.Length - 1)
        Dim originalpathprefix As String = Join(tempArray, "\")
        tempArray = newpath.Split("\").ToArray()
        Array.Resize(tempArray, tempArray.Length - 1)
        Dim newpathprefix As String = Join(tempArray, "\")

        If Not IO.File.Exists(originalpath) Then
            Return True 'Don't continue if the file we're moving doesn't exist
        End If

        'Create directories in the newpathprefix folder to store the original files in
        If Not IO.Directory.Exists(newpathprefix) Then
            Try
                CreateDir(newpathprefix)
            Catch ex As Exception
                ErrorOccurred("Failed to create directory (" + jobDescription + "): " + newpathprefix + ": " + ex.ToString())
                Return False
            End Try
        End If

        'Now move the file, or copy if copyFile is True
        Try
            If copyFile = False Then
                IO.File.Delete(newpath)
                IO.File.Move(originalpath, newpath)
            Else
                IO.File.Copy(originalpath, newpath, True)
            End If
        Catch ex As Exception
            If copyFile = False Then
                ErrorOccurred("Failed to move " + originalpath + " to " + newpath + " (" + jobDescription + "): " + ex.ToString())
            Else
                ErrorOccurred("Failed to copy " + originalpath + " to " + newpath + " (" + jobDescription + "): " + ex.ToString())
            End If
            Return False
        End Try
        Return True
    End Function

    Function MoveFolder(ByVal originalpath As String, newpath As String, ByVal jobDescription As String, Optional ByVal copyFolder As Boolean = False)
        'e.g.: C:\ProgramData\Microsoft\Windows\Start Menu\Programs\Accessibility, C:\ProgramData\Microsoft\Windows\Start Menu\Programs\Accessories\Accessibility, "folder movements", False
        ' moves Accessibility to its Windows 7 location in Programs

        originalpath = ToNative(originalpath) 'WOW32 countermeasure - System32 (x64) = SysNative in WOW32
        newpath = ToNative(newpath) 'WOW32 countermeasure for Restores/Replacements

        Dim tempArray As String()
        tempArray = originalpath.Split("\").ToArray()
        Array.Resize(tempArray, tempArray.Length - 1)
        Dim originalpathprefix As String = Join(tempArray, "\")
        tempArray = newpath.Split("\").ToArray()
        Array.Resize(tempArray, tempArray.Length - 1)
        Dim newpathprefix As String = Join(tempArray, "\")

        If Not IO.Directory.Exists(originalpath) Then
            IO.Directory.CreateDirectory(originalpath) 'create the directory so that it exists, at least
        End If

        'Create directories in the newpathprefix folder to store the original files in
        If Not IO.Directory.Exists(newpathprefix) Then
            Try
                CreateDir(newpathprefix)
            Catch ex As Exception
                ErrorOccurred("Failed to create directory (" + jobDescription + "): " + newpathprefix + ": " + ex.ToString())
                Return False
            End Try
        End If

        Try
            'Copy the directory first
            My.Computer.FileSystem.CopyDirectory(originalpath, newpath, True)
            ' and then delete the original folder if we're moving, afterwards
            If copyFolder = False Then
                Dim tries As Integer = 0
                While Not tries = 10
                    Shell(windir + "\" + sysprefix + "\cmd.exe /c del /s /q /f /a """ + originalpath + """", AppWinStyle.Hide, True)
                    Shell(windir + "\" + sysprefix + "\cmd.exe /c rd /s /q """ + originalpath + """", AppWinStyle.Hide, True)
                    If Not IO.Directory.Exists(originalpath) Then
                        Exit While
                    End If
                    tries += 1
                End While
            End If
        Catch ex As Exception
            If copyFolder = False Then
                ErrorOccurred("Failed to move " + originalpath + " to " + newpath + " (" + jobDescription + "): " + ex.ToString())
            Else
                ErrorOccurred("Failed to copy " + originalpath + " to " + newpath + " (" + jobDescription + "): " + ex.ToString())
            End If
            Return False
        End Try
        Return True
    End Function

    Function MoveUnknown(ByVal originalpath As String, ByVal newpath As String, ByVal jobDescription As String, Optional ByVal Backup As Boolean = True, Optional ByVal backuporigprefix As String = "", Optional ByVal backupnewprefix As String = "")
        Dim directoriesList As List(Of String)
        Dim changesMade As New List(Of String)
        Dim fiArr As IO.FileInfo()
        Dim loopfileinfo As IO.FileInfo

        If backuporigprefix = "" Then
            backuporigprefix = windrive
        End If
        If backupnewprefix = "" Then
            backupnewprefix = storagelocation + "\Backups\"
        End If

        If IO.Directory.Exists(ToNative(originalpath)) Then
            If Backup = True Then
                'Back up original folder
                If MoveFolder(originalpath, originalpath.Replace(backuporigprefix, backupnewprefix), jobDescription + " backups", True) = False Then 'overwrite is off to prevent backups being overwritten
                    changesMade = New List(Of String)
                    changesMade.Add("FAILURE:")
                    Return changesMade 'It failed, and we can't just exit sub via the function, so this is done.
                End If
                'Back up target folder, if it exists
                If MoveFolder(newpath, newpath.Replace(backuporigprefix, backupnewprefix), jobDescription + " target backups", True) = False Then
                    changesMade = New List(Of String)
                    changesMade.Add("FAILURE:")
                    Return changesMade
                End If
            End If

            'Now they're backed up, make a note of the files that'll be changed
            ' ...in the original location
            directoriesList = ListDirectory(originalpath)
            For Each item In directoriesList
                fiArr = New IO.DirectoryInfo(item).GetFiles() 'Get the files in this directory
                For Each loopfileinfo In fiArr
                    If Not changesMade.Contains(loopfileinfo.FullName) Then
                        changesMade.Add(loopfileinfo.FullName)
                    End If
                Next
            Next
            ' ...and the new one
            If IO.Directory.Exists(newpath) Then
                directoriesList = ListDirectory(newpath)
                For Each item In directoriesList
                    fiArr = New IO.DirectoryInfo(item).GetFiles() 'Get the files in this directory
                    For Each loopfileinfo In fiArr
                        If Not changesMade.Contains(loopfileinfo.FullName) Then
                            changesMade.Add(loopfileinfo.FullName)
                        End If
                    Next
                Next
            End If

            'Now move the folder over to the new locations
            If MoveFolder(originalpath, newpath, jobDescription, False) = False Then
                changesMade = New List(Of String)
                changesMade.Add("FAILURE:")
                Return changesMade
            End If
        ElseIf IO.File.Exists(ToNative(originalpath)) Then
            If Backup = True Then
                'Backup file in original location
                If MoveFile(originalpath, originalpath.Replace(backuporigprefix, backupnewprefix), jobDescription + " backups", True) = False Then
                    changesMade = New List(Of String)
                    changesMade.Add("FAILURE:")
                    Return changesMade 'It failed, and we can't just exit sub via the function, so this is done.
                End If
            End If

            'Now move the file
            If MoveFile(originalpath, newpath, jobDescription, False) = False Then
                changesMade = New List(Of String)
                changesMade.Add("FAILURE:")
                Return changesMade
            End If
            If Not changesMade.Contains(newpath) Then
                changesMade.Add(newpath)
            End If
        End If
        Return changesMade
    End Function
#End Region

#Region "Resource/File Extraction"
    Function ExtractEverything()
        'Before we do that, we need to make sure the directories exist
        For Each direc In {storagelocation + "\SetupFiles", storagelocation + "\SetupTools", windir + "\" + sysprefix + "\OldNewExplorer", windir + "\" + sysprefix + "\Win8To7"}
            Try
                CreateDir(direc)
            Catch ex As Exception
                ErrorOccurred("Failed to create directory for programs extraction - " + direc + ": " + ex.ToString())
                Return False
            End Try
        Next

        'Extract remaining installers and setup executables for usage
        Dim targetPath As String
        Dim tempArray As String()
        For Each dictEntry In My.Resources.ResourceManager.GetResourceSet(System.Globalization.CultureInfo.CurrentCulture, True, True) 'Iterate over our Resources:
            If dictEntry.Key.StartsWith("int_") Then ' Skip internal resources
                Continue For
            End If
            If dictEntry.Key.StartsWith("i386_") And Environment.Is64BitOperatingSystem = True Then ' Skip incompatible architectures - 64-Bit
                Continue For
            End If
            If dictEntry.Key.StartsWith("amd64_") And Environment.Is64BitOperatingSystem = False Then ' Skip incompatible architectures - 32-Bit
                Continue For
            End If

            ' Strip architecture identifier from id for next steps
            tempArray = dictEntry.Key.Split("_")
            tempArray = tempArray.Skip(1).ToArray()
            targetPath = String.Join("_", tempArray.ToArray)

            If targetPath.StartsWith("9200_") And System.Environment.OSVersion.Version.Minor = 3 Then ' Skip 8.1-incompatible files
                Continue For
            End If
            If targetPath.StartsWith("9600_") And System.Environment.OSVersion.Version.Minor = 2 Then ' Skip 8.0-incompatible files
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


            ' Now skip depending on Ribbon styling
            If targetPath.StartsWith("DPRibbon_") And Not HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("Ribbon") = "win8" Then '  Skip DP Ribbon if not chosen
                Continue For
            End If
            If targetPath.StartsWith("7Ribbon_") And Not HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("Ribbon") = "win7" Then '  Skip 7 Ribbon if not chosen
                Continue For
            End If
            ' Now skip depending on Start Menu
            If targetPath.StartsWith("SMEx7ForWin8_") And Not HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("Start") = "ex7forwin8" Then '  Skip Ex7ForWin8 files if not chosen
                Continue For
            End If
            If targetPath.StartsWith("SMStartIsBack_") And Not HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("Start") = "startisback" Then '  Skip StartIsBack files if not chosen
                Continue For
            End If
            If targetPath.StartsWith("SMOpenShell_") And Not HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("Start") = "openshell" Then '  Skip Open Shell if not chosen
                Continue For
            End If
            ' Now skip depending on Glass styling
            If targetPath.StartsWith("ShinelessTheme_") And Not HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).GetValue("GlassShine") = "false" Then '  Skip Shineless Theme if not chosen
                Continue For
            End If
            If targetPath.StartsWith("ShineTheme_") And Not HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).GetValue("GlassShine") = "true" Then '  Skip Shiny Theme if not chosen
                Continue For
            End If
            ' Now skip depending on shell32 preference
            If targetPath.StartsWith("AllowShell32_") And Not HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).GetValue("AllowShell32") = "true" Then '  Skip shell32 replacement if not chosen
                Continue For
            End If
            ' Now skip depending on usercpl preference
            If targetPath.StartsWith("AllowUserCPL_") And Not HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).GetValue("AllowUserCPL") = "true" Then '  Skip usercpl replacement if not chosen
                Continue For
            End If
            ' Now skip depending on remaining preferences
            If targetPath.StartsWith("AllowUXThemePatcher_") And Not HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).GetValue("AllowUXThemePatcher") = "true" Then '  Skip UXThemePatcher stuff if not chosen
                Continue For
            End If
            If targetPath.StartsWith("Allow7Games_") And Not HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).GetValue("Allow7Games") = "true" Then '  Skip Win7's Games if not chosen
                Continue For
            End If
            If targetPath.StartsWith("Allow7Gadgets_") And Not HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).GetValue("Allow7Gadgets") = "true" Then '  Skip Win7's Gadgets if not chosen
                Continue For
            End If
            If targetPath.StartsWith("AllowMediaCenter_") And Not HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).GetValue("AllowMediaCenter") = "true" Then '  Skip Media Center if not chosen, and if not on 8.x +MediaCenter SKU
                Continue For
            End If
            If targetPath.StartsWith("Allow7TaskManager_") And Not HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).GetValue("Allow7TaskManager") = "true" Then '  Skip Task Manager if not chosen
                Continue For
            End If
            If targetPath.StartsWith("ReduceWinX_") And Not HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).GetValue("ReduceWinX") = "true" Then '  Skip alt. Win+X if not chosen
                Continue For
            End If
            If targetPath.StartsWith("NotReduceWinX_") And Not HKLMKey32.OpenSubKey("SOFTWARE\Win8To7", True).GetValue("ReduceWinX") = "false" Then '  Skip not alt. Win+X if chosen
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
                'Create directory for extraction, and directory for backup
                targetPath = targetPath.Replace("setupfile:", "")

                'Now copy the new file into the temporary path
                If WriteFileFromResources(dictEntry.Key.ToString(), storagelocation + "\SetupFiles\" + targetPath) = False Then
                    ErrorOccurred("Failed to write setup executables to disk")
                    Return False
                End If
            End If

            'Second, deal with Setup tools
            If targetPath.StartsWith("setuptool:") Then
                'Create directory for extraction
                targetPath = targetPath.Replace("setuptool:", "")

                'Now copy the new file into the temporary path
                If WriteFileFromResources(dictEntry.Key.ToString(), storagelocation + "\SetupTools\" + targetPath) = False Then
                    ErrorOccurred("Failed to write required setup tools to disk")
                    Return False
                End If
            End If

            'Third, deal with OldNewExplorer files
            If targetPath.StartsWith("setupone:") Then
                'Create directory for extraction, and directory for backup
                targetPath = targetPath.Replace("setupone:", "")

                If IO.File.Exists(windir + "\" + sysprefix + "\OldNewExplorer\" + targetPath) Then
                    Continue For
                End If

                'Now copy the new file into the temporary path
                If WriteFileFromResources(dictEntry.Key.ToString(), windir + "\" + sysprefix + "\OldNewExplorer\" + targetPath) = False Then
                    ErrorOccurred("Failed to write OldNewExplorer files to disk")
                    Return False
                End If
            End If

            'Fourth, deal with background process files
            If targetPath.StartsWith("setupbgproc:") Then
                'Create directory for extraction, and directory for backup
                targetPath = targetPath.Replace("setupbgproc:", "")

                If IO.File.Exists(windir + "\" + sysprefix + "\Win8To7\" + targetPath) Then
                    Continue For
                End If

                'Now copy the new file into the temporary path
                If WriteFileFromResources(dictEntry.Key.ToString(), windir + "\" + sysprefix + "\Win8To7\" + targetPath) = False Then
                    Thread.Sleep(10000)
                    ErrorOccurred("Failed to write background processes to disk")
                    Return False
                End If
            End If
        Next
        Return True
    End Function

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

#Region "Automation/Anti Softlock"
    Sub KillExplorer()
        While True
            Shell(windir + "\" + sysprefix + "\cmd.exe /c taskkill /f /im explorer.exe", AppWinStyle.Hide, True)
            Thread.Sleep(400)
        End While
    End Sub
#End Region

#Region "String translations"
    Function ToNative(ByVal path As String)
        Return path.Replace(windir + "\System32", windir + "\" + sysprefix)
    End Function
#End Region
End Module
