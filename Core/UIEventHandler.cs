/*
 * Copyright 2004,2006 The Poderosa Project.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *
 * $Id: UIEventHandler.cs,v 1.3 2011/12/17 09:49:44 kzmi Exp $
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

//�}�E�X�̃C�x���g�D��x�̊Ǘ��@�\
//�@.NET��OnMouseMove���R���g���[���̌p���֌W�Ŏ󂯂Ă��\���I�ɃL�r�V�C�̂�
namespace Poderosa.View {
    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public enum UIHandleResult {
        Pass,         //���̗D��x�̃n���h���ɓn��
        Stop,         //�������I������
        Capture,      //�������D�挠���l������
        EndCapture    //�D�挠���������
    }

    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public interface IUIHandler {
        string Name {
            get;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public interface IMouseHandler : IUIHandler {
        UIHandleResult OnMouseDown(MouseEventArgs args);
        UIHandleResult OnMouseMove(MouseEventArgs args);
        UIHandleResult OnMouseUp(MouseEventArgs args);
        UIHandleResult OnMouseWheel(MouseEventArgs args);
    }

    //ProcessCmdKey/ProcessDialogKey�̎��ӂɊւ��Ă̏������s��
    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public interface IKeyHandler : IUIHandler {
        UIHandleResult OnKeyProcess(Keys key);
    }

    //�����
    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public abstract class DefaultMouseHandler : IMouseHandler {
        private string _name;
        public DefaultMouseHandler(string name) {
            _name = name;
        }
        public string Name {
            get {
                return _name;
            }
        }

        public virtual UIHandleResult OnMouseDown(MouseEventArgs args) {
            return UIHandleResult.Pass;
        }

        public virtual UIHandleResult OnMouseMove(MouseEventArgs args) {
            return UIHandleResult.Pass;
        }

        public virtual UIHandleResult OnMouseUp(MouseEventArgs args) {
            return UIHandleResult.Pass;
        }

        public virtual UIHandleResult OnMouseWheel(MouseEventArgs args) {
            return UIHandleResult.Pass;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="HANDLER"></typeparam>
    /// <typeparam name="ARG"></typeparam>
    /// <exclude/>
    public abstract class UIHandlerManager<HANDLER, ARG> where HANDLER : class, IUIHandler {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <exclude/>
        public delegate UIHandleResult HandlerDelegate(HANDLER handler, ARG args);

        private List<HANDLER> _handlers; //�擪���ō��D��x
        private HANDLER _capturingHandler; //�C�x���g���L���v�`�����Ă���n���h���B���݂��Ȃ��Ƃ���null

        public UIHandlerManager() {
            _handlers = new List<HANDLER>();
        }
        public void AddLastHandler(HANDLER handler) {
            _handlers.Add(handler);
        }
        public void AddFirstHandler(HANDLER handler) {
            _handlers.Insert(0, handler);
        }
        //�O���̗v���ŃL���v�`�����������������Ƃ�����B
        public void EndCapture() {
            _capturingHandler = null;
        }
        public HANDLER CapturingHandler {
            get {
                return _capturingHandler;
            }
        }

        //WinForms�̃C�x���g�n���h���Ƃ̊֘A�t�� OnXXX����������override�������Ȃ��̂ŃC�x���g�n���h���ōs��
        public abstract void AttachControl(Control c);

        //�_���v
        public string DumpHandlerList() {
            StringBuilder bld = new StringBuilder();
            foreach (IUIHandler h in _handlers) {
                if (bld.Length > 0)
                    bld.Append(',');
                bld.Append(h.Name);
            }
            return bld.ToString();
        }

        //����̖{��
        protected UIHandleResult Process(HandlerDelegate action, ARG args) {
            try {
                if (_capturingHandler != null) {
                    UIHandleResult r = action(_capturingHandler, args);
                    if (r == UIHandleResult.EndCapture)
                        _capturingHandler = null; //�L���v�`���̏I��
                    return r;
                }
                else {
                    //����̏��ɂ܂킵�Ă���
                    foreach (HANDLER h in _handlers) {
                        UIHandleResult r = action(h, args);
                        Debug.Assert(r != UIHandleResult.EndCapture);
                        if (r == UIHandleResult.Stop)
                            return r;
                        if (r == UIHandleResult.Capture) {
                            Debug.Assert(_capturingHandler == null);
                            _capturingHandler = h;
                            return r;
                        }
                    }

                }
            }
            catch (Exception ex) {
                RuntimeUtil.ReportException(ex);
            }

            return UIHandleResult.Pass;
        }
    }

    //�n���h���̃}�l�[�W��
    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public class MouseHandlerManager : UIHandlerManager<IMouseHandler, MouseEventArgs> {

        //�����g�p�̃f���Q�[�g
        private static HandlerDelegate _mouseDownDelegate =
            delegate(IMouseHandler handler, MouseEventArgs args) {
                return handler.OnMouseDown(args);
            };
        private static HandlerDelegate _mouseUpDelegate =
            delegate(IMouseHandler handler, MouseEventArgs args) {
                return handler.OnMouseUp(args);
            };
        private static HandlerDelegate _mouseMoveDelegate =
            delegate(IMouseHandler handler, MouseEventArgs args) {
                return handler.OnMouseMove(args);
            };
        private static HandlerDelegate _mouseWheelDelegate =
            delegate(IMouseHandler handler, MouseEventArgs args) {
                return handler.OnMouseWheel(args);
            };

        public override void AttachControl(Control c) {
            c.MouseDown += new MouseEventHandler(RootMouseDown);
            c.MouseUp += new MouseEventHandler(RootMouseUp);
            c.MouseMove += new MouseEventHandler(RootMouseMove);
            c.MouseWheel += new MouseEventHandler(RootMouseWheel);
        }

        //WinForms�̃C�x���g�n���h��
        private void RootMouseDown(object sender, MouseEventArgs args) {
            Process(_mouseDownDelegate, args);
        }
        private void RootMouseUp(object sender, MouseEventArgs args) {
            Process(_mouseUpDelegate, args);
        }
        private void RootMouseMove(object sender, MouseEventArgs args) {
            Process(_mouseMoveDelegate, args);
        }
        private void RootMouseWheel(object sender, MouseEventArgs args) {
            Process(_mouseWheelDelegate, args);
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public class KeyboardHandlerManager : UIHandlerManager<IKeyHandler, Keys> {

        private static HandlerDelegate _keyDelegate =
            delegate(IKeyHandler handler, Keys key) {
                return handler.OnKeyProcess(key);
            };

        public override void AttachControl(Control c) {
            //ProcessDialogKey���C�x���g�Ŏ���Ƃ����񂾂��A����͂ł��Ȃ��̂ŋ����
        }

        //������O����Ăяo��
        public UIHandleResult Process(Keys key) {
            return base.Process(_keyDelegate, key);
        }
    }

}
