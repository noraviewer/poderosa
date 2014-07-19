using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Granados;
using Poderosa.Forms;
using Poderosa.Plugins;
using Poderosa.Protocols;
using Poderosa.Sessions;
using Poderosa.Terminal;
using Poderosa.View;

namespace Poderosa.TerminalControl
{
	/// <summary>
    /// SSH接続もしくはTelnet接続のターミナルユーザインターフェースを提供します。
	/// </summary>
    /// <remarks>
    /// SSH接続もしくはTelnet接続のターミナルコネクションを作成し、それを表示するためのビューを
    /// MainWindow上に作成してターミナルセッションを開始します。そしてMainWindowにターミナル画面
    /// のみを表示した状態にした上で本コントロール上に載せることでターミナルUIを提供します。
    /// また、接続の成功、失敗、切断をイベント通知します。
    /// </remarks>
    [System.ComponentModel.DesignerCategory("Code")]
    public class SshTelnetTerminal : UserControl, IInterruptableConnectorClient
	{
        // --------------------------------------------------------------------
        // フィールド定義
        // --------------------------------------------------------------------

        /// <summary>
        /// ターミナル設定です。
        /// </summary>
        protected ITerminalSettings _settings;

        /// <summary>
        /// 新規のターミナル接続をキャンセルするためのインターフェースです。
        /// </summary>
        /// <remarks>
        /// AsyncTelnetConnect()やAsyncSSHConnect()を呼び出してから、接続が完了もしくは失敗
        /// するまでの間に接続をキャンセルしたい場合に、IInterruptable.Interrupt()を呼び出します。
        /// </remarks>
        private IInterruptable _connector;

        /// <summary>
        /// 確立したターミナルコネクションがIClosableTerminalConnectionであるかどうかを示します。
        /// </summary>
        /// <remarks>
        /// SSH接続であればIClosableTerminalConnectionであり、ConnectionClosedイベントと
        /// ConnectionLostイベントが利用できますが、Telnet接続の場合はそうでないので、
        /// 両イベントの通知を受け取れません。
        /// </remarks>
        private bool _isCloseableConnection;

        /// <summary>
        /// ターミナルセッションです。
        /// </summary>
        /// <remarks>
        /// ターミナル接続の成功時に取得したターミナルセッションを接続解除を要求するため(Close
        /// メソッド)、もしくは接続状態を確認するためのセッションソケットの取得(BackColorChanged
        /// イベントハンドラ)のために保持しています。
        /// </remarks>
        private Sessions.TerminalSession _session;

        // --------------------------------------------------------------------
        // 列挙体定義
        // --------------------------------------------------------------------

        /// <summary>
        /// ターミナルの接続状態を示す列挙体です。
        /// </summary>
        public enum LineState
        {
            NotConnected,
            Connecting,
            Connected
        }

        // --------------------------------------------------------------------
        // プロパティ定義
        // --------------------------------------------------------------------

        /// <summary>
        /// ログインプロファイルです。
        /// </summary>
        public LoginProfile LoginProfile { get; set; }

        /// <summary>
        /// ターミナルの接続状態です。
        /// </summary>
        public LineState State { get; private set; }

        // --------------------------------------------------------------------
        // イベント定義
        // --------------------------------------------------------------------

		/// <summary>
		/// ターミナルの接続が完了したことを示すイベントです。
		/// </summary>
		public event EventHandler TerminalConnected;

		/// <summary>
        /// 確立されていたターミナルの接続が異常切断されたことを示すイベントです。
		/// </summary>
		public event ErrorEventHandler TerminalDisconnected;

		/// <summary>
        /// 確立されていたターミナルの接続が正常切断されたことを示すイベントです。
		/// </summary>
		public event EventHandler TerminalClosed;

        // --------------------------------------------------------------------
        // コンストラクタ
        // --------------------------------------------------------------------

#if DEBUG
        /// <summary>
        /// ログメッセージをコンソールに出力します。
        /// </summary>
        /// <param name="message"></param>
        private void WriteLog(string message)
        {
            Console.WriteLine(string.Format("{0}：{1}", typeof(SshTelnetTerminal).Name,
                message));
        }
#endif
        /// <summary>
		/// コンストラクタです。
		/// </summary>
		public SshTelnetTerminal()
		{
#if DEBUG
            WriteLog("コンストラクタが呼び出されました。");
#endif
			InitializeComponent();

            State = LineState.NotConnected;
            LoginProfile = new TerminalControl.LoginProfile();
		}

        /// <summary> 
        /// コンポーネントを初期化します。
        /// </summary>
        private void InitializeComponent()
        {
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        }

        /// <summary> 
        /// 接続を解除してリソースを解放します。
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
#if DEBUG
            WriteLog("Dispose()が呼び出されました。");
#endif
            Close();
            base.Dispose(disposing);
        }

        // --------------------------------------------------------------------
        // 接続開始
        // --------------------------------------------------------------------

		/// <summary>
        /// 非同期接続を開始します。
		/// </summary>
        /// <remarks>
        /// 接続パラメータとターミナル設定を作成して、非同期接続を開始します。
        /// 接続が成功するとSuccessfullyExit()が呼ばれ、失敗するとConnectionFailed()が呼ばれます。
        /// </remarks>
		public void AsyncConnect()
		{
            State = LineState.Connecting;
            _settings = null;
            _session = null;
            _connector = null;

            //
            // ターミナルエミュレータサービスとプロトコルサービスを取得します。
            //
			ITerminalEmulatorService terminalEmulatorService =
				(ITerminalEmulatorService)PoderosaAccessPoint.World.PluginManager.FindPlugin(
                "org.poderosa.terminalemulator", typeof(ITerminalEmulatorService));
			IProtocolService protocolService = 
                (IProtocolService)PoderosaAccessPoint.World.PluginManager.FindPlugin(
                "org.poderosa.protocols", typeof(IProtocolService));

            //
            // 接続パラメータを作成します。
            //
            ITCPParameter tcpParameter = null;
            ISSHLoginParameter sshLoginParameter =  null;
            if (LoginProfile.ConnectionMethod == ConnectionParam.ConnectionMethod.Telnet)
            {
                //
                // Telnet接続パラメータの作成
                // ※ tcpParameterの実体はTCPParameterクラスのインスタンスです。
                //
                tcpParameter = protocolService.CreateDefaultTelnetParameter();
                tcpParameter.Destination = LoginProfile.Host;
                tcpParameter.Port = LoginProfile.Port;
            }
            else
            {
                //
                // SSH接続パラメータの作成
                // ※ sshLoginParameterの実体はSSHLoginParameterクラスのインスタンスであり、
                //    SSHLoginParameterクラスはTCPParameterクラスを継承しています。
                //
			    sshLoginParameter = protocolService.CreateDefaultSSHParameter();
			    sshLoginParameter.Account = LoginProfile.UserName;
				if (LoginProfile.Password != null && LoginProfile.Password.Length > 0)
				{
					IntPtr pswdBytes = Marshal.SecureStringToGlobalAllocAnsi(LoginProfile.Password);
					sshLoginParameter.PasswordOrPassphrase = Marshal.PtrToStringAnsi(pswdBytes);
				}
			    if (!String.IsNullOrEmpty(LoginProfile.IdentityFile))
			    {
				    sshLoginParameter.AuthenticationType = AuthenticationType.PublicKey;
				    sshLoginParameter.IdentityFileName = LoginProfile.IdentityFile;
			    }
			    else
			    {
				    sshLoginParameter.AuthenticationType = AuthenticationType.Password;
			    }
			    sshLoginParameter.Method = (SSHProtocol)Enum.Parse(
                    typeof(SSHProtocol), LoginProfile.ConnectionMethod.ToString("G"));

			    tcpParameter = (ITCPParameter)sshLoginParameter.GetAdapter(typeof(ITCPParameter));
			    tcpParameter.Destination = LoginProfile.Host;
			    tcpParameter.Port = LoginProfile.Port;
            }

			//
            // ターミナル設定のパラメータをセットします。
            //
			terminalEmulatorService.TerminalEmulatorOptions.RightButtonAction =
                MouseButtonAction.Paste;
			_settings = terminalEmulatorService.CreateDefaultTerminalSettings(
                tcpParameter.Destination, null);
			_settings.BeginUpdate();
			_settings.TerminalType = (ConnectionParam.TerminalType)Enum.Parse(
                typeof(ConnectionParam.TerminalType), LoginProfile.TerminalType.ToString("G"));
            _settings.Encoding = LoginProfile.EncodingType;
            _settings.LocalEcho = LoginProfile.LocalEcho;
            _settings.TransmitNL = LoginProfile.TransmitNL;
            _settings.RenderProfile = LoginProfile.ExportRenderProfile();
			_settings.EndUpdate();
			ITerminalParameter param = 
                (ITerminalParameter)tcpParameter.GetAdapter(typeof(ITerminalParameter));
			param.SetTerminalName(_settings.TerminalType.ToString("G").ToLower());

            //
            // 非同期接続開始処理を行います。
            //
            if (LoginProfile.ConnectionMethod == ConnectionParam.ConnectionMethod.Telnet)
            {
#if DEBUG
                WriteLog("Telnet非同期接続を開始します。");
#endif
                _connector = protocolService.AsyncTelnetConnect(this, tcpParameter);
            }
            else
            {
#if DEBUG
                WriteLog("SSH非同期接続を開始します。");
#endif
                _connector = protocolService.AsyncSSHConnect(this, sshLoginParameter);
            }
		}

        // --------------------------------------------------------------------
        // 接続成功 (IInterruptableConnectorClient.SuccessfullyExit)
        // --------------------------------------------------------------------

		/// <summary>
        /// IInterruptableConnectorClient.SuccessfullyExitのコールバック関数です。
		/// </summary>
        /// <remarks>
        /// SSH接続もしくはTelnet接続が確立したときに呼ばれます。ターミナル画面を表示するために
        /// 以下の手順で処理を行います。<br/>
        /// (1) 新規にPoderosa.Forms.MainWindowを作成します。<br/>
        /// (2) MainWindow上にビューを作成します。<br/>
        /// (3) 作成したビュー上でターミナルセッションを開始します。<br/>
        /// (4) MainWindow上にあるメニューチップとツールチップを非表示にします。<br/>
        /// (5) ターミナル画面のみが表示されたMainWindowを本コントロール上に載せます。<br/>
        /// (6) TerminalConnectedイベントを発行します。
        /// </remarks>
		/// <param name="result">Newly-created SSH connection.</param>
		public void SuccessfullyExit(ITerminalConnection result)
		{
#if DEBUG
            WriteLog("SuccessfullyExit()が呼び出されました。");
#endif
            //
            // 返されたターミナルコネクションがICloseableTerminalConnectionであれば、
            // コネクションが切断されたことを通知するイベントが利用できますので、ハンドラを
            // 設定します。
            // ※ SSH接続時はICloseableTerminalConnectionが返されますが、Telnet接続時は返されません。
            //
            ICloseableTerminalConnection closableConnection = result as ICloseableTerminalConnection;
            if (closableConnection != null)
            {
                _isCloseableConnection = true;
                closableConnection.ConnectionClosed += terminal_ConnectionClosed;
                closableConnection.ConnectionLost += terminal_ConnectionLost;
            }
            else
            {
                _isCloseableConnection = false;
            }

            //
            // ウィンドウマネージャを取得します。
            //
			ICoreServices coreServices =
                (ICoreServices)PoderosaAccessPoint.World.GetAdapter(typeof(ICoreServices));
			IWindowManager windowManager = coreServices.WindowManager;

            //
            // Invokeメソッドを用いてGUIスレッド上で処理実行させます。
            //
			windowManager.MainWindows.First().AsForm().Invoke(new Action(() =>
			{
                //
                // 返されたターミナルコネクションとターミナル設定から、ターミナルセッションを
                // 作成します。
                //
				_session = new Sessions.TerminalSession(result, _settings);

                //
                // Poderosa.Forms.MainWindowを新規に生成し、ビューを作成します。
                //
				IPoderosaMainWindow window = windowManager.CreateNewWindow(
                    new MainWindowArgument(ClientRectangle, FormWindowState.Normal, "", "", 1));
				IViewManager viewManager = window.ViewManager;
				IContentReplaceableView view = 
                    (IContentReplaceableView)viewManager.GetCandidateViewForNewDocument().GetAdapter(
                    typeof(IContentReplaceableView));

                //
                // セッションマネージャに新しいターミナルセッションを作成したビュー上で
                // 開始するように要求します。
                //
				coreServices.SessionManager.StartNewSession(_session, view);

                //
                // ターミナルセッションの設定を調整します。
                // - サイズチップは表示しません。
                //
				_session.TerminalControl.HideSizeTip = true;

                // 
                // ビューの親フォームはPoderosa.Forms.MainWindowです。
                // MainWindow上にはメニューストリップやツールストリップが載せられていますので、
                // ターミナル画面以外の全てを非表示にします。
                //
				Form containerForm = view.ParentForm.AsForm();
				foreach (Control control in containerForm.Controls)
				{
                    if (control is MenuStrip || control.GetType().Name == "PoderosaStatusBar")
                    {
                        control.Visible = false;
                    }
                    else if (control.GetType().Name == "PoderosaToolStripContainer")
                    {
                        foreach (ToolStripPanel child in control.Controls.OfType<ToolStripPanel>())
                        {
                            child.Visible = false;
                        }
                        foreach (ToolStripContentPanel child 
                            in control.Controls.OfType<ToolStripContentPanel>())
                        {
                            foreach (Control grandChild in child.Controls)
                            {
                                if (grandChild.GetType().Name != "TerminalControl")
                                {
                                    grandChild.Visible = false;
                                }
                            }
                        }
                    }
				}

                //
                // ターミナル画面のみが表示されるMainWindowを本コントロール上に載せます。
                //
				containerForm.TopLevel = false;
				containerForm.FormBorderStyle = FormBorderStyle.None;
				containerForm.Width = Width;
				containerForm.Height = Height;
				containerForm.Dock = DockStyle.Fill;
				containerForm.Parent = this;

                //
                // ビューコントロールのBackColorChangedイベントを捕捉することで、
                // Telnet接続時のCtrl-D入力やlogoutコマンドによる切断を検査する契機とします。
                // ※ Ctrl-Dを送信した後、ビューコントロールのBackColorChangedイベントは
                //    2回発生します。1回目はBlackに変わっており、2回目はControlDarkです。
                //    これにより、表示プロファイルの背景色とかぶっても問題がないと言えます。
                //
                view.AsControl().BackColorChanged += new EventHandler(view_BackColorChanged);

				view.AsControl().Focus();
			}));

            //
            // TerminalConnectイベントを発行します。
            //
            State = LineState.Connected;
            if (TerminalConnected != null)
            {
                TerminalConnected(this, new EventArgs());
            }
		}

        // --------------------------------------------------------------------
        // 接続失敗 (IInterruptableConnectorClient.ConnectionFailed)
        // --------------------------------------------------------------------

        /// <summary>
        /// IInterruptableConnectorClient.ConnectionFailedのコールバック関数です。
        /// </summary>
        /// <remarks>
        /// SSH接続もしくはTelnet接続に失敗したときに呼ばれます。
        /// </remarks>
        /// <param name="message">接続失敗の理由を示すテキスト</param>
        public void ConnectionFailed(string message)
        {
#if DEBUG
            WriteLog("ConnectionFailed()が呼び出されました。");
#endif
            State = LineState.NotConnected;
            if (TerminalDisconnected != null)
            {
                TerminalDisconnected(this, new ErrorEventArgs(new Exception(message)));
            }
        }

        // --------------------------------------------------------------------
        // 接続解除
        // --------------------------------------------------------------------

        /// <summary>
        /// 接続を解除します。
        /// </summary>
        public void Close()
        {
#if DEBUG
            WriteLog("Close()が呼び出されました。");
#endif
            if (State == LineState.Connecting)
            {
                if (_connector != null)
                {
                    //
                    // 待機中の接続をキャンセルします。
                    //
#if DEBUG
                    WriteLog("待機中の接続をキャンセルします。");
#endif
                    _connector.Interrupt();
                }
            }
            else if (State == LineState.Connected)
            {
                if (_session != null)
                {
                    //
                    // セッションマネージャにセッション終了を要求します。
                    //
#if DEBUG
                    WriteLog("セッションマネージャにセッション終了を要求します。");
#endif
                    ICoreServices coreServices =
                        (ICoreServices)PoderosaAccessPoint.World.GetAdapter(typeof(ICoreServices));
                    coreServices.SessionManager.TerminateSession(_session);

                    //
                    // ターミナルコネクションがICloseableTerminalConnectionでなかった場合、
                    // ConnectionClosedイベントによる通知は得られませんので、ここでTerminalClosed
                    // イベントを発行します。
                    //
                    if (!_isCloseableConnection)
                    {
                        State = LineState.NotConnected;
                        if (TerminalClosed != null)
                        {
                            TerminalClosed(this, new EventArgs());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// ICloseableTerminalConnection.ConnectionClosedイベントに対するイベントハンドラです。
		/// </summary>
        /// <remarks>
        /// 確立されたターミナルコネクションが正常切断された場合に呼び出されるイベントハンドラです。
        /// </remarks>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void terminal_ConnectionClosed(object sender, EventArgs e)
		{
#if DEBUG
            WriteLog("ConnectionClosedイベントハンドラが呼び出されました。");
#endif
            State = LineState.NotConnected;
            if (TerminalClosed != null)
            {
                TerminalClosed(this, new EventArgs());
            }
		}

		/// <summary>
        /// ICloseableTerminalConnection.ConnectionLostイベントに対するイベントハンドラです。
		/// </summary>
        /// <remarks>
        /// 確立されたターミナルコネクションが異常切断された場合に呼び出されるイベントハンドラです。
        /// </remarks>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void terminal_ConnectionLost(object sender, ErrorEventArgs e)
		{
#if DEBUG
            WriteLog("ConnectionLostイベントハンドラが呼び出されました。");
#endif
            State = LineState.NotConnected;
			ConnectionFailed(e.GetException().Message);
		}

        /// <summary>
        /// ビューコントロールのBackColorChangedイベントに対するイベントハンドラです。
        /// </summary>
        /// <remarks>
        /// このハンドラはTelnet接続などのIClosableTerminalConnectionではない接続において、
        /// Ctrl-D入力時にサーバから切断されたことを本コントロールで判断するために利用しています。
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void view_BackColorChanged(object sender, EventArgs e)
        {
#if DEBUG
            WriteLog("BackColorChangedイベントハンドラが呼び出されました。");
#endif
            //
            // ターミナルコネクションの状態を調べ、クローズ状態であればTerminalClosedイベントを
            // 発行します。
            //
            if (_session != null && _session.TerminalConnection.IsClosed)
            {
                State = LineState.NotConnected;
                if (TerminalClosed != null)
                {
#if DEBUG
                    WriteLog("コネクション切断を検知したのでTerminalClosedイベントを通知します。");
#endif
                    TerminalClosed(this, new EventArgs());
                }
            }
        }

        // --------------------------------------------------------------------
        // 表示プロファイルの更新
        // --------------------------------------------------------------------

        /// <summary>
        /// ターミナルセッションの表示プロファイルを更新します。
        /// </summary>
        /// <param name="renderProfile"></param>
        public void UpdateRenderProfile(RenderProfile renderProfile)
        {
#if DEBUG
            WriteLog("ターミナルセッションの表示プロファイルを更新します。");
#endif
            if (_session != null)
            {
                _session.TerminalSettings.BeginUpdate();
                _session.TerminalSettings.RenderProfile = renderProfile;
                _session.TerminalSettings.EndUpdate();
            }
        }
    }
}
