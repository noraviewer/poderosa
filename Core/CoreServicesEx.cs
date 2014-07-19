/*
 * Copyright 2004,2006 The Poderosa Project.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *
 * $Id: CoreServicesEx.cs,v 1.2 2011/10/27 23:21:55 kzmi Exp $
 */
using System;
using System.Collections.Generic;
using System.Text;

using Poderosa.Forms;
using Poderosa.Preferences;
using Poderosa.Commands;
using Poderosa.Serializing;
using Poderosa.Sessions;
using Poderosa.Boot;

namespace Poderosa.Plugins {
    //�p�o�@�\�ւ̃A�N�Z�T�BPoderosaWorld����GetAdapter�ł���悤�ɂ��Ă��܂���I
    /// <summary>
    /// <ja>
    /// �W���v���O�C�����������\�I�ȃC���^�[�t�F�C�X��g���|�C���g��Ԃ��v���p�e�B��񋟂��܂��B
    /// </ja>
    /// <en>
    /// Property that returns a typical interface and the extension point with which a standard plug-in provides is offered. 
    /// </en>
    /// </summary>
    /// <remarks>
    /// <ja>
    /// ICoreServices�C���^�[�t�F�C�X�́A<seealso cref="IPoderosaWorld">IPoderosaWorld�C���^�[�t�F�C�X</seealso>
    /// ��<see cref="IAdaptable.GetAdapter">GetAdapter���\�b�h</see>�Ŏ擾�ł��܂��B
    /// </ja>
    /// <en>
    /// The ICoreServices interface can be got in the <see cref="IAdaptable.GetAdapter">GetAdapter method</see> of the <seealso cref="IPoderosaWorld">IPoderosaWorld interface</seealso>. 
    /// </en>
    /// </remarks>
    /// <example>
    /// <ja>
    /// ICoreServices���擾���܂��B
    /// <code>
    /// ICoreServices cs = PoderosaWorld.GetAdapter(typeof(ICoreServices));
    /// // IWindowManager�C���^�[�t�F�C�X���擾���܂�
    /// IWindowManager wm = cs.WindowManager;
    /// </code>
    /// </ja>
    /// <en>
    /// Get ICoreServices
    /// <code>
    /// ICoreServices cs = PoderosaWorld.GetAdapter(typeof(ICoreServices));
    /// // Get IWindowManager interface.
    /// IWindowManager wm = cs.WindowManager;
    /// </code>
    /// </en>
    /// </example>
    public interface ICoreServices : IAdaptable {
        /// <summary>
        /// <ja>
        /// IWindowManager�C���^�[�t�F�C�X��Ԃ��܂��B
        /// </ja>
        /// <en>
        /// Return IWindowManager interface.
        /// </en>
        /// </summary>
        /// <remarks>
        /// <ja>
        /// <seealso cref="IPluginManager">IPluginManager</seealso>��<see cref="IPluginManager.FindPlugin">FindPlugin���\�b�h</see>
        /// ���g���āuorg.poderosa.core.windows�v����������̂Ɠ����ł��B
        /// </ja>
        /// <en>
        /// It is the same as the retrieval of "org.poderosa.core.windows" by using the <see cref="IPluginManager.FindPlugin">FindPlugin method</see> of <seealso cref="IPluginManager">IPluginManager</seealso>. 
        /// </en>
        /// </remarks>
        IWindowManager WindowManager {
            get;
        }
        /// <summary>
        /// <ja>
        /// IPreferences�C���^�[�t�F�C�X��Ԃ��܂��B
        /// </ja>
        /// <en>
        /// Return IPreferences interface.
        /// </en>
        /// </summary>
        /// <remarks>
        /// <ja>
        /// <seealso cref="IPluginManager">IPluginManager</seealso>��<see cref="IPluginManager.FindPlugin">FindPlugin���\�b�h</see>
        /// ���g���āuorg.poderosa.core.preferences�v����������̂Ɠ����ł��B
        /// </ja>
        /// <en>
        /// It is the same as the retrieval of "org.poderosa.core.preferences" by using the <see cref="IPluginManager.FindPlugin">FindPlugin method</see> of <seealso cref="IPluginManager">IPluginManager</seealso>. 
        /// </en>
        /// </remarks>
        IPreferences Preferences {
            get;
        }
        /// <summary>
        /// <ja>
        /// ICommandManager�C���^�[�t�F�C�X��Ԃ��܂��B
        /// </ja>
        /// <en>
        /// Return ICommandManager interface.
        /// </en>
        /// </summary>
        /// <remarks>
        /// <ja>
        /// <seealso cref="IPluginManager">IPluginManager</seealso>��<see cref="IPluginManager.FindPlugin">FindPlugin���\�b�h</see>
        /// ���g���āuorg.poderosa.core.commands�v����������̂Ɠ����ł��B
        /// </ja>
        /// <en>
        /// It is the same as the retrieval of "org.poderosa.core.commands" by using the <see cref="IPluginManager.FindPlugin">FindPlugin method</see> of <seealso cref="IPluginManager">IPluginManager</seealso>. 
        /// </en>
        /// </remarks>
        ICommandManager CommandManager {
            get;
        }
        /// <summary>
        /// <ja>
        /// ISessionManager�C���^�[�t�F�C�X��Ԃ��܂��B
        /// </ja>
        /// <en>
        /// Return ISessionManager interface.
        /// </en>
        /// </summary>
        /// <remarks>
        /// <ja>
        /// <seealso cref="IPluginManager">IPluginManager</seealso>��<see cref="IPluginManager.FindPlugin">FindPlugin���\�b�h</see>
        /// ���g���āuorg.poderosa.core.sessions�v����������̂Ɠ����ł��B
        /// </ja>
        /// <en>
        /// It is the same as the retrieval of "org.poderosa.core.sessions" by using the <see cref="IPluginManager.FindPlugin">FindPlugin method</see> of <seealso cref="IPluginManager">IPluginManager</seealso>. 
        /// </en>
        /// </remarks>
        ISessionManager SessionManager {
            get;
        }
        /// <summary>
        /// <ja>
        /// ISerializeService�C���^�[�t�F�C�X��Ԃ��܂��B
        /// </ja>
        /// <en>
        /// Return ISerializeService interface.
        /// </en>
        /// </summary>
        /// <remarks>
        /// <ja>
        /// <seealso cref="IPluginManager">IPluginManager</seealso>��<see cref="IPluginManager.FindPlugin">FindPlugin���\�b�h</see>
        /// ���g���āuorg.poderosa.core.serializing�v����������̂Ɠ����ł��B
        /// </ja>
        /// <en>
        /// It is the same as the retrieval of "org.poderosa.core.serializing" by using the <see cref="IPluginManager.FindPlugin">FindPlugin method</see> of <seealso cref="IPluginManager">IPluginManager</seealso>. 
        /// </en>
        /// </remarks>
        ISerializeService SerializeService {
            get;
        }

        //�ȉ��͕p�oExtensionPoint
        /// <summary>
        /// <ja>
        /// PreferencePlugin�v���O�C�����񋟂���g���|�C���g��Ԃ��܂��B
        /// </ja>
        /// <en>
        /// Return the extension point of the PreferencePlugin plug-in.
        /// </en>
        /// </summary>
        /// <remarks>
        /// <ja>
        /// <seealso cref="IPluginManager">IPluginManager</seealso>��<see cref="IPluginManager.FindExtensionPoint">FindExtensionPoint���\�b�h</see>
        /// ���g���āuorg.poderosa.core.preferences�v����������̂Ɠ����ł��B
        /// </ja>
        /// <en>
        /// It is the same as the retrieval of "org.poderosa.core.preferences" by using the <see cref="IPluginManager.FindExtensionPoint">FindExtensionPoint method</see> of <seealso cref="IPluginManager">IPluginManager</seealso>. 
        /// </en>
        /// </remarks>
        IExtensionPoint PreferenceExtensionPoint {
            get;
        }
        /// <summary>
        /// <ja>
        /// SerializeServicePlugin�v���O�C�����񋟂���g���|�C���g��Ԃ��܂��B
        /// </ja>
        /// <en>
        /// Return the extension point of the SerializeServicePlugin plug-in.
        /// </en>
        /// </summary>
        /// <remarks>
        /// <ja>
        /// <seealso cref="IPluginManager">IPluginManager</seealso>��<see cref="IPluginManager.FindExtensionPoint">FindExtensionPoint���\�b�h</see>
        /// ���g���āuorg.poderosa.core.serializeElement�v����������̂Ɠ����ł��B
        /// </ja>
        /// <en>
        /// It is the same as the retrieval of "org.poderosa.core.serializeElement" by using the <see cref="IPluginManager.FindExtensionPoint">FindExtensionPoint method</see> of <seealso cref="IPluginManager">IPluginManager</seealso>. 
        /// </en>
        /// </remarks>
        IExtensionPoint SerializerExtensionPoint {
            get;
        }
    }

    //���̎����Ɠo�^
    internal class CoreServices : ICoreServices {
        private IPoderosaWorld _world;
        private AF _adapterFactory;

        private IWindowManager _windowManager;
        private IPreferences _preferences;
        private ICommandManager _commandManager;
        private ISessionManager _sessionManager;
        private ISerializeService _serializeService;
        private IExtensionPoint _preferenceExtensionPoint;
        private IExtensionPoint _serializerExtensionPoint;

        public CoreServices(IPoderosaWorld world) {
            _world = world;
            _adapterFactory = new AF(_world, this);
            _world.AdapterManager.RegisterFactory(_adapterFactory);
        }

        public IWindowManager WindowManager {
            get {
                if (_windowManager == null)
                    _windowManager = (IWindowManager)_world.PluginManager.FindPlugin(WindowManagerPlugin.PLUGIN_ID, typeof(IWindowManager));
                return _windowManager;
            }
        }

        public IPreferences Preferences {
            get {
                if (_preferences == null)
                    _preferences = (IPreferences)_world.PluginManager.FindPlugin(PreferencePlugin.PLUGIN_ID, typeof(IPreferences));
                return _preferences;
            }
        }

        public ICommandManager CommandManager {
            get {
                if (_commandManager == null)
                    _commandManager = (ICommandManager)_world.PluginManager.FindPlugin(CommandManagerPlugin.PLUGIN_ID, typeof(ICommandManager));
                return _commandManager;
            }
        }

        public ISerializeService SerializeService {
            get {
                if (_serializeService == null)
                    _serializeService = (ISerializeService)_world.PluginManager.FindPlugin(SerializeServicePlugin.PLUGIN_ID, typeof(ISerializeService));
                return _serializeService;
            }
        }

        public ISessionManager SessionManager {
            get {
                if (_sessionManager == null)
                    _sessionManager = (ISessionManager)_world.PluginManager.FindPlugin(SessionManagerPlugin.PLUGIN_ID, typeof(ISessionManager));
                return _sessionManager;
            }
        }


        public IExtensionPoint PreferenceExtensionPoint {
            get {
                if (_preferenceExtensionPoint == null)
                    _preferenceExtensionPoint = (IExtensionPoint)_world.PluginManager.FindExtensionPoint(PreferencePlugin.EXTENSIONPOINT_NAME);
                return _preferenceExtensionPoint;
            }
        }

        public IExtensionPoint SerializerExtensionPoint {
            get {
                if (_serializerExtensionPoint == null)
                    _serializerExtensionPoint = (IExtensionPoint)_world.PluginManager.FindExtensionPoint(SerializeServicePlugin.EXTENSIONPOINT_NAME);
                return _serializerExtensionPoint;
            }
        }

        public IAdaptable GetAdapter(Type adapter) {
            return _world.AdapterManager.GetAdapter(this, adapter);
        }

        private class AF : IDualDirectionalAdapterFactory {
            private IPoderosaWorld _world;
            private CoreServices _coreServices;

            public AF(IPoderosaWorld world, CoreServices cs) {
                _world = world;
                _coreServices = cs;
            }


            public Type SourceType {
                get {
                    return _world.GetType();
                }
            }

            public Type AdapterType {
                get {
                    return typeof(CoreServices);
                }
            }

            //������C���X�^���X���Ȃ��̂��������ƂɗB��̂�Ԃ�
            public IAdaptable GetAdapter(IAdaptable obj) {
                return _coreServices;
            }

            public IAdaptable GetSource(IAdaptable obj) {
                return _world;
            }
        }
    }
}
