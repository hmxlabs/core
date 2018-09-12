using System;
using HmxLabs.Core.Config;

namespace HmxLabs.Core.Log
{
    /// <summary>
    /// The configuration required to instatiate a new logger
    /// </summary>
	public class LogConfig : ILogConfig
	{
        /// <summary>
        /// The keys to use when reading the configuration from an
        /// <code>IConfigProvider</code>
        /// </summary>
        public static class ConfigKeys
        {
            /// <summary>
            /// The key for the location: log.location
            /// </summary>
            public const string Location = "log.location";

            /// <summary>
            /// The key for the type: log.type
            /// </summary>
            public const string Type = "log.type";

            /// <summary>
            /// The key for the name: log.name
            /// </summary>
            public const string Name = "log.name";
        }

        /// <summary>
        /// Constructor. All parameters are required
        /// </summary>
        /// <param name="name_">The name of the logger</param>
        /// <param name="location_">The log location, for example for a file logger this would be the directory</param>
        /// <param name="type_">The type of logger to use, for example File, Trace, Console etc</param>
		public LogConfig(string name_, string location_, string type_)
		{
			if (string.IsNullOrEmpty(name_))
			{
				throw new ArgumentNullException(nameof(name_));
			}

			if (string.IsNullOrEmpty(type_))
			{
				throw new ArgumentNullException(nameof(type_));
			}

			Location = location_;
			Type = type_;
			Name = name_;
		}

        /// <summary>
        /// Create log config by reading the values from the provided
        /// <code>IConfigProvider</code>
        /// </summary>
        /// <param name="configProvider_"></param>
        public LogConfig(IConfigProvider configProvider_)
        {
            if (null == configProvider_)
                throw new ArgumentNullException(nameof(configProvider_));

            Name = configProvider_.GetConifgAsStringStrict(ConfigKeys.Name);
            Type = configProvider_.GetConifgAsStringStrict(ConfigKeys.Type);
            Location = configProvider_.GetConifgAsStringStrict(ConfigKeys.Location);
        }

        /// <summary>
        /// Read only property providing the name of the logger
        /// </summary>
		public string Name { get; }

        /// <summary>
        /// Read only property providing the type of the logger
        /// </summary>
		public string Type { get; }

        /// <summary>
        /// Read only property providing the log output location.
        /// This does not apply to all logger types, for example
        /// it doesn't apply for Trace or Console loggers.
        /// </summary>
		public string Location { get; }
	}
}
