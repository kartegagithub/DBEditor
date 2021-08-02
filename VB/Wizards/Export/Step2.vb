Public Class Exporter2
    Private oTransferEntity As TransferEntity
    Private Thread As Threading.Thread
    Private Exported As Boolean = False
    Public Property TransferEntity() As TransferEntity
        Get
            Return oTransferEntity
        End Get
        Set(ByVal value As TransferEntity)
            oTransferEntity = value
        End Set
    End Property
    Private Sub ClearNullNode(ByVal Node As TreeNode)
        If Node.Nodes.Count = 0 AndAlso Node.BackColor = Color.LightBlue Then
            If Node.Parent Is Nothing Then
                Me.TreeView1.Nodes.Remove(Node)
            Else
                Node.Parent.Nodes.Remove(Node)
            End If
        End If
    End Sub
    Private Sub Step2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim Doc As System.Xml.XmlDocument = Me.TransferEntity.SourceDataSource.GetDataDocument
        Dim n As Integer
        Dim ConnectionsNode As Xml.XmlNode = Doc.SelectSingleNode("Connections")
        For n = 0 To ConnectionsNode.ChildNodes.Count - 1
            Me.CreateTreeNode(Nothing, ConnectionsNode.ChildNodes(n))
        Next
        Select Case Me.TransferEntity.TransferType
            Case TransferType.Excel, TransferType.XML
                Me.ServerPanel.Enabled = False
                Me.DirectPanel.Enabled = True
            Case Else
                If Me.TransferEntity.TransferType = TransferType.AnotherTable Then
                    Me.TreeView1.Enabled = False
                End If
                Me.ServerPanel.Enabled = True
                Me.DirectPanel.Enabled = False
        End Select
    End Sub
    Private Sub CreateTreeNode(ByVal ParentNode As TreeNode, ByVal XmlNode As System.Xml.XmlNode)
        If XmlNode.Name.IndexOf("Connection") > -1 Then
            If XmlNode.Attributes("Name") Is Nothing OrElse XmlNode.SelectSingleNode("Type").InnerText = Me.TransferEntity.TransferType Then
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
                Me.ClearNullNode(Node)
            End If
        End If
    End Sub
    Public Sub AfterProcess()
        If Me.Exported Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
        End If
        Me.Enabled = True
    End Sub
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Select Case Me.TransferEntity.TransferType
            Case TransferType.XML, TransferType.Excel
                If Me.sAlias.Text = "" OrElse Me.sLocation.Text = "" Then
                    MsgBox("You have to enter required informations", MsgBoxStyle.Information)
                    Exit Sub
                End If
            Case Else
                If (Me.TreeView1.SelectedNode Is Nothing OrElse Me.TreeView1.SelectedNode.BackColor <> Color.LightGreen) AndAlso Me.TransferEntity.TransferType <> TransferType.AnotherTable Then
                    MsgBox("You have to select a destination source", MsgBoxStyle.Information)
                    Exit Sub
                End If
                If Me.ExistingTableName.Text = "" Then
                    MsgBox("You have to enter destination table name", MsgBoxStyle.Information)
                    Exit Sub
                End If
        End Select
        Me.TransferEntity.Alias = Me.sAlias.Text
        Me.TransferEntity.File = Me.sLocation.Text
        If Me.ServerPanel.Enabled AndAlso Not Me.TreeView1.SelectedNode Is Nothing AndAlso Me.TreeView1.SelectedNode.BackColor = Color.LightGreen Then
            Me.TransferEntity.DestinationDataSource = New DataSource
            Me.TransferEntity.DestinationDataSource.Define(Me.TreeView1.SelectedNode.Tag)
            Me.TransferEntity.DestinationTableName = Me.ExistingTableName.Text
        ElseIf Me.TransferEntity.TransferType = TransferType.AnotherTable Then
            Me.TransferEntity.DestinationDataSource = Me.TransferEntity.SourceDataSource
            Me.TransferEntity.DestinationTableName = Me.ExistingTableName.Text
        End If
        Me.TransferEntity.ClearData = Me.CheckBox1.Checked
        Dim Message As New MessageForm
        Message.MessageToShow = "Export process is currently running."
        Message.Form = Me
        Me.Thread = New Threading.Thread(AddressOf Me.Export)
        Message.Thread = Me.Thread
        Thread.Start()
        Message.ShowDialog()
    End Sub
    Private Sub Export()
        Exported = Me.TransferEntity.Export()
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If Me.SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            Me.sLocation.Text = Me.SaveFileDialog1.FileName
        End If
    End Sub
End Class