<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form 覆寫 Dispose 以清除元件清單。
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    '為 Windows Form 設計工具的必要項
    Private components As System.ComponentModel.IContainer

    '注意: 以下為 Windows Form 設計工具所需的程序
    '可以使用 Windows Form 設計工具進行修改。
    '請勿使用程式碼編輯器進行修改。
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.buttonS = New System.Windows.Forms.Button()
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.ButtonZ = New System.Windows.Forms.Button()
        Me.ButtonSave = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.ComboBoxHis = New System.Windows.Forms.ComboBox()
        Me.ComboBoxHotKey = New System.Windows.Forms.ComboBox()
        Me.ButtonHotKey = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.CheckBoxShift = New System.Windows.Forms.CheckBox()
        Me.CheckBoxAlt = New System.Windows.Forms.CheckBox()
        Me.CheckBoxCtrl = New System.Windows.Forms.CheckBox()
        Me.ComboBoxF12 = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ComboBoxF11 = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.ComboBoxF10 = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.ComboBoxF9 = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.ComboBoxF8 = New System.Windows.Forms.ComboBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.ComboBoxF7 = New System.Windows.Forms.ComboBox()
        Me.LabelF7 = New System.Windows.Forms.Label()
        Me.ComboBoxF6 = New System.Windows.Forms.ComboBox()
        Me.LabelF6 = New System.Windows.Forms.Label()
        Me.ComboBoxF5 = New System.Windows.Forms.ComboBox()
        Me.LabelF5 = New System.Windows.Forms.Label()
        Me.ComboBoxF4 = New System.Windows.Forms.ComboBox()
        Me.LabelF4 = New System.Windows.Forms.Label()
        Me.ComboBoxF3 = New System.Windows.Forms.ComboBox()
        Me.LabelF3 = New System.Windows.Forms.Label()
        Me.ComboBoxF2 = New System.Windows.Forms.ComboBox()
        Me.LabelF2 = New System.Windows.Forms.Label()
        Me.ComboBoxF1 = New System.Windows.Forms.ComboBox()
        Me.LabelF1 = New System.Windows.Forms.Label()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'buttonS
        '
        Me.buttonS.Location = New System.Drawing.Point(13, 56)
        Me.buttonS.Name = "buttonS"
        Me.buttonS.Size = New System.Drawing.Size(113, 38)
        Me.buttonS.TabIndex = 7
        Me.buttonS.Text = "掛號室設定"
        Me.buttonS.UseVisualStyleBackColor = True
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "愛半月一鍵連接"
        Me.NotifyIcon1.Visible = True
        '
        'PictureBox1
        '
        Me.PictureBox1.Location = New System.Drawing.Point(13, 101)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(900, 685)
        Me.PictureBox1.TabIndex = 8
        Me.PictureBox1.TabStop = False
        '
        'ButtonZ
        '
        Me.ButtonZ.Location = New System.Drawing.Point(127, 56)
        Me.ButtonZ.Name = "ButtonZ"
        Me.ButtonZ.Size = New System.Drawing.Size(113, 38)
        Me.ButtonZ.TabIndex = 9
        Me.ButtonZ.Text = "診間設定"
        Me.ButtonZ.UseVisualStyleBackColor = True
        '
        'ButtonSave
        '
        Me.ButtonSave.Location = New System.Drawing.Point(355, 56)
        Me.ButtonSave.Name = "ButtonSave"
        Me.ButtonSave.Size = New System.Drawing.Size(113, 38)
        Me.ButtonSave.TabIndex = 10
        Me.ButtonSave.Text = "設定存檔"
        Me.ButtonSave.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(255, 19)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(100, 24)
        Me.Label1.TabIndex = 11
        Me.Label1.Text = "主機位置 : "
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(355, 15)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(45, 33)
        Me.TextBox1.TabIndex = 12
        '
        'ComboBoxHis
        '
        Me.ComboBoxHis.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxHis.FormattingEnabled = True
        Me.ComboBoxHis.Items.AddRange(New Object() {"耀聖資訊", "展望資訊", "方鼎資訊", "常誠資料", "醫聖資訊", "開蘭安心", "蒙利特"})
        Me.ComboBoxHis.Location = New System.Drawing.Point(13, 12)
        Me.ComboBoxHis.Name = "ComboBoxHis"
        Me.ComboBoxHis.Size = New System.Drawing.Size(113, 32)
        Me.ComboBoxHis.TabIndex = 14
        '
        'ComboBoxHotKey
        '
        Me.ComboBoxHotKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxHotKey.FormattingEnabled = True
        Me.ComboBoxHotKey.Items.AddRange(New Object() {"F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12"})
        Me.ComboBoxHotKey.Location = New System.Drawing.Point(129, 12)
        Me.ComboBoxHotKey.Name = "ComboBoxHotKey"
        Me.ComboBoxHotKey.Size = New System.Drawing.Size(110, 32)
        Me.ComboBoxHotKey.TabIndex = 15
        '
        'ButtonHotKey
        '
        Me.ButtonHotKey.Location = New System.Drawing.Point(241, 56)
        Me.ButtonHotKey.Name = "ButtonHotKey"
        Me.ButtonHotKey.Size = New System.Drawing.Size(113, 38)
        Me.ButtonHotKey.TabIndex = 16
        Me.ButtonHotKey.Text = "快捷鍵設定"
        Me.ButtonHotKey.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.CheckBoxShift)
        Me.Panel1.Controls.Add(Me.CheckBoxAlt)
        Me.Panel1.Controls.Add(Me.CheckBoxCtrl)
        Me.Panel1.Controls.Add(Me.ComboBoxF12)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.ComboBoxF11)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.ComboBoxF10)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.ComboBoxF9)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.ComboBoxF8)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.ComboBoxF7)
        Me.Panel1.Controls.Add(Me.LabelF7)
        Me.Panel1.Controls.Add(Me.ComboBoxF6)
        Me.Panel1.Controls.Add(Me.LabelF6)
        Me.Panel1.Controls.Add(Me.ComboBoxF5)
        Me.Panel1.Controls.Add(Me.LabelF5)
        Me.Panel1.Controls.Add(Me.ComboBoxF4)
        Me.Panel1.Controls.Add(Me.LabelF4)
        Me.Panel1.Controls.Add(Me.ComboBoxF3)
        Me.Panel1.Controls.Add(Me.LabelF3)
        Me.Panel1.Controls.Add(Me.ComboBoxF2)
        Me.Panel1.Controls.Add(Me.LabelF2)
        Me.Panel1.Controls.Add(Me.ComboBoxF1)
        Me.Panel1.Controls.Add(Me.LabelF1)
        Me.Panel1.Location = New System.Drawing.Point(13, 101)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(455, 276)
        Me.Panel1.TabIndex = 17
        '
        'CheckBoxShift
        '
        Me.CheckBoxShift.AutoSize = True
        Me.CheckBoxShift.Location = New System.Drawing.Point(158, 6)
        Me.CheckBoxShift.Name = "CheckBoxShift"
        Me.CheckBoxShift.Size = New System.Drawing.Size(70, 28)
        Me.CheckBoxShift.TabIndex = 46
        Me.CheckBoxShift.Text = "Shift"
        Me.CheckBoxShift.UseVisualStyleBackColor = True
        '
        'CheckBoxAlt
        '
        Me.CheckBoxAlt.AutoSize = True
        Me.CheckBoxAlt.Location = New System.Drawing.Point(98, 6)
        Me.CheckBoxAlt.Name = "CheckBoxAlt"
        Me.CheckBoxAlt.Size = New System.Drawing.Size(54, 28)
        Me.CheckBoxAlt.TabIndex = 45
        Me.CheckBoxAlt.Text = "Alt"
        Me.CheckBoxAlt.UseVisualStyleBackColor = True
        '
        'CheckBoxCtrl
        '
        Me.CheckBoxCtrl.AutoSize = True
        Me.CheckBoxCtrl.Location = New System.Drawing.Point(31, 6)
        Me.CheckBoxCtrl.Name = "CheckBoxCtrl"
        Me.CheckBoxCtrl.Size = New System.Drawing.Size(61, 28)
        Me.CheckBoxCtrl.TabIndex = 44
        Me.CheckBoxCtrl.Text = "Ctrl"
        Me.CheckBoxCtrl.UseVisualStyleBackColor = True
        '
        'ComboBoxF12
        '
        Me.ComboBoxF12.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxF12.FormattingEnabled = True
        Me.ComboBoxF12.Location = New System.Drawing.Point(291, 233)
        Me.ComboBoxF12.Name = "ComboBoxF12"
        Me.ComboBoxF12.Size = New System.Drawing.Size(146, 32)
        Me.ComboBoxF12.TabIndex = 40
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(236, 235)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(56, 24)
        Me.Label2.TabIndex = 39
        Me.Label2.Text = "F12 : "
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ComboBoxF11
        '
        Me.ComboBoxF11.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxF11.FormattingEnabled = True
        Me.ComboBoxF11.Location = New System.Drawing.Point(291, 194)
        Me.ComboBoxF11.Name = "ComboBoxF11"
        Me.ComboBoxF11.Size = New System.Drawing.Size(146, 32)
        Me.ComboBoxF11.TabIndex = 38
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(236, 196)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(56, 24)
        Me.Label3.TabIndex = 37
        Me.Label3.Text = "F11 : "
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ComboBoxF10
        '
        Me.ComboBoxF10.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxF10.FormattingEnabled = True
        Me.ComboBoxF10.Location = New System.Drawing.Point(291, 155)
        Me.ComboBoxF10.Name = "ComboBoxF10"
        Me.ComboBoxF10.Size = New System.Drawing.Size(146, 32)
        Me.ComboBoxF10.TabIndex = 36
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(236, 157)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(56, 24)
        Me.Label4.TabIndex = 35
        Me.Label4.Text = "F10 : "
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ComboBoxF9
        '
        Me.ComboBoxF9.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxF9.FormattingEnabled = True
        Me.ComboBoxF9.Location = New System.Drawing.Point(291, 116)
        Me.ComboBoxF9.Name = "ComboBoxF9"
        Me.ComboBoxF9.Size = New System.Drawing.Size(146, 32)
        Me.ComboBoxF9.TabIndex = 34
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(236, 118)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(56, 24)
        Me.Label5.TabIndex = 33
        Me.Label5.Text = "F9 : "
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ComboBoxF8
        '
        Me.ComboBoxF8.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxF8.FormattingEnabled = True
        Me.ComboBoxF8.Location = New System.Drawing.Point(291, 77)
        Me.ComboBoxF8.Name = "ComboBoxF8"
        Me.ComboBoxF8.Size = New System.Drawing.Size(146, 32)
        Me.ComboBoxF8.TabIndex = 32
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(236, 79)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(56, 24)
        Me.Label6.TabIndex = 31
        Me.Label6.Text = "F8 : "
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ComboBoxF7
        '
        Me.ComboBoxF7.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxF7.FormattingEnabled = True
        Me.ComboBoxF7.Items.AddRange(New Object() {""})
        Me.ComboBoxF7.Location = New System.Drawing.Point(291, 38)
        Me.ComboBoxF7.Name = "ComboBoxF7"
        Me.ComboBoxF7.Size = New System.Drawing.Size(146, 32)
        Me.ComboBoxF7.TabIndex = 30
        '
        'LabelF7
        '
        Me.LabelF7.Location = New System.Drawing.Point(236, 40)
        Me.LabelF7.Name = "LabelF7"
        Me.LabelF7.Size = New System.Drawing.Size(56, 24)
        Me.LabelF7.TabIndex = 29
        Me.LabelF7.Text = "F7 : "
        Me.LabelF7.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ComboBoxF6
        '
        Me.ComboBoxF6.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxF6.FormattingEnabled = True
        Me.ComboBoxF6.Location = New System.Drawing.Point(63, 233)
        Me.ComboBoxF6.Name = "ComboBoxF6"
        Me.ComboBoxF6.Size = New System.Drawing.Size(146, 32)
        Me.ComboBoxF6.TabIndex = 28
        '
        'LabelF6
        '
        Me.LabelF6.Location = New System.Drawing.Point(8, 235)
        Me.LabelF6.Name = "LabelF6"
        Me.LabelF6.Size = New System.Drawing.Size(56, 24)
        Me.LabelF6.TabIndex = 27
        Me.LabelF6.Text = "F6 : "
        Me.LabelF6.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ComboBoxF5
        '
        Me.ComboBoxF5.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxF5.FormattingEnabled = True
        Me.ComboBoxF5.Location = New System.Drawing.Point(63, 194)
        Me.ComboBoxF5.Name = "ComboBoxF5"
        Me.ComboBoxF5.Size = New System.Drawing.Size(146, 32)
        Me.ComboBoxF5.TabIndex = 26
        '
        'LabelF5
        '
        Me.LabelF5.Location = New System.Drawing.Point(8, 196)
        Me.LabelF5.Name = "LabelF5"
        Me.LabelF5.Size = New System.Drawing.Size(56, 24)
        Me.LabelF5.TabIndex = 25
        Me.LabelF5.Text = "F5 : "
        Me.LabelF5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ComboBoxF4
        '
        Me.ComboBoxF4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxF4.FormattingEnabled = True
        Me.ComboBoxF4.Location = New System.Drawing.Point(63, 155)
        Me.ComboBoxF4.Name = "ComboBoxF4"
        Me.ComboBoxF4.Size = New System.Drawing.Size(146, 32)
        Me.ComboBoxF4.TabIndex = 24
        '
        'LabelF4
        '
        Me.LabelF4.Location = New System.Drawing.Point(8, 157)
        Me.LabelF4.Name = "LabelF4"
        Me.LabelF4.Size = New System.Drawing.Size(56, 24)
        Me.LabelF4.TabIndex = 23
        Me.LabelF4.Text = "F4 : "
        Me.LabelF4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ComboBoxF3
        '
        Me.ComboBoxF3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxF3.FormattingEnabled = True
        Me.ComboBoxF3.Location = New System.Drawing.Point(63, 116)
        Me.ComboBoxF3.Name = "ComboBoxF3"
        Me.ComboBoxF3.Size = New System.Drawing.Size(146, 32)
        Me.ComboBoxF3.TabIndex = 22
        '
        'LabelF3
        '
        Me.LabelF3.Location = New System.Drawing.Point(8, 118)
        Me.LabelF3.Name = "LabelF3"
        Me.LabelF3.Size = New System.Drawing.Size(56, 24)
        Me.LabelF3.TabIndex = 21
        Me.LabelF3.Text = "F3 : "
        Me.LabelF3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ComboBoxF2
        '
        Me.ComboBoxF2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxF2.FormattingEnabled = True
        Me.ComboBoxF2.Location = New System.Drawing.Point(63, 77)
        Me.ComboBoxF2.Name = "ComboBoxF2"
        Me.ComboBoxF2.Size = New System.Drawing.Size(146, 32)
        Me.ComboBoxF2.TabIndex = 20
        '
        'LabelF2
        '
        Me.LabelF2.Location = New System.Drawing.Point(8, 79)
        Me.LabelF2.Name = "LabelF2"
        Me.LabelF2.Size = New System.Drawing.Size(56, 24)
        Me.LabelF2.TabIndex = 19
        Me.LabelF2.Text = "F2 : "
        Me.LabelF2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ComboBoxF1
        '
        Me.ComboBoxF1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxF1.FormattingEnabled = True
        Me.ComboBoxF1.Location = New System.Drawing.Point(63, 38)
        Me.ComboBoxF1.Name = "ComboBoxF1"
        Me.ComboBoxF1.Size = New System.Drawing.Size(146, 32)
        Me.ComboBoxF1.TabIndex = 18
        '
        'LabelF1
        '
        Me.LabelF1.Location = New System.Drawing.Point(8, 40)
        Me.LabelF1.Name = "LabelF1"
        Me.LabelF1.Size = New System.Drawing.Size(56, 24)
        Me.LabelF1.TabIndex = 0
        Me.LabelF1.Text = "F1 : "
        Me.LabelF1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Form1
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(476, 798)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.ButtonHotKey)
        Me.Controls.Add(Me.ComboBoxHotKey)
        Me.Controls.Add(Me.ComboBoxHis)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ButtonSave)
        Me.Controls.Add(Me.ButtonZ)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.buttonS)
        Me.Font = New System.Drawing.Font("微軟正黑體", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Margin = New System.Windows.Forms.Padding(6)
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "愛半月一鍵連接 Ver 1.0.0.0"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Private WithEvents buttonS As Button
    Friend WithEvents NotifyIcon1 As NotifyIcon
    Friend WithEvents PictureBox1 As PictureBox
    Private WithEvents ButtonZ As Button
    Private WithEvents ButtonSave As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents ComboBoxHis As ComboBox
    Friend WithEvents ComboBoxHotKey As ComboBox
    Private WithEvents ButtonHotKey As Button
    Friend WithEvents Panel1 As Panel
    Friend WithEvents ComboBoxF7 As ComboBox
    Friend WithEvents LabelF7 As Label
    Friend WithEvents ComboBoxF6 As ComboBox
    Friend WithEvents LabelF6 As Label
    Friend WithEvents ComboBoxF5 As ComboBox
    Friend WithEvents LabelF5 As Label
    Friend WithEvents ComboBoxF4 As ComboBox
    Friend WithEvents LabelF4 As Label
    Friend WithEvents ComboBoxF3 As ComboBox
    Friend WithEvents LabelF3 As Label
    Friend WithEvents ComboBoxF2 As ComboBox
    Friend WithEvents LabelF2 As Label
    Friend WithEvents ComboBoxF1 As ComboBox
    Friend WithEvents LabelF1 As Label
    Friend WithEvents ComboBoxF12 As ComboBox
    Friend WithEvents Label2 As Label
    Friend WithEvents ComboBoxF11 As ComboBox
    Friend WithEvents Label3 As Label
    Friend WithEvents ComboBoxF10 As ComboBox
    Friend WithEvents Label4 As Label
    Friend WithEvents ComboBoxF9 As ComboBox
    Friend WithEvents Label5 As Label
    Friend WithEvents ComboBoxF8 As ComboBox
    Friend WithEvents Label6 As Label
    Friend WithEvents CheckBoxShift As CheckBox
    Friend WithEvents CheckBoxAlt As CheckBox
    Friend WithEvents CheckBoxCtrl As CheckBox
End Class
