/*
 * Copyright 2004,2006 The Poderosa Project.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *
 * $Id: PoderosaStartup.cs,v 1.7 2011/12/23 19:15:15 kzmi Exp $
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Reflection;

using Poderosa.Plugins;

#if UNITTEST
using NUnit.Framework;
#endif

namespace Poderosa.Boot {

    //�u�[�g�p�̃G���g���|�C���g
    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public static class PoderosaStartup {

        // for compatibility
        public static IPoderosaApplication CreatePoderosaApplication(string[] args) {
            return CreatePoderosaApplication(args, false);
        }

        public static IPoderosaApplication CreatePoderosaApplication(string[] args, bool isMonolithic) {
            string home_directory = AppDomain.CurrentDomain.BaseDirectory;
            string preference_home = ResolveProfileDirectory("appdata");
            string open_file = null;

            PluginManifest pm;
            if (isMonolithic)
                pm = PluginManifest.CreateEmptyManifest();
            else
                pm = PluginManifest.CreateByFileSystem(home_directory);

            //�R�}���h���C��������ǂ�
            int i = 0;
            while (i < args.Length) {
                string t = args[i];
                string v = i < args.Length - 1 ? args[i + 1] : "";
                switch (t) {
                    case "-p":
                    case "--profile":
                        preference_home = ResolveProfileDirectory(v);
                        i += 2;
                        break;
                    case "-a":
                    case "--addasm":
                        pm.AddAssembly(home_directory, v.Split(';'));
                        i += 2;
                        break;
                    case "-r":
                    case "--remasm":
                        pm.RemoveAssembly(home_directory, v.Split(';'));
                        i += 2;
                        break;
                    case "-open":
                        open_file = v;
                        i += 2;
                        break;
                    default:
                        i++;
                        break;
                }
            }

            if (open_file != null && TryToSendOpenFileMessage(open_file))
                return null; //�ʃC���X�^���X�ɑ��M

            PoderosaStartupContext ctx = new PoderosaStartupContext(pm, home_directory, preference_home, args, open_file);
            return new InternalPoderosaWorld(ctx);
        }

        [Obsolete]
        public static IPoderosaApplication CreatePoderosaApplication(string plugin_manifest, string preference_home, string[] args) {
            string home_directory = Directory.GetCurrentDirectory();
            InternalPoderosaWorld w = new InternalPoderosaWorld(new PoderosaStartupContext(PluginManifest.CreateByText(plugin_manifest), home_directory, preference_home, args, null));
            return w;
        }

        [Obsolete]
        public static IPoderosaApplication CreatePoderosaApplication(string plugin_manifest, StructuredText preference, string[] args) {
            string home_directory = Directory.GetCurrentDirectory();
            InternalPoderosaWorld w = new InternalPoderosaWorld(new PoderosaStartupContext(PluginManifest.CreateByText(plugin_manifest), home_directory, preference, args, null));
            return w;
        }

        //����w��̃p�X���`�F�b�N
        private static string ResolveProfileDirectory(string value) {
            if (StringComparer.InvariantCultureIgnoreCase.Compare(value, "appdata") == 0)
                return ConfirmDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            if (StringComparer.InvariantCultureIgnoreCase.Compare(value, "commonappdata") == 0)
                return ConfirmDirectory(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData));
            if (StringComparer.InvariantCultureIgnoreCase.Compare(value, "bindir") == 0)
                return AppDomain.CurrentDomain.BaseDirectory;
            else
                return value;
        }
        private static string ConfirmDirectory(string dir) {
            string r = dir + "\\Poderosa";
            if (!Directory.Exists(r))
                Directory.CreateDirectory(r);
            return r;
        }

        //�ʃC���X�^���X�ւ̑��M�����݂�B�V���[�g�J�b�g���J�����Ƃ��̑��d�N���Ɋւ���Ƃ���ŁB
        private static bool TryToSendOpenFileMessage(string filename) {
            //�E�B���h�E��������
            unsafe {
                //find target
                IntPtr hwnd = Win32.FindWindowEx(IntPtr.Zero, IntPtr.Zero, null, null);
                char[] name = new char[256];
                char[] mf = new char[256];
                while (hwnd != IntPtr.Zero) {
                    int len = Win32.GetWindowText(hwnd, name, 256);
                    if (new string(name, 0, len).IndexOf("Poderosa") != -1) { //Window Class���m�F����Ƃ����Ƃ����ׂ������A����
                        if (TryToSendOpenFileMessage(hwnd, filename))
                            return true;
                    }
                    hwnd = Win32.FindWindowEx(IntPtr.Zero, hwnd, null, null);
                }

                return false;
            }
        }
        private unsafe static bool TryToSendOpenFileMessage(IntPtr hwnd, string filename) {
            char[] data = filename.ToCharArray();
            char* b = stackalloc char[data.Length + 1];
            for (int i = 0; i < data.Length; i++)
                b[i] = data[i];
            b[data.Length] = '\0';

            Win32.COPYDATASTRUCT cddata = new Win32.COPYDATASTRUCT();
            cddata.dwData = Win32.PODEROSA_OPEN_FILE_REQUEST;
            cddata.cbData = (uint)(sizeof(char) * (data.Length + 1));
            cddata.lpData = b;

            int lresult = Win32.SendMessage(hwnd, Win32.WM_COPYDATA, IntPtr.Zero, new IntPtr(&cddata));
            Debug.WriteLine("TryToSend " + lresult);
            return lresult == Win32.PODEROSA_OPEN_FILE_OK;
        }
    }


    //�N�����̃p�����[�^�@�R�}���h���C�������Ȃǂ���\�z
    internal class PoderosaStartupContext {
        private static PoderosaStartupContext _instance;
        private string _homeDirectory;
        private string _profileHomeDirectory;
        private string _preferenceFileName;
        private string _initialOpenFile;
        private PluginManifest _pluginManifest;
        private StructuredText _preferences;
        private string[] _args; //�N�����̃R�}���h���C������
        private ITracer _tracer; //�N�����̃G���[�̒ʒm��

        public static PoderosaStartupContext Instance {
            get {
                return _instance;
            }
        }

        public PoderosaStartupContext(PluginManifest pluginManifest, string home_directory, string profile_home, string[] args, string open_file) {
            _instance = this;
            _homeDirectory = AdjustDirectory(home_directory);
            _profileHomeDirectory = AdjustDirectory(profile_home);
            _initialOpenFile = open_file;
            _args = args;
            Debug.Assert(pluginManifest != null);
            _pluginManifest = pluginManifest;
            _preferenceFileName = Path.Combine(_profileHomeDirectory, "options.conf");
            _preferences = BuildPreference(_preferenceFileName);
        }
        public PoderosaStartupContext(PluginManifest pluginManifest, string home_directory, StructuredText preference, string[] args, string open_file) {
            _instance = this;
            _homeDirectory = AdjustDirectory(home_directory);
            _profileHomeDirectory = _homeDirectory;
            _initialOpenFile = open_file;
            _args = args;
            Debug.Assert(pluginManifest != null);
            _pluginManifest = pluginManifest;
            Debug.Assert(preference != null);
            _preferenceFileName = null;
            _preferences = preference;
        }
        private static string AdjustDirectory(string value) {
            return value.EndsWith("\\") ? value : value + "\\";
        }

        public PluginManifest PluginManifest {
            get {
                return _pluginManifest;
            }
        }
        public StructuredText Preferences {
            get {
                return _preferences;
            }
        }
        public string PreferenceFileName {
            get {
                return _preferenceFileName;
            }
        }
        public string HomeDirectory {
            get {
                return _homeDirectory;
            }
        }
        public string ProfileHomeDirectory {
            get {
                return _profileHomeDirectory;
            }
        }
        public string[] CommandLineArgs {
            get {
                return _args;
            }
        }

        //�ŏ��ɃI�[�v������t�@�C���B���w��Ȃ�null
        public string InitialOpenFile {
            get {
                return _initialOpenFile;
            }
        }



        public ITracer Tracer {
            get {
                return _tracer;
            }
            set {
                _tracer = value;
            }
        }

        private static StructuredText BuildPreference(string preference_file) {
            //TODO ��O���Ȃǂǂ����K���ɒʒm���K�v
            StructuredText pref = null;
            if (File.Exists(preference_file)) {
                using (TextReader r = new StreamReader(preference_file, Encoding.Default)) {
                    pref = new TextStructuredTextReader(r).Read();
                }
                // Note:
                //   if the file is empty or consists of empty lines,
                //   pref will be null.
            }

            if (pref == null)
                pref = new StructuredText("Poderosa");

            return pref;
        }

    }

    internal class PluginManifest {

        public class AssemblyEntry {
            public readonly string AssemblyName;

            private readonly List<string> _pluginTypes;

            public IEnumerable<string> PluginTypes {
                get {
                    return _pluginTypes;
                }
            }

            public int PluginTypeCount {
                get {
                    return _pluginTypes.Count;
                }
            }

            public AssemblyEntry(string assemblyName) {
                this.AssemblyName = assemblyName;
                this._pluginTypes = new List<string>();
            }

            public void AddPluginType(string name) {
                _pluginTypes.Add(name);
            }
        }

        private readonly List<AssemblyEntry> _entries = new List<AssemblyEntry>();

        //�O������̍쐬�֎~�B�ȉ���static���\�b�h�g�p�̂���
        private PluginManifest() {
        }

        public IEnumerable<AssemblyEntry> Entries {
            get {
                return _entries;
            }
        }

        public void AddAssembly(string home, string[] filenames) {
            foreach (string f in filenames) {
                AddAssembly(Path.Combine(home, f));
            }
        }

        public void RemoveAssembly(string home, string[] filenames) {
            foreach (String f in filenames) {
                string path = Path.Combine(home, f);
                List<AssemblyEntry> entriesToRemove = new List<AssemblyEntry>();
                foreach (AssemblyEntry entry in _entries) {
                    if (entry.AssemblyName == path) {
                        entriesToRemove.Add(entry);
                    }
                }
                foreach (AssemblyEntry entry in entriesToRemove) {
                    _entries.Remove(entry);
                }
            }
        }

        private AssemblyEntry AddAssembly(string name) {
            AssemblyEntry entry = new AssemblyEntry(name);
            _entries.Add(entry);
            return entry;
        }

        //������`������쐬
        public static PluginManifest CreateByText(string text) {
            PluginManifest m = new PluginManifest();

            StructuredText s = new TextStructuredTextReader(new StringReader(text)).Read();

            if (s.Name == "manifest") {
                foreach (object manifestChild in s.Children) {
                    StructuredText assemblyEntryNode = manifestChild as StructuredText;
                    if (assemblyEntryNode != null) {
                        PluginManifest.AssemblyEntry entry = m.AddAssembly(assemblyEntryNode.Name);
                        foreach(object assemblyEntryChild in assemblyEntryNode.Children) {
                            StructuredText.Entry pluginEntry = assemblyEntryChild as StructuredText.Entry;
                            if (pluginEntry != null && pluginEntry.name == "plugin") {
                                entry.AddPluginType(pluginEntry.value);
                            }
                        }
                    }
                }
            }

            return m;
        }

        //�t�@�C���V�X�e����ǂ�ō쐬
        public static PluginManifest CreateByFileSystem(string base_dir) {
            PluginManifest m = new PluginManifest();

            //�����̃f�B���N�g���ɂ���.dll�������B�A�v���P�[�V�����łł͕s�v�����A�J�����̃f�o�b�O���s���ɂ͕K�v
            string[] dlls = Directory.GetFiles(base_dir, "*.dll");
            foreach (string dll in dlls)
                m.AddAssembly(dll);

            //�q�f�B���N�g�������̂݌����B
            string[] dirs = Directory.GetDirectories(base_dir);
            foreach (string dir in dirs) {
                dlls = Directory.GetFiles(dir, "*.dll");
                foreach (string dll in dlls)
                    m.AddAssembly(dll);
            }

            return m;
        }

        public static PluginManifest CreateEmptyManifest() {
            PluginManifest m = new PluginManifest();
            return m;
        }
    }

#if UNITTEST
    [TestFixture]
    public class PluginManifestTests {

        private StringResource _stringResource;

        [TestFixtureSetUp]
        public void Init() {
            //Core.dll������͂�������Ƃ��Ȃ��ƃ��[�h�ł��Ȃ�
            Assembly.LoadFrom(String.Format("{0}\\Plugin.dll", PoderosaAppDir()));
            _stringResource = new StringResource("Plugin.strings", typeof(PluginManifest).Assembly);
        }

        [Test]
        public void Test1_DLLList() {
            PluginManifest pm = PluginManifest.CreateByFileSystem(PoderosaAppDir());
            TextWriter strm = new StringWriter();
            TextStructuredTextWriter wr = new TextStructuredTextWriter(strm);
            wr.Write(pm.RawData);
            strm.Close();
            UnitTestUtil.Trace(strm.ToString());
            //NOTE ����͂������ɖڎ������Ȃ���
        }

        [Test]
        public void Test2_NormalLoad() {
            ITracer tracer = CreateDefaultTracer();
            PluginManifest pm = PluginManifest.CreateByText(String.Format("manifest {{\r\n  {0}\\Core\\Core.dll {{\r\n plugin=Poderosa.Preferences.PreferencePlugin\r\n}}\r\n}}\r\n", PoderosaAppDir()));
            int count = 0;
            foreach (StructuredText t in pm.Children) {
                PluginManifest.AssemblyNode node = pm.LoadAssemblyNode(t);
                node.TryToBind(tracer);
                Assert.AreEqual(1, node.PluginTypes.Length); //����Ɏ��s����Ƃ��͌^��������Ȃ�
                Assert.AreEqual("Poderosa.Preferences.PreferencePlugin", node.PluginTypes[0].FullName);
                count++;
            }
            Assert.AreEqual(1, count); //�A�Z���u���w��͂P�����Ȃ��̂�
        }

        [Test]
        public void Test3_AssemblyLoadError() {
            ITracer tracer = CreateDefaultTracer();
            PluginManifest pm = PluginManifest.CreateByText(String.Format("manifest {{\r\n  {0}\\notexist.dll {{\r\n  }}\r\n}}\r\n", PoderosaAppDir()));
            try {
                foreach (StructuredText t in pm.Children) {
                    PluginManifest.AssemblyNode node = pm.LoadAssemblyNode(t);
                    Assert.Fail("we expect exception");
                }
            }
            catch (Exception ex) {
                tracer.Trace(ex);
                Console.Out.WriteLine(ex.Message);
            }
        }

        [Test]
        public void Test4_TypeNotFound() {
            ITracer tracer = CreateDefaultTracer();
            PluginManifest pm = PluginManifest.CreateByText(String.Format("manifest {{\r\n  {0}\\Core\\Core.dll {{\r\n plugin=NotFoundPlugin\r\n}}\r\n}}\r\n", PoderosaAppDir()));
            foreach (StructuredText t in pm.Children) {
                PluginManifest.AssemblyNode node = pm.LoadAssemblyNode(t);
                node.TryToBind(tracer);
                Assert.AreEqual(0, node.PluginTypes.Length);
                CheckOneErrorMessage(tracer.Document, String.Format(_stringResource.GetString("PluginManager.Messages.TypeLoadError"), node.Assembly.CodeBase, "NotFoundPlugin"));
            }
        }

        //NOTE
        // �{���͂����plugin=...�̋L�q���ȗ������`���e�X�g����ׂ������A���̂܂܂ł�PluginDeclarationAttribute��Poderosa.Monolithic.dll�̂��̂ɂȂ��Ă���
        // ������e�X�g�p�Ƀ��[�h����Plugin.dll���̂��̂��Q�Ƃ���悤�ɂ��Ȃ��ƃe�X�g�����s�ł����A����͂��Ȃ胀�Y���̂Œ��߂�B
        // �����r���h��Ԃ�Poderosa�������ƋN���ł��Ă���΂����̋@�\�͂����Ƃ��Ă���A�Ƃ݂Ȃ��B

        //�Ȃ��APluginManifet�ōs���̂�Type�����[�h����Ƃ���܂łŁA���ꂪ�����Ƃ����v���O�C���ł��邩�ǂ����̌�����PluginManager���s���B

        private string PoderosaAppDir() {
            return UnitTestUtil.GetUnitTestConfig("poderosa_installed_dir");
        }

        //PoderosaWorld���o�R���Ȃ��e�X�g�Ȃ̂ł���ŗ���
        private ITracer CreateDefaultTracer() {
            return new DefaultTracer(_stringResource);
        }

        private void CheckOneErrorMessage(TraceDocument doc, string msg) {
            string actual = doc.GetDataAt(0);
            if (actual != msg) {
                //���΂��Β����Ȃ�BDebug�ɏo���Ȃ��Ƃ킩��Â炢
                Debug.WriteLine("actual=" + actual);
            }
            Assert.AreEqual(msg, actual);
        }
    }
#endif
}
