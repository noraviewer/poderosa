/*
 * Copyright 2004,2006 The Poderosa Project.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *
 * $Id: PromptRecognizer.cs,v 1.6 2011/12/11 11:19:50 kzmi Exp $
 */

//#define DEBUG_LINECACHE

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;

using Poderosa.Util.Collections;
using Poderosa.Document;
using Poderosa.Preferences;

namespace Poderosa.Terminal {
    //�O���ɒʒm����C���^�t�F�[�X
    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public interface IPromptProcessor {
        void OnPromptLine(GLine line, string prompt, string command); //line�̓v�����v�g�̂���s�B�J�����g�̍s�Ƃ͌���Ȃ�
        void OnNotPromptLine();
    }




    internal class PromptRecognizer : ITerminalSettingsChangeListener {
        private AbstractTerminal _terminal;
        private Regex _promptExpression;
        private List<IPromptProcessor> _listeners;

        private StringBuilder _commandBuffer;

        private bool _contentUpdateMark;

        private struct PromptInfo {
            public readonly string Prompt;
            public readonly int NextOffset;

            public PromptInfo(string prompt, int nextOffset) {
                Prompt = prompt;
                NextOffset = nextOffset;
            }
        }

        private class LineCache {
            private string[] _buff;
            private int _lastID;
            private int _lastIndex;

            public LineCache(int capacity) {
                _buff = new string[capacity];
                _lastID = -1;
                _lastIndex = 0;
#if DEBUG_LINECACHE
                Debug.WriteLine(String.Format("LineCache: Capacity={0}", capacity));
#endif
            }

            public void Extend(int capacity) {
                if (capacity <= _buff.Length)
                    return;
                _buff = new string[capacity];
                _lastID = -1;
                _lastIndex = 0;
#if DEBUG_LINECACHE
                Debug.WriteLine(String.Format("LineCache: Capacity={0}", capacity));
#endif
            }

            public void Update(int id, string text) {
                int firstID = GetFirstID();
                int buffSize = _buff.Length;
                int idLimit = _lastID + buffSize - 1;
                if (_lastID < id && id <= idLimit) {
                    int n = _lastID + 1;
                    int index = (_lastIndex + 1) % buffSize;
                    while(n < id) {
                        _buff[index] = null;
                        n++;
                        index = (index + 1) % buffSize;
                    }
                    _buff[index] = text;
                    _lastID = id;
                    _lastIndex = index;
#if DEBUG_LINECACHE
                    Debug.WriteLine(String.Format("LineCache[{0}] <-- {1}:{2}", index, id, text));
#endif
                }
                else if (firstID <= id && id <= _lastID) {
                    int index = GetIndex(id);
                    _buff[index] = text;
#if DEBUG_LINECACHE
                    Debug.WriteLine(String.Format("LineCache[{0}] <-- {1}:{2}", index, id, text));
#endif
                }
                else {
                    // reset
#if DEBUG_LINECACHE
                    Debug.WriteLine("LineCache: Reset");
#endif
                    _buff[0] = text;
                    for (int i = 1; i < buffSize; i++)
                        _buff[i] = null;
                    _lastID = id;
                    _lastIndex = 0;
                }
            }

            public string Get(int id) {
                int firstID = GetFirstID();
                if (firstID <= id && id <= _lastID) {
                    return _buff[GetIndex(id)];
                }
                else {
                    return null;
                }
            }

            private int GetFirstID() {
                return _lastID - (_buff.Length - 1);
            }

            private int GetIndex(int id) {
                int offset = _lastID - id;
                int buffSize = _buff.Length;
                return (_lastIndex + buffSize - offset) % buffSize;
            }
        }

        private readonly LineCache _lineCache;
        private int _lastCachedLineID;


        public PromptRecognizer(AbstractTerminal term) {
            _terminal = term;
            _commandBuffer = new StringBuilder();
            ITerminalSettings ts = term.TerminalHost.TerminalSettings;
            ts.AddListener(this);
            _promptExpression = new Regex(ts.ShellScheme.PromptExpression, RegexOptions.Compiled); //����̓V�F���ɂ���
            _listeners = new List<IPromptProcessor>();
            _lineCache = new LineCache(PromptRecognizerPreferences.Instance.PromptSearchMaxLines);
            _lastCachedLineID = -1;
        }

        public void AddListener(IPromptProcessor l) {
            _listeners.Add(l);
        }
        public void RemoveListener(IPromptProcessor l) {
            _listeners.Remove(l);
        }

        //���K�\���̃}�b�`�������肷��̂����炷�ׂ��A�^�C�}�[�������Ƃ��ɍX�V����X�^�C�����m��
        public void SetContentUpdateMark() {
            _contentUpdateMark = true;
        }
        public void CheckIfUpdated() {
            if (_contentUpdateMark)
                Recognize();
            _contentUpdateMark = false;
        }


        public void Recognize() {
            if (_promptExpression == null)
                return;
            if (_terminal.TerminalMode == TerminalMode.Application)
                return; //�A�v���P�[�V�������[�h�͒ʒm�̕K�v�Ȃ�
            //�ꉞ�A�O��`�F�b�N���ƃf�[�^��M�̗L����������Ă���Ώ����̊ȗ����͉\

            TerminalDocument doc = _terminal.GetDocument();
            int maxLines = PromptRecognizerPreferences.Instance.PromptSearchMaxLines;
            _lineCache.Extend(maxLines);
            GLine promptCandidate = FindPromptCandidateLine(doc, maxLines);
            if (promptCandidate == null)
                return; // too large command line
            string prompt;
            string command;

            if (!DeterminePromptLine(promptCandidate, doc.CurrentLine.ID, doc.CaretColumn, out prompt, out command)) { //�v�����v�g�ł͂Ȃ��Ƃ�
                NotifyNotPromptLine();
            }
            else {
                Debug.WriteLineIf(DebugOpt.PromptRecog, "Prompt " + command);
                NotifyPromptLine(promptCandidate, prompt, command);
            }
        }

        //�O���Ŏw�肵��GLine�ɂ��āACurrentLine�܂ł̗̈�Ɋւ��Ĕ�����s��
        public bool DeterminePromptLine(GLine line, int limitLineID, int limitColumn, out string prompt, out string command) {
            prompt = command = null;
            if (_promptExpression == null)
                return false;
            if (_terminal.TerminalMode == TerminalMode.Application)
                return false; //�A�v���P�[�V�������[�h�͒ʒm�̕K�v�Ȃ�

            PromptInfo promptInfo = CheckPrompt(line);
            if (promptInfo.Prompt == null) //�v�����v�g�ł͂Ȃ��Ƃ�
                return false;
            else {
                prompt = promptInfo.Prompt;
                command = ParseCommand(line, limitLineID, limitColumn, promptInfo);
                return true;
            }
        }

        //�ӂ��͌��ݍs�����A�O�s������s�I�[�Ȃ炳���̂ڂ�
        private GLine FindPromptCandidateLine(TerminalDocument doc, int maxLines) {
            GLine line = doc.CurrentLine;
            for (int i = 0; i < maxLines; i++) {
                GLine prev = line.PrevLine;
                if (prev == null || prev.EOLType != EOLType.Continue)
                    return line;
                line = prev;
            }
            return null;
        }

        private string GetTextAndUpdateCache(GLine line, bool forceUpdate) {
            int lineID = line.ID;
            string lineText = forceUpdate ? null : _lineCache.Get(lineID);
            if (lineText == null) {
                lineText = line.ToNormalString();
                _lineCache.Update(lineID, lineText);
                _lastCachedLineID = lineID;
            }
            return lineText;
        }

        private PromptInfo CheckPrompt(GLine promptCandidate) {
            bool forceUpdate = (promptCandidate.ID == _lastCachedLineID) ? true : false;
            string lineText = GetTextAndUpdateCache(promptCandidate, forceUpdate);

            //���g���Ȃ���΃`�F�b�N�����Ȃ�
            if (lineText.Length == 0)
                return new PromptInfo(null, 0);

            Match match = _promptExpression.Match(lineText);
            if (match.Success)
                return new PromptInfo(match.Value, match.Index + match.Length);
            else
                return new PromptInfo(null, 0);
        }

        //�R�}���h�S�e������ ����ʒu�̏I�[��limit_line_id, limit_column�Ō��߂���
        private string ParseCommand(GLine promptCandidate, int limitLineID, int limitColumn, PromptInfo promptInfo) {
            _commandBuffer.Remove(0, _commandBuffer.Length);

            Debug.Assert(promptCandidate.ID <= _terminal.GetDocument().CurrentLine.ID);

            int firstLineID = promptCandidate.ID;
            int offset = promptInfo.NextOffset; // initial offset of the first line
            for (GLine line = promptCandidate; line != null && line.ID <= limitLineID; line = line.NextLine) {
                bool forceUpdate;
                if (line.ID == _lastCachedLineID && line.ID != firstLineID) {
                    // If the line was last-cached line, retrieve new text from the line.
                    // But if the line was promptCandidate, the latest text has been already cached in CheckPrompt().
                    forceUpdate = true;
                }
                else {
                    forceUpdate = false;
                }
                string lineText = GetTextAndUpdateCache(line, forceUpdate);

                if (offset < lineText.Length) {
                    int copyLength;
                    if (line.ID == limitLineID && limitColumn < lineText.Length)
                        copyLength = limitColumn - offset;
                    else
                        copyLength = lineText.Length - offset;

                    if (copyLength > 0)
                        _commandBuffer.Append(lineText, offset, copyLength);
                }

                offset = 0; // initial offset of the next line
            }

            return _commandBuffer.ToString();
        }

        private void NotifyPromptLine(GLine line, string prompt, string command) {
            foreach (IPromptProcessor l in _listeners)
                l.OnPromptLine(line, prompt, command);
        }
        private void NotifyNotPromptLine() {
            foreach (IPromptProcessor l in _listeners)
                l.OnNotPromptLine();
        }

        //ITerminalSettingChangeListener
        public void OnBeginUpdate(ITerminalSettings current) {
        }

        public void OnEndUpdate(ITerminalSettings current) {
            _promptExpression = new Regex(current.ShellScheme.PromptExpression, RegexOptions.Compiled);
            Debug.WriteLineIf(DebugOpt.IntelliSenseMenu, "UpdatePrompt");
        }
    }


    /// <summary>
    /// Preferences for PromptRecognizer
    /// </summary>
    internal class PromptRecognizerPreferences : IPreferenceSupplier {

        private static PromptRecognizerPreferences _instance = new PromptRecognizerPreferences();

        public static PromptRecognizerPreferences Instance {
            get {
                return _instance;
            }
        }

        private const int DEFAULT_PROMPT_SEARCH_MAX_LINES = 5;

        private IIntPreferenceItem _promptSearchMaxLines;

        /// <summary>
        /// Get max lines for searching prompt
        /// </summary>
        public int PromptSearchMaxLines {
            get {
                if (_promptSearchMaxLines != null)
                    return _promptSearchMaxLines.Value;
                else
                    return DEFAULT_PROMPT_SEARCH_MAX_LINES;
            }
        }

        #region IPreferenceSupplier

        public string PreferenceID {
            get {
                return TerminalEmulatorPlugin.PLUGIN_ID + ".promptrecognizer";
            }
        }

        public void InitializePreference(IPreferenceBuilder builder, IPreferenceFolder folder) {
            _promptSearchMaxLines = builder.DefineIntValue(folder, "promptSearchMaxLines", DEFAULT_PROMPT_SEARCH_MAX_LINES, PreferenceValidatorUtil.PositiveIntegerValidator);
        }

        public object QueryAdapter(IPreferenceFolder folder, Type type) {
            return null;
        }

        public void ValidateFolder(IPreferenceFolder folder, IPreferenceValidationResult output) {
        }

        #endregion
    }


}
