/*
 * Copyright 2004,2006 The Poderosa Project.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *
 * $Id: TerminalOutput.cs,v 1.5 2012/02/25 04:47:05 kzmi Exp $
 */
using System;
using System.Resources;
using System.Collections;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using Poderosa.Protocols;
using Poderosa.Forms;

namespace Poderosa.Terminal {
    //����TerminalControl��AbstractTerminal�ɂ����Ⴒ���Ⴕ�Ă������M�@�\�𔲂��o��
    /// <summary>
    /// <ja>
    /// �^�[�~�i���ւƑ��M����@�\��񋟂��܂��B
    /// </ja>
    /// <en>
    /// Offer the function to transmit to the terminal.
    /// </en>
    /// </summary>
    public class TerminalTransmission {
        private AbstractTerminal _host;
        private ITerminalSettings _settings;
        private ITerminalConnection _connection;
        private ByteDataFragment _dataForLocalEcho;
        private readonly object _transmitSync = new object();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        /// <param name="settings"></param>
        /// <param name="connection"></param>
        /// <exclude/>
        public TerminalTransmission(AbstractTerminal host, ITerminalSettings settings, ITerminalConnection connection) {
            _host = host;
            _settings = settings;
            _connection = connection;
            _dataForLocalEcho = new ByteDataFragment();
        }

        /// <summary>
        /// <ja>
        /// �^�[�~�i���̃R�l�N�V�����������܂��B
        /// </ja>
        /// <en>
        /// Show the connection of terminal.
        /// </en>
        /// </summary>
        public ITerminalConnection Connection {
            get {
                return _connection;
            }
        }

        //���s�͓����Ă��Ȃ��O���
        /// <summary>
        /// <ja>
        /// Char�^�̔z��𑗐M���܂��B
        /// </ja>
        /// <en>
        /// Send a array of Char type.
        /// </en>
        /// </summary>
        /// <param name="chars"><ja>���M���镶���z��</ja><en>String array to send</en></param>
        /// <remarks>
        /// <ja>
        /// �����͌��݂̃G���R�[�h�ݒ�ɂ��G���R�[�h����Ă��瑗�M����܂��B
        /// </ja>
        /// <en>
        /// After it is encoded by a present encode setting, the character is transmitted. 
        /// </en>
        /// </remarks>
        public void SendString(char[] chars) {
            byte[] data = EncodingProfile.Get(_settings.Encoding).GetBytes(chars);
            Transmit(data);
        }
        /// <summary>
        /// <ja>
        /// ���s�𑗐M���܂��B
        /// </ja>
        /// <en>
        /// Transmit line feed.
        /// </en>
        /// </summary>
        /// <remarks>
        /// <ja>
        /// ���ۂɑ���f�[�^�͉��s�ݒ�ɂ��A�uCR�v�uLF�v�uCR+LF�v�̂����ꂩ�ɂȂ�܂��B
        /// </ja>
        /// <en>
        /// The data actually sent becomes either of "CR" "LF" "CR+LF" by the changing line setting. 
        /// </en>
        /// </remarks>
        public void SendLineBreak() {
            byte[] t = TerminalUtil.NewLineBytes(_settings.TransmitNL);
            Transmit(t);
        }
        /// <summary>
        /// <ja>
        /// �^�[�~�i���̃T�C�Y��ύX���܂��B
        /// </ja>
        /// <en>
        /// Change terminal size.
        /// </en>
        /// </summary>
        /// <param name="width"><ja>�^�[�~�i���̕�</ja><en>Width of terminal.</en></param>
        /// <param name="height"><ja>�^�[�~�i���̍���</ja><en>Height of terminal</en></param>
        public void Resize(int width, int height) {
            //TODO Transmit()�Ɠ��l��try...catch
            if (_connection.TerminalOutput != null) //keyboard-interactive�F�ؒ��ȂǁA�T�C�Y�ύX�ł��Ȃ��ǖʂ�����
                _connection.TerminalOutput.Resize(width, height);
        }

        /// <summary>
        /// <ja>
        /// �o�C�g�z��𑗐M���܂��B
        /// </ja>
        /// <en>
        /// Send array of byte.
        /// </en>
        /// </summary>
        /// <param name="data"><ja>���M����f�[�^���i�[���ꂽ�o�C�g�z��</ja><en>Byte array that contains data to send.</en></param>
        public void Transmit(byte[] data) {
            TransmitInternal(data, 0, data.Length, true);
        }

        /// <summary>
        /// Sends bytes. Data may be repeated as local echo.
        /// </summary>
        /// <param name="data">Byte array that contains data to send.</param>
        /// <param name="offset">Offset in data</param>
        /// <param name="length">Length of bytes to transmit</param>
        internal void Transmit(byte[] data, int offset, int length) {
            TransmitInternal(data, offset, length, true);
        }

        /// <summary>
        /// Sends bytes without local echo.
        /// </summary>
        /// <param name="data">Byte array that contains data to send.</param>
        /// <param name="offset">Offset in data</param>
        /// <param name="length">Length of bytes to transmit</param>
        internal void TransmitDirect(byte[] data, int offset, int length) {
            TransmitInternal(data, offset, length, false);
        }

        /// <summary>
        /// Sends bytes.
        /// </summary>
        /// <param name="data">Byte array that contains data to send.</param>
        /// <param name="offset">Offset in data</param>
        /// <param name="length">Length of bytes to transmit</param>
        /// <param name="localEcho">Whether bytes can be repeated as local echo</param>
        private void TransmitInternal(byte[] data, int offset, int length, bool localEcho) {
            // Note:
            //  This method may be called from multiple threads.
            //  One is UI thread which is processing key events, and another is communication thread
            //  which is going to send back something.
            //
            //  Some IPoderosaSocket implementations have thread-safe Transmit() method, but not all.
            //  So we transmit data exclusively.
            lock (_transmitSync) {
                try {
                    if (localEcho && _settings.LocalEcho) {
                        _dataForLocalEcho.Set(data, 0, data.Length);
                        _host.OnReception(_dataForLocalEcho);
                    }
                    _connection.Socket.Transmit(data, offset, length);
                }
                catch (Exception) {
                    try {
                        _connection.Close();
                    }
                    catch (Exception ex) {
                        RuntimeUtil.ReportException(ex);
                    }

                    _host.TerminalHost.OwnerWindow.Warning(GEnv.Strings.GetString("Message.TerminalControl.FailedToSend"));
                }
            }
        }

        //���Paste�p�����s���M�B�I����N���[�Y
        /// <summary>
        /// <ja>
        /// TextStream����ǂݎ�����f�[�^�𑗐M���܂��B
        /// </ja>
        /// <en>
        /// Transmit the data read from TextStream.
        /// </en>
        /// </summary>
        /// <param name="reader"><ja>�ǂݎ��TextStream</ja><en>Read TextStream</en></param>
        /// <param name="send_linebreak_last"><ja>�Ō�ɉ��s��t���邩�ǂ������w�肷��t���O�Btrue�̂Ƃ��A�Ō�ɉ��s���t�^����܂��B</ja><en>Flag that specifies whether to put changing line at the end. Line feed is given at the end at true. </en></param>
        /// <remarks>
        /// <para>
        /// <ja>�f�[�^�͌��݂̃G���R�[�h�ݒ�ɂ��A�G���R�[�h����Ă��瑗�M����܂��B</ja><en>After it is encoded by a present encode setting, data is transmitted. </en>
        /// </para>
        /// <para>
        /// <ja><paramref name="reader"/>�̓f�[�^�̑��M��ɕ����܂��iClose���\�b�h���Ăяo����܂��j�B</ja><en>After data is transmitted, <paramref name="reader"/> is closed (The Close method is called). </en>
        /// </para>
        /// </remarks>
        public void SendTextStream(TextReader reader, bool send_linebreak_last) {
            string line = reader.ReadLine();
            while (line != null) {
                SendString(line.ToCharArray());

                //�Â��̍s������Ȃ�΁A���s�͕K������B�ŏI�s�ł���Ȃ�΁A���ꂪ���s�����ŏI����Ă���ꍇ�̂݉��s�𑗂�B
                //������s�̓N���b�v�{�[�h�̓��e�Ɋւ�炸�^�[�~�i���̐ݒ�Ɋ�Â����Ƃɒ���
                bool last = reader.Peek() == -1;
                bool linebreak = last ? send_linebreak_last : true;
                if (linebreak)
                    SendLineBreak();

                line = reader.ReadLine();
            }
            reader.Close();
        }

        //����
        /// <exclude/>
        public void Revive(ITerminalConnection connection, int terminal_width, int terminal_height) {
            _connection = connection;
            Resize(terminal_width, terminal_height);
        }

    }

}
