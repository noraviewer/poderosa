/*
 * Copyright 2004,2006 The Poderosa Project.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *
 * $Id: ProtocolComponentEx.cs,v 1.2 2011/10/27 23:21:57 kzmi Exp $
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace Poderosa.Protocols {
    //�f�[�^����M�̈����Z�b�g(byte[], int, int)�̑�
    /// <summary>
    /// <ja>
    /// �f�[�^����M�̈����Z�b�g����舵���N���X�ł��B
    /// </ja>
    /// <en>
    /// Class that handles set of argument of data transmitting and receiving.
    /// </en>
    /// </summary>
    /// <remarks>
    /// <ja>
    /// ���̃N���X�́A�u�o�C�g�f�[�^�v�u�I�t�Z�b�g�v�u�����v���Z�b�g�Ƃ��Ĉ������̂ŁA�f�[�^�𑗎�M����ۂ̈����Ƃ��Ďg���܂��B
    /// </ja>
    /// <en>
    /// This class is used as an argument when data is transmit and received by the one to treat "Byte data", "Offset", and "Length" as a set. 
    /// </en>
    /// </remarks>
    public class ByteDataFragment {
        private byte[] _buffer;
        private int _offset;
        private int _length;

        /// <summary>
        /// <ja>
        /// �f�[�^����M�Z�b�g���쐬���܂��B
        /// </ja>
        /// <en>
        /// Create a set of data transmitting / recieving.
        /// </en>
        /// </summary>
        public ByteDataFragment() {
        }
        /// <summary>
        /// <ja>
        /// �u�f�[�^�v�u�I�t�Z�b�g�v�u�����v���w�肵�ăf�[�^����M�Z�b�g���쐬���܂��B
        /// </ja>
        /// <en>
        /// This class is used as an argument when data is transmit and received by the one to treat "Byte data", "Offset", and "Length" as a set. 
        /// </en>
        /// </summary>
        /// <param name="data"><ja>����M�����f�[�^�������z��ł��B</ja><en>It is an array that shows the transmit and received data. </en></param>
        /// <param name="offset"><ja>����M�������<paramref name="data"/>�̃I�t�Z�b�g�ʒu�ł��B</ja><en>It is an offset position of <paramref name="data"/> that shows the transmitting and receiving destination. </en></param>
        /// <param name="length"><ja>����M���钷���ł��B</ja><en>It is sent and received length. </en></param>
        public ByteDataFragment(byte[] data, int offset, int length) {
            Set(data, offset, length);
        }

        /// <summary>
        /// <ja>
        /// �u�f�[�^�v�u�I�t�Z�b�g�v�u�����v��ݒ肵�܂��B
        /// </ja>
        /// <en>
        /// Set "Data", "Offset", "Length".
        /// </en>
        /// </summary>
        /// <param name="buffer"><ja>����M�����f�[�^�������z��ł��B</ja><en>It is an array that shows the tranmit and received data. </en></param>
        /// <param name="offset"><ja>����M�������<paramref name="buffer"/>�ւ̃I�t�Z�b�g�ʒu�ł��B</ja><en>It is an offset position to <paramref name="buffer"/> that shows the transmitting and receiving destination. </en></param>
        /// <param name="length"><ja>����M���钷���ł��B</ja><en>It is transmit and received length. </en></param>
        /// <returns></returns>
        public ByteDataFragment Set(byte[] buffer, int offset, int length) {
            _buffer = buffer;
            _offset = offset;
            _length = length;
            return this;
        }

        /// <summary>
        /// <ja>����M�o�b�t�@�ł��B</ja>
        /// <en>Tranmit / recieve buffer</en>
        /// </summary>
        /// <remarks>
        /// <ja>
        /// ����M�f�[�^�͂����Ɋi�[���܂��B
        /// </ja>
        /// <en>
        /// Tranmit / Recieved data stored here.
        /// </en>
        /// </remarks>
        public byte[] Buffer {
            get {
                return _buffer;
            }
        }

        /// <summary>
        /// <ja>
        /// ����M�̃I�t�Z�b�g�ł��B
        /// </ja>
        /// <en>
        /// Offset of tranmitting and receiving. 
        /// </en>
        /// </summary>
        public int Offset {
            get {
                return _offset;
            }
        }

        /// <summary>
        /// <ja>
        /// ����M���钷���ł��B
        /// </ja>
        /// <en>
        /// Length of tranmitting and receiving. 
        /// </en>
        /// </summary>
        public int Length {
            get {
                return _length;
            }
        }
    }

    //byte[]�x�[�X�̏o�́B��AbstractGuevaraSocket
    /// <summary>
    /// <ja>
    /// �f�[�^�𑗐M���邽�߂̃C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface to transmit data.
    /// </en>
    /// </summary>
    public interface IByteOutputStream {
        /// <summary>
        /// <ja>
        /// ByteDataFragment�I�u�W�F�N�g���w�肵�ăf�[�^�𑗐M���܂��B
        /// </ja>
        /// <en>
        /// Data is transmitted specifying the ByteDataFragment object. 
        /// </en>
        /// </summary>
        /// <param name="data"><ja>���M����f�[�^�������Ă���I�u�W�F�N�g�ł��B</ja><en>Object with transmitted data</en></param>
        /// <overloads>
        /// <summary>
        /// <ja>
        /// �f�[�^�𑗐M���܂��B
        /// </ja>
        /// <en>
        /// Transmitting data.
        /// </en>
        /// </summary>
        /// </overloads>
        void Transmit(ByteDataFragment data);
        /// <summary>
        /// <ja>
        /// �u�o�C�g�z��v�u�I�t�Z�b�g�v�u�����v���w�肵�ăf�[�^�𑗐M���܂��B
        /// </ja>
        /// <en>
        /// Data is transmitted specifying "Byte array", "Offset", and "Length". 
        /// </en>
        /// </summary>
        /// <param name="data"><ja>�f�[�^�������Ă���o�C�g�z��</ja><en>Byte array with data</en></param>
        /// <param name="offset"><ja>�f�[�^�𑗐M����ʒu���������I�t�Z�b�g</ja><en>Offset that showed position in which data is transmitted</en></param>
        /// <param name="length"><ja>���M���钷��</ja><en>Transmitted length</en></param>
        void Transmit(byte[] data, int offset, int length);
        /// <summary>
        /// <ja>
        /// �ڑ�����܂��B
        /// </ja>
        /// <en>
        /// Close the connection.
        /// </en>
        /// </summary>
        void Close();
    }
    //byte[]�x�[�X�̔񓯊����́B��IDataReceiver
    /// <summary>
    /// <ja>
    /// <seealso cref="IPoderosaSocket">IPoderosaSocket</seealso>��ʂ��ăf�[�^��񓯊���
    /// ��M����Ƃ��ɗp����C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface that 	used when data is asynchronously received through <seealso cref="IPoderosaSocket">IPoderosaSocket</seealso>. 
    /// </en>
    /// </summary>
    /// <remarks>
    /// <ja>
    /// �f�[�^����M�������v���O�C���́A���̃C���^�[�t�F�C�X�����������I�u�W�F�N�g��p�ӂ��A
    /// <seealso cref="IPoderosaSocket">IPoderosaSocket</seealso>��<see cref="IPoderosaSocket.RepeatAsyncRead">
    /// RepeatAsyncRead���\�b�h</see>���Ăяo���ēo�^���܂��B
    /// </ja>
    /// <en>
    /// The object that implements this interface is prepared, and the plug-in that wants to receive the data calls and registers the <see cref="IPoderosaSocket.RepeatAsyncRead">
    /// RepeatAsyncRead method</see> of <seealso cref="IPoderosaSocket">IPoderosaSocket</seealso>. 
    /// </en>
    /// </remarks>
    public interface IByteAsyncInputStream {
        /// <summary>
        /// <ja>
        /// �f�[�^���͂����Ƃ��ɌĂяo����܂��B
        /// </ja>
        /// <en>
        /// Called when data recieves
        /// </en>
        /// </summary>
        /// <param name="data"><ja>��M�f�[�^�������I�u�W�F�N�g</ja><en>Object that show the recieved data.</en></param>
        void OnReception(ByteDataFragment data);
        /// <summary>
        /// <ja>
        /// �ڑ����ʏ�̐ؒf�菇�ŏI�������Ƃ��ɌĂяo����܂��B
        /// </ja>
        /// <en>
        /// When the connection terminates normally, it is called. 
        /// </en>
        /// </summary>
        void OnNormalTermination();
        /// <summary>
        /// <ja>
        /// �ڑ����G���[�Ȃǂُ̈�ɂ���ďI�������Ƃ��ɌĂяo����܂��B
        /// </ja>
        /// <en>
        /// When the connection terminates due to abnormality of the error etc. , it is called. 
        /// </en>
        /// </summary>
        /// <param name="message"><ja>�ؒf���ꂽ���R������������</ja><en>String that shows closed reason</en></param>
        void OnAbnormalTermination(string message);
    }

    //�ڑ��̐����E���s�̒ʒm�B���Ƃ���MRU�R���|�[�l���g���������M���Ď��g�̏����X�V����
    //Interrupt���ꂽ�ꍇ�͒ʒm�Ȃ�
    /// <summary>
    /// <ja>
    /// �ڑ��̐����E���s�̒ʒm���󂯎��C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface that receives notification of success and failure of connection
    /// </en>
    /// </summary>
    /// <remarks>
    /// <ja>
    /// ���̃C���^�[�t�F�C�X�́AProtocols�v���O�C���̊g���|�C���g�uorg.poderosa.protocols.resultEventHandler�v�ɂ���Ē񋟂���܂��B
    /// </ja>
    /// <en>
    /// This interface is offered with the extension point (org.poderosa.protocols.resultEventHandler) of the Protocols plug-in. 
    /// </en>
    /// </remarks>
    public interface IConnectionResultEventHandler {
        /// <summary>
        /// <ja>
        /// �ڑ������������Ƃ��ɌĂяo����܂��B
        /// </ja>
        /// <en>
        /// When the connection succeeds, it is called. 
        /// </en>
        /// </summary>
        /// <param name="param"><ja>�ڑ��p�����[�^�ł�</ja><en>Connection parameter.</en></param>
        void OnSucceeded(ITerminalParameter param);
        /// <summary>
        /// <ja>
        /// �ڑ������s�����Ƃ��ɌĂяo����܂��B
        /// </ja>
        /// <en>
        /// When the connection fails, it is called. 
        /// </en>
        /// </summary>
        /// <param name="param"><ja>�ڑ��p�����[�^�ł�</ja><en>Connection parameter.</en></param>
        /// <param name="msg"><ja>���s�������R���܂܂�郁�b�Z�[�W�ł�</ja><en>Message being included for failing reason</en></param>
        void OnFailed(ITerminalParameter param, string msg);

        /// <summary>
        /// <ja>
        /// �񓯊��ڑ��J�n�O�ɌĂ΂�܂�
        /// </ja>
        /// <en>
        /// Called before the asynchronous connection starts
        /// </en>
        /// </summary>
        /// <param name="param"></param>
        void BeforeAsyncConnect(ITerminalParameter param);
    }

}
