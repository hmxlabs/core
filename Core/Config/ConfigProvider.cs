using System;
using System.Collections.Generic;

namespace HmxLabs.Core.Config
{
    /// <summary>
    /// Interface to provide configuration information that is represented as key value pairs.
    /// 
    /// Provides configuration values based on the provided key
    /// and will attempt to cast the value to a <code>string</code>, <code>integer</code> or <code>double</code>
    /// as required.
    /// </summary>
    public abstract class ConfigProvider : IConfigProvider
    {
        /// <summary>
        /// See documentation on <code>IConfigProvider</code>
        /// </summary>
        /// <param name="key_">The key of the configuration</param>
        /// <returns><code>true</code> if this provider can provide a value for this key else <code>false</code></returns>
        public bool Contains(string key_)
        {
            GetConfigParameterGuard(key_);
            return _config.ContainsKey(key_);
        }

        /// <summary>
        /// See documentation on <code>IConfigProvider</code>
        /// </summary>
        /// <param name="key_">The key of the configuration</param>
        /// <returns>A string representation of the value corresponding to the provided key</returns>
        public string GetConfigAsString(string key_)
        {
            GetConfigParameterGuard(key_);
            return GetConfigValue(key_);
        }

        /// <summary>
        /// See documentation on <code>IConfigProvider</code>
        /// </summary>
        /// <param name="key_">The key of the configuration</param>
        /// <returns>A string representation of the value corresponding to the provided key</returns>
        public string GetConifgAsStringStrict(string key_)
        {
            var configValue = GetConfigAsString(key_);
            if (null == configValue)
                throw new ConfigException("The requested configuration is a null string when a non null value is required", key_);

            if (string.IsNullOrEmpty(configValue))
                throw new ConfigException("The requested configuration is an empty string when a non empty value is required", key_);

            return configValue;
        }

        /// <summary>
        /// See documentation on <code>IConfigProvider</code>
        /// </summary>
        /// <param name="key_">The key of the configuration</param>
        /// <returns>An <code>integer</code> representation of the value corresponding to the provided key</returns>`
        public int GetConfigAsInteger(string key_)
        {
            GetConfigParameterGuard(key_);
            var strConfig = GetConfigValue(key_);
            int intConfig;
            if (!int.TryParse(strConfig, out intConfig))
                throw new ConfigException("The requested configuration could not be parsed as an int", key_);

            return intConfig;
        }

        /// <summary>
        /// See documentation on <code>IConfigProvider</code>
        /// </summary>
        /// <param name="key_">The key of the configuration</param>
        /// <returns>A <code>double</code> representation of the value corresponding to the provided key</returns>
        public double GetConfigAsDouble(string key_)
        {
            GetConfigParameterGuard(key_);
            var strConfig = GetConfigValue(key_);
            Double doubleConfig;
            if (!double.TryParse(strConfig, out doubleConfig))
                throw new ConfigException("The requested configuration could not be parsed as a double", key_);

            return doubleConfig;
        }

        /// <summary>
        /// See documentation on <code>IConfigProvider</code>
        /// </summary>
        /// <param name="key_">The key of the configuration</param>
        /// <returns>A <code>bool</code> representation of the value corresponding to the provided key</returns>
        public bool GetConfigAsBool(string key_)
        {
            GetConfigParameterGuard(key_);
            var strConfig = GetConfigValue(key_);
            bool boolConfig;
            if (!bool.TryParse(strConfig, out boolConfig))
                throw new ConfigException("The requested configuration could not be parsed as a bool", key_);

            return boolConfig;
        }

        /// <summary>
        /// This <code>Dictionary</code> object acts as the internal store for all the configuration information.
        /// The values are all stored as strings (that's how they are read from the config file)
        /// and cast to <code>integer</code> or <code>double</code> when needed.
        /// </summary>
        protected Dictionary<string, string> Config { get { return _config; } }

        /// <summary>
        /// Internal implementation. Retrieves the configuration from the Dictionary and throws the 
        /// appropriate exception if not contained.
        /// </summary>
        /// <param name="key_"></param>
        /// <returns></returns>
        protected string GetConfigValue(string key_)
        {
            if (!_config.ContainsKey(key_))
                throw new KeyNotFoundException($"No config value corresponding to the requested key [{key_}] could be found");

            return _config[key_];
        }

        /// <summary>
        /// Checks that the input parameters for the Get functions are valid. Behaves as per the public
        /// API documentation on <code>IConfigProvider</code> for the Get methods
        /// </summary>
        /// <param name="key_"></param>
        protected void GetConfigParameterGuard(string key_)
        {
            if (null == key_)
                throw new ArgumentNullException(nameof(key_));

            if (string.IsNullOrWhiteSpace(key_))
                throw new ArgumentException("The requested config key is not valid (empty)");
        }

        private readonly Dictionary<string, string> _config = new Dictionary<string, string>();
    }
}
