Public Class Index
    Private oCollection As IndexCollection
    Private sName As String = ""
    Private sColumn As String = ""
    Public ReadOnly Property Name() As String
        Get
            Return Me.sName
        End Get
    End Property
    Public ReadOnly Property Column() As String
        Get
            Return Me.sColumn
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
    Public Function IsEquilant(ByVal Index As Index) As Boolean
        If LCase(Index.Name).Replace("ý", "i") = LCase(Me.Name).Replace("ý", "i") Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Sub New(ByVal Collection As IndexCollection, ByVal Name As String)
        oCollection = Collection
        Me.sName = Name
    End Sub
End Class
