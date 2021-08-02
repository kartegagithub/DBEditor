Public Class Table
    Private oCollection As TableCollection
    Private sName As String = ""
    Private sSchemaName As String = ""
    Private oIndexes As IndexCollection
    Private oColumns As ColumnCollection
    Public ReadOnly Property Columns() As ColumnCollection
        Get
            Return Me.oColumns
        End Get
    End Property
    Public ReadOnly Property Name() As String
        Get
            Return Me.sName
        End Get
    End Property
    Public ReadOnly Property SchemaName() As String
        Get
            Return Me.sSchemaName
        End Get
    End Property
    Public ReadOnly Property Indexes() As IndexCollection
        Get
            Return Me.oIndexes
        End Get
    End Property
    Public ReadOnly Property Collection() As TableCollection
        Get
            Return Me.oCollection
        End Get
    End Property
    Public ReadOnly Property DataSource() As DataSource
        Get
            Return Me.Collection.Source
        End Get
    End Property
    Public Function IsEquilant(ByVal Table As Table) As Boolean
        If LCase(Table.Name).Replace("ı", "i") = LCase(Me.Name).Replace("ı", "i") Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Sub LoadColumns()
        If Me.oColumns Is Nothing Then
            Me.oColumns = New ColumnCollection(Me)
            Me.oColumns.Load()
        End If
    End Sub
    Public Sub ResetIndexes()
        Me.oIndexes = Nothing
    End Sub
    Public Sub ResetColumns()
        Me.oColumns = Nothing
    End Sub
    Public Sub LoadIndexes()
        If Me.oIndexes Is Nothing Then
            Me.oIndexes = New IndexCollection(Me)
            Me.oIndexes.Load()
        End If
    End Sub
    Public Sub New(ByVal Collection As TableCollection, ByVal Name As String)
        oCollection = Collection
        Me.sName = Name
    End Sub
    Public Sub New(ByVal Collection As TableCollection, ByVal Name As String, ByVal SchemaName As String)
        Me.New(Collection, Name)
        Me.sSchemaName = SchemaName
    End Sub
End Class
