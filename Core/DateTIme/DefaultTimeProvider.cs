using System;

namespace HmxLabs.Core.DateTIme
{
    /// <summary>
    /// See <code>ITimeProvider</code>
    /// 
    /// This implementation is the equivalent of just calling <code>DateTime.Now</code>
    /// </summary>
    public class DefaultTimeProvider : ITimeProvider
    {
        /// <summary>
        /// Get the current time as per <code>DateTime.Now</code>
        /// </summary>
        public DateTime Now => DateTime.Now;
    }
}
