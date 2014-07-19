/*
 * Copyright 2004,2006 The Poderosa Project.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *
 * $Id: ProtocolsEx.cs,v 1.2 2011/10/27 23:21:57 kzmi Exp $
 */
using System;
using System.Collections.Generic;
using System.Text;
using Poderosa.Forms;
using Granados;

namespace Poderosa.Protocols {

    /// <summary>
    /// <ja>
    /// �V�K�Ƀ^�[�~�i���ڑ��������Ƃ��A������L�����Z�����邽�߂̃C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface to cancel it when terminal was newly connected.
    /// </en>
    /// </summary>
    /// <remarks>
    /// <ja>
    /// ���̃C���^�[�t�F�C�X�́A<seealso cref="IProtocolService">IProtocolService</seealso>��
    /// <see cref="IProtocolService.AsyncCygwinConnect">AsyncCygwinConnect���\�b�h</see>�A
    /// <see cref="IProtocolService.AsyncTelnetConnect">AsyncTelnetConnect���\�b�h</see>�A
    /// <see cref="IProtocolService.AsyncSSHConnect">AsyncSSHConnect���\�b�h</see>�̖߂�l�Ƃ��Ďg���܂��B
    /// </ja>
    /// <en>
    /// This interface is used as a return value of the <see cref="IProtocolService.AsyncCygwinConnect">AsyncCygwinConnect method</see> and the method of <seealso cref="IProtocolService">IProtocolService</seealso> of <see cref="IProtocolService.AsyncTelnetConnect">AsyncTelnetConnect method</see>, <see cref="IProtocolService.AsyncSSHConnect">AsyncSSHConnect method</see>. 
    /// </en>
    /// </remarks>
    public interface IInterruptable {
        /// <summary>
        /// <ja>
        /// �ڑ��𒆎~���܂��B
        /// </ja>
        /// <en>
        /// Interrupt the connection.
        /// </en>
        /// <remarks>
        /// <ja>
        /// ���̃��\�b�h���Ăяo���ƁA<seealso cref="IInterruptableConnectorClient">IInterruptableConnectorClient</seealso>��
        /// �����������\�b�h�͌Ăяo���ꂸ�ɁA�ڑ������~����܂��B
        /// </ja>
        /// <en>
        /// The connection is discontinued without calling the method of implementing on <seealso cref="IInterruptableConnectorClient">IInterruptableConnectorClient</seealso> when this method is called. 
        /// </en>
        /// </remarks>
        /// </summary>
        void Interrupt();
    }

    /// <summary>
    /// <ja>
    /// �V�K�Ƀ^�[�~�i���R�l�N�V������񓯊��ō쐬����Ƃ��A�ڑ��̐����⎸�s�̏�Ԃ��󂯎�邽�߂̃C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface to receive state of success and failure of connection when terminal connection is asynchronously newly made
    /// </en>
    /// </summary>
    /// <remarks>
    /// <ja>
    /// <para>
    /// ���̃C���^�[�t�F�C�X�́A<seealso cref="IProtocolService">IProtocolService</seealso>��
    /// <see cref="IProtocolService.AsyncCygwinConnect">AsyncCygwinConnect���\�b�h</see>�A
    /// <see cref="IProtocolService.AsyncTelnetConnect">AsyncTelnetConnect���\�b�h</see>�A
    /// <see cref="IProtocolService.AsyncSSHConnect">AsyncSSHConnect���\�b�h</see>���Ăяo���āA�񓯊��̐ڑ�������ہA
    /// �����⎸�s�̏�Ԃ��󂯎�邽�߂ɗp���܂��B
    /// </para>
    /// <para>
    /// �ȈՓI�ȓ����ڑ�������̂ł���΁A���̃C���^�[�t�F�C�X�����������I�u�W�F�N�g��p�ӂ������ɁA
    /// <seealso cref="IProtocolService">IProtocolService</seealso>��<see cref="IProtocolService.CreateFormBasedSynchronozedConnector">CreateFormBasedSynchronozedConnector���\�b�h</see>
    /// ���Ăяo���A����<see cref="ISynchronizedConnector.InterruptableConnectorClient">InterruptableConnectorClient�v���p�e�B</see>��
    /// �l���g�����Ƃ��ł��܂��B
    /// </para>
    /// </ja>
    /// <en>
    /// <para>
    /// This interface is used to receive the state of the success and the failure when the <see cref="IProtocolService.AsyncCygwinConnect">AsyncCygwinConnect method</see>, the <see cref="IProtocolService.AsyncTelnetConnect">AsyncTelnetConnect method</see>, and the <see cref="IProtocolService.AsyncSSHConnect">AsyncSSHConnect method</see> of <seealso cref="IProtocolService">IProtocolService</seealso> are called, and the asynchronous system is connected. 
    /// </para>
    /// <para>
    /// The <see cref="IProtocolService.CreateFormBasedSynchronozedConnector">CreateFormBasedSynchronozedConnector method</see> of <seealso cref="IProtocolService">IProtocolService</seealso> can be called instead of preparing the object that mounts this interface, and the value of the <see cref="ISynchronizedConnector.InterruptableConnectorClient">InterruptableConnectorClient property </see>be used if it simplicity and synchronous connects it. 
    /// </para>
    /// </en>
    /// </remarks>
    public interface IInterruptableConnectorClient {
        /// <summary>
        /// <ja>
        /// �ڑ������������Ƃ��ɌĂяo����܂��B
        /// </ja>
        /// <en>
        /// Called when the connection is succeeded.
        /// </en>
        /// </summary>
        /// <param name="result"><ja>�ڑ������������R�l�N�V�����ł��B</ja><en>Connection that connection is completed.</en></param>
        /// <remarks>
        /// <ja>
        /// ���̃��\�b�h���Ăяo���ꂽ��ڑ��͊������Ă��܂��B�ȍ~�A<paramref name="result"/>��ʂ��ăf�[�^�𑗎�M�ł��܂��B
        /// </ja>
        /// <en>
        /// If this method is called, the connection is completed. Data can be sent and received at the following through <paramref name="result"/>. 
        /// </en>
        /// </remarks>
        void SuccessfullyExit(ITerminalConnection result);
        /// <summary>
        /// <ja>
        /// �ڑ������s�����Ƃ��ɌĂяo����܂��B
        /// </ja>
        /// <en>
        /// Called when the connection is failed.
        /// </en>
        /// </summary>
        /// <param name="message"><ja>���s�������郁�b�Z�[�W�ł��B</ja><en>Message to report failure</en></param>
        void ConnectionFailed(string message);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="connection"></param>
    /// <exclude/>
    public delegate void SuccessfullyExitDelegate(ITerminalConnection connection);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <exclude/>
    public delegate void ConnectionFailedDelegate(string message);

    /// <summary>
    /// <ja>
    /// �ȈՓI�ȓ����ڑ��@�\�̂��߂�<see cref="IInterruptableConnectorClient">IInterruptableConnectorClient</see>��񋟂��A
    /// �ڑ��̊����܂��̓G���[�̔����܂��̓^�C���A�E�g�܂ŁA�ڑ�������҂@�\��񋟂��܂��B
    /// </ja>
    /// <en>
    /// <see cref="IInterruptableConnectorClient">IInterruptableConnectorClient</see> for a simple synchronization and the connection functions is offered, and the function to wait for connected completion is offered until generation or the time-out of completion or the error of the connection. 
    /// </en>
    /// </summary>
    /// <remarks>
    /// <ja>
    /// <para>
    /// �J���҂͎��̂悤�ɂ��邱�ƂŁA�ڑ�����������܂ő҂��Ƃ��ł��܂��B
    /// </para>
    /// <code>
    /// // <value>form</value>�̓��[�U�[�ɕ\������t�H�[���ł�
    /// ISynchronizedConnector sc = protocolservice.CreateFormBasedSynchronozedConnector(<value>form</value>);
    /// // <value>sshparam</value>��SSH�ڑ��̃p�����[�^�ł�
    /// IInterrutable t = protocol_service.AsyncSSHConnect(sc.InterruptableConnectorClient, sshparam);
    /// // 30�b�ԑ҂�
    /// int timeout = 30 * 1000;
    /// ITerminalConnection connection = sc.WaitConnection(t, timeout);
    /// </code>
    /// </ja>
    /// <en>
    /// <para>
    /// The developer can wait until the connection is completed by doing as follows. 
    /// </para>
    /// <code>
    /// // <value>form</value> is the form that show to user.
    /// ISynchronizedConnector sc = protocolservice.CreateFormBasedSynchronozedConnector(<value>form</value>);
    /// // <value>sshparam</value> is a parameter for SSH connection.
    /// IInterrutable t = protocol_service.AsyncSSHConnect(sc.InterruptableConnectorClient, sshparam);
    /// // Wait 30second
    /// int timeout = 30 * 1000;
    /// ITerminalConnection connection = sc.WaitConnection(t, timeout);
    /// </code>
    /// </en>
    /// </remarks>
    public interface ISynchronizedConnector {
        /// <summary>
        /// <ja>
        /// �ڑ���҂@�\������<see cref="IInterruptableConnectorClient">IInterruptableConnectorClient</see>��Ԃ��܂��B
        /// </ja>
        /// <en>
        /// <see cref="IInterruptableConnectorClient">IInterruptableConnectorClient</see> that waits for the connection is returned. 
        /// </en>
        /// </summary>
        IInterruptableConnectorClient InterruptableConnectorClient {
            get;
        }
        /// <summary>
        /// <ja>
        /// �ڑ������܂��͐ڑ��G���[�܂��̓^�C���A�E�g����������܂ő҂��܂��B
        /// </ja>
        /// <en>
        /// It waits until connected completion or connected error or the time-out occurs. 
        /// </en>
        /// </summary>
        /// <param name="connector"><ja>�ڑ����~�߂邽�߂̃C���^�[�t�F�C�X</ja><en>Interface to stop connection</en></param>
        /// <param name="timeout"><ja>�^�C���A�E�g�l�i�~���b�j�BSystem.Threading.Timeout.Infinite���w�肵�āA�������ɑ҂��Ƃ��ł��܂��B</ja><en>Time-out value (millisecond). It is possible to wait indefinitely by specifying System.Threading.Timeout.Infinite. </en></param>
        /// <returns><ja>�ڑ�����������<seealso cref="ITerminalConnection">ITerminalConnection</seealso>�B�ڑ��Ɏ��s�����Ƃ��ɂ�null</ja><en><seealso cref="ITerminalConnection">ITerminalConnection</seealso> that completes connection. When failing in the connection, return null. </en></returns>
        /// <remarks>
        /// <para>
        /// <ja>
        /// <paramref name="connector"/>�ɂ́A<seealso cref="IProtocolService">IProtocolService</seealso>��
        /// <see cref="IProtocolService.AsyncCygwinConnect">AsyncCygwinConnect���\�b�h</see>�A
        /// <see cref="IProtocolService.AsyncTelnetConnect">AsyncTelnetConnect���\�b�h</see>�A
        /// <see cref="IProtocolService.AsyncSSHConnect">AsyncSSHConnect���\�b�h</see>
        /// ����̖߂�l��n���܂��B
        /// </ja>
        /// <en>
        /// The return value from the <see cref="IProtocolService.AsyncCygwinConnect">AsyncCygwinConnect method</see>, the <see cref="IProtocolService.AsyncTelnetConnect">AsyncTelnetConnect method</see>, and the <see cref="IProtocolService.AsyncSSHConnect">AsyncSSHConnect method</see> of <seealso cref="IProtocolService">IProtocolService</seealso> is passed to connector. 
        /// </en>
        /// </para>
        /// </remarks>
        ITerminalConnection WaitConnection(IInterruptable connector, int timeout);
    }

    /// <summary>
    /// <ja>
    /// �V�K�ڑ��@�\��񋟂���C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface that offers new connection function.
    /// </en>
    /// </summary>
    /// <remarks>
    /// <ja>
    /// ���̃C���^�[�t�F�C�X�́AProtocols�v���O�C���i�v���O�C��ID�uorg.poderosa.protocols�v�j���񋟂��܂��B���̂悤�ɂ��Ď擾�ł��܂��B
    /// <code>
    /// IProtocolService protocolservice = 
    ///  (IProtocolService)PoderosaWorld.PluginManager.FindPlugin(
    ///   "org.poderosa.protocols", typeof(IProtocolService));
    /// Debug.Assert(protocolservice != null);
    /// </code>
    /// </ja>
    /// <en>
    /// This interface is offered by Protocols plug-in (plug-in ID [org.poderosa.protocols]). It is possible to get it as follows. 
    /// <code>
    /// IProtocolService protocolservice = 
    ///  (IProtocolService)PoderosaWorld.PluginManager.FindPlugin(
    ///   "org.poderosa.protocols", typeof(IProtocolService));
    /// Debug.Assert(protocolservice != null);
    /// </code>
    /// </en>
    /// </remarks>
    public interface IProtocolService {
        /// <summary>
        /// <ja>
        /// Cygwin�ڑ��̃f�t�H���g�p�����[�^���i�[�����I�u�W�F�N�g�𐶐����܂��B
        /// </ja>
        /// <en>
        /// Create the object stored default parameter of Cygwin connection.
        /// </en>
        /// </summary>
        /// <returns><ja>�f�t�H���g�p�����[�^���i�[���ꂽ�I�u�W�F�N�g</ja><en>Object with default parameter.</en></returns>
        ICygwinParameter CreateDefaultCygwinParameter();
        /// <summary>
        /// <ja>
        /// Telnet�ڑ��̃f�t�H���g�p�����[�^���i�[�����I�u�W�F�N�g�𐶐����܂��B
        /// </ja>
        /// <en>
        /// Create the object stored default parameter of telnet connection.
        /// </en>
        /// </summary>
        /// <returns><ja>�f�t�H���g�p�����[�^���i�[���ꂽ�I�u�W�F�N�g</ja>
        /// <en>Object that stored default parameter.</en>
        /// </returns>
        ITCPParameter CreateDefaultTelnetParameter();
        /// <summary>
        /// <ja>
        /// SSH�ڑ��̃f�t�H���g�p�����[�^���i�[�����I�u�W�F�N�g�𐶐����܂��B
        /// </ja>
        /// <en>
        /// Create the object stored default parameter of SSH connection.
        /// </en>
        /// </summary>
        /// <returns><ja>�f�t�H���g�p�����[�^���i�[���ꂽ�I�u�W�F�N�g</ja><en>Object with default parameter.</en></returns>
        ISSHLoginParameter CreateDefaultSSHParameter();

        /// <summary>
        /// <ja>
        /// SSH�ڑ��̃T�u�V�X�e���w��t���p�����[�^���i�[�����I�u�W�F�N�g�𐶐����܂��B
        /// </ja>
        /// <en>
        /// Create the object stored default parameter of SSH connection with subsystem designation.
        /// </en>
        /// </summary>
        /// <returns><ja>�f�t�H���g�p�����[�^���i�[���ꂽ�I�u�W�F�N�g</ja><en>Object with default parameter.</en></returns>
        ISSHSubsystemParameter CreateDefaultSSHSubsystemParameter();

        /// <summary>
        /// <ja>
        /// �񓯊��ڑ���Cygwin�ڑ��̃^�[�~�i���R�l�N�V���������܂��B
        /// </ja>
        /// <en>
        /// The terminal connection of the Cygwin connection is made for an asynchronous connection. 
        /// </en>
        /// </summary>
        /// <param name="result_client"><ja>�ڑ��̐��ۂ��󂯎��C���^�[�t�F�C�X</ja><en>Interface that receives success or failure of connection</en></param>
        /// <param name="destination"><ja>�ڑ����̃p�����[�^</ja><en>Connecting parameter.</en></param>
        /// <returns><ja>�ڑ�������L�����Z�����邽�߂̃C���^�[�t�F�C�X</ja><en>Interface to cancel connected operation</en></returns>
        /// <remarks>
        /// <ja>
        /// ���̃��\�b�h�̓u���b�N�����ɁA�������ɐ����߂��܂��B�ڑ������������<paramref name="result_client"/>��
        /// <see cref="IInterruptableConnectorClient.SuccessfullyExit">SuccessfullyExit���\�b�h</see>���Ăяo����܂��B
        /// </ja>
        /// <en>
        /// The control is returned at once without blocking this method. When the connection succeeds, the <see cref="IInterruptableConnectorClient.SuccessfullyExit">SuccessfullyExit method</see> of <paramref name="result_client"/> is called. 
        /// </en>
        /// </remarks>
        IInterruptable AsyncCygwinConnect(IInterruptableConnectorClient result_client, ICygwinParameter destination);
        /// <summary>
        /// <ja>
        /// �񓯊��ڑ���Telnet�ڑ��̃^�[�~�i���R�l�N�V���������܂��B
        /// </ja>
        /// <en>
        /// Create a terminal connection of the Telnet connection by an asynchronous connection. 
        /// </en>
        /// </summary>
        /// <en>
        /// The terminal connection of the telnet connection is made for an asynchronous connection. 
        /// </en>
        /// <param name="result_client"><ja>�ڑ��̐��ۂ��󂯎��C���^�[�t�F�C�X</ja><en>Interface that receives success or failure of connection</en></param>
        /// <param name="destination"><ja>�ڑ����̃p�����[�^</ja><en>Connecting parameter.</en></param>
        /// <returns><ja>�ڑ�������L�����Z�����邽�߂̃C���^�[�t�F�C�X</ja><en>Interface to cancel connected operation</en></returns>
        /// <remarks>
        /// <ja>
        /// ���̃��\�b�h�̓u���b�N�����ɁA�������ɐ����߂��܂��B�ڑ������������<paramref name="result_client"/>��
        /// <see cref="IInterruptableConnectorClient.SuccessfullyExit">SuccessfullyExit���\�b�h</see>���Ăяo����܂��B
        /// </ja>
        /// <en>
        /// The control is returned at once without blocking this method. When the connection succeeds, the <see cref="IInterruptableConnectorClient.SuccessfullyExit">SuccessfullyExit method</see> of <paramref name="result_client"/> is called. 
        /// </en>
        /// </remarks>
        IInterruptable AsyncTelnetConnect(IInterruptableConnectorClient result_client, ITCPParameter destination);
        /// <summary>
        /// <ja>
        /// �񓯊��ڑ���SSH�ڑ��̃^�[�~�i���R�l�N�V���������܂��B
        /// </ja>
        /// <en>
        /// The terminal connection of the SSH connection is made for an asynchronous connection. 
        /// </en>
        /// </summary>
        /// <param name="result_client"><ja>�ڑ��̐��ۂ��󂯎��C���^�[�t�F�C�X</ja><en>Interface that receives success or failure of connection</en></param>
        /// <param name="destination"><ja>�ڑ����̃p�����[�^</ja><en>Connecting parameter.</en></param>
        /// <returns><ja>�ڑ�������L�����Z�����邽�߂̃C���^�[�t�F�C�X</ja>
        /// <en>Interface to cancel connection operation.</en>
        /// </returns>
        /// <remarks>
        /// <ja>
        /// ���̃��\�b�h�̓u���b�N�����ɁA�������ɐ����߂��܂��B�ڑ������������<paramref name="result_client"/>��
        /// <see cref="IInterruptableConnectorClient.SuccessfullyExit">SuccessfullyExit���\�b�h</see>���Ăяo����܂��B
        /// </ja>
        /// <en>
        /// The control is returned at once without blocking this method. When the connection succeeds, the <see cref="IInterruptableConnectorClient.SuccessfullyExit">SuccessfullyExit method</see> of <paramref name="result_client"/> is called. 
        /// </en>
        /// </remarks>
        IInterruptable AsyncSSHConnect(IInterruptableConnectorClient result_client, ISSHLoginParameter destination);

        /// <summary>
        /// <ja>
        /// �ȈՓI�ȓ����ڑ��@�\��񋟂���C���^�[�t�F�C�X��Ԃ��܂��B
        /// </ja>
        /// <en>
        /// Return the interface that offers a simple synchronization and the connection functions.
        /// </en>
        /// </summary>
        /// <param name="form"><ja>�ڑ����Ƀ��[�U�[�ɕ\������t�H�[��</ja><en>Form displayed to user when connecting it</en></param>
        /// <returns><ja>�����ڑ��@�\��񋟂���C���^�[�t�F�C�X</ja><en>Interface that offers synchronization and connection functions</en></returns>
        ISynchronizedConnector CreateFormBasedSynchronozedConnector(IPoderosaForm form);

        /// <summary>
        /// <ja>
        /// �v���g�R���̃I�v�V�����������C���^�[�t�F�C�X�ł��B
        /// </ja>
        /// <en>
        /// Interface that shows option of protocol
        /// </en>
        /// </summary>
        IProtocolOptions ProtocolOptions {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exclude/>
        IPassphraseCache PassphraseCache {
            get;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public interface IPassphraseCache {
        void Add(string host, string account, string passphrase);
        string GetOrEmpty(string host, string account);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public interface ISSHConnectionChecker {
        //SSH�ڑ��𒣂�Ƃ��ɉ�����邽�߂̃C���^�t�F�[�X�@AgentForwarding�p�ɓ����������̂����g�����邩��
        void BeforeNewConnection(SSHConnectionParameter cp);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public interface IProtocolTestService {
        ITerminalConnection CreateLoopbackConnection();
    }

}
