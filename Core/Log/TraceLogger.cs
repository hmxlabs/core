using System;
using System.Diagnostics;

namespace HmxLabs.Core.Log
{
    /// <summary>
    /// An implementation of ILogger that just logs to the IDE.
    /// Useful when debugging locally.
    /// </summary>
    public class TraceLogger: LoggerBase
	{
        /// <summary>
        /// Convenience static method to get an instance and use the same one
        /// </summary>
        public static readonly ILogger Instance = new TraceLogger("TraceLoggerInstance");

		internal TraceLogger(string name_) : base(null, name_)
		{
		}

		internal TraceLogger(LoggerFactory factory_, string name_) : base(factory_, name_)
		{
		}

        /// <summary>
        /// No op
        /// </summary>
		public override void Open()
		{
		}

        /// <summary>
        /// See <code>LoggerBase.WriteLogLine</code>
        /// 
        /// Write out to Trace in the IDE
        /// </summary>
        /// <param name="exception_">The exception (if any) to log</param>
        /// <param name="level_">The log level to write at</param>
        /// <param name="content_">The log message</param>
        /// <param name="args_">Any additional data to include in the log</param>
	    protected override void WriteLogLine(Exception exception_, LogLevel level_, string content_, params object[] args_)
	    {
	        Trace.WriteLine(CreateLogLine(exception_, level_, content_, args_));
	    }
	}
}