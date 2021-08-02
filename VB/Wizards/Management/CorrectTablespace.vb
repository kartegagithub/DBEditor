Public Class CorrectTablespace
    Private DataSource As DataSource
    Private EditorForm As EditorForm
    Public Sub New()
        InitializeComponent()
    End Sub
    Public Sub New(ByVal DataSource As DataSource, ByVal EditorForm As EditorForm)
        Me.New()
        Me.DataSource = DataSource
        Me.EditorForm = EditorForm
        Control.CheckForIllegalCrossThreadCalls = False
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Enabled = False
        Dim Thread As New Threading.Thread(AddressOf Correct)
        Thread.Start()
    End Sub
    Private Sub Correct()
        Try
            Dim DataTable1 As DataTable = Me.DataSource.GetDataTable("SELECT TABLE_NAME FROM ALL_TABLES WHERE OWNER='" & Me.DataSource.UserName & "' AND TABLESPACE_NAME <> '" & Me.Tablespace.Text & "'")
            Dim i As Integer
            For i = 0 To DataTable1.Rows.Count - 1
                Me.Message.Text &= i & " / " & DataTable1.Rows.Count - 1 & " - " & DataTable1.Rows(i)("TABLE_NAME") & vbCrLf
                Me.DataSource.ExecuteQuery("ALTER TABLE " & DataTable1.Rows(i)("TABLE_NAME") & " MOVE TABLESPACE " & Me.Tablespace.Text)
            Next
            DataTable1.Rows.Clear()
            DataTable1.Columns.Clear()

            DataTable1 = Me.DataSource.GetDataTable("SELECT INDEX_NAME FROM ALL_INDEXES WHERE OWNER='" & Me.DataSource.UserName & "' AND TABLESPACE_NAME <> '" & Me.Indexspace.Text & "'")
            For i = 0 To DataTable1.Rows.Count - 1
                Me.Message.Text = i & " / " & DataTable1.Rows.Count - 1 & " - " & DataTable1.Rows(i)("INDEX_NAME") & vbCrLf
                Me.DataSource.ExecuteQuery("ALTER INDEX " & DataTable1.Rows(i)("INDEX_NAME") & " REBUILD TABLESPACE " & Me.Indexspace.Text)
            Next
            DataTable1.Rows.Clear()
            DataTable1.Columns.Clear()
        Catch ex As Exception
            MsgBox("Correction is failed : " & ex.Message, MsgBoxStyle.Critical)
        End Try
        Me.Enabled = True
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub
    Private Sub ColumnName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Tablespace.KeyDown, Indexspace.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
        End If
    End Sub
    Private Sub NewColumn_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Tablespace.Focus()
    End Sub
End Class