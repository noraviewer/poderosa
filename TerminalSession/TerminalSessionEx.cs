/*
 * Copyright 2004,2006 The Poderosa Project.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *
 * $Id: TerminalSessionEx.cs,v 1.2 2011/10/27 23:21:59 kzmi Exp $
 */
using System;
using System.Collections.Generic;
using System.Text;

using Poderosa.Commands;
using Poderosa.Protocols;
using Poderosa.Terminal;
using Poderosa.Forms;

namespace Poderosa.Sessions {

    //�^�[�~�i���ڑ��̃Z�b�V����
    //  TerminalEmulator�v���O�C�����̃R�}���h�́ACommandTarget->View->Document->Session->TerminalSession->Terminal�ƒH���ăR�}���h���s�Ώۂ𓾂�B
    /// <summary>
    /// <ja>
    /// �^�[�~�i���Z�b�V�����������C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface that show the terminal session.
    /// </en>
    /// </summary>
    /// <remarks>
    /// <ja>
    /// <para>
    /// Poderosa��W���̃^�[�~�i���G�~�����[�^�Ƃ��ėp����ꍇ�ɂ́A<seealso cref="ISession">ISession</seealso>�̎��Ԃ́A
    /// ����ITerminalSession�ł���AGetAdapter���g���Ď擾�ł��܂��B
    /// </para>
    /// <para>
    /// <para>
    /// �^�[�~�i���Z�b�V�����́A���̕��@�Ŏ擾�ł��܂��B
    /// </para>
    /// <list type="number">
    /// <item>
    /// <term>�A�N�e�B�u�ȃE�B���h�E�^�r���[����擾����ꍇ</term>
    /// <description>
    /// <para>
    /// �E�B���h�E�}�l�[�W����ActiveWindow�v���p�e�B�́A�A�N�e�B�u�E�B���h�E�������܂��B
    /// ���̃A�N�e�B�u�E�B���h�E����h�L�������g�A�����āA�Z�b�V�����ւƂ��ǂ邱�ƂŃ^�[�~�i���Z�b�V�������擾�ł��܂��B 
    /// </para>
    /// <code>
    /// // �E�B���h�E�}�l�[�W�����擾
    /// ICoreServices cs = (ICoreServices)PoderosaWorld.GetAdapter(typeof(ICoreServices));
    /// IWindowManager wm = cs.WindowManager;
    ///
    /// // �A�N�e�B�u�E�B���h�E���擾
    /// IPoderosaMainWindow window = wm.ActiveWindow;
    ///
    /// // �r���[���擾
    /// IPoderosaView view = window.LastActivatedView;
    /// 
    /// // �h�L�������g���擾
    /// IPoderosaDocument doc = view.Document;
    /// 
    /// // �Z�b�V�������擾
    /// ISession session = doc.OwnerSession;
    /// 
    /// // �^�[�~�i���Z�b�V�����ւƕϊ�
    /// ITerminalSession termsession = 
    ///   (ITerminalSession)session.GetAdapter(typeof(ITerminalSession));
    /// </code>
    /// </description>
    /// </item>
    /// <item><term>���j���[��c�[���o�[�̃^�[�Q�b�g����^�[�~�i���Z�b�V�����𓾂�ꍇ</term>
    /// <description>
    /// <para>
    /// ���j���[��c�[���o�[����R�}���h�������n�����Ƃ��ɂ́A�^�[�Q�b�g�Ƃ��đ���Ώۂ̃E�B���h�E�������܂��B
    /// ���̃^�[�Q�b�g�𗘗p���ă^�[�~�i���Z�b�V�����𓾂邱�Ƃ��ł��܂��B 
    /// </para>
    /// <para>
    /// <seealso cref="CommandTargetUtil">CommandTargetUtil</seealso>�ɂ́A�A�N�e�B�u�ȃh�L�������g�𓾂邽�߂�AsDocumentOrViewOrLastActivatedDocument���\�b�h������܂��B
    /// ���̃��\�b�h���g���ăh�L�������g���擾���A��������ITerminalSession�ւƕϊ����邱�ƂŁA�^�[�Q�b�g�ɂȂ��Ă���^�[�~�i���Z�b�V�������擾�ł��܂��B 
    /// </para>
    /// <code>
    /// // target�̓R�}���h�ɓn���ꂽ�^�[�Q�b�g�ł���Ƒz�肵�܂�
    /// // �h�L�������g���擾
    /// IPoderosaDocument doc = 
    ///   CommandTargetUtil.AsDocumentOrViewOrLastActivatedDocument(target);
    /// if (doc != null)
    /// {
    ///   // �Z�b�V�������擾
    ///   ISession session = doc.OwnerSession;
    ///   // �^�[�~�i���Z�b�V�����ւƕϊ�
    ///   ITerminalSession termsession = 
    ///     (ITerminalSession)session.GetAdapter(typeof(ITerminalSession));
    /// }
    /// </code>
    /// </description>
    /// </item>
    /// </list>
    /// </para>
    /// </ja>
    /// <en>
    /// <para>
    /// The realities of <seealso cref="ISession">ISession</seealso> are these ITerminalSession when Poderosa is used as a standard terminal emulator, and it is possible to acquire it by using GetAdapter. 
    /// </para>
    /// <para>
    /// <para>
    /// The terminal session can be got in the following method. 
    /// </para>
    /// <list type="number">
    /// <item>
    /// <term>Get from active window or view.</term>
    /// <description>
    /// <para>
    /// Window manager's ActiveWindow property shows the active window. The terminal session can be got by tracing it from this active window to the document and the session. 
    /// </para>
    /// <code>
    /// // Get the window manager.
    /// ICoreServices cs = (ICoreServices)PoderosaWorld.GetAdapter(typeof(ICoreServices));
    /// IWindowManager wm = cs.WindowManager;
    ///
    /// // Get the active window.
    /// IPoderosaMainWindow window = wm.ActiveWindow;
    ///
    /// // Get the view.
    /// IPoderosaView view = window.LastActivatedView;
    /// 
    /// // Get the document.
    /// IPoderosaDocument doc = view.Document;
    /// 
    /// // Get the session
    /// ISession session = doc.OwnerSession;
    /// 
    /// // Convert to terminal session.
    /// ITerminalSession termsession = 
    ///   (ITerminalSession)session.GetAdapter(typeof(ITerminalSession));
    /// </code>
    /// </description>
    /// </item>
    /// <item><term>Get the terminal session from the target of menu or toolbar.</term>
    /// <description>
    /// <para>
    /// When the command is handed over from the menu and the toolbar, the window to be operated as a target is obtained. 
    /// The terminal session can be obtained by using this target. 
    /// </para>
    /// <para>
    /// In <seealso cref="CommandTargetUtil">CommandTargetUtil</seealso>, there is AsDocumentOrViewOrLastActivatedDocument method to obtain an active document. 
    /// </para>
    /// <code>
    /// // It is assumed that target is a target passed to the command. 
    /// // Get the document.
    /// IPoderosaDocument doc = 
    ///   CommandTargetUtil.AsDocumentOrViewOrLastActivatedDocument(target);
    /// if (doc != null)
    /// {
    ///   // Get the session.
    ///   ISession session = doc.OwnerSession;
    ///   // Convert to terminal session.
    ///   ITerminalSession termsession = 
    ///     (ITerminalSession)session.GetAdapter(typeof(ITerminalSession));
    /// }
    /// </code>
    /// </description>
    /// </item>
    /// </list>
    /// </para>
    /// </en>
    /// </remarks>
    public interface ITerminalSession : ISession {
        /// <summary>
        /// <ja>�^�[�~�i�����Ǘ�����I�u�W�F�N�g�ł��B</ja>
        /// <en>Object that manages terminal.</en>
        /// </summary>
        /// <remarks>
        /// <ja>���̃I�u�W�F�N�g�́A����M���t�b�N�������ꍇ�⃍�O���Ƃ肽���ꍇ�Ȃǂɗp���܂��B</ja><en>This object uses transmitting and receiving to hook and to take the log. </en>
        /// </remarks>
        AbstractTerminal Terminal {
            get;
        }
        /// <summary>
        /// <ja>
        /// �^�[�~�i���̃��[�U�[�C���^�[�t�F�C�X��񋟂���R���g���[���ł��B
        /// </ja>
        /// <en>
        /// Control that offers user interface of terminal.
        /// </en>
        /// </summary>
        TerminalControl TerminalControl {
            get;
        }
        /// <summary>
        /// <ja>�^�[�~�i���ݒ�������I�u�W�F�N�g�ł��B</ja>
        /// <en>Object that shows terminal setting.</en>
        /// </summary>
        ITerminalSettings TerminalSettings {
            get;
        }
        /// <summary>
        /// <ja>�^�[�~�i���̐ڑ��������I�u�W�F�N�g�ł��B</ja>
        /// <en>Object that shows connection of terminal.</en>
        /// </summary>
        ITerminalConnection TerminalConnection {
            get;
        }
        /// <summary>
        /// <ja>���L����E�B���h�E�������܂��B</ja>
        /// <en>The owned window is shown. </en>
        /// </summary>
        IPoderosaMainWindow OwnerWindow {
            get;
        }
        /// <summary>
        /// <ja>�^�[�~�i���ւ̑��M�@�\��񋟂��܂��B</ja>
        /// <en>The transmission function to the terminal is offered. </en>
        /// </summary>
        TerminalTransmission TerminalTransmission {
            get;
        }
    }


    /// <summary>
    /// <ja>
    /// �^�[�~�i���T�[�r�X��񋟂���C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface that provides terminal service.
    /// </en>
    /// </summary>
    /// <remarks>
    /// <ja>
    /// <para>
    /// ���̃C���^�[�t�F�C�X�́A�V�KTelnet�^SSH�^Cygwin�ڑ��̋@�\��񋟂��܂��B
    /// </para>
    /// <para>
    /// TerminalSessionPlugin�v���O�C���i�v���O�C��ID�F�uorg.poderosa.terminalsessions�v�j�ɂ����
    /// �񋟂���Ă���A���̂悤�ɂ��Ď擾�ł��܂��B
    /// </para>
    /// <code>
    /// ITerminalSessionsService termservice = 
    ///  (ITerminalSessionsService)PoderosaWorld.PluginManager.FindPlugin(
    ///     "org.poderosa.terminalsessions", typeof(ITerminalSessionsService));
    /// Debug.Assert(termservice != null);
    /// </code>
    /// </ja>
    /// <en>
    /// <para>
    /// This interface offers the function of a new Telnet/SSH/Cygwin connection. 
    /// </para>
    /// <para>
    /// It is offered by the TerminalSessionPlugin plug-in (plugin ID[org.poderosa.terminalsessions]) , and it is possible to get it as follows. 
    /// </para>
    /// <code>
    /// ITerminalSessionsService termservice = 
    ///  (ITerminalSessionsService)PoderosaWorld.PluginManager.FindPlugin(
    ///     "org.poderosa.terminalsessions", typeof(ITerminalSessionsService));
    /// Debug.Assert(termservice != null);
    /// </code>
    /// </en>
    /// </remarks>
    public interface ITerminalSessionsService : IAdaptable {
        /// <summary>
        /// <ja>
        /// �V�K�^�[�~�i���ڑ������邽�߂̃C���^�[�t�F�C�X�������܂��B
        /// </ja>
        /// <en>
        /// The interface to connect a new terminal is shown. 
        /// </en>
        /// </summary>
        ITerminalSessionStartCommand TerminalSessionStartCommand {
            get;
        }
        /// <summary>
        /// <ja>
        /// �ڑ��R�}���h�̃J�e�S���������܂��B
        /// </ja>
        /// <en>
        /// The category of connected command is shown. 
        /// </en>
        /// </summary>
        ICommandCategory ConnectCommandCategory {
            get;
        }
    }

    /// <summary>
    /// <ja>
    /// �V�K�^�[�~�i���̐ڑ��@�\��񋟂���C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface that offers connected function of new terminal.
    /// </en>
    /// </summary>
    /// <remarks>
    /// <ja>���̃C���^�[�t�F�C�X�́A<seealso cref="Poderosa.Sessions.ITerminalSessionsService">ITerminalSessionsServicen</seealso>��
    /// <see cref="Poderosa.Sessions.ITerminalSessionsService.TerminalSessionStartCommand">TerminalSessionStartCommand�v���p�e�B</see>
    /// ����擾�ł��܂��B</ja>
    /// <en>This interface can be got from the <see cref="Poderosa.Sessions.ITerminalSessionsService.TerminalSessionStartCommand">TerminalSessionStartCommand property</see> of ITerminalSessionsServicen. </en>
    /// </remarks>
    public interface ITerminalSessionStartCommand : IPoderosaCommand {
        /// <summary>
        /// <ja>�����̐ڑ���p���ĐV�K�^�[�~�i���Z�b�V�������J�n���܂��B</ja><en>A new terminal session is begun by using an existing connection. </en>
        /// </summary>
        /// <param name="target"><ja>�^�[�~�i���Ɍ��т���r���[�܂��̓E�B���h�E</ja><en>View or window that ties to terminal</en></param>
        /// <param name="existing_connection"><ja>�����̐ڑ��I�u�W�F�N�g</ja><en>Existing connected object</en></param>
        /// <param name="settings"><ja>�^�[�~�i���ݒ肪�i�[���ꂽ�I�u�W�F�N�g</ja><en>Object where terminal setting is stored</en></param>
        /// <returns><ja>�J�n���ꂽ�^�[�~�i���Z�b�V�������Ԃ���܂�</ja><en>The begun terminal session is returned. </en></returns>
        /// <overloads>
        /// <summary>
        /// <ja>�V�K�^�[�~�i���ڑ����J�n���܂��B</ja><en>Start a new terminal session.</en>
        /// </summary>
        /// </overloads>
        ITerminalSession StartTerminalSession(ICommandTarget target, ITerminalConnection existing_connection, ITerminalSettings settings);
        //ITerminalParameter�́ATelnet/SSH/Cygwin�̂����ꂩ�ł���K�v������B
        /// <summary>
        /// <ja>
        /// �ڑ��p�����[�^��p���ĐV�K�ڑ������A���̃^�[�~�i���Z�b�V�������J�n���܂��B
        /// </ja>
        /// <en>
        /// It newly connects by using connected parameter, and the terminal session is begun. 
        /// </en>
        /// </summary>
        /// <param name="target"><ja>�^�[�~�i���Ɍ��т���r���[�܂��̓E�B���h�E</ja><en>View or window that ties to terminal</en></param>
        /// <param name="destination">
        /// <ja>�ڑ����̃p�����[�^���i�[���ꂽ�I�u�W�F�N�g�B
        /// <seealso cref="ICygwinParameter">ICygwinParameter</seealso>�A
        /// <seealso cref="ISSHLoginParameter">ISSHLoginParameter</seealso>�A
        /// <seealso cref="ITCPParameter">ITCPParameter</seealso>�̂����ꂩ�łȂ���΂Ȃ�܂���B</ja>
        /// <en>
        /// Object where parameter when connecting it is stored. 
        /// It should be either <seealso cref="ICygwinParameter">ICygwinParameter</seealso>, <seealso cref="ISSHLoginParameter">ISSHLoginParameter</seealso> or <seealso cref="ITCPParameter">ITCPParameter</seealso>. </en>
        /// </param>
        /// <param name="settings"><ja>�^�[�~�i���ݒ肪�i�[���ꂽ�I�u�W�F�N�g</ja><en>Object where terminal setting is stored</en></param>
        /// <returns><ja>�J�n���ꂽ�^�[�~�i���Z�b�V�������Ԃ���܂�</ja><en>The begun terminal session is returned. </en></returns>
        ITerminalSession StartTerminalSession(ICommandTarget target, ITerminalParameter destination, ITerminalSettings settings);

        /// <summary>
        /// <ja>�Z�b�V�����Ƃ͖��֌W�ɐڑ������J���܂�</ja>
        /// <en>Opens not any session but connection</en>
        /// </summary>
        /// <exclude/>
        ITerminalConnection OpenConnection(IPoderosaMainWindow window, ITerminalParameter destination, ITerminalSettings settings);

        void OpenShortcutFile(ICommandTarget target, string filename);
    }

    //ITerminalParameter���C���X�^���V�G�[�g����ITerminalConnection�ɂ���ExtensionPoint�̃C���^�t�F�[�X
    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public interface ITerminalConnectionFactory {
        bool IsSupporting(ITerminalParameter param, ITerminalSettings settings);
        ITerminalConnection EstablishConnection(IPoderosaMainWindow window, ITerminalParameter param, ITerminalSettings settings);
    }


    //���O�C���_�C�A���O�̎g���������p�̃T�|�[�g
    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public interface ITelnetSSHLoginDialogInitializeInfo : IAdaptable {
        //�ڑ�����
        void AddHost(string value);
        void AddAccount(string value);
        void AddIdentityFile(string value);
        void AddPort(int value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public interface ITelnetSSHLoginDialogInitializer {
        void ApplyLoginDialogInfo(ITelnetSSHLoginDialogInitializeInfo info);
    }

    //Extension Point����
    //���Ɋi�[����Ă�����͉󂳂Ȃ��悤�ɂ���̂����[��
    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public interface ILoginDialogUISupport {
        //�Q�̖߂�l������̂�out���g���Badapter�́ATerminalParameter�̎�ނ��w�肷�邽�߂̈����B�Ή�������̂��Ȃ��Ƃ���null��Ԃ�
        void FillTopDestination(Type adapter, out ITerminalParameter parameter, out ITerminalSettings settings);
        //�z�X�g���Ŏw�肷��
        void FillCorrespondingDestination(Type adapter, string destination, out ITerminalParameter parameter, out ITerminalSettings settings);
    }

    //Terminal Session�ŗL�I�v�V����
    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public interface ITerminalSessionOptions {
        bool AskCloseOnExit {
            get;
            set;
        }

        //preference editor only
        int TerminalEstablishTimeout {
            get;
        }
        string GetDefaultLoginDialogUISupportTypeName(string logintype);
    }

}
