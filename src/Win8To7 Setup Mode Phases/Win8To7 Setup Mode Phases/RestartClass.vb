'
'    WindowsController class for VB.NET
'		Version: 1.1
'
'    Copyright © 2002-2003, The KPD-Team
'    All rights reserved.
'    http://www.mentalis.org/
'
'  Redistribution and use in source and binary forms, with or without
'  modification, are permitted provided that the following conditions
'  are met:
'
'    - Redistributions of source code must retain the above copyright
'       notice, this list of conditions and the following disclaimer. 
'
'    - Neither the name of the KPD-Team, nor the names of its contributors
'       may be used to endorse or promote products derived from this
'       software without specific prior written permission. 
'
'  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
'  "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
'  LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS
'  FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL
'  THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT,
'  INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
'  (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
'  SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
'  HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT,
'  STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
'  ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED
'  OF THE POSSIBILITY OF SUCH DAMAGE.
'

Option Explicit On 
Option Strict On

Public Enum RestartOptions
    Reboot = 1
    ShutDown = 0
End Enum

' An LUID is a 64-bit value guaranteed to be unique only on the system on which it was generated. The uniqueness of a locally unique identifier (LUID) is guaranteed only until the system is restarted.
<StructLayout(LayoutKind.Sequential, Pack:=1)> _
Friend Structure LUID

    ' The low order part of the 64 bit value.
    ' </summary>
    Public LowPart As Integer

    ' The high order part of the 64 bit value.
    ' </summary>
    Public HighPart As Integer
End Structure

' The LUID_AND_ATTRIBUTES structure represents a locally unique identifier (LUID) and its attributes.
<StructLayout(LayoutKind.Sequential, Pack:=1)> _
Friend Structure LUID_AND_ATTRIBUTES
    ' Specifies an LUID value.
    Public pLuid As LUID

    ' Specifies attributes of the LUID. This value contains up to 32 one-bit flags. Its meaning is dependent on the definition and use of the LUID.
    Public Attributes As Integer
End Structure

' The TOKEN_PRIVILEGES structure contains information about a set of privileges for an access token.
' </summary>
<StructLayout(LayoutKind.Sequential, Pack:=1)> _
Friend Structure TOKEN_PRIVILEGES

    ' Specifies the number of entries in the Privileges array.
    Public PrivilegeCount As Integer

    ' Specifies an array of LUID_AND_ATTRIBUTES structures. Each structure contains the LUID and attributes of a privilege.
    Public Privileges As LUID_AND_ATTRIBUTES
End Structure

' Implements methods to exit Windows.
' </summary>
Public Class RestartClass
    ' Required to enable or disable the privileges in an access token.
    Private Const TOKEN_ADJUST_PRIVILEGES As Integer = &H20
    ' Required to query an access token.
    Private Const TOKEN_QUERY As Integer = &H8
    ' The privilege is enabled.
    Private Const SE_PRIVILEGE_ENABLED As Integer = &H2
    ' Specifies that the function should search the system message-table resource(s) for the requested message.
    Private Const FORMAT_MESSAGE_FROM_SYSTEM As Integer = &H1000
    ' Forces processes to terminate. When this flag is set, the system does not send the WM_QUERYENDSESSION and WM_ENDSESSION messages. This can cause the applications to lose data. Therefore, you should only use this flag in an emergency.
    Private Const EWX_FORCE As Integer = 4

    ' The LoadLibrary function maps the specified executable module into the address space of the calling process.
    Private Declare Ansi Function LoadLibrary Lib "kernel32" Alias "LoadLibraryA" (ByVal lpLibFileName As String) As IntPtr

    ' The FreeLibrary function decrements the reference count of the loaded dynamic-link library (DLL). When the reference count reaches zero, the module is unmapped from the address space of the calling process and the handle is no longer valid.
    Private Declare Ansi Function FreeLibrary Lib "kernel32" (ByVal hLibModule As IntPtr) As Integer

    ' The GetProcAddress function retrieves the address of an exported function or variable from the specified dynamic-link library (DLL).
    Private Declare Ansi Function GetProcAddress Lib "kernel32" (ByVal hModule As IntPtr, ByVal lpProcName As String) As IntPtr

    ' The OpenProcessToken function opens the access token associated with a process.
    Private Declare Ansi Function OpenProcessToken Lib "advapi32" (ByVal ProcessHandle As IntPtr, ByVal DesiredAccess As Integer, ByRef TokenHandle As IntPtr) As Integer

    ' The LookupPrivilegeValue function retrieves the locally unique identifier (LUID) used on a specified system to locally represent the specified privilege name.
    Private Declare Ansi Function LookupPrivilegeValue Lib "advapi32" Alias "LookupPrivilegeValueA" (ByVal lpSystemName As String, ByVal lpName As String, ByRef lpLuid As LUID) As Integer

    ' The AdjustTokenPrivileges function enables or disables privileges in the specified access token. Enabling or disabling privileges in an access token requires TOKEN_ADJUST_PRIVILEGES access.
    Private Declare Ansi Function AdjustTokenPrivileges Lib "advapi32" (ByVal TokenHandle As IntPtr, ByVal DisableAllPrivileges As Integer, ByRef NewState As TOKEN_PRIVILEGES, ByVal BufferLength As Integer, ByRef PreviousState As TOKEN_PRIVILEGES, ByRef ReturnLength As Integer) As Integer

    ' The ExitWindowsEx function either logs off the current user, shuts down the system, or shuts down and restarts the system. It sends the WM_QUERYENDSESSION message to all applications to determine if they can be terminated.
    Private Declare Ansi Function NtShutdownSystem Lib "ntdll" (ByVal ShutdownAction As Integer) As Integer

    ' The FormatMessage function formats a message string. The function requires a message definition as input. The message definition can come from a buffer passed into the function. It can come from a message table resource in an already-loaded module. Or the caller can ask the function to search the system's message table resource(s) for the message definition. The function finds the message definition in a message table resource based on a message identifier and a language identifier. The function copies the formatted message text to an output buffer, processing any embedded insert sequences if requested.
    Private Declare Ansi Function FormatMessage Lib "kernel32" Alias "FormatMessageA" (ByVal dwFlags As Integer, ByVal lpSource As IntPtr, ByVal dwMessageId As Integer, ByVal dwLanguageId As Integer, ByVal lpBuffer As StringBuilder, ByVal nSize As Integer, ByVal Arguments As Integer) As Integer

    ' Exits windows (and tries to enable any required access rights, if necesarry).
    Public Shared Sub ExitWindows(ByVal how As RestartOptions)
        ExitWindows(CType(how, Integer))
    End Sub

    ' Exits windows (and tries to enable any required access rights, if necesarry).
    Protected Shared Sub ExitWindows(ByVal how As Integer)
        EnableToken("SeShutdownPrivilege")

        NtShutdownSystem(how)
    End Sub

    ' Tries to enable the specified privilege.
    Protected Shared Sub EnableToken(ByVal privilege As String)
        If (Environment.OSVersion.Platform <> PlatformID.Win32NT) OrElse (Not CheckEntryPoint("advapi32.dll", "AdjustTokenPrivileges")) Then Return
        Dim tokenHandle As IntPtr
        Dim privilegeLUID As LUID
        Dim newPrivileges As TOKEN_PRIVILEGES
        Dim tokenPrivileges As TOKEN_PRIVILEGES
        If OpenProcessToken(Process.GetCurrentProcess.Handle, TOKEN_ADJUST_PRIVILEGES Or TOKEN_QUERY, tokenHandle) = 0 Then Throw New PrivilegeException(FormatError(Marshal.GetLastWin32Error))
        If LookupPrivilegeValue("", privilege, privilegeLUID) = 0 Then Throw New PrivilegeException(FormatError(Marshal.GetLastWin32Error))
        tokenPrivileges.PrivilegeCount = 1
        tokenPrivileges.Privileges.Attributes = SE_PRIVILEGE_ENABLED
        tokenPrivileges.Privileges.pLuid = privilegeLUID
        If AdjustTokenPrivileges(tokenHandle, 0, tokenPrivileges, 4 + (12 * tokenPrivileges.PrivilegeCount), newPrivileges, 4 + (12 * newPrivileges.PrivilegeCount)) = 0 Then Throw New PrivilegeException(FormatError(Marshal.GetLastWin32Error))
    End Sub

    ' Checks whether a specified method exists on the local computer.
    Protected Shared Function CheckEntryPoint(ByVal library As String, ByVal method As String) As Boolean
        Dim libPtr As IntPtr = LoadLibrary(library)
        If Not libPtr.Equals(IntPtr.Zero) Then
            If Not GetProcAddress(libPtr, method).Equals(IntPtr.Zero) Then
                FreeLibrary(libPtr)
                Return True
            End If
            FreeLibrary(libPtr)
        End If
        Return False
    End Function

    ' Formats an error number into an error message.
    Protected Shared Function FormatError(ByVal number As Integer) As String
        Try
            Dim buffer As New StringBuilder(255)
            FormatMessage(FORMAT_MESSAGE_FROM_SYSTEM, IntPtr.Zero, number, 0, buffer, buffer.Capacity, 0)
            Return buffer.ToString()
        Catch e As Exception
            Return "Unspecified error: " + number.ToString()
        End Try
    End Function
End Class

' The exception that is thrown when an error occures when requesting a specific privilege.
Public Class PrivilegeException
    Inherits Exception

    ' Initializes a new instance of the PrivilegeException class.
    Public Sub New()
        MyBase.New()
    End Sub

    ' Initializes a new instance of the PrivilegeException class with a specified error message.
    Public Sub New(ByVal message As String)
        MyBase.New(message)
    End Sub
End Class
