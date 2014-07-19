/*
 * Copyright 2004,2006 The Poderosa Project.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *
 * $Id: GLine.cs,v 1.23 2012/05/27 15:02:26 kzmi Exp $
 */
/*
* Copyright (c) 2005 Poderosa Project, All Rights Reserved.
* $Id: GLine.cs,v 1.23 2012/05/27 15:02:26 kzmi Exp $
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System.Text;

#if UNITTEST
using NUnit.Framework;
#endif

using Poderosa.Util.Drawing;
using Poderosa.Forms;
using Poderosa.View;

namespace Poderosa.Document {
    // GLine�̍\���v�f�B�P��GWord�͓����`�悪�Ȃ���A�V���O�������N���X�g�ɂȂ��Ă���B
    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public sealed class GWord {
        private readonly TextDecoration _decoration; //�`����
        private readonly int _offset;                //�R���e�i��GLine�̉������ڂ���n�܂��Ă��邩
        private readonly CharGroup _charGroup;       //�����O���[�v
        private GWord _next;                //����GWord

        /// �\���p�̑���
        internal TextDecoration Decoration {
            get {
                return _decoration;
            }
        }
        /// ��������GLine�̒��ŉ������ڂ���n�܂��Ă��邩
        public int Offset {
            get {
                return _offset;
            }
        }

        ///����Word
        public GWord Next {
            get {
                return _next;
            }
            set {
                _next = value;
            }
        }

        public CharGroup CharGroup {
            get {
                return _charGroup;
            }
        }

        /// ������A�f�R���[�V�����A�I�t�Z�b�g���w�肷��R���X�g���N�^�B
        public GWord(TextDecoration d, int o, CharGroup chargroup) {
            Debug.Assert(d != null);
            _offset = o;
            _decoration = d;
            _next = null;
            _charGroup = chargroup;
        }

        //Next�̒l�ȊO���R�s�[����
        internal GWord StandAloneClone() {
            return new GWord(_decoration, _offset, _charGroup);
        }

        internal GWord DeepClone() {
            GWord w = StandAloneClone();
            if (_next != null)
                w._next = _next.DeepClone();
            return w;
        }

    }


    /// �P�s�̃f�[�^
    /// GWord�ւ̕����͒x���]�������B���ׂ̍s�̓_�u�������N���X�g
    /// <exclude/>
    public sealed class GLine {

        /// <summary>
        /// Delegate for copying characters in GLine.
        /// </summary>
        /// <remarks>
        /// WIDECHAR_PAD is not contained in buff.
        /// </remarks>
        /// <param name="buff">An array of char which contains characters to copy.</param>
        /// <param name="length">Number of characters to copy from buff.</param>
        public delegate void BufferWriter(char[] buff, int length);

        internal const char WIDECHAR_PAD = '\uFFFF';

        private char[] _text; //�{�́F\0�͍s��������
        private int _displayLength;
        private EOLType _eolType;
        private int _id;
        private GWord _firstWord;
        private GLine _nextLine;
        private GLine _prevLine;

        [ThreadStatic]
        private static char[] _copyTempBuff;

        // Returns thread-local temporary buffer
        // for copying characters in _text.
        private char[] GetInternalTemporaryBufferForCopy() {
            char[] buff = _copyTempBuff;
            int minLen = _text.Length;
            if (buff == null || buff.Length < minLen) {
                buff = _copyTempBuff = new char[minLen];
            }
            return buff;
        }


        public int Length {
            get {
                return _text.Length;
            }
        }

        public int DisplayLength {
            get {
                return _displayLength;
            }
        }

        public GWord FirstWord {
            get {
                return _firstWord;
            }
        }

        //ID, �אڍs�̐ݒ�@���̕ύX�͐T�d���K�v�I
        public int ID {
            get {
                return _id;
            }
            set {
                _id = value;
            }
        }

        public GLine NextLine {
            get {
                return _nextLine;
            }
            set {
                _nextLine = value;
            }
        }

        public GLine PrevLine {
            get {
                return _prevLine;
            }
            set {
                _prevLine = value;
            }
        }

        public EOLType EOLType {
            get {
                return _eolType;
            }
            set {
                _eolType = value;
            }
        }

        public GLine(int length) {
            Debug.Assert(length > 0);
            _text = new char[length];
            _displayLength = 0;
            _firstWord = new GWord(TextDecoration.Default, 0, CharGroup.LatinHankaku);
            _id = -1;
        }

        //GLineManipulator�Ȃǂ̂��߂̃R���X�g���N�^
        internal GLine(char[] data, int dataLength, GWord firstWord) {
            _text = data;
            _displayLength = dataLength;
            _firstWord = firstWord;
            _id = -1;
        }

        private static int GetDisplayLength(char[] data) {
            int limit = data.Length;
            for (int len = 0; len < limit; len++) {
                if (data[len] == '\0')
                    return len;
            }
            return limit;
        }

        internal char[] DuplicateBuffer(char[] reusableBuffer) {
            if (reusableBuffer != null && reusableBuffer.Length == _text.Length) {
                Buffer.BlockCopy(_text, 0, reusableBuffer, 0, _text.Length * sizeof(char));
                return reusableBuffer;
            }
            else {
                return (char[])_text.Clone();
            }
        }

        public GLine Clone() {
            GLine nl = new GLine((char[])_text.Clone(), _displayLength, _firstWord.DeepClone());
            nl._eolType = _eolType;
            nl._id = _id;
            return nl;
        }

        public void Clear() {
            Clear(null);
        }

        public void Clear(TextDecoration dec) {
            TextDecoration fillDec = (dec != null) ? dec.RetainBackColor() : TextDecoration.Default;
            char fill = fillDec.IsDefault ? '\0' : ' '; // �F�w��t���̂��Ƃ�����̂ŃX�y�[�X
            for (int i = 0; i < _text.Length; i++)
                _text[i] = fill;
            _displayLength = fillDec.IsDefault ? 0 : _text.Length;
            _firstWord = new GWord(fillDec, 0, CharGroup.LatinHankaku);
        }

        //�O��̒P���؂��������B�Ԃ��ʒu�́Apos��GetWordBreakGroup�̒l����v���钆�ŉ����n�_
        public int FindPrevWordBreak(int pos) {
            int v = ToCharGroupForWordBreak(_text[pos]);
            while (pos >= 0) {
                if (v != ToCharGroupForWordBreak(_text[pos]))
                    return pos;
                pos--;
            }
            return -1;
        }

        public int FindNextWordBreak(int pos) {
            int v = ToCharGroupForWordBreak(_text[pos]);
            while (pos < _text.Length) {
                if (v != ToCharGroupForWordBreak(_text[pos]))
                    return pos;
                pos++;
            }
            return _text.Length;
        }

        private static int ToCharGroupForWordBreak(char ch) {
            if (ch < 0x80)
                return ASCIIWordBreakTable.Default.GetAt(ch);
            else if (ch == '\u3000') //�S�p�X�y�[�X
                return ASCIIWordBreakTable.SPACE;
            else //����ɂ�����UnicodeCategory�����݂ēK���ɂ����炦�邱�Ƃ��ł��邪
                return ASCIIWordBreakTable.NOT_ASCII;
        }

        public void ExpandBuffer(int length) {
            if (length <= _text.Length)
                return;

            char[] current = _text;
            char[] newBuff = new char[length];
            Buffer.BlockCopy(current, 0, newBuff, 0, current.Length * sizeof(char));
            _text = newBuff;
            // Note; _displayLength is not changed.
        }

        private void Append(GWord w) {
            if (_firstWord == null)
                _firstWord = w;
            else
                this.LastWord.Next = w;
        }

        private GWord LastWord {
            get {
                GWord w = _firstWord;
                while (w.Next != null)
                    w = w.Next;
                return w;
            }
        }

        internal void Render(IntPtr hdc, RenderProfile prof, Color baseBackColor, int x, int y) {
            if (_text.Length == 0 || _text[0] == '\0')
                return; //�����`���Ȃ��Ă悢�B����͂悭����P�[�X

            float fx0 = (float)x;
            float fx1 = fx0;
            int y1 = y;
            int y2 = y1 + (int)prof.Pitch.Height;

            float pitch = prof.Pitch.Width;
            int defaultBackColorArgb = baseBackColor.ToArgb();

            Win32.SetBkMode(hdc, Win32.TRANSPARENT);

            GWord word = _firstWord;
            while (word != null) {
                TextDecoration dec = word.Decoration;

                IntPtr hFont = prof.CalcHFONT_NoUnderline(dec, word.CharGroup);
                Win32.SelectObject(hdc, hFont);

                uint foreColorRef = DrawUtil.ToCOLORREF(prof.CalcTextColor(dec));
                Win32.SetTextColor(hdc, foreColorRef);

                Color bkColor = prof.CalcBackColor(dec);
                bool isOpaque = (bkColor.ToArgb() != defaultBackColorArgb);
                if (isOpaque) {
                    uint bkColorRef = DrawUtil.ToCOLORREF(bkColor);
                    Win32.SetBkColor(hdc, bkColorRef);
                }

                int nextOffset = GetNextOffset(word);

                float fx2 = fx0 + pitch * nextOffset;

                if (prof.CalcBold(dec) || CharGroupUtil.IsCJK(word.CharGroup)) {
                    // It is not always true that width of a character in the CJK font is twice of a character in the ASCII font.
                    // Characters are drawn one by one to adjust pitch.

                    int step = CharGroupUtil.GetColumnsPerCharacter(word.CharGroup);
                    float charPitch = pitch * step;
                    int offset = word.Offset;
                    float fx = fx1;
                    if (isOpaque) {
                        // If background fill is required, we call ExtTextOut() with ETO_OPAQUE to draw the first character.
                        if (offset < nextOffset) {
                            Win32.RECT rect = new Win32.RECT((int)fx1, y1, (int)fx2, y2);
                            char ch = _text[offset];
                            Debug.Assert(ch != GLine.WIDECHAR_PAD);
                            unsafe {
                                Win32.ExtTextOut(hdc, rect.left, rect.top, Win32.ETO_OPAQUE, &rect, &ch, 1, null);
                            }
                        }
                        offset += step;
                        fx += charPitch;
                    }

                    for (; offset < nextOffset; offset += step) {
                        char ch = _text[offset];
                        Debug.Assert(ch != GLine.WIDECHAR_PAD);
                        unsafe {
                            Win32.ExtTextOut(hdc, (int)fx, y1, 0, null, &ch, 1, null);
                        }
                        fx += charPitch;
                    }
                }
                else {
                    int offset = word.Offset;
                    int displayLength = nextOffset - offset;
                    if (isOpaque) {
                        Win32.RECT rect = new Win32.RECT((int)fx1, y1, (int)fx2, y2);
                        unsafe {
                            fixed (char* p = &_text[offset]) {
                                Win32.ExtTextOut(hdc, rect.left, rect.top, Win32.ETO_OPAQUE, &rect, p, displayLength, null);
                            }
                        }
                    }
                    else {
                        unsafe {
                            fixed (char* p = &_text[offset]) {
                                Win32.ExtTextOut(hdc, (int)fx1, y1, 0, null, p, displayLength, null);
                            }
                        }
                    }
                }

                if (dec.Underline)
                    DrawUnderline(hdc, foreColorRef, (int)fx1, y2 - 1, (int)fx2 - (int)fx1);

                fx1 = fx2;
                word = word.Next;
            }

        }

        private void DrawUnderline(IntPtr hdc, uint col, int x, int y, int length) {
            //Underline�������Ƃ͂��܂�Ȃ����낤���疈��Pen�����B���ɂȂ肻���������炻�̂Ƃ��ɍl���悤
            IntPtr pen = Win32.CreatePen(0, 1, col);
            IntPtr prev = Win32.SelectObject(hdc, pen);
            Win32.MoveToEx(hdc, x, y, IntPtr.Zero);
            Win32.LineTo(hdc, x + length, y);
            Win32.SelectObject(hdc, prev);
            Win32.DeleteObject(pen);
        }

        internal int GetNextOffset(GWord word) {
            if (word.Next == null)
                return _displayLength;
            else
                return word.Next.Offset;
        }


        /// <summary>
        /// Invert text attribute at the specified position.
        /// </summary>
        /// <remarks>
        /// <para>If doInvert was false, only splitting of GWords will be performed.
        /// It is required to avoid problem when the text which conatins blinking cursor is drawn by DrawWord().</para>
        /// <para>DrawWord() draws contiguous characters at once,
        /// and the character pitch depends how the character in the font was designed.</para>
        /// <para>By split GWord even if inversion is not required,
        /// the position of a character of the blinking cursor will be constant.</para>
        /// </remarks>
        /// <param name="index">Column index to invert.</param>
        /// <param name="doInvert">Whether inversion is really applied.</param>
        /// <param name="color">Background color of the inverted character.</param>
        internal void InvertCharacter(int index, bool doInvert, Color color) {
            //��Ƀf�[�^�̂���Ƃ������̈ʒu���w�肳�ꂽ��o�b�t�@���L���Ă���
            if (index >= _displayLength) {
                int prevLength = _displayLength;
                ExpandBuffer(index + 1);
                for (int i = prevLength; i < index + 1; i++)
                    _text[i] = ' ';
                _displayLength = index + 1;
                this.LastWord.Next = new GWord(TextDecoration.Default, prevLength, CharGroup.LatinHankaku);
            }
            if (_text[index] == WIDECHAR_PAD)
                index--;

            GWord prev = null;
            GWord word = _firstWord;
            int nextoffset = 0;
            while (word != null) {
                nextoffset = GetNextOffset(word);
                if (word.Offset <= index && index < nextoffset) {
                    GWord next = word.Next;

                    //�L�����b�g�̔��]
                    TextDecoration inv_dec = word.Decoration;
                    if (doInvert)
                        inv_dec = inv_dec.GetInvertedCopyForCaret(color);

                    //GWord�͍ő�R��(head:index�̑O�Amiddle:index�Atail:index�̎�)�ɕ��������
                    GWord head = word.Offset < index ? new GWord(word.Decoration, word.Offset, word.CharGroup) : null;
                    GWord mid = new GWord(inv_dec, index, word.CharGroup);
                    int nextIndex = index + CharGroupUtil.GetColumnsPerCharacter(word.CharGroup);
                    GWord tail = nextIndex < nextoffset ? new GWord(word.Decoration, nextIndex, word.CharGroup) : null;

                    //�A�� head,tail��null�̂��Ƃ�����̂ł�₱����
                    List<GWord> list = new List<GWord>(3);
                    if (head != null) {
                        list.Add(head);
                        head.Next = mid;
                    }

                    list.Add(mid);
                    mid.Next = tail == null ? next : tail;

                    if (tail != null)
                        list.Add(tail);

                    //�O��Ƃ̘A��
                    if (prev == null)
                        _firstWord = list[0];
                    else
                        prev.Next = list[0];

                    list[list.Count - 1].Next = next;

                    break;
                }

                prev = word;
                word = word.Next;
            }
        }

        /// <summary>
        /// Clone this instance that text attributes in the specified range are inverted.
        /// </summary>
        /// <param name="from">start column index of the range. (inclusive)</param>
        /// <param name="to">end column index of the range. (exclusive)</param>
        /// <returns>new instance</returns>
        internal GLine CreateInvertedClone(int from, int to) {
            ExpandBuffer(Math.Max(from + 1, to)); //���������T�C�Y�����Ƃ��Ȃǂɂ��̏������������Ȃ����Ƃ�����
            Debug.Assert(from >= 0 && from < _text.Length);
            if (from < _text.Length && _text[from] == WIDECHAR_PAD)
                from--;
            if (to < _text.Length && _text[to] == WIDECHAR_PAD)
                to++;

            const int PHASE_LEFT = 0;
            const int PHASE_MIDDLE = 1;
            const int PHASE_RIGHT = 2;

            int phase = PHASE_LEFT;
            int inverseIndex = from;

            GWord first = null;
            GWord last = null;

            for (GWord word = _firstWord; word != null; word = word.Next) {
                TextDecoration originalDecoration = word.Decoration;
                if (originalDecoration == null)
                    originalDecoration = TextDecoration.Default;

                int wordStart = word.Offset;
                int wordEnd = GetNextOffset(word);

                do {
                    GWord newWord;

                    if (phase == PHASE_RIGHT || inverseIndex < wordStart || wordEnd <= inverseIndex) {
                        TextDecoration newDec = (phase == PHASE_MIDDLE) ? originalDecoration.GetInvertedCopy() : originalDecoration;
                        newWord = new GWord(newDec, wordStart, word.CharGroup);
                        wordStart = wordEnd;
                    }
                    else {
                        TextDecoration leftDec = (phase == PHASE_LEFT) ? originalDecoration : originalDecoration.GetInvertedCopy();

                        if (wordStart < inverseIndex)
                            newWord = new GWord(leftDec, wordStart, word.CharGroup);
                        else
                            newWord = null;

                        wordStart = inverseIndex;

                        // update phase
                        if (phase == PHASE_LEFT) {
                            phase = PHASE_MIDDLE;
                            inverseIndex = to;
                        }
                        else if (phase == PHASE_MIDDLE) {
                            phase = PHASE_RIGHT;
                        }
                    }

                    // append new GWord to the list.
                    if (newWord != null) {
                        if (last == null)
                            first = newWord;
                        else
                            last.Next = newWord;

                        last = newWord;
                    }
                } while (wordStart < wordEnd);
            }

            GLine ret = new GLine((char[])_text.Clone(), _displayLength, first);
            ret.ID = _id;
            ret.EOLType = _eolType;

            return ret;
        }

        public bool IsRightSideOfZenkaku(int index) {
            return _text[index] == WIDECHAR_PAD;
        }

        public void WriteTo(BufferWriter writer, int index) {
            WriteToInternal(writer, index, _text.Length);
        }

        public void WriteTo(BufferWriter writer, int index, int length) {
            int limit = index + length;
            if (limit > _text.Length)
                limit = _text.Length;
            WriteToInternal(writer, index, limit);
        }

        private void WriteToInternal(BufferWriter writer, int index, int limit) {
            char[] temp = GetInternalTemporaryBufferForCopy();
            // Note: must be temp.Length >= limit here
            int tempIndex = 0;
            for (int i = index; i < limit; i++) {
                char ch = _text[i];
                if (ch == '\0')
                    break;
                if (ch != WIDECHAR_PAD)
                    temp[tempIndex++] = ch;
            }
            if (writer != null)
                writer(temp, tempIndex);
        }

        public string ToNormalString() {
            string s = null;
            WriteToInternal(
                delegate(char[] buff, int length) {
                    s = new string(buff, 0, length);
                },
                0, _text.Length);
            return s;
        }

        public static GLine CreateSimpleGLine(string text, TextDecoration dec) {
            char[] buff = new char[text.Length * 2];
            int offset = 0;
            int start = 0;
            CharGroup prevType = CharGroup.LatinHankaku;
            GWord firstWord = null;
            GWord lastWord = null;
            for (int i = 0; i < text.Length; i++) {
                char originalChar = text[i];
                char privateChar = Unicode.ToPrivate(originalChar);
                CharGroup nextType = Unicode.GetCharGroup(privateChar);
                int size = CharGroupUtil.GetColumnsPerCharacter(nextType);
                if (nextType != prevType) {
                    if (offset > start) {
                        GWord newWord = new GWord(dec, start, prevType);
                        if (lastWord == null) {
                            firstWord = lastWord = newWord;
                        }
                        else {
                            lastWord.Next = newWord;
                            lastWord = newWord;
                        }
                    }
                    prevType = nextType;
                    start = offset;
                }

                buff[offset++] = originalChar;
                if (size == 2)
                    buff[offset++] = WIDECHAR_PAD;
            }

            GWord w = new GWord(dec, start, prevType);
            if (lastWord == null) {
                firstWord = w;
            }
            else {
                lastWord.Next = w;
            }

            return new GLine(buff, offset, firstWord);
        }
    }

    /// <summary>
    /// <ja>
    /// <seealso cref="GLine">GLine</seealso>�ɑ΂��āA�����̒ǉ��^�폜�Ȃǂ𑀍삵�܂��B
    /// </ja>
    /// <en>
    /// Addition/deletion of the character etc. are operated for <seealso cref="GLine">GLine</seealso>. 
    /// </en>
    /// </summary>
    /// <remarks>
    /// <ja>
    /// ���̃N���X�́A���Ƃ��΃^�[�~�i�����h�L�������g�̓����Gline��u��������ꍇ�ȂǂɎg���܂��B
    /// </ja>
    /// <en>
    /// When the terminal replaces specific Gline of the document for instance, this class uses it. 
    /// </en>
    /// </remarks>
    /// <exclude/>
    public class GLineManipulator {

        private struct CharAttr {
            public TextDecoration Decoration;
            public CharGroup CharGroup;

            public CharAttr(TextDecoration dec, CharGroup cg) {
                Decoration = dec;
                CharGroup = cg;
            }
        }

        private char[] _text;
        private CharAttr[] _attrs;
        private int _caretColumn;
        private TextDecoration _defaultDecoration;
        private EOLType _eolType;

        /// <summary>
        /// <ja>
        /// �o�b�t�@�T�C�Y�ł��B
        /// </ja>
        /// <en>
        /// Buffer size.
        /// </en>
        /// </summary>
        public int BufferSize {
            get {
                return _text.Length;
            }
        }

        /// <summary>
        /// <ja>
        /// �L�����b�g�ʒu���擾�^�ݒ肵�܂��B
        /// </ja>
        /// <en>
        /// Get / set the position of the caret.
        /// </en>
        /// </summary>
        public int CaretColumn {
            get {
                return _caretColumn;
            }
            set {
                Debug.Assert(value >= 0);
                ExpandBuffer(value);
                Debug.Assert(value <= _text.Length);
                _caretColumn = value;
                value--;
                while (value >= 0 && _text[value] == '\0')
                    _text[value--] = ' ';
            }
        }

        /// <summary>
        /// <ja>
        /// �L�����b�W���^�[����}�����܂��B
        /// </ja>
        /// <en>
        /// Insert the carriage return.
        /// </en>
        /// </summary>
        public void CarriageReturn() {
            _caretColumn = 0;
            _eolType = EOLType.CR;
        }

        /// <summary>
        /// <ja>
        /// ���e���󂩂ǂ����������܂��Btrue�ł���΋�Afalse�Ȃ牽�炩�̕����������Ă��܂��B
        /// </ja>
        /// <en>
        /// It is shown whether the content is empty. Return false if here are some characters in it. True retuens if it is empty.
        /// </en>
        /// </summary>
        public bool IsEmpty {
            get {
                //_text��S������K�v�͂Ȃ����낤
                return _caretColumn == 0 && _text[0] == '\0';
            }
        }
        /// <summary>
        /// <ja>
        /// �e�L�X�g�̕`������擾�^�ݒ肵�܂��B
        /// </ja>
        /// <en>
        /// Drawing information in the text is get/set. 
        /// </en>
        /// </summary>
        public TextDecoration DefaultDecoration {
            get {
                return _defaultDecoration;
            }
            set {
                _defaultDecoration = value;
            }
        }

        // �S���e��j������
        /// <summary>
        /// <ja>
        /// �ێ����Ă���e�L�X�g���N���A���܂��B
        /// </ja>
        /// <en>
        /// Clear the held text.
        /// </en>
        /// </summary>
        /// <param name="length"><ja>�N���A���钷��</ja><en>Length to clear</en></param>
        public void Clear(int length) {
            if (_text == null || length != _text.Length) {
                _text = new char[length];
                _attrs = new CharAttr[length];
            }
            else {
                for (int i = 0; i < _attrs.Length; i++) {
                    _attrs[i] = new CharAttr(null, CharGroup.LatinHankaku);
                }
                for (int i = 0; i < _text.Length; i++) {
                    _text[i] = '\0';
                }
            }
            _caretColumn = 0;
            _eolType = EOLType.Continue;
        }

        /// �����Ɠ������e�ŏ���������Bline�̓��e�͔j�󂳂�Ȃ��B
        /// ������null�̂Ƃ��͈����Ȃ��̃R���X�g���N�^�Ɠ������ʂɂȂ�B
        /// <summary>
        /// <ja>
        /// �����Ɠ������e�ŏ��������܂��B
        /// </ja>
        /// <en>
        /// Initialize same as argument.
        /// </en>
        /// </summary>
        /// <param name="cc">
        /// <ja>
        /// �ݒ肷��L�����b�g�ʒu
        /// </ja>
        /// <en>
        /// The caret position to set.
        /// </en>
        /// </param>
        /// <param name="line">
        /// <ja>�R�s�[���ƂȂ�GLine�I�u�W�F�N�g</ja>
        /// <en>GLine object that becomes copy origin</en>
        /// </param>
        /// <remarks>
        /// <ja>
        /// <paramref name="line"/>��null�̂Ƃ��ɂ́A�����Ȃ��̃R���X�g���N�^�Ɠ������ʂɂȂ�܂��B
        /// </ja>
        /// <en>
        /// The same results with the constructor who doesn't have the argument when <paramref name="line"/> is null. 
        /// </en>
        /// </remarks>
        public void Load(GLine line, int cc) {
            if (line == null) { //���ꂪnull�ɂȂ��Ă���Ƃ����v���Ȃ��N���b�V�����|�[�g���������B�{���͂Ȃ��͂��Ȃ񂾂�...
                Clear(80);
                return;
            }

            Clear(line.Length);
            GWord w = line.FirstWord;
            _text = line.DuplicateBuffer(_text);

            int n = 0;
            while (w != null) {
                int nextoffset = line.GetNextOffset(w);
                while (n < nextoffset) {
                    _attrs[n++] = new CharAttr(w.Decoration, w.CharGroup);
                }
                w = w.Next;
            }

            _eolType = line.EOLType;
            ExpandBuffer(cc + 1);
            this.CaretColumn = cc; //' '�Ŗ��߂邱�Ƃ�����̂Ńv���p�e�B�Z�b�g���g��
        }
#if UNITTEST
        public void Load(char[] text, int cc) {
            _text = text;
            _decorations = new TextDecoration[text.Length];
            _eolType = EOLType.Continue;
            _caretColumn = cc;
        }
        public char[] InternalBuffer {
            get {
                return _text;
            }
        }
#endif

        /// <summary>
        /// <ja>
        /// �o�b�t�@���g�����܂��B
        /// </ja>
        /// <en>
        /// Expand the buffer.
        /// </en>
        /// </summary>
        /// <param name="length">Expanded buffer size.</param>
        public void ExpandBuffer(int length) {
            if (length <= _text.Length)
                return;

            char[] oldText = _text;
            _text = new char[length];
            Buffer.BlockCopy(oldText, 0, _text, 0, oldText.Length * sizeof(char)); 

            CharAttr[] oldAttrs = _attrs;
            _attrs = new CharAttr[length];
            Array.Copy(oldAttrs, 0, _attrs, 0, oldAttrs.Length);
        }

        /// <summary>
        /// <ja>
        /// �w��ʒu��1�����������݂܂��B
        /// </ja>
        /// <en>
        /// Write one character to specified position.
        /// </en>
        /// </summary>
        /// <param name="ch"><ja>�������ޕ���</ja><en>Character to write.</en></param>
        /// <param name="dec"><ja>�e�L�X�g�������w�肷��TextDecoration�I�u�W�F�N�g</ja>
        /// <en>TextDecoration object that specifies text format
        /// </en></param>
        public void PutChar(char ch, TextDecoration dec) {
            Debug.Assert(dec != null);
            Debug.Assert(_caretColumn >= 0);
            Debug.Assert(_caretColumn < _text.Length);

            char originalChar = Unicode.ToOriginal(ch);
            CharGroup charGroup = Unicode.GetCharGroup(ch);

            bool onZenkaku = (_attrs[_caretColumn].CharGroup == CharGroup.CJKZenkaku);
            bool onZenkakuRight = (_text[_caretColumn] == GLine.WIDECHAR_PAD);

            if (onZenkaku) {
                //�S�p�̏�ɏ���
                if (!onZenkakuRight) {
                    _text[_caretColumn] = originalChar;
                    _attrs[_caretColumn] = new CharAttr(dec, charGroup);
                    if (CharGroupUtil.GetColumnsPerCharacter(charGroup) == 1) {
                        //�S�p�̏�ɔ��p���������ꍇ�A�ׂɃX�y�[�X�����Ȃ��ƕ\���������
                        _caretColumn++;
                        if (_caretColumn < _text.Length) {
                            _text[_caretColumn] = ' ';
                            _attrs[_caretColumn].CharGroup = CharGroup.LatinHankaku;
                        }
                    }
                    else {
                        _attrs[_caretColumn + 1] = new CharAttr(dec, charGroup);
                        _caretColumn += 2;
                    }
                }
                else {
                    _text[_caretColumn - 1] = ' ';
                    _attrs[_caretColumn - 1].CharGroup = CharGroup.LatinHankaku;
                    _text[_caretColumn] = originalChar;
                    _attrs[_caretColumn] = new CharAttr(dec, charGroup);
                    if (CharGroupUtil.GetColumnsPerCharacter(charGroup) == 2) {
                        if (CharGroupUtil.GetColumnsPerCharacter(_attrs[_caretColumn + 1].CharGroup) == 2) {
                            if (_caretColumn + 2 < _text.Length) {
                                _text[_caretColumn + 2] = ' ';
                                _attrs[_caretColumn + 2].CharGroup = CharGroup.LatinHankaku;
                            }
                        }
                        _text[_caretColumn + 1] = GLine.WIDECHAR_PAD;
                        _attrs[_caretColumn + 1] = _attrs[_caretColumn];
                        _caretColumn += 2;
                    }
                    else {
                        _caretColumn++;
                    }
                }
            }
            else { //���p�̏�ɏ���
                _text[_caretColumn] = originalChar;
                _attrs[_caretColumn] = new CharAttr(dec, charGroup);
                if (CharGroupUtil.GetColumnsPerCharacter(charGroup) == 2) {
                    if (CharGroupUtil.GetColumnsPerCharacter(_attrs[_caretColumn + 1].CharGroup) == 2) { //���p�A�S�p�ƂȂ��Ă���Ƃ���ɑS�p����������
                        if (_caretColumn + 2 < _text.Length) {
                            _text[_caretColumn + 2] = ' ';
                            _attrs[_caretColumn + 2].CharGroup = CharGroup.LatinHankaku;
                        }
                    }
                    _text[_caretColumn + 1] = GLine.WIDECHAR_PAD;
                    _attrs[_caretColumn + 1] = _attrs[_caretColumn];
                    _caretColumn += 2;
                }
                else {
                    _caretColumn++; //���ꂪ�ł�common�ȃP�[�X����
                }
            }
        }

        /// <summary>
        /// <ja>
        /// �e�L�X�g�������w�肷��TextDecoration�I�u�W�F�N�g��ݒ肵�܂��B
        /// </ja>
        /// <en>
        /// Set the TextDecoration object that specifies the text format.
        /// </en>
        /// </summary>
        /// <param name="dec"><ja>�ݒ肷��TextDecoration�I�u�W�F�N�g</ja><en>Set TextDecoration object</en></param>
        public void SetDecoration(TextDecoration dec) {
            if (_caretColumn < _attrs.Length)
                _attrs[_caretColumn].Decoration = dec;
        }

        /// <summary>
        /// <ja>
        /// �L�����b�g���ЂƂ�O�ɖ߂��܂��B
        /// </ja>
        /// <en>
        /// Move the caret to the left of one character. 
        /// </en>
        /// </summary>
        /// <remarks>
        /// <ja>�L�����b�g�����łɍō��[�ɂ���Ƃ��ɂ́A�������܂���B</ja>
        /// <en>Nothing is done when there is already a caret at the high order end. </en>
        /// </remarks>
        public void BackCaret() {
            if (_caretColumn > 0) { //�ō��[�ɂ���Ƃ��͉������Ȃ�
                _caretColumn--;
            }
        }

        /// <summary>
        /// <ja>
        /// �w��͈͂𔼊p�X�y�[�X�Ŗ��߂܂��B
        /// </ja>
        /// <en>
        /// Fill the range of specification with space. 
        /// </en>
        /// </summary>
        /// <param name="from"><ja>���߂�J�n�ʒu�i���̈ʒu���܂݂܂��j</ja><en>Start position(include this position)</en></param>
        /// <param name="to"><ja>���߂�I���ʒu�i���̈ʒu�͊܂݂܂���j</ja><en>End position(exclude this position)</en></param>
        /// <param name="dec"><ja>�e�L�X�g�������w�肷��TextDecoration�I�u�W�F�N�g</ja><en>TextDecoration object that specifies text format
        /// </en></param>
        public void FillSpace(int from, int to, TextDecoration dec) {
            if (to > _text.Length)
                to = _text.Length;
            TextDecoration fillDec = dec;
            if (fillDec != null) {
                fillDec = fillDec.RetainBackColor();
                if (fillDec.IsDefault)
                    fillDec = null;
            }
            for (int i = from; i < to; i++) {
                _text[i] = ' ';
                _attrs[i] = new CharAttr(fillDec, CharGroup.LatinHankaku);
            }
        }
        //start����count�������������ċl�߂�B�E�[�ɂ�null������
        /// <summary>
        /// <ja>
        /// �w�肳�ꂽ�ꏊ����w�肳�ꂽ���������폜���A���̌����l�߂܂��B
        /// </ja>
        /// <en>
        /// The number of characters specified from the specified place is deleted, and the furnace is packed afterwards. 
        /// </en>
        /// </summary>
        /// <param name="start"><ja>�폜����J�n�ʒu</ja><en>Start position</en></param>
        /// <param name="count"><ja>�폜���镶����</ja><en>Count to delete</en></param>
        /// <param name="dec"><ja>�����̐V�����󔒗̈�̃e�L�X�g����</ja><en>text decoration for the new empty spaces at the tail of the line</en></param>
        public void DeleteChars(int start, int count, TextDecoration dec) {
            char fillChar;
            TextDecoration fillDec = dec;
            if (fillDec != null) {
                fillDec = fillDec.RetainBackColor();
                if (fillDec.IsDefault) {
                    fillDec = null;
                    fillChar = '\0';
                }
                else {
                    fillChar = ' ';
                }
            }
            else {
                fillChar = '\0';
            }
            for (int i = start; i < _text.Length; i++) {
                int j = i + count;
                if (j < _text.Length) {
                    _text[i] = _text[j];
                    _attrs[i] = _attrs[j];
                }
                else {
                    _text[i] = fillChar;
                    _attrs[i] = new CharAttr(fillDec, CharGroup.LatinHankaku);
                }
            }
        }

        /// <summary>
        /// <ja>�w��ʒu�Ɏw�肳�ꂽ�������̔��p�X�y�[�X��}�����܂��B</ja>
        /// <en>The half angle space only of the number specified for a specified position is inserted. </en>
        /// </summary>
        /// <param name="start"><ja>�폜����J�n�ʒu</ja><en>Start position</en></param>
        /// <param name="count"><ja>�}�����锼�p�X�y�[�X�̐�</ja><en>Count space to insert</en></param>
        /// <param name="dec"><ja>�󔒗̈�̃e�L�X�g����</ja><en>text decoration for the new empty spaces</en></param>
        public void InsertBlanks(int start, int count, TextDecoration dec) {
            TextDecoration fillDec = dec;
            if (fillDec != null) {
                fillDec = fillDec.RetainBackColor();
                if (fillDec.IsDefault)
                    fillDec = null;
            }
            for (int i = _text.Length - 1; i >= _caretColumn; i--) {
                int j = i - count;
                if (j >= _caretColumn) {
                    _text[i] = _text[j];
                    _attrs[i] = _attrs[j];
                }
                else {
                    _text[i] = ' ';
                    _attrs[i] = new CharAttr(fillDec, CharGroup.LatinHankaku);
                }
            }
        }

        /// <summary>
        /// <ja>
        /// �f�[�^���G�N�X�|�[�g���܂��B
        /// </ja>
        /// <en>
        /// Export the data.
        /// </en>
        /// </summary>
        /// <returns><ja>�G�N�X�|�[�g���ꂽGLine�I�u�W�F�N�g</ja><en>Exported GLine object</en></returns>
        public GLine Export() {
            GWord firstWord;
            GWord lastWord;

            CharAttr firstAttr = _attrs[0];
            if (firstAttr.Decoration == null)
                firstAttr.Decoration = TextDecoration.Default;
            firstWord = lastWord = new GWord(firstAttr.Decoration, 0, firstAttr.CharGroup);

            int limit = _text.Length;
            int offset;
            if (_text[0] == '\0') {
                offset = 0;
            }
            else {
                CharAttr prevAttr = firstAttr;
                for (offset = 1; offset < limit; offset++) {
                    char ch = _text[offset];
                    if (ch == '\0')
                        break;
                    else if (ch == GLine.WIDECHAR_PAD)
                        continue;

                    CharAttr attr = _attrs[offset];
                    if (attr.Decoration != prevAttr.Decoration || attr.CharGroup != prevAttr.CharGroup) {
                        if (attr.Decoration == null)
                            attr.Decoration = TextDecoration.Default;
                        GWord w = new GWord(attr.Decoration, offset, attr.CharGroup);
                        lastWord.Next = w;
                        lastWord = w;
                        prevAttr = attr;
                    }
                }
            }

            GLine line = new GLine((char[])_text.Clone(), offset, firstWord);
            line.EOLType = _eolType;
            return line;
        }
    }

#if UNITTEST
    [TestFixture]
    public class GLineManipulatorTests {

        [Test]
        public void PutChar1() {
            Assert.AreEqual("��aaz", TestPutChar("aaaaz", 0, '��'));
        }
        [Test]
        public void PutChar2() {
            Assert.AreEqual("�� az", TestPutChar("a��az", 0, '��'));
        }
        [Test]
        public void PutChar3() {
            Assert.AreEqual("b ��z", TestPutChar("����z", 0, 'b'));
        }
        [Test]
        public void PutChar4() {
            Assert.AreEqual("����z", TestPutChar("����z", 0, '��'));
        }
        [Test]
        public void PutChar5() {
            Assert.AreEqual(" b��z", TestPutChar("����z", 1, 'b'));
        }
        [Test]
        public void PutChar6() {
            Assert.AreEqual(" ��az", TestPutChar("��aaz", 1, '��'));
        }
        [Test]
        public void PutChar7() {
            Assert.AreEqual(" �� z", TestPutChar("����z", 1, '��'));
        }

        private static string TestPutChar(string initial, int col, char ch) {
            GLineManipulator m = new GLineManipulator();
            m.Load(GLine.ToCharArray(initial), col);
            //Debug.WriteLine(String.Format("Test{0}  [{1}] col={2} char={3}", num, SafeString(m._text), m.CaretColumn, ch));
            m.PutChar(ch, TextDecoration.ClonedDefault());
            //Debug.WriteLine(String.Format("Result [{0}] col={1}", SafeString(m._text), m.CaretColumn));
            return SafeString(m.InternalBuffer);
        }
    }
#endif

    /// <summary>
    /// <ja>
    /// ���s�R�[�h�̎�ނ������܂��B
    /// </ja>
    /// <en>
    /// Kind of Line feed code
    /// </en>
    /// </summary>
    public enum EOLType {
        /// <summary>
        /// <ja>���s�����Ɍp�����܂��B</ja><en>It continues without changing line.</en>
        /// </summary>
        Continue,
        /// <summary>
        /// <ja>CRLF�ŉ��s���܂��B</ja><en>It changes line with CRLF. </en>
        /// </summary>
        CRLF,
        /// <summary>
        /// <ja>CR�ŉ��s���܂��B</ja><en>It changes line with CR. </en>
        /// </summary>
        CR,
        /// <summary>
        /// <ja>LF�ŉ��s���܂��B</ja><en>It changes line with LF. </en>
        /// </summary>
        LF
    }

    /// <summary>
    /// <ja>�������ǂ̂悤�ɕ\������邩�������܂��B</ja>
    /// <en>Represents how the characters will be displayed.</en>
    /// </summary>
    /// <remarks>
    /// <para>
    /// "Hankaku" and "Zenkaku" are representing the width of the character.
    /// A "Hankaku" character will be displayed using single column width,
    /// and a "Zenkaku" character will be displayed using two column width.
    /// </para>
    /// </remarks>
    public enum CharGroup {
        /// <summary>
        /// <ja>���C���t�H���g�ŕ\������锼�p�����B</ja>
        /// <en>Hankaku characters to be displayed using main font.</en>
        /// </summary>
        LatinHankaku,
        /// <summary>
        /// <ja>CJK�t�H���g�ŕ\������锼�p�����B</ja>
        /// <en>Hankaku characters to be displayed using CJK font.</en>
        /// </summary>
        CJKHankaku,
        /// <summary>
        /// <ja>CJK�t�H���g�ŕ\�������S�p�����B</ja>
        /// <en>Zenkaku characters to be displayed using CJK font.</en>
        /// </summary>
        CJKZenkaku,
    }

    public static class CharGroupUtil {
        public static int GetColumnsPerCharacter(CharGroup cg) {
            if (cg == CharGroup.CJKZenkaku)
                return 2;
            else
                return 1;
        }

        public static bool IsCJK(CharGroup cg) {
            return (cg == CharGroup.CJKHankaku || cg == CharGroup.CJKZenkaku);
        }
    }


    //�P���؂�ݒ�B�܂�Preference�ɂ���Ԃł��Ȃ����낤
    /// <exclude/>
    public class ASCIIWordBreakTable {
        public const int LETTER = 0;
        public const int SYMBOL = 1;
        public const int SPACE = 2;
        public const int NOT_ASCII = 3;

        private int[] _data;

        public ASCIIWordBreakTable() {
            _data = new int[0x80];
            Reset();
        }

        public void Reset() { //�ʏ�ݒ�ɂ���
            //���䕶���p�[�g
            for (int i = 0; i <= 0x20; i++)
                _data[i] = SPACE;
            _data[0x7F] = SPACE; //DEL

            //�ʏ핶���p�[�g
            for (int i = 0x21; i <= 0x7E; i++) {
                char c = (char)i;
                if (('0' <= c && c <= '9') || ('a' <= c && c <= 'z') || ('A' <= c && c <= 'Z') || c == '_')
                    _data[i] = LETTER;
                else
                    _data[i] = SYMBOL;
            }
        }

        public int GetAt(char ch) {
            Debug.Assert(ch < 0x80);
            return _data[(int)ch];
        }

        //�ꕶ���ݒ�
        public void Set(char ch, int type) {
            Debug.Assert(ch < 0x80);
            _data[(int)ch] = type;
        }

        //�C���X�^���X
        private static ASCIIWordBreakTable _instance;

        public static ASCIIWordBreakTable Default {
            get {
                if (_instance == null)
                    _instance = new ASCIIWordBreakTable();
                return _instance;
            }
        }
    }

}
