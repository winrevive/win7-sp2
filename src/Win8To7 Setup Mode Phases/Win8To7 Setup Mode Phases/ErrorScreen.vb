Public Class ErrorScreen

    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        Const WM_PAINT = &HF
        Const WM_CAPTURECHANGED = &H215
        Const WM_SETFOCUS = &H7

        If (m.Msg = WM_PAINT) Or (m.Msg = WM_CAPTURECHANGED) Or (m.Msg = WM_SETFOCUS) Then
            Try
                Me.BringToFront()
                Me.Focus()
            Catch
            End Try
        End If

        MyBase.WndProc(m)
    End Sub

    Public Sub setStrings(ByVal tasking As String, ByVal taskname As String, ByVal errorstr As String)
        Label3.Text = "An error occurred TASKING Windows. An issue with your Windows installation, or with this program might be the cause.".Replace("TASKING", tasking)
        Label6.Text = "Task: TASKNAME".Replace("TASKNAME", taskname)
        Label7.Text = errorstr
    End Sub

    Private Sub ErrorScreen_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If Environment.UserName = "SYSTEM" Then
            RestartTime("") 'SYSTEM's the user programs use in Setup Mode, and nowhere else intentionally
        Else
            RestartTime("inwin") 'Otherwise restart Windows normally, instead of via API
        End If
    End Sub

    Private Sub ErrorScreen_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then
            Me.Close()
        End If
    End Sub

    Private Sub ErrorScreen_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Cursor.Hide()
        Label2.Text = Me.Text
    End Sub
End Class