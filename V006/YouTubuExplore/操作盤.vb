Public Class 操作盤
    '引数として受け取ったメソッド保存用
    Private 親フォーム As メインフォーム
    Public ブラウザ As ブラウザーフォーム

    Public Sub Set親フォーム(ByVal f As メインフォーム)
        親フォーム = f
    End Sub

    Public Sub Setブラウザフォーム(ByVal f As ブラウザーフォーム)
        ブラウザ = f
    End Sub


    Private Sub 操作盤_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        UrlTextBox.Text = "http://www.youtube.com/"
        ステータス.Text = "待機中"
    End Sub

    Private Sub NumericUpDown1_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If Not 親フォーム Is Nothing Then
            親フォーム.Refresh()
        End If
    End Sub

    Private Sub ページのダウンロード_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If Not 親フォーム Is Nothing Then
            親フォーム.AddUrlQueue(UrlTextBox.Text, -1)
            親フォーム.UrlQの先頭アドレスをダウンロード()
        End If
    End Sub


    Private Sub ノードのクリア_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        親フォーム.ノードのクリア()
    End Sub


    'Private Sub 標準ブラウザで開く_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    If Not 親フォーム Is Nothing Then
    '        If Not ブラウザ Is Nothing Then
    '            If False Then 'YouTubeExploerで開く.Checked = True Then
    '                Try
    '                    ブラウザ.Show()
    '                Catch ex As System.ObjectDisposedException
    '                    ブラウザ = New ブラウザーフォーム
    '                    '親フォーム.AddOwnedForm(ブラウザ)
    '                    親フォーム.ブラウザ = ブラウザ
    '                    ブラウザ.Show()
    '                End Try
    '            Else
    '                ブラウザ.Close()
    '            End If
    '        End If
    '    End If
    'End Sub

    'Private Sub ブラウザウィンドウのページを展開_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    If Not 親フォーム Is Nothing Then
    '        Dim url As String
    '        url = ブラウザ.WebBrowser1.Url.ToString()
    '        親フォーム.AddUrlQueue(url, -1)
    '        親フォーム.UrlQの先頭アドレスをダウンロード()
    '    End If
    'End Sub

   
    Private Sub 終了QToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        End
    End Sub

    Private Sub 終了ボタン_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If Not 親フォーム Is Nothing Then
            親フォーム.イニシャルファイルを書き込み()
        End If

        End
    End Sub

    Private Sub ヒントボタン_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim f As ヘルプフォーム
        f = New ヘルプフォーム
        f.Show()
    End Sub

    'Private Sub バージョン情報ボタン_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Dim f As バージョン情報フォーム
    '    f = New バージョン情報フォーム
    '    f.ShowDialog()
    'End Sub

    Private Sub すべてのノードを展開_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles すべてのノードを展開.Click
        If Not 親フォーム Is Nothing Then
            親フォーム.すべてのノードを展開()
        End If
    End Sub

    Private Sub ノード展開をとめる_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ノード展開中止.Click
        If Not 親フォーム Is Nothing Then
            親フォーム.ノード展開Timer.Stop()
        End If
    End Sub

    Private Sub リンクの表示_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles リンクの表示.CheckedChanged
        If Not 親フォーム Is Nothing Then
            親フォーム.Refresh()
        End If
    End Sub

    Private Sub デバッグ用ボタン_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If Not 親フォーム Is Nothing Then

        End If
    End Sub

    Private Sub 展開待ちノードクリア_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 展開待ちノードクリア.Click
        If Not 親フォーム Is Nothing Then
            親フォーム.展開待ノードをクリア()
        End If
    End Sub

    Private Sub 位置計算繰り返し数_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 位置計算繰り返し数.ValueChanged
        If Not 親フォーム Is Nothing Then
            親フォーム.再計算数 = 位置計算繰り返し数.Value
        End If
    End Sub
End Class