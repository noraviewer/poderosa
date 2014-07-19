/*
 * Copyright 2004,2006 The Poderosa Project.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *
 * $Id: AboutBoxEx.cs,v 1.3 2011/10/27 23:21:55 kzmi Exp $
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;

using Poderosa.Preferences;
using Poderosa.Plugins;

namespace Poderosa.Forms {

    //�ꉞOEM�Ƃ����邩���ȁA�Ƃ����^�e�}�G���������炭�Q�o�����[�h��p�@�\�B���O���P������
    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public interface IPoderosaAboutBoxFactory {
        string AboutBoxID {
            get;
        }
        Form CreateAboutBox();
        Icon ApplicationIcon {
            get;
        }

        string EnterMessage {
            get;
        }
        string ExitMessage {
            get;
        }
    }

    //�e��AboutBox�����ʂ��Ďg���ł��낤�@�\
    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public static class AboutBoxUtil {
        public const string DEFAULT_ABOUTBOX_ID = "default";

        public static string[] GetVersionInfoContent() {
            string[] s = new string[6];
            s[0] = "Terminal Emulator <Poderosa>";
            s[1] = "Copyright(c) 2005,2006 Poderosa Project,";
            s[2] = "All Rights Reserved.";
            s[3] = "";
            s[4] = " Version : " + VersionInfo.PODEROSA_VERSION;
            s[5] = " CLR     : " + System.Environment.Version.ToString();
            return s;
        }

        //ExtensionPoint��Preference�B�ʃN���X�ɕ�������WindowManager�̃����o�ɓ���悤���ȁH
        private static IStringPreferenceItem _aboutBoxID;
        public static IStringPreferenceItem AboutBoxID {
            get {
                return _aboutBoxID;
            }
        }
        internal static void DefineExtensionPoint(IPluginManager pm) {
            IExtensionPoint pt = pm.CreateExtensionPoint("org.poderosa.window.aboutbox", typeof(IPoderosaAboutBoxFactory), WindowManagerPlugin.Instance);
            pt.RegisterExtension(new DefaultAboutBoxFactory());
        }
        internal static void InitPreference(IPreferenceBuilder builder, IPreferenceFolder window_root) {
            _aboutBoxID = builder.DefineStringValue(window_root, "aboutBoxFactoryID", "default", null);
        }
        public static IPoderosaAboutBoxFactory GetCurrentAboutBoxFactory() {
            //AboutBox������������
            if (AboutBoxUtil.AboutBoxID == null)
                return null;
            string factory_id = AboutBoxUtil.AboutBoxID.Value;
            IPoderosaAboutBoxFactory found_factory = null;
            IPoderosaAboutBoxFactory[] factories = (IPoderosaAboutBoxFactory[])WindowManagerPlugin.Instance.PoderosaWorld.PluginManager.FindExtensionPoint("org.poderosa.window.aboutbox").GetExtensions();
            foreach (IPoderosaAboutBoxFactory f in factories) {
                if (f.AboutBoxID == factory_id) {
                    found_factory = f;
                    break;
                }
                else if (f.AboutBoxID == DEFAULT_ABOUTBOX_ID) { //TODO �����Ƃ���const string�Q��
                    found_factory = f; //���̂��Ƃ̃��[�v�Ő����Ɉ�v����������������㏑������邱�Ƃɒ���
                }
            }

            Debug.Assert(found_factory != null);
            return found_factory;
        }

        private static StringBuilder _keyBufferInAboutBox;

        public static void ResetKeyBufferInAboutBox() {
            _keyBufferInAboutBox = new StringBuilder();
        }
        //�_�C�A���O�{�b�N�X���ł̃L�[���͏���
        public static bool ProcessDialogChar(char charCode) {
            if ('A' <= charCode && charCode <= 'Z')
                charCode = (char)('a' + charCode - 'A');
            _keyBufferInAboutBox.Append(charCode);
            string t = _keyBufferInAboutBox.ToString();
            IPoderosaAboutBoxFactory[] factories = (IPoderosaAboutBoxFactory[])WindowManagerPlugin.Instance.PoderosaWorld.PluginManager.FindExtensionPoint("org.poderosa.window.aboutbox").GetExtensions();
            foreach (IPoderosaAboutBoxFactory f in factories) {
                if (t == f.AboutBoxID) {
                    if (_aboutBoxID.Value == f.AboutBoxID) { //���Z�b�g
                        MessageBox.Show(f.ExitMessage, "Poderosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        _aboutBoxID.Value = DEFAULT_ABOUTBOX_ID;
                    }
                    else { //���[�h�ύX
                        MessageBox.Show(f.EnterMessage, "Poderosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        _aboutBoxID.Value = f.AboutBoxID;
                    }

                    WindowManagerPlugin.Instance.ReloadPreference(WindowManagerPlugin.Instance.WindowPreference.OriginalPreference);
                    return true;
                }
            }

            return false;

        }
    }
}
