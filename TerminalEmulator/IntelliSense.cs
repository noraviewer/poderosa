/*
 * Copyright 2004,2006 The Poderosa Project.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *
 * $Id: IntelliSense.cs,v 1.4 2011/12/10 12:36:51 kzmi Exp $
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

using Poderosa.View;
using Poderosa.Document;

namespace Poderosa.Terminal {
    internal enum IntelliSenseMode {
        CharComplement, //���O�̕�����őO����v������݂̂̂����X�g����
        ArgComplement   //�����ɂ�����Ȃ�
    }
    internal enum IntelliSenseSort {
        Historical,
        Alphabet
    }

    //���M->TerminalDocument�X�V�܂ł̊Ԃɕ⊮�����݂��ꍇ�̃J�o�[�����O�̂��߂̃L���[
    internal class CharQueue {
        private char[] _buffer;
        private int _start;
        private int _offset;

        public CharQueue() {
            _buffer = new char[80];
        }

        public void PushChar(char ch) {
            _buffer[_offset++] = ch;
        }
        public void LockedPushChar(char ch) {
            lock (this) {
                PushChar(ch);
            }
        }
        public char PopChar() {
            char r = _buffer[_start++];
            if (_start == _offset) {
                Clear();
            }
            return r;
        }
        public bool IsEmpty {
            get {
                return _offset == _start;
            }
        }
        public void Clear() {
            _offset = 0;
            _start = 0;
        }
        //�������B�L�[���͕s�v�̂Ƃ���\0�n��
        public void LockedInit(char ch) {
            lock (this) {
                Clear();
                if (ch != '\0')
                    PushChar(ch);
            }
        }

        //�ʃX���b�h����̎�M�ŁA�P�|�b�v����B�ꕶ�����M����Ή��炩�̍X�V������͂�
        public void LockedSafePopChar() {
            lock (this) {
                if (!this.IsEmpty)
                    PopChar();
            }
        }
    }


    //�v�����v�g��ԂƃL�[���͂��󂯎��AIntelliSenseContext�쐬�ƃ|�b�v�A�b�v���s��
    internal class IntelliSense : IPromptProcessor {
        private static IntelliSenseWindow _intelliSenseWindow; //����͑S�̂ŋ��ʂł悢
        private IntelliSenseContext _context;
        private AbstractTerminal _terminal;
        private string _currentCommand; //null�͔�v�����v�g��Ԃ�����
        private GLine _promptLine;
        private bool _cancelLockFlag; //�蓮�L�����Z��������蓮Enter������܂Ŏ����|�b�v�A�b�v�͂��Ȃ�

        public IntelliSense(AbstractTerminal terminal) {
            _terminal = terminal;
            _context = new IntelliSenseContext(this);
            _terminal.PromptRecognizer.AddListener(this);
        }
        public AbstractTerminal Terminal {
            get {
                return _terminal;
            }
        }
        public bool OwnsIntelliSenseWindow {
            get {
                return _intelliSenseWindow != null && _intelliSenseWindow.CurrentContext == _context;
            }
        }
        public GLine PromptLine {
            get {
                return _promptLine;
            }
        }
        public string WholeCommand {
            get {
                return _currentCommand;
            }
        }
        public void SetCancelLockFlag() {
            _cancelLockFlag = true;
        }

        //TerminalControl���痈����
        public bool ProcessKey(Keys modifiers, Keys keybody) {
            if (TerminalEmulatorPlugin.Instance.TerminalEmulatorOptions.IntelliSenseKey == (modifiers | keybody)) {
                if (CanPopupIntelliSense()) {
                    PopupMain('\0');
                    return true;
                }
            }
            else if (modifiers == Keys.None && keybody == Keys.Enter) { //�R�}���h���͂ƃ��X�g�̍X�V
                _cancelLockFlag = false;
                //Enter�O�ɂ̓v�����v�g�F����K���X�V
                _terminal.PromptRecognizer.CheckIfUpdated();
                if (_currentCommand != null && _currentCommand.Length > 0) {
                    _context.UpdateCommandList(_currentCommand);
                }
                else { //�����s�����Ƃ��̏ꍇ���~�ρB�������Ɏg������͈������낤
                    TryParseMultiLineCommand();
                }
            }

            return false;
        }
        private void TryParseMultiLineCommand() {
            GLine current = _terminal.GetDocument().CurrentLine;
            GLine command_start_candidate = current;
            IShellScheme scheme = GetTerminalSettings().ShellScheme;

            //����������Ɨ��������Ȃ��̂ŕ����s�͂�߂Ă����B
            //���Ƃ��΁A
            //  > user *****
            //  > Password:
            //�݂����Ȃ��Ƃ������ƁA�����I�ɕ����s�R�}���h�Ȃ̂��͋�ʂ��t���Ȃ��B�S�s�̉E�̂ق��܂Ńe�L�X�g�����܂��Ă��邱�ƂŔ��f���邭�炢�����A100%�ł͂Ȃ�
            int limit = 1;
            while (command_start_candidate != null && limit > 0) {
                string prompt;
                string command;
                if (_terminal.PromptRecognizer.DeterminePromptLine(command_start_candidate, current.ID, current.DisplayLength, out prompt, out command)) {
                    if (command.Length > 0)
                        _context.UpdateCommandList(command);
                }
                command_start_candidate = command_start_candidate.PrevLine;

                limit--;
            }
        }

        public void ProcessChar(char ch) {
            if (CanPopupIntelliSense() && IsPrintableChar(ch) && !_cancelLockFlag) {
                PopupMain(ch);
            }
        }

        private bool CanPopupIntelliSense() {
            return _currentCommand != null && !this.OwnsIntelliSenseWindow;
        }
        private bool IsPrintableChar(char ch) {
            int i = (int)ch;
            return 0x21 <= i && i <= 0x7E;
        }

        //append_char�́A�C���e���Z���X�N�������������͂ł���Ƃ����̕����ACtrl+.�Ȃǂ̒��ڋN���ł���Ƃ�\0
        private void PopupMain(char append_char) {
            IShellScheme ss = GetTerminalSettings().ShellScheme;
            StringBuilder buf = new StringBuilder();
            buf.Append(_currentCommand);
            if (append_char != '\0')
                buf.Append(append_char);

            string line = buf.ToString();
            string[] args = ss.ParseCommandInput(line);
            //(��U�p�~)���{�ꂪ�����Ă����CaretColumn�ŒT���ƃA�E�g�ɂȂ�B���{�I�ɒ����ɂ̓R�}���h�p�[�T��GLine�̓�����m���Ă��Ȃ��Ƃ����Ȃ�
            //int cc = _terminal.GetDocument().CaretColumn;
            IntelliSenseMode mode = line.Length == 0 || ss.IsDelimiter(line[line.Length - 1]) ? IntelliSenseMode.ArgComplement : IntelliSenseMode.CharComplement;

            _context.Init(_terminal, ss, args, mode, append_char);
            if (!_context.IsEmpty) {
                if (_intelliSenseWindow == null)
                    _intelliSenseWindow = new IntelliSenseWindow(); //�x���쐬
                _intelliSenseWindow.Popup(_context);
            }
        }
        private ITerminalSettings GetTerminalSettings() {
            return _terminal.TerminalHost.TerminalSettings;
        }

        #region IPromptProcessor
        //��M�X���b�h�Ŏ��s���邱�Ƃ�����
        public void OnPromptLine(GLine line, string prompt, string command) {
            _currentCommand = command;
            Debug.WriteLineIf(DebugOpt.IntelliSense, "Command " + _currentCommand);
            _promptLine = line;
            if (this.OwnsIntelliSenseWindow) {
                Debug.WriteLineIf(DebugOpt.IntelliSense, "LockedPop");
                _context.CharQueue.LockedSafePopChar();
            }
        }

        public void OnNotPromptLine() {
            if (this.OwnsIntelliSenseWindow) {
                Debug.Assert(_context.OwnerControl.InvokeRequired);
                IAsyncResult ar = _context.OwnerControl.BeginInvoke(_intelliSenseWindow.CancelDelegate);

                /* NOTE
                /*  ������TerminalDocument�̃��b�N���Ɏ�M�X���b�h�ɂ����Ď��s����邪�A���������s����Ƃ����C���X���b�h��OnPaint���őҋ@���Ă���
                 *  ���Ƃ�����B���̂Ƃ���EndInvoke�Ńf�b�h���b�N�ɂȂ�B
                 *  �����ł̎��s��BeginInvoke�������Ă����΂悭�A���s������҂K�v�͂Ȃ��̂ł��邪�AEndInvoke���Ă΂Ȃ���IAsyncResult���Ŏ����Ă���
                 *  ���\�[�X�����[�N���邩������Ȃ��B
                 *  ����͂����ƒ��ׂ�K�v�����邪�A���ׂ�̂��ʓ|�Ȃ̂Łu���C���X���b�h���u���b�N���Ă���Ƃ��̂݁i��Wait�Ɏ��s�����Ƃ��̂݁j
                 *  EndInvoke�����ڂ�v�Ƃ����悤�ɂ��Ă����B�������\�[�X���[�N�̊댯���Ȃ��̂ł����EndInvoke�͕s�v�B
                 * 
                 *  IAsyncResult���R���N�V�����ɗ��߂Ă����āA�Z���Ԃ�WaitOne�ɐ������邲�ƂɃR���N�V��������O���Ƃ����������
                 */

                if (ar.AsyncWaitHandle.WaitOne(100, false))
                    _context.OwnerControl.EndInvoke(ar);
            }
            Debug.WriteLineIf(DebugOpt.IntelliSense, "OnNotPrompt");
            _currentCommand = null;
        }
        #endregion
    }

    //���̃R���N�V����
    internal class IntelliSenseCandidateList : IIntelliSenseCandidateList {
        private List<IIntelliSenseItem> _items;
        public IntelliSenseCandidateList() {
            _items = new List<IIntelliSenseItem>();
        }
        public void AddItem(IIntelliSenseItem item) {
            _items.Add(item);
        }
        public void RemoveItem(IIntelliSenseItem item) {
            _items.Remove(item);
        }
        public void Clear() {
            _items.Clear();
        }
        public int Count {
            get {
                return _items.Count;
            }
        }
        public void Sort() {
            _items.Sort();
        }
        public int IndexOf(IIntelliSenseItem item) {
            return _items.IndexOf(item);
        }

        IEnumerator<IIntelliSenseItem> IEnumerable<IIntelliSenseItem>.GetEnumerator() {
            return _items.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator() {
            return _items.GetEnumerator();
        }
        public IAdaptable GetAdapter(Type adapter) {
            return TerminalEmulatorPlugin.Instance.PoderosaWorld.AdapterManager.GetAdapter(this, adapter);
        }
    }


    //���|�b�v�A�b�v���邲�Ƃɏ�������������
    internal class IntelliSenseContext {
        private IntelliSense _owner;
        private Point _commandStartPoint; //�L�����b�g�ʒu���e�L�X�g���W��
        private TerminalControl _ownerControl;
        private IShellScheme _scheme;
        private IntelliSenseMode _intelliSenseMode;
        private IntelliSenseSort _sortStyle;
        private IntelliSenseCandidateList _candidates;
        private CharQueue _charQueue;
        private StringBuilder _buffer;
        private string[] _currentInput;
        private int _initialSelectedIndex;

        public IntelliSenseContext(IntelliSense owner) {
            _owner = owner;
            _candidates = new IntelliSenseCandidateList();
            _buffer = new StringBuilder();
            _charQueue = new CharQueue();
        }

        public IntelliSenseSort SortStyle {
            get {
                return _sortStyle;
            }
            set {
                _sortStyle = value;
            }
        }

        public Point CommandStartPoint {
            get {
                return _commandStartPoint;
            }
            set {
                _commandStartPoint = value;
            }
        }
        public IntelliSense Owner {
            get {
                return _owner;
            }
        }
        public TerminalControl OwnerControl {
            get {
                return _ownerControl;
            }
        }
        public IShellScheme CurrentScheme {
            get {
                return _scheme;
            }
        }
        public RenderProfile RenderProfile {
            get {
                return _owner.Terminal.TerminalHost.TerminalControl.GetRenderProfile();
            }
        }

        public bool IsEmpty {
            get {
                return _candidates.Count == 0;
            }
        }
        public string[] ConcatToCurrentInput(string[] after) {
            string[] r = new string[_currentInput.Length + after.Length];
            for (int i = 0; i < _currentInput.Length; i++)
                r[i] = _currentInput[i];
            for (int i = 0; i < after.Length; i++)
                r[_currentInput.Length + i] = after[i];
            return r;
        }

        public CharQueue CharQueue {
            get {
                return _charQueue;
            }
        }

        public IEnumerable<IIntelliSenseItem> Candidates {
            get {
                return _candidates;
            }
        }

        public int InitialSelectedIndex {
            get {
                return _initialSelectedIndex;
            }
        }

        //�o�b�t�@�}�l�[�W�����g�n
        public string AppendChar(char ch) {
            _buffer.Append(ch);
            return _buffer.ToString();
        }
        public string RemoveChar() {
            if (_buffer.Length > 0)
                _buffer.Remove(_buffer.Length - 1, 1);
            return _buffer.ToString();
        }


        public void Init(AbstractTerminal terminal, IShellScheme scheme, string[] current_input, IntelliSenseMode mode, char append_char) {
            _ownerControl = terminal.TerminalHost.TerminalControl;
            Debug.Assert(_ownerControl != null);
            TerminalDocument doc = terminal.GetDocument();
            _commandStartPoint = new Point(doc.CaretColumn + (append_char == '\0' ? 0 : 1), doc.CurrentLineNumber - doc.TopLineNumber);
            Debug.WriteLineIf(DebugOpt.IntelliSense, String.Format("IS CtxInit M={0} CaretC={1}", mode.ToString(), doc.CaretColumn));
            _scheme = scheme;
            _currentInput = current_input;
            _intelliSenseMode = mode;
            Debug.Assert(_currentInput != null);

            _charQueue.LockedInit(append_char);

            _buffer.Remove(0, _buffer.Length);
            if (_intelliSenseMode == IntelliSenseMode.CharComplement) {
                string last_arg = current_input[current_input.Length - 1];
                _buffer.Append(last_arg);
                _commandStartPoint.X -= last_arg.Length;
            }

            BuildCandidates();
        }

        public void UpdateCommandList(string command) {
            if (_scheme == null)
                return; //�֘A�t����ꂽ���̂��Ȃ��Ƃ��͉������Ȃ�

            string[] cmds = _scheme.ParseCommandInput(command);
            if (cmds.Length > 0) {
                Debug.WriteLineIf(DebugOpt.IntelliSense, "Update Command " + command);
                _scheme.CommandHistory.UpdateItem(cmds);
            }
        }

        public void Complement(string complement) {
            _buffer.Remove(0, _buffer.Length);
            if (_intelliSenseMode == IntelliSenseMode.CharComplement)
                _currentInput[_currentInput.Length - 1] = complement;
            else {
                //append
                string[] t = new string[_currentInput.Length + 1];
                Array.Copy(_currentInput, 0, t, 0, _currentInput.Length);
                t[t.Length - 1] = complement;
                _currentInput = t;
            }

            _intelliSenseMode = IntelliSenseMode.ArgComplement;

            BuildCandidates();
        }

        public void BuildCandidates() {
            _candidates.Clear();
            _initialSelectedIndex = -1;
            IntelliSenseItem initial = null;
            IntelliSenseItemCollection col = (IntelliSenseItemCollection)_scheme.CommandHistory.GetAdapter(typeof(IntelliSenseItemCollection));
            foreach (IntelliSenseItem item in col.Items) {
                IntelliSenseItem.MatchForwardResult r = item.MatchForward(_currentInput);
                if (_intelliSenseMode == IntelliSenseMode.ArgComplement && r == IntelliSenseItem.MatchForwardResult.PartialArg) {
                    _candidates.AddItem(new IntelliSenseItem(item.Text, _currentInput.Length, item));
                }
                else if (_intelliSenseMode == IntelliSenseMode.CharComplement && r == IntelliSenseItem.MatchForwardResult.PartialChar) {
                    IntelliSenseItem i = new IntelliSenseItem(item.Text, _currentInput.Length - 1, item);
                    if (initial == null)
                        initial = i; //Partial�̈�v�������ŏ��̂��̂������l�Ƃ���
                    _candidates.AddItem(i);
                }
            }

            //�O���ɒ��߂̋@���^����
            IIntelliSenseCandidateExtension[] extensions = TerminalEmulatorPlugin.Instance.IntelliSenseExtensions;
            if (extensions.Length > 0) {
                foreach (IIntelliSenseCandidateExtension e in extensions)
                    e.AdjustItem(_owner.Terminal, _candidates, _currentInput);
            }

            if (_sortStyle == IntelliSenseSort.Alphabet) {
                _candidates.Sort();
            }

            if (initial == null && _candidates.Count > 0)
                _initialSelectedIndex = 0; //�d���Ȃ������I��
            else
                _initialSelectedIndex = _candidates.IndexOf(initial);
        }
    }
}
