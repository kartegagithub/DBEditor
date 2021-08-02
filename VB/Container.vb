Public Class Container
    Dim FirstForm As New FirstForm
    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub
    Private Sub ConnectToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ConnectToolStripMenuItem.Click
        Dim Datasource As New DataSource
        Dim Step1 As New Step1(Datasource)
        If Step1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Dim Step2 As New Step2(Datasource)
            If Step2.ShowDialog() = Windows.Forms.DialogResult.OK Then
                Dim Step3 As New Step3(Datasource)
                Step3.ShowDialog()
            End If
        End If
    End Sub
    Private Sub OnlineHelpToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OnlineHelpToolStripMenuItem.Click
        Utility.AskTheExceptionGoogle()
    End Sub
    Private Sub AboutDBEditorToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutDBEditorToolStripMenuItem.Click
        Dim About As New About
        About.ShowDialog()
    End Sub
    Private Sub CheckUpdatesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckUpdatesToolStripMenuItem.Click
        MsgBox("You are up to date. No updates found", MsgBoxStyle.Information)
    End Sub
    Private Sub Container_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.Control AndAlso e.KeyCode = Keys.F Then
            Utility.AskTheExceptionGoogle()
        End If
    End Sub
    Private Sub Container_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.LoadConnections()
        Utility.Container = Me
        Control.CheckForIllegalCrossThreadCalls = False
        Me.Panel1.Visible = False
        FirstForm.MdiParent = Me
        FirstForm.WindowState = FormWindowState.Maximized
        FirstForm.Show()
    End Sub
    Public Sub PrepareConnection(ByVal DataSource As DataSource)
        Me.Panel1.Visible = True
        Dim EditorForm As New EditorForm(DataSource)
        EditorForm.MdiParent = Me
        EditorForm.WindowState = FormWindowState.Maximized
        EditorForm.Show()
        Dim Button As New Button
        Me.Panel1.Controls.Add(Button)
        Button.FlatStyle = FlatStyle.Popup
        Button.Text = DataSource.Location & " / " & DataSource.Name & " (*)"
        Button.Tag = EditorForm
        Button.Dock = DockStyle.Left
        Button.Width = 250
        Button.TextAlign = ContentAlignment.MiddleLeft
        Me.Button_Click(Button, New EventArgs)
        AddHandler Button.Click, AddressOf Button_Click
        If Not FirstForm Is Nothing Then
            FirstForm.Close()
            FirstForm.Dispose()
            FirstForm = Nothing
        End If
        If DataSource.Type = ConnectionType.XML Then
            EditorForm.ShowData()
        End If
    End Sub
    Private Sub Button_Click(ByVal sender As Object, ByVal e As EventArgs)
        Me.ActivateForm(sender)
    End Sub
    Public Sub ActivateForm(ByVal Sender As Object)
        Dim n As Integer
        For n = 0 To Me.Panel1.Controls.Count - 1
            Me.Panel1.Controls(n).BackColor = Color.DarkGray
            Me.Panel1.Controls(n).Enabled = True
            Me.Panel1.Controls(n).Text = Me.Panel1.Controls(n).Tag.DataSource.Location & " / " & Me.Panel1.Controls(n).Tag.DataSource.Name
        Next
        If Sender.GetType.Name = "Button" Then
            Dim Button As Button = Sender
            Button.BackColor = Color.White
            Dim Editor As EditorForm = Sender.Tag
            Button.Text = Editor.DataSource.Location & " / " & Editor.DataSource.Name & " (*)"
            Button.Enabled = False
            Editor.BringToFront()
        Else
            For n = 0 To Me.Panel1.Controls.Count - 1
                If DirectCast(Me.Panel1.Controls(n).Tag, EditorForm).InstanceID = Sender.InstanceID Then
                    Dim Button As Button = Me.Panel1.Controls(n)
                    Button.BackColor = Color.White
                    Dim Editor As EditorForm = Sender
                    Button.Text = Editor.DataSource.Location & " / " & Editor.DataSource.Name & " (*)"
                    Button.Enabled = False
                    Exit For
                End If
            Next
        End If
    End Sub
    Private Sub LoadConnections()
        Dim DataSource As New DataSource
        Dim Doc As System.Xml.XmlDocument = DataSource.GetDataDocument
        Dim n As Integer
        Dim Connections As Xml.XmlNode = Doc.SelectSingleNode("Connections")
        For n = 0 To Connections.ChildNodes.Count - 1
            Me.CreateMenuItem(Nothing, Connections.ChildNodes(n))
        Next
    End Sub
    Private Sub CreateMenuItem(ByVal ParentNode As ToolStripMenuItem, ByVal XmlNode As System.Xml.XmlNode)
        If XmlNode.Name.IndexOf("Connection") > -1 Then
            Dim Node As ToolStripMenuItem
            If ParentNode Is Nothing Then
                Node = Me.ConnectionsToolStripMenuItem.DropDownItems.Add("")
            Else
                Node = ParentNode.DropDownItems.Add("")
            End If
            Node.Tag = XmlNode
            If XmlNode.Name = "Connection" Then AddHandler Node.Click, AddressOf Connection_Selected
            If Not XmlNode.Attributes("Name") Is Nothing Then
                Node.Text = XmlNode.Attributes("Name").Value
            Else
                Node.Text = XmlNode.Attributes("Alias").Value
            End If
            Dim n As Integer
            For n = 0 To XmlNode.ChildNodes.Count - 1
                Me.CreateMenuItem(Node, XmlNode.ChildNodes(n))
            Next
        End If
    End Sub
    Private Sub Connection_Selected(ByVal sender As Object, ByVal e As EventArgs)
        Dim DataSource As New DataSource
        System.Windows.Forms.Cursor.Current = Cursors.WaitCursor
        If DataSource.Connect(sender.Tag) Is Nothing Then
            System.Windows.Forms.Cursor.Current = Cursors.Default
            MsgBox("Could not connect to data source, source is down or required components are not installed properly.", MsgBoxStyle.Critical)
        Else
            Me.PrepareConnection(DataSource)
        End If
        System.Windows.Forms.Cursor.Current = Cursors.Default
    End Sub
    Private Sub EditConnectionsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EditConnectionsToolStripMenuItem.Click
        Dim EditConnectionForm As New EditConnections
        EditConnectionForm.ShowDialog()
    End Sub
    Private Sub ShowHideConnectionsPanelToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowHideConnectionsPanelToolStripMenuItem.Click
        Me.Panel1.Visible = Not Me.Panel1.Visible
    End Sub
End Class
