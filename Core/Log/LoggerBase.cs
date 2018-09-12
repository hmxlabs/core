using System;
using System.Text;
using HmxLabs.Core.DateTIme;

namespace HmxLabs.Core.Log
{
    /// <summary>
    /// An abstract base logging class that performs all the utility functions and provides the multiple
    /// log function overloads allowing derived classes to deal only with how to persiste / present the 
    /// actual log output.
    /// 
    /// See also <code>ILogger</code>.
    /// </summary>
    public abstract class LoggerBase : ILogger
	{
        /// <summary>
        /// The default time provider to use should one not be specified
        /// </summary>
        public static ITimeProvider DefaultTimeProvider = new DefaultTimeProvider();

        /// <summary>
        /// The default log encoding to use should one not be specified
        /// </summary>
        public static Encoding DefaultLogEncoding = Encoding.UTF8;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="factory_"></param>
        /// <param name="name_"></param>
	    protected LoggerBase(LoggerFactory factory_, string name_)
		{
			if (string.IsNullOrEmpty(name_))
			{
				throw new ArgumentNullException("name_");
			}

			_factory = factory_;
			Name = name_;
	        _timeProvider = DefaultTimeProvider;
	        _logEncoding = DefaultLogEncoding;
		}

        /// <summary>
        /// Destructor. Used as part of the implementation of a disposable pattern.
        /// Calling dispose on this object correctly will remove it from the
        /// finalization queue.
        /// </summary>
	    ~LoggerBase()
	    {
	        Dispose(false);
	    }

        /// <summary>
        /// See <code>ILogger.TimeProvider</code>
        /// </summary>
	    public ITimeProvider TimeProvider
	    {
            get { return _timeProvider; }
	        set
	        {
	            if (null == value)
	            {
	                _timeProvider = DefaultTimeProvider;
	                return;
	            }

	            _timeProvider = value;
	        }
	    }

        /// <summary>
        /// See <code>ILogger.LogEncoding</code>
        /// </summary>
	    public Encoding LogEncoding
	    {
            get { return _logEncoding; }
	        set
	        {
	            if (null == value)
	            {
	                _logEncoding = DefaultLogEncoding;
	                return;
	            }

	            _logEncoding = value;
	        }
	    }

        /// <summary>
        /// See <code>ILogger.Open</code>
        /// 
        /// This is an abstract method and should be implemented by any derived classes.
        /// </summary>
		public abstract void Open();

        /// <summary>
        /// See <code>ILogger.Name</code>
        /// </summary>
		public string Name { get; }

        /// <summary>
        /// This base class implements the IDisposable interface and the standard .NET
        /// disposable pattern.
        /// </summary>
	    public void Dispose()
		{
            Dispose(true);
            GC.SuppressFinalize(this);
		}

        /// <summary>
        /// See <code>ILogger.Debug</code>
        /// </summary>
        /// <param name="logLine_"></param>
        public void Debug(string logLine_)
        {
            WriteLogLine(null, LogLevel.Debug, logLine_, null);
        }

        /// <summary>
        /// /// See <code>ILogger.Debug</code>
        /// </summary>
        /// <param name="format_"></param>
        /// <param name="args_"></param>
        public void Debug(string format_, params object[] args_)
        {
            WriteLogLine(null, LogLevel.Debug, format_, args_);
        }

        /// <summary>
        /// See <code>ILogger.Debug</code>
        /// </summary>
        /// <param name="exception_"></param>
        /// <param name="logLine_"></param>
        public void Debug(Exception exception_, string logLine_)
        {
            WriteLogLine(exception_, LogLevel.Debug, logLine_, null);
        }

        /// <summary>
        /// See <code>ILogger.Debug</code>
        /// </summary>
        /// <param name="exception_"></param>
        /// <param name="format_"></param>
        /// <param name="args_"></param>
        public void Debug(Exception exception_, string format_, params object[] args_)
        {
            WriteLogLine(exception_, LogLevel.Debug, format_, args_);
        }

        /// <summary>
        /// See <code>ILogger.Info</code>
        /// </summary>
        /// <param name="logLine_"></param>
        public void Info(string logLine_)
        {
            WriteLogLine(null, LogLevel.Information, logLine_, null);
        }

        /// <summary>
        /// See <code>ILogger.Info</code>
        /// </summary>
        /// <param name="format_"></param>
        /// <param name="args_"></param>
        public void Info(string format_, params object[] args_)
        {
            WriteLogLine(null, LogLevel.Information, format_, args_);
        }

        /// <summary>
        /// See <code>ILogger.Info</code>
        /// </summary>
        /// <param name="exception_"></param>
        /// <param name="logLine_"></param>
        public void Info(Exception exception_, string logLine_)
        {
            WriteLogLine(exception_, LogLevel.Information, logLine_, null);
        }

        /// <summary>
        /// See <code>ILogger.Info</code>
        /// </summary>
        /// <param name="exception_"></param>
        /// <param name="format_"></param>
        /// <param name="args_"></param>
        public void Info(Exception exception_, string format_, params object[] args_)
        {
            WriteLogLine(exception_, LogLevel.Information, format_, args_);
        }

        /// <summary>
        /// See <code>ILogger.Notice</code>
        /// </summary>
        /// <param name="logLine_"></param>
        public void Notice(string logLine_)
        {
            WriteLogLine(null, LogLevel.Notice, logLine_, null);
        }

        /// <summary>
        /// See <code>ILogger.Notice</code>
        /// </summary>
        /// <param name="logLine_"></param>
        /// <param name="args_"></param>
        public void Notice(string logLine_, params object[] args_)
        {
            WriteLogLine(null, LogLevel.Notice, logLine_, args_);
        }

        /// <summary>
        /// See <code>ILogger.Notice</code>
        /// </summary>
        /// <param name="exception_"></param>
        /// <param name="logLine_"></param>
        public void Notice(Exception exception_, string logLine_)
        {
            WriteLogLine(exception_, LogLevel.Notice, logLine_, null);
        }

        /// <summary>
        /// See <code>ILogger.Notice</code>
        /// </summary>
        /// <param name="exception_"></param>
        /// <param name="logLine_"></param>
        /// <param name="args_"></param>
        public void Notice(Exception exception_, string logLine_, params object[] args_)
        {
            WriteLogLine(exception_, LogLevel.Notice, logLine_, args_);
        }

        /// <summary>
        /// See <code>ILogger.Warning</code>
        /// </summary>
        /// <param name="logLine_"></param>
        public void Warning(string logLine_)
        {
            WriteLogLine(null, LogLevel.Warning, logLine_, null);
        }

        /// <summary>
        /// See <code>ILogger.Warning</code>
        /// </summary>
        /// <param name="logLine_"></param>
        /// <param name="args_"></param>
        public void Warning(string logLine_, params object[] args_)
        {
            WriteLogLine(null, LogLevel.Warning, logLine_, args_);
        }

        /// <summary>
        /// See <code>ILogger.Warning</code>
        /// </summary>
        /// <param name="exception_"></param>
        /// <param name="logLine_"></param>
        public void Warning(Exception exception_, string logLine_)
        {
            WriteLogLine(exception_, LogLevel.Warning, logLine_, null);
        }

        /// <summary>
        /// See <code>ILogger.Warning</code>
        /// </summary>
        /// <param name="exception_"></param>
        /// <param name="logLine_"></param>
        /// <param name="args_"></param>
        public void Warning(Exception exception_, string logLine_, params object[] args_)
        {
            WriteLogLine(exception_, LogLevel.Warning, logLine_, args_);
        }

        /// <summary>
        /// See <code>ILogger.Error</code>
        /// </summary>
        /// <param name="logLine_"></param>
        public void Error(string logLine_)
        {
            WriteLogLine(null, LogLevel.Error, logLine_, null);
        }

        /// <summary>
        /// See <code>ILogger.Error</code>
        /// </summary>
        /// <param name="logLine_"></param>
        /// <param name="args_"></param>
        public void Error(string logLine_, params object[] args_)
        {
            WriteLogLine(null, LogLevel.Error, logLine_, args_);
        }

        /// <summary>
        /// See <code>ILogger.Error</code>
        /// </summary>
        /// <param name="exception_"></param>
        /// <param name="logLine_"></param>
        public void Error(Exception exception_, string logLine_)
        {
            WriteLogLine(exception_, LogLevel.Error, logLine_, null);
        }

        /// <summary>
        /// See <code>ILogger.Error</code>
        /// </summary>
        /// <param name="exception_"></param>
        /// <param name="logLine_"></param>
        /// <param name="args_"></param>
        public void Error(Exception exception_, string logLine_, params object[] args_)
        {
            WriteLogLine(exception_, LogLevel.Error, logLine_, args_);
        }

        /// <summary>
        /// See <code>ILogger.Critical</code>
        /// </summary>
        /// <param name="logLine_"></param>
        public void Critical(string logLine_)
        {
            WriteLogLine(null, LogLevel.Critical, logLine_, null);
        }

        /// <summary>
        /// See <code>ILogger.Critical</code>
        /// </summary>
        /// <param name="logLine_"></param>
        /// <param name="args_"></param>
        public void Critical(string logLine_, params object[] args_)
        {
            WriteLogLine(null, LogLevel.Critical, logLine_, args_);
        }

        /// <summary>
        /// See <code>ILogger.Critical</code>
        /// </summary>
        /// <param name="exception_"></param>
        /// <param name="logLine_"></param>
        public void Critical(Exception exception_, string logLine_)
        {
            WriteLogLine(exception_, LogLevel.Critical, logLine_, null);
        }

        /// <summary>
        /// See <code>ILogger.Critical</code>
        /// </summary>
        /// <param name="exception_"></param>
        /// <param name="logLine_"></param>
        /// <param name="args_"></param>
        public void Critical(Exception exception_, string logLine_, params object[] args_)
        {
            WriteLogLine(exception_, LogLevel.Critical, logLine_, args_);
        }

        /// <summary>
        /// See <code>ILogger.Fata</code>
        /// </summary>
        /// <param name="logLine_"></param>
        public void Fatal(string logLine_)
        {
            WriteLogLine(null, LogLevel.Fatal, logLine_, null);
        }

        /// <summary>
        /// See <code>ILogger.Fata</code>
        /// </summary>
        /// <param name="logLine_"></param>
        /// <param name="args_"></param>
        public void Fatal(string logLine_, params object[] args_)
        {
            WriteLogLine(null, LogLevel.Fatal, logLine_, args_);
        }

        /// <summary>
        /// See <code>ILogger.Fata</code>
        /// </summary>
        /// <param name="exception_"></param>
        /// <param name="logLine_"></param>
        public void Fatal(Exception exception_, string logLine_)
        {
            WriteLogLine(exception_, LogLevel.Fatal, logLine_, null);
        }

        /// <summary>
        /// See <code>ILogger.Fata</code>
        /// </summary>
        /// <param name="exception_"></param>
        /// <param name="logLine_"></param>
        /// <param name="args_"></param>
        public void Fatal(Exception exception_, string logLine_, params object[] args_)
        {
            WriteLogLine(exception_, LogLevel.Fatal, logLine_, args_);
        }

        /// <summary>
        /// Part of the .NET disposable pattern
        /// </summary>
        /// <param name="disposing_"></param>
	    protected virtual void Dispose(bool disposing_)
	    {
	        if (!disposing_) // No managed resources to dispose in the this base class
	            return;

            if (null != _factory)
            {
                _factory.RemoveLogger(this);
            }
	    }
        
        /// <summary>
        /// Create the log output line
        /// </summary>
        /// <param name="exception_">The exception, if any to include</param>
        /// <param name="level_">The log level</param>
        /// <param name="content_">The actual log message</param>
        /// <param name="args_">Any additional objects to log out</param>
        /// <returns>The constructed log line as it should be persisted / presented</returns>
        protected string CreateLogLine(Exception exception_, LogLevel level_, string content_, params object[] args_)
		{
			var logLine = new StringBuilder();
			logLine.Append(TimeProvider.Now.ToIsoDateTimeString());
			logLine.Append(" ");
			logLine.Append(level_.ToLogString());
			logLine.Append(": ");
		    
            if (null == args_ || 0 == args_.Length)
		        logLine.Append(content_);
		    else
		        logLine.Append(string.Format(content_, args_));
		    
            if (null != exception_)
		    {
		        logLine.Append(Environment.NewLine);
                logLine.Append(exception_);
		    }
		    
            logLine.Append(Environment.NewLine);
			
            return logLine.ToString();
		}

        /// <summary>
        /// Abstract method that derived classes should implement to actually persist or present the log output.
        /// 
        /// The protected method <code>CreateLogLine</code> to create the required output
        /// </summary>
        /// <param name="exception_">The exception (if any) to log</param>
        /// <param name="level_">The log level to write at</param>
        /// <param name="content_">The log message</param>
        /// <param name="args_">Any additional data to include in the log</param>
	    protected abstract void WriteLogLine(Exception exception_, LogLevel level_, string content_, params object[] args_);

	    private ITimeProvider _timeProvider;
	    private Encoding _logEncoding;
		private readonly LoggerFactory _factory;
	}
}
