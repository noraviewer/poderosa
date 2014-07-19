using System;
using System.Windows.Forms;

namespace Poderosa.TerminalControl
{
    /// <summary>
    /// SshTelnetTerminalControl上で接続状態を表示するための画面を提供します。
    /// </summary>
    public partial class InfoDialog : Form
    {
        /// <summary>
        /// 接続状態を説明するテキストです。
        /// </summary>
        public string LoginInfoText
        {
            get { return _loginInfoRichTextBox.Text; }
            set { _loginInfoRichTextBox.Text = value; }
        }

        /// <summary>
        /// コンストラクタです。
        /// </summary>
        public InfoDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// フォームのLoadイベントに対するイベントハンドラです。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InfoDialog_Load(object sender, System.EventArgs e)
        {
            _programInfoRichTextBox.Rtf =
                global::Poderosa.TerminalControl.Properties.Resources.ProgramInfo;
        }

        /// <summary>
        /// ログイン情報のRichTextBoxでのEnterイベントに対するイベントハンドラです。
        /// </summary>
        /// <remarks>
        /// RichTextBox内にキャレットを表示させないためにフォーカスを強制的に移動させます。
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loginInfoRichTextBox_Enter(object sender, System.EventArgs e)
        {
            _loginInfoTitleLabel.Focus();
        }

        /// <summary>
        /// プログラム情報のRichTextBoxでのEnterイベントに対するイベントハンドラです。
        /// </summary>
        /// <remarks>
        /// RichTextBox内にキャレットを表示させないためにフォーカスを強制的に移動させます。
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void programInfoRichTextBox_Enter(object sender, System.EventArgs e)
        {
            _programInfoTitleLabel.Focus();
        }

        /// <summary>
        /// プログラム情報のRichTextBoxでのLinkClickedイベントに対するイベントハンドラです。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void programInfoRichTextBox_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            //
            // ブラウザを起動します。
            //
            try
            {
                System.Diagnostics.Process.Start(e.LinkText);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "ブラウザの起動に失敗しました。" + Environment.NewLine
                    + "(" + ex.GetType().Name + "：" + ex.Message + ")", "ブラウザを起動できません",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
