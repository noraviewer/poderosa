/*
 * Copyright 2004,2006 The Poderosa Project.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *
 * $Id: ListenerList.cs,v 1.2 2011/10/27 23:21:55 kzmi Exp $
 */
using System;
using System.Collections;
using System.Collections.Generic;

namespace Poderosa.Util {
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <exclude/>
    public class ListenerList<T> : IEnumerable<T> {
        private LinkedList<T> _list;

        public ListenerList() {
        }
        public void Add(T listener) {
            Precheck();
            _list.AddLast(listener);
        }
        public void Remove(T listener) {
            Precheck();
            _list.Remove(listener);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            Precheck();
            return _list.GetEnumerator();
        }
        IEnumerator<T> IEnumerable<T>.GetEnumerator() {
            Precheck();
            return _list.GetEnumerator();
        }

        public bool IsEmpty {
            get {
                return _list != null && _list.Count == 0;
            }
        }

        public void Clear() {
            if (_list != null)
                _list.Clear();
        }

        //�����͈��Listener���o�^����Ȃ��B�x���쐬����
        private void Precheck() {
            if (_list == null)
                _list = new LinkedList<T>();
        }

    }

    //���X�i�̓o�^�E�폜�C���^�t�F�[�X
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <exclude/>
    public interface IListenerRegistration<T> {
        void AddListener(T listener);
        void RemoveListener(T listener);
    }
}
