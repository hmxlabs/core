using System;
using System.Collections.Generic;

namespace HmxLabs.Core.Log
{
    /// <summary>
    /// An implementation of <code>ILogger</code> that is useful for unit
    /// tests (and little else).
    /// 
    /// This implementation does persist or output the log messages anywhere
    /// at all. Instead if just holds them in memory and makes it easy
    /// to query for them.
    /// </summary>
    public class DiscreteMemoryLogger : LoggerBase, ILogOutput
    {
        /// <summary>
        /// Constructor. Initialises the various memory structures used internally to hold the
        /// log output
        /// </summary>
        /// <param name="name_">The name for this logger</param>
        public DiscreteMemoryLogger(string name_) : base(null, name_)
        {
            PopulateMessagesDictionary();
            PopulateExceptionsDictionary();
            LogMessages = new LogMessageAccessProvider(_messages);
            LogExceptions = new LogExceptionAccessProvider(_exceptions);
        }

        /// <summary>
        /// Read only access to all the log messages that have been written
        /// indexed by their severity. e.g. <code>logger.LogMessage[LogLevel.Info]</code>
        /// </summary>
        public ILogMessages LogMessages { get; }

        /// <summary>
        /// Read only access to all the exceptions that have been written indexed
        /// by their severity
        /// </summary>
        public ILogExceptions LogExceptions { get; }

        /// <summary>
        /// See <code>ILogger.Open</code>.
        /// In this particular implementation this is a no op.
        /// </summary>
        public override void Open()
        {
        }

        /// <summary>
        /// Clears all log messages that have been written so far
        /// </summary>
        public void Clear()
        {
            foreach (var messageList in _messages.Values)
            {
                messageList.Clear();
            }

            foreach (var exceptionList in _exceptions.Values)
            {
                exceptionList.Clear();
            }
        }

        /// <summary>
        /// Writes the log line into the memory structure of this logger.
        /// 
        /// See <code>LoggerBase.WriteLogLine</code>
        /// </summary>
        /// <param name="exception_"></param>
        /// <param name="level_"></param>
        /// <param name="content_"></param>
        /// <param name="args_"></param>
        protected override void WriteLogLine(Exception exception_, LogLevel level_, string content_, params object[] args_)
        {
            var message = CreateLogLine(exception_, level_, content_, args_);
            _messages[level_].Add(message);
            if (null == exception_)
                return;

            _exceptions[level_].Add(exception_);
        }

        private void PopulateMessagesDictionary()
        {
            _messages[LogLevel.Debug] = new List<string>();
            _messages[LogLevel.Information] = new List<string>();
            _messages[LogLevel.Notice] = new List<string>();
            _messages[LogLevel.Warning] = new List<string>();
            _messages[LogLevel.Error] = new List<string>();
            _messages[LogLevel.Critical] = new List<string>();
            _messages[LogLevel.Fatal] = new List<string>();
        }

        private void PopulateExceptionsDictionary()
        {
            _exceptions[LogLevel.Debug] = new List<Exception>();
            _exceptions[LogLevel.Information] = new List<Exception>();
            _exceptions[LogLevel.Notice] = new List<Exception>();
            _exceptions[LogLevel.Warning] = new List<Exception>();
            _exceptions[LogLevel.Error] = new List<Exception>();
            _exceptions[LogLevel.Critical] = new List<Exception>();
            _exceptions[LogLevel.Fatal] = new List<Exception>();
        }

        private readonly Dictionary<LogLevel, List<string>> _messages = new Dictionary<LogLevel, List<string>>();
        private readonly Dictionary<LogLevel, List<Exception>> _exceptions = new Dictionary<LogLevel, List<Exception>>();

        private class LogMessageAccessProvider : ILogMessages
        {
            public IEnumerable<string> this[LogLevel level_]
            {
                get { return _messages[level_]; }
            }

            public LogMessageAccessProvider(Dictionary<LogLevel, List<string>> messages_)
            {
                _messages = messages_;
            }

            private readonly Dictionary<LogLevel, List<string>> _messages;
        }

        private class LogExceptionAccessProvider : ILogExceptions
        {
            public LogExceptionAccessProvider(Dictionary<LogLevel, List<Exception>> exceptions_)
            {
                _exceptions = exceptions_;
            }

            public IEnumerable<Exception> this[LogLevel level_]
            {
                get { return _exceptions[level_]; }
            }

            private readonly Dictionary<LogLevel, List<Exception>> _exceptions;
        }
    }
}