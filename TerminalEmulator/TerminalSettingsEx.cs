/*
 * Copyright 2004,2006 The Poderosa Project.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *
 * $Id: TerminalSettingsEx.cs,v 1.3 2012/03/18 02:52:24 kzmi Exp $
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using Poderosa.ConnectionParam;
using Poderosa.View;
using Poderosa.Terminal;
using Poderosa.Util;

namespace Poderosa.Terminal {
    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public interface ITerminalSettingsChangeListener {
        void OnBeginUpdate(ITerminalSettings current);
        void OnEndUpdate(ITerminalSettings current);
    }

    /// <summary>
    /// <ja>
    /// �e�탍�O�ݒ�̃C���^�[�t�F�C�X�̊��ł��B
    /// </ja>
    /// <en>
    /// Base class of interface of log setting.
    /// </en>
    /// </summary>
    public interface ILogSettings : IAdaptable {
        /// <summary>
        /// <ja>���O�ݒ�𕡐����܂��B</ja><en>Duplicate the log setting.</en>
        /// </summary>
        /// <returns><en>Interface that shows object after it duplidates</en><ja>������̃I�u�W�F�N�g�������C���^�[�t�F�C�X</ja></returns>
        ILogSettings Clone();
    }
    //���O�ݒ�@Terminal�̐ݒ��͕����X�g���[���ɏo�͂ł���悤�ɂȂ��Ă��邪�ATerminalSetting��̓t�@�C���ւ̂P��̂�
    /// <summary>
    /// <ja>
    /// �ȈՂȃ��O�ݒ�������C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface that shows simple log setting
    /// </en>
    /// </summary>
    public interface ISimpleLogSettings : ILogSettings {
        /// <summary>
        /// <ja>
        /// ���O�̎�ނ������܂��B
        /// </ja>
        /// <en>
        /// Type of log.
        /// </en>
        /// </summary>
        LogType LogType {
            get;
            set;
        }
        /// <summary>
        /// <ja>
        /// ���O�̃p�X�������܂��B
        /// </ja>
        /// <en>
        /// Path of the log.
        /// </en>
        /// </summary>
        string LogPath {
            get;
            set;
        }
        /// <summary>
        /// <ja>
        /// ���O��ǋL���邩���Ȃ����������܂��Btrue�̂Ƃ��ǋL���܂��B
        /// </ja>
        /// <en>
        /// Whether the log is append is shown. At true, append
        /// </en>
        /// </summary>
        bool LogAppend {
            get;
            set;
        }
    }

    //�����o��
    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public interface IMultiLogSettings : ILogSettings, IEnumerable<ILogSettings> {
        /// <summary>
        /// Clears all settings, then adds one setting.
        /// </summary>
        /// <param name="log">logging setting</param>
        void Reset(ILogSettings log);

        /// <summary>
        /// Adds setting to the list.
        /// </summary>
        /// <param name="log">logging setting</param>
        void Add(ILogSettings log);

        /// <summary>
        /// Remove setting from the list.
        /// </summary>
        /// <param name="log">logging setting</param>
        void Remove(ILogSettings log);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="newvalue"></param>
    /// <exclude/>
    public delegate void ChangeHandler<T>(T newvalue);

    /// <summary>
    /// <ja>
    /// �^�[�~�i���ݒ�𑀍삷��C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface that control terminal setting.
    /// </en>
    /// </summary>
    public interface ITerminalSettings : IListenerRegistration<ITerminalSettingsChangeListener>, IAdaptable {
        /// <summary>
        /// <ja>
        /// �^�[�~�i���ݒ�̕��������܂��B
        /// </ja>
        /// <en>
        /// Duplicate terminal setting.
        /// </en>
        /// </summary>
        /// <returns><ja>�������ꂽ�^�[�~�i���ݒ�I�u�W�F�N�g�������C���^�[�t�F�C�X�ł��B</ja><en>Interface that shows duplicated terminal setting object</en></returns>
        ITerminalSettings Clone();
        /// <summary>
        /// <ja>
        /// �^�[�~�i���ݒ���C���|�[�g���܂��B
        /// </ja>
        /// <en>
        /// Import the terminal setting.
        /// </en>
        /// </summary>
        /// <param name="src"><ja>�C���|�[�g����^�[�~�i���ݒ�B</ja><en>Terminal setting to import.</en></param>
        void Import(ITerminalSettings src);

        //�ύX����Ƃ���StartUpdate...EndUpdate���s���BEndUpdate�̎��_�Ń��X�i�ɒʒm
        /// <summary>
        /// <ja>
        /// �v���p�e�B�̕ύX���J�n���܂��B
        /// </ja>
        /// <en>
        /// Start changing the property.
        /// </en>
        /// </summary>
        /// <remarks>
        /// <ja>
        /// <para>
        /// �v���p�e�B��ύX����O�ɂ́A<see cref="BeginUpdate">BeginUpdate���\�b�h</see>���Ăяo���A�v���p�e�B�̕ύX���I�������A
        /// <see cref="EndUpdate">EndUpdate���\�b�h</see>���Ăяo���Ȃ���΂Ȃ�܂���B
        /// </para>
        /// <para>
        /// <see cref="BeginUpdate">BeginUpdate���\�b�h</see>���Ăяo���O�Ƀv���p�e�B��ύX���悤�Ƃ���Ɨ�O���������܂��B
        /// </para>
        /// </ja>
        /// <en>
        /// <para>
        /// When the <see cref="BeginUpdate">BeginUpdate method</see> is called before property is changed, and the change in property ends, it is necessary to call the <see cref="EndUpdate">EndUpdate method</see>. 
        /// </para>
        /// <para>
        /// When it starts changing property before the <see cref="BeginUpdate">BeginUpdate method</see> is called, the exception is generated.
        /// </para>
        /// </en>
        /// </remarks>
        void BeginUpdate();
        /// <summary>
        /// <ja>
        /// �v���p�e�B�̕ύX���������܂��B
        /// </ja>
        /// <en>
        /// Finish changing the property.
        /// </en>
        /// <remarks>
        /// <ja>���̃��\�b�h���Ăяo���ƃv���p�e�B�̕ύX�������������̂Ƃ���A�e��C�x���g���������܂��B</ja><en>It is assumed that the change in property was completed when this method is called, and generates various events. </en>
        /// </remarks>
        /// </summary>
        void EndUpdate();
        /// <summary>
        /// <ja>
        /// �G���R�[�h�������擾�^�ݒ肵�܂��B
        /// </ja>
        /// <en>
        /// Set / get the encode type.
        /// </en>
        /// </summary>
        /// <remarks>
        /// <ja>
        /// <para>
        /// �v���p�e�B��ύX����O�ɂ́A<see cref="BeginUpdate">BeginUpdate���\�b�h</see>���Ăяo���A�v���p�e�B�̕ύX���I�������A
        /// <see cref="EndUpdate">EndUpdate���\�b�h</see>���Ăяo���Ȃ���΂Ȃ�܂���B
        /// </para>
        /// <para>
        /// <see cref="BeginUpdate">BeginUpdate���\�b�h</see>���Ăяo���O�Ƀv���p�e�B��ύX���悤�Ƃ���Ɨ�O���������܂��B
        /// </para>
        /// </ja>
        /// <en>
        /// <para>
        /// When the <see cref="BeginUpdate">BeginUpdate method</see> is called before property is changed, and the change in property ends, it is necessary to call the <see cref="EndUpdate">EndUpdate method</see>. 
        /// </para>
        /// <para>
        /// When it starts changing property before the <see cref="BeginUpdate">BeginUpdate method</see> is called, the exception is generated.
        /// </para>
        /// </en>
        /// </remarks>
        EncodingType Encoding {
            get;
            set;
        }
        /// <summary>
        /// <ja>
        /// �^�[�~�i���̎�ނ��擾�^�ݒ肵�܂��B
        /// </ja>
        /// <en>
        /// Set / get the type of termina.
        /// </en>
        /// </summary>
        TerminalType TerminalType {
            get;
            set;
        }
        /// <summary>
        /// <ja>
        /// ���s�R�[�h�̎�舵�����@���擾�^�ݒ肵�܂��B
        /// </ja>
        /// <en>
        /// Set / get the rule of line feed code.
        /// </en>
        /// </summary>
        /// <remarks>
        /// <ja>
        /// <para>
        /// �v���p�e�B��ύX����O�ɂ́A<see cref="BeginUpdate">BeginUpdate���\�b�h</see>���Ăяo���A�v���p�e�B�̕ύX���I�������A
        /// <see cref="EndUpdate">EndUpdate���\�b�h</see>���Ăяo���Ȃ���΂Ȃ�܂���B
        /// </para>
        /// <para>
        /// <see cref="BeginUpdate">BeginUpdate���\�b�h</see>���Ăяo���O�Ƀv���p�e�B��ύX���悤�Ƃ���Ɨ�O���������܂��B
        /// </para>
        /// </ja>
        /// <en>
        /// <para>
        /// When the <see cref="BeginUpdate">BeginUpdate method</see> is called before property is changed, and the change in property ends, it is necessary to call the <see cref="EndUpdate">EndUpdate method</see>. 
        /// </para>
        /// <para>
        /// When it starts changing property before the <see cref="BeginUpdate">BeginUpdate method</see> is called, the exception is generated.
        /// </para>
        /// </en>
        /// </remarks>
        LineFeedRule LineFeedRule {
            get;
            set;
        }
        /// <summary>
        /// <ja>
        /// ���M���̉��s�R�[�h�̎�ނ��擾�^�ݒ肵�܂��B
        /// </ja>
        /// <en>
        /// Set / get the line feed code when transmitting.
        /// </en>
        /// </summary>
        /// <remarks>
        /// <ja>
        /// <para>
        /// �v���p�e�B��ύX����O�ɂ́A<see cref="BeginUpdate">BeginUpdate���\�b�h</see>���Ăяo���A�v���p�e�B�̕ύX���I�������A
        /// <see cref="EndUpdate">EndUpdate���\�b�h</see>���Ăяo���Ȃ���΂Ȃ�܂���B
        /// </para>
        /// <para>
        /// <see cref="BeginUpdate">BeginUpdate���\�b�h</see>���Ăяo���O�Ƀv���p�e�B��ύX���悤�Ƃ���Ɨ�O���������܂��B
        /// </para>
        /// </ja>
        /// <en>
        /// <para>
        /// When the <see cref="BeginUpdate">BeginUpdate method</see> is called before property is changed, and the change in property ends, it is necessary to call the <see cref="EndUpdate">EndUpdate method</see>. 
        /// </para>
        /// <para>
        /// When it starts changing property before the <see cref="BeginUpdate">BeginUpdate method</see> is called, the exception is generated.
        /// </para>
        /// </en>
        /// </remarks>
        NewLine TransmitNL {
            get;
            set;
        }
        /// <summary>
        /// <ja>
        /// ���[�J���G�R�[�̗L�����擾�^�ݒ肵�܂��B
        /// </ja>
        /// <en>
        /// Set / get the local echo.
        /// </en>
        /// </summary>
        /// <remarks>
        /// <ja>
        /// <para>
        /// �v���p�e�B��ύX����O�ɂ́A<see cref="BeginUpdate">BeginUpdate���\�b�h</see>���Ăяo���A�v���p�e�B�̕ύX���I�������A
        /// <see cref="EndUpdate">EndUpdate���\�b�h</see>���Ăяo���Ȃ���΂Ȃ�܂���B
        /// </para>
        /// <para>
        /// <see cref="BeginUpdate">BeginUpdate���\�b�h</see>���Ăяo���O�Ƀv���p�e�B��ύX���悤�Ƃ���Ɨ�O���������܂��B
        /// </para>
        /// </ja>
        /// <en>
        /// <para>
        /// When the <see cref="BeginUpdate">BeginUpdate method</see> is called before property is changed, and the change in property ends, it is necessary to call the <see cref="EndUpdate">EndUpdate method</see>. 
        /// </para>
        /// <para>
        /// When it starts changing property before the <see cref="BeginUpdate">BeginUpdate method</see> is called, the exception is generated.
        /// </para>
        /// </en>
        /// </remarks>
        bool LocalEcho {
            get;
            set;
        }
        /// <summary>
        /// <ja>
        /// �v�����v�g�̔F����R�}���h�̗������L������@�\�����I�u�W�F�N�g�������܂��B
        /// </ja>
        /// <en>
        /// Object that memorizes recognition of prompt and history of command.
        /// </en>
        /// </summary>
        /// <remarks>
        /// <ja>
        /// <para>
        /// �v���p�e�B��ύX����O�ɂ́A<see cref="BeginUpdate">BeginUpdate���\�b�h</see>���Ăяo���A�v���p�e�B�̕ύX���I�������A
        /// <see cref="EndUpdate">EndUpdate���\�b�h</see>���Ăяo���Ȃ���΂Ȃ�܂���B
        /// </para>
        /// <para>
        /// <see cref="BeginUpdate">BeginUpdate���\�b�h</see>���Ăяo���O�Ƀv���p�e�B��ύX���悤�Ƃ���Ɨ�O���������܂��B
        /// </para>
        /// </ja>
        /// <en>
        /// <para>
        /// When the <see cref="BeginUpdate">BeginUpdate method</see> is called before property is changed, and the change in property ends, it is necessary to call the <see cref="EndUpdate">EndUpdate method</see>. 
        /// </para>
        /// <para>
        /// When it starts changing property before the <see cref="BeginUpdate">BeginUpdate method</see> is called, the exception is generated.
        /// </para>
        /// </en>
        /// </remarks>
        IShellScheme ShellScheme {
            get;
            set;
        }
        /// <summary>
        /// <ja>
        /// �ʏ�̕������͂ɔ����C���e���Z���X��L���ɂ��邩�ۂ��̐ݒ�ł��B
        /// </ja>
        /// <en>
        /// Setting whether to make IntelliSense according to usual character input effective.
        /// </en>
        /// </summary>
        /// <remarks>
        /// <ja>
        /// <para>
        /// �v���p�e�B��ύX����O�ɂ́A<see cref="BeginUpdate">BeginUpdate���\�b�h</see>���Ăяo���A�v���p�e�B�̕ύX���I�������A
        /// <see cref="EndUpdate">EndUpdate���\�b�h</see>���Ăяo���Ȃ���΂Ȃ�܂���B
        /// </para>
        /// <para>
        /// <see cref="BeginUpdate">BeginUpdate���\�b�h</see>���Ăяo���O�Ƀv���p�e�B��ύX���悤�Ƃ���Ɨ�O���������܂��B
        /// </para>
        /// </ja>
        /// <en>
        /// <para>
        /// When the <see cref="BeginUpdate">BeginUpdate method</see> is called before property is changed, and the change in property ends, it is necessary to call the <see cref="EndUpdate">EndUpdate method</see>. 
        /// </para>
        /// <para>
        /// When it starts changing property before the <see cref="BeginUpdate">BeginUpdate method</see> is called, the exception is generated.
        /// </para>
        /// </en>
        /// </remarks>
        bool EnabledCharTriggerIntelliSense {
            get;
            set;
        }

        /// <summary>
        /// <ja>
        /// �h�L�������g�o�[�ɕ\������L���v�V�����ł��B
        /// </ja>
        /// <en>
        /// Caption displayed in document bar.
        /// </en>
        /// </summary>
        /// <remarks>
        /// <ja>
        /// <para>
        /// �v���p�e�B��ύX����O�ɂ́A<see cref="BeginUpdate">BeginUpdate���\�b�h</see>���Ăяo���A�v���p�e�B�̕ύX���I�������A
        /// <see cref="EndUpdate">EndUpdate���\�b�h</see>���Ăяo���Ȃ���΂Ȃ�܂���B
        /// </para>
        /// <para>
        /// <see cref="BeginUpdate">BeginUpdate���\�b�h</see>���Ăяo���O�Ƀv���p�e�B��ύX���悤�Ƃ���Ɨ�O���������܂��B
        /// </para>
        /// </ja>
        /// <en>
        /// <para>
        /// When the <see cref="BeginUpdate">BeginUpdate method</see> is called before property is changed, and the change in property ends, it is necessary to call the <see cref="EndUpdate">EndUpdate method</see>. 
        /// </para>
        /// <para>
        /// When it starts changing property before the <see cref="BeginUpdate">BeginUpdate method</see> is called, the exception is generated.
        /// </para>
        /// </en>
        /// </remarks>
        string Caption {
            get;
            set;
        }

        /// <summary>
        /// <ja>
        /// �h�L�������g�o�[�ɕ\������A�C�R���ł��B
        /// </ja>
        /// <en>
        /// Icon displayed in document bar
        /// </en>
        /// </summary>
        /// <remarks>
        /// <ja>
        /// <para>
        /// �v���p�e�B��ύX����O�ɂ́A<see cref="BeginUpdate">BeginUpdate���\�b�h</see>���Ăяo���A�v���p�e�B�̕ύX���I�������A
        /// <see cref="EndUpdate">EndUpdate���\�b�h</see>���Ăяo���Ȃ���΂Ȃ�܂���B
        /// </para>
        /// <para>
        /// <see cref="BeginUpdate">BeginUpdate���\�b�h</see>���Ăяo���O�Ƀv���p�e�B��ύX���悤�Ƃ���Ɨ�O���������܂��B
        /// </para>
        /// </ja>
        /// <en>
        /// <para>
        /// When the <see cref="BeginUpdate">BeginUpdate method</see> is called before property is changed, and the change in property ends, it is necessary to call the <see cref="EndUpdate">EndUpdate method</see>. 
        /// </para>
        /// <para>
        /// When it starts changing property before the <see cref="BeginUpdate">BeginUpdate method</see> is called, the exception is generated.
        /// </para>
        /// </en>
        /// </remarks>
        Image Icon {
            get;
            set;
        }

        /// <summary>
        /// <ja>
        /// �R���\�[���̕\�����@���w�肷��RenderProfile�I�u�W�F�N�g�ł��B�t�H���g�A�w�i�F�Ȃǂ̏�񂪊܂܂�܂��B
        /// </ja>
        /// <en>
        /// It is RenderProfile object that specifies the method of displaying the console. Information of the font and the background color, etc. is included. 
        /// </en>
        /// </summary>
        /// <remarks>
        /// <ja>
        /// <para>
        /// �v���p�e�B��ύX����O�ɂ́A<see cref="BeginUpdate">BeginUpdate���\�b�h</see>���Ăяo���A�v���p�e�B�̕ύX���I�������A
        /// <see cref="EndUpdate">EndUpdate���\�b�h</see>���Ăяo���Ȃ���΂Ȃ�܂���B
        /// </para>
        /// <para>
        /// <see cref="BeginUpdate">BeginUpdate���\�b�h</see>���Ăяo���O�Ƀv���p�e�B��ύX���悤�Ƃ���Ɨ�O���������܂��B
        /// </para>
        /// </ja>
        /// <en>
        /// <para>
        /// When the <see cref="BeginUpdate">BeginUpdate method</see> is called before property is changed, and the change in property ends, it is necessary to call the <see cref="EndUpdate">EndUpdate method</see>. 
        /// </para>
        /// <para>
        /// When it starts changing property before the <see cref="BeginUpdate">BeginUpdate method</see> is called, the exception is generated.
        /// </para>
        /// </en>
        /// </remarks>
        RenderProfile RenderProfile {
            get;
            set;
        }

        /// <summary>
        /// <ja>
        /// �f�t�H���g�̕\���v���t�@�C����p���Ă��邩�ǂ������擾���܂��B
        /// </ja>
        /// <en>
        /// Get whether to use the display profile of default. 
        /// </en>
        /// </summary>
        bool UsingDefaultRenderProfile {
            get;
        }

        /// <summary>
        /// <ja>
        /// ���O�̐ݒ���ł��B
        /// </ja>
        /// <en>
        /// Setting of log.
        /// </en>
        /// </summary>
        IMultiLogSettings LogSettings {
            get;
        }

        //TODO ������ITerminalSettingsChangeListener�ɓ�������
        /// <summary>
        /// 
        /// </summary>
        /// <exclude/>
        event ChangeHandler<string> ChangeCaption;
        /// <summary>
        /// 
        /// </summary>
        /// <exclude/>
        event ChangeHandler<RenderProfile> ChangeRenderProfile;
        /// <summary>
        /// 
        /// </summary>
        /// <exclude/>
        event ChangeHandler<EncodingType> ChangeEncoding;
    }

}
