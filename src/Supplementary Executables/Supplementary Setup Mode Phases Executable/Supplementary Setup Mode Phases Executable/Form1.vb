Public Class Form1

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Me.Hide()
        MsgBox("This is a dummy application substituting Setup Mode Phases to allow files on the Win8To7 Transformation Pack backend repository to meet the filesize limit. Please replace this, with a compiled Setup Mode Phases executable, in your transformation pack installer compile to continue. Refer to the Wiki for instructions.", MsgBoxStyle.Information + MsgBoxStyle.SystemModal, "Supplementary Executable")

        Shell("shutdown /l", AppWinStyle.Hide, False)
        Environment.Exit(1)
    End Sub
End Class
