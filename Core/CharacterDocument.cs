/*
 * Copyright 2004,2006 The Poderosa Project.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *
 * $Id: CharacterDocument.cs,v 1.10 2012/04/22 16:42:19 kzmi Exp $
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.IO;

using Poderosa.Sessions;
using Poderosa.Commands;

namespace Poderosa.Document {
    //�����x�[�X�̃h�L�������g�B��ʕ\���̂݁B
    /// <summary>
    /// <ja>
    /// �����x�[�X�̃h�L�������g��񋟂��܂��B
    /// </ja>
    /// <en>
    /// The document of the character base is offered.
    /// </en>
    /// </summary>
    /// <remarks>
    /// <ja>
    /// ���̃N���X�ɂ��Ẳ���́A�܂�����܂���B
    /// </ja>
    /// <en>
    /// This class has not explained yet. 
    /// </en>
    /// </remarks>
    public class CharacterDocument : IPoderosaDocument, IPoderosaContextMenuPoint {
        protected string _caption;
        protected Image _icon;
        protected ISession _owner;

        protected InvalidatedRegion _invalidatedRegion;
        protected GLine _firstLine;
        protected GLine _lastLine;
        protected int _size; //�T�C�Y��_firstLine/lastLine����v�Z�\�����悭�g���̂ŃL���b�V��

        protected Color _appModeBgColor = Color.Empty;
        protected bool _bApplicationMode;

        public InvalidatedRegion InvalidatedRegion {
            get {
                return _invalidatedRegion;
            }
        }

        public int FirstLineNumber {
            get {
                return _firstLine.ID;
            }
        }
        public int LastLineNumber {
            get {
                return _lastLine.ID;
            }
        }
        public GLine FirstLine {
            get {
                return _firstLine;
            }
        }
        public GLine LastLine {
            get {
                return _lastLine;
            }
        }
        public int Size {
            get {
                return _size;
            }
        }
        public Color ApplicationModeBackColor {
            get {
                return _appModeBgColor;
            }
            set {
                _appModeBgColor = value;
            }
        }
        public bool IsApplicationMode {
            get {
                return _bApplicationMode;
            }
            set {
                _bApplicationMode = value;
            }
        }

        public CharacterDocument() {
            _invalidatedRegion = new InvalidatedRegion();
        }

        public GLine FindLineOrNull(int index) {
            if (index < _firstLine.ID || index > _lastLine.ID)
                return null;
            else
                return FindLine(index);
        }
        public GLine FindLineOrEdge(int index) {
            if (index < _firstLine.ID)
                index = _firstLine.ID;
            else if (index > _lastLine.ID)
                index = _lastLine.ID;

            return FindLine(index);
        }
        protected GLine FindLineOrNullClipTop(int index) {
            if (index < _firstLine.ID)
                index = _firstLine.ID;
            else if (index > _lastLine.ID)
                return null;

            return FindLine(index);
        }

        public void SetFirstLine(int id) {
            _firstLine = FindLineOrEdge(id);
        }

        //�����C���f�N�X���猩����@CurrentLine���炻�������Ȃ��ʒu���낤�Ƃ����������
        public virtual GLine FindLine(int index) {
            //current��top�̋߂������珇�ɂ݂Ă���
            int d1 = Math.Abs(index - _lastLine.ID);
            int d2 = Math.Abs(index - _firstLine.ID);
            if (d1 < d2)
                return FindLineByHint(index, _lastLine);
            else
                return FindLineByHint(index, _firstLine);
        }

        //TODO �h�L�������g���ł����Ȃ�Ƃ���ł͒x���Ȃ肻��
        protected GLine FindLineByHint(int index, GLine hintLine) {
            int h = hintLine.ID;
            GLine l = hintLine;
            if (index >= h) {
                for (int i = h; i < index; i++) {
                    l = l.NextLine;
                    if (l == null) {
                        FindLineByHintFailed(index, hintLine);
                        l = hintLine;
                        break;
                    }
                }
            }
            else {
                for (int i = h; i > index; i--) {
                    l = l.PrevLine;
                    if (l == null) {
                        FindLineByHintFailed(index, hintLine);
                        l = hintLine;
                        break;
                    }
                }
            }
            return l;
        }

        //FindLineByHint�͂��΂��Ύ��s����̂Ńf�o�b�O�p�Ɍ��ݏ�Ԃ��_���v
        protected void FindLineByHintFailed(int index, GLine hintLine) {
#if DEBUG
            Debug.WriteLine(String.Format("FindLine {0}, hint_id={1}", index, hintLine.ID));
            Debugger.Break();
#endif
        }

        public virtual IPoderosaMenuGroup[] ContextMenu {
            get {
                return null;
            }
        }

        public void SetOwner(ISession owner) {
            _owner = owner;
        }

        //�����ɒǉ�
        public virtual void AddLine(GLine line) {
            if (_firstLine == null) { //�󂾂���
                _firstLine = line;
                _lastLine = line;
                _size = 1;
                line.ID = 0;
                _invalidatedRegion.InvalidateLine(0);
            }
            else { //�ʏ�̒ǉ�
                Debug.Assert(_lastLine.NextLine == null);
                int lastID = _lastLine.ID;
                _lastLine.NextLine = line;
                line.PrevLine = _lastLine;
                _lastLine = line;
                line.ID = lastID + 1;
                _size++;
                _invalidatedRegion.InvalidateLine(lastID + 1);
            }
        }

        public void InvalidateAll() {
            _invalidatedRegion.InvalidatedAll = true;
        }

        //TODO �ȉ��Q���\�b�h��ID�����e�i���X��ʓr�s�����Ƃ�O��ɂ��Ă���B

        protected void InsertBefore(GLine pos, GLine line) {
            if (pos.PrevLine != null)
                pos.PrevLine.NextLine = line;

            line.PrevLine = pos.PrevLine;
            line.NextLine = pos;

            pos.PrevLine = line;

            if (pos == _firstLine)
                _firstLine = line;
            _size++;
            _invalidatedRegion.InvalidatedAll = true;
        }
        protected void InsertAfter(GLine pos, GLine line) {
            if (pos.NextLine != null)
                pos.NextLine.PrevLine = line;

            line.NextLine = pos.NextLine;
            line.PrevLine = pos;

            pos.NextLine = line;

            if (pos == _lastLine)
                _lastLine = line;
            _size++;
            _invalidatedRegion.InvalidatedAll = true;
        }

#if false
        //BACK-BURNER
        public void Dump(string title) {
            Debug.WriteLine("<<<< DEBUG DUMP [" + title + "] >>>>");
            Debug.WriteLine(String.Format("[size={0} top={1} current={2} caret={3} first={4} last={5} region={6},{7}]", _size, TopLineNumber, CurrentLineNumber, _caretColumn, FirstLineNumber, LastLineNumber, _scrollingTop, _scrollingBottom));
            GLine gl = FindLineOrEdge(TopLineNumber);
            int count = 0;
            while (gl != null && count++ < _connection.TerminalHeight) {
                Debug.Write(String.Format("{0,3}", gl.ID));
                Debug.Write(":");
                Debug.Write(GLineManipulator.SafeString(gl.Text));
                Debug.Write(":");
                Debug.WriteLine(gl.EOLType);
                gl = gl.NextLine;
            }
        }
#endif

#if UNITTEST
        public virtual void AssertValid(GLine line, int count) {
            GLine next = line.NextLine;
            while (next != null) {
                Debug.Assert(line.ID + 1 == next.ID);
                Debug.Assert(line == next.PrevLine);
                line = next;
                next = next.NextLine;
            }
        }
#endif

        //�e�L�X�g�t�@�C������ǂݏo���B�����̓���������
        public void LoadForTest(string filename) {
            StreamReader r = null;
            try {
                r = new StreamReader(filename, Encoding.Default);
                TextDecoration dec = TextDecoration.Default;
                string line = r.ReadLine();
                while (line != null) {
                    this.AddLine(GLine.CreateSimpleGLine(line, dec));
                    line = r.ReadLine();
                }
            }
            finally {
                if (r != null)
                    r.Close();
            }
        }
        //�P��s����̍쐬
        public static CharacterDocument SingleLine(string content) {
            CharacterDocument doc = new CharacterDocument();
            doc.AddLine(GLine.CreateSimpleGLine(content, TextDecoration.Default));
            return doc;
        }

        #region IAdaptable
        public virtual IAdaptable GetAdapter(Type adapter) {
            return SessionManagerPlugin.Instance.PoderosaWorld.AdapterManager.GetAdapter(this, adapter);
        }
        #endregion
        #region IPoderosaDocument
        public virtual ISession OwnerSession {
            get {
                return _owner;
            }
        }
        public virtual Image Icon {
            get {
                return _icon;
            }
        }
        public virtual string Caption {
            get {
                return _caption;
            }
        }
        #endregion
    }

    //�`��̕K�v�̂���ID�͈̔�
    /// <exclude/>
    public class InvalidatedRegion {
        private const int NOT_SET = -1;

        private int _lineIDStart;
        private int _lineIDEnd;
        private bool _invalidatedAll;

        private bool _empty;

        public InvalidatedRegion() {
            Reset();
        }

        public int LineIDStart {
            get {
                return _lineIDStart;
            }
        }
        public int LineIDEnd {
            get {
                return _lineIDEnd;
            }
        }
        public bool IsEmpty {
            get {
                return _empty;
            }
        }
        public bool InvalidatedAll {
            get {
                return _invalidatedAll;
            }
            set {
                lock (this) {
                    _empty = false;
                    _invalidatedAll = value; //���ꂪ�����Ă���Ƃ��͖������őS�`��@������Invalidate�͈͂̌v�Z���ʓ|�ȂƂ��͂���
                }
            }
        }

        public void InvalidateLine(int id) {
            lock (this) {
                _empty = false;
                if (_lineIDStart == NOT_SET || _lineIDStart > id)
                    _lineIDStart = id;
                if (_lineIDEnd == NOT_SET || _lineIDEnd < id)
                    _lineIDEnd = id;
            }
        }
        public void Reset() {
            lock (this) {
                _lineIDStart = NOT_SET;
                _lineIDEnd = NOT_SET;
                _invalidatedAll = false;
                _empty = true;
            }
        }

        public InvalidatedRegion GetCopyAndReset() {
            lock (this) {
                InvalidatedRegion copy = (InvalidatedRegion)MemberwiseClone();
                Reset();
                return copy;
            }
        }
    }
}
