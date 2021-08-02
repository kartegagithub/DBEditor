Public Class CollectionBase : Inherits System.Collections.CollectionBase
    Default Public ReadOnly Property Item(ByVal Index As Integer) As Object
        Get
            Return Me.List(Index)
        End Get
    End Property
End Class
