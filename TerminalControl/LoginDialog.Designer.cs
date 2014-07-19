namespace Poderosa.TerminalControl
{
	partial class LoginDialog
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this._okButton = new System.Windows.Forms.Button();
            this._hostNameTextBox = new System.Windows.Forms.TextBox();
            this._hostNameLabel = new System.Windows.Forms.Label();
            this._portLabel = new System.Windows.Forms.Label();
            this._portUpDown = new System.Windows.Forms.NumericUpDown();
            this._usernameTextBox = new System.Windows.Forms.TextBox();
            this._usernameLabel = new System.Windows.Forms.Label();
            this._passwordLabel = new System.Windows.Forms.Label();
            this._cancelButton = new System.Windows.Forms.Button();
            this._passwordTextBox = new System.Windows.Forms.TextBox();
            this._identityFileTextBox = new System.Windows.Forms.TextBox();
            this._identityFileBrowseButton = new System.Windows.Forms.Button();
            this._identityFileDialog = new System.Windows.Forms.OpenFileDialog();
            this._identityFileLabel = new System.Windows.Forms.Label();
            this._terminalTypeDropDown = new System.Windows.Forms.ComboBox();
            this._connectionMethodDropDown = new System.Windows.Forms.ComboBox();
            this._terminalTypeLabel = new System.Windows.Forms.Label();
            this._connectionMethodLabel = new System.Windows.Forms.Label();
            this._editRenderProfileBbutton = new System.Windows.Forms.Button();
            this._encodingTypeLabel = new System.Windows.Forms.Label();
            this._encodingTypeDropDown = new System.Windows.Forms.ComboBox();
            this._transmitNlLabel = new System.Windows.Forms.Label();
            this._transmitNlDropDown = new System.Windows.Forms.ComboBox();
            this._localEchoCheckBox = new System.Windows.Forms.CheckBox();
            this._sshGroupBox = new System.Windows.Forms.GroupBox();
            this._terminalGroupBox = new System.Windows.Forms.GroupBox();
            this._destinationGroupBox = new System.Windows.Forms.GroupBox();
            this._saveOnExitCheckBox = new System.Windows.Forms.CheckBox();
            this._lockDestAndTermCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this._portUpDown)).BeginInit();
            this._sshGroupBox.SuspendLayout();
            this._terminalGroupBox.SuspendLayout();
            this._destinationGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // _okButton
            // 
            this._okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._okButton.Location = new System.Drawing.Point(261, 393);
            this._okButton.Name = "_okButton";
            this._okButton.Size = new System.Drawing.Size(75, 23);
            this._okButton.TabIndex = 4;
            this._okButton.Text = "OK";
            this._okButton.UseVisualStyleBackColor = true;
            // 
            // _hostNameTextBox
            // 
            this._hostNameTextBox.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._hostNameTextBox.Location = new System.Drawing.Point(106, 22);
            this._hostNameTextBox.Name = "_hostNameTextBox";
            this._hostNameTextBox.Size = new System.Drawing.Size(281, 19);
            this._hostNameTextBox.TabIndex = 1;
            // 
            // _hostNameLabel
            // 
            this._hostNameLabel.AutoSize = true;
            this._hostNameLabel.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._hostNameLabel.Location = new System.Drawing.Point(20, 25);
            this._hostNameLabel.Name = "_hostNameLabel";
            this._hostNameLabel.Size = new System.Drawing.Size(68, 12);
            this._hostNameLabel.TabIndex = 0;
            this._hostNameLabel.Text = "接続先ホスト";
            // 
            // _portLabel
            // 
            this._portLabel.AutoSize = true;
            this._portLabel.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._portLabel.Location = new System.Drawing.Point(271, 58);
            this._portLabel.Name = "_portLabel";
            this._portLabel.Size = new System.Drawing.Size(57, 12);
            this._portLabel.TabIndex = 4;
            this._portLabel.Text = "ポート番号";
            // 
            // _portUpDown
            // 
            this._portUpDown.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._portUpDown.Location = new System.Drawing.Point(334, 56);
            this._portUpDown.Maximum = new decimal(new int[] {
            65000,
            0,
            0,
            0});
            this._portUpDown.Name = "_portUpDown";
            this._portUpDown.Size = new System.Drawing.Size(53, 19);
            this._portUpDown.TabIndex = 5;
            this._portUpDown.Value = new decimal(new int[] {
            22,
            0,
            0,
            0});
            // 
            // _usernameTextBox
            // 
            this._usernameTextBox.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._usernameTextBox.Location = new System.Drawing.Point(106, 22);
            this._usernameTextBox.Name = "_usernameTextBox";
            this._usernameTextBox.Size = new System.Drawing.Size(150, 19);
            this._usernameTextBox.TabIndex = 1;
            // 
            // _usernameLabel
            // 
            this._usernameLabel.AutoSize = true;
            this._usernameLabel.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._usernameLabel.Location = new System.Drawing.Point(20, 25);
            this._usernameLabel.Name = "_usernameLabel";
            this._usernameLabel.Size = new System.Drawing.Size(47, 12);
            this._usernameLabel.TabIndex = 0;
            this._usernameLabel.Text = "ユーザ名";
            // 
            // _passwordLabel
            // 
            this._passwordLabel.AutoSize = true;
            this._passwordLabel.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._passwordLabel.Location = new System.Drawing.Point(20, 58);
            this._passwordLabel.Name = "_passwordLabel";
            this._passwordLabel.Size = new System.Drawing.Size(52, 12);
            this._passwordLabel.TabIndex = 2;
            this._passwordLabel.Text = "パスワード";
            // 
            // _cancelButton
            // 
            this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancelButton.Location = new System.Drawing.Point(342, 393);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(75, 23);
            this._cancelButton.TabIndex = 5;
            this._cancelButton.Text = "キャンセル";
            this._cancelButton.UseVisualStyleBackColor = true;
            // 
            // _passwordTextBox
            // 
            this._passwordTextBox.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._passwordTextBox.Location = new System.Drawing.Point(106, 55);
            this._passwordTextBox.Name = "_passwordTextBox";
            this._passwordTextBox.PasswordChar = '*';
            this._passwordTextBox.Size = new System.Drawing.Size(148, 19);
            this._passwordTextBox.TabIndex = 3;
            // 
            // _identityFileTextBox
            // 
            this._identityFileTextBox.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._identityFileTextBox.Location = new System.Drawing.Point(106, 88);
            this._identityFileTextBox.Name = "_identityFileTextBox";
            this._identityFileTextBox.Size = new System.Drawing.Size(251, 19);
            this._identityFileTextBox.TabIndex = 5;
            // 
            // _identityFileBrowseButton
            // 
            this._identityFileBrowseButton.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._identityFileBrowseButton.Location = new System.Drawing.Point(363, 88);
            this._identityFileBrowseButton.Name = "_identityFileBrowseButton";
            this._identityFileBrowseButton.Size = new System.Drawing.Size(24, 19);
            this._identityFileBrowseButton.TabIndex = 6;
            this._identityFileBrowseButton.Text = "...";
            this._identityFileBrowseButton.UseVisualStyleBackColor = true;
            this._identityFileBrowseButton.Click += new System.EventHandler(this.identityFileBrowseButton_Click);
            // 
            // _identityFileLabel
            // 
            this._identityFileLabel.AutoSize = true;
            this._identityFileLabel.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._identityFileLabel.Location = new System.Drawing.Point(20, 91);
            this._identityFileLabel.Name = "_identityFileLabel";
            this._identityFileLabel.Size = new System.Drawing.Size(51, 12);
            this._identityFileLabel.TabIndex = 4;
            this._identityFileLabel.Text = "鍵ファイル";
            // 
            // _terminalTypeDropDown
            // 
            this._terminalTypeDropDown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._terminalTypeDropDown.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._terminalTypeDropDown.FormattingEnabled = true;
            this._terminalTypeDropDown.Location = new System.Drawing.Point(106, 23);
            this._terminalTypeDropDown.Name = "_terminalTypeDropDown";
            this._terminalTypeDropDown.Size = new System.Drawing.Size(148, 20);
            this._terminalTypeDropDown.TabIndex = 1;
            // 
            // _connectionMethodDropDown
            // 
            this._connectionMethodDropDown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._connectionMethodDropDown.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._connectionMethodDropDown.FormattingEnabled = true;
            this._connectionMethodDropDown.Location = new System.Drawing.Point(106, 55);
            this._connectionMethodDropDown.Name = "_connectionMethodDropDown";
            this._connectionMethodDropDown.Size = new System.Drawing.Size(148, 20);
            this._connectionMethodDropDown.TabIndex = 3;
            this._connectionMethodDropDown.SelectedIndexChanged += new System.EventHandler(this.connectionMethodDropDown_SelectedIndexChanged);
            // 
            // _terminalTypeLabel
            // 
            this._terminalTypeLabel.AutoSize = true;
            this._terminalTypeLabel.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._terminalTypeLabel.Location = new System.Drawing.Point(20, 26);
            this._terminalTypeLabel.Name = "_terminalTypeLabel";
            this._terminalTypeLabel.Size = new System.Drawing.Size(55, 12);
            this._terminalTypeLabel.TabIndex = 0;
            this._terminalTypeLabel.Text = "端末タイプ";
            // 
            // _connectionMethodLabel
            // 
            this._connectionMethodLabel.AutoSize = true;
            this._connectionMethodLabel.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._connectionMethodLabel.Location = new System.Drawing.Point(20, 58);
            this._connectionMethodLabel.Name = "_connectionMethodLabel";
            this._connectionMethodLabel.Size = new System.Drawing.Size(53, 12);
            this._connectionMethodLabel.TabIndex = 2;
            this._connectionMethodLabel.Text = "接続方式";
            // 
            // _editRenderProfileBbutton
            // 
            this._editRenderProfileBbutton.Location = new System.Drawing.Point(12, 393);
            this._editRenderProfileBbutton.Name = "_editRenderProfileBbutton";
            this._editRenderProfileBbutton.Size = new System.Drawing.Size(121, 23);
            this._editRenderProfileBbutton.TabIndex = 3;
            this._editRenderProfileBbutton.Text = "表示プロファイル ...";
            this._editRenderProfileBbutton.UseVisualStyleBackColor = true;
            this._editRenderProfileBbutton.Click += new System.EventHandler(this.editRenderProfileButton_Click);
            // 
            // _encodingTypeLabel
            // 
            this._encodingTypeLabel.AutoSize = true;
            this._encodingTypeLabel.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._encodingTypeLabel.Location = new System.Drawing.Point(20, 59);
            this._encodingTypeLabel.Name = "_encodingTypeLabel";
            this._encodingTypeLabel.Size = new System.Drawing.Size(76, 12);
            this._encodingTypeLabel.TabIndex = 2;
            this._encodingTypeLabel.Text = "エンコーディング";
            // 
            // _encodingTypeDropDown
            // 
            this._encodingTypeDropDown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._encodingTypeDropDown.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._encodingTypeDropDown.FormattingEnabled = true;
            this._encodingTypeDropDown.Location = new System.Drawing.Point(106, 56);
            this._encodingTypeDropDown.Name = "_encodingTypeDropDown";
            this._encodingTypeDropDown.Size = new System.Drawing.Size(148, 20);
            this._encodingTypeDropDown.TabIndex = 3;
            // 
            // _transmitNlLabel
            // 
            this._transmitNlLabel.AutoSize = true;
            this._transmitNlLabel.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._transmitNlLabel.Location = new System.Drawing.Point(20, 92);
            this._transmitNlLabel.Name = "_transmitNlLabel";
            this._transmitNlLabel.Size = new System.Drawing.Size(63, 12);
            this._transmitNlLabel.TabIndex = 4;
            this._transmitNlLabel.Text = "改行の送信";
            // 
            // _transmitNlDropDown
            // 
            this._transmitNlDropDown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._transmitNlDropDown.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._transmitNlDropDown.FormattingEnabled = true;
            this._transmitNlDropDown.Location = new System.Drawing.Point(106, 89);
            this._transmitNlDropDown.Name = "_transmitNlDropDown";
            this._transmitNlDropDown.Size = new System.Drawing.Size(148, 20);
            this._transmitNlDropDown.TabIndex = 5;
            // 
            // _localEchoCheckBox
            // 
            this._localEchoCheckBox.AutoSize = true;
            this._localEchoCheckBox.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._localEchoCheckBox.Location = new System.Drawing.Point(298, 91);
            this._localEchoCheckBox.Name = "_localEchoCheckBox";
            this._localEchoCheckBox.Size = new System.Drawing.Size(89, 16);
            this._localEchoCheckBox.TabIndex = 6;
            this._localEchoCheckBox.Text = "ローカルエコー";
            this._localEchoCheckBox.UseVisualStyleBackColor = true;
            // 
            // _sshGroupBox
            // 
            this._sshGroupBox.BackColor = System.Drawing.SystemColors.Control;
            this._sshGroupBox.Controls.Add(this._usernameLabel);
            this._sshGroupBox.Controls.Add(this._usernameTextBox);
            this._sshGroupBox.Controls.Add(this._passwordLabel);
            this._sshGroupBox.Controls.Add(this._passwordTextBox);
            this._sshGroupBox.Controls.Add(this._identityFileTextBox);
            this._sshGroupBox.Controls.Add(this._identityFileBrowseButton);
            this._sshGroupBox.Controls.Add(this._identityFileLabel);
            this._sshGroupBox.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._sshGroupBox.Location = new System.Drawing.Point(12, 106);
            this._sshGroupBox.Name = "_sshGroupBox";
            this._sshGroupBox.Size = new System.Drawing.Size(405, 123);
            this._sshGroupBox.TabIndex = 1;
            this._sshGroupBox.TabStop = false;
            this._sshGroupBox.Text = "SSH設定";
            // 
            // _terminalGroupBox
            // 
            this._terminalGroupBox.Controls.Add(this._terminalTypeLabel);
            this._terminalGroupBox.Controls.Add(this._terminalTypeDropDown);
            this._terminalGroupBox.Controls.Add(this._localEchoCheckBox);
            this._terminalGroupBox.Controls.Add(this._encodingTypeLabel);
            this._terminalGroupBox.Controls.Add(this._transmitNlDropDown);
            this._terminalGroupBox.Controls.Add(this._encodingTypeDropDown);
            this._terminalGroupBox.Controls.Add(this._transmitNlLabel);
            this._terminalGroupBox.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._terminalGroupBox.Location = new System.Drawing.Point(12, 235);
            this._terminalGroupBox.Name = "_terminalGroupBox";
            this._terminalGroupBox.Size = new System.Drawing.Size(405, 124);
            this._terminalGroupBox.TabIndex = 2;
            this._terminalGroupBox.TabStop = false;
            this._terminalGroupBox.Text = "端末設定";
            // 
            // _destinationGroupBox
            // 
            this._destinationGroupBox.Controls.Add(this._hostNameLabel);
            this._destinationGroupBox.Controls.Add(this._hostNameTextBox);
            this._destinationGroupBox.Controls.Add(this._portLabel);
            this._destinationGroupBox.Controls.Add(this._portUpDown);
            this._destinationGroupBox.Controls.Add(this._connectionMethodLabel);
            this._destinationGroupBox.Controls.Add(this._connectionMethodDropDown);
            this._destinationGroupBox.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._destinationGroupBox.Location = new System.Drawing.Point(12, 12);
            this._destinationGroupBox.Name = "_destinationGroupBox";
            this._destinationGroupBox.Size = new System.Drawing.Size(405, 88);
            this._destinationGroupBox.TabIndex = 0;
            this._destinationGroupBox.TabStop = false;
            this._destinationGroupBox.Text = "接続先";
            // 
            // _saveOnExitCheckBox
            // 
            this._saveOnExitCheckBox.AutoSize = true;
            this._saveOnExitCheckBox.Checked = true;
            this._saveOnExitCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this._saveOnExitCheckBox.Location = new System.Drawing.Point(310, 365);
            this._saveOnExitCheckBox.Name = "_saveOnExitCheckBox";
            this._saveOnExitCheckBox.Size = new System.Drawing.Size(93, 16);
            this._saveOnExitCheckBox.TabIndex = 6;
            this._saveOnExitCheckBox.TabStop = false;
            this._saveOnExitCheckBox.Text = "終了時に保存";
            this._saveOnExitCheckBox.UseVisualStyleBackColor = true;
            // 
            // _lockDestAndTermCheckBox
            // 
            this._lockDestAndTermCheckBox.AutoSize = true;
            this._lockDestAndTermCheckBox.Location = new System.Drawing.Point(118, 365);
            this._lockDestAndTermCheckBox.Name = "_lockDestAndTermCheckBox";
            this._lockDestAndTermCheckBox.Size = new System.Drawing.Size(165, 16);
            this._lockDestAndTermCheckBox.TabIndex = 7;
            this._lockDestAndTermCheckBox.TabStop = false;
            this._lockDestAndTermCheckBox.Text = "[接続先]と[端末設定]をロック";
            this._lockDestAndTermCheckBox.UseVisualStyleBackColor = true;
            this._lockDestAndTermCheckBox.CheckedChanged += new System.EventHandler(this.lockDestAndTermCheckBox_CheckedChanged);
            // 
            // LoginDialog
            // 
            this.AcceptButton = this._okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._cancelButton;
            this.ClientSize = new System.Drawing.Size(432, 428);
            this.Controls.Add(this._lockDestAndTermCheckBox);
            this.Controls.Add(this._saveOnExitCheckBox);
            this.Controls.Add(this._destinationGroupBox);
            this.Controls.Add(this._terminalGroupBox);
            this.Controls.Add(this._sshGroupBox);
            this.Controls.Add(this._editRenderProfileBbutton);
            this.Controls.Add(this._cancelButton);
            this.Controls.Add(this._okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ログイン";
            this.Load += new System.EventHandler(this.LoginDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this._portUpDown)).EndInit();
            this._sshGroupBox.ResumeLayout(false);
            this._sshGroupBox.PerformLayout();
            this._terminalGroupBox.ResumeLayout(false);
            this._terminalGroupBox.PerformLayout();
            this._destinationGroupBox.ResumeLayout(false);
            this._destinationGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button _okButton;
		private System.Windows.Forms.TextBox _hostNameTextBox;
		private System.Windows.Forms.Label _hostNameLabel;
		private System.Windows.Forms.Label _portLabel;
		private System.Windows.Forms.NumericUpDown _portUpDown;
		private System.Windows.Forms.TextBox _usernameTextBox;
		private System.Windows.Forms.Label _usernameLabel;
		private System.Windows.Forms.Label _passwordLabel;
		private System.Windows.Forms.Button _cancelButton;
		private System.Windows.Forms.TextBox _passwordTextBox;
		private System.Windows.Forms.TextBox _identityFileTextBox;
		private System.Windows.Forms.Button _identityFileBrowseButton;
		private System.Windows.Forms.OpenFileDialog _identityFileDialog;
		private System.Windows.Forms.Label _identityFileLabel;
		private System.Windows.Forms.ComboBox _terminalTypeDropDown;
		private System.Windows.Forms.ComboBox _connectionMethodDropDown;
		private System.Windows.Forms.Label _terminalTypeLabel;
        private System.Windows.Forms.Label _connectionMethodLabel;
        private System.Windows.Forms.Button _editRenderProfileBbutton;
        private System.Windows.Forms.Label _encodingTypeLabel;
        private System.Windows.Forms.ComboBox _encodingTypeDropDown;
        private System.Windows.Forms.Label _transmitNlLabel;
        private System.Windows.Forms.ComboBox _transmitNlDropDown;
        private System.Windows.Forms.CheckBox _localEchoCheckBox;
        private System.Windows.Forms.GroupBox _sshGroupBox;
        private System.Windows.Forms.GroupBox _terminalGroupBox;
        private System.Windows.Forms.GroupBox _destinationGroupBox;
        private System.Windows.Forms.CheckBox _saveOnExitCheckBox;
        private System.Windows.Forms.CheckBox _lockDestAndTermCheckBox;
	}
}