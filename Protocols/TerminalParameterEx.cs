/*
 * Copyright 2004,2006 The Poderosa Project.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *
 * $Id: TerminalParameterEx.cs,v 1.5 2012/03/14 16:33:38 kzmi Exp $
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

using Poderosa.Util;
using Granados;

namespace Poderosa.Protocols {
    /// <summary>
    /// <ja>
    /// �^�[�~�i���ڑ��̂��߂̃p�����[�^�������C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface that shows parameter for terminal connection.
    /// </en>
    /// </summary>
    /// <remarks>
    /// <ja>
    /// <seealso cref="ITCPParameter">ITCPParameter</seealso>�A
    /// <seealso cref="ISSHLoginParameter">ISSHLoginParameter</seealso>
    /// <seealso cref="ICygwinParameter">ICygwinParameter</seealso>�́AGetAdapter���\�b�h���g����
    /// ����ITerminalParameter�ւƕϊ��ł��܂��B
    /// </ja>
    /// <en>
    /// <seealso cref="ITCPParameter">ITCPParameter</seealso>,
    /// <seealso cref="ISSHLoginParameter">ISSHLoginParameter</seealso>,
    /// <seealso cref="ICygwinParameter">ICygwinParameter</seealso> can be converted to ITerminalParameter by GetAdapter method.
    /// </en>
    /// </remarks>
    public interface ITerminalParameter : IAdaptable, ICloneable {
        /// <summary>
        /// <ja>
        /// �������ł��B
        /// </ja>
        /// <en>
        /// Internal width.
        /// </en>
        /// </summary>
        int InitialWidth {
            get;
        }
        /// <summary>
        /// <ja>
        /// ���������ł��B
        /// </ja>
        /// <en>
        /// Internal height.
        /// </en>
        /// </summary>
        int InitialHeight {
            get;
        }
        /// <summary>
        /// <ja>�^�[�~�i���^�C�v�ł��B</ja>
        /// <en>Terminal type.</en>
        /// </summary>
        string TerminalType {
            get;
        }
        /// <summary>
        /// <ja>�^�[�~�i������ݒ肵�܂��B</ja>
        /// <en>Set the terminal name.</en>
        /// </summary>
        /// <param name="terminaltype"><ja>�ݒ肷��^�[�~�i����</ja><en>Terminal name to set.</en></param>
        void SetTerminalName(string terminaltype);
        /// <summary>
        /// <ja>
        /// �^�[�~�i���̃T�C�Y��ύX���܂��B
        /// </ja>
        /// <en>
        /// Change the terminal size.
        /// </en>
        /// </summary>
        /// <param name="width"><ja>�ύX��̕�</ja><en>Width after it changes</en></param>
        /// <param name="height"><ja>�ύX��̍���</ja><en>Height after it changes</en></param>
        void SetTerminalSize(int width, int height);

        /// <summary>
        /// <ja>
        /// 2�̃C���^�[�t�F�C�X���u�����ڂƂ��āv�����ł��邩�ǂ����𒲂ׂ܂��B
        /// </ja>
        /// <en>
        /// Comparing two interfaces examine and "Externals" examines be the same. 
        /// </en>
        /// </summary>
        /// <param name="t"><ja>��r�ΏۂƂȂ�I�u�W�F�N�g</ja><en>Object to exemine</en></param>
        /// <returns><en>Result of comparing. If it is equal, return true. </en><ja>��r���ʁB�������Ȃ�true</ja></returns>
        /// <remarks>
        /// <ja>
        /// <para>
        /// �u�����ڂƂ��āv�Ƃ́ASSH�v���g�R���̃o�[�W�����̈Ⴂ�ȂǁA�u�ڑ�����r����ꍇ�v
        /// �̓��ꎋ���Ӗ����܂��B
        /// </para>
        /// <para>
        /// MRU�v���O�C���ł͂��̃��\�b�h�𗘗p���āA���ׂȈႢ�̍��ڂ������A�ŋߎg�����ڑ��̕����ɕ\������Ă��܂����Ƃ�h���ł��܂��B
        /// </para>
        /// </ja>
        /// <en>
        /// <para>
        /// "Externals" means one seeing of "The connection destinations are compared" of the difference etc. of the version of the SSH protocol. 
        /// </para>
        /// <para>
        /// The item of a trifling difference is two or more pieces, and it is prevented from being displayed by using this method in the MRU plug-in in the part of the connection used recently. 
        /// </para>
        /// </en>
        /// </remarks>
        bool UIEquals(ITerminalParameter t);
    }

    /// <summary>
    /// <ja>
    /// Telnet�܂���SSH�ڑ��̋��ʂ̃p�����[�^�ł��B
    /// </ja>
    /// <en>
    /// Common parameters for the telnet or the SSH connection.
    /// </en>
    /// </summary>
    /// <remarks>
    /// <en>
    /// <para>
    /// Default parameter of Telnet connnection can be got by <see cref="Poderosa.Protocols.IProtocolService.CreateDefaultTelnetParameter">CreateDefaultTelnetParameter method</see> on <seealso cref="IProtocolService">IProtocolService</seealso>
    /// </para>
    /// <para>
    /// This interface can be convater to <seealso cref="ITerminalParameter">ITerminalParameter</seealso> by GetAdapter method.
    /// </para>
    /// </en>
    /// <ja>
    /// <para>
    /// �f�t�H���g��Telnet�ڑ��p�����[�^�́A<seealso cref="IProtocolService">IProtocolService</seealso>��
    /// <see cref="Poderosa.Protocols.IProtocolService.CreateDefaultTelnetParameter">CreateDefaultTelnetParameter���\�b�h</see>���g���Ď擾�ł��܂��B
    /// </para>
    /// <para>
    /// ���̃C���^�[�t�F�C�X�́AGetAdapter���\�b�h���g�����ƂŁA<seealso cref="ITerminalParameter">ITerminalParameter</seealso>
    /// �ւƕϊ��ł��܂��B
    /// </para>
    /// </ja>
    /// </remarks>
    public interface ITCPParameter : IAdaptable, ICloneable {
        /// <summary>
        /// <ja>
        /// �ڑ���̃z�X�g���i�܂���IP�A�h���X�j�ł��B
        /// </ja>
        /// <en>
        /// Hostname or IP Address to connect.
        /// </en>
        /// </summary>
        string Destination {
            get;
            set;
        }
        /// <summary>
        /// <ja>
        /// �ڑ���̃|�[�g�ԍ��ł��B
        /// </ja>
        /// <en>
        /// Port number to connect.
        /// </en>
        /// </summary>
        int Port {
            get;
            set;
        }
    }

    /// <summary>
    /// Telnet-specific parameters.
    /// </summary>
    public interface ITelnetParameter {

        /// <summary>
        /// Whether the "New Line" code of the telnet protocol is used for sending CR+LF.
        /// </summary>
        bool TelnetNewLine {
            get;
            set;
        }
    }

    /// <summary>
    /// <ja>
    /// SSH�ڑ����̃��O�C���p�����[�^�������C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Inteface that show the login parameter on SSH connection.
    /// </en>
    /// </summary>
    /// <remarks>
    /// <ja>
    /// <para>
    /// �f�t�H���g��SSH�ڑ��p�����[�^�́A<seealso cref="IProtocolService">IProtocolService</seealso>��
    /// <see cref="IProtocolService.CreateDefaultSSHParameter">CreateDefaultSSHParameter���\�b�h</see>���g���Ď擾�ł��܂��B
    /// </para>
    /// <para>
    /// �ڑ���̃z�X�g����|�[�g�ԍ��́AGetAdapter���\�b�h��p����<seealso cref="ITCPParameter">ITCPParameter</seealso>�ւ�
    /// �ϊ����Đݒ肵�܂��B
    /// </para>
    /// <para>
    /// ���̃C���^�[�t�F�C�X�́AGetAdapter���\�b�h���g�����ƂŁA<seealso cref="ITerminalParameter">ITerminalParameter</seealso>
    /// �ւƕϊ��ł��܂��B
    /// </para>
    /// </ja>
    /// <en>
    /// <para>
    /// Default parameter of SSH connnection can be got by > by <see cref="Poderosa.Protocols.IProtocolService.CreateDefaultSSHParameter">CreateDefaultTelnetParameter method</see> on<seealso cref="IProtocolService">IProtocolService</seealso>
    /// </para>
    /// <para>
    /// The host name and the port number at the connection destination are converted into <seealso cref="ITCPParameter">ITCPParameter</seealso> by using the GetAdapter method and set. 
    /// </para>
    /// <para>
    /// This interface can be convater to <seealso cref="ITerminalParameter">ITerminalParameter</seealso> by GetAdapter method.
    /// </para>
    /// </en>
    /// </remarks>
    public interface ISSHLoginParameter : IAdaptable, ICloneable {
        /// <summary>
        /// <ja>SSH�v���g�R���̃o�[�W�����ł��B</ja>
        /// <en>Version of the SSH protocol.</en>
        /// </summary>
        SSHProtocol Method {
            get;
            set;
        }
        /// <summary>
        /// <ja>
        /// �F�ؕ����ł��B
        /// </ja>
        /// <en>
        /// Authentification method.
        /// </en>
        /// </summary>
        AuthenticationType AuthenticationType {
            get;
            set;
        }
        /// <summary>
        /// <ja>
        /// ���O�C������A�J�E���g���i���[�U�[���j�ł��B
        /// </ja>
        /// <en>
        /// Account name (User name) to login.
        /// </en>
        /// </summary>
        string Account {
            get;
            set;
        }
        /// <summary>
        /// <ja>���[�U�̔F�؂Ɏg�p����閧���̃t�@�C�����ł��B</ja>
        /// <en>Private key file name to use to user authentification.</en>
        /// </summary>
        /// <remarks>
        /// <ja>
        /// ���̃v���p�e�B�́AAuthenticationType��AutehnticationType.PublicKey�̂Ƃ��̂ݎg���܂��B
        /// </ja>
        /// <en>
        /// This property is used when AuthenticationType is AutehnticationType.PublicKey only.
        /// </en>
        /// </remarks>
        string IdentityFileName {
            get;
            set;
        }
        /// <summary>
        /// <ja>
        /// �p�X���[�h�܂��̓p�X�t���[�Y�ł��B
        /// </ja>
        /// <en>
        /// Password or passphrase
        /// </en>
        /// </summary>
        /// <remarks>
        /// <ja>
        /// AuthenticationType�v���p�e�B��AuthenticationType.Password�̂Ƃ��ɂ́u�p�X���[�h�v���A
        /// AuthenticationType.PublicKey�̂Ƃ��ɂ́u�p�X�t���[�Y�v��ݒ肵�܂��B
        /// AuthenticationType.KeyboardInteractive�̂Ƃ��ɂ́A���̃v���p�e�B�͖�������܂��B
        /// </ja>
        /// <en>
        /// Set password when AuthenticationType is AuthenticationType.Password, and, set passphrase when AuthenticationType.PublicKey.
        /// This property is ignored if it is AuthenticationType.KeyboardInteractive.
        /// </en>
        /// </remarks>
        string PasswordOrPassphrase {
            get;
            set;
        }
        //���[�U�Ƀp�X���[�h����͂����邩�ǂ����Btrue�̂Ƃ���PasswordOrPassphrase�͎g�p���Ȃ�
        /// <summary>
        /// <ja>
        /// ���[�U�[�� �p�X���[�h����͂����邩�ǂ����̃t���O�ł��B
        /// </ja>
        /// <en>
        /// Flag whether to make user input password
        /// </en>
        /// </summary>
        /// <remarks>
        /// <ja>true�̏ꍇ�APasswordOrPassphrase�v���p�e�B�͎g���܂���B</ja><en>If it is true, PasswordOrPassphrase property is not used.</en>
        /// </remarks>
        /// <exclude/>
        bool LetUserInputPassword {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exclude/>
        IAgentForward AgentForward {
            get;
            set;
        }
    }

    /// <summary>
    /// </summary>
    /// <exclude/>
    public interface ISSHSubsystemParameter : IAdaptable, ICloneable {
        string SubsystemName {
            get;
            set;
        }
    }

    /// <summary>
    /// <ja>
    /// Cygwin�ڑ�����Ƃ��Ɏg����p�����[�^�������C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface that show the parameter using on Cygwin connection.
    /// </en>
    /// </summary>
    /// <remarks>
    /// <ja>
    /// <para>
    /// ���̃C���^�[�t�F�C�X�́A<seealso cref="IProtocolService">IProtocolService</seealso>��
    /// <see cref="IProtocolService.CreateDefaultCygwinParameter">CreateDefaultCygwinParameter���\�b�h</see>
    /// ���Ăяo�����ƂŎ擾�ł��܂��B
    /// </para>
    /// <para>
    /// ���̃C���^�[�t�F�C�X�́AGetAdapter���\�b�h���g�����ƂŁA<seealso cref="ITerminalParameter">ITerminalParameter</seealso>
    /// �ւƕϊ��ł��܂��B
    /// </para>
    /// </ja>
    /// <en>
    /// <para>
    /// This interface can be  got using <see cref="IProtocolService.CreateDefaultCygwinParameter">CreateDefaultCygwinParameter method</see> on <seealso cref="IProtocolService">IProtocolService</seealso>.
    /// </para>
    /// <para>
    /// This interface is convert to <seealso cref="ITerminalParameter">ITerminalParameter</seealso> by GetAdapter method.
    /// </para>
    /// </en>
    /// </remarks>
    public interface ICygwinParameter : IAdaptable, ICloneable {
        /// <summary>
        /// <ja>
        /// �V�F���̖��O���擾�^�ݒ肵�܂��B
        /// </ja>
        /// <en>
        /// Get / set shell name.
        /// </en>
        /// </summary>
        string ShellName {
            get;
            set;
        }
        /// <summary>
        /// <ja>
        /// �z�[���f�B���N�g�����擾�^�ݒ肵�܂��B
        /// </ja>
        /// <en>
        /// Get / set the home directory.
        /// </en>
        /// </summary>
        string Home {
            get;
            set;
        }
        /// <summary>
        /// <ja>
        /// �V�F�����������������菜�����R�}���h����������Ԃ��܂��B
        /// </ja>
        /// <en>
        /// Only the command part where the argument part was removed from the shell is returned. 
        /// </en>
        /// </summary>
        string ShellBody {
            get;
        }
        /// <summary>
        /// <ja>
        /// Cygwin�̏ꏊ���擾�^�ݒ肵�܂��B
        /// �ݒ肳��Ȃ��ꍇ�̓��W�X�g�����猟�o���܂��B
        /// </ja>
        /// <en>
        /// Get or Set path where Cygwin is installed.
        /// If this property was not set, the path will be detected from the registry entry.
        /// </en>
        /// </summary>
        string CygwinDir {
            get;
            set;
        }
    }

    /// <summary>
    /// <ja>
    /// �}�N���̎������s�̃p�����[�^�������C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// This interface represents parameters for macro auto execution.
    /// </en>
    /// </summary>
    public interface IAutoExecMacroParameter : IAdaptable, ICloneable {
        /// <summary>
        /// <ja>
        /// �ڑ���Ɏ������s����}�N���̃p�X�B���w��̂Ƃ���null�B
        /// </ja>
        /// <en>
        /// Path to a macro which will be run automatically after the connection was established.
        /// Null if it is not specified.
        /// </en>
        /// </summary>
        String AutoExecMacroPath {
            get;
            set;
        }
    }
}
