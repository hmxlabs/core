namespace HmxLabs.Core.Log
{
    /// <summary>
    /// Static class containing the known logger type string literals that can be specified in the config
    /// </summary>
    public static class LoggerType
    {
        /// <summary>
        /// Consols logger: console
        /// </summary>
        public const string Console = "console";

        /// <summary>
        /// Trace  logger: trace
        /// </summary>
        public const string Trace = "trace";

        /// <summary>
        /// File logger: file
        /// </summary>
        public const string File = "file";
    }
}
