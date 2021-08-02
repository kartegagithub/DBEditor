<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Step2
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
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Button2 = New System.Windows.Forms.Button
        Me.Button1 = New System.Windows.Forms.Button
        Me.Label3 = New System.Windows.Forms.Label
        Me.Server = New System.Windows.Forms.TextBox
        Me.Catalog = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.PWD = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.UID = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.Button3 = New System.Windows.Forms.Button
        Me.GeneralDB = New System.Windows.Forms.Panel
        Me.FolderRelated = New System.Windows.Forms.Panel
        Me.Button4 = New System.Windows.Forms.Button
        Me.DBLocation = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.MessageLabel = New System.Windows.Forms.Label
        Me.GeneralDB.SuspendLayout()
        Me.FolderRelated.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(62, 36)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(109, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Enter DB informations"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(162, Byte))
        Me.Label1.Location = New System.Drawing.Point(62, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(55, 17)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Step 2"
        '
        'Button2
        '
        Me.Button2.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.Button2.Location = New System.Drawing.Point(198, 196)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(49, 23)
        Me.Button2.TabIndex = 7
        Me.Button2.Text = "Cancel"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.Button1.Location = New System.Drawing.Point(253, 196)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(49, 23)
        Me.Button1.TabIndex = 5
        Me.Button1.Text = "Done"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(9, 6)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(38, 13)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "Server"
        '
        'Server
        '
        Me.Server.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Server.Location = New System.Drawing.Point(62, 3)
        Me.Server.Name = "Server"
        Me.Server.Size = New System.Drawing.Size(116, 20)
        Me.Server.TabIndex = 1
        '
        'Catalog
        '
        Me.Catalog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Catalog.Location = New System.Drawing.Point(62, 29)
        Me.Catalog.Name = "Catalog"
        Me.Catalog.Size = New System.Drawing.Size(116, 20)
        Me.Catalog.TabIndex = 2
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(9, 32)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(22, 13)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "DB"
        '
        'PWD
        '
        Me.PWD.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PWD.Location = New System.Drawing.Point(240, 29)
        Me.PWD.Name = "PWD"
        Me.PWD.Size = New System.Drawing.Size(102, 20)
        Me.PWD.TabIndex = 4
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(196, 32)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(32, 13)
        Me.Label5.TabIndex = 14
        Me.Label5.Text = "PSW"
        '
        'UID
        '
        Me.UID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.UID.Location = New System.Drawing.Point(240, 3)
        Me.UID.Name = "UID"
        Me.UID.Size = New System.Drawing.Size(102, 20)
        Me.UID.TabIndex = 3
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(196, 6)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(26, 13)
        Me.Label6.TabIndex = 12
        Me.Label6.Text = "UID"
        '
        'Button3
        '
        Me.Button3.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.Button3.Location = New System.Drawing.Point(308, 196)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(49, 23)
        Me.Button3.TabIndex = 15
        Me.Button3.Text = "Next"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'GeneralDB
        '
        Me.GeneralDB.Controls.Add(Me.Server)
        Me.GeneralDB.Controls.Add(Me.Label3)
        Me.GeneralDB.Controls.Add(Me.PWD)
        Me.GeneralDB.Controls.Add(Me.Label4)
        Me.GeneralDB.Controls.Add(Me.Label5)
        Me.GeneralDB.Controls.Add(Me.Catalog)
        Me.GeneralDB.Controls.Add(Me.UID)
        Me.GeneralDB.Controls.Add(Me.Label6)
        Me.GeneralDB.Location = New System.Drawing.Point(12, 73)
        Me.GeneralDB.Name = "GeneralDB"
        Me.GeneralDB.Size = New System.Drawing.Size(345, 62)
        Me.GeneralDB.TabIndex = 16
        '
        'FolderRelated
        '
        Me.FolderRelated.Controls.Add(Me.Button4)
        Me.FolderRelated.Controls.Add(Me.DBLocation)
        Me.FolderRelated.Controls.Add(Me.Label7)
        Me.FolderRelated.Location = New System.Drawing.Point(12, 87)
        Me.FolderRelated.Name = "FolderRelated"
        Me.FolderRelated.Size = New System.Drawing.Size(345, 33)
        Me.FolderRelated.TabIndex = 17
        '
        'Button4
        '
        Me.Button4.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.Button4.Location = New System.Drawing.Point(282, 3)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(57, 23)
        Me.Button4.TabIndex = 16
        Me.Button4.Text = "Browse"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'DBLocation
        '
        Me.DBLocation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.DBLocation.Location = New System.Drawing.Point(62, 5)
        Me.DBLocation.Name = "DBLocation"
        Me.DBLocation.Size = New System.Drawing.Size(214, 20)
        Me.DBLocation.TabIndex = 9
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(9, 8)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(41, 13)
        Me.Label7.TabIndex = 10
        Me.Label7.Text = "Source"
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.DBEditor.My.Resources.Resources.arrow
        Me.PictureBox1.Location = New System.Drawing.Point(12, 12)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(44, 45)
        Me.PictureBox1.TabIndex = 1
        Me.PictureBox1.TabStop = False
        '
        'MessageLabel
        '
        Me.MessageLabel.Location = New System.Drawing.Point(21, 150)
        Me.MessageLabel.Name = "MessageLabel"
        Me.MessageLabel.Size = New System.Drawing.Size(324, 30)
        Me.MessageLabel.TabIndex = 18
        Me.MessageLabel.Text = "Label8"
        '
        'Step2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(373, 231)
        Me.ControlBox = False
        Me.Controls.Add(Me.FolderRelated)
        Me.Controls.Add(Me.MessageLabel)
        Me.Controls.Add(Me.GeneralDB)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.PictureBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "Step2"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Connection wizard"
        Me.GeneralDB.ResumeLayout(False)
        Me.GeneralDB.PerformLayout()
        Me.FolderRelated.ResumeLayout(False)
        Me.FolderRelated.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Server As System.Windows.Forms.TextBox
    Friend WithEvents Catalog As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents PWD As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents UID As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents GeneralDB As System.Windows.Forms.Panel
    Friend WithEvents FolderRelated As System.Windows.Forms.Panel
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents DBLocation As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents FolderBrowserDialog1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents MessageLabel As System.Windows.Forms.Label
End Class
