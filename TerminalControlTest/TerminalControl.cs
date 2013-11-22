using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Forms;
using Granados;
using Poderosa.Boot;
using Poderosa.Forms;
using Poderosa.Plugins;
using Poderosa.Protocols;
using Poderosa.Sessions;
using Poderosa.Terminal;

namespace Poderosa.TerminalControl
{
	public partial class TerminalControl : UserControl, IInterruptableConnectorClient
	{
		protected static IPoderosaApplication _poderosaApplication;
		protected static IPoderosaWorld _poderosaWorld;

		protected ITerminalSettings _settings;

		public event EventHandler Connected;
		public event ErrorEventHandler Disconnected;
		public event EventHandler LoggedOff;

		public TerminalControl()
		{
			InitializeComponent();

			Port = 22;
			TerminalType = TerminalType.VT100;
			SshProtocol = SshProtocol.SSH2;
		}

		static TerminalControl()
		{
			_poderosaApplication = PoderosaStartup.CreatePoderosaApplication(new string[] { });
			_poderosaWorld = _poderosaApplication.Start();
		}

		public string Username
		{
			get;
			set;
		}

		public string IdentityFile
		{
			get;
			set;
		}

		public SecureString Password
		{
			get;
			set;
		}

		public string HostName
		{
			get;
			set;
		}

		public int Port
		{
			get;
			set;
		}

		public TerminalType TerminalType
		{
			get;
			set;
		}

		public SshProtocol SshProtocol
		{
			get;
			set;
		}

		public void AsyncConnect()
		{
			ITerminalEmulatorService terminalEmulatorService =
				(ITerminalEmulatorService)_poderosaWorld.PluginManager.FindPlugin("org.poderosa.terminalemulator", typeof(ITerminalEmulatorService));
			IProtocolService protocolService = (IProtocolService)_poderosaWorld.PluginManager.FindPlugin("org.poderosa.protocols", typeof(IProtocolService));

			ISSHLoginParameter sshLoginParameter = protocolService.CreateDefaultSSHParameter();

			sshLoginParameter.Account = Username;

			if (!String.IsNullOrEmpty(IdentityFile))
			{
				sshLoginParameter.AuthenticationType = AuthenticationType.PublicKey;
				sshLoginParameter.IdentityFileName = IdentityFile;
			}

			else
			{
				sshLoginParameter.AuthenticationType = AuthenticationType.Password;

				if (Password != null && Password.Length > 0)
				{
					IntPtr passwordBytes = Marshal.SecureStringToGlobalAllocAnsi(Password);
					sshLoginParameter.PasswordOrPassphrase = Marshal.PtrToStringAnsi(passwordBytes);
				}
			}

			sshLoginParameter.Method = (SSHProtocol)Enum.Parse(typeof(SSHProtocol), SshProtocol.ToString("G"));

			ITCPParameter tcpParameter = (ITCPParameter)sshLoginParameter.GetAdapter(typeof(ITCPParameter));

			tcpParameter.Destination = HostName;
			tcpParameter.Port = Port;

			_settings = terminalEmulatorService.CreateDefaultTerminalSettings(tcpParameter.Destination, null);

			_settings.BeginUpdate();
			_settings.TerminalType = (ConnectionParam.TerminalType)Enum.Parse(typeof(ConnectionParam.TerminalType), TerminalType.ToString("G"));
			_settings.RenderProfile = terminalEmulatorService.TerminalEmulatorOptions.CreateRenderProfile();
			_settings.RenderProfile.BackColor = BackColor;
			_settings.RenderProfile.ForeColor = ForeColor;
			_settings.RenderProfile.FontName = Font.Name;
			_settings.RenderProfile.FontSize = Font.Size;
			_settings.EndUpdate();

			ITerminalParameter param = (ITerminalParameter)tcpParameter.GetAdapter(typeof(ITerminalParameter));
			param.SetTerminalName(_settings.TerminalType.ToString("G").ToLower());

			protocolService.AsyncSSHConnect(this, sshLoginParameter);
		}

		public void SuccessfullyExit(ITerminalConnection result)
		{
			ICoreServices cs = (ICoreServices)_poderosaWorld.GetAdapter(typeof(ICoreServices));
			IWindowManager wm = cs.WindowManager;

			(result as ICloseableTerminalConnection).ConnectionClosed += TerminalControl_ConnectionClosed;
			(result as ICloseableTerminalConnection).ConnectionLost += TerminalControl_ConnectionLost;

			wm.ActiveWindow.AsForm().Invoke(
				new Action(
					() =>
						{
							IPoderosaMainWindow window = wm.CreateNewWindow(new MainWindowArgument(ClientRectangle, FormWindowState.Normal, "", "", 1));
							IViewManager pm = window.ViewManager;
							Sessions.TerminalSession ts = new Sessions.TerminalSession(result, _settings);

							IContentReplaceableView rv = (IContentReplaceableView)pm.GetCandidateViewForNewDocument().GetAdapter(typeof(IContentReplaceableView));
							cs.SessionManager.StartNewSession(ts, rv);

							ts.TerminalControl.HideSizeTip = true;

							Form containerForm = rv.ParentForm.AsForm();

							foreach (Control control in containerForm.Controls)
							{
								if (control is MenuStrip || control.GetType().Name == "PoderosaStatusBar")
									control.Visible = false;

								else if (control.GetType().Name == "PoderosaToolStripContainer")
								{
									foreach (ToolStripPanel child in control.Controls.OfType<ToolStripPanel>())
										child.Visible = false;

									foreach (ToolStripContentPanel child in control.Controls.OfType<ToolStripContentPanel>())
									{
										foreach (Control grandChild in child.Controls)
										{
											if (grandChild.GetType().Name != "TerminalControl")
												grandChild.Visible = false;
										}
									}
								}
							}

							containerForm.TopLevel = false;
							containerForm.FormBorderStyle = FormBorderStyle.None;
							containerForm.Width = Width;
							containerForm.Height = Height;
							containerForm.Dock = DockStyle.Fill;
							containerForm.Parent = this;

							rv.AsControl().Focus();
						}));

			if (Connected != null)
				Connected(this, new EventArgs());
		}

		void TerminalControl_ConnectionLost(object sender, ErrorEventArgs e)
		{
			ConnectionFailed(e.GetException().Message);
		}

		void TerminalControl_ConnectionClosed(object sender, EventArgs e)
		{
			if (LoggedOff != null)
				LoggedOff(this, new EventArgs());
		}

		public void ConnectionFailed(string message)
		{
			if (Disconnected != null)
				Disconnected(this, new ErrorEventArgs(new Exception(message)));
		}
	}
}
