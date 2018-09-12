using System;

namespace HmxLabs.Core.DateTIme
{
    /// <summary>
    /// An interface to be used by application code to get the "time".
    /// 
    /// The intention here (amongst other concerns) is to facilitate the testing of time based code. 
    /// 
    /// If the application retrieves the current time directly by simply calling <code>DateTime.Now</code>
    /// then it becomes very difficult to test any time logic that may be performed as it is never possible
    /// to know what value was obtained.
    /// 
    /// If all calls to obtain the time are instead done via this interface, other implementations of this
    /// interface can be provided during test usage where the exact time provided can be controlled and
    /// therefore allow for very specific test cases
    /// 
    /// Additionally it means that it becomes possible to swap an application from operating on Local time
    /// to UTC time or some other time standard simply by changing the implementation of the <code>ITimeProvider</code>
    /// that is used.
    /// </summary>
    public interface ITimeProvider
    {
        /// <summary>
        /// Get the current time. This may be either local or UTC or any other
        /// variaion depending on the implementation of this interface
        /// </summary>
        DateTime Now { get; }
    }
}
