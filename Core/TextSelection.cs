/*
 * Copyright 2004,2006 The Poderosa Project.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *
 * $Id: TextSelection.cs,v 1.4 2011/12/10 09:59:38 kzmi Exp $
 */
using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;

using Poderosa.Sessions;
using Poderosa.Document;
using Poderosa.Forms;
using Poderosa.Commands;

namespace Poderosa.View {
    internal enum RangeType {
        Char,
        Word,
        Line
    }
    internal enum SelectionState {
        Empty,     //���I��
        Pivot,     //�I���J�n
        Expansion, //�I��
        Fixed      //�I��̈�m��
    }

    //CharacterDocument�̈ꕔ��I�����邽�߂̋@�\
    internal class TextSelection : ITextSelection {

        //�[�_
        internal class TextPoint : ICloneable {
            private int _line;
            private int _column;

            public int Line {
                get {
                    return _line;
                }
                set {
                    _line = value;
                }
            }
            public int Column {
                get {
                    return _column;
                }
                set {
                    _column = value;
                }
            }

            public TextPoint() {
                Clear();
            }
            public TextPoint(int line, int column) {
                _line = line;
                _column = column;
            }


            public void Clear() {
                Line = -1;
                Column = 0;
            }

            public object Clone() {
                return MemberwiseClone();
            }
        }

        private SelectionState _state;

        private List<ISelectionListener> _listeners;

        private CharacterDocumentViewer _owner;
        //�ŏ��̑I��_�B�P���s��I�������Ƃ��̂��߂ɂQ��(forward/backward)�݂���B
        private TextPoint _forwardPivot;
        private TextPoint _backwardPivot;
        //�I���̍ŏI�_
        private TextPoint _forwardDestination;
        private TextPoint _backwardDestination;

        //pivot�̏��
        private RangeType _pivotType;

        //�I�����J�n�����Ƃ��̃}�E�X���W
        private int _startX;
        private int _startY;

        //������Ɖ����t���O
        //private bool _disabledTemporary;

        public TextSelection(CharacterDocumentViewer owner) {
            _owner = owner;
            _forwardPivot = new TextPoint();
            _backwardPivot = new TextPoint();
            _forwardDestination = new TextPoint();
            _backwardDestination = new TextPoint();
            _listeners = new List<ISelectionListener>();
        }

        public SelectionState State {
            get {
                return _state;
            }
        }
        public RangeType PivotType {
            get {
                return _pivotType;
            }
        }

        //�}�E�X�𓮂����Ȃ��Ă��N���b�N������MouseMove�C�x���g���������Ă��܂��̂ŁA�ʒu�̃`�F�b�N�̂��߂Ƀ}�E�X���W�L�����K�v
        public int StartX {
            get {
                return _startX;
            }
        }
        public int StartY {
            get {
                return _startY;
            }
        }


        public void Clear() {
            //if(_owner!=null)
            //	_owner.ExitTextSelection();
            _state = SelectionState.Empty;
            _forwardPivot.Clear();
            _backwardPivot.Clear();
            _forwardDestination.Clear();
            _backwardDestination.Clear();
            //_disabledTemporary = false;
        }

        /*
        public void DisableTemporary() {
            _disabledTemporary = true;
        }*/

        #region ISelection
        public IPoderosaView OwnerView {
            get {
                return (IPoderosaView)_owner.GetAdapter(typeof(IPoderosaView));
            }
        }
        public IPoderosaCommand TranslateCommand(IGeneralCommand command) {
            return null;
        }
        public IAdaptable GetAdapter(Type adapter) {
            return WindowManagerPlugin.Instance.PoderosaWorld.AdapterManager.GetAdapter(this, adapter);
        }
        #endregion

        //�h�L�������g��Discard���ꂽ�Ƃ��ɌĂ΂��Bfirst_line���O�ɑI��̈悪�d�Ȃ��Ă�����N���A����
        public void ClearIfOverlapped(int first_line) {
            if (_forwardPivot.Line != -1 && _forwardPivot.Line < first_line) {
                _forwardPivot.Line = first_line;
                _forwardPivot.Column = 0;
                _backwardPivot.Line = first_line;
                _backwardPivot.Column = 0;
            }

            if (_forwardDestination.Line != -1 && _forwardDestination.Line < first_line) {
                _forwardDestination.Line = first_line;
                _forwardDestination.Column = 0;
                _backwardDestination.Line = first_line;
                _backwardDestination.Column = 0;
            }
        }

        public bool IsEmpty {
            get {
                return _forwardPivot.Line == -1 || _backwardPivot.Line == -1 ||
                    _forwardDestination.Line == -1 || _backwardDestination.Line == -1;
            }
        }

        public bool StartSelection(GLine line, int position, RangeType type, int x, int y) {
            Debug.Assert(position >= 0);
            //���{�ꕶ���̉E������̑I���͍����ɏC��
            line.ExpandBuffer(position + 1);
            if (line.IsRightSideOfZenkaku(position))
                position--;

            //_disabledTemporary = false;
            _pivotType = type;
            _forwardPivot.Line = line.ID;
            _backwardPivot.Line = line.ID;
            _forwardDestination.Line = line.ID;
            _forwardDestination.Column = position;
            _backwardDestination.Line = line.ID;
            _backwardDestination.Column = position;
            switch (type) {
                case RangeType.Char:
                    _forwardPivot.Column = position;
                    _backwardPivot.Column = position;
                    break;
                case RangeType.Word:
                    _forwardPivot.Column = line.FindPrevWordBreak(position) + 1;
                    _backwardPivot.Column = line.FindNextWordBreak(position);
                    break;
                case RangeType.Line:
                    _forwardPivot.Column = 0;
                    _backwardPivot.Column = line.DisplayLength;
                    break;
            }
            _state = SelectionState.Pivot;
            _startX = x;
            _startY = y;
            FireSelectionStarted();
            return true;
        }

        public bool ExpandTo(GLine line, int position, RangeType type) {
            line.ExpandBuffer(position + 1);
            //_disabledTemporary = false;
            _state = SelectionState.Expansion;

            _forwardDestination.Line = line.ID;
            _backwardDestination.Line = line.ID;
            //Debug.WriteLine(String.Format("ExpandTo Line{0} Position{1}", line.ID, position));
            switch (type) {
                case RangeType.Char:
                    _forwardDestination.Column = position;
                    _backwardDestination.Column = position;
                    break;
                case RangeType.Word:
                    _forwardDestination.Column = line.FindPrevWordBreak(position) + 1;
                    _backwardDestination.Column = line.FindNextWordBreak(position);
                    break;
                case RangeType.Line:
                    _forwardDestination.Column = 0;
                    _backwardDestination.Column = line.DisplayLength;
                    break;
            }

            return true;
        }

        public void SelectAll() {
            //_disabledTemporary = false;
            _forwardPivot.Line = _owner.CharacterDocument.FirstLine.ID;
            _forwardPivot.Column = 0;
            _backwardPivot = (TextPoint)_forwardPivot.Clone();
            _forwardDestination.Line = _owner.CharacterDocument.LastLine.ID;
            _forwardDestination.Column = _owner.CharacterDocument.LastLine.DisplayLength;
            _backwardDestination = (TextPoint)_forwardDestination.Clone();

            _pivotType = RangeType.Char;
            FixSelection();
        }

        //�I�����[�h�ɉ����Ĕ͈͂��߂�B�}�E�X�Ńh���b�O���邱�Ƃ�����̂ŁAcolumn<0�̃P�[�X�����݂���
        public TextPoint ConvertSelectionPosition(GLine line, int column) {
            TextPoint result = new TextPoint(line.ID, column);

            int line_length = line.DisplayLength;
            if (_pivotType == RangeType.Line) {
                //�s�I���̂Ƃ��́A�I���J�n�_�ȑO�̂ł������炻�̍s�̐擪�A�����łȂ��Ȃ炻�̍s�̃��X�g�B
                //�����������(Pivot-Destination)���s���E�s�������Ɋg�債�����̂ɂȂ�悤��
                if (result.Line <= _forwardPivot.Line)
                    result.Column = 0;
                else
                    result.Column = line.DisplayLength;
            }
            else { //Word,Char�I��
                if (result.Line < _forwardPivot.Line) { //�J�n�_���O�̂Ƃ���
                    if (result.Column < 0)
                        result.Column = 0; //�s���܂ŁB
                    else if (result.Column >= line_length) { //�s�̉E�[�̉E�܂őI�����Ă���Ƃ��́A���s�̐擪�܂�
                        result.Line++;
                        result.Column = 0;
                    }
                }
                else if (result.Line == _forwardPivot.Line) { //����s���I��.���̍s�ɂ����܂�悤��
                    result.Column = RuntimeUtil.AdjustIntRange(result.Column, 0, line_length);
                }
                else { //�J�n�_�̌���ւ̑I��
                    if (result.Column < 0) {
                        result.Line--;
                        result.Column = line.PrevLine == null ? 0 : line.PrevLine.DisplayLength;
                    }
                    else if (result.Column >= line_length)
                        result.Column = line_length;
                }
            }

            return result;
        }

        public void FixSelection() {
            _state = SelectionState.Fixed;
            FireSelectionFixed();
        }

        public string GetSelectedText(TextFormatOption opt) {
            //if(_owner==null || _disabledTemporary) return null;

            StringBuilder bld = new StringBuilder();
            TextPoint a = HeadPoint;
            TextPoint b = TailPoint;

            GLine l = _owner.CharacterDocument.FindLineOrEdge(a.Line);
            int pos = a.Column;
            if (pos < 0)
                return "";

            do {
                bool eol_required = (opt == TextFormatOption.AsLook || l.EOLType != EOLType.Continue);
                if (l.ID == b.Line) { //�ŏI�s
                    //������NULL����������P�[�X������悤��
                    AppendTrim(bld, l, pos, b.Column - pos);
                    if (_pivotType == RangeType.Line && eol_required)
                        bld.Append("\r\n");
                    break;
                }
                else { //�ŏI�ȊO�̍s
                    if (l.Length - pos > 0) { //l.CharLength==pos�ƂȂ�P�[�X���������B�^�̗��R�͔[�����Ă��Ȃ���
                        AppendTrim(bld, l, pos, l.Length - pos);
                    }
                    if (eol_required && bld.Length > 0) //bld.Length>0�͍s�P�ʑI���ŗ]�v�ȉ��s������̂�����邽�߂̏��u
                        bld.Append("\r\n"); //LF�݂̂��N���b�v�{�[�h�Ɏ����Ă����Ă����̃A�v���̍��������邾���Ȃ̂ł�߂Ă���
                    l = l.NextLine;
                    if (l == null)
                        break; //!!�{������͂Ȃ��͂������N���b�V�����|�[�g�̂��߉��
                    pos = 0;
                }
            } while (true);

            //Debug.WriteLine("Selected Text Len="+bld.Length);

            return bld.ToString();
        }
        private void AppendTrim(StringBuilder bld, GLine line, int pos, int length) {
            Debug.Assert(pos >= 0);
            if (line.IsRightSideOfZenkaku(pos)) { //���{�ꕶ���̉E�[����̂Ƃ��͊g�傷��
                pos--;
                length++;
            }

            line.WriteTo(
                delegate(char[] buff, int len) {
                    bld.Append(buff, 0, len);
                },
                pos, length);
        }

        internal TextPoint HeadPoint {
            get {
                return Min(_forwardPivot, _forwardDestination);
            }
        }
        internal TextPoint TailPoint {
            get {
                return Max(_backwardPivot, _backwardDestination);
            }
        }
        private static TextPoint Min(TextPoint p1, TextPoint p2) {
            int id1 = p1.Line;
            int id2 = p2.Line;
            if (id1 == id2) {
                int pos1 = p1.Column;
                int pos2 = p2.Column;
                if (pos1 == pos2)
                    return p1;
                else
                    return pos1 < pos2 ? p1 : p2;
            }
            else
                return id1 < id2 ? p1 : p2;

        }
        private static TextPoint Max(TextPoint p1, TextPoint p2) {
            int id1 = p1.Line;
            int id2 = p2.Line;
            if (id1 == id2) {
                int pos1 = p1.Column;
                int pos2 = p2.Column;
                if (pos1 == pos2)
                    return p1;
                else
                    return pos1 > pos2 ? p1 : p2;
            }
            else
                return id1 > id2 ? p1 : p2;

        }

        //Listener�n
        public void AddSelectionListener(ISelectionListener listener) {
            _listeners.Add(listener);
        }
        public void RemoveSelectionListener(ISelectionListener listener) {
            _listeners.Remove(listener);
        }

        void FireSelectionStarted() {
            foreach (ISelectionListener listener in _listeners)
                listener.OnSelectionStarted();
        }
        void FireSelectionFixed() {
            foreach (ISelectionListener listener in _listeners)
                listener.OnSelectionFixed();
        }


    }
}
