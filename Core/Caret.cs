/*
 * Copyright 2004,2006 The Poderosa Project.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *
 * $Id: Caret.cs,v 1.2 2011/10/27 23:21:55 kzmi Exp $
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

using Poderosa.Util;
using Poderosa.Forms;

namespace Poderosa.View {

    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public enum CaretType {
        Line = 0,
        Box = 2,
        Underline = 4,
        StyleMask = Box | Underline,
    }

    //Caret�̍��W�Ə�Ԃ����^
    /// <summary>
    /// <ja>
    /// �L�����b�g�̍��W�Ə�Ԃ��i�[����I�u�W�F�N�g�ł��B
    /// </ja>
    /// <en>
    /// Object that stores coordinates and state of caret
    /// </en>
    /// </summary>
    /// <exclude/>
    public class Caret {
        private const int TICKER_LOOP_INTERVAL = 2;

        private CaretType _style; //Line, Box, Underline�̂����ꂩ
        private Color _color;
        private int _x; //�����P�ʂł̍��W
        private int _y;
        private int _tick; //��莞�Ԗ��̐؂�ւ��
        private bool _enabled;
        private bool _blink;
        private Pen _pen;

        public Caret() {
            _style = CaretType.Box;
            _color = Color.Empty;
            _enabled = false;
            _blink = true;
        }

        public CaretType Style {
            get {
                return _style;
            }
            set {
                _style = value & CaretType.StyleMask;
            }
        }
        public int X {
            get {
                return _x;
            }
            set {
                _x = value;
            }
        }
        public int Y {
            get {
                return _y;
            }
            set {
                _y = value;
            }
        }
        public bool Enabled {
            get {
                return _enabled;
            }
            set {
                _enabled = value;
            }
        }
        public bool Blink {
            get {
                return _blink;
            }
            set {
                _blink = value;
            }
        }
        public Color Color {
            get {
                return _color;
            }
            set {
                _color = value;
                DisposePen();
            }
        }


        public bool IsActiveTick {
            get {
                return _tick <= 0;
            }
        }
        public void Tick() {
            if (++_tick == TICKER_LOOP_INTERVAL)
                _tick = 0;
        }
        public void KeepActiveUntilNextTick() {
            //TODO �^�C�}�[�̃��Z�b�g�܂łł���Ƃ悢
            _tick = -1;
        }
        public void Reset() {
            DisposePen();
        }

        //Pen
        internal Pen ToPen(RenderProfile p) {
            if (_pen == null) {
                _pen = new Pen(_color == Color.Empty ? p.ForeColor : _color);
            }
            return _pen;
        }

        internal void Dispose() {
            DisposePen();
        }

        private void DisposePen() {
            if (_pen != null) {
                _pen.Dispose(); //�y���̃Z�b�g�Ń��Z�b�g
                _pen = null;
            }
        }


    }
}
