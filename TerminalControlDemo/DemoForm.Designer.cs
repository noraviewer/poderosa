namespace TerminalControlDemo
{
    partial class DemoForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DemoForm));
            this.toolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this.sshTelnetTerminalControl = new Poderosa.TerminalControl.SshTelnetTerminalControl();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.createToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.disposeToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripContainer.ContentPanel.SuspendLayout();
            this.toolStripContainer.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer
            // 
            // 
            // toolStripContainer.ContentPanel
            // 
            this.toolStripContainer.ContentPanel.Controls.Add(this.sshTelnetTerminalControl);
            this.toolStripContainer.ContentPanel.Size = new System.Drawing.Size(541, 330);
            this.toolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer.Name = "toolStripContainer";
            this.toolStripContainer.Size = new System.Drawing.Size(541, 355);
            this.toolStripContainer.TabIndex = 0;
            this.toolStripContainer.Text = "toolStripContainer1";
            // 
            // toolStripContainer.TopToolStripPanel
            // 
            this.toolStripContainer.TopToolStripPanel.Controls.Add(this.toolStrip);
            // 
            // sshTelnetTerminalControl
            // 
            this.sshTelnetTerminalControl.BackColor = System.Drawing.SystemColors.Control;
            this.sshTelnetTerminalControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sshTelnetTerminalControl.Location = new System.Drawing.Point(0, 0);
            this.sshTelnetTerminalControl.LoginProfile = null;
            this.sshTelnetTerminalControl.Name = "sshTelnetTerminalControl";
            this.sshTelnetTerminalControl.Size = new System.Drawing.Size(541, 330);
            this.sshTelnetTerminalControl.TabIndex = 1;
            this.sshTelnetTerminalControl.WakeupTimerMsec = 0;
            // 
            // toolStrip
            // 
            this.toolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createToolStripButton,
            this.disposeToolStripButton});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(541, 25);
            this.toolStrip.Stretch = true;
            this.toolStrip.TabIndex = 0;
            // 
            // createToolStripButton
            // 
            this.createToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("createToolStripButton.Image")));
            this.createToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.createToolStripButton.Name = "createToolStripButton";
            this.createToolStripButton.Size = new System.Drawing.Size(112, 22);
            this.createToolStripButton.Text = "ターミナル生成";
            this.createToolStripButton.Click += new System.EventHandler(this.createToolStripButton_Click);
            // 
            // disposeToolStripButton
            // 
            this.disposeToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("disposeToolStripButton.Image")));
            this.disposeToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.disposeToolStripButton.Name = "disposeToolStripButton";
            this.disposeToolStripButton.Size = new System.Drawing.Size(112, 22);
            this.disposeToolStripButton.Text = "ターミナル廃棄";
            this.disposeToolStripButton.Click += new System.EventHandler(this.disposeToolStripButton_Click);
            // 
            // DemoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 355);
            this.Controls.Add(this.toolStripContainer);
            this.Name = "DemoForm";
            this.Text = "Poderosa TerminalControl Demo";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DemoForm_FormClosing);
            this.Load += new System.EventHandler(this.DemoForm_Load);
            this.toolStripContainer.ContentPanel.ResumeLayout(false);
            this.toolStripContainer.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer.TopToolStripPanel.PerformLayout();
            this.toolStripContainer.ResumeLayout(false);
            this.toolStripContainer.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer;
        private Poderosa.TerminalControl.SshTelnetTerminalControl sshTelnetTerminalControl;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton createToolStripButton;
        private System.Windows.Forms.ToolStripButton disposeToolStripButton;

    }
}

