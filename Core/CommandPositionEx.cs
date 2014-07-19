/*
 * Copyright 2004,2006 The Poderosa Project.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *
 * $Id: CommandPositionEx.cs,v 1.2 2011/10/27 23:21:55 kzmi Exp $
 */
using System;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using System.Text;

namespace Poderosa.Commands {
    /// <summary>
    /// <ja>
    /// ���j���[��c�[���o�[�̈ʒu���w�肵�܂��B
    /// </ja>
    /// <en>
    /// Specifies the position of the menu and the toolbar.
    /// </en>
    /// </summary>
    /// <remarks>
    /// <ja>
    /// �ڍׂ́A<seealso cref="IPositionDesignation">IPositionDesignation</seealso>�̉�����Q�Ƃ��Ă��������B
    /// </ja>
    /// <en>
    /// For more information, please refer to <seealso cref="IPositionDesignation">IPositionDesignation</seealso>.
    /// </en>
    /// </remarks>
    public enum PositionType {
        /// <summary>
        /// <ja>�擪</ja>
        /// <en>First</en>
        /// </summary>
        First,
        /// <summary>
        /// <ja>����</ja>
        /// <en>Last</en>
        /// </summary>
        Last,
        /// <summary>
        /// <ja>�Ώۂ̒��O</ja>
        /// <en>Previous to the object.</en>
        /// </summary>
        PreviousTo,
        /// <summary>
        /// <ja>�Ώۂ̒���</ja>
        /// <en>Next to the object.</en>
        /// </summary>
        NextTo,
        /// <summary>
        /// <ja>�����I�Ɏw�肵�Ȃ�</ja>
        /// <en>It doesn't specify it specifying it. </en>
        /// </summary>
        DontCare
    }


    /// <summary>
    /// <ja>
    /// ���j���[��c�[���o�[�̈ʒu�𐧌䂷�邽�߂̃C���^�[�t�F�C�X�ł��B
    /// </ja>
    /// <en>
    /// Interface to control position of menu and toolbar
    /// </en>
    /// </summary>
    public interface IPositionDesignation : IAdaptable {
        //Target��null���w�肵���Ƃ��́AFirst, Last, DontCare�̂ǂꂩ�B
        //Target����null�̂Ƃ��́APreviousTo, NextTo�̂ǂꂩ�B
        /// <summary>
        /// <ja>
        /// �ǂ̍��ڂɑ΂��đO��֌W�������̂����w�肵�܂��B
        /// </ja>
        /// <en>
        /// Specifies items the context is shown.
        /// </en>
        /// </summary>
        /// <remarks>
        /// <ja>
        /// <para>
        /// ���̃v���p�e�B�́A<seealso cref="DesignationPosition">DesignationPosition</seealso>�̑Ώۂ������܂��B
        /// </para>
        /// <para>
        /// ���j���[�̏ꍇ�ɂ́A<seealso cref="IPoderosaMenuGroup">IPoderosaMenuGroup</seealso>���A�c�[���o�[�̏ꍇ�ɂ�
        /// <seealso cref="Poderosa.Forms.IToolBarComponent">IToolBarComponent</seealso>���w�肵�܂��B
        /// </para>
        /// </ja>
        /// <en>
        /// <para>
        /// This property shows the object of <seealso cref="DesignationPosition">DesignationPosition</seealso>. 
        /// </para>
        /// <para>
        /// <seealso cref="IPoderosaMenuGroup">IPoderosaMenuGroup</seealso> is specified for the menu and <seealso cref="Poderosa.Forms.IToolBarComponent">IToolBarComponent</seealso> is specified for the toolbar. 
        /// </para>
        /// </en>
        /// </remarks>
        IAdaptable DesignationTarget {
            get;
        } //can be null
        /// <summary>
        /// <ja>
        /// <seealso cref="DesignationTarget">DesignationTarget</seealso>�ɑ΂���ʒu���w�肵�܂��B
        /// </ja>
        /// <en>
        /// The position to <seealso cref="DesignationTarget">DesignationTarget</seealso> is specified. 
        /// </en>
        /// </summary>
        /// <remarks>
        /// <ja>
        /// <para>
        /// �w��ł���ʒu�́A�u�擪�v�u�����v�u���O�v�u����v�u�����I�Ɏw�肵�Ȃ��v�̂����ꂩ�ł��B
        /// </para>
        /// <note type="implementnotes">
        /// 2�̈قȂ郁�j���[�����҂Ƃ��u�擪�v��v�������ꍇ�ȂǁA�����s�\�Ȉʒu���\�����ꂽ�Ƃ��ɂ́A������Poderosa�ɂ���Ē��₳��܂��B���̂��߁A�K�������w�肵���ʒu�ǂ���ɕ��ԂƂ͌���܂���B
        /// </note>
        /// <list type="table">
        ///     <listheader>
        ///         <term>�l</term>
        ///         <description>�Ӗ�</description>
        ///         <description><seealso cref="DesignationTarget">DesignationTarget</seealso>�̒l</description>
        ///     </listheader>
        ///     <item>
        ///         <term>First</term>
        ///         <description>�擪</description>
        ///         <description>null���w�肵�Ă�������</description>
        ///     </item>
        ///     <item>
        ///         <term>Last</term>
        ///         <description>����</description>
        ///         <description>null���w�肵�Ă�������</description>
        ///     </item>
        ///     <item>
        ///         <term>PreviousTo</term>
        ///         <description><seealso cref="DesignationTarget">DesignationTarget</seealso>�Ŏw�肵�����ڂ̒��O</description>
        ///         <description>�Ώۍ��ڂ�n���Ă�������</description>
        ///     </item>
        ///     <item>
        ///         <term>NextTo</term>
        ///         <description><seealso cref="DesignationTarget">DesignationTarget</seealso>�Ŏw�肵�����ڂ̒���</description>
        ///         <description>�Ώۍ��ڂ�n���Ă�������</description>
        ///     </item>
        ///     <item>
        ///         <term>DontCare</term>
        ///         <description>�����I�Ɏw�肵�Ȃ�</description>
        ///         <description>null���w�肵�Ă�������</description>
        ///     </item>
        /// </list>
        /// </ja>
        /// <en>
        /// <para>
        /// The position that can be specified is either "Head", "End", "Immediately before", "Immediately after" or "Do not specify it specifying it". 
        /// </para>
        /// <note type="implementnotes">
        /// When the position that cannot be achieved is composed when two different menus demand both and "Head", the order is mediated by Poderosa. 
        /// Therefore, it doesn't necessarily queue up as it is a specified position. 
        /// </note>
        /// <list type="table">
        ///     <listheader>
        ///         <term>Value</term>
        ///         <description>Meaning</description>
        ///         <description>Value of <seealso cref="DesignationTarget">DesignationTarget</seealso></description>
        ///     </listheader>
        ///     <item>
        ///         <term>First</term>
        ///         <description>Head</description>
        ///         <description>Set null</description>
        ///     </item>
        ///     <item>
        ///         <term>Last</term>
        ///         <description>Last</description>
        ///         <description>Set null</description>
        ///     </item>
        ///     <item>
        ///         <term>PreviousTo</term>
        ///         <description>The item specified with <seealso cref="DesignationTarget">DesignationTarget</seealso></description>
        ///         <description>Pass the specified item.</description>
        ///     </item>
        ///     <item>
        ///         <term>NextTo</term>
        ///         <description>The item specified with <seealso cref="DesignationTarget">DesignationTarget</seealso> just behind</description>
        ///         <description>Pass the specified item.</description>
        ///     </item>
        ///     <item>
        ///         <term>DontCare</term>
        ///         <description>It doesn't specify it specifying it. </description>
        ///         <description>Set null</description>
        ///     </item>
        /// </list>
        /// </en>
        /// </remarks>
        PositionType DesignationPosition {
            get;
        }
    }

    //���ꂾ���Ȃ̂œ����t�@�C���Ƀ\�[�^�ƃe�X�g�P�[�X�܂ŏ����Ă��܂�
    /// <exclude/>
    public class PositionDesignationSorter {
        private class Entry : IComparable<Entry> {
            public int index;
            public IAdaptable content;
            public IPositionDesignation designation;
            public Entry dependency;

            public Entry(int i, IAdaptable c) {
                index = i;
                content = c;
                designation = (IPositionDesignation)c.GetAdapter(typeof(IPositionDesignation));
            }

            private bool IsIndependent {
                get {
                    return dependency == null;
                }
            }

            //�ˑ����Ă�����̂���ɗ���悤�ɕ��ёւ���
            public int CompareTo(Entry other) {
                if (this.IsIndependent) {
                    if (other.IsIndependent)
                        return this.index - other.index; //���̏�����ێ�
                    else
                        return -1; //�������O�ɗ���
                }
                else {
                    if (other.IsIndependent)
                        return 1; //��������ɗ���
                    else {
                        int r = this.dependency.CompareTo(other.dependency);
                        if (r == 0)
                            r = this.index - other.index; //�ˑ���Ŕ���ł��Ȃ��ꍇ�͎d���Ȃ�
                        return r;
                    }
                }
            }
        }

        //�ˑ��֌W�ɏ]���ă\�[�g����B�eIAdaptable�́A�I�v�V���i����IPositionDesignation����������B
        //�ˑ��悪����Ȃ�΁Asrc�Ɋ܂܂�Ă���K�v������B
        public static ICollection SortItems(ICollection src) {
            List<Entry> map = new List<Entry>(src.Count);
            int i = 0;
            //Entry���\��
            foreach (IAdaptable a in src) {
                Debug.Assert(a != null);
                map.Add(new Entry(i++, a));
            }
            //�ˑ�����`�F�b�N
            foreach (Entry e in map)
                e.dependency = FindDependencyFor(e, map);
            //�\�[�g
            //TODO �ˑ��֌W�Ƀ��[�v������Ƃ����~��
            map.Sort();

            //���ʂ̍\�z
            return BuildResult(map);
        }

        private static ICollection BuildResult(List<Entry> map) {
            LinkedList<IAdaptable> result = new LinkedList<IAdaptable>();
            LinkedListNode<IAdaptable> firstzone = null;
            LinkedListNode<IAdaptable> lastzone = null;
            foreach (Entry e in map) {
                if (e.dependency == null) {
                    //�ˑ����Ȃ��̂΂����Afirst-dontcare-last�̊e�]�[�����ŕ��ԁB�e�]�[�����͌��̓��͏���ێ�

                    //designation�Ȃ���DontCare�ɓ�����
                    if (e.designation == null || e.designation.DesignationPosition == PositionType.DontCare) {
                        if (lastzone == null)
                            result.AddLast(e.content);
                        else
                            result.AddBefore(lastzone, e.content);
                    }
                    else if (e.designation.DesignationPosition == PositionType.First) {
                        if (firstzone == null)
                            firstzone = result.AddFirst(e.content);
                        else
                            firstzone = result.AddAfter(firstzone, e.content);
                    }
                    else if (e.designation.DesignationPosition == PositionType.Last) {
                        if (lastzone == null)
                            lastzone = result.AddLast(e.content);
                        else
                            lastzone = result.AddBefore(lastzone, e.content);
                    }
                }
                else { //�ˑ�������
                    LinkedListNode<IAdaptable> n = result.Find(e.dependency.content);
                    Debug.Assert(n != null);
                    Debug.Assert(e.designation.DesignationPosition != PositionType.DontCare);
                    if (e.designation.DesignationPosition == PositionType.NextTo)
                        result.AddAfter(n, e.content);
                    else
                        result.AddBefore(n, e.content);
                }
            }
            return result;
        }

        //�ˑ����������
        private static Entry FindDependencyFor(Entry e, List<Entry> map) {
            if (e.designation == null)
                return null;
            else if (e.designation.DesignationTarget == null) {
                PositionType p = e.designation.DesignationPosition;
                if (!(p == PositionType.First || p == PositionType.Last || p == PositionType.DontCare))
                    throw new ArgumentException("if IPositionDesignation#Target returns null, #Position must be First, Last, or DontCare");
                return null;
            }
            else {
                IAdaptable target = e.designation.DesignationTarget;
                Entry r = Find(target, map);
                if (r == null)
                    throw new ArgumentException("IPositionDesignation#Target must return a member of the argument collection of SortItem()");
                if (!(e.designation.DesignationPosition == PositionType.NextTo || e.designation.DesignationPosition == PositionType.PreviousTo))
                    throw new ArgumentException("if IPositionDesignation#Target returns an object, #Position must be PreviousTo or NextTo");
                return r;
            }
        }

        private static Entry Find(IAdaptable target, List<Entry> map) {
            foreach (Entry e in map)
                if (e.content == target)
                    return e;
            return null;
        }
    }
}
