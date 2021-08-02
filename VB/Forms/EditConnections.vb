Imports System.Xml
Public Class EditConnections
    Private Doc As XmlDocument
    Private NewContainerIndex As Integer = 1
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub
    Private Sub EditConnections_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.TreeView1.ContextMenuStrip = New ContextMenuStrip
        Me.TreeView1.LabelEdit = True
        Me.LoadConnections()
    End Sub
    Private Sub LoadConnections()
        Dim DataSource As New DataSource
        Doc = DataSource.GetDataDocument
        Dim n As Integer
        Dim ConnectionsNode As Xml.XmlNode = Doc.SelectSingleNode("Connections")
        For n = 0 To ConnectionsNode.ChildNodes.Count - 1
            Me.CreateTreeNode(Nothing, ConnectionsNode.ChildNodes(n))
        Next
    End Sub
    Private Sub CreateTreeNode(ByVal ParentNode As TreeNode, ByVal XmlNode As XmlNode)
        If XmlNode.Name.IndexOf("Connection") > -1 Then
            Dim Node As TreeNode
            If ParentNode Is Nothing Then
                Node = Me.TreeView1.Nodes.Add("")
            Else
                Node = ParentNode.Nodes.Add("")
            End If
            Node.Tag = XmlNode
            If Not XmlNode.Attributes("Name") Is Nothing Then
                Node.Text = XmlNode.Attributes("Name").Value
                Node.BackColor = Color.LightGreen
            Else
                Node.Text = XmlNode.Attributes("Alias").Value
                Node.BackColor = Color.LightBlue
            End If
            Dim n As Integer
            For n = 0 To XmlNode.ChildNodes.Count - 1
                Me.CreateTreeNode(Node, XmlNode.ChildNodes(n))
            Next
        End If
    End Sub
    Public Sub TreeView1_ItemDrag(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ItemDragEventArgs) Handles TreeView1.ItemDrag
        DoDragDrop(e.Item, DragDropEffects.Move)
    End Sub
    Public Sub TreeView1_DragEnter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles TreeView1.DragEnter
        If e.Data.GetDataPresent("System.Windows.Forms.TreeNode", True) Then
            e.Effect = DragDropEffects.Move
        Else
            e.Effect = DragDropEffects.None
        End If
    End Sub
    Public Sub TreeView1_DragOver(ByVal sender As System.Object, ByVal e As DragEventArgs) Handles TreeView1.DragOver
        If e.Data.GetDataPresent("System.Windows.Forms.TreeNode", True) = False Then Exit Sub
        Dim selectedTreeview As TreeView = CType(sender, TreeView)
        Dim pt As Point = CType(sender, TreeView).PointToClient(New Point(e.X, e.Y))
        Dim targetNode As TreeNode = selectedTreeview.GetNodeAt(pt)
        If Not (selectedTreeview Is targetNode) Then
            selectedTreeview.SelectedNode = targetNode
            Dim dropNode As TreeNode = CType(e.Data.GetData("System.Windows.Forms.TreeNode"), TreeNode)
            Do Until targetNode Is Nothing
                If targetNode Is dropNode Then
                    e.Effect = DragDropEffects.None
                    Exit Sub
                End If
                targetNode = targetNode.Parent
            Loop
        End If
        e.Effect = DragDropEffects.Move
    End Sub
    Public Sub TreeView1_DragDrop(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles TreeView1.DragDrop
        If e.Data.GetDataPresent("System.Windows.Forms.TreeNode", True) = False Then Exit Sub
        Dim selectedTreeview As TreeView = CType(sender, TreeView)
        Dim dropNode As TreeNode = CType(e.Data.GetData("System.Windows.Forms.TreeNode"), TreeNode)
        Dim targetNode As TreeNode = selectedTreeview.SelectedNode
        Dim OldXMLNode, NewXMLNode, CurrentNode As XmlNode
        If Not dropNode.Parent Is Nothing Then
            OldXMLNode = dropNode.Parent.Tag
        Else
            OldXMLNode = CType(dropNode.Tag, XmlNode).ParentNode
        End If
        dropNode.Remove()
        If targetNode Is Nothing Then
            selectedTreeview.Nodes.Add(dropNode)
        Else
            CurrentNode = dropNode.Tag
            NewXMLNode = targetNode.Tag
            OldXMLNode.RemoveChild(CurrentNode)
            NewXMLNode.AppendChild(CurrentNode)
            targetNode.Nodes.Add(dropNode)
        End If
        dropNode.EnsureVisible()
        selectedTreeview.SelectedNode = dropNode
    End Sub
    Private Sub TreeView1_AfterLabelEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.NodeLabelEditEventArgs) Handles TreeView1.AfterLabelEdit
        If Not e.Label = "" Then
            Dim XmlNode As XmlNode = e.Node.Tag
            If Not XmlNode.Attributes("Name") Is Nothing Then
                XmlNode.Attributes("Name").InnerText = e.Label
            Else
                XmlNode.Attributes("Alias").InnerText = e.Label
            End If
        Else
            e.CancelEdit = True
        End If
    End Sub
    Private Sub TreeView1_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TreeView1.AfterSelect
        Dim Node As TreeNode = e.Node
        Dim XmlNode As XmlNode = Node.Tag
        If Not XmlNode.Attributes("Name") Is Nothing Then
            Me.Label3.Text = XmlNode.Attributes("Name").Value & " : " & vbCrLf & XmlNode.SelectSingleNode("Location").InnerText & ", " & XmlNode.SelectSingleNode("Catalog").InnerText & ", " & XmlNode.SelectSingleNode("UserName").InnerText
        Else
            Me.Label3.Text = XmlNode.Attributes("Alias").Value
        End If
    End Sub
    Private Sub RemoveNode(ByVal sender As Object, ByVal e As EventArgs)
        Dim XmlNode As XmlNode = Me.TreeView1.SelectedNode.Tag
        Me.TreeView1.SelectedNode.Remove()
        If Not XmlNode.Attributes("Name") Is Nothing Then
            If XmlNode.ParentNode Is Nothing Then
                Me.Doc.SelectSingleNode("Connections").RemoveChild(XmlNode)
            Else
                XmlNode.ParentNode.RemoveChild(XmlNode)
            End If
        Else
            If XmlNode.ParentNode Is Nothing Then
                Me.Doc.SelectSingleNode("Connections").RemoveChild(XmlNode)
            Else
                XmlNode.ParentNode.RemoveChild(XmlNode)
            End If
        End If
    End Sub
    Private Sub CreateNode(ByVal sender As Object, ByVal e As EventArgs)
        Dim NewNode As TreeNode
        If Not Me.TreeView1.SelectedNode Is Nothing Then
            NewNode = Me.TreeView1.SelectedNode.Nodes.Add("New container " & Me.NewContainerIndex)
            NewNode.Tag = CType(Me.TreeView1.SelectedNode.Tag, XmlNode).AppendChild(Me.Doc.CreateElement("Connections"))
            CType(NewNode.Tag, XmlNode).Attributes.Append(Me.Doc.CreateAttribute("Alias")).Value = "New container " & Me.NewContainerIndex
        Else
            NewNode = Me.TreeView1.Nodes.Add("New container " & Me.NewContainerIndex)
            NewNode.Tag = Me.Doc.SelectSingleNode("Connections").AppendChild(Me.Doc.CreateElement("Connections"))
            CType(NewNode.Tag, XmlNode).Attributes.Append(Me.Doc.CreateAttribute("Alias")).Value = "New container " & Me.NewContainerIndex
        End If
        NewNode.BackColor = Color.LightSkyBlue
        Me.NewContainerIndex += 1
        Me.TreeView1.Refresh()
        NewNode.BeginEdit()
    End Sub
    Private Sub TreeView1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles TreeView1.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Right Then
            Me.TreeView1.ContextMenuStrip.Items.Clear()
            Dim Node As TreeNode = Me.TreeView1.GetNodeAt(e.Location)
            If Not Node Is Nothing Then
                Me.TreeView1.SelectedNode = Node
                Dim XmlNode As XmlNode = Node.Tag
                If Not XmlNode.Attributes("Name") Is Nothing Then
                    Me.TreeView1.ContextMenuStrip.Items.Add("Remove connection", Nothing, AddressOf RemoveNode)
                Else
                    Me.TreeView1.ContextMenuStrip.Items.Add("Remove container", Nothing, AddressOf RemoveNode)
                    Me.TreeView1.ContextMenuStrip.Items.Add("CreateContainer", Nothing, AddressOf CreateNode)
                End If
            Else
                Me.TreeView1.SelectedNode = Nothing
                Me.TreeView1.ContextMenuStrip.Items.Add("New container", Nothing, AddressOf CreateNode)
            End If
        End If
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Doc.Save(AppDomain.CurrentDomain.BaseDirectory & "\Data\Connections.xml")
        MsgBox("Connections saved succesfully", MsgBoxStyle.Information)
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub
    Private Sub TreeView1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TreeView1.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
        ElseIf e.KeyCode = Keys.Enter Then
            Me.Doc.Save(AppDomain.CurrentDomain.BaseDirectory & "\Data\Connections.xml")
            MsgBox("Connections saved succesfully", MsgBoxStyle.Information)
            Me.DialogResult = Windows.Forms.DialogResult.OK
        End If
    End Sub
End Class