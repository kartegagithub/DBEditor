<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Exporter2
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.DirectPanel = New System.Windows.Forms.Panel
        Me.Button1 = New System.Windows.Forms.Button
        Me.sLocation = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.sAlias = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.ServerPanel = New System.Windows.Forms.Panel
        Me.Label5 = New System.Windows.Forms.Label
        Me.ExistingTableName = New System.Windows.Forms.TextBox
        Me.TreeView1 = New System.Windows.Forms.TreeView
        Me.Label4 = New System.Windows.Forms.Label
        Me.Button2 = New System.Windows.Forms.Button
        Me.Button3 = New System.Windows.Forms.Button
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog
        Me.CheckBox1 = New System.Windows.Forms.CheckBox
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.DirectPanel.SuspendLayout()
        Me.ServerPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.DBEditor.My.Resources.Resources.arrow
        Me.PictureBox1.Location = New System.Drawing.Point(12, 12)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(48, 48)
        Me.PictureBox1.TabIndex = 1
        Me.PictureBox1.TabStop = False
        Me.PictureBox1.WaitOnLoad = True
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(162, Byte))
        Me.Label2.Location = New System.Drawing.Point(66, 12)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(252, 19)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Step 2 - Destination properties"
        '
        'DirectPanel
        '
        Me.DirectPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.DirectPanel.Controls.Add(Me.Button1)
        Me.DirectPanel.Controls.Add(Me.sLocation)
        Me.DirectPanel.Controls.Add(Me.Label3)
        Me.DirectPanel.Controls.Add(Me.sAlias)
        Me.DirectPanel.Controls.Add(Me.Label1)
        Me.DirectPanel.Location = New System.Drawing.Point(12, 82)
        Me.DirectPanel.Name = "DirectPanel"
        Me.DirectPanel.Size = New System.Drawing.Size(387, 97)
        Me.DirectPanel.TabIndex = 4
        '
        'Button1
        '
        Me.Button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.Button1.Location = New System.Drawing.Point(313, 53)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(62, 20)
        Me.Button1.TabIndex = 4
        Me.Button1.Text = "Browse"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'sLocation
        '
        Me.sLocation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.sLocation.Location = New System.Drawing.Point(6, 53)
        Me.sLocation.Name = "sLocation"
        Me.sLocation.Size = New System.Drawing.Size(308, 20)
        Me.sLocation.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(3, 37)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(63, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "File location"
        '
        'sAlias
        '
        Me.sAlias.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.sAlias.Location = New System.Drawing.Point(167, 8)
        Me.sAlias.Name = "sAlias"
        Me.sAlias.Size = New System.Drawing.Size(208, 20)
        Me.sAlias.TabIndex = 1
        Me.sAlias.Text = "DataTable1"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 10)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(138, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Type a name without space"
        '
        'ServerPanel
        '
        Me.ServerPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ServerPanel.Controls.Add(Me.CheckBox1)
        Me.ServerPanel.Controls.Add(Me.Label5)
        Me.ServerPanel.Controls.Add(Me.ExistingTableName)
        Me.ServerPanel.Controls.Add(Me.TreeView1)
        Me.ServerPanel.Controls.Add(Me.Label4)
        Me.ServerPanel.Location = New System.Drawing.Point(12, 196)
        Me.ServerPanel.Name = "ServerPanel"
        Me.ServerPanel.Size = New System.Drawing.Size(387, 198)
        Me.ServerPanel.TabIndex = 5
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(6, 144)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(63, 13)
        Me.Label5.TabIndex = 18
        Me.Label5.Text = "Table name"
        '
        'ExistingTableName
        '
        Me.ExistingTableName.Location = New System.Drawing.Point(75, 141)
        Me.ExistingTableName.Name = "ExistingTableName"
        Me.ExistingTableName.Size = New System.Drawing.Size(300, 20)
        Me.ExistingTableName.TabIndex = 16
        '
        'TreeView1
        '
        Me.TreeView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TreeView1.Location = New System.Drawing.Point(6, 30)
        Me.TreeView1.Name = "TreeView1"
        Me.TreeView1.Size = New System.Drawing.Size(369, 105)
        Me.TreeView1.TabIndex = 14
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(3, 14)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(141, 13)
        Me.Label4.TabIndex = 1
        Me.Label4.Text = "Select destination data base"
        '
        'Button2
        '
        Me.Button2.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.Button2.Location = New System.Drawing.Point(351, 400)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(48, 23)
        Me.Button2.TabIndex = 6
        Me.Button2.Text = "Export"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.Button3.Location = New System.Drawing.Point(297, 400)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(48, 23)
        Me.Button3.TabIndex = 7
        Me.Button3.Text = "Cancel"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Location = New System.Drawing.Point(75, 167)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(139, 17)
        Me.CheckBox1.TabIndex = 19
        Me.CheckBox1.Text = "Clear data before export"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'Exporter2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(410, 435)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.ServerPanel)
        Me.Controls.Add(Me.DirectPanel)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.PictureBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "Exporter2"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Exporter"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.DirectPanel.ResumeLayout(False)
        Me.DirectPanel.PerformLayout()
        Me.ServerPanel.ResumeLayout(False)
        Me.ServerPanel.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents DirectPanel As System.Windows.Forms.Panel
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents sLocation As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents sAlias As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ServerPanel As System.Windows.Forms.Panel
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents TreeView1 As System.Windows.Forms.TreeView
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents ExistingTableName As System.Windows.Forms.TextBox
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
End Class
