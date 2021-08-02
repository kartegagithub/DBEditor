Imports System.Data.OracleClient
Imports System.Xml
Public Class DataSource
    Private eType As ConnectionType
    Private oConnection As Object
    Private oTables As TableCollection
    Private oSequences As SequenceCollection
    Private oCommand As Object
    Private oAdapter, CommandBuilder As Object
    Private sLocation, sName, sDataBase, sUserName, sPassword As String
    Public ReferencedXMLNode As Xml.XmlNode
    Private bConnected As Boolean = False
    Private SaveFileDialog As New SaveFileDialog
    Private OpenFileDialog As New OpenFileDialog
    Public ReadOnly Property Connected() As Boolean
        Get
            Return Me.bConnected
        End Get
    End Property
    Public Property Type() As ConnectionType
        Get
            Return eType
        End Get
        Set(ByVal value As ConnectionType)
            eType = value
        End Set
    End Property
    Public Property Name() As String
        Get
            Return Me.sName
        End Get
        Set(ByVal value As String)
            Me.sName = value
        End Set
    End Property
    Public Property Location() As String
        Get
            Return Me.sLocation
        End Get
        Set(ByVal value As String)
            Me.sLocation = value
        End Set
    End Property
    Public Property DataBase() As String 'Listener
        Get
            Return Me.sDataBase
        End Get
        Set(ByVal value As String)
            Me.sDataBase = value
        End Set
    End Property
    Public Property UserName() As String
        Get
            Return Me.sUserName
        End Get
        Set(ByVal value As String)
            Me.sUserName = value
        End Set
    End Property
    Public Property Password() As String
        Get
            Return Me.sPassword
        End Get
        Set(ByVal value As String)
            Me.sPassword = value
        End Set
    End Property
    Public ReadOnly Property Tables() As TableCollection
        Get
            Me.LoadTables()
            Return Me.oTables
        End Get
    End Property
    Public ReadOnly Property Sequences() As SequenceCollection
        Get
            Me.LoadSequences()
            Return Me.oSequences
        End Get
    End Property
    Public ReadOnly Property Connection() As Object
        Get
            Return Me.oConnection
        End Get
    End Property
    Public Sub SaveQuery(ByVal Query As String)
        If SaveFileDialog.ShowDialog = DialogResult.OK Then
            If SaveFileDialog.FileName.IndexOf(".txt") = -1 Then
                IO.File.AppendAllText(SaveFileDialog.FileName & ".txt", vbCrLf & "************************" & vbCrLf & Query)
            Else
                IO.File.AppendAllText(SaveFileDialog.FileName, vbCrLf & "************************" & vbCrLf & Query)
            End If
            MsgBox("Saved", MsgBoxStyle.Information)
        End If
    End Sub
    Public Function OpenQuery() As String
        If OpenFileDialog.ShowDialog = DialogResult.OK Then
            Return IO.File.ReadAllText(OpenFileDialog.FileName)
        End If
        Return ""
    End Function
    Public Sub ResetTables()
        Me.oTables = Nothing
    End Sub
    Public Sub LoadTables()
        If Me.oTables Is Nothing Then
            Me.oTables = New TableCollection(Me)
            Me.oTables.Load()
        End If
    End Sub
    Public Sub LoadSequences()
        If Me.oSequences Is Nothing Then
            Me.oSequences = New SequenceCollection(Me)
            Me.oSequences.Load()
        End If
    End Sub
    Public Sub Analyze()
        If Me.Type = ConnectionType.Oracle Then
            Dim n, i As Integer
            For n = 0 To Me.Tables.Count - 1
                Try
                    Me.ExecuteQuery("Analyze Table " & Me.GetOpenBracket & Me.Tables(n).Name & Me.GetCloseBracket & " Estimate Statistics")
                    For i = 0 To Me.Tables(n).Indexes.Count - 1
                        Try
                            Me.ExecuteQuery("ANALYZE INDEX " & Me.GetOpenBracket & Me.Tables(n).Indexes(i).Name & Me.GetCloseBracket & " Estimate STATISTICS")
                        Catch ex As Exception

                        End Try
                    Next
                Catch ex As Exception

                End Try
            Next
        End If
    End Sub
    Public Sub NewTable(ByVal TableName As String, ByVal Column As DataColumn)
        Dim SQL As String = ""
        Select Case Me.Type
            Case ConnectionType.Access, ConnectionType.Excel
                SQL = "CREATE TABLE " & Me.GetOpenBracket & TableName & Me.GetCloseBracket & " (" & Me.GetOpenBracket & Column.ColumnName & Me.GetCloseBracket & " " & Me.GetSQLDataType(Column.DataType) & " NOT NULL)"
            Case ConnectionType.SQLServer
                SQL = "CREATE TABLE " & Me.GetOpenBracket & TableName & Me.GetCloseBracket & " (" & Me.GetOpenBracket & Column.ColumnName & Me.GetCloseBracket & " " & Me.GetSQLDataType(Column.DataType) & "  Not Null Primary Key)"
            Case ConnectionType.Oracle
                SQL = "CREATE TABLE " & Me.GetOpenBracket & TableName & Me.GetCloseBracket & " (" & Me.GetOpenBracket & Column.ColumnName & Me.GetCloseBracket & " " & Me.GetSQLDataType(Column.DataType) & ", PRIMARY KEY (" & Column.ColumnName & ") VALIDATE )"
            Case ConnectionType.FoxPro
                SQL = "CREATE TABLE " & Me.GetOpenBracket & Me.Location & "\" & TableName & Me.GetCloseBracket & " (" & Me.GetOpenBracket & Column.ColumnName & Me.GetCloseBracket & " " & Me.GetSQLDataType(Column.DataType) & " NOT NULL)"
            Case Else
                SQL = ""
        End Select
        If SQL <> "" Then
            Me.ExecuteQuery(SQL)
        End If
    End Sub
    Public Function GetFormattedColumnName(ByVal Name As String, Optional ByVal UseBrackets As Boolean = True) As String
        Select Case Me.Type
            Case ConnectionType.FoxPro
                If UseBrackets Then
                    Return Me.GetOpenBracket & LCase(Left(Name, 10)).Replace("ý", "i") & Me.GetCloseBracket
                Else
                    Return LCase(Left(Name, 10)).Replace("ý", "i")
                End If
            Case ConnectionType.Oracle
                If UseBrackets Then
                    Return Me.GetOpenBracket & Left(Name, 30) & Me.GetCloseBracket
                Else
                    Return Left(Name, 30)
                End If
        End Select
        If UseBrackets Then
            Return Me.GetOpenBracket & Name & Me.GetCloseBracket
        End If
        Return Name
    End Function
    Public Function ExecuteQuery(ByVal Command As Object, ByVal SQL As String) As Object
        Me.Connect()
        Command.CommandText = SQL
        Return Command.ExecuteNonQuery()
    End Function
    Public Function ExecuteQuery(ByVal SQL As String) As Object
        Me.Connect()
        Me.oCommand.CommandText = SQL
        Return Me.oCommand.ExecuteNonQuery()
    End Function
    Public Function GetDataTable(ByVal SQL As String) As DataTable
        Me.Connect()
        Dim Table As New DataTable
        oCommand.CommandText = SQL
        oAdapter.Fill(Table)
        Return Table
    End Function
    Public Function GetDataTable(ByVal Command As Object, ByVal Adapter As Object, ByVal SQL As String) As DataTable
        Me.Connect()
        Dim Table As New DataTable
        Command.CommandText = SQL
        Adapter.Fill(Table)
        Return Table
    End Function
    Public Function Cancel(ByVal Command As Object) As Boolean
        Try
            Command.Cancel()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Function Cancel() As Boolean
        Try
            Me.oCommand.Cancel()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Function Connect(ByVal Location As String, ByVal DB As String, ByVal UserName As String, ByVal Password As String, ByVal Type As ConnectionType, Optional ByVal Name As String = "") As Object
        Me.Location = Location
        Me.DataBase = DB
        Me.UserName = UserName
        Me.Password = Password
        Me.Type = Type
        Me.Name = Name
        Return Me.Connect
    End Function
    Public Sub UpdateData(ByVal Adapter As Object, ByVal Table As DataTable)
        Try
            Adapter.Update(Table)
            MsgBox("Saved", MsgBoxStyle.Information)
        Catch ex As Exception
            Utility.LastOccuredException = ex
            MsgBox("Could not save updated data. Exception is " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub
    Public Sub Define(ByVal Element As System.Xml.XmlElement)
        Me.ReferencedXMLNode = Element
        Me.Location = Element.SelectSingleNode("Location").InnerText
        Me.DataBase = Element.SelectSingleNode("Catalog").InnerText
        Me.UserName = Element.SelectSingleNode("UserName").InnerText()
        Me.Password = Element.SelectSingleNode("Password").InnerText()
        Me.Type = Element.SelectSingleNode("Type").InnerText
        Me.Name = Element.Attributes("Name").Value
    End Sub
    Public Function Connect(ByVal Element As System.Xml.XmlElement) As Object
        Me.ReferencedXMLNode = Element
        Return Me.Connect(Element.SelectSingleNode("Location").InnerText, Element.SelectSingleNode("Catalog").InnerText, Element.SelectSingleNode("UserName").InnerText, Element.SelectSingleNode("Password").InnerText, Element.SelectSingleNode("Type").InnerText, Element.Attributes("Name").Value)
    End Function
    Public Sub Export(Optional ByVal DataTable As DataTable = Nothing, Optional ByVal DataTableName As String = "")
        Dim TransferEntity As New TransferEntity
        TransferEntity.SourceDataSource = Me
        TransferEntity.TableName = DataTableName
        TransferEntity.DataTable = DataTable
        Dim Exporter As New Exporter
        Exporter.TransferEntity = TransferEntity
        If Exporter.ShowDialog() = DialogResult.OK Then
            Dim Step2 As New Exporter2
            Step2.TransferEntity = TransferEntity
            Step2.ShowDialog()
        End If
    End Sub
    Public Function RefreshConnection()
        Try
            If DirectCast(Me.oConnection, OracleConnection).State = ConnectionState.Broken OrElse DirectCast(Me.oConnection, OracleConnection).State = ConnectionState.Closed Then
                DirectCast(Me.oConnection, OracleConnection).Open()
            End If
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function
    Public Function Connect() As Object
        If Not Me.Connected Then
            If Me.Location <> "" Then
                Select Case Me.Type
                    Case ConnectionType.Oracle
                        oConnection = New OracleConnection("Data Source=" & Me.Location & "/" & Me.DataBase & ";user id=" & Me.UserName & ";password=" & Me.Password & ";Pooling=false;")
                        Me.oCommand = New OracleCommand
                        Me.oAdapter = New OracleDataAdapter(oCommand)
                        CommandBuilder = New OracleCommandBuilder(Me.oAdapter)
                    Case ConnectionType.SQLServer
                        oConnection = New SqlClient.SqlConnection("initial Catalog=" & Me.DataBase & ";Data Source=" & Me.Location & ";User iD=" & Me.UserName & ";password=" & Me.Password & ";")
                        Me.oCommand = New SqlClient.SqlCommand
                        Me.oAdapter = New SqlClient.SqlDataAdapter(oCommand)
                        Me.CommandBuilder = New Data.SqlClient.SqlCommandBuilder(Me.oAdapter)
                    Case ConnectionType.FoxPro
                        Me.oConnection = New OdbcConnection("Driver={Microsoft Visual FoxPro Driver};SourceType=DBF;SourceDB=" & Me.Location & ";Exclusive=No;Collate=Machine")
                        Me.oCommand = New OdbcCommand
                        Me.oAdapter = New OdbcDataAdapter(oCommand)
                        Me.CommandBuilder = New OdbcCommandBuilder(Me.oAdapter)
                    Case ConnectionType.Access
                        If Me.Location.IndexOf(".accdb") > -1 Then
                            Me.oConnection = New OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Me.Location & ";Persist Security Info=True")
                        Else
                            Me.oConnection = New OleDb.OleDbConnection("Provider=Microsoft.Jet.OleDB.4.0;Data Source=" & Me.Location)
                        End If
                        Me.oCommand = New OleDb.OleDbCommand
                        Me.oAdapter = New OleDb.OleDbDataAdapter(oCommand)
                        Me.CommandBuilder = New OleDb.OleDbCommandBuilder(Me.oAdapter)
                    Case ConnectionType.Excel
                        If Me.Location.IndexOf(".xlsx") > -1 Then
                            Me.oConnection = New OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Me.Location & ";Extended Properties=""Excel 12.0;HDR=YES"";")
                        Else
                            Me.oConnection = New OleDb.OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & Me.Location & ";Extended Properties=""Excel 8.0;HDR=Yes;IMEX=1"";")
                        End If
                        Me.oCommand = New OleDb.OleDbCommand
                        Me.oAdapter = New OleDb.OleDbDataAdapter(oCommand)
                        Me.CommandBuilder = New OleDb.OleDbCommandBuilder(Me.oAdapter)
                    Case ConnectionType.XML
                        Me.oConnection = New Xml.XmlDocument
                End Select
                If Me.Type <> ConnectionType.XML Then Me.oCommand.Connection = Me.oConnection
            End If
            Try
                If Me.Type <> ConnectionType.XML Then
                    Me.oConnection.Open()
                    Me.bConnected = True
                Else
                    Me.oConnection.Load(Me.Location)
                    Me.bConnected = True
                End If
                Return oConnection
            Catch ex As Exception
                Return Nothing
            End Try
        End If
        Return Nothing
    End Function
    Public Function CreateCommand() As Object
        Me.Connect()
        Dim Command As Object = Nothing
        Select Case Me.Type
            Case ConnectionType.Oracle
                Command = New OracleCommand
            Case ConnectionType.SQLServer
                Command = New SqlClient.SqlCommand
            Case ConnectionType.FoxPro
                Command = New OdbcCommand
            Case ConnectionType.Access
                Command = New OleDb.OleDbCommand
            Case ConnectionType.Excel
                Command = New OleDb.OleDbCommand
        End Select
        If Not Command Is Nothing Then Command.Connection = Me.oConnection
        Return Command
    End Function
    Public Function CreateBuilder(ByVal Adapter As Object) As Object
        Dim Builder As Object = Nothing
        Select Case Me.Type
            Case ConnectionType.Oracle
                Builder = New OracleCommandBuilder(Adapter)
            Case ConnectionType.SQLServer
                Builder = New Data.SqlClient.SqlCommandBuilder(Adapter)
            Case ConnectionType.FoxPro
                Builder = New OdbcCommandBuilder(Adapter)
            Case ConnectionType.Access
                Builder = New OleDb.OleDbCommandBuilder(Adapter)
            Case ConnectionType.Excel
                Builder = New OleDb.OleDbCommandBuilder(Adapter)
        End Select
        Return Builder
    End Function
    Public Function CreateAdapter(ByVal Command As Object) As Object
        If Command Is Nothing Then Command = Me.CreateCommand
        Dim Adapter As Object = Nothing
        Select Case Me.Type
            Case ConnectionType.Oracle
                Adapter = New OracleDataAdapter(Command)
            Case ConnectionType.SQLServer
                Adapter = New SqlClient.SqlDataAdapter(Command)
            Case ConnectionType.FoxPro
                Adapter = New OdbcDataAdapter(Command)
            Case ConnectionType.Access
                Adapter = New OleDb.OleDbDataAdapter(Command)
            Case ConnectionType.Excel
                Adapter = New OleDb.OleDbDataAdapter(Command)
        End Select
        Return Adapter
    End Function
    Public Sub SaveConnection(ByVal Path As String)
        Dim Doc As XmlDocument = Me.GetDataDocument
        Dim ConnectionsNode As XmlNode = Doc.SelectSingleNode("Connections")
        Dim ConnectionNode As XmlNode
        If Path = "" Then
            ConnectionNode = ConnectionsNode.AppendChild(Doc.CreateElement("Connection"))
        Else
            Dim sPath() As String = Path.Split("\")
            Dim Node As XmlNode = ConnectionsNode
            Dim n As Integer
            For n = 0 To sPath.Length - 1
                Node = Node.SelectSingleNode(sPath(n))
            Next
            ConnectionNode = Node.AppendChild(Doc.CreateElement("Connection"))
        End If
        ConnectionNode.Attributes.Append(Doc.CreateAttribute("Name")).Value = Me.Name
        ConnectionNode.AppendChild(Doc.CreateElement("Location")).InnerText = Me.Location
        ConnectionNode.AppendChild(Doc.CreateElement("Catalog")).InnerText = Me.DataBase
        ConnectionNode.AppendChild(Doc.CreateElement("UserName")).InnerText = Me.UserName
        ConnectionNode.AppendChild(Doc.CreateElement("Password")).InnerText = Me.Password
        ConnectionNode.AppendChild(Doc.CreateElement("Type")).InnerText = Me.Type
        Doc.Save(AppDomain.CurrentDomain.BaseDirectory & "\Data\Connections.xml")
    End Sub
    Public Function GetDataDocument() As XmlDocument
        Dim Doc As New XmlDocument
        If Not IO.Directory.Exists(AppDomain.CurrentDomain.BaseDirectory & "\Data\") Then
            IO.Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory & "\Data\")
        End If
        If IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory & "\Data\Connections.xml") Then
            Doc.Load(AppDomain.CurrentDomain.BaseDirectory & "\Data\Connections.xml")
        Else
            Doc.AppendChild(Doc.CreateElement("Connections"))
            Doc.Save(AppDomain.CurrentDomain.BaseDirectory & "\Data\Connections.xml")
        End If
        Return Doc
    End Function
    Public Sub Disconnect()
        Try
            Me.oConnection.Close()
        Catch ex As Exception

        End Try
    End Sub
    Public Function GetOpenBracket() As String
        Select Case Me.Type
            Case ConnectionType.Access, ConnectionType.SQLServer, ConnectionType.FoxPro
                Return "["
            Case ConnectionType.Oracle
                Return """"
        End Select
        Return ""
    End Function
    Public Function GetCloseBracket() As String
        Select Case Me.Type
            Case ConnectionType.Access, ConnectionType.SQLServer, ConnectionType.FoxPro
                Return "]"
            Case ConnectionType.Oracle
                Return """"
        End Select
        Return ""
    End Function
    Public Function GetSQLDataType(ByVal Type As Type) As String
        Select Case Type.Name
            Case "String"
                Return Me.GetSQLDataType("Memo", "4000")
            Case "Char"
                Return Me.GetSQLDataType("Char", "1")
            Case "Byte"
                Return Me.GetSQLDataType("Byte", "8")
            Case "Boolean"
                Return Me.GetSQLDataType("Boolean", "1")
            Case "Int32", "Integer"
                If Me.Type = ConnectionType.Oracle Then
                    Return Me.GetSQLDataType("Int32", "32")
                ElseIf Me.Type = ConnectionType.FoxPro Then
                    Return Me.GetSQLDataType("Int32", "20,5")
                Else
                    Return Me.GetSQLDataType("Int32", "28")
                End If
            Case "Long", "Int64"
                If Me.Type = ConnectionType.Oracle Then
                    Return Me.GetSQLDataType("Int64", "38")
                ElseIf Me.Type = ConnectionType.FoxPro Then
                    Return Me.GetSQLDataType("Int64", "20,5")
                Else
                    Return Me.GetSQLDataType("Int64", "28")
                End If
            Case "Decimal", "Float", "Single"
                If Me.Type = ConnectionType.Oracle Then
                    Return Me.GetSQLDataType("Decimal", "38,5")
                ElseIf Me.Type = ConnectionType.FoxPro Then
                    Return Me.GetSQLDataType("Decimal", "20,5")
                Else
                    Return Me.GetSQLDataType("Decimal", "28,5")
                End If
            Case "Date", "DateTime"
                Return Me.GetSQLDataType("DateTime", "")
        End Select
        Return Me.GetSQLDataType("String", "100")
    End Function
    Public Function GetSQLDataType(ByVal Code As String, ByVal Value As String) As String
        Select Case Code
            Case "String"
                Select Case Me.Type
                    Case ConnectionType.Access
                        Return "text(" & Value & ")"
                    Case ConnectionType.SQLServer
                        Return "varchar(" & Value & ")"
                    Case ConnectionType.Oracle
                        Return "VARCHAR2(" & Value & ")"
                    Case ConnectionType.FoxPro
                        If Value > 254 Then Value = 254
                        Return "C(" & Value & ")"
                End Select
            Case "Char"
                Select Case Me.Type
                    Case ConnectionType.Access
                        Return "text(1)"
                    Case ConnectionType.SQLServer
                        Return "varchar(1)"
                    Case ConnectionType.Oracle
                        Return "VARCHAR2(1)"
                    Case ConnectionType.FoxPro
                        Return "C(1)"
                End Select
            Case "Byte"
                Select Case Me.Type
                    Case ConnectionType.Access
                        Return "byte"
                    Case ConnectionType.SQLServer
                        Return "tinyint"
                    Case ConnectionType.Oracle
                        Return "NUMBER(8)"
                    Case ConnectionType.FoxPro
                        Return "N(8)"
                End Select
            Case "Boolean"
                Select Case Me.Type
                    Case ConnectionType.Access
                        Return "byte"
                    Case ConnectionType.SQLServer
                        Return "bit"
                    Case ConnectionType.Oracle
                        Return "NUMBER(1)"
                    Case ConnectionType.FoxPro
                        Return "N(1)"
                End Select
            Case "Bit"
                Select Case Me.Type
                    Case ConnectionType.Access
                        Return "byte"
                    Case ConnectionType.SQLServer
                        Return "bit"
                    Case ConnectionType.Oracle
                        Return "NUMBER(1)"
                    Case ConnectionType.FoxPro
                        Return "N(1)"
                End Select
            Case "Int32", "Integer"
                Select Case Me.Type
                    Case ConnectionType.Access
                        Return "integer"
                    Case ConnectionType.SQLServer
                        Return "int"
                    Case ConnectionType.Oracle
                        Return "NUMBER"
                    Case ConnectionType.FoxPro
                        If Value > 20 Then Value = 20
                        Return "N(" & Value & ")"
                End Select
            Case "Int64", "Long"
                Select Case Me.Type
                    Case ConnectionType.Access
                        Return "long"
                    Case ConnectionType.SQLServer
                        Return "bigint"
                    Case ConnectionType.Oracle
                        Return "NUMBER(" & Value & ")"
                    Case ConnectionType.FoxPro
                        If Value > 20 Then Value = 20
                        Return "N(" & Value & ")"
                End Select
            Case "Date", "DateTime"
                Select Case Me.Type
                    Case ConnectionType.Access, ConnectionType.SQLServer
                        Return "datetime"
                    Case ConnectionType.Oracle
                        Return "DATE"
                    Case ConnectionType.FoxPro
                        Return "D"
                End Select
            Case "Object"
                Select Case Me.Type
                    Case ConnectionType.Access
                        Return "long"
                    Case ConnectionType.SQLServer
                        Return "bigint"
                    Case ConnectionType.Oracle
                        Return "NUMBER(" & Value & ")"
                    Case ConnectionType.FoxPro
                        If Value > 20 Then Value = 20
                        Return "N(" & Value & ")"
                End Select
            Case "Empty"
                Select Case Me.Type
                    Case ConnectionType.Access
                        Return "text(" & Value & ")"
                    Case ConnectionType.SQLServer
                        Return "varchar(" & Value & ")"
                    Case ConnectionType.Oracle
                        Return "VARCHAR2(" & Value & ")"
                    Case ConnectionType.FoxPro
                        If Value > 254 Then Value = 254
                        Return "C(" & Value & ")"
                End Select
            Case "Decimal", "Float"
                Select Case Me.Type
                    Case ConnectionType.Access
                        Return "decimal(" & Value & ")"
                    Case ConnectionType.SQLServer
                        Return "decimal(" & Value & ")"
                    Case ConnectionType.Oracle
                        Return "NUMBER(" & Value & ")"
                    Case ConnectionType.FoxPro
                        If Value > 20 Then Value = 20
                        Return "N(" & Value & ")"
                End Select
            Case "Double"
                Select Case Me.Type
                    Case ConnectionType.Access, ConnectionType.SQLServer
                        Return "double"
                    Case ConnectionType.Oracle
                        Return "NUMBER(" & Value & ")"
                    Case ConnectionType.FoxPro
                        If Value > 20 Then Value = 20
                        Return "N(" & Value & ")"
                End Select
            Case "Memo" 'Own type definition of Ardita
                Select Case Me.Type
                    Case ConnectionType.Access
                        Return "memo"
                    Case ConnectionType.SQLServer
                        Return "ntext"
                    Case ConnectionType.Oracle
                        Return "VARCHAR2(" & Value & ")"
                    Case ConnectionType.FoxPro
                        If Value > 254 Then Value = 254
                        Return "C(" & Value & ")"
                End Select
            Case "Single"
                Select Case Me.Type
                    Case ConnectionType.Access
                        Return "single"
                    Case ConnectionType.SQLServer
                        Return "tinyint"
                    Case ConnectionType.Oracle
                        Return "NUMBER(" & Value & ")"
                    Case ConnectionType.FoxPro
                        If Value > 20 Then Value = 20
                        Return "N(" & Value & ")"
                End Select
        End Select
        Return ""
    End Function
End Class
Public Enum ConnectionType As Integer
    Oracle = 1
    SQLServer = 2
    Access = 3
    FoxPro = 4
    Excel = 5
    XML = 6
End Enum
