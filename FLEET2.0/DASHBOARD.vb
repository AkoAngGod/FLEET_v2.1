Imports System.Data.SqlClient
Imports System.Security.Cryptography

Public Class DASHBOARD
    Private Sub DASHBOARD_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Connect()
        Age = DateTime.Now.Year - Birthday.Year
        If (DateTime.Now < Birthday.AddYears(Age)) Then
            Age -= 1
        End If
        Label1.Text = Fullname
        Label2.Text = Username
        Label3.Text = usrid.ToString()
        Label4.Text = _Address
        Label5.Text = Contact
        Label6.Text = Gender
        Label8.Text = Birthday
        Label9.Text = Role
        Label16.Text = Age
        ViewImage()
    End Sub
    '===============================================================HANDLES============================================================================'
    Private Sub DRIVERSPROFILEToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DRIVERSPROFILEToolStripMenuItem.Click
        DRIVERSPROFILE.Show()
        Me.Close()
    End Sub
    Private Sub ACCOUNTSToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ACCOUNTSToolStripMenuItem.Click
        ACCOUNTS.Show()
        Me.Close()
    End Sub
    Private Sub FLEETToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FLEETToolStripMenuItem.Click
        Refresh()
    End Sub

    Private Sub PROFILEToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PROFILEToolStripMenuItem.Click
        PROFILE.Show()
        Me.Close()
    End Sub
    Private Sub LOGOUTToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LOGOUTToolStripMenuItem.Click
        Logout(usrid)
        LOGIN.Show()
        Me.Close()
    End Sub

    '===============================================================HANDLES============================================================================'
End Class