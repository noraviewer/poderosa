using System;
using System.Windows.Forms;
using Poderosa.TerminalControl;

namespace TerminalControlDemo
{
    /// <summary>
    /// SshTelnetTerminalControlのデモ画面を表示します。
    /// </summary>
    public partial class DemoForm : Form
    {
        /// <summary>
        /// コンストラクタです。
        /// </summary>
        public DemoForm()
        {
            //
            // Poderosaのoptions.confファイルを配置するディレクトリを指定します。
            // ※ options.confファイルは指定されたディレクトリに自動生成されます。
            // ※ PoderosaAccessPoint.PreferenceDirプロパティにディレクトリパスを指定しない場合、
            //    (LocalUserAppDataPath)\Poderosaがデフォルト設定されます。
            // ※ PreferenceDirの設定はPoderosa環境が初期化される前に一度だけです。
            //    初期化後に設定するとInvalidOperationExceptionが発生します。
            //

            //
            // コンポーネントを初期化します。
            //
            InitializeComponent();
        }

        /// <summary>
        /// フォームのLoadイベントに対するイベントハンドラです。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DemoForm_Load(object sender, EventArgs e)
        {
            sshTelnetTerminalControl.LoginProfile = PrepareLoginProfile();
            createToolStripButton.Enabled = false;
            disposeToolStripButton.Enabled = true;
        }

        /// <summary>
        /// フォームのFormClosingイベントに対するイベントハンドラです。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DemoForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveLoginProfile();
        }

        /// <summary>
        /// ログインプロファイルを準備します。
        /// </summary>
        /// <returns></returns>
        private LoginProfile PrepareLoginProfile()
        {
            LoginProfile loginProfile = LoginProfile.Deserialize(
                sshTelnetTerminalControl.Name, Environment.CurrentDirectory);
            loginProfile.LockDestinationAndTerminal = false;
            loginProfile.SaveOnExit = true;
            return loginProfile;
        }

        /// <summary>
        /// ログインプロファイルを保存します。
        /// </summary>
        private void SaveLoginProfile()
        {
            if (sshTelnetTerminalControl != null &&
                sshTelnetTerminalControl.LoginProfile.SaveOnExit)
            {
                LoginProfile.Serialize(sshTelnetTerminalControl.LoginProfile,
                    sshTelnetTerminalControl.Name, Environment.CurrentDirectory);
            }
        }

        /// <summary>
        /// [ターミナル生成]ボタンのClickイベントに対するイベントハンドラです。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void createToolStripButton_Click(object sender, EventArgs e)
        {
            sshTelnetTerminalControl = new SshTelnetTerminalControl();
            sshTelnetTerminalControl.BackColor = System.Drawing.SystemColors.Control;
            sshTelnetTerminalControl.Dock = System.Windows.Forms.DockStyle.Fill;
            sshTelnetTerminalControl.Location = new System.Drawing.Point(0, 0);
            sshTelnetTerminalControl.LoginProfile = null;
            sshTelnetTerminalControl.Name = "sshTelnetTerminalControl";
            sshTelnetTerminalControl.Size = new System.Drawing.Size(541, 330);
            sshTelnetTerminalControl.TabIndex = 1;
            sshTelnetTerminalControl.WakeupTimerMsec = 0;
            sshTelnetTerminalControl.LoginProfile = PrepareLoginProfile();

            toolStripContainer.ContentPanel.Controls.Add(sshTelnetTerminalControl);
            createToolStripButton.Enabled = false;
            disposeToolStripButton.Enabled = true;
        }

        /// <summary>
        /// [ターミナル廃棄]ボタンのClickイベントに対するイベントハンドラです。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void disposeToolStripButton_Click(object sender, EventArgs e)
        {
            SaveLoginProfile();
            toolStripContainer.ContentPanel.Controls.Remove(sshTelnetTerminalControl);
            sshTelnetTerminalControl.Dispose();
            sshTelnetTerminalControl = null;

            createToolStripButton.Enabled = true;
            disposeToolStripButton.Enabled = false;
        }
    }
}
