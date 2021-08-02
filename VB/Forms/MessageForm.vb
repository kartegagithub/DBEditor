Public Class MessageForm
    Public MessageToShow As String = ""
    Public Thread As Threading.Thread
    Public Form As Form
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If Not Me.Thread Is Nothing Then
            If Me.Thread.ThreadState = Threading.ThreadState.Stopped Then
                Me.Timer1.Stop()
                CType(Form, Object).AfterProcess()
                Me.DialogResult = Windows.Forms.DialogResult.OK
            End If
        Else
            Me.Timer1.Stop()
            CType(Form, Object).AfterProcess()
            Me.DialogResult = Windows.Forms.DialogResult.OK
        End If
        If Me.Label1.Text.Length > 50 Then
            Me.Label1.Text = "Please wait" & "."
        Else
            Me.Label1.Text &= "."
        End If
    End Sub
    Private Sub MessageForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try
            Me.Timer1.Stop()
        Catch ex As Exception

        End Try
    End Sub
    Private Sub MessageForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Form.Enabled = False
        Me.MessageLabel.Text = Me.MessageToShow & "."
        Me.Timer1.Interval = 200
        Me.Timer1.Start()
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If Not Me.Thread Is Nothing Then
            If MsgBox("Are you sure to interrupt current process?", MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
                Me.Form.Enabled = True
                Me.Thread.Abort()
                Me.DialogResult = Windows.Forms.DialogResult.OK
            End If
        End If
    End Sub
End Class