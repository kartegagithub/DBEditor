Public Class Sequence
    Private oCollection As SequenceCollection
    Private sName As String = ""
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
    Public Function IsEquilant(ByVal Sequence As Sequence) As Boolean
        If LCase(Sequence.Name).Replace("ý", "i") = LCase(Me.Name).Replace("ý", "i") Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Sub New(ByVal Collection As SequenceCollection, ByVal Name As String)
        Me.oCollection = Collection
        Me.sName = Name
    End Sub
End Class
