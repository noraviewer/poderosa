/*
 * Copyright 2004,2006 The Poderosa Project.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *
 * $Id: PluginEx.cs,v 1.2 2011/10/27 23:21:56 kzmi Exp $
 */
using System;
using System.Collections.Generic;

namespace Poderosa.Plugins {

    /// <summary>
    /// <ja>
    /// �v���O�C���̑�����ݒ肵�܂��B
    /// </ja>
    /// <en>
    /// Set the attribute of the plug-in
    /// </en>
    /// </summary>
    /// <remarks>
    /// <ja>
    /// ���ׂẴv���O�C���́APluginInfoAttribute����������Ȃ���΂Ȃ�܂���B
    /// </ja>
    /// <en>
    /// All plug-ins must have the PluginInfoAttribute attribute.
    /// </en>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class PluginInfoAttribute : Attribute {
        /// <summary>
        /// <ja>�v���O�C���̎��ʎq�ƂȂ�u�v���O�C��ID�v�ł��B�K�{�ł��B</ja>
        /// <en>REQUIRED:Plug-in that identifies the plug-in.</en>
        /// </summary>
        /// <remarks>
        /// <ja>
        /// ���ׂẴv���O�C���ŗB�ꖳ��̂��̂��w�肷��K�v������܂��B
        /// ���ʎq�́AJava�̃p�b�P�[�W�W���ɏ����������Ŏ��ʂ���܂��B
        /// �J���҂��ۗL����h���C����������Ȃ�΁A����Ɋ�Â��ăv���O�C��ID���߂Ă��������i���Ƃ��΁A�ujp.co.example.�C�Ӗ��v�Ȃǁj �B
        /// Poderosa�W���̃v���O�C����ID�����ł́A�uorg.poderosa�v���g���Ă��܂��B�J���҂��Ǝ��̃v���O�C�����쐬����ۂɂ́A�uorg.poderosa�v�ȉ���ID�l��t���Ă͂����܂���B�uorg.poderosa�v�ȉ���ID�l��t����ꍇ�ɂ́APoderosa�J���҃R�~���j�e�B�ł̏��F��v���܂��B 
        /// </ja>
        /// <en>
        /// It is necessary to specify the unique one by all plug-ins. 
        /// The identifier is identified by the method based on the package standard of Java. 
        /// Please provide plug-in ID based on it if there is a domain name that the developer has (for instance, "jp.co.example.foo" etc.). 
        ///In the ID attribute of the plug-in of the Poderosa standard, "org.poderosa" is used.
        /// Do not set ID following "org.poderosa" when the developer makes an original plug-in. "org.poderosa"
        /// When ID is set, approval in the Poderosa developer community is required. 
        /// </en>
        /// </remarks>
        public string ID;
        /// <summary>
        /// <ja>�v���O�C���̖��̂ł�</ja>
        /// <en>Name of the plug-in.</en>
        /// </summary>
        public string Name;
        /// <summary>
        /// <ja>�ˑ����鑼�̃v���O�C���̃v���O�C��ID�ł��B</ja>
        /// <en>The plug-in ID that depends other plug-ins. </en>
        /// </summary>
        /// <remarks>
        /// <ja>
        /// �ˑ����鑼�̃v���O�C��������ꍇ�ɂ́A���̃v���O�C��ID���Z�~�R�����i;�j�ŋ�؂��ė񋓂��܂��B
        /// �����ŗ񋓂����v���O�C��������ɓǂݍ��܂�邱�Ƃ��ۏ؂���܂��B
        /// </ja>
        /// <en>
        /// When other depending plug-ins exist, the plug-in ID is delimited by semicolon (;) and enumerated. 
        /// It is guaranteed to be read from the plug-in enumerated here back. 
        /// </en>
        /// </remarks>
        public string Dependencies;
        /// <summary>
        /// <ja>�v���O�C���̃o�[�W�����ԍ��ł��B</ja>
        /// <en>Version number of the plug-in.</en>
        /// </summary>
        public string Version;
        /// <summary>
        /// <ja>�v���O�C���̒���ҏ��ł�</ja>
        /// <en>Copyright information of the plug-in.</en>
        /// </summary>
        public string Author;
    }

    /// <summary>
    /// <ja>
    /// �v���O�C�����\������A�Z���u�������ׂ������ł��B
    /// </ja>
    /// <en>
    /// It is an attribute that the assembly that composes the plug-in should have. 
    /// </en>
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public class PluginDeclarationAttribute : Attribute {
        /// <summary>
        /// <ja>���̃A�Z���u���Ɋ܂܂��v���O�C���̃N���X���`���܂��B</ja>
        /// <en>
        /// Define the class of the plug-in included in this assembly.
        /// </en>
        /// </summary>
        /// <param name="type"><ja>�v���O�C���̃N���X�ł�</ja>
        /// <en>
        /// Class of plug-in.
        /// </en>
        /// </param>
        /// <remarks>
        /// <ja>
        /// Poderosa��<var>type</var>�Ɏw�肳�ꂽ�N���X�̃C���X�^���X�����A����InitializePlugin���\�b�h���Ăяo�����ƂŃv���O�C�������������A����\�ȏ�Ԃɂ��܂��B
        /// </ja>
        /// <en>
        /// The plug-in is initialized by making the instance of the class specified for type, 
        /// and calling the InitializePlugin method, and Poderosa is put into the state that can be operated. 
        /// </en>
        /// </remarks>
        public PluginDeclarationAttribute(Type type) {
            Target = type;
        }
        /// <summary>
        /// <ja>�v���O�C�����\������N���X�ł��B</ja>
        /// <en>Class that composes plug-in</en>
        /// </summary>
        public Type Target;
    }

    /// <summary>
    /// <ja>
    /// ���ׂẴv���O�C�����������Ȃ���΂Ȃ�Ȃ��C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface that all plug-ins should implement
    /// </en>
    /// </summary>
    /// <remarks>
    /// <ja>
    /// �J���҂́AIPlugin�C���^�[�t�F�C�X�������������ɁA<seealso cref="PluginBase">PluginBase�N���X</seealso>
    /// ����p�������N���X�Ƃ��č쐬���邱�Ƃ��ł��܂��B
    /// </ja>
    /// <en>
    /// The developer can make it as a class that inheritances to from the 
    /// <seealso cref="PluginBase">PluginBase class</seealso> instead of implementing the IPlugin interface. 
    /// </en>
    /// </remarks>
    public interface IPlugin : IAdaptable {
        /// <summary>
        /// <ja>
        /// �v���O�C���������������ۂɌĂяo����郁�\�b�h�ł��B
        /// </ja>
        /// <en>
        /// Method of call when plug-in is initialized
        /// </en>
        /// </summary>
        /// <param name="poderosa">
        /// <ja>
        /// Poderosa�{�̂ƒʐM���邽�߂�IPoderosaWorld�C���^�[�t�F�C�X�ł��B
        /// </ja>
        /// <en>
        /// IPoderosaWorld interface to communicate with Poderosa
        /// </en>
        /// </param>
        /// <remarks>
        /// <ja>
        /// ���̃��\�b�h�́APoderosa�{�̂ɂ���ăv���O�C�����ǂݍ��܂ꂽ����ɌĂяo����܂��B<br/>
        /// �����n�����IPoderosaWorld�C���^�[�t�F�C�X�̓v���O�C������������܂ŕs�ςł��B<br/>
        /// �v���O�C���J���҂́A���̃��\�b�h���Ńv���O�C���̏��������������邱�ƂɂȂ�܂��B
        /// </ja>
        /// <en>
        /// This method is called immediately after the plug-in was read by Poderosa. 
        /// The IPoderosaWorld interface handed over is invariable until the plug-in is relesed.
        /// The developer will do the initialization of the plug-in in this method. 
        /// </en>
        /// </remarks>
        void InitializePlugin(IPoderosaWorld poderosa);
        /// <summary>
        /// <ja>
        /// �v���O�C�����������钼�O�ɌĂяo����郁�\�b�h�ł��B
        /// </ja>
        /// <en>
        /// Method of call immediately before plug-in is released.
        /// </en>
        /// </summary>
        /// <remarks>
        /// <ja>
        /// �v���O�C���J���҂́A���̃��\�b�h���Ńv���O�C���̌㏈�������Ă��������B
        /// </ja>
        /// <en>
        /// The developer must postprocess the plug-in in this method. 
        /// </en>
        /// </remarks>
        void TerminatePlugin();
    }

    /// <summary>
    /// <ja>
    /// �v���O�C���𓝊��Ǘ�����u�v���O�C���}�l�[�W���v�̃C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface of "Plug-in manager" that manages generalization as for the plug-in. 
    /// </en>
    /// </summary>
    /// <remarks>
    /// <ja>
    /// IPluginManager�́A<seealso cref="IPoderosaWorld">IPoderosaWorld</seealso>��<see cref="IPoderosaWorld.PluginManager">PluginManager�v���p�e�B</see>����擾�ł��܂��B
    /// </ja>
    /// <en>
    /// IPluginManager can be acquired from the <see cref="IPoderosaWorld.PluginManager">PluginManager property</see> of 
    /// <seealso cref="IPoderosaWorld">IPoderosaWorld</seealso>. 
    /// </en>
    /// </remarks>
    public interface IPluginManager : IAdaptable {
        //Plugins
        /// <summary>
        /// <ja>�v���O�C�����������܂��B</ja>
        /// <en>Retrieval of the plug-in.</en>
        /// </summary>
        /// <param name="id">
        /// <ja>��������v���O�C��ID�ł��B
        /// </ja>
        /// <en>Retrieved plug-in ID
        /// </en>
        /// </param>
        /// <param name="adapter">
        /// <ja>
        /// �擾����v���O�C���̃C���^�[�t�F�C�X�̌^�ł��B
        /// </ja>
        /// <en>
        /// Type in interface of acquired plug-in.
        /// </en>
        /// </param>
        /// <returns>
        /// <ja>
        /// ���������v���O�C���̃C���^�[�t�F�C�X��Ԃ��܂��B�Y���̃v���O�C����������Ȃ������ꍇ�ɂ́Anull���߂�܂��B
        /// </ja>
        /// <en>
        /// The interface of the found plug-in is returned. Null returns when the plug-in of the correspondence is not found. 
        /// </en>
        /// </returns>
        object FindPlugin(string id, Type adapter);
        //Extension Points
        /// <summary>
        /// <ja>
        /// �g���|�C���g���쐬���܂��B
        /// </ja>
        /// <en>
        /// Making of the extension point.
        /// </en>
        /// </summary>
        /// <param name="id">
        /// <ja>
        /// �쐬����g���|�C���g�́u�g���|�C���gID�v�ł��B
        /// </ja>
        /// <en>
        /// Extension point ID of made extension point
        /// </en>
        /// </param>
        /// <param name="requiredInterface">
        /// <ja>
        /// �g���|�C���g���v������C���^�[�t�F�C�X�ł��B
        /// </ja>
        /// <en>
        /// Interface that extension point demands.
        /// </en>
        /// </param>
        /// <param name="owner">
        /// <ja>
        /// �g���|�C���g�̏��L�҂ƂȂ�v���O�C���̃I�u�W�F�N�g�ł��B�����̏ꍇ�A�uthis�v��n���܂��B
        /// </ja>
        /// <en>
        /// It is an object of the plug-in that becomes the owner of the extension point.
        /// In many cases, "this" is passed. 
        /// </en>
        /// </param>
        /// <returns>
        /// <ja>
        /// ����Ɋg���|�C���g���쐬���ꂽ�ꍇ�A�쐬���ꂽ�g���|�C���g��IExtensionPoint�C���^�[�t�F�C�X���߂�܂��B
        /// �g���|�C���g�̍쐬�Ɏ��s�����ꍇ�ɂ́Anull���߂�܂��B
        /// </ja>
        /// <en>
        /// The IExtensionPoint interface of the made extension point returns when the extension point is normally made. 
        /// Null returns when failing in making the extension point. 
        /// </en>
        /// </returns>
        IExtensionPoint CreateExtensionPoint(string id, Type requiredInterface, IPlugin owner);
        /// <summary>
        /// <ja>
        /// �g���|�C���g���������܂��B
        /// </ja>
        /// <en>
        /// Retrieval of the extension point.
        /// </en>
        /// </summary>
        /// <param name="id">
        /// <ja>
        /// ��������g���|�C���gID�ł��B
        /// </ja>
        /// <en>
        /// Retrieved extension point ID
        /// </en>
        /// </param>
        /// <returns>
        /// <ja>
        /// �Y���̊g���|�C���g�����������ꍇ�ɂ́A����IExtensionPoint�C���^�[�t�F�C�X���߂�܂��B
        /// �g���|�C���g��������Ȃ������ꍇ�ɂ́Anull���߂�܂��B
        /// </ja>
        /// <en>
        /// The IExtensionPoint interface returns when the extension point of the correspondence is found. 
        /// Null returns when the enhancing point is not found. 
        /// </en>
        /// </returns>
        IExtensionPoint FindExtensionPoint(string id);
    }

    /// <summary>
    /// <ja>
    /// �g���|�C���g�������C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface that shows extension point.
    /// </en>
    /// </summary>
    public interface IExtensionPoint {
        /// <summary>
        /// <ja>
        /// �g���|�C���g�����L����v���O�C����IPlugin�C���^�[�t�F�C�X�ł��B
        /// </ja>
        /// <en>
        /// IPlugin interface of plug-in to own extension point.
        /// </en>
        /// </summary>
        IPlugin OwnerPlugin {
            get;
        }
        /// <summary>
        /// <ja>
        /// �g���|�C���gID�ł��B
        /// </ja>
        /// <en>
        /// Extension point ID.
        /// </en>
        /// </summary>
        string ID {
            get;
        }
        /// <summary>
        /// <ja>
        /// �g���|�C���g���v������C���^�[�t�F�C�X�ł��B
        /// </ja>
        /// <en>
        /// Interface that entension point demands
        /// </en>
        /// </summary>
        Type ExtensionInterface {
            get;
        }
        /// <summary>
        /// <ja>
        /// �g���|�C���g�ɃI�u�W�F�N�g��o�^���܂��B
        /// </ja>
        /// <en>
        /// The object is registered in the extension point. 
        /// </en>
        /// </summary>
        /// <param name="extension">
        /// <ja>
        /// �o�^����I�u�W�F�N�g�ł��B���̃I�u�W�F�N�g��ExtensionInterface�v���p�e�B�Ŏw�肳���
        /// �C���^�[�t�F�C�X������Ă��Ȃ���΂Ȃ�܂���B
        /// </ja>
        /// <en>
        /// It is a registered object. This object should have the interface specified in the ExtensionInterface property. 
        /// </en>
        /// </param>
        /// <exception cref="ArgumentException">
        /// <ja>
        /// extension�Ɏw�肳�ꂽ�I�u�W�F�N�g��ExtensionInterface�v���p�e�B�Ŏw�肳���C���^�[�t�F�C�X������Ă��܂���B
        /// </ja>
        /// <en>
        /// The interface for which the object specified for extension is specified in the ExtensionInterface property is not provided with. 
        /// </en>
        /// </exception>
        void RegisterExtension(object extension);
        /// <summary>
        /// <ja>
        /// ���̊g���|�C���g�ɓo�^����Ă���I�u�W�F�N�g�̔z����擾���܂��B
        /// </ja>
        /// <en>
        /// Get the array of the object registered in this extension point.
        /// </en>
        /// </summary>
        /// <returns>
        /// <ja>
        /// ���̊g���|�C���g�ɓo�^����Ă���I�u�W�F�N�g�̔z�񂪕Ԃ���܂��B
        /// </ja>
        /// <en>
        /// The array of the object registered in this extension point is returned. 
        /// </en>
        /// </returns>
        Array GetExtensions();
    }

    //�ŏ���ExtensionPoint�p�̃C���^�t�F�[�X
    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public interface IRootExtension {
        void InitializeExtension();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public interface IGUIMessageLoop : IRootExtension {
        void RunExtension();
    }

    //Plugin Inspector�p��
    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public interface IPluginInspector : IAdaptable {
        IEnumerable<IPluginInfo> Plugins {
            get;
        }
        IEnumerable<IExtensionPoint> ExtensionPoints {
            get;
        }
        IPluginInfo GetPluginInfo(IPlugin plugin);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public interface IPluginInfo : IAdaptable {
        IPlugin Body {
            get;
        }
        PluginInfoAttribute PluginInfoAttribute {
            get;
        }
        PluginStatus Status {
            get;
        }
    }

    /// <summary>
    /// <ja>
    /// �v���O�C���J���҂ɁAIPlugin�C���^�[�t�F�C�X��IAdaptable�C���^�[�t�F�C�X�̕W��������񋟂��܂��B
    /// </ja>
    /// <en>
    /// A default implementation in the IPlugin interface and the IAdaptable interface is offered to the developer. 
    /// </en>
    /// </summary>
    /// <remarks>
    /// <ja>
    /// �v���O�C���J���҂́A���̃N���X����p�������邱�Ƃŏ��Ȃ��R�[�h�Ńv���O�C�����������Ƃ��ł��܂��B
    /// ���L�̎����ɂȂ��Ă��܂��B
    /// <code>
    /// public abstract class PluginBase : MarshalByRefObject, IPlugin
    /// {
    ///    protected IPoderosaWorld _poderosaWorld;
    ///    public virtual void InitializePlugin(IPoderosaWorld poderosa)
    ///    {
    ///        poderosaWorld = poderosa;
    ///    }
    ///    public IPoderosaWorld PoderosaWorld
    ///    {
    ///        get
    ///        {
    ///            return _poderosaWorld;
    ///        }
    ///    }
    ///    public virtual void TerminatePlugin()
    ///    {
    ///    }
    ///    public virtual IAdaptable GetAdapter(Type adapter)
    ///    {
    ///        return _poderosaWorld.AdapterManager.GetAdapter(this, adapter);
    ///    }
    /// }
    /// </code>
    /// </ja>
    /// <en>
    /// The plug-in developer can write the plug-in by a little code by making it inheritances to from this class. 
    /// It is the following implementation. 
    /// <code>
    /// public abstract class PluginBase : MarshalByRefObject, IPlugin
    /// {
    ///    protected IPoderosaWorld _poderosaWorld;
    ///    public virtual void InitializePlugin(IPoderosaWorld poderosa)
    ///    {
    ///        poderosaWorld = poderosa;
    ///    }
    ///    public IPoderosaWorld PoderosaWorld
    ///    {
    ///        get
    ///        {
    ///            return _poderosaWorld;
    ///        }
    ///    }
    ///    public virtual void TerminatePlugin()
    ///    {
    ///    }
    ///    public virtual IAdaptable GetAdapter(Type adapter)
    ///    {
    ///        return _poderosaWorld.AdapterManager.GetAdapter(this, adapter);
    ///    }
    /// }
    /// </code>
    /// </en>
    /// </remarks>
    public abstract class PluginBase : MarshalByRefObject, IPlugin {
        /// <summary>
        /// <ja>
        /// �������̂Ƃ��Ɏ󂯎����IPoderosaWorld�C���^�[�t�F�C�X��ێ����܂��B
        /// </ja>
        /// <en>
        /// The IPoderosaWorld interface received when initializing it is maintained. 
        /// </en>
        /// </summary>
        protected IPoderosaWorld _poderosaWorld;
        /// <summary>
        /// <ja>
        /// �v���O�C���̏������̍ۂɌĂяo����܂��B�f�t�H���g�̎����ł́A_poderosaWorld�Ɏ󂯎����IPoderosaWorld�C���^�[�t�F�C�X��ۑ����܂��B
        /// </ja>
        /// <en>
        /// When the plug-in is initialized, it is called.
        /// In default implementation, the IPoderosaWorld interface received in _poderosaWorld is preserved. 
        /// </en>
        /// </summary>
        /// <param name="poderosa">
        /// <ja>Poderosa�{�̂��n�����IPoderosaWorld�C���^�[�t�F�C�X</ja>
        /// <en>IPoderosaWorld interface to which Poderosa is passed</en>
        /// </param>
        public virtual void InitializePlugin(IPoderosaWorld poderosa) {
            _poderosaWorld = poderosa;
        }

        /// <summary>
        /// <ja>
        /// Poderosa�{�̂ƒʐM���邽�߂�IPoderosaWorld�C���^�[�t�F�C�X��Ԃ��܂��B
        /// </ja>
        /// <en>
        /// The IPoderosaWorld interface to communicate with Poderosa is returned. 
        /// </en>
        /// </summary>
        public IPoderosaWorld PoderosaWorld {
            get {
                return _poderosaWorld;
            }
        }

        /// <summary>
        /// <ja>
        /// �v���O�C������������O�ɌĂяo����܂��B
        /// </ja>
        /// <en>
        /// It is called before the plug-in is released. 
        /// </en>
        /// </summary>
        public virtual void TerminatePlugin() {
        }

        public virtual IAdaptable GetAdapter(Type adapter) {
            return _poderosaWorld.AdapterManager.GetAdapter(this, adapter);
        }
    }

}
