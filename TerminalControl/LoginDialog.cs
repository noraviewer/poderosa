using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Poderosa.ConnectionParam;
using Poderosa.View;

namespace Poderosa.TerminalControl
{
    /// <summary>
    /// LoginDialogクラスは[ログイン]画面を提供します。
    /// </summary>
	public partial class LoginDialog : Form
	{
        /// <summary>
        /// ログインプロファイルです。
        /// </summary>
        private LoginProfile _loginProfile;
        
        /// <summary>
        /// ログインプロファイルです。
        /// </summary>
        public LoginProfile LoginProfile
        {
            get
            {
                UpdateLoginProfile();
                return _loginProfile;
            }
            set
            {
                _loginProfile = value;
                FetchLoginProfile();
            }
        }

        /// <summary>
        /// コンストラクタです。
        /// </summary>
		public LoginDialog()
		{
            //
            // コンポーネントを初期化します。
            //
			InitializeComponent();

            //
            // 列挙体の値をコンボボックスのアイテムとして設定します。
            //
            _terminalTypeDropDown.DataSource = Enum.GetValues(typeof(TerminalType));
            _connectionMethodDropDown.DataSource = Enum.GetValues(typeof(ConnectionMethod));
            _encodingTypeDropDown.DataSource = Enum.GetValues(typeof(EncodingType));
            _transmitNlDropDown.DataSource = Enum.GetValues(typeof(NewLine));
		}

        /// <summary>
        /// フォームのLoadイベントに対するイベントハンドラです。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginDialog_Load(object sender, EventArgs e)
        {
            SetInputFocus();
        }

        /// <summary>
        /// 各項目の状態に合わせてカーソル位置を調整します。
        /// </summary>
        private void SetInputFocus()
        {
            if (Visible != true) return;

            if (String.IsNullOrEmpty(_hostNameTextBox.Text))
            {
                ActiveControl = _hostNameTextBox;
            }
            else
            {
                if (_sshGroupBox.Enabled != true)
                {
                    ActiveControl = _hostNameTextBox;
                }
                else
                {
                    if (String.IsNullOrEmpty(_usernameTextBox.Text))
                    {
                        ActiveControl = _usernameTextBox;
                    }
                    else
                    {
                        ActiveControl = _passwordTextBox;
                    }
                }
            }
        }

        /// <summary>
        /// ログインプロファイルの内容を画面に反映します。
        /// </summary>
        private void FetchLoginProfile()
        {
            _hostNameTextBox.Text = _loginProfile.Host;
            _connectionMethodDropDown.SelectedItem = _loginProfile.ConnectionMethod;
            _portUpDown.Value = _loginProfile.Port;
            if (_loginProfile.ConnectionMethod == ConnectionMethod.Telnet)
            {
                _usernameTextBox.Text = String.Empty;
                _passwordTextBox.Text = String.Empty;
                _identityFileTextBox.Text = String.Empty;
            }
            else
            {
                _usernameTextBox.Text = _loginProfile.UserName;
                _passwordTextBox.Text = Marshal.PtrToStringUni(
                    Marshal.SecureStringToGlobalAllocUnicode(_loginProfile.Password));
                _identityFileTextBox.Text = _loginProfile.IdentityFile;
            }
            _terminalTypeDropDown.SelectedItem = _loginProfile.TerminalType;
            _encodingTypeDropDown.SelectedItem = _loginProfile.EncodingType;
            _transmitNlDropDown.SelectedItem = _loginProfile.TransmitNL;
            _localEchoCheckBox.Checked = _loginProfile.LocalEcho;
            _lockDestAndTermCheckBox.Checked = _loginProfile.LockDestinationAndTerminal;
            _saveOnExitCheckBox.Checked = _loginProfile.SaveOnExit;
        }

        /// <summary>
        /// 画面の入力内容をログインプロファイルに反映します。
        /// </summary>
        private void UpdateLoginProfile()
        {
            if (_loginProfile == null) return;

            _loginProfile.Host = _hostNameTextBox.Text;
            _loginProfile.ConnectionMethod = (ConnectionMethod)_connectionMethodDropDown.SelectedValue;
            _loginProfile.Port = (int)_portUpDown.Value;
            if ((ConnectionMethod)_connectionMethodDropDown.SelectedValue == ConnectionMethod.Telnet)
            {
                _loginProfile.UserName = String.Empty;
                _loginProfile.Password = new System.Security.SecureString();
                _loginProfile.IdentityFile = String.Empty;
            }
            else
            {
                _loginProfile.UserName = _usernameTextBox.Text;
                _loginProfile.Password = new System.Security.SecureString();
                foreach (char c in _passwordTextBox.Text) _loginProfile.Password.AppendChar(c);
                _loginProfile.IdentityFile = _identityFileTextBox.Text;
            }
            _loginProfile.TerminalType = (TerminalType)_terminalTypeDropDown.SelectedValue;
            _loginProfile.EncodingType = (EncodingType)_encodingTypeDropDown.SelectedItem;
            _loginProfile.TransmitNL = (NewLine)_transmitNlDropDown.SelectedItem;
            _loginProfile.LocalEcho = _localEchoCheckBox.Checked;
            _loginProfile.LockDestinationAndTerminal = _lockDestAndTermCheckBox.Checked;
            _loginProfile.SaveOnExit = _saveOnExitCheckBox.Checked;
        }

        /// <summary>
        /// [鍵ファイル]ボタンのClickイベントに対するイベントハンドラです。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void identityFileBrowseButton_Click(object sender, EventArgs e)
		{
			if (_identityFileDialog.ShowDialog(this) == DialogResult.OK)
			{
				_identityFileTextBox.Text = _identityFileDialog.FileName;
			}
		}

        /// <summary>
        /// [表示プロファイル]ボタンのClickイベントに対するイベントハンドラです。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editRenderProfileButton_Click(object sender, EventArgs e)
        {
            if (_loginProfile != null)
            {
                RenderProfile profile = _loginProfile.ExportRenderProfile();
                RenderProfile editedProfile = PoderosaAccessPoint.CallEditRenderProfile(profile);
                if (editedProfile != null)
                {
                    //
                    // 編集された表示プロファイルをログインプロファイルに取り込みます。
                    //
                    _loginProfile.ImportRenderProfile(editedProfile);
                }
            }
        }

        /// <summary>
        /// [接続方式]コンボボックスのSelectedIndexChangedイベントに対するイベントハンドラです。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void connectionMethodDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((ConnectionMethod)_connectionMethodDropDown.SelectedItem == ConnectionMethod.Telnet)
            {
                _sshGroupBox.Enabled = false;
                _portUpDown.Value = 23;
            }
            else
            {
                _sshGroupBox.Enabled = true;
                _portUpDown.Value = 22;
            }
            SetInputFocus();
        }

        /// <summary>
        /// [接続先と端末設定をロック]チェックボックスのCheckedChangedイベントに対する
        /// イベントハンドラです。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lockDestAndTermCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (_lockDestAndTermCheckBox.Checked)
            {
                _destinationGroupBox.Enabled = false;
                _terminalGroupBox.Enabled = false;
            }
            else
            {
                _destinationGroupBox.Enabled = true;
                _terminalGroupBox.Enabled = true;
            }
            SetInputFocus();
        }
	}
}
