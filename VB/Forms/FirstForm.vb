Public Class FirstForm
    Private Sub FirstForm_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.Control AndAlso e.KeyCode = Keys.F Then
            Utility.AskTheExceptionGoogle()
        End If
    End Sub
End Class