using System;
using System.Collections.Generic;

namespace HmxLabs.Core.Log
{
    /// <summary>
    /// Factory instantiation of loggers to help ensure
    /// only one of each type/name is created and to 
    /// also help abstraction of knowledge of which logger
    /// is used from the application.
    /// </summary>
	public class LoggerFactory
	{
        /// <summary>
        /// Constructor. Creates the local cache of loggers
        /// </summary>
		public LoggerFactory()
		{
			_loggers = new Dictionary<string, ILogger>();
		}

        /// <summary>
        /// Calls <code>Close</code> on all loggers
        /// that were obtained from this factory
        /// </summary>
		public void CloseAllLoggers()
		{
			lock (this)
			{
				foreach (ILogger logger in _loggers.Values)
				{
					logger.Dispose();
				}

				_loggers.Clear();
			}
		}

        /// <summary>
        /// Get (or create if one does not already exist) a logger
        /// matching the specification in the <code>LogConfig</code>
        /// provided
        /// </summary>
        /// <param name="logConfig_">The configuration specifying the properties of the logger required</param>
        /// <returns>An <code>ILogger</code> implementation</returns>
		public ILogger GetLogger(ILogConfig logConfig_)
		{
			lock (this)
			{
				ILogger logger;
				if (_loggers.TryGetValue(logConfig_.Name, out logger))
				{
					return logger;
				}

				// Need to create a new one
				logger = CreateLogger(logConfig_);
				_loggers.Add(logConfig_.Name, logger);

				return logger;
			}
		}

		internal void RemoveLogger(ILogger logger_)
		{
			lock (this)
			{
				_loggers.Remove(logger_.Name);
			}
		}

		private ILogger CreateLogger(ILogConfig logConfig_)
		{
			if (LoggerType.Console.Equals(logConfig_.Type))
			{
				return CreateConsoleLogger(logConfig_);
			}

			if (LoggerType.Trace.Equals(logConfig_.Type))
			{
				return CreateTraceLogger(logConfig_);
			}

			if (LoggerType.File.EndsWith(logConfig_.Type))
			{
				return CreateFileLogger(logConfig_);
			}

			throw new ArgumentException("The specified type of logger can not be created", nameof(logConfig_));
		}

		private ILogger CreateConsoleLogger(ILogConfig logConfig_)
		{
			return new ConsoleLogger(this, logConfig_.Name);
		}

		private ILogger CreateTraceLogger(ILogConfig logConfig_)
		{
			return new TraceLogger(this, logConfig_.Name);
		}

		private ILogger CreateFileLogger(ILogConfig logConfig_)
		{
			return new FileLogger(this, logConfig_.Name, logConfig_.Location);
		}

		private readonly Dictionary<string, ILogger> _loggers;
	}
}
