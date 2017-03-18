<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class メインフォーム
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows フォーム デザイナーで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナーで必要です。
    'Windows フォーム デザイナーを使用して変更できます。  
    'コード エディターを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ノードタイトル = New System.Windows.Forms.ToolTip(Me.components)
        Me.ノード展開Timer = New System.Windows.Forms.Timer(Me.components)
        Me.読み込みボタン = New System.Windows.Forms.Button()
        Me.テキストボックスURL = New System.Windows.Forms.TextBox()
        Me.クリアボタン = New System.Windows.Forms.Button()
        Me.操作盤表示 = New System.Windows.Forms.Button()
        Me.タイトル表示_非表示 = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'ノードタイトル
        '
        Me.ノードタイトル.ToolTipTitle = "テスト"
        '
        'ノード展開Timer
        '
        '
        '読み込みボタン
        '
        Me.読み込みボタン.Location = New System.Drawing.Point(5, 30)
        Me.読み込みボタン.Name = "読み込みボタン"
        Me.読み込みボタン.Size = New System.Drawing.Size(85, 25)
        Me.読み込みボタン.TabIndex = 0
        Me.読み込みボタン.Text = "読み込み"
        Me.読み込みボタン.UseVisualStyleBackColor = True
        '
        'テキストボックスURL
        '
        Me.テキストボックスURL.Font = New System.Drawing.Font("MS UI Gothic", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.テキストボックスURL.Location = New System.Drawing.Point(5, 5)
        Me.テキストボックスURL.Name = "テキストボックスURL"
        Me.テキストボックスURL.Size = New System.Drawing.Size(403, 20)
        Me.テキストボックスURL.TabIndex = 1
        Me.テキストボックスURL.Text = "プログラミング"
        '
        'クリアボタン
        '
        Me.クリアボタン.Location = New System.Drawing.Point(96, 30)
        Me.クリアボタン.Name = "クリアボタン"
        Me.クリアボタン.Size = New System.Drawing.Size(120, 25)
        Me.クリアボタン.TabIndex = 2
        Me.クリアボタン.Text = "全てのノードをクリア"
        Me.クリアボタン.UseVisualStyleBackColor = True
        '
        '操作盤表示
        '
        Me.操作盤表示.Location = New System.Drawing.Point(5, 61)
        Me.操作盤表示.Name = "操作盤表示"
        Me.操作盤表示.Size = New System.Drawing.Size(85, 25)
        Me.操作盤表示.TabIndex = 3
        Me.操作盤表示.Text = "操作盤表示"
        Me.操作盤表示.UseVisualStyleBackColor = True
        '
        'タイトル表示_非表示
        '
        Me.タイトル表示_非表示.Location = New System.Drawing.Point(99, 61)
        Me.タイトル表示_非表示.Name = "タイトル表示_非表示"
        Me.タイトル表示_非表示.Size = New System.Drawing.Size(117, 25)
        Me.タイトル表示_非表示.TabIndex = 4
        Me.タイトル表示_非表示.Text = "タイトル表示/非表示"
        Me.タイトル表示_非表示.UseVisualStyleBackColor = True
        '
        'メインフォーム
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(484, 463)
        Me.Controls.Add(Me.タイトル表示_非表示)
        Me.Controls.Add(Me.操作盤表示)
        Me.Controls.Add(Me.クリアボタン)
        Me.Controls.Add(Me.テキストボックスURL)
        Me.Controls.Add(Me.読み込みボタン)
        Me.Name = "メインフォーム"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "YouTubeExplorer[メインウィンドウ]"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ノードタイトル As System.Windows.Forms.ToolTip
    Friend WithEvents ノード展開Timer As System.Windows.Forms.Timer
    Friend WithEvents 読み込みボタン As System.Windows.Forms.Button
    Friend WithEvents テキストボックスURL As System.Windows.Forms.TextBox
    Friend WithEvents クリアボタン As System.Windows.Forms.Button
    Friend WithEvents 操作盤表示 As System.Windows.Forms.Button
    Friend WithEvents タイトル表示_非表示 As System.Windows.Forms.Button

End Class
