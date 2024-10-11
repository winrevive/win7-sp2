Public Class Configs
    Public itemsToDelete As New List(Of String)
    Public userItemsToDelete As New List(Of String)
    Public itemsToMove As New Dictionary(Of String, List(Of String)) 'id: {original location, new location}
    Public userItemsToMove As New Dictionary(Of String, List(Of String))

    Public Sub populateItems()
        populateItemsToDelete()
        populateUserItemsToDelete()
        populateItemsToMove()
        populateUserItemsToMove()
    End Sub

    Public Sub populateItemsToDelete()
        itemsToDelete.Add("Windows\Web\Wallpaper\Theme1\img13.jpg")
        itemsToDelete.Add("Windows\System32\oobe\FirstLogonAnim.exe")
        itemsToDelete.Add("Windows\System32\oobe\FirstLogonAnim.html")
        'For fixing All Programs layout
        itemsToDelete.Add("ProgramData\Microsoft\Windows\Start Menu\Programs\System Tools\desktop.ini")
        itemsToDelete.Add("ProgramData\Microsoft\Windows\Start Menu\Programs\Accessibility\desktop.ini")
        'Get rid of Classic Task Manager's shortcut
        itemsToDelete.Add("ProgramData\Microsoft\Windows\Start Menu\Programs\System Tools\Classic Task Manager.lnk")
    End Sub

    Public Sub populateUserItemsToDelete()
        If HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("ReduceWinX") = "true" Then
            userItemsToDelete.Add("AppData\Local\Microsoft\Windows\WinX\Group1\1 - Desktop.lnk")
            userItemsToDelete.Add("AppData\Local\Microsoft\Windows\WinX\Group2")
            userItemsToDelete.Add("AppData\Local\Microsoft\Windows\WinX\Group3")
        End If
        'For fixing All Programs layout
        itemsToDelete.Add("AppData\Roaming\Microsoft\Windows\Start Menu\Programs\System Tools\desktop.ini")
        itemsToDelete.Add("AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Accessibility\desktop.ini")
    End Sub

    Public Sub populateItemsToMove()
        'For fixing All Programs layout
        itemsToMove.Add("ProgramFilesAccessibility", ToList("ProgramData\Microsoft\Windows\Start Menu\Programs\Accessibility", "ProgramData\Microsoft\Windows\Start Menu\Programs\Accessories\Accessibility"))
        itemsToMove.Add("ProgramFilesSysTools", ToList("ProgramData\Microsoft\Windows\Start Menu\Programs\System Tools", "ProgramData\Microsoft\Windows\Start Menu\Programs\Accessories\System Tools"))
    End Sub

    Public Sub populateUserItemsToMove()
        'For fixing All Programs layout
        userItemsToMove.Add("ProgramFilesAccessibility", ToList("AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Accessibility", "AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Accessories\Accessibility"))
        userItemsToMove.Add("ProgramFilesSysTools", ToList("AppData\Roaming\Microsoft\Windows\Start Menu\Programs\System Tools", "AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Accessories\System Tools"))
    End Sub

    Function ToList(ByVal Location As String, ByVal NewLocation As String)
        Dim inputs() As String = {Location, NewLocation}
        Dim result As List(Of String) = New List(Of String)(inputs)

        Return result
    End Function
End Class
