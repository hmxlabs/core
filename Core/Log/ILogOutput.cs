namespace HmxLabs.Core.Log
{
    /// <summary>
    /// Utility interface to provide a fluent API
    /// </summary>
    public interface ILogOutput
    {
        /// <summary>
        /// The log message in the output
        /// </summary>
        ILogMessages LogMessages { get; }

        /// <summary>
        /// The log exceptions in the output
        /// </summary>
        ILogExceptions LogExceptions { get; }
    }
}
