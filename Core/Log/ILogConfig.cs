namespace HmxLabs.Core.Log
{
    /// <summary>
    /// Interface used by the <code>LoggerFactory</code> to 
    /// obtain the necessary config needed to create a new
    /// logger
    /// </summary>
    public interface ILogConfig
    {
        /// <summary>
        /// Read only property providing the name of the logger.
        /// Must always be non <code>null</code> and non empty
        /// </summary>
		string Name { get; }

        /// <summary>
        /// Read only property providing the type of the logger
        /// This must never be <code>null</code>
        /// </summary>
		string Type { get; }

        /// <summary>
        /// Read only property providing the log output location.
        /// This does not apply to all logger types, for example
        /// it doesn't apply for Trace or Console loggers.
        /// 
        /// Given it isn't always needed, the Location can be <code>null</code>
        /// </summary>
		string Location { get; }
    }
}
