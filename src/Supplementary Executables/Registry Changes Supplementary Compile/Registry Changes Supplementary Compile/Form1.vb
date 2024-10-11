Public Class Form1

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Me.Hide()
        Dim args As String() = Environment.GetCommandLineArgs()
        Dim exfile As String = args(4 + 1)

        Dim file As System.IO.StreamWriter
        file = My.Computer.FileSystem.OpenTextFileWriter(exfile, False)
        file.WriteLine("This is a dummy application in place of Win8To7 Registry Changes to allow files on the Win8To7 Transformation Pack backend repository to meet the filesize limit. Please compile 'Win8To7 Registry Changes' (for 64-Bit and 32-Bit) over regchange.exe in your 'Setup Mode Phases' compile to continue. Refer to the Wiki for instructions.")
        file.Close()

        Environment.Exit(1)
    End Sub
End Class
