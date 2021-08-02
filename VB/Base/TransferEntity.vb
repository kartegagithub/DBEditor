Public Class TransferEntity
    Private oDataTable As DataTable
    Private sFile As String
    Private sTableName As String
    Private sDestinationTableName As String
    Private eTransferType As TransferType = TransferType.XML
    Private oSourceDataSource As DataSource
    Private oDestinationDataSource As DataSource
    Private sAlias As String = "DataTable1"
    Private bClearData As Boolean = False
    Public Property DestinationDataSource() As DataSource
        Get
            Return Me.oDestinationDataSource
        End Get
        Set(ByVal value As DataSource)
            Me.oDestinationDataSource = value
        End Set
    End Property
    Public Property ClearData() As Boolean
        Get
            Return Me.bClearData
        End Get
        Set(ByVal value As Boolean)
            Me.bClearData = value
        End Set
    End Property
    Public Property SourceDataSource() As DataSource
        Get
            Return Me.oSourceDataSource
        End Get
        Set(ByVal value As DataSource)
            Me.oSourceDataSource = value
        End Set
    End Property
    Public Property DataTable() As DataTable
        Get
            Return Me.oDataTable
        End Get
        Set(ByVal value As DataTable)
            Me.oDataTable = value
        End Set
    End Property
    Public Property [Alias]() As String
        Get
            Return Me.sAlias
        End Get
        Set(ByVal value As String)
            Me.sAlias = value
        End Set
    End Property
    Public Property File() As String
        Get
            Return Me.sFile
        End Get
        Set(ByVal value As String)
            Me.sFile = value
        End Set
    End Property
    Public Property DestinationTableName() As String
        Get
            Return Me.sDestinationTableName
        End Get
        Set(ByVal value As String)
            Me.sDestinationTableName = value
        End Set
    End Property
    Public Property TableName() As String
        Get
            Return Me.sTableName
        End Get
        Set(ByVal value As String)
            Me.sTableName = value
        End Set
    End Property
    Public Property TransferType() As TransferType
        Get
            Return Me.eTransferType
        End Get
        Set(ByVal value As TransferType)
            Me.eTransferType = value
            If value = DBEditor.TransferType.AnotherTable Then
                Me.DestinationDataSource = Me.SourceDataSource
            End If
        End Set
    End Property
    Public Function Export() As Boolean
        Try
            Select Case Me.TransferType
                Case DBEditor.TransferType.XML
                    Dim Extension As String = ""
                    If Me.File.IndexOf(".xml") = -1 Then
                        Extension = ".xml"
                    End If
                    If Not Me.DataTable Is Nothing Then
                        Me.DataTable.TableName = Me.Alias
                        Me.DataTable.WriteXml(Me.File & Extension)
                        Return True
                    ElseIf Me.TableName <> "" Then
                        Me.DataTable = Me.SourceDataSource.GetDataTable("SELECT * FROM " & Me.TableName)
                        Me.DataTable.TableName = Me.Alias
                        Me.DataTable.WriteXml(Me.File & Extension)
                        Return True
                    End If
                Case DBEditor.TransferType.Excel
                    Dim Extension As String = ""
                    Dim oldCultureInfo As System.Globalization.CultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture
                    Dim NewCultureInfo As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-US")
                    System.Threading.Thread.CurrentThread.CurrentCulture = NewCultureInfo
                    If Me.DataTable Is Nothing Then
                        Me.DataTable = Me.SourceDataSource.GetDataTable("SELECT * FROM " & Me.TableName)
                    End If
                    If Me.File.IndexOf(".xls") = -1 AndAlso Me.File.IndexOf(".xlsx") = -1 Then
                        Extension = ".xls"
                    Else
                        Extension = Right(Me.File, Me.File.Length - Me.File.IndexOf(".xls"))
                        If Extension.IndexOf(".") = -1 Then
                            Extension = "." & Extension
                        End If
                    End If
                    Dim oExcel As Microsoft.Office.Interop.Excel.Application
                    oExcel = CreateObject("Excel.Application")
                    oExcel.Visible = True
                    oExcel.UserControl = True
                    Dim oWorkBooks As Object = oExcel.Workbooks
                    Dim oWorkBook As Object
                    Dim oWorksheet As Object
                    oWorkBook = oExcel.Workbooks.Add()
                    Dim oWorksheets As Object = oWorkBook.Worksheets
                    oWorksheet = oWorkBook.Sheets(1)
                    Dim StartingRow As Integer = 0
                    Dim i As Integer
                    Dim n As Integer
                    Dim Column As DataColumn
                    Dim RowCount As Integer = Me.DataTable.Rows.Count
                    Dim DrawRowHeader As Integer = 0
                    For n = 0 To Me.DataTable.Columns.Count - 1
                        Column = Me.DataTable.Columns(n)
                        oWorksheet.Cells(1, n + 1) = Column.ColumnName
                    Next
                    For i = 0 To Me.DataTable.Rows.Count - 1
                        For n = 0 To Me.DataTable.Columns.Count - 1
                            Column = Me.DataTable.Columns(n)
                            oWorksheet.Cells(i + 2, n + 1) = Me.DataTable.Rows(i)(Column.ColumnName)
                        Next
                    Next
                    oWorksheet.Columns.AutoFit()
                    oExcel.SaveWorkspace(Me.File.Replace(Extension, "") & Extension)
                    System.Threading.Thread.CurrentThread.CurrentCulture = oldCultureInfo
                    Return True
                Case Else
                    If Me.DataTable Is Nothing AndAlso Me.TableName <> "" Then
                        Me.DataTable = Me.SourceDataSource.GetDataTable("SELECT * FROM " & Me.TableName)
                    End If
                    Dim PrimaryKeyColumn As DataColumn = Nothing
                    Dim n As Integer
                    For n = 0 To Me.DataTable.Columns.Count - 1
                        If Me.DataTable.Columns(n).Unique Then
                            PrimaryKeyColumn = Me.DataTable.Columns(n)
                            Exit For
                        End If
                    Next
                    If PrimaryKeyColumn Is Nothing Then
                        For n = 0 To Me.DataTable.Columns.Count - 1
                            If Not Me.DataTable.Columns(n).AllowDBNull Then
                                PrimaryKeyColumn = Me.DataTable.Columns(n)
                                Exit For
                            End If
                        Next
                    End If
                    If PrimaryKeyColumn Is Nothing Then PrimaryKeyColumn = Me.DataTable.Columns(0)
                    If Not Me.DestinationDataSource.Tables(Me.DestinationTableName) Is Nothing Then
                        If Me.ClearData Then
                            Me.DestinationDataSource.ExecuteQuery("DROP TABLE " & Me.DestinationTableName)
                        Else
                            Me.TableName &= 2
                        End If
                        Me.DestinationDataSource.NewTable(Me.DestinationTableName, PrimaryKeyColumn)
                    Else
                        Me.DestinationDataSource.NewTable(Me.DestinationTableName, PrimaryKeyColumn)
                    End If
                    Me.CheckColumns(Me.DestinationDataSource, Me.DestinationTableName, Me.DataTable)
                    Dim Row As DataRow
                    For n = 0 To Me.DataTable.Rows.Count - 1
                        Row = Me.DataTable.Rows(n)
                        Me.DestinationDataSource.ExecuteQuery(Me.CreateInsertCommand(Row))
                    Next
                    Return True
            End Select
        Catch ex As Exception
            MsgBox("Could not finish exporting. Exception : " & ex.Message, MsgBoxStyle.Critical)
            Return False
        End Try
        Return True
    End Function
    Private Function CreateInsertCommand(ByVal Row As DataRow) As String
        Dim n As Integer
        Dim Fields As String = ""
        Dim Values As String = ""
        For n = 0 To Me.DataTable.Columns.Count - 1
            If Not Row(Me.SourceDataSource.GetFormattedColumnName(Me.DataTable.Columns(n).ColumnName, False)) Is DBNull.Value Then
                If Fields <> "" Then Fields &= ", "
                If Values <> "" Then Values &= ", "
                Fields &= Me.DestinationDataSource.GetFormattedColumnName(Me.DataTable.Columns(n).ColumnName)
                If Me.DataTable.Columns(n).DataType.Name = "String" OrElse Me.DataTable.Columns(n).DataType.Name = "Char" Then
                    Values &= "'" & Trim(Replace(Row(Me.SourceDataSource.GetFormattedColumnName(Me.DataTable.Columns(n).ColumnName, False)), "'", "''")) & "'"
                ElseIf Me.DataTable.Columns(n).DataType.Name = "DateTime" OrElse Me.DataTable.Columns(n).DataType.Name = "Date" Then
                    If Me.DestinationDataSource.Type = ConnectionType.Oracle Then
                        Values &= "TO_DATE('" & Row(Me.SourceDataSource.GetFormattedColumnName(Me.DataTable.Columns(n).ColumnName, False)) & "', 'DD:MM:YYYY HH24:MI:SS')"
                    ElseIf Me.DestinationDataSource.Type = ConnectionType.FoxPro Then
                        Dim Value As DateTime = Row(Me.SourceDataSource.GetFormattedColumnName(Me.DataTable.Columns(n).ColumnName, False))
                        Values &= "DATE(" & Value.Year & "," & Value.Month & "," & Value.Day & ")"
                    Else
                        Values &= "'" & Row(Me.SourceDataSource.GetFormattedColumnName(Me.DataTable.Columns(n).ColumnName, False)) & "'"
                    End If
                Else
                    Values &= Replace(Row(Me.SourceDataSource.GetFormattedColumnName(Me.DataTable.Columns(n).ColumnName, False)), ",", ".")
                End If
            End If
        Next
        Return "INSERT INTO " & Me.DestinationDataSource.GetOpenBracket & Me.DestinationTableName & Me.DestinationDataSource.GetCloseBracket & " (" & Fields & ") VALUES (" & Values & ")"
    End Function
    Private Sub CheckColumns(ByVal DataSource As DataSource, ByVal TableName As String, ByVal Data As DataTable)
        Dim n As Integer
        Dim Column As DataColumn
        For n = 0 To Data.Columns.Count - 1
            Column = Data.Columns(n)
            Try
                Me.DestinationDataSource.ExecuteQuery("ALTER TABLE " & Me.DestinationDataSource.GetOpenBracket & TableName & Me.DestinationDataSource.GetCloseBracket & " ADD " & Me.DestinationDataSource.GetFormattedColumnName(Column.ColumnName) & " " & Me.DestinationDataSource.GetSQLDataType(Column.DataType))
            Catch ex As Exception

            End Try
        Next
    End Sub
End Class
Public Enum TransferType As Integer
    Oracle = 1
    SQLServer = 2
    Access = 3
    FoxPro = 4
    Excel = 5
    XML = 6
    AnotherTable = 7
End Enum