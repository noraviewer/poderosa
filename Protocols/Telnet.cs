/*
 * Copyright 2004,2006 The Poderosa Project.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *
 * $Id: Telnet.cs,v 1.3 2011/10/27 23:21:57 kzmi Exp $
 */
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;

namespace Poderosa.Protocols {

    /// <summary>
    /// TelnetOption�̑���M������B���܂蕡�G�ȃT�|�[�g���������͂Ȃ��B
    /// Guevara�ŕK�v�Ȃ̂�SuppressGoAhead(�o����), TerminalType, NAWS�̂R�����ŁA����炪�������Ȃ���Η�O�𓊂���B
    /// ����ȊO��TelnetOption�͋��ۂ��邪�A���ۂ��������Ȃ��Ă�_refusedOption�Ɋi�[���邾���ŃG���[�ɂ͂��Ȃ��B
    /// �I�v�V�����̃l�S�V�G�[�V�������I��������A�Ō�Ɏ�M�����p�P�b�g�͂����V�F���{�̂ł���̂ŁA�Ăяo�����͂�����g���悤�ɂ��Ȃ��Ƃ����Ȃ��B
    /// </summary>
    internal class TelnetNegotiator {
        private string _terminalType;
        //�K�v�Ȃ炱���������ǂ�
        private int _width;
        private int _height;

        private TelnetCode _state;
        private MemoryStream _sequenceBuffer;
        private TelnetOptionWriter _optionWriter;
        private bool _defaultOptionSent;

        internal enum ProcessResult {
            NOP,
            REAL_0xFF
        }

        //�ڑ��𒆒f����قǂł͂Ȃ������҂ǂ���łȂ������ꍇ�Ɍx�����o��
        private List<string> _warnings;
        public List<string> Warnings {
            get {
                return _warnings;
            }
        }

        public TelnetNegotiator(string terminal_type, int width, int height) {
            Debug.Assert(terminal_type != null);
            _terminalType = terminal_type;
            _width = width;
            _height = height;
            _warnings = new List<string>();
            _state = TelnetCode.NA;
            _sequenceBuffer = new MemoryStream();
            _optionWriter = new TelnetOptionWriter();
            _defaultOptionSent = false;
        }

        public void Flush(IPoderosaSocket s) {
            if (!_defaultOptionSent) {
                WriteDefaultOptions();
                _defaultOptionSent = true;
            }

            if (_optionWriter.Length > 0) {
                _optionWriter.WriteTo(s);
                //s.Flush();
                _optionWriter.Clear();
            }
        }

        private void WriteDefaultOptions() {
            _optionWriter.Write(TelnetCode.WILL, TelnetOption.TerminalType);
            _optionWriter.Write(TelnetCode.DO, TelnetOption.SuppressGoAhead);
            _optionWriter.Write(TelnetCode.WILL, TelnetOption.SuppressGoAhead);
            _optionWriter.Write(TelnetCode.WILL, TelnetOption.NAWS);
        }

        public bool InProcessing {
            get {
                return _state != TelnetCode.NA;
            }
        }
        public void StartNegotiate() {
            _state = TelnetCode.IAC;
        }

        public ProcessResult Process(byte data) {
            Debug.Assert(_state != TelnetCode.NA);
            switch (_state) {
                case TelnetCode.IAC:
                    if (data == (byte)TelnetCode.SB || ((byte)TelnetCode.WILL <= data && data <= (byte)TelnetCode.DONT))
                        _state = (TelnetCode)data;
                    else if (data == (byte)TelnetCode.IAC) {
                        _state = TelnetCode.NA;
                        return ProcessResult.REAL_0xFF;
                    }
                    else
                        _state = TelnetCode.NA;
                    break;
                case TelnetCode.SB:
                    if (data != (byte)TelnetCode.SE && data != (byte)TelnetOption.NAWS) //IAC SB 0x1F �Ƃ��Ă��������A�Ƃ����P�[�X���������B�z�X�g���̎d�l�ᔽ�̂悤�Ɍ����邪�APoderosa�������̉�����Ԃ��킯�ł͂Ȃ��̂ł���ŉ��
                        _sequenceBuffer.WriteByte(data);
                    else {
                        ProcessSequence(_sequenceBuffer.ToArray());
                        _state = TelnetCode.NA;
                        _sequenceBuffer.SetLength(0);
                    }
                    break;
                case TelnetCode.DO:
                case TelnetCode.DONT:
                case TelnetCode.WILL:
                case TelnetCode.WONT:
                    ProcessOptionRequest(data);
                    _state = TelnetCode.NA;
                    break;
            }

            return ProcessResult.NOP;
        }

        private void ProcessSequence(byte[] response) {
            if (response.Length > 1 && response[1] == 1) {
                if (response[0] == (byte)TelnetOption.TerminalType)
                    _optionWriter.WriteTerminalName(_terminalType);
            }
        }

        private void ProcessOptionRequest(byte option_) {
            TelnetOption option = (TelnetOption)option_;
            switch (option) {
                case TelnetOption.TerminalType:
                    if (_state == TelnetCode.DO)
                        _optionWriter.Write(TelnetCode.WILL, option);
                    else
                        _warnings.Add(PEnv.Strings.GetString("Message.Telnet.FailedToSendTerminalType"));
                    break;
                case TelnetOption.NAWS:
                    if (_state == TelnetCode.DO)
                        _optionWriter.WriteTerminalSize(_width, _height);
                    else
                        _warnings.Add(PEnv.Strings.GetString("Message.Telnet.FailedToSendWidnowSize"));
                    break;
                case TelnetOption.SuppressGoAhead:
                    if (_state != TelnetCode.WILL && _state != TelnetCode.DO) //!!�������������Ƃ��m�F����
                        _warnings.Add(PEnv.Strings.GetString("Message.Telnet.FailedToSendSuppressGoAhead"));
                    break;
                case TelnetOption.LocalEcho:
                    if (_state == TelnetCode.DO)
                        _optionWriter.Write(TelnetCode.WILL, option);
                    break;
                default: //��L�ȊO�͂��ׂċ��ہBDO�ɂ�WON'T, WILL�ɂ�DON'T�̉�����Ԃ��B 
                    if (_state == TelnetCode.DO)
                        _optionWriter.Write(TelnetCode.WONT, option);
                    else if (_state == TelnetCode.WILL)
                        _optionWriter.Write(TelnetCode.DONT, option);
                    break;
            }
        }

    }


    internal class TelnetOptionWriter {
        private MemoryStream _strm;
        public TelnetOptionWriter() {
            _strm = new MemoryStream();
        }
        public long Length {
            get {
                return _strm.Length;
            }
        }
        public void Clear() {
            _strm.SetLength(0);
        }

        public void WriteTo(IPoderosaSocket target) {
            byte[] data = _strm.ToArray();
            target.Transmit(data, 0, data.Length);
            //target.Flush();
        }
        public void Write(TelnetCode code, TelnetOption opt) {
            _strm.WriteByte((byte)TelnetCode.IAC);
            _strm.WriteByte((byte)code);
            _strm.WriteByte((byte)opt);
        }
        public void WriteTerminalName(string name) {
            _strm.WriteByte((byte)TelnetCode.IAC);
            _strm.WriteByte((byte)TelnetCode.SB);
            _strm.WriteByte((byte)TelnetOption.TerminalType);
            _strm.WriteByte(0); //0 = IS
            byte[] t = Encoding.ASCII.GetBytes(name);
            _strm.Write(t, 0, t.Length);
            _strm.WriteByte((byte)TelnetCode.IAC);
            _strm.WriteByte((byte)TelnetCode.SE);
        }
        public void WriteTerminalSize(int width, int height) {
            _strm.WriteByte((byte)TelnetCode.IAC);
            _strm.WriteByte((byte)TelnetCode.SB);
            _strm.WriteByte((byte)TelnetOption.NAWS);
            _strm.WriteByte((byte)(width >> 8));
            _strm.WriteByte((byte)width);
            _strm.WriteByte((byte)(height >> 8));
            _strm.WriteByte((byte)height);
            _strm.WriteByte((byte)TelnetCode.IAC);
            _strm.WriteByte((byte)TelnetCode.SE);
        }
    }

    internal enum TelnetCode {
        NA = 0,
        SE = 240,
        NOP = 241,
        Break = 243,
        AreYouThere = 246,
        SB = 250,
        WILL = 251,
        WONT = 252,
        DO = 253,
        DONT = 254,
        IAC = 255
    }
    internal enum TelnetOption {
        LocalEcho = 1,
        SuppressGoAhead = 3,
        TerminalType = 24,
        NAWS = 31
    }

    internal class TelnetNegotiationException : ApplicationException {
        public TelnetNegotiationException(string msg)
            : base(msg) {
        }
    }

}
