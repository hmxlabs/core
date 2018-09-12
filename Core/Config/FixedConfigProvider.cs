using System;

namespace HmxLabs.Core.Config
{
    /// <summary>
    /// An implementaton of IConfigProvider that is populated only
    /// by calling the <code>AddConfig</code> method on this claas.
    /// 
    /// See <code>IConfigProvider</code> for further details
    /// </summary>
    public class FixedConfigProvider : ConfigProvider
    {
        /// <summary>
        /// Add the specified key value pair to the configuration.
        /// </summary>
        /// <exception cref="ArgumentNullException">If the provided key value is null</exception>
        /// <param name="key_">The configuration key</param>
        /// <param name="value_">The configuration value</param>
        public void AddConfig(string key_, string value_)
        {
            if (null == key_)
                throw new ArgumentNullException(nameof(key_));

            Config.Add(key_, value_);
        }
    }
}
