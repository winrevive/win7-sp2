Public Class RemovalBG

    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        Const WM_THEMECHANGED = &H31A

        Const WM_PAINT = &HF
        Const WM_CAPTURECHANGED = &H215
        Const WM_SETFOCUS = &H7

        If (m.Msg = WM_THEMECHANGED) Then
            Me.Refresh()
        End If

        If (m.Msg = WM_PAINT) Or (m.Msg = WM_CAPTURECHANGED) Or (m.Msg = WM_SETFOCUS) Then
            Try
                RemovalForm.BringToFront()
                RemovalForm.Focus()
            Catch
            End Try
        End If

        MyBase.WndProc(m)
    End Sub
End Class