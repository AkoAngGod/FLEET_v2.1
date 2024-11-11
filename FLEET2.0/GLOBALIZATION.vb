Imports System.Data.SqlClient
Imports System.IO
Imports System.Security.Cryptography
Imports System.Text

Module GLOBALIZATION
    Public sqlconn As New SqlConnection
    Public cmd As New SqlCommand
    Public dr As SqlDataReader
    Public da As New SqlDataAdapter
    Public query, query1 As String
    Public str, str1, str2 As String
    Public userlevel, usrid, CID As String
    Public Fullname, Username, pass, _Address, Contact, Gender, Role As String
    Public Birthday As DateTime
    Public Age As Integer
    Sub Connect()
        Try
            If sqlconn.State = ConnectionState.Open Then sqlconn.Close()
            sqlconn.ConnectionString = "Server=DESKTOP-OMQQVCQ\SQLEXPRESS;Database=FLEET;Trusted_Connection=True; MultipleActiveResultsets=True;"
            sqlconn.Open()
        Catch ex As Exception
            MsgBox("Error in connection please contact administrator")
        End Try
    End Sub
    Sub Logout(ByVal PID As String)
        query = "UPDATE User_Profile SET last_login = @LL WHERE PID = @PID"
        cmd = New SqlCommand(query, sqlconn)
        With cmd.Parameters
            .AddWithValue("@PID", PID)
            .AddWithValue("LL", DateTime.Now)
        End With
        cmd.ExecuteNonQuery()
        cmd.Dispose()
    End Sub

    Sub ViewImage()
        Dim img() As Byte
        Dim str As String = "SELECT ProfileImage FROM User_Profile WHERE PID = @PID"
        cmd = New SqlCommand(str, sqlconn)
        cmd.Parameters.AddWithValue("@PID", usrid)
        dr = cmd.ExecuteReader()
        If dr.HasRows Then
            dr.Read()
            If Not IsDBNull(dr("ProfileImage")) Then
                img = CType(dr("ProfileImage"), Byte())
                Dim ms As New MemoryStream(img)
                DASHBOARD.PictureBox1.Image = Image.FromStream(ms)
                PROFILE.PictureBox3.Image = Image.FromStream(ms)
            End If
        End If
        dr.Close()
        cmd.Dispose()
    End Sub


    '================================================================= ENCRYPTION ================================================================='
    ' Function to generate a salt
    Function GenerateSalt() As String
        Dim rng As New RNGCryptoServiceProvider()
        Dim saltBytes(16) As Byte
        rng.GetBytes(saltBytes)
        Return Convert.ToBase64String(saltBytes)
    End Function
    ' Function to hash a password with salt
    Function HashPassword(password As String, salt As String) As String
        Dim sha256 As New SHA256Managed()
        Dim saltedPassword As String = password & salt
        Dim bytes As Byte() = Encoding.UTF8.GetBytes(saltedPassword)
        Dim hash As Byte() = sha256.ComputeHash(bytes)
        Return Convert.ToBase64String(hash)
    End Function

    '================================================================= ENCRYPTION ================================================================='
End Module
