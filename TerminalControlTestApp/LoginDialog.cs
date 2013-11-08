using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Poderosa.TerminalControl;

namespace TerminalControlTestApp
{
	public partial class LoginDialog : Form
	{
		public LoginDialog()
		{
			InitializeComponent();

			_terminalTypeDropdown.SelectedIndex = 0;
			_protocolTypeDropdown.SelectedIndex = 1;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			_hostNameTextBox.Focus();
		}

		private void _okButton_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			Close();
		}

		private void _cancelButton_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}

		public string HostName
		{
			get
			{
				return _hostNameTextBox.Text;
			}
		}

		public int Port
		{
			get
			{
				return Convert.ToInt32(_portUpDown.Value);
			}
		}

		public string Username
		{
			get
			{
				return _usernameTextBox.Text;
			}
		}

		public string Password
		{
			get
			{
				return _passwordTextBox.Text;
			}
		}

		public string IdentityFile
		{
			get
			{
				return _identityFileTextBox.Text;
			}
		}

		public TerminalType TerminalType
		{
			get
			{
				return (TerminalType) Enum.Parse(typeof (TerminalType), _terminalTypeDropdown.SelectedItem.ToString());
			}
		}

		public SshProtocol ProtocolType
		{
			get
			{
				return (SshProtocol)Enum.Parse(typeof(SshProtocol), _protocolTypeDropdown.SelectedItem.ToString());
			}
		}

		private void LoginDialog_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				_okButton_Click(null, null);
				e.Handled = true;
			}

			else if (e.KeyCode == Keys.Escape)
			{
				_cancelButton_Click(null, null);
				e.Handled = true;
			}
		}

		private void _identityFileBrowseButton_Click(object sender, EventArgs e)
		{
			if (_identityFileDialog.ShowDialog(this) == DialogResult.OK)
			{
				_identityFileTextBox.Text = _identityFileDialog.FileName;
			}
		}
	}
}
