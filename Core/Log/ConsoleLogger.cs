using System;

namespace HmxLabs.Core.Log
{
    /// <summary>
    /// An implementation of <code>ILogger</code> that writes out to the console.
    /// 
    /// Derives from <code>StreamWriter</code> where the stream provided is just the standard output
    /// </summary>
	public class ConsoleLogger: StreamLogger
	{
        /// <summary>
        /// Construct a new instance of the logger with the given name
        /// </summary>
        /// <param name="name_">A unique name for the logger instance</param>
		public ConsoleLogger(string name_) : base(null, name_)
		{
		}

		internal ConsoleLogger(LoggerFactory factory_, string name_) : base(factory_, name_)
		{
		}

        /// <summary>
        /// Finalizer used as part of disposing pattern.
        /// </summary>
	    ~ConsoleLogger()
	    {
	        Dispose(false);
	    }

        /// <summary>
        /// Ensures that Std Out is open
        /// </summary>
		public override void Open()
		{
		    WriteStream = Console.OpenStandardOutput();
		}

        /// <summary>
        /// Disposing pattern implementation. Removes any references to the output stream.
        /// </summary>
        /// <param name="disposing_"></param>
	    protected override void Dispose(bool disposing_)
	    {
	        if (!disposing_)
	            return;

	        WriteStream = null;
	    }
	}
}
