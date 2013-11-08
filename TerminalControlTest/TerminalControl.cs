using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Windows.Forms;
using Granados;
using Poderosa.Boot;
using Poderosa.ConnectionParam;
using Poderosa.Forms;
using Poderosa.Plugins;
using Poderosa.Protocols;
using Poderosa.Sessions;
using Poderosa.Terminal;

namespace TerminalControlTest
{
	public partial class TerminalControl : UserControl, IInterruptableConnectorClient
	{
		protected static IPoderosaApplication PoderosaApplication;
		protected static IPoderosaWorld PoderosaWorld;

		protected ITerminalSettings _settings;

		public TerminalControl()
		{
			InitializeComponent();

			Port = 22;
			TerminalType = TerminalType.VT100;
			SshProtocol = SshProtocol.SSH2;
		}

		static TerminalControl()
		{
			PoderosaApplication = PoderosaStartup.CreatePoderosaApplication(new string[] { });
			PoderosaWorld = PoderosaApplication.Start();
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
				(ITerminalEmulatorService)PoderosaWorld.PluginManager.FindPlugin("org.poderosa.terminalemulator", typeof(ITerminalEmulatorService));
			IProtocolService protocolService = (IProtocolService)PoderosaWorld.PluginManager.FindPlugin("org.poderosa.protocols", typeof(IProtocolService));

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
			_settings.TerminalType = (Poderosa.ConnectionParam.TerminalType)Enum.Parse(typeof(Poderosa.ConnectionParam.TerminalType), TerminalType.ToString("G"));
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
			ICoreServices cs = (ICoreServices)PoderosaWorld.GetAdapter(typeof(ICoreServices));
			IWindowManager wm = cs.WindowManager;
			IViewManager pm = wm.ActiveWindow.ViewManager;
			TerminalSession ts = new TerminalSession(result, _settings);

			wm.ActiveWindow.AsForm().Invoke(
				new Action(
					() =>
					{
						IContentReplaceableView rv = (IContentReplaceableView)pm.GetCandidateViewForNewDocument().GetAdapter(typeof(IContentReplaceableView));
						cs.SessionManager.StartNewSession(ts, rv);
						cs.SessionManager.ActivateDocument(ts.Terminal.IDocument, ActivateReason.InternalAction);

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
						containerForm.Parent = this;
						containerForm.Dock = DockStyle.Fill;
					}));
		}

		public void ConnectionFailed(string message)
		{
			MessageBox.Show(message);
		}
	}
}
