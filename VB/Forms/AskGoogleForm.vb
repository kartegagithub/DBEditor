Public Class AskGoogleForm
    Private Sub TextBox1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox1.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        ElseIf e.KeyCode = Keys.Enter OrElse e.KeyCode = Keys.Return Then
            Utility.AskGoogle(Me.TextBox1.Text)
            Me.Close()
        End If
    End Sub
    Private Sub AskGoogleForm_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Me.TextBox1.Focus()
    End Sub
    Private Sub AskGoogleForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.TextBox1.Focus()
    End Sub
End Class