/*
 * Copyright 2004,2006 The Poderosa Project.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *
 * $Id: PaneDivision.cs,v 1.3 2012/05/20 09:10:30 kzmi Exp $
 */
using System;
using System.Diagnostics;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Poderosa.UI {

    /*
     * �y�C���̕���
     */

    /// <exclude/>
    public class PaneDivision {

        /// <summary>
        /// 
        /// </summary>
        /// <exclude/>
        public enum Direction {
            TB, //�㉺����
            LR  //���E����
        }
        //���������ɂ���ċ���������̗v�f(��or����)��Ԃ�
        public static int SizeToLength(Size size, Direction dir) {
            return dir == Direction.TB ? size.Height : size.Width;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exclude/>
        public interface IPane {
            Control AsDotNet();
            string Label {
                get;
            }
            void Dispose();

            Size Size {
                get;
                set;
            }
            DockStyle Dock {
                get;
                set;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        /// <exclude/>
        public delegate IPane PaneCreationDelegate(string label);

        /// <summary>
        /// �����̂P�m�[�h
        /// </summary>
        private sealed class DivisionNode {
            private DivisionNode _next;      //���X�g�̎��B���[�ł�0
            private double _ratio;           //�����̊���
            private IPane _pane;             //�Ђ����Ă���y�C��
            private DivisionList _childList; //�q���ٕ̈����̃��X�g�B_pane,_childList�͏�ɕЕ��݂̂�null
            private PaneSplitter _splitter;  //�X�v���b�^
            private IntermediateContainer _intermediateContainer;

            private DivisionList _parent;    //�������i�[���郊�X�g

            private bool _splitterEventGuarding; //�]�v�ȃC�x���g�𖳎����邽�߂̃t���O

            //SplitInfo����̍\�z�p
            public DivisionNode(DivisionList parent, IPane pane, double ratio) {
                Debug.Assert(parent != null);
                Debug.Assert(pane != null);
                _parent = parent;
                _pane = pane;
                _ratio = ratio;
            }
            public DivisionNode(DivisionList parent, DivisionList child, double ratio) {
                Debug.Assert(parent != null);
                Debug.Assert(child != null);
                Debug.Assert(child.ParentNode == null);
                _parent = parent;
                _childList = child;
                _childList.ParentNode = this;
                _ratio = ratio;
            }

            internal void Dispose() {
                if (_pane != null)
                    _pane.Dispose();
                if (_childList != null)
                    _childList.Dispose();
                if (_splitter != null)
                    _splitter.Dispose();
                if (_intermediateContainer != null)
                    _intermediateContainer.Dispose();
            }

            public double Ratio {
                get {
                    return _ratio;
                }
                set {
                    _ratio = value;
                }
            }
            public DivisionNode Next {
                get {
                    return _next;
                }
                set {
                    _next = value;
                }
            }
            public DivisionList ParentList {
                get {
                    return _parent;
                }
            }
            public IPane Pane {
                get {
                    return _pane;
                }
            }
            public DivisionList ChildList {
                get {
                    return _childList;
                }
            }

            public bool IsLast {
                get {
                    return _next == null;
                }
            }
            public bool IsPreLast {
                get {
                    return _next._next == null;
                }
            }
            public bool IsLeaf {
                get {
                    return _pane != null;
                }
            }
            private PaneDivision ParentDivision {
                get {
                    return _parent.Division;
                }
            }

            //�����̃v���p�e�B�̈Ӗ��̓h�L�������g���Q��
            internal Control HostingControl {
                get {
                    return _pane != null ? _pane.AsDotNet() : _childList.HostingControl;
                }
            }
            private int HostingLength {
                get {
                    return SizeToLength(this.HostingControl.Size);
                }
            }
            private int TotalLength {
                get {
                    if (IsLast)
                        return this.HostingLength;
                    else
                        return this.HostingLength + PaneSplitter.SPLITTER_WIDTH + _next.TotalLength;
                }
            }
            private int RequiredMinimumLength {
                get {
                    if (IsLast)
                        return this.RequiredMinimumHostingLength;
                    else
                        return this.RequiredMinimumHostingLength + PaneSplitter.SPLITTER_WIDTH + _next.RequiredMinimumLength;
                }
            }
            private int RequiredMinimumHostingLength {
                get {
                    return SizeToLength(this.RequiredMinimumHostingSize);
                }
            }
            private Size RequiredMinimumHostingSize {
                get {
                    if (IsLeaf) {
                        int min = this.ParentDivision.MinimumEdgeLength;
                        return new Size(min, min);
                    }
                    else {
                        return _childList.FirstNode.RequiredMinimumSize;
                    }
                }
            }
            internal Size RequiredMinimumSize {
                get {
                    if (IsLast)
                        return this.RequiredMinimumHostingSize;
                    else {
                        Size s1 = this.RequiredMinimumHostingSize;
                        Size s2 = _next.RequiredMinimumSize;
                        if (_parent.Direction == Direction.TB)
                            return new Size(Math.Max(s1.Width, s2.Width), s1.Height + PaneSplitter.SPLITTER_WIDTH + s2.Height);
                        else
                            return new Size(s1.Width + PaneSplitter.SPLITTER_WIDTH + s2.Width, Math.Max(s1.Height, s2.Height));
                    }
                }
            }
            private int SizeToLength(Size size) {
                return _parent.Direction == Direction.TB ? size.Height : size.Width;
            }


            //�q������^�[�Q�b�g��T��
            public DivisionNode FindNode(IPane t) {
                DivisionNode n = null;

                if (_pane != null && _pane.Equals(t))
                    return this; //struct�̂��Ƃ�����̂�Equals���g��

                if (_next != null)
                    n = _next.FindNode(t);
                if (n != null)
                    return n;

                if (_childList != null)
                    n = _childList.FirstNode.FindNode(t);
                return n;
            }
            public DivisionNode FindChildList(DivisionList list) {
                if (_childList != null && _childList == list)
                    return this;

                DivisionNode n = null;
                if (_next != null)
                    n = _next.FindChildList(list);
                if (n != null)
                    return n;

                if (_childList != null)
                    n = _childList.FirstNode.FindChildList(list);
                return n;
            }

            //���
            internal SplitFormat.Node Format() {
                return new SplitFormat.Node(
                    this.IsLast ? 0 : _ratio,
                    _childList == null ? null : _childList.Format(),
                    _pane == null ? null : _pane.Label,
                    _next == null ? null : _next.Format());
            }

            public IPane FindFirst(PaneCondition condition) {
                if (_pane != null) {
                    if (condition(_pane))
                        return _pane;
                }
                else if (_childList != null) {
                    IPane p = _childList.FindFirst(condition);
                    if (p != null)
                        return p;
                }

                if (_next != null) {
                    return _next.FindFirst(condition);
                }

                return null;
            }

            //�č\�z�ɐ旧���ăR���g���[���̐e�q�֌W��j�����Ă����B���̕��������ƕ��ꂪ�Ȃ�
            public void ClearControlTree() {
                if (_intermediateContainer != null)
                    _intermediateContainer.Controls.Clear();
                if (_next != null)
                    _next.ClearControlTree();
                if (!this.IsLeaf)
                    _childList.ClearControlTree();
            }

            //�R���g���[���̐e�q�֌W�̍č\�z
            public void BuildControlTree(Control container) {
                if (IsLast) {
                    //Leaf�̂Ƃ��͉���*1�őg�ݍ��܂�Ă���
                    if (!IsLeaf)
                        _childList.FirstNode.BuildControlTree(container);
                }
                else {
                    if (_splitter == null)
                        CreateSplitter(); //�x���쐬

                    //fill�����R���g���[��
                    Control fill = this.HostingControl;
                    Debug.Assert(fill != null);

                    //�X�v���b�^�̃^�[�Q�b�g�R���g���[��
                    Control target;
                    if (this.IsPreLast) {
                        target = _next.HostingControl; //*1
                    }
                    else {
                        if (_intermediateContainer == null)
                            CreateIntermediateControl(); //�x���쐬
                        target = _intermediateContainer;
                    }

                    //�ǉ�
                    fill.Dock = DockStyle.Fill;
                    target.Dock = _parent.Direction == Direction.TB ? DockStyle.Bottom : DockStyle.Right;
                    _splitter.TargetControl = target;
                    container.Controls.AddRange(new Control[] { fill, _splitter, target });

                    //�����A�����Ŏ}�ւƍċA
                    _next.BuildControlTree(target);
                    if (!this.IsLeaf)
                        _childList.FirstNode.BuildControlTree(_childList.HostingControl);
                }
            }

            public void SetParentList(DivisionList list) {
                _parent = list;
            }

            //size�ɂ͂܂�悤�ɃT�C�Y����
            public void AdjustSplitPosition(Size size) {
                if (IsLast) { //splitter������
                    if (!IsLeaf)
                        _childList.AdjustSplitPosition(size);
                }
                else {
                    int splitpos = (int)(_parent.HostingLength * SumRatio(_next)) - PaneSplitter.SPLITTER_WIDTH / 2;
                    //����E����
                    int minimum_splitpos = _next.RequiredMinimumLength;
                    int maximum_splitpos = SizeToLength(size) - this.RequiredMinimumHostingLength - PaneSplitter.SPLITTER_WIDTH;
                    splitpos = UIUtil.AdjustRange(splitpos, minimum_splitpos, maximum_splitpos);

                    SetSplitterPosition(splitpos);

                    Size splitsize = _parent.Direction == Direction.TB ?
                        new Size(size.Width, splitpos) : new Size(splitpos, size.Height);
                    _splitter.TargetControl.Size = splitsize; //next.HostingControl or _container

                    //�����A�����Ŏ}�ւƍċA
                    _next.AdjustSplitPosition(splitsize);

                    if (!IsLeaf) {
                        Size hostingsize = _parent.Direction == Direction.TB ?
                            new Size(size.Width, size.Height - PaneSplitter.SPLITTER_WIDTH - splitpos) : new Size(size.Width - PaneSplitter.SPLITTER_WIDTH - splitpos, size.Height);
                        _childList.AdjustSplitPosition(hostingsize);
                    }
                }
            }

            //splitter��MinSize, MinExtra�ݒ�
            public void AdjustSplitMinSize() {
                if (!this.IsPreLast)
                    _next.AdjustSplitMinSize();
                if (!this.IsLeaf)
                    _childList.FirstNode.AdjustSplitMinSize();

                //�u���̎��v�������Ă��錻��̒����́A_splitter�ɂ���ĉe�����󂯂邱�Ƃ͂Ȃ��悤�ɂ���B���̒�����_next�̍ŏ������𑫂������̂�MinSize�Ƃ������ƂɂȂ�
                DivisionNode nn = _next._next;
                int current_fixed = nn == null ? 0 : nn.TotalLength + PaneSplitter.SPLITTER_WIDTH;
                _splitter.MinSize = current_fixed + _next.RequiredMinimumHostingLength;
                _splitter.MinExtra = this.RequiredMinimumHostingLength;
            }

            //�ׂ֑}��
            public void InsertNext(IPane newpane) {
                _ratio /= 2;
                DivisionNode newnode = new DivisionNode(_parent, newpane, _ratio); //������
                newnode._next = _next;
                _next = newnode;
            }

            //�q�y�C���ƃ��X�g�̌����B�������ƕ������Ɏg��
            public void ReplacePaneByChildList(DivisionList newlist) {
                Debug.Assert(IsLeaf);
                newlist.HostingControl.Dock = _pane.Dock;
                newlist.HostingControl.Size = _pane.Size;
                _pane = null;
                _childList = newlist;
                Debug.Assert(!IsLeaf);
            }
            public void ReplaceChildListByPane(IPane pane) {
                Debug.Assert(!IsLeaf);
                _pane = pane;
                _pane.Size = _childList.HostingControl.Size;
                _childList = null;
                Debug.Assert(IsLeaf);
            }

            //�C�x���g�𖳎�����K�[�h���ŃZ�b�g
            private void SetSplitterPosition(int value) {
                _splitterEventGuarding = true;
                _splitter.SplitPosition = value;
                _splitterEventGuarding = false;
            }

            private void OnSplitterMoved(object sender, SplitterEventArgs e) {
                if (_splitterEventGuarding)
                    return; //����̓v���O��������ʒu�w�肵���ꍇ�Ɏ��s����̂�����邽��

                double length = (double)_parent.HostingLength;
                double newratio = this.HostingLength / length;
                double diff = newratio - _ratio;

                if (_next != null)
                    _next._ratio -= diff;
                _ratio = newratio;
                Debug.Assert(_ratio >= 0 && _ratio <= 1.0);

                _parent.FirstNode.AdjustSplitMinSize();
                //Splitter�����X�g�𕪊����Ă���Ƃ��͎q�ɓ`�d������K�v������
                if (!IsLeaf)
                    _childList.AdjustSplitPosition(this.HostingControl.Size);
                if (!_next.IsLeaf)
                    _next.ChildList.AdjustSplitPosition(_next.HostingControl.Size);
            }

            private void OnSplitterMouseUp(object sender, MouseEventArgs args) {
                if (args.Button != MouseButtons.Middle)
                    return; //���N���b�N�݂̂ɋ�������

                PaneDivision.IUIActionHandler h = this.ParentDivision.UIActionHandler;
                if (h != null) {
                    IPane unify_target = FindUnifyTarget();
                    if (unify_target != null)
                        h.RequestUnify(unify_target);
                    //TODO �����ł��Ȃ��Ƃ��͒ʒm(�X�e�[�^�X�o�[��)���o�����ق�����������
                }
            }

            //�����Ώۂ�T��
            private IPane FindUnifyTarget() {
                //_next._pane�����݂���΂���B����->�����ƍs�����Ƃ��ɂ��Ƃ̃r���[�������Ă���悤�ɂ��邽�߁B
                //�łȂ����_pane�A������Ȃ���Ό����s��
                if (_next != null && _next._pane != null)
                    return _next._pane;
                else if (_pane != null)
                    return _pane;
                else
                    return null;
            }

            //�t�����쐬�n
            private void CreateIntermediateControl() {
                _intermediateContainer = new IntermediateContainer(this.ParentDivision);
            }
            private void CreateSplitter() {
                _splitter = new PaneSplitter();
                _splitter.SplitterMoved += new SplitterEventHandler(OnSplitterMoved);
                _splitter.MouseUp += new MouseEventHandler(OnSplitterMouseUp);
                _splitter.EnabledCollapse = false;
            }

            // node���疖���܂ł�_ratio�����v����
            private static double SumRatio(DivisionNode node) {
                double sum = 0;
                while (node != null) {
                    sum += node._ratio;
                    node = node._next;
                }
                return sum;
            }
        }

        /// <summary>
        /// �P�����ւ̕����̃��X�g�B�����͂Q�ȏ�
        /// </summary>
        private sealed class DivisionList {
            private DivisionNode _first;             //�ŏ��̃m�[�h
            private readonly Direction _direction;            //vertical or horizontal
            private IntermediateContainer _hostingControl;
            private readonly PaneDivision _parentDivision; //container
            private DivisionNode _parentNode;

            //������ԂłQ�쐬
            public DivisionList(PaneDivision division, DivisionNode parent, Direction direction, IPane pane1, IPane pane2, Size host_size, DockStyle host_dock) {
                _parentDivision = division;
                _parentNode = parent;
                _direction = direction;

                pane1.Dock = DockStyle.Fill;
                pane2.Dock = _direction == Direction.TB ? DockStyle.Bottom : DockStyle.Right;
                _first = new DivisionNode(this, pane1, 1.0);
                _first.InsertNext(pane2);

                _hostingControl = new IntermediateContainer(division);
                _hostingControl.Size = host_size;
                _hostingControl.Dock = host_dock;
            }
            //SplitInfo����̍\�z�p
            public DivisionList(PaneDivision division, Direction direction, DockStyle host_dock) {
                _parentDivision = division;
                _direction = direction;

                _hostingControl = new IntermediateContainer(division);
                _hostingControl.Dock = host_dock;
            }

            internal void Dispose() {
                _hostingControl.Dispose();
                DivisionNode node = _first;
                while (node != null) {
                    node.Dispose();
                    node = node.Next;
                }
            }

            public DivisionNode ParentNode {
                get {
                    return _parentNode;
                }
                set {
                    _parentNode = value;
                }
            }
            public Direction Direction {
                get {
                    return _direction;
                }
            }
            public PaneDivision Division {
                get {
                    return _parentDivision;
                }
            }
            public IntermediateContainer HostingControl {
                get {
                    return _hostingControl;
                }
            }
            public int HostingLength {
                get {
                    return _direction == Direction.TB ? _hostingControl.Height : _hostingControl.Width;
                }
            }
            public IPane FirstPane {
                get {
                    return _first.IsLeaf ? _first.Pane : _first.ChildList.FirstPane;
                }
            }
            public int NodeCount {
                get {
                    int c = 0;
                    DivisionNode n = _first;
                    while (n != null) {
                        c++;
                        n = n.Next;
                    }
                    return c;
                }
            }
            public DivisionNode FirstNode {
                get {
                    return _first;
                }
                set {
                    _first = value;
                }
            }

            //�����ɕ�������Ă��邩�H
            public bool IsEquallyDivided {
                get {
                    double min = 1, max = 0;
                    DivisionNode n = _first;
                    while (n != null) {
                        min = Math.Min(min, n.Ratio);
                        max = Math.Max(max, n.Ratio);
                        n = n.Next;
                    }
                    return (max - min) < 0.05; //���ꂭ�炢�����Ԃ�K��
                }
            }

            public int GetDivisionCount() {
                int c = 0;
                DivisionNode n = _first;
                while (n != null) {
                    if (n.IsLeaf)
                        c++;
                    else
                        c += n.ChildList.GetDivisionCount();
                    n = n.Next;
                }
                return c;
            }

            public DivisionNode FindPrevOf(DivisionNode target) {
                Debug.Assert(target != null);
                //Next��target�ł�����̂�T��
                DivisionNode node = _first;
                while (node != null && node.Next != target) {
                    node = node.Next;
                }
                return node;
            }

            public SplitFormat Format() {
                return new SplitFormat(_direction, _first.Format());
            }

            public IPane FindFirst(PaneCondition condition) {
                return _first.FindFirst(condition);
            }

            //�č\�z�ɐ旧���ăR���g���[���̐e�q�֌W�̉���
            public void ClearControlTree() {
                if (_hostingControl != null)
                    _hostingControl.Controls.Clear();
                _first.ClearControlTree();
            }

            //�����ɕ�����
            public void AdjustRatioEqually() {
                double ratio = 1.0 / this.NodeCount;
                DivisionNode n = _first;
                while (n != null) {
                    n.Ratio = ratio;
                    n = n.Next;
                }
            }

            public void ClearParentNode() {
                _parentNode = null;
            }

            //�T�C�Y�̒���
            public void AdjustSplitPosition(Size size) {
                _hostingControl.Size = size;
                _first.AdjustSplitPosition(size);
            }


            //�m�[�h�̍폜�@����͂�₱�����̂Ńh�L�������g���Q��
            public DivisionNode Remove(DivisionNode target) {
                DivisionNode result;
                bool equally_divided = this.IsEquallyDivided;

                if (target == _first) {
                    result = _first.Next;
                    _first = result;
                }
                else {
                    DivisionNode node = FindPrevOf(target);
                    Debug.Assert(node != null);

                    node.Next = target.Next;
                    result = node;
                }

                result.HostingControl.Size = AddSize(result.HostingControl.Size, target.HostingControl.Size);
                result.Ratio += target.Ratio;

                if (equally_divided)
                    AdjustRatioEqually();

                //���̎��_�ł͎c�肪��̉\��������B�Ăяo�������œK�؂ɑΏ�
                return result;
            }

            //src�̈ʒu���Alist�̒��g�Œu��������Bratio�����ɒ���
            public void ReplaceNodeByList(DivisionNode src, DivisionList list) {
                Debug.Assert(src.ParentList == this);
                int oldcount = this.NodeCount;
                int addedcount = list.NodeCount;
                double r = src.Ratio;

                DivisionNode t = list.FirstNode;
                DivisionNode last = null;
                while (t != null) {
                    t.Ratio *= r;
                    t.SetParentList(this);
                    last = t;
                    t = t.Next;
                }
                Debug.Assert(last.Next == null); //�����������
                ReplaceNode(src, list.FirstNode);
                last.Next = src.Next;

                Debug.Assert(oldcount + addedcount - 1 == this.NodeCount);
            }

            private void ReplaceNode(DivisionNode src, DivisionNode dest) {
                Debug.Assert(src.ParentList == this);
                dest.SetParentList(this);

                if (_first == src) {
                    _first = dest;
                }
                else {
                    DivisionNode n = _first;
                    while (n.Next != src) {
                        n = n.Next;
                        Debug.Assert(n != null);
                    }
                    n.Next = dest;
                }
            }
            private Size AddSize(Size s1, Size s2) {
                if (_direction == Direction.TB)
                    return new Size(Math.Max(s1.Width, s2.Width), s1.Height + PaneSplitter.SPLITTER_WIDTH + s2.Height);
                else
                    return new Size(s1.Width + PaneSplitter.SPLITTER_WIDTH + s2.Width, Math.Max(s1.Height, s2.Height));
            }

            //index�w��̔z�񂩂�IPane�擾�B�C���f�b�N�X�Ȃ����null
            public IPane GetPaneByIndices(int[] indices, int position) {
                int index = indices[position];
                DivisionNode node = _first;
                for (int i = 0; i < index; i++) {
                    node = node.Next;
                    if (node == null)
                        return null; //out of index
                }

                bool is_last = position == indices.Length - 1;
                if (is_last)
                    return node.Pane; //node������Ƀ��X�g��������null���Ԃ邱�Ƃɒ���
                else
                    return node.ChildList.GetPaneByIndices(indices, position + 1); //�q�̔z��
            }
        }

        // �R���e�i
        private class IntermediateContainer : ContainerControl {
            private PaneDivision _division;

            public IntermediateContainer(PaneDivision division) {
                _division = division;
            }
            public bool IsRoot {
                get {
                    return !(this.Parent is IntermediateContainer);
                }
            }

            protected override void OnResize(EventArgs e) {
                base.OnResize(e);
                if (this.IsRoot)
                    _division.OnContainerResize(this, e);
            }
        }

        //�O���ւ̒ʒm
        /// <summary>
        /// 
        /// </summary>
        /// <exclude/>
        public interface IUIActionHandler {
            void RequestUnify(IPane unify_target);
        }

        ////////////////////////////////////////////////////////////////////////////////////////
        //body of PaneDivision 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pane"></param>
        /// <returns></returns>
        /// <exclude/>
        public delegate bool PaneCondition(IPane pane);

        private int _count;             //���݂̕�����
        private int _countLimit;        //�������̏��
        private int _minimumEdgeLength; //�e�y�C���̊e�ӂ͍Œ�����̕���v����
        private DivisionList _rootList; //�������̃��X�g
        private IUIActionHandler _actionHandler;

        public PaneDivision() {
            _count = 1;
            _countLimit = 16;
            _minimumEdgeLength = 24;
        }

        public int PaneCount {
            get {
                return _count;
            }
        }
        public int CountLimit {
            get {
                return _countLimit;
            }
            set {
                _countLimit = value;
            }
        }
        public int MinimumEdgeLength {
            get {
                return _minimumEdgeLength;
            }
            set {
                _minimumEdgeLength = value;
            }
        }
        public bool IsEmpty {
            get {
                return _rootList == null;
            }
        }
        public Control RootControl {
            get {
                return _rootList == null ? null : _rootList.HostingControl;
            }
        }
        public IUIActionHandler UIActionHandler {
            get {
                return _actionHandler;
            }
            set {
                _actionHandler = value;
            }
        }


        //Split�̌���
        /// <summary>
        /// 
        /// </summary>
        /// <exclude/>
        public enum SplitResult {
            Success,
            F_TooManyPanes, //�������E
            F_TooSmallToSplit,
            F_UnifySingle
        }

        //index�̗�ňʒu���w�肵�Ẵ|�W�V�����擾
        public IPane GetPaneByIndices(int[] indices) {
            if (_rootList == null)
                return null;
            else
                return _rootList.GetPaneByIndices(indices, 0);
        }

        public string FormatSplit() {
            if (_rootList != null)
                return _rootList.Format().ToString();
            else
                return "";
        }

        //condition���ŏ���true��Ԃ���Pane��Ԃ��B�����Ȃ���null
        public IPane FindFirst(PaneCondition condition) {
            if (_rootList != null)
                return _rootList.FindFirst(condition);
            else
                return null;
        }


        //MinSize/Extra�̒���
        public void AdjustSplitMinSize() {
            _rootList.FirstNode.AdjustSplitMinSize();
        }

        //�T�C�Y���w�肵�Ē���
        internal void AdjustSplitPosition(Size size) {
            _rootList.AdjustSplitPosition(size);
        }

        //����̃T�C�Y�Œ���
        public void AdjustSplitPosition() {
            _rootList.AdjustSplitPosition(_rootList.HostingControl.Size);
        }

        //�e�q�֌W�̍\�z
        private void Rebuild() {
            _rootList.ClearControlTree();
            _rootList.FirstNode.BuildControlTree(_rootList.HostingControl);
        }

        //�T�C�Y����
        private void DoLayout() {
            AdjustSplitPosition(_rootList.HostingControl.Size);
            AdjustSplitMinSize();
        }

        //���T�C�Y IntermediateContainer#OnResize����Ă�
        private void OnContainerResize(object sender, EventArgs args) {
            if (_rootList == null)
                return; //�\�z���̌Ăяo���̓��V
            _rootList.HostingControl.SuspendLayout();
            AdjustSplitPosition(_rootList.HostingControl.Size);
            _rootList.HostingControl.ResumeLayout(true);
        }

        //�y�C���̕���
        public SplitResult SplitPane(IPane target, IPane newpane, Direction direction) {
            Debug.Assert(newpane.AsDotNet().Parent == null);

            //�����\���ǂ����̃`�F�b�N1 ����
            if (_count >= _countLimit)
                return SplitResult.F_TooManyPanes;

            //�����\���ǂ����̃`�F�b�N2 �����Ώۂ��ŏ��T�C�Y�𖞂����Ă��邩
            if (SizeToLength(target.Size, direction) < _minimumEdgeLength * 2 + PaneSplitter.SPLITTER_WIDTH)
                return SplitResult.F_TooSmallToSplit;

            Control parent = target.AsDotNet().Parent;
            bool splitting_root = _rootList == null;

            if (splitting_root) { //��̏�Ԃ���̍\�z
                _rootList = new DivisionList(this, null, direction, target, newpane, target.Size, target.Dock);
                UIUtil.ReplaceControl(parent, target.AsDotNet(), _rootList.HostingControl);
            }
            else {
                DivisionNode node = _rootList.FirstNode.FindNode(target);
                Debug.Assert(node != null);
                if (direction == node.ParentList.Direction) { //����������
                    bool eq = node.ParentList.IsEquallyDivided;
                    node.InsertNext(newpane);
                    if (eq)
                        node.ParentList.AdjustRatioEqually();
                }
                else { //�ٕ�������
                    DivisionList newlist = new DivisionList(this, node, direction, target, newpane, target.Size, target.Dock);
                    node.ReplacePaneByChildList(newlist);
                }
            }

            Rebuild();
            DoLayout();
            FindForm().MinimumSize = _rootList.FirstNode.RequiredMinimumSize; //!!TODO ����̓R���g���[���̃T�C�Y�ł���A�t�H�[���{�[�_�[�Ƃ͕ʂ̘b

            _count++;
            return SplitResult.Success;
        }

        //�y�C������Č�������Bout�̈����͎��Ƀt�H�[�J�X��^����ׂ��y�C��
        public SplitResult UnifyPane(IPane target, out IPane nextfocus) {
            nextfocus = null; //���s���ɂ̓N���A�ł���悤��
            Debug.Assert(_rootList != null);

            DivisionNode node = _rootList.FirstNode.FindNode(target);
            bool unifying_root = node.ParentList == _rootList && _rootList.NodeCount == 2; //�����̌���RootList������ւ��\��̂Ƃ�
            Control parent = _rootList.HostingControl.Parent;
            if (unifying_root && _rootList.FirstNode.IsLast)
                return SplitResult.F_UnifySingle;

            DivisionList list = node.ParentList;
            DivisionNode active = list.Remove(node);
            if (list.NodeCount == 1) { //�����Ȃ����Ƃ��ɖʓ|������
                if (active.IsLeaf) { // (1) �y�C���ł���Ƃ�
                    IPane newpane = active.Pane;
                    Debug.Assert(newpane != null);
                    if (list.ParentNode == null) { //1-1
                        UIUtil.ReplaceControl(parent, list.HostingControl, newpane.AsDotNet());
                        _rootList = null;
                    }
                    else { //1-2
                        list.ParentNode.ReplaceChildListByPane(newpane);
                    }
                }
                else { // (2) �m�[�h�ł���Ƃ�
                    DivisionList newlist = active.ChildList;
                    if (list.ParentNode == null) { //2-1 
                        _rootList = newlist;
                        newlist.ClearParentNode();
                        UIUtil.ReplaceControl(parent, list.HostingControl, newlist.HostingControl);
                    }
                    else { //2-2
                        DivisionList pp = list.ParentNode.ParentList; //�����Ȃ���̃��X�g
                        Debug.Assert(pp.Direction == newlist.Direction);
                        pp.ReplaceNodeByList(list.ParentNode, newlist);
                    }
                }

            }

            if (_rootList != null) {
                Rebuild();
                DoLayout();
                FindForm().MinimumSize = _rootList.FirstNode.RequiredMinimumSize; //!!TODO Splitter�����ō\������Ă��Ȃ��t�H�[���ŃA�E�g
            }

            _count--;
            nextfocus = active.IsLeaf ? active.Pane : active.ChildList.FirstPane;
            return SplitResult.Success;
        }
        public IPane UnifyAll() {
            if (_rootList == null)
                return null;

            IPane r = _rootList.FirstPane;
            Control parent = _rootList.HostingControl.Parent;
            UIUtil.ReplaceControl(parent, _rootList.HostingControl, r.AsDotNet());
            _rootList = null;
            _count = 1;
            return r;
        }

        public void ApplySplitInfo(Control parent, Control prev_content, string format, PaneCreationDelegate creation) {
            bool was_empty = this.IsEmpty;
            SplitFormat info = SplitFormat.Parse(format);
            _rootList = CreateDivisionList(info, creation, DockStyle.Fill);
            _count = _rootList.GetDivisionCount();
            Rebuild();

            if (prev_content == null)
                parent.Controls.Add(_rootList.HostingControl);
            else {
                Debug.Assert(prev_content.Parent == parent);
                UIUtil.ReplaceControl(parent, prev_content, _rootList.HostingControl);
            }

            DoLayout();
        }
        private DivisionList CreateDivisionList(SplitFormat info, PaneCreationDelegate creation, DockStyle host_dock) {
            DivisionList list = new DivisionList(this, info.Direction, host_dock);
            list.FirstNode = CreateDivisionNodeList(list, info, creation);
            return list;
        }
        private DivisionNode CreateDivisionNodeList(DivisionList list, SplitFormat info, PaneCreationDelegate creation) {
            SplitFormat.Node tag = info.FirstTag;
            DivisionNode firstnode = null;
            DivisionNode prev = null;
            double remain = 1.0;
            while (tag != null) {
                DivisionNode node = null;

                if (tag.Content != null) {
                    DockStyle dock = tag.Next == null ? (info.Direction == Direction.TB ? DockStyle.Bottom : DockStyle.Right) : DockStyle.Fill;
                    node = new DivisionNode(list, CreateDivisionList(tag.Content, creation, dock), tag.GetActualRatio(remain));
                }
                else {
                    node = new DivisionNode(list, creation(tag.Label), tag.GetActualRatio(remain));
                }
                remain -= tag.Ratio;

                if (firstnode == null)
                    firstnode = node;
                else
                    prev.Next = node;

                Debug.Assert(node.ParentList == list);
                prev = node;
                tag = tag.Next;
            }

            return firstnode;
        }

        public void DumpControlTree() {
            if (_rootList != null)
                UIUtil.DumpControlTree(_rootList.HostingControl);
        }

        private Form FindForm() {
            return _rootList.HostingControl.FindForm();
        }
    }

    //Split���ƃp�[�X
    //�����́A<H|V>( (<ratio>|L):(<INFO>), ... )
    internal class SplitFormat {
        private PaneDivision.Direction _direction;
        private Node _firstTag;

        public SplitFormat(PaneDivision.Direction dir, Node first) {
            _direction = dir;
            _firstTag = first;
        }

        //��R���X�g���N�^�̓p�[�X���̂�
        private SplitFormat() {
        }

        public Node FirstTag {
            get {
                return _firstTag;
            }
        }
        public PaneDivision.Direction Direction {
            get {
                return _direction;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exclude/>
        public class Node {
            public const double ALL_OF_REST = 0; //�c��S���A������ratio

            private double _ratio;
            private SplitFormat _content; //DivisionNode�ɑΉ�����Ƃ���null
            private string _label;
            private Node _next;

            public double Ratio {
                get {
                    return _ratio;
                }
            }
            public SplitFormat Content {
                get {
                    return _content;
                }
            }
            public string Label {
                get {
                    return _label;
                }
            }
            public Node Next {
                get {
                    return _next;
                }
                set {
                    _next = value;
                }
            }


            public Node(double ratio, SplitFormat content, string label, Node next) {
                _ratio = ratio;
                _content = content;
                _label = label;
                _next = next;
            }
            public Node() {
            }
            public double GetActualRatio(double remain) {
                if (_ratio == ALL_OF_REST)
                    return remain;
                else
                    return _ratio;
            }

            public void ToString(StringBuilder bld) {
                bld.Append(_ratio == ALL_OF_REST ? "L" : ((int)(_ratio * 100)).ToString());
                bld.Append(":");

                if (_label != null) {
                    bld.Append("L");
                    bld.Append(_label); //label�̓e�X�g�E�f�o�b�O��p
                }
                if (_content != null)
                    _content.ToString(bld);
            }

            public int Parse(char[] format, int index) {
                int colon = SplitFormat.FindChar(format, index, ':');
                string ratio = new string(format, index, colon - index);
                if (ratio == "L")
                    _ratio = ALL_OF_REST;
                else {
                    int r = Int32.Parse(ratio);
                    if (r <= 0 || r >= 100)
                        throw new FormatException("ratio range error");
                    _ratio = r / 100.0;
                }

                if (format[colon + 1] == 'L') { //label
                    int l = SplitFormat.FindChar2(format, colon + 2, ',', ')');
                    _label = new string(format, colon + 2, l - (colon + 2));
                    return l;
                }
                else { //split
                    _content = new SplitFormat();
                    return _content.Parse(format, colon + 1);
                }
            }
        }

        public override string ToString() {
            StringBuilder bld = new StringBuilder();
            ToString(bld);
            return bld.ToString();
        }
        public void ToString(StringBuilder bld) {
            bld.Append(_direction == PaneDivision.Direction.TB ? "H" : "V");
            bld.Append("(");
            Node t = _firstTag;
            while (t != null) {
                t.ToString(bld);
                t = t.Next;
                if (t != null)
                    bld.Append(",");
            }
            bld.Append(")");
        }

        public static SplitFormat Parse(string format) {
            char[] content = format.ToCharArray();
            SplitFormat info = new SplitFormat();
            int len = info.Parse(content, 0);
            if (len != content.Length)
                throw new FormatException();
            return info;
        }
        private int Parse(char[] format, int index) {
            char dir = format[index++];
            if (dir == 'H')
                _direction = PaneDivision.Direction.TB;
            else if (dir == 'V')
                _direction = PaneDivision.Direction.LR;
            else
                throw new FormatException();

            VerifyChar(format[index++], '(');

            Node prev = null;
            double remain_ratio = 1.0;
            do {
                Node t = new Node();
                index = t.Parse(format, index);
                remain_ratio -= t.Ratio;
                if (remain_ratio < 0)
                    throw new FormatException("ratio overflow");

                if (prev == null)
                    _firstTag = t;
                else
                    prev.Next = t;
                prev = t;

                if (format[index] == ')')
                    break; //end of content
                VerifyChar(format[index++], ',');
            } while (true);

            VerifyChar(format[index++], ')');
            return index;
        }

        //ch���݂��Ă��̈ʒu��Ԃ����A�����Ȃ��Η�O
        private static int FindChar(char[] content, int index, char ch) {
            while (content.Length > index) {
                if (content[index] == ch)
                    return index;
                index++;
            }

            throw new FormatException();
        }
        //�Q�����̂ǂ��炩��������o�[�W����
        private static int FindChar2(char[] content, int index, char ch1, char ch2) {
            while (content.Length > index) {
                if (content[index] == ch1 || content[index] == ch2)
                    return index;
                index++;
            }

            throw new FormatException();
        }

        private static void VerifyChar(char actual, char expected) {
            if (actual != expected)
                throw new FormatException();
        }
    }
}


