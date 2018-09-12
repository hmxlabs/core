using System;

namespace HmxLabs.Core.Log
{
    /// <summary>
    /// Enumeration detailing the different log levels available
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Debug logging level. This is the finest level of logging available and should generally only be used in test builds
        /// to help identify problems
        /// </summary>
        Debug = 0,
        
        /// <summary>
        /// Informational level logging. Useful for outputting information that may be of use when investigating problems
        /// in a production environment or running in final stages of testing
        /// </summary>
        Information = 1,

        /// <summary>
        /// High level useful information such as connection events or the port a server is listeniing on etc
        /// </summary>
        Notice = 2,

        /// <summary>
        /// Warnings. Rare events that do not cause an immediate problem but need to be investigated
        /// </summary>
        Warning = 3,

        /// <summary>
        /// An unexpected occurence that has caused a problem that needs to be rectifieds
        /// </summary>
        Error = 4,

        /// <summary>
        /// An unexpected occurence that requires immediate attention
        /// </summary>
        Critical = 5,

        /// <summary>
        /// A problem that has caused the application to die
        /// </summary>
        Fatal = 6
    }

    /// <summary>
    /// String representations of the enumerated log levels. These are the serialised
    /// values that will be used in the log output.
    /// 
    /// See also the enumeration <code>LogLevel</code>
    /// </summary>
    public static class LogLevelStrings
    {
        /// <summary>
        /// Debug level: DEBUG
        /// </summary>
        public const string Debug = "DEBUG";

        /// <summary>
        /// Information level: INFO
        /// </summary>
	    public const string Information = "INFO";

        /// <summary>
        /// Notice level: NOTICE
        /// </summary>
	    public const string Notice = "NOTICE";

        /// <summary>
        /// Warning level: WARNING
        /// </summary>
	    public const string Warning = "WARNING";

        /// <summary>
        /// Error level: ERROR
        /// </summary>
	    public const string Error = "ERROR";

        /// <summary>
        /// Critical level: CRITICAL
        /// </summary>
	    public const string Critical = "CRITICAL";

        /// <summary>
        /// Fatal level: FATAL
        /// </summary>
	    public const string Fatal = "FATAL";
	}

    /// <summary>
    /// Extension methods for log levels that aid in parsing and serialising the enum
    /// </summary>
    public static class LogLevelExtensions
    {
        /// <summary>
        /// Parse a string representation of the enum
        /// </summary>
        /// <param name="logLevel_">A string value of the LogLevel enumeration</param>
        /// <returns>The enumeration value</returns>
        public static LogLevel Parse(string logLevel_)
        {
            return (LogLevel) Enum.Parse(typeof (LogLevel), logLevel_);
        }

        /// <summary>
        /// Convert the enum to the string representation using the 
        /// <code>LogLevelStrings</code>
        /// </summary>
        /// <param name="level_">The enum value to convert</param>
        /// <returns>A string representation of the enum value</returns>
        public static string ToLogString(this LogLevel level_)
        {
            switch (level_)
            {
                case LogLevel.Debug:
                    return LogLevelStrings.Debug;

                case LogLevel.Information:
                    return LogLevelStrings.Information;

                case LogLevel.Notice:
                    return LogLevelStrings.Notice;

                case LogLevel.Warning:
                    return LogLevelStrings.Warning;

                case LogLevel.Error:
                    return LogLevelStrings.Error;

                case LogLevel.Critical:
                    return LogLevelStrings.Critical;

                case LogLevel.Fatal:
                    return LogLevelStrings.Fatal;

                default:
                    throw new ArgumentException("Unknown log level type");
            }   
        }
    }
}
