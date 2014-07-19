/*
 * Copyright 2004,2006 The Poderosa Project.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *
 * $Id: SessionEx.cs,v 1.2 2011/10/27 23:21:55 kzmi Exp $
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using Poderosa.Forms;
using Poderosa.Document;
using Poderosa.Commands;

namespace Poderosa.Sessions {
    /// <summary>
    /// <ja>
    /// �Z�b�V�����}�l�[�W���������C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface that shows session manager.
    /// </en>
    /// </summary>
    /// <remarks>
    /// <ja>
    /// <para>
    /// ���̃C���^�[�t�F�C�X�̓Z�b�V�����}�l�[�W���iSessionManagerPlugin�v���O�C���F�v���O�C��ID�uorg.poderosa.core.sessions�v�j
    /// �ɂ���Ē񋟂����C���^�[�t�F�C�X�ł���A�Z�b�V�������𑀍삵�܂��B
    /// </para>
    /// <para>
    /// ���̃C���^�[�t�F�C�X�́A<seealso cref="Poderosa.Plugins.ICoreServices">ICoreServices</seealso>��
    /// <see cref="Poderosa.Plugins.ICoreServices.SessionManager">SessionManager�v���p�e�B</see>
    /// ���g���Ď擾�ł��܂��B
    /// </para>
    /// </ja>
    /// <en>
    /// <para>
    /// This interface is an interface offered by the session manager (SessionManagerPlugin plug-in : Plug-inID "org.poderosa.core.sessions") , and session information is operated. 
    /// </para>
    /// <para>
    /// This interface can be acquired by using the <see cref="Poderosa.Plugins.ICoreServices.SessionManager">SessionManager property</see> of <seealso cref="Poderosa.Plugins.ICoreServices">ICoreServices</seealso>. 
    /// </para>
    /// </en>
    /// </remarks>
    /// <example>
    /// <ja>
    /// ISessionManager���擾���܂��B
    /// <code>
    /// // ICoreServices���擾
    /// ICoreServices cs = (ICoreServices)PoderosaWorld.GetAdapter(typeof(ICoreServices));
    /// // ISessionManager���擾
    /// ISessionManager sessionman = cs.SessionManager;
    /// </code>
    /// </ja>
    /// <en>
    /// Get the ISessionManager.
    /// <code>
    /// // Get the ICoreServices.
    /// ICoreServices cs = (ICoreServices)PoderosaWorld.GetAdapter(typeof(ICoreServices));
    /// // Get the ISessionManager.
    /// ISessionManager sessionman = cs.SessionManager;
    /// </code>
    /// </en>
    /// </example>
    public interface ISessionManager : IAdaptable {
        //Structure
        /// <summary>
        /// <ja>
        /// ���ׂẴZ�b�V������񋓂��܂��B
        /// </ja>
        /// <en>
        /// Enumerate all sessions.
        /// </en>
        /// </summary>
        IEnumerable<ISession> AllSessions {
            get;
        }
        /// <summary>
        /// <ja>
        /// �E�B���h�E�Ɍ��т���ꂽ�h�L�������g��z��Ƃ��ē��܂��B
        /// </ja>
        /// <en>
        /// The document tie to the window is obtained as an array. 
        /// </en>
        /// </summary>
        /// <param name="window"><ja>�ΏۂƂȂ�E�B���h�E�ł��B</ja><en>It is a window that becomes an object. </en></param>
        /// <returns><ja>�E�B���h�E�Ɋ܂܂��h�L�������g�̔z�񂪕Ԃ���܂��B</ja><en>The array of the document included in the window is returned. </en></returns>
        IPoderosaDocument[] GetDocuments(IPoderosaMainWindow window);

        //Start/End
        /// <summary>
        /// <ja>
        /// �V�����Z�b�V�������J�n���܂��B
        /// </ja>
        /// <en>
        /// Start a new session.
        /// </en>
        /// </summary>
        /// <param name="session"><ja>�J�n����Z�b�V����</ja><en>Session to start.</en></param>
        /// <param name="firstView"><ja>�Z�b�V�����Ɋ��蓖�Ă�r���[</ja><en>View allocated in session</en></param>
        /// <remarks>
        /// <ja>
        /// �V�����Z�b�V�������쐬���邽�߂̃r���[�́A<seealso cref="IViewManager">IViewManager</seealso>��
        /// <see cref="IViewManager.GetCandidateViewForNewDocument">GetCandidateViewForNewDocument���\�b�h</see>
        /// �ō�邱�Ƃ��ł��܂��B
        /// </ja>
        /// <en>
        /// The view to make the session newly can be made by the <see cref="IViewManager.GetCandidateViewForNewDocument">GetCandidateViewForNewDocument method</see> of IViewManager. 
        /// </en>
        /// </remarks>
        void StartNewSession(ISession session, IPoderosaView firstView);

        /// <summary>
        /// <ja>
        /// �Z�b�V��������܂��B
        /// </ja>
        /// <en>
        /// Close the session.
        /// </en>
        /// </summary>
        /// <param name="session"><ja>�������Z�b�V�����ł��B</ja><en>Session to close.</en></param>
        /// <returns><ja>�Z�b�V����������ꂽ���ǂ����������l�ł��B</ja><en>It is a value in which it is shown whether the session was closed. </en></returns>
        /// <remarks>
        /// <ja>
        /// <para>
        /// ���̃��\�b�h���Ăяo���ƁA�Z�b�V�������\������<seealso cref="ISession">ISession</seealso>
        /// ��<see cref="ISession.PrepareCloseSession">PrepareCloseSession���\�b�h</see>
        /// ���Ăяo����܂��B<see cref="ISession.PrepareCloseSession">PrepareCloseSession���\�b�h</see>��PrepareCloseResult.Cancel
        /// ��Ԃ����Ƃ��ɂ́A�Z�b�V��������铮��͒��~����܂��B
        /// </para>
        /// </ja>
        /// <en>
        /// <para>
        /// When this method is called, the <see cref="ISession.PrepareCloseSession">PrepareCloseSession method</see> of <seealso cref="ISession">ISession</seealso> that composes the session is called. When the <see cref="ISession.PrepareCloseSession">PrepareCloseSession method</see> returns PrepareCloseResult.Cancel, operation that shuts the session is discontinued. 
        /// </para>
        /// </en>
        /// </remarks>
        PrepareCloseResult TerminateSession(ISession session);
        /// <summary>
        /// <ja>
        /// �h�L�������g����܂��B
        /// </ja>
        /// <en>
        /// Close the document.
        /// </en>
        /// </summary>
        /// <param name="document"><ja>�������h�L�������g�ł��B</ja><en>Document to close.</en></param>
        /// <returns><ja>�h�L�������g������ꂽ���ǂ����������l�ł��B</ja><en>It is a value in which it is shown whether the document was closed. </en></returns>
        /// <remarks>
        /// <ja>
        /// <para>
        /// ���̃��\�b�h���Ăяo���ƁA�Z�b�V�������\������<seealso cref="ISession">ISession</seealso>
        /// ��<see cref="ISession.PrepareCloseDocument">PrepareCloseDocument���\�b�h</see>
        /// ���Ăяo����܂��B<see cref="ISession.PrepareCloseDocument">PrepareCloseDocument���\�b�h</see>��PrepareCloseResult.Cancel
        /// ��Ԃ����Ƃ��ɂ́A�h�L�������g����铮��͒��~����܂��B
        /// </para>
        /// </ja>
        /// <en>
        /// <para>
        /// When this method is called, the <see cref="ISession.PrepareCloseDocument">PrepareCloseDocument method</see> of 
        /// <seealso cref="ISession">ISession</seealso> that composes the session is called. 
        /// When the <see cref="ISession.PrepareCloseDocument">PrepareCloseDocument method</see> 
        /// returns PrepareCloseResult.Cancel, operation that shuts the document is discontinued. 
        /// </para>
        /// </en>
        /// </remarks>
        PrepareCloseResult CloseDocument(IPoderosaDocument document);

        //Document Management
        /// <summary>
        /// <ja>�h�L�������g���A�N�e�B�u�����܂��B</ja><en>Activate the document.</en>
        /// </summary>
        /// <param name="document"><ja>�A�N�e�B�u������h�L�������g</ja><en>Document to activate.</en></param>
        /// <param name="reason"><ja>�A�N�e�B�u�����闝�R���i�[�����I�u�W�F�N�g</ja><en>Object that stored reason made active</en></param>
        void ActivateDocument(IPoderosaDocument document, ActivateReason reason);
        /// <summary>
        /// <ja>�h�L�������g�ƃr���[�Ƃ����т��܂��B</ja><en>Tie the document and the view.</en>
        /// </summary>
        /// <param name="document"><ja>�ΏۂƂȂ�h�L�������g</ja>
        /// <en>Document to target.</en>
        /// </param>
        /// <param name="view"><ja>���蓖�Ă�r���[</ja>
        /// <en>View to assign.</en>
        /// </param>
        void AttachDocumentAndView(IPoderosaDocument document, IPoderosaView view);
        /// <summary>
        /// <ja>
        /// �h�L�������g�̃X�e�[�^�X���X�V���܂��B
        /// </ja>
        /// <en>
        /// Update the status of the document.
        /// </en>
        /// </summary>
        /// <param name="document"><ja>�X�V����h�L�������g</ja><en>Document to update.</en></param>
        void RefreshDocumentStatus(IPoderosaDocument document);

        //Listener
        /// <summary>
        /// <ja>
        /// �A�N�e�B�u�ȃh�L�������g���ω������Ƃ��̒ʒm���󂯎�郊�X�i��o�^���܂��B
        /// </ja>
        /// <en>
        /// The listener that receives the notification when an active document is changed is registered. 
        /// </en>
        /// </summary>
        /// <param name="listener"><ja>�o�^���郊�X�i</ja><en>Registered listener</en></param>
        void AddActiveDocumentChangeListener(IActiveDocumentChangeListener listener);
        /// <summary>
        /// <ja>
        /// �A�N�e�B�u�ȃh�L�������g���ω������Ƃ��̒ʒm���󂯎�郊�X�i���������܂��B
        /// </ja>
        /// <en>
        /// The listener that receives the notification when an active document is changed is released. 
        /// </en>
        /// </summary>
        /// <param name="listener"><ja>�������郊�X�i</ja><en>Listener to release.</en></param>
        void RemoveActiveDocumentChangeListener(IActiveDocumentChangeListener listener);
        /// <summary>
        /// <ja>
        /// �Z�b�V�������J�n���ꂽ��ؒf���ꂽ�Ƃ��̒ʒm���󂯎�郊�X�i��o�^���܂��B
        /// </ja>
        /// <en>
        /// The listener that is begun the session and receives the notification when close is registered. 
        /// </en>
        /// </summary>
        /// <param name="listener"><ja>�o�^���郊�X�i</ja><en>Listener to regist</en></param>
        void AddSessionListener(ISessionListener listener);
        /// <summary>
        /// <ja>�Z�b�V�������J�n���ꂽ��ؒf���ꂽ�Ƃ��̒ʒm���󂯎�郊�X�i���������܂��B</ja>
        /// <en>The listener that is begun the session and receives the notification when close is released.</en>
        /// </summary>
        /// <param name="listener"><ja>�������郊�X�i</ja><en>Listener to release.</en></param>
        void RemoveSessionListener(ISessionListener listener);
    }

    /// <summary>
    /// <ja>
    /// �Z�b�V�����z�X�g�I�u�W�F�N�g�������C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface that shows session host object.
    /// </en>
    /// </summary>
    public interface ISessionHost {
        /// <summary>
        /// <ja>
        /// �h�L�������g��o�^���܂��B
        /// </ja>
        /// <en>
        /// Regist the document.
        /// </en>
        /// </summary>
        /// <param name="document"><ja>�o�^����h�L�������g</ja><en>Document to regist.</en></param>
        void RegisterDocument(IPoderosaDocument document);
        //TODO RemoveDocument�����Ă悢
        /// <summary>
        /// <ja>
        /// �Z�b�V�������v���O�C��������I�������܂��B
        /// </ja>
        /// <en>
        /// Terminate the sessoin from plug-in side.
        /// </en>
        /// </summary>
        void TerminateSession();
        /// <summary>
        /// <ja>
        /// �h�L�������g�Ɍ��т���ꂽ�t�H�[���𓾂܂��B
        /// </ja>
        /// <en>
        /// Get the form tied with document.
        /// </en>
        /// </summary>
        /// <param name="document"><ja>�ΏۂƂȂ�h�L�������g�ł��B</ja><en>Targeted document.</en></param>
        /// <returns><ja>�h�L�������g�Ɍ��т���ꂽ�t�H�[�����Ԃ���܂��B</ja><en>The form tie to the document is returned. </en></returns>
        IPoderosaForm GetParentFormFor(IPoderosaDocument document);
    }

    //�A�N�e�B�u�ɂ��鑀��̊J�n����
    /// <summary>
    /// <ja>
    /// �h�L�������g���A�N�e�B�u�ɂȂ����Ƃ��̗��R�������܂��B
    /// </ja>
    /// <en>
    /// The reason when the document becomes active is shown. 
    /// </en>
    /// </summary>
    public enum ActivateReason {
        /// <summary>
        /// <ja>��������ɂ��A�N�e�B�u�ɂȂ���</ja>
        /// <en>It became active by internal operation. </en>
        /// </summary>
        InternalAction,
        /// <summary>
        /// <ja>�^�u�N���b�N�ɂ��A�N�e�B�u�ɂȂ���</ja>
        /// <en>It became active by the tab click. </en>
        /// </summary>
        TabClick,
        /// <summary>
        /// <ja>�r���[���t�H�[�J�X���󂯎�������߂ɃA�N�e�B�u�ɂȂ���</ja>
        /// <en>Because the view had got focus, it became active. </en>
        /// </summary>
        ViewGotFocus,
        /// <summary>
        /// <ja>�h���b�O���h���b�v����ɂ��A�N�e�B�u�ɂȂ���</ja>
        /// <en>It became active by the drag &amp; drop operation. </en>
        /// </summary>
        DragDrop
    }

    /// <summary>
    /// <ja>
    /// �h�L�������g��Z�b�V�����������悤�Ƃ���Ƃ��̖߂�l�������܂��B
    /// </ja>
    /// <en>
    /// The return value when the document and the session start being shut is shown. 
    /// </en>
    /// </summary>
    /// <remarks>
    /// <ja>
    /// <para>
    /// ���̗񋓑̂́A<seealso cref="ISession">ISession</seealso>��<see cref="ISession.PrepareCloseDocument">PrepareCloseDocument���\�b�h</see>
    /// ��<see cref="ISession.PrepareCloseSession">PrepareCloseSession���\�b�h</see>�̖߂�l�Ƃ��Ďg���܂��B
    /// </para>
    /// <para>
    /// PrepareCloseResult.ContinueSession���g����̂́A<see cref="ISession.PrepareCloseDocument">PrepareCloseDocument���\�b�h</see>�̂Ƃ������ł��B
    /// </para>
    /// </ja>
    /// <en>
    /// <para>
    /// This enumeration is used as a return value of the <see cref="ISession.PrepareCloseDocument">PrepareCloseDocument method</see> and the <see cref="ISession.PrepareCloseSession">PrepareCloseSession method</see> of <seealso cref="ISession">ISession</seealso>. 
    /// </para>
    /// <para>
    /// Only PrepareCloseResult.ContinueSession is used on <see cref="ISession.PrepareCloseDocument">PrepareCloseDocument method</see>
    /// </para>
    /// </en>
    /// </remarks>
    public enum PrepareCloseResult {
        /// <summary>
        /// <ja>
        /// �h�L�������g�͕��܂����A�Z�b�V�����͕��܂���B
        /// </ja>
        /// <en>
        /// Close the document, but session is not close.
        /// </en>
        /// </summary>
        ContinueSession,
        /// <summary>
        /// <ja>
        /// �h�L�������g��Z�b�V��������܂��B
        /// </ja>
        /// <en>
        /// Close the document and the session.
        /// </en>
        /// </summary>
        TerminateSession,
        /// <summary>
        /// <ja>
        /// �h�L�������g��Z�b�V��������铮����L�����Z�����܂��B
        /// </ja>
        /// <en>
        /// Cancel closing the document and the session.
        /// </en>
        /// </summary>
        Cancel
    }

    /// <summary>
    /// <ja>
    /// �Z�b�V�����������܂��B
    /// </ja>
    /// <en>
    /// The session is shown. 
    /// </en>
    /// </summary>
    /// <remarks>
    /// <ja>
    /// �W���̃^�[�~�i���G�~�����[�^�Ƃ��ėp����ꍇ�AISession�̎��Ԃ́AISession����p�����Ă���
    /// <seealso cref="Poderosa.Sessions.ITerminalSession">ITerminalSession</seealso>�ł���AGetAdapter���\�b�h�ŕϊ��ł��܂��B
    /// </ja>
    /// <en>
    /// The realities of ISession are <seealso cref="Poderosa.Sessions.ITerminalSession">ITerminalSession</seealso> that has been succeeded to, 
    /// and can be converted from ISession by the GetAdapter method when using it as a standard terminal emulator. 
    /// </en>
    /// </remarks>
    public interface ISession : IAdaptable {
        /// <summary>
        /// <ja>
        /// �Z�b�V�����̃L���v�V�����ł��B
        /// </ja>
        /// <en>
        /// Caption of the session.
        /// </en>
        /// </summary>
        string Caption {
            get;
        }
        /// <summary>
        /// <ja>
        /// �Z�b�V�����̃A�C�R���ł��B
        /// </ja>
        /// <en>
        /// Icon of the session.
        /// </en>
        /// </summary>
        Image Icon {
            get;
        } //16*16

        //�ȉ���SessionManager���ĂԁB����ȊO�ł͌Ă�ł͂����Ȃ�
        /// <summary>
        /// <ja>
        /// �Z�b�V�����}�l�[�W������Ăяo����鏉�����̃��\�b�h�ł��B
        /// </ja>
        /// <en>
        /// Initialization called from session manager method.
        /// </en>
        /// </summary>
        /// <param name="host"><ja>�Z�b�V�����𑀍삷�邽�߂̃Z�b�V�����z�X�g�I�u�W�F�N�g�ł��B</ja>
        /// <en>Session host object to operate session.</en>
        /// </param>
        /// <remarks>
        /// <ja>
        /// <para>
        /// ���̃��\�b�h�́A<seealso cref="ISessionManager">ISessionManager</seealso>��<see cref="ISessionManager.StartNewSession">StartNewSession���\�b�h</see>
        /// ���Ăяo���ꂽ�Ƃ��ɁA�Z�b�V�����}�l�[�W���ɂ���ĊԐړI�ɌĂяo����܂��B
        /// �J���҂́A���̃��\�b�h�𒼐ڌĂяo���Ă͂����܂���B
        /// </para>
        /// <para>
        /// �J���҂͈�ʂɁA���̃��\�b�h�̏����ɂ����ăh�L�������g���쐬���A<paramref name="host">host</paramref>�Ƃ��ēn���ꂽ<seealso cref="ISessionHost">ISessionHost</seealso>
        /// ��<see cref="ISessionHost.RegisterDocument">RegisterDocument���\�b�h</see>���Ăяo���ăh�L�������g��o�^���܂��B
        /// </para>
        /// <para>
        /// �Z�b�V�����̏ڍׂɂ��ẮA<see href="chap04_02_04.html">�Z�b�V�����̑���</see>���Q�Ƃ��Ă��������B
        /// </para>
        /// </ja>
        /// <en>
        /// <para>
        /// When the <see cref="ISessionManager.StartNewSession">StartNewSession method</see> of 
        /// <seealso cref="ISessionManager">ISessionManager</seealso> is called, this method is 
        /// indirectly called by the session manager. The developer must not call this method directly. 
        /// </para>
        /// <para>
        /// The developer makes the document in general in the processing of this method, calls the <see cref="ISessionHost.RegisterDocument">RegisterDocument method</see> of <seealso cref="ISessionHost">ISessionHost</seealso> passed as <paramref name="host">host</paramref>, and registers the document. 
        /// </para>
        /// <para>
        /// Please refer to <see href="chap04_02_04.html">Operation of session</see> for details of the session. 
        /// </para>
        /// </en>
        /// </remarks>
        void InternalStart(ISessionHost host);

        /// <summary>
        /// <ja>
        /// �Z�b�V�������I�������Ƃ��ɌĂяo����郁�\�b�h�ł��B
        /// </ja>
        /// <en>
        /// It is a method of the call when the session ends. 
        /// </en>
        /// </summary>
        void InternalTerminate();

        //Session����Terminate���w�肵���ꍇ�ł��A�K��Terminate�����Ƃ͌���Ȃ��_�ɒ��ӁB
        /// <summary>
        /// <ja>
        /// �h�L�������g����Ă��悢�������肵�܂��B
        /// </ja>
        /// <en>
        /// It is decided whether I may close the document. 
        /// </en>
        /// </summary>
        /// <param name="document"><ja>����ΏۂƂȂ�h�L�������g</ja><en>Document that close object</en></param>
        /// <returns><ja>�h�L�������g����邩�ǂ��������肷��l�ł��B</ja><en>Value in which it is decided whether to close document</en></returns>
        /// <remarks>
        /// <ja>
        /// <para>
        /// ���̃��\�b�h�́A�h�L�������g����鑀�삪�s����ƌĂяo����܂��B
        /// </para>
        /// <para>
        /// �J���҂̓h�L�������g����邩�ǂ�����<seealso cref="PrepareCloseResult">PrepareCloseResult�񋓑�</seealso>
        /// �Ƃ��ĕԂ��Ă��������BPrepareCloseResult.Cancel��Ԃ����ꍇ�A�h�L�������g����铮��͎�������܂��B
        /// </para>
        /// </ja>
        /// <en>
        /// <para>
        /// When the operation that closes the document is done, this method is called. 
        /// </para>
        /// <para>
        /// The developer must return whether to close the document as 
        /// <seealso cref="PrepareCloseResult">PrepareCloseResult enumeration</seealso>. 
        /// When PrepareCloseResult.Cancel is returned, operation that closed the document is canceled. 
        /// </para>
        /// </en>
        /// </remarks>
        PrepareCloseResult PrepareCloseDocument(IPoderosaDocument document);
        /// <summary>
        /// <ja>
        /// �Z�b�V��������Ă��悢�������肵�܂��B
        /// </ja>
        /// <en>
        /// It is decided whether I may close the session. 
        /// </en>
        /// </summary>
        /// <returns><ja>�Z�b�V��������邩�ǂ��������肷��l�ł��B</ja><en>It is a value in which it is decided whether to close the session. </en></returns>
        /// <remarks>
        /// <ja>
        ///    <para>
        ///    ���̃��\�b�h�́A�Z�b�V��������鑀�삪�s����ƌĂяo����܂��B
        ///    </para>
        ///    <para>
        ///    �J���҂̓Z�b�V��������邩�ǂ�����<seealso cref="T:Poderosa.Sessions.PrepareCloseResult">PrepareCloseResult�񋓑�</seealso>
        ///    �Ƃ��ĕԂ��Ă��������BPrepareCloseResult.Cancel��Ԃ����ꍇ�A�Z�b�V��������铮��͎�������܂��B
        ///    </para>
        /// </ja>
        /// <en>
        /// <para>
        /// When the operation that close the session is done, this method is called. 
        /// </para>
        /// <para>
        /// The developer must return whether to close the document as 
        /// <seealso cref="PrepareCloseResult">PrepareCloseResult enumeration</seealso>. 
        /// When PrepareCloseResult.Cancel is returned, operation that closed the document is canceled. 
        /// </para>
        /// </en>
        /// </remarks>
        PrepareCloseResult PrepareCloseSession();


        /// <summary>
        /// 
        /// </summary>
        /// <param name="document"></param>
        /// <param name="view"></param>
        /// <exclude/>
        void InternalAttachView(IPoderosaDocument document, IPoderosaView view);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="document"></param>
        /// <param name="view"></param>
        /// <exclude/>
        void InternalDetachView(IPoderosaDocument document, IPoderosaView view);

        /// <summary>
        /// <ja>
        /// �h�L�������g��������ۂɌĂяo����郁�\�b�h�ł��B
        /// </ja>
        /// <en>
        /// It is a method of the call when the document is closed. 
        /// </en>
        /// </summary>
        /// <param name="document"><ja>������ΏۂƂȂ�h�L�������g</ja><en>Document that becomes close object</en></param>
        /// <exclude/>
        void InternalCloseDocument(IPoderosaDocument document);

    }

    //Doc/View�̊֘A�t���ύX�̒ʒm�@�ύX���e�𒀈�擾�ł���悤�ɂ���̂͌�̉ۑ�
    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    /// 
    public interface IDocViewRelationEventHandler {
        void OnDocViewRelationChange();
    }

    /// <summary>
    /// <ja>
    /// �h�L�������g���A�N�e�B�u�����ꂽ���A�N�e�B�u�����ꂽ���Ƃ�ʒm����C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface that notifies for document to be made active and to have been made non-actively.
    /// </en>
    /// </summary>
    public interface IActiveDocumentChangeListener {
        /// <summary>
        /// <ja>
        /// �h�L�������g���A�N�e�B�u�����ꂽ�Ƃ��ɌĂяo����܂��B
        /// </ja>
        /// <en>
        /// Called when the document is activated.
        /// </en>
        /// </summary>
        /// <param name="window"><ja>�ΏۂƂȂ�E�B���h�E</ja><en>Window that becomes object.</en></param>
        /// <param name="document"><ja>�ΏۂƂȂ�h�L�������g</ja><en>Window that becomes object</en></param>
        void OnDocumentActivated(IPoderosaMainWindow window, IPoderosaDocument document);
        /// <summary>
        /// <ja>
        /// �h�L�������g����A�N�e�B�u�����ꂽ�Ƃ��ɌĂяo����܂��B
        /// </ja>
        /// <en>
        /// When the document is made non-active, it is called. 
        /// </en>
        /// </summary>
        /// <param name="window"><ja>�ΏۂƂȂ�E�B���h�E</ja><en>Window that becomes object</en>
        /// </param>
        void OnDocumentDeactivated(IPoderosaMainWindow window);
    }

    /// <summary>
    /// <ja>
    /// �Z�b�V�������J�n�^�ؒf���ꂽ�Ƃ��̒ʒm���󂯎��C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface that receives notification when session begin/finish
    /// </en>
    /// </summary>
    public interface ISessionListener {
        /// <summary>
        /// <ja>
        /// �Z�b�V�������J�n���ꂽ�Ƃ��ɌĂяo����܂��B
        /// </ja>
        /// <en>
        /// When the session is started, it is called. 
        /// </en>
        /// </summary>
        /// <param name="session"><ja>�J�n���ꂽ�Z�b�V����</ja><en>Started session.</en></param>
        void OnSessionStart(ISession session);
        /// <summary>
        /// <ja>
        /// �Z�b�V�������I�������Ƃ��ɌĂяo����܂��B
        /// </ja>
        /// <en>
        /// When the session ends, it is called. 
        /// </en>
        /// </summary>
        /// <param name="session"><ja>�I�������Z�b�V����</ja><en>Ended session</en></param>
        void OnSessionEnd(ISession session);
    }

}
