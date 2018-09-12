using System;
using System.Collections.Generic;

namespace HmxLabs.Core.Log
{
    /// <summary>
    /// Utility interface used to provide a fluent API
    /// </summary>
    public interface ILogExceptions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="level_"></param>
        IEnumerable<Exception> this[LogLevel level_] { get; }
    }
}