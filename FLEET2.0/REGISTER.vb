Imports System.Data.SqlClient
Imports System.IO

Public Class REGISTER
    Private Sub REGISTER_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Connect()
        TextBox2.PasswordChar = "•"
        TextBox3.PasswordChar = "•"
    End Sub
    Sub Savedata()                                                          'TO SAVE DATA ==============================================================
        Dim ms As New MemoryStream()

        query = "DECLARE @ID INT; SET @ID = (SELECT COUNT(*) + 1 FROM User_Profile); 
        INSERT INTO User_Accounts (Username, Password, Created_at, Updated_at) VALUES (@Username, @Password, GETDATE(), NULL);
        INSERT INTO User_Profile (Firstname, Middlename, Lastname, Address, Contact, Gender, Birthday, ProfileImage, ID, Role, Status, Last_Login, Created_at, Updated_at) 
        VALUES (@Fname, @Mname, @Lname, 'N A', 'N A', 'Male', '2001-01-01', NULL, @ID, 'Driver', 'Inactive', NULL, GETDATE(), NULL);"
        cmd = New SqlCommand(query, sqlconn)
        With cmd.Parameters
            .AddWithValue("@Username", TextBox1.Text)
            .AddWithValue("@Password", TextBox2.Text)
            .AddWithValue("@Fname", TextBox4.Text)
            .AddWithValue("@Mname", TextBox5.Text)
            .AddWithValue("@Lname", TextBox6.Text)
            .AddWithValue("@Created_at", DateTime.Now)
        End With
        cmd.ExecuteNonQuery()
        cmd.Dispose()
    End Sub
    '===============================================================HANDLES============================================================================'
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim existingUname As Boolean = False
        Dim existingname As Boolean = False
        str = "SELECT * FROM User_Accounts FULL JOIN User_Profile ON User_Accounts.ID = User_Profile.PID"
        cmd = New SqlClient.SqlCommand(str, sqlconn)
        dr = cmd.ExecuteReader
        While dr.Read
            If dr("Username").ToString.Equals(TextBox1.Text) Then
                existingUname = True
            ElseIf dr("Firstname").ToString.Equals(TextBox4.Text) And dr("Middlename").ToString.Equals(TextBox5.Text) And dr("Lastname").ToString.Equals(TextBox6.Text) Then
                existingname = True
            End If
        End While
        dr.Close()
        cmd.Dispose()
        If existingUname = True Then
            MsgBox("Username already exist", MsgBoxStyle.Critical)
        ElseIf existingname = True Then
            MsgBox("Name already exist", MsgBoxStyle.Critical)
        ElseIf TextBox1.Text = "" Then
            MsgBox("invalid username", MsgBoxStyle.Critical)
        ElseIf TextBox2.Text = "" Then
            MsgBox("invalid password", MsgBoxStyle.Critical)
        ElseIf TextBox3.Text = "" Then
            MsgBox("invalid confirm password", MsgBoxStyle.Critical)
        ElseIf TextBox2.Text <> TextBox3.Text Then
            MsgBox("failed to confirm password", MsgBoxStyle.Critical)
        ElseIf TextBox4.Text = "" Then
            MsgBox("invalid firstname", MsgBoxStyle.Critical)
        ElseIf TextBox5.Text = "" Then
            MsgBox("invalid middlename", MsgBoxStyle.Critical)
        ElseIf TextBox6.Text = "" Then
            MsgBox("invalid lastname", MsgBoxStyle.Critical)
        Else
            Savedata()
            Refresh()
            MsgBox($"Welcome {TextBox4.Text}, you are now registered please login in", MessageBoxIcon.Information)
            Me.Close()
            LOGIN.Show()
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        LOGIN.Show()
        Me.Close()
    End Sub
    Private Sub FLEETToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FLEETToolStripMenuItem.Click
        LOGIN.Show()
        Me.Close()
    End Sub
    '===============================================================HANDLES============================================================================'
End Class