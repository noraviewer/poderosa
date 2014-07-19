/*
 * Copyright 2004,2006 The Poderosa Project.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *
 * $Id: DocumentAndViewEx.cs,v 1.2 2011/10/27 23:21:55 kzmi Exp $
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

using Poderosa.Forms;
using Poderosa.Commands;
using Poderosa.UI;
using Poderosa.View;

namespace Poderosa.Sessions {
    /// <summary>
    /// <ja>
    /// �h�L�������g�������C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface that shows document
    /// </en>
    /// </summary>
    public interface IPoderosaDocument : ICommandTarget {
        /// <summary>
        /// <ja>
        /// �h�L�������g�̃A�C�R���ł��B
        /// </ja>
        /// <en>
        /// Icon of the document.
        /// </en>
        /// </summary>
        Image Icon {
            get;
        }
        /// <summary>
        /// <ja>
        /// �h�L�������g�̃L���v�V�����ł��B
        /// </ja>
        /// <en>
        /// Caption of the document.
        /// </en>
        /// </summary>
        string Caption {
            get;
        }
        /// <summary>
        /// <ja>
        /// �h�L�������g���\������Z�b�V�����ł��B
        /// </ja>
        /// <en>
        /// Session that composes the document.
        /// </en>
        /// </summary>
        /// <remarks>
        /// <ja>
        /// �W���̃^�[�~�i���G�~�����[�^�Ƃ��ėp����ꍇ�A���̃C���^�[�t�F�C�X�́A
        /// <seealso cref="Poderosa.Sessions.ITerminalSession">ITerminalSession</seealso>�ւƕϊ��ł��܂��B
        /// </ja>
        /// <en>
        /// This interface can be converted into <seealso cref="Poderosa.Sessions.ITerminalSession">ITerminalSession</seealso> when using it as a standard terminal emulator. 
        /// </en>
        /// </remarks>
        ISession OwnerSession {
            get;
        }
    }

    /// <summary>
    /// <ja>
    /// �r���[��\������C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// The interface that show the view.
    /// </en>
    /// </summary>
    /// <remarks>
    /// <ja>
    /// <para>
    /// �Ō�ɃA�N�e�B�u�ɂȂ����r���[�́A<seealso cref="Poderosa.Forms.IPoderosaMainWindow">IPoderosaMainWindow</seealso>
    /// ��<see cref="IPoderosaMainWindow.LastActivatedView">LastActivatedView�v���p�e�B</see>�Ŏ擾�ł��܂��B
    /// </para>
    /// <para>
    /// �܂�<seealso cref="CommandTargetUtil">CommandTargetUtil</seealso>��
    /// <see cref="CommandTargetUtil.AsViewOrLastActivatedView">AsViewOrLastActivatedView���\�b�h</see>
    /// ���Ăяo���ƁA�R�}���h���s���̈����Ƃ��ēn�����^�[�Q�b�g���r���[�ւƕϊ��ł��܂��B
    /// </para>
    /// </ja>
    /// <en>
    /// <para>
    /// The view that became active at the end can be got in the 
    /// <see cref="IPoderosaMainWindow.LastActivatedView">LastActivatedView property</see> of 
    /// <seealso cref="Poderosa.Forms.IPoderosaMainWindow">IPoderosaMainWindow</seealso>. 
    /// </para>
    /// <para>
    /// Moreover, the target passed as an argument when the command is executed can be converted into the 
    /// view by calling the <see cref="CommandTargetUtil.AsViewOrLastActivatedView">AsViewOrLastActivatedView method</see> 
    /// of <seealso cref="CommandTargetUtil">CommandTargetUtil</seealso>. 
    /// </para>
    /// </en>
    /// </remarks>
    public interface IPoderosaView : IPoderosaControl, ICommandTarget {
        /// <summary>
        /// <ja>
        /// �r���[�Ɍ��т����Ă���h�L�������g�������܂��B
        /// </ja>
        /// <en>
        /// Document tie to view
        /// </en>
        /// </summary>
        IPoderosaDocument Document {
            get;
        }
        /// <summary>
        /// <ja>
        /// ���ݑI������Ă��镔��������ISelection�ł��B
        /// </ja>
        /// <en>
        /// ISelection that shows part that has been selected now.
        /// </en>
        /// </summary>
        /// <remarks>
        /// <ja>
        /// ISelection��<seealso cref="ITextSelection">ITextSelection</seealso>�ւƕϊ����A����<see cref="ITextSelection.GetSelectedText">GetSelectedText���\�b�h</see>
        /// ���Ăяo���ƁA���ݑI������Ă��镶������擾�ł��܂��B
        /// 
        /// <code>
        /// ITextSelection select = (ITextSelection)<var>view</var>.CurrentSelection.GetAdapter(
        ///     typeof(ITextSelection));
        /// if ((select != null) &amp;&amp; (!select.IsEmpty))
        /// {
        ///     MessageBox.Show(select.GetSelectedText(TextFormatOption.Default));
        /// }
        /// </code>
        /// </ja>
        /// <en>
        /// The character string that has been selected now can be acquired by converting ISelection into 
        /// <seealso cref="ITextSelection">ITextSelection</seealso>, and calling the 
        /// <see cref="ITextSelection.GetSelectedText">GetSelectedText method</see>. 
        /// 
        /// <code>
        /// ITextSelection select = (ITextSelection)<var>view</var>.CurrentSelection.GetAdapter(
        ///     typeof(ITextSelection));
        /// if ((select != null) &amp;&amp; (!select.IsEmpty))
        /// {
        ///     MessageBox.Show(select.GetSelectedText(TextFormatOption.Default));
        /// }
        /// </code>
        /// </en>
        /// </remarks>
        ISelection CurrentSelection {
            get;
        }

        /// <summary>
        /// <ja>
        /// �r���[�̐e�ƂȂ�t�H�[���������܂��B
        /// </ja>
        /// <en>
        /// Form that becomes parents of view.
        /// </en>
        /// </summary>
        IPoderosaForm ParentForm {
            get;
        }
    }

    //�r���[�N���X
    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public interface IViewFactory : IAdaptable {
        IPoderosaView CreateNew(IPoderosaForm parent);
        Type GetViewType();
        Type GetDocumentType();
    }


    //�r���[��Windows.Forms��̌^�𓮓I�ɕύX�ł���^�C�v�̃r���[
    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public interface IContentReplaceableView : IPoderosaView {
        IViewManager ViewManager {
            get;
        }
        IPoderosaView GetCurrentContent();
        IPoderosaView AssureViewClass(Type viewclass);
        void AssureEmptyViewClass();
    }
    //���g������������C���^�t�F�[�X�BReplaceContent���Ă΂�邽�тɒʒm���󂯂���悤�ɂ���
    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public interface IContentReplaceableViewSite : IAdaptable {
        IContentReplaceableView CurrentContentReplaceableView {
            get;
            set;
        }
    }

    //�r���[�p�̕W���R�}���h�BIPoderosaView���I�v�V���i���Œ񋟂���B

    /// <summary>
    /// <ja>
    /// �r���[�̕W���R�}���h��񋟂��܂��B
    /// </ja>
    /// <en>
    /// Offered a standard command of the view.
    /// </en>
    /// </summary>
    public interface IGeneralViewCommands : IAdaptable {
        /// <summary>
        /// <ja>
        /// �N���b�v�{�[�h�փR�s�[���܂��B
        /// </ja>
        /// <en>
        /// Copy to the clipboard. 
        /// </en>
        /// </summary>
        IPoderosaCommand Copy {
            get;
        }
        /// <summary>
        /// <ja>
        /// �N���b�v�{�[�h����\��t���܂��B
        /// </ja>
        /// <en>
        /// Paste from the clipboard. 
        /// </en>
        /// </summary>
        IPoderosaCommand Paste {
            get;
        }
        //IPoderosaCommand Cut { get; } ���g���^�[�~�i���G�~�����[�^�Ƃ������ƂŁA�J�b�g�͕W���Ɋ܂߂�
    }


    /// <summary>
    /// <ja>
    /// �r���[�}�l�[�W���������܂��B
    /// </ja>
    /// <en>
    /// It shows the view manager.
    /// </en>
    /// </summary>
    /// <remarks>
    /// <ja>
    /// �r���[�}�l�[�W���́A<seealso cref="IPoderosaMainWindow">IPoderosaMainWindow</seealso>��
    /// <see cref="IPoderosaMainWindow.ViewManager">ViewManager�v���p�e�B</see>����擾�ł��܂��B
    /// </ja>
    /// <en>
    /// The view manager can acquire it from the <see cref="IPoderosaMainWindow.ViewManager">ViewManager property</see> of <seealso cref="IPoderosaMainWindow">IPoderosaMainWindow</seealso>. 
    /// </en>
    /// </remarks>
    public interface IViewManager : IAdaptable {
        /// <summary>
        /// 
        /// </summary>
        /// <exclude/>
        Control RootControl {
            get;
        }
        /// <summary>
        /// <ja>
        /// �V�����h�L�������g���쐬���邽�߂̃r���[�����܂��B
        /// </ja>
        /// <en>
        /// Create the view to make a new document.
        /// </en>
        /// </summary>
        /// <returns><ja>���ꂽ�r���[���Ԃ���܂��B</ja><en>return thr created view</en></returns>
        IPoderosaView GetCandidateViewForNewDocument();
        /// <summary>
        /// <ja>
        /// ���̃r���[��������E�B���h�E�ł��B
        /// </ja>
        /// <en>
        /// Window to which this view belongs
        /// </en>
        /// </summary>
        IPoderosaMainWindow ParentWindow {
            get;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public interface ISplittableViewManager : IViewManager {
        //factory��null�̂Ƃ��́Aview�̃N���X��񋟂���factory���g����
        CommandResult SplitHorizontal(IContentReplaceableView view, IViewFactory factory);
        CommandResult SplitVertical(IContentReplaceableView view, IViewFactory factory);
        CommandResult Unify(IContentReplaceableView view, out IContentReplaceableView next_focus);
        CommandResult UnifyAll(out IContentReplaceableView next_focus);
        bool CanSplit(IContentReplaceableView view);
        bool CanUnify(IContentReplaceableView view);
        bool IsSplitted();

        IPoderosaView[] GetAllViews();

        string FormatSplitInfo();
        void ApplySplitInfo(string value);
    }

    //�����֌W�̕ύX�̃C�x���g�n���h��
    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public interface IViewFormatEventHandler {
        void OnSplit(ISplittableViewManager viewmanager);
        void OnUnify(ISplittableViewManager viewmanager);
    }
}
