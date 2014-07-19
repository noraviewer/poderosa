using System;
using Poderosa.Boot;

namespace Poderosa.TerminalControl
{
	/// <summary>
	/// ロードできないDLLに関する通知をユーザに見せないためのダミートレーサです。
	/// </summary>
    /// <remarks>
    /// ITracerインターフェースを継承していますが、実際の処理は何も行わないトレーサです。<br/>
    /// Luke Stratmanによる記述は以下の通りです。<br/>
    /// Fake tracer class designed to avoid notices about DLLs unable to be loaded being shown to 
    /// the user.
    /// </remarks>
	public class EmptyTracer : ITracer
	{
        /// <summary>
        /// トレースドキュメントです。
        /// </summary>
		public TraceDocument Document
		{
			get;
			private set;
		}

        /// <summary>
        /// コンストラクタです。
        /// </summary>
		public EmptyTracer()
		{
			Document = new TraceDocument();
		}

        /// <summary>
        /// トレースします。(実際には何も行いません。)
        /// </summary>
        /// <param name="string_id"></param>
		public void Trace(string string_id)
		{
		}

        /// <summary>
        /// トレースします。(実際には何も行いません。)
        /// </summary>
        /// <param name="string_id"></param>
        /// <param name="param1"></param>
		public void Trace(string string_id, string param1)
		{
		}

        /// <summary>
        /// トレースします。(実際には何も行いません。)
        /// </summary>
        /// <param name="string_id"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
		public void Trace(string string_id, string param1, string param2)
		{
		}

        /// <summary>
        /// トレースします。(実際には何も行いません。)
        /// </summary>
        /// <param name="ex"></param>
		public void Trace(Exception ex)
		{
		}
	}
}
