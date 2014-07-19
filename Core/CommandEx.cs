/*
 * Copyright 2004,2006 The Poderosa Project.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *
 * $Id: CommandEx.cs,v 1.2 2011/10/27 23:21:55 kzmi Exp $
 */
using System;
using System.Collections;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;

using Poderosa.Preferences;

namespace Poderosa.Commands {
    /// <summary>
    /// <ja>
    /// �R�}���h�̎��s���ʂ������܂��B
    /// </ja>
    /// <en>
    /// Return the result of the command.
    /// </en>
    /// </summary>
    /// <remarks>
    /// <ja>
    /// <para>
    /// <seealso cref="IPoderosaCommand">IPoderosaCommand</seealso>��<see cref="IPoderosaCommand.InternalExecute">InternalExecute���\�b�h</see>�̎����҂́A
    /// �R�}���h�̎��s�̉ۂ��A���̗񋓑̂ŕԂ��܂��B
    /// </para>
    /// <para>
    /// ���������ꍇ�ɂ�Succeeded�A���s�����ꍇ�ɂ�Failed��Ԃ��悤�Ɏ������Ă��������B
    /// </para>
    /// <para>
    /// Cancelled�̓��[�U�[����ɂ���ăL�����Z�����ꂽ�ꍇ�Ȃǂɗp���܂��B
    /// </para>
    /// <para>
    /// Ignored�̓R�}���h�����s����Ώۂ��Ȃ������Ƃ��i���Ƃ��ΑI�����ꂽ�e�L�X�g�ɑ΂��ď������ׂ��R�}���h�̏ꍇ�ɁA���ݑI�����Ă���e�L�X�g���Ȃ������Ƃ��Ȃǁj�ɗp���܂��B
    /// </para>
    /// </ja>
    /// <en>
    /// <para>
    /// Those who implement about the <see cref="IPoderosaCommand.InternalExecute">InternalExecute method</see> of <seealso cref="IPoderosaCommand">IPoderosaCommand</seealso> return right or wrong of the execution of the command with this enumeration. 
    /// </para>
    /// <para>
    /// Please implement to return Failed when Succeeded and failing when succeeding. 
    /// </para>
    /// <para>
    /// Cancelled is used when canceled by the user operation. 
    /// </para>
    /// <para>
    /// Ignored is used when there is no object the execution of the command (For instance, there is no text that has been selected now for the command that should be processed to the selected text). 
    /// </para>
    /// </en>
    /// </remarks>
    public enum CommandResult {
        /// <summary>
        /// <ja>
        /// ����
        /// </ja>
        /// <en>
        /// Succeeded.
        /// </en>
        /// </summary>
        Succeeded,
        /// <summary>
        /// <ja>
        /// ���s
        /// </ja>
        /// <en>
        /// Failed
        /// </en>
        /// </summary>
        Failed,
        /// <summary>
        /// <ja>
        /// �L�����Z������
        /// </ja>
        /// <en>
        /// Canceleld
        /// </en>
        /// </summary>
        Cancelled,
        /// <summary>
        /// <ja>
        /// ��������
        /// </ja>
        /// <en>
        /// Ignored.
        /// </en>
        /// </summary>
        Ignored
    }

    //�R�}���h�̋쓮�ΏہB�R���e�L�X�g���j���[���o�������񋟂��āAICommand#Execute�̈����ɂȂ�B
    //���C�����j���[�z���̏ꍇ�A���C���E�B���h�E��IAdaptable�o�R�Ŏ擾���邱�ƂɂȂ�B
    /// <summary>
    /// <ja>
    /// �R�}���h�����s���ׂ��^�[�Q�b�g�������C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface that shows target that command should execute.
    /// </en>
    /// </summary>
    /// <remarks>
    /// <ja>
    /// <para>
    /// ���̃C���^�[�t�F�C�X�́A<seealso cref="ICommandManager">ICommandManager</seealso>��<see cref="ICommandManager.Execute">Execute���\�b�h</see>���Ăяo���āA�R�}���h�����s����ۂɈ����n��
    /// �^�[�Q�b�g�Ƃ��Ďg���܂��B
    /// </para>
    /// <para>
    /// �n���ꂽ�^�[�Q�b�g�́A<seealso cref="IPoderosaCommand">IPoderosaCommand</seealso>��<see cref="IPoderosaCommand.InternalExecute">InternamExecute���\�b�h</see>
    /// �ɂ��̂܂܈����n����܂��B
    /// </para>
    /// <para>
    /// ���j���[��c�[���o�[����Ăяo�����R�}���h�́A<paramref name="target"/>�ɂ́A���C���E�B���h�E������<see cref="Poderosa.Forms.IPoderosaMainWindow">IPoderosaMainWindow</see>
    /// ���n����邱�Ƃ�z�肵�Ă��܂��B���̎�ȊO�̃C���^�[�t�F�C�X���n���ꂽ�Ƃ��ɂ́A
    /// ���������삵�܂���B
    /// </para>
    /// <para>
    /// <seealso cref="CommandTargetUtil">CommandTargetUtil</seealso>���g���ƁA�^�[�Q�b�g���E�B���h�E��r���[�ւƕϊ��ł��܂��B
    /// </para>
    /// </ja>
    /// <en>
    /// <para>
    /// This interface is used as a target handed over when the <see cref="ICommandManager.Execute">Execute method</see> of <seealso cref="ICommandManager">ICommandManager</seealso> is called, and the command is executed. 
    /// 
    /// </para>
    /// <para>
    /// As for the passed target, off is passed to the <see cref="IPoderosaCommand.InternalExecute">InternamExecute method</see> of <seealso cref="IPoderosaCommand">IPoderosaCommand</seealso> as it is. 
    /// </para>
    /// <para>
    /// The command called from the menu and the toolbar assumes to <paramref name="target"/> 
    /// <see cref="Poderosa.Forms.IPoderosaMainWindow">IPoderosaMainWindow</see>'s that shows the main window being passed. 
    /// It doesn't operate correctly when you pass interfaces other than this kind.
    /// </para>
    /// <para>
    /// The target can be converted into the window and the view by using <seealso cref="CommandTargetUtil">CommandTargetUtil</seealso>. 
    /// </para>
    /// </en>
    /// </remarks>
    public interface ICommandTarget : IAdaptable {
    }

    //Menu/Command�̎��s�ۂ�delegate
    /// <summary>
    /// <ja>���j���[�Ƀ`�F�b�N���t���Ă��邩�ǂ��������߂�Ƃ��ɌĂяo�����f���Q�[�g�ł��B</ja>
    /// <en>Delegate called when it is decided whether the check has adhered to the menu. </en>
    /// </summary>
    /// <param name="target">
    /// <ja>
    /// �R�}���h�̑Ώۂ������^�[�Q�b�g�ł��B
    /// </ja>
    /// <en>
    /// Target that shows object of command.
    /// </en>
    /// </param>
    /// <returns>
    /// <ja>
    /// �`�F�b�N���t���Ă���Ȃ�true���A�����łȂ��Ȃ�false��Ԃ��Ă��������B
    /// </ja>
    /// <en>
    /// Please return true and return false if it is not so if the check has adhered. 
    /// </en>
    /// </returns>
    /// <remarks>
    /// <ja>
    /// <para>
    /// <paramref name="target">target</paramref>�ɂ̓A�N�e�B�u�E�B���h�E������<see cref="Poderosa.Forms.IPoderosaMainWindow">IPoderosaMainWindow</see>�i���C�����j���[�̏ꍇ�j�܂��̓A�N�e�B�u�r���[������<see cref="Poderosa.Sessions.IPoderosaView">IPoderosaView</see>�i�R���e�L�X�g���j���[�̏ꍇ�j�̂����ꂩ���n����܂��B
    /// </para>
    /// <para>
    /// ���̃f���Q�[�g����̖߂�l�́A���j���[�Ƀ`�F�b�N��t����̂��ǂ����𔻒f����̂Ɏg���܂��B <seealso cref="PoderosaMenuItemImpl">PoderosaMenuItemImpl</seealso>���Q�Ƃ��Ă��������B
    /// </para>
    /// </ja>
    /// <en>
    /// The return value from this delegate is used to judge whether to put the check on the menu. Refer to <seealso cref="PoderosaMenuItemImpl">PoderosaMenuItemImpl</seealso>  
    /// </en>
    /// </remarks>
    public delegate bool CheckedDelegate(ICommandTarget target);

    /// <summary>
    /// <ja>
    /// ���j���[��c�[���o�[�{�^�����C�l�[�u�����f�B�X�G�u���������߂�Ƃ��ɌĂяo�����f���Q�[�g�ł��B
    /// </ja>
    /// <en>
    /// Delegate called when whether menu and toolbar button are enable or disable is decided
    /// </en>
    /// </summary>
    /// <param name="target">
    /// <ja>
    /// �R�}���h�̑Ώۂ������^�[�Q�b�g�ł��B
    /// </ja>
    /// <en>
    /// Target that shows object of command.
    /// </en>
    /// </param>
    /// <returns>
    /// <ja>
    /// ���j���[��c�[���o�[�̃{�^�����I���ł���Ȃ�true���A�����łȂ��Ȃ�false��Ԃ��Ă��������B
    /// </ja>
    /// <en>
    ///  Please return true and return false if it is not so if you can select the button of the menu and the toolbar. 
    /// </en>
    /// </returns>
    /// <remarks>
    /// <ja>
    /// <para>
    /// <paramref name="target">target</paramref>�ɂ̓A�N�e�B�u�E�B���h�E������<see cref="Poderosa.Forms.IPoderosaMainWindow">IPoderosaMainWindow</see>�i���C�����j���[��c�[���o�[�̏ꍇ�j�܂��̓A�N�e�B�u�r���[������<see cref="Poderosa.Sessions.IPoderosaView">IPoderosaView</see>�i�R���e�L�X�g���j���[�̏ꍇ�j�̂����ꂩ���n����܂��B
    /// </para>
    /// <para>
    /// ���̃f���Q�[�g����̖߂�l�́A���j���[��c�[���{�^�����C�l�[�u���ɂ��邩�f�B�X�G�u���ɂ��邩���߂�̂Ɏg���܂��B<seealso cref="PoderosaMenuItemImpl">PoderosaMenuItemImpl</seealso>��<seealso cref="Poderosa.Forms.ToolBarElementImpl">ToolBarElementImpl</seealso>���Q�Ƃ��Ă��������B
    /// </para>
    /// </ja>
    /// <en>
    /// The return value from this Derigat is used to provide whether to make the menu and the tool button enable or to make it to disable. Refer to <seealso cref="PoderosaMenuItemImpl">PoderosaMenuItemImpl</seealso> or <seealso cref="Poderosa.Forms.ToolBarElementImpl">ToolBarElementImpl</seealso>.
    /// </en>
    /// </remarks>
    public delegate bool EnabledDelegate(ICommandTarget target);

    /// <summary>
    /// <ja>
    /// �R�}���h�����s�\���ǂ������߂�Ƃ��ɌĂяo�����f���Q�[�g�ł��B
    /// </ja>
    /// <en>
    /// Delegate called when it is provided whether command is executable.
    /// </en>
    /// </summary>
    /// <param name="target">
    /// <ja>
    /// �R�}���h�̑Ώۂ������^�[�Q�b�g�ł��B
    /// </ja>
    /// <en>
    /// Target that shows object of command.
    /// </en>
    /// </param>
    /// <returns>
    /// <ja>
    /// �R�}���h�����s�\�Ȃ�true���A�����łȂ��Ȃ�false��Ԃ��Ă��������B
    /// </ja>
    /// <en>
    /// Return true if it is executable, false if it is not.
    /// </en>
    /// </returns>
    /// <remarks>
    /// <ja>
    /// <para>
    /// ���j���[��c�[���o�[����Ăяo�����ꍇ�A<paramref name="target">target</paramref>�ɂ̓A�N�e�B�u�E�B���h�E������<see cref="Poderosa.Forms.IPoderosaMainWindow">IPoderosaMainWindow</see>�i���C�����j���[��c�[���o�[�̏ꍇ�j�܂��̓A�N�e�B�u�r���[������<see cref="Poderosa.Sessions.IPoderosaView">IPoderosaView</see>�i�R���e�L�X�g���j���[�̏ꍇ�j�̂����ꂩ���n����܂��B
    /// </para>
    /// <para>
    /// ���̃f���Q�[�g��<seealso cref="GeneralCommandImpl">GeneralCommandImpl</seealso>��<seealso cref="PoderosaCommandImpl">PoderosaCommandImpl</seealso>�ȂǂŁA<seealso cref="IPoderosaCommand">IPoderosaCommand</seealso>��
    /// <see cref="IPoderosaCommand.CanExecute">CanExecute���\�b�h</see>���Ăяo�����^�C�~���O�ŌĂяo����܂��B
    /// </para>
    /// </ja>
    /// <en>This delegatee is called in <seealso cref="GeneralCommandImpl">GeneralCommandImpl</seealso> and <seealso cref="PoderosaCommandImpl">PoderosaCommandImpl</seealso>, etc. according to timing where the <see cref="IPoderosaCommand.CanExecute">CanExecute method</see> of <seealso cref="IPoderosaCommand">IPoderosaCommand</seealso> is called. 
    /// </en>
    /// </remarks>
    public delegate bool CanExecuteDelegate(ICommandTarget target);


    /// <exclude/>
    public delegate CommandResult ExecuteDelegateArgs(ICommandTarget target, params IAdaptable[] args);


    /// <summary>
    /// <ja>
    /// �R�}���h�����s�����Ƃ��ɌĂяo�����f���Q�[�g�ł��B
    /// </ja>
    /// <en>
    /// Delegate called when command is executed
    /// </en>
    /// </summary>
    /// <param name="target">
    /// <ja>
    /// �R�}���h�̑Ώۂ������^�[�Q�b�g�ł��B
    /// </ja>
    /// <en>
    /// Target that shows object of command.
    /// </en>
    /// </param>
    /// <returns>
    /// <ja>
    /// �R�}���h�̎��s���ʂ�Ԃ��Ă��������B
    /// </ja>
    /// <en>
    /// Please return the execution result of the command. 
    /// </en>
    /// </returns>
    /// <remarks>
    /// <ja>
    /// <para>
    /// ���j���[��c�[���o�[����Ăяo�����ꍇ�A<paramref name="target">target</paramref>�ɂ̓A�N�e�B�u�E�B���h�E������<see cref="T:Poderosa.Forms.IPoderosaMainWindow">IPoderosaMainWindow</see>�i���C�����j���[��c�[���o�[�̏ꍇ�j�܂��̓A�N�e�B�u�r���[������<see cref="T:Poderosa.Sessions.IPoderosaView">IPoderosaView</see>�i�R���e�L�X�g���j���[�̏ꍇ�j�̂����ꂩ���n����܂��B
    /// </para>
    /// <para>
    /// ���̃f���Q�[�g��<seealso cref="GeneralCommandImpl">GeneralCommandImpl</seealso>��<seealso cref="PoderosaCommandImpl">PoderosaCommandImpl</seealso>�ȂǂŁA<seealso cref="IPoderosaCommand">IPoderosaCommand</seealso>��
    /// <see cref="IPoderosaCommand.InternalExecute">InternalExecute���\�b�h</see>���Ăяo�����^�C�~���O�ŌĂяo����܂��B
    /// </para>
    /// </ja>
    /// <en>
    /// <para>
    /// Either of <see cref="Poderosa.Sessions.IPoderosaView">IPoderosaView</see> (For the context menu) that shows 
    /// <see cref="Poderosa.Forms.IPoderosaMainWindow">IPoderosaMainWindow</see> (For the main menu and the toolbar) 
    /// that shows the active window or an active view is passed to <paramref name="target">target</paramref>
    ///  when it is called from the menu and the toolbar. 
    /// </para>
    /// <para>
    /// This delegate is called in <seealso cref="GeneralCommandImpl">GeneralCommandImpl</seealso> and <seealso cref="PoderosaCommandImpl">PoderosaCommandImpl</seealso>, etc. according to timing where the <seealso cref="IPoderosaCommand">IPoderosaCommand</seealso>��
    /// <see cref="IPoderosaCommand.InternalExecute">InternalExecute method</see> of IPoderosaCommand is called. 
    /// </para>
    /// </en>
    /// </remarks>
    public delegate CommandResult ExecuteDelegate(ICommandTarget target);

    //�R�}���h�̊��
    /// <summary>
    /// <ja>
    /// �R�}���h�@�\��񋟂���v���O�C������������C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface that plug-in that offers command function implements.
    /// </en>
    /// </summary>
    /// <remarks>
    /// <ja>
    /// <para>
    /// �R�}���h�}�l�[�W���i<seealso cref="T:Poderosa.Commands.ICommandManager">ICommandManager</seealso>�j�ɂ���Ď��s�����R�}���h��񋟂���ꍇ�ɂ́A�v���O�C�������̃C���^�[�t�F�C�X���������܂��B
    /// </para>
    /// <para>
    /// �R�}���h�}�l�[�W����<see cref="M:Poderosa.Commands.ICommandManager.Execute(Poderosa.Commands.IPoderosaCommand,Poderosa.Commands.ICommandTarget,Poderosa.IAdaptable[])">Execute���\�b�h</see>���Ăяo���ƁA���̃C���^�[�t�F�C�X�Ɏ�������Ă���
    /// <see cref="M:Poderosa.Commands.IPoderosaCommand.InternalExecute(Poderosa.Commands.ICommandTarget,Poderosa.IAdaptable[])">InternalExecute���\�b�h</see>���ԐړI�ɌĂяo����܂��B
    /// </para>
    /// </ja>
    /// <en>
    /// <para>
    /// When the command executed by the command manager(<seealso cref="ICommandManager">ICommandManager</seealso>) is offered, 
    /// the plug-in implements this interface. 
    /// </para>
    /// <para>
    /// When command manager's <see cref="ICommandManager.Execute">Execute method</see> is called, 
    /// the <see cref="IPoderosaCommand.InternalExecute">InternalExecute method </see> implemented on this interface is indirectly called. 
    /// </para>
    /// </en>
    /// </remarks>
    public interface IPoderosaCommand : IAdaptable {
        //���[�U������𒼐ڌĂ�ł͂����Ȃ��BCommandManager#Execute���g�����ƁI
        /// <summary>
        /// <ja>
        /// �R�}���h�����s�����Ƃ��ɌĂяo����郁�\�b�h�ł��B
        /// </ja>
        /// <en>
        /// Method of call when command is executed
        /// </en>
        /// </summary>
        /// <param name="target">
        /// <ja>
        /// �R�}���h�̑ΏۂƂȂ�^�[�Q�b�g�ł��B
        /// </ja>
        /// <en>
        /// Target target for command.
        /// </en>
        /// <en>
        /// Target that shows object of command.
        /// </en>
        /// </param>
        /// <param name="args">
        /// <ja>
        /// �R�}���h�ɓn�����C�ӂ̈����ł��B
        /// </ja>
        /// <en>
        /// It is an arbitrary argument passed to the command. 
        /// </en>
        /// </param>
        /// <returns>
        /// <ja>
        /// �R�}���h���������������ǂ����������߂�l�ł��B���������Ƃ��ɂ�<see cref="CommandResult.Succeeded">CommandResult.Succeeded</see>��Ԃ��܂��B
        /// </ja>
        /// <en>
        /// It is a return value that shows whether it was that the command succeeds. 
        /// When succeeding, CommandResult.<see cref="CommandResult.Succeeded">CommandResult.Succeeded</see> is returned. 
        /// </en>
        /// </returns>
        /// <remarks>
        /// <ja>
        /// <para>
        /// ���̃��\�b�h�́A�R�}���h�}�l�[�W���i<seealso cref="ICommandManager">ICommandManager</seealso>�j��<see cref="ICommandManager.Execute">Execute���\�b�h</see>
        /// ���Ăяo���ꂽ�Ƃ��ɁA�ԐړI�ɌĂяo����܂��B�J���҂́A���̃��\�b�h�𒼐ڌĂяo���Ă͂����܂���B
        /// </para>
        /// <para>
        /// <paramref name="target">target</paramref>��<paramref name="args">args</paramref>�́A<see cref="ICommandManager.Execute">Execute���\�b�h</see>�̌Ăяo���œn���ꂽ���������̂܂ܓn����܂��B
        /// </para>
        /// <para>
        /// <seealso cref="CommandTargetUtil">CommandTargetUtil</seealso>���g���ƁA<paramref name="target">target</paramref>���E�B���h�E��r���[�ւƕϊ��ł��܂��B
        /// </para>
        /// </ja>
        /// <en>
        /// <para>
        /// When command manager(<seealso cref="ICommandManager">ICommandManager</seealso>)'s <see cref="ICommandManager.Execute">Execute method</see> is called, this method is indirectly called.
        /// The developer must not call this method directly. 
        /// </para>
        /// <para>
        /// As for <paramref name="target">target</paramref> and <paramref name="args">args</paramref>, the argument passed by calling 
        /// the <see cref="ICommandManager.Execute">Execute method</see> is passed as it is. 
        /// </para>
        /// <para>
        /// <paramref name="target">target</paramref> can be converted into the window and the view by using <seealso cref="CommandTargetUtil">CommandTargetUtil</seealso>. 
        /// </para>
        /// </en>
        /// </remarks>
        CommandResult InternalExecute(ICommandTarget target, params IAdaptable[] args); //Eclipse�ł͂����ɂ͈���������A�p�����[�^��R�}���h�N����������B���A����͈Ӗ��I��ICommand�̎������m���Ă���ׂ����e��

        //�R�}���h�����s�\���ǂ����̔���B���s���Ă݂�܂ł킩��Ȃ��悤�ȂƂ��͂Ƃ肠����true��Ԃ����ƁB
        /// <summary>
        /// <ja>
        /// �R�}���h�����s�\���ǂ�����Ԃ��܂��B
        /// </ja>
        /// <en>
        /// Return whether the command is executable.
        /// </en>
        /// </summary>
        /// <param name="target">
        /// <ja>
        /// �R�}���h�̎��s�ΏۂƂȂ�^�[�Q�b�g�ł��B
        /// </ja>
        /// <en>
        /// Target that shows object of command.
        /// </en>
        /// </param>
        /// <returns>
        /// <ja>
        /// ���s�\�Ȃ�true�A�����łȂ��Ȃ�false��Ԃ��Ă��������B
        /// </ja>
        /// <en>
        /// Return true if it is executable, false if it is not.
        /// </en>
        /// </returns>
        /// <remarks>
        /// <ja>
        /// <para>
        /// ���̃��\�b�h�́A���j���[��c�[���o�[���A���ڂ��f�B�X�G�u���ɂ��邩�ǂ��������߂�Ƃ��Ɏg���܂��B
        /// </para>
        /// <para>
        /// false��Ԃ��ƃf�B�X�G�u���ɂȂ�A���[�U�[���I���ł��Ȃ��Ȃ�܂��B
        /// </para>
        /// <para>
        /// �R�}���h�����s����܂ŁA���s�\���ǂ������킩��Ȃ��Ƃ��ɂ́Atrue��Ԃ��悤�Ɏ������Ă��������B
        /// </para>
        /// </ja>
        /// <en>
        /// <para>
        /// When it is decided whether the menu and the toolbar make the item disable, this method is used. 
        /// </para>
        /// <para>
        /// It becomes disable if false is returned, and the user cannot select it. 
        /// </para>
        /// <para>
        /// Please implement to return true when it is executable until the command is executed is not understood. 
        /// </para>
        /// </en>
        /// </remarks>
        bool CanExecute(ICommandTarget target);
    }

    //���C�����j���[���炽�ǂ��^�C�v�̂��
    /// <summary>
    /// <ja>
    /// �R�}���h�}�l�[�W���ɂ���ĊǗ������R�}���h�������C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface that shows command managed by command manager
    /// </en>
    /// </summary>
    public interface IGeneralCommand : IPoderosaCommand {
        /// <summary>
        /// <ja>
        /// �R�}���h������Ŏ��ʂ��邽�߂́u�R�}���hID�v�ł��B���̃R�}���h�Ƃ͏d�����Ȃ���ӂ̂��̂�ݒ肵�܂��B 
        /// </ja>
        /// <en>
        /// It is "command ID" to identify the command internally. The unique one that doesn't overlap is set as other commands. 
        /// </en>
        /// </summary>
        /// <remarks>
        /// <ja>
        /// �J���҂��R�}���h��񋟂���ꍇ�ɂ́A���̃v���O�C�����񋟂���R�}���hID�Əd�����Ȃ��悤�ɂ��邽�߁A
        /// �u�v���O�C��ID�v�̉��ɓK���Ȗ��O��t���������K���Łu�R�}���hID�v�����肷�邱�Ƃ𐄏����܂��B
        /// ���Ƃ��΁u<c>co.jp.example.myplugin</c>�v�Ƃ����v���O�C��ID�����v���O�C���Ȃ�΁A
        /// �R�}���hID�Ƃ��āu<c>co.jp.example.myplugin.mycommand</c>�v�Ƃ��������O��t����悤�ɂ��܂��B 
        /// </ja>
        /// <en>
        /// Command ID is recommended to be decided in the naming convention that names a suitable name under "plug-in ID" to make it not overlap with "command ID" that other plug-ins offer when the developer offers the command. 
        /// For instance, if it is a plug-in with plug-in ID of "<c>co.jp.example.myplugin</c>", the name of "<c>co.jp.example.myplugin.mycommand</c>" is named as command ID. 
        /// </en>
        /// </remarks>
        string CommandID {
            get;
        }
        /// <summary>
        /// <ja>
        /// �R�}���h�̐������ł��B
        /// </ja>
        /// <en>
        /// Explanation of command
        /// </en>
        /// </summary>
        /// <remarks>
        /// <ja>
        /// �ݒ肵���l�́A�I�v�V������ʂ́u�R�}���h�v���ɕ\������镶����ɂȂ�܂��B
        /// </ja>
        /// <en>
        /// The set value becomes a character string displayed in "Command" column on the option screen. 
        /// </en>
        /// </remarks>
        string Description {
            get;
        }
        /// <summary>
        /// <ja>
        /// ���̃R�}���h�Ɋ��蓖�Ă���f�t�H���g�̃V���[�g�J�b�g�L�[�ł��B
        /// </ja>
        /// <en>
        /// It is a shortcut key of the default allocated in this command. 
        /// </en>
        /// </summary>
        /// <remarks>
        /// <ja>
        /// <para>
        /// �V���[�g�J�b�g�L�[�����蓖�ĂȂ��ꍇ�ɂ́A<c>Keys.None</c>��n���Ă��������B
        /// </para>
        /// <para>
        /// �V���[�g�J�b�g�������̃R�}���h���p���Ă�����̂Əd������ꍇ�ɂ́A��������܂��B
        /// </para>
        /// </ja>
        /// <en>
        /// <para>
        /// Please pass <c>Keys.None</c> when you do not allocate the shortcut key. 
        /// </para>
        /// <para>
        /// When the short cut overlaps with the one that an existing command uses, it is disregarded. 
        /// </para>
        /// </en>
        /// </remarks>
        Keys DefaultShortcutKey {
            get;
        }
        /// <summary>
        /// <ja>
        /// �R�}���h�J�e�S��������<seealso cref="Poderosa.Commands.ICommandCategory">ICommandCategory</seealso>�ł��B
        /// </ja>
        /// <en>
        /// <seealso cref="Poderosa.Commands.ICommandCategory">ICommandCategory</seealso> that show the command category.
        /// </en>
        /// </summary>
        /// <remarks>
        /// <ja>
        /// <seealso cref="Poderosa.Commands.ICommandCategory">ICommandCategory</seealso>�́A
        /// <seealso cref="Poderosa.Commands.ICommandManager">ICommandManager</seealso>��
        /// <see cref="Poderosa.Commands.ICommandManager.CommandCategories">CommandCategories�v���p�e�B</see>���瓾���A
        /// ��`�ς݃J�e�S����p���邱�Ƃ��ł��܂��B
        /// </ja>
        /// <en>
        /// <seealso cref="Poderosa.Commands.ICommandCategory">ICommandCategory</seealso> can use the category that has been 
        /// defined obtaining it from the <see cref="Poderosa.Commands.ICommandManager.CommandCategories">CommandCategories property</see> 
        /// of <seealso cref="Poderosa.Commands.ICommandManager">ICommandManager</seealso>. 
        /// </en>
        /// </remarks>
        ICommandCategory CommandCategory {
            get;
        }
    }

    //PositionDesignation��
    /// <summary>
    /// <ja>
    /// �R�}���h�J�e�S���������C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface that shows command category.
    /// </en>
    /// </summary>
    /// <remarks>
    /// <ja>
    /// ��`�ς݃J�e�S���́A<seealso cref="ICommandManager">ICommandManager</seealso>��<see cref="ICommandManager.CommandCategories">CommandCategories�v���p�e�B</see>����擾�ł��܂��B
    /// </ja>
    /// <en>
    /// It is possible to get it from the <see cref="ICommandManager.CommandCategories">CommandCategories property</see> of <seealso cref="ICommandManager">ICommandManager</seealso>. 
    /// </en>
    /// </remarks>
    public interface ICommandCategory : IAdaptable {
        /// <summary>
        /// <ja>
        /// �R�}���h�J�e�S���̖��O�ł��B
        /// </ja>
        /// <en>
        /// Name of the command category.
        /// </en>
        /// </summary>
        string Name {
            get;
        }
        /// <summary>
        /// <ja>
        /// �L�[�o�C���h�̃J�X�^�}�C�Y���\���ǂ����������܂��B
        /// </ja>
        /// <en>
        /// It is shown whether customizing key bind is possible. 
        /// </en>
        /// </summary>
        /// <remarks>
        /// <ja>
        /// <para>
        /// true�ɂ���ƃI�v�V�����ݒ��ʂɁA���̍��ڂ��\������A�L�[�o�C���h�̕ύX���ł���悤�ɂȂ�܂��B
        /// </para>
        /// <para>
        /// false�ɂ���ƃI�v�V�����ݒ��ʂ���B����܂��B
        /// </para>
        /// </ja>
        /// <en>
        /// <para>
        /// This item is displayed on the option setting screen when making it to true, and it comes to be able to change key bind. 
        /// </para>
        /// <para>
        /// It is concealed because of the option setting screen when making it to false. 
        /// </para>
        /// </en>
        /// </remarks>
        bool IsKeybindCustomizable {
            get;
        }
    }

    /// <summary>
    /// <ja>
    /// ��`�ς݃R�}���h�J�e�S���������܂�
    /// </ja>
    /// <en>
    /// Defined  command category.
    /// </en>
    /// </summary>
    public interface IDefaultCommandCategories {
        /// <summary>
        /// <ja>
        /// �m�t�@�C���n�������J�e�S���ł��B
        /// </ja>
        /// <en>
        /// Category that shows the "File".
        /// </en>
        /// </summary>
        ICommandCategory File {
            get;
        }
        /// <summary>
        /// <ja>
        /// �m�_�C�A���O�n�������J�e�S���ł��B
        /// </ja>
        /// <en>
        /// Category that shows the "Dialog".
        /// </en>
        /// </summary>
        ICommandCategory Dialogs {
            get;
        }
        /// <summary>
        /// <ja>
        /// �m�ҏW�n�������J�e�S���ł��B
        /// </ja>
        /// <en>
        /// Category that shows the "Edit".
        /// </en>
        /// </summary>
        ICommandCategory Edit {
            get;
        }
        /// <summary>
        /// <ja>
        /// �m�E�B���h�E�n�������J�e�S���ł��B
        /// </ja>
        /// <en>
        /// Category that shows the "Window".
        /// </en>
        /// </summary>
        ICommandCategory Window {
            get;
        }
    }

    //GeneralCommand�̃R���N�V����
    /// <summary>
    /// <ja>
    /// �R�}���h�}�l�[�W���������C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface that shows command manager.
    /// </en>
    /// </summary>
    /// <remarks>
    /// <ja>
    /// <para>
    /// �R�}���h�}�l�[�W���́u<c>org.poderosa.core.commands</c>�v�Ƃ����v���O�C��ID������CommandManagerPlugin�v���O�C���ɂ����
    /// �񋟂���Ă��܂��B
    /// </para>
    /// <para>
    /// ���̃C���^�[�t�F�C�X���擾����ɂ́A�i1�j<seealso cref="Poderosa.Plugins.IPluginManager">IPluginManager</seealso>��
    /// <see cref="Poderosa.Plugins.IPluginManager.FindPlugin">FindPlugin���\�b�h</see>�Łu<c>org.poderosa.core.commands</c>�v����������A
    /// �i2�j<seealso cref="Poderosa.Plugins.ICoreServices">ICoreServices</seealso>��<see cref="Poderosa.Plugins.ICoreServices.CommandManager">CommandManager�v���p�e�B</see>���g���Ď擾����A�̂����ꂩ�̕��@���Ƃ�܂��B
    /// </para>
    /// </ja>
    /// <en>
    /// <para>
    /// The command manager is offered by CommandManagerPlugin plug-in with plug-in ID of "<c>org.poderosa.core.commands</c>".
    /// </para>
    /// <para>
    /// To get this interface, the developer cat adopt either of method. (1) Retrieve [<c>org.poderosa.core.commands</c>] by <see cref="Poderosa.Plugins.IPluginManager.FindPlugin">FindPlugin method</see> on <seealso cref="Poderosa.Plugins.IPluginManager">IPluginManager</seealso>.
    /// </para>
    /// </en>
    /// </remarks>
    /// <example>
    /// <ja>
    /// ICoreService��CommandManager�v���p�e�B����ICommandManager���擾���܂��B
    /// <code>
    /// // ������PoderosaWorld�́AInitializePlugin���\�b�h�Ŏ󂯎����
    /// // IPoderosaWorld�ł���Ɖ��肵�܂��B
    /// ICoreServices cs = (ICoreServices)PoderosaWorld.GetAdapter(typeof(ICoreServices));
    /// ICommandManager cm = cs.CommandManager;
    /// </code>
    /// </ja>
    /// <en>
    /// Get ICommandManager from CommandManager property on ICoreService.
    /// <code>
    /// // It is assumed that PoderosaWorld is IPoderosaWorld here received by the InitializePlugin method. 
    /// ICoreServices cs = (ICoreServices)PoderosaWorld.GetAdapter(typeof(ICoreServices));
    /// ICommandManager cm = cs.CommandManager;
    /// </code>
    /// </en>
    /// </example>
    public interface ICommandManager : IAdaptable {
        /// <summary>
        /// <ja>
        /// �R�}���h���R�}���h�}�l�[�W���ɓo�^���܂��B
        /// </ja>
        /// <en>
        /// Regist command to the command manager.
        /// </en>
        /// </summary>
        /// <param name="command">
        /// <ja>
        /// �o�^�������R�}���h�ł��B
        /// </ja>
        /// <en>
        /// Command to be regist.
        /// </en>
        /// </param>
        void Register(IGeneralCommand command);
        /// <summary>
        /// <ja>
        /// �R�}���hID���L�[�ɂ��āA�R�}���h�}�l�[�W���ɓo�^���ꂽ�R�}���h���������܂��B
        /// </ja>
        /// <en>
        /// Retrieve command ID is made a key, and the command registered by the command manager.
        /// </en>
        /// </summary>
        /// <param name="id">
        /// <ja>
        /// ��������R�}���hID�ł��B
        /// </ja>
        /// <en>
        /// Retrieval of the command ID.
        /// </en>
        /// </param>
        /// <returns>
        /// <ja>
        /// ���������R�}���h�I�u�W�F�N�g��<seealso cref="IGeneralCommand">IGeneralCommand</seealso>���Ԃ���܂��B
        /// ������Ȃ������Ƃ��ɂ�<c>null</c>���Ԃ���܂��B
        /// </ja>
        /// <en>
        /// <seealso cref="IGeneralCommand">IGeneralCommand</seealso> of the found command object is returned. 
        /// When not found, <c>null</c> is returned. 
        /// </en>
        /// </returns>
        /// <overloads>
        /// <summary>
        /// <ja>�R�}���h�����s���܂��B</ja>
        /// <en>Execute the command.</en>
        /// </summary>
        /// </overloads>
        IGeneralCommand Find(string id);
        /// <summary>
        /// <ja>
        /// �V���[�g�J�b�g�L�[���L�[�ɂ��āA�R�}���h�}�l�[�W���ɓo�^���ꂽ�R�}���h���������܂��B
        /// </ja>
        /// <en>
        /// The shortcut key is made a key, and the command registered by the command manager is retrieved. 
        /// </en>
        /// </summary>
        /// <param name="key">
        /// <ja>
        /// ��������V���[�g�J�b�g�L�[�ł��B
        /// </ja>
        /// <en>
        /// Retrieval short cut key.
        /// </en>
        /// </param>
        /// <returns>
        /// <ja>
        /// ���������R�}���h�I�u�W�F�N�g��<seealso cref="IGeneralCommand">IGeneralCommand</seealso>���Ԃ���܂��B
        /// ������Ȃ������Ƃ��ɂ�<c>null</c>���Ԃ���܂��B
        /// </ja>
        /// <en>
        /// <seealso cref="IGeneralCommand">IGeneralCommand</seealso> of the found command object is returned. 
        /// When not found, <c>null</c> is returned. 
        /// </en>
        /// </returns>
        IGeneralCommand Find(Keys key); //�V���[�g�J�b�g�L�[����
        /// <summary>
        /// <ja>
        /// �R�}���h�}�l�[�W���ɓo�^����Ă��邷�ׂẴR�}���h�I�u�W�F�N�g��񋓂��܂��B
        /// </ja>
        /// <en>
        /// Enumerate all the command objects being registered by the command manager.
        /// </en>
        /// </summary>
        IEnumerable<IGeneralCommand> Commands {
            get;
        }

        /// <summary>
        /// <ja>
        /// �w�肳�ꂽ�R�}���h�����s���܂��B
        /// </ja>
        /// <en>
        /// Execute the specified command.
        /// </en>
        /// </summary>
        /// <param name="command">
        /// <ja>
        /// ���s����R�}���h�ł��B
        /// </ja>
        /// <en>
        /// Command to execute.
        /// </en>
        /// </param>
        /// <param name="target">
        /// <ja>
        /// �R�}���h�̃^�[�Q�b�g�ł��B
        /// </ja>
        /// <en>
        /// Target of command.
        /// </en>
        /// </param>
        /// <param name="args">
        /// <ja>
        /// �R�}���h�ɓn���C�ӂ̈����ł��B
        /// </ja>
        /// <en>
        /// Arbitrary argument passed to command
        /// </en>
        /// </param>
        /// <returns>
        /// <ja>
        /// �R�}���h�̎��s���ʂł��B���̒l�́A<seealso cref="IPoderosaCommand">IPoderosaCommand</seealso>��
        /// <see cref="IPoderosaCommand.InternalExecute">InternalExecute���\�b�h</see>���Ԃ��l�Ɠ����ł��B
        /// </ja>
        /// <en>
        /// It is an execution result of the command. This value is the same as the value that the 
        /// <see cref="IPoderosaCommand.InternalExecute">InternalExecute method</see>
        ///  of <seealso cref="IPoderosaCommand">IPoderosaCommand</seealso> returns. 
        /// </en>
        /// </returns>
        /// <remarks>
        /// <ja>
        /// <para>
        /// ���j���[��c�[���o�[����Ăяo�����R�}���h�́A<paramref name="target"/>�ɂ́A
        /// ���C���E�B���h�E������<see cref="Poderosa.Forms.IPoderosaMainWindow">IPoderosaMainWindow</see>���n����邱�Ƃ�z�肵�Ă��܂��B���̎�ȊO�̃C���^�[�t�F�C�X���n���ꂽ�Ƃ��ɂ́A
        /// ���������삵�܂���B
        /// </para>
        /// </ja>
        /// <en>
        /// <para>
        /// The command called from the menu and the toolbar assumes to <paramref name="target"/> <see cref="Poderosa.Forms.IPoderosaMainWindow">IPoderosaMainWindow</see>'s that shows the main window being passed. 
        /// When interfaces other than this seed are passed, it doesn't operate correctly. 
        /// </para>
        /// </en>
        /// </remarks>
        /// <example>
        /// <ja>
        /// �u�t�@�C���փR�s�[�v�̋@�\���������Ă���R�}���h�u<c>org.poderosa.terminalemulator.copytofile</c>�v���Ăяo���āA
        /// ���ݑI������Ă���͈͂��t�@�C���ւƃR�s�[���܂��B
        /// <code>
        /// // ICoreServices�̎擾
        /// ICoreServices cs = (ICoreServices)PoderosaWorld.GetAdapter(typeof(ICoreServices));
        /// // �R�}���h�}�l�[�W���̎擾
        /// ICommandManager cm = cs.CommandManager;
        /// 
        /// // �u�t�@�C���փR�s�[�v�̃R�}���h������
        /// IGeneralCommand cmd = cm.Find("org.poderosa.terminalemulator.copytofile");
        /// 
        /// // �A�N�e�B�u�E�B���h�E��IPoderosaMainWindow�𓾂�
        /// IPoderosaMainWindow mainwin = cs.WindowManager.ActiveWindow;
        /// 
        /// // ���s
        /// CommandResult result = cm.Execute(cmd, mainwin);
        /// </code>
        /// </ja>
        /// <en>
        /// Command "<c>org.poderosa.terminalemulator.copytofile</c>" where the function of "Save to file" is implemented is called, 
        /// and the range that has been selected now is copied to the file. 
        /// <code>
        /// // Get ICoreServices
        /// ICoreServices cs = (ICoreServices)PoderosaWorld.GetAdapter(typeof(ICoreServices));
        /// // Get command manager.
        /// ICommandManager cm = cs.CommandManager;
        /// 
        /// // Retrieval of the command of "Save to file"
        /// IGeneralCommand cmd = cm.Find("org.poderosa.terminalemulator.copytofile");
        /// 
        /// // Get the IPoderosaMainWindow of the active window.
        /// IPoderosaMainWindow mainwin = cs.WindowManager.ActiveWindow;
        /// 
        /// // Execute
        /// CommandResult result = cm.Execute(cmd, mainwin);
        /// </code>
        /// </en>
        /// </example>
        CommandResult Execute(IPoderosaCommand command, ICommandTarget target, params IAdaptable[] args);

        /// <exclude/>
        IKeyBinds CurrentKeyBinds {
            get;
        }

        /// <exclude/>
        IKeyBinds GetKeyBinds(IPreferenceFolder folder); //������ƈ�a���B�ʃC���^�t�F�[�X�ɕ�����H

        /// <summary>
        /// <ja>
        /// ��`�ς݃R�}���h�J�e�S�����擾���邽�߂̃C���^�[�t�F�C�X�ł��B
        /// </ja>
        /// <en>
        /// Interface to get command category that has been defined.
        /// </en>
        /// </summary>
        IDefaultCommandCategories CommandCategories {
            get;
        }
    }

    //�L�[�o�C���h�ݒ�
    /// <summary>
    /// <ja>
    /// �L�[�o�C���h�̐ݒ�𑀍삷��C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface that operates setting of key bind.
    /// </en>
    /// </summary>
    public interface IKeyBinds {
        /// <summary>
        /// <ja>
        /// ���蓖�Ă��Ă���V���[�g�J�b�g�L�[�̃R���N�V�����ł��B
        /// </ja>
        /// <en>
        /// Collection of allocated shortcut key.
        /// </en>
        /// </summary>
        ICollection Commands {
            get;
        }

        /// <summary>
        /// <ja>
        /// �R�}���h�Ɋ��蓖�Ă�ꂽ�V���[�g�J�b�g�L�[��Ԃ��܂��B
        /// </ja>
        /// <en>
        /// Return the shortcut key allocated in the command.
        /// </en>
        /// </summary>
        /// <param name="command">
        /// <ja>
        /// ���ׂ����R�}���h�ł��B
        /// </ja>
        /// <en>
        /// Command that wants to be examined.
        /// </en>
        /// </param>
        /// <returns>
        /// <ja>
        /// �L�[�Ɋ��蓖�Ă�ꂽ�V���[�g�J�b�g�L�[���߂�܂��B�V���[�g�J�b�g�L�[�����蓖�Ă��Ă��Ȃ��ꍇ�ɂ́AKeys.None���߂�܂��B
        /// </ja>
        /// <en>
        /// The shortcut key allocated in the key returns. Keys.None returns when the shortcut key is not allocated. 
        /// </en>
        /// </returns>
        Keys GetKey(IGeneralCommand command);
        /// <summary>
        /// <ja>
        /// �R�}���h�ɑ΂��ăV���[�g�J�b�g�L�[�����蓖�Ă܂��B
        /// </ja>
        /// <en>
        /// The shortcut key is allocated to the command. 
        /// </en>
        /// </summary>
        /// <param name="command">
        /// <ja>
        /// �ΏۂƂȂ�R�}���h�ł��B
        /// </ja>
        /// <en>
        /// Command that becomes object.
        /// </en>
        /// </param>
        /// <param name="key">
        /// <ja>
        /// ���蓖�Ă�V���[�g�J�b�g�L�[�ł��B
        /// </ja>
        /// <en>
        /// Allocated shortcut key
        /// </en>
        /// </param>
        /// <exception cref="ArgumentException">
        /// <ja>
        /// �Y���̃L�[�ɂ́A���łɂق��̃R�}���h�����蓖�Ă��Ă��܂��B
        /// </ja>
        /// <en>
        /// Other commands have already been allocated in the key to the correspondence. 
        /// </en>
        /// </exception>
        /// <remarks>
        /// <ja>
        /// <para>
        /// <paramref name="key">key</paramref>��Keys.None��n���ƁA�V���[�g�J�b�g�L�[�̊��蓖�Ă������ł��܂��B
        /// </para>
        /// <para>
        /// <paramref name="key">key</paramref>�ɂ��łɃR�}���h�����蓖�Ă��Ă���Ƃ��ɂ͗�O���������܂��B
        /// </para>
        /// </ja>
        /// <en>
        /// <para>
        /// The allocation of the shortcut <paramref name="key">key</paramref> can be released by passing key Keys.None. 
        /// </para>
        /// <para>
        /// When the command has already been allocated in <paramref name="key">key</paramref>, the exception is generated. 
        /// </para>
        /// </en>
        /// </remarks>
        void SetKey(IGeneralCommand command, Keys key);

        /// <summary>
        /// <ja>
        /// �V���[�g�J�b�g�L�[�Ɋ��蓖�Ă��Ă���R�}���h���������܂��B
        /// </ja>
        /// <en>
        /// Retrieval the command allocated in the shortcut key.
        /// </en>
        /// </summary>
        /// <param name="key">
        /// <ja>
        /// ��������V���[�g�J�b�g�L�[�ł��B
        /// </ja>
        /// <en>
        /// Retrieved shortcut key
        /// </en>
        /// </param>
        /// <returns>
        /// <ja>�V���[�g�J�b�g�L�[�Ɋ��蓖�Ă��Ă���R�}���h���Ԃ���܂��B������Ȃ��Ƃ��ɂ�null���Ԃ���܂��B</ja>
        /// <en>The command allocated in the shortcut key is returned. When not found, null is returned. </en>
        /// </returns>
        IGeneralCommand FindCommand(Keys key);

        /// <summary>
        /// <ja>
        /// �V���[�g�J�b�g�L�[�̊��蓖�Ă����ׂăN���A���܂��B
        /// </ja>
        /// <en>
        /// The allocation of the shortcut key is all cleared. 
        /// </en>
        /// </summary>
        void ClearAll();
        /// <summary>
        /// <ja>
        /// �V���[�g�J�b�g�L�[�̊��蓖�Ă��f�t�H���g�ɖ߂��܂��B
        /// </ja>
        /// <en>
        /// The allocation of the shortcut key is set to default.
        /// </en>
        /// </summary>
        void ResetToDefault();
        /// <summary>
        /// <ja>
        /// �V���[�g�J�b�g�L�[�̊��蓖�Ă��C���|�[�g���܂��B
        /// </ja>
        /// <en>
        /// Import the allocation of the shortcut key.
        /// </en>
        /// </summary>
        /// <param name="keybinds">
        /// <ja>
        /// �C���|�[�g����V���[�g�J�b�g�L�[�ł��B
        /// </ja>
        /// <en>
        /// The shortcut key to import.
        /// </en>
        /// </param>
        void Import(IKeyBinds keybinds);

    }

    //���j���[����

    //Extension Point�ւ̐ڑ��p
    /// <summary>
    /// <ja>
    /// ���j���[�̌X�̃A�C�e���������C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface that shows each item of menu.
    /// </en>
    /// </summary>
    /// <remarks>
    /// <ja>
    /// ���j���[���ڂ��쐬����ꍇ�ɂ́A<seealso cref="PoderosaMenuItemImpl">PoderosaMenuItemTmpl</seealso>���g���܂��B
    /// </ja>
    /// <en>
    /// When the menu item is made, <seealso cref="PoderosaMenuItemImpl">PoderosaMenuItemTmpl</seealso> is used. 
    /// </en>
    /// </remarks>
    public interface IPoderosaMenu : IAdaptable {
        /// <summary>
        /// <ja>
        /// ���j���[�ɕ\�������e�L�X�g�ł��B
        /// </ja>
        /// <en>
        /// Text displayed in menu
        /// </en>
        /// </summary>
        string Text {
            get;
        }
        /// <summary>
        /// <ja>
        /// ���j���[�̃C�l�[�u���^�f�B�X�G�u���̏�Ԃ�Ԃ����\�b�h�ł��B
        /// </ja>
        /// <en>
        /// Method of returning state of enable/disable of menu
        /// </en>
        /// </summary>
        /// <param name="target">
        /// <ja>
        /// �R�}���h�̃^�[�Q�b�g�ł��B
        /// </ja>
        /// <en>
        /// Target of command.
        /// </en>
        /// </param>
        /// <returns>
        /// <ja>
        /// ���j���[���C�l�[�u���ł���ꍇ�ɂ�true�A�f�B�X�G�u���ł���ꍇ�ɂ�false���Ԃ���܂��B
        /// </ja>
        /// <en>
        /// When it is true, and disable when the menu is enable, false is returned. 
        /// </en>
        /// </returns>
        bool IsEnabled(ICommandTarget target);
        /// <summary>
        /// <ja>
        /// ���j���[�̃`�F�b�N��Ԃ�Ԃ����\�b�h�ł��B
        /// </ja>
        /// <en>
        /// Method of returning check state on menu
        /// </en>
        /// </summary>
        /// <param name="target">
        /// <ja>
        /// �R�}���h�̃^�[�Q�b�g�ł��B
        /// </ja>
        /// <en>
        /// Target of command.
        /// </en>
        /// </param>
        /// <returns>
        /// <ja>
        /// ���j���[�Ƀ`�F�b�N���t���Ă���ꍇ�ɂ�true�A�����łȂ��ꍇ�ɂ�false���Ԃ���܂��B
        /// </ja>
        /// <en>
        /// When the menu is checked, true is returned when it is false so. 
        /// </en>
        /// </returns>
        bool IsChecked(ICommandTarget target);
    }

    //MenuGroup�̓o�[�̃f���~�^������P��
    /// <summary>
    /// <ja>
    /// ���j���[���ڂ��W�߂����j���[�O���[�v���\������C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface that composes menu group that collects menu items.
    /// </en>
    /// </summary>
    /// <remarks>
    /// <ja>
    /// <para>
    /// ���j���[��Poderosa�ɓo�^����ꍇ�ɂ́A���j���[���ڂ��W�߂����j���[�O���[�v���쐬���A�g���|�C���g�ւƓo�^���܂��B
    /// </para>
    /// <para>
    /// ���j���[�O���[�v���쐬����ꍇ�ɂ́A<seealso cref="PoderosaMenuGroupImpl">PoderosaMenuGroupImpl</seealso>���g�����Ƃ��ł��܂��B
    /// </para>
    /// </ja>
    /// <en>
    /// <para>
    /// The menu group that collects the menu items is made when the menu is registered in Poderosa, and it registers to the extension point. 
    /// </para>
    /// <para>
    /// When the menu group is made, <seealso cref="PoderosaMenuGroupImpl">PoderosaMenuGroupImpl</seealso> can be used. 
    /// </para>
    /// </en>
    /// </remarks>
    public interface IPoderosaMenuGroup : IAdaptable {
        /// <summary>
        /// <ja>
        /// ���̃��j���[�O���[�v�Ɋ܂܂�郁�j���[���ڂ̔z��ł��B
        /// </ja>
        /// <en>
        /// Array of menu item included in this menu group.
        /// </en>
        /// </summary>
        IPoderosaMenu[] ChildMenus {
            get;
        }
        /// <summary>
        /// <ja>
        /// ���j���[���ڂ����I�ɍ���邩�ǂ����������܂��B
        /// </ja>
        /// <en>
        /// It is shown whether the menu item is dynamically made. 
        /// </en>
        /// </summary>
        /// <remarks>
        /// <ja>
        /// true�ł���ꍇ�A���j���[���ڂ��\������悤�Ƃ��邽�тɁA���j���[���Đ�������܂��B���I�ȃ��j���[���\������ꍇ�ɂ�true���A�����łȂ��ꍇ�ɂ�false��Ԃ��悤�Ɏ������܂��B
        /// </ja>
        /// <en>
        /// The menu is done whenever the menu item tries to be displayed when it is true and the reproduction is done. 
        /// When a dynamic menu is composed, true is implemented to return false when it is not so. 
        /// </en>
        /// </remarks>
        bool IsVolatileContent {
            get;
        }
        /// <summary>
        /// <ja>
        /// ���̃��j���[�O���[�v�̑O�ɋ�؂�L���i�Z�p���[�^�j�����邩�ǂ����������܂��B
        /// </ja>
        /// <en>
        /// It is shown whether the separator enters ahead of this menu group. 
        /// </en>
        /// </summary>
        /// <remarks>
        /// <ja>
        /// true�ł���ꍇ�A���̃��j���[�O���[�v�̒��O�ɋ�؂�L���i�Z�p���[�^�j���\������܂��B
        /// </ja>
        /// <en>
        /// When it is true, the separator is displayed just before this menu group. 
        /// </en>
        /// </remarks>
        bool ShowSeparator {
            get;
        } //�O���[�v�̑O�ɃZ�p���[�^�����邩�ǂ���
    }

    /// <summary>
    /// <ja>
    /// ���j���[���K�w�����邽�߂̃C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface to hierarchize menu
    /// </en>
    /// </summary>
    public interface IPoderosaMenuFolder : IPoderosaMenu {
        /// <summary>
        /// <ja>
        /// �K�w�������T�u���j���[�̔z��ł��B
        /// </ja>
        /// <en>
        /// Array of hierarchized submenu
        /// </en>
        /// </summary>
        IPoderosaMenuGroup[] ChildGroups {
            get;
        }
    }


    /// <summary>
    /// <ja>
    /// ���s�����Ƃ��Ɉ����𔺂�Ȃ����j���[���ڂ������C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface that shows menu item not to accompany argument when executed.
    /// </en>
    /// </summary>
    /// <remarks>
    /// <ja>
    /// ���̃��j���[���ڂ́A<seealso cref="PoderosaMenuItemImpl">PoderosaMenuItemImpl</seealso>���g�����Ƃō쐬�ł��܂��B
    /// </ja>
    /// <en>
    /// This menu item can be made by using <seealso cref="PoderosaMenuItemImpl">PoderosaMenuItemImpl</seealso>. 
    /// </en>
    /// </remarks>
    public interface IPoderosaMenuItem : IPoderosaMenu {
        /// <summary>
        /// <ja>
        /// ���j���[���I�����ꂽ�Ƃ��ɌĂяo�����R�}���h�ł��B
        /// </ja>
        /// <en>
        /// Command called when menu is selected.
        /// </en>
        /// </summary>
        /// <remarks>
        /// <ja>
        /// ���j���[���I�������ƁA���̃v���p�e�B�Őݒ肵��<see cref="IPoderosaCommand.InternalExecute">InternalExecute���\�b�h</see>
        /// ���Ăяo����܂��B
        /// </ja>
        /// <en>
        /// The <see cref="IPoderosaCommand.InternalExecute">InternalExecute method</see> that sets for the menu to be selected in this property is called. 
        /// </en>
        /// </remarks>
        IPoderosaCommand AssociatedCommand {
            get;
        }
    }

    //MRU�ȂǁA�����t���̂��
    /// <summary>
    /// <ja>
    /// ���s�����Ƃ��Ɉ����𔺂����j���[���ڂ������C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface that shows menu item with argument when executed
    /// </en>
    /// </summary>
    public interface IPoderosaMenuItemWithArgs : IPoderosaMenuItem {
        /// <summary>
        /// <ja>
        /// �R�}���h�����s�����Ƃ��Ɉ����n���C�ӂ̈����ł��B
        /// </ja>
        /// <en>
        /// Arbitrary argument handed over when command is executed.
        /// </en>
        /// </summary>
        /// <remarks>
        /// <ja>
        /// ���̈����́A<seealso cref="IPoderosaCommand">IPoderosaCommand</seealso>��
        /// <see cref="IPoderosaCommand.InternalExecute">InternalExecute���\�b�h</see>
        /// ���Ăяo�����Ƃ��A��3�����ɂ��̂܂ܓn����܂��B
        /// </ja>
        /// <en>
        /// When the <see cref="IPoderosaCommand.InternalExecute">InternalExecute method</see> of <seealso cref="IPoderosaCommand">IPoderosaCommand</seealso> is called, this argument is passed to the third argument as it is. 
        /// </en>
        /// </remarks>
        IAdaptable[] AdditionalArgs {
            get;
        }
    }

    //�R���e�L�X�g���j���[����������\�͂̂���N���X������
    /// <summary>
    /// <ja>
    /// �R���e�L�X�g���j���[�̋@�\�����v���O�C�����������ׂ��N���X�ł��B
    /// </ja>
    /// <en>
    /// Class that plug-in with function of context menu should implement.
    /// </en>
    /// </summary>
    public interface IPoderosaContextMenuPoint : IAdaptable {
        //null����
        /// <summary>
        /// <ja>
        /// �R���e�L�X�g���j���[���������j���[�O���[�v�ł��B
        /// </ja>
        /// <en>
        /// Menu group that shows context menu
        /// </en>
        /// </summary>
        IPoderosaMenuGroup[] ContextMenu {
            get;
        }
    }



    //IPoderosaCommand�W������
    /// <summary>
    /// <ja>
    /// <seealso cref="IPoderosaCommand">IPoderosaCommand</seealso>�����������N���X�ł��B�R�}���h���쐬����ۂɎg���܂��B
    /// </ja>
    /// <en>
    /// It is a class that implements <seealso cref="IPoderosaCommand">IPoderosaCommand</seealso>. When the command is made, it uses it. 
    /// </en>
    /// </summary>
    /// <remarks>
    /// <ja>
    /// <para>
    /// �R�}���h����������J���҂́A���̃N���X���g�����Ƃ�<seealso cref="IPoderosaCommand">IPoderosaCommand</seealso>�����������I�u�W�F�N�g
    /// ��e�Ղɍ쐬�ł��܂��B
    /// </para>
    /// <para>
    /// �V���[�g�J�b�g�L�[�����蓖�Ă�R�}���h���쐬����ꍇ�ɂ́A<seealso cref="GeneralCommandImpl">GeneralCommandImpl</seealso>���g���Ă��������B
    /// </para>
    /// </ja>
    /// <en>
    /// <para>
    /// The developer who implements the command can easily make the object where <seealso cref="IPoderosaCommand">IPoderosaCommand</seealso> is implemented by using this class. 
    /// </para>
    /// <para>
    /// Please use <seealso cref="GeneralCommandImpl">GeneralCommandImpl</seealso> when you make the command that allocates the shortcut key. 
    /// </para>
    /// </en>
    /// </remarks>
    /// <example>
    /// <ja>
    /// <seealso cref="IPoderosaCommand">IPoderosaCommand</seealso>�����������I�u�W�F�N�g�́A���̂悤�ɂ��č쐬�ł��܂��B
    /// <code>
    /// PoderosaCommandImpl mycommand = new PoderosaCommandImpl(
    ///   delegate(ICommandTarget target)
    ///   {
    ///     // �R�}���h�����s���ꂽ�Ƃ��̏���
    ///    MessageBox.Show("���s����܂���");
    ///    return CommandResult.Succeeded;
    ///   },
    ///   delegate(ICommandTarget target)
    ///   {
    ///     // �R�}���h�����s�ł��邩�ǂ�����Ԃ�
    ///    return true;
    ///  }
    /// );
    /// </code>
    /// </ja>
    /// <en>
    /// The object that implements <seealso cref="IPoderosaCommand">IPoderosaCommand</seealso> can be made as follows. 
    /// <code>
    /// PoderosaCommandImpl mycommand = new PoderosaCommandImpl(
    ///   delegate(ICommandTarget target)
    ///   {
    ///     // Processing when command is executed
    ///    MessageBox.Show("Executed");
    ///    return CommandResult.Succeeded;
    ///   },
    ///   delegate(ICommandTarget target)
    ///   {
    ///     // Return whether the command can be executed. 
    ///    return true;
    ///  }
    /// );
    /// </code>
    /// </en>
    /// </example>
    public class PoderosaCommandImpl : IPoderosaCommand {
        /// <exclude/>
        protected ExecuteDelegate _execute;
        /// <exclude/>
        protected CanExecuteDelegate _canExecute;

        /// <summary>
        /// <ja>
        /// �����Ȃ��̃R���X�g���N�^�ł��B
        /// </ja>
        /// <en>
        /// Constructor that doesn't have argument
        /// </en>
        /// </summary>
        /// <remarks>
        /// <ja>
        /// ���̃R���X�g���N�^�ō쐬���ꂽ�R�}���h�́A�R�}���h�����s����Ă������Ȃ鏈�������܂���B
        /// </ja>
        /// <en>
        /// The command made by this constructor is executed the command or doesn't do the becoming it processing either. 
        /// </en>
        /// </remarks>
        /// <overloads>
        /// <summary>
        /// <ja>
        /// �R�}���h�I�u�W�F�N�g���쐬���܂��B
        /// </ja>
        /// <en>
        /// Making the command object. 
        /// </en>
        /// </summary>
        /// </overloads>
        public PoderosaCommandImpl() {
            _execute = null;
            _canExecute = null;
        }
        /// <summary>
        /// <ja>
        /// �R�}���h�����s�����ۂɌĂяo���f���Q�[�g���w�肵���R���X�g���N�^�ł��B
        /// </ja>
        /// <en>
        /// Constructor who specified delegate called when command is executed
        /// </en>
        /// </summary>
        /// <param name="execute">
        /// <ja>
        /// �R�}���h�����s�����ۂɌĂяo�����f���Q�[�g�ł��B
        /// </ja>
        /// <en>
        /// Delegate called when command is executed
        /// </en>
        /// </param>
        /// <remarks>
        /// <ja>
        /// <para>
        /// �R�}���h�����s�����ہ\�\�����������<seealso cref="IPoderosaCommand">IPoderosaCommand</seealso>��
        /// <see cref="IPoderosaCommand.InternalExecute">InternalExecute</see>���\�b�h���Ăяo�����Ƃ��ɁA<paramref name="execute">execute</paramref>
        /// �Ɏw�肵���f���Q�[�g���Ăяo����܂��B
        /// </para>
        /// <para>
        /// <seealso cref="IPoderosaCommand">IPoderosaCommand</seealso>��<see cref="IPoderosaCommand.CanExecute">CanExecute</see>���\�b�h�̏����ł́A���true���Ԃ����悤�Ɏ�������܂��B
        /// </para>
        /// </ja>
        /// <en>
        /// <para>
        /// When the <see cref="IPoderosaCommand.InternalExecute">InternalExecute</see> method of <seealso cref="IPoderosaCommand">IPoderosaCommand</seealso> is called, specified delegate is called in execute when paraphrasing it when the command is executed. 
        /// </para>
        /// <para>
        /// In the processing of the <see cref="IPoderosaCommand.CanExecute">CanExecute</see> method of <seealso cref="IPoderosaCommand">IPoderosaCommand</seealso>, true is always implemented so that it is returned. 
        /// </para>
        /// </en>
        /// </remarks>
        public PoderosaCommandImpl(ExecuteDelegate execute) {
            _execute = execute;
            _canExecute = null;
        }
        /// <summary>
        /// <ja>
        /// �R�}���h�����s�����ۂɌĂяo���f���Q�[�g�ƁA���j���[��c�[���o�[���I���\���ǂ����������f���Q�[�g���w�肵���R���X�g���N�^�ł��B
        /// </ja>
        /// <en>
        /// Constructor that specified delegate that shows whether delegate, menu, and toolbar called when command is executed can be selected
        /// </en>
        /// </summary>
        /// <param name="execute">
        /// <ja>
        /// �R�}���h�����s�����ۂɌĂяo�����f���Q�[�g�ł��B
        /// </ja>
        /// <en>
        /// Delegate called when command is executed.
        /// </en>
        /// </param>
        /// <param name="canExecute">
        /// <ja>���j���[��c�[���o�[���C�l�[�u���ɂ��邩�f�B�X�G�u���ɂ��邩�����߂�Ƃ��ɌĂяo�����f���Q�[�g�ł��B</ja>
        /// <en>Delegate called when whether menu and toolbar are made Inabl or making to disable is decided.</en>
        /// </param>
        /// <remarks>
        /// <ja>
        /// <para>
        /// �R�}���h�����s�����ہ\�\�����������<seealso cref="IPoderosaCommand">IPoderosaCommand</seealso>��
        /// <see cref="IPoderosaCommand.InternalExecute">InternalExecute</see>���\�b�h���Ăяo�����Ƃ��ɁA<paramref name="execute">execute</paramref>
        /// �Ɏw�肵���f���Q�[�g���Ăяo����܂��B
        /// </para>
        /// <para>
        /// ���j���[��c�[���o�[���C�l�[�u���ɂ��邩�f�B�X�G�u���ɂ��邩�����߂�Ƃ��\�\
        /// �����������<seealso cref="IPoderosaCommand">IPoderosaCommand</seealso>��<see cref="IPoderosaCommand.CanExecute">CanExecute</see>���\�b�h���Ăяo�����Ƃ��ɁA
        /// <paramref name="canExecute">canExecute</paramref>�Ɏw�肵���f���Q�[�g���Ăяo����܂��B
        /// </para>
        /// </ja>
        /// <en>
        /// <para>
        /// When the <see cref="IPoderosaCommand.InternalExecute">InternalExecute</see> method of <seealso cref="IPoderosaCommand">IPoderosaCommand</seealso> is called, specified delegate is called in execute when paraphrasing it when the command is executed. 
        /// </para>
        /// <para>
        /// In the processing of the <see cref="IPoderosaCommand.CanExecute">CanExecute</see> method of <seealso cref="IPoderosaCommand">IPoderosaCommand</seealso>, true is always implemented so that it is returned. 
        /// </para>
        /// </en>
        /// </remarks>
        public PoderosaCommandImpl(ExecuteDelegate execute, CanExecuteDelegate canExecute) {
            _execute = execute;
            _canExecute = canExecute;
        }

        /// <summary>
        /// <ja>
        /// �R���X�g���N�^�Ŏw�肳�ꂽ�f���Q�[�g�����s���邽�߂̃I�[�o�[���C�h����Ă��܂��B
        /// </ja>
        /// <en>
        /// The override to execute delegate specified by the constructor is done. 
        /// </en>
        /// </summary>
        /// <param name="target"><ja>�R�}���h�̃^�[�Q�b�g�ł��B</ja>
        /// <en>
        /// Target of command.
        /// </en>
        /// </param>
        /// <param name="args"><ja>�R�}���h�̈����ł��B</ja>
        /// <en>Argument of commane.</en></param>
        /// <returns><ja>�R���X�g���N�^�Ŏw�肳�ꂽ�f���Q�[�g�����s�������ʂ��߂���܂��B</ja>
        /// <en>The result of executing delegate specified by the constructor is returned. </en></returns>
        /// <remarks>
        /// <ja>
        /// �����Ȃ��̃R���X�g���N�^�ł��̃I�u�W�F�N�g�����ꂽ�ꍇ�A�������s����邱�Ƃ͂Ȃ��A�߂�l�Ƃ���CommandResult.Ignored���Ԃ���܂��B
        /// </ja>
        /// <en>
        /// Nothing is executed when this object is made from the constructor who doesn't have the argument, and CommandResult.Ignored is returned as a return value. 
        /// </en>
        /// </remarks>
        public virtual CommandResult InternalExecute(ICommandTarget target, params IAdaptable[] args) {
            return _execute == null ? CommandResult.Ignored : _execute(target);
        }

        /// <summary>
        /// <ja>
        /// �R���X�g���N�^�Ŏw�肳�ꂽ�f���Q�[�g�����s���邽�߂ɃI�[�o�[���C�h����Ă��܂��B
        /// </ja>
        /// <en>
        /// To execute delegate specified by the constructor, override is done. 
        /// </en>
        /// </summary>
        /// <param name="target"><ja>�R�}���h�̃^�[�Q�b�g�ł��B</ja>
        /// <en>
        /// Target of command.
        /// </en>
        /// </param>
        /// <returns><ja>�R���X�g���N�^�Ŏw�肳�ꂽ�f���Q�[�g�����s�������ʂ��߂���܂��B</ja>
        /// <en>The result of executing delegate specified by the constructor is returned. </en></returns>
        /// <remarks>
        /// <ja>
        /// �����Ȃ��A�܂��́A������1�̃R���X�g���N�^�ł��̃I�u�W�F�N�g�����ꂽ�ꍇ�A���true���Ԃ���܂��B
        /// </ja>
        /// <en>
        /// The argument none or the argument is returned and when this object is made from one constructor, true is always returned. 
        /// </en>
        /// </remarks>
        public virtual bool CanExecute(ICommandTarget target) {
            return _canExecute == null ? true : _canExecute(target);
        }

        public virtual IAdaptable GetAdapter(Type adapter) {
            return CommandManagerPlugin.Instance.PoderosaWorld.AdapterManager.GetAdapter(this, adapter);
        }
    }

    //IGeneralCommand�W������
    /// <summary>
    /// <ja>
    /// <seealso cref="IGeneralCommand">IGeneralCommand</seealso>�����������N���X�ł��B�V���[�g�J�b�g�L�[�����蓖�Ă�R�}���h���쐬����ۂɎg���܂��B
    /// </ja>
    /// <en>
    /// It is a class that implements <seealso cref="IGeneralCommand">IGeneralCommand</seealso>. When the command that allocates the shortcut key is made, it uses it. 
    /// </en>
    /// </summary>
    /// <remarks>
    /// <ja>
    /// <para>
    /// �V���[�g�J�b�g�L�[�����蓖�Ă�R�}���h����������J���҂́A���̃N���X���g�����Ƃ�<seealso cref="IGeneralCommand">IGeneralCommand</seealso>����������
    /// �I�u�W�F�N�g��e�Ղɍ쐬�ł��܂��B
    /// </para>
    /// <para>
    /// �V���[�g�J�b�g�L�[�����蓖�Ă�K�v���Ȃ��ꍇ�ɂ́A<seealso cref="PoderosaCommandImpl">PoderosaCommandImpl</seealso>���g���Ă��������B
    /// </para>
    /// </ja>
    /// <en>
    /// <para>
    /// The developer who implements the command that allocates the shortcut key can easily make the object where <seealso cref="IGeneralCommand">IGeneralCommand</seealso> is implemented by using this class. 
    /// </para>
    /// <para>
    /// Please use <seealso cref="PoderosaCommandImpl">PoderosaCommandImpl</seealso> when you need not allocate the shortcut key. 
    /// </para>
    /// </en>
    /// </remarks>
    /// <example>
    /// <ja>
    /// [�ҏW�n�Ƃ�����`�ς݃J�e�S���ɑ�����R�}���h�I�u�W�F�N�g���쐬����ɂ́A���̂悤�ɂ��܂��B
    /// <code>
    /// // �R�}���h�}�l�[�W���̎擾
    /// ICoreServices cs = (ICoreServices)PoderosaWorld.GetAdapter(typeof(ICoreServices));
    /// ICommandManager cm = cs.CommandManager;
    /// 
    /// // �R�}���h�쐬
    /// IGeneralCommand mycmd = new GeneralCommandImpl(
    ///   "co.example.myplugin.mycommand",
    ///  "MyCommand", cm.CommandCategories.Edit,
    ///   delegate(ICommandTarget target)
    ///  {
    ///     // �����ɃR�}���h�����s�����Ƃ��̏������L�q���܂�
    ///   return CommandResult.Succeeded; // �����Ȃ�Succeeded��Ԃ�
    ///  },
    ///  delegate(ICommandTarget target)
    ///  {
    ///     // �����ɃR�}���h�̉ۂ����ׂ���Ƃ��̏������L�q���܂�
    ///     return true; // ���s�\�Ȃ�true��Ԃ�
    ///  }
    /// );
    /// </code>
    /// 
    /// </ja>
    /// <en>
    /// To make the command object that belongs to the definition ending category of "Edit", as follows is done. 
    /// <code>
    /// // Get command manager.
    /// ICoreServices cs = (ICoreServices)PoderosaWorld.GetAdapter(typeof(ICoreServices));
    /// ICommandManager cm = cs.CommandManager;
    /// 
    /// // Make the command.
    /// IGeneralCommand mycmd = new GeneralCommandImpl(
    ///   "co.example.myplugin.mycommand",
    ///  "MyCommand", cm.CommandCategories.Edit,
    ///   delegate(ICommandTarget target)
    ///  {
    ///     // The processing when the command is executed here is described. 
    ///   return CommandResult.Succeeded; // If it is a success, Succeeded is returned. 
    ///  },
    ///  delegate(ICommandTarget target)
    ///  {
    ///     // The processing when right or wrong of the command is examined here is described. 
    ///     return true; // If it is executable, true is returned. 
    ///  }
    /// );
    /// </code>
    /// 
    /// </en>
    /// </example>
    public class GeneralCommandImpl : IGeneralCommand {
        /// <exclude/>
        protected string _commandID;
        /// <exclude/>
        protected string _descriptionTextID;
        /// <exclude/>
        protected StringResource _strResource;
        /// <exclude/>
        protected bool _usingStringResource;
        /// <exclude/>
        protected Keys _defaultShortcutKey;
        /// <exclude/>
        protected ICommandCategory _commandCategory;
        /// <exclude/>
        protected CanExecuteDelegate _canExecuteDelegate;
        /// <exclude/>
        protected ExecuteDelegate _executeDelegate;

        //�K�{�v�f��^����R���X�g���N�^
        /// <summary>
        /// <ja>
        /// �R�}���hID�A�J���`���A�����e�L�X�gID�A�R�}���h�J�e�S���A�R�}���h�����s�����ۂɌĂяo�����f���Q�[�g�A���s�\���ǂ����𒲂ׂ�ۂɌĂяo�����f���Q�[�g���w�肵�ăI�u�W�F�N�g���쐬���܂��B
        /// </ja>
        /// <en>
        /// The object is made specifying delegate called when delegate called when command ID, Culture, explanation text ID, the command category, and the command are executed and it is executable is examined. 
        /// </en>
        /// </summary>
        /// <param name="commandID">
        /// <ja>
        /// ���蓖�Ă�R�}���hID�ł��B�ق��̃R�}���h�Ƃ͏d�����Ȃ��B�ꖳ��̂��̂��w�肵�Ȃ���΂Ȃ�܂���B
        /// </ja>
        /// <en>
        /// It is allocated command ID. The unique one that doesn't overlap should be specified other commands. 
        /// </en>
        /// </param>
        /// <param name="sr">
        /// <ja>
        /// �J���`�����ł��B
        /// </ja>
        /// <en>
        /// Information of the culture.
        /// </en>
        /// </param>
        /// <param name="descriptionTextID">
        /// <ja>
        /// �R�}���h�̐������������e�L�X�gID�ł��B
        /// </ja>
        /// <en>
        /// Text ID that shows explanation of command
        /// </en>
        /// </param>
        /// <param name="commandCategory">
        /// <ja>
        /// �R�}���h�̃J�e�S���ł��B
        /// </ja>
        /// <en>
        /// Category of command.
        /// </en>
        /// </param>
        /// <param name="exec">
        /// <ja>
        /// �R�}���h�����s�����Ƃ��ɌĂяo�����f���Q�[�g�ł��B
        /// </ja>
        /// <en>
        /// Delegate called when command is executed.
        /// </en>
        /// </param>
        /// <param name="canExecute">
        /// <ja>
        /// �R�}���h�����s�\���ǂ����𒲂ׂ�ۂɌĂяo�����f���Q�[�g�ł��B
        /// </ja>
        /// <en>
        /// Delegate called when it is examined whether command is executable
        /// </en>
        /// </param>
        /// <overloads>
        /// <summary>
        /// <ja>
        /// �R�}���h�I�u�W�F�N�g���쐬���܂��B
        /// </ja>
        /// <en>
        /// Create the command object.
        /// </en>
        /// </summary>
        /// </overloads>
        public GeneralCommandImpl(string commandID, StringResource sr, string descriptionTextID, ICommandCategory commandCategory, ExecuteDelegate exec, CanExecuteDelegate canExecute) {
            _commandID = commandID;
            _usingStringResource = sr != null;
            _strResource = sr;
            _descriptionTextID = descriptionTextID;
            _commandCategory = commandCategory;
            _executeDelegate = exec;
            _canExecuteDelegate = canExecute;
        }
        //�ꕔ�v�f���ȗ�����R���X�g���N�^�Q
        /// <summary>
        /// <ja>
        /// �R�}���hID�A�J���`���A�����e�L�X�g���A�R�}���h�J�e�S���A�R�}���h�����s�����ۂɌĂяo�����f���Q�[�g�A���s�\���ǂ����𒲂ׂ�ۂɌĂяo�����f���Q�[�g���w�肵�ăI�u�W�F�N�g���쐬���܂��B
        /// </ja>
        /// <en>
        /// The object is made specifying delegate called when delegate called when command ID, Culture, explanation text ID, the command category, and the command are executed and it is executable is examined. 
        /// </en>
        /// </summary>
        /// <param name="commandID">
        /// <ja>
        /// ���蓖�Ă�R�}���hID�ł��B�ق��̃R�}���h�Ƃ͏d�����Ȃ��B�ꖳ��̂��̂��w�肵�Ȃ���΂Ȃ�܂���B
        /// </ja>
        /// <en>
        /// It is allocated command ID. The unique one that doesn't overlap should be specified other commands. 
        /// </en>
        /// </param>
        /// <param name="description">
        /// <ja>
        /// �R�}���h�̐������������e�L�X�g�ł��B
        /// </ja>
        /// <en>
        /// Text that shows explanation of command
        /// </en>
        /// </param>
        /// <param name="category">
        /// <ja>
        /// �R�}���h�̃J�e�S���ł��B
        /// </ja>
        /// <en>
        /// Category of command.
        /// </en>
        /// </param>
        /// <param name="execute">
        /// <ja>
        /// �R�}���h�����s�����Ƃ��ɌĂяo�����f���Q�[�g�ł��B
        /// </ja>
        /// <en>
        /// Delegate called when command is executed.
        /// </en>
        /// </param>
        /// <param name="canExecute">
        /// <ja>
        /// �R�}���h�����s�\���ǂ����𒲂ׂ�ۂɌĂяo�����f���Q�[�g�ł��B
        /// </ja>
        /// <en>
        /// Dalagate called when it is examined whether the command is executable. 
        /// </en>
        /// </param>
        public GeneralCommandImpl(string commandID, string description, ICommandCategory category, ExecuteDelegate execute, CanExecuteDelegate canExecute)
            : this(commandID, null, description, category, execute, canExecute) {
        }
        /// <summary>
        /// <ja>
        /// �R�}���hID�A�J���`���A�����e�L�X�gID�A�R�}���h�J�e�S���A�R�}���h�����s�����ۂɌĂяo�����f���Q�[�g���w�肵�ăI�u�W�F�N�g���쐬���܂��B
        /// </ja>
        /// <en>
        /// The object is made specifying delegate called when command ID, culture, explanation text ID, the command category, and the command are executed. 
        /// </en>
        /// </summary>
        /// <param name="commandID">
        /// <ja>
        /// ���蓖�Ă�R�}���hID�ł��B�ق��̃R�}���h�Ƃ͏d�����Ȃ��B�ꖳ��̂��̂��w�肵�Ȃ���΂Ȃ�܂���B
        /// </ja>
        /// <en>
        /// It is allocated command ID. The unique one that doesn't overlap should be specified other commands. 
        /// </en>
        /// </param>
        /// <param name="sr">
        /// <ja>
        /// �J���`�����ł��B
        /// </ja>
        /// <en>
        /// Information of the culture.
        /// </en>
        /// </param>
        /// <param name="descriptionTextID">
        /// <ja>
        /// �R�}���h�̐������������e�L�X�gID�ł��B
        /// </ja>
        /// <en>
        /// Text ID that shows explanation of command.
        /// </en>
        /// </param>
        /// <param name="category">
        /// <ja>
        /// �R�}���h�̃J�e�S���ł��B
        /// </ja>
        /// <en>
        /// Category of command.
        /// </en></param>
        /// <param name="execute">
        /// <ja>
        /// �R�}���h�����s�����Ƃ��ɌĂяo�����f���Q�[�g�ł��B
        /// </ja>
        /// <en>
        /// Delegate called when command is executed
        /// </en>
        /// </param>
        public GeneralCommandImpl(string commandID, StringResource sr, string descriptionTextID, ICommandCategory category, ExecuteDelegate execute)
            : this(commandID, sr, descriptionTextID, category, execute, null) {
        }
        /// <summary>
        /// <ja>
        /// �R�}���hID�A�J���`���A�����e�L�X�g���A�R�}���h�J�e�S���A�R�}���h�����s�����ۂɌĂяo�����f���Q�[�g���w�肵�ăI�u�W�F�N�g���쐬���܂��B
        /// </ja>
        /// <en>
        /// The object is made specifying delegate called when command ID, culture, explanation text ID, the command category, and the command are executed. 
        /// </en>
        /// </summary>
        /// <param name="commandID">
        /// <ja>
        /// ���蓖�Ă�R�}���hID�ł��B�ق��̃R�}���h�Ƃ͏d�����Ȃ��B�ꖳ��̂��̂��w�肵�Ȃ���΂Ȃ�܂���B
        /// </ja>
        /// <en>
        /// It is allocated command ID. The unique one that doesn't overlap should be specified other commands. 
        /// </en>
        /// </param>
        /// <param name="description">
        /// <ja>
        /// �R�}���h�̐������������e�L�X�g�ł��B
        /// </ja>
        /// <en>
        /// Text that shows explanation of command.
        /// </en>
        /// </param>
        /// <param name="category">
        /// <ja>
        /// �R�}���h�̃J�e�S���ł��B
        /// </ja>
        /// <en>
        /// Category of command.
        /// </en>
        /// </param>
        /// <param name="execute">
        /// <ja>
        /// �R�}���h�����s�����Ƃ��ɌĂяo�����f���Q�[�g�ł��B
        /// </ja>
        /// <en>
        /// Delegate called when command is executed.
        /// </en>
        /// </param>
        public GeneralCommandImpl(string commandID, string description, ICommandCategory category, ExecuteDelegate execute)
            : this(commandID, null, description, category, execute, null) {
        }

        /// <summary>
        /// <ja>
        /// �R�}���hID�A�J���`���A�����e�L�X�g���A�R�}���h�J�e�S�����w�肵�ăI�u�W�F�N�g���쐬���܂��B
        /// </ja>
        /// <en>
        /// The object is made specifying command ID, culture, the explanation text sentence, and the command category. 
        /// </en>
        /// </summary>
        /// <param name="commandID">
        /// <ja>
        /// ���蓖�Ă�R�}���hID�ł��B�ق��̃R�}���h�Ƃ͏d�����Ȃ��B�ꖳ��̂��̂��w�肵�Ȃ���΂Ȃ�܂���B
        /// </ja>
        /// <en>
        /// It is allocated command ID. The unique one that doesn't overlap should be specified other commands. 
        /// </en>
        /// </param>
        /// <param name="sr">
        /// <ja>
        /// �J���`�����ł��B
        /// </ja>
        /// <en>
        /// Information of the culture.
        /// </en>
        /// </param>
        /// <param name="descriptionTextID">
        /// <ja>
        /// �R�}���h�̐������������e�L�X�gID�ł��B
        /// </ja>
        /// <en>
        /// Text ID that shows explanation of command.
        /// </en>
        /// </param>
        /// <param name="category">
        /// <ja>
        /// �R�}���h�̃J�e�S���ł��B
        /// </ja>
        /// <en>
        /// Category of command.
        /// </en>
        /// </param>
        public GeneralCommandImpl(string commandID, StringResource sr, string descriptionTextID, ICommandCategory category)
            : this(commandID, sr, descriptionTextID, category, null, null) {
        }
        /// <summary>
        /// <ja>
        /// �R�}���hID�A�����e�L�X�g���A�R�}���h�J�e�S�����w�肵�ăI�u�W�F�N�g���쐬���܂��B
        /// </ja>
        /// <en>
        /// Create a object specifying command ID, the explanation text sentence, and the command category. 
        /// </en>
        /// </summary>
        /// <param name="commandID">
        /// <ja>
        /// ���蓖�Ă�R�}���hID�ł��B�ق��̃R�}���h�Ƃ͏d�����Ȃ��B�ꖳ��̂��̂��w�肵�Ȃ���΂Ȃ�܂���B
        /// </ja>
        /// <en>
        /// It is allocated command ID. The unique one that doesn't overlap should be specified other commands. 
        /// </en>
        /// </param>
        /// <param name="description">
        /// <ja>
        /// �R�}���h�̐������������e�L�X�g�ł��B
        /// </ja>
        /// <en>
        /// Text that shows explanation of command
        /// </en>
        /// </param>
        /// <param name="category">
        /// <ja>
        /// �R�}���h�̃J�e�S���ł��B
        /// </ja>
        /// <en>
        /// Category of command.
        /// </en>
        /// </param>
        public GeneralCommandImpl(string commandID, string description, ICommandCategory category)
            : this(commandID, null, description, category, null, null) {
        }

        /// <summary>
        /// <ja>
        /// ���̃R�}���h�Ɋ��蓖�Ă��Ă���R�}���hID�ł��B
        /// </ja>
        /// <en>
        /// Command ID allocated in this command.
        /// </en>
        /// </summary>
        public virtual string CommandID {
            get {
                return _commandID;
            }
        }

        /// <summary>
        /// <ja>
        /// ���̃R�}���h�ɐݒ肳��Ă���������ł��B
        /// </ja>
        /// <en>
        /// Explanation set to this command.
        /// </en>
        /// </summary>
        public virtual string Description {
            get {
                return _usingStringResource ? _strResource.GetString(_descriptionTextID) : _descriptionTextID;
            }
        }

        /// <summary>
        /// <ja>
        /// �f�t�H���g�̃V���[�g�J�b�g�L�[�������܂��B
        /// </ja>
        /// <en>
        /// The shortcut key of default is shown. 
        /// </en>
        /// </summary>
        /// <remarks>
        /// <ja>�f�t�H���g�̃V���[�g�J�b�g�L�[�����݂��Ȃ��ꍇ�ɂ́AKeys.None���Ԃ���܂��B</ja>
        /// <en>When the shortcut key of default doesn't exist, Keys.None is returned. </en>
        /// </remarks>
        public virtual Keys DefaultShortcutKey {
            get {
                return _defaultShortcutKey;
            }
        }

        /// <summary>
        /// <ja>
        /// �R�}���h�J�e�S���������܂��B
        /// </ja>
        /// <en>
        /// The command category is shown. 
        /// </en>
        /// </summary>
        public virtual ICommandCategory CommandCategory {
            get {
                return _commandCategory;
            }
        }

        //Args���K�v�Ȃ�͓Ǝ��ɔh������
        /// <summary>
        /// <ja>
        /// �I�[�o�[���[�h�ł��B�R�}���h�����s����Ƃ��ɌĂяo���悤�ɐݒ肳�ꂽ�f���Q�[�g������ŌĂяo���܂��B
        /// </ja>
        /// <en>
        /// It is an overload. Delegate set for the command to be executed or to call it is called internally. 
        /// </en>
        /// </summary>
        /// <param name="target"><ja>�����Ώۂ������^�[�Q�b�g�ł��B</ja><en>Target that shows processing object</en></param>
        /// <param name="args"><ja>�R�}���h�Ɉ����n�����C�ӂ̈����ł��B</ja><en>Arbitrary argument handed over to command</en></param>
        /// <returns><ja>���s���ꂽ�f���Q�[�g�̖߂�l�ł��B</ja><en>Return value of executed delegate</en></returns>
        public virtual CommandResult InternalExecute(ICommandTarget target, params IAdaptable[] args) {
            return _executeDelegate == null ? CommandResult.Ignored : _executeDelegate(target);
        }

        /// <summary>
        /// <ja>
        /// �I�[�o�[���[�h�ł��B�R�}���h�����s�\���ǂ����̊m�F�Ăяo���̍ۂɁA�ݒ肳�ꂽ�f���Q�[�g������ŌĂяo���܂��B
        /// </ja>
        /// <en>
        /// It is an overload. When it is called whether the command is executable to confirm it, set delegate is called internally. 
        /// </en>
        /// </summary>
        /// <param name="target"><ja>�����Ώۂ������^�[�Q�b�g�ł��B</ja><en>Target that shows processing object.</en></param>
        /// <returns><ja>���s���ꂽ�f���Q�[�g�̖߂�l�ł��B</ja><en>Return value of executed delegate.</en></returns>
        public virtual bool CanExecute(ICommandTarget target) {
            return _canExecuteDelegate == null ? true : _canExecuteDelegate(target);
        }

        public virtual IAdaptable GetAdapter(Type adapter) {
            return CommandManagerPlugin.Instance.PoderosaWorld.AdapterManager.GetAdapter(this, adapter);
        }

        /// <summary>
        /// <ja>�L�[�o�C���h�̃f�t�H���g�ݒ��ύX���܂��B</ja>
        /// <en>The default setting of key bind is changed. </en>
        /// </summary>
        /// <param name="key"><ja>���蓖�Ă����L�[</ja><en>Key that wants to be allocated</en></param>
        /// <returns><ja>���̃I�u�W�F�N�g���g��Ԃ��܂��B</ja><en>This object is returned. </en></returns>
        public GeneralCommandImpl SetDefaultShortcutKey(Keys key) {
            _defaultShortcutKey = key;
            return this;
        }
    }

    //IPoderosaMenuGroup�W������
    /// <summary>
    /// <ja>
    /// <seealso cref="IPoderosaMenuGroup">IPoderosaMenuGroup</seealso>�����������N���X�ł��B���j���[�O���[�v���쐬����ۂɎg���܂��B
    /// </ja>
    /// <en>
    /// It is a class that implements <seealso cref="IPoderosaMenuGroup">IPoderosaMenuGroup</seealso>. When the menu group is made, it uses it. 
    /// </en>
    /// </summary>
    /// <remarks>
    /// <ja>
    /// <para>
    /// ���j���[����������J���҂́A���̃N���X���g�����ƂŁA<seealso cref="IPoderosaMenuGroup">IPoderosaMenuGroup</seealso>
    /// �����������I�u�W�F�N�g��e�Ղɍ쐬�ł��܂��B���̃N���X�́A<seealso cref="IPositionDesignation">IPositionDesignation</seealso>
    /// ���������Ă���A���j���[�̏��������߂邱�Ƃ��ł��܂��B
    /// </para>
    /// <para>
    /// �쐬�������j���[�O���[�v�́A�g���|�C���g�ւƓo�^���܂��B
    /// </para>
    /// <note type="implementnotes">
    /// ��x���j���[�O���[�v���쐬���ꂽ�Ȃ�A����𑝌�����@�\�̓T�|�[�g����Ă��܂���B�܂����j���[���ڂ͍쐬���Ɍ��܂�A���I�ɕω����邱�Ƃ͂���܂���B
    /// </note>
    /// </ja>
    /// <en>
    /// <para>
    /// The developer who implements the menu can easily make the object that implements 
    /// <seealso cref="IPoderosaMenuGroup">IPoderosaMenuGroup</seealso> by using this class. This class implements 
    /// <seealso cref="IPositionDesignation">IPositionDesignation</seealso>, and can decide the order of the menu. 
    /// </para>
    /// <para>
    /// The menu group that makes it registers to the extension point. 
    /// </para>
    /// <note type="implementnotes">
    /// If the menu group was made once, the function to increase and decrease it is not supported. Moreover, the menu item is decided when making it, and never changes dynamically. 
    /// </note>
    /// </en>
    /// </remarks>
    /// <example>
    /// <ja>
    /// <seealso cref="IPoderosaMenuGroup">IPoderosaMenuGroup</seealso>�����������I�u�W�F�N�g�́A���̂悤�ɂ��č쐬�ł��܂��B
    /// <code>
    /// // ���炩���߃��j���[�����s���ꂽ�Ƃ��̃��j���[�ƃ��j���[���ڂ��쐬���Ă����܂��B
    /// 
    /// // �R�}���h
    /// PoderosaCommandImpl mycommand = new PoderosaCommandImpl(
    ///   delegate(ICommandTarget target)
    ///   {
    ///     // �R�}���h�����s���ꂽ�Ƃ��̏���
    ///    MessageBox.Show("���s����܂���");
    ///    return CommandResult.Succeeded;
    ///   },
    ///   delegate(ICommandTarget target)
    ///   {
    ///     // �R�}���h�����s�ł��邩�ǂ�����Ԃ�
    ///    return true;
    ///  }
    /// );
    /// 
    /// // ���j���[����
    /// PoderosaMenuItemImpl menuitem = new PoderosaMenuItemImpl(
    ///     mycommand, "My Menu Name");
    ///
    /// // ���j���[�O���[�v
    /// PoderosaMenuGroupImpl menugroup = new PoderosaMenuGroupImpl(menuitem);
    /// 
    /// // ���̃��j���[�O���[�v���A���Ƃ��΁m�ҏW�n���j���[�iorg.poderosa.menu.edit�j�ɓo�^
    /// // �g���|�C���g������
    /// IExtensionPoint editmenu = 
    ///     PoderosaWorld.PluginManager.FindExtensionPoint("org.poderosa.menu.edit");
    /// // �g���|�C���g�Ƀ��j���[�O���[�v��o�^
    /// editmenu.RegisterExtension(menugroup);
    /// </code>
    /// </ja>
    /// <en>
    /// The object that implements <seealso cref="IPoderosaMenuGroup">IPoderosaMenuGroup</seealso> can be made as follows. 
    /// <code>
    /// // The menu and the menu item when the menu is executed are made beforehand. 
    /// 
    /// // Command
    /// PoderosaCommandImpl mycommand = new PoderosaCommandImpl(
    ///   delegate(ICommandTarget target)
    ///   {
    ///     // Processing when command is executed
    ///    MessageBox.Show("Executed");
    ///    return CommandResult.Succeeded;
    ///   },
    ///   delegate(ICommandTarget target)
    ///   {
    ///     // It is returned whether the command can be executed. 
    ///    return true;
    ///  }
    /// );
    /// 
    /// // Menu item.
    /// PoderosaMenuItemImpl menuitem = new PoderosaMenuItemImpl(
    ///     mycommand, "My Menu Name");
    ///
    /// // Menu group.
    /// PoderosaMenuGroupImpl menugroup = new PoderosaMenuGroupImpl(menuitem);
    /// 
    /// // For instance, this menu group is registered in "Edit" menu (org.poderosa.menu.edit). 
    /// // Retrieval of The enhancing point.
    /// IExtensionPoint editmenu = 
    ///     PoderosaWorld.PluginManager.FindExtensionPoint("org.poderosa.menu.edit");
    /// // The menu group is registered in the extension point. 
    /// editmenu.RegisterExtension(menugroup);
    /// </code>
    /// </en>
    /// </example>
    public class PoderosaMenuGroupImpl : IPoderosaMenuGroup, IPositionDesignation {
        /// <exclude/>
        protected IPoderosaMenu[] _childMenus;
        /// <exclude/>
        protected bool _isVolatile;
        /// <exclude/>
        protected bool _showSeparator;
        /// <exclude/>
        protected IAdaptable _designationTarget;
        /// <exclude/>
        protected PositionType _positionType;

        /// <summary>
        /// <ja>
        /// �܂܂�郁�j���[���ڂ��ЂƂ��Ȃ����j���[�O���[�v���쐬���܂��B
        /// </ja>
        /// <en>
        /// The included menu item makes the menu group that is not no. 
        /// </en>
        /// </summary>
        /// <remarks>
        /// <ja>
        /// ���j���[���ڂ͂ЂƂ�����܂��񂪁A��؂�L���i�Z�p���[�^�j�͕\������܂��B
        /// </ja>
        /// <en>
        /// The separator is displayed though nothing is in the menu item. 
        /// </en>
        /// </remarks>
        /// <overloads>
        /// <summary>
        /// <ja>
        /// ���j���[�O���[�v���쐬���܂��B
        /// </ja>
        /// <en>
        /// Create the menu group.
        /// </en>
        /// </summary>
        /// </overloads>
        public PoderosaMenuGroupImpl()
            : this(null, true) {
        }

        /// <summary>
        /// <ja>
        /// �܂܂�郁�j���[���ڂ��ЂƂ����w�肵�����j���[�O���[�v���쐬���܂��B
        /// </ja>
        /// <en>
        /// The menu group that specifies only one included menu item is made. 
        /// </en>
        /// </summary>
        /// <remarks>
        /// <ja>
        /// ��؂�L���i�Z�p���[�^�j�͕\������܂��B
        /// </ja>
        /// <en>
        /// The separator is displayed. 
        /// </en>
        /// </remarks>
        /// <param name="child"><ja>�܂߂������j���[���ڂł��B</ja><en>Menu item that wants to be included</en></param>
        public PoderosaMenuGroupImpl(IPoderosaMenu child)
            : this(new IPoderosaMenu[] { child }, true) {
        }

        /// <summary>
        /// <ja>
        /// �܂܂�郁�j���[���ڂ𕡐��w�肵�����j���[�O���[�v���쐬���܂��B
        /// </ja>
        /// <en>
        /// The menu group that specifies two or more included menu items is made. 
        /// </en>
        /// </summary>
        /// <param name="childMenus"><ja>�܂߂������j���[���ڂ̔z��ł��B</ja><en>Array of menu item that wants to be included</en></param>
        /// <remarks>
        /// <ja>
        /// ��؂�L���i�Z�p���[�^�j�͕\������܂��B
        /// </ja>
        /// <en>
        /// The separator is displayed. 
        /// </en>
        /// </remarks>
        public PoderosaMenuGroupImpl(IPoderosaMenu[] childMenus)
            :
            this(childMenus, true) {
        }
        /// <summary>
        /// <ja>
        /// �܂܂�镡���̃��j���[���ڂƃ��j���[���ڂ̒��O�ɋ�؂�L���i�Z�p���[�^�j��\�����邩�ۂ�
        /// ���w�肵�ă��j���[�O���[�v���쐬���܂��B
        /// </ja>
        /// <en>
        /// The menu group is made specifying whether to display the separator just before two or more included menu item and menu item. 
        /// </en>
        /// </summary>
        /// <param name="childMenus"><ja>�܂߂������j���[���ڂ̔z��ł��B</ja><en>Array of menu item that wants to be included</en></param>
        /// <param name="showSeparator"><ja>�Z�p���[�^��\�����邩�ۂ��̎w��ł��Btrue�̂Ƃ��\���Afalse�̂Ƃ���\���ł��B</ja>
        /// <en>It is specification whether to display the separator. It displays at true, and non-display at false. </en></param>
        public PoderosaMenuGroupImpl(IPoderosaMenu[] childMenus, bool showSeparator) {
            _childMenus = childMenus;
            _isVolatile = false;
            _showSeparator = showSeparator;
            _designationTarget = null;
            _positionType = PositionType.First;
        }

        /// <summary>
        /// <ja>
        /// �܂܂�郁�j���[���ڂ̔z��ł��B
        /// </ja>
        /// <en>
        /// Array of included menu item
        /// </en>
        /// </summary>
        public virtual IPoderosaMenu[] ChildMenus {
            get {
                return _childMenus;
            }
        }

        /// <summary>
        /// <ja>
        /// ���j���[�����I�ɍ쐬����邩�ǂ����������v���p�e�B�ł��B
        /// </ja>
        /// <en>
        /// Property that shows whether menu is dynamically made.
        /// </en>
        /// </summary>
        /// <remarks>
        /// <ja>���false�i���j���[�𓮓I�ɍ쐬���Ȃ��j���Ԃ���܂��B</ja>
        /// <en>False (The menu is not dynamically made) is always returned. </en>
        /// </remarks>
        public virtual bool IsVolatileContent {
            get {
                return _isVolatile;
            }
        }

        /// <summary>
        /// <ja>
        /// ��؂�L���i�Z�p���[�^�j��\�����邩�ۂ��������܂��B
        /// </ja>
        /// <en>
        /// It is shown whether to display the separator. 
        /// </en>
        /// </summary>
        /// <remarks>
        /// <ja>
        /// true�̏ꍇ�A���j���[�O���[�v�̒��O�ɋ�؂�L���i�Z�p���[�^�j���\������܂��Bfalse�̏ꍇ�ɂ͕\������܂���B
        /// </ja>
        /// <en>
        /// The separator is displayed for true just before the menu group. It is not displayed for false. 
        /// </en>
        /// </remarks>
        public virtual bool ShowSeparator {
            get {
                return _showSeparator;
            }
        }


        /// <summary>
        /// <ja>
        /// ���j���[��z�u����ꏊ�̑Ώۂ������܂��B
        /// </ja>
        /// <en>
        /// The object of the place where the menu is arranged is shown. 
        /// </en>
        /// </summary>
        /// <remarks>
        /// <ja>
        /// <seealso cref="IPositionDesignation">IPositionDesignation</seealso>���Q�Ƃ��Ă��������B
        /// </ja>
        /// <en>
        /// Please refer to <seealso cref="IPositionDesignation">IPositionDesignation</seealso>.
        /// </en>
        /// </remarks>
        public virtual IAdaptable DesignationTarget {
            get {
                return _designationTarget;
            }
        }

        /// <summary>
        /// <ja>
        /// ���j���[�̕\���ʒu�������܂��B
        /// </ja>
        /// <en>
        /// The position where the menu is displayed is shown. 
        /// </en>
        /// </summary>
        /// <remarks>
        /// <ja>
        /// <seealso cref="IPositionDesignation">IPositionDesignation</seealso>���Q�Ƃ��Ă��������B
        /// </ja>
        /// <en>
        /// Refer to <seealso cref="IPositionDesignation">IPositionDesignation</seealso>.
        /// </en>
        /// </remarks>
        public virtual PositionType DesignationPosition {
            get {
                return _positionType;
            }
        }
        public virtual IAdaptable GetAdapter(Type adapter) {
            return CommandManagerPlugin.Instance.PoderosaWorld.AdapterManager.GetAdapter(this, adapter);
        }

        //�|�W�V�����Z�b�g
        /// <summary>
        /// <ja>
        /// ���j���[�̈ʒu��ݒ肵�܂��B
        /// </ja>
        /// <en>
        /// Set The position of the menu.
        /// </en>
        /// </summary>
        /// <param name="type">
        /// <ja>���j���[�̏ꏊ���w�肵�܂��B</ja>
        /// <en>Specifies the place of the menu.</en>
        /// </param>
        /// <param name="target">
        /// <ja>�ǂ̃��j���[�ɑ΂���ʒu�Ȃ̂����w�肵�܂��B<see cref="IPoderosaMenuGroup">IPoderosaMenuGroup</see>�łȂ���΂Ȃ�܂���B</ja>
        /// <en>To which menu position it is is specified. It should be <see cref="IPoderosaMenuGroup">IPoderosaMenuGroup</see>. </en>
        /// </param>
        /// <returns><ja>���̃I�u�W�F�N�g���g���߂�܂��B</ja><en>This object returns. </en></returns>
        /// <remarks>
        /// <ja>
        /// �f�t�H���g�ł́A���j���[�ʒu�́A�u�擪�iPositionType.First�j�v�ɐݒ肳��܂��B�ڍׂ́A<seealso cref="IPositionDesignation">IPositionDesignation</seealso>���Q�Ƃ��Ă��������B
        /// </ja>
        /// <en>
        /// In default, the menu position is set to the "head(PositionType.First)". 
        /// Refer to <seealso cref="IPositionDesignation">IPositionDesignation</seealso> for more information.
        /// </en>
        /// </remarks>
        public PoderosaMenuGroupImpl SetPosition(PositionType type, IAdaptable target) {
            _positionType = type;
            _designationTarget = target;
            return this;
        }
    }


    //IPoderosaMenuItem�W������
    /// <summary>
    /// <ja>
    /// <seealso cref="IPoderosaMenuItem">IPoderosaMenuItem</seealso>�����������N���X�ł��B�����Ȃ��Ŏ��s�����R�}���h���`���郁�j���[���ڂ��쐬����ۂɎg���܂��B
    /// </ja>
    /// <en>
    /// It is a class that implements <seealso cref="IPoderosaMenuItem">IPoderosaMenuItem</seealso>. When the menu item that defines the command executed without the argument is made, it uses it. 
    /// </en>
    /// </summary>
    /// <remarks>
    /// <ja>
    /// ���j���[���ڂ���������J���҂́A���̃N���X���g�����Ƃ�<seealso cref="IPoderosaMenuItem">IPoderosaMenuItem</seealso>�����������I�u�W�F�N�g
    /// ��e�Ղɍ쐬�ł��܂��B
    /// </ja>
    /// <en>
    /// The developer who implements the menu item can easily make the object where <seealso cref="IPoderosaMenuItem">IPoderosaMenuItem</seealso> is implemented by using this class. 
    /// </en>
    /// </remarks>
    /// <example>
    /// <ja>
    /// <seealso cref="IPoderosaMenuItem">IPoderosaMenuItem</seealso>�����������I�u�W�F�N�g�́A���̂悤�ɂ��č쐬�ł��܂��B
    /// <code>
    /// // ���炩���߃��j���[�����s���ꂽ�Ƃ��̃��j���[�ƃ��j���[���ڂ��쐬���Ă����܂��B
    /// 
    /// // �R�}���h
    /// PoderosaCommandImpl mycommand = new PoderosaCommandImpl(
    ///   delegate(ICommandTarget target)
    ///   {
    ///     // �R�}���h�����s���ꂽ�Ƃ��̏���
    ///    MessageBox.Show("���s����܂���");
    ///    return CommandResult.Succeeded;
    ///   },
    ///   delegate(ICommandTarget target)
    ///   {
    ///     // �R�}���h�����s�ł��邩�ǂ�����Ԃ�
    ///    return true;
    ///  }
    /// );
    /// 
    /// // ���j���[���ڂ��쐬
    /// PoderosaMenuItemImpl menuitem = new PoderosaMenuItemImpl(
    ///     mycommand, "My Menu Name");
    /// </code>
    /// </ja>
    /// <en>
    /// The object that implements <seealso cref="IPoderosaMenuItem">IPoderosaMenuItem</seealso> can be made as follows. 
    /// <code>
    /// // The menu and the menu item when the menu is executed are made beforehand. 
    /// 
    /// // Command.
    /// PoderosaCommandImpl mycommand = new PoderosaCommandImpl(
    ///   delegate(ICommandTarget target)
    ///   {
    ///     // Processing when command is executed
    ///    MessageBox.Show("Executed");
    ///    return CommandResult.Succeeded;
    ///   },
    ///   delegate(ICommandTarget target)
    ///   {
    ///     // Returned whether the command can be executed. 
    ///    return true;
    ///  }
    /// );
    /// 
    /// // Create the menu item.
    /// PoderosaMenuItemImpl menuitem = new PoderosaMenuItemImpl(
    ///     mycommand, "My Menu Name");
    /// </code>
    /// </en>
    /// </example>
    public class PoderosaMenuItemImpl : IPoderosaMenuItem {
        /// <exclude/>
        protected IPoderosaCommand _command;
        /// <exclude/>
        protected bool _usingStringResource;
        /// <exclude/>
        protected StringResource _strResource;
        /// <exclude/>
        protected string _textID;
        /// <exclude/>
        protected CheckedDelegate _checked;

        /// <summary>
        /// <ja>
        /// �R�}���hID�ƃ��j���[�̕\�������w�肵�ă��j���[���ڂ��쐬���܂��B
        /// </ja>
        /// <en>
        /// The menu item is made specifying the display name of command ID and the menu. 
        /// </en>
        /// </summary>
        /// <param name="command_id"><ja>���j���[���I�����ꂽ�Ƃ��ɌĂяo�������R�}���hID�ł��B</ja><en>It is command ID that wants to call when the menu is selected. </en></param>
        /// <param name="text"><ja>���j���[�ɕ\������e�L�X�g�ł��B</ja><en>Text displayed in menu.</en></param>
        /// <remarks>
        /// <ja>
        /// <paramref name="command_id">command_id</paramref>�Ɏw�肵���R�}���hID��������Ȃ��Ƃ��ɂ́A<see cref="P:Poderosa.Commands.PoderosaMenuItemImpl.AssociatedCommand">AssociatedCommand�v���p�e�B</see>
        /// ��null�ɂȂ�܂��B
        /// </ja>
        /// <en>
        /// When command ID specified for <paramref name="command_id">command_id</paramref> is not found, the <see cref="AssociatedCommand">AssociatedCommand property</see> becomes null. 
        /// </en>
        /// </remarks>
        /// <overloads>
        /// <summary>
        /// <ja>
        /// ���j���[���ڂ��쐬���܂��B
        /// </ja>
        /// <en>
        /// Create the menu item.
        /// </en>
        /// </summary>
        /// </overloads>
        public PoderosaMenuItemImpl(string command_id, string text)
            : this(BindCommand(command_id), null, text) {
        }
        /// <summary>
        /// <ja>
        /// ���s����R�}���h��<seealso cref="IPoderosaCommand">IPoderosaCommand</seealso>�ƃ��j���[�̕\�������w�肵�ă��j���[���ڂ��쐬���܂��B
        /// </ja>
        /// <en>
        /// The menu item is made specifying the display name of <seealso cref="IPoderosaCommand">IPoderosaCommand</seealso> and the menu of the executed command. 
        /// </en>
        /// </summary>
        /// <param name="command"><ja>���j���[���I�����ꂽ�Ƃ��ɌĂяo�������R�}���h�ł��B</ja><en>It is command that wants to call when the menu is selected. </en></param>
        /// <param name="text"><ja>���j���[�ɕ\������e�L�X�g�ł��B</ja><en>Text displayed in menu.</en></param>
        /// <remarks>
        /// <ja><paramref name="command">command</paramref>��null���w�肵�Ă͂����܂���B</ja><en>Do not specify null for command. </en>
        /// </remarks>
        public PoderosaMenuItemImpl(IPoderosaCommand command, string text)
            : this(command, null, text) {
        }

        /// <summary>
        /// <ja>
        /// �R�}���hID�ƃJ���`���A���j���[�̕\�������w�肵�ă��j���[���ڂ��쐬���܂��B
        /// </ja>
        /// <en>
        /// The menu item is made specifying the display name of command ID, culture, and the menu. 
        /// </en>
        /// </summary>
        /// <param name="command_id"><ja>���j���[���I�����ꂽ�Ƃ��ɌĂяo�������R�}���hID�ł��B</ja><en>Command ID that wants to call when menu is selected</en></param>
        /// <param name="sr"><ja>�J���`�����ł��B</ja><en>Information of culture.</en></param>
        /// <param name="textID"><ja>���j���[�ɕ\������e�L�X�gID�ł��B</ja><en>Text ID displayed in menu</en></param>
        /// <remarks>
        /// <ja>
        /// <paramref name="command_id">command_id</paramref>�Ɏw�肵���R�}���hID��������Ȃ��Ƃ��ɂ́A<see cref="AssociatedCommand">AssociatedCommand�v���p�e�B</see>
        /// ��null�ɂȂ�܂��B
        /// </ja>
        /// <en>
        /// When command ID specified for <paramref name="command_id">command_id</paramref> is not found, the <see cref="AssociatedCommand">AssociatedCommand property</see> becomes null. 
        /// </en>
        /// </remarks>
        public PoderosaMenuItemImpl(string command_id, StringResource sr, string textID)
            : this(BindCommand(command_id), sr, textID) {
        }

        /// <summary>
        /// <ja>���s����R�}���h��<seealso cref="IPoderosaCommand">IPoderosaCommand</seealso>�A�J���`���A���j���[�̕\�������w�肵�ă��j���[���ڂ��쐬���܂��B</ja>
        /// <en>The menu item is made specifying the display name of executed <seealso cref="IPoderosaCommand">IPoderosaCommand</seealso> of the command, culture, and menu. </en>
        /// </summary>
        /// <param name="command"><ja>���j���[���I�����ꂽ�Ƃ��ɌĂяo�������R�}���h�ł��B</ja><en>Command that wants to call when menu is selected</en></param>
        /// <param name="sr"><ja>�J���`�����ł��B</ja><en>Information of culture.</en></param>
        /// <param name="textID"><ja>���j���[�ɕ\������e�L�X�gID�ł��B</ja><en>Text ID displayed in menu</en></param>
        public PoderosaMenuItemImpl(IPoderosaCommand command, StringResource sr, string textID) {
            Debug.Assert(command != null);
            _command = command;
            _usingStringResource = sr != null;
            _strResource = sr;
            _textID = textID;
        }
        private static IGeneralCommand BindCommand(string command_id) {
            IGeneralCommand cmd = CommandManagerPlugin.Instance.Find(command_id);
            Debug.Assert(cmd != null, "Command Not Found");
            return cmd;
        }

        /// <summary>
        /// <ja>
        /// ���j���[���I�����ꂽ�Ƃ��ɌĂяo�����R�}���h�������܂��B
        /// </ja>
        /// <en>
        /// The command called when the menu is selected is shown. 
        /// </en>
        /// </summary>
        public virtual IPoderosaCommand AssociatedCommand {
            get {
                return _command;
            }
        }

        /// <summary>
        /// <ja>
        /// ���j���[�ɕ\������e�L�X�g�������܂��B
        /// </ja>
        /// <en>
        /// The text displayed in the menu is shown. 
        /// </en>
        /// </summary>
        public virtual string Text {
            get {
                return _usingStringResource ? _strResource.GetString(_textID) : _textID;
            }
        }

        /// <summary>
        /// <ja>
        /// ���j���[���I���\���ǂ����������܂��B
        /// </ja>
        /// <en>
        /// It is shown whether the menu can be selected. 
        /// </en>
        /// </summary>
        /// <param name="target"><ja>�����ΏۂƂȂ�^�[�Q�b�g�ł��B</ja><en>Target to be processed</en></param>
        /// <returns><ja>�I���\�Ȃ�true�A�I��s�Ȃ�false���Ԃ���܂��B</ja><en>If it is selectable, return true. It isn't, return false.</en></returns>
        /// <remarks>
        /// <ja>
        /// ���̃��\�b�h�́A������<see cref="AssociatedCommand">AssiciatedCommand</see>�v���p�e�B�Ŏ����ꂽ
        /// <seealso cref="IPoderosaCommand">IPoderosaCommand</seealso>��<seealso cref="IPoderosaCommand.CanExecute">CanExecute���\�b�h</seealso>
        /// ���Ăяo�����ƂŎ�������Ă��܂��B
        /// </ja>
        /// <en>
        /// This method is implemented by calling the <seealso cref="IPoderosaCommand.CanExecute">CanExecute method</seealso> of <seealso cref="IPoderosaCommand">IPoderosaCommand</seealso> shown internally in the <see cref="AssociatedCommand">AssiciatedCommand</see> property. 
        /// </en>
        /// </remarks>
        public virtual bool IsEnabled(ICommandTarget target) {
            return _command.CanExecute(target);
        }

        /// <summary>
        /// <ja>
        /// ���j���[�Ƀ`�F�b�N���t�����Ă��邩�ǂ����������܂��B
        /// </ja>
        /// <en>
        /// It is shown whether the menu is checked. 
        /// </en>
        /// </summary>
        /// <param name="target"><ja>�����ΏۂƂȂ�^�[�Q�b�g�ł��B</ja><en>Target to be processed.</en></param>
        /// <returns><ja>�`�F�b�N���t���Ă���Ȃ�true�A�����łȂ��Ȃ�false���Ԃ���܂��B</ja><en>It is true, and a return of false if checked if not so. </en></returns>
        /// <remarks>
        /// <ja>
        /// ���̃��\�b�h�́A���false���߂�܂��B
        /// </ja>
        /// <en>
        /// False always returns in this method. 
        /// </en>
        /// </remarks>
        public virtual bool IsChecked(ICommandTarget target) {
            return _checked == null ? false : _checked(target);
        }

        public virtual IAdaptable GetAdapter(Type adapter) {
            return CommandManagerPlugin.Instance.PoderosaWorld.AdapterManager.GetAdapter(this, adapter);
        }
    }
}
