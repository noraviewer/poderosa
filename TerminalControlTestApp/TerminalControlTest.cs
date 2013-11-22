using System;
using System.Drawing;
using System.IO;
using System.Security;
using System.Windows.Forms;
using Poderosa.TerminalControl;

namespace TerminalControlTestApp
{
	public partial class TerminalControlTest : Form
	{
		public TerminalControlTest()
		{
			InitializeComponent();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			ShowLoginDialog();
		}

		private void _terminalTabs_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_terminalTabs.SelectedIndex == _terminalTabs.TabCount - 1)
				ShowLoginDialog();
		}

		private void ShowLoginDialog()
		{
			LoginDialog loginDialog = new LoginDialog();

			if (loginDialog.ShowDialog(this) == DialogResult.OK)
			{
				TabPage newTab = new TabPage(loginDialog.HostName);
				TerminalControl terminalControl = new TerminalControl
				                   {
					                   Font = new Font("Consolas", 14),
					                   ForeColor = Color.LightGray,
					                   BackColor = Color.Black,
					                   HostName = loginDialog.HostName,
					                   Username = loginDialog.Username,
									   Port = loginDialog.Port,
									   Dock = DockStyle.Fill,
									   TerminalType = loginDialog.TerminalType,
									   SshProtocol = loginDialog.ProtocolType
				                   };

				if (!string.IsNullOrEmpty(loginDialog.IdentityFile))
					terminalControl.IdentityFile = loginDialog.IdentityFile;

				else if (!string.IsNullOrEmpty(loginDialog.Password))
				{
					terminalControl.Password = new SecureString();

					foreach (char character in loginDialog.Password)
						terminalControl.Password.AppendChar(character);
				}

				terminalControl.Disconnected += terminalControl_Disconnected;
				terminalControl.LoggedOff += terminalControl_LoggedOff;

				newTab.Controls.Add(terminalControl);

				_terminalTabs.TabPages.Insert(_terminalTabs.SelectedIndex, newTab);
				_terminalTabs.SelectTab(newTab);

				terminalControl.AsyncConnect();
			}

			else if (_terminalTabs.TabCount > 1)
				_terminalTabs.SelectedIndex = _terminalTabs.TabCount - 2;
		}

		void terminalControl_LoggedOff(object sender, EventArgs e)
		{
			Invoke(
				new Action(
					() =>
					{
						for (int i = 0; i < _terminalTabs.TabPages.Count; i++)
						{
							TabPage tab = _terminalTabs.TabPages[i];

							if (tab.Controls[0] == sender)
							{
								_terminalTabs.TabPages.Remove(tab);
								break;
							}
						}
					}));
		}

		void terminalControl_Disconnected(object sender, ErrorEventArgs e)
		{
			Invoke(
				new Action(
					() =>
						{
							MessageBox.Show(e.GetException().Message);

							for (int i = 0; i < _terminalTabs.TabPages.Count; i++)
							{
								TabPage tab = _terminalTabs.TabPages[i];

								if (tab.Controls[0] == sender)
								{
									_terminalTabs.TabPages.Remove(tab);
									break;
								}
							}
						}));
		}
	}
}
