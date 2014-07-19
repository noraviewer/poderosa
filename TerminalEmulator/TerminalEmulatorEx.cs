/*
 * Copyright 2004,2006 The Poderosa Project.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *
 * $Id: TerminalEmulatorEx.cs,v 1.2 2011/10/27 23:21:58 kzmi Exp $
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using Poderosa.Protocols;
using Poderosa.Sessions;
using Poderosa.Forms;
using Poderosa.Commands;

namespace Poderosa.Terminal {
    //AbstractTerminal���K�v�ȋ@�\���󂯓n��

    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public interface IAbstractTerminalHost {
        ITerminalSettings TerminalSettings {
            get;
        }
        TerminalTransmission TerminalTransmission {
            get;
        }
        ISession ISession {
            get;
        }
        IPoderosaMainWindow OwnerWindow {
            get;
        } //ISession�ɏ��i������̂��悢�H�@���邢��SessionManager�̋@�\���H�@�����ǂ���
        ITerminalConnection TerminalConnection {
            get;
        }

        TerminalControl TerminalControl {
            get;
        }
        void NotifyViewsDataArrived();
        void CloseByReceptionThread(string msg);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public interface ITerminalControlHost {
        ITerminalSettings TerminalSettings {
            get;
        }
        IPoderosaMainWindow OwnerWindow {
            get;
        }
        ITerminalConnection TerminalConnection {
            get;
        }

        AbstractTerminal Terminal {
            get;
        }
        TerminalTransmission TerminalTransmission {
            get;
        }
    }

    /// <summary>
    /// <ja>
    /// �^�[�~�i���G�~�����[�^�T�[�r�X�ɃA�N�Z�X���邽�߂̃C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface to access terminal emulator service
    /// </en>
    /// </summary>
    /// <remarks>
    /// <ja>
    /// ���̃C���^�[�t�F�C�X�́ATermiunalEmuratorPlugin�v���O�C���i�v���O�C��ID�uorg.poderosa.terminalemulator�v�j��
    /// �񋟂��܂��B���̂悤�ɂ���ƁAITerminalEmulatorService���擾�ł��܂��B
    /// <code>
    /// ITerminalEmulatorService emuservice = 
    ///   (ITerminalEmulatorService)PoderosaWorld.PluginManager.FindPlugin(
    ///     "org.poderosa.terminalemulator", typeof(ITerminalEmulatorService));
    /// </code>
    /// </ja>
    /// <en>
    /// The TermiunalEmuratorPlugin plug-in (plug-in ID[org.poderosa.terminalemulator]) offers this interface. ITerminalEmulatorService can be acquired by doing as follows. 
    /// <code>
    /// ITerminalEmulatorService emuservice = 
    ///   (ITerminalEmulatorService)PoderosaWorld.PluginManager.FindPlugin(
    ///     "org.poderosa.terminalemulator", typeof(ITerminalEmulatorService));
    /// </code>
    /// </en>
    /// </remarks>
    public interface ITerminalEmulatorService {
        /// <summary>
        /// 
        /// </summary>
        /// <exclude/>
        void LaterInitialize();

        /// <summary>
        /// <ja>
        /// �^�[�~�i���G�~�����[�^�̃I�v�V�����������܂��B
        /// </ja>
        /// <en>
        /// The option of the terminal emulator is shown. 
        /// </en>
        /// </summary>
        ITerminalEmulatorOptions TerminalEmulatorOptions {
            get;
        }
        /// <summary>
        /// <ja>
        /// �f�t�H���g�̃^�[�~�i���ݒ���쐬���܂��B
        /// </ja>
        /// <en>
        /// Create a default terminal setting.
        /// </en>
        /// </summary>
        /// <param name="caption"><ja>�^�[�~�i���̃L���v�V�����ł��B</ja><en>Caption of terminal.</en></param>
        /// <param name="icon"><ja>�^�[�~�i���̃A�C�R���ł��Bnull���w�肷��ƃf�t�H���g�̃A�C�R�����g���܂��B</ja><en>It is an icon of the terminal. When null is specified, the icon of default is used. </en></param>
        /// <returns><ja>�쐬���ꂽ�^�[�~�i���ݒ�I�u�W�F�N�g������ITerminalSettings�ł��B</ja><en>It is ITerminalSettings that shows the made terminal setting object. </en></returns>
        ITerminalSettings CreateDefaultTerminalSettings(string caption, Image icon);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exclude/>
        ISimpleLogSettings CreateDefaultSimpleLogSettings();
        /// <summary>
        /// 
        /// </summary>
        /// <exclude/>
        IPoderosaMenuGroup[] ContextMenu {
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <exclude/>
        ICommandCategory TerminalCommandCategory {
            get;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <exclude/>
        IShellSchemeCollection ShellSchemeCollection {
            get;
        }
    }

    //���O�t�@�C�����̃J�X�^�}�C�Y
    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public interface IAutoLogFileFormatter {
        string FormatFileName(string default_directory, ITerminalParameter param, ITerminalSettings settings);
    }

    //���I�ȃE�B���h�E�L���v�V�����J�X�^�}�C�Y
    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public interface IDynamicCaptionFormatter {
        //�X���b�h�ł̃u���b�N�͂Ȃ��̂Œ���
        string FormatCaptionUsingWindowTitle(ITerminalParameter param, ITerminalSettings settings, string windowTitle);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public class TerminalInitializeInfo {
        private IAbstractTerminalHost _session;
        private ITerminalParameter _param;
        private int _initialWidth;
        private int _initialHeight;

        public TerminalInitializeInfo(IAbstractTerminalHost session, ITerminalParameter param) {
            _session = session;
            _param = param;
            _initialWidth = param.InitialWidth;
            _initialHeight = param.InitialHeight;
        }

        public IAbstractTerminalHost Session {
            get {
                return _session;
            }
        }
        public ITerminalParameter TerminalParameter {
            get {
                return _param;
            }
        }
        public int InitialWidth {
            get {
                return _initialWidth;
            }
        }
        public int InitialHeight {
            get {
                return _initialHeight;
            }
        }
    }
}
