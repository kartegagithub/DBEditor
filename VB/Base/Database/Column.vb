Public Class Column
    Private oCollection As ColumnCollection
    Private sName As String = ""
    Private oDefault As Object = ""
    Private bNullable As Boolean = True
    Private sDataType As String = ""
    Private nDataLength As Integer = 0
    Private nPrecision As Integer = 0
    Private nDateTimePrecision As Integer = 0
    Private nScale As Integer = 0
    Private sCharSetSchema As String = ""
    Private sCharSetName As String = ""
    Public ReadOnly Property [Default]() As Object
        Get
            Return Me.oDefault
        End Get
    End Property
    Public ReadOnly Property Nullable() As Boolean
        Get
            Return Me.bNullable
        End Get
    End Property
    Public ReadOnly Property DataType() As String
        Get
            Return Me.sDataType
        End Get
    End Property
    Public ReadOnly Property DataLength() As Integer
        Get
            Return Me.nDataLength
        End Get
    End Property
    Public ReadOnly Property Precision() As Integer
        Get
            Return Me.nPrecision
        End Get
    End Property
    Public ReadOnly Property DateTimePrecision() As Integer
        Get
            Return Me.nDateTimePrecision
        End Get
    End Property
    Public ReadOnly Property Scale() As Integer
        Get
            Return Me.nScale
        End Get
    End Property
    Public ReadOnly Property CharSetSchema() As String
        Get
            Return Me.sCharSetSchema
        End Get
    End Property
    Public ReadOnly Property CharSetName() As String
        Get
            Return Me.sCharSetName
        End Get
    End Property
    Public ReadOnly Property Name() As String
        Get
            Return Me.sName
        End Get
    End Property
    Public ReadOnly Property DataSource() As DataSource
        Get
            Return Me.oCollection.Source
        End Get
    End Property
    Public ReadOnly Property Table() As Table
        Get
            Return Me.oCollection.Table
        End Get
    End Property
    Public Function IsEquilant(ByVal Column As Column) As Boolean
        If LCase(Column.Name).Replace("ý", "i") = LCase(Me.Name).Replace("ý", "i") AndAlso Me.DataLength = Column.DataLength AndAlso Me.DataType = Column.DataType AndAlso Me.Precision = Column.Precision AndAlso Me.Scale = Column.Scale AndAlso Me.Default = Column.Default AndAlso Me.Nullable = Column.Nullable Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Sub New(ByVal Collection As ColumnCollection, ByVal Row As DataRow)
        Me.oCollection = Collection
        Me.sName = Row("COLUMN_NAME")
        Try
            If Not Row("COLUMN_DEFAULT") Is DBNull.Value Then Me.oDefault = Row("COLUMN_DEFAULT")
        Catch ex As Exception
            Try
                If Not Row("DATA_DEFAULT") Is DBNull.Value Then Me.oDefault = Row("DATA_DEFAULT")
            Catch ex2 As Exception

            End Try
        End Try
        Try
            If Not Row("IS_NULLABLE") Is DBNull.Value Then
                If (IsNumeric(Row("IS_NULLABLE")) AndAlso Row("IS_NULLABLE") > 0) OrElse Row("IS_NULLABLE") = "YES" OrElse Row("IS_NULLABLE") = "yes" OrElse Row("IS_NULLABLE") = "Y" OrElse Row("IS_NULLABLE") = "y" Then
                    Me.bNullable = True
                Else
                    Me.bNullable = False
                End If
            End If
        Catch ex As Exception
            Try
                If Not Row("NULLABLE") Is DBNull.Value Then
                    If (IsNumeric(Row("NULLABLE")) AndAlso Row("NULLABLE") > 0) OrElse Row("NULLABLE") = "YES" OrElse Row("NULLABLE") = "yes" OrElse Row("NULLABLE") = "Y" OrElse Row("NULLABLE") = "y" Then
                        Me.bNullable = True
                    Else
                        Me.bNullable = False
                    End If
                End If
            Catch ex2 As Exception

            End Try
        End Try
        Try
            If Not Row("DATA_TYPE") Is DBNull.Value Then Me.sDataType = Row("DATA_TYPE")
        Catch ex As Exception

        End Try
        Try
            If Not Row("CHARACTER_MAXIMUM_LENGTH") Is DBNull.Value Then Me.nDataLength = Row("CHARACTER_MAXIMUM_LENGTH")
        Catch ex As Exception
            Try
                If Not Row("DATA_LENGTH") Is DBNull.Value Then Me.nDataLength = Row("DATA_LENGTH")
            Catch ex2 As Exception

            End Try
        End Try
        Try
            If Not Row("NUMERIC_PRECISION") Is DBNull.Value Then Me.nPrecision = Row("NUMERIC_PRECISION")
        Catch ex As Exception
            Try
                If Not Row("DATA_PRECISION") Is DBNull.Value Then Me.nPrecision = Row("DATA_PRECISION")
            Catch ex2 As Exception

            End Try
        End Try
        Try
            If Not Row("NUMERIC_SCALE") Is DBNull.Value Then Me.nScale = Row("NUMERIC_SCALE")
        Catch ex As Exception
            Try
                If Not Row("DATA_SCALE") Is DBNull.Value Then Me.nScale = Row("DATA_SCALE")
            Catch ex2 As Exception

            End Try
        End Try
        Try
            If Not Row("DATETIME_PRECISION") Is DBNull.Value Then Me.nDateTimePrecision = Row("DATETIME_PRECISION")
        Catch ex As Exception
            Me.nDateTimePrecision = Me.nPrecision
        End Try
        Try
            If Not Row("CHARACTER_SET_SCHEMA") Is DBNull.Value Then Me.sCharSetSchema = Row("CHARACTER_SET_SCHEMA")
        Catch ex As Exception

        End Try
        Try
            If Not Row("CHARACTER_SET_NAME") Is DBNull.Value Then Me.sCharSetName = Row("CHARACTER_SET_NAME")
        Catch ex As Exception

        End Try
    End Sub
    Public Sub New(ByVal Collection As ColumnCollection, ByVal Name As String)
        oCollection = Collection
        sName = Name
    End Sub
End Class
