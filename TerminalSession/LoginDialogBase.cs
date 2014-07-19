/*
 * Copyright 2004,2006 The Poderosa Project.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *
 * $Id: LoginDialogBase.cs,v 1.4 2011/10/27 23:21:58 kzmi Exp $
 */
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

using Poderosa.Plugins;
using Poderosa.Util;
using Poderosa.Terminal;
using Poderosa.ConnectionParam;
using Poderosa.Protocols;
using Poderosa.Forms;
using Poderosa.View;

namespace Poderosa.Sessions {

#if UIDESIGN
    internal class LoginDialogBase : Form, IInterruptableConnectorClient
#else
    internal abstract class LoginDialogBase : Form, IInterruptableConnectorClient
#endif
 {
        private IPoderosaMainWindow _parentWindow;
        private string _originalText;
        private ITerminalConnection _result;
        private IPoderosaView _targetView;

        protected IInterruptable _connector;
        protected ILoginDialogUISupport _loginDialogUISupport;

        protected System.Windows.Forms.Button _loginButton;
        protected System.Windows.Forms.Button _cancelButton;

        private ITerminalSettings _terminalSettings;

#if UIDESIGN
        public LoginDialogBase() {
            InitializeComponent();
        }
#endif

        public LoginDialogBase(IPoderosaMainWindow parentWindow) {
            InitializeComponent();

            _parentWindow = parentWindow;
        }

        private void InitializeComponent() {
            this._loginButton = new System.Windows.Forms.Button();
            this._cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _loginButton
            // 
            this._loginButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._loginButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._loginButton.Location = new System.Drawing.Point(103, 219);
            this._loginButton.Name = "_loginButton";
            this._loginButton.Size = new System.Drawing.Size(75, 23);
            this._loginButton.TabIndex = 0;
            this._loginButton.Click += new System.EventHandler(this.OnOK);
            // 
            // _cancelButton
            // 
            this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._cancelButton.Location = new System.Drawing.Point(191, 219);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(75, 23);
            this._cancelButton.TabIndex = 1;
            // 
            // LoginDialogBase
            // 
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this._cancelButton);
            this.Controls.Add(this._loginButton);
            this.Name = "LoginDialogBase";
            this.ResumeLayout(false);

        }

        //���̂Q�͐������ɃZ�b�g���Ă���
        public ITerminalConnection Result {
            get {
                return _result;
            }
        }
        public IPoderosaView TargetView {
            get {
                return _targetView;
            }
        }

        public ITerminalSettings TerminalSettings {
            get {
                return _terminalSettings;
            }
            set {
                _terminalSettings = value;
            }
        }

        private void InterruptConnecting() {
            _connector.Interrupt();
        }

        private bool IsConnecting {
            get {
                return _connector != null;
            }
        }
#if UIDESIGN
        protected virtual void ShowError(string msg) { }
        protected virtual ITerminalParameter PrepareTerminalParameter() { return null; }
        protected virtual void StartConnection() { }
#else
        protected abstract void ShowError(string msg);
        protected abstract ITerminalParameter PrepareTerminalParameter();
        protected abstract void StartConnection();
#endif

        protected virtual void ClearConnectingState() {
            _loginButton.Enabled = true;
            _cancelButton.Enabled = true;
            this.Cursor = Cursors.Default;
            this.Text = _originalText;
        }

        protected void OnOK(object sender, EventArgs args) {
            this.DialogResult = DialogResult.None;
            _targetView = GetTargetView();
            ITerminalParameter term = PrepareTerminalParameter();
            if (term == null)
                return; //�ݒ�Ɍ�肪����ꍇ

            TerminalControl tc = (TerminalControl)_targetView.GetAdapter(typeof(TerminalControl));
            Size sz = tc.CalcTerminalSize((_terminalSettings.RenderProfile == null) ?
                        this.GetInitialRenderProfile() : _terminalSettings.RenderProfile);
            term.SetTerminalSize(sz.Width, sz.Height);

            _loginButton.Enabled = false;
            _cancelButton.Enabled = false;
            this.Cursor = Cursors.WaitCursor;
            _originalText = this.Text;
            this.Text = String.Format("{0} - {1}", _originalText, TEnv.Strings.GetString("Caption.HowToCancel"));

            StartConnection();
        }
        protected override bool ProcessDialogKey(Keys key) {
            if (this.IsConnecting && (key == (Keys.Control | Keys.C) || key == Keys.Escape)) {
                InterruptConnecting();
                ClearConnectingState();
                return true;
            }
            else
                return base.ProcessDialogKey(key);
        }
        //ISocketWithTimeoutClient �����͂��̃E�B���h�E�Ƃ͕ʂ̃X���b�h�Ŏ��s�����̂ŐT�d��
        public void SuccessfullyExit(ITerminalConnection result) {
            if (this.InvokeRequired) {
                this.Invoke(new SuccessfullyExitDelegate(this.SuccessfullyExit), new object[] { result });
            }
            else {
                _result = result;
                this.DialogResult = DialogResult.OK;
                this.Cursor = Cursors.Default;
                Close();
            }
        }
        public void ConnectionFailed(string message) {
            if (this.InvokeRequired) {
                this.Invoke(new ConnectionFailedDelegate(this.ConnectionFailed), new object[] { message });
            }
            else {
                ClearConnectingState();
                ShowError(message);
            }
        }

        //���O�ݒ�����B�P��t�@�C���ŁB
        protected ISimpleLogSettings CreateSimpleLogSettings(LogType logtype, string path) {
            ISimpleLogSettings logsettings = TerminalSessionsPlugin.Instance.TerminalEmulatorService.CreateDefaultSimpleLogSettings();
            logsettings.LogPath = path;
            logsettings.LogType = logtype;
            LogFileCheckResult r = LogUtil.CheckLogFileName(path, this);
            if (r == LogFileCheckResult.Cancel || r == LogFileCheckResult.Error)
                return null;
            logsettings.LogAppend = (r == LogFileCheckResult.Append);
            return logsettings;
        }

        private IPoderosaView GetTargetView() {
            IViewManager pm = _parentWindow.ViewManager;
            //�Ɨ��E�B���h�E�Ƀ|�b�v�A�b�v������悤�Ȃ��Ƃ͍l���Ă��Ȃ�
            IContentReplaceableView rv = (IContentReplaceableView)pm.GetCandidateViewForNewDocument().GetAdapter(typeof(IContentReplaceableView));
            return rv.AssureViewClass(typeof(TerminalView));
        }
        private RenderProfile GetInitialRenderProfile() {
            return TerminalSessionsPlugin.Instance.TerminalEmulatorService.TerminalEmulatorOptions.CreateRenderProfile();

        }

        //���O�C���_�C�A���O���œ��I��TerminalSetting����U�蕪����z������΃Z�b�g�B�Ȃ����null
        protected void AdjustLoginDialogUISupport(string extension_point_name, string logintype) {
            _loginDialogUISupport = null;
            IExtensionPoint ep = TerminalSessionsPlugin.Instance.PoderosaWorld.PluginManager.FindExtensionPoint(extension_point_name);
            if (ep != null && ep.ExtensionInterface == typeof(ILoginDialogUISupport)) {
                //Preference�Ŋl��
                string config = TerminalSessionsPlugin.Instance.TerminalSessionOptions.GetDefaultLoginDialogUISupportTypeName(logintype);
                foreach (ILoginDialogUISupport sup in ep.GetExtensions()) {
                    if (sup.GetType().FullName == config) {
                        _loginDialogUISupport = sup;
                        return;
                    }
                }
            }
        }


        public static string ToTerminalName(TerminalType tt) {
            switch (tt) {
                case TerminalType.KTerm:
                    return "kterm";
                case TerminalType.XTerm:
                    return "xterm";
                default:
                    return "vt100";
            }
        }

    }
}
