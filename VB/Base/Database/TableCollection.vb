Public Class TableCollection : Inherits CollectionBase
    Private oSource As DataSource
    Public ReadOnly Property Source() As DataSource
        Get
            Return Me.oSource
        End Get
    End Property
    Default Public Shadows ReadOnly Property Item(ByVal Index As Integer) As Table
        Get
            Return Me.List.Item(Index)
        End Get
    End Property
    Default Public Shadows ReadOnly Property Item(ByVal Name As String) As Table
        Get
            Dim n As Integer
            For n = 0 To Me.Count - 1
                If Me(n).Name = Name Then
                    Return Me(n)
                End If
            Next
            Return Nothing
        End Get
    End Property
    Public Sub Load()
        Me.Clear()
        Try
            Dim Tables As DataTable = Nothing
            Select Case Me.Source.Type
                Case ConnectionType.Oracle
                    Tables = Me.Source.GetDataTable("SELECT TABLE_NAME from ALL_TABLES WHERE OWNER='" & Me.Source.UserName & "' ORDER BY TABLE_NAME ASC")
                Case ConnectionType.SQLServer
                    Try
                        Tables = Me.Source.GetDataTable("SELECT SCHEMA_NAME(schema_id) as SchemaName, name FROM sys.tables ORDER BY SchemaName, name")
                    Catch ex As Exception
                        Tables = Me.Source.GetDataTable("SELECT SCHEMA_NAME(schema_id) as SchemaName, name FROM sys.objects where TYPE='U' ORDER BY NAME")
                    End Try
                Case ConnectionType.Access
                    Tables = DirectCast(Me.Source.Connection, OleDb.OleDbConnection).GetSchema("Tables")
                Case ConnectionType.FoxPro
                    Tables = DirectCast(Me.Source.Connection, Odbc.OdbcConnection).GetSchema("Tables")
                Case ConnectionType.Excel
                    Tables = DirectCast(Me.Source.Connection, OleDb.OleDbConnection).GetSchema("Tables")
            End Select
            Try
                Dim Row As DataRow
                For Each Row In Tables.Rows
                    Select Case Me.Source.Type
                        Case ConnectionType.Oracle, ConnectionType.Access
                            Me.List.Add(New Table(Me, Row("TABLE_NAME")))
                        Case ConnectionType.FoxPro
                            Me.List.Add(New Table(Me, Me.Source.Location & "\" & Row("TABLE_NAME")))
                        Case ConnectionType.SQLServer
                            Me.List.Add(New Table(Me, Row("name"), Row("SchemaName")))
                        Case ConnectionType.Excel
                            Me.List.Add(New Table(Me, "[" & Row("TABLE_NAME") & "]"))
                    End Select
                Next
            Catch ex As Exception

            End Try
        Catch ex As Exception

        End Try
    End Sub
    Public Sub New(ByVal Source As DataSource)
        oSource = Source
    End Sub
End Class
