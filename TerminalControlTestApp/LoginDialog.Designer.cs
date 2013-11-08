namespace TerminalControlTestApp
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
			this._terminalTypeDropdown = new System.Windows.Forms.ComboBox();
			this._protocolTypeDropdown = new System.Windows.Forms.ComboBox();
			this._terminalTypeLabel = new System.Windows.Forms.Label();
			this._protocolTypeLabel = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this._portUpDown)).BeginInit();
			this.SuspendLayout();
			// 
			// _okButton
			// 
			this._okButton.Location = new System.Drawing.Point(168, 220);
			this._okButton.Name = "_okButton";
			this._okButton.Size = new System.Drawing.Size(75, 23);
			this._okButton.TabIndex = 12;
			this._okButton.Text = "OK";
			this._okButton.UseVisualStyleBackColor = true;
			this._okButton.Click += new System.EventHandler(this._okButton_Click);
			// 
			// _hostNameTextBox
			// 
			this._hostNameTextBox.Location = new System.Drawing.Point(107, 17);
			this._hostNameTextBox.Name = "_hostNameTextBox";
			this._hostNameTextBox.Size = new System.Drawing.Size(150, 20);
			this._hostNameTextBox.TabIndex = 1;
			// 
			// _hostNameLabel
			// 
			this._hostNameLabel.AutoSize = true;
			this._hostNameLabel.Location = new System.Drawing.Point(69, 21);
			this._hostNameLabel.Name = "_hostNameLabel";
			this._hostNameLabel.Size = new System.Drawing.Size(32, 13);
			this._hostNameLabel.TabIndex = 2;
			this._hostNameLabel.Text = "Host:";
			// 
			// _portLabel
			// 
			this._portLabel.AutoSize = true;
			this._portLabel.Location = new System.Drawing.Point(72, 46);
			this._portLabel.Name = "_portLabel";
			this._portLabel.Size = new System.Drawing.Size(29, 13);
			this._portLabel.TabIndex = 3;
			this._portLabel.Text = "Port:";
			// 
			// _portUpDown
			// 
			this._portUpDown.Location = new System.Drawing.Point(107, 44);
			this._portUpDown.Maximum = new decimal(new int[] {
            65000,
            0,
            0,
            0});
			this._portUpDown.Name = "_portUpDown";
			this._portUpDown.Size = new System.Drawing.Size(71, 20);
			this._portUpDown.TabIndex = 4;
			this._portUpDown.Value = new decimal(new int[] {
            22,
            0,
            0,
            0});
			// 
			// _usernameTextBox
			// 
			this._usernameTextBox.Location = new System.Drawing.Point(107, 71);
			this._usernameTextBox.Name = "_usernameTextBox";
			this._usernameTextBox.Size = new System.Drawing.Size(150, 20);
			this._usernameTextBox.TabIndex = 5;
			// 
			// _usernameLabel
			// 
			this._usernameLabel.AutoSize = true;
			this._usernameLabel.Location = new System.Drawing.Point(43, 74);
			this._usernameLabel.Name = "_usernameLabel";
			this._usernameLabel.Size = new System.Drawing.Size(58, 13);
			this._usernameLabel.TabIndex = 7;
			this._usernameLabel.Text = "Username:";
			// 
			// _passwordLabel
			// 
			this._passwordLabel.AutoSize = true;
			this._passwordLabel.Location = new System.Drawing.Point(45, 101);
			this._passwordLabel.Name = "_passwordLabel";
			this._passwordLabel.Size = new System.Drawing.Size(56, 13);
			this._passwordLabel.TabIndex = 8;
			this._passwordLabel.Text = "Password:";
			// 
			// _cancelButton
			// 
			this._cancelButton.Location = new System.Drawing.Point(249, 220);
			this._cancelButton.Name = "_cancelButton";
			this._cancelButton.Size = new System.Drawing.Size(75, 23);
			this._cancelButton.TabIndex = 13;
			this._cancelButton.Text = "Cancel";
			this._cancelButton.UseVisualStyleBackColor = true;
			this._cancelButton.Click += new System.EventHandler(this._cancelButton_Click);
			// 
			// _passwordTextBox
			// 
			this._passwordTextBox.Location = new System.Drawing.Point(107, 97);
			this._passwordTextBox.Name = "_passwordTextBox";
			this._passwordTextBox.PasswordChar = '*';
			this._passwordTextBox.Size = new System.Drawing.Size(148, 20);
			this._passwordTextBox.TabIndex = 7;
			// 
			// _identityFileTextBox
			// 
			this._identityFileTextBox.Location = new System.Drawing.Point(107, 124);
			this._identityFileTextBox.Name = "_identityFileTextBox";
			this._identityFileTextBox.Size = new System.Drawing.Size(148, 20);
			this._identityFileTextBox.TabIndex = 8;
			// 
			// _identityFileBrowseButton
			// 
			this._identityFileBrowseButton.Location = new System.Drawing.Point(259, 123);
			this._identityFileBrowseButton.Name = "_identityFileBrowseButton";
			this._identityFileBrowseButton.Size = new System.Drawing.Size(24, 21);
			this._identityFileBrowseButton.TabIndex = 9;
			this._identityFileBrowseButton.Text = "...";
			this._identityFileBrowseButton.UseVisualStyleBackColor = true;
			this._identityFileBrowseButton.Click += new System.EventHandler(this._identityFileBrowseButton_Click);
			// 
			// _identityFileLabel
			// 
			this._identityFileLabel.AutoSize = true;
			this._identityFileLabel.Location = new System.Drawing.Point(41, 127);
			this._identityFileLabel.Name = "_identityFileLabel";
			this._identityFileLabel.Size = new System.Drawing.Size(60, 13);
			this._identityFileLabel.TabIndex = 12;
			this._identityFileLabel.Text = "Identity file:";
			// 
			// _terminalTypeDropdown
			// 
			this._terminalTypeDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._terminalTypeDropdown.FormattingEnabled = true;
			this._terminalTypeDropdown.Items.AddRange(new object[] {
            "VT100",
            "XTerm",
            "KTerm"});
			this._terminalTypeDropdown.Location = new System.Drawing.Point(107, 151);
			this._terminalTypeDropdown.Name = "_terminalTypeDropdown";
			this._terminalTypeDropdown.Size = new System.Drawing.Size(121, 21);
			this._terminalTypeDropdown.TabIndex = 10;
			// 
			// _protocolTypeDropdown
			// 
			this._protocolTypeDropdown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._protocolTypeDropdown.FormattingEnabled = true;
			this._protocolTypeDropdown.Items.AddRange(new object[] {
            "SSH1",
            "SSH2"});
			this._protocolTypeDropdown.Location = new System.Drawing.Point(107, 179);
			this._protocolTypeDropdown.Name = "_protocolTypeDropdown";
			this._protocolTypeDropdown.Size = new System.Drawing.Size(121, 21);
			this._protocolTypeDropdown.TabIndex = 11;
			// 
			// _terminalTypeLabel
			// 
			this._terminalTypeLabel.AutoSize = true;
			this._terminalTypeLabel.Location = new System.Drawing.Point(28, 154);
			this._terminalTypeLabel.Name = "_terminalTypeLabel";
			this._terminalTypeLabel.Size = new System.Drawing.Size(73, 13);
			this._terminalTypeLabel.TabIndex = 15;
			this._terminalTypeLabel.Text = "Terminal type:";
			// 
			// _protocolTypeLabel
			// 
			this._protocolTypeLabel.AutoSize = true;
			this._protocolTypeLabel.Location = new System.Drawing.Point(29, 182);
			this._protocolTypeLabel.Name = "_protocolTypeLabel";
			this._protocolTypeLabel.Size = new System.Drawing.Size(72, 13);
			this._protocolTypeLabel.TabIndex = 16;
			this._protocolTypeLabel.Text = "Protocol type:";
			// 
			// LoginDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(336, 256);
			this.Controls.Add(this._protocolTypeLabel);
			this.Controls.Add(this._terminalTypeLabel);
			this.Controls.Add(this._protocolTypeDropdown);
			this.Controls.Add(this._terminalTypeDropdown);
			this.Controls.Add(this._identityFileLabel);
			this.Controls.Add(this._identityFileBrowseButton);
			this.Controls.Add(this._identityFileTextBox);
			this.Controls.Add(this._passwordTextBox);
			this.Controls.Add(this._cancelButton);
			this.Controls.Add(this._passwordLabel);
			this.Controls.Add(this._usernameLabel);
			this.Controls.Add(this._usernameTextBox);
			this.Controls.Add(this._portUpDown);
			this.Controls.Add(this._portLabel);
			this.Controls.Add(this._hostNameLabel);
			this.Controls.Add(this._hostNameTextBox);
			this.Controls.Add(this._okButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "LoginDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Login";
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LoginDialog_KeyDown);
			((System.ComponentModel.ISupportInitialize)(this._portUpDown)).EndInit();
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
		private System.Windows.Forms.ComboBox _terminalTypeDropdown;
		private System.Windows.Forms.ComboBox _protocolTypeDropdown;
		private System.Windows.Forms.Label _terminalTypeLabel;
		private System.Windows.Forms.Label _protocolTypeLabel;
	}
}