Public Class Step3
    Private DataSource As DataSource
    Private Thread As Threading.Thread
    Private Connected As Boolean = False
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
        Me.SaveConnection()
        Dim Message As New MessageForm
        Message.MessageToShow = "Trying to connect selected datasource"
        Message.Form = Me
        Me.Thread = New Threading.Thread(AddressOf Connect)
        Message.Thread = Me.Thread
        Thread.Start()
        Message.ShowDialog()
    End Sub
    Public Sub AfterProcess()
        If Not Me.Connected Then
            MsgBox("Could not connect to data source, source is down or required components are not installed properly.", MsgBoxStyle.Critical)
        Else
            Utility.Container.PrepareConnection(Me.DataSource)
        End If
        Me.Enabled = True
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub
    Private Sub Connect()
        Me.Connected = Not Me.DataSource.Connect() Is Nothing
    End Sub
    Private Sub SaveConnection()
        Me.DataSource.Name = Me.ConName.Text
        If Me.TreeView1.SelectedNode Is Nothing Then
            Me.DataSource.SaveConnection("")
        Else
            Dim SelectedNode As TreeNode = Me.TreeView1.SelectedNode
            Dim Path As String = "Connections[@Alias='" & SelectedNode.Text & "']"
            Do Until SelectedNode.Parent Is Nothing
                SelectedNode = SelectedNode.Parent
                If Path <> "" Then Path = "\" & Path
                Path = "Connections[@Alias='" & SelectedNode.Text & "']" & Path
            Loop
            Me.DataSource.SaveConnection(Path)
        End If
    End Sub
    Private Sub Step3_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim Doc As System.Xml.XmlDocument = Me.DataSource.GetDataDocument
        Dim n As Integer
        Dim ConnectionsNode As Xml.XmlNode = Doc.SelectSingleNode("Connections")
        For n = 0 To ConnectionsNode.ChildNodes.Count - 1
            Me.CreateTreeNode(Nothing, ConnectionsNode.ChildNodes(n))
        Next
    End Sub
    Private Sub CreateTreeNode(ByVal ParentNode As TreeNode, ByVal XmlNode As System.Xml.XmlNode)
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
    Private Sub ConName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ConName.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
        End If
    End Sub
End Class