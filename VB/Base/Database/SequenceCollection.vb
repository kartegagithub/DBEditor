Public Class SequenceCollection : Inherits CollectionBase
    Private oSource As DataSource
    Default Public Shadows ReadOnly Property Item(ByVal Index As Integer) As Sequence
        Get
            Return Me.List(Index)
        End Get
    End Property
    Public ReadOnly Property Source() As DataSource
        Get
            Return Me.oSource
        End Get
    End Property
    Public Sub Load()
        If Me.Source.Type = ConnectionType.Oracle Then
            Me.Clear()
            Dim Sequences As DataTable = Nothing
            Select Case Me.Source.Type
                Case ConnectionType.Oracle
                    Sequences = Me.Source.GetDataTable("SELECT SEQUENCE_NAME FROM ALL_SEQUENCES WHERE SEQUENCE_OWNER='" & Me.Source.UserName & "' ORDER BY SEQUENCE_NAME")
            End Select
            Dim Row As DataRow
            For Each Row In Sequences.Rows
                Select Case Me.Source.Type
                    Case ConnectionType.Oracle
                        Me.List.Add(New Sequence(Me, Row("SEQUENCE_NAME")))
                End Select
            Next
        End If
    End Sub
    Public Sub New(ByVal Source As DataSource)
        oSource = Source
    End Sub
End Class
