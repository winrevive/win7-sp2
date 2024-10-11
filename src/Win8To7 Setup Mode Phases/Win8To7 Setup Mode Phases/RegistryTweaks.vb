Public Class RegistryTweaks
    ' ID: {DWORD: {Key, Value, Data}, String: {Key, Value, Data}, Delete: {Key, Value, Data}}
    Public SystemTweaks As New Dictionary(Of String, List(Of List(Of String)))
    Public UserTweaks As New Dictionary(Of String, List(Of List(Of String)))
    Public UserClassTweaks As New Dictionary(Of String, List(Of List(Of String)))
    Private sibdir As String
    Private openshelldir As String


    Public Sub init()
        'Deal with misc. paths
        If Environment.Is64BitOperatingSystem = True Then
            sibdir = windrive + "Program Files (x86)\StartIsBack"
        Else
            sibdir = windrive + "Program Files\StartIsBack"
        End If
        openshelldir = windrive + "Program Files\Open-Shell"

        PopulateSystemTweaks()
        PopulateUserTweaks()
    End Sub


    Public Sub PopulateSystemTweaks() 'Here is where you can dump the keys to change on a system-wide basis
        Dim itemstodelete As New List(Of List(Of String))
        Dim dwordchanges As New List(Of List(Of String))
        Dim stringchanges As New List(Of List(Of String))
        Dim multistringchanges As New List(Of List(Of String))
        Dim hexchanges As New List(Of List(Of String))

        'Service Pack 1
        If Not HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("Branding") = "win8pro" And _
            Not HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("Branding") = "win8home" And _
            Not HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("Branding") = "win8starter" Then
            stringchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "CSDVersion", "Service Pack 1"))
            dwordchanges.Add(ToList("HKLM\SYSTEM\ControlSet001\Control\Windows", "CSDVersion", "256"))
            dwordchanges.Add(ToList("HKLM\SYSTEM\ControlSet001\Control\Windows", "CSDReleaseType", "0"))
            dwordchanges.Add(ToList("HKLM\SYSTEM\ControlSet002\Control\Windows", "CSDVersion", "256"))
            dwordchanges.Add(ToList("HKLM\SYSTEM\ControlSet002\Control\Windows", "CSDReleaseType", "0"))
        End If


        'Colour Swatches
        ' Blush
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{1E806D7D-F0C4-4F5E-876A-232C1E62F955}", "PreviewOrder", "10"))
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{1E806D7D-F0C4-4F5E-876A-232C1E62F955}", "PreviewId", "22")) 'Add one to make room for Sky following Automatic
        hexchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{1E806D7D-F0C4-4F5E-876A-232C1E62F955}", "Resource", "7400680065006d006500630070006c0037002e0064006c006c000000"))
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{1E806D7D-F0C4-4F5E-876A-232C1E62F955}", "NameId", "110"))
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{1E806D7D-F0C4-4F5E-876A-232C1E62F955}", "Color", "1895614456"))
        stringchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{1E806D7D-F0C4-4F5E-876A-232C1E62F955}", "/ve", "Blush"))
        ' Frost
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{2F1411A0-5BA2-40BB-A731-2EB5BCC8CA24}", "PreviewOrder", "16"))
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{2F1411A0-5BA2-40BB-A731-2EB5BCC8CA24}", "PreviewId", "29"))
        hexchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{2F1411A0-5BA2-40BB-A731-2EB5BCC8CA24}", "Resource", "7400680065006d006500630070006c0037002e0064006c006c000000"))
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{2F1411A0-5BA2-40BB-A731-2EB5BCC8CA24}", "NameId", "117"))
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{2F1411A0-5BA2-40BB-A731-2EB5BCC8CA24}", "Color", "1425865980"))
        stringchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{2F1411A0-5BA2-40BB-A731-2EB5BCC8CA24}", "/ve", "Frost"))
        ' Sky (default)
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{2F4CBDD2-35D9-4618-9B7F-4D5EE36B60B4}", "PreviewOrder", "1"))
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{2F4CBDD2-35D9-4618-9B7F-4D5EE36B60B4}", "PreviewId", "1"))
        hexchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{2F4CBDD2-35D9-4618-9B7F-4D5EE36B60B4}", "Resource", "7400680065006d006500630070006c0037002e0064006c006c000000"))
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{2F4CBDD2-35D9-4618-9B7F-4D5EE36B60B4}", "NameId", "101"))
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{2F4CBDD2-35D9-4618-9B7F-4D5EE36B60B4}", "Color", "1802811644"))
        stringchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{2F4CBDD2-35D9-4618-9B7F-4D5EE36B60B4}", "/ve", "Sky (default)"))
        ' Twilight
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{32D17E91-6042-4c7f-9E59-CB1EFF01391C}", "PreviewOrder", "2"))
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{32D17E91-6042-4c7f-9E59-CB1EFF01391C}", "PreviewId", "2"))
        hexchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{32D17E91-6042-4c7f-9E59-CB1EFF01391C}", "Resource", "7400680065006d006500630070006c0037002e0064006c006c000000"))
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{32D17E91-6042-4c7f-9E59-CB1EFF01391C}", "NameId", "102"))
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{32D17E91-6042-4c7f-9E59-CB1EFF01391C}", "Color", "2818590381"))
        stringchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{32D17E91-6042-4c7f-9E59-CB1EFF01391C}", "/ve", "Twilight"))
        ' Lavender
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{57D1C9EC-BDE8-451C-A8C0-DC9618A77262}", "PreviewOrder", "12"))
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{57D1C9EC-BDE8-451C-A8C0-DC9618A77262}", "PreviewId", "24"))
        hexchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{57D1C9EC-BDE8-451C-A8C0-DC9618A77262}", "Resource", "7400680065006d006500630070006c0037002e0064006c006c000000"))
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{57D1C9EC-BDE8-451C-A8C0-DC9618A77262}", "NameId", "112"))
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{57D1C9EC-BDE8-451C-A8C0-DC9618A77262}", "Color", "1384995476"))
        stringchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{57D1C9EC-BDE8-451C-A8C0-DC9618A77262}", "/ve", "Lavender"))
        ' Pumpkin
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{64A6FD77-9D49-4cdd-BA05-70DFE9ECE9B4}", "PreviewOrder", "7"))
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{64A6FD77-9D49-4cdd-BA05-70DFE9ECE9B4}", "PreviewId", "7"))
        hexchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{64A6FD77-9D49-4cdd-BA05-70DFE9ECE9B4}", "Resource", "7400680065006d006500630070006c0037002e0064006c006c000000"))
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{64A6FD77-9D49-4cdd-BA05-70DFE9ECE9B4}", "NameId", "107"))
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{64A6FD77-9D49-4cdd-BA05-70DFE9ECE9B4}", "Color", "2164235264"))
        stringchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{64A6FD77-9D49-4cdd-BA05-70DFE9ECE9B4}", "/ve", "Pumpkin"))
        ' Sun
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{7E90182C-B135-4a81-BBC6-64A82883219A}", "PreviewOrder", "6"))
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{7E90182C-B135-4a81-BBC6-64A82883219A}", "PreviewId", "6"))
        hexchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{7E90182C-B135-4a81-BBC6-64A82883219A}", "Resource", "7400680065006d006500630070006c0037002e0064006c006c000000"))
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{7E90182C-B135-4a81-BBC6-64A82883219A}", "NameId", "106"))
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{7E90182C-B135-4a81-BBC6-64A82883219A}", "Color", "1425726478"))
        stringchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{7E90182C-B135-4a81-BBC6-64A82883219A}", "/ve", "Sun"))
        ' Fuchsia
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{86A129BE-D8E4-4190-8F0D-7EDADEC798D5}", "PreviewOrder", "9"))
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{86A129BE-D8E4-4190-8F0D-7EDADEC798D5}", "PreviewId", "21"))
        hexchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{86A129BE-D8E4-4190-8F0D-7EDADEC798D5}", "Resource", "7400680065006d006500630070006c0037002e0064006c006c000000"))
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{86A129BE-D8E4-4190-8F0D-7EDADEC798D5}", "NameId", "109"))
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{86A129BE-D8E4-4190-8F0D-7EDADEC798D5}", "Color", "1727987865"))
        stringchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{86A129BE-D8E4-4190-8F0D-7EDADEC798D5}", "/ve", "Fuchsia"))
        ' Taupe
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{8DD9E9AD-9315-4B9B-8473-9A1C9EA7D4B6}", "PreviewOrder", "13"))
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{8DD9E9AD-9315-4B9B-8473-9A1C9EA7D4B6}", "PreviewId", "25"))
        hexchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{8DD9E9AD-9315-4B9B-8473-9A1C9EA7D4B6}", "Resource", "7400680065006d006500630070006c0037002e0064006c006c000000"))
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{8DD9E9AD-9315-4B9B-8473-9A1C9EA7D4B6}", "NameId", "113"))
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{8DD9E9AD-9315-4B9B-8473-9A1C9EA7D4B6}", "Color", "1721271372"))
        stringchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{8DD9E9AD-9315-4B9B-8473-9A1C9EA7D4B6}", "/ve", "Taupe"))
        ' Sea
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{AFE71214-8915-49f3-8719-C38D8BEDCCD0}", "PreviewOrder", "3"))
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{AFE71214-8915-49f3-8719-C38D8BEDCCD0}", "PreviewId", "3"))
        hexchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{AFE71214-8915-49f3-8719-C38D8BEDCCD0}", "Resource", "7400680065006d006500630070006c0037002e0064006c006c000000"))
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{AFE71214-8915-49f3-8719-C38D8BEDCCD0}", "NameId", "103"))
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{AFE71214-8915-49f3-8719-C38D8BEDCCD0}", "Color", "2150813133"))
        stringchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{AFE71214-8915-49f3-8719-C38D8BEDCCD0}", "/ve", "Sea"))
        ' Leaf
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{B5650616-CAD6-4961-85D3-6BE2385482B8}", "PreviewOrder", "4"))
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{B5650616-CAD6-4961-85D3-6BE2385482B8}", "PreviewId", "4"))
        hexchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{B5650616-CAD6-4961-85D3-6BE2385482B8}", "Resource", "7400680065006d006500630070006c0037002e0064006c006c000000"))
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{B5650616-CAD6-4961-85D3-6BE2385482B8}", "NameId", "104"))
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{B5650616-CAD6-4961-85D3-6BE2385482B8}", "Color", "1712629248"))
        stringchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{B5650616-CAD6-4961-85D3-6BE2385482B8}", "/ve", "Leaf"))
        ' Violet
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{B8C9A217-1BEC-49BB-A919-C69CE5EA31A2}", "PreviewOrder", "11"))
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{B8C9A217-1BEC-49BB-A919-C69CE5EA31A2}", "PreviewId", "23"))
        hexchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{B8C9A217-1BEC-49BB-A919-C69CE5EA31A2}", "Resource", "7400680065006d006500630070006c0037002e0064006c006c000000"))
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{B8C9A217-1BEC-49BB-A919-C69CE5EA31A2}", "NameId", "111"))
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{B8C9A217-1BEC-49BB-A919-C69CE5EA31A2}", "Color", "2238593953"))
        stringchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{B8C9A217-1BEC-49BB-A919-C69CE5EA31A2}", "/ve", "Violet"))
        ' Slate
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{DB39F7C6-CB7A-4F86-B41A-E1491F2F8717}", "PreviewOrder", "15"))
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{DB39F7C6-CB7A-4F86-B41A-E1491F2F8717}", "PreviewId", "27"))
        hexchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{DB39F7C6-CB7A-4F86-B41A-E1491F2F8717}", "Resource", "7400680065006d006500630070006c0037002e0064006c006c000000"))
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{DB39F7C6-CB7A-4F86-B41A-E1491F2F8717}", "NameId", "115"))
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{DB39F7C6-CB7A-4F86-B41A-E1491F2F8717}", "Color", "2153076053"))
        stringchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{DB39F7C6-CB7A-4F86-B41A-E1491F2F8717}", "/ve", "Slate"))
        ' Ruby
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{EB768E8E-0A90-4f0b-BA48-5FB46067AD41}", "PreviewOrder", "8"))
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{EB768E8E-0A90-4f0b-BA48-5FB46067AD41}", "PreviewId", "8"))
        hexchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{EB768E8E-0A90-4f0b-BA48-5FB46067AD41}", "Resource", "7400680065006d006500630070006c0037002e0064006c006c000000"))
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{EB768E8E-0A90-4f0b-BA48-5FB46067AD41}", "NameId", "108"))
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{EB768E8E-0A90-4f0b-BA48-5FB46067AD41}", "Color", "2832076559"))
        stringchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{EB768E8E-0A90-4f0b-BA48-5FB46067AD41}", "/ve", "Ruby"))
        If HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("KeepChocolate") = "true" Then
            ' Chocolate
            dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{EF700953-1426-4E45-8967-C7727C208CD2}", "PreviewOrder", "14"))
            dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{EF700953-1426-4E45-8967-C7727C208CD2}", "PreviewId", "26"))
            hexchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{EF700953-1426-4E45-8967-C7727C208CD2}", "Resource", "7400680065006d006500630070006c0037002e0064006c006c000000"))
            dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{EF700953-1426-4E45-8967-C7727C208CD2}", "NameId", "114"))
            dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{EF700953-1426-4E45-8967-C7727C208CD2}", "Color", "2823756571"))
            stringchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{EF700953-1426-4E45-8967-C7727C208CD2}", "/ve", "Chocolate"))
        End If
        ' Lime
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{FD81078C-1B36-4595-A92E-91F05C4FA5DC}", "PreviewOrder", "5"))
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{FD81078C-1B36-4595-A92E-91F05C4FA5DC}", "PreviewId", "5"))
        hexchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{FD81078C-1B36-4595-A92E-91F05C4FA5DC}", "Resource", "7400680065006d006500630070006c0037002e0064006c006c000000"))
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{FD81078C-1B36-4595-A92E-91F05C4FA5DC}", "NameId", "105"))
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{FD81078C-1B36-4595-A92E-91F05C4FA5DC}", "Color", "1721227575"))
        stringchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{FD81078C-1B36-4595-A92E-91F05C4FA5DC}", "/ve", "Lime"))
        If HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("KeepAutomatic") = "true" Then
            ' Automatic (default)
            If HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("KeepChocolate") = "true" Then
                dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{69F343NF-3420-4E30-B2CD-4D59F3C1E67F}", "PreviewOrder", "99")) 'push it to the third row if Chocolate is also kept
            Else
                dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{69F343NF-3420-4E30-B2CD-4D59F3C1E67F}", "PreviewOrder", "0"))
            End If
            dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{69F343NF-3420-4E30-B2CD-4D59F3C1E67F}", "PreviewId", "28"))
            hexchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{69F343NF-3420-4E30-B2CD-4D59F3C1E67F}", "Resource", "7400680065006d006500630070006c0037002e0064006c006c000000"))
            dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{69F343NF-3420-4E30-B2CD-4D59F3C1E67F}", "NameId", "116"))
            dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{69F343NF-3420-4E30-B2CD-4D59F3C1E67F}", "Color", "0"))
            stringchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Control Panel\Glass Colorization\Swatches\{69F343NF-3420-4E30-B2CD-4D59F3C1E67F}", "/ve", "Automatic (default)"))
        End If

        'Apply the Basic theme for SYSTEM, to match new values
        ' Backend NOTE: Normally this would be to 'aerostandard.msstyles', however since aerostandard doesn't exist in Backend, it'd break Windows thus it was changed to just use 'aerolite.msstyles'
        stringchanges.Add(ToList("HKU\.DEFAULT\Software\Microsoft\Windows\CurrentVersion\ThemeManager", "DllName", "%WINDIR%\Resources\Themes\aero\aerolite.msstyles")) '%WINDIR% will be translated later
        'Change .DEFAULT's window colours to be Home Basic like, since, well, no Basic
        dwordchanges.Add(ToList("HKU\.DEFAULT\Software\Microsoft\Windows\DWM", "ColorizationGlassAttribute", "0"))
        dwordchanges.Add(ToList("HKU\.DEFAULT\Software\Microsoft\Windows\DWM", "ColorizationColor", "4289183964"))
        dwordchanges.Add(ToList("HKU\.DEFAULT\Software\Microsoft\Windows\DWM", "ColorizationColorBalance", "100"))
        dwordchanges.Add(ToList("HKU\.DEFAULT\Software\Microsoft\Windows\DWM", "ColorizationAfterglow", "4289183964"))
        dwordchanges.Add(ToList("HKU\.DEFAULT\Software\Microsoft\Windows\DWM", "ColorizationAfterglowBalance", "10"))
        dwordchanges.Add(ToList("HKU\.DEFAULT\Software\Microsoft\Windows\DWM", "ColorizationBlurBalance", "0"))
        dwordchanges.Add(ToList("HKU\.DEFAULT\Software\Microsoft\Windows\DWM", "EnableWindowColorization", "1"))
        dwordchanges.Add(ToList("HKU\.DEFAULT\Software\Microsoft\Windows\DWM", "ColorizationGlassReflectionIntensity", "100"))
        dwordchanges.Add(ToList("HKU\.DEFAULT\Software\Microsoft\Windows\DWM", "TextGlowMode", "3"))
        dwordchanges.Add(ToList("HKU\.DEFAULT\Software\Microsoft\Windows\DWM", "RoundRectRadius", "12"))
        dwordchanges.Add(ToList("HKU\.DEFAULT\Software\Microsoft\Windows\DWM", "ColorizationBlurBalanceInactive", "0"))
        dwordchanges.Add(ToList("HKU\.DEFAULT\Software\Microsoft\Windows\DWM", "ColorizationColorBalanceInactive", "100"))
        'Finally give SYSTEM schematics
        stringchanges.Add(ToList("HKU\.DEFAULT\Control Panel\Desktop\WindowMetrics", "SmCaptionWidth", "-330"))
        stringchanges.Add(ToList("HKU\.DEFAULT\Control Panel\Desktop\WindowMetrics", "BorderWidth", "-15"))
        stringchanges.Add(ToList("HKU\.DEFAULT\Control Panel\Desktop\WindowMetrics", "SmCaptionHeight", "-315"))
        stringchanges.Add(ToList("HKU\.DEFAULT\Control Panel\Desktop\WindowMetrics", "CaptionWidth", "-330"))
        stringchanges.Add(ToList("HKU\.DEFAULT\Control Panel\Desktop\WindowMetrics", "IconTitleWrap", "1"))
        stringchanges.Add(ToList("HKU\.DEFAULT\Control Panel\Desktop\WindowMetrics", "ScrollHeight", "-255"))
        stringchanges.Add(ToList("HKU\.DEFAULT\Control Panel\Desktop\WindowMetrics", "CaptionHeight", "-315"))
        hexchanges.Add(ToList("HKU\.DEFAULT\Control Panel\Desktop\WindowMetrics", "CaptionFont", "f4ffffff0000000000000000000000009001000000000001000000005300650067006f006500200055004900000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000"))
        stringchanges.Add(ToList("HKU\.DEFAULT\Control Panel\Desktop\WindowMetrics", "ScrollWidth", "-255"))
        stringchanges.Add(ToList("HKU\.DEFAULT\Control Panel\Desktop\WindowMetrics", "MenuWidth", "-285"))
        hexchanges.Add(ToList("HKU\.DEFAULT\Control Panel\Desktop\WindowMetrics", "SmCaptionFont", "f4ffffff0000000000000000000000009001000000000001000000005300650067006f006500200055004900000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000"))
        hexchanges.Add(ToList("HKU\.DEFAULT\Control Panel\Desktop\WindowMetrics", "MenuFont", "f4ffffff0000000000000000000000009001000000000001000005005300650067006f006500200055004900000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000"))
        hexchanges.Add(ToList("HKU\.DEFAULT\Control Panel\Desktop\WindowMetrics", "StatusFont", "f4ffffff0000000000000000000000009001000000000001000005005300650067006f006500200055004900000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000"))
        stringchanges.Add(ToList("HKU\.DEFAULT\Control Panel\Desktop\WindowMetrics", "MenuHeight", "-285"))
        hexchanges.Add(ToList("HKU\.DEFAULT\Control Panel\Desktop\WindowMetrics", "MessageFont", "f4ffffff0000000000000000000000009001000000000001000005005300650067006f006500200055004900000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000"))
        hexchanges.Add(ToList("HKU\.DEFAULT\Control Panel\Desktop\WindowMetrics", "IconFont", "f4ffffff0000000000000000000000009001000000000001000005005300650067006f006500200055004900000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000"))
        stringchanges.Add(ToList("HKU\.DEFAULT\Control Panel\Desktop\WindowMetrics", "Shell Icon Size", "40"))
        stringchanges.Add(ToList("HKU\.DEFAULT\Control Panel\Desktop\WindowMetrics", "PaddedBorderWidth", "-60"))
        dwordchanges.Add(ToList("HKU\.DEFAULT\Control Panel\Desktop\WindowMetrics", "AppliedDPI", "96"))
        stringchanges.Add(ToList("HKU\.DEFAULT\Control Panel\Desktop\WindowMetrics", "IconSpacing", "-1125"))
        stringchanges.Add(ToList("HKU\.DEFAULT\Control Panel\Desktop\WindowMetrics", "IconVerticalSpacing", "-1125"))
        stringchanges.Add(ToList("HKU\.DEFAULT\Control Panel\Desktop\WindowMetrics", "MinAnimate", "1"))
        stringchanges.Add(ToList("HKU\.DEFAULT\Control Panel\Desktop\WindowMetrics", "Shell Icon Size", "18"))
        'Give SYSTEM default sounds
        itemstodelete.Add(ToList("HKU\.DEFAULT\AppEvents\EventLabels\WindowsLogon", "ExcludeFromCPL", ""))
        itemstodelete.Add(ToList("HKU\.DEFAULT\AppEvents\EventLabels\WindowsLogoff", "ExcludeFromCPL", ""))
        itemstodelete.Add(ToList("HKU\.DEFAULT\AppEvents\EventLabels\SystemExit", "ExcludeFromCPL", ""))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\.Default\.Default", "/ve", "%WINDIR%\Media\Windows Ding.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\.Default\.Current", "/ve", "%WINDIR%\Media\Windows Ding.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\.Default\Custom", "/ve", "%WINDIR%\Media\Windows Ding.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\ChangeTheme\.Default", "/ve", "%WINDIR%\Media\Windows Logon Sound.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\ChangeTheme\.Current", "/ve", "%WINDIR%\Media\Windows Logon Sound.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\ChangeTheme\Custom", "/ve", "%WINDIR%\Media\Windows Logon Sound.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\CriticalBatteryAlarm\.Default", "/ve", "%WINDIR%\Media\Windows Battery Critical.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\CriticalBatteryAlarm\.Current", "/ve", "%WINDIR%\Media\Windows Battery Critical.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\CriticalBatteryAlarm\Custom", "/ve", "%WINDIR%\Media\Windows Battery Critical.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\DeviceConnect\.Default", "/ve", "%WINDIR%\Media\Windows Hardware Insert.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\DeviceConnect\.Current", "/ve", "%WINDIR%\Media\Windows Hardware Insert.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\DeviceConnect\Custom", "/ve", "%WINDIR%\Media\Windows Hardware Insert.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\DeviceConnect\.Default", "/ve", "%WINDIR%\Media\Windows Hardware Insert.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\DeviceConnect\.Current", "/ve", "%WINDIR%\Media\Windows Hardware Insert.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\DeviceConnect\Custom", "/ve", "%WINDIR%\Media\Windows Hardware Insert.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\DeviceDisconnect\.Default", "/ve", "%WINDIR%\Media\Windows Hardware Remove.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\DeviceDisconnect\.Current", "/ve", "%WINDIR%\Media\Windows Hardware Remove.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\DeviceDisconnect\Custom", "/ve", "%WINDIR%\Media\Windows Hardware Remove.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\DeviceFail\.Default", "/ve", "%WINDIR%\Media\Windows Hardware Fail.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\DeviceFail\.Current", "/ve", "%WINDIR%\Media\Windows Hardware Fail.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\DeviceFail\Custom", "/ve", "%WINDIR%\Media\Windows Hardware Fail.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\FaxBeep\.Default", "/ve", "%WINDIR%\Media\Windows Notify.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\FaxBeep\.Current", "/ve", "%WINDIR%\Media\Windows Notify.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\FaxBeep\Custom", "/ve", "%WINDIR%\Media\Windows Notify.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\LowBatteryAlarm\.Default", "/ve", "%WINDIR%\Media\Windows Battery Low.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\LowBatteryAlarm\.Current", "/ve", "%WINDIR%\Media\Windows Battery Low.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\LowBatteryAlarm\Custom", "/ve", "%WINDIR%\Media\Windows Battery Low.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\MailBeep\.Default", "/ve", "%WINDIR%\Media\Windows Notify.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\MailBeep\.Current", "/ve", "%WINDIR%\Media\Windows Notify.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\MailBeep\Custom", "/ve", "%WINDIR%\Media\Windows Notify.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\PrintComplete\.Default", "/ve", "%WINDIR%\Media\Windows Print complete.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\PrintComplete\.Current", "/ve", "%WINDIR%\Media\Windows Print complete.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\PrintComplete\Custom", "/ve", "%WINDIR%\Media\Windows Print complete.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\SystemAsterisk\.Default", "/ve", "%WINDIR%\Media\Windows Error.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\SystemAsterisk\.Current", "/ve", "%WINDIR%\Media\Windows Error.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\SystemAsterisk\Custom", "/ve", "%WINDIR%\Media\Windows Error.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\SystemExclamation\.Default", "/ve", "%WINDIR%\Media\Windows Exclamation.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\SystemExclamation\.Current", "/ve", "%WINDIR%\Media\Windows Exclamation.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\SystemExclamation\Custom", "/ve", "%WINDIR%\Media\Windows Exclamation.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\SystemExit\.Default", "/ve", "%WINDIR%\Media\Windows Shutdown.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\SystemExit\.Current", "/ve", "%WINDIR%\Media\Windows Shutdown.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\SystemExit\Custom", "/ve", "%WINDIR%\Media\Windows Shutdown.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\SystemHand\.Default", "/ve", "%WINDIR%\Media\Windows Critical Stop.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\SystemHand\.Current", "/ve", "%WINDIR%\Media\Windows Critical Stop.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\SystemHand\Custom", "/ve", "%WINDIR%\Media\Windows Critical Stop.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\SystemNotification\.Default", "/ve", "%WINDIR%\Media\Windows Balloon.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\SystemNotification\.Current", "/ve", "%WINDIR%\Media\Windows Balloon.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\SystemNotification\Custom", "/ve", "%WINDIR%\Media\Windows Balloon.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\WindowsLogoff\.Default", "/ve", "%WINDIR%\Media\Windows Logoff Sound.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\WindowsLogoff\.Current", "/ve", "%WINDIR%\Media\Windows Logoff Sound.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\WindowsLogoff\Custom", "/ve", "%WINDIR%\Media\Windows Logoff Sound.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\WindowsLogon\.Default", "/ve", "%WINDIR%\Media\Windows Logon Sound.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\WindowsLogon\.Current", "/ve", "%WINDIR%\Media\Windows Logon Sound.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\WindowsLogon\Custom", "/ve", "%WINDIR%\Media\Windows Logon Sound.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\WindowsUAC\.Default", "/ve", "%WINDIR%\Media\Windows User Account Control.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\WindowsUAC\.Current", "/ve", "%WINDIR%\Media\Windows User Account Control.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\.Default\WindowsUAC\Custom", "/ve", "%WINDIR%\Media\Windows User Account Control.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\Explorer\BlockedPopup\.Default", "/ve", "%WINDIR%\Media\Windows Pop-up Blocked.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\Explorer\BlockedPopup\.Current", "/ve", "%WINDIR%\Media\Windows Pop-up Blocked.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\Explorer\BlockedPopup\Custom", "/ve", "%WINDIR%\Media\Windows Pop-up Blocked.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\Explorer\EmptyRecycleBin\.Default", "/ve", "%WINDIR%\Media\Windows Recycle.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\Explorer\EmptyRecycleBin\.Current", "/ve", "%WINDIR%\Media\Windows Recycle.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\Explorer\EmptyRecycleBin\Custom", "/ve", "%WINDIR%\Media\Windows Recycle.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\Explorer\FaxError\.Default", "/ve", "%WINDIR%\Media\ding.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\Explorer\FaxError\.Current", "/ve", "%WINDIR%\Media\ding.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\Explorer\FaxError\Custom", "/ve", "%WINDIR%\Media\ding.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\Explorer\FaxLineRings\.Default", "/ve", "%WINDIR%\Media\Windows Ringin.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\Explorer\FaxLineRings\.Current", "/ve", "%WINDIR%\Media\Windows Ringin.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\Explorer\FaxLineRings\Custom", "/ve", "%WINDIR%\Media\Windows Ringin.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\Explorer\FaxSent\.Default", "/ve", "%WINDIR%\Media\tada.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\Explorer\FaxSent\.Current", "/ve", "%WINDIR%\Media\tada.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\Explorer\FaxSent\Custom", "/ve", "%WINDIR%\Media\tada.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\Explorer\FeedDiscovered\.Default", "/ve", "%WINDIR%\Media\Windows Feed Discovered.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\Explorer\FeedDiscovered\.Current", "/ve", "%WINDIR%\Media\Windows Feed Discovered.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\Explorer\FeedDiscovered\Custom", "/ve", "%WINDIR%\Media\Windows Feed Discovered.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\Explorer\Navigating\.Default", "/ve", "%WINDIR%\Media\Windows Navigation Start.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\Explorer\Navigating\.Current", "/ve", "%WINDIR%\Media\Windows Navigation Start.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\Explorer\Navigating\Custom", "/ve", "%WINDIR%\Media\Windows Navigation Start.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\Explorer\SecurityBand\.Default", "/ve", "%WINDIR%\Media\Windows Information Bar.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\Explorer\SecurityBand\.Current", "/ve", "%WINDIR%\Media\Windows Information Bar.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\Explorer\SecurityBand\Custom", "/ve", "%WINDIR%\Media\Windows Information Bar.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\sapisvr\DisNumbersSound\.Default", "/ve", "%WINDIR%\Media\Speech Disambiguation.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\sapisvr\DisNumbersSound\.Current", "/ve", "%WINDIR%\Media\Speech Disambiguation.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\sapisvr\DisNumbersSound\Custom", "/ve", "%WINDIR%\Media\Speech Disambiguation.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\sapisvr\HubOnSound\.Default", "/ve", "%WINDIR%\Media\Speech On.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\sapisvr\HubOnSound\.Current", "/ve", "%WINDIR%\Media\Speech On.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\sapisvr\HubOnSound\Custom", "/ve", "%WINDIR%\Media\Speech On.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\sapisvr\HubSleepSound\.Default", "/ve", "%WINDIR%\Media\Speech Sleep.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\sapisvr\HubSleepSound\.Current", "/ve", "%WINDIR%\Media\Speech Sleep.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\sapisvr\HubSleepSound\Custom", "/ve", "%WINDIR%\Media\Speech Sleep.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\sapisvr\MisrecoSound\.Default", "/ve", "%WINDIR%\Media\Speech Misrecognition.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\sapisvr\MisrecoSound\.Current", "/ve", "%WINDIR%\Media\Speech Misrecognition.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\sapisvr\MisrecoSound\Custom", "/ve", "%WINDIR%\Media\Speech Misrecognition.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\sapisvr\PanelSound\.Default", "/ve", "%WINDIR%\Media\Speech Disambiguation.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\sapisvr\PanelSound\.Current", "/ve", "%WINDIR%\Media\Speech Disambiguation.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Apps\sapisvr\PanelSound\Custom", "/ve", "%WINDIR%\Media\Speech Disambiguation.wav"))
        stringchanges.Add(ToList("HKU\.DEFAULT\AppEvents\Schemes\Names\.Default", "/ve", "@mmres.dll,-800"))

        'Enable the Startup Sound
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\LogonUI\BootAnimation", "DisableStartupSound", "0"))

        If HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("DisableLockScreen") = "true" Then
            'Optionally disable the lockscreen
            dwordchanges.Add(ToList("HKLM\SOFTWARE\Policies\Microsoft\Windows\Personalization", "NoLockScreen", "1"))
        Else
            itemstodelete.Add(ToList("HKLM\SOFTWARE\Policies\Microsoft\Windows\Personalization", "NoLockScreen", ""))
        End If

        'Change Metro UI colours for the login screen to be as 7 styled as currently possible
        stringchanges.Add(ToList("HKLM\SOFTWARE\Policies\Microsoft\Windows\Personalization", "PersonalColors_Accent", "#366E93"))
        stringchanges.Add(ToList("HKLM\SOFTWARE\Policies\Microsoft\Windows\Personalization", "PersonalColors_Background", "#185D7C"))

        'Shut up about new default programs
        dwordchanges.Add(ToList("HKLM\SOFTWARE\Policies\Microsoft\Windows\Explorer", "NoNewAppAlert", "1"))

        'Enable funni Pointer Shadows
        hexchanges.Add(ToList("HKU\.DEFAULT\Control Panel\Desktop", "UserPreferencesMask", "9E3E078012000000"))

        'Reroute Getting Started in Control Panel
        stringchanges.Add(ToList("HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\ControlPanel\NameSpace\{CB1B7F8C-C50A-4176-B604-9E24DEE8D4D1}", "/ve", "Getting Started"))
        stringchanges.Add(ToList("HKLM\SOFTWARE\Classes\CLSID\{CB1B7F8C-C50A-4176-B604-9E24DEE8D4D1}\Shell\Open\Command", "/ve", "%SystemRoot%\system32\GettingStarted.exe"))
        itemstodelete.Add(ToList("HKLM\SOFTWARE\Classes\CLSID\{CB1B7F8C-C50A-4176-B604-9E24DEE8D4D1}\InProcServer32", "/ve", ""))
        itemstodelete.Add(ToList("HKLM\SOFTWARE\Classes\CLSID\{CB1B7F8C-C50A-4176-B604-9E24DEE8D4D1}\InProcServer32", "ThreadingModel", ""))
        itemstodelete.Add(ToList("HKLM\SOFTWARE\Classes\CLSID\{CB1B7F8C-C50A-4176-B604-9E24DEE8D4D1}\ShellFolder", "Attributes", ""))
        itemstodelete.Add(ToList("HKLM\SOFTWARE\Classes\CLSID\{CB1B7F8C-C50A-4176-B604-9E24DEE8D4D1}\ShellFolder", "HideAsDeletePerUser", ""))
        itemstodelete.Add(ToList("HKLM\SOFTWARE\Classes\CLSID\{CB1B7F8C-C50A-4176-B604-9E24DEE8D4D1}\ShellFolder", "NoFileFolderJunction", ""))
        stringchanges.Add(ToList("HKLM\SOFTWARE\Classes\CLSID\{CB1B7F8C-C50A-4176-B604-9E24DEE8D4D1}", "/ve", "Getting Started"))
        stringchanges.Add(ToList("HKLM\SOFTWARE\Classes\CLSID\{CB1B7F8C-C50A-4176-B604-9E24DEE8D4D1}", "System.AppUserModel.ID", "Getting Started"))
        stringchanges.Add(ToList("HKLM\SOFTWARE\Classes\CLSID\{CB1B7F8C-C50A-4176-B604-9E24DEE8D4D1}", "InfoTip", "@%SystemRoot%\system32\OobeFldr.dll,-33057"))
        stringchanges.Add(ToList("HKLM\SOFTWARE\Classes\CLSID\{CB1B7F8C-C50A-4176-B604-9E24DEE8D4D1}", "LocalizedString", "@%SystemRoot%\system32\OobeFldr.dll,-33056"))
        stringchanges.Add(ToList("HKLM\SOFTWARE\Classes\CLSID\{CB1B7F8C-C50A-4176-B604-9E24DEE8D4D1}", "System.ApplicationName", "Microsoft.GettingStarted"))
        stringchanges.Add(ToList("HKLM\SOFTWARE\Classes\CLSID\{CB1B7F8C-C50A-4176-B604-9E24DEE8D4D1}", "System.AppUserModel.ID", "Microsoft.Windows.GettingStarted"))
        stringchanges.Add(ToList("HKLM\SOFTWARE\Classes\CLSID\{CB1B7F8C-C50A-4176-B604-9E24DEE8D4D1}", "System.ControlPanel.Category", "0"))
        stringchanges.Add(ToList("HKLM\SOFTWARE\Classes\CLSID\{CB1B7F8C-C50A-4176-B604-9E24DEE8D4D1}", "System.Software.TasksFileUrl", "Internal"))
        stringchanges.Add(ToList("HKLM\SOFTWARE\Classes\CLSID\{CB1B7F8C-C50A-4176-B604-9E24DEE8D4D1}\DefaultIcon", "/ve", "%SystemRoot%\branding\shellbrd\shellbrd.dll"))

        'Allow StartIsBack to run in Safe Mode
        dwordchanges.Add(ToList("HKLM\SOFTWARE\StartIsBack", "EnableInSafeMode", "1"))

        SystemTweaks.Add("DWORD", dwordchanges)
        SystemTweaks.Add("String", stringchanges)
        SystemTweaks.Add("MultiString", multistringchanges)
        SystemTweaks.Add("Binary", hexchanges)
        SystemTweaks.Add("Delete", itemstodelete)
    End Sub

    Sub PopulateUserTweaks() 'Here is where you can dump the keys to change on a user-wide basis
        Dim itemstodelete As New List(Of List(Of String))
        Dim dwordchanges As New List(Of List(Of String))
        Dim stringchanges As New List(Of List(Of String))
        Dim multistringchanges As New List(Of List(Of String))
        Dim hexchanges As New List(Of List(Of String))
        Dim itemstodeleteclass As New List(Of List(Of String))
        Dim dwordchangesclass As New List(Of List(Of String))
        Dim stringchangesclass As New List(Of List(Of String))
        Dim multistringchangesclass As New List(Of List(Of String))
        Dim hexchangesclass As New List(Of List(Of String))

        'Enable Show Desktop Peek
        dwordchanges.Add(ToList("HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisablePreviewDesktop", "0"))

        'Restore accessibility of certain sounds for editing
        itemstodelete.Add(ToList("HKCU\AppEvents\EventLabels\WindowsLogon", "ExcludeFromCPL", ""))
        itemstodelete.Add(ToList("HKCU\AppEvents\EventLabels\WindowsLogoff", "ExcludeFromCPL", ""))
        itemstodelete.Add(ToList("HKCU\AppEvents\EventLabels\SystemExit", "ExcludeFromCPL", ""))

        'Change the user's WindowMetrics
        stringchanges.Add(ToList("HKCU\Control Panel\Desktop\WindowMetrics", "SmCaptionWidth", "-330"))
        stringchanges.Add(ToList("HKCU\Control Panel\Desktop\WindowMetrics", "BorderWidth", "-15"))
        stringchanges.Add(ToList("HKCU\Control Panel\Desktop\WindowMetrics", "SmCaptionHeight", "-315"))
        stringchanges.Add(ToList("HKCU\Control Panel\Desktop\WindowMetrics", "CaptionWidth", "-330"))
        stringchanges.Add(ToList("HKCU\Control Panel\Desktop\WindowMetrics", "IconTitleWrap", "1"))
        stringchanges.Add(ToList("HKCU\Control Panel\Desktop\WindowMetrics", "ScrollHeight", "-255"))
        stringchanges.Add(ToList("HKCU\Control Panel\Desktop\WindowMetrics", "CaptionHeight", "-315"))
        hexchanges.Add(ToList("HKCU\Control Panel\Desktop\WindowMetrics", "CaptionFont", "f4ffffff0000000000000000000000009001000000000001000000005300650067006f006500200055004900000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000"))
        stringchanges.Add(ToList("HKCU\Control Panel\Desktop\WindowMetrics", "ScrollWidth", "-255"))
        stringchanges.Add(ToList("HKCU\Control Panel\Desktop\WindowMetrics", "MenuWidth", "-285"))
        hexchanges.Add(ToList("HKCU\Control Panel\Desktop\WindowMetrics", "SmCaptionFont", "f4ffffff0000000000000000000000009001000000000001000000005300650067006f006500200055004900000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000"))
        hexchanges.Add(ToList("HKCU\Control Panel\Desktop\WindowMetrics", "MenuFont", "f4ffffff0000000000000000000000009001000000000001000005005300650067006f006500200055004900000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000"))
        hexchanges.Add(ToList("HKCU\Control Panel\Desktop\WindowMetrics", "StatusFont", "f4ffffff0000000000000000000000009001000000000001000005005300650067006f006500200055004900000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000"))
        stringchanges.Add(ToList("HKCU\Control Panel\Desktop\WindowMetrics", "MenuHeight", "-285"))
        hexchanges.Add(ToList("HKCU\Control Panel\Desktop\WindowMetrics", "MessageFont", "f4ffffff0000000000000000000000009001000000000001000005005300650067006f006500200055004900000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000"))
        hexchanges.Add(ToList("HKCU\Control Panel\Desktop\WindowMetrics", "IconFont", "f4ffffff0000000000000000000000009001000000000001000005005300650067006f006500200055004900000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000"))
        stringchanges.Add(ToList("HKCU\Control Panel\Desktop\WindowMetrics", "Shell Icon Size", "40"))
        stringchanges.Add(ToList("HKCU\Control Panel\Desktop\WindowMetrics", "PaddedBorderWidth", "-60"))
        dwordchanges.Add(ToList("HKCU\Control Panel\Desktop\WindowMetrics", "AppliedDPI", "96"))
        stringchanges.Add(ToList("HKCU\Control Panel\Desktop\WindowMetrics", "IconSpacing", "-1125"))
        stringchanges.Add(ToList("HKCU\Control Panel\Desktop\WindowMetrics", "IconVerticalSpacing", "-1125"))
        stringchanges.Add(ToList("HKCU\Control Panel\Desktop\WindowMetrics", "MinAnimate", "1"))
        stringchanges.Add(ToList("HKCU\Control Panel\Desktop\WindowMetrics", "Shell Icon Size", "18"))

        'Change the user's Glass8 and DWM configuration
        dwordchanges.Add(ToList("HKCU\Software\Microsoft\Windows\DWM", "ColorizationGlassAttribute", "1"))
        dwordchanges.Add(ToList("HKCU\Software\Microsoft\Windows\DWM", "EnableAeroPeek", "1"))
        dwordchanges.Add(ToList("HKCU\Software\Microsoft\Windows\DWM", "AlwaysHibernateThumbnails", "0"))
        dwordchanges.Add(ToList("HKCU\Software\Microsoft\Windows\DWM", "ColorizationGlassReflectionIntensity", "100"))
        dwordchanges.Add(ToList("HKCU\Software\Microsoft\Windows\DWM", "TextGlowMode", "3"))
        dwordchanges.Add(ToList("HKCU\Software\Microsoft\Windows\DWM", "RoundRectRadius", "12"))
        dwordchanges.Add(ToList("HKCU\Software\Microsoft\Windows\DWM", "ColorizationColorBalance", "42"))
        dwordchanges.Add(ToList("HKCU\Software\Microsoft\Windows\DWM", "ColorizationAfterglowBalance", "10"))
        dwordchanges.Add(ToList("HKCU\Software\Microsoft\Windows\DWM", "ColorizationBlurBalance", "48"))
        dwordchanges.Add(ToList("HKCU\Software\Microsoft\Windows\DWM", "EnableWindowColorization", "1"))

        'Change the sounds over
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default", "DispFileName", "@mmres.dll,-5856"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\.Default\.Default", "/ve", "%WINDIR%\Media\Windows Ding.wav")) '%WINDIR% will be translated later
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\.Default\.Current", "/ve", "%WINDIR%\Media\Windows Ding.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\.Default\Custom", "/ve", "%WINDIR%\Media\Windows Ding.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\.Default\Afternoon", "/ve", "%WINDIR%\Media\Afternoon\Windows Ding.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\.Default\Calligraphy", "/ve", "%WINDIR%\Media\Calligraphy\Windows Ding.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\.Default\Characters", "/ve", "%WINDIR%\Media\Characters\Windows Ding.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\.Default\Cityscape", "/ve", "%WINDIR%\Media\Cityscape\Windows Ding.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\.Default\Delta", "/ve", "%WINDIR%\Media\Delta\Windows Ding.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\.Default\Festival", "/ve", "%WINDIR%\Media\Festival\Windows Ding.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\.Default\Garden", "/ve", "%WINDIR%\Media\Garden\Windows Ding.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\.Default\Heritage", "/ve", "%WINDIR%\Media\Heritage\Windows Ding.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\.Default\Landscape", "/ve", "%WINDIR%\Media\Landscape\Windows Ding.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\.Default\Quirky", "/ve", "%WINDIR%\Media\Quirky\Windows Ding.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\.Default\Raga", "/ve", "%WINDIR%\Media\Raga\Windows Ding.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\.Default\Savanna", "/ve", "%WINDIR%\Media\Savanna\Windows Ding.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\.Default\Sonata", "/ve", "%WINDIR%\Media\Sonata\Windows Ding.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\ChangeTheme\.Default", "/ve", "%WINDIR%\Media\Windows Logon Sound.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\ChangeTheme\.Current", "/ve", "%WINDIR%\Media\Windows Logon Sound.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\ChangeTheme\Custom", "/ve", "%WINDIR%\Media\Windows Logon Sound.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\ChangeTheme\Afternoon", "/ve", "%WINDIR%\Media\Afternoon\Windows Logon Sound.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\ChangeTheme\Calligraphy", "/ve", "%WINDIR%\Media\Calligraphy\Windows Logon Sound.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\ChangeTheme\Characters", "/ve", "%WINDIR%\Media\Characters\Windows Logon Sound.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\ChangeTheme\Cityscape", "/ve", "%WINDIR%\Media\Cityscape\Windows Logon Sound.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\ChangeTheme\Delta", "/ve", "%WINDIR%\Media\Delta\Windows Logon Sound.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\ChangeTheme\Festival", "/ve", "%WINDIR%\Media\Festival\Windows Logon Sound.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\ChangeTheme\Garden", "/ve", "%WINDIR%\Media\Garden\Windows Logon Sound.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\ChangeTheme\Heritage", "/ve", "%WINDIR%\Media\Heritage\Windows Logon Sound.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\ChangeTheme\Landscape", "/ve", "%WINDIR%\Media\Landscape\Windows Logon Sound.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\ChangeTheme\Quirky", "/ve", "%WINDIR%\Media\Quirky\Windows Logon Sound.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\ChangeTheme\Raga", "/ve", "%WINDIR%\Media\Raga\Windows Logon Sound.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\ChangeTheme\Savanna", "/ve", "%WINDIR%\Media\Savanna\Windows Logon Sound.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\ChangeTheme\Sonata", "/ve", "%WINDIR%\Media\Sonata\Windows Logon Sound.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\CriticalBatteryAlarm\.Default", "/ve", "%WINDIR%\Media\Windows Battery Critical.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\CriticalBatteryAlarm\.Current", "/ve", "%WINDIR%\Media\Windows Battery Critical.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\CriticalBatteryAlarm\Custom", "/ve", "%WINDIR%\Media\Windows Battery Critical.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\CriticalBatteryAlarm\Afternoon", "/ve", "%WINDIR%\Media\Afternoon\Windows Battery Critical.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\CriticalBatteryAlarm\Calligraphy", "/ve", "%WINDIR%\Media\Calligraphy\Windows Battery Critical.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\CriticalBatteryAlarm\Characters", "/ve", "%WINDIR%\Media\Characters\Windows Battery Critical.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\CriticalBatteryAlarm\Cityscape", "/ve", "%WINDIR%\Media\Cityscape\Windows Battery Critical.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\CriticalBatteryAlarm\Delta", "/ve", "%WINDIR%\Media\Delta\Windows Battery Critical.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\CriticalBatteryAlarm\Festival", "/ve", "%WINDIR%\Media\Festival\Windows Battery Critical.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\CriticalBatteryAlarm\Garden", "/ve", "%WINDIR%\Media\Garden\Windows Battery Critical.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\CriticalBatteryAlarm\Heritage", "/ve", "%WINDIR%\Media\Heritage\Windows Battery Critical.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\CriticalBatteryAlarm\Landscape", "/ve", "%WINDIR%\Media\Landscape\Windows Battery Critical.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\CriticalBatteryAlarm\Quirky", "/ve", "%WINDIR%\Media\Quirky\Windows Battery Critical.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\CriticalBatteryAlarm\Raga", "/ve", "%WINDIR%\Media\Raga\Windows Battery Critical.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\CriticalBatteryAlarm\Savanna", "/ve", "%WINDIR%\Media\Savanna\Windows Battery Critical.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\CriticalBatteryAlarm\Sonata", "/ve", "%WINDIR%\Media\Sonata\Windows Battery Critical.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\DeviceConnect\.Default", "/ve", "%WINDIR%\Media\Windows Hardware Insert.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\DeviceConnect\.Current", "/ve", "%WINDIR%\Media\Windows Hardware Insert.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\DeviceConnect\Custom", "/ve", "%WINDIR%\Media\Windows Hardware Insert.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\DeviceConnect\Afternoon", "/ve", "%WINDIR%\Media\Afternoon\Windows Hardware Insert.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\DeviceConnect\Calligraphy", "/ve", "%WINDIR%\Media\Calligraphy\Windows Hardware Insert.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\DeviceConnect\Characters", "/ve", "%WINDIR%\Media\Characters\Windows Hardware Insert.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\DeviceConnect\Cityscape", "/ve", "%WINDIR%\Media\Cityscape\Windows Hardware Insert.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\DeviceConnect\Delta", "/ve", "%WINDIR%\Media\Delta\Windows Hardware Insert.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\DeviceConnect\Festival", "/ve", "%WINDIR%\Media\Festival\Windows Hardware Insert.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\DeviceConnect\Garden", "/ve", "%WINDIR%\Media\Garden\Windows Hardware Insert.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\DeviceConnect\Heritage", "/ve", "%WINDIR%\Media\Heritage\Windows Hardware Insert.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\DeviceConnect\Landscape", "/ve", "%WINDIR%\Media\Landscape\Windows Hardware Insert.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\DeviceConnect\Quirky", "/ve", "%WINDIR%\Media\Quirky\Windows Hardware Insert.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\DeviceConnect\Raga", "/ve", "%WINDIR%\Media\Raga\Windows Hardware Insert.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\DeviceConnect\Savanna", "/ve", "%WINDIR%\Media\Savanna\Windows Hardware Insert.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\DeviceConnect\Sonata", "/ve", "%WINDIR%\Media\Sonata\Windows Hardware Insert.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\DeviceConnect\.Default", "/ve", "%WINDIR%\Media\Windows Hardware Insert.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\DeviceConnect\.Current", "/ve", "%WINDIR%\Media\Windows Hardware Insert.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\DeviceConnect\Custom", "/ve", "%WINDIR%\Media\Windows Hardware Insert.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\DeviceDisconnect\.Default", "/ve", "%WINDIR%\Media\Windows Hardware Remove.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\DeviceDisconnect\.Current", "/ve", "%WINDIR%\Media\Windows Hardware Remove.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\DeviceDisconnect\Custom", "/ve", "%WINDIR%\Media\Windows Hardware Remove.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\DeviceDisconnect\Afternoon", "/ve", "%WINDIR%\Media\Afternoon\Windows Hardware Remove.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\DeviceDisconnect\Calligraphy", "/ve", "%WINDIR%\Media\Calligraphy\Windows Hardware Remove.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\DeviceDisconnect\Characters", "/ve", "%WINDIR%\Media\Characters\Windows Hardware Remove.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\DeviceDisconnect\Cityscape", "/ve", "%WINDIR%\Media\Cityscape\Windows Hardware Remove.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\DeviceDisconnect\Delta", "/ve", "%WINDIR%\Media\Delta\Windows Hardware Remove.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\DeviceDisconnect\Festival", "/ve", "%WINDIR%\Media\Festival\Windows Hardware Remove.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\DeviceDisconnect\Garden", "/ve", "%WINDIR%\Media\Garden\Windows Hardware Remove.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\DeviceDisconnect\Heritage", "/ve", "%WINDIR%\Media\Heritage\Windows Hardware Remove.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\DeviceDisconnect\Landscape", "/ve", "%WINDIR%\Media\Landscape\Windows Hardware Remove.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\DeviceDisconnect\Quirky", "/ve", "%WINDIR%\Media\Quirky\Windows Hardware Remove.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\DeviceDisconnect\Raga", "/ve", "%WINDIR%\Media\Raga\Windows Hardware Remove.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\DeviceDisconnect\Savanna", "/ve", "%WINDIR%\Media\Savanna\Windows Hardware Remove.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\DeviceDisconnect\Sonata", "/ve", "%WINDIR%\Media\Sonata\Windows Hardware Remove.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\DeviceFail\.Default", "/ve", "%WINDIR%\Media\Windows Hardware Fail.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\DeviceFail\.Current", "/ve", "%WINDIR%\Media\Windows Hardware Fail.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\DeviceFail\Custom", "/ve", "%WINDIR%\Media\Windows Hardware Fail.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\DeviceFail\Afternoon", "/ve", "%WINDIR%\Media\Afternoon\Windows Hardware Fail.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\DeviceFail\Calligraphy", "/ve", "%WINDIR%\Media\Calligraphy\Windows Hardware Fail.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\DeviceFail\Characters", "/ve", "%WINDIR%\Media\Characters\Windows Hardware Fail.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\DeviceFail\Cityscape", "/ve", "%WINDIR%\Media\Cityscape\Windows Hardware Fail.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\DeviceFail\Delta", "/ve", "%WINDIR%\Media\Delta\Windows Hardware Fail.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\DeviceFail\Festival", "/ve", "%WINDIR%\Media\Festival\Windows Hardware Fail.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\DeviceFail\Garden", "/ve", "%WINDIR%\Media\Garden\Windows Hardware Fail.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\DeviceFail\Heritage", "/ve", "%WINDIR%\Media\Heritage\Windows Hardware Fail.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\DeviceFail\Landscape", "/ve", "%WINDIR%\Media\Landscape\Windows Hardware Fail.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\DeviceFail\Quirky", "/ve", "%WINDIR%\Media\Quirky\Windows Hardware Fail.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\DeviceFail\Raga", "/ve", "%WINDIR%\Media\Raga\Windows Hardware Fail.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\DeviceFail\Savanna", "/ve", "%WINDIR%\Media\Savanna\Windows Hardware Fail.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\DeviceFail\Sonata", "/ve", "%WINDIR%\Media\Sonata\Windows Hardware Fail.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\FaxBeep\.Default", "/ve", "%WINDIR%\Media\Windows Notify.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\FaxBeep\.Current", "/ve", "%WINDIR%\Media\Windows Notify.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\FaxBeep\Custom", "/ve", "%WINDIR%\Media\Windows Notify.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\FaxBeep\Afternoon", "/ve", "%WINDIR%\Media\Afternoon\Windows Notify.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\FaxBeep\Calligraphy", "/ve", "%WINDIR%\Media\Calligraphy\Windows Notify.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\FaxBeep\Characters", "/ve", "%WINDIR%\Media\Characters\Windows Notify.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\FaxBeep\Cityscape", "/ve", "%WINDIR%\Media\Cityscape\Windows Notify.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\FaxBeep\Delta", "/ve", "%WINDIR%\Media\Delta\Windows Notify.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\FaxBeep\Festival", "/ve", "%WINDIR%\Media\Festival\Windows Notify.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\FaxBeep\Garden", "/ve", "%WINDIR%\Media\Garden\Windows Notify.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\FaxBeep\Heritage", "/ve", "%WINDIR%\Media\Heritage\Windows Notify.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\FaxBeep\Landscape", "/ve", "%WINDIR%\Media\Landscape\Windows Notify.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\FaxBeep\Quirky", "/ve", "%WINDIR%\Media\Quirky\Windows Notify.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\FaxBeep\Raga", "/ve", "%WINDIR%\Media\Raga\Windows Notify.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\FaxBeep\Savanna", "/ve", "%WINDIR%\Media\Savanna\Windows Notify.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\FaxBeep\Sonata", "/ve", "%WINDIR%\Media\Sonata\Windows Notify.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\LowBatteryAlarm\.Default", "/ve", "%WINDIR%\Media\Windows Battery Low.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\LowBatteryAlarm\.Current", "/ve", "%WINDIR%\Media\Windows Battery Low.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\LowBatteryAlarm\Custom", "/ve", "%WINDIR%\Media\Windows Battery Low.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\LowBatteryAlarm\Afternoon", "/ve", "%WINDIR%\Media\Afternoon\Windows Battery Low.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\LowBatteryAlarm\Calligraphy", "/ve", "%WINDIR%\Media\Calligraphy\Windows Battery Low.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\LowBatteryAlarm\Characters", "/ve", "%WINDIR%\Media\Characters\Windows Battery Low.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\LowBatteryAlarm\Cityscape", "/ve", "%WINDIR%\Media\Cityscape\Windows Battery Low.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\LowBatteryAlarm\Delta", "/ve", "%WINDIR%\Media\Delta\Windows Battery Low.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\LowBatteryAlarm\Festival", "/ve", "%WINDIR%\Media\Festival\Windows Battery Low.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\LowBatteryAlarm\Garden", "/ve", "%WINDIR%\Media\Garden\Windows Battery Low.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\LowBatteryAlarm\Heritage", "/ve", "%WINDIR%\Media\Heritage\Windows Battery Low.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\LowBatteryAlarm\Landscape", "/ve", "%WINDIR%\Media\Landscape\Windows Battery Low.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\LowBatteryAlarm\Quirky", "/ve", "%WINDIR%\Media\Quirky\Windows Battery Low.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\LowBatteryAlarm\Raga", "/ve", "%WINDIR%\Media\Raga\Windows Battery Low.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\LowBatteryAlarm\Savanna", "/ve", "%WINDIR%\Media\Savanna\Windows Battery Low.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\LowBatteryAlarm\Sonata", "/ve", "%WINDIR%\Media\Sonata\Windows Battery Low.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\MailBeep\.Default", "/ve", "%WINDIR%\Media\Windows Notify.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\MailBeep\.Current", "/ve", "%WINDIR%\Media\Windows Notify.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\MailBeep\Custom", "/ve", "%WINDIR%\Media\Windows Notify.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\MailBeep\Afternoon", "/ve", "%WINDIR%\Media\Afternoon\Windows Notify.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\MailBeep\Calligraphy", "/ve", "%WINDIR%\Media\Calligraphy\Windows Notify.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\MailBeep\Characters", "/ve", "%WINDIR%\Media\Characters\Windows Notify.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\MailBeep\Cityscape", "/ve", "%WINDIR%\Media\Cityscape\Windows Notify.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\MailBeep\Delta", "/ve", "%WINDIR%\Media\Delta\Windows Notify.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\MailBeep\Festival", "/ve", "%WINDIR%\Media\Festival\Windows Notify.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\MailBeep\Garden", "/ve", "%WINDIR%\Media\Garden\Windows Notify.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\MailBeep\Heritage", "/ve", "%WINDIR%\Media\Heritage\Windows Notify.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\MailBeep\Landscape", "/ve", "%WINDIR%\Media\Landscape\Windows Notify.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\MailBeep\Quirky", "/ve", "%WINDIR%\Media\Quirky\Windows Notify.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\MailBeep\Raga", "/ve", "%WINDIR%\Media\Raga\Windows Notify.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\MailBeep\Savanna", "/ve", "%WINDIR%\Media\Savanna\Windows Notify.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\MailBeep\Sonata", "/ve", "%WINDIR%\Media\Sonata\Windows Notify.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\PrintComplete\.Default", "/ve", "%WINDIR%\Media\Windows Print complete.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\PrintComplete\.Current", "/ve", "%WINDIR%\Media\Windows Print complete.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\PrintComplete\Custom", "/ve", "%WINDIR%\Media\Windows Print complete.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\PrintComplete\Afternoon", "/ve", "%WINDIR%\Media\Afternoon\Windows Print complete.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\PrintComplete\Calligraphy", "/ve", "%WINDIR%\Media\Calligraphy\Windows Print complete.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\PrintComplete\Characters", "/ve", "%WINDIR%\Media\Characters\Windows Print complete.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\PrintComplete\Cityscape", "/ve", "%WINDIR%\Media\Cityscape\Windows Print complete.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\PrintComplete\Delta", "/ve", "%WINDIR%\Media\Delta\Windows Print complete.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\PrintComplete\Festival", "/ve", "%WINDIR%\Media\Festival\Windows Print complete.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\PrintComplete\Garden", "/ve", "%WINDIR%\Media\Garden\Windows Print complete.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\PrintComplete\Heritage", "/ve", "%WINDIR%\Media\Heritage\Windows Print complete.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\PrintComplete\Landscape", "/ve", "%WINDIR%\Media\Landscape\Windows Print complete.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\PrintComplete\Quirky", "/ve", "%WINDIR%\Media\Quirky\Windows Print complete.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\PrintComplete\Raga", "/ve", "%WINDIR%\Media\Raga\Windows Print complete.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\PrintComplete\Savanna", "/ve", "%WINDIR%\Media\Savanna\Windows Print complete.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\PrintComplete\Sonata", "/ve", "%WINDIR%\Media\Sonata\Windows Print complete.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemAsterisk\.Default", "/ve", "%WINDIR%\Media\Windows Error.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemAsterisk\.Current", "/ve", "%WINDIR%\Media\Windows Error.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemAsterisk\Custom", "/ve", "%WINDIR%\Media\Windows Error.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemAsterisk\Afternoon", "/ve", "%WINDIR%\Media\Afternoon\Windows Error.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemAsterisk\Calligraphy", "/ve", "%WINDIR%\Media\Calligraphy\Windows Error.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemAsterisk\Characters", "/ve", "%WINDIR%\Media\Characters\Windows Error.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemAsterisk\Cityscape", "/ve", "%WINDIR%\Media\Cityscape\Windows Error.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemAsterisk\Delta", "/ve", "%WINDIR%\Media\Delta\Windows Error.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemAsterisk\Festival", "/ve", "%WINDIR%\Media\Festival\Windows Error.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemAsterisk\Garden", "/ve", "%WINDIR%\Media\Garden\Windows Error.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemAsterisk\Heritage", "/ve", "%WINDIR%\Media\Heritage\Windows Error.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemAsterisk\Landscape", "/ve", "%WINDIR%\Media\Landscape\Windows Error.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemAsterisk\Quirky", "/ve", "%WINDIR%\Media\Quirky\Windows Error.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemAsterisk\Raga", "/ve", "%WINDIR%\Media\Raga\Windows Error.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemAsterisk\Savanna", "/ve", "%WINDIR%\Media\Savanna\Windows Error.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemAsterisk\Sonata", "/ve", "%WINDIR%\Media\Sonata\Windows Error.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemExclamation\.Default", "/ve", "%WINDIR%\Media\Windows Exclamation.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemExclamation\.Current", "/ve", "%WINDIR%\Media\Windows Exclamation.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemExclamation\Custom", "/ve", "%WINDIR%\Media\Windows Exclamation.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemExclamation\Afternoon", "/ve", "%WINDIR%\Media\Afternoon\Windows Exclamation.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemExclamation\Calligraphy", "/ve", "%WINDIR%\Media\Calligraphy\Windows Exclamation.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemExclamation\Characters", "/ve", "%WINDIR%\Media\Characters\Windows Exclamation.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemExclamation\Cityscape", "/ve", "%WINDIR%\Media\Cityscape\Windows Exclamation.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemExclamation\Delta", "/ve", "%WINDIR%\Media\Delta\Windows Exclamation.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemExclamation\Festival", "/ve", "%WINDIR%\Media\Festival\Windows Exclamation.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemExclamation\Garden", "/ve", "%WINDIR%\Media\Garden\Windows Exclamation.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemExclamation\Heritage", "/ve", "%WINDIR%\Media\Heritage\Windows Exclamation.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemExclamation\Landscape", "/ve", "%WINDIR%\Media\Landscape\Windows Exclamation.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemExclamation\Quirky", "/ve", "%WINDIR%\Media\Quirky\Windows Exclamation.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemExclamation\Raga", "/ve", "%WINDIR%\Media\Raga\Windows Exclamation.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemExclamation\Savanna", "/ve", "%WINDIR%\Media\Savanna\Windows Exclamation.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemExclamation\Sonata", "/ve", "%WINDIR%\Media\Sonata\Windows Exclamation.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemExit\.Default", "/ve", "%WINDIR%\Media\Windows Shutdown.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemExit\.Current", "/ve", "%WINDIR%\Media\Windows Shutdown.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemExit\Custom", "/ve", "%WINDIR%\Media\Windows Shutdown.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemExit\Afternoon", "/ve", "%WINDIR%\Media\Windows Shutdown.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemExit\Calligraphy", "/ve", "%WINDIR%\Media\Windows Shutdown.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemExit\Characters", "/ve", "%WINDIR%\Media\Windows Shutdown.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemExit\Cityscape", "/ve", "%WINDIR%\Media\Windows Shutdown.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemExit\Delta", "/ve", "%WINDIR%\Media\Windows Shutdown.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemExit\Festival", "/ve", "%WINDIR%\Media\Windows Shutdown.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemExit\Garden", "/ve", "%WINDIR%\Media\Windows Shutdown.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemExit\Heritage", "/ve", "%WINDIR%\Media\Windows Shutdown.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemExit\Landscape", "/ve", "%WINDIR%\Media\Windows Shutdown.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemExit\Quirky", "/ve", "%WINDIR%\Media\Windows Shutdown.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemExit\Raga", "/ve", "%WINDIR%\Media\Windows Shutdown.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemExit\Savanna", "/ve", "%WINDIR%\Media\Windows Shutdown.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemExit\Sonata", "/ve", "%WINDIR%\Media\Windows Shutdown.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemHand\.Default", "/ve", "%WINDIR%\Media\Windows Critical Stop.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemHand\.Current", "/ve", "%WINDIR%\Media\Windows Critical Stop.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemHand\Custom", "/ve", "%WINDIR%\Media\Windows Critical Stop.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemHand\Afternoon", "/ve", "%WINDIR%\Media\Afternoon\Windows Critical Stop.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemHand\Calligraphy", "/ve", "%WINDIR%\Media\Calligraphy\Windows Critical Stop.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemHand\Characters", "/ve", "%WINDIR%\Media\Characters\Windows Critical Stop.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemHand\Cityscape", "/ve", "%WINDIR%\Media\Cityscape\Windows Critical Stop.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemHand\Delta", "/ve", "%WINDIR%\Media\Delta\Windows Critical Stop.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemHand\Festival", "/ve", "%WINDIR%\Media\Festival\Windows Critical Stop.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemHand\Garden", "/ve", "%WINDIR%\Media\Garden\Windows Critical Stop.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemHand\Heritage", "/ve", "%WINDIR%\Media\Heritage\Windows Critical Stop.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemHand\Landscape", "/ve", "%WINDIR%\Media\Landscape\Windows Critical Stop.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemHand\Quirky", "/ve", "%WINDIR%\Media\Quirky\Windows Critical Stop.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemHand\Raga", "/ve", "%WINDIR%\Media\Raga\Windows Critical Stop.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemHand\Savanna", "/ve", "%WINDIR%\Media\Savanna\Windows Critical Stop.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemHand\Sonata", "/ve", "%WINDIR%\Media\Sonata\Windows Critical Stop.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemNotification\.Default", "/ve", "%WINDIR%\Media\Windows Balloon.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemNotification\.Current", "/ve", "%WINDIR%\Media\Windows Balloon.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemNotification\Custom", "/ve", "%WINDIR%\Media\Windows Balloon.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemNotification\Afternoon", "/ve", "%WINDIR%\Media\Afternoon\Windows Balloon.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemNotification\Calligraphy", "/ve", "%WINDIR%\Media\Calligraphy\Windows Balloon.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemNotification\Characters", "/ve", "%WINDIR%\Media\Characters\Windows Balloon.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemNotification\Cityscape", "/ve", "%WINDIR%\Media\Cityscape\Windows Balloon.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemNotification\Delta", "/ve", "%WINDIR%\Media\Delta\Windows Balloon.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemNotification\Festival", "/ve", "%WINDIR%\Media\Festival\Windows Balloon.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemNotification\Garden", "/ve", "%WINDIR%\Media\Garden\Windows Balloon.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemNotification\Heritage", "/ve", "%WINDIR%\Media\Heritage\Windows Balloon.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemNotification\Landscape", "/ve", "%WINDIR%\Media\Landscape\Windows Balloon.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemNotification\Quirky", "/ve", "%WINDIR%\Media\Quirky\Windows Balloon.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemNotification\Raga", "/ve", "%WINDIR%\Media\Raga\Windows Balloon.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemNotification\Savanna", "/ve", "%WINDIR%\Media\Savanna\Windows Balloon.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\SystemNotification\Sonata", "/ve", "%WINDIR%\Media\Sonata\Windows Balloon.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\WindowsLogoff\.Default", "/ve", "%WINDIR%\Media\Windows Logoff Sound.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\WindowsLogoff\.Current", "/ve", "%WINDIR%\Media\Windows Logoff Sound.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\WindowsLogoff\Custom", "/ve", "%WINDIR%\Media\Windows Logoff Sound.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\WindowsLogoff\Afternoon", "/ve", "%WINDIR%\Media\Afternoon\Windows Logoff Sound.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\WindowsLogoff\Calligraphy", "/ve", "%WINDIR%\Media\Calligraphy\Windows Logoff Sound.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\WindowsLogoff\Characters", "/ve", "%WINDIR%\Media\Characters\Windows Logoff Sound.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\WindowsLogoff\Cityscape", "/ve", "%WINDIR%\Media\Cityscape\Windows Logoff Sound.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\WindowsLogoff\Delta", "/ve", "%WINDIR%\Media\Delta\Windows Logoff Sound.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\WindowsLogoff\Festival", "/ve", "%WINDIR%\Media\Festival\Windows Logoff Sound.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\WindowsLogoff\Garden", "/ve", "%WINDIR%\Media\Garden\Windows Logoff Sound.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\WindowsLogoff\Heritage", "/ve", "%WINDIR%\Media\Heritage\Windows Logoff Sound.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\WindowsLogoff\Landscape", "/ve", "%WINDIR%\Media\Landscape\Windows Logoff Sound.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\WindowsLogoff\Quirky", "/ve", "%WINDIR%\Media\Quirky\Windows Logoff Sound.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\WindowsLogoff\Raga", "/ve", "%WINDIR%\Media\Raga\Windows Logoff Sound.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\WindowsLogoff\Savanna", "/ve", "%WINDIR%\Media\Savanna\Windows Logoff Sound.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\WindowsLogoff\Sonata", "/ve", "%WINDIR%\Media\Sonata\Windows Logoff Sound.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\WindowsLogon\.Default", "/ve", "%WINDIR%\Media\Windows Logon Sound.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\WindowsLogon\.Current", "/ve", "%WINDIR%\Media\Windows Logon Sound.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\WindowsLogon\Custom", "/ve", "%WINDIR%\Media\Windows Logon Sound.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\WindowsLogon\Afternoon", "/ve", "%WINDIR%\Media\Afternoon\Windows Logon Sound.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\WindowsLogon\Calligraphy", "/ve", "%WINDIR%\Media\Calligraphy\Windows Logon Sound.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\WindowsLogon\Characters", "/ve", "%WINDIR%\Media\Characters\Windows Logon Sound.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\WindowsLogon\Cityscape", "/ve", "%WINDIR%\Media\Cityscape\Windows Logon Sound.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\WindowsLogon\Delta", "/ve", "%WINDIR%\Media\Delta\Windows Logon Sound.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\WindowsLogon\Festival", "/ve", "%WINDIR%\Media\Festival\Windows Logon Sound.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\WindowsLogon\Garden", "/ve", "%WINDIR%\Media\Garden\Windows Logon Sound.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\WindowsLogon\Heritage", "/ve", "%WINDIR%\Media\Heritage\Windows Logon Sound.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\WindowsLogon\Landscape", "/ve", "%WINDIR%\Media\Landscape\Windows Logon Sound.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\WindowsLogon\Quirky", "/ve", "%WINDIR%\Media\Quirky\Windows Logon Sound.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\WindowsLogon\Raga", "/ve", "%WINDIR%\Media\Raga\Windows Logon Sound.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\WindowsLogon\Savanna", "/ve", "%WINDIR%\Media\Savanna\Windows Logon Sound.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\WindowsLogon\Sonata", "/ve", "%WINDIR%\Media\Sonata\Windows Logon Sound.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\WindowsUAC\.Default", "/ve", "%WINDIR%\Media\Windows User Account Control.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\WindowsUAC\.Current", "/ve", "%WINDIR%\Media\Windows User Account Control.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\WindowsUAC\Custom", "/ve", "%WINDIR%\Media\Windows User Account Control.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\WindowsUAC\Afternoon", "/ve", "%WINDIR%\Media\Afternoon\Windows User Account Control.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\WindowsUAC\Calligraphy", "/ve", "%WINDIR%\Media\Calligraphy\Windows User Account Control.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\WindowsUAC\Characters", "/ve", "%WINDIR%\Media\Characters\Windows User Account Control.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\WindowsUAC\Cityscape", "/ve", "%WINDIR%\Media\Cityscape\Windows User Account Control.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\WindowsUAC\Delta", "/ve", "%WINDIR%\Media\Delta\Windows User Account Control.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\WindowsUAC\Festival", "/ve", "%WINDIR%\Media\Festival\Windows User Account Control.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\WindowsUAC\Garden", "/ve", "%WINDIR%\Media\Garden\Windows User Account Control.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\WindowsUAC\Heritage", "/ve", "%WINDIR%\Media\Heritage\Windows User Account Control.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\WindowsUAC\Landscape", "/ve", "%WINDIR%\Media\Landscape\Windows User Account Control.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\WindowsUAC\Quirky", "/ve", "%WINDIR%\Media\Quirky\Windows User Account Control.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\WindowsUAC\Raga", "/ve", "%WINDIR%\Media\Raga\Windows User Account Control.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\WindowsUAC\Savanna", "/ve", "%WINDIR%\Media\Savanna\Windows User Account Control.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\.Default\WindowsUAC\Sonata", "/ve", "%WINDIR%\Media\Sonata\Windows User Account Control.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\BlockedPopup\.Default", "/ve", "%WINDIR%\Media\Windows Pop-up Blocked.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\BlockedPopup\.Current", "/ve", "%WINDIR%\Media\Windows Pop-up Blocked.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\BlockedPopup\Custom", "/ve", "%WINDIR%\Media\Windows Pop-up Blocked.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\BlockedPopup\Afternoon", "/ve", "%WINDIR%\Media\Afternoon\Windows Pop-up Blocked.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\BlockedPopup\Calligraphy", "/ve", "%WINDIR%\Media\Calligraphy\Windows Pop-up Blocked.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\BlockedPopup\Characters", "/ve", "%WINDIR%\Media\Characters\Windows Pop-up Blocked.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\BlockedPopup\Cityscape", "/ve", "%WINDIR%\Media\Cityscape\Windows Pop-up Blocked.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\BlockedPopup\Delta", "/ve", "%WINDIR%\Media\Delta\Windows Pop-up Blocked.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\BlockedPopup\Festival", "/ve", "%WINDIR%\Media\Festival\Windows Pop-up Blocked.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\BlockedPopup\Garden", "/ve", "%WINDIR%\Media\Garden\Windows Pop-up Blocked.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\BlockedPopup\Heritage", "/ve", "%WINDIR%\Media\Heritage\Windows Pop-up Blocked.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\BlockedPopup\Landscape", "/ve", "%WINDIR%\Media\Landscape\Windows Pop-up Blocked.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\BlockedPopup\Quirky", "/ve", "%WINDIR%\Media\Quirky\Windows Pop-up Blocked.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\BlockedPopup\Raga", "/ve", "%WINDIR%\Media\Raga\Windows Pop-up Blocked.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\BlockedPopup\Savanna", "/ve", "%WINDIR%\Media\Savanna\Windows Pop-up Blocked.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\BlockedPopup\Sonata", "/ve", "%WINDIR%\Media\Sonata\Windows Pop-up Blocked.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\EmptyRecycleBin\.Default", "/ve", "%WINDIR%\Media\Windows Recycle.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\EmptyRecycleBin\.Current", "/ve", "%WINDIR%\Media\Windows Recycle.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\EmptyRecycleBin\Custom", "/ve", "%WINDIR%\Media\Windows Recycle.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\EmptyRecycleBin\Afternoon", "/ve", "%WINDIR%\Media\Windows Recycle.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\EmptyRecycleBin\Calligraphy", "/ve", "%WINDIR%\Media\Windows Recycle.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\EmptyRecycleBin\Characters", "/ve", "%WINDIR%\Media\Windows Recycle.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\EmptyRecycleBin\Cityscape", "/ve", "%WINDIR%\Media\Windows Recycle.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\EmptyRecycleBin\Delta", "/ve", "%WINDIR%\Media\Windows Recycle.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\EmptyRecycleBin\Festival", "/ve", "%WINDIR%\Media\Windows Recycle.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\EmptyRecycleBin\Garden", "/ve", "%WINDIR%\Media\Windows Recycle.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\EmptyRecycleBin\Heritage", "/ve", "%WINDIR%\Media\Windows Recycle.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\EmptyRecycleBin\Landscape", "/ve", "%WINDIR%\Media\Windows Recycle.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\EmptyRecycleBin\Quirky", "/ve", "%WINDIR%\Media\Windows Recycle.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\EmptyRecycleBin\Raga", "/ve", "%WINDIR%\Media\Windows Recycle.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\EmptyRecycleBin\Savanna", "/ve", "%WINDIR%\Media\Windows Recycle.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\EmptyRecycleBin\Sonata", "/ve", "%WINDIR%\Media\Windows Recycle.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FaxError\.Default", "/ve", "%WINDIR%\Media\ding.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FaxError\.Current", "/ve", "%WINDIR%\Media\ding.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FaxError\Custom", "/ve", "%WINDIR%\Media\ding.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FaxError\Afternoon", "/ve", "%WINDIR%\Media\ding.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FaxError\Calligraphy", "/ve", "%WINDIR%\Media\ding.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FaxError\Characters", "/ve", "%WINDIR%\Media\ding.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FaxError\Cityscape", "/ve", "%WINDIR%\Media\ding.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FaxError\Delta", "/ve", "%WINDIR%\Media\ding.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FaxError\Festival", "/ve", "%WINDIR%\Media\ding.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FaxError\Garden", "/ve", "%WINDIR%\Media\ding.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FaxError\Heritage", "/ve", "%WINDIR%\Media\ding.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FaxError\Landscape", "/ve", "%WINDIR%\Media\ding.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FaxError\Quirky", "/ve", "%WINDIR%\Media\ding.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FaxError\Raga", "/ve", "%WINDIR%\Media\ding.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FaxError\Savanna", "/ve", "%WINDIR%\Media\ding.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FaxError\Sonata", "/ve", "%WINDIR%\Media\ding.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FaxLineRings\.Default", "/ve", "%WINDIR%\Media\Windows Ringin.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FaxLineRings\.Current", "/ve", "%WINDIR%\Media\Windows Ringin.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FaxLineRings\Custom", "/ve", "%WINDIR%\Media\Windows Ringin.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FaxLineRings\Afternoon", "/ve", "%WINDIR%\Media\Windows Ringin.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FaxLineRings\Calligraphy", "/ve", "%WINDIR%\Media\Windows Ringin.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FaxLineRings\Characters", "/ve", "%WINDIR%\Media\Windows Ringin.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FaxLineRings\Cityscape", "/ve", "%WINDIR%\Media\Windows Ringin.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FaxLineRings\Delta", "/ve", "%WINDIR%\Media\Windows Ringin.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FaxLineRings\Festival", "/ve", "%WINDIR%\Media\Windows Ringin.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FaxLineRings\Garden", "/ve", "%WINDIR%\Media\Windows Ringin.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FaxLineRings\Heritage", "/ve", "%WINDIR%\Media\Windows Ringin.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FaxLineRings\Landscape", "/ve", "%WINDIR%\Media\Windows Ringin.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FaxLineRings\Quirky", "/ve", "%WINDIR%\Media\Windows Ringin.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FaxLineRings\Raga", "/ve", "%WINDIR%\Media\Windows Ringin.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FaxLineRings\Savanna", "/ve", "%WINDIR%\Media\Windows Ringin.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FaxLineRings\Sonata", "/ve", "%WINDIR%\Media\Windows Ringin.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FaxSent\.Default", "/ve", "%WINDIR%\Media\tada.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FaxSent\.Current", "/ve", "%WINDIR%\Media\tada.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FaxSent\Custom", "/ve", "%WINDIR%\Media\tada.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FaxSent\Afternoon", "/ve", "%WINDIR%\Media\tada.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FaxSent\Calligraphy", "/ve", "%WINDIR%\Media\tada.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FaxSent\Characters", "/ve", "%WINDIR%\Media\tada.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FaxSent\Cityscape", "/ve", "%WINDIR%\Media\tada.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FaxSent\Delta", "/ve", "%WINDIR%\Media\tada.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FaxSent\Festival", "/ve", "%WINDIR%\Media\tada.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FaxSent\Garden", "/ve", "%WINDIR%\Media\tada.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FaxSent\Heritage", "/ve", "%WINDIR%\Media\tada.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FaxSent\Landscape", "/ve", "%WINDIR%\Media\tada.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FaxSent\Quirky", "/ve", "%WINDIR%\Media\tada.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FaxSent\Raga", "/ve", "%WINDIR%\Media\tada.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FaxSent\Savanna", "/ve", "%WINDIR%\Media\tada.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FaxSent\Sonata", "/ve", "%WINDIR%\Media\tada.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FeedDiscovered\.Default", "/ve", "%WINDIR%\Media\Windows Feed Discovered.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FeedDiscovered\.Current", "/ve", "%WINDIR%\Media\Windows Feed Discovered.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FeedDiscovered\Custom", "/ve", "%WINDIR%\Media\Windows Feed Discovered.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FeedDiscovered\Afternoon", "/ve", "%WINDIR%\Media\Afternoon\Windows Feed Discovered.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FeedDiscovered\Calligraphy", "/ve", "%WINDIR%\Media\Calligraphy\Windows Feed Discovered.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FeedDiscovered\Characters", "/ve", "%WINDIR%\Media\Characters\Windows Feed Discovered.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FeedDiscovered\Cityscape", "/ve", "%WINDIR%\Media\Cityscape\Windows Feed Discovered.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FeedDiscovered\Delta", "/ve", "%WINDIR%\Media\Delta\Windows Feed Discovered.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FeedDiscovered\Festival", "/ve", "%WINDIR%\Media\Festival\Windows Feed Discovered.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FeedDiscovered\Garden", "/ve", "%WINDIR%\Media\Garden\Windows Feed Discovered.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FeedDiscovered\Heritage", "/ve", "%WINDIR%\Media\Heritage\Windows Feed Discovered.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FeedDiscovered\Landscape", "/ve", "%WINDIR%\Media\Landscape\Windows Feed Discovered.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FeedDiscovered\Quirky", "/ve", "%WINDIR%\Media\Quirky\Windows Feed Discovered.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FeedDiscovered\Raga", "/ve", "%WINDIR%\Media\Raga\Windows Feed Discovered.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FeedDiscovered\Savanna", "/ve", "%WINDIR%\Media\Savanna\Windows Feed Discovered.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\FeedDiscovered\Sonata", "/ve", "%WINDIR%\Media\Sonata\Windows Feed Discovered.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\Navigating\.Default", "/ve", "%WINDIR%\Media\Windows Navigation Start.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\Navigating\.Current", "/ve", "%WINDIR%\Media\Windows Navigation Start.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\Navigating\Custom", "/ve", "%WINDIR%\Media\Windows Navigation Start.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\Navigating\Afternoon", "/ve", "%WINDIR%\Media\Afternoon\Windows Navigation Start.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\Navigating\Calligraphy", "/ve", "%WINDIR%\Media\Calligraphy\Windows Navigation Start.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\Navigating\Characters", "/ve", "%WINDIR%\Media\Characters\Windows Navigation Start.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\Navigating\Cityscape", "/ve", "%WINDIR%\Media\Cityscape\Windows Navigation Start.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\Navigating\Delta", "/ve", "%WINDIR%\Media\Delta\Windows Navigation Start.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\Navigating\Festival", "/ve", "%WINDIR%\Media\Festival\Windows Navigation Start.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\Navigating\Garden", "/ve", "%WINDIR%\Media\Garden\Windows Navigation Start.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\Navigating\Heritage", "/ve", "%WINDIR%\Media\Heritage\Windows Navigation Start.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\Navigating\Landscape", "/ve", "%WINDIR%\Media\Landscape\Windows Navigation Start.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\Navigating\Quirky", "/ve", "%WINDIR%\Media\Quirky\Windows Navigation Start.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\Navigating\Raga", "/ve", "%WINDIR%\Media\Raga\Windows Navigation Start.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\Navigating\Savanna", "/ve", "%WINDIR%\Media\Savanna\Windows Navigation Start.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\Navigating\Sonata", "/ve", "%WINDIR%\Media\Sonata\Windows Navigation Start.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\SecurityBand\.Default", "/ve", "%WINDIR%\Media\Windows Information Bar.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\SecurityBand\.Current", "/ve", "%WINDIR%\Media\Windows Information Bar.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\SecurityBand\Custom", "/ve", "%WINDIR%\Media\Windows Information Bar.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\SecurityBand\Afternoon", "/ve", "%WINDIR%\Media\Afternoon\Windows Information Bar.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\SecurityBand\Calligraphy", "/ve", "%WINDIR%\Media\Calligraphy\Windows Information Bar.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\SecurityBand\Characters", "/ve", "%WINDIR%\Media\Characters\Windows Information Bar.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\SecurityBand\Cityscape", "/ve", "%WINDIR%\Media\Cityscape\Windows Information Bar.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\SecurityBand\Delta", "/ve", "%WINDIR%\Media\Delta\Windows Information Bar.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\SecurityBand\Festival", "/ve", "%WINDIR%\Media\Festival\Windows Information Bar.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\SecurityBand\Garden", "/ve", "%WINDIR%\Media\Garden\Windows Information Bar.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\SecurityBand\Heritage", "/ve", "%WINDIR%\Media\Heritage\Windows Information Bar.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\SecurityBand\Landscape", "/ve", "%WINDIR%\Media\Landscape\Windows Information Bar.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\SecurityBand\Quirky", "/ve", "%WINDIR%\Media\Quirky\Windows Information Bar.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\SecurityBand\Raga", "/ve", "%WINDIR%\Media\Raga\Windows Information Bar.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\SecurityBand\Savanna", "/ve", "%WINDIR%\Media\Savanna\Windows Information Bar.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\Explorer\SecurityBand\Sonata", "/ve", "%WINDIR%\Media\Sonata\Windows Information Bar.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\DisNumbersSound\.Default", "/ve", "%WINDIR%\Media\Speech Disambiguation.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\DisNumbersSound\.Current", "/ve", "%WINDIR%\Media\Speech Disambiguation.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\DisNumbersSound\Custom", "/ve", "%WINDIR%\Media\Speech Disambiguation.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\DisNumbersSound\Afternoon", "/ve", "%WINDIR%\Media\Speech Disambiguation.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\DisNumbersSound\Calligraphy", "/ve", "%WINDIR%\Media\Speech Disambiguation.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\DisNumbersSound\Characters", "/ve", "%WINDIR%\Medi\Speech Disambiguation.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\DisNumbersSound\Cityscape", "/ve", "%WINDIR%\Media\Speech Disambiguation.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\DisNumbersSound\Delta", "/ve", "%WINDIR%\Media\Speech Disambiguation.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\DisNumbersSound\Festival", "/ve", "%WINDIR%\Media\Speech Disambiguation.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\DisNumbersSound\Garden", "/ve", "%WINDIR%\Media\Speech Disambiguation.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\DisNumbersSound\Heritage", "/ve", "%WINDIR%\Media\Speech Disambiguation.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\DisNumbersSound\Landscape", "/ve", "%WINDIR%\Media\Speech Disambiguation.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\DisNumbersSound\Quirky", "/ve", "%WINDIR%\Media\Speech Disambiguation.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\DisNumbersSound\Raga", "/ve", "%WINDIR%\Media\Speech Disambiguation.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\DisNumbersSound\Savanna", "/ve", "%WINDIR%\Media\Speech Disambiguation.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\DisNumbersSound\Sonata", "/ve", "%WINDIR%\Media\Speech Disambiguation.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\HubOnSound\.Default", "/ve", "%WINDIR%\Media\Speech On.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\HubOnSound\.Current", "/ve", "%WINDIR%\Media\Speech On.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\HubOnSound\Custom", "/ve", "%WINDIR%\Media\Speech On.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\HubOnSound\Afternoon", "/ve", "%WINDIR%\Media\Speech On.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\HubOnSound\Calligraphy", "/ve", "%WINDIR%\Media\Speech On.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\HubOnSound\Characters", "/ve", "%WINDIR%\Medi\Speech On.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\HubOnSound\Cityscape", "/ve", "%WINDIR%\Media\Speech On.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\HubOnSound\Delta", "/ve", "%WINDIR%\Media\Speech On.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\HubOnSound\Festival", "/ve", "%WINDIR%\Media\Speech On.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\HubOnSound\Garden", "/ve", "%WINDIR%\Media\Speech On.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\HubOnSound\Heritage", "/ve", "%WINDIR%\Media\Speech On.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\HubOnSound\Landscape", "/ve", "%WINDIR%\Media\Speech On.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\HubOnSound\Quirky", "/ve", "%WINDIR%\Media\Speech On.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\HubOnSound\Raga", "/ve", "%WINDIR%\Media\Speech On.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\HubOnSound\Savanna", "/ve", "%WINDIR%\Media\Speech On.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\HubOnSound\Sonata", "/ve", "%WINDIR%\Media\Speech On.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\HubSleepSound\.Default", "/ve", "%WINDIR%\Media\Speech Sleep.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\HubSleepSound\.Current", "/ve", "%WINDIR%\Media\Speech Sleep.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\HubSleepSound\Custom", "/ve", "%WINDIR%\Media\Speech Sleep.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\HubSleepSound\Afternoon", "/ve", "%WINDIR%\Media\Speech Sleep.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\HubSleepSound\Calligraphy", "/ve", "%WINDIR%\Media\Speech Sleep.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\HubSleepSound\Characters", "/ve", "%WINDIR%\Medi\Speech Sleep.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\HubSleepSound\Cityscape", "/ve", "%WINDIR%\Media\Speech Sleep.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\HubSleepSound\Delta", "/ve", "%WINDIR%\Media\Speech Sleep.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\HubSleepSound\Festival", "/ve", "%WINDIR%\Media\Speech Sleep.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\HubSleepSound\Garden", "/ve", "%WINDIR%\Media\Speech Sleep.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\HubSleepSound\Heritage", "/ve", "%WINDIR%\Media\Speech Sleep.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\HubSleepSound\Landscape", "/ve", "%WINDIR%\Media\Speech Sleep.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\HubSleepSound\Quirky", "/ve", "%WINDIR%\Media\Speech Sleep.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\HubSleepSound\Raga", "/ve", "%WINDIR%\Media\Speech Sleep.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\HubSleepSound\Savanna", "/ve", "%WINDIR%\Media\Speech Sleep.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\HubSleepSound\Sonata", "/ve", "%WINDIR%\Media\Speech Sleep.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\MisrecoSound\.Default", "/ve", "%WINDIR%\Media\Speech Misrecognition.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\MisrecoSound\.Current", "/ve", "%WINDIR%\Media\Speech Misrecognition.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\MisrecoSound\Custom", "/ve", "%WINDIR%\Media\Speech Misrecognition.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\MisrecoSound\Afternoon", "/ve", "%WINDIR%\Media\Speech Misrecognition.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\MisrecoSound\Calligraphy", "/ve", "%WINDIR%\Media\Speech Misrecognition.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\MisrecoSound\Characters", "/ve", "%WINDIR%\Medi\Speech Misrecognition.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\MisrecoSound\Cityscape", "/ve", "%WINDIR%\Media\Speech Misrecognition.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\MisrecoSound\Delta", "/ve", "%WINDIR%\Media\Speech Misrecognition.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\MisrecoSound\Festival", "/ve", "%WINDIR%\Media\Speech Misrecognition.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\MisrecoSound\Garden", "/ve", "%WINDIR%\Media\Speech Misrecognition.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\MisrecoSound\Heritage", "/ve", "%WINDIR%\Media\Speech Misrecognition.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\MisrecoSound\Landscape", "/ve", "%WINDIR%\Media\Speech Misrecognition.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\MisrecoSound\Quirky", "/ve", "%WINDIR%\Media\Speech Misrecognition.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\MisrecoSound\Raga", "/ve", "%WINDIR%\Media\Speech Misrecognition.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\MisrecoSound\Savanna", "/ve", "%WINDIR%\Media\Speech Misrecognition.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\MisrecoSound\Sonata", "/ve", "%WINDIR%\Media\Speech Misrecognition.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\PanelSound\.Default", "/ve", "%WINDIR%\Media\Speech Disambiguation.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\PanelSound\.Current", "/ve", "%WINDIR%\Media\Speech Disambiguation.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\PanelSound\Custom", "/ve", "%WINDIR%\Media\Speech Disambiguation.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\PanelSound\Afternoon", "/ve", "%WINDIR%\Media\Speech Disambiguation.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\PanelSound\Calligraphy", "/ve", "%WINDIR%\Media\Speech Disambiguation.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\PanelSound\Characters", "/ve", "%WINDIR%\Medi\Speech Disambiguation.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\PanelSound\Cityscape", "/ve", "%WINDIR%\Media\Speech Disambiguation.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\PanelSound\Delta", "/ve", "%WINDIR%\Media\Speech Disambiguation.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\PanelSound\Festival", "/ve", "%WINDIR%\Media\Speech Disambiguation.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\PanelSound\Garden", "/ve", "%WINDIR%\Media\Speech Disambiguation.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\PanelSound\Heritage", "/ve", "%WINDIR%\Media\Speech Disambiguation.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\PanelSound\Landscape", "/ve", "%WINDIR%\Media\Speech Disambiguation.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\PanelSound\Quirky", "/ve", "%WINDIR%\Media\Speech Disambiguation.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\PanelSound\Raga", "/ve", "%WINDIR%\Media\Speech Disambiguation.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\PanelSound\Savanna", "/ve", "%WINDIR%\Media\Speech Disambiguation.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Apps\sapisvr\PanelSound\Sonata", "/ve", "%WINDIR%\Media\Speech Disambiguation.wav"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Names\.Default", "/ve", "@mmres.dll,-800"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Names\.None", "/ve", "@mmres.dll,-801"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Names\Afternoon", "/ve", "@mmres.dll,-810"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Names\Calligraphy", "/ve", "@mmres.dll,-811"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Names\Characters", "/ve", "@mmres.dll,-812"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Names\Cityscape", "/ve", "@mmres.dll,-813"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Names\Delta", "/ve", "@mmres.dll,-814"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Names\Festival", "/ve", "@mmres.dll,-815"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Names\Garden", "/ve", "@mmres.dll,-816"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Names\Heritage", "/ve", "@mmres.dll,-817"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Names\Landscape", "/ve", "@mmres.dll,-818"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Names\Quirky", "/ve", "@mmres.dll,-819"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Names\Raga", "/ve", "@mmres.dll,-820"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Names\Savanna", "/ve", "@mmres.dll,-821"))
        stringchanges.Add(ToList("HKCU\AppEvents\Schemes\Names\Sonata", "/ve", "@mmres.dll,-822"))

        'Configure OldNewExplorer
        dwordchanges.Add(ToList("HKCU\Software\Tihiy\OldNewExplorer", "DriveGrouping", "1"))
        dwordchanges.Add(ToList("HKCU\Software\Tihiy\OldNewExplorer", "HideFolders", "1"))
        dwordchanges.Add(ToList("HKCU\Software\Tihiy\OldNewExplorer", "NoRibbon", "1"))
        dwordchanges.Add(ToList("HKCU\Software\Tihiy\OldNewExplorer", "NoCaption", "1"))
        dwordchanges.Add(ToList("HKCU\Software\Tihiy\OldNewExplorer", "NoIcon", "1"))
        dwordchanges.Add(ToList("HKCU\Software\Tihiy\OldNewExplorer", "NoUpButton", "1"))
        dwordchanges.Add(ToList("HKCU\Software\Tihiy\OldNewExplorer", "NavBarGlass", "1"))
        dwordchanges.Add(ToList("HKCU\Software\Tihiy\OldNewExplorer", "IEButtons", "1"))
        dwordchanges.Add(ToList("HKCU\Software\Tihiy\OldNewExplorer", "Details", "1"))
        dwordchanges.Add(ToList("HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowStatusBar", "0"))
        hexchanges.Add(ToList("HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\Modules\NavPane", "ExpandedState", "05000000160014001F8080A63C324DC29940B94D446DD2D7249E0000010000004D0000001C00000031535053A66A63283D95D211B5D600C04FD918D0000000002D00000031535053357EC777E31B5043A48C7563D727776D1100000002000000000B000000FFFF00000000000000000000160014001F43983FFBB4EAC18D42A78AD1F5659CBA930000010000004D0000001C00000031535053A66A63283D95D211B5D600C04FD918D0000000002D00000031535053357EC777E31B5043A48C7563D727776D1100000002000000000B000000FFFF00000000000000000000160014001F50E04FD020EA3A6910A2D808002B30309D0000010000004D0000001C00000031535053A66A63283D95D211B5D600C04FD918D0000000002D00000031535053357EC777E31B5043A48C7563D727776D1100000002000000000B000000FFFF00000000000000000000160014001F580D1A2CF021BE504388B07367FC96EF3C0000010000004D0000001C00000031535053A66A63283D95D211B5D600C04FD918D0000000002D00000031535053357EC777E31B5043A48C7563D727776D1100000002000000000B000000000000000000000000000000160014001F5425481E03947BC34DB131E946B44C8DD50000010000004D0000001C00000031535053A66A63283D95D211B5D600C04FD918D0000000002D00000031535053357EC777E31B5043A48C7563D727776D1100000002000000000B000000FFFF00000000000000000000"))
        If HKLMKey32.OpenSubKey("SOFTWARE\Win8To7").GetValue("CurrentPhase") >= 97 Then 'Extra keys - restoration only
            dwordchangesclass.Add(ToList("HKCU\Software\Classes\CLSID\{031E4825-7B94-4dc3-B131-E946B44C8DD5}", "SortOrderIndex", "69")) 'nice
            dwordchangesclass.Add(ToList("HKCU\Software\Classes\CLSID\{031E4825-7B94-4dc3-B131-E946B44C8DD5}", "System.IsPinnedToNameSpaceTree", "1"))
        End If

        'Configure 7TaskbarTweaker
        stringchanges.Add(ToList("HKCU\Software\7 Taskbar Tweaker", "install_dir", windrive + "Program Files\7+ Taskbar Tweaker"))
        dwordchanges.Add(ToList("HKCU\Software\7 Taskbar Tweaker", "MementoSection_StartMenuLnk", "1"))
        dwordchanges.Add(ToList("HKCU\Software\7 Taskbar Tweaker", "MementoSection_DesktopLnk", "0"))
        dwordchanges.Add(ToList("HKCU\Software\7 Taskbar Tweaker", "updcheck", "1"))
        dwordchanges.Add(ToList("HKCU\Software\7 Taskbar Tweaker", "updcheckauto", "1"))
        dwordchanges.Add(ToList("HKCU\Software\7 Taskbar Tweaker", "hidetray", "1"))
        dwordchanges.Add(ToList("HKCU\Software\7 Taskbar Tweaker\OptionsEx", "always_show_thumb_labels", "0"))
        dwordchanges.Add(ToList("HKCU\Software\7 Taskbar Tweaker\OptionsEx", "always_show_tooltip", "0"))
        dwordchanges.Add(ToList("HKCU\Software\7 Taskbar Tweaker\OptionsEx", "disable_items_drag", "0"))
        dwordchanges.Add(ToList("HKCU\Software\7 Taskbar Tweaker\OptionsEx", "disable_taskbar_transparency", "0"))
        dwordchanges.Add(ToList("HKCU\Software\7 Taskbar Tweaker\OptionsEx", "disable_topmost", "0"))
        dwordchanges.Add(ToList("HKCU\Software\7 Taskbar Tweaker\OptionsEx", "disable_tray_icons_drag", "0"))
        dwordchanges.Add(ToList("HKCU\Software\7 Taskbar Tweaker\OptionsEx", "drag_towards_desktop", "0"))
        dwordchanges.Add(ToList("HKCU\Software\7 Taskbar Tweaker\OptionsEx", "fix_hang_reposition", "1"))
        dwordchanges.Add(ToList("HKCU\Software\7 Taskbar Tweaker\OptionsEx", "list_reverse_order", "0"))
        dwordchanges.Add(ToList("HKCU\Software\7 Taskbar Tweaker\OptionsEx", "multipage_wheel_scroll", "0"))
        dwordchanges.Add(ToList("HKCU\Software\7 Taskbar Tweaker\OptionsEx", "multirow_equal_width", "0"))
        dwordchanges.Add(ToList("HKCU\Software\7 Taskbar Tweaker\OptionsEx", "no_start_btn_spacing", "0"))
        dwordchanges.Add(ToList("HKCU\Software\7 Taskbar Tweaker\OptionsEx", "no_width_limit", "0"))
        dwordchanges.Add(ToList("HKCU\Software\7 Taskbar Tweaker\OptionsEx", "nocheck_close", "0"))
        dwordchanges.Add(ToList("HKCU\Software\7 Taskbar Tweaker\OptionsEx", "nocheck_maximize", "0"))
        dwordchanges.Add(ToList("HKCU\Software\7 Taskbar Tweaker\OptionsEx", "nocheck_minimize", "0"))
        dwordchanges.Add(ToList("HKCU\Software\7 Taskbar Tweaker\OptionsEx", "pinned_ungrouped_animate_launch", "0"))
        dwordchanges.Add(ToList("HKCU\Software\7 Taskbar Tweaker\OptionsEx", "right_drag_toggle_labels", "0"))
        dwordchanges.Add(ToList("HKCU\Software\7 Taskbar Tweaker\OptionsEx", "scroll_maximize_restore", "0"))
        dwordchanges.Add(ToList("HKCU\Software\7 Taskbar Tweaker\OptionsEx", "scroll_no_wrap", "0"))
        dwordchanges.Add(ToList("HKCU\Software\7 Taskbar Tweaker\OptionsEx", "scroll_reverse_cycle", "0"))
        dwordchanges.Add(ToList("HKCU\Software\7 Taskbar Tweaker\OptionsEx", "scroll_reverse_minimize", "0"))
        dwordchanges.Add(ToList("HKCU\Software\7 Taskbar Tweaker\OptionsEx", "show_desktop_button_size", "15"))
        dwordchanges.Add(ToList("HKCU\Software\7 Taskbar Tweaker\OptionsEx", "show_desktop_on_hover", "0"))
        dwordchanges.Add(ToList("HKCU\Software\7 Taskbar Tweaker\OptionsEx", "show_labels", "0"))
        dwordchanges.Add(ToList("HKCU\Software\7 Taskbar Tweaker\OptionsEx", "tray_clock_fix_width", "0"))
        dwordchanges.Add(ToList("HKCU\Software\7 Taskbar Tweaker\OptionsEx", "tray_icons_padding", "0"))
        dwordchanges.Add(ToList("HKCU\Software\7 Taskbar Tweaker\OptionsEx", "w7_tasklist_htclient", "0"))

        'Configure StartIsBack
        dwordchanges.Add(ToList("HKCU\Software\StartIsBack", "WelcomeShown", "2"))
        dwordchanges.Add(ToList("HKCU\Software\StartIsBack", "Start_LargeMFUIcons", "1"))
        dwordchanges.Add(ToList("HKCU\Software\StartIsBack", "StartScreenShortcut", "0"))
        dwordchanges.Add(ToList("HKCU\Software\StartIsBack", "StartMetroAppsMFU", "0"))
        dwordchanges.Add(ToList("HKCU\Software\StartIsBack", "Start_SortByName", "1"))
        dwordchanges.Add(ToList("HKCU\Software\StartIsBack", "StartMetroAppsFolder", "1"))
        dwordchanges.Add(ToList("HKCU\Software\StartIsBack", "Start_SortFoldersFirst", "0"))
        dwordchanges.Add(ToList("HKCU\Software\StartIsBack", "Start_NotifyNewApps", "1"))
        dwordchanges.Add(ToList("HKCU\Software\StartIsBack", "Start_ShowUser", "1"))
        dwordchanges.Add(ToList("HKCU\Software\StartIsBack", "Start_ShowMyDocs", "1"))
        dwordchanges.Add(ToList("HKCU\Software\StartIsBack", "Start_ShowMyPics", "1"))
        dwordchanges.Add(ToList("HKCU\Software\StartIsBack", "Start_ShowMyMusic", "1"))
        dwordchanges.Add(ToList("HKCU\Software\StartIsBack", "Start_ShowVideos", "0"))
        dwordchanges.Add(ToList("HKCU\Software\StartIsBack", "Start_ShowSkyDrive", "0"))
        dwordchanges.Add(ToList("HKCU\Software\StartIsBack", "Start_ShowDownloads", "0"))
        dwordchanges.Add(ToList("HKCU\Software\StartIsBack", "Start_ShowRecordedTV", "0"))
        dwordchanges.Add(ToList("HKCU\Software\StartIsBack", "Start_ShowMyGames", "1"))
        dwordchanges.Add(ToList("HKCU\Software\StartIsBack", "StartMenuFavorites", "0"))
        dwordchanges.Add(ToList("HKCU\Software\StartIsBack", "Start_ShowRecentDocs", "0"))
        dwordchanges.Add(ToList("HKCU\Software\StartIsBack", "Start_ShowHomegroup", "0"))
        dwordchanges.Add(ToList("HKCU\Software\StartIsBack", "Start_ShowNetPlaces", "0"))
        dwordchanges.Add(ToList("HKCU\Software\StartIsBack", "Start_ShowNetConn", "0"))
        dwordchanges.Add(ToList("HKCU\Software\StartIsBack", "Start_ShowMyComputer", "1"))
        dwordchanges.Add(ToList("HKCU\Software\StartIsBack", "Start_ShowControlPanel", "1"))
        dwordchanges.Add(ToList("HKCU\Software\StartIsBack", "Start_ShowPCSettings", "0"))
        dwordchanges.Add(ToList("HKCU\Software\StartIsBack", "Start_AdminToolsRoot", "0"))
        dwordchanges.Add(ToList("HKCU\Software\StartIsBack", "Start_ShowPrinters", "1"))
        dwordchanges.Add(ToList("HKCU\Software\StartIsBack", "Start_ShowSetProgramAccessAndDefaults", "1"))
        dwordchanges.Add(ToList("HKCU\Software\StartIsBack", "Start_ShowHelp", "1"))
        dwordchanges.Add(ToList("HKCU\Software\StartIsBack", "Start_ShowRun", "0"))
        dwordchanges.Add(ToList("HKCU\Software\StartIsBack", "Start_MinMFU", "10"))
        dwordchanges.Add(ToList("HKCU\Software\StartIsBack", "TerminateOnClose", "1"))
        dwordchanges.Add(ToList("HKCU\Software\StartIsBack", "EnableLeftEdge", "0"))
        dwordchanges.Add(ToList("HKCU\Software\StartIsBack", "EnableRightTopEdge", "0"))
        dwordchanges.Add(ToList("HKCU\Software\StartIsBack", "EnableRightBottomEdge", "0"))
        dwordchanges.Add(ToList("HKCU\Software\StartIsBack", "EnableTopEdge", "0"))
        dwordchanges.Add(ToList("HKCU\Software\StartIsBack", "StartIsApps", "1"))
        dwordchanges.Add(ToList("HKCU\Software\StartIsBack", "ImmersiveTaskbar", "1"))
        dwordchanges.Add(ToList("HKCU\Software\StartIsBack", "CombineWinX", "1"))
        dwordchanges.Add(ToList("HKCU\Software\StartIsBack", "TaskbarAlpha", "255"))
        dwordchanges.Add(ToList("HKCU\Software\StartIsBack", "StartMenuAlpha", "255"))
        dwordchanges.Add(ToList("HKCU\Software\StartIsBack", "HideUserFrame", "0"))
        dwordchanges.Add(ToList("HKCU\Software\StartIsBack", "EnableStartEdge", "0"))
        dwordchanges.Add(ToList("HKCU\Software\StartIsBack", "WinkeyFunction", "0"))
        dwordchanges.Add(ToList("HKCU\Software\StartIsBack", "MetroCloseSwitch", "2"))
        dwordchanges.Add(ToList("HKCU\Software\StartIsBack", "WinDoubleTap", "0"))
        dwordchanges.Add(ToList("HKCU\Software\StartIsBack", "MetroHotkeyFunction", "0"))
        dwordchanges.Add(ToList("HKCU\Software\StartIsBack", "MetroHotKey", "10"))
        stringchanges.Add(ToList("HKCU\Software\StartIsBack", "OrbBitmap", "%STARTISBACK%\Orbs\win7_54.bmp")) '%STARTISBACK% will be translated later
        stringchanges.Add(ToList("HKCU\Software\StartIsBack", "AlterStyle", "%STARTISBACK%\Styles\Default start menu style.msstyles"))
        dwordchanges.Add(ToList("HKCU\Software\StartIsBack", "AutoUpdates", "1"))
        itemstodelete.Add(ToList("HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\StartPage2", "FavoritesResolve", ""))
        hexchanges.Add(ToList("HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\StartPage2", "Favorites", "FF"))


        'Configure Open Shell
        dwordchanges.Add(ToList("HKCU\Software\OpenShell\StartMenu", "ShowedStyle2", "1"))
        dwordchanges.Add(ToList("HKCU\Software\OpenShell\StartMenu\Settings", "StartScreenShortcut", "0"))
        stringchanges.Add(ToList("HKCU\Software\OpenShell\StartMenu\Settings", "MainMenuAnimation", "None"))
        dwordchanges.Add(ToList("HKCU\Software\OpenShell\StartMenu\Settings", "AeroGlass", "1"))
        stringchanges.Add(ToList("HKCU\Software\OpenShell\StartMenu\Settings", "SkinC1", "Windows Aero"))
        stringchanges.Add(ToList("HKCU\Software\OpenShell\StartMenu\Settings", "SkinC2", "Windows Aero"))
        stringchanges.Add(ToList("HKCU\Software\OpenShell\StartMenu\Settings", "SkinW7", "WIN7LIKE COMBO revC"))
        stringchanges.Add(ToList("HKCU\Software\OpenShell\StartMenu\Settings", "SkinVariationW7", ""))
        multistringchanges.Add(ToList("HKCU\Software\OpenShell\StartMenu\Settings", "SkinOptionsW7", "USER_IMAGE=1\0UP48=1\0UP60=0\0UP64=0\0UP72=0\0UP84=0\0UP96=0\0UP128=0\0USER_IMAGE_IN=0\0CLEARPIC=0\0GLARE=1\0NO_USER_FRAME=0\0SMALL_ICONS=0\0TEXT_PAD=0\0SMALL_FONT=0\0REG_FONT=1\0LARGE_FONT=0\0GLASS_SHADOW=0\0WIN7SCROLL=1\0METRO2=0\0METRO=0\0HIGH=1\0MED=0\0LOW=0\0LOW_MILD=0\0WHITE_SPLIT=1\0WHITE_SUBMENUS=1\0NO_INTERN=0\0INTERN3=1\0INTERN1=0\0INTERN2=0\0INTERN4=0")) '\0 is the syntax for separating strings out in reg.exe
        stringchanges.Add(ToList("HKCU\Software\OpenShell\StartMenu\Settings", "StartButtonType", "CustomButton"))
        stringchanges.Add(ToList("HKCU\Software\OpenShell\StartMenu\Settings", "StartButtonPath", "%OPENSHELL%\Orbs\win7.png")) '%OPENSHELL% will be translated later
        dwordchanges.Add(ToList("HKCU\Software\OpenShell\StartMenu\Settings", "SkipMetro", "1"))
        dwordchanges.Add(ToList("HKCU\Software\OpenShell\StartMenu\Settings", "EnableStartButton", "1"))
        dwordchanges.Add(ToList("HKCU\Software\OpenShell\StartMenu\Settings", "MenuShadow", "0"))
        dwordchanges.Add(ToList("HKCU\Software\OpenShell\StartMenu\Settings", "MenuFadeSpeed", "0"))
        stringchanges.Add(ToList("HKCU\Software\OpenShell\StartMenu\Settings", "ShiftClick", "Nothing"))
        stringchanges.Add(ToList("HKCU\Software\OpenShell\StartMenu\Settings", "ShiftWin", "Nothing"))
        multistringchanges.Add(ToList("HKCU\Software\OpenShell\StartMenu\Settings", "MenuItems7", "Item1.Command=user_files\0Item1.Settings=NOEXPAND\0Item2.Command=lib_documents\0Item2.Settings=NOEXPAND\0Item3.Command=lib_pictures\0Item3.Settings=NOEXPAND\0Item4.Command=lib_music\0Item4.Settings=NOEXPAND\0Item5.Command=lib_videos\0Item5.Settings=ITEM_DISABLED\0Item6.Command=downloads\0Item6.Settings=ITEM_DISABLED\0Item7.Command=homegroup\0Item7.Settings=ITEM_DISABLED\0Item8.Command=separator\0Item9.Command=games\0Item9.Settings=TRACK_RECENT|NOEXPAND\0Item10.Command=favorites\0Item10.Settings=ITEM_DISABLED\0Item11.Command=recent_documents\0Item11.Settings=ITEM_DISABLED\0Item12.Command=computer\0Item12.Settings=NOEXPAND\0Item13.Command=network\0Item13.Settings=ITEM_DISABLED\0Item14.Command=network_connections\0Item14.Settings=ITEM_DISABLED\0Item15.Command=separator\0Item16.Command=control_panel\0Item16.Settings=TRACK_RECENT|NOEXPAND\0Item17.Command=pc_settings\0Item17.Settings=TRACK_RECENT|ITEM_DISABLED\0Item18.Command=admin\0Item18.Settings=TRACK_RECENT|ITEM_DISABLED\0Item19.Command=devices\0Item19.Settings=NOEXPAND\0Item20.Command=defaults\0Item21.Command=help\0Item22.Command=run\0Item22.Settings=ITEM_DISABLED\0Item23.Command=apps\0Item23.Settings=ITEM_DISABLED\0Item24.Command=windows_security"))
        dwordchanges.Add(ToList("HKCU\Software\OpenShell\StartMenu\Settings", "ShiftRight", "1"))
        dwordchanges.Add(ToList("HKCU\Software\OpenShell\StartMenu\Settings", "CustomTaskbar", "1"))
        stringchanges.Add(ToList("HKCU\Software\OpenShell\StartMenu\Settings", "TaskbarLook", "Opaque"))

        'Change to AERO theme
        stringchanges.Add(ToList("HKCU\Software\Microsoft\Windows\CurrentVersion\ThemeManager", "DllName", "%WINDIR%\Resources\Themes\aero\aero.msstyles"))

        'Configure Metro (Windows 8.0)
        dwordchanges.Add(ToList("HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\Accent", "ColorSet_Version3", "10"))

        'Enable funni Pointer Shadows
        hexchanges.Add(ToList("HKCU\Control Panel\Desktop", "UserPreferencesMask", "9E3E078012000000"))

        If Microsoft.Win32.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ProductName", "Windows 8 Pro").ToString.EndsWith(" Starter") Then
            'Starter: Blue desktop background
            stringchanges.Add(ToList("HKCU\Control Panel\Colors", "Background", "17 128 210"))
        End If

        If Not IO.File.Exists(storagelocation + "\Backups\Windows\System32\GettingStarted.exe") Then
            'Add Getting Started to startup to finish some things off (not on Install -> Uninstall -> Install installs, though)
            stringchanges.Add(ToList("HKCU\Software\Microsoft\Windows\CurrentVersion\Run", "GettingStarted", "%WINDIR%\System32\GettingStarted.exe"))
        End If

        UserTweaks.Add("DWORD", dwordchanges)
        UserTweaks.Add("String", stringchanges)
        UserTweaks.Add("MultiString", multistringchanges)
        UserTweaks.Add("Binary", hexchanges)
        UserTweaks.Add("Delete", itemstodelete)
        UserClassTweaks.Add("DWORD", dwordchangesclass)
        UserClassTweaks.Add("String", stringchangesclass)
        UserClassTweaks.Add("MultiString", multistringchangesclass)
        UserClassTweaks.Add("Binary", hexchangesclass)
        UserClassTweaks.Add("Delete", itemstodeleteclass)
    End Sub





    Public Sub SetSZ(ByVal Key As String, ByVal Value As String, ByVal Data As String, Optional ByVal wait As Boolean = True)
        Dim newKey As String
        Dim newValue As String
        Dim newData As String
        If Key.Contains(" ") Then
            newKey = """" + Key + """"
        Else
            newKey = Key
        End If
        If Value.Contains(" ") Then
            newValue = """" + Value + """"
        Else
            newValue = Value
        End If
        If Data.Contains(" ") Then
            newData = """" + Data + """"
        Else
            newData = Data
        End If
        Dim slashd As String
        If Not String.IsNullOrEmpty(Data) And Not Data = "" Then
            slashd = "/d " + newData
        Else
            slashd = ""
        End If
        Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD " + newKey + " /v " + newValue + " /t REG_SZ " + slashd + " /f", AppWinStyle.Hide, wait)
    End Sub

    Public Sub SetEXPANDSZ(ByVal Key As String, ByVal Value As String, ByVal Data As String, Optional ByVal wait As Boolean = True)
        Dim newKey As String
        Dim newValue As String
        Dim newData As String
        If Key.Contains(" ") Then
            newKey = """" + Key + """"
        Else
            newKey = Key
        End If
        If Value.Contains(" ") Then
            newValue = """" + Value + """"
        Else
            newValue = Value
        End If
        If Data.Contains(" ") Then
            newData = """" + Data + """"
        Else
            newData = Data
        End If
        Dim slashd As String
        If Not String.IsNullOrEmpty(Data) And Not Data = "" Then
            slashd = "/d " + newData
        Else
            slashd = ""
        End If
        Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD " + newKey + " /v " + newValue + " /t REG_EXPAND_SZ " + slashd + " /f", AppWinStyle.Hide, wait)
    End Sub

    Public Sub SetMultiSZ(ByVal Key As String, ByVal Value As String, ByVal Data As String, Optional ByVal wait As Boolean = True)
        Dim newKey As String
        Dim newValue As String
        Dim newData As String
        If Key.Contains(" ") Then
            newKey = """" + Key + """"
        Else
            newKey = Key
        End If
        If Value.Contains(" ") Then
            newValue = """" + Value + """"
        Else
            newValue = Value
        End If
        If Not Data.Contains("""") Then 'Inverted logic here to prevent Open Shell configs from failing, I guess
            newData = """" + Data + """"
        Else
            newData = Data
        End If
        Dim slashd As String
        If Not String.IsNullOrEmpty(Data) And Not Data = "" Then
            slashd = "/d " + newData
        Else
            slashd = ""
        End If
        Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD " + newKey + " /v " + newValue + " /t REG_MULTI_SZ " + slashd + " /f", AppWinStyle.Hide, wait)
    End Sub

    Public Function ConvertToBinHex(ByVal Data As String)
        'Convert data to hex:
        Dim result1 As String = ""
        Dim charcount As Integer = 0
        For Each c As Char In Data
            result1 = result1 + c
            charcount += 1
            If charcount = 2 Then
                result1 = result1 + ","
                charcount = 0
            End If
        Next
        If result1.EndsWith(",") Then
            result1 = result1.Remove(result1.Length - 1, 1)
        End If

        Dim result2() As Byte = Array.ConvertAll(result1.Split(","), Function(b) Convert.ToByte(b, 16))
        Return result2
    End Function

    Public Sub SetBinary(ByVal Key As String, ByVal Value As String, ByVal Data As String, Optional ByVal wait As Boolean = True)
        Dim newKey As String
        Dim newValue As String
        Dim newData As String
        If Key.Contains(" ") Then
            newKey = """" + Key + """"
        Else
            newKey = Key
        End If
        If Value.Contains(" ") Then
            newValue = """" + Value + """"
        Else
            newValue = Value
        End If
        If Data.Contains(" ") Then
            newData = """" + Data + """"
        Else
            newData = Data
        End If
        Dim slashd As String
        If Not String.IsNullOrEmpty(Data) And Not Data = "" Then
            slashd = "/d " + newData
        Else
            slashd = ""
        End If
        Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD " + newKey + " /v " + newValue + " /t REG_BINARY " + slashd + " /f", AppWinStyle.Hide, wait)
    End Sub

    Public Function ConvertToDWORD(ByVal Data As String)
        'Convert data to dword:
        Dim result As String = ""
        Dim charcount As Integer = Hex(Data).ToString().Length()
        While result.Length() < (8 - charcount)
            result = result + "0"
        End While
        result = result + Hex(Data).ToString()
        Return result
    End Function

    Public Sub SetDWORD(ByVal Key As String, ByVal Value As String, ByVal Data As String, Optional ByVal wait As Boolean = True)
        Dim newKey As String
        Dim newValue As String
        Dim newData As String
        If Key.Contains(" ") Then
            newKey = """" + Key + """"
        Else
            newKey = Key
        End If
        If Value.Contains(" ") Then
            newValue = """" + Value + """"
        Else
            newValue = Value
        End If
        If Data.Contains(" ") Then
            newData = """" + Data + """"
        Else
            newData = Data
        End If
        Dim slashd As String
        If Not String.IsNullOrEmpty(Data) And Not Data = "" Then
            slashd = "/d " + newData
        Else
            slashd = ""
        End If
        Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD " + newKey + " /v " + newValue + " /t REG_DWORD " + slashd + " /f", AppWinStyle.Hide, wait)
    End Sub

    Public Sub SetDefaultStr(ByVal Key As String, ByVal Data As String, Optional ByVal wait As Boolean = True)
        Dim newKey As String
        Dim newData As String
        If Key.Contains(" ") Then
            newKey = """" + Key + """"
        Else
            newKey = Key
        End If
        If Data.Contains(" ") Then
            newData = """" + Data + """"
        Else
            newData = Data
        End If
        Dim slashd As String
        If Not String.IsNullOrEmpty(Data) And Not Data = "" Then
            slashd = "/d " + newData
        Else
            slashd = ""
        End If
        Shell(windir + "\" + sysprefix + "\cmd.exe /c reg ADD " + newKey + " /ve " + slashd + " /f", AppWinStyle.Hide, wait)
    End Sub

    Public Sub DeleteKey(ByVal Key As String, ByVal Value As String, Optional ByVal wait As Boolean = True)
        Dim newKey As String
        Dim newValue As String
        If Key.Contains(" ") Then
            newKey = """" + Key + """"
        Else
            newKey = Key
        End If
        If Value.Contains(" ") Then
            newValue = """" + Value + """"
        Else
            newValue = Value
        End If

        Shell(windir + "\" + sysprefix + "\cmd.exe /c reg DELETE " + newKey + " /v " + newValue + " /f", AppWinStyle.Hide, wait)
    End Sub

    Function translate(ByVal input As String)
        Return input.Replace("%WINDIR%", windir).Replace("%STARTISBACK%", sibdir).Replace("%OPENSHELL%", openshelldir)
    End Function


    Function ToList(ByVal Key As String, ByVal Value As String, ByVal Data As String)
        Dim inputs() As String = {Key, Value, Data}
        Dim result As List(Of String) = New List(Of String)(inputs)

        Return result
    End Function
End Class
