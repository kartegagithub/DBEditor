Public Class Step1
    Private DataSource As DataSource
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Select Case Me.ComboBox1.Text
            Case "Oracle"
                Me.DataSource.Type = ConnectionType.Oracle
            Case "SQLServer"
                Me.DataSource.Type = ConnectionType.SQLServer
            Case "Access"
                Me.DataSource.Type = ConnectionType.Access
            Case "FoxPro"
                Me.DataSource.Type = ConnectionType.FoxPro
            Case "Excel"
                Me.DataSource.Type = ConnectionType.Excel
            Case "XML"
                Me.DataSource.Type = ConnectionType.XML
        End Select
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub
    Public Sub New()
        InitializeComponent()
    End Sub
    Public Sub New(ByVal DataSource As DataSource)
        InitializeComponent()
        Me.DataSource = DataSource
    End Sub
    Private Sub Step1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Button1.Enabled = False
    End Sub
    Private Sub ComboBox1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ComboBox1.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
        End If
    End Sub
    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        If Me.ComboBox1.Text <> "" Then
            Me.Button1.Enabled = True
        Else
            Me.Button1.Enabled = False
        End If
    End Sub
End Class