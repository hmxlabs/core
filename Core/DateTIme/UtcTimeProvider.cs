using System;

namespace HmxLabs.Core.DateTIme
{
    /// <summary>
    /// See <code>ITimeProvider</code>
    /// 
    /// This implementation is the equivalent of always using the UTC time
    /// i.e. <code>DateTime.UtcNow</code>
    /// </summary>
    public class UtcTimeProvider : ITimeProvider
    {
        /// <summary>
        /// The current UTC time, equivalent to <code>DateTime.UtcNow</code>
        /// </summary>
        public DateTime Now => DateTime.UtcNow;
    }
}
