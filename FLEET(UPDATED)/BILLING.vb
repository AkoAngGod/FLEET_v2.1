Public Class BILLING
    Private Sub DRIVERSPROFILEToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BILLINGToolStripMenuItem.Click
        Refresh()
    End Sub

    Private Sub FLEETToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FLEETToolStripMenuItem.Click
        DASHBOARD.Show()
        Me.Close()
    End Sub

    Private Sub BILLING_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class