Imports System.Data.SqlClient
Imports System.IO
Imports System.Windows.Forms.VisualStyles.VisualStyleElement

Public Class PROFILE
    Private Sub PROFILE_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Connect()
        Viewimage()

        str2 = "SELECT Firstname, Middlename, Lastname, Address, Contact, Gender, Birthday, Role FROM dbo.User_Profile WHERE PID = @UserID;"
        cmd = New SqlCommand(str2, sqlconn)
        cmd.Parameters.AddWithValue("@UserID", usrid)
        dr = cmd.ExecuteReader()
        dr.Read()
        Fullname = $"{dr("Firstname")} {dr("Middlename")} {dr("Lastname")}"
        _Address = dr("Address")
        Contact = dr("Contact")
        Gender = dr("Gender")
        Birthday = Convert.ToDateTime(dr("Birthday"))
        Role = dr("Role")
        Age = DateTime.Now.Year - Birthday.Year
        If (DateTime.Now < Birthday.AddYears(Age)) Then
            Age -= 1
        End If
        Label1.Text = Fullname
        Label2.Text = Username
        Label3.Text = usrid.ToString()
        Label14.Text = _Address
        Label15.Text = Contact
        Label16.Text = Gender
        Label18.Text = Birthday
        Label19.Text = Role
        Label20.Text = Age

    End Sub
    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        OpenFileDialog1.FileName = ""
        OpenFileDialog1.Filter = "JPG Files(*.jpg)|*.jpg"
        If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            PictureBox1.Image = Image.FromFile(OpenFileDialog1.FileName)
        End If
    End Sub

    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
        OpenFileDialog1.FileName = ""
        OpenFileDialog1.Filter = "JPG Files(*.jpg)|*.jpg"
        If OpenFileDialog2.ShowDialog = Windows.Forms.DialogResult.OK Then
            PictureBox2.Image = Image.FromFile(OpenFileDialog2.FileName)
        End If
    End Sub

    Sub Clear()                                                             'CLEAR BUTTON ==============================================================
        TextBox4.Clear()
        TextBox5.Clear()
        TextBox6.Clear()
        TextBox7.Clear()
        TextBox8.Clear()
        ComboBox1.ResetText()
        TextBox10.Clear()
        TextBox11.Clear()
        OpenFileDialog1.Reset()
        OpenFileDialog2.Reset()
    End Sub

    Private Function GetLastInsertedId() As Integer                         'TO SAVE DATA ==============================================================
        Dim id As Integer
        query = "SELECT TOP 1 ID FROM User_Accounts ORDER BY ID DESC"
        cmd = New SqlCommand(query, sqlconn)
        dr = cmd.ExecuteReader()
        If dr.Read() Then
            id = Convert.ToInt32(dr("ID"))
        End If
        dr.Close()
        cmd.Dispose()
        Return id
    End Function

    Sub Updatedata()                                                        'UPDATE DATA ==============================================================
        Dim ms As New MemoryStream()
        PictureBox1.Image.Save(ms, PictureBox1.Image.RawFormat)

        query = "Update User_Profile set Firstname = @Fname, Middlename = @Mname, Lastname = @Lname, Address = @Address, Contact = @Contact, Gender = @Gender, Birthday = @Birthday, ProfileImage = @UserImage, Updated_at = @Updated_at WHERE PID = @UserID"
        cmd = New SqlCommand(query, sqlconn)
        With cmd.Parameters
            .AddWithValue("@UserID", usrid)
            .AddWithValue("@Fname", TextBox4.Text)
            .AddWithValue("@Mname", TextBox5.Text)
            .AddWithValue("@Lname", TextBox6.Text)
            .AddWithValue("@Address", TextBox7.Text)
            .AddWithValue("@Contact", TextBox8.Text)
            .AddWithValue("@Gender", ComboBox1.Text)
            .AddWithValue("@Birthday", DateTimePicker1.Text)
            .AddWithValue("@UserImage", ms.ToArray())
            .AddWithValue("@Created_at", DateTime.Now)
            .AddWithValue("@Updated_at", DateTime.Now)
        End With
        cmd.ExecuteNonQuery()
        cmd.Dispose()
    End Sub

    Sub Deletedata(ByVal PID As String)
        query = "DELETE FROM User_Profile where PID = @PID"
        cmd = New SqlCommand(query, sqlconn)
        With cmd.Parameters
            .AddWithValue("@PID", PID)
        End With
        cmd.ExecuteNonQuery()
        cmd.Dispose()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim existingID As Boolean = False
        Dim existingname As Boolean = False
        str = "Select * from User_Profile"
        cmd = New SqlClient.SqlCommand(str, sqlconn)
        dr = cmd.ExecuteReader
        While dr.Read
            If dr("PID").ToString.Equals(usrid) Then
                existingID = True
            ElseIf dr("Firstname").ToString.Equals(TextBox4.Text) And dr("Middlename").ToString.Equals(TextBox5.Text) And dr("Lastname").ToString.Equals(TextBox6.Text) Then
                existingname = True
            End If
        End While
        dr.Close()
        cmd.Dispose()
        If existingname = True Then
            MsgBox("Name already exist", MsgBoxStyle.Critical)
        Else
            Updatedata()
            Clear()
        End If
    End Sub


    '===============================================================HANDLES============================================================================'
    Private Sub FLEETToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FLEETToolStripMenuItem.Click
        DASHBOARD.Show()
        Me.Close()
    End Sub
    Private Sub ACCOUNTSToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ACCOUNTSToolStripMenuItem.Click
        ACCOUNTS.Show()
        Me.Close()
    End Sub
    Private Sub DRIVERSPROFILEToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DRIVERSPROFILEToolStripMenuItem.Click
        DRIVERSPROFILE.Show()
        Me.Close()
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Clear()
        Refresh()
    End Sub

    Private Sub PROFILE_Load_1(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
    '===============================================================HANDLES============================================================================'
End Class