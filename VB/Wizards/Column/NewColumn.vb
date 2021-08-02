Public Class NewColumn
    Private Table As Table
    Private DataSource As DataSource
    Private EditorForm As EditorForm
    Public Sub New()
        InitializeComponent()
    End Sub
    Public Sub New(ByVal Table As Table, ByVal EditorForm As EditorForm)
        Me.New()
        Me.Table = Table
        Me.DataSource = Me.Table.DataSource
        Me.TableName.Text = Me.Table.Name
        Me.EditorForm = EditorForm
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.EditorForm.ExecuteSQL("ALTER TABLE " & Me.Table.Name & " ADD " & Me.ColumnName.Text & " " & Me.DataSource.GetSQLDataType(Me.DataType.Text, Me.Value.Text))
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub
    Private Sub ColumnName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ColumnName.KeyDown, DataType.KeyDown, Value.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
        End If
    End Sub
    Private Sub NewColumn_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.ColumnName.Focus()
    End Sub
    Private Sub DataType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataType.SelectedIndexChanged
        Me.Value.Enabled = True
        Select Case Me.DataType.Text
            Case "Memo", "Bit", "Byte", "Date", "Char"
                Me.Value.Enabled = False
                If Me.DataType.Text = "Memo" And (Me.DataSource.Type = ConnectionType.Oracle Or Me.DataSource.Type = ConnectionType.FoxPro) Then
                    Me.Value.Text = 4000
                Else
                    Me.Value.Text = ""
                End If
            Case "String"
                Me.Value.Text = "255"
            Case "Decimal"
                Me.Value.Text = "38,2"
            Case "Integer"
                Me.Value.Text = "16"
            Case "Long"
                Me.Value.Text = "38"
        End Select
    End Sub
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Me.Message.Text = "ALTER TABLE " & Me.Table.Name & " ADD " & Me.ColumnName.Text & " " & Me.DataSource.GetSQLDataType(Me.DataType.Text, Me.Value.Text)
    End Sub
End Class