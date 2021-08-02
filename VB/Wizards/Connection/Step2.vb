Public Class Step2
    Private DataSource As DataSource
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub
    Public Sub New()
        InitializeComponent()
    End Sub
    Public Sub New(ByVal DataSource As DataSource)
        InitializeComponent()
        Me.DataSource = DataSource
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            Select Case Me.DataSource.Type
                Case ConnectionType.Access, ConnectionType.Excel, ConnectionType.FoxPro, ConnectionType.XML
                    Me.DataSource.Location = Me.DBLocation.Text
                Case Else
                    Me.DataSource.Password = Me.PWD.Text
                    Me.DataSource.UserName = Me.UID.Text
                    Me.DataSource.Location = Me.Server.Text
                    Me.DataSource.DataBase = Me.Catalog.Text
            End Select
            Me.DataSource.Connect()
            Utility.Container.PrepareConnection(Me.DataSource)
            Me.DialogResult = Windows.Forms.DialogResult.Ignore
        Catch ex As Exception
            MsgBox("Could not connect to data source, source is down or required components are not installed properly.", MsgBoxStyle.Critical)
        End Try
    End Sub
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Select Case Me.DataSource.Type
            Case ConnectionType.Access, ConnectionType.Excel, ConnectionType.FoxPro
                Me.DataSource.Location = Me.DBLocation.Text
            Case Else
                Me.DataSource.Password = Me.PWD.Text
                Me.DataSource.UserName = Me.UID.Text
                Me.DataSource.Location = Me.Server.Text
                Me.DataSource.DataBase = Me.Catalog.Text
        End Select
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub
    Private Sub Step2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Select Case Me.DataSource.Type
            Case ConnectionType.Access, ConnectionType.Excel, ConnectionType.FoxPro, ConnectionType.XML
                Me.FolderRelated.Visible = True
                Me.GeneralDB.Visible = False
                Me.DBLocation.Focus()
            Case Else
                Me.FolderRelated.Visible = False
                Me.GeneralDB.Visible = True
                Me.Server.Focus()
        End Select
        Select Case Me.DataSource.Type
            Case ConnectionType.Access, ConnectionType.XML
                Me.MessageLabel.Text = "Please enter file location."
            Case ConnectionType.FoxPro
                Me.MessageLabel.Text = "Please enter folder location which the files located."
            Case ConnectionType.Excel
                Me.MessageLabel.Text = "Please enter file location. Columns of first row are the table columns."
            Case ConnectionType.Oracle
                Me.MessageLabel.Text = "DB is the listener of data source."
            Case ConnectionType.SQLServer
                Me.MessageLabel.Text = ""
        End Select
    End Sub
    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Select Case Me.DataSource.Type
            Case ConnectionType.Access, ConnectionType.Excel, ConnectionType.XML
                If Me.OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
                    Me.DBLocation.Text = Me.OpenFileDialog1.FileName
                End If
            Case ConnectionType.FoxPro
                If Me.FolderBrowserDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
                    Me.DBLocation.Text = Me.FolderBrowserDialog1.SelectedPath
                End If
        End Select
    End Sub
    Private Sub Server_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Server.KeyDown, UID.KeyDown, PWD.KeyDown, DBLocation.KeyDown, Catalog.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
        End If
    End Sub
End Class