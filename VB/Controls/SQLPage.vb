Public Class SQLPage : Inherits TabPage
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.RichTextBox1 = New System.Windows.Forms.RichTextBox
        Me.Splitter2 = New System.Windows.Forms.Splitter
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip
        Me.DataGridView1 = New System.Windows.Forms.DataGridView
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel
        Me.ToolStripToolStripButton = New System.Windows.Forms.ToolStripButton
        Me.StatusStrip1.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        '
        'RichTextBox1
        '
        Me.RichTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.RichTextBox1.DetectUrls = False
        Me.RichTextBox1.Dock = System.Windows.Forms.DockStyle.Top
        Me.RichTextBox1.HideSelection = False
        Me.RichTextBox1.Location = New System.Drawing.Point(3, 3)
        Me.RichTextBox1.Name = "RichTextBox1"
        Me.RichTextBox1.Size = New System.Drawing.Size(552, 121)
        Me.RichTextBox1.TabIndex = 0
        Me.RichTextBox1.Text = ""
        '
        'Splitter2
        '
        Me.Splitter2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Splitter2.Location = New System.Drawing.Point(3, 124)
        Me.Splitter2.Name = "Splitter2"
        Me.Splitter2.Size = New System.Drawing.Size(552, 3)
        Me.Splitter2.TabIndex = 1
        Me.Splitter2.TabStop = False
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1, Me.ToolStripToolStripButton})
        Me.StatusStrip1.Location = New System.Drawing.Point(3, 422)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(552, 22)
        Me.StatusStrip1.TabIndex = 2
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToOrderColumns = True
        Me.DataGridView1.AllowUserToResizeRows = False
        Me.DataGridView1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystrokeOrF2
        Me.DataGridView1.EnableHeadersVisualStyles = False
        Me.DataGridView1.Location = New System.Drawing.Point(3, 127)
        Me.DataGridView1.MultiSelect = True
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.DataGridView1.Size = New System.Drawing.Size(552, 295)
        Me.DataGridView1.TabIndex = 3
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(162, 17)
        Me.ToolStripStatusLabel1.Text = "Execution status : Not executed"

        Me.ToolStripToolStripButton.Text = "Cancel"
        Me.ToolStripToolStripButton.BackColor = Color.LightGray

        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Controls.Add(Me.RichTextBox1)
        Me.Controls.Add(Me.Splitter2)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.DataGridView1)
        Me.RichTextBox1.BringToFront()
        Me.Splitter2.BringToFront()
        Me.DataGridView1.BringToFront()
    End Sub
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents Splitter2 As System.Windows.Forms.Splitter
    Friend WithEvents RichTextBox1 As System.Windows.Forms.RichTextBox
    Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripToolStripButton As System.Windows.Forms.ToolStripButton
    Private TempTextBox As New TextBox
    Friend oDataSource As DataSource
    Friend DataTable As New DataTable
    Friend Thread As Threading.Thread
    Friend WithEvents Timer As New Timer
    Friend LastSQL As String
    Private Command As Object = Nothing
    Private Adapter As Object = Nothing
    Private Builder As Object = Nothing
    Public Property DataSource() As DataSource
        Get
            Return oDataSource
        End Get
        Set(ByVal value As DataSource)
            oDataSource = value
            If Me.Command Is Nothing Then
                Me.Command = Me.DataSource.CreateCommand
                Me.Adapter = Me.DataSource.CreateAdapter(Me.Command)
                Me.Builder = Me.DataSource.CreateBuilder(Me.Adapter)
            End If
        End Set
    End Property
    Public Sub New()
        Me.InitializeComponent()
        Me.Timer.Interval = 100
        Me.ToolStripToolStripButton.Visible = False
        Me.DataGridView1.ContextMenuStrip = New ContextMenuStrip
        Me.DataGridView1.ContextMenuStrip.Items.Add("Export", Nothing, AddressOf Export_Click)
    End Sub
    Private Sub Export_Click(ByVal sender As Object, ByVal e As EventArgs)
        Me.DataSource.Export(Me.DataTable)
    End Sub
    Public Sub ShowSQL(ByVal SQL As String)
        TempTextBox.Text = SQL
        Me.RichTextBox1.Text = TempTextBox.Text
        TempTextBox.Text = ""
    End Sub
    Private Sub DataGridView1_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles DataGridView1.DataError
        e.ThrowException = False
    End Sub
    Public Function ExecuteSQL() As Boolean
        Dim SQL As String = Me.RichTextBox1.Text
        If Me.Thread Is Nothing OrElse Me.Thread.ThreadState = Threading.ThreadState.Stopped OrElse Me.Thread.ThreadState = Threading.ThreadState.Unstarted OrElse Me.Thread.ThreadState = Threading.ThreadState.Aborted Then
            If UCase(Trim(SQL)).StartsWith("SELECT") Then
                DataTable.Reset()
                Me.DataGridView1.Visible = True
                Me.LastSQL = SQL
                Me.ToolStripStatusLabel1.Text = "Executing..."
                Me.ToolStripToolStripButton.Visible = True
                Me.Timer.Start()
                Me.Thread = New Threading.Thread(AddressOf Execute)
                Me.Thread.Start()
            Else
                Try
                    Me.LastSQL = SQL
                    If (UCase(Me.LastSQL).IndexOf("DROP") = -1 OrElse UCase(Me.LastSQL).IndexOf("DELETE") = -1) AndAlso MsgBox("Are you sure to do this action?", MsgBoxStyle.OkCancel) = MsgBoxResult.Cancel Then
                        Me.ToolStripStatusLabel1.Text = "Cancelled"
                        Return False
                    End If

                    Me.DataGridView1.Visible = False
                    Dim timeStarts As DateTime = Now
                    Dim timeEnds As DateTime
                    Me.ToolStripStatusLabel1.Text = "Effected row count " & Me.DataSource.ExecuteQuery(Me.Command, SQL) & ". Execution time (m:sn:ms): " & timeEnds.Subtract(timeStarts).Minutes & ":" & timeEnds.Subtract(timeStarts).Seconds & ":" & timeEnds.Subtract(timeStarts).Milliseconds
                Catch ex As Exception
                    MsgBox("Exception occured : " & ex.Message, MsgBoxStyle.Critical)
                    Utility.LastOccuredException = ex
                    Return False
                End Try
                Me.ToolStripToolStripButton.Visible = False
            End If
            Me.Text = Microsoft.VisualBasic.Left(Me.LastSQL, 20)
        End If
        Return True
    End Function
    Private Sub Execute()
        Try
            Dim timeStarts As DateTime = Now
            Dim timeEnds As DateTime
            Me.DataTable = Me.DataSource.GetDataTable(Me.Command, Me.Adapter, Me.LastSQL)
            timeEnds = Now
            Me.ToolStripStatusLabel1.Text = "Total row count " & Me.DataTable.Rows.Count & ". Execution time (m:sn:ms): " & timeEnds.Subtract(timeStarts).Minutes & ":" & timeEnds.Subtract(timeStarts).Seconds & ":" & timeEnds.Subtract(timeStarts).Milliseconds
        Catch ex As Exception
            If ex.Message.IndexOf("aborted") = -1 Then
                MsgBox("Exception occured : " & ex.Message, MsgBoxStyle.Critical)
                Utility.LastOccuredException = ex
            End If
        End Try
        Me.ToolStripToolStripButton.Visible = False
    End Sub
    Private Sub Timer_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer.Tick
        If Not Me.Thread Is Nothing AndAlso Me.Thread.ThreadState = Threading.ThreadState.Stopped Then
            Me.Timer.Stop()
            Me.DataGridView1.DataSource = Me.DataTable
            Me.Thread = Nothing
        End If
    End Sub
    Private Sub ToolStripToolStripButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ToolStripToolStripButton.Click
        Me.Cancel()
    End Sub
    Private Sub Cancel()
        If Not Me.Thread Is Nothing Then
            Me.DataSource.Cancel(Me.Command)
            Me.DataGridView1.DataSource = Me.DataTable
            Me.ToolStripStatusLabel1.Text = "Execution cancelled."
            Me.ToolStripToolStripButton.Visible = False
            Try
                Me.Thread.Abort()
                Me.Thread = Nothing
            Catch ex As Exception

            End Try
        End If
    End Sub
    Private Sub RichTextBox1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles RichTextBox1.KeyDown
        If e.Control AndAlso e.KeyCode = Keys.F Then
            Utility.AskTheExceptionGoogle()
        ElseIf e.KeyCode = Keys.F5 Then
            Me.ExecuteSQL()
        ElseIf e.KeyCode = Keys.F8 Then
            Me.Cancel()
        ElseIf e.Control AndAlso e.KeyCode = Keys.S Then
            Me.DataSource.SaveQuery(Me.RichTextBox1.Text)
        ElseIf e.Control AndAlso e.KeyCode = Keys.O Then
            Me.RichTextBox1.Text = Me.DataSource.OpenQuery()
        End If
    End Sub
    Public Sub AcceptChanges()
        If Not Me.DataGridView1.DataSource Is Nothing AndAlso Me.DataGridView1.Visible Then
            Me.DataSource.UpdateData(Me.Adapter, Me.DataGridView1.DataSource)
        End If
    End Sub
End Class
