/*
 * Copyright 2004,2006 The Poderosa Project.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *
 * $Id: DocActivationCommands.cs,v 1.2 2011/10/27 23:21:55 kzmi Exp $
 */
using System;
using System.Collections.Generic;
using System.Text;

using Poderosa.View;
using Poderosa.Forms;
using Poderosa.Sessions;

namespace Poderosa.Commands {
    //�E�B���h�E���j���[�̉��ɗ���A�e�h�L�������g���A�N�e�B�x�[�g����R�}���h
    internal class DocActivationCommand : IPoderosaCommand {
        public CommandResult InternalExecute(ICommandTarget target, params IAdaptable[] args) {
            IPoderosaDocument doc = (IPoderosaDocument)args[0].GetAdapter(typeof(IPoderosaDocument));
            if (doc == null)
                return CommandResult.Failed;

            SessionManagerPlugin.Instance.ActivateDocument(doc, ActivateReason.InternalAction);
            return CommandResult.Succeeded;
        }

        public bool CanExecute(ICommandTarget target) {
            return true;
        }

        public IAdaptable GetAdapter(Type adapter) {
            return CommandManagerPlugin.Instance.PoderosaWorld.AdapterManager.GetAdapter(this, adapter);
        }
    }

    internal class DocActivationMenuGroup : IPoderosaMenuGroup, IPositionDesignation {
        #region IPoderosaMenuGroup
        public IPoderosaMenu[] ChildMenus {
            get {
                IPoderosaMainWindow w = WindowManagerPlugin.Instance.ActiveWindow; //TODO�@�{���̓C�}�C�`�����A�����Ɉ�����CommandTarget�����Ă���̂��g��������
                if (w == null)
                    return new IPoderosaMenu[0];

                List<DocActivationMenuItem> result = new List<DocActivationMenuItem>();
                for (int i = 0; i < w.DocumentTabFeature.DocumentCount; i++)
                    result.Add(new DocActivationMenuItem(i, w, w.DocumentTabFeature.GetAtOrNull(i))); //foreach�g����悤�ɂ��ׂ�����
                return result.ToArray();
            }
        }

        public bool IsVolatileContent {
            get {
                return true;
            }
        }

        public bool ShowSeparator {
            get {
                return true;
            }
        }

        public IAdaptable GetAdapter(Type adapter) {
            return CommandManagerPlugin.Instance.PoderosaWorld.AdapterManager.GetAdapter(this, adapter);
        }
        #endregion

        #region IPositionDesignation
        //�������j���[�̎�
        public IAdaptable DesignationTarget {
            get {
                return null;
            }
        }

        public PositionType DesignationPosition {
            get {
                return PositionType.Last;
            }
        }
        #endregion
    }

    internal class DocActivationMenuItem : IPoderosaMenuItemWithArgs {
        private IPoderosaMainWindow _mainWindow;
        private IPoderosaDocument _document;
        private int _index;

        public DocActivationMenuItem(int index, IPoderosaMainWindow window, IPoderosaDocument document) {
            _mainWindow = window;
            _index = index;
            _document = document;
        }

        public IAdaptable[] AdditionalArgs {
            get {
                return new IAdaptable[] { _document };
            }
        }

        public IPoderosaCommand AssociatedCommand {
            get {
                return BasicCommandImplementation.DocActivationCommand;
            }
        }

        public string Text {
            get {
                //9�Ԗڂ܂łɂ�1...9�̃j�[���j�b�N
                return String.Format("{0}{1} {2}", _index < 9 ? "&" : "", _index + 1, _document.Caption);
            }
        }

        public bool IsEnabled(ICommandTarget target) {
            return true;
        }

        public bool IsChecked(ICommandTarget target) {
            return _mainWindow.DocumentTabFeature.ActiveDocument == _document;
        }

        public IAdaptable GetAdapter(Type adapter) {
            return CommandManagerPlugin.Instance.PoderosaWorld.AdapterManager.GetAdapter(this, adapter);
        }
    }
}
