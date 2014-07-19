/*
 * Copyright 2004,2006 The Poderosa Project.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *
 * $Id: ToolBarEx.cs,v 1.3 2012/03/11 12:19:21 kzmi Exp $
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using Poderosa.Commands;

namespace Poderosa.Forms {
    /// <summary>
    /// <ja>
    /// �c�[���o�[�Ɋ܂܂��v�f���������C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Base interface that shows element included in toolbar.
    /// </en>
    /// </summary>
    public interface IToolBarElement : IAdaptable {
        /// <summary>
        /// <ja>
        /// �v�f�̃c�[���`�b�v�e�L�X�g�������܂��B
        /// </ja>
        /// <en>
        /// The tooltip text of the element is shown. 
        /// </en>
        /// </summary>
        string ToolTipText {
            get;
        }
    }

    /// <summary>
    /// <ja>
    /// �c�[���o�[���̃��x���������C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface that shows label in toolbar.
    /// </en>
    /// </summary>
    /// <remarks>
    /// <ja>
    /// �J���҂����x�������ꍇ�A���̃C���^�[�t�F�C�X�����������I�u�W�F�N�g��������ɁA<seealso cref="ToolBarLabelImpl">ToolBarLabelImpl</seealso>���g�����Ƃ��ł��܂��B
    /// </ja>
    /// <en>
    /// <seealso cref="ToolBarLabelImpl">ToolBarLabelImpl</seealso> can be used instead of making the object that mounts this interface when the developer creates labels. 
    /// </en>
    /// </remarks>
    public interface IToolBarLabel : IToolBarElement {
        /// <summary>
        /// <ja>���x���̃e�L�X�g�ł��B</ja>
        /// <en>Text of the label.</en>
        /// </summary>
        string Text {
            get;
        }
        /// <summary>
        /// <ja>���x���̕��ł��B�P�ʂ̓s�N�Z���ł��B</ja>
        /// <en>Width of the label. The unit is a pixel. </en>
        /// </summary>
        int Width {
            get;
        }
    }

    /// <summary>
    /// <ja>
    /// �c�[���o�[���̃{�^���������C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface that shows button in toolbar.
    /// </en>
    /// </summary>
    /// <remarks>
    /// <ja>
    /// �J���҂��{�^�������ꍇ�A���̃C���^�[�t�F�C�X�����������I�u�W�F�N�g��������ɁA<seealso cref="ToolBarCommandButtonImpl">ToolBarCommandButtonImpl</seealso>���g�����Ƃ��ł��܂��B
    /// </ja>
    /// <en>
    /// <seealso cref="ToolBarCommandButtonImpl">ToolBarCommandButtonImpl</seealso> can be used instead of making the object that implements this interface when the developer makes the button. 
    /// </en>
    /// </remarks>
    public interface IToolBarCommandButton : IToolBarElement {
        /// <summary>
        /// <ja>
        /// �{�^�����N���b�N���ꂽ�Ƃ��Ɏ��s�����R�}���h�ł��B
        /// </ja>
        /// <en>
        /// It is a command executed when the button is clicked. 
        /// </en>
        /// </summary>
        IPoderosaCommand Command {
            get;
        }
        /// <summary>
        /// <ja>
        /// �{�^���\�ʂɕ\������A�C�R���ł��B
        /// </ja>
        /// <en>
        /// Icon displayed on the surface of the button. 
        /// </en>
        /// </summary>
        /// <remarks>
        /// <ja>
        /// �A�C�R���̑傫����16�~16�s�N�Z���łȂ���΂Ȃ�܂���B
        /// </ja>
        /// <en>
        /// The size of the icon should be 16�~16 pixels. 
        /// </en>
        /// </remarks>
        Image Icon {
            get;
        }
    }

    /// <summary>
    /// <ja>
    /// �c�[���o�[���̃R���{�{�b�N�X�������C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface that shows combobox in toolbar.
    /// </en>
    /// </summary>
    /// <remarks>
    /// <ja>
    /// �J���҂��R���{�{�b�N�X�����ꍇ�A���̃C���^�[�t�F�C�X�����������I�u�W�F�N�g��������ɁA<seealso cref="ToolBarComboBoxImpl">ToolBarComboBoxImpl</seealso>���g�����Ƃ��ł��܂��B
    /// </ja>
    /// <en>
    /// <seealso cref="ToolBarComboBoxImpl">ToolBarComboBoxImpl</seealso> can be used instead of making the object that implements this interface when the developer makes the combo box. 
    /// </en>
    /// </remarks>
    public interface IToolBarComboBox : IToolBarElement {
        /// <summary>
        /// <ja>
        /// �R���{�{�b�N�X���ɕ\������I�����ƂȂ�A�C�e���ł��B
        /// </ja>
        /// <en>
        /// Item that becomes choices displayed in combo box.
        /// </en>
        /// </summary>
        object[] Items {
            get;
        }
        /// <summary>
        /// <ja>
        /// �R���{�{�b�N�X�̕��ł��B�P�ʂ̓s�N�Z���ł��B
        /// </ja>
        /// <en>Width of the combo box. The unit is a pixel. </en>
        /// </summary>
        int Width {
            get;
        }
        /// <summary>
        /// <ja>���̃R���{�{�b�N�X���I���ł��邩�ǂ����������܂��B</ja>
        /// <en>It is shown whether this combo box can be selected. </en>
        /// </summary>
        /// <param name="target"><ja>���s�̑ΏۂƂȂ�^�[�Q�b�g�ł��B</ja><en>target for execution. </en></param>
        /// <returns><ja>�I���ł���Ƃ��ɂ�true�A�����łȂ��Ƃ��ɂ�false���Ԃ���܂��B</ja><en>False is returned when it is not true so when it is possible to select it. </en></returns>
        /// <remarks>
        /// <ja>
        /// <paramref name="target">target</paramref>�́A���̃c�[���o�[��������<see cref="IPoderosaMainWindow">IPoderosaMainWindow</see>�ł��B
        /// </ja>
        /// <en>
        /// <paramref name="target">target</paramref> is a toolbar that belongs to <see cref="IPoderosaMainWindow">IPoderosaMainWindow</see>.
        /// </en>
        /// </remarks>
        bool IsEnabled(ICommandTarget target);
        /// <summary>
        /// <ja>
        /// ���ݑI������Ă���A�C�e���̃C���f�b�N�X�ԍ���Ԃ��܂��B
        /// </ja>
        /// <en>
        /// Return the index of the item that has been selected now.
        /// </en>
        /// </summary>
        /// <param name="target"><ja>���s�̑ΏۂƂȂ�^�[�Q�b�g�ł��B</ja><en>Target for execution</en></param>
        /// <returns><ja><paramref name="target">target</paramref>�̃C���f�b�N�X�ʒu���Ԃ���܂��B</ja>
        /// <en>Return index position of the <paramref name="target">target</paramref></en>
        /// </returns>
        /// <remarks>
        /// <ja>
        /// <paramref name="target">target</paramref>�́A���̃c�[���o�[��������<see cref="IPoderosaMainWindow">IPoderosaMainWindow</see>�ł��B
        /// </ja>
        /// <en>
        /// <paramref name="target">target</paramref> is a toolbar that belongs to <see cref="IPoderosaMainWindow">IPoderosaMainWindow</see>.
        /// </en>
        /// </remarks>
        int GetSelectedIndex(ICommandTarget target);
        /// <summary>
        /// <ja>
        /// �R���{�{�b�N�X�őI������Ă���I�������ω������Ƃ��ɌĂяo����郁�\�b�h�ł��B
        /// </ja>
        /// <en>
        /// Method of call when choices that have been selected in combobox change
        /// </en>
        /// </summary>
        /// <param name="target"><ja>���s�̑ΏۂƂȂ�^�[�Q�b�g�ł��B</ja><en>Target for execution.</en></param>
        /// <param name="selectedIndex"><ja>���[�U�[���I�������A�C�e���̃C���f�b�N�X�ԍ��ł��B</ja><en>Index of item that user selected.</en></param>
        /// <param name="selectedItem"><ja>���[�U�[���I�������A�C�e���̃I�u�W�F�N�g�ł��B</ja><en>An object of item that user selected.</en></param>
        /// <remarks>
        /// <ja>
        /// <paramref name="target">target</paramref>�́A���̃c�[���o�[��������<see cref="IPoderosaMainWindow">IPoderosaMainWindow</see>�ł��B
        /// </ja>
        /// <en>
        /// <paramref name="target">target</paramref> is <see cref="IPoderosaMainWindow">IPoderosaMainWindow</see> that this toolbar belongs. 
        /// </en>
        /// </remarks>
        void OnChange(ICommandTarget target, int selectedIndex, object selectedItem);
    }

    /// <summary>
    /// <ja>
    /// �c�[���o�[���̃g�O���{�^���������C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface that shows Toggle button in toolbar.
    /// </en>
    /// </summary>
    /// <remarks>
    /// <ja>
    /// �J���҂��g�O���{�^�������ꍇ�A���̃C���^�[�t�F�C�X�����������I�u�W�F�N�g��������ɁA<seealso cref="ToolBarToggleButtonImpl">ToolBarToggleButtonImpl</seealso>���g�����Ƃ��ł��܂��B
    /// </ja>
    /// <en>
    /// <seealso cref="ToolBarToggleButtonImpl">ToolBarToggleButtonImpl</seealso> can be used instead of making the object that implements this interface when the developer makes the toggle button. 
    /// </en>
    /// </remarks>
    public interface IToolBarToggleButton : IToolBarElement {
        /// <summary>
        /// <ja>
        /// �{�^���\�ʂɕ\������A�C�R���ł��B
        /// </ja>
        /// <en>
        /// Icon displayed on surface of button
        /// </en>
        /// </summary>
        /// <remarks>
        /// <ja>
        /// �A�C�R���̑傫����16�~16�s�N�Z���łȂ���΂Ȃ�܂���B
        /// </ja>
        /// <en>
        /// The size of the icon should be 16�~16 pixels. 
        /// </en>
        /// </remarks>
        Image Icon {
            get;
        }
        /// <summary>
        /// <ja>���̃g�O���{�^�����I���ł��邩�ǂ����������܂��B</ja>
        /// <en>It is shown whether this toggle button can be selected. </en>
        /// </summary>
        /// <param name="target"><ja>���s�̑ΏۂƂȂ�^�[�Q�b�g�ł��B</ja><en>Target for execution</en></param>
        /// <returns><ja>�I���ł���Ƃ��ɂ�true�A�����łȂ��Ƃ��ɂ�false���Ԃ���܂��B</ja><en>False is returned when it is not true so when it is possible to select it. </en></returns>
        /// <remarks>
        /// <ja>
        /// <paramref name="target">target</paramref>�́A���̃c�[���o�[��������<see cref="IPoderosaMainWindow">IPoderosaMainWindow</see>�ł��B
        /// </ja>
        /// <en>
        /// <paramref name="target">target</paramref> is <see cref="IPoderosaMainWindow">IPoderosaMainWindow</see> that this toolbar belongs. 
        /// </en>
        /// </remarks>
        bool IsEnabled(ICommandTarget target);
        /// <summary>
        /// <ja>���̃g�O���{�^���̃I���^�I�t�̏�Ԃ������܂��B</ja>
        /// <en>The state of on/off of this toggle button is shown. </en>
        /// </summary>
        /// <param name="target"><ja>���s�̑ΏۂƂȂ�^�[�Q�b�g�ł��B</ja>
        /// <en>Target for execution</en>
        /// </param>
        /// <returns><ja>�I���̂Ƃ��i����ł����ԁj�̂Ƃ��ɂ�true�A�I�t�ł���Ƃ��i����ł��Ȃ���ԁj�̂Ƃ��ɂ�false���Ԃ���܂��B</ja>
        /// <en>False is returned when it is true off (state that doesn't dent) when turning it on (state that has dented). </en>
        /// </returns>
        /// <remarks>
        /// <ja>
        /// <paramref name="target">target</paramref>�́A���̃c�[���o�[��������<see cref="IPoderosaMainWindow">IPoderosaMainWindow</see>�ł��B
        /// </ja>
        /// <en>
        /// <paramref name="target">target</paramref> is <see cref="IPoderosaMainWindow">IPoderosaMainWindow</see> that this toolbar belongs. 
        /// </en>
        /// </remarks>
        bool IsChecked(ICommandTarget target);
        /// <summary>
        /// <ja>
        /// �g�O���{�^���̃I���^�I�t�̏�Ԃ��ς�����Ƃ��ɌĂяo����郁�\�b�h�ł��B
        /// </ja>
        /// <en>
        /// Method of the call when the state of on/off of the toggle button changes. 
        /// </en>
        /// </summary>
        /// <param name="target"><ja>���s�̑ΏۂƂȂ�^�[�Q�b�g�ł��B</ja><en>Target for execution</en></param>
        /// <param name="is_checked"><ja>�I���ɂ��ꂽ�Ƃ��ɂ�true�A�I�t�ɂ��ꂽ�Ƃ��ɂ�false�ł��B</ja>
        /// <en>When turned off true, it is false when turned on. </en>
        /// </param>
        /// <remarks>
        /// <ja>
        /// <paramref name="target">target</paramref>�́A���̃c�[���o�[��������<see cref="IPoderosaMainWindow">IPoderosaMainWindow</see>�ł��B
        /// </ja>
        /// <en>
        /// <paramref name="target">target</paramref> is a toolbar that belongs to <see cref="IPoderosaMainWindow">IPoderosaMainWindow</see>.
        /// </en>
        /// </remarks>
        void OnChange(ICommandTarget target, bool is_checked);
    }

    /// <summary>
    /// <ja>
    /// �c�[���o�[�������C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface that shows toolbar.
    /// </en>
    /// </summary>
    public interface IToolBar : IAdaptable {
        /// <summary>
        /// <ja>�c�[���o�[��������E�B���h�E�ł��B</ja>
        /// <en>Window to which toolbar belongs</en>
        /// </summary>
        IPoderosaMainWindow ParentWindow {
            get;
        }
        /// <summary>
        /// <ja>���ׂĂ̗v�f���ĕ`�悵�܂��B</ja>
        /// <en>It draws in all elements again. </en>
        /// </summary>
        void RefreshAll();
        /// <summary>
        /// <ja>�w�肵���R���|�[�l���g���ĕ`�悵�܂��B</ja>
        /// <en>It draws in the specified component again. </en>
        /// </summary>
        /// <param name="component"><ja>�ĕ`�悵�����R���|�[�l���g</ja><en>Component where it wants to draw again</en></param>
        void RefreshComponent(IToolBarComponent component);
        /// <summary>
        /// <ja>
        /// �c�[���o�[�̈ʒu�𕶎���Ƃ��č\���������̂�Ԃ��܂��B
        /// </ja>
        /// <en>
        /// Return the one that the position of the toolbar was composed as a character string.
        /// </en>
        /// </summary>
        /// <returns><ja>�c�[���o�[�̈ʒu������������������</ja><en>Character string that makes position of toolbar format</en></returns>
        /// <remarks>
        /// <ja>
        /// ���̃��\�b�h����߂��ꂽ������́APreference�Ƀc�[���o�[�ʒu��ۑ�����Ƃ��Ɏg���܂��B
        /// </ja>
        /// <en>
        /// When the toolbar position is preserved in Preference, the character string returned from this method is used. 
        /// </en>
        /// </remarks>
        string FormatLocations();
    }

    /// <summary>
    /// <ja>
    /// �c�[���o�[�R���|�[�l���g�������C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface that shows toolbar component.
    /// </en>
    /// </summary>
    /// <remarks>
    /// <ja>
    /// �c�[���o�[���̃��x���A�{�^���A�g�O���{�^���A�R���{�{�b�N�X�́A<see cref="ToolBarElements">ToolBarElemnents�v���p�e�B</see>�ɔz��Ƃ��Ċi�[���܂��B
    /// </ja>
    /// <en>
    /// The label, the button, the toggle button, and the combobox in the toolbar are stored in the <see cref="ToolBarElements">ToolBarElemnents property</see> as an array. 
    /// </en>
    /// </remarks>
    /// <example>
    /// <ja>
    /// �c�[���o�[�R���|�[�l���g������������܂��B
    /// <code>
    /// [assembly: PluginDeclaration(typeof(MyPlugin.HelloWorldPlugin))]
    /// namespace MyPlugin
    /// {
    ///    [PluginInfo(ID="jp.example.helloworld", Version="1.0",
    ///        Dependencies="org.poderosa.core.window")]
    ///
    ///    // �����ł̓v���O�C�����g��IToolBarComponent������
    ///    internal class HelloWorldPlugin : PluginBase, IToolBarComponent
    ///    {
    ///        private IToolBarElement[] _elements;
    ///
    ///        public override void InitializePlugin(IPoderosaWorld poderosa)
    ///        {
    ///            base.InitializePlugin(poderosa);
    ///            
    ///            // �i1�j�R�}���h�I�u�W�F�N�g��p�ӂ���
    ///            PoderosaCommandImpl btncommand = new PoderosaCommandImpl(
    ///              delegate(ICommandTarget target)
    ///              {
    ///                  // ���s���ꂽ�Ƃ��̃R�}���h
    ///                  MessageBox.Show("�{�^�����N���b�N����܂���");
    ///                  return CommandResult.Succeeded;
    ///              },
    ///              delegate(ICommandTarget target)
    ///              {
    ///                  // �R�}���h�����s�ł��邩�ǂ����������f���Q�[�g
    ///                  return true;
    ///              }
    ///            );
    ///
    ///            // �i2�j�v�f�Ƃ��ă{�^�������imyImage��16�~16�̃r�b�g�}�b�v�j
    ///            System.Drawing.Image myImage = 
    ///              new System.Drawing.Bitmap("�摜�t�@�C����");
    ///            ToolBarCommandButtonImpl btn = ]
    ///              new ToolBarCommandButtonImpl(btncommand, myImage);
    ///
    ///            // �v�f�Ƃ��Đݒ�
    ///            _elements = new IToolBarElement[]{ btn };
    ///
    ///            // �i3�j�g���|�C���g���������ēo�^
    ///            IExtensionPoint toolbarExt = 
    ///              poderosa.PluginManager.FindExtensionPoint("org.poderosa.core.window.toolbar");
    ///            // �o�^
    ///            toolbarExt.RegisterExtension(this);
    ///        }
    ///
    ///        public IToolBarElement[] ToolBarElements
    ///        {
    ///            // �v�f��Ԃ�
    ///            get { return _elements; }
    ///        }
    ///    }
    ///}
    /// </code>
    /// </ja>
    /// <en>
    /// The example of making the toolbar component is shown. 
    /// <code>
    /// [assembly: PluginDeclaration(typeof(MyPlugin.HelloWorldPlugin))]
    /// namespace MyPlugin
    /// {
    ///    [PluginInfo(ID="jp.example.helloworld", Version="1.0",
    ///        Dependencies="org.poderosa.core.window")]
    ///
    ///    // Here, implmenent IToolBarComponent to this plug-in.
    ///    internal class HelloWorldPlugin : PluginBase, IToolBarComponent
    ///    {
    ///        private IToolBarElement[] _elements;
    ///
    ///        public override void InitializePlugin(IPoderosaWorld poderosa)
    ///        {
    ///            base.InitializePlugin(poderosa);
    ///            
    ///            // (1) Prepare the command object.
    ///            PoderosaCommandImpl btncommand = new PoderosaCommandImpl(
    ///              delegate(ICommandTarget target)
    ///              {
    ///                  // Command when executed.
    ///                  MessageBox.Show("Button is clicked.");
    ///                  return CommandResult.Succeeded;
    ///              },
    ///              delegate(ICommandTarget target)
    ///              {
    ///                  // Delegate that shows whether command can be executed
    ///                  return true;
    ///              }
    ///            );
    ///
    ///            // (2)Create the button as element (myImage is a bitmap that size is 16x16)
    ///            System.Drawing.Image myImage = 
    ///              new System.Drawing.Bitmap("Graphics file name.");
    ///            ToolBarCommandButtonImpl btn = ]
    ///              new ToolBarCommandButtonImpl(btncommand, myImage);
    ///
    ///            // Set as element.
    ///            _elements = new IToolBarElement[]{ btn };
    ///
    ///            // (3)Retrieve the extension point and regist.
    ///            IExtensionPoint toolbarExt = 
    ///              poderosa.PluginManager.FindExtensionPoint("org.poderosa.core.window.toolbar");
    ///            // Regist
    ///            toolbarExt.RegisterExtension(this);
    ///        }
    ///
    ///        public IToolBarElement[] ToolBarElements
    ///        {
    ///            // Return the element.
    ///            get { return _elements; }
    ///        }
    ///    }
    ///}
    /// </code>
    /// </en>
    /// </example>
    public interface IToolBarComponent : IAdaptable {
        /// <summary>
        /// <ja>
        /// �c�[���o�[�R���|�[�l���g�Ɋ܂܂��A���x���A�{�^���A�g�O���{�^���A�R���{�{�b�N�X�̔z��ł��B
        /// </ja>
        /// <en>
        /// Inclusion in toolbar component, and array of label, button, toggle button, and combobox
        /// </en>
        /// </summary>
        IToolBarElement[] ToolBarElements {
            get;
        }
    }

    //�eToolBarElement�̕W������
    /// <summary>
    /// <ja>
    /// �c�[���o�[�̗v�f�̊��ƂȂ�N���X�ł��B
    /// </ja>
    /// <en>
    /// Class that becomes base of element of toolbar.
    /// </en>
    /// </summary>
    public abstract class ToolBarElementImpl : IToolBarElement {
        public virtual IAdaptable GetAdapter(Type adapter) {
            return WindowManagerPlugin.Instance.PoderosaWorld.AdapterManager.GetAdapter(this, adapter);
        }
        /// <summary>
        /// <ja>
        /// �c�[���`�b�v�e�L�X�g��Ԃ��܂��B
        /// </ja>
        /// <en>
        /// Return the tooltip text.
        /// </en>
        /// </summary>
        /// <remarks>
        /// <ja>
        /// �f�t�H���g�ł́A���當���i""�j��Ԃ��悤�Ɏ�������Ă��܂��B�K�v�ɉ����ăI�[�o�[���C�h���Ă��������B
        /// </ja>
        /// <en>
        /// In default, to return the null character (""), it is implemente. Please do override if necessary. 
        /// </en>
        /// </remarks>
        public virtual string ToolTipText {
            get {
                return "";
            }
        }
    }
    /// <summary>
    /// <ja>
    /// �c�[���o�[�v�f�̃��x�����\������@�\��񋟂��܂��B
    /// </ja>
    /// <en>
    /// The function to compose the label of the toolbar element is offered. 
    /// </en>
    /// </summary>
    /// <remarks>
    /// <ja>
    /// �J���҂͂��̃N���X��p���邱�ƂŁA<seealso cref="IToolBarLabel">IToolBarLabel</seealso>
    /// ��������I�u�W�F�N�g���\���ł��܂��B
    /// </ja>
    /// <en>
    /// The developer can compose the object that has <seealso cref="IToolBarLabel">IToolBarLabel</seealso> by using this class. 
    /// </en>
    /// </remarks>
    public class ToolBarLabelImpl : ToolBarElementImpl, IToolBarLabel {
        /// <summary>
        /// <ja>
        /// �J���`���������������ϐ��ł��B
        /// </ja>
        /// <en>
        /// Internal variable that shows culture information
        /// </en>
        /// </summary>
        protected StringResource _res;
        /// <summary>
        /// <ja>
        /// �J���`�������g�����ǂ��������������ϐ��ł��B
        /// </ja>
        /// <en>
        /// It is an internal variable that shows whether to use culture information. 
        /// </en>
        /// </summary>
        protected bool _usingStringResource;
        /// <summary>
        /// <ja>
        /// ���x���������������ϐ��ł��B�P�ʂ̓s�N�Z���ł��B
        /// </ja>
        /// <en>
        /// It is an internal variable that shows the width of the label. The unit is a pixel. 
        /// </en>
        /// </summary>
        /// <remarks>
        /// <ja>
        /// ���̃v���p�e�B�̒l��<seealso cref="Width">Width�v���p�e�B</seealso>����Ԃ���܂��B
        /// </ja>
        /// <en>
        /// The value of this property is returned by the Width property. 
        /// </en>
        /// </remarks>
        protected int _width;
        /// <summary>
        /// <ja>
        /// ���x���Ƃ��ĕ\������e�L�X�g��ێ���������ϐ��ł��B
        /// </ja>
        /// <en>
        /// It is an internal variable that holds the text to show as the label.
        /// </en>
        /// </summary>
        protected string _text;

        /// <summary>
        /// <ja>
        /// ��̃��x�����쐬���܂��B
        /// </ja>
        /// <en>
        /// Create a null label.
        /// </en>
        /// </summary>
        /// <overloads>
        /// <summary>
        /// <ja>
        /// �c�[���o�[�̃��x�����쐬���܂��B
        /// </ja>
        /// <en>
        /// Create a label of toolbar.
        /// </en>
        /// </summary>
        /// </overloads>
        public ToolBarLabelImpl() {
        }

        /// <summary>
        /// <ja>�J���`�������w�肵�ă��x�����쐬���܂��B</ja>
        /// <en>Create label specified with culture information</en>
        /// </summary>
        /// <param name="res"><ja>�J���`�����ł�</ja><en>Culture information.</en></param>
        /// <param name="text"><ja>���x���ɕ\������e�L�X�gID�ł��B</ja><en>The text ID to show on the label.</en></param>
        /// <param name="width"><ja>���x���̕��ł��B�P�ʂ̓s�N�Z���ł��B</ja><en>WIdth of the label. The unit is a pixel.</en></param>

        public ToolBarLabelImpl(StringResource res, string text, int width) {
            _res = res;
            _usingStringResource = true;
            _text = text;
            _width = width;
        }

        /// <summary>
        /// <ja>
        /// �e�L�X�g�ƕ����w�肵�ă��x�����쐬���܂��B
        /// </ja>
        /// <en>
        /// Create the label specifying the text and width. 
        /// </en>
        /// </summary>
        /// <param name="text"><ja>���x���ɕ\������e�L�X�g�ł��B</ja><en>The text to show on the label.</en></param>
        /// <param name="width"><ja>���x���̕��ł��B�P�ʂ̓s�N�Z���ł��B</ja><en>WIdth of the label. The unit is a pixel.</en></param>
        public ToolBarLabelImpl(string text, int width) {
            _usingStringResource = false;
            _text = text;
            _width = width;
        }

        /// <summary>
        /// <ja>
        /// ���x���ɕ\������e�L�X�g��Ԃ��܂��B
        /// </ja>
        /// <en>
        /// Return the text to show on the label.
        /// </en>
        /// </summary>
        /// <remarks>
        /// <ja>
        /// �J���`�����t���̃R���X�g���N�^�ō쐬���ꂽ�ꍇ�ɂ́A<seealso cref="StringResource">StringResource</seealso>
        /// ��<see cref="StringResource.GetString">GetString���\�b�h</see>���Ăяo���ꂽ���ʂ��߂�܂��B
        /// </ja>
        /// <en>
        /// The result that the <see cref="StringResource.GetString">GetString method</see> of <seealso cref="StringResource">StringResource</seealso> is called returns when made by the constructor with culture information. 
        /// </en>
        /// </remarks>
        public virtual string Text {
            get {
                return _usingStringResource ? _res.GetString(_text) : _text;
            }
        }

        /// <summary>
        /// <ja>
        /// ���x������Ԃ��܂��B�P�ʂ̓s�N�Z���ł��B
        /// </ja>
        /// <en>
        /// Return the width of the label. The unit is a pixel. 
        /// </en>
        /// </summary>
        public virtual int Width {
            get {
                return _width;
            }
        }
    }

    /// <summary>
    /// <ja>
    /// �c�[���o�[�v�f�̃{�^�����\������@�\��񋟂��܂��B
    /// </ja>
    /// <en>
    /// Offer the function to compose the button of the toolbar element.
    /// </en>
    /// </summary>
    /// <remarks>
    /// <ja>
    /// �J���҂͂��̃N���X��p���邱�ƂŁA<seealso cref="IToolBarCommandButton">IToolBarCommandButton</seealso>
    /// ��������I�u�W�F�N�g���\���ł��܂��B
    /// </ja>
    /// <en>
    /// The developer can compose the object that has <seealso cref="IToolBarCommandButton">IToolBarCommandButton</seealso> by using this class. 
    /// </en>
    /// </remarks>
    public class ToolBarCommandButtonImpl : ToolBarElementImpl, IToolBarCommandButton {
        /// <summary>
        /// <ja>
        /// �c�[���o�[���N���b�N���ꂽ�Ƃ��Ɏ��s�����R�}���h�ł��B
        /// </ja>
        /// <en>
        /// Command executed when toolbar is clicked.
        /// </en>
        /// </summary>
        /// <remarks>
        /// <ja>
        /// �R�}���h�̓R���X�g���N�^�ɂ���Đݒ肳��A<seealso cref="Command">Command�v���p�e�B</seealso>�ŕԂ���܂��B
        /// </ja>
        /// <en>
        /// The command is set by the constractor, and return by <seealso cref="Command">Command property</seealso>.
        /// </en>
        /// </remarks>
        protected IPoderosaCommand _command;
        /// <summary>
        /// <ja>
        /// �A�C�R����ێ���������ϐ��ł��B�A�C�R���̑傫����16�~16�s�N�Z���łȂ���΂Ȃ�܂���B
        /// </ja>
        /// <en>
        /// Inside variable that holds the icon. The size of the icon should be 16�~16 pixels. 
        /// </en>
        /// </summary>
        /// <ja>
        /// �A�C�R���̓R���X�g���N�^�ɂ���Đݒ肳��A<seealso cref="Icon">Icon�v���p�e�B</seealso>�ŕԂ���܂��B
        /// </ja>
        /// <en>
        /// The icon is set by the constractor, and return by <seealso cref="Command">Command property</seealso>.
        /// </en>
        protected Image _icon;

        /// <summary>
        /// <ja>
        /// �c�[���o�[�̃{�^�����쐬���܂��B
        /// </ja>
        /// <en>
        /// Create the button of the toolbar.
        /// </en>
        /// </summary>
        /// <remarks>
        /// <ja>
        /// �����Ȃ��̃R���X�g���N�^�ł́A�����R�}���h�����s���ꂸ�A�A�C�R�����ݒ肳��܂���B
        /// </ja>
        /// <en>
        /// For the constructor who doesn't have the argument, as for anything, the command is not executed, and the icon is not set. 
        /// </en>
        /// </remarks>
        public ToolBarCommandButtonImpl() {
        }
        /// <summary>
        /// <ja>
        /// �c�[���o�[�̃{�^�����쐬���܂��B
        /// </ja>
        /// <en>
        /// Create the button on the toolbar.
        /// </en>
        /// </summary>
        /// <param name="command"><ja>�{�^�����N���b�N���ꂽ�Ƃ��Ɏ��s�����R�}���h�ł��B</ja><en>Command that execuses when the button is clicked</en></param>
        /// <param name="icon"><ja>�{�^���ɕ\������A�C�R���ł��B�A�C�R���̑傫����16�~16�h�b�g�łȂ���΂Ȃ�܂���</ja>
        /// <en>
        /// Icon that show on the button. The size of the icon should be 16�~16 pixels. 
        /// </en></param>
        public ToolBarCommandButtonImpl(IPoderosaCommand command, Image icon) {
            _command = command;
            _icon = icon;
        }

        /// <summary>
        /// <ja>
        /// �{�^�����N���b�N���ꂽ�Ƃ��Ɏ��s����R�}���h�������܂��B
        /// </ja>
        /// <en>
        /// The command executed when the button is clicked is shown. 
        /// </en>
        /// </summary>
        public virtual IPoderosaCommand Command {
            get {
                return _command;
            }
        }

        /// <summary>
        /// <ja>
        /// �{�^���ɕ\������A�C�R���ł��B
        /// </ja>
        /// <en>
        /// Icon displayed in button
        /// </en>
        /// </summary>
        public virtual Image Icon {
            get {
                return _icon;
            }
        }
    }

    /// <summary>
    /// <ja>
    /// �c�[���o�[�v�f�̃R���{�{�b�N�X���\������@�\��񋟂��܂��B
    /// </ja>
    /// <en>
    /// Offers the function to compose the combobox of the toolbar element. 
    /// </en>
    /// </summary>
    /// <remarks>
    /// <ja>
    /// <para>
    /// �J���҂͂��̃N���X��p���邱�ƂŁA<seealso cref="IToolBarComboBox">IToolBarComboBox</seealso>
    /// ��������I�u�W�F�N�g���\���ł��܂��B
    /// </para>
    /// <para>
    /// ���̃N���X�͒��ۃN���X�ł���A�ЂȌ`�ɂ����܂���B�K�v�ɉ����ăI�[�o�[���C�h���K�v�ł��B
    /// </para>
    /// </ja>
    /// <en>
    /// <para>
    /// The developer can compose the object that has <seealso cref="IToolBarComboBox">IToolBarComboBox</seealso> by using this class. 
    /// </para>
    /// <para>
    /// This class is an abstraction class. Therefore, this is a model. Override is necessary if necessary. 
    /// </para>
    /// </en>
    /// </remarks>
    public abstract class ToolBarComboBoxImpl : ToolBarElementImpl, IToolBarComboBox {
        /// <summary>
        /// <ja>
        /// �R���{�{�b�N�X�̑I�����i�A�C�e���j�������ϐ��ł��B
        /// </ja>
        /// <en>
        /// Variable that shows item of combobox.
        /// </en>
        /// </summary>
        /// <remarks>
        /// <ja>
        /// ���̒l�́A<seealso cref="Items">Items�v���p�e�B</seealso>����Ԃ���܂��B
        /// </ja>
        /// <en>
        /// This variable is returned from <seealso cref="Items">Items property</seealso>.
        /// </en>
        /// </remarks>
        protected object[] _items;

        /// <summary>
        /// <ja>
        /// �R���{�{�b�N�X�̕��ł��B�P�ʂ̓s�N�Z���ł��B
        /// </ja>
        /// <en>
        /// Width of the combobox. This unit is pixel.
        /// </en>
        /// </summary>
        /// <remarks>
        /// <ja>
        /// ���̒l�́A<seealso cref="Width">Width�v���p�e�B</seealso>����Ԃ���܂��B
        /// </ja>
        /// <en>
        /// This variable is returned from <seealso cref="Items">Width property</seealso>.
        /// </en>
        /// </remarks>
        protected int _width;

        /// <summary>
        /// <ja>
        /// �R���{�{�b�N�X�̑I�����i�A�C�e���j�������܂��B
        /// </ja>
        /// <en>
        /// The item of the combobox is shown. 
        /// </en>
        /// </summary>
        public virtual object[] Items {
            get {
                return _items;
            }
        }

        /// <summary>
        /// <ja>
        /// �R���{�{�b�N�X�̕��ł��B�P�ʂ̓s�N�Z���ł��B
        /// </ja>
        /// <en>
        /// Return the width of the combobox. The unit is a pixel. 
        /// </en>
        /// </summary>
        public virtual int Width {
            get {
                return _width;
            }
        }

        /// <summary>
        /// <ja>
        /// �R���{�{�b�N�X���I���ł��邩�ǂ����������܂��B
        /// </ja>
        /// <en>
        /// It is shown whether the combobox can be selected. 
        /// </en>
        /// </summary>
        /// <param name="target"><ja>���s�̑ΏۂƂȂ�^�[�Q�b�g�ł��B</ja><en>Target for execution</en></param>
        /// <returns><ja>�I���ł���Ƃ��ɂ�true�A�����łȂ��Ƃ��ɂ�false���Ԃ���܂��B</ja><en>False is returned when it is not true so when it is possible to select it. </en></returns>
        /// <remarks>
        /// <ja>
        /// <paramref name="target">target</paramref>�́A���̃c�[���o�[��������<see cref="IPoderosaMainWindow">IPoderosaMainWindow</see>�ł��B
        /// </ja>
        /// <en>
        /// <paramref name="target">target</paramref> is <see cref="IPoderosaMainWindow">IPoderosaMainWindow</see> that this toolbar belongs. 
        /// </en>
        /// </remarks>
        public virtual bool IsEnabled(ICommandTarget target) {
            return true;
        }

        /// <summary>
        /// <ja>
        /// ���ݑI������Ă���A�C�e���̃C���f�b�N�X�ԍ���Ԃ��܂��B
        /// </ja>
        /// <en>
        /// The index number of the item that has been selected now is returned. 
        /// </en>
        /// </summary>
        /// <param name="target"><ja>���s�̑ΏۂƂȂ�^�[�Q�b�g�ł��B</ja><en>Target for execution</en></param>
        /// <returns><ja><paramref name="target">target</paramref>�̃C���f�b�N�X�ʒu��Ԃ��܂��B</ja>
        /// <en>Return index position of the <paramref name="target">target</paramref></en>
        /// </returns>
        /// <remarks>
        /// <ja>
        /// <paramref name="target">target</paramref>�́A���̃c�[���o�[��������<see cref="IPoderosaMainWindow">IPoderosaMainWindow</see>�ł��B
        /// </ja>
        /// <en>
        /// <paramref name="target">target</paramref> is <see cref="IPoderosaMainWindow">IPoderosaMainWindow</see> that this toolbar belongs. 
        /// </en>
        /// </remarks>
        public abstract int GetSelectedIndex(ICommandTarget target);
        /// <summary>
        /// <ja>
        /// �R���{�{�b�N�X�őI������Ă���I�������ω������Ƃ��ɌĂяo����郁�\�b�h�ł��B
        /// </ja>
        /// <en>
        /// It is a method of the call when choices that have been selected in the combobox change. 
        /// </en>
        /// </summary>
        /// <param name="target"><ja>���s�̑ΏۂƂȂ�^�[�Q�b�g�ł��B</ja><en>Target for execution</en></param>
        /// <param name="selectedIndex"><ja>���[�U�[���I�������A�C�e���̃C���f�b�N�X�ԍ��ł��B</ja><en>It is an index number of the item that the user selected. </en></param>
        /// <param name="selectedItem"><ja>���[�U�[���I�������A�C�e���̃I�u�W�F�N�g�ł��B</ja><en>An object of the item that the user selected. </en></param>
        /// <remarks>
        /// <ja>
        /// <paramref name="target">target</paramref>�́A���̃c�[���o�[��������<see cref="IPoderosaMainWindow">IPoderosaMainWindow</see>�ł��B
        /// </ja>
        /// <en>
        /// <paramref name="target">target</paramref> is <see cref="IPoderosaMainWindow">IPoderosaMainWindow</see> that this toolbar belongs. 
        /// </en>
        /// </remarks>
        public virtual void OnChange(ICommandTarget target, int selectedIndex, object selectedItem) {
        }
    }

    /// <summary>
    /// <ja>
    /// �c�[���o�[�v�f�̃g�O���{�^�����\������@�\��񋟂��܂��B
    /// </ja>
    /// <en>
    /// Offers the function to compose the toggle button of the toolbar element.
    /// </en>
    /// </summary>
    /// <remarks>
    /// <ja>
    /// �J���҂͂��̃N���X��p���邱�ƂŁA<seealso cref="IToolBarToggleButton">IToolBarToggleButton</seealso>
    /// ��������I�u�W�F�N�g���\���ł��܂��B
    /// </ja>
    /// <en>
    /// The developer can compose the object that has <seealso cref="IToolBarToggleButton">IToolBarToggleButton</seealso> by using this class. 
    /// </en>
    /// </remarks>
    public abstract class ToolBarToggleButtonImpl : ToolBarElementImpl, IToolBarToggleButton {
        /// <summary>
        /// <ja>
        /// �A�C�R�������������ϐ��ł��B�A�C�R���̑傫����16�~16�s�N�Z���łȂ���΂Ȃ�܂���B
        /// </ja>
        /// <en>
        /// Inside variable that holds the icon. The size of the icon should be 16�~16 pixels. 
        /// </en>
        /// </summary>
        protected Image _icon;

        /// <summary>
        /// <ja>
        /// �A�C�R����Ԃ��܂��B
        /// </ja>
        /// <en>
        /// Return the icon.
        /// </en>
        /// </summary>
        public virtual Image Icon {
            get {
                return _icon;
            }
        }

        /// <summary>
        /// <ja>
        /// �g�O���{�^�����I���ł��邩�ǂ����������܂��B
        /// </ja>
        /// <en>
        /// It is shown whether the toggle button can be selected. 
        /// </en>
        /// </summary>
        /// <param name="target"><ja>���s�̑ΏۂƂȂ�^�[�Q�b�g�ł��B</ja><en>Target for execution</en></param>
        /// <returns><ja>�I���ł���Ƃ��ɂ�true�A�����łȂ��Ƃ��ɂ�false���Ԃ���܂��B</ja><en>False is returned when it is not true so when it is possible to select it. </en></returns>
        /// <remarks>
        /// <ja>
        /// <paramref name="target">target</paramref>�́A���̃c�[���o�[��������<see cref="IPoderosaMainWindow">IPoderosaMainWindow</see>�ł��B
        /// </ja>
        /// <en>
        /// <paramref name="target">target</paramref> is <see cref="IPoderosaMainWindow">IPoderosaMainWindow</see> that this toolbar belongs. 
        /// </en>
        /// </remarks>
        public virtual bool IsEnabled(ICommandTarget target) {
            return true;
        }

        /// <summary>
        /// <ja>
        /// �g�O���{�^���̃I���^�I�t�̏�Ԃ�Ԃ��܂��B
        /// </ja>
        /// <en>
        /// Return the state of on/off of the toggle button.
        /// </en>
        /// </summary>
        /// <param name="target"><ja>���s�̑ΏۂƂȂ�^�[�Q�b�g�ł��B</ja><en>Target for execution</en></param>
        /// <returns><ja>�I���̂Ƃ��i����ł���Ƃ��j�ɂ�true�A�I�t�̂Ƃ��i����ł��Ȃ��Ƃ��j�ɂ�false��Ԃ��܂��B</ja><en>False is returned when true off (When not denting) when turning it on (When denting). </en></returns>
        /// <remarks>
        /// <ja>
        /// <paramref name="target">target</paramref>�́A���̃c�[���o�[��������<see cref="IPoderosaMainWindow">IPoderosaMainWindow</see>�ł��B
        /// </ja>
        /// <en>
        /// <paramref name="target">target</paramref> is <see cref="IPoderosaMainWindow">IPoderosaMainWindow</see> that this toolbar belongs. 
        /// </en>
        /// </remarks>
        public virtual bool IsChecked(ICommandTarget target) {
            return false;
        }

        /// <summary>
        /// <ja>
        /// �I���^�I�t�̏�Ԃ��ω������Ƃ��ɌĂяo����郁�\�b�h�ł��B
        /// </ja>
        /// <en>
        /// It is a method of the call when the state of on/off changes. 
        /// </en>
        /// </summary>
        /// <param name="target"><ja>���s�̑ΏۂƂȂ�^�[�Q�b�g�ł��B</ja><en>Target for execution</en></param>
        /// <param name="is_checked"><ja>�I���^�I�t�̏�Ԃł��Btrue�̂Ƃ��ɂ̓I���i����ł����ԁj�Afalse�̂Ƃ��ɂ̓I�t�i����ł��Ȃ���ԁj�ł��B</ja><en>It is a state of on/off. At on (state that has dented) and false at true, it is off (state that doesn't dent). </en></param>
        /// <remarks>
        /// <ja>
        /// <paramref name="target">target</paramref>�́A���̃c�[���o�[��������<see cref="IPoderosaMainWindow">IPoderosaMainWindow</see>�ł��B
        /// </ja>
        /// <en>
        /// <paramref name="target">target</paramref> is <see cref="IPoderosaMainWindow">IPoderosaMainWindow</see> that this toolbar belongs. 
        /// </en>
        /// </remarks>
        public abstract void OnChange(ICommandTarget target, bool is_checked);
    }
}
