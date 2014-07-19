namespace Poderosa.TerminalControl
{
    partial class InfoDialog
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
            this._loginInfoTitleLabel = new System.Windows.Forms.Label();
            this._programInfoTitleLabel = new System.Windows.Forms.Label();
            this._programInfoRichTextBox = new System.Windows.Forms.RichTextBox();
            this._loginInfoRichTextBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // _okButton
            // 
            this._okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._okButton.Location = new System.Drawing.Point(362, 213);
            this._okButton.Name = "_okButton";
            this._okButton.Size = new System.Drawing.Size(75, 23);
            this._okButton.TabIndex = 0;
            this._okButton.Text = "OK";
            this._okButton.UseVisualStyleBackColor = true;
            // 
            // _loginInfoTitleLabel
            // 
            this._loginInfoTitleLabel.AutoSize = true;
            this._loginInfoTitleLabel.Location = new System.Drawing.Point(12, 9);
            this._loginInfoTitleLabel.Name = "_loginInfoTitleLabel";
            this._loginInfoTitleLabel.Size = new System.Drawing.Size(65, 12);
            this._loginInfoTitleLabel.TabIndex = 1;
            this._loginInfoTitleLabel.Text = "ログイン情報";
            // 
            // _programInfoTitleLabel
            // 
            this._programInfoTitleLabel.AutoSize = true;
            this._programInfoTitleLabel.Location = new System.Drawing.Point(12, 115);
            this._programInfoTitleLabel.Name = "_programInfoTitleLabel";
            this._programInfoTitleLabel.Size = new System.Drawing.Size(74, 12);
            this._programInfoTitleLabel.TabIndex = 3;
            this._programInfoTitleLabel.Text = "プログラム情報";
            // 
            // _programInfoRichTextBox
            // 
            this._programInfoRichTextBox.BackColor = System.Drawing.Color.White;
            this._programInfoRichTextBox.Location = new System.Drawing.Point(12, 130);
            this._programInfoRichTextBox.Name = "_programInfoRichTextBox";
            this._programInfoRichTextBox.ReadOnly = true;
            this._programInfoRichTextBox.Size = new System.Drawing.Size(425, 70);
            this._programInfoRichTextBox.TabIndex = 4;
            this._programInfoRichTextBox.Text = "";
            this._programInfoRichTextBox.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.programInfoRichTextBox_LinkClicked);
            this._programInfoRichTextBox.Enter += new System.EventHandler(this.programInfoRichTextBox_Enter);
            // 
            // _loginInfoRichTextBox
            // 
            this._loginInfoRichTextBox.BackColor = System.Drawing.Color.White;
            this._loginInfoRichTextBox.DetectUrls = false;
            this._loginInfoRichTextBox.Location = new System.Drawing.Point(12, 24);
            this._loginInfoRichTextBox.Name = "_loginInfoRichTextBox";
            this._loginInfoRichTextBox.ReadOnly = true;
            this._loginInfoRichTextBox.Size = new System.Drawing.Size(425, 77);
            this._loginInfoRichTextBox.TabIndex = 5;
            this._loginInfoRichTextBox.Text = "";
            this._loginInfoRichTextBox.Enter += new System.EventHandler(this.loginInfoRichTextBox_Enter);
            // 
            // InfoDialog
            // 
            this.AcceptButton = this._okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(449, 248);
            this.Controls.Add(this._loginInfoRichTextBox);
            this.Controls.Add(this._programInfoRichTextBox);
            this.Controls.Add(this._programInfoTitleLabel);
            this.Controls.Add(this._loginInfoTitleLabel);
            this.Controls.Add(this._okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InfoDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "情報";
            this.Load += new System.EventHandler(this.InfoDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button _okButton;
        private System.Windows.Forms.Label _loginInfoTitleLabel;
        private System.Windows.Forms.Label _programInfoTitleLabel;
        private System.Windows.Forms.RichTextBox _programInfoRichTextBox;
        private System.Windows.Forms.RichTextBox _loginInfoRichTextBox;
    }
}