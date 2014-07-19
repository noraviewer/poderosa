using System;
using System.Configuration;
using System.IO;
using System.Windows.Forms;
using Poderosa.Boot;
using Poderosa.Forms;
using Poderosa.Plugins;
using Poderosa.View;

namespace Poderosa.TerminalControl
{
    /// <summary>
    /// Poderosaシステムへのアクセスを管理するためのクラスです。
    /// </summary>
    public class PoderosaAccessPoint
    {
        /// <summary>
        /// プリファレンス情報が配置されるディレクトリパスです。
        /// </summary>
        private static string _preferenceDir;
        
        /// <summary>
        /// IPoderosaApplicationはIPoderosaWorldを形成するための起動パラメータを持ちます。
        /// </summary>
        private static IPoderosaApplication _poderosaApplication;
        
        /// <summary>
        /// IPoderosaWorldはPoderosa本体を示します。
        /// </summary>
        private static IPoderosaWorld _poderosaWorld;

        /// <summary>
        /// プリファレンス情報が配置されるディレクトリパスです。
        /// </summary>
        public static string PreferenceDir
        {
            get { return _preferenceDir; }
            set
            {
                if (String.IsNullOrEmpty(_preferenceDir))
                {
                    _preferenceDir = value;
                }
                else
                {
                    throw new InvalidOperationException("PreferenceDirは既に設定されています。");
                }
            }
        }

        /// <summary>
        /// プラグインからPoderosa本体と通信するためのインターフェースです。
        /// </summary>
        public static IPoderosaWorld World
        {
            get
            {
                Initialize();
                return _poderosaWorld;
            }
        }

        /// <summary>
        /// デフォルト値で構成された表示プロファイルを取得します。
        /// </summary>
        /// <returns></returns>
        public static RenderProfile GetDefaultRenderProfile()
        {
            Initialize();
            return DirtyAccessUtility.GetDefaultRenderProfile();
        }

        /// <summary>
        /// 表示プロファイル編集画面を呼び出します。
        /// </summary>
        /// <param name="profile"></param>
        /// <returns>キャンセルされた場合はnull</returns>
        public static RenderProfile CallEditRenderProfile(RenderProfile profile)
        {
            Initialize();
            return DirtyAccessUtility.CallEditRenderProfile(profile);
        }

        /// <summary>
        /// Poderosa環境へのアクセスを可能にするための初期化を行います。
        /// </summary>
        private static void Initialize()
        {
            if (_poderosaWorld == null)
            {
                //
                // options.confファイルが存在すべきディレクトリが未決定であれば設定します。
                //
                if (String.IsNullOrEmpty(_preferenceDir))
                {
                    _preferenceDir = Path.Combine(Application.LocalUserAppDataPath, "Poderosa");
                }

                //
                // options.confファイルが存在すべきディレクトリが存在しない場合は作成します。
                //
                if (!Directory.Exists(_preferenceDir))
                {
                    Directory.CreateDirectory(_preferenceDir);
                }

                //
                // options.confファイルの存在をチェックします。
                // ※ 存在しない場合はリソースResourcesからoption.confファイルを作成します。
                //
                string optionsConfPath = Path.Combine(_preferenceDir, "options.conf");
                if (!File.Exists(optionsConfPath))
                {
                    string memoryString =
                        global::Poderosa.TerminalControl.Properties.Resources.PoderosaOptionsConf;
                    StreamWriter writer =
                        new StreamWriter(optionsConfPath, false, System.Text.Encoding.UTF8);
                    writer.Write(memoryString);
                    writer.Close();
                }

                //
                // PoderosaアプリケーションとPoderosaワールドを作成します。
                //
                string[] args = new string[2];
                args[0] = "--profile";
                args[1] = _preferenceDir;
                _poderosaApplication = PoderosaStartup.CreatePoderosaApplication(args);
                _poderosaWorld = _poderosaApplication.Start(new EmptyTracer());
            }
        }
    }
}
