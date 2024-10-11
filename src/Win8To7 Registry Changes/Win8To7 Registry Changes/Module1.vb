Module Module1
    Private HKLMKey32 As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.LocalMachine
    Private exfile As String

    Sub Main(args As String())
        Dim Key As String = ""
        Dim Value As String = ""
        Dim LocationKey As String = ""
        Dim Recursive As String = False

        Try
            If args.Length = 6 Then
                Key = args(1)
                Value = args(2)
                LocationKey = args(3)
                exfile = args(4) 'File to write exception messages, if they occur, to
                Boolean.TryParse(args(5), Recursive) 'Sets value of Recursive by converting from String to Boolean
            ElseIf args.Length = 5 Then
                Key = args(1)
                Value = args(2)
                LocationKey = args(3)
                exfile = args(4)
                Recursive = False
            Else
                End
            End If

            If args(0) = "backup" Then
                BackupRegistry(Key, Value, LocationKey, Recursive)
            ElseIf args(0) = "restore" Then
                RestoreRegistry(Key, Value, LocationKey)
            ElseIf args(0) = "restoreall" Then
                RestoreRegistryAll(Key, LocationKey)
            ElseIf args(0) = "deleteifempty" Then
                DeleteEmpty(Key)
            End If
        Catch ex As Exception
            ErrorOccurred("Registry changes encountered an unexpected exception (Key: " + Key + ", Value: " + Value + ", LocationKey: " + LocationKey + ", Recursive: " + Recursive.ToString() + "): " + ex.ToString()) 'Fallback exception-handler in case
        End Try
    End Sub

    Private Sub BackupRegistry(ByVal Key As String, ByVal Value As String, ByVal LocationKey As String, Optional ByVal Recursive As Boolean = False)
        Dim RegistryEdit As Microsoft.Win32.RegistryKey
        Dim KeyPath As String
        Dim tempArray As String() = Key.Split("\") 'Split Key path to obtain prefix
        Dim LocationPath As String

        'Determine the correct Registry for getting original data with
        If tempArray.GetValue(0) = "HKCU" Then 'If HKCU, open CurrentUser for backupping
            RegistryEdit = Microsoft.Win32.Registry.CurrentUser
        ElseIf tempArray.GetValue(0) = "HKU" Then 'If HKU, open Users for backupping
            RegistryEdit = Microsoft.Win32.Registry.Users
        Else 'Otherwise open HKLM for backupping
            RegistryEdit = Microsoft.Win32.Registry.LocalMachine
        End If
        LocationPath = Join(LocationKey.Split("\").Skip(1).ToArray(), "\") 'Get rid of prefix, keep rest as path for HKLMKey32

        'First, make sure the backups area is created
        Dim pathToCreate As String = ""
        tempArray = LocationPath.Split("\")
        For Each item In tempArray
            If pathToCreate = "" Then
                pathToCreate = item 'We need to start from somewhere, and having it begin with \ isn't so favourable
            Else
                pathToCreate = pathToCreate + "\" + item 'Append next key in path so far to path
            End If
            If HKLMKey32.OpenSubKey(pathToCreate) Is Nothing Then 'If it doesn't exist as a key...
                HKLMKey32.CreateSubKey(pathToCreate) 'Make it a key.
            End If
        Next
        tempArray = Key.Split("\").Skip(1).ToArray() 'Get rid of useless prefix from path
        KeyPath = Join(tempArray, "\").ToString() 'Now make it a string path

        'First, make sure the keys exist in our Backups area
        pathToCreate = ""
        For Each item In tempArray
            If pathToCreate = "" Then
                pathToCreate = item
            Else
                pathToCreate = pathToCreate + "\" + item
            End If
            If HKLMKey32.OpenSubKey(LocationPath + "\" + pathToCreate) Is Nothing Then
                HKLMKey32.CreateSubKey(LocationPath + "\" + pathToCreate) 'Repeat the above for the area to back the value(s) to
            End If
        Next

        If Value = "" Then
            'Now, save all values in the key
            If RegistryEdit.OpenSubKey(KeyPath) Is Nothing Then
                Exit Sub 'If the key doesn't exist, we can't back its values up - just quit out
            End If
            Try
                For Each ValueItem In RegistryEdit.OpenSubKey(KeyPath).GetValueNames 'Set value ValueItem of HKLMKey32's backup storage location's key to value of ValueItem on key we're backing up in RegistryEdit
                    HKLMKey32.OpenSubKey(LocationPath + "\" + KeyPath, True).SetValue(ValueItem, RegistryEdit.OpenSubKey(KeyPath).GetValue(ValueItem), RegistryEdit.OpenSubKey(KeyPath).GetValueKind(ValueItem))
                Next
            Catch ex As Exception
                ErrorOccurred("Failed to backup all values of " + Key + " (LocationPath: " + LocationPath + ", KeyPath: " + KeyPath + "): " + ex.ToString())
            End Try
        ElseIf Value = "(Default)" Then
            'Now, save default value in the key
            Try
                If RegistryEdit.OpenSubKey(KeyPath) Is Nothing Then
                    Exit Sub
                End If
                If RegistryEdit.OpenSubKey(KeyPath).GetValue("") Is Nothing Then
                    Exit Sub '"" = Default, and if there is no Default value (e.g.: "(value not set)"), we can't continue - just quit out
                End If 'Set default value of HKLMKey32's backup storage location's key to default value of the key we're backing up in RegistryEdit
                HKLMKey32.OpenSubKey(LocationPath + "\" + KeyPath, True).SetValue("", RegistryEdit.OpenSubKey(KeyPath).GetValue(""), RegistryEdit.OpenSubKey(KeyPath).GetValueKind(""))
            Catch ex As Exception
                ErrorOccurred("Failed to backup '(Default)' value of " + Key + " (LocationPath: " + LocationPath + ", KeyPath: " + KeyPath + "): " + ex.ToString())
            End Try
        Else
            'Now, save the data in the backups area.
            If RegistryEdit.OpenSubKey(KeyPath) Is Nothing Then
                Exit Sub
            End If
            If Not RegistryEdit.OpenSubKey(KeyPath).GetValueNames.Contains(Value) Then
                Exit Sub
            End If
            Try 'Set value Value of HKLMKey32's backup storage location's key to default value of the key we're backing up in RegistryEdit
                HKLMKey32.OpenSubKey(LocationPath + "\" + KeyPath, True).SetValue(Value, RegistryEdit.OpenSubKey(KeyPath).GetValue(Value), RegistryEdit.OpenSubKey(KeyPath).GetValueKind(Value))
            Catch ex As Exception
                ErrorOccurred("Failed to backup '" + Value + "' value of " + Key + " (LocationPath: " + LocationPath + ", KeyPath: " + KeyPath + "): " + ex.ToString())
            End Try
        End If

        'If recursive, iterate over the children of this key
        For Each SubKey In RegistryEdit.OpenSubKey(KeyPath).GetSubKeyNames
            BackupRegistry(Key + "\" + SubKey, Value, LocationKey, Recursive)
        Next
    End Sub


    Private Sub RestoreRegistry(ByVal Key As String, ByVal Value As String, ByVal LocationKey As String)
        Dim RegistryEdit As Microsoft.Win32.RegistryKey
        Dim KeyPath As String
        Dim tempArray As String() = Key.Split("\")
        Dim pathToCreate As String

        'Determine the correct Registry for setting backup's values with
        If tempArray.GetValue(0) = "HKCU" Then
            RegistryEdit = Microsoft.Win32.Registry.CurrentUser
        ElseIf tempArray.GetValue(0) = "HKU" Then
            RegistryEdit = Microsoft.Win32.Registry.Users
        Else
            RegistryEdit = Microsoft.Win32.Registry.LocalMachine
        End If
        KeyPath = Join(Key.Split("\").Skip(1).ToArray(), "\") 'Get rid of useless prefix from path

        Dim LocationPath As String = Join(LocationKey.Split("\").Skip(1).ToArray(), "\") 'Get rid of prefix, keep rest as path for HKCU

        'First, make sure the backup's source still exists
        pathToCreate = ""
        tempArray = KeyPath.Split("\")
        For Each item In tempArray
            If pathToCreate = "" Then
                pathToCreate = item 'We need to start from somewhere, and having it begin with \ isn't so favourable
            Else
                pathToCreate = pathToCreate + "\" + item 'Append next key in path so far to path
            End If
            Try
                If RegistryEdit.OpenSubKey(pathToCreate) Is Nothing Then 'If it doesn't exist as a key...
                    RegistryEdit.CreateSubKey(pathToCreate) 'Make it a key.
                End If
            Catch ex As Exception
                ErrorOccurred("Failed to create key: " + pathToCreate + ": " + ex.ToString())
            End Try
        Next
        tempArray = KeyPath.Split("\").ToArray()

        If Value = "(Default)" Then
            'Now, restore the default value from its backups
            If RegistryEdit.OpenSubKey(KeyPath) Is Nothing Then
                Exit Sub 'If the key we're restoring to doesn't exist, we cannot restore its values - just quit out
            End If

            Try
                'If the key doesn't exist in backups, this means it was created by us - delete it and return
                If HKLMKey32.OpenSubKey(LocationPath + "\" + KeyPath) Is Nothing Then
                    RegistryEdit.OpenSubKey(KeyPath, True).DeleteValue("", False)
                    Exit Sub 'Exit Sub is the better 'Return' equivalent - it instantly ends this Sub.
                End If
                'If the value doesn't exist in the backups of the key, also delete it for the same reason
                If HKLMKey32.OpenSubKey(LocationPath + "\" + KeyPath).GetValue("") Is Nothing And Not RegistryEdit.OpenSubKey(KeyPath).GetValue("") Is Nothing Then
                    RegistryEdit.OpenSubKey(KeyPath, True).DeleteValue("", False)
                End If
                'If the value doesn't even exist in backups, obviously just quit out
                If HKLMKey32.OpenSubKey(LocationPath + "\" + KeyPath).GetValue("") Is Nothing Then
                    Exit Sub
                End If
            Catch ex As Exception
                ErrorOccurred("Failed to delete '(Default)' value of " + KeyPath + " (LocationPath: " + LocationPath + ", KeyPath: " + KeyPath + "): " + ex.ToString())
            End Try

            Try
                'Set default value of the key to the backup storage location's key's default value
                RegistryEdit.OpenSubKey(KeyPath, True).SetValue("", HKLMKey32.OpenSubKey(LocationPath + "\" + KeyPath).GetValue(""), HKLMKey32.OpenSubKey(LocationPath + "\" + KeyPath).GetValueKind(""))
            Catch ex As Exception
                ErrorOccurred("Failed to restore '(Default)' value of " + Key + " (LocationPath: " + LocationPath + ", KeyPath: " + KeyPath + "): " + ex.ToString())
            End Try
        Else
            'Now, restore the value from its backups
            If RegistryEdit.OpenSubKey(KeyPath) Is Nothing Then
                Exit Sub
            End If

            Try
                'If the value doesn't exist in backups, this means it was created by us - delete it and return
                If HKLMKey32.OpenSubKey(LocationPath + "\" + KeyPath) Is Nothing Then
                    RegistryEdit.OpenSubKey(KeyPath, True).DeleteValue(Value, False)
                    Exit Sub
                End If
                'If the value doesn't exist in the backups of the key, also delete it for the same reason
                If Not HKLMKey32.OpenSubKey(LocationPath + "\" + KeyPath).GetValueNames.Contains(Value) And RegistryEdit.OpenSubKey(KeyPath).GetValueNames.Contains(Value) Then
                    RegistryEdit.OpenSubKey(KeyPath, True).DeleteValue(Value, False)
                End If
                'If the value doesn't even exist in backups, obviously just quit out
                If Not HKLMKey32.OpenSubKey(LocationPath + "\" + KeyPath).GetValueNames.Contains(Value) Then
                    Exit Sub
                End If
            Catch ex As Exception
                ErrorOccurred("Failed to delete " + KeyPath + " from " + RegistryEdit.Name + " (LocationPath: " + LocationPath + ", KeyPath: " + KeyPath + "): " + ex.ToString())
            End Try

            Try
                'Set value Value of the key to the backup storage location's key's value Value
                RegistryEdit.OpenSubKey(KeyPath, True).SetValue(Value, HKLMKey32.OpenSubKey(LocationPath + "\" + KeyPath).GetValue(Value), HKLMKey32.OpenSubKey(LocationPath + "\" + KeyPath).GetValueKind(Value))
            Catch ex As Exception
                ErrorOccurred("Failed to restore '" + Value + "' value of " + Key + " (LocationPath: " + LocationPath + ", KeyPath: " + KeyPath + "): " + ex.ToString())
            End Try
        End If
    End Sub


    Private Sub RestoreRegistryAll(ByVal Key As String, ByVal LocationKey As String)
        'Recursively restore all registry values (no deleting)

        Dim RegistryEdit As Microsoft.Win32.RegistryKey
        Dim KeyPath As String
        Dim tempArray As String() = Key.Split("\")
        Dim pathToCreate As String

        'Determine the correct Registry for setting backup's values with
        If tempArray.GetValue(0) = "HKCU" Then
            RegistryEdit = Microsoft.Win32.Registry.CurrentUser
        ElseIf tempArray.GetValue(0) = "HKU" Then
            RegistryEdit = Microsoft.Win32.Registry.Users
        Else
            RegistryEdit = Microsoft.Win32.Registry.LocalMachine
        End If
        Dim LocationPath As String = Join(LocationKey.Split("\").Skip(1).ToArray(), "\")
        If Key.Contains("\") Then
            KeyPath = Join(Key.Split("\").Skip(1).ToArray(), "\") 'Get rid of useless prefix from path
        Else
            KeyPath = "" 'Empty without its prefix
        End If

        'First, make sure the key that we made the backup from in the first place still exists
        pathToCreate = ""
        If KeyPath.Contains("\") Then
            tempArray = KeyPath.Split("\")
            For Each item In tempArray
                If pathToCreate = "" Then
                    pathToCreate = item
                Else
                    pathToCreate = pathToCreate + "\" + item
                End If
                If RegistryEdit.OpenSubKey(pathToCreate) Is Nothing Then
                    Try
                        RegistryEdit.CreateSubKey(pathToCreate)
                    Catch ex As Exception
                        ErrorOccurred("Failed to create key: " + pathToCreate + ": " + ex.ToString())
                    End Try
                End If
            Next
        End If

        'Now, restore all values in the key
        If HKLMKey32.OpenSubKey(LocationPath + "\" + KeyPath) Is Nothing Then 'Make sure that the backups' equivalent key even exists
            Exit Sub
        End If
        For Each ValueItem In HKLMKey32.OpenSubKey(LocationPath + "\" + KeyPath).GetValueNames 'Iterate over each value in the backups' equivalent keys, and restore the data of values from the backups' equivalent of it
            RegistryEdit.OpenSubKey(KeyPath, True).SetValue(ValueItem, HKLMKey32.OpenSubKey(LocationPath + "\" + KeyPath).GetValue(ValueItem), HKLMKey32.OpenSubKey(LocationPath + "\" + KeyPath).GetValueKind(ValueItem))
        Next

        'Iterate over the children of this key
        For Each SubKey In HKLMKey32.OpenSubKey(LocationPath + "\" + KeyPath).GetSubKeyNames
            RestoreRegistryAll(Key + "\" + SubKey, LocationKey)
        Next
    End Sub


    Private Sub DeleteEmpty(ByVal Key As String)
        Dim RegistryEdit As Microsoft.Win32.RegistryKey
        Dim KeyPath As String
        Dim tempArray As String() = Key.Split("\")

        'Determine the correct Registry for setting backup's values with
        If tempArray.GetValue(0) = "HKCU" Then
            RegistryEdit = Microsoft.Win32.Registry.CurrentUser
        ElseIf tempArray.GetValue(0) = "HKU" Then
            RegistryEdit = Microsoft.Win32.Registry.Users
        Else
            RegistryEdit = Microsoft.Win32.Registry.LocalMachine
        End If
        KeyPath = Join(Key.Split("\").Skip(1).ToArray(), "\") 'Get rid of useless prefix from path
        If RegistryEdit.OpenSubKey(KeyPath) Is Nothing Then
            Exit Sub
        End If
        'Delete key if it's empty
        If RegistryEdit.OpenSubKey(KeyPath).GetValueNames().Count() = 0 And RegistryEdit.OpenSubKey(KeyPath).GetSubKeyNames().Count() = 0 Then 'If the key is now empty, delete it
            RegistryEdit.DeleteSubKey(KeyPath, False)
        End If
    End Sub


    Private Sub ErrorOccurred(ByVal message As String)
        Dim file As System.IO.StreamWriter
        file = My.Computer.FileSystem.OpenTextFileWriter(exfile, False)
        file.WriteLine(message) 'Write the exception message to the file
        file.Close()

        End 'Terminate immediately upon failing
    End Sub
End Module