Public Class Form1

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Me.Hide()
        MsgBox("This is a dummy application substituting " + System.Reflection.Assembly.GetExecutingAssembly.Location + _
               " to allow files on the Win8To7 Transformation Pack backend repository to meet the filesize limit. Please replace this file, with a copy of the appropriate installer's executable, in your 'Setup Mode Phases' compile to continue. Refer to the Wiki for instructions.", MsgBoxStyle.Information + MsgBoxStyle.SystemModal, "Supplementary Executable")

        Environment.Exit(1)
    End Sub
End Class
