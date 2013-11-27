using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Poderosa.Boot;

namespace Poderosa.TerminalControl
{
	public class EmptyTracer : ITracer
	{
		public EmptyTracer()
		{
			Document = new TraceDocument();
		}

		public void Trace(string string_id)
		{
		}

		public void Trace(string string_id, string param1)
		{
		}

		public void Trace(string string_id, string param1, string param2)
		{
		}

		public void Trace(Exception ex)
		{
		}

		public TraceDocument Document
		{
			get;
			private set;
		}
	}
}
