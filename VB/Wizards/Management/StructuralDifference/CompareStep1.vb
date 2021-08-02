Public Class CompareStep1
    Private oDataSource As DataSource
    Public Property DataSource() As DataSource
        Get
            Return Me.oDataSource
        End Get
        Set(ByVal value As DataSource)
            Me.oDataSource = value
        End Set
    End Property
    Public DestinationDataSource As DataSource
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub
    Private Sub CompareStep1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim Doc As System.Xml.XmlDocument = Me.DataSource.GetDataDocument
        Dim n As Integer
        Dim ConnectionsNode As Xml.XmlNode = Doc.SelectSingleNode("Connections")
        For n = 0 To ConnectionsNode.ChildNodes.Count - 1
            Me.CreateTreeNode(Me.TreeView2, Nothing, ConnectionsNode.ChildNodes(n))
        Next
    End Sub
    Private Sub CreateTreeNode(ByVal Treeview As TreeView, ByVal ParentNode As TreeNode, ByVal XmlNode As System.Xml.XmlNode)
        If Not XmlNode.Attributes("Name") Is Nothing OrElse Not XmlNode.Attributes("Alias") Is Nothing Then
            Dim Node As TreeNode
            If ParentNode Is Nothing Then
                Node = Treeview.Nodes.Add("")
            Else
                Node = ParentNode.Nodes.Add("")
            End If
            Node.Tag = XmlNode
            If Not XmlNode.Attributes("Name") Is Nothing Then
                Node.Text = XmlNode.Attributes("Name").Value
                Node.Tag = XmlNode
                Node.BackColor = Color.LightGreen
            Else
                Node.Text = XmlNode.Attributes("Alias").Value
                Node.Tag = XmlNode
                Node.BackColor = Color.LightBlue
            End If
            Dim n As Integer
            For n = 0 To XmlNode.ChildNodes.Count - 1
                Me.CreateTreeNode(Treeview, Node, XmlNode.ChildNodes(n))
            Next
        End If
    End Sub
    Private Sub ClearNullNode(ByVal Node As TreeNode)
        If Node.Nodes.Count = 0 AndAlso Node.BackColor = Color.LightBlue Then
            If Node.Parent Is Nothing Then
                Me.TreeView2.Nodes.Remove(Node)
            Else
                Node.Parent.Nodes.Remove(Node)
            End If
        End If
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If Not Me.TreeView2.SelectedNode Is Nothing AndAlso Me.TreeView2.SelectedNode.BackColor = Color.LightGreen Then
            Me.DestinationDataSource = New DataSource
            Me.DestinationDataSource.Define(Me.TreeView2.SelectedNode.Tag)
            Me.DialogResult = Windows.Forms.DialogResult.OK
        Else
            MsgBox("You must specify a data source", MsgBoxStyle.Critical)
            Me.TreeView2.Focus()
            Exit Sub
        End If
    End Sub
End Class