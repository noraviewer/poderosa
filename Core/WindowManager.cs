/*
 * Copyright 2004,2006 The Poderosa Project.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *
 * $Id: WindowManager.cs,v 1.3 2011/10/27 23:21:55 kzmi Exp $
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Globalization;

using Poderosa.Util;
using Poderosa.Plugins;
using Poderosa.Sessions;
using Poderosa.Preferences;
using Poderosa.View;
using Poderosa.Commands;

[assembly: PluginDeclaration(typeof(Poderosa.Forms.WindowManagerPlugin))]

namespace Poderosa.Forms {
    [PluginInfo(ID = WindowManagerPlugin.PLUGIN_ID, Version = VersionInfo.PODEROSA_VERSION, Author = VersionInfo.PROJECT_NAME, Dependencies = "org.poderosa.core.preferences;org.poderosa.core.commands")]
    internal class WindowManagerPlugin :
            PluginBase,
            IGUIMessageLoop,
            IWindowManager,
            IWinFormsService,
            ICultureChangeListener,
            IKeyBindChangeListener {
        public const string PLUGIN_ID = "org.poderosa.core.window";

        private List<MainWindow> _windows;
        private List<PopupViewContainer> _popupWindows;
        private MainWindow _activeWindow;
        private PoderosaAppContext _appContext;
        private MainWindowMenu _menu;
        private WindowPreference _preferences;
        private ViewFactoryManager _viewFactoryManager;

		private object _draggingObject;
        private SelectionService _selectionService;

        private bool _executingAllWindowClose;

        private static WindowManagerPlugin _instance;

	    private bool _invisibleMode;

        public static WindowManagerPlugin Instance {
            get {
                return _instance;
            }
        }


        public override void InitializePlugin(IPoderosaWorld poderosa) {
            base.InitializePlugin(poderosa);
            _instance = this;

#if UNITTEST
            StartMode =  StartMode.Slave;
#else
			//NOTE Preference����擾����Ȃǂ��ׂ���
			StartMode = StartMode.StandAlone;
#endif

            //Core�A�Z���u�����̃v���O�C�����\���Ă�����AdapterFactory���Z�b�g
            new CoreServices(poderosa);

            TabBar.Init();

            IPluginManager pm = poderosa.PluginManager;
            pm.FindExtensionPoint("org.poderosa.root").RegisterExtension(this);
            pm.CreateExtensionPoint(WindowManagerConstants.MAINWINDOWCONTENT_ID, typeof(IViewManagerFactory), this);
            pm.CreateExtensionPoint(WindowManagerConstants.VIEW_FACTORY_ID, typeof(IViewFactory), this);
            pm.CreateExtensionPoint(WindowManagerConstants.VIEWFORMATEVENTHANDLER_ID, typeof(IViewFormatEventHandler), this);
            pm.CreateExtensionPoint(WindowManagerConstants.TOOLBARCOMPONENT_ID, typeof(IToolBarComponent), this);
            pm.CreateExtensionPoint(WindowManagerConstants.MAINWINDOWEVENTHANDLER_ID, typeof(IMainWindowEventHandler), this);
            pm.CreateExtensionPoint(WindowManagerConstants.FILEDROPHANDLER_ID, typeof(IFileDropHandler), this);
            AboutBoxUtil.DefineExtensionPoint(pm);

            _preferences = new WindowPreference();
            pm.FindExtensionPoint(PreferencePlugin.EXTENSIONPOINT_NAME)
                .RegisterExtension(_preferences);
            pm.FindExtensionPoint(WindowManagerConstants.MAINWINDOWCONTENT_ID)
                .RegisterExtension(new DefaultViewManagerFactory());

            _windows = new List<MainWindow>();
            _popupWindows = new List<PopupViewContainer>();

            _menu = new MainWindowMenu();
            _selectionService = new SelectionService(this);
            _viewFactoryManager = new ViewFactoryManager();

            CommandManagerPlugin.Instance.AddKeyBindChangeListener(this);
            poderosa.Culture.AddChangeListener(this);
        }

        public void RunExtension() {
            try {
				_appContext = new PoderosaAppContext(StartMode);

                _poderosaWorld.Culture.SetCulture(CoreServicePreferenceAdapter.LangToCulture(_preferences.OriginalPreference.Language));
                MainWindowArgument[] args = MainWindowArgument.Parse(_preferences);
                foreach (MainWindowArgument arg in args)
                    _windows.Add(CreateMainWindow(arg));

                if (StartMode == StartMode.StandAlone) {
                    Application.Run(_appContext);
                    IPoderosaApplication app = (IPoderosaApplication)_poderosaWorld.GetAdapter(typeof(IPoderosaApplication));
                    app.Shutdown();
                }
            }
            catch (Exception ex) {
                RuntimeUtil.ReportException(ex);
            }
        }

        private MainWindow CreateMainWindow(MainWindowArgument arg) {
			if (InvisibleMode) {
				arg = new MainWindowArgument(arg.Location, FormWindowState.Minimized, arg.SplitInfo, arg.ToolBarInfo, arg.TabRowCount);
			}
			
			MainWindow w = new MainWindow(arg, _menu);
            w.Text = "Poderosa";
            w.FormClosed += new FormClosedEventHandler(WindowClosedHandler);
            w.Activated += delegate(object sender, EventArgs args) {
                _activeWindow = (MainWindow)sender; //�Ō�ɃA�N�e�B�u�ɂȂ������̂��w�肷��
            };

	        if (InvisibleMode) {
#if TERMCONTROL
                w.Opacity = 0;
#endif
		        w.WindowState = FormWindowState.Minimized;
		        w.ShowInTaskbar = false;
	        }

			w.Show();

	        return w;
        }

        public IPoderosaMainWindow CreateNewWindow(MainWindowArgument arg)
        {
	        MainWindow newWindow = CreateMainWindow(arg);
			_windows.Add(newWindow);

	        return newWindow;
        }

        //�A�v���I����
        public CommandResult CloseAllWindows() {
            try {
                _executingAllWindowClose = true;
                _preferences.WindowArray.Clear();
                //�R�s�[�̃R���N�V�����ɑ΂��Ď��s���Ȃ��Ƃ�����
                List<MainWindow> targets = new List<MainWindow>(_windows);
                foreach (MainWindow window in targets) {
                    CommandResult r = window.CancellableClose();
                    if (r != CommandResult.Succeeded)
                        return r; //�L�����Z�����ꂽ�ꍇ�͂����Œ��~
                    _preferences.FormatWindowPreference(window);
                }

                return CommandResult.Succeeded;
            }
            finally {
                _executingAllWindowClose = false;
            }
        }

        private void WindowClosedHandler(object sender, FormClosedEventArgs arg) {
            MainWindow w = (MainWindow)sender;
            if (!_executingAllWindowClose) { //�Ō�̃E�B���h�E�����ʂɕ���ꂽ�ꍇ
                _preferences.WindowArray.Clear();
                _preferences.FormatWindowPreference(w);
            }
            _windows.Remove(w);
            NotifyMainWindowUnloaded(w);
            if (_windows.Count == 0 && StartMode == StartMode.StandAlone) {
                CloseAllPopupWindows();
                _appContext.ExitThread();
            }
        }

        public override void TerminatePlugin() {
            base.TerminatePlugin();
            if (_windows.Count > 0) {
                CloseAllPopupWindows();
                MainWindow[] t = _windows.ToArray(); //�N���[�Y�C�x���g����_windows�̗v�f���ω�����̂Ń��[�J���R�s�[���K�v
                foreach (MainWindow w in t)
                    w.Close();
            }
        }

        public void InitializeExtension() {
        }


        #region IWindowManager
        public IPoderosaMainWindow[] MainWindows {
            get {
                return _windows.ToArray();
            }
        }
        public IPoderosaMainWindow ActiveWindow {
            get {
                return _activeWindow;
            }
        }
        public ISelectionService SelectionService {
            get {
                return _selectionService;
            }
        }
        public void ReloadMenu() {
            foreach (MainWindow w in _windows)
                w.ReloadMenu(_menu, true);
        }
        /*
        public void ReloadMenu(string extension_point_name) {
            MainMenuItem item = _menu.FindMainMenuItem(extension_point_name);
            if(item==null) throw new ArgumentException("extension point not found");
            foreach(MainWindow w in _windows) w.ReloadMenu(_menu, item);
        }
         */
        public void ReloadPreference(ICoreServicePreference pref) {
            foreach (MainWindow w in _windows)
                w.ReloadPreference(pref);
        }
        public void ReloadPreference() {
            //�f�t�H���g���g��
            ReloadPreference(_preferences.OriginalPreference);
        }

	    public bool InvisibleMode {
		    get {
				return _invisibleMode;
		    }
		    set {
				_invisibleMode = value;

				foreach (MainWindow w in _windows) {
					w.WindowState = FormWindowState.Minimized;
					w.ShowInTaskbar = false;
				}
		    }
	    }

	    //Popup�쐬
        public IPoderosaPopupWindow CreatePopupView(PopupViewCreationParam viewcreation) {
            PopupViewContainer vc = new PopupViewContainer(viewcreation);
            if (viewcreation.OwnedByCommandTargetWindow)
                vc.Owner = this.ActiveWindow.AsForm();
            vc.ShowInTaskbar = viewcreation.ShowInTaskBar;
            _popupWindows.Add(vc);
            vc.FormClosed += delegate(object sender, FormClosedEventArgs args) {
                _popupWindows.Remove((PopupViewContainer)sender);
            };

            return vc;
        }


        #endregion


        #region IKeyBindChangeListener
        public void OnKeyBindChanged(IKeyBinds newvalues) {
            foreach (MainWindow w in _windows)
                w.ReloadMenu(_menu, false);
        }
        #endregion


        #region ICultureChangeListener
        public void OnCultureChanged(CultureInfo newculture) {
            //���j���[�̃����[�h�܂ߑS�����
            CoreUtil.Strings.OnCultureChanged(newculture); //��Ƀ��\�[�X�X�V
            ReloadMenu();
        }
        #endregion

        public ITimerSite CreateTimer(int interval, TimerDelegate callback) {
            return new TimerSite(interval, callback);
        }
        public MainWindowMenu MainMenu {
            get {
                return _menu;
            }
        }
        public WindowPreference WindowPreference {
            get {
                return _preferences;
            }
        }
        public ViewFactoryManager ViewFactoryManager {
            get {
                return _viewFactoryManager;
            }
        }
        #region IWinFormsService
        public object GetDraggingObject(IDataObject data, Type required_type) {
            //TODO IDataObject�g��Ȃ��Ă����́H
            if (_draggingObject == null)
                return null;
            else {
                //TODO ���ꂿ����Ƃ�����������
                Debug.Assert(required_type == typeof(IPoderosaDocument));
                return ((TabBarManager.InternalTabKey)_draggingObject).PoderosaDocument;
            }
        }
        public void BypassDragEnter(Control target, DragEventArgs args) {
            ICommandTarget ct = CommandTargetUtil.AsCommandTarget(target as IAdaptable);
            if (ct == null)
                return;

            args.Effect = DragDropEffects.None;
            if (args.Data.GetDataPresent("FileDrop")) {
                string[] filenames = (string[])args.Data.GetData("FileDrop", true);
                IFileDropHandler[] hs = (IFileDropHandler[])_poderosaWorld.PluginManager.FindExtensionPoint(WindowManagerConstants.FILEDROPHANDLER_ID).GetExtensions();
                foreach (IFileDropHandler h in hs) {
                    if (h.CanAccept(ct, filenames)) {
                        args.Effect = DragDropEffects.Link;
                        return;
                    }
                }
            }
        }
        public void BypassDragDrop(Control target, DragEventArgs args) {
            ICommandTarget ct = CommandTargetUtil.AsCommandTarget(target as IAdaptable);
            if (ct == null)
                return;

            if (args.Data.GetDataPresent("FileDrop")) {
                string[] filenames = (string[])args.Data.GetData("FileDrop", true);
                IFileDropHandler[] hs = (IFileDropHandler[])_poderosaWorld.PluginManager.FindExtensionPoint(WindowManagerConstants.FILEDROPHANDLER_ID).GetExtensions();
                foreach (IFileDropHandler h in hs) {
                    if (h.CanAccept(ct, filenames)) {
                        h.DoDropAction(ct, filenames);
                        return;
                    }
                }
            }
        }
        //����̓C���^�t�F�[�X�����o�ł͂Ȃ��BMainWindow��WndProc��WM_COPYDATA��߂܂��ČĂԁB
        public void TurningOpenFile(ICommandTarget ct, string filename) {
            string[] filenames = new string[] { filename };
            IFileDropHandler[] hs = (IFileDropHandler[])_poderosaWorld.PluginManager.FindExtensionPoint(WindowManagerConstants.FILEDROPHANDLER_ID).GetExtensions();
            foreach (IFileDropHandler h in hs) {
                if (h.CanAccept(ct, filenames)) {
                    h.DoDropAction(ct, filenames);
                    return;
                }
            }
        }
        #endregion

        public void SetDraggingTabBar(TabKey value) {
            _draggingObject = value;
        }

		public StartMode StartMode {
			get;
			set;
		}

        private void CloseAllPopupWindows() {
            PopupViewContainer[] ws = _popupWindows.ToArray();
            foreach (PopupViewContainer w in ws)
                w.Close();
        }

        //�E�B���h�E�J�C�x���g�ʒm
        public void NotifyMainWindowLoaded(MainWindow w) {
            IMainWindowEventHandler[] hs = (IMainWindowEventHandler[])_poderosaWorld.PluginManager.FindExtensionPoint(WindowManagerConstants.MAINWINDOWEVENTHANDLER_ID).GetExtensions();
            foreach (IMainWindowEventHandler h in hs) {
                if (_windows.Count == 0)
                    h.OnFirstMainWindowLoaded(w);
                else
                    h.OnMainWindowLoaded(w);
            }
        }
        public void NotifyMainWindowUnloaded(MainWindow w) {
            IMainWindowEventHandler[] hs = (IMainWindowEventHandler[])_poderosaWorld.PluginManager.FindExtensionPoint(WindowManagerConstants.MAINWINDOWEVENTHANDLER_ID).GetExtensions();
            foreach (IMainWindowEventHandler h in hs) {
                if (_windows.Count == 0)
                    h.OnLastMainWindowUnloaded(w);
                else
                    h.OnMainWindowUnloaded(w);
            }
        }

    }

    internal class TimerSite : ITimerSite {
        private TimerDelegate _callback;
        private Timer _timer;

        public TimerSite(int interval, TimerDelegate callback) {
            _callback = callback;
            _timer = new Timer();
            _timer.Interval = interval;
            _timer.Tick += delegate(object sender, EventArgs ars) {
                try {
                    _callback();
                }
                catch (Exception ex) {
                    RuntimeUtil.ReportException(ex);
                }
            };
            _timer.Enabled = true;
        }

        public void Close() {
            _timer.Stop();
            _timer.Dispose();
        }
    }

    public class MainWindowArgument {
        private Rectangle _location;
        private FormWindowState _windowState;
        private string _splitInfo;
        private string _toolBarInfo;
        private int _tabRowCount;

        public MainWindowArgument(Rectangle location, FormWindowState state, string split, string toolbar, int tabrowcount) {
            _location = location;
            _windowState = state;
            _splitInfo = split;
            _toolBarInfo = toolbar;
            _tabRowCount = tabrowcount;
        }
        public string ToolBarInfo {
            get {
                return _toolBarInfo;
            }
        }
        public int TabRowCount {
            get {
                return _tabRowCount;
            }
        }
	    public Rectangle Location {
		    get {
			    return _location;
		    }
	    }
	    public FormWindowState WindowState {
		    get {
			    return _windowState;
		    }
	    }
	    public string SplitInfo {
		    get {
			    return _splitInfo;
		    }
	    }

        //�ʒu���̕ۑ��ƕ���
        //���̐��K�\���Ŏ������l�ŁB�� (Max,0,0,1024,768) �ʒu�ɕ��̒l���������Ƃɒ��ӁB
        public static MainWindowArgument[] Parse(IWindowPreference pref) {
            int count = pref.WindowCount;

            //�}�b�`���Ȃ��Ƃ��̓f�t�H���g
            if (count == 0) {
                //������Ԃōŏ����͋�����
                MainWindowArgument arg = new MainWindowArgument(GetInitialLocation(), FormWindowState.Normal, "", "", 1);
                return new MainWindowArgument[] { arg };
            }
            else {
                //���K�\�����̃R�����g: �\�[�X��\������t�H���g����ł͂�����������
                //                      (<FormWindowState>, left,      ,     top,         ,    width       ,     height   )  
                Regex re = new Regex("\\((Max,|Min,)?\\s*(-?[\\d]+)\\s*,\\s*(-?[\\d]+)\\s*,\\s*([\\d]+)\\s*,\\s*([\\d]+)\\)");

                MainWindowArgument[] result = new MainWindowArgument[count];
                for (int i = 0; i < count; i++) {
                    string positions = pref.WindowPositionAt(i);

                    Match m = re.Match(positions);
                    GroupCollection gc = m.Groups;
                    Debug.Assert(gc.Count == 6); //���g�Ǝq�v�f�T��
                    //�Ȃ��A�ŏ��������܂܏I�����Ă�����N�����̓m�[�}���T�C�Y�ŁB
                    result[i] = new MainWindowArgument(
                      ParseRectangle(gc[2].Value, gc[3].Value, gc[4].Value, gc[5].Value),
                      gc[1].Value == "Max," ? FormWindowState.Maximized : FormWindowState.Normal, //�J���}���ɒ���
                      pref.WindowSplitFormatAt(i), pref.ToolBarFormatAt(i), pref.TabRowCountAt(i));
                }
                return result;
            }
        }

        private static Rectangle ParseRectangle(string left, string top, string width, string height) {
            try {
                Rectangle r = new Rectangle(Int32.Parse(left), Int32.Parse(top), Int32.Parse(width), Int32.Parse(height));
                return r;
            }
            catch (FormatException) {
                return GetInitialLocation();
            }
        }

        private static Rectangle GetInitialLocation() {
            //�v���C�}���X�N���[���̔����̃T�C�Y�𒆉���
            Rectangle r = Screen.PrimaryScreen.Bounds;
            return new Rectangle(r.X + r.Width / 4, r.Y + r.Height / 4, r.Width / 2, r.Height / 2);
        }

    }

    internal class PoderosaAppContext : ApplicationContext {
        public PoderosaAppContext(StartMode startMode) {
	        if (startMode == StartMode.StandAlone) {
		        Application.EnableVisualStyles();
		        Application.SetCompatibleTextRenderingDefault(false);
		        Application.VisualStyleState = System.Windows.Forms.VisualStyles.VisualStyleState.ClientAndNonClientAreasEnabled;
	        }
        }

    }


    public enum StartMode {
        StandAlone,
        Slave
    }
}

namespace Poderosa {
    //���̃A�Z���u����StringResource�ւ̃A�N�Z�T WindowManager�ɑ�\������̂͂�����J���W
    internal static class CoreUtil {
        private static StringResource _strings;
        public static StringResource Strings {
            get {
                if (_strings == null)
                    _strings = new StringResource("Poderosa.Core.strings", typeof(CoreUtil).Assembly, true);
                return _strings;
            }
        }
    }
}
