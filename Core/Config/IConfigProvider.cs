namespace HmxLabs.Core.Config
{
    /// <summary>
    /// Interface to provide configuration information that is represented as key value pairs.
    /// 
    /// Any implementer of this interface is able to provide configuration values based on the provided key
    /// and will attempt to cast the value to a <code>string</code>, <code>integer</code> or <code>double</code>
    /// as required.
    /// </summary>
    public interface IConfigProvider
    {
        /// <summary>
        /// Checks if the requested key can be provided by this configuration provider.
        /// 
        /// The provided key can not be <code>null</code>, empty or only whitespace.
        /// </summary>
        /// <param name="key_">The key of the configuration</param>
        /// <returns><code>true</code> if this provider can provide a value for this key else <code>false</code></returns>
        bool Contains(string key_);

        /// <summary>
        /// Gets the requested configuration value for the key specified and interprets it as a <code>string</code>.
        /// 
        /// If the requeted key can not be provided by this provider it will throw a
        /// <code>KeyNotFoundException</code>.
        /// 
        /// The provided key can not be <code>null</code>, empty or only whitespace.
        /// </summary>
        /// <param name="key_">The key of the configuration</param>
        /// <returns>A string representation of the value corresponding to the provided key</returns>
        string GetConfigAsString(string key_);

        /// <summary>
        /// As per <code>GetConfigAsString</code> except that it will also check that if the retrieved config value
        /// is null or an empty string and throw a <code>ConfigException</code> in either case.
        /// </summary>
        /// <param name="key_"></param>
        /// <returns></returns>
        string GetConifgAsStringStrict(string key_);

        /// <summary>
        /// Gets the requested configuration value for the key specified and interprets it as a <code>integer</code>.
        /// 
        /// If the requeted key can not be provided by this provider it will throw a
        /// <code>KeyNotFoundException</code>.
        /// 
        /// If the configuration value can not be parsed as an integer a <code>ConfigurationException</code>
        /// will be thrown.
        /// 
        /// The provided key can not be <code>null</code>, empty or only whitespace.
        /// </summary>
        /// <param name="key_">The key of the configuration</param>
        /// <returns>An <code>integer</code> representation of the value corresponding to the provided key</returns>
        int GetConfigAsInteger(string key_);

        /// <summary>
        /// Gets the requested configuration value for the key specified and interprets it as a <code>double</code>.
        /// 
        /// If the requeted key can not be provided by this provider it will throw a
        /// <code>KeyNotFoundException</code>.
        /// 
        /// If the configuration value can not be parsed as a double a <code>ConfigurationException</code>
        /// will be thrown.
        /// 
        /// The provided key can not be <code>null</code>, empty or only whitespace.
        /// </summary>
        /// <param name="key_">The key of the configuration</param>
        /// <returns>A <code>double</code> representation of the value corresponding to the provided key</returns>
        double GetConfigAsDouble(string key_);

        /// <summary>
        /// Gets the requested configuration value for the key specified and interprets it as a <code>bool</code>.
        /// 
        /// If the requeted key can not be provided by this provider it will throw a
        /// <code>KeyNotFoundException</code>.
        /// 
        /// If the configuration value can not be parsed as a boolean a <code>ConfigurationException</code>
        /// will be thrown.
        /// 
        /// The provided key can not be <code>null</code>, empty or only whitespace.
        /// 
        /// The value must explicitly be defined as <code>true</code> or <code>false</code> but is not case
        /// sensitive.
        /// </summary>
        /// <param name="key_">The key of the configuration</param>
        /// <returns>A <code>double</code> representation of the value corresponding to the provided key</returns>
        bool GetConfigAsBool(string key_);
    }
}
