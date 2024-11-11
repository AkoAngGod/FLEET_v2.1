Imports System.Data.SqlClient
Imports System.IO
Imports System.Security.Cryptography.X509Certificates
Imports System.Windows.Forms.VisualStyles.VisualStyleElement

Public Class DRIVERSPROFILE
    Private Sub DRIVERSPROFILE_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Connect()
        Readdata()
        Rowheight()
    End Sub
    Sub Rowheight()
        For i = 0 To DRIVERSVIEW.Rows.Count - 1
            DRIVERSVIEW.Rows(i).Height = 50
        Next
    End Sub
    Sub Readdata()                                                          'DATAGRIDVIEW ==============================================================
        DRIVERSVIEW.Rows.Clear()
        str2 = "Select * from User_Profile"
        cmd = New SqlCommand(str2, sqlconn)
        dr = cmd.ExecuteReader()
        While dr.Read()
            DRIVERSVIEW.Rows.Add(dr("PID"), dr("Firstname"), dr("Middlename"), dr("Lastname"), dr("Address"), dr("Contact"), dr("Gender"), dr("Birthday"), dr("ProfileImage"), "Edit", "Delete")
        End While
        dr.Close()
        cmd.Dispose()
    End Sub
    Sub Clear()                                                                 'CLEAR DATA ==============================================================
        TextBox4.Clear()
        TextBox5.Clear()
        TextBox6.Clear()
        TextBox7.Clear()
        TextBox8.Clear()
        ComboBox1.ResetText()
        PictureBox1.Refresh()
    End Sub
    Sub Updatedata(ByVal UserID As String)                                      'UPDATE DATA ==============================================================
        Dim ms As New MemoryStream()

        query = "Update User_Profile set Firstname = @Fname, Middlename = @Mname, Lastname = @Lname, Address = @Address, Contact = @Contact, Gender = @Gender, Birthday = @Birthday, ProfileImage = @UserImage, Updated_at = @Updated_at where PID = @CID;"
        cmd = New SqlCommand(query, sqlconn)
        With cmd.Parameters
            .AddWithValue("@CID", UserID)
            .AddWithValue("@Fname", TextBox4.Text)
            .AddWithValue("@Mname", TextBox5.Text)
            .AddWithValue("@Lname", TextBox6.Text)
            .AddWithValue("@Address", TextBox7.Text)
            .AddWithValue("@Contact", TextBox8.Text)
            .AddWithValue("@Gender", ComboBox1.Text)
            .AddWithValue("@Birthday", DateTimePicker1.Text)
            .AddWithValue("@UserImage", ms.ToArray())
            .AddWithValue("@Updated_at", DateTime.Now)
        End With
        cmd.ExecuteNonQuery()
        cmd.Dispose()
    End Sub
    Sub Deletedata(ByVal uid As String)

        query = "DELETE FROM User_Accounts where ID = @UserID; delete from User_Profile where PID = @UserID
        DECLARE @NextUserID INT; SET @NextUserID = (SELECT COUNT(*) FROM User_Accounts); 
        DBCC CHECKIDENT('User_Accounts', RESEED, @NextUserID);
        DBCC CHECKIDENT('User_Profile', RESEED, @NextUserID);"
        cmd = New SqlClient.SqlCommand(query, sqlconn)
        With cmd.Parameters
            .AddWithValue("@UserID", uid.ToString)
        End With
        cmd.ExecuteReader()
        cmd.Dispose()
    End Sub
    Sub ViewProfile(ByVal UserID As String)
        Dim img() As Byte
        Dim str As String = "SELECT ProfileImage FROM User_Profile WHERE PID = @UserID"
        cmd = New SqlCommand(str, sqlconn)
        cmd.Parameters.AddWithValue("@UserID", UserID)
        dr = cmd.ExecuteReader()
        If dr.HasRows Then
            dr.Read()
            If Not IsDBNull(dr("ProfileImage")) Then
                img = CType(dr("ProfileImage"), Byte())
                Dim ms As New MemoryStream(img)
                PictureBox1.Image = Image.FromStream(ms)
            Else
                PictureBox1.Image = My.Resources.UPLOAD_ICON
            End If
        End If
        dr.Close()
        cmd.Dispose()
    End Sub
    Sub ViewLicense(ByVal UserID As String)
        Dim img() As Byte
        Dim str As String = "SELECT ProfileImage FROM User_License WHERE PID = @UserID"
        cmd = New SqlCommand(str, sqlconn)
        cmd.Parameters.AddWithValue("@UserID", UserID)
        dr = cmd.ExecuteReader()
        If dr.HasRows Then
            dr.Read()
            If Not IsDBNull(dr("ProfileImage")) Then
                img = CType(dr("ProfileImage"), Byte())
                Dim ms As New MemoryStream(img)
                PictureBox3.Image = Image.FromStream(ms)
            Else
                PictureBox1.Image = My.Resources.UPLOAD_ICON
            End If
        End If
        dr.Close()
        cmd.Dispose()
    End Sub

    Private Sub DRIVERSVIEW_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DRIVERSVIEW.CellContentClick
        If e.ColumnIndex = 9 Then
            Dim i As Integer = DRIVERSVIEW.CurrentRow.Index
            CID = DRIVERSVIEW.Item(0, i).Value
            TextBox4.Text = DRIVERSVIEW.Item(1, i).Value
            TextBox5.Text = DRIVERSVIEW.Item(2, i).Value
            TextBox6.Text = DRIVERSVIEW.Item(3, i).Value
            TextBox7.Text = DRIVERSVIEW.Item(4, i).Value
            TextBox8.Text = DRIVERSVIEW.Item(5, i).Value
            ComboBox1.Text = DRIVERSVIEW.Item(6, i).Value
            ViewProfile(CID)
        ElseIf e.ColumnIndex = 10 Then
            Dim confirm = MsgBox("Do you want to delete this record?", MsgBoxStyle.YesNo)
            If confirm = MsgBoxResult.Yes Then
                Dim i As Integer = DRIVERSVIEW.CurrentRow.Index
                Deletedata(DRIVERSVIEW.Item(0, i).Value)
                Readdata()
                Rowheight()
            End If
        End If
    End Sub

    '===============================================================HANDLES============================================================================'
    Private Sub DRIVERSPROFILEToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DRIVERSPROFILEToolStripMenuItem.Click
        Refresh()
    End Sub
    Private Sub ACCOUNTSToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ACCOUNTSToolStripMenuItem.Click
        ACCOUNTS.Show()
        Me.Close()
    End Sub
    Private Sub FLEETToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FLEETToolStripMenuItem.Click
        DASHBOARD.Show()
        Me.Close()
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        OpenFileDialog1.FileName = ""
        OpenFileDialog1.Filter = "JPG Files(*.jpg)|*.jpg"
        If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            PictureBox1.Image = Image.FromFile(OpenFileDialog1.FileName)
        End If
    End Sub
    Private Sub PictureBox3_Click(sender As Object, e As EventArgs) Handles PictureBox3.Click
        OpenFileDialog2.FileName = ""
        OpenFileDialog2.Filter = "JPG Files(*.jpg)|*.jpg"
        If OpenFileDialog2.ShowDialog = Windows.Forms.DialogResult.OK Then
            PictureBox3.Image = Image.FromFile(OpenFileDialog2.FileName)
        End If
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim existingID As Boolean = False
        Dim existingname As Boolean = False
        str = "SELECT * FROM User_Profile WHERE PID <> @CID;"
        cmd = New SqlClient.SqlCommand(str, sqlconn)
        cmd.Parameters.AddWithValue("@CID", CID)
        dr = cmd.ExecuteReader
        While dr.Read
            If dr("Firstname").ToString.Equals(TextBox4.Text) And dr("Middlename").ToString.Equals(TextBox5.Text) And dr("Lastname").ToString.Equals(TextBox6.Text) Then
                existingname = True
            End If
        End While
        dr.Close()
        cmd.Dispose()
        If existingname = True Then
            MsgBox("Name already exist", MsgBoxStyle.Critical)
        ElseIf TextBox4.Text = "" Then
            MsgBox("Invalid Firstname", MsgBoxStyle.Critical)
        ElseIf TextBox5.Text = "" Then
            MsgBox("Invalid Fiddlename", MsgBoxStyle.Critical)
        ElseIf TextBox6.Text = "" Then
            MsgBox("Invalid Lastname", MsgBoxStyle.Critical)
        Else
            Dim i As Integer = DRIVERSVIEW.CurrentRow.Index
            Updatedata(CID)
            Readdata()
            Rowheight()
            Refresh()
            Clear()
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Refresh()
        Clear()
    End Sub

    '===============================================================HANDLES============================================================================'
End Class