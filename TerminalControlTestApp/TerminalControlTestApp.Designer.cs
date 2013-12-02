namespace TerminalControlTestApp
{
	partial class TerminalControlTestApp
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TerminalControlTestApp));
			this._terminalTabs = new System.Windows.Forms.TabControl();
			this._newTerminalTab = new System.Windows.Forms.TabPage();
			this._terminal = new Poderosa.TerminalControl.SshTerminalControl();
			this._terminalTabs.SuspendLayout();
			this._newTerminalTab.SuspendLayout();
			this.SuspendLayout();
			// 
			// _terminalTabs
			// 
			this._terminalTabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._terminalTabs.Controls.Add(this._newTerminalTab);
			this._terminalTabs.Location = new System.Drawing.Point(4, 4);
			this._terminalTabs.Name = "_terminalTabs";
			this._terminalTabs.SelectedIndex = 0;
			this._terminalTabs.Size = new System.Drawing.Size(843, 532);
			this._terminalTabs.TabIndex = 0;
			this._terminalTabs.SelectedIndexChanged += new System.EventHandler(this._terminalTabs_SelectedIndexChanged);
			// 
			// _newTerminalTab
			// 
			this._newTerminalTab.Controls.Add(this._terminal);
			this._newTerminalTab.Location = new System.Drawing.Point(4, 22);
			this._newTerminalTab.Name = "_newTerminalTab";
			this._newTerminalTab.Padding = new System.Windows.Forms.Padding(3);
			this._newTerminalTab.Size = new System.Drawing.Size(835, 506);
			this._newTerminalTab.TabIndex = 1;
			this._newTerminalTab.Text = "Create New...";
			this._newTerminalTab.UseVisualStyleBackColor = true;
			// 
			// _terminal
			// 
			this._terminal.BackColor = System.Drawing.Color.Black;
			this._terminal.Dock = System.Windows.Forms.DockStyle.Fill;
			this._terminal.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._terminal.ForeColor = System.Drawing.Color.Silver;
			this._terminal.HostName = null;
			this._terminal.IdentityFile = null;
			this._terminal.Location = new System.Drawing.Point(3, 3);
			this._terminal.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
			this._terminal.Name = "_terminal";
			this._terminal.Password = null;
			this._terminal.Port = 22;
			this._terminal.Size = new System.Drawing.Size(829, 500);
			this._terminal.SshProtocol = Poderosa.TerminalControl.SshProtocol.SSH2;
			this._terminal.TabIndex = 0;
			this._terminal.TerminalType = Poderosa.TerminalControl.TerminalType.VT100;
			this._terminal.Username = null;
			// 
			// TerminalControlTest
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(850, 539);
			this.Controls.Add(this._terminalTabs);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "TerminalControlTest";
			this.Text = "Terminal Control Test";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this._terminalTabs.ResumeLayout(false);
			this._newTerminalTab.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl _terminalTabs;
		private System.Windows.Forms.TabPage _newTerminalTab;
		private Poderosa.TerminalControl.SshTerminalControl _terminal;

	}
}

