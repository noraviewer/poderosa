using System;
using System.IO;
using System.Windows.Forms;
using Poderosa.View;

namespace Poderosa.TerminalControl
{
    /// <summary>
    /// SshTelnetTerminalコントロールを利用するための機能を持ったユーザコントロールです。
    /// </summary>
    /// <remarks>
    /// 以下の機能を持っています。<br/>
    /// - ログインプロファイルの編集<br/>
    /// - 接続要求<br/>
    /// - 表示プロファイルの編集<br/>
    /// - 切断要求<br/>
    /// - 接続状態の表示
    /// </remarks>
    public partial class SshTelnetTerminalControl : UserControl
    {
        // --------------------------------------------------------------------
        // 定数値定義
        // --------------------------------------------------------------------

        private const string MSG_NOT_CONNECTED = "接続されていません";
        private const string MSG_CONNECTING = "接続処理中です";
        private const string MSG_LOGGED_OFF = "接続が解除されました";
        private const int WAKEUP_TIMER_DEFAULT = 0;
        private const int WAKEUP_TIMER_MAX = 10000;

        // --------------------------------------------------------------------
        // フィールド定義
        // --------------------------------------------------------------------

        private SshTelnetTerminal _sshTelnetTerminal;
        private LoginProfile _loginProfile;
        private int _wakeupTimerMsec = WAKEUP_TIMER_DEFAULT;

        // --------------------------------------------------------------------
        // プロパティ定義
        // --------------------------------------------------------------------

        /// <summary>
        /// ログインプロファイルです。
        /// </summary>
        public LoginProfile LoginProfile
        {
            get { return _loginProfile; }
            set
            {
                _loginProfile = value;
                ReflectRenderProfile();
            }
        }

        /// <summary>
        /// 接続完了時タイマ(ミリ秒)です。
        /// </summary>
        /// <remarks>
        /// 接続完了時タイマが0より大きい値になっていると、TerminalConnectedイベント受信後に
        /// 設定された時間が経過してからリサイズ操作が行われた後に端末画面が利用可能になります。
        /// デフォルト値は0であり、その場合はリサイズ操作は行われずに端末画面が利用可能になります。
        /// 端末画面の画面サイズの設定はサーバ側のログインスクリプトで行われる(resizeコマンド)
        /// ことを期待していますが、そうでない場合に補助的に利用できます。
        /// </remarks>
        public int WakeupTimerMsec
        {
            get { return _wakeupTimerMsec; }
            set
            {
                if (value >= 0 && value <= WAKEUP_TIMER_MAX)
                {
                    _wakeupTimerMsec = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("WakeupTimerMsec");
                }
            }
        }

        // --------------------------------------------------------------------
        // コンストラクタと初期化処理
        // --------------------------------------------------------------------

#if DEBUG
        /// <summary>
        /// ログメッセージをコンソールに出力します。
        /// </summary>
        /// <param name="message"></param>
        private void WriteLog(string message)
        {
            Console.WriteLine(string.Format("{0}：{1}", typeof(SshTelnetTerminalControl).Name,
                message));
        }
#endif

        /// <summary>
        /// コンストラクタです。
        /// </summary>
        public SshTelnetTerminalControl()
        {
#if DEBUG
            WriteLog("コンストラクタが呼び出されました。");
#endif
            InitializeComponent();
        }

        /// <summary>
        /// 本コントロールのLoadイベントに対するイベントハンドラです。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SshTelnetTerminalControl_Load(object sender, EventArgs e)
        {
            _conditionLabel.Text = MSG_NOT_CONNECTED;
            PrepareButton();
        }

        /// <summary>
        /// ツールストリップ上のボタンの利用可否を設定します。
        /// </summary>
        private void PrepareButton()
        {
            if (_sshTelnetTerminal != null)
            {
                switch (_sshTelnetTerminal.State)
                {
                    case SshTelnetTerminal.LineState.Connected:
                        _connectToolStripButton.Visible = false;
                        _disconnectToolStripButton.Visible = true;
                        _settingToolStripButton.Visible = true;
                        break;
                    case SshTelnetTerminal.LineState.NotConnected:
                        _connectToolStripButton.Visible = true;
                        _disconnectToolStripButton.Visible = false;
                        _settingToolStripButton.Visible = false;
                        break;
                    case SshTelnetTerminal.LineState.Connecting:
                    default:
                        _connectToolStripButton.Visible = false;
                        _disconnectToolStripButton.Visible = false;
                        _settingToolStripButton.Visible = false;
                        break;
                }
            }
            else
            {
                _connectToolStripButton.Visible = true;
                _disconnectToolStripButton.Visible = false;
                _settingToolStripButton.Visible = false;
            }
        }

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
#if DEBUG
            WriteLog("Dispose()が呼び出されました。");
#endif
            //
            // SshTelnetTerminalコントロールを先に廃棄します。
            // ※ SshTelnetTerminalコントロールはTCP接続を持っているので、イベントハンドラの解除、
            //    接続の解除を先に実行します。
            //
            DisposeSshTelnetTerminal();

            //
            // 通常の廃棄処理です。
            //
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        // --------------------------------------------------------------------
        // 接続開始
        // --------------------------------------------------------------------

        /// <summary>
        /// [接続]ボタンのClickイベントに対するイベントハンドラです。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void connectToolStripButton_Click(object sender, EventArgs e)
        {
            ShowLoginDialog();
        }

        /// <summary>
        /// ログイン画面を表示します。
        /// </summary>
        private void ShowLoginDialog()
        {
            if (LoginProfile != null)
            {
                LoginDialog loginDialog = new LoginDialog();
                loginDialog.LoginProfile = LoginProfile;

                if (loginDialog.ShowDialog(this) == DialogResult.OK)
                {
#if DEBUG
                    WriteLog("ログイン画面で[OK]ボタンが押されました。");
#endif
                    LoginProfile = loginDialog.LoginProfile;
                    RecreateSshTelnetTerminal();
                    _sshTelnetTerminal.LoginProfile = loginDialog.LoginProfile;
                    _sshTelnetTerminal.Dock = DockStyle.Fill;

                    //
                    // 非同期接続開始処理を行います。
                    //
#if DEBUG
                    WriteLog("非同期接続を開始します。");
#endif
                    _conditionLabel.Text = MSG_CONNECTING;
                    _sshTelnetTerminal.AsyncConnect();
                    PrepareButton();
                }
            }
        }

        /// <summary>
        /// SshTelnetTerminalコントロールを再生成します。
        /// </summary>
        /// <remarks>
        /// 初回生成時にも呼び出します。本コントロールの起動時点ではSshTelnetTerminalコントロールは
        /// 生成されていません。
        /// </remarks>
        private void RecreateSshTelnetTerminal()
        {
            //
            // SshTelnetTerminalコントロールが生成されているならば廃棄します。
            //
            DisposeSshTelnetTerminal();

            //
            // SshTelnetTerminalコントロールを生成して本コントロールに載せます。
            //
#if DEBUG
            WriteLog("SshTelnetTerminalコントロールを生成します。");
#endif
            _sshTelnetTerminal = new Poderosa.TerminalControl.SshTelnetTerminal();
            _sshTelnetTerminal.Dock = System.Windows.Forms.DockStyle.Fill;
            _sshTelnetTerminal.Location = new System.Drawing.Point(0, 0);
            _sshTelnetTerminal.Name = "terminalControl";
            _sshTelnetTerminal.Size = new System.Drawing.Size(300, 240);
            _sshTelnetTerminal.TabIndex = 2;
            _sshTelnetTerminal.TerminalConnected += sshTelnetTerminal_TerminalConnected;
            _sshTelnetTerminal.TerminalDisconnected += sshTelnetTerminal_TerminalDisconnected;
            _sshTelnetTerminal.TerminalClosed += sshTelnetTerminal_TerminalClosed;
            SuspendLayout();
            Controls.Add(_sshTelnetTerminal);
            _sshTelnetTerminal.SendToBack();
            ResumeLayout();
        }

        /// <summary>
        /// SshTelnetTerminalコントロールを廃棄します。
        /// </summary>
        private void DisposeSshTelnetTerminal()
        {
            if (_sshTelnetTerminal != null)
            {
#if DEBUG
                WriteLog("SshTelnetTerminalコントロールを廃棄します。");
#endif
                _sshTelnetTerminal.TerminalConnected -= sshTelnetTerminal_TerminalConnected;
                _sshTelnetTerminal.TerminalDisconnected -= sshTelnetTerminal_TerminalDisconnected;
                _sshTelnetTerminal.TerminalClosed -= sshTelnetTerminal_TerminalClosed;
                Controls.Remove(_sshTelnetTerminal);
                _sshTelnetTerminal.Dispose();
                _sshTelnetTerminal = null;
            }
        }

        // --------------------------------------------------------------------
        // 接続成功
        // --------------------------------------------------------------------

        /// <summary>
        /// SshTelnetTerminalのTerminalConnectedイベントに対するイベントハンドラです。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sshTelnetTerminal_TerminalConnected(object sender, EventArgs e)
        {
#if DEBUG
            WriteLog("TerminalConnectedイベントハンドラが呼び出されました。");
#endif
            Invoke(new Action(() =>
            {
                if (_wakeupTimerMsec > 0)
                {
                    _wakeupTimer.Interval = _wakeupTimerMsec;
                    _wakeupTimer.Enabled = true;
                }
                else
                {
                    _sshTelnetTerminal.BringToFront();
                    PrepareButton();
                }
            }));
        }

        /// <summary>
        /// 接続完了時タイマのTickイベントに対するイベントハンドラです。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wakeupTimer_Tick(object sender, EventArgs e)
        {
            //
            // SshTelnetTerminalコントロールをリサイズして揺さぶります。
            //
            _wakeupTimer.Enabled = false;
            SwitchPadding();
            System.Threading.Thread.Sleep(100);
            SwitchPadding();

            //
            // SshTelnetTerminalコントロールを最前面に配置してボタンの利用可否を設定します。
            //
            _sshTelnetTerminal.BringToFront();
            PrepareButton();
        }

        /// <summary>
        /// SshTelnetTerminalコントロールのPadding値を切り替えます。
        /// </summary>
        /// <remarks>
        /// Padding値を変更することで結果的にリサイズ操作と同じ結果が得られます。
        /// </remarks>
        private void SwitchPadding()
        {
            if (_sshTelnetTerminal.Padding.Top == 0)
            {
                _sshTelnetTerminal.Padding = new Padding(20);
            }
            else
            {
                _sshTelnetTerminal.Padding = new Padding(0);
            }
        }

        // --------------------------------------------------------------------
        // 接続解除
        // --------------------------------------------------------------------

        /// <summary>
        /// [切断]ボタンのClickイベントに対するイベントハンドラです。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void disconnectToolStripButton_Click(object sender, EventArgs e)
        {
#if DEBUG
            WriteLog("[切断]ボタンが押されました。");
#endif
            if (_sshTelnetTerminal != null)
            {
                _sshTelnetTerminal.Close();
            }
        }

        /// <summary>
        /// SshTelnetTerminalのTerminalClosedイベントに対するイベントハンドラです。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sshTelnetTerminal_TerminalClosed(object sender, EventArgs e)
        {
#if DEBUG
            WriteLog("TerminalClosedイベントハンドラが呼び出されました。");
#endif
            Invoke(new Action(() =>
            {
                _conditionLabel.Text = MSG_LOGGED_OFF;
                _sshTelnetTerminal.SendToBack();
                PrepareButton();
            }));
        }

        /// <summary>
        /// SshTelnetTerminalのTerminalDisconnectedイベントに対するイベントハンドラです。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sshTelnetTerminal_TerminalDisconnected(object sender, ErrorEventArgs e)
        {
#if DEBUG
            WriteLog("TerminalDisconnectedイベントハンドラが呼び出されました。");
#endif
            Invoke(new Action(() =>
            {
                MessageBox.Show(e.GetException().Message);

                _conditionLabel.Text = MSG_NOT_CONNECTED;
                _sshTelnetTerminal.SendToBack();
                PrepareButton();
            }));
        }

        // --------------------------------------------------------------------
        // 接続先情報の表示
        // --------------------------------------------------------------------

        /// <summary>
        /// 接続先情報ラベルのClickイベントに対するイベントハンドラです。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hostInfoToolStripLabel_Click(object sender, EventArgs e)
        {
            DisplayInfoDialog();
        }

        /// <summary>
        /// 接続先アイコンのClickイベントに対するイベントハンドラです。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iconToolStripLabel_Click(object sender, EventArgs e)
        {
            DisplayInfoDialog();
        }

        /// <summary>
        /// 接続情報を表示します。
        /// </summary>
        private void DisplayInfoDialog()
        {
#if DEBUG
            WriteLog("接続情報を表示します。");
#endif
            InfoDialog infoDialog = new InfoDialog();
            infoDialog.LoginInfoText = string.Format(
                "接続先ホスト：　{0}\n接続状態：　{1}\n接続方式：　{2}　ポート番号：　{3}　端末タイプ：　{4}\nエンコーディング：　{5}",
                LoginProfile.GetTerminalTitle(), 
                (_sshTelnetTerminal != null) 
                ? _sshTelnetTerminal.State.ToString() 
                : SshTelnetTerminal.LineState.NotConnected.ToString(),
                LoginProfile.ConnectionMethod.ToString(), LoginProfile.Port.ToString(),
                LoginProfile.TerminalType.ToString(), LoginProfile.EncodingType.ToString());
            infoDialog.ShowDialog();
        }

        // --------------------------------------------------------------------
        // 表示プロファイルの編集
        // --------------------------------------------------------------------

        /// <summary>
        /// [設定]ボタンのClickイベントに対するイベントハンドラです。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _settingToolStripButton_Click(object sender, EventArgs e)
        {
            if (LoginProfile == null) return;

#if DEBUG
            WriteLog("表示プロファイル編集画面を呼び出します。");
#endif
            RenderProfile renderProfle =
                PoderosaAccessPoint.CallEditRenderProfile(LoginProfile.ExportRenderProfile());
            if (renderProfle != null)
            {
                LoginProfile.ImportRenderProfile(renderProfle);
                ReflectRenderProfile();

                if (_sshTelnetTerminal != null)
                {
#if DEBUG
                    WriteLog("表示プロファイルをターミナルセッションに反映します。");
#endif
                    _sshTelnetTerminal.UpdateRenderProfile(renderProfle);
                }
            }
        }

        /// <summary>
        /// 表示プロファイルの設定を外観に反映します。
        /// </summary>
        private void ReflectRenderProfile()
        {
            if (LoginProfile != null)
            {
#if DEBUG
                WriteLog("表示プロファイルの設定を外観に反映します。");
#endif
                _hostInfoToolStripLabel.Text = LoginProfile.GetTerminalTitle();
                RenderProfile renderProfile = LoginProfile.ExportRenderProfile();
                _conditionLabel.BackColor = renderProfile.BackColor;
                _conditionLabel.ForeColor = renderProfile.ForeColor;
                _conditionLabel.ImageStyle = renderProfile.ImageStyle;
                _conditionLabel.Image = renderProfile.GetImage();
            }
        }
    }
}
