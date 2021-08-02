<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NewColumn
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
        Me.TableName = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.ColumnName = New System.Windows.Forms.TextBox
        Me.DataType = New System.Windows.Forms.ComboBox
        Me.Value = New System.Windows.Forms.TextBox
        Me.Button1 = New System.Windows.Forms.Button
        Me.Button2 = New System.Windows.Forms.Button
        Me.Button3 = New System.Windows.Forms.Button
        Me.Message = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'TableName
        '
        Me.TableName.AutoSize = True
        Me.TableName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(162, Byte))
        Me.TableName.Location = New System.Drawing.Point(113, 9)
        Me.TableName.Name = "TableName"
        Me.TableName.Size = New System.Drawing.Size(71, 13)
        Me.TableName.TabIndex = 0
        Me.TableName.Text = "TableName"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(162, Byte))
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(107, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "New column for : "
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 37)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(71, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Column name"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(141, 36)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(53, 13)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "Data type"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(249, 37)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(34, 13)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "Value"
        '
        'ColumnName
        '
        Me.ColumnName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ColumnName.Location = New System.Drawing.Point(15, 53)
        Me.ColumnName.Name = "ColumnName"
        Me.ColumnName.Size = New System.Drawing.Size(123, 20)
        Me.ColumnName.TabIndex = 1
        '
        'DataType
        '
        Me.DataType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.DataType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.DataType.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.DataType.Items.AddRange(New Object() {"String", "Char", "Memo", "Integer", "Long", "Decimal", "Byte", "Bit", "Date"})
        Me.DataType.Location = New System.Drawing.Point(144, 52)
        Me.DataType.MaxDropDownItems = 10
        Me.DataType.Name = "DataType"
        Me.DataType.Size = New System.Drawing.Size(102, 21)
        Me.DataType.TabIndex = 2
        '
        'Value
        '
        Me.Value.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Value.Location = New System.Drawing.Point(252, 53)
        Me.Value.Name = "Value"
        Me.Value.Size = New System.Drawing.Size(74, 20)
        Me.Value.TabIndex = 3
        Me.Value.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Button1
        '
        Me.Button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.Button1.Location = New System.Drawing.Point(278, 187)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(49, 23)
        Me.Button1.TabIndex = 5
        Me.Button1.Text = "Create"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.Button2.Location = New System.Drawing.Point(168, 187)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(49, 23)
        Me.Button2.TabIndex = 4
        Me.Button2.Text = "Cancel"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.Button3.Location = New System.Drawing.Point(223, 187)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(49, 23)
        Me.Button3.TabIndex = 10
        Me.Button3.Text = "SQL"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Message
        '
        Me.Message.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.Message.Location = New System.Drawing.Point(15, 90)
        Me.Message.Multiline = True
        Me.Message.Name = "Message"
        Me.Message.ReadOnly = True
        Me.Message.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.Message.Size = New System.Drawing.Size(311, 82)
        Me.Message.TabIndex = 11
        Me.Message.Text = "Column name : The name of the created column." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Data type      : Type of the colum" & _
            "n" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Value             : Value of the column data type. (ex:35,2 for decimal, 16 f" & _
            "or integer, 255 for string)"
        '
        'NewColumn
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(338, 222)
        Me.Controls.Add(Me.Message)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Value)
        Me.Controls.Add(Me.DataType)
        Me.Controls.Add(Me.ColumnName)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.TableName)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "NewColumn"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "New column wizard"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TableName As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents ColumnName As System.Windows.Forms.TextBox
    Friend WithEvents DataType As System.Windows.Forms.ComboBox
    Friend WithEvents Value As System.Windows.Forms.TextBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Message As System.Windows.Forms.TextBox
End Class
