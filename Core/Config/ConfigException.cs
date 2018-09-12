using System;

namespace HmxLabs.Core.Config
{
    /// <summary>
    /// An exception thrown when errors are encountered reading application configuration
    /// </summary>
    public class ConfigException : Exception
    {
        /// <summary>
        /// Construct the exception and provide the configuration key that resulted in the exception being thrown
        /// and a message detailing that problem
        /// </summary>
        /// <param name="message_">A message detailing the problem that was encountered</param>
        /// <param name="key_">The configuration key that caused the exception</param>
        public ConfigException(string message_, string key_)
        {
            Message = $"Key: [{key_}]. Message: " + message_;
        }

        /// <summary>
        /// An explanation for why the exception was thrown.
        /// </summary>
        public override string Message { get; }
    }
}
