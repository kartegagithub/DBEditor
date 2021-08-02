Public Class Exporter
    Private oTransferEntity As TransferEntity
    Public Property TransferEntity() As TransferEntity
        Get
            Return oTransferEntity
        End Get
        Set(ByVal value As TransferEntity)
            oTransferEntity = value
        End Set
    End Property
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Select Case Me.List.Text
            Case "Excel"
                Me.TransferEntity.TransferType = DBEditor.TransferType.Excel
            Case "XML"
                Me.TransferEntity.TransferType = DBEditor.TransferType.XML
            Case "Access"
                Me.TransferEntity.TransferType = DBEditor.TransferType.Access
            Case "FoxPro"
                Me.TransferEntity.TransferType = DBEditor.TransferType.FoxPro
            Case "SQL Server"
                Me.TransferEntity.TransferType = DBEditor.TransferType.SQLServer
            Case "Oracle"
                Me.TransferEntity.TransferType = DBEditor.TransferType.Oracle
            Case "Another table at the current data source"
                Me.TransferEntity.TransferType = DBEditor.TransferType.AnotherTable
        End Select
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub
End Class