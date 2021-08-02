Public Class ColumnCollection : Inherits CollectionBase
    Private oTable As Table
    Default Public Shadows ReadOnly Property Item(ByVal Index As Integer) As Column
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
        Dim Colums As DataTable = Nothing
        Select Case Me.Source.Type
            Case ConnectionType.Oracle
                Colums = Me.oTable.Collection.Source.GetDataTable("SELECT * FROM ALL_TAB_COLUMNS where TABLE_NAME='" & Me.oTable.Name & "' AND OWNER='" & Me.Source.UserName & "' ORDER BY COLUMN_NAME")
            Case ConnectionType.SQLServer
                Colums = Me.oTable.Collection.Source.GetDataTable("SELECT * FROM INFORMATION_SCHEMA.COLUMNS where table_name = '" & Me.oTable.Name & "' AND TABLE_CATALOG='" & Me.oTable.DataSource.DataBase & "' ORDER BY column_name")
            Case ConnectionType.Access
                Dim Tables() As String = {Nothing, Nothing, Me.Table.Name, Nothing}
                Colums = DirectCast(Me.Source.Connection, OleDb.OleDbConnection).GetSchema("Columns", Tables)
            Case ConnectionType.FoxPro
                Dim Tables() As String = {Nothing, Nothing, Me.Table.Name, Nothing}
                Colums = DirectCast(Me.Source.Connection, Odbc.OdbcConnection).GetSchema("Columns", Tables)
            Case ConnectionType.Excel
                Dim Tables() As String = {Nothing, Nothing, Me.Table.Name.Replace("[", "").Replace("]", ""), Nothing}
                Colums = DirectCast(Me.Source.Connection, OleDb.OleDbConnection).GetSchema("Columns", Tables)
        End Select
        Dim row As DataRow
        For Each row In Colums.Rows
            Me.List.Add(New Column(Me, row))
        Next
    End Sub
    Public Sub New(ByVal Table As Table)
        oTable = Table
    End Sub
End Class
