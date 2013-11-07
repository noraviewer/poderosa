using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
using Poderosa.View;

namespace TerminalControlTest
{
	public partial class Form1 : Form, IInterruptableConnectorClient
	{
		private ITerminalSettings _settings;

		public Form1()
		{
			InitializeComponent();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			ITerminalEmulatorService terminalEmulatorService =
				(ITerminalEmulatorService) Program.PoderosaWorld.PluginManager.FindPlugin("org.poderosa.terminalemulator", typeof (ITerminalEmulatorService));
			IProtocolService protocolService = (IProtocolService) Program.PoderosaWorld.PluginManager.FindPlugin("org.poderosa.protocols", typeof (IProtocolService));

			ISSHLoginParameter sshLoginParameter = protocolService.CreateDefaultSSHParameter();

			sshLoginParameter.Account = "luke.stratman";
			sshLoginParameter.AuthenticationType = AuthenticationType.Password;
			sshLoginParameter.Method = SSHProtocol.SSH2;
			sshLoginParameter.PasswordOrPassphrase = "TifLBd1B,0BA";

			ITCPParameter tcpParameter = (ITCPParameter)sshLoginParameter.GetAdapter(typeof(ITCPParameter));

			tcpParameter.Destination = "localhost";
            tcpParameter.Port = 22;

			_settings = terminalEmulatorService.CreateDefaultTerminalSettings(tcpParameter.Destination, null);

			_settings.BeginUpdate();
			_settings.TerminalType = TerminalType.VT100;
			_settings.EndUpdate();

            ITerminalParameter param = (ITerminalParameter)tcpParameter.GetAdapter(typeof(ITerminalParameter));
			param.SetTerminalName(_settings.TerminalType.ToString("G").ToLower());

			protocolService.AsyncSSHConnect(this, sshLoginParameter);
		}

		public void SuccessfullyExit(ITerminalConnection result)
		{
			ICoreServices cs = (ICoreServices)Program.PoderosaWorld.GetAdapter(typeof(ICoreServices));
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
