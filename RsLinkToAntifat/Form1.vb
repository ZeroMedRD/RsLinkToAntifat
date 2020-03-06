Imports System.Data.Odbc
Imports System.Configuration
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.IO
Imports System.Net
Imports System.Web.Script.Serialization

Public Class Form1

    Dim WH_KEYBOARD_LL As Integer = 13

    Public Delegate Function HookProc(nCode As Integer, wParam As IntPtr, lParam As IntPtr) As Integer
    Private Shared m_HookHandle As Integer = 0         ' Hook handle
    Private m_KbdHookProc As HookProc                  ' 鍵盤掛鉤函式指標

    Public Delegate Function EnumWindowsProc(hwnd As IntPtr, lParam As IntPtr) As Integer


    Declare Function FindWindow Lib "user32" Alias "FindWindowA" (ByVal lpClassName As String, ByVal lpWindowName As String) As Integer

    Declare Function FindWindowEx Lib "user32" Alias "FindWindowExA" (ByVal hwndParent As IntPtr,
                                                                      ByVal hwndChildAfter As IntPtr,
                                                                      ByVal lpszClass As String,
                                                                      ByVal lpszWindow As String) As Integer

    Declare Function GetWindowText Lib "user32" Alias "GetWindowTextA" (ByVal hwndParent As IntPtr,
                                                                        ByVal buff As StringBuilder,
                                                                        ByVal count As Integer) As Integer

    Declare Function EnumWindows Lib "user32" Alias "EnumWindows" (ByVal hWnd As EnumWindowsProc,
                                                                        ByVal lParam As IntPtr) As Integer

    Private Declare Auto Function PostMessage Lib "user32" (ByVal hwnd As Integer,
                                                            ByVal message As UInteger,
                                                            ByVal wParam As Integer,
                                                            ByVal lParam As Integer) As Boolean

    Private Declare Function BringWindowToTop Lib "user32" Alias "BringWindowToTop" (ByVal hwnd As Integer) As Integer

    Private Declare Function SetWindowPos Lib "user32" Alias "SetWindowPos" (ByVal _
        hwnd As Integer, ByVal hWndInsertAfter As Integer, ByVal x As Integer,
        ByVal y As Integer, ByVal cx As Integer, ByVal cy As Integer,
        ByVal wFlags As Integer) As Integer

    'Public Declare Function SetForegroundWindow Lib "user32" (ByVal hwnd As IntPtr) As Integer

    Declare Function GetForegroundWindow Lib "user32" Alias "GetForegroundWindow" () As Long

    <DllImport("kernel32")>
    Private Shared Function GetPrivateProfileString(ByVal section As String, ByVal key As String, ByVal def As String, ByVal retVal As StringBuilder, ByVal size As Integer, ByVal filePath As String) As Integer
    End Function

    <DllImport("kernel32")>
    Private Shared Function WritePrivateProfileString(ByVal section As String, ByVal key As String, ByVal def As String, ByVal filePath As String) As Integer
    End Function


    <DllImport("user32.dll")>
    Private Shared Function ShowWindowAsync(hWnd As IntPtr, nCmdShow As Integer) As Boolean
    End Function

    <DllImport("user32.dll")>
    Private Shared Function ShowWindow(hWnd As IntPtr, nCmdShow As Integer) As Boolean
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
    Private Shared Function SetForegroundWindow(ByVal hWnd As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function


    Public Shared Sub SetTopmostWindow(ByVal hWnd As Integer, Optional ByVal topmost As Boolean = True)
        Const HWND_NOTOPMOST = -2
        Const HWND_TOPMOST = -1
        Const SWP_NOMOVE = &H2
        Const SWP_NOSIZE = &H1
        SetWindowPos(hWnd, IIf(topmost, HWND_TOPMOST, HWND_NOTOPMOST), 0, 0, 0, 0,
            SWP_NOMOVE + SWP_NOSIZE)
    End Sub

    <DllImport("kernel32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
    Private Shared Function GetModuleHandle(lpModuleName As String) As IntPtr
    End Function

    ' 設置掛鉤.
    <DllImport("user32.dll", CharSet:=CharSet.Auto, CallingConvention:=CallingConvention.StdCall)>
    Public Shared Function SetWindowsHookEx(idHook As Integer, lpfn As HookProc, hInstance As IntPtr, threadId As Integer) As Integer
    End Function

    ' 將之前設置的掛鉤移除。記得在應用程式結束前呼叫此函式.
    <DllImport("user32.dll", CharSet:=CharSet.Auto, CallingConvention:=CallingConvention.StdCall)>
    Public Shared Function UnhookWindowsHookEx(idHook As Integer) As Boolean
    End Function

    ' 呼叫下一個掛鉤處理常式（若不這麼做，會令其他掛鉤處理常式失效）.
    <DllImport("user32.dll", CharSet:=CharSet.Auto, CallingConvention:=CallingConvention.StdCall)>
    Public Shared Function CallNextHookEx(idHook As Integer, nCode As Integer, wParam As IntPtr, lParam As IntPtr) As Integer
    End Function

    Shared systemIni As New SystemIni
    Shared hospital As String = ""        ' 目前操作的院所代號 
    Dim currentOcrEdit = ""            ' 目前編輯的 OCR 畫面(S , Z)
    Shared SCTitle As String = ""      ' 醫聖 , 蒙利特 系統的 Title(取病歷號)
    Shared AntifatHttp As String = "www.weightobserver.com.tw"  ' 凌醫網址
    ' Shared AntifatHttp As String = "23.97.65.134:8080/antifat"  ' 凌醫網址

    Public Sub New()
        ' 設計工具需要此呼叫。
        InitializeComponent()
        ' 在 InitializeComponent() 呼叫之後加入所有初始設定。

        ' 取得所有 process 的 title
        'Dim processList = System.Diagnostics.Process.GetProcesses()
        'For i = 0 To processList.Length - 1
        '    Dim process = processList(i)
        '    If process.MainWindowTitle <> "" Then
        '        Dim title = process.MainWindowTitle
        '        If title.Contains("診療室作業") Then
        '            MessageBox.Show(title)
        '            Dim ParenthWnd = FindWindow(Nothing, title)
        '            Dim ChildWnd = IntPtr.Zero
        '            While True
        '                ChildWnd = FindWindowEx(ParenthWnd, ChildWnd, Nothing, Nothing)
        '                If ChildWnd = IntPtr.Zero Then
        '                    Exit While
        '                End If
        '                Dim buf = New StringBuilder(100)
        '                GetWindowText(ChildWnd, buf, 100)
        '                MessageBox.Show(buf.ToString)
        '            End While
        '        End If
        '    End If
        'Next

        Dim ver = "Ver 4.0.0.4"
        ' 版本說明
        ' Ver 1.1.0.0
        '    1.重新開放耀聖畫面放大及縮小
        '    2.加入方鼎系統一鍵連接
        ' Ver 1.2.0.0
        '    1.加入常誠資料
        ' Ver 1.3.0.0
        '    1.加入智抗糖連結
        ' Ver 1.3.1.0
        '    1.加入常誠主機目錄不用含 WM003
        ' Ver 1.3.1.1
        '    1.加入常誠主機目錄要含 WM003
        ' Ver 1.4.0.0
        '    1.加入方鼎文字型病歷號
        ' Ver 1.4.0.1
        '    1.病歷號一律將 O 轉換為 0
        ' Ver 1.4.0.2
        '    1.取方鼎病歷號位置及長度不固定
        ' Ver 1.4.0.3
        '    1.取方鼎病歷號位置及長度不固定 , 重新上版
        ' Ver 1.4.0.4
        '    1.取展望病歷號將 ] => 1
        '    2.取展望病歷號將 l => 1
        ' Ver 1.4.0.5
        '    1.取展望病歷號將 I => 1
        ' Ver 1.4.0.6
        '    1.方鼎病歷號可能只有 4 碼
        '    2.取方鼎病歷號將 — => -
        ' Ver 1.4.0.7
        '    1.取方鼎病歷號將 , => -
        ' Ver 1.5.0.0
        '    1.支援開蘭安心診所
        '    2.方鼎行動電話改取 BBC
        ' Ver 2.0.0.0
        '    1.支援醫聖一鍵連接
        ' Ver 2.0.0.1
        '    1.醫聖一鍵連接 , 使用 window form title 作病患判別
        ' Ver 2.0.0.2
        '    1.醫聖一鍵連接 , 合併使用 OCR
        ' Ver 2.0.0.3
        '    1.耀聖一鍵連接 , 取 patdb.mobil 欄位
        ' Ver 2.0.0.4
        '    1.病歷號 OCR 判別 , 只取數字及-
        ' Ver 2.0.0.5
        '    1.方鼎病歷號取連續 4 ~ 8 碼為數字(含-)
        ' Ver 2.1.0.0
        '    1.支援蒙利特一鍵連接
        ' Ver 2.1.0.1
        '    1.支援常誠 () = 0
        ' Ver 2.1.0.2
        '    1.支援蒙利特(三個電話 , 自動篩檢住家或行動)
        ' Ver 2.1.0.3
        '    1.支援蒙利特病歷取 Window Form 的 Title
        ' Ver 2.1.0.4
        '    1.常誠掛號室取號成功時 , 不要再作診間取號
        ' Ver 2.1.0.5
        '    1.支援方鼎 Z = 2
        ' Ver 2.1.0.6
        '    1.支援蒙利特上傳地址
        ' Ver 2.1.0.7
        '    1.醫聖一鍵連接 , 使用 window form title 支援 整照:000012
        ' Ver 3.0.0.0
        '    1.支援一鍵連接網路掛號
        '    2.支援一鍵連接網路預約查詢
        ' Ver 4.0.0.0
        '    1.一鍵連接改用 www.weightobserver.com.tw
        ' Ver 4.0.0.1
        '    1.一鍵連接支援常誠病患行動電話轉入
        ' Ver 4.0.0.2
        '    1.網路掛號指定院所代號
        '    1.預約查詢指定院所代號
        ' Ver 4.0.0.3
        '    1.病患資料除了耀聖以外 , 一律排除刪除資料
        ' Ver 4.0.0.4
        '    1.展望支援利用 Window Fomr Title 取病患資料

        Me.Text = "愛半月一鍵連接 " + ver

        Me.ComboBoxF1.Items.AddRange(New Object() {"", "個管問診", "檢驗病歷", "檢查報告", "量測資料", "預約作業", "檢驗預約", "關懷記錄", "圖形管理", "身體組成", "門診預約", "門診預約查詢", "智抗糖", "本機功能(一)", "本機功能(二)", "本機功能(三)"})
        Me.ComboBoxF2.Items.AddRange(New Object() {"", "個管問診", "檢驗病歷", "檢查報告", "量測資料", "預約作業", "檢驗預約", "關懷記錄", "圖形管理", "身體組成", "門診預約", "門診預約查詢", "智抗糖", "本機功能(一)", "本機功能(二)", "本機功能(三)"})
        Me.ComboBoxF3.Items.AddRange(New Object() {"", "個管問診", "檢驗病歷", "檢查報告", "量測資料", "預約作業", "檢驗預約", "關懷記錄", "圖形管理", "身體組成", "門診預約", "門診預約查詢", "智抗糖", "本機功能(一)", "本機功能(二)", "本機功能(三)"})
        Me.ComboBoxF4.Items.AddRange(New Object() {"", "個管問診", "檢驗病歷", "檢查報告", "量測資料", "預約作業", "檢驗預約", "關懷記錄", "圖形管理", "身體組成", "門診預約", "門診預約查詢", "智抗糖", "本機功能(一)", "本機功能(二)", "本機功能(三)"})
        Me.ComboBoxF5.Items.AddRange(New Object() {"", "個管問診", "檢驗病歷", "檢查報告", "量測資料", "預約作業", "檢驗預約", "關懷記錄", "圖形管理", "身體組成", "門診預約", "門診預約查詢", "智抗糖", "本機功能(一)", "本機功能(二)", "本機功能(三)"})
        Me.ComboBoxF6.Items.AddRange(New Object() {"", "個管問診", "檢驗病歷", "檢查報告", "量測資料", "預約作業", "檢驗預約", "關懷記錄", "圖形管理", "身體組成", "門診預約", "門診預約查詢", "智抗糖", "本機功能(一)", "本機功能(二)", "本機功能(三)"})
        Me.ComboBoxF7.Items.AddRange(New Object() {"", "個管問診", "檢驗病歷", "檢查報告", "量測資料", "預約作業", "檢驗預約", "關懷記錄", "圖形管理", "身體組成", "門診預約", "門診預約查詢", "智抗糖", "本機功能(一)", "本機功能(二)", "本機功能(三)"})
        Me.ComboBoxF8.Items.AddRange(New Object() {"", "個管問診", "檢驗病歷", "檢查報告", "量測資料", "預約作業", "檢驗預約", "關懷記錄", "圖形管理", "身體組成", "門診預約", "門診預約查詢", "智抗糖", "本機功能(一)", "本機功能(二)", "本機功能(三)"})
        Me.ComboBoxF9.Items.AddRange(New Object() {"", "個管問診", "檢驗病歷", "檢查報告", "量測資料", "預約作業", "檢驗預約", "關懷記錄", "圖形管理", "身體組成", "門診預約", "門診預約查詢", "智抗糖", "本機功能(一)", "本機功能(二)", "本機功能(三)"})
        Me.ComboBoxF10.Items.AddRange(New Object() {"", "個管問診", "檢驗病歷", "檢查報告", "量測資料", "預約作業", "檢驗預約", "關懷記錄", "圖形管理", "身體組成", "門診預約", "門診預約查詢", "智抗糖", "本機功能(一)", "本機功能(二)", "本機功能(三)"})
        Me.ComboBoxF11.Items.AddRange(New Object() {"", "個管問診", "檢驗病歷", "檢查報告", "量測資料", "預約作業", "檢驗預約", "關懷記錄", "圖形管理", "身體組成", "門診預約", "門診預約查詢", "智抗糖", "本機功能(一)", "本機功能(二)", "本機功能(三)"})
        Me.ComboBoxF12.Items.AddRange(New Object() {"", "個管問診", "檢驗病歷", "檢查報告", "量測資料", "預約作業", "檢驗預約", "關懷記錄", "圖形管理", "身體組成", "門診預約", "門診預約查詢", "智抗糖", "本機功能(一)", "本機功能(二)", "本機功能(三)"})


        ' Me.ShowInTaskbar = False  ' 隱藏工作列 
        Me.WindowState = FormWindowState.Minimized  ' 最小化
        button1_Click(Nothing, Nothing)

        Me.Height = 138
        Me.KeyPreview = True


        ' 讀取設定檔
        Dim Ini_File = My.Application.Info.DirectoryPath + "\system.ini"
        If File.Exists(Ini_File) Then
            Dim iniFileName = My.Application.Info.DirectoryPath & "\System.ini"
            ' 讀取系統參數(system.ini)
            systemIni.OcrPath = GetIniValue("RsLinkToAntifat", "OcrPath", iniFileName)
            systemIni.Server = GetIniValue("RsLinkToAntifat", "Server", iniFileName)
            systemIni.His = GetIniValue("RsLinkToAntifat", "His", iniFileName)
            If (systemIni.His = "") Then
                systemIni.His = "RS"
            End If
            systemIni.HotKey = GetIniValue("RsLinkToAntifat", "HotKey", iniFileName)
            systemIni.F1 = GetIniValue("RsLinkToAntifat", "F1", iniFileName)
            systemIni.F2 = GetIniValue("RsLinkToAntifat", "F2", iniFileName)
            systemIni.F3 = GetIniValue("RsLinkToAntifat", "F3", iniFileName)
            systemIni.F4 = GetIniValue("RsLinkToAntifat", "F4", iniFileName)
            systemIni.F5 = GetIniValue("RsLinkToAntifat", "F5", iniFileName)
            systemIni.F6 = GetIniValue("RsLinkToAntifat", "F6", iniFileName)
            systemIni.F7 = GetIniValue("RsLinkToAntifat", "F7", iniFileName)
            systemIni.F8 = GetIniValue("RsLinkToAntifat", "F8", iniFileName)
            systemIni.F9 = GetIniValue("RsLinkToAntifat", "F9", iniFileName)
            systemIni.F10 = GetIniValue("RsLinkToAntifat", "F10", iniFileName)
            systemIni.F11 = GetIniValue("RsLinkToAntifat", "F11", iniFileName)
            systemIni.F12 = GetIniValue("RsLinkToAntifat", "F12", iniFileName)
            systemIni.SPosiX = GetIniValue("RsLinkToAntifat", "SPosiX", iniFileName)
            systemIni.SPosiY = GetIniValue("RsLinkToAntifat", "SPosiY", iniFileName)
            systemIni.SWidth = GetIniValue("RsLinkToAntifat", "SWidth", iniFileName)
            systemIni.SHeight = GetIniValue("RsLinkToAntifat", "SHeight", iniFileName)
            systemIni.ZPosiX = GetIniValue("RsLinkToAntifat", "ZPosiX", iniFileName)
            systemIni.ZPosiY = GetIniValue("RsLinkToAntifat", "ZPosiY", iniFileName)
            systemIni.ZWidth = GetIniValue("RsLinkToAntifat", "ZWidth", iniFileName)
            systemIni.ZHeight = GetIniValue("RsLinkToAntifat", "ZHeight", iniFileName)
            systemIni.LocalFunction1 = GetIniValue("RsLinkToAntifat", "LocalFunction1", iniFileName)
            systemIni.LocalFunction2 = GetIniValue("RsLinkToAntifat", "LocalFunction2", iniFileName)
            systemIni.LocalFunction3 = GetIniValue("RsLinkToAntifat", "LocalFunction3", iniFileName)
            If GetIniValue("RsLinkToAntifat", "IsCtrl", iniFileName) = "True" Then
                systemIni.IsCtrl = True
            Else
                systemIni.IsCtrl = False
            End If
            If GetIniValue("RsLinkToAntifat", "IsAlt", iniFileName) = "True" Then
                systemIni.IsAlt = True
            Else
                systemIni.IsAlt = False
            End If
            If GetIniValue("RsLinkToAntifat", "IsShift", iniFileName) = "True" Then
                systemIni.IsShift = True
            Else
                systemIni.IsShift = False
            End If
            Dim sql = GetIniValue("RsLinkToAntifat", "SQL-NAME", iniFileName)
            Dim ip = GetIniValue("RsLinkToAntifat", "SQL-IP", iniFileName)
            Dim user = GetIniValue("RsLinkToAntifat", "SQL-USER", iniFileName)
            Dim pw = GetIniValue("RsLinkToAntifat", "SQL-PW", iniFileName)
            If (sql <> "") Then
                systemIni.SQLNAME = sql
            End If
            If (ip <> "") Then
                systemIni.SQLIP = ip
            End If
            If (user <> "") Then
                systemIni.SQLUSER = user
            End If
            If (pw <> "") Then
                systemIni.SQLPW = pw
            End If

        End If
        Me.TextBox1.Text = systemIni.Server
        Select Case systemIni.His
            Case "RS"
                Me.ComboBoxHis.SelectedIndex = 0
            Case "VISW"
                Me.ComboBoxHis.SelectedIndex = 1
            Case "TECH"
                Me.ComboBoxHis.SelectedIndex = 2
            Case "DHA"
                Me.ComboBoxHis.SelectedIndex = 3
            Case "SC"
                Me.ComboBoxHis.SelectedIndex = 4
            Case "KN"
                Me.ComboBoxHis.SelectedIndex = 5
            Case "MTR"
                Me.ComboBoxHis.SelectedIndex = 6
        End Select
        Dim key As Integer = 4
        If systemIni.HotKey <> "" Then
            key = Convert.ToInt32(systemIni.HotKey.Substring(1))
        End If

        Me.CheckBoxCtrl.Checked = systemIni.IsCtrl
        Me.CheckBoxAlt.Checked = systemIni.IsAlt
        Me.CheckBoxShift.Checked = systemIni.IsShift

        Me.ComboBoxHotKey.SelectedIndex = key - 1
        Me.ComboBoxF1.Text = systemIni.F1
        Me.ComboBoxF2.Text = systemIni.F2
        Me.ComboBoxF3.Text = systemIni.F3
        Me.ComboBoxF4.Text = systemIni.F4
        Me.ComboBoxF5.Text = systemIni.F5
        Me.ComboBoxF6.Text = systemIni.F6
        Me.ComboBoxF7.Text = systemIni.F7
        Me.ComboBoxF8.Text = systemIni.F8
        Me.ComboBoxF9.Text = systemIni.F9
        Me.ComboBoxF10.Text = systemIni.F10
        Me.ComboBoxF11.Text = systemIni.F11
        Me.ComboBoxF12.Text = systemIni.F12

        '
        '新建一個方鼎連接字串
        '
        resetConfiguration()

    End Sub

    ''' <summary>
    ''' 新建一個連接字串
    ''' </summary>
    Private Sub resetConfiguration()
        Dim config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
        Dim connectionStringsSection As ConnectionStringsSection = config.GetSection("connectionStrings")

        '
        ' 修正方鼎資料庫連線
        '
        Dim newConString = "metadata=res://*/ModelTgk.csdl|res://*/ModelTgk.ssdl|res://*/ModelTgk.msl;provider=System.Data.SqlClient;provider connection string='data source=" + systemIni.SQLIP + ";initial catalog=" + systemIni.SQLNAME + ";user id=" + systemIni.SQLUSER + ";password=" + systemIni.SQLPW + ";MultipleActiveResultSets=True;App=EntityFramework'"
        connectionStringsSection.ConnectionStrings("tgkEntities").ConnectionString = newConString
        '
        ' 修正開蘭資料庫連線
        '
        Dim newConStringKn = "metadata=res://*/ModelKn.csdl|res://*/ModelKn.ssdl|res://*/ModelKn.msl;provider=System.Data.SqlClient;provider connection string='data source=" + systemIni.SQLIP + ";initial catalog=" + systemIni.SQLNAME + ";user id=" + systemIni.SQLUSER + ";password=" + systemIni.SQLPW + ";MultipleActiveResultSets=True;App=EntityFramework'"
        connectionStringsSection.ConnectionStrings("knEntities").ConnectionString = newConStringKn

        config.Save()
        ConfigurationManager.RefreshSection("connectionStrings")
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        systemIni.Server = Me.TextBox1.Text
    End Sub
    Private Sub form1_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If currentOcrEdit = "S" Then
            Select Case e.KeyCode
                Case Keys.Left
                    If e.Modifiers = Keys.Shift Then
                        systemIni.SWidth = systemIni.SWidth - 1
                    Else
                        systemIni.SPosiX = systemIni.SPosiX - 1
                    End If
                Case Keys.Right
                    If e.Modifiers = Keys.Shift Then
                        systemIni.SWidth = systemIni.SWidth + 1
                    Else
                        systemIni.SPosiX = systemIni.SPosiX + 1
                    End If
                Case Keys.Up
                    If e.Modifiers = Keys.Shift Then
                        systemIni.SHeight = systemIni.SHeight - 1
                    Else
                        systemIni.SPosiY = systemIni.SPosiY - 1
                    End If
                Case Keys.Down
                    If e.Modifiers = Keys.Shift Then
                        systemIni.SHeight = systemIni.SHeight + 1
                    Else
                        systemIni.SPosiY = systemIni.SPosiY + 1
                    End If
            End Select
        Else
            Select Case e.KeyCode
                Case Keys.Left
                    If e.Modifiers = Keys.Shift Then
                        systemIni.ZWidth = systemIni.ZWidth - 1
                    Else
                        systemIni.ZPosiX = systemIni.ZPosiX - 1
                    End If
                Case Keys.Right
                    If e.Modifiers = Keys.Shift Then
                        systemIni.ZWidth = systemIni.ZWidth + 1
                    Else
                        systemIni.ZPosiX = systemIni.ZPosiX + 1
                    End If
                Case Keys.Up
                    If e.Modifiers = Keys.Shift Then
                        systemIni.ZHeight = systemIni.ZHeight - 1
                    Else
                        systemIni.ZPosiY = systemIni.ZPosiY - 1
                    End If
                Case Keys.Down
                    If e.Modifiers = Keys.Shift Then
                        systemIni.ZHeight = systemIni.ZHeight + 1
                    Else
                        systemIni.ZPosiY = systemIni.ZPosiY + 1
                    End If
            End Select
        End If
        reDrawBlock()
    End Sub
    Private Sub buttonS_Click(sender As Object, e As EventArgs) Handles buttonS.Click
        Me.Height = 716
        Me.Panel1.Visible = False
        Me.PictureBox1.Visible = True
        currentOcrEdit = "S"
        createScreenShot()
        reDrawBlock()
    End Sub
    Private Sub ButtonZ_Click(sender As Object, e As EventArgs) Handles ButtonZ.Click
        Me.Height = 716
        Me.Panel1.Visible = False
        Me.PictureBox1.Visible = True
        currentOcrEdit = "Z"
        createScreenShot()
        reDrawBlock()
    End Sub
    Private Sub ButtonHotKey_Click(sender As Object, e As EventArgs) Handles ButtonHotKey.Click
        Me.Height = 416
        Me.Panel1.Visible = True
        Me.PictureBox1.Visible = False
        currentOcrEdit = "HotKey"
    End Sub
    ''' <summary>
    ''' 截取畫面作 OCR 定位
    ''' </summary>
    Private Sub createScreenShot()
        Dim bit = New System.Drawing.Bitmap(900, 676)
        Dim screenshot = New System.Drawing.Bitmap(900, 676, System.Drawing.Imaging.PixelFormat.Format32bppRgb)
        Dim graph = Graphics.FromImage(screenshot)
        graph.CopyFromScreen(0, 0, 0, 0, bit.Size, CopyPixelOperation.SourceCopy)
        Me.PictureBox1.Image = screenshot
    End Sub
    ''' <summary>
    ''' OCR 定位 block
    ''' </summary>
    Private Sub reDrawBlock()
        Me.PictureBox1.Refresh()
        Dim gc = Me.PictureBox1.CreateGraphics()
        Dim pen As New Pen(Color.White, 2)
        Dim brush As New SolidBrush(Color.FromArgb(80, 0, 0, 0))
        Dim rect As Rectangle
        If currentOcrEdit = "S" Then
            rect = New Rectangle(systemIni.SPosiX, systemIni.SPosiY, systemIni.SWidth, systemIni.SHeight)
        Else
            rect = New Rectangle(systemIni.ZPosiX, systemIni.ZPosiY, systemIni.ZWidth, systemIni.ZHeight)
        End If
        gc.DrawRectangle(pen, rect)
        gc.FillRectangle(brush, rect)
    End Sub

    Private Sub ButtonSave_Click(sender As Object, e As EventArgs) Handles ButtonSave.Click

        ' 測試用 post 開網頁
        ' windowOpenByPost()

        Me.Height = 138

        currentOcrEdit = ""

        ' 更新系統設定
        Dim iniFileName = My.Application.Info.DirectoryPath & "\System.ini"
        WritePrivateProfileString("RsLinkToAntifat", "OcrPath", systemIni.OcrPath, iniFileName)
        WritePrivateProfileString("RsLinkToAntifat", "Server", systemIni.Server, iniFileName)
        WritePrivateProfileString("RsLinkToAntifat", "His", systemIni.His, iniFileName)
        WritePrivateProfileString("RsLinkToAntifat", "HotKey", systemIni.HotKey, iniFileName)
        WritePrivateProfileString("RsLinkToAntifat", "F1", systemIni.F1, iniFileName)
        WritePrivateProfileString("RsLinkToAntifat", "F2", systemIni.F2, iniFileName)
        WritePrivateProfileString("RsLinkToAntifat", "F3", systemIni.F3, iniFileName)
        WritePrivateProfileString("RsLinkToAntifat", "F4", systemIni.F4, iniFileName)
        WritePrivateProfileString("RsLinkToAntifat", "F5", systemIni.F5, iniFileName)
        WritePrivateProfileString("RsLinkToAntifat", "F6", systemIni.F6, iniFileName)
        WritePrivateProfileString("RsLinkToAntifat", "F7", systemIni.F7, iniFileName)
        WritePrivateProfileString("RsLinkToAntifat", "F8", systemIni.F8, iniFileName)
        WritePrivateProfileString("RsLinkToAntifat", "F9", systemIni.F9, iniFileName)
        WritePrivateProfileString("RsLinkToAntifat", "F10", systemIni.F10, iniFileName)
        WritePrivateProfileString("RsLinkToAntifat", "F11", systemIni.F11, iniFileName)
        WritePrivateProfileString("RsLinkToAntifat", "F12", systemIni.F12, iniFileName)
        WritePrivateProfileString("RsLinkToAntifat", "SPosiX", systemIni.SPosiX.ToString, iniFileName)
        WritePrivateProfileString("RsLinkToAntifat", "SPosiY", systemIni.SPosiY.ToString, iniFileName)
        WritePrivateProfileString("RsLinkToAntifat", "SWidth", systemIni.SWidth.ToString, iniFileName)
        WritePrivateProfileString("RsLinkToAntifat", "SHeight", systemIni.SHeight.ToString, iniFileName)
        WritePrivateProfileString("RsLinkToAntifat", "ZPosiX", systemIni.ZPosiX.ToString, iniFileName)
        WritePrivateProfileString("RsLinkToAntifat", "ZPosiY", systemIni.ZPosiY.ToString, iniFileName)
        WritePrivateProfileString("RsLinkToAntifat", "ZWidth", systemIni.ZWidth.ToString, iniFileName)
        WritePrivateProfileString("RsLinkToAntifat", "ZHeight", systemIni.ZHeight.ToString, iniFileName)
        WritePrivateProfileString("RsLinkToAntifat", "IsCtrl", systemIni.IsCtrl.ToString, iniFileName)
        WritePrivateProfileString("RsLinkToAntifat", "IsAlt", systemIni.IsAlt.ToString, iniFileName)
        WritePrivateProfileString("RsLinkToAntifat", "IsShift", systemIni.IsShift.ToString, iniFileName)
        WritePrivateProfileString("RsLinkToAntifat", "SQL-NAME", systemIni.SQLNAME, iniFileName)
        WritePrivateProfileString("RsLinkToAntifat", "SQL-IP", systemIni.SQLIP, iniFileName)
        WritePrivateProfileString("RsLinkToAntifat", "SQL-USER", systemIni.SQLUSER, iniFileName)
        WritePrivateProfileString("RsLinkToAntifat", "SQL-PW", systemIni.SQLPW, iniFileName)
        WritePrivateProfileString("RsLinkToAntifat", "LocalFunction1", systemIni.LocalFunction1, iniFileName)
        WritePrivateProfileString("RsLinkToAntifat", "LocalFunction2", systemIni.LocalFunction2, iniFileName)
        WritePrivateProfileString("RsLinkToAntifat", "LocalFunction3", systemIni.LocalFunction3, iniFileName)

        MessageBox.Show("設定存檔完成 !!", "設定存檔", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub windowOpenByPost()
        ' Using WebRequest to send data to a website
        Dim request = WebRequest.Create("http: //www.weightobserver.com.tw")
        ' Set method property
        request.Method = "POST"
        ' Create POST data 
        Dim postData = ""
        ' And convert it to byte array
        Dim dataBuffer As Byte() = System.Text.Encoding.UTF8.GetBytes(postData)
        ' Set ContentType property of the request
        request.ContentType = "application/x-www-form-urlencoded"
        ' Set ContentLength property of the request
        request.ContentLength = dataBuffer.Length

        ' Get request stream
        Dim postStream = request.GetRequestStream()
        ' Write data to request stream
        postStream.Write(dataBuffer, 0, dataBuffer.Length)
        postStream.Close()

        Dim response As HttpWebResponse = request.GetResponse()
        Dim Stream = response.GetResponseStream()
        Dim reader = New StreamReader(Stream, System.Text.Encoding.GetEncoding("utf-8"))
        Dim data = reader.ReadToEnd()
        Stream.Close()
        reader.Close()
        response.Close()

    End Sub
    Private Sub button1_Click(sender As Object, e As EventArgs)
        If m_HookHandle = 0 Then
            Using curProcess As Process = Process.GetCurrentProcess()
                Using curModule As ProcessModule = curProcess.MainModule
                    m_KbdHookProc = New HookProc(AddressOf KeyboardHookProc)
                    m_HookHandle = SetWindowsHookEx(WH_KEYBOARD_LL, m_KbdHookProc, GetModuleHandle(curModule.ModuleName), 0)
                End Using
            End Using
            If m_HookHandle = 0 Then
                MessageBox.Show("呼叫 SetWindowsHookEx 失敗!")
                Return
            End If
            'button1.Text = "解除愛半月一鍵連結"
        Else
            Dim ret As Boolean = UnhookWindowsHookEx(m_HookHandle)
            If ret = False Then
                MessageBox.Show("呼叫 UnhookWindowsHookEx 失敗!")
                Return
            End If
            m_HookHandle = 0
            'button1.Text = "啟動愛半月一鍵連結"
        End If
    End Sub

    ''' <summary>
    ''' 分析所有 WindowsFormText
    ''' </summary>
    ''' <param name="hwnd"></param>
    ''' <param name="lParam"></param>
    ''' <returns></returns>
    Public Shared Function GetWindowsFormText(hwnd As IntPtr, lParam As IntPtr) As Integer
        If hwnd <> 0 Then
            Dim buf = New StringBuilder(100)
            GetWindowText(hwnd, buf, 100)
            ' 醫聖 , 展望 判別方式
            If buf.ToString.StartsWith("病歷") Then
                SCTitle = buf.ToString
            End If
            ' 醫聖判別方式
            If buf.ToString.StartsWith("整照") Then
                SCTitle = buf.ToString
            End If
            ' 蒙利特判別方式
            If buf.ToString.StartsWith("【") Then
                SCTitle = buf.ToString
            End If
        End If
        Return hwnd
    End Function

    Private Sub NotifyIcon1_Click(sender As Object, e As EventArgs) Handles NotifyIcon1.Click
        Me.WindowState = FormWindowState.Normal
    End Sub
    Public Function KeyboardHookProc(nCode As Integer, wParam As IntPtr, lParam As IntPtr) As Integer
        ' 當按鍵按下及鬆開時都會觸發此函式，這裡只處理鍵盤按下的情形。
        Dim isPressed As Boolean = (lParam.ToInt32() And &H80000000UI) = 0

        If nCode < 0 OrElse Not isPressed Then
            Return CallNextHookEx(m_HookHandle, nCode, wParam, lParam)
        End If
        ' 取得欲攔截之按鍵狀態
        Dim ctrlKey As KeyStateInfo = KeyboardInfo.GetKeyState(Keys.ControlKey)
        Dim altKey As KeyStateInfo = KeyboardInfo.GetKeyState(Keys.Alt)
        Dim shiftKey As KeyStateInfo = KeyboardInfo.GetKeyState(Keys.ShiftKey)
        Dim Key1 As KeyStateInfo = KeyboardInfo.GetKeyState(Keys.D1)
        Dim f1Key As KeyStateInfo = KeyboardInfo.GetKeyState(Keys.F1)
        Dim f2Key As KeyStateInfo = KeyboardInfo.GetKeyState(Keys.F2)
        Dim f3Key As KeyStateInfo = KeyboardInfo.GetKeyState(Keys.F3)
        Dim f4Key As KeyStateInfo = KeyboardInfo.GetKeyState(Keys.F4)
        Dim f5Key As KeyStateInfo = KeyboardInfo.GetKeyState(Keys.F5)
        Dim f6Key As KeyStateInfo = KeyboardInfo.GetKeyState(Keys.F6)
        Dim f7Key As KeyStateInfo = KeyboardInfo.GetKeyState(Keys.F7)
        Dim f8Key As KeyStateInfo = KeyboardInfo.GetKeyState(Keys.F8)
        Dim f9Key As KeyStateInfo = KeyboardInfo.GetKeyState(Keys.F9)
        Dim f10Key As KeyStateInfo = KeyboardInfo.GetKeyState(Keys.F10)
        Dim f11Key As KeyStateInfo = KeyboardInfo.GetKeyState(Keys.F11)
        Dim f12Key As KeyStateInfo = KeyboardInfo.GetKeyState(Keys.F12)

        Dim functionPressed = ""  ' 快捷鍵要執行的功能
        If Key1.IsPressed And altKey.IsPressed Then
            functionPressed = "個管問診"
        End If
        ' 判斷是否要按 Ctrl 鍵
        If systemIni.IsCtrl Then
            If Not ctrlKey.IsPressed Then
                Return CallNextHookEx(m_HookHandle, nCode, wParam, lParam)
            End If
        End If
        ' 判斷是否要按 Alt 鍵
        If systemIni.IsAlt Then
            If Not altKey.IsPressed Then
                Return CallNextHookEx(m_HookHandle, nCode, wParam, lParam)
            End If
        End If
        ' 判斷是否要按 Shift 鍵
        If systemIni.IsShift Then
            If Not shiftKey.IsPressed Then
                Return CallNextHookEx(m_HookHandle, nCode, wParam, lParam)
            End If
        End If
        ' 判斷各個功能鍵
        If f1Key.IsPressed Then
            If systemIni.HotKey = "F1" Then
                functionPressed = "個管問診"
            ElseIf systemIni.F1 <> "" Then
                functionPressed = systemIni.F1
            End If
        ElseIf f2Key.IsPressed Then
            If systemIni.HotKey = "F2" Then
                functionPressed = "個管問診"
            ElseIf systemIni.F2 <> "" Then
                functionPressed = systemIni.F2
            End If
        ElseIf f3Key.IsPressed Then
            If systemIni.HotKey = "F3" Then
                functionPressed = "個管問診"
            ElseIf systemIni.F3 <> "" Then
                functionPressed = systemIni.F3
            End If
        ElseIf f4Key.IsPressed Then
            If systemIni.HotKey = "F4" Then
                functionPressed = "個管問診"
            ElseIf systemIni.F4 <> "" Then
                functionPressed = systemIni.F4
            End If
        ElseIf f5Key.IsPressed Then
            If systemIni.HotKey = "F5" Then
                functionPressed = "個管問診"
            ElseIf systemIni.F5 <> "" Then
                functionPressed = systemIni.F5
            End If
        ElseIf f6Key.IsPressed Then
            If systemIni.HotKey = "F6" Then
                functionPressed = "個管問診"
            ElseIf systemIni.F6 <> "" Then
                functionPressed = systemIni.F6
            End If
        ElseIf f7Key.IsPressed Then
            If systemIni.HotKey = "F7" Then
                functionPressed = "個管問診"
            ElseIf systemIni.F7 <> "" Then
                functionPressed = systemIni.F7
            End If
        ElseIf f8Key.IsPressed Then
            If systemIni.HotKey = "F8" Then
                functionPressed = "個管問診"
            ElseIf systemIni.F8 <> "" Then
                functionPressed = systemIni.F8
            End If
        ElseIf f9Key.IsPressed Then
            If systemIni.HotKey = "F9" Then
                functionPressed = "個管問診"
            ElseIf systemIni.F9 <> "" Then
                functionPressed = systemIni.F9
            End If
        ElseIf f10Key.IsPressed Then
            If systemIni.HotKey = "F10" Then
                functionPressed = "個管問診"
            ElseIf systemIni.F10 <> "" Then
                functionPressed = systemIni.F10
            End If
        ElseIf f11Key.IsPressed Then
            If systemIni.HotKey = "F11" Then
                functionPressed = "個管問診"
            ElseIf systemIni.F11 <> "" Then
                functionPressed = systemIni.F11
            End If
        ElseIf f12Key.IsPressed Then
            If systemIni.HotKey = "F12" Then
                functionPressed = "個管問診"
            ElseIf systemIni.F12 <> "" Then
                functionPressed = systemIni.F12
            End If
        End If
        If functionPressed <> "" Then
            Dim proc As New System.Diagnostics.Process()
            Dim src As String = ""
            Select Case functionPressed
                Case "本機功能(一)"
                    proc.StartInfo.FileName = systemIni.LocalFunction1
                Case "本機功能(二)"
                    proc.StartInfo.FileName = systemIni.LocalFunction2
                Case "本機功能(三)"
                    proc.StartInfo.FileName = systemIni.LocalFunction3
                Case Else
                    ' create OCR 畫面
                    src = createOCR(functionPressed)
                    If src.StartsWith("hisUvSearch") Then
                        Dim idno = src.Substring(12)
                        ' 讀取門診預約資訊
                        displayUvInformation(hospital, idno)
                        Return CallNextHookEx(m_HookHandle, nCode, wParam, lParam)
                    Else
                        proc.StartInfo.FileName = "chrome.exe"
                    End If
            End Select

            proc.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Maximized
            'proc.StartInfo.Arguments = src
            proc.StartInfo.Arguments = src + " --new-window --start-maximized"
            proc.EnableRaisingEvents = True
            proc.Start()

            ' chrome 關閉時 , 將耀聖系統最大化
            AddHandler proc.Exited, AddressOf procExited

            ' 取得所有 process 的 title
            Dim processList = System.Diagnostics.Process.GetProcesses()
            For i = 0 To processList.Length - 1
                Dim process = processList(i)
                If process.MainWindowTitle <> "" Then
                    Dim title = process.MainWindowTitle
                    If title.Contains("Microsoft Visual FoxPro") Or
                       title.Contains("TGK") Then
                        Dim hisHWnd = FindWindow(Nothing, title)
                        Dim SW_HIDE = 0
                        Dim SW_NORMAL = 1
                        Dim SW_MAXIMIZE = 3
                        Dim SW_SHOWNOACTIVATE = 4
                        Dim SW_SHOW = 5
                        Dim SW_MINIMIZE = 6
                        Dim SW_RESTORE = 9
                        Dim SW_SHOWDEFAULT = 10
                        ShowWindow(hisHWnd, SW_MINIMIZE)
                    End If
                End If
            Next
            'If True Then
            '    Dim foxProTitle = "Microsoft Visual FoxPro"
            '    Dim rsHWnd As Integer = FindWindow(Nothing, foxProTitle)

            '    Dim SW_HIDE = 0
            '    Dim SW_NORMAL = 1
            '    Dim SW_MAXIMIZE = 3
            '    Dim SW_SHOWNOACTIVATE = 4
            '    Dim SW_SHOW = 5
            '    Dim SW_MINIMIZE = 6
            '    Dim SW_RESTORE = 9
            '    Dim SW_SHOWDEFAULT = 10
            '    ShowWindow(rsHWnd, SW_MINIMIZE)
            'End If
        End If
        Return CallNextHookEx(m_HookHandle, nCode, wParam, lParam)
    End Function

    Private Shared Sub displayUvInformation(hno As String, idno As String)
        Dim myWebRequest = WebRequest.Create("http://cloud.weightobserver.com.tw:2031/Appointment/SerachAppointmentNumberByOneClick?HospID=" + hno + "&PatientIdentity=" + idno)
        myWebRequest.Credentials = CredentialCache.DefaultCredentials
        ' Get the response.  
        Dim response = myWebRequest.GetResponse()
        ' Display the status.  
        ' Dim webResponse As HttpWebResponse = response
        ' MessageBox.Show(webResponse.StatusDescription)
        ' Get the stream containing content returned by the server.  
        Dim dataStream = response.GetResponseStream()
        ' Open the stream by using a StreamReader for easy access.  
        Dim reader = New StreamReader(dataStream)
        ' Read the content.  
        Dim result = reader.ReadToEnd()
        ' Clean up the response.  
        reader.Close()
        response.Close()

        result = result.Replace("\""", """")
        result = result.Replace("""[", "[")
        result = result.Replace("]""", "]")

        'Dim ws As New tw.com.weightobserver.cloud.AntiFat
        'Dim result = ws.WS_GetUv("1234567890", 33347, True)

        If Not result.StartsWith("[") Then
            ' 回傳不是 json 格式 , 視為錯誤訊息
            MyMessageBox.Show(result, 400, 140)
        Else
            Try
                Dim serializer As New JavaScriptSerializer
                'Dim uvPojo = New UvPojo
                'uvPojo = serializer.Deserialize(result, uvPojo.GetType)

                Dim uvList As List(Of UvPojo) = serializer.Deserialize(Of List(Of UvPojo))(result)
                If uvList.Count = 0 Then
                    MyMessageBox.Show("【" + idno + "】該病患沒有預約資料 !!", 440, 140)
                Else
                    Dim uvMessage = ""
                    For i = 0 To uvList.Count - 1
                        Dim uvPojo As UvPojo = uvList(i)
                        If uvPojo.PatientReason = Nothing Then
                            uvPojo.PatientReason = ""
                        End If
                        ' 不要換行 , 改用 " , "
                        uvPojo.PatientReason = uvPojo.PatientReason.Replace(vbCrLf, "，")
                        uvPojo.PatientReason = uvPojo.PatientReason.Replace("\n", "，")
                        uvMessage = uvMessage + IIf(uvMessage = "", "", vbCrLf) +
                                    "預約時段：【" + uvPojo.ClinicRoom + "】" + uvPojo.ShiftTime + "【 " + uvPojo.ClinicNumber + " 】" + vbCrLf +
                                    "預約註記：" + uvPojo.PatientReason
                        If uvPojo.Item1 <> "" Then
                            uvMessage = uvMessage + vbCrLf + "預約項目：" + uvPojo.Item1
                        End If
                    Next
                    Dim height = 100
                    height = height + uvList.Count * 50
                    MyMessageBox.Show(uvMessage, 400, height)
                End If
            Catch ex As Exception
                MessageBox.Show(ex.ToString)
            End Try
        End If

        'MessageBox.Show(aa)
        'MyMessageBox.Show("預約時段：【一診】下午【 20 】" + vbCrLf +
        '                      "預約註記：糖尿病抽血" + vbCrLf +
        '                      "預約項目：(IC21)四十歲到六十五歲", 400, 180)
        'MyMessageBox.Show("該病患沒有預約資料 !!", 300, 140)
    End Sub

    Private Shared Sub procExited(ByVal sender As Object, ByVal e As EventArgs)
        'Dim foxProTitle = "Microsoft Visual FoxPro"
        'Dim rsHWnd As Integer = FindWindow(Nothing, foxProTitle)

        'Dim SW_HIDE = 0
        'Dim SW_NORMAL = 1
        'Dim SW_MAXIMIZE = 3
        'Dim SW_SHOWNOACTIVATE = 4
        'Dim SW_SHOW = 5
        'Dim SW_MINIMIZE = 6
        'Dim SW_RESTORE = 9
        'Dim SW_SHOWDEFAULT = 10
        'ShowWindow(rsHWnd, SW_MAXIMIZE)

        ' 取得所有 process 的 title
        Dim processList = System.Diagnostics.Process.GetProcesses()
        For i = 0 To processList.Length - 1
            Dim process = processList(i)
            If process.MainWindowTitle <> "" Then
                Dim title = process.MainWindowTitle
                If title.Contains("Microsoft Visual FoxPro") Or
                       title.Contains("TGK") Then
                    Dim hisHWnd = FindWindow(Nothing, title)
                    Dim SW_HIDE = 0
                    Dim SW_NORMAL = 1
                    Dim SW_MAXIMIZE = 3
                    Dim SW_SHOWNOACTIVATE = 4
                    Dim SW_SHOW = 5
                    Dim SW_MINIMIZE = 6
                    Dim SW_RESTORE = 9
                    Dim SW_SHOWDEFAULT = 10
                    ShowWindow(hisHWnd, SW_MAXIMIZE)
                End If
            End If
        Next

    End Sub


    Private Shared Function createOCR(functionPressed As String) As String
        '
        ' 取得電腦名稱
        '
        Dim computerName = System.Environment.GetEnvironmentVariable("COMPUTERNAME")
        Dim userName = System.Environment.GetEnvironmentVariable("USERNAME")
        Dim computerName_10 = computerName + Space(10)
        computerName_10 = computerName_10.Substring(0, 10)
        '
        ' 取得登入者帳號
        '
        Dim drId As String = "1111"
        Dim drName As String = ""
        ' RS : 耀聖
        If systemIni.His = "RS" AndAlso System.IO.File.Exists(systemIni.Server + ":\Z\travel.dbf") Then
            ' 排除刪除的資料
            Dim connZ As New OdbcConnection("Driver={Microsoft Visual FoxPro Driver};sourcedb=" + systemIni.Server + ":\z;sourcetype=DBF;exclusive=No;backgroundfetch=Yes;collate=Machine;null=Yes;deleted=Yes")    '連接字串
            Dim table As New DataTable
            Dim adatper As New System.Data.Odbc.OdbcDataAdapter("select * from travel where date2=' ' and  memo='" + computerName_10 + "' order by date1 desc , time1 desc", connZ)
            adatper.Fill(table)
            If table.Rows.Count > 0 Then
                For i = 0 To table.Rows.Count - 1
                    drId = table.Rows(i).Item("code")
                    drId = drId.Trim
                    Exit For
                Next
            End If
        End If
        ' VISW : 展望 , 取登入人員會當 , 暫不使用 , 待續
        If False And systemIni.His = "VISW" AndAlso System.IO.File.Exists(systemIni.Server + ":\VISW\vislog.dbf") AndAlso System.IO.File.Exists(systemIni.Server + ":\VISW\vis00.dbf") Then
            ' 排除刪除的資料
            Dim connZ As New OdbcConnection("Driver={Microsoft Visual FoxPro Driver};sourcedb=" + systemIni.Server + ":\VISW;sourcetype=DBF;exclusive=No;backgroundfetch=Yes;collate=Machine;null=Yes;deleted=Yes")    '連接字串
            ' 讀取人員資料
            Dim tableEmp As New DataTable
            Dim adatperEmp As New System.Data.Odbc.OdbcDataAdapter("select * from vis00", connZ)
            adatperEmp.Fill(tableEmp)
            ' create 人員字典
            Dim empDict = New Dictionary(Of String, String)
            For i = 0 To tableEmp.Rows.Count - 1
                Dim Vuser As String = tableEmp.Rows(i).Item("Vuser")
                Dim Vname As String = tableEmp.Rows(i).Item("Vname")
                Vname = Vname.Substring(0, 3)
                If Not empDict.ContainsKey(Vname) Then
                    empDict.Add(Vname, Vuser)
                End If
            Next

            ' 
            ' 排除刪除的資料
            Dim viswKey = computerName + " # " + userName + Space(10)
            viswKey = viswKey.Substring(0, 30)
            Dim table As New DataTable
            Dim adatper As New System.Data.Odbc.OdbcDataAdapter("select * from vislog where Lnet ='" + viswKey + "' order by Ldate desc , Ltime desc", connZ)
            adatper.Fill(table)
            If table.Rows.Count > 0 Then
                For i = 0 To table.Rows.Count - 1
                    drName = table.Rows(i).Item("Lsur")
                    drName = drName.Trim.Substring(0, 3)
                    If empDict.ContainsKey(drName) Then
                        drId = empDict(drName).Trim
                    End If
                    Exit For
                Next
            End If
        End If
        '
        ' 取得院所代號
        '
        Dim hno As String = ""
        ' RS : 耀聖
        If systemIni.His = "RS" AndAlso System.IO.File.Exists(systemIni.Server + ":\S\title.dbf") Then
            ' 排除刪除的資料
            Dim connS As New OdbcConnection("Driver={Microsoft Visual FoxPro Driver};sourcedb=" + systemIni.Server + ":\s;sourcetype=DBF;exclusive=No;backgroundfetch=Yes;collate=Machine;null=Yes;deleted=Yes")    '連接字串
            Dim table As New DataTable
            Dim adatper As New System.Data.Odbc.OdbcDataAdapter("select * from title", connS)
            adatper.Fill(table)
            If table.Rows.Count > 0 Then
                For i = 0 To table.Rows.Count - 1
                    hno = table.Rows(i).Item("h_no")
                    hno = hno.Trim
                    Exit For
                Next
            End If
        End If
        ' VISW : 展望
        If systemIni.His = "VISW" AndAlso System.IO.File.Exists(systemIni.Server + ":\VISW\vispara.dbf") Then
            ' 排除刪除的資料
            Dim connS As New OdbcConnection("Driver={Microsoft Visual FoxPro Driver};sourcedb=" + systemIni.Server + ":\VISW;sourcetype=DBF;exclusive=No;backgroundfetch=Yes;collate=Machine;null=Yes;deleted=Yes")    '連接字串
            Dim table As New DataTable
            Dim adatper As New System.Data.Odbc.OdbcDataAdapter("select * from vispara", connS)
            adatper.Fill(table)
            If table.Rows.Count > 0 Then
                For i = 0 To table.Rows.Count - 1
                    hno = table.Rows(i).Item("cco_no0")
                    hno = hno.Trim
                    Exit For
                Next
            End If
        End If
        ' TECH : 方鼎
        Dim dbTgk As tgkEntities = Nothing
        If systemIni.His = "TECH" Then
            dbTgk = New tgkEntities
            Dim hospitalList = From hospital In dbTgk.院所資料檔

            If hospitalList.Count > 0 Then
                hno = hospitalList.First.院所代號
            End If
        End If
        ' DHA : 常誠
        ' \MAINDATA\BASICSET.dbf   : 院所資料
        ' \MAINDATA\POWER.dbf      : 人員資料
        If systemIni.His = "DHA" AndAlso System.IO.File.Exists(systemIni.Server + ":\MAINDATA\BASICSET.dbf") Then
            Try
                ' 排除刪除的資料
                Dim connS As New OdbcConnection("Driver={Microsoft Visual FoxPro Driver};sourcedb=" + systemIni.Server + ":\MAINDATA;sourcetype=DBF;exclusive=No;backgroundfetch=Yes;collate=Machine;null=Yes;deleted=Yes")    '連接字串
                Dim table As New DataTable
                Dim adatper As New System.Data.Odbc.OdbcDataAdapter("select * from BASICSET", connS)
                adatper.Fill(table)
                If table.Rows.Count > 0 Then
                    hno = table.Rows(68).Item("C_field")
                    hno = hno.Trim
                End If
            Catch ex As Exception
                MessageBox.Show(ex.ToString)
            End Try
        End If
        ' SC : 醫聖
        ' SC\Dat\HospInfo.Set
        If systemIni.His = "SC" AndAlso System.IO.File.Exists(systemIni.Server + ":\SC\Dat\HospInfo.Set") Then
            Dim fileReader As String
            fileReader = My.Computer.FileSystem.ReadAllText(systemIni.Server + ":\SC\Dat\HospInfo.Set",
               System.Text.Encoding.Default)
            hno = fileReader.Substring(10, 10)
        End If
        ' KN : 開蘭
        Dim dbKn As knEntities = Nothing
        If systemIni.His = "KN" Then
            dbKn = New knEntities
            Dim hospitalList = From hospital In dbKn.hosp_basic
            If hospitalList.Count > 0 Then
                hno = hospitalList.First.hosp_ins_no.Trim
            End If
        End If

        ' MTR : 蒙利特
        If systemIni.His = "MTR" AndAlso System.IO.File.Exists(systemIni.Server + ":\MTR\DBF\hostsys.dbf") Then
            ' 排除刪除的資料
            Dim connS As New OdbcConnection("Driver={Microsoft Visual FoxPro Driver};sourcedb=" + systemIni.Server + ":\MTR\DBF;sourcetype=DBF;exclusive=No;backgroundfetch=Yes;collate=Machine;null=Yes;deleted=Yes")    '連接字串
            Dim table As New DataTable
            Dim adatper As New System.Data.Odbc.OdbcDataAdapter("select * from hostsys", connS)
            adatper.Fill(table)
            If table.Rows.Count > 0 Then
                For i = 0 To table.Rows.Count - 1
                    hno = table.Rows(i).Item("H1")
                    hno = hno.Trim
                    Exit For
                Next
            End If
        End If
        ' 記錄院所代號
        hospital = hno
        Try
            Dim code As String = ""
            Dim line As String
            Dim is_OCR_Got = False ' 是否已完成 OCR 辨識
            Dim doOcr = True       ' 是否要執行 OCR , dolin 測試時關閉
            ' 醫聖,蒙利特,展望 先使用 window form 的 tilte 來判別病患
            If systemIni.His = "SC" Then
                ' 先將 WindowsFormText 清除
                SCTitle = ""
                ' 分析所有 WindowsFormText
                Dim m_EnumWindowsProc = New EnumWindowsProc(AddressOf Form1.GetWindowsFormText)
                EnumWindows(m_EnumWindowsProc, 0)
                If SCTitle <> "" Then
                    Dim _title = SCTitle.Substring(3)
                    If _title.Contains(" ") Then
                        _title = _title.Substring(0, _title.IndexOf(" "))
                        code = _title
                        is_OCR_Got = True
                    End If
                End If
            End If
            If systemIni.His = "MTR" Then
                ' 先將 WindowsFormText 清除
                SCTitle = ""
                ' 分析所有 WindowsFormText
                Dim m_EnumWindowsProc = New EnumWindowsProc(AddressOf Form1.GetWindowsFormText)
                EnumWindows(m_EnumWindowsProc, 0)
                If SCTitle <> "" Then
                    code = SCTitle.Substring(1, 6)
                    is_OCR_Got = True
                End If
            End If
            If systemIni.His = "VISW" Then
                ' 病歷號：0000001 姓名：Dolin
                ' 先將 WindowsFormText 清除
                SCTitle = ""
                ' 分析所有 WindowsFormText
                Dim m_EnumWindowsProc = New EnumWindowsProc(AddressOf Form1.GetWindowsFormText)
                EnumWindows(m_EnumWindowsProc, 0)
                If SCTitle <> "" Then
                    Dim _title = SCTitle.Substring(4)
                    If _title.Contains(" ") Then
                        _title = _title.Substring(0, _title.IndexOf(" "))
                        code = _title
                        is_OCR_Got = True
                    End If
                End If
            End If
            ' 沒有取到病歷號時 , 使用 OCR 判讀
            If doOcr = True And is_OCR_Got = False Then
                Dim bitS = New System.Drawing.Bitmap(systemIni.SWidth, systemIni.SHeight)
                Dim bitZ = New System.Drawing.Bitmap(systemIni.ZWidth, systemIni.ZHeight)
                Dim screenshotS = New System.Drawing.Bitmap(systemIni.SWidth, systemIni.SHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb)
                Dim screenshotZ = New System.Drawing.Bitmap(systemIni.ZWidth, systemIni.ZHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb)
                Dim graphS = Graphics.FromImage(screenshotS)
                Dim graphZ = Graphics.FromImage(screenshotZ)
                graphS.CopyFromScreen(systemIni.SPosiX, systemIni.SPosiY, 0, 0, bitS.Size, CopyPixelOperation.SourceCopy)
                graphZ.CopyFromScreen(systemIni.ZPosiX, systemIni.ZPosiY, 0, 0, bitZ.Size, CopyPixelOperation.SourceCopy)
                screenshotS.Save(systemIni.OcrPath & "\tempS.bmp", System.Drawing.Imaging.ImageFormat.Bmp)
                screenshotZ.Save(systemIni.OcrPath & "\tempZ.bmp", System.Drawing.Imaging.ImageFormat.Bmp)

                ' OCR 轉換 
                Dim ocrFile = systemIni.OcrPath & "\tempS.txt"
                If System.IO.File.Exists(ocrFile) = True Then
                    System.IO.File.Delete(ocrFile)
                End If
                ocrFile = systemIni.OcrPath & "\tempZ.txt"
                If System.IO.File.Exists(ocrFile) = True Then
                    System.IO.File.Delete(ocrFile)
                End If
                ' OCR 轉換 設為最小化
                Dim proc As New System.Diagnostics.Process()
                proc.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Minimized
                proc.StartInfo.FileName = systemIni.OcrPath & "\tesseract.exe"

                Dim ocrSrcS = systemIni.OcrPath & "\tempS.bmp " & systemIni.OcrPath & "\tempS"
                Dim ocrSrcZ = systemIni.OcrPath & "\tempZ.bmp " & systemIni.OcrPath & "\tempZ"
                ' OCR 轉換 掛號室
                proc.StartInfo.Arguments = ocrSrcS
                proc.Start()
                ' OCR 轉換 診間
                proc.StartInfo.Arguments = ocrSrcZ
                proc.Start()

                '
                ' 等待轉換後的文字檔
                '
                Threading.Thread.Sleep(500)
                '
                ' 取出轉換後的文字檔
                '
                If System.IO.File.Exists(systemIni.OcrPath & "\tempS.txt") Then
                    Using sr As New StreamReader(systemIni.OcrPath & "\tempS.txt")
                        line = sr.ReadToEnd()
                        code = line
                        code = code.Trim
                        code = code.Replace(" ", "")
                        code = code.Replace("O", "0")
                        code = code.Replace("]", "1")
                        code = code.Replace("l", "1")
                        code = code.Replace("I", "1")
                        code = code.Replace("—", "-")
                        code = code.Replace(",", "-")
                        code = code.Replace("()", "0")
                        code = code.Replace("Z", "2")
                    End Using
                End If
                ' 耀聖病歷號 000012 , 6 碼
                If systemIni.His = "RS" AndAlso code.Length = 6 AndAlso IsNumeric(code) Then
                    is_OCR_Got = True
                End If
                ' 展望病歷號 0000012 , 7 碼
                If systemIni.His = "VISW" AndAlso code.Length = 7 AndAlso IsNumeric(code) Then
                    is_OCR_Got = True
                End If
                ' 醫聖病歷號 000012 , 6 碼
                If systemIni.His = "SC" AndAlso code.Length = 6 AndAlso IsNumeric(code) Then
                    is_OCR_Got = True
                End If
                ' 常誠病歷號 000012 , 6 碼
                If systemIni.His = "DHA" AndAlso code.Length = 6 AndAlso IsNumeric(code) Then
                    is_OCR_Got = True
                End If
                If systemIni.His = "TECH" Then
                    dbTgk = New tgkEntities
                    Dim patientList = From patient In dbTgk.病患檔
                                      Where patient.病歷號碼 = code

                    If patientList.Count > 0 Then
                        ' 直接取到病歷號碼
                        is_OCR_Got = True
                    Else
                        ' 方鼎病歷號 000123(6碼) 或 0601201-2(生日-序) 或 1234(四碼) 或 1234-1
                        Dim iniCode = code
                        Dim subCode = "xxxxxx"
                        Dim i = 0
                        Dim isGot = False
                        ' 先取出連續 8 ~ 4 個數字(含-)
                        For i = 0 To iniCode.Length - 1
                            For j = 8 To 4 Step -1
                                If iniCode.Length - i >= j Then
                                    subCode = iniCode.Substring(i, j)
                                    If MyIsNumeric(subCode) Then
                                        isGot = True
                                        Exit For
                                    End If
                                End If
                            Next
                            If isGot Then
                                Exit For
                            End If
                        Next
                        If MyIsNumeric(subCode) Then
                            code = subCode
                            is_OCR_Got = True
                        End If
                    End If
                End If
            End If
            If systemIni.His = "KN" Then
                Dim chr_no = Val(code)
                dbKn = New knEntities
                Dim patientList = From patient In dbKn.chart
                                  Where patient.chr_no = chr_no

                If patientList.Count > 0 Then
                    ' 直接取到病歷號碼
                    is_OCR_Got = True
                End If
            End If
            If doOcr = True And is_OCR_Got = False Then
                If System.IO.File.Exists(systemIni.OcrPath & "\tempZ.txt") Then
                    Using sr As New StreamReader(systemIni.OcrPath & "\tempZ.txt")
                        line = sr.ReadToEnd()
                        code = line
                        code = code.Trim
                        code = code.Replace(" ", "")
                        code = code.Replace("O", "0")
                        code = code.Replace("]", "1")
                        code = code.Replace("l", "1")
                        code = code.Replace("I", "1")
                        code = code.Replace("—", "-")
                        code = code.Replace(",", "-")
                        code = code.Replace("()", "0")
                        code = code.Replace("Z", "2")
                    End Using
                    If systemIni.His = "TECH" Then
                        dbTgk = New tgkEntities
                        Dim patientList = From patient In dbTgk.病患檔
                                          Where patient.病歷號碼 = code

                        ' 沒有取到病歷號碼 , 再作處理
                        If patientList.Count = 0 Then
                            ' 方鼎病歷號 000123(6碼) 或 0601201-2(生日-序) 或 1234(四碼) 或 1234-1
                            Dim iniCode = code
                            Dim subCode = "xxxxxx"
                            Dim i = 0
                            Dim isGot = False
                            ' 先取出連續 8 ~ 4 個數字(含-)
                            For i = 0 To iniCode.Length - 1
                                For j = 8 To 4 Step -1
                                    If iniCode.Length - i >= j Then
                                        subCode = iniCode.Substring(i, j)
                                        If MyIsNumeric(subCode) Then
                                            isGot = True
                                            Exit For
                                        End If
                                    End If
                                Next
                                If isGot Then
                                    Exit For
                                End If
                            Next
                            If MyIsNumeric(subCode) Then
                                code = subCode
                                is_OCR_Got = True
                            End If
                        End If
                    End If
                End If
            End If
            ' 取病歷號
            Dim recno As Integer = 0
            ' 方鼎不能直接轉成病歷號
            If code <> "" And systemIni.His <> "TECH" Then
                recno = Integer.Parse(code)
            End If
            Dim patientStrRecno As String = ""
            Dim patientName As String = ""
            Dim patientIdno As String = ""
            Dim patientTel As String = ""
            Dim patientCell As String = ""
            Dim patientBirth As String = ""
            Dim patientAddr As String = ""

            '
            ' 取得病患資料
            '
            If systemIni.His = "RS" AndAlso System.IO.File.Exists(systemIni.Server + ":\S\patdb.dbf") Then
                ' 不排除刪除的資料
                Dim connS As New OdbcConnection("Driver={Microsoft Visual FoxPro Driver};sourcedb=" + systemIni.Server + ":\s;sourcetype=DBF;exclusive=No;backgroundfetch=Yes;collate=Machine;null=Yes;deleted=No")    '連接字串
                Dim table As New DataTable
                Dim adatper As New System.Data.Odbc.OdbcDataAdapter("select * from patdb where recno()=" + recno.ToString, connS)
                adatper.Fill(table)
                If table.Rows.Count > 0 Then
                    For i = 0 To table.Rows.Count - 1
                        patientName = table.Rows(i).Item("name")
                        patientIdno = table.Rows(i).Item("id")
                        patientTel = table.Rows(i).Item("tel")
                        patientBirth = table.Rows(i).Item("birth")
                        Try
                            ' 檢查是否有 mobil 欄位
                            patientCell = table.Rows(i).Item("mobil")
                        Catch ex As Exception
                            patientCell = ""
                        End Try

                        patientIdno = patientIdno.Trim
                        patientTel = patientTel.Trim
                        patientBirth = patientBirth.Trim
                        If patientBirth.Substring(0, 1) = "A" Then
                            patientBirth = "10" + patientBirth.Substring(1)
                        ElseIf patientBirth.Substring(0, 1) = "B" Then
                            patientBirth = "11" + patientBirth.Substring(1)
                        ElseIf patientBirth.Substring(0, 1) = "C" Then
                            patientBirth = "12" + patientBirth.Substring(1)
                        End If
                    Next
                End If
            End If
            If systemIni.His = "VISW" AndAlso System.IO.File.Exists(systemIni.Server + ":\VISW\CO01M.dbf") Then
                ' 排除刪除資料
                Dim connS As New OdbcConnection("Driver={Microsoft Visual FoxPro Driver};sourcedb=" + systemIni.Server + ":\visw;sourcetype=DBF;exclusive=No;backgroundfetch=Yes;collate=Machine;null=Yes;deleted=Yes")    '連接字串
                Dim table As New DataTable
                Dim adatper As New System.Data.Odbc.OdbcDataAdapter("select * from CO01m where Kcstmr = '" + code + "'", connS)
                adatper.Fill(table)
                If table.Rows.Count > 0 Then
                    patientName = table.Rows(0).Item("Mname")
                    patientIdno = table.Rows(0).Item("Mpersonid")
                    patientTel = table.Rows(0).Item("Mtelh")
                    patientCell = table.Rows(0).Item("Mrec")
                    patientBirth = table.Rows(0).Item("Mbirthdt")
                    patientIdno = patientIdno.Trim
                    patientTel = patientTel.Trim
                    patientBirth = patientBirth.Trim
                End If
            End If
            If systemIni.His = "TECH" Then
                dbTgk = New tgkEntities
                Dim patientList = From patient In dbTgk.病患檔
                                  Where patient.病歷號碼 = code

                If patientList.Count > 0 Then
                    Dim patientTgk = patientList.First
                    recno = patientTgk.Counter
                    code = (recno + 1000000).ToString.Substring(1)
                    patientStrRecno = patientTgk.病歷號碼
                    patientName = patientTgk.姓名
                    patientIdno = patientTgk.身份證字號
                    patientBirth = patientTgk.生日
                    If patientTgk.電話H <> Nothing Then
                        patientTel = patientTgk.電話H
                    End If
                    If patientTgk.BBC <> Nothing Then
                        patientCell = patientTgk.BBC
                    End If
                    patientIdno = patientIdno.Trim
                    patientBirth = patientBirth.Trim
                    patientTel = patientTel.Trim
                    patientCell = patientCell.Trim
                    ' 電話補 0
                    If patientTel <> "" Then
                        patientTel = "0" + patientTel
                    End If
                    ' 行動補 0
                    If patientCell <> "" Then
                        patientCell = "0" + patientCell
                    End If

                End If
            End If
            If systemIni.His = "DHA" AndAlso System.IO.File.Exists(systemIni.Server + ":\DATA\PD011M1.dbf") Then
                ' 排除刪除資料
                Dim connS As New OdbcConnection("Driver={Microsoft Visual FoxPro Driver};sourcedb=" + systemIni.Server + ":\DATA;sourcetype=DBF;exclusive=No;backgroundfetch=Yes;collate=Machine;null=Yes;deleted=Yes")    '連接字串
                Dim table As New DataTable
                Dim adatper As New System.Data.Odbc.OdbcDataAdapter("select * from PD011M1 where Num = '" + code + "'", connS)
                adatper.Fill(table)
                If table.Rows.Count > 0 Then
                    patientName = table.Rows(0).Item("name")
                    patientIdno = table.Rows(0).Item("id")
                    patientTel = table.Rows(0).Item("tel")
                    patientCell = table.Rows(0).Item("act_tel")
                    patientBirth = table.Rows(0).Item("birth")
                    patientIdno = patientIdno.Trim
                    patientTel = patientTel.Trim
                    patientBirth = patientBirth.Trim

                    Dim year As String = (Val(patientBirth.Split("/")(0)) - 1911 + 1000).ToString.Substring(1)
                    Dim month As String = (Val(patientBirth.Split("/")(1)) + 100).ToString.Substring(1)
                    Dim day As String = (Val(patientBirth.Split("/")(2)) + 100).ToString.Substring(1)
                    patientBirth = year + month + day
                End If
            End If
            If systemIni.His = "SC" AndAlso System.IO.File.Exists(systemIni.Server + ":\SC\Dat\User.dat") Then
                ' 排除刪除資料
                Dim connS As New OdbcConnection("Driver={Microsoft Visual FoxPro Driver};sourcedb=" + systemIni.Server + ":\SC\Dat;sourcetype=DBF;exclusive=No;backgroundfetch=Yes;collate=Machine;null=Yes;deleted=Yes")    '連接字串
                Dim table As New DataTable
                Dim adatper As New System.Data.Odbc.OdbcDataAdapter("select * from User.dat where MEDICAL_NO = '" + code + "'", connS)
                adatper.Fill(table)
                If table.Rows.Count > 0 Then
                    patientName = table.Rows(0).Item("NAME")
                    patientIdno = table.Rows(0).Item("ID")
                    patientTel = table.Rows(0).Item("TEL")
                    patientCell = table.Rows(0).Item("TEL2")
                    patientBirth = table.Rows(0).Item("BIRTHDAY")
                    patientIdno = patientIdno.Trim
                    patientTel = patientTel.Trim
                    patientBirth = patientBirth.Trim
                    patientBirth = patientBirth.Replace(".", "")
                End If
            End If
            If systemIni.His = "KN" Then
                dbKn = New knEntities
                Dim chr_no = Val(code)
                Dim patientList = From patient In dbKn.chart
                                  Where patient.chr_no = chr_no

                If patientList.Count > 0 Then
                    Dim patientKn = patientList.First
                    recno = patientKn.chr_no
                    code = (recno + 1000000).ToString.Substring(1)
                    patientName = patientKn.pt_name
                    patientIdno = patientKn.id_no
                    patientBirth = patientKn.birth_date
                    If patientKn.tel_no <> Nothing Then
                        patientTel = patientKn.tel_no
                    End If
                    If patientKn.cell_phone <> Nothing Then
                        patientCell = patientKn.cell_phone
                    End If
                    patientIdno = patientIdno.Trim
                    patientBirth = patientBirth.Trim
                    patientTel = patientTel.Trim
                    patientCell = patientCell.Trim
                End If
            End If
            If systemIni.His = "MTR" AndAlso System.IO.File.Exists(systemIni.Server + ":\MTR\DBF\Client.dbf") Then
                Dim tel2 = ""
                ' 排除刪除的資料
                Dim connS As New OdbcConnection("Driver={Microsoft Visual FoxPro Driver};sourcedb=" + systemIni.Server + ":\MTR\DBF;sourcetype=DBF;exclusive=No;backgroundfetch=Yes;collate=Machine;null=Yes;deleted=Yes")    '連接字串
                Dim table As New DataTable
                Dim adatper As New System.Data.Odbc.OdbcDataAdapter("select * from client where medical_no='" + code + "'", connS)
                adatper.Fill(table)
                If table.Rows.Count > 0 Then
                    For i = 0 To table.Rows.Count - 1
                        patientName = table.Rows(i).Item("name")
                        patientIdno = table.Rows(i).Item("id")
                        patientTel = table.Rows(i).Item("tel")
                        tel2 = table.Rows(i).Item("tel2")
                        patientCell = table.Rows(i).Item("bigtel")
                        patientBirth = table.Rows(i).Item("birthday")
                        patientAddr = table.Rows(i).Item("Address")
                        patientIdno = patientIdno.Trim
                        patientTel = patientTel.Trim
                        patientCell = patientCell.Trim
                        patientBirth = patientBirth.Trim
                        patientBirth = patientBirth.Replace(".", "")
                    Next
                End If
                patientTel = patientTel.Trim
                tel2 = tel2.Trim
                patientCell = patientCell.Trim
                ' 沒有手機 但 電話2 為 10 碼 , 電話2 填入 手機
                If patientCell = "" And tel2.Length = 10 Then
                    patientCell = tel2
                End If
                ' 沒有電話 但 電話2 不為 10 碼 , 電話2 填入 電話
                If patientTel = "" And tel2.Length <> 10 Then
                    patientTel = tel2
                End If
            End If
            patientName = patientName.Trim

            Dim functionAntifat = "his"
            Select Case functionPressed
                Case "個管問診"
                    functionAntifat = "his"
                Case "檢驗病歷"
                    functionAntifat = "hisLaboratory"
                Case "檢查報告"
                    functionAntifat = "hisCustomTable"
                Case "量測資料"
                    functionAntifat = "hisBodyMeasure"
                Case "預約作業"
                    functionAntifat = "hisEquipment"
                Case "檢驗預約"
                    functionAntifat = "hisLabAppoint"
                Case "關懷記錄"
                    functionAntifat = "hisWorkRecord"
                Case "圖形管理"
                    functionAntifat = "hisImageManage"
                Case "身體組成"
                    functionAntifat = "bodyMeasure"
                Case "門診預約"
                    functionAntifat = "hisUv"
                Case "門診預約查詢"
                    functionAntifat = "hisUvSearch"
                Case "智抗糖"
                    functionAntifat = "Health2"
                Case "本機功能(一)"
                    functionAntifat = "localFunction1"
                Case "本機功能(二)"
                    functionAntifat = "localFunction2"
                Case "本機功能(三)"
                    functionAntifat = "localFunction3"
            End Select

            If functionAntifat = "hisUv" Then
                Dim srcUcare = "http://cloud.weightobserver.com.tw:2031/ClinicAppointment?HospID=" + hno + "&PatientIdentity=" + patientIdno + "&PatientName=" + patientName
                Return srcUcare
            End If

            If functionAntifat = "bodyMeasure" Then
                Dim srcUcare = "http://ucare.netown.tw/Report/Yilan/report/printreport_001.aspx?ax33889=" + patientIdno
                Return srcUcare
            End If
            If functionAntifat = "Health2" Then
                Dim srcUcare = "https://www.health2sync.com/wellness/redirect/" + patientIdno
                Return srcUcare
            End If
            If functionAntifat.StartsWith("localFunction") Then
                Dim srcUcare = functionAntifat
                Return srcUcare
            End If
            Dim src = AntifatHttp + "/" + functionAntifat + "?hospital=" & hno & "&dr=" & drId
            If code <> "" Then
                src = src & "&patientno=" & code
            End If
            If patientName <> "" Then
                src = src & "&patientname=" & patientName
            End If
            If patientIdno <> "" Then
                src = src & "&patientIdno=" & patientIdno
            End If
            If patientBirth <> "" Then
                src = src & "&patientBirth=" & patientBirth
            End If
            If patientTel <> "" Then
                src = src & "&tel=" & patientTel
            End If
            If patientCell <> "" Then
                src = src & "&cell=" & patientCell
            End If
            If patientAddr <> "" Then
                src = src & "&addr=" & patientAddr
            End If
            If patientStrRecno <> "" Then
                src = src & "&strRecno=" & patientStrRecno
            End If
            If functionPressed = "門診預約查詢" Then
                ' 回傳身份證
                src = "hisUvSearch:" + patientIdno
            End If
            Return src
        Catch ex As Exception
            Dim src = ""
            If functionPressed = "門診預約查詢" Then
                src = "hisUvSearch:"  ' 找不到病患時回傳空白
            ElseIf functionPressed = "門診預約" Then
                Dim patientIdno = ""
                Dim patientName = ""
                Dim srcUcare = "http://cloud.weightobserver.com.tw:2031/ClinicAppointment?HospID=" + hno + "&PatientIdentity=" + patientIdno + "&PatientName=" + patientName
                Return srcUcare
            Else
                src = AntifatHttp + "/his?hospital=" & hno & "&dr=" & drId
            End If
            Return src
        End Try

    End Function

    Public Class KeyboardInfo
        Private Sub New()
        End Sub

        <DllImport("user32")>
        Private Shared Function GetKeyState(vKey As Integer) As Short
        End Function

        Public Shared Function GetKeyState(key As Keys) As KeyStateInfo
            Dim vkey As Integer = CInt(key)

            If key = Keys.Alt Then
                ' VK_ALT
                vkey = &H12
            End If

            Dim keyState As Short = GetKeyState(vkey)
            Dim low__1 As Integer = Low(keyState)
            Dim high__2 As Integer = High(keyState)
            Dim toggled As Boolean = (low__1 = 1)
            Dim pressed As Boolean = (high__2 = 1)

            Return New KeyStateInfo(key, pressed, toggled)
        End Function

        Private Shared Function High(keyState As Integer) As Integer
            If keyState > 0 Then
                Return keyState >> &H10
            Else
                Return (keyState >> &H10) And &H1
            End If

        End Function

        Private Shared Function Low(keyState As Integer) As Integer
            Return keyState And &HFFFF
        End Function
    End Class


    Public Structure KeyStateInfo
        Private m_Key As Keys
        Private m_IsPressed As Boolean
        Private m_IsToggled As Boolean

        Public Sub New(key As Keys, ispressed As Boolean, istoggled As Boolean)
            m_Key = key
            m_IsPressed = ispressed
            m_IsToggled = istoggled
        End Sub

        Public Shared ReadOnly Property [Default]() As KeyStateInfo
            Get
                Return New KeyStateInfo(Keys.None, False, False)
            End Get
        End Property

        Public ReadOnly Property Key() As Keys
            Get
                Return m_Key
            End Get
        End Property

        Public ReadOnly Property IsPressed() As Boolean
            Get
                Return m_IsPressed
            End Get
        End Property

        Public ReadOnly Property IsToggled() As Boolean
            Get
                Return m_IsToggled
            End Get
        End Property
    End Structure
    ''' <summary>
    ''' His 切換
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ComboBoxHis_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxHis.SelectedIndexChanged
        Select Case Me.ComboBoxHis.SelectedIndex
            Case 0
                systemIni.His = "RS"
            Case 1
                systemIni.His = "VISW"
            Case 2
                systemIni.His = "TECH"
            Case 3
                systemIni.His = "DHA"
            Case 4
                systemIni.His = "SC"
            Case 5
                systemIni.His = "KN"
            Case 6
                systemIni.His = "MTR"
        End Select
    End Sub
    ''' <summary>
    ''' Ctrl 切換
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CheckBoxCtrl_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxCtrl.CheckedChanged
        systemIni.IsCtrl = Me.CheckBoxCtrl.Checked
    End Sub
    ''' <summary>
    ''' Alt 切換
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CheckBoxAlt_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxAlt.CheckedChanged
        systemIni.IsAlt = Me.CheckBoxAlt.Checked
    End Sub
    ''' <summary>
    ''' Shift 切換
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CheckBoxShift_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxShift.CheckedChanged
        systemIni.IsShift = Me.CheckBoxShift.Checked
    End Sub
    ''' <summary>
    ''' 取消 HotKey 的鍵盤輸入
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ComboBoxHotKey_KeyPress(sender As Object, e As KeyEventArgs) Handles ComboBoxHotKey.KeyDown
        e.Handled = True
    End Sub
    ''' <summary>
    ''' HotKey 切換
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ComboBoxHotKey_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxHotKey.SelectedIndexChanged
        Dim hotKey = "F" + (Me.ComboBoxHotKey.SelectedIndex + 1).ToString
        systemIni.HotKey = hotKey
    End Sub

    Private Sub ComboBoxF1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxF1.SelectedIndexChanged
        systemIni.F1 = Me.ComboBoxF1.Text
    End Sub
    Private Sub ComboBoxF2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxF2.SelectedIndexChanged
        systemIni.F2 = Me.ComboBoxF2.Text
    End Sub
    Private Sub ComboBoxF3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxF3.SelectedIndexChanged
        systemIni.F3 = Me.ComboBoxF3.Text
    End Sub
    Private Sub ComboBoxF4_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxF4.SelectedIndexChanged
        systemIni.F4 = Me.ComboBoxF4.Text
    End Sub
    Private Sub ComboBoxF5_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxF5.SelectedIndexChanged
        systemIni.F5 = Me.ComboBoxF5.Text
    End Sub
    Private Sub ComboBoxF6_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxF6.SelectedIndexChanged
        systemIni.F6 = Me.ComboBoxF6.Text
    End Sub
    Private Sub ComboBoxF7_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxF7.SelectedIndexChanged
        systemIni.F7 = Me.ComboBoxF7.Text
    End Sub
    Private Sub ComboBoxF8_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxF8.SelectedIndexChanged
        systemIni.F8 = Me.ComboBoxF8.Text
    End Sub
    Private Sub ComboBoxF9_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxF9.SelectedIndexChanged
        systemIni.F9 = Me.ComboBoxF9.Text
    End Sub
    Private Sub ComboBoxF10_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxF10.SelectedIndexChanged
        systemIni.F10 = Me.ComboBoxF10.Text
    End Sub
    Private Sub ComboBoxF11_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxF11.SelectedIndexChanged
        systemIni.F11 = Me.ComboBoxF11.Text
    End Sub
    Private Sub ComboBoxF12_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxF12.SelectedIndexChanged
        systemIni.F12 = Me.ComboBoxF12.Text
    End Sub

    Public Function GetIniValue(section As String, key As String, filename As String, Optional defaultValue As String = "") As String
        Dim sb As New StringBuilder(500)
        If GetPrivateProfileString(section, key, defaultValue, sb, sb.Capacity, filename) > 0 Then
            Return sb.ToString
        Else
            Return defaultValue
        End If
    End Function
    Shared Function MyIsNumeric(ByVal data As String)
        For i = 0 To data.Length - 1
            Dim subData = data.Substring(i, 1)
            If Not IsNumeric(subData) And subData <> "-" Then
                Return False
            End If
        Next
        Return True
    End Function

End Class
''' <summary>
''' 自訂 MessageBox
''' </summary>
Public Class MyMessageBox
    Inherits Form
    Private WithEvents Btn As New Button
    Private CustomHeight As Integer
    Private Message As String
    Private Sub New(width As Integer, height As Integer)
        CustomHeight = height
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.FixedDialog
        Me.Text = "門診預約資訊"
        Me.KeyPreview = True
        MinimizeBox = False
        MaximizeBox = False
        Size = New Size(width, height)
        With Btn
            .SetBounds(width - 130, height - 70, 94, 25)
            .Text = "確定"
        End With
        Controls.Add(Btn)
    End Sub
    Public Overloads Shared Sub Show(ByVal Message As String, width As Integer, height As Integer)
        Dim F As New MyMessageBox(width, height)
        F.Message = Message
        F.CenterToScreen()
        F.ShowDialog()
    End Sub
    Private Sub myMessageBox_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
        e.Graphics.FillRectangle(Brushes.White, New Rectangle(0, 0, ClientRectangle.Width, CustomHeight - 80))
        e.Graphics.DrawIcon(SystemIcons.Information, 10, 30 - SystemIcons.Exclamation.Height \ 2)
        Dim Rect As New Rectangle(SystemIcons.Information.Width + 20, 0, Width - 20 - SystemIcons.Information.Width, CustomHeight - 80)
        Dim Fmt As New StringFormat
        Fmt.LineAlignment = StringAlignment.Center
        e.Graphics.DrawString(Message, New Font("微軟正黑體", 14), Brushes.Blue, Rect, Fmt)
    End Sub
    Private Sub myMessageBox_Keydown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = 27 Then
            DialogResult = Windows.Forms.DialogResult.OK
        End If
    End Sub
    Private Sub Btn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btn.Click
        DialogResult = Windows.Forms.DialogResult.OK
    End Sub

End Class

Public Class UvPojo
    Public Property ClinicRoom() As String
        Get
            Return m_ClinicRoom
        End Get
        Set
            m_ClinicRoom = Value
        End Set
    End Property
    Private m_ClinicRoom As String
    Public Property ShiftTime() As String
        Get
            Return m_ShiftTime
        End Get
        Set
            m_ShiftTime = Value
        End Set
    End Property
    Private m_ShiftTime As String
    Public Property ClinicNumber() As String
        Get
            Return m_ClinicNumber
        End Get
        Set
            m_ClinicNumber = Value
        End Set
    End Property
    Private m_ClinicNumber As String
    Public Property PatientReason() As String
        Get
            Return m_PatientReason
        End Get
        Set
            m_PatientReason = Value
        End Set
    End Property
    Private m_PatientReason As String = ""
    Public Property Item1() As String
        Get
            Return m_Item1
        End Get
        Set
            m_Item1 = Value
        End Set
    End Property
    Private m_Item1 As String

    Private m_Friends As List(Of UvPojo)

End Class

Public Class SystemIni
    Public OcrPath As String = "C:\RsLinkToAntifat"
    Public His As String = "RS"
    Public Server As String = "Z"
    Public HotKey As String = "F4"
    Public F1 As String = ""
    Public F2 As String = ""
    Public F3 As String = ""
    Public F4 As String = ""
    Public F5 As String = ""
    Public F6 As String = ""
    Public F7 As String = ""
    Public F8 As String = ""
    Public F9 As String = ""
    Public F10 As String = ""
    Public F11 As String = ""
    Public F12 As String = ""
    Public SPosiX As Integer = 136
    Public SPosiY As Integer = 172
    Public SWidth As Integer = 96
    Public SHeight As Integer = 34
    Public ZPosiX As Integer = 4
    Public ZPosiY As Integer = 31
    Public ZWidth As Integer = 88
    Public ZHeight As Integer = 30
    Public IsCtrl As Boolean = False
    Public IsAlt As Boolean = False
    Public IsShift As Boolean = False
    Public SQLNAME As String = "tgk"
    Public SQLIP As String = "127.0.0.1,1933"
    Public SQLUSER As String = "sa"
    Public SQLPW As String = "tech"
    Public LocalFunction1 As String = ""
    Public LocalFunction2 As String = ""
    Public LocalFunction3 As String = ""
End Class
