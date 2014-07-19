using Poderosa.Forms;
using Poderosa.Plugins;

[assembly: PluginDeclaration(typeof(Poderosa.TerminalControl.InvisibleModePlugin))]

namespace Poderosa.TerminalControl
{
	/// <summary>
    /// InvisibleModePluginはPoderosaアプリケーションを非表示モード(メインウィンドウを表示しない)
    /// にするためのプラグインです。
	/// </summary>
    /// <remarks>
    /// Luke Stratmanによる記述は以下の通りです。<br/>
	/// Poderosa plugin that will turn on invisible mode (main window is not shown) for the 
    /// Poderosa application.
    /// </remarks>
	[PluginInfo(ID = "org.poderosa.core.window.invisibleMode", Version = "1.0", 
        Author = "Luke Stratman", Dependencies = "org.poderosa.core.window")]
	internal class InvisibleModePlugin : PluginBase
	{
		/// <summary>
        /// プラグインを初期化します。
		/// </summary>
        /// <remarks>
        /// Luke Stratmanによる記述は以下の通りです。<br/>
		/// Called when the plugin is initialized, it gets <see cref="IWindowManager"/> and sets 
        /// its <see cref="IWindowManager.InvisibleMode"/> property to true and its 
        /// <see cref="IWindowManager.StartMode"/> property to <see cref="StartMode.Slave"/>.
        /// </remarks>
		/// <param name="poderosa">アプリケーションのためのIPoderosaWorldインターフェース</param>
		public override void InitializePlugin(IPoderosaWorld poderosa)
		{
			base.InitializePlugin(poderosa);

			IWindowManager windowManager = (IWindowManager) poderosa.PluginManager.FindPlugin(
                "org.poderosa.core.window", typeof (IWindowManager));
			
			windowManager.InvisibleMode = true;
			windowManager.StartMode = StartMode.Slave;
		}
	}
}
