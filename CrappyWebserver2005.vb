Imports System
Imports System.Net
Imports System.Text
Imports System.Security.Cryptography
Imports System.IO

Public Class CrappyWebServer2005

    Public Shared Sub Main()
        Dim prefix As String = "http://localhost:8080/"
        Dim listener As HttpListener = New HttpListener()
        listener.Prefixes.Add(prefix)
        listener.Start()
        Console.WriteLine("Server is running on " & prefix)

        While True
            Dim context As HttpListenerContext = listener.GetContext()
            Dim request As HttpListenerRequest = context.Request
            Dim response As HttpListenerResponse = context.Response

            If request.HttpMethod = "POST" AndAlso request.Url.PathAndQuery = "/api/md5" Then
                Dim reader As New StreamReader(request.InputStream, request.ContentEncoding)
                Dim requestBody As String = reader.ReadToEnd()

                Dim md5Hash As String = CalculateMD5(requestBody)

                Dim responseString As String = "MD5 Hash: " & md5Hash
                Dim buffer() As Byte = Encoding.UTF8.GetBytes(responseString)

                response.ContentLength64 = buffer.Length
                response.OutputStream.Write(buffer, 0, buffer.Length)
            Else
                response.StatusCode = 404
            End If

            response.Close()
        End While
    End Sub

    Private Shared Function CalculateMD5(ByVal input As String) As String
        Dim md5 As MD5 = MD5.Create()
        Dim inputBytes() As Byte = Encoding.ASCII.GetBytes(input)
        Dim hashBytes() As Byte = md5.ComputeHash(inputBytes)

        Dim sb As New StringBuilder()
        For i As Integer = 0 To hashBytes.Length - 1
            sb.Append(hashBytes(i).ToString("X2"))
        Next

        Return sb.ToString()
    End Function

End Class
