Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Xml
Imports System.Xml.Serialization

Public Class メインフォーム
    '描画関連のプロパティ
    Dim イメージ倍率 As Double

    'ノード管理
    Dim UrlQueue As Queue(Of ダウンロードQ)   'ダウンロードするUrlのQueue
    Public ノードリスト As List(Of ページノード)
    Dim 選択中のノード番号 As Integer
    Dim 操作盤フォーム As 操作盤
    Public ブラウザ As ブラウザーフォーム

    '視点
    Dim 視点原点Wx As Integer    'World座標
    Dim 視点原点Wy As Integer    'World座標
    Dim クリックVx As Integer    'View座標
    Dim クリックVy As Integer    'View座標
    Dim オブジェクト領域 As 領域

    'ドラッグ
    Dim マウスドラッグ中 As Boolean
    Dim マウスドラッグ開始点 As Point
    Dim マウスドラッグ開始時の視点原点 As Point

    Dim ノード移動中 As Boolean
    Dim ノード移動開始位置 As Point
    Dim ノード移動開始クリック位置 As Point


    Dim 乱数生成器 As Random
    Dim 前回のツールチップ As Integer

    Public 再計算数 As Integer

    Dim タイトル描画 As Boolean

    
    '----------------------------------------------------------------------------------------------
    '   初期化
    '----------------------------------------------------------------------------------------------
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        再計算数 = 50

        ノードタイトル = New ToolTip
        前回のツールチップ = -1

        マウスドラッグ中 = False
        マウスドラッグ開始点 = New Point(0, 0)
        マウスドラッグ開始時の視点原点 = New Point(0, 0)

        ノード移動中 = False
        ノード移動開始位置 = New Point(0, 0)
        ノード移動開始クリック位置 = New Point(0, 0)

        乱数生成器 = New Random(0)

        選択中のノード番号 = 0


        タイトル描画 = True

        視点原点Wx = 0
        視点原点Wy = 0
        オブジェクト領域 = New 領域

        UrlQueue = New Queue(Of ダウンロードQ)
        ノードリスト = New List(Of ページノード)

        'ダブルバッファの設定
        Me.SetStyle(ControlStyles.ResizeRedraw, True)
        Me.SetStyle(ControlStyles.DoubleBuffer, True)
        Me.SetStyle(ControlStyles.UserPaint, True)
        Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)

        ブラウザ = New ブラウザーフォーム
        イメージ倍率 = 0.5

        操作盤フォーム = New 操作盤
        操作盤フォーム.Set親フォーム(Me)
        操作盤フォーム.Show()
        操作盤フォーム.TopMost = True
        操作盤フォーム.Visible = False
        操作盤フォーム.位置計算繰り返し数.Value = 再計算数

        'タイマーの設定
        ノード展開Timer = New Timer()
        ノード展開Timer.Interval = 1500

        イニシャルファイルを読み込み初期値をセット()
    End Sub

    Public Sub イニシャルファイルを読み込み初期値をセット()
        Dim sr As System.IO.StreamReader
        '------イニシャルファイルを開き文字列へ保存
        Try
            sr = New System.IO.StreamReader(".\YouTubeExplorer.ini", System.Text.Encoding.GetEncoding("shift_jis"))
        Catch ex As System.Exception
            Return
        End Try

        Dim テキスト As String
        Dim 行 As String
        テキスト = sr.ReadToEnd()
        '閉じる
        sr.Close()

        '-------一行ずつハッシュテーブルに保存する--------------
        Dim rs As New System.IO.StringReader(テキスト)
        Dim tmp As Object
        Dim ht As Hashtable = New Hashtable
        'ストリームの末端まで繰り返す
        While rs.Peek() > -1
            '一行読み込んで表示する
            行 = rs.ReadLine()
            tmp = Split(行, "=")
            ht(tmp(0)) = tmp(1)
        End While
        rs.Close()

        '------ハッシュテーブルに保存した値をセットする----------
        Width = CInt(ht("メインウィンドウ幅"))
        Height = CInt(ht("メインウィンドウ高"))
        Top = CInt(ht("メインウィンドウTop"))
        Left = CInt(ht("メインウィンドウLeft"))
        操作盤フォーム.Width = CInt(ht("操作盤ウィンドウ幅"))
        操作盤フォーム.Height = CInt(ht("操作盤ウィンドウ高"))
        操作盤フォーム.Top = CInt(ht("操作盤ウィンドウTop"))
        操作盤フォーム.Left = CInt(ht("操作盤ウィンドウLeft"))
    End Sub

    '------------------ ----------------------------------------------------------------------------
    '   終了前処理
    '----------------------------------------------------------------------------------------------
    Private Sub メインフォーム_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        イニシャルファイルを書き込み()
    End Sub

    Public Sub イニシャルファイルを書き込み()
        'Shift JISで書き込む
        '書き込むファイルが既に存在している場合は、上書きする
        Dim sw As New System.IO.StreamWriter(".\YouTubeExplorer.ini", False, System.Text.Encoding.GetEncoding("shift_jis"))
        'イニシャルファイルへ書き込む
        If Me.WindowState <> FormWindowState.Maximized And Me.WindowState <> FormWindowState.Minimized Then
            sw.Write("メインウィンドウ幅=" & CStr(Width) & vbCrLf)
            sw.Write("メインウィンドウ高=" & CStr(Height) & vbCrLf)
            sw.Write("メインウィンドウTop=" & CStr(Me.Top) & vbCrLf)
            sw.Write("メインウィンドウLeft=" & CStr(Me.Left) & vbCrLf)
            sw.Write("操作盤ウィンドウ幅=" & CStr(操作盤フォーム.Width) & vbCrLf)
            sw.Write("操作盤ウィンドウ高=" & CStr(操作盤フォーム.Height) & vbCrLf)
            sw.Write("操作盤ウィンドウTop=" & CStr(操作盤フォーム.Top) & vbCrLf)
            sw.Write("操作盤ウィンドウLeft=" & CStr(操作盤フォーム.Left) & vbCrLf)
        Else
            sw.Write("メインウィンドウ幅=" & CStr(500) & vbCrLf)
            sw.Write("メインウィンドウ高=" & CStr(500) & vbCrLf)
            sw.Write("メインウィンドウTop=" & CStr(0) & vbCrLf)
            sw.Write("メインウィンドウLeft=" & CStr(0) & vbCrLf)
            sw.Write("操作盤ウィンドウ幅=" & CStr(375) & vbCrLf)
            sw.Write("操作盤ウィンドウ高=" & CStr(175) & vbCrLf)
            sw.Write("操作盤ウィンドウTop=" & CStr(300) & vbCrLf)
            sw.Write("操作盤ウィンドウLeft=" & CStr(100) & vbCrLf)
        End If

        '閉じる
        sw.Close()
    End Sub

    '----------------------------------------------------------------------------------------------
    '   座標変換
    '----------------------------------------------------------------------------------------------
    Private Sub ワールド座標からView座標へ変換(ByVal ワールド座標x, ByVal ワールド座標y, ByRef View座標x, ByRef View座標y)
        View座標x = (ワールド座標x - 視点原点Wx) * イメージ倍率
        View座標y = (ワールド座標y - 視点原点Wy) * イメージ倍率
    End Sub

    Private Sub View座標からワールド座標へ変換(ByVal View座標x, ByVal View座標y, ByRef ワールド座標x, ByRef ワールド座標y)
        ワールド座標x = View座標x / イメージ倍率 + 視点原点Wx
        ワールド座標y = View座標y / イメージ倍率 + 視点原点Wy
    End Sub

    Public Sub Set視点(ByVal x As Integer, ByVal y As Integer)
        視点原点Wx = x
        視点原点Wy = y
        '再描画
        Me.Refresh()
    End Sub

    '----------------------------------------------------------------------------------------------
    '   描画関連
    '----------------------------------------------------------------------------------------------
    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        MyBase.OnPaint(e)
        ノードの描画(e)
    End Sub

    Private Sub ノードの描画(ByVal e As PaintEventArgs)
        '------デバッグ描画用フラグ : True にすると描画を行う------
        Dim debug_draw As Boolean
        debug_draw = False


        Dim i, j As Integer                             'ループ用の変数
        Dim pic As Image                                'イメージ変数
        Dim fnt As New Font("MS UI Gothic", 11)          'フォントオブジェクトの作成

        '--------------------------リンクの表示-------------------------------------
        For i = 0 To ノードリスト.Count - 1
            pic = ノードリスト(i).イメージ
            Dim x As Integer
            Dim y As Integer
            Dim 親x As Integer
            Dim 親y As Integer
            Dim P As Point

            P = ノードリスト(i).GetWorldPos中心()
            ワールド座標からView座標へ変換(P.X, P.Y, x, y)

            If 操作盤フォーム.リンクの表示.Checked = True Then
                '親ノードへの線を描画
                If ノードリスト(i).親ノード番号リスト.Count <> 0 Then
                    For j = 0 To ノードリスト(i).親ノード番号リスト.Count - 1
                        Dim 親ノード番号 As Integer
                        親ノード番号 = ノードリスト(i).親ノード番号リスト(j)
                        P = ノードリスト(親ノード番号).GetWorldPos中心()
                        ワールド座標からView座標へ変換(P.X, P.Y, 親x, 親y)
                        e.Graphics.DrawLine(Pens.Black, x, y, 親x, 親y)               'アイコンの描画
                    Next
                End If
            End If

            '---------------------------デバッグ用の描画--------------------------
            If debug_draw = True Then
                e.Graphics.DrawString(ノードリスト(i).world_pos_x & ", " & ノードリスト(i).world_pos_y & ", " & ノードリスト(i).world_pos_x + ノードリスト(i).イメージ.Width & ", " & ノードリスト(i).world_pos_y + ノードリスト(i).イメージ.Height, fnt, Brushes.Red, x, y - 10)
                e.Graphics.DrawString(ノードリスト(i).ページUrl, fnt, Brushes.Blue, x + CInt(pic.Width * イメージ倍率) + 25, y)
            End If
        Next

        '------------------------------イメージ（アイコン）の描画----------------------
        For i = 0 To ノードリスト.Count - 1
            pic = ノードリスト(i).イメージ
            Dim x As Integer
            Dim y As Integer

            ワールド座標からView座標へ変換(ノードリスト(i).world_pos_x, ノードリスト(i).world_pos_y, x, y)
            'イメージの描画
            e.Graphics.DrawImage(pic, x, y, CInt(pic.Width * イメージ倍率), CInt(pic.Height * イメージ倍率))
        Next

        '----------------------タイトルの描画-----------------------------------------
        'StringFormatオブジェクトの作成
        Dim sf As New StringFormat
        'Dim stringSize As SizeF

        If タイトル描画 And イメージ倍率 > 0.5 Then
            For i = 0 To ノードリスト.Count - 1
                pic = ノードリスト(i).イメージ
                Dim x As Integer
                Dim y As Integer

                ワールド座標からView座標へ変換(ノードリスト(i).world_pos_x, ノードリスト(i).world_pos_y, x, y)

                Dim 描画範囲 As New RectangleF(x, y - 30, CInt(pic.Width * イメージ倍率), 30)

                '文字の背景を塗りつぶす
                'stringSize = e.Graphics.MeasureString(ノードリスト(i).タイトル, fnt, 1000, sf)
                e.Graphics.FillRectangle(Brushes.LightGray, 描画範囲)

                'タイトルの描画
                e.Graphics.DrawString(ノードリスト(i).タイトル, fnt, Brushes.Blue, 描画範囲)
            Next
        End If


        '-----------------------デバッグのための描画--------------------------------
        If debug_draw = True Then
            Dim x As Integer
            Dim y As Integer
            Dim tmp As Integer
            tmp = 600
            e.Graphics.DrawString("選択中のノード : " & 選択中のノード番号, fnt, Brushes.Blue, tmp, 20)
            e.Graphics.DrawString("視点原点Wx(ワールド座標) : " & 視点原点Wx, fnt, Brushes.Blue, tmp, 40)
            e.Graphics.DrawString("視点原点Wy(ワールド座標) : " & 視点原点Wy, fnt, Brushes.Blue, tmp, 60)
            e.Graphics.DrawString("クリックVx(View座標) : " & クリックVx, fnt, Brushes.Blue, tmp, 80)
            e.Graphics.DrawString("クリックVy(View座標) : " & クリックVy, fnt, Brushes.Blue, tmp, 100)
            View座標からワールド座標へ変換(クリックVx, クリックVy, x, y)
            e.Graphics.DrawString("クリックVx(ワールド座標) : " & x, fnt, Brushes.Blue, tmp, 120)
            e.Graphics.DrawString("クリックVy(ワールド座標) : " & y, fnt, Brushes.Blue, tmp, 140)
            e.Graphics.DrawString("マウスドラッグ開始点x(View座標) : " & マウスドラッグ開始点.X, fnt, Brushes.Blue, tmp, 160)
            e.Graphics.DrawString("マウスドラッグ開始点y(View座標) : " & マウスドラッグ開始点.Y, fnt, Brushes.Blue, tmp, 180)
            For i = 0 To UrlQueue.Count - 1
                e.Graphics.DrawString(UrlQueue(i).url, fnt, Brushes.Blue, tmp, i * 20 + 200)
            Next
        End If

        'リソースを開放する
        fnt.Dispose()
    End Sub

    '----------------------------------------------------------------------------------------------
    '       ボタンに対する処理
    '----------------------------------------------------------------------------------------------
    Public Sub Setイメージ倍率(ByVal d As Double)
        イメージ倍率 = d

        Setオブジェクト領域()
    End Sub

    Private Sub 読み込みボタン_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 読み込みボタン.Click
        Dim s As String
        s = テキストボックスURL.Text
        Dim r As System.Text.RegularExpressions.Regex
        Dim m As System.Text.RegularExpressions.Match
        r = New System.Text.RegularExpressions.Regex("http", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        m = r.Match(s)

        If Not m.Success Then
            s = "http://www.youtube.com/results?search_query=" + s + "&aq=f"
        End If

        AddUrlQueue(s, -1)
        UrlQの先頭アドレスをダウンロード()
    End Sub

    Private Sub クリアボタン_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles クリアボタン.Click
        ノードのクリア()
    End Sub

    Private Sub 操作盤表示_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles 操作盤表示.Click
        操作盤フォーム.Visible = Not 操作盤フォーム.Visible
    End Sub

    Private Sub タイトル表示_非表示_Click(sender As Object, e As EventArgs) Handles タイトル表示_非表示.Click
        If タイトル描画 = True Then
            タイトル描画 = False
        Else
            タイトル描画 = True
        End If

        '再描画
        Me.Refresh()
    End Sub

    '----------------------------------------------------------------------------------------------
    '　　　マウス操作に対する処理
    '----------------------------------------------------------------------------------------------
    Private Sub メインフォーム_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseWheel
        Dim カーソル_world_x As Integer
        Dim カーソル_world_y As Integer


        クリックVx = e.X
        クリックVy = e.Y
        View座標からワールド座標へ変換(e.X, e.Y, カーソル_world_x, カーソル_world_y)

        Dim 変更前のイメージ倍率 As Double
        Dim 変更後のイメージ倍率 As Double
        Dim 変更前の視点原点Wx As Integer
        Dim 変更前の視点原点Wy As Integer
        変更前の視点原点Wx = 視点原点Wx
        変更前の視点原点Wy = 視点原点Wy
        変更前のイメージ倍率 = イメージ倍率

        If e.Delta > 0 Then
            変更後のイメージ倍率 = 変更前のイメージ倍率 * Math.Sqrt(2)
            If 変更後のイメージ倍率 > 2 Then
                変更後のイメージ倍率 = 2
            End If
        Else
            変更後のイメージ倍率 = 変更前のイメージ倍率 / Math.Sqrt(2)
            If 変更後のイメージ倍率 < 0.01 Then
                変更後のイメージ倍率 = 0.01
            End If
        End If


        Dim 変更後の視点原点Wx As Integer
        Dim 変更後の視点原点Wy As Integer
        変更後の視点原点Wx = (カーソル_world_x * (変更後のイメージ倍率 - 変更前のイメージ倍率) + 変更前のイメージ倍率 * 変更前の視点原点Wx) / 変更後のイメージ倍率
        変更後の視点原点Wy = (カーソル_world_y * (変更後のイメージ倍率 - 変更前のイメージ倍率) + 変更前のイメージ倍率 * 変更前の視点原点Wy) / 変更後のイメージ倍率


        視点原点Wx = 変更後の視点原点Wx
        視点原点Wy = 変更後の視点原点Wy

        Dim 確認用Vx As Integer
        Dim 確認用Vy As Integer
        ワールド座標からView座標へ変換(カーソル_world_x, カーソル_world_y, 確認用Vx, 確認用Vy)

        イメージ倍率 = 変更後のイメージ倍率
        '再描画
        Me.Refresh()
    End Sub

    Private Sub メインフォーム_DoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.DoubleClick
        Dim i, j As Integer
        Dim x, y As Integer

        クリックVx = e.X
        クリックVy = e.Y
        View座標からワールド座標へ変換(e.X, e.Y, x, y)

        For i = 0 To ノードリスト.Count - 1
            j = ノードリスト.Count - 1 - i
            If ノードリスト(j).クリック判定(x, y) Then
                選択中のノード番号 = j
                ノードが中心になるようにViewを移動する(ノードリスト(選択中のノード番号))
                Me.Refresh()
                ノード移動中 = False
                AddUrlQueue(ノードリスト(選択中のノード番号).ページUrl, 選択中のノード番号)
                ノードリスト(選択中のノード番号).展開済み = True
                UrlQの先頭アドレスをダウンロード()
                Exit Sub
            End If
        Next
    End Sub

    Public Sub ノードが中心になるようにViewを移動する(ByRef ノード As ページノード)
        視点原点Wx = ノード.world_pos_x - (Me.Width - 40) / イメージ倍率 / 2
        視点原点Wy = ノード.world_pos_y - (Me.Height - 65) / イメージ倍率 / 2
    End Sub

    Private Sub メインフォーム_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown
        Select Case e.Button
            Case Windows.Forms.MouseButtons.Left '左クリック
                Dim i, j As Integer
                Dim x, y As Integer

                クリックVx = e.X
                クリックVy = e.Y
                View座標からワールド座標へ変換(e.X, e.Y, x, y)

                For i = 0 To ノードリスト.Count - 1
                    j = ノードリスト.Count - 1 - i
                    If ノードリスト(j).クリック判定(x, y) Then
                        選択中のノード番号 = j
                        操作盤フォーム.UrlTextBox.Text = ノードリスト(j).ページUrl
                        ワールド座標からView座標へ変換(ノードリスト(j).world_pos_x, ノードリスト(j).world_pos_y, x, y)
                        ノードタイトル.Show(ノードリスト(j).タイトル, Me, x, y - 10, 3000)
                        ノード移動中 = True
                        ノード移動開始位置.X = ノードリスト(j).world_pos_x
                        ノード移動開始位置.Y = ノードリスト(j).world_pos_y
                        View座標からワールド座標へ変換(e.X, e.Y, ノード移動開始クリック位置.X, ノード移動開始クリック位置.Y)
                        '再描画
                        Me.Refresh()
                        Exit Sub
                    End If
                Next

                'ノードが選択されていない場合
                マウスドラッグ中 = True
                マウスドラッグ開始点.X = e.X
                マウスドラッグ開始点.Y = e.Y
                マウスドラッグ開始時の視点原点.X = 視点原点Wx
                マウスドラッグ開始時の視点原点.Y = 視点原点Wy



            Case Windows.Forms.MouseButtons.Right '右クリック
                Dim i, j As Integer
                Dim x, y As Integer
                View座標からワールド座標へ変換(e.X, e.Y, x, y)

                For i = 0 To ノードリスト.Count - 1
                    j = ノードリスト.Count - 1 - i
                    If ノードリスト(j).クリック判定(x, y) Then
                        選択中のノード番号 = j
                        If False Then '操作盤フォーム.YouTubeExploerで開く.Checked = True Then
                            Try
                                ブラウザ.WebBrowser1.Navigate(ノードリスト(選択中のノード番号).ページUrl)
                            Catch ex As System.ObjectDisposedException
                                ブラウザ = New ブラウザーフォーム
                                操作盤フォーム.ブラウザ = ブラウザ
                                ブラウザ.Show()
                            End Try

                        Else
                            System.Diagnostics.Process.Start(ノードリスト(選択中のノード番号).ページUrl)
                        End If

                        Exit Sub
                    End If
                Next
        End Select

        '再描画
        Me.Refresh()
    End Sub

    Private Sub メインフォーム_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseMove
        Dim x, y As Integer
        Dim i, j As Integer

        If マウスドラッグ中 = True Then
            クリックVx = e.X
            クリックVy = e.Y
            視点原点Wx = マウスドラッグ開始時の視点原点.X + (マウスドラッグ開始点.X - e.X) / イメージ倍率
            視点原点Wy = マウスドラッグ開始時の視点原点.Y + (マウスドラッグ開始点.Y - e.Y) / イメージ倍率
            '再描画
            Me.Refresh()
            Exit Sub
        End If

        If ノード移動中 = True Then
            View座標からワールド座標へ変換(e.X, e.Y, x, y)
            ノードリスト(選択中のノード番号).world_pos_x = ノード移動開始位置.X - ノード移動開始クリック位置.X + x
            ノードリスト(選択中のノード番号).world_pos_y = ノード移動開始位置.Y - ノード移動開始クリック位置.Y + y
            '再描画
            Me.Refresh()
            Exit Sub
        End If

        '------------ツールチップにタイトルの表示-----------------
        View座標からワールド座標へ変換(e.X, e.Y, x, y)
        For i = 0 To ノードリスト.Count - 1
            j = ノードリスト.Count - 1 - i
            If ノードリスト(j).クリック判定(x, y) Then
                If 前回のツールチップ <> j Then
                    ワールド座標からView座標へ変換(ノードリスト(j).world_pos_x, ノードリスト(j).world_pos_y, x, y)
                    ノードタイトル.Show(ノードリスト(j).タイトル, Me, x, y - 10, 3000)
                    前回のツールチップ = j
                End If
                Exit Sub
            End If
        Next
    End Sub

    Private Sub メインフォーム_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseUp
        If マウスドラッグ中 = True Then
            クリックVx = e.X
            クリックVy = e.Y
            視点原点Wx = マウスドラッグ開始時の視点原点.X + (マウスドラッグ開始点.X - e.X) / イメージ倍率
            視点原点Wy = マウスドラッグ開始時の視点原点.Y + (マウスドラッグ開始点.Y - e.Y) / イメージ倍率
            マウスドラッグ中 = False
            '再描画
            Me.Refresh()
        End If

        Dim x, y As Integer
        If ノード移動中 = True Then
            View座標からワールド座標へ変換(e.X, e.Y, x, y)
            ノードリスト(選択中のノード番号).world_pos_x = ノード移動開始位置.X - ノード移動開始クリック位置.X + x
            ノードリスト(選択中のノード番号).world_pos_y = ノード移動開始位置.Y - ノード移動開始クリック位置.Y + y
            ノード移動中 = False
            '再描画
            Me.Refresh()
            Exit Sub
        End If
    End Sub

    '----------------------------------------------------------------------------------------------
    '　　　HTMLブラウザからファイルを文字列で取得する等
    '----------------------------------------------------------------------------------------------
    Public Function GetHTMLウェッブブラウザから取得する(ByVal url As String) As String
        Return "" 'ブラウザ.GetHtml(url)
    End Function

    Public Function GetHTMLファイル(ByVal url As String) As String
        操作盤フォーム.ステータス.Text = "取得中:" & url
        操作盤フォーム.Refresh()

        'WebClientの作成
        Dim wc As New System.Net.WebClient()
        '文字コードを指定
        wc.Encoding = System.Text.Encoding.GetEncoding("utf-8")
        Dim ソース As String
        Try
            'HTMLソースをダウンロードする
            ソース = wc.DownloadString(url)
        Catch ex As Exception 'System.Net.WebException
            ソース = ""
            操作盤フォーム.ステータス.Text = "エラー: " & ex.Message
            操作盤フォーム.Refresh()

            '後始末
            wc.Dispose()
            GetHTMLファイル = ソース
            Exit Function
        End Try


        '後始末
        wc.Dispose()

        操作盤フォーム.ステータス.Text = "取得完了:" & url
        操作盤フォーム.Refresh()


        GetHTMLファイル = ソース
    End Function

    '-------------Urlキューにアドレスを追加----------------------
    Public Sub AddUrlQueue(ByVal Url As String, ByVal 親ノード番号 As Integer)
        UrlQueue.Enqueue(New ダウンロードQ(Url, 親ノード番号))
        '再描画
        Me.Refresh()
    End Sub

    Public Sub すべてのノードを展開()
        If ノードリスト.Count = 0 Then
            MsgBox("展開できるノードがありません")
            Return
        End If

        If UrlQueue.Count = 0 Then
            For i = 0 To ノードリスト.Count - 1
                If ノードリスト(i).展開済み = False Then
                    AddUrlQueue(ノードリスト(i).ページUrl, ノードリスト(i).ノード番号)
                    ノードリスト(i).展開済み = True
                End If
            Next
        End If

        操作盤フォーム.残りノード数.Text = "展開待ちノード数 : " & UrlQueue.Count

        If UrlQueue.Count > 0 Then
            UrlQの先頭アドレスをダウンロード()
            操作盤フォーム.残りノード数.Text = "展開待ちノード数 : " & UrlQueue.Count
        End If
        ノード展開Timer.Start()
    End Sub

    Public Sub 展開待ノードをクリア()
        ノード展開Timer.Stop()
        UrlQueue.Clear()
        操作盤フォーム.残りノード数.Text = "展開待ちノード数 : " & UrlQueue.Count
    End Sub

    Private Sub ノード展開Timer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ノード展開Timer.Tick
        If UrlQueue.Count > 0 Then
            UrlQの先頭アドレスをダウンロード()
            操作盤フォーム.残りノード数.Text = "展開残りノード数 : " & UrlQueue.Count
        Else
            ノード展開Timer.Stop()
            操作盤フォーム.残りノード数.Text = "展開残りノード数 : " & UrlQueue.Count
        End If
    End Sub

    '-----------------------------------------------------------------------------------
    '-- UrlQの展開
    '-----------------------------------------------------------------------------------
    Public Sub UrlQの先頭アドレスをダウンロード()
        Dim ソース As String
        Dim dQ As ダウンロードQ

        Dim LinkList As List(Of String)
        LinkList = New List(Of String)

        Dim TitleList As List(Of String)
        TitleList = New List(Of String)

        Dim ImageUrlList As List(Of String)
        ImageUrlList = New List(Of String)

        Dim ノード As ページノード


        If UrlQueue.Count > 0 Then
            dQ = UrlQueue.Dequeue()

            ソース = GetHTMLファイル(dQ.url)
            dQ.親ノード番号 = 最初のノード作成(dQ.url, ソース)

            Dim r As System.Text.RegularExpressions.Regex
            Dim m As System.Text.RegularExpressions.Match

            '--------検索ページからリンクの抽出------------------
            r = New System.Text.RegularExpressions.Regex("search_query", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            m = r.Match(dQ.url)
            If m.Success Then
                Youtubeの検索ソースからリンクとタイトルを抽出(LinkList, TitleList, ソース)
            Else
                '--------------通常ページからリンクの抽出------------
                r = New System.Text.RegularExpressions.Regex("watch", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                m = r.Match(dQ.url)
                If m.Success Then
                    Youtubeの通常ソースからリンクとタイトルを抽出(LinkList, TitleList, ソース)
                Else
                    '--------------トップページからのリンクの抽出------------
                    Youtubeのトップソースからリンクとタイトルを抽出(LinkList, TitleList, ソース)
                End If
            End If



            For i = 0 To LinkList.Count - 1
                Dim left As Integer = InStr(LinkList(i), "=")
                Dim right As Integer = InStr(LinkList(i), "&")
                If right <> 0 Then
                    ImageUrlList.Add("http://i4.ytimg.com/vi/" & Mid(LinkList(i), left + 1, right - 1 - left) & "/default.jpg")
                Else
                    ImageUrlList.Add("http://i4.ytimg.com/vi/" & Mid(LinkList(i), left + 1) & "/default.jpg")
                End If
            Next


            'ノードの作成
            For i = 0 To LinkList.Count - 1
                ノード = New ページノード
                ノード.ページUrl = LinkList(i)
                ノード.タイトル = TitleList(i)
                ノード.イメージUrl = ImageUrlList(i)

                '画像のロード
                ノード.LoadImage()
                ノード.ノード番号 = ノードリスト.Count
                ノード.親ノードを追加(dQ.親ノード番号)

                'ノードの追加
                If ノードの追加(ノード, dQ.親ノード番号) = True Then
                    'ノードの重複がなかった場合
                    If dQ.親ノード番号 <> -1 Then
                        ノードリスト(dQ.親ノード番号).親ノードを追加(ノード.ノード番号)  '相方向リストにする
                    End If
                End If
            Next
        End If
        For j = 0 To 再計算数
            ノードの位置調整()
        Next
    End Sub
    '----------------------------------------------------------------------------------------------
    '　　　最初のノード作成
    '----------------------------------------------------------------------------------------------
    Public Function 最初のノード作成(ByVal url As String, ByVal ソース As String) As Integer
        '既にノードがある場合はノード番号を返す
        If ノードリスト.Count <> 0 Then
            Dim i As Integer
            For i = 0 To ノードリスト.Count - 1
                If ノードリスト(i).ページUrl = url Then
                    ノードが中心になるようにViewを移動する(ノードリスト(i))
                    Return ノードリスト(i).ノード番号
                End If
            Next
        End If

        Dim ノード As ページノード
        ノード = New ページノード
        ノード.ページUrl = url
        ノード.タイトル = YouTubeよりタイトルを取得する(ソース)

        Dim r As System.Text.RegularExpressions.Regex
        Dim m As System.Text.RegularExpressions.Match

        '検索ページ
        r = New System.Text.RegularExpressions.Regex("search_query", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        m = r.Match(url)
        If m.Success Then
            ノード.イメージUrl = "./検索.jpg"
        Else
            '通常ページ
            r = New System.Text.RegularExpressions.Regex("watch", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            m = r.Match(url)
            If m.Success Then
                '通常ページ
                Dim left As Integer = InStr(ノード.ページUrl, "=")
                Dim right As Integer = InStr(ノード.ページUrl, "&")
                If right <> 0 Then
                    ノード.イメージUrl = "http://i4.ytimg.com/vi/" & Mid(ノード.ページUrl, left + 1, right - 1 - left) & "/default.jpg"
                Else
                    ノード.イメージUrl = "http://i4.ytimg.com/vi/" & Mid(ノード.ページUrl, left + 1) & "/default.jpg"
                End If
            Else
                'トップページ
                ノード.イメージUrl = "./トップページ.jpg"
            End If
        End If



        '画像のロード
        ノード.展開済み = True
        ノード.LoadImage()
        ノード.ノード番号 = ノードリスト.Count
        ノード.親ノードを追加(-1)
        ノードの追加(ノード, -1)


        Return ノード.ノード番号
    End Function

    '----------------------------------------------------------------------------------------------
    '　　　YouTubeソースの読み込みとノード作成
    '----------------------------------------------------------------------------------------------
    Public Sub Youtubeの検索ソースからリンクとタイトルを抽出(ByRef LinkList As List(Of String), ByRef TitleList As List(Of String), ByVal ソース As String)
        Dim r As System.Text.RegularExpressions.Regex
        Dim m As System.Text.RegularExpressions.Match
        Dim r2 As System.Text.RegularExpressions.Regex
        Dim m2 As System.Text.RegularExpressions.Match
        Dim r3 As System.Text.RegularExpressions.Regex
        Dim m3 As System.Text.RegularExpressions.Match
        Dim url As String
        Dim title As String


        '---------検索ソースよりリンクの抽出----------
        r = New System.Text.RegularExpressions.Regex("href=""/watch.*?title="".*?""", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        m = r.Match(ソース)
        While m.Success
            r2 = New System.Text.RegularExpressions.Regex("href=""/watch.*?""", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            m2 = r2.Match(m.Value)
            url = ""
            If m2.Success Then
                url = "http://www.youtube.com" & Mid(m2.Value, 7)
                url = Microsoft.VisualBasic.Left(url, Len(url) - 1)
            End If

            r3 = New System.Text.RegularExpressions.Regex("title="".*?""", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            m3 = r3.Match(m.Value)
            title = m3.Value
            If m3.Success Then
                title = Mid(m3.Value, 8)
                title = Microsoft.VisualBasic.Left(title, Len(title) - 1)
            End If

            If m2.Success And m3.Success And title <> "" And url <> "" Then
                LinkList.Add(url)
                TitleList.Add(title)
            End If
            m = m.NextMatch()
        End While
    End Sub

    Public Sub Youtubeの通常ソースからリンクとタイトルを抽出(ByRef LinkList As List(Of String), ByRef TitleList As List(Of String), ByVal ソース As String)
        Dim r As System.Text.RegularExpressions.Regex
        Dim m As System.Text.RegularExpressions.Match
        Dim r2 As System.Text.RegularExpressions.Regex
        Dim m2 As System.Text.RegularExpressions.Match
        Dim r3 As System.Text.RegularExpressions.Regex
        Dim m3 As System.Text.RegularExpressions.Match
        Dim url As String
        Dim title As String


        '---------通常ソースよりリンクの抽出----------
        r = New System.Text.RegularExpressions.Regex("<li class=""video-list-item related-list-item show-video-time"">.*?</li>", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        ソース = ソース.Replace(Chr(10), "")
        m = r.Match(ソース)
        While m.Success
            '-------動画へのリンクの抽出------
            r2 = New System.Text.RegularExpressions.Regex("href=""/watch.*?""", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            m2 = r2.Match(m.Value)
            url = ""
            If m2.Success Then
                url = "http://www.youtube.com" & Mid(m2.Value, 7)
                url = Microsoft.VisualBasic.Left(url, Len(url) - 1)
            End If
            '-------タイトルの抽出------
            'r3 = New System.Text.RegularExpressions.Regex("class=""title"" title="".*?""", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            r3 = New System.Text.RegularExpressions.Regex("title="".*""", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            m3 = r3.Match(m.Value)
            title = m3.Value
            If m3.Success Then
                title = Mid(m3.Value, 8)
                Dim i As Integer
                i = title.IndexOf("""")
                title = Microsoft.VisualBasic.Left(title, i)
            End If


            If m2.Success And m3.Success And title <> "" And url <> "" Then
                LinkList.Add(url)
                TitleList.Add(title)
            End If
            m = m.NextMatch()
        End While
    End Sub

    Public Sub Youtubeのトップソースからリンクとタイトルを抽出(ByRef LinkList As List(Of String), ByRef TitleList As List(Of String), ByVal ソース As String)
        Dim r As System.Text.RegularExpressions.Regex
        Dim m As System.Text.RegularExpressions.Match
        Dim r2 As System.Text.RegularExpressions.Regex
        Dim m2 As System.Text.RegularExpressions.Match
        Dim r3 As System.Text.RegularExpressions.Regex
        Dim m3 As System.Text.RegularExpressions.Match
        Dim url As String
        Dim title As String


        '---------トップソースよりリンクの抽出----------
        r = New System.Text.RegularExpressions.Regex("href=""/watch.*?</a>", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        m = r.Match(ソース)
        While m.Success
            r2 = New System.Text.RegularExpressions.Regex("href=""/watch.*?""", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            m2 = r2.Match(m.Value)
            url = ""
            If m2.Success Then
                url = "http://www.youtube.com" & Mid(m2.Value, 7)
                url = Microsoft.VisualBasic.Left(url, Len(url) - 1)
            End If

            r3 = New System.Text.RegularExpressions.Regex(">.*?<", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
            m3 = r3.Match(m.Value)
            title = m3.Value
            If m3.Success Then
                title = Mid(m3.Value, 2)
                title = Microsoft.VisualBasic.Left(title, Len(title) - 1)
            End If

            If m2.Success And m3.Success And title <> "" And url <> "" Then
                LinkList.Add(url)
                TitleList.Add(title)
            End If
            m = m.NextMatch()
        End While
    End Sub

    Public Function YouTubeよりタイトルを取得する(ByVal ソース As String) As String
        Dim r As System.Text.RegularExpressions.Regex
        Dim m As System.Text.RegularExpressions.Match

        '---------タイトルの抽出(正規表現を使っている)----------
        ソース = ソース.Replace(Chr(10), "") '.Replace("\n", "")
        r = New System.Text.RegularExpressions.Regex("<title>.*</title>", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        m = r.Match(ソース)

        '---------<title>*</title>から*のみを取り出す-----------
        Dim tmp As String = m.Value
        Dim i As Integer = tmp.IndexOf(">")
        tmp = tmp.Substring(i + 1)
        i = tmp.IndexOf("<")
        tmp = tmp.Substring(0, i)

        Return tmp
    End Function

    '----------------------------------------------------------------------------------------------
    '　　　YouTubeソースの読み込み (未使用関数)
    '----------------------------------------------------------------------------------------------

    'Public Sub Youtubeのソースからリンクを抽出(ByRef LinkList As List(Of String), ByVal ソース As String)
    '    Dim r As System.Text.RegularExpressions.Regex
    '    Dim m As System.Text.RegularExpressions.Match
    '    Dim url As String
    '    '---------リンクの抽出----------
    '    r = New System.Text.RegularExpressions.Regex("a href=""/watch.*?""", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
    '    m = r.Match(ソース)

    '    While m.Success
    '        url = "http://www.youtube.com" & Mid(m.Value, 9)
    '        url = Microsoft.VisualBasic.Left(url, Len(url) - 1)
    '        LinkList.Add(url)
    '        m = m.NextMatch()
    '    End While
    'End Sub



    '----------------------------------------------------------------------------------------------
    '　　　ノード関連の追加
    '----------------------------------------------------------------------------------------------
    Public Function ノードの追加(ByVal ノード As ページノード, ByVal 親ノード番号 As Integer) As Boolean
        'ノードに重複がないか調べる
        If ノードリスト.Count <> 0 Then
            Dim i As Integer
            For i = 0 To ノードリスト.Count - 1
                If ノードリスト(i).ページUrl = ノード.ページUrl Then
                    ノードリスト(i).親ノードを追加(親ノード番号)
                    '近くにノードが追加されたら計算回数を初期化する
                    For j = 0 To ノードリスト.Count - 1
                        ノードリスト(j).位置計算回数初期化(ノードリスト(親ノード番号).world_pos_x, ノードリスト(親ノード番号).world_pos_y)
                    Next
                    '再描画
                    Return False
                End If
            Next
        End If


        'まずはランダムに表示
        If ノードリスト.Count = 0 Then
            ノード.world_pos_x = 視点原点Wx + (Me.Width - 40) / イメージ倍率 / 2
            ノード.world_pos_y = 視点原点Wy + (Me.Height - 65) / イメージ倍率 / 2
        Else
            Dim x As Integer
            Dim y As Integer

            x = 乱数生成器.Next(50) - 20
            y = 乱数生成器.Next(50) - 20

            '重なりを避ける
            If x = 0 Then
                x = 1
            End If
            If y = 0 Then
                y = 1
            End If

            If ノード.親ノード番号リスト.Count <> 0 Then
                ノード.world_pos_x = ノードリスト(ノード.親ノード番号リスト(0)).world_pos_x + x
                ノード.world_pos_y = ノードリスト(ノード.親ノード番号リスト(0)).world_pos_y + y
            Else
                ノード.world_pos_x = 視点原点Wx + (Me.Width - 40) / イメージ倍率 / 2 + x
                ノード.world_pos_y = 視点原点Wy + (Me.Height - 65) / イメージ倍率 / 2 + y
            End If
        End If

        ノードリスト.Add(ノード)
        Setオブジェクト領域()

        '近くにノードが追加されたら計算回数を初期化する
        For i = 0 To ノードリスト.Count - 1
            ノードリスト(i).位置計算回数初期化(ノード.world_pos_x, ノード.world_pos_y)
        Next


        '再描画
        Me.Refresh()
        Return True
    End Function

    '----------------------------------------------------------------------------------------------
    '　　　ノード関連の位置計算 ( 改良版のアルゴリズム )
    '----------------------------------------------------------------------------------------------
    Private Const gBaseDistance_Connection As Double = 400
    Private Const gFactor_Connection As Double = 0.1
    Private Const gBaseDistance_NoConnection As Double = 500
    Private Const gFactor_NoConnection As Double = 0.1

    Public Sub ノードの位置調整()
        Dim i, j As Integer
        Dim 力 As Point
        力 = New Point


        For i = 0 To ノードリスト.Count - 1
            If ノードリスト(i).位置計算回数取得() > 0 Then
                For j = 0 To ノードリスト.Count - 1
                    If i <> j Then
                        If ノードリスト(i).親ノードかどうか(j) Then
                            '引力の計算
                            力.X = 0
                            力.Y = 0
                            接続したノードの力(ノードリスト(i), ノードリスト(j), 力)
                            ノードリスト(i).移動量.X += 力.X
                            ノードリスト(i).移動量.Y += 力.Y
                        Else
                            '斥力の計算
                            力.X = 0
                            力.Y = 0
                            接続してないノードの力(ノードリスト(i), ノードリスト(j), 力)
                            ノードリスト(i).移動量.X += 力.X
                            ノードリスト(i).移動量.Y += 力.Y
                        End If
                    End If
                Next
            End If
        Next

        '移動させる
        For i = 0 To ノードリスト.Count - 1
            If ノードリスト(i).移動量.X <> 0 Then
                ノードリスト(i).world_pos_x += ノードリスト(i).移動量.X
                ノードリスト(i).移動量.X = 0
            End If

            If ノードリスト(i).移動量.Y <> 0 Then
                ノードリスト(i).world_pos_y += ノードリスト(i).移動量.Y
                ノードリスト(i).移動量.Y = 0
            End If
        Next

        '再描画
        Me.Refresh()
    End Sub

    Public Sub 接続してないノードの力(ByRef ノード1 As ページノード, ByRef ノード2 As ページノード, ByRef 斥力 As Point)
        Dim ベクトルX As Double   'ノード2 → ノード1のベクトル
        Dim ベクトルY As Double   'ノード2 → ノード1のベクトル
        Dim 距離 As Double
        ベクトルX = ノード1.world_pos_x - ノード2.world_pos_x
        ベクトルY = ノード1.world_pos_y - ノード2.world_pos_y

        距離 = Math.Sqrt((ベクトルX ^ 2 + ベクトルY ^ 2))
        If 距離 = 0 Then
            距離 = 1
        End If

        'ベクトルを単位ベクトルにする
        ベクトルX = ベクトルX / 距離
        ベクトルY = ベクトルY / 距離

        '標準距離に対する引力と斥力を計算する
        Dim factor As Double 'マイナス:引力, プラス:斥力
        factor = gBaseDistance_NoConnection - 距離

        factor = Math.Min(factor, 500)
        factor = Math.Max(factor, -500)

        '遠くても引力は0にする
        If factor < 0 Then
            factor = 0
        End If

        ベクトルX = ベクトルX * factor * gFactor_NoConnection
        ベクトルY = ベクトルY * factor * gFactor_NoConnection

        斥力.X = CInt(ベクトルX)
        斥力.Y = CInt(ベクトルY)

    End Sub

    Public Sub 接続したノードの力(ByRef ノード1 As ページノード, ByRef ノード2 As ページノード, ByRef 引力 As Point)
        Dim ベクトルX As Double   'ノード1→ノード2のベクトル
        Dim ベクトルY As Double   'ノード1→ノード2のベクトル
        Dim 距離 As Double
        ベクトルX = ノード2.world_pos_x - ノード1.world_pos_x
        ベクトルY = ノード2.world_pos_y - ノード1.world_pos_y
        距離 = Math.Sqrt((ベクトルX ^ 2 + ベクトルY ^ 2))

        If 距離 = 0 Then
            距離 = 1
        End If

        'ベクトルを単位ベクトルにする
        ベクトルX = ベクトルX / 距離
        ベクトルY = ベクトルY / 距離

        '標準距離に対する引力と斥力を計算する
        Dim factor As Double 'マイナス　斥力, プラス　引力
        factor = 距離 - gBaseDistance_Connection

        factor = Math.Min(factor, 500)
        factor = Math.Max(factor, -500)

        '近くても斥力は0にする
        'If factor < 0 Then
        '    factor = 0
        'End If


        ベクトルX = ベクトルX * factor * gFactor_Connection
        ベクトルY = ベクトルY * factor * gFactor_Connection

        引力.X = CInt(ベクトルX)
        引力.Y = CInt(ベクトルY)
    End Sub

    '----------------------------------------------------------------------------------------------
    '　　　ノード関連の位置計算 ( オリジナルのアルゴリズム )
    '----------------------------------------------------------------------------------------------
    'Public Sub ノードの位置調整()
    '    Dim i, j As Integer
    '    Dim 力 As Point
    '    力 = New Point


    '    For i = 0 To ノードリスト.Count - 1
    '        If ノードリスト(i).位置計算回数取得() > 0 Then
    '            For j = 0 To ノードリスト.Count - 1
    '                If i <> j Then
    '                    If ノードリスト(i).親ノードかどうか(j) Then
    '                        '引力の計算
    '                        力.X = 0
    '                        力.Y = 0
    '                        接続したノードの引力(ノードリスト(i), ノードリスト(j), 力)
    '                        ノードリスト(i).移動量.X += 力.X
    '                        ノードリスト(i).移動量.Y += 力.Y
    '                    Else
    '                        '斥力の計算
    '                        力.X = 0
    '                        力.Y = 0
    '                        接続してないノードの斥力(ノードリスト(i), ノードリスト(j), 力)
    '                        ノードリスト(i).移動量.X += 力.X
    '                        ノードリスト(i).移動量.Y += 力.Y
    '                    End If
    '                End If
    '            Next
    '        End If
    '    Next

    '    '移動させる
    '    For i = 0 To ノードリスト.Count - 1
    '        If ノードリスト(i).移動量.X <> 0 Then
    '            ノードリスト(i).world_pos_x += ノードリスト(i).移動量.X
    '            ノードリスト(i).移動量.X = 0
    '        End If

    '        If ノードリスト(i).移動量.Y <> 0 Then
    '            ノードリスト(i).world_pos_y += ノードリスト(i).移動量.Y
    '            ノードリスト(i).移動量.Y = 0
    '        End If
    '    Next

    '    '再描画
    '    Me.Refresh()
    'End Sub

    'Public Sub 接続してないノードの斥力(ByRef ノード1 As ページノード, ByRef ノード2 As ページノード, ByRef 斥力 As Point)
    '    Dim ベクトルX As Double   'ノード2 → ノード1のベクトル
    '    Dim ベクトルY As Double   'ノード2 → ノード1のベクトル
    '    Dim 距離 As Double
    '    ベクトルX = ノード1.world_pos_x - ノード2.world_pos_x
    '    ベクトルY = ノード1.world_pos_y - ノード2.world_pos_y

    '    距離 = Math.Sqrt((ベクトルX ^ 2 + ベクトルY ^ 2))
    '    If 距離 = 0 Then
    '        距離 = 1
    '    End If

    '    'ベクトルを単位ベクトルにする
    '    ベクトルX = ベクトルX / 距離
    '    ベクトルY = ベクトルY / 距離

    '    '距離に反比例（階段状）になるようにベクトルの大きさを決める
    '    If 距離 > 800 Then
    '        ベクトルX = ベクトルX * 0
    '        ベクトルY = ベクトルY * 0
    '    ElseIf 距離 > 300 Then
    '        ベクトルX = ベクトルX * 5
    '        ベクトルY = ベクトルY * 5
    '    ElseIf 距離 > 100 Then
    '        ベクトルX = ベクトルX * 10
    '        ベクトルY = ベクトルY * 10
    '    Else
    '        ベクトルX = ベクトルX * 30
    '        ベクトルY = ベクトルY * 30
    '    End If

    '    斥力.X = CInt(ベクトルX)
    '    斥力.Y = CInt(ベクトルY)

    'End Sub

    'Public Sub 接続したノードの引力(ByRef ノード1 As ページノード, ByRef ノード2 As ページノード, ByRef 引力 As Point)
    '    Dim ベクトルX As Double   'ノード1→ノード2のベクトル
    '    Dim ベクトルY As Double   'ノード1→ノード2のベクトル
    '    Dim 距離 As Double
    '    ベクトルX = ノード2.world_pos_x - ノード1.world_pos_x
    '    ベクトルY = ノード2.world_pos_y - ノード1.world_pos_y
    '    距離 = Math.Sqrt((ベクトルX ^ 2 + ベクトルY ^ 2))

    '    If 距離 = 0 Then
    '        距離 = 1
    '    End If

    '    'ベクトルを単位ベクトルにする
    '    ベクトルX = ベクトルX / 距離
    '    ベクトルY = ベクトルY / 距離

    '    '距離に反比例（階段状）になるようにベクトルの大きさを決める
    '    If 距離 < 300 Then
    '        ベクトルX = (300 - ベクトルX) * 0.05
    '        ベクトルY = (300 - ベクトルY) * 0.05
    '    ElseIf 距離 < 400 Then
    '        ベクトルX = ベクトルX * 5
    '        ベクトルY = ベクトルY * 5
    '    ElseIf 距離 < 600 Then
    '        ベクトルX = ベクトルX * 10
    '        ベクトルY = ベクトルY * 10
    '    ElseIf 距離 < 1000 Then
    '        ベクトルX = ベクトルX * 15
    '        ベクトルY = ベクトルY * 15
    '    Else
    '        ベクトルX = ベクトルX * 15
    '        ベクトルY = ベクトルY * 15
    '    End If

    '    引力.X = CInt(ベクトルX)
    '    引力.Y = CInt(ベクトルY)

    'End Sub

    '----------------------------------------------------------------------------------------------
    '　　　ボタンの操作関連関数
    '----------------------------------------------------------------------------------------------

    Public Sub ノードのクリア()
        ノードリスト.Clear()
        Setオブジェクト領域()
        視点原点Wx = 0
        視点原点Wy = 0
        '再描画
        Me.Refresh()
    End Sub

    Public Sub Setオブジェクト領域()
        オブジェクト領域.left = 0
        オブジェクト領域.top = 0
        オブジェクト領域.right = 0
        オブジェクト領域.bottom = 0

        Dim left As Integer
        Dim top As Integer
        Dim right As Integer
        Dim bottom As Integer


        For i = 0 To ノードリスト.Count - 1
            If オブジェクト領域.left > ノードリスト(i).world_pos_x Then
                オブジェクト領域.left = ノードリスト(i).world_pos_x
            End If
            If オブジェクト領域.top > ノードリスト(i).world_pos_y Then
                オブジェクト領域.top = ノードリスト(i).world_pos_y
            End If
            If オブジェクト領域.right < ノードリスト(i).world_pos_x + ノードリスト(i).イメージ.Width Then
                オブジェクト領域.right = ノードリスト(i).world_pos_x + ノードリスト(i).イメージ.Width
            End If
            If オブジェクト領域.bottom < ノードリスト(i).world_pos_y + ノードリスト(i).イメージ.Height Then
                オブジェクト領域.bottom = ノードリスト(i).world_pos_y + ノードリスト(i).イメージ.Height
            End If
        Next

        ワールド座標からView座標へ変換(オブジェクト領域.left, オブジェクト領域.top, left, top)
        ワールド座標からView座標へ変換(オブジェクト領域.right, オブジェクト領域.bottom, right, bottom)
    End Sub

End Class

'----------------------------------------------------------------------------------------------
'　　　ページノードクラス
'----------------------------------------------------------------------------------------------
Public Class ページノード
    Public タイトル As String
    Public ページUrl As String
    Public イメージUrl As String
    Public イメージ As Image
    Public world_pos_x As Integer 'ノードのポジションx
    Public world_pos_y As Integer 'ノードのポジションy
    Public ノード番号 As Integer 'ノード番号
    Public 親ノード番号リスト As List(Of Integer)   '親ノードのリスト
    Public 移動量 As Point
    Public 展開済み As Boolean
    Public 位置計算回数 As Integer


    Public Sub New()
        親ノード番号リスト = New List(Of Integer)
        移動量 = New Point(0, 0)
        展開済み = False
        位置計算回数初期化()
    End Sub

    Public Sub 位置計算回数初期化()
        位置計算回数 = 40
    End Sub

    Public Sub 位置計算回数初期化(ByVal x As Integer, ByVal y As Integer)
        If System.Math.Abs(world_pos_x - x) < 1000 Then
            If System.Math.Abs(world_pos_y - y) < 1000 Then
                位置計算回数初期化()
            End If
        End If
    End Sub

    Public Function 位置計算回数取得()
        位置計算回数 = 位置計算回数 - 1
        If 位置計算回数 < 0 Then
            位置計算回数 = 0
        End If

        Return 位置計算回数
    End Function

    Public Function GetWorldPos中心() As Point
        Dim p As Point
        p = New Point(world_pos_x + イメージ.Width / 2, world_pos_y + イメージ.Height / 2)
        Return p
    End Function

    Public Function 親ノードかどうか(ByVal 番号 As Integer) As Boolean
        If 親ノード番号リスト.Count = 0 Then
            親ノードかどうか = False
            Exit Function
        End If

        For i = 0 To 親ノード番号リスト.Count - 1
            If 親ノード番号リスト(i) = 番号 Then
                親ノードかどうか = True
                Exit Function
            End If
        Next

        親ノードかどうか = False
    End Function

    Public Sub 親ノードを追加(ByVal ノード番号 As Integer)
        Dim i As Integer

        If ノード番号 = -1 Then
            Exit Sub
        End If

        If 親ノード番号リスト.Count = 0 Then
            親ノード番号リスト.Add(ノード番号)
            Exit Sub
        End If

        For i = 0 To 親ノード番号リスト.Count - 1
            If 親ノード番号リスト(i) = ノード番号 Then
                Exit Sub
            End If
        Next

            親ノード番号リスト.Add(ノード番号)
    End Sub

    Public Sub LoadImage()
        Dim wc As System.Net.WebClient = New System.Net.WebClient
        Dim stream As System.IO.Stream

        Try
            stream = wc.OpenRead(イメージUrl)
        Catch ex As System.Exception
            stream = wc.OpenRead("./err.jpg")
        End Try

        イメージ = New Bitmap(stream)
        stream.Close()
    End Sub

    Public Function クリック判定(ByVal x As Integer, ByVal y As Integer) As Boolean
        If world_pos_x < x And x < world_pos_x + イメージ.Width Then
            If world_pos_y < y And y < world_pos_y + イメージ.Height Then
                クリック判定 = True
                Exit Function
            End If
        End If

        クリック判定 = False
    End Function

End Class

'----------------------------------------------------------------------------------------------
'　　　領域クラス
'----------------------------------------------------------------------------------------------
Public Class 領域
    Public left As Integer
    Public top As Integer
    Public right As Integer
    Public bottom As Integer
End Class

'----------------------------------------------------------------------------------------------
'　　　ダウンロードQクラス
'----------------------------------------------------------------------------------------------
Public Class ダウンロードQ
    Public url As String
    Public 親ノード番号 As Integer

    Public Sub New(ByVal s_url As String, ByVal n親ノード番号 As Integer)
        url = s_url
        親ノード番号 = n親ノード番号
    End Sub
End Class
