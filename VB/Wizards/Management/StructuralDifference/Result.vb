Public Class Result
    Public DataSource1, DataSource2 As DataSource
    Private Sub Result_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Enabled = False
        Me.TreeView2.ContextMenuStrip = New ContextMenuStrip
        Me.TreeView1.ContextMenuStrip = New ContextMenuStrip
        Me.Label1.Text = "Processing..."
        Me.Label2.Text = ""
        System.Windows.Forms.Cursor.Current = Cursors.WaitCursor
        Me.Populate()
        System.Windows.Forms.Cursor.Current = Cursors.Default
    End Sub
    Private Sub TreeView1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles TreeView1.MouseDown, TreeView2.MouseDown
        Dim Node As TreeNode = sender.GetNodeAt(e.Location)
        sender.SelectedNode = Node
        sender.ContextMenuStrip.Items.Clear()
        If e.Button = Windows.Forms.MouseButtons.Right Then
            AddHandler DirectCast(sender, TreeView).ContextMenuStrip.Items.Add("Collapse all").Click, AddressOf Collapse
            AddHandler DirectCast(sender, TreeView).ContextMenuStrip.Items.Add("Expand all").Click, AddressOf Expand
            AddHandler DirectCast(sender, TreeView).ContextMenuStrip.Items.Add("Just show differences").Click, AddressOf ShowDifferences
        End If
    End Sub
    Private Sub ShowDifferences(ByVal sender As Object, ByVal e As EventArgs)
        Dim n As Integer
        For n = Me.TreeView1.Nodes.Count - 1 To 0 Step -1
            If Me.TreeView1.Nodes(n).BackColor.Name = "0" Then
                Me.TreeView1.Nodes.RemoveAt(n)
            End If
        Next
        For n = Me.TreeView2.Nodes.Count - 1 To 0 Step -1
            If Me.TreeView2.Nodes(n).BackColor.Name = "0" Then
                Me.TreeView2.Nodes.RemoveAt(n)
            End If
        Next
    End Sub
    Private Sub Expand(ByVal sender As Object, ByVal e As EventArgs)
        Me.TreeView2.ExpandAll()
        Me.TreeView1.ExpandAll()
    End Sub
    Private Sub Collapse(ByVal sender As Object, ByVal e As EventArgs)
        Me.TreeView2.CollapseAll()
        Me.TreeView1.CollapseAll()
    End Sub
    Private Sub Populate()
        Try
            Me.DataSource2.Connect()
            Dim Doc As System.Xml.XmlDocument = Me.DataSource1.GetDataDocument
            Dim n As Integer
            Me.Label1.Text = "Populating main data source information..."
            For n = 0 To Me.DataSource1.Tables.Count - 1
                Me.DataSource1.Tables(n).LoadColumns()
                Me.DataSource1.Tables(n).LoadIndexes()
                Me.CreateTreeNode(Me.TreeView1, Nothing, Me.DataSource1.Tables(n), True)
            Next
            Me.Label1.Text = "Populating compared data source information..."
            For n = 0 To Me.DataSource2.Tables.Count - 1
                Me.DataSource2.Tables(n).LoadColumns()
                Me.DataSource2.Tables(n).LoadIndexes()
                Me.CreateTreeNode(Me.TreeView2, Nothing, Me.DataSource2.Tables(n), False)
            Next
            Me.Label1.Text = Me.DataSource1.Location & ";" & Me.DataSource1.DataBase & ";" & Me.DataSource1.UserName
            Me.Label2.Text = Me.DataSource2.Location & ";" & Me.DataSource2.DataBase & ";" & Me.DataSource2.UserName
            Me.TreeView1.Focus()
        Catch ex As Exception
            Me.DataSource2.Disconnect()
            MsgBox("Could not finish comparing. Exception is : " & ex.Message & " " & ex.StackTrace, MsgBoxStyle.Critical)
            Me.Label1.Text = "Error occured."
        End Try
        Me.Enabled = True
    End Sub
    Private Sub CreateTreeNode(ByVal Treeview As TreeView, ByVal ParentNode As TreeNode, ByVal Tag As Object, ByVal Reverse As Boolean, Optional ByVal Tag2 As Object = Nothing)
        Dim Node As TreeNode
        If ParentNode Is Nothing Then
            Node = Treeview.Nodes.Add("")
        Else
            Node = ParentNode.Nodes.Add("")
        End If
        Node.Tag = Tag
        Node.Text = Tag.Name
        Dim n, i As Integer
        If Tag.GetType.Name = "Table" Then
            Dim ColumnsNode As TreeNode = Node.Nodes.Add("Columns")
            Dim IndexesNode As TreeNode = Node.Nodes.Add("Indexes")
            If Reverse Then
                For n = 0 To Me.DataSource2.Tables.Count - 1
                    If DirectCast(Tag, Table).IsEquilant(Me.DataSource2.Tables(n)) Then
                        For i = 0 To DirectCast(Tag, Table).Columns.Count - 1
                            Me.CreateTreeNode(Treeview, ColumnsNode, DirectCast(Tag, Table).Columns(i), Reverse, Me.DataSource2.Tables(n))
                        Next
                        For i = 0 To DirectCast(Tag, Table).Indexes.Count - 1
                            Me.CreateTreeNode(Treeview, IndexesNode, DirectCast(Tag, Table).Indexes(i), Reverse, Me.DataSource2.Tables(n))
                        Next
                        Exit For
                    End If
                Next
                If n = Me.DataSource2.Tables.Count Then
                    Node.BackColor = Color.Red
                    Node.ForeColor = Color.White
                    For i = 0 To DirectCast(Tag, Table).Columns.Count - 1
                        Me.CreateTreeNode(Treeview, ColumnsNode, DirectCast(Tag, Table).Columns(i), Reverse)
                    Next
                    For i = 0 To DirectCast(Tag, Table).Indexes.Count - 1
                        Me.CreateTreeNode(Treeview, IndexesNode, DirectCast(Tag, Table).Indexes(i), Reverse)
                    Next
                End If
            Else
                For n = 0 To Me.DataSource1.Tables.Count - 1
                    If DirectCast(Tag, Table).IsEquilant(Me.DataSource1.Tables(n)) Then
                        For i = 0 To DirectCast(Tag, Table).Columns.Count - 1
                            Me.CreateTreeNode(Treeview, ColumnsNode, DirectCast(Tag, Table).Columns(i), Reverse, Me.DataSource1.Tables(n))
                        Next
                        For i = 0 To DirectCast(Tag, Table).Indexes.Count - 1
                            Me.CreateTreeNode(Treeview, IndexesNode, DirectCast(Tag, Table).Indexes(i), Reverse, Me.DataSource1.Tables(n))
                        Next
                        Exit For
                    End If
                Next
                If n = Me.DataSource1.Tables.Count Then
                    Node.BackColor = Color.Red
                    Node.ForeColor = Color.White
                    For i = 0 To DirectCast(Tag, Table).Columns.Count - 1
                        Me.CreateTreeNode(Treeview, ColumnsNode, DirectCast(Tag, Table).Columns(i), Reverse)
                    Next
                    For i = 0 To DirectCast(Tag, Table).Indexes.Count - 1
                        Me.CreateTreeNode(Treeview, IndexesNode, DirectCast(Tag, Table).Indexes(i), Reverse)
                    Next
                End If
            End If
        ElseIf Tag.GetType.Name = "Column" Then
            Dim Column As Column = Tag
            Dim OtherTable As Table = Tag2
            Dim BackColor, ForeColor As Color
            If OtherTable Is Nothing Then
                BackColor = Color.Red
                ForeColor = Color.White
            Else
                OtherTable.LoadColumns()
                For n = 0 To OtherTable.Columns.Count - 1
                    If LCase(Column.Name).Replace("ý", "i") = LCase(OtherTable.Columns(n).Name).Replace("ý", "i") Then
                        If Column.IsEquilant(OtherTable.Columns(n)) Then
                            BackColor = Color.White
                            ForeColor = Color.Black
                        Else
                            BackColor = Color.LightGreen
                            ForeColor = Color.Black
                        End If
                        Exit For
                    End If
                Next
                If n = OtherTable.Columns.Count Then
                    BackColor = Color.Red
                    ForeColor = Color.White
                End If
            End If
            Node.BackColor = BackColor
            Node.ForeColor = ForeColor
            Dim InnerNode As TreeNode = Node.Nodes.Add("Default value (" & Column.Default & ")")
            InnerNode.BackColor = BackColor
            InnerNode.ForeColor = ForeColor
            InnerNode = Node.Nodes.Add("Nullable (" & Column.Nullable & ")")
            InnerNode.BackColor = BackColor
            InnerNode.ForeColor = ForeColor
            InnerNode = Node.Nodes.Add("DataType (" & Column.DataType & ")")
            InnerNode.BackColor = BackColor
            InnerNode.ForeColor = ForeColor
            InnerNode = Node.Nodes.Add("DataLength (" & Column.DataLength & ")")
            InnerNode.BackColor = BackColor
            InnerNode.ForeColor = ForeColor
            InnerNode = Node.Nodes.Add("Scale (" & Column.Scale & ")")
            InnerNode.BackColor = BackColor
            InnerNode.ForeColor = ForeColor
            InnerNode = Node.Nodes.Add("Precision (" & Column.Precision & ")")
            InnerNode.BackColor = BackColor
            InnerNode.ForeColor = ForeColor
            InnerNode = Node.Nodes.Add("DateTimePrecision (" & Column.DateTimePrecision & ")")
            InnerNode.BackColor = BackColor
            InnerNode.ForeColor = ForeColor
            InnerNode = Node.Nodes.Add("CharSetSchema (" & Column.CharSetSchema & ")")
            InnerNode.BackColor = BackColor
            InnerNode.ForeColor = ForeColor
            InnerNode = Node.Nodes.Add("CharSetName (" & Column.CharSetName & ")")
            InnerNode.BackColor = BackColor
            InnerNode.ForeColor = ForeColor
            If BackColor <> Color.White Then
                ParentNode.BackColor = BackColor
                ParentNode.ForeColor = ForeColor
                ParentNode.Parent.BackColor = BackColor
                ParentNode.Parent.ForeColor = ForeColor
            End If
        ElseIf Tag.GetType.Name = "Index" Then
            Dim Index As Index = Tag
            Dim OtherTable As Table = Tag2
            Dim BackColor, ForeColor As Color
            If OtherTable Is Nothing Then
                BackColor = Color.Red
                ForeColor = Color.White
            Else
                OtherTable.LoadIndexes()
                For n = 0 To OtherTable.Indexes.Count - 1
                    If LCase(Index.Name).Replace("ý", "i") = LCase(OtherTable.Indexes(n).Name).Replace("ý", "i") Then
                        If Index.IsEquilant(OtherTable.Indexes(n)) Then
                            BackColor = Color.White
                            ForeColor = Color.Black
                        Else
                            BackColor = Color.LightGreen
                            ForeColor = Color.Black
                        End If
                        Exit For
                    End If
                Next
                If n = OtherTable.Indexes.Count Then
                    BackColor = Color.Red
                    ForeColor = Color.White
                End If
            End If
            Node.BackColor = BackColor
            Node.ForeColor = ForeColor
            If BackColor <> Color.White Then
                ParentNode.BackColor = BackColor
                ParentNode.ForeColor = ForeColor
                ParentNode.Parent.BackColor = BackColor
                ParentNode.Parent.ForeColor = ForeColor
            End If
        ElseIf Tag.GetType.Name = "Sequence" Then
            If Reverse Then
                For n = 0 To Me.DataSource2.Sequences.Count - 1
                    If DirectCast(Tag, Sequence).IsEquilant(Me.DataSource2.Sequences(n)) Then
                        Exit For
                    End If
                Next
                If n = Me.DataSource2.Sequences.Count Then
                    Node.BackColor = Color.Red
                    Node.ForeColor = Color.White
                End If
            Else
                For n = 0 To Me.DataSource1.Sequences.Count - 1
                    If DirectCast(Tag, Sequence).IsEquilant(Me.DataSource1.Sequences(n)) Then
                        Exit For
                    End If
                Next
                If n = Me.DataSource1.Sequences.Count Then
                    Node.BackColor = Color.Red
                    Node.ForeColor = Color.White
                End If
            End If
        End If
    End Sub
    Private Sub TreeView1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TreeView1.KeyDown, TreeView2.KeyDown
        If e.KeyCode = Keys.Escape Then
            If MsgBox("Are you sure to exit", MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
                Me.DialogResult = Windows.Forms.DialogResult.Cancel
            End If
        End If
    End Sub
End Class