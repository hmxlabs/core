using System.Collections.Generic;

namespace HmxLabs.Core.Log
{
    /// <summary>
    /// Utility interface to provide a fluent API
    /// </summary>
    public interface ILogMessages
    {
        /// <summary>
        /// Utility indexer to provide a fluent API and enable access to log message by
        /// the log level
        /// </summary>
        /// <param name="level_"></param>
        /// <returns></returns>
        IEnumerable<string> this[LogLevel level_] { get; }
    }
}