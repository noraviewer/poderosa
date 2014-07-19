/*
 * Copyright 2004,2006 The Poderosa Project.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *
 * $Id: SelectionEx.cs,v 1.2 2011/10/27 23:21:55 kzmi Exp $
 */
using System;
using System.Collections.Generic;
using System.Text;

using Poderosa.Sessions;
using Poderosa.Commands;

namespace Poderosa.View {

    //�I���T�[�r�X
    // �����ɕ�����Selection�������Ƃ��ł��邪�iFireFox�Ȃǂ������Ȃ��Ă���j�A�A�N�e�B�u�Ȃ͓̂����ɂ͈�����B

    /// <summary>
    /// <ja>
    /// �I�u�W�F�N�g�̑I���Ɋւ���@�\��񋟂���C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface that offers function concerning selection of object
    /// </en>
    /// </summary>
    public interface ISelectionService {
        /// <summary>
        /// <ja>
        /// ���݂̑I���󋵂��܂�ISelection�ł��B
        /// </ja>
        /// <en>
        /// ISelection including present selection situation
        /// </en>
        /// </summary>
        /// <remarks>
        /// <ja>
        /// ���݃A�N�e�B�u�ȃr���[��<see cref="IPoderosaView.CurrentSelection">CurrentSelection�v���p�e�B</see>���Q�Ƃ���̂Ɠ����ł��B
        /// </ja>
        /// <en>
        /// It is the same as the reference to the <see cref="IPoderosaView.CurrentSelection">CurrentSelection property</see> of an active view at present. 
        /// </en>
        /// </remarks>
        ISelection ActiveSelection {
            get;
        } //ActiveView��Selection�Ɠ��`
        /// <summary>
        /// <ja>
        /// �f�t�H���g�̃R�s�[��\��t���Ɋւ���R�}���h�ւ̃C���^�[�t�F�C�X�ł��B
        /// </ja>
        /// <en>
        /// Interface to command concerning copy and putting default.
        /// </en>
        /// </summary>
        IPoderosaCommand DefaultCopyCommand {
            get;
        }
    }

    /// <summary>
    /// <ja>
    /// �I����Ԃ��ω������Ƃ��̒ʒm���󂯎�郊�X�i�ł��B
    /// </ja>
    /// <en>
    /// Listener that receives notification when selection changes.
    /// </en>
    /// </summary>
    public interface ISelectionListener {
        /// <summary>
        /// <ja>
        /// �I�����J�n���ꂽ�Ƃ��ɌĂяo����܂��B
        /// </ja>
        /// <en>
        /// When the selection is begun, it is called. 
        /// </en>
        /// </summary>
        void OnSelectionStarted();
        /// <summary>
        /// <ja>
        /// �I�����m�肵���Ƃ��ɌĂяo����܂��B
        /// </ja>
        /// <en>
        /// When the selection is fixed, it is called. 
        /// </en>
        /// </summary>
        void OnSelectionFixed();
    }

    //�I�����Ă���I�u�W�F�N�g �����͊eIPoderosaView���Ǘ����邾�낤
    /// <summary>
    /// <ja>
    /// �I�����Ă���I�u�W�F�N�g�𑀍삷��C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface that operates object that has been selected
    /// </en>
    /// </summary>
    /// <remarks>
    /// <ja>
    /// �^�[�~�i���G�~�����[�^�ł���r���[�̏ꍇ�AGetAdapter���\�b�h���g�����Ƃ�
    /// <seealso cref="ITextSelection">ITextSelecton</seealso>�ւƕϊ��ł��܂��B
    /// </ja>
    /// <en>
    /// It is possible to convert it into <seealso cref="ITextSelection">ITextSelecton</seealso> by using the GetAdapter 
    /// method for the view that is the terminal emulator. 
    /// </en>
    /// </remarks>
    public interface ISelection : ICommandTarget {
        /// <summary>
        /// <ja>
        /// ���L����r���[�������܂��B
        /// </ja>
        /// <en>
        /// The owned view is shown. 
        /// </en>
        /// </summary>
        IPoderosaView OwnerView {
            get;
        }

        /// <summary>
        /// <ja>
        /// �I��͈͂��ω������Ƃ��̃��X�i��o�^���܂��B
        /// </ja>
        /// <en>
        /// The listener when the range of the selection changes is registered. 
        /// </en>
        /// </summary>
        /// <param name="listener"><ja>�o�^���郊�X�i</ja><en>The listener to regist.</en></param>
        void AddSelectionListener(ISelectionListener listener);
        /// <summary>
        /// <ja>
        /// �I��͈͂��ω������Ƃ��̃��X�i���������܂��B
        /// </ja>
        /// <en>
        /// The listener when the range of the selection changes is released. 
        /// </en>
        /// </summary>
        /// <param name="listener">
        /// <ja>�������郊�X�i</ja>
        /// <en>The listener to remove.</en>
        /// </param>
        void RemoveSelectionListener(ISelectionListener listener);
    }

    /* �T�^�I�ȃV�i���I
     * �@�I���J�n�E�I����UI�����View���ŕ���B���̒���Selection�̓�����Ԃ��X�V����B
     * �@View�̕`��ɂ����ẮA���g��Selection������΂�������Ƃɕ`�悷��B
     * �@�R�s�[�Ȃǂ̔ėp�R�}���h�́ASelection�ɑ΂���TranslateCommand���Ă�ŁASelection�̃^�C�v�ɂ��ŗL�R�}���h��Ԃ�����B
     * �@�R���e�L�X�g���j���[�́ASelection��CommandTarget�Ƃ��郁�j���[�c���[���r���[���p�ӂ��ĕ\������
     */

    //�e�L�X�g�̑I��p
    /// <summary>
    /// <ja>
    /// �e�L�X�g��I������Ƃ��̏������w�肵�܂��B
    /// </ja>
    /// <en>
    /// The format when the text is selected is specified. 
    /// </en>
    /// </summary>
    public enum TextFormatOption {
        /// <summary>
        /// <ja>
        /// �W���I�ȃe�L�X�g�Ƃ��ĕԂ��܂��B
        /// </ja>
        /// <en>
        /// Returns as a standard text. 
        /// </en>
        /// </summary>
        Default,
        /// <summary>
        /// <ja>
        /// �����܂܂̏�ԂŕԂ��܂��B���Ȃ킿�r���[�̉E�[�Ő܂�Ԃ��ꂽ�ӏ���\r\n���t���܂��B
        /// </ja>
        /// <en>
        /// It returns it while seen. That is, \r\n adheres to the part turned on a right edge of the view. 
        /// </en>
        /// </summary>
        AsLook
    }

    /// <summary>
    /// <ja>
    /// �I������Ă���e�L�X�g�𑀍삷��C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface that operates text that has been selected.
    /// </en>
    /// </summary>
    /// <remarks>
    /// <ja>
    /// �^�[�~�i���G�~�����[�^�������r���[�̏ꍇ�AISelection�́A����ITextSelection�ւƕϊ��ł��܂��B
    /// </ja>
    /// <en>
    /// ISelection can be converted into this ITextSelection for the view that shows the terminal emulator. 
    /// </en>
    /// </remarks>
    /// <example>
    /// <ja>
    /// �A�N�e�B�u�ȃr���[�őI������Ă���e�L�X�g���擾���܂��B
    /// <code>
    /// // <value>target</value>����A�N�e�B�u�ȃr���[�𓾂܂�
    /// IPoderosaView view = CommandTargetUtil.AsViewOrLastActivatedView(target);
    /// // ITextSelection�𓾂܂��B
    /// ITextSelection select = (ITextSelection)view.CurrentSelection.GetAdapter(
    ///   typeof(ITextSelection));
    /// // �I������Ă���e�L�X�g�𓾂܂�
    /// if ((select != null) &amp;&amp; (!select.IsEmpty))
    /// {
    ///   MessageBox.Show(select.GetSelectedText(TextFormatOption.Default));
    /// }
    /// </code>
    /// </ja>
    /// <en>
    /// The text has been selected by an active view is got.
    /// <code>
    /// // Get the active view from <value>target</value>.
    /// IPoderosaView view = CommandTargetUtil.AsViewOrLastActivatedView(target);
    /// // Get ITextSelection
    /// ITextSelection select = (ITextSelection)view.CurrentSelection.GetAdapter(
    ///   typeof(ITextSelection));
    /// // Get the selected text.
    /// if ((select != null) &amp;&amp; (!select.IsEmpty))
    /// {
    ///   MessageBox.Show(select.GetSelectedText(TextFormatOption.Default));
    /// }
    /// </code>
    /// </en>
    /// </example>
    public interface ITextSelection : ISelection {
        /// <summary>
        /// <ja>
        /// �I������Ă���e�L�X�g�𓾂܂��B
        /// </ja>
        /// <en>
        /// The text that has been selected is obtained. 
        /// </en>
        /// </summary>
        /// <param name="opt"><ja>�擾����t�H�[�}�b�g���w�肵�܂��B</ja><en>Specifies the acquired format.</en></param>
        /// <returns><ja>�I������Ă���e�L�X�g�ł��B</ja><en>Selected text</en></returns>
        string GetSelectedText(TextFormatOption opt);
        /// <summary>
        /// <ja>
        /// �I������Ă���e�L�X�g�����݂��邩�ǂ����������܂��B�����I������Ă��Ȃ��Ƃ��ɂ�true�A�I������Ă���Ƃ��ɂ�false�ł��B
        /// </ja>
        /// <en>
        /// It is shown whether the text that has been selected exists. When true and selected, it is false when nothing has been selected. 
        /// </en>
        /// </summary>
        bool IsEmpty {
            get;
        }
        /// <summary>
        /// <ja>
        /// ���ׂđI����Ԃɂ��܂��B
        /// </ja>
        /// <en>
        /// It all puts it into the state of the selection. 
        /// </en>
        /// </summary>
        void SelectAll();
        /// <summary>
        /// <ja>
        /// �����I������Ă��Ȃ���Ԃɂ��܂��B
        /// </ja>
        /// <en>
        /// It puts it into the state that nothing has been selected. 
        /// </en>
        /// </summary>
        void Clear();
    }

}
