using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Poderosa.Forms;
using Poderosa.Plugins;

[assembly: PluginDeclaration(typeof(Poderosa.TerminalControl.InvisibleModePlugin))]

namespace Poderosa.TerminalControl
{
	[PluginInfo(ID = "org.poderosa.core.window.invisibleMode", Version = "1.0", Author = "Luke Stratman", Dependencies = "org.poderosa.core.window")]
	internal class InvisibleModePlugin : PluginBase
	{
		public override void InitializePlugin(IPoderosaWorld poderosa)
		{
			base.InitializePlugin(poderosa);

			IWindowManager windowManager = (IWindowManager) poderosa.PluginManager.FindPlugin("org.poderosa.core.window", typeof (IWindowManager));
			
			windowManager.InvisibleMode = true;
			windowManager.StartMode = StartMode.Slave;
		}
	}
}
