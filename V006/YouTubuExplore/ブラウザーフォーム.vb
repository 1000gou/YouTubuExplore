Public Class ブラウザーフォーム
    Public 親フォーム As メインフォーム

    Private Sub ブラウザーフォーム_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        WebBrowser1.Navigate("http://www.youtube.com/")
    End Sub

    Protected Overrides ReadOnly Property CreateParams() As  _
        System.Windows.Forms.CreateParams
        <System.Security.Permissions.SecurityPermission( _
            System.Security.Permissions.SecurityAction.LinkDemand, _
            Flags:=System.Security.Permissions.SecurityPermissionFlag.UnmanagedCode)> _
        Get
            Const CS_NOCLOSE As Integer = &H200
            Dim cp As CreateParams = MyBase.CreateParams
            cp.ClassStyle = cp.ClassStyle Or CS_NOCLOSE

            Return cp
        End Get
    End Property

End Class