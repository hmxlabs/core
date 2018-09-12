using System;
using System.Text;
using HmxLabs.Core.DateTIme;

namespace HmxLabs.Core.Log
{
	/// <summary>
	/// A standardised looging interface which can wrap your favourite logging implementation.
	/// 
	/// This is useful because it allows all HMx Labs code libs to use the same logging interface
	/// without imposing a choice of which logging mechanism to use on the application. This
	/// means that whichever logging mechansim the application chooses to use, all of the log
	/// outtput from the entire HMx Labs code stack will be in the same place
	/// 
	/// Additionally it means that your log output becomes testable. Yes that's right
	/// we have unit test cases for failure where we also check that the failure was logged.
	/// Proviing a test implementation or a mock of this interface means that such testing
	/// becomes trivial.
	/// 
	/// The log levels supported, in ascending order of severity are:
	/// <code>Debug</code>, <code>Info</code>, <code>Notice</code>, <code>Warning</code>,
	/// <code>Error</code>, <code>Critical</code>, <code>Fatal</code>
	/// </summary>
	public interface ILogger : IDisposable
	{
		/// <summary>
		/// The Name of this logger. Useful if more that one logger is used within
		/// the codebase
		/// </summary>
		string Name { get; }

        /// <summary>
        /// The time provider to use in order to timestamp the log messages.
        /// When serialised an ISO serialisation of this timestamp will always be used.
        /// </summary>
        ITimeProvider TimeProvider { get; set; }

        /// <summary>
        /// The encoding to use when serialising out the log
        /// </summary>
        Encoding LogEncoding { get; set; }

        /// <summary>
        /// Open/ start the logging mechanism. For file based implementations this
        /// might mean creating the file for example
        /// </summary>
		void Open();

        /// <summary>
        /// Write a debug level log statement with the text string provided
        /// </summary>
        /// <param name="logLine_">The text information to log</param>
        void Debug(string logLine_);

        /// <summary>
        /// Write a debug level log statement with the text string provided and also
        /// serialises out the objects in the parameter array
        /// </summary>
        /// <param name="logLine_">The text information to log</param>
        /// <param name="args_">The additional parameters to log</param>
        void Debug(string logLine_, params object[] args_);

        /// <summary>
        /// Writes a debug level log statemnet with the text string and the exception
        /// details provided.
        /// 
        /// The exception is logged on a new line and contains the complete message
        /// and stack trace
        /// </summary>
        /// <param name="exception_">The exception to log</param>
        /// <param name="logLine_">The text log line</param>
        void Debug(Exception exception_, string logLine_);

        /// <summary>
        /// Writes a debug level log statement with the text string, the exception
        /// and the additional parameters provided`s
        /// </summary>
        /// <param name="exception_">The exception to log</param>
        /// <param name="logLine_">The text log line</param>
        /// <param name="args_">The additional parameters to log</param>
        void Debug(Exception exception_, string logLine_, params object[] args_);

        /// <summary>
        /// Write an info level log statement with the text string provided
        /// </summary>
        /// <param name="logLine_">The text information to log</param>
		void Info(string logLine_);

        /// <summary>
        /// Write an info level log statement with the text string provided and also
        /// serialises out the objects in the parameter array
        /// </summary>
        /// <param name="logLine_">The text information to log</param>
        /// <param name="args_">The additional parameters to log</param>
        void Info(string logLine_, params object[] args_);

        /// <summary>
        /// Writes an info level log statemnet with the text string and the exception
        /// details provided.
        /// 
        /// The exception is logged on a new line and contains the complete message
        /// and stack trace
        /// </summary>
        /// <param name="exception_">The exception to log</param>
        /// <param name="logLine_">The text log line</param>
        void Info(Exception exception_, string logLine_);

        /// <summary>
        /// Writes an info level log statement with the text string, the exception
        /// and the additional parameters provided`s
        /// </summary>
        /// <param name="exception_">The exception to log</param>
        /// <param name="logLine_">The text log line</param>
        /// <param name="args_">The additional parameters to log</param>
        void Info(Exception exception_, string logLine_, params object[] args_);

        /// <summary>
        /// Write a notice level log statement with the text string provided
        /// </summary>
        /// <param name="logLine_">The text information to log</param>
        void Notice(string logLine_);
        
        /// <summary>
        /// Write a notice level log statement with the text string provided and also
        /// serialises out the objects in the parameter array
        /// </summary>
        /// <param name="logLine_">The text information to log</param>
        /// <param name="args_">The additional parameters to log</param>
        void Notice(string logLine_, params object[] args_);

        /// <summary>
        /// Writes a notice level log statemnet with the text string and the exception
        /// details provided.
        /// 
        /// The exception is logged on a new line and contains the complete message
        /// and stack trace
        /// </summary>
        /// <param name="exception_">The exception to log</param>
        /// <param name="logLine_">The text log line</param>
        void Notice(Exception exception_, string logLine_);

        /// <summary>
        /// Writes a notice level log statement with the text string, the exception
        /// and the additional parameters provided`s
        /// </summary>
        /// <param name="exception_">The exception to log</param>
        /// <param name="logLine_">The text log line</param>
        /// <param name="args_">The additional parameters to log</param>
        void Notice(Exception exception_, string logLine_, params object[] args_);

        /// <summary>
        /// Write a warning level log statement with the text string provided
        /// </summary>
        /// <param name="logLine_">The text information to log</param>
		void Warning(string logLine_);

        /// <summary>
        /// Write a warning level log statement with the text string provided and also
        /// serialises out the objects in the parameter array
        /// </summary>
        /// <param name="logLine_">The text information to log</param>
        /// <param name="args_">The additional parameters to log</param>
        void Warning(string logLine_, params object[] args_);

        /// <summary>
        /// Writes a warning level log statemnet with the text string and the exception
        /// details provided.
        /// 
        /// The exception is logged on a new line and contains the complete message
        /// and stack trace
        /// </summary>
        /// <param name="exception_">The exception to log</param>
        /// <param name="logLine_">The text log line</param>
        void Warning(Exception exception_, string logLine_);

        /// <summary>
        /// Writes a notice level log statement with the text string, the exception
        /// and the additional parameters provided`s
        /// </summary>
        /// <param name="exception_">The exception to log</param>
        /// <param name="logLine_">The text log line</param>
        /// <param name="args_">The additional parameters to log</param>
        void Warning(Exception exception_, string logLine_, params object[] args_);

        /// <summary>
        /// Write an error level log statement with the text string provided
        /// </summary>
        /// <param name="logLine_">The text information to log</param>
		void Error(string logLine_);

        /// <summary>
        /// Write an error level log statement with the text string provided and also
        /// serialises out the objects in the parameter array
        /// </summary>
        /// <param name="logLine_">The text information to log</param>
        /// <param name="args_">The additional parameters to log</param>
        void Error(string logLine_, params object[] args_);

        /// <summary>
        /// Writes an error level log statemnet with the text string and the exception
        /// details provided.
        /// 
        /// The exception is logged on a new line and contains the complete message
        /// and stack trace
        /// </summary>
        /// <param name="exception_">The exception to log</param>
        /// <param name="logLine_">The text log line</param>
        void Error(Exception exception_, string logLine_);

        /// <summary>
        /// Writes an error level log statement with the text string, the exception
        /// and the additional parameters provided`s
        /// </summary>
        /// <param name="exception_">The exception to log</param>
        /// <param name="logLine_">The text log line</param>
        /// <param name="args_">The additional parameters to log</param>
        void Error(Exception exception_, string logLine_, params object[] args_);

        /// <summary>
        /// Write a critical level log statement with the text string provided
        /// </summary>
        /// <param name="logLine_">The text information to log</param>
        void Critical(string logLine_);

        /// <summary>
        /// Write a critical level log statement with the text string provided and also
        /// serialises out the objects in the parameter array
        /// </summary>
        /// <param name="logLine_">The text information to log</param>
        /// <param name="args_">The additional parameters to log</param>
        void Critical(string logLine_, params object[] args_);

        /// <summary>
        /// Writes a critical level log statemnet with the text string and the exception
        /// details provided.
        /// 
        /// The exception is logged on a new line and contains the complete message
        /// and stack trace
        /// </summary>
        /// <param name="exception_">The exception to log</param>
        /// <param name="logLine_">The text log line</param>
        void Critical(Exception exception_, string logLine_);

        /// <summary>
        /// Writes a critical level log statement with the text string, the exception
        /// and the additional parameters provided`s
        /// </summary>
        /// <param name="exception_">The exception to log</param>
        /// <param name="logLine_">The text log line</param>
        /// <param name="args_">The additional parameters to log</param>
        void Critical(Exception exception_, string logLine_, params object[] args_);

        /// <summary>
        /// Write a fatal level log statement with the text string provided
        /// </summary>
        /// <param name="logLine_">The text information to log</param>
        void Fatal(string logLine_);

        /// <summary>
        /// Write a fatal level log statement with the text string provided and also
        /// serialises out the objects in the parameter array
        /// </summary>
        /// <param name="logLine_">The text information to log</param>
        /// <param name="args_">The additional parameters to log</param>
        void Fatal(string logLine_, params object[] args_);

        /// <summary>
        /// Writes a fatal level log statemnet with the text string and the exception
        /// details provided.
        /// 
        /// The exception is logged on a new line and contains the complete message
        /// and stack trace
        /// </summary>
        /// <param name="exception_">The exception to log</param>
        /// <param name="logLine_">The text log line</param>
        void Fatal(Exception exception_, string logLine_);

        /// <summary>
        /// Writes a fatal level log statement with the text string, the exception
        /// and the additional parameters provided`s
        /// </summary>
        /// <param name="exception_">The exception to log</param>
        /// <param name="logLine_">The text log line</param>
        /// <param name="args_">The additional parameters to log</param>
        void Fatal(Exception exception_, string logLine_, params object[] args_);
	}
}
