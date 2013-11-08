using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Forms;

namespace TerminalControlTest
{
	public partial class Form1 : Form
	{
		protected TerminalControl _terminalControl;

		public Form1()
		{
			InitializeComponent();

			_terminalControl = new TerminalControl
				                   {
					                   Font = new Font("Consolas", 14),
					                   ForeColor = Color.LightGray,
					                   BackColor = Color.Black,
					                   HostName = "localhost",
					                   Password = new SecureString(),
					                   Username = "luke.stratman",
									   Dock = DockStyle.Fill,
									   TerminalType = TerminalType.XTerm
				                   };

			foreach (char character in "TifLBd1B,0BA")
				_terminalControl.Password.AppendChar(character);

			Controls.Add(_terminalControl);
		}

		protected override void OnLoad(System.EventArgs e)
		{
			base.OnLoad(e);

			_terminalControl.AsyncConnect();
		}
	}
}
