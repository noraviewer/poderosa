using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Poderosa.Boot;
using Poderosa.Plugins;

namespace TerminalControlTest
{
	static class Program
	{
		public static IPoderosaApplication PoderosaApplication;
		public static IPoderosaWorld PoderosaWorld;

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			PoderosaApplication = PoderosaStartup.CreatePoderosaApplication(new string[] {});
			PoderosaWorld = PoderosaApplication.Start();

			Application.Run(new Form1());
		}
	}
}
