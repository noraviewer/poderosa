/*
 * Copyright 2011 The Poderosa Project.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *
 * $Id: MacroEngineEx.cs,v 1.2 2011/11/01 15:24:56 kzmi Exp $
 */
using System;
using System.Windows.Forms;
using Poderosa.Sessions;

namespace Poderosa.MacroEngine {

    /// <summary>
    /// <ja>
    /// �}�N�����s�T�|�[�g�̂��߂̃C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface for supporting executing macro.
    /// </en>
    /// </summary>
    public interface IMacroEngine {

        /// <summary>
        /// <ja>
        /// �Z�b�V�������w�肵�ă}�N�������s����B
        /// </ja>
        /// <en>
        /// Run a macro with specifying session.
        /// </en>
        /// </summary>
        /// <param name="path">
        /// <ja>�}�N���̃p�X</ja>
        /// <en>Path of a macro to execute.</en>
        /// </param>
        /// <param name="session">
        /// <ja>�Z�b�V����</ja>
        /// <en>Session.</en>
        /// </param>
        void RunMacro(string path, ISession session);

        /// <summary>
        /// <ja>
        /// �}�N���I���_�C�A���O��\������B
        /// </ja>
        /// <en>
        /// Show a dialog for selecting macro.
        /// </en>
        /// </summary>
        /// <param name="owner">
        /// <ja>�I�[�i�[�t�H�[��</ja>
        /// <en>Owner form</en>
        /// </param>
        /// <returns>
        /// <ja>�I�������}�N���̃p�X�B�I�����Ă��Ȃ����null�B</ja>
        /// <en>Path of the selected macro. Null if no macro was selected.</en>
        /// </returns>
        string SelectMacro(Form owner);

    }

    /// <summary>
    /// <ja>
    /// �v���p�e�B�l���}�N�����Őڑ��p�����[�^�Ƃ��Ď擾�ł��邱�Ƃ������܂��B
    /// </ja>
    /// <en>
    /// Represents the property value can be obtained as a connection parameter in the macro environment. 
    /// </en>
    /// </summary>
    /// <remarks>
    /// <ja>
    /// ���̑�����<see cref="Poderosa.Protocols.ITerminalParameter"/>�A�܂���<see cref="Poderosa.Terminal.ITerminalSettings"/>�����N���X�̃v���p�e�B�Ɏw�肵�܂��B
    /// </ja>
    /// <en>
    /// This attribute must be specified to a property of a class which implements <see cref="Poderosa.Protocols.ITerminalParameter"/> or <see cref="Poderosa.Terminal.ITerminalSettings"/>.
    /// </en>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property)]
    public class MacroConnectionParameterAttribute : Attribute {
    }

}
