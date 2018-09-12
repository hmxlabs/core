using System;
using System.IO;

namespace HmxLabs.Core.Log
{
    /// <summary>
    /// An implementation of <code>ILogger</code> deriving from <code>LoggerBase</code>
    /// that just writes all output to the provided stream
    /// </summary>
    public class StreamLogger : LoggerBase
    {
        /// <summary>
        /// Constructor. Initialises the logger to write to the provided stream
        /// </summary>
        /// <param name="name_">The name of this logger</param>
        /// <param name="writeStream_">The stream to write the log output to</param>
        public StreamLogger(string name_, Stream writeStream_) : base(null, name_)
        {
            if (null == writeStream_)
                throw new ArgumentNullException(nameof(writeStream_));
            WriteStream = writeStream_;
        }

        /// <summary>
        /// Finalization as per .NET disposable pattern
        /// </summary>
        ~StreamLogger()
        {
            Dispose(false);
        }

        /// <summary>
        /// See <code>ILogger.Open</code>.
        /// This implementation is a no op 
        /// </summary>
        public override void Open()
        {
        }

        /// <summary>
        /// Protected constructor for derived classes that will set the write stream
        /// later in the object lifecycle
        /// </summary>
        /// <param name="name_"></param>
        protected StreamLogger(string name_) : base(null, name_)
        {
        }

        internal StreamLogger(LoggerFactory factory_, string name_) : base(factory_, name_)
        {
        }

        /// <summary>
        /// The stream that the output should be written to
        /// </summary>
        protected Stream WriteStream { get; set; }

        /// <summary>
        /// The object used to provide thread safe access to the <code>WriteStream</code>
        /// </summary>
        protected object WriteStreamLock { get { return _writeStreamLock; } }

        /// <summary>
        /// Implementation of .NET disposable pattern
        /// </summary>
        /// <param name="disposing_"></param>
        protected override void Dispose(bool disposing_)
        {
            base.Dispose(disposing_);
            if (!disposing_)
                return; // No unmanaged resources to release

            lock (WriteStreamLock)
            {
                if (null == WriteStream) return;
                WriteStream.Dispose();
                WriteStream = null;
            }
        }

        /// <summary>
        /// See <code>LoggerBase.WriteLogLine</code>
        /// 
        /// Write the log line to the stream
        /// </summary>
        /// <param name="exception_">The exception (if any) to log</param>
        /// <param name="level_">The log level to write at</param>
        /// <param name="content_">The log message</param>
        /// <param name="args_">Any additional data to include in the log</param>
        protected override void WriteLogLine(Exception exception_, LogLevel level_, string content_, params object[] args_)
        {
            var logLine = CreateLogLine(exception_, level_, content_, args_);
            lock (WriteStreamLock)
            {
                if (null == WriteStream)
                    return;

                var bytesToWrite = LogEncoding.GetBytes(logLine);
                WriteStream.Write(bytesToWrite, 0, bytesToWrite.Length);
                WriteStream.Flush();
            }
        }

        private readonly object _writeStreamLock = new object();
    }
}