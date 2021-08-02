Public Module Utility
    Public LastOccuredException As Exception
    Public Container As Container
    Public Sub AskGoogle(ByVal Text As String)
        Process.Start("iexplore.exe", "http://www.google.com.tr/search?hl=tr&ie=UTF-8&oe=UTF-8&q=" & FormatText(Text))
    End Sub
    Public Sub AskTheExceptionGoogle()
        If Not LastOccuredException Is Nothing Then
            AskGoogle(LastOccuredException.Message)
        Else
            Dim Form As New AskGoogleForm
            Form.Show()
        End If
    End Sub
    Public Sub ActivateForm(ByVal Form As Form)
        Form.ShowDialog()
    End Sub
    Private Function FormatText(ByVal Value As String) As String
        Return LCase(Value).Replace(" ", "+").Replace("�", "%C3%A7").Replace("�", "%C5%9").Replace("�", "%C4%B1").Replace("�", "%C4%9F").Replace("�", "%C3%BC")
    End Function
End Module
