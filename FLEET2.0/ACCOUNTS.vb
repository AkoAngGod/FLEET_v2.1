Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports System.Data.SqlClient

Public Class ACCOUNTS
    Private Sub Accounts_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Connect()
        Readdata()
        Rowheight()
        TextBox2.PasswordChar = "•"
        TextBox3.PasswordChar = "•"

    End Sub

    Sub Rowheight()
        For i = 0 To DataGridView1.Rows.Count - 1
            DataGridView1.Rows(i).Height = 50
        Next
    End Sub

    Sub Clear()                                                             'CLEAR BUTTON ==============================================================
        TextBox1.Clear()
        TextBox2.Clear()
        TextBox3.Clear()
        TextBox4.Clear()
        TextBox5.Clear()
        TextBox6.Clear()
        TextBox12.Clear()
    End Sub

    Sub Readdata()                                                          'DATAGRIDVIEW ==============================================================
        DataGridView1.Rows.Clear()
        str1 = "SELECT * FROM User_Accounts FULL JOIN User_Profile ON User_Accounts.ID = User_Profile.PID"
        cmd = New SqlCommand(str1, sqlconn)
        dr = cmd.ExecuteReader()
        While dr.Read()
            DataGridView1.Rows.Add(dr("ID"), dr("Username"), dr("Password"), dr("Firstname"), dr("Middlename"), dr("Lastname"), dr("Created_at"), dr("Updated_at"), "Edit", "Delete")
        End While
        dr.Close()
        cmd.Dispose()
    End Sub

    Sub Savedata()                                                          'TO SAVE DATA ==============================================================
        Dim ms As New MemoryStream()

        query = "Insert into User_Accounts ( Username, Password, Created_at, Updated_at) values (@Username, @Password, @Created_at, @Updated_at)"
        cmd = New SqlCommand(query, sqlconn)
        With cmd.Parameters
            .AddWithValue("@Username", TextBox1.Text)
            .AddWithValue("@Password", TextBox2.Text)
            .AddWithValue("@Created_at", DateTime.Now)
            .AddWithValue("@Updated_at", DateTime.Now)
        End With
        cmd.ExecuteNonQuery()
        cmd.Dispose()

        query1 = "Insert into User_Profile (PID, Firstname, Middlename, Lastname, Role Created_at, Updated_at) values (@PID, @Fname, @Mname, @Lname, @Role, @Created_at, @Updated_at)"
        cmd = New SqlCommand(query1, sqlconn)
        With cmd.Parameters
            .AddWithValue("@PID", GetLastInsertedId())
            .AddWithValue("@Fname", TextBox4.Text)
            .AddWithValue("@Mname", TextBox5.Text)
            .AddWithValue("@Lname", TextBox6.Text)
            .AddWithValue("@Role", "Driver")
            .AddWithValue("@Created_at", DateTime.Now)
            .AddWithValue("@Updated_at", DateTime.Now)
        End With
        cmd.ExecuteNonQuery()
        cmd.Dispose()
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

        query = "Update User_Accounts set Username = @Username, Password = @Password, Updated_at = @Updated_at where ID = @usrid;Update User_Profile set Firstname = @Fname, Middlename = @Mname, Lastname = @Lname, ProfileImage = @UserImage, Updated_at = @Updated_at where PID = @usrid;"
        cmd = New SqlCommand(query, sqlconn)
        With cmd.Parameters
            .AddWithValue("@usrid", TextBox12.Text)
            .AddWithValue("@Username", TextBox1.Text)
            .AddWithValue("@Password", TextBox2.Text)
            .AddWithValue("@usrid", TextBox12.Text)
            .AddWithValue("@Fname", TextBox4.Text)
            .AddWithValue("@Mname", TextBox5.Text)
            .AddWithValue("@Lname", TextBox6.Text)
            .AddWithValue("@UserImage", ms.ToArray())
            .AddWithValue("@Updated_at", DateTime.Now)
        End With
        cmd.ExecuteNonQuery()
        cmd.Dispose()
    End Sub

    Sub Deletedata(ByVal UserID As String)
        query = "DELETE FROM User_Accounts WHERE ID = @UserID;DELETE FROM User_Profile WHERE PID = @UserID;
        DECLARE @NextUserID INT; 
        SET @NextUserID = (SELECT COUNT(*) FROM User_Accounts); 
        DBCC CHECKIDENT('User_Accounts', RESEED, @NextUserID);
        DBCC CHECKIDENT('User_Profile', RESEED, @NextUserID);"
        cmd = New SqlCommand(query, sqlconn)
        With cmd.Parameters
            .AddWithValue("@UserID", UserID)
        End With
        cmd.ExecuteNonQuery()
        cmd.Dispose()
    End Sub
    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        If e.ColumnIndex = 8 Then
            Dim i As Integer = DataGridView1.CurrentRow.Index
            TextBox12.Text = DataGridView1.Item(0, i).Value
            TextBox1.Text = DataGridView1.Item(1, i).Value
            TextBox2.Text = DataGridView1.Item(2, i).Value
            TextBox4.Text = DataGridView1.Item(3, i).Value
            TextBox5.Text = DataGridView1.Item(4, i).Value
            TextBox6.Text = DataGridView1.Item(5, i).Value
        ElseIf e.ColumnIndex = 9 Then
            Dim confirm = MsgBox("Do you want to delete this record?", MsgBoxStyle.YesNo)
            If confirm = MsgBoxResult.Yes Then
                Dim i As Integer = DataGridView1.CurrentRow.Index
                Deletedata(DataGridView1.Item(0, i).Value)
                Refresh()
                Readdata()
                Rowheight()
            End If
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim existingID As Boolean = False
        Dim existingname As Boolean = False
        str = "SELECT * FROM User_Profile"
        cmd = New SqlClient.SqlCommand(str, sqlconn)
        dr = cmd.ExecuteReader
        While dr.Read
            If dr("ID").ToString.Equals(TextBox12.Text) Then
                existingID = True
            ElseIf dr("Firstname").ToString.Equals(TextBox4.Text) And dr("Middlename").ToString.Equals(TextBox5.Text) And dr("Lastname").ToString.Equals(TextBox6.Text) Then
                existingname = True
            End If
        End While
        dr.Close()
        cmd.Dispose()
        If existingID = True Then
            MsgBox("ID already exist", MsgBoxStyle.Critical)
        ElseIf existingname = True Then
            MsgBox("Name already exist", MsgBoxStyle.Critical)
        ElseIf TextBox1.Text = "" Then
            MsgBox("Invalid Username", MsgBoxStyle.Critical)
        ElseIf TextBox2.Text = "" Then
            MsgBox("Invalid Password", MsgBoxStyle.Critical)
        ElseIf TextBox3.Text = "" Then
            MsgBox("Invalid to Confirm Password", MsgBoxStyle.Critical)
        ElseIf TextBox2.Text <> TextBox3.Text Then
            MsgBox("Failed to Confirm Password", MsgBoxStyle.Critical)
        ElseIf TextBox4.Text = "" Then
            MsgBox("Invalid Firstname", MsgBoxStyle.Critical)
        ElseIf TextBox5.Text = "" Then
            MsgBox("Invalid Middlename", MsgBoxStyle.Critical)
        ElseIf TextBox6.Text = "" Then
            MsgBox("Invalid Lastname", MsgBoxStyle.Critical)
        Else
            Savedata()
            Readdata()
            Rowheight()
            Refresh()
            Clear()
        End If
    End Sub

    '===============================================================HANDLES============================================================================'
    Private Sub FLEETToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FLEETToolStripMenuItem.Click
        DASHBOARD.Show()
        Me.Close()
    End Sub
    Private Sub DRIVERSPROFILEToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DRIVERSPROFILEToolStripMenuItem.Click
        DRIVERSPROFILE.Show()
        Me.Close()
    End Sub
    Private Sub ACCOUNTSToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ACCOUNTSToolStripMenuItem.Click
        Refresh()
    End Sub
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Updatedata()
        Readdata()
        Rowheight()
        Clear()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Clear()
    End Sub
    '===============================================================HANDLES============================================================================'
End Class
