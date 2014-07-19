namespace Poderosa.TerminalControl
{
    partial class SshTelnetTerminalControl
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this._toolStrip = new System.Windows.Forms.ToolStrip();
            this._iconToolStripLabel = new System.Windows.Forms.ToolStripLabel();
            this._hostInfoToolStripLabel = new System.Windows.Forms.ToolStripLabel();
            this._connectToolStripButton = new System.Windows.Forms.ToolStripButton();
            this._disconnectToolStripButton = new System.Windows.Forms.ToolStripButton();
            this._settingToolStripButton = new System.Windows.Forms.ToolStripButton();
            this._wakeupTimer = new System.Windows.Forms.Timer(this.components);
            this._conditionLabel = new Poderosa.TerminalControl.ImageStyleLabel();
            this._toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // _toolStrip
            // 
            this._toolStrip.AutoSize = false;
            this._toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._iconToolStripLabel,
            this._hostInfoToolStripLabel,
            this._connectToolStripButton,
            this._disconnectToolStripButton,
            this._settingToolStripButton});
            this._toolStrip.Location = new System.Drawing.Point(0, 0);
            this._toolStrip.Name = "_toolStrip";
            this._toolStrip.Size = new System.Drawing.Size(339, 25);
            this._toolStrip.TabIndex = 0;
            this._toolStrip.Text = "toolStrip1";
            // 
            // _iconToolStripLabel
            // 
            this._iconToolStripLabel.AutoSize = false;
            this._iconToolStripLabel.BackColor = System.Drawing.Color.Transparent;
            this._iconToolStripLabel.BackgroundImage = global::Poderosa.TerminalControl.Properties.Resources.host;
            this._iconToolStripLabel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this._iconToolStripLabel.Name = "_iconToolStripLabel";
            this._iconToolStripLabel.Size = new System.Drawing.Size(22, 22);
            this._iconToolStripLabel.Click += new System.EventHandler(this.iconToolStripLabel_Click);
            // 
            // _hostInfoToolStripLabel
            // 
            this._hostInfoToolStripLabel.Font = new System.Drawing.Font("メイリオ", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._hostInfoToolStripLabel.ForeColor = System.Drawing.Color.DimGray;
            this._hostInfoToolStripLabel.Name = "_hostInfoToolStripLabel";
            this._hostInfoToolStripLabel.Size = new System.Drawing.Size(80, 22);
            this._hostInfoToolStripLabel.Text = "接続先ホスト";
            this._hostInfoToolStripLabel.Click += new System.EventHandler(this.hostInfoToolStripLabel_Click);
            // 
            // _connectToolStripButton
            // 
            this._connectToolStripButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this._connectToolStripButton.Image = global::Poderosa.TerminalControl.Properties.Resources.connect;
            this._connectToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._connectToolStripButton.Name = "_connectToolStripButton";
            this._connectToolStripButton.Size = new System.Drawing.Size(52, 22);
            this._connectToolStripButton.Text = "接続";
            this._connectToolStripButton.Click += new System.EventHandler(this.connectToolStripButton_Click);
            // 
            // _disconnectToolStripButton
            // 
            this._disconnectToolStripButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this._disconnectToolStripButton.Image = global::Poderosa.TerminalControl.Properties.Resources.disconnect;
            this._disconnectToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._disconnectToolStripButton.Name = "_disconnectToolStripButton";
            this._disconnectToolStripButton.Size = new System.Drawing.Size(52, 22);
            this._disconnectToolStripButton.Text = "切断";
            this._disconnectToolStripButton.Click += new System.EventHandler(this.disconnectToolStripButton_Click);
            // 
            // _settingToolStripButton
            // 
            this._settingToolStripButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this._settingToolStripButton.Image = global::Poderosa.TerminalControl.Properties.Resources.config;
            this._settingToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._settingToolStripButton.Name = "_settingToolStripButton";
            this._settingToolStripButton.Size = new System.Drawing.Size(52, 22);
            this._settingToolStripButton.Text = "設定";
            this._settingToolStripButton.Click += new System.EventHandler(this._settingToolStripButton_Click);
            // 
            // _wakeupTimer
            // 
            this._wakeupTimer.Interval = 1000;
            this._wakeupTimer.Tick += new System.EventHandler(this.wakeupTimer_Tick);
            // 
            // _conditionLabel
            // 
            this._conditionLabel.BackColor = System.Drawing.Color.DimGray;
            this._conditionLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._conditionLabel.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this._conditionLabel.ForeColor = System.Drawing.SystemColors.Control;
            this._conditionLabel.Image = null;
            this._conditionLabel.ImageStyle = Poderosa.View.ImageStyle.Center;
            this._conditionLabel.Location = new System.Drawing.Point(0, 25);
            this._conditionLabel.Name = "_conditionLabel";
            this._conditionLabel.Size = new System.Drawing.Size(339, 232);
            this._conditionLabel.TabIndex = 1;
            this._conditionLabel.Text = "接続されていません";
            this._conditionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SshTelnetTerminalControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this._conditionLabel);
            this.Controls.Add(this._toolStrip);
            this.Name = "SshTelnetTerminalControl";
            this.Size = new System.Drawing.Size(339, 257);
            this.Load += new System.EventHandler(this.SshTelnetTerminalControl_Load);
            this._toolStrip.ResumeLayout(false);
            this._toolStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip _toolStrip;
        private System.Windows.Forms.ToolStripLabel _hostInfoToolStripLabel;
        private System.Windows.Forms.ToolStripButton _connectToolStripButton;
        private Poderosa.TerminalControl.ImageStyleLabel _conditionLabel;
        private System.Windows.Forms.Timer _wakeupTimer;
        private System.Windows.Forms.ToolStripButton _disconnectToolStripButton;
        private System.Windows.Forms.ToolStripLabel _iconToolStripLabel;
        private System.Windows.Forms.ToolStripButton _settingToolStripButton;
    }
}
