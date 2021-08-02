Public Class EditorForm
    Public InstanceID As Decimal = 0
    Private oDataSource As DataSource
    Private CurrentNode As TreeNode
    Private SequencesGenerated As Boolean = False
    Public Property DataSource() As DataSource
        Get
            Return oDataSource
        End Get
        Set(ByVal value As DataSource)
            oDataSource = value
            Me.Text = "Editor : " & Me.DataSource.Location & " / " & Me.DataSource.UserName
        End Set
    End Property
    Public Sub New()
        InitializeComponent()
        Randomize()
        Me.InstanceID = Rnd()
    End Sub
    Public Sub New(ByVal DataSource As DataSource)
        InitializeComponent()
        Me.DataSource = DataSource
        Randomize()
        Me.InstanceID = Rnd()
    End Sub
    Private Sub EditorForm_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Dim Container As Container = Me.MdiParent
        Container.ActivateForm(Me)
    End Sub
    Private Sub EditorForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Me.DataSource.Disconnect()
    End Sub
    Private Sub EditorForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.BindTables()
        Me.TreeView1.ContextMenuStrip = New ContextMenuStrip
        Me.TreeView2.ContextMenuStrip = New ContextMenuStrip
        Me.TreeView3.ContextMenuStrip = New ContextMenuStrip
        Me.TreeView4.ContextMenuStrip = New ContextMenuStrip
        Select Case Me.DataSource.Type
            Case ConnectionType.SQLServer
                Me.TabControl1.TabPages.Remove(Me.TabPage2)
                Me.TabControl1.TabPages.Remove(Me.TabPage4)
            Case ConnectionType.Access, ConnectionType.Excel, ConnectionType.FoxPro
                Me.TabControl1.TabPages.Remove(Me.TabPage2)
                Me.TabControl1.TabPages.Remove(Me.TabPage3)
                Me.TabControl1.TabPages.Remove(Me.TabPage4)
        End Select
    End Sub
    Public Sub BindTables()
        Me.TreeView1.Nodes.Clear()
        Dim n As Integer
        Dim Node As TreeNode
        For n = 0 To Me.DataSource.Tables.Count - 1
            Node = New TreeNode
            Dim table As Table = Me.DataSource.Tables(n)
            Node.Tag = table
            If String.IsNullOrEmpty(table.SchemaName) Then
                Node.Text = table.Name
            Else
                Node.Text = table.SchemaName & "." & table.Name
            End If
            Me.TreeView1.Nodes.Add(Node)
            If Node.Tag.GetType.Name = "Table" Then
                Node.Nodes.Add("Columns").Nodes.Add("")
                Node.Nodes.Add("Indexes").Nodes.Add("")
            End If
        Next
    End Sub
    Public Sub BindSequences()
        If Not Me.SequencesGenerated Then
            Dim n As Integer
            Dim Node As TreeNode
            For n = 0 To Me.DataSource.Sequences.Count - 1
                Node = New TreeNode
                Node.Tag = Me.DataSource.Sequences(n)
                Node.Text = Node.Tag.Name
                Me.TreeView2.Nodes.Add(Node)
            Next
            Me.SequencesGenerated = True
        End If
    End Sub
    Private Sub TreeView1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles TreeView1.MouseDown
        Dim Node As TreeNode = Me.TreeView1.GetNodeAt(e.Location)
        If Not Node Is Nothing Then
            CurrentNode = Node
            Me.TreeView1.SelectedNode = CurrentNode
            If (Node.Text = "Columns" OrElse Node.Text = "Indexes") AndAlso Node.Nodes.Count = 1 AndAlso Node.Nodes(0).Text = "" Then
                System.Windows.Forms.Cursor.Current = Cursors.WaitCursor
                FillColumnAndIndexes()
                System.Windows.Forms.Cursor.Current = Cursors.Default
            End If
        Else
            CurrentNode = Nothing
            Me.TreeView1.SelectedNode = CurrentNode
        End If
        If e.Button = Windows.Forms.MouseButtons.Right Then
            Me.ShowMenuItems(Me.TreeView1, Node)
        End If
    End Sub
    Private Sub FillColumnAndIndexes()
        If CurrentNode.Text = "Columns" Then
            If CurrentNode.Nodes.Count > 0 Then CurrentNode.Nodes(0).Remove()
            DirectCast(DirectCast(CurrentNode.Parent, TreeNode).Tag, Table).LoadColumns()
            Dim n As Integer
            Dim oNode As TreeNode
            For n = 0 To DirectCast(DirectCast(CurrentNode.Parent, TreeNode).Tag, Table).Columns.Count - 1
                oNode = New TreeNode
                oNode.Tag = DirectCast(DirectCast(CurrentNode.Parent, TreeNode).Tag, Table).Columns(n)
                oNode.Text = oNode.Tag.Name
                CurrentNode.Nodes.Add(oNode)
            Next
        ElseIf CurrentNode.Text = "Indexes" Then
            If CurrentNode.Nodes.Count > 0 Then CurrentNode.Nodes(0).Remove()
            DirectCast(DirectCast(CurrentNode.Parent, TreeNode).Tag, Table).LoadIndexes()
            Dim n As Integer
            Dim oNode As TreeNode
            For n = 0 To DirectCast(DirectCast(CurrentNode.Parent, TreeNode).Tag, Table).Indexes.Count - 1
                oNode = New TreeNode
                oNode.Tag = DirectCast(DirectCast(CurrentNode.Parent, TreeNode).Tag, Table).Indexes(n)
                oNode.Text = oNode.Tag.Name
                CurrentNode.Nodes.Add(oNode)
            Next
        End If
    End Sub
    Private Sub ShowMenuItems(ByVal TreeView As TreeView, ByVal Node As TreeNode)
        TreeView.ContextMenuStrip.Items.Clear()
        If TreeView Is Me.TreeView1 Then
            If Not Node Is Nothing AndAlso Node.GetType.Name = "TreeNode" Then
                Dim TreeNode As TreeNode = Node
                If Not TreeNode.Tag Is Nothing Then
                    If TreeNode.Tag.GetType.Name = "Table" Then
                        AddHandler TreeView.ContextMenuStrip.Items.Add("Show data").Click, AddressOf ShowTableData
                        AddHandler TreeView.ContextMenuStrip.Items.Add("Export").Click, AddressOf Export_Click
                        If Me.DataSource.Type <> ConnectionType.Excel Then
                            AddHandler TreeView.ContextMenuStrip.Items.Add("New column").Click, AddressOf NewColumn
                            AddHandler TreeView.ContextMenuStrip.Items.Add("Drop").Click, AddressOf DropTable
                        End If
                        If Me.DataSource.Type = ConnectionType.Oracle Then
                            AddHandler TreeView.ContextMenuStrip.Items.Add("Analyze").Click, AddressOf AnalyzeTable_Click
                        End If
                        AddHandler TreeView.ContextMenuStrip.Items.Add("Copy name").Click, AddressOf CopyName
                        AddHandler TreeView.ContextMenuStrip.Items.Add("Refresh all").Click, AddressOf RefreshTable_Click
                        If Me.DataSource.Type = ConnectionType.Oracle Then
                            AddHandler TreeView.ContextMenuStrip.Items.Add("Analyze all").Click, AddressOf AnalyzeAll_Click
                            AddHandler TreeView.ContextMenuStrip.Items.Add("Show tables at different tablespace").Click, AddressOf WrongTableTablespace_Click
                            AddHandler TreeView.ContextMenuStrip.Items.Add("Show indexes at different tablespace").Click, AddressOf WrongIndexTablespace_Click
                            AddHandler TreeView.ContextMenuStrip.Items.Add("Correct tablespaces").Click, AddressOf CorrectTablespace_Click
                        End If
                        If Me.DataSource.Type <> ConnectionType.XML Then
                            AddHandler TreeView.ContextMenuStrip.Items.Add("Compare structure").Click, AddressOf CompareStructure_Click
                        End If
                    ElseIf TreeNode.Tag.GetType.Name = "Column" Then
                        If Me.DataSource.Type <> ConnectionType.Excel Then
                            AddHandler TreeView.ContextMenuStrip.Items.Add("Index").Click, AddressOf NewIndex
                        End If
                        AddHandler TreeView.ContextMenuStrip.Items.Add("Copy name").Click, AddressOf CopyName
                    ElseIf TreeNode.Tag.GetType.Name = "Index" Then
                        AddHandler TreeView.ContextMenuStrip.Items.Add("Copy name").Click, AddressOf CopyName
                        If Me.DataSource.Type <> ConnectionType.Excel Then
                            AddHandler TreeView.ContextMenuStrip.Items.Add("Drop").Click, AddressOf DropIndex
                            AddHandler TreeView.ContextMenuStrip.Items.Add("Rebuild").Click, AddressOf RebuildIndex
                        End If
                        If Me.DataSource.Type = ConnectionType.Oracle Then
                            AddHandler TreeView.ContextMenuStrip.Items.Add("Analyze").Click, AddressOf AnalyzeIndex_Click
                        End If
                    End If
                ElseIf Node.Text = "Columns" Then
                    If Me.DataSource.Type <> ConnectionType.Excel Then
                        AddHandler TreeView.ContextMenuStrip.Items.Add("New column").Click, AddressOf NewColumn
                    End If
                    AddHandler TreeView.ContextMenuStrip.Items.Add("Refresh").Click, AddressOf RefreshColumns
                ElseIf Node.Text = "Indexes" Then
                    AddHandler TreeView.ContextMenuStrip.Items.Add("Refresh").Click, AddressOf RefreshIndexes
                End If
            Else
                AddHandler TreeView.ContextMenuStrip.Items.Add("Refresh").Click, AddressOf RefreshTable_Click
                If Me.DataSource.Type = ConnectionType.Oracle Then
                    AddHandler TreeView.ContextMenuStrip.Items.Add("Analyze all").Click, AddressOf AnalyzeAll_Click
                    AddHandler TreeView.ContextMenuStrip.Items.Add("Show tables at different tablespace").Click, AddressOf WrongTableTablespace_Click
                    AddHandler TreeView.ContextMenuStrip.Items.Add("Show indexes at different tablespace").Click, AddressOf WrongIndexTablespace_Click
                    AddHandler TreeView.ContextMenuStrip.Items.Add("Correct tablespaces").Click, AddressOf CorrectTablespace_Click
                End If
                If Me.DataSource.Type <> ConnectionType.XML Then
                    AddHandler TreeView.ContextMenuStrip.Items.Add("Compare structure").Click, AddressOf CompareStructure_Click
                End If
            End If
        End If
    End Sub
    Private Sub CompareStructure_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim Compare As New CompareStep1
        Compare.DataSource = Me.DataSource
        If Compare.ShowDialog = Windows.Forms.DialogResult.OK AndAlso Not Compare.DestinationDataSource Is Nothing Then
            Dim Result As New Result
            Result.DataSource1 = Me.DataSource
            Result.DataSource2 = Compare.DestinationDataSource
            Result.ShowDialog()
        End If
    End Sub
    Private Sub CorrectTablespace_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim CorrectTablespace As New CorrectTablespace(Me.DataSource, Me)
        CorrectTablespace.ShowDialog()
    End Sub
    Private Sub WrongTableTablespace_Click(ByVal sender As Object, ByVal e As EventArgs)
        Me.ShowSQL("SELECT TABLE_NAME FROM ALL_TABLES WHERE OWNER='" & Me.DataSource.UserName & "' AND TABLESPACE_NAME <> '" & Me.DataSource.UserName & "'")
    End Sub
    Private Sub WrongIndexTablespace_Click(ByVal sender As Object, ByVal e As EventArgs)
        Me.ShowSQL("SELECT INDEX_NAME FROM ALL_INDEXES WHERE OWNER='" & Me.DataSource.UserName & "' AND TABLESPACE_NAME <> '" & Me.DataSource.UserName & "'")
    End Sub
    Private Sub AnalyzeAll_Click(ByVal sender As Object, ByVal e As EventArgs)
        If MsgBox("Are you sure to do this action?", MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
            Dim Message As New MessageForm
            Message.MessageToShow = "Analyzing current data source."
            Message.Form = Me
            Dim Thread As New Threading.Thread(AddressOf Me.DataSource.Analyze)
            Message.Thread = Thread
            Thread.Start()
            Me.Enabled = False
            Message.ShowDialog()
        End If
    End Sub
    Public Sub AfterProcess()
        Me.Enabled = True
        MsgBox("Action is completed.", MsgBoxStyle.Information)
    End Sub
    Private Sub AnalyzeTable_Click(ByVal sender As Object, ByVal e As EventArgs)
        Me.ShowSQL("Analyze Table " & Me.DataSource.GetOpenBracket & Me.CurrentTable.Name & Me.DataSource.GetCloseBracket & " Estimate Statistics")
    End Sub
    Private Sub AnalyzeIndex_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim Index As Index = Me.CurrentNode.Tag
        Me.ShowSQL("ANALYZE INDEX " & Me.DataSource.GetOpenBracket & Index.Name & Me.DataSource.GetCloseBracket & " Estimate STATISTICS")
    End Sub
    Private Sub RefreshTable_Click(ByVal sender As Object, ByVal e As EventArgs)
        Me.DataSource.ResetTables()
        Me.BindTables()
    End Sub
    Private Sub Export_Click(ByVal sender As Object, ByVal e As EventArgs)
        Me.DataSource.Export(Nothing, Me.CurrentTable.Name)
    End Sub
    Public Sub SelectNode(ByVal Name As String)
        Dim Node As TreeNode = Me.Item(Name)
        If Not Node Is Nothing Then
            Me.TreeView1.SelectedNode = Node
        End If
    End Sub
    Default Public ReadOnly Property Item(ByVal Name As String) As TreeNode
        Get
            Return Me.GetNode(Me.TreeView1.Nodes(0), Name)
        End Get
    End Property
    Public Function GetNode(ByVal Text As String) As TreeNode
        Dim n As Integer
        Dim SelectedNode As TreeNode = Nothing
        For n = 0 To Me.TreeView1.Nodes.Count - 1
            If SelectedNode Is Nothing Then
                If Me.TreeView1.Nodes(n).Text = Text Then
                    Return Me.TreeView1.Nodes(n)
                Else
                    SelectedNode = Me.GetNode(Me.TreeView1.Nodes(n), Text)
                End If
            Else
                Exit For
            End If
        Next
        Return SelectedNode
    End Function
    Public Function GetNode(ByVal Node As TreeNode, ByVal Text As String) As TreeNode
        Dim n As Integer
        Dim SelectedNode As TreeNode = Nothing
        For n = 0 To Node.Nodes.Count - 1
            If SelectedNode Is Nothing Then
                If Node.Nodes(n).Text = Text Then
                    Return Node.Nodes(n)
                Else
                    SelectedNode = Me.GetNode(Node.Nodes(n), Text)
                End If
            Else
                Exit For
            End If
        Next
        Return SelectedNode
    End Function
    Public ReadOnly Property CurrentTable() As Table
        Get
            If Not Me.CurrentNode Is Nothing AndAlso Not Me.CurrentNode.Tag Is Nothing Then
                Select Case Me.CurrentNode.Tag.GetType.Name
                    Case "Table"
                        Return Me.CurrentNode.Tag
                    Case "Column"
                        Return Me.CurrentNode.Tag.Table
                    Case "Index"
                        Return Me.CurrentNode.Tag.Table
                    Case Else
                        If Me.CurrentNode.Text = "Indexes" OrElse Me.CurrentNode.Text = "Columns" Then
                            Return Me.CurrentNode.Tag
                        End If
                End Select
            ElseIf Not Me.CurrentNode Is Nothing Then
                Return Me.CurrentNode.Parent.Tag
            End If
            Return Nothing
        End Get
    End Property
    Public Sub RefreshColumns()
        If Not Me.CurrentTable Is Nothing Then
            Dim Table As Table = Me.CurrentTable
            Dim Node As TreeNode = Me.GetNode(Table.Name)
            Node.Nodes(0).Nodes.Clear()
            Me.CurrentNode = Node.Nodes(0)
            Table.ResetColumns()
            Me.FillColumnAndIndexes()
        End If
    End Sub
    Public Sub RefreshIndexes()
        If Not Me.CurrentTable Is Nothing Then
            Dim Table As Table = Me.CurrentTable
            Dim Node As TreeNode = Me.GetNode(Table.Name)
            Node.Nodes(1).Nodes.Clear()
            Me.CurrentNode = Node.Nodes(1)
            Table.ResetIndexes()
            Me.FillColumnAndIndexes()
        End If
    End Sub
    Private Sub PopulateSequences()

    End Sub
    Private Sub PopulateTablespaces()

    End Sub
    Private Sub PopulateUsers()

    End Sub
    Private Sub NewColumn(ByVal sender As Object, ByVal e As EventArgs)
        Dim NewColumn As New NewColumn(Me.CurrentTable, Me)
        NewColumn.ShowDialog()
    End Sub
    Private Sub CopyName(ByVal sender As Object, ByVal e As EventArgs)
        If Not Me.CurrentNode Is Nothing Then My.Computer.Clipboard.SetText(Me.CurrentNode.Text)
    End Sub
    Private Sub NewQuery(ByVal sender As Object, ByVal e As EventArgs)
        Me.ShowSQL("")
    End Sub
    Private Sub RefreshIndexes(ByVal sender As Object, ByVal e As EventArgs)
        Me.CurrentNode.Nodes.Clear()
        Me.CurrentTable.ResetIndexes()
        Me.FillColumnAndIndexes()
    End Sub
    Private Sub RefreshColumns(ByVal sender As Object, ByVal e As EventArgs)
        Me.CurrentNode.Nodes.Clear()
        Me.CurrentTable.ResetColumns()
        Me.FillColumnAndIndexes()
    End Sub
    Private Sub ShowTableData(ByVal sender As Object, ByVal e As EventArgs)
        Dim table As Table = Me.CurrentNode.Tag
        If String.IsNullOrEmpty(table.SchemaName) Then
            Me.ShowSQL("SELECT * FROM " & Me.DataSource.GetOpenBracket & table.Name & Me.DataSource.GetCloseBracket)
        Else
            Me.ShowSQL("SELECT * FROM " & Me.DataSource.GetOpenBracket & table.SchemaName & Me.DataSource.GetCloseBracket & "." & Me.DataSource.GetOpenBracket & table.Name & Me.DataSource.GetCloseBracket)
        End If
    End Sub
    Private Sub DropIndex(ByVal sender As Object, ByVal e As EventArgs)
        Dim Index As Index = Me.CurrentNode.Tag
        Me.ShowSQL("DROP INDEX " & Me.DataSource.GetOpenBracket & Index.Name & Me.DataSource.GetCloseBracket)
    End Sub
    Private Sub DropColumn(ByVal sender As Object, ByVal e As EventArgs)
        Me.ShowSQL("ALTER TABLE " & Me.DataSource.GetOpenBracket & DirectCast(Me.CurrentNode.Tag, Column).Table.Name & Me.DataSource.GetCloseBracket & " DROP COLUMN " & Me.DataSource.GetOpenBracket & Me.CurrentNode.Tag.Name & Me.DataSource.GetCloseBracket)
    End Sub
    Private Sub DropTable(ByVal sender As Object, ByVal e As EventArgs)
        Dim table As Table = Me.CurrentNode.Tag
        If String.IsNullOrEmpty(table.SchemaName) Then
            Me.ShowSQL("DROP TABLE " & Me.DataSource.GetOpenBracket & table.Name & Me.DataSource.GetCloseBracket)
        Else
            Me.ShowSQL("DROP TABLE " & Me.DataSource.GetOpenBracket & table.SchemaName & Me.DataSource.GetCloseBracket & "." & Me.DataSource.GetOpenBracket & table.Name & Me.DataSource.GetCloseBracket)
        End If
    End Sub
    Private Sub RebuildIndex(ByVal sender As Object, ByVal e As EventArgs)
        Me.ShowSQL("ALTER INDEX " & Me.DataSource.GetOpenBracket & Me.CurrentNode.Tag.Name & Me.DataSource.GetCloseBracket & " REBUILD")
    End Sub
    Private Sub NewIndex(ByVal sender As Object, ByVal e As EventArgs)
        Dim Column As Column = Me.CurrentNode.Tag
        Me.ShowSQL("CREATE INDEX IND" & Column.Name & " ON " & Me.DataSource.GetOpenBracket & Column.Table.Name & Me.DataSource.GetCloseBracket & "(" & Me.DataSource.GetOpenBracket & Column.Name & Me.DataSource.GetCloseBracket & ") TABLESPACE " & Column.Table.DataSource.UserName & "IND")
    End Sub
    Public Function ShowData() As Boolean
        Dim SQLPage As SQLPage = Me.ShowSQL("")
        Dim DataSet As New DataSet
        Try
            DataSet.ReadXml(Me.DataSource.Location)
        Catch ex As Exception
            MsgBox("Could not read xml file. Exception is : " & ex.Message)
        End Try
        If DataSet.Tables.Count > 0 Then
            SQLPage.DataGridView1.DataSource = DataSet.Tables(0)
            SQLPage.DataTable = DataSet.Tables(0)
        End If
        SQLPage.RichTextBox1.Visible = False
        Me.TabControl1.Visible = False
    End Function
    Public Function ExecuteSQL(ByVal SQL As String) As Boolean
        Dim SQLPage As SQLPage = Me.ShowSQL(SQL)
        Return SQLPage.ExecuteSQL()
    End Function
    Private Function ShowSQL(ByVal SQL As String) As SQLPage
        Dim SQLPage As New SQLPage
        Me.TabControl2.TabPages.Add(SQLPage)
        SQLPage.ShowSQL(SQL)
        SQLPage.DataSource = Me.DataSource
        Me.TabControl2.SelectedTab = SQLPage
        Return SQLPage
    End Function
    Private Sub TabControl1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabControl1.SelectedIndexChanged
        If Me.TabControl1.SelectedTab.Text = "Sequences" Then
            Me.BindSequences()
        End If
    End Sub
    Private Sub ToolStripButton7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton7.Click
        MsgBox(Me.DataSource.Location & "/" & Me.DataSource.UserName, MsgBoxStyle.Information)
    End Sub
    Private Sub ToolStripButton8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton8.Click
        If Not Me.TabControl2.SelectedTab Is Nothing Then
            DirectCast(Me.TabControl2.SelectedTab, SQLPage).ExecuteSQL()
        End If
    End Sub
    Private Sub ToolStripButton5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton5.Click
        If Not Me.TabControl2.SelectedTab Is Nothing Then
            Me.TabControl2.TabPages.Remove(Me.TabControl2.SelectedTab)
        End If
    End Sub
    Private Sub ToolStripButton4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton4.Click
        If Me.TabControl1.SelectedTab.Text = "Tables" Then
            If Not Me.CurrentNode Is Nothing Then My.Computer.Clipboard.SetText(Me.CurrentNode.Text)
        End If
    End Sub
    Private Sub TreeView2_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles TreeView2.MouseDown
        Dim Node As TreeNode = Me.TreeView2.GetNodeAt(e.Location)
        If Not Node Is Nothing Then Me.CurrentNode = Node
    End Sub
    Private Sub TreeView3_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles TreeView3.MouseDown
        Dim Node As TreeNode = Me.TreeView3.GetNodeAt(e.Location)
        If Not Node Is Nothing Then Me.CurrentNode = Node
    End Sub
    Private Sub TreeView4_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles TreeView4.MouseDown
        Dim Node As TreeNode = Me.TreeView4.GetNodeAt(e.Location)
        If Not Node Is Nothing Then Me.CurrentNode = Node
    End Sub
    Private Sub ToolStripButton6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton6.Click
        If Me.DataSource.Type = ConnectionType.Oracle Then
            Me.ExecuteSQL("select MACHINE, PROGRAM, USERNAME, COUNT(*) AS CONNECTIONCOUNT from v$session GROUP BY MACHINE, PROGRAM, USERNAME ORDER BY CONNECTIONCOUNT DESC")
        End If
    End Sub
    Private Sub ToolStripButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton3.Click
        If Not Me.TabControl2.SelectedTab Is Nothing Then
            DirectCast(Me.TabControl2.SelectedTab, SQLPage).AcceptChanges()
        End If
    End Sub
    Private Sub ToolStripButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton1.Click
        If MsgBox("Are you sure to exit?", MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
            Me.Close()
        End If
    End Sub
    Private Sub ToolStripButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton2.Click
        Me.ShowSQL("")
    End Sub
End Class