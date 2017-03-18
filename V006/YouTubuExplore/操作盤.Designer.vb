<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class 操作盤
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
        Me.Label4 = New System.Windows.Forms.Label()
        Me.位置計算繰り返し数 = New System.Windows.Forms.NumericUpDown()
        Me.ステータス = New System.Windows.Forms.TextBox()
        Me.すべてのノードを展開 = New System.Windows.Forms.Button()
        Me.残りノード数 = New System.Windows.Forms.TextBox()
        Me.ノード展開中止 = New System.Windows.Forms.Button()
        Me.リンクの表示 = New System.Windows.Forms.CheckBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.UrlTextBox = New System.Windows.Forms.TextBox()
        Me.展開待ちノードクリア = New System.Windows.Forms.Button()
        CType(Me.位置計算繰り返し数, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(112, 32)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(118, 12)
        Me.Label4.TabIndex = 21
        Me.Label4.Text = "位置調整繰り返し計算"
        '
        '位置計算繰り返し数
        '
        Me.位置計算繰り返し数.Location = New System.Drawing.Point(236, 30)
        Me.位置計算繰り返し数.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.位置計算繰り返し数.Minimum = New Decimal(New Integer() {5, 0, 0, 0})
        Me.位置計算繰り返し数.Name = "位置計算繰り返し数"
        Me.位置計算繰り返し数.Size = New System.Drawing.Size(51, 19)
        Me.位置計算繰り返し数.TabIndex = 22
        Me.位置計算繰り返し数.Value = New Decimal(New Integer() {20, 0, 0, 0})
        '
        'ステータス
        '
        Me.ステータス.Location = New System.Drawing.Point(4, 115)
        Me.ステータス.Name = "ステータス"
        Me.ステータス.ReadOnly = True
        Me.ステータス.Size = New System.Drawing.Size(341, 19)
        Me.ステータス.TabIndex = 26
        Me.ステータス.Text = "0"
        '
        'すべてのノードを展開
        '
        Me.すべてのノードを展開.Font = New System.Drawing.Font("MS UI Gothic", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.すべてのノードを展開.Location = New System.Drawing.Point(4, 53)
        Me.すべてのノードを展開.Name = "すべてのノードを展開"
        Me.すべてのノードを展開.Size = New System.Drawing.Size(93, 55)
        Me.すべてのノードを展開.TabIndex = 32
        Me.すべてのノードを展開.Text = "すべてのノードを展開(再開)"
        Me.すべてのノードを展開.UseVisualStyleBackColor = True
        '
        '残りノード数
        '
        Me.残りノード数.Location = New System.Drawing.Point(103, 90)
        Me.残りノード数.Name = "残りノード数"
        Me.残りノード数.ReadOnly = True
        Me.残りノード数.Size = New System.Drawing.Size(242, 19)
        Me.残りノード数.TabIndex = 33
        Me.残りノード数.Text = "展開待ちノード数 : 0"
        '
        'ノード展開中止
        '
        Me.ノード展開中止.Font = New System.Drawing.Font("MS UI Gothic", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.ノード展開中止.Location = New System.Drawing.Point(103, 53)
        Me.ノード展開中止.Name = "ノード展開中止"
        Me.ノード展開中止.Size = New System.Drawing.Size(107, 30)
        Me.ノード展開中止.TabIndex = 34
        Me.ノード展開中止.Text = "ノード展開停止"
        Me.ノード展開中止.UseVisualStyleBackColor = True
        '
        'リンクの表示
        '
        Me.リンクの表示.AutoSize = True
        Me.リンクの表示.Checked = True
        Me.リンクの表示.CheckState = System.Windows.Forms.CheckState.Checked
        Me.リンクの表示.Location = New System.Drawing.Point(4, 31)
        Me.リンクの表示.Name = "リンクの表示"
        Me.リンクの表示.Size = New System.Drawing.Size(102, 16)
        Me.リンクの表示.TabIndex = 36
        Me.リンクの表示.Text = "リンク(線)の表示"
        Me.リンクの表示.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(2, 9)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(27, 12)
        Me.Label3.TabIndex = 17
        Me.Label3.Text = "URL"
        '
        'UrlTextBox
        '
        Me.UrlTextBox.Location = New System.Drawing.Point(35, 6)
        Me.UrlTextBox.Name = "UrlTextBox"
        Me.UrlTextBox.Size = New System.Drawing.Size(310, 19)
        Me.UrlTextBox.TabIndex = 11
        Me.UrlTextBox.Text = "http://www.youtube.com/"
        '
        '展開待ちノードクリア
        '
        Me.展開待ちノードクリア.Font = New System.Drawing.Font("MS UI Gothic", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.展開待ちノードクリア.Location = New System.Drawing.Point(216, 53)
        Me.展開待ちノードクリア.Name = "展開待ちノードクリア"
        Me.展開待ちノードクリア.Size = New System.Drawing.Size(100, 30)
        Me.展開待ちノードクリア.TabIndex = 37
        Me.展開待ちノードクリア.Text = "展開待ノードクリア"
        Me.展開待ちノードクリア.UseVisualStyleBackColor = True
        '
        '操作盤
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(334, 138)
        Me.ControlBox = False
        Me.Controls.Add(Me.展開待ちノードクリア)
        Me.Controls.Add(Me.リンクの表示)
        Me.Controls.Add(Me.ノード展開中止)
        Me.Controls.Add(Me.残りノード数)
        Me.Controls.Add(Me.すべてのノードを展開)
        Me.Controls.Add(Me.ステータス)
        Me.Controls.Add(Me.位置計算繰り返し数)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.UrlTextBox)
        Me.Location = New System.Drawing.Point(300, 100)
        Me.Name = "操作盤"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "YouTubeExplorer[操作盤]"
        CType(Me.位置計算繰り返し数, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents 位置計算繰り返し数 As System.Windows.Forms.NumericUpDown
    Friend WithEvents ステータス As System.Windows.Forms.TextBox
    Friend WithEvents すべてのノードを展開 As System.Windows.Forms.Button
    Friend WithEvents 残りノード数 As System.Windows.Forms.TextBox
    Friend WithEvents ノード展開中止 As System.Windows.Forms.Button
    Friend WithEvents リンクの表示 As System.Windows.Forms.CheckBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents UrlTextBox As System.Windows.Forms.TextBox
    Friend WithEvents 展開待ちノードクリア As System.Windows.Forms.Button
End Class
