Imports System.Data.SqlClient
Imports System.IO
Imports System.Net.Sockets

Public Class LOGIN
    Dim attempts As Integer = 0

    Private Sub LOGIN_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Connect()
        TextBox2.PasswordChar = "•"
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If String.IsNullOrEmpty(TextBox1.Text) Or String.IsNullOrEmpty(TextBox2.Text) Then
            MsgBox("Invalid Credentials", MsgBoxStyle.Critical)
            Return
        End If
        str = "SELECT * FROM User_Accounts WHERE Username = @Username"
        cmd = New SqlCommand(str, sqlconn)
        cmd.Parameters.AddWithValue("@Username", TextBox1.Text)

        cmd.Parameters.AddWithValue("@Password", TextBox2.Text)
        dr = cmd.ExecuteReader()
        If dr.HasRows Then
            dr.Read()
            pass = dr("Password")
            usrid = dr("ID")
            Username = dr("Username")
            If TextBox2.Text = pass Then
                dr.Close()
                str2 = "SELECT Firstname, Middlename, Lastname, Address, Contact, Gender, Birthday, Role FROM dbo.User_Profile WHERE PID = @UserID; UPDATE User_Profile SET status = 'Active' WHERE PID = @UserID;"
                cmd = New SqlCommand(str2, sqlconn)
                cmd.Parameters.AddWithValue("@UserID", usrid)
                dr = cmd.ExecuteReader()
                If dr.HasRows Then
                    dr.Read()
                    Fullname = $"{dr("Firstname")} {dr("Middlename")} {dr("Lastname")}"
                    _Address = dr("Address")
                    Contact = dr("Contact")
                    Gender = dr("Gender")
                    Birthday = Convert.ToDateTime(dr("Birthday"))
                    Role = dr("Role")
                    MsgBox($"Welcome {Fullname}, you are now logged in")
                    DASHBOARD.Show()
                    Me.Hide()
                End If
            Else
                MsgBox("Wrong username and password", MsgBoxStyle.Critical)
                TextBox1.Clear()
                TextBox2.Clear()
                attempts += 1
                If attempts >= 3 Then
                    MsgBox("Reached the maximum login attempts, system will shutdown", MsgBoxStyle.Critical, "System Message")
                    Label4.Text = "30"
                    Label4.Visible = True
                    Label3.Visible = True
                    Timer1.Start()
                    Me.Enabled = False
                End If
            End If
        End If
        dr.Close()
        cmd.Dispose()
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs)
        TextBox2.PasswordChar = If(CheckBox1.Checked, "", "•")
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If Val(Label4.Text) = 0 Then
            Label3.Visible = False
            Label4.Visible = False
            Me.Enabled = True
            Timer1.Stop()
        Else
            Label4.Text = (Val(Label4.Text) - 1).ToString()
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        REGISTER.Show()
        Me.Hide()
    End Sub
End Class
