Public Class IndexCollection : Inherits CollectionBase
    Private oTable As Table
    Default Public Shadows ReadOnly Property Item(ByVal Index As Integer) As Index
        Get
            Return Me.List(Index)
        End Get
    End Property
    Public ReadOnly Property Source() As DataSource
        Get
            Return Me.oTable.Collection.Source
        End Get
    End Property
    Public ReadOnly Property Table() As Table
        Get
            Return Me.oTable
        End Get
    End Property
    Public Sub Load()
        Me.Clear()
        Dim Indexes As DataTable = Nothing
        Select Case Me.Source.Type
            Case ConnectionType.Oracle
                Indexes = Me.oTable.Collection.Source.GetDataTable("SELECT ALL_INDEXES.* FROM ALL_INDEXES WHERE TABLE_NAME='" & Me.oTable.Name & "' AND OWNER='" & Me.Source.UserName & "' ORDER BY INDEX_NAME")
            Case ConnectionType.SQLServer
                Indexes = Me.oTable.Collection.Source.GetDataTable("SELECT sys.indexes.* from sys.indexes, sys.tables where sys.tables.object_id=sys.indexes.object_id AND sys.tables.name='" & Me.oTable.Name & "' ORDER BY sys.indexes.name")
            Case ConnectionType.Access
                Dim Tables() As String = {Nothing, Nothing, Me.Table.Name, Nothing}
                Indexes = DirectCast(Me.Source.Connection, OleDb.OleDbConnection).GetSchema("Indexes", Tables)
            Case ConnectionType.FoxPro
                Dim Tables() As String = {Nothing, Nothing, Me.Table.Name, Nothing}
                Indexes = DirectCast(Me.Source.Connection, Odbc.OdbcConnection).GetSchema("Indexes", Tables)
        End Select
        If Not Indexes Is Nothing Then
            Dim row As DataRow
            For Each row In Indexes.Rows
                Select Case Me.Source.Type
                    Case ConnectionType.Oracle
                        Me.List.Add(New Index(Me, row("INDEX_NAME")))
                    Case ConnectionType.SQLServer
                        If Not row("name") Is DBNull.Value Then
                            Me.List.Add(New Index(Me, row("name")))
                        End If
                End Select
            Next
        End If
    End Sub
    Public Sub New(ByVal Table As Table)
        oTable = Table
    End Sub
End Class
