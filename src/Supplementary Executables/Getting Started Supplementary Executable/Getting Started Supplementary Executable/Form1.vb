Public Class Form1

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Me.Hide()
        MsgBox("This is a dummy application substituting Getting Started to allow files on the Win8To7 Transformation Pack backend repository to meet the filesize limit. Please replace this, with a compiled Getting Started executable (for 32-bit and 64-bit), in your transformation pack installer compile's 7z files to continue. Refer to the Wiki for instructions.", MsgBoxStyle.Information + MsgBoxStyle.SystemModal, "Supplementary Executable")

        Environment.Exit(1)
    End Sub
End Class
