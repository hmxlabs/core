using System;
using System.Net;
using HmxLabs.Core.Config;

namespace HmxLabs.Core.Net.Mail
{
    /// <summary>
    /// Implementation of the <code>ISmtpCongif</code> interface. See <code>ISmtpConfig</code> for
    /// further details.
    /// 
    /// This implementation can be initialised with an instance of an <code>IConfigProvider</code>
    /// i.e. it can be passed an instance of (for example) a <code>PosixConfigReader</code> which
    /// has read the configuration for the application and then extract the necessary
    /// configuration for <code>ISmtpConfig</code> from this automatically.
    /// 
    /// In order to do this it uses the key values defined in the nested public class
    /// <code>ConfigKey</code>
    /// 
    /// Specifically it will look for the following keys:
    /// 
    /// ServerName: "smtp.server"
    /// Port: "smtp.port"
    /// Username: "smtp.username"
    /// Password: "smtp.password"
    /// 
    /// The class can also be constructed passing in the necessary configuration values
    /// explicitly instead.
    /// </summary>
    public class SmtpConfig : ISmtpConfig
    {
        /// <summary>
        /// Static class containing the keys for the various configuration values
        /// </summary>
        public static class ConfigKeys
        {
            /// <summary>
            /// The server name key
            /// </summary>
            public const string ServerName = "smtp.server";

            /// <summary>
            /// The port key
            /// </summary>
            public const string Port = "smtp.port";

            /// <summary>
            /// The username key
            /// </summary>
            public const string UserName = "smtp.username";

            /// <summary>
            /// The password key
            /// </summary>
            public const string Password = "smtp.password";

            /// <summary>
            /// Whether or not SSL/TLS should be used
            /// </summary>
            public const string EnableSsl = "smtp.enablessl";
        }

        /// <summary>
        /// The default SMTP port. This value is used if no port is specified
        /// </summary>
        public const int DefaultPort = 25;

        /// <summary>
        /// The default for whether or not a secure connection should be used
        /// </summary>
        public const bool DefaultEnableSsl = false;

        /// <summary>
        /// Constructs an instance of the <code>SmtpConfig</code> object with only a server name
        /// specified. A default value will be used for the port number, <code>DefaultPort</code>, 25.
        /// 
        /// No network credentials will be provided.
        /// </summary>
        /// <exception cref="ArgumentNullException">If the server name provided is null</exception>
        /// <exception cref="ArgumentException">If the server name provided is empty or whitespace</exception>
        /// <param name="server_">The server name of the SMTP server</param>
        public SmtpConfig(string server_) : this(server_, DefaultPort, null)
        {
            
        }

        /// <summary>
        /// Constructs an instance of the <code>SmtpConfig</code> with a server name and network
        /// credentials only. The default port value, <code>DefaultPort</code> 25 will be used.
        /// </summary>
        /// <exception cref="ArgumentNullException">If the server name is null</exception>
        /// <exception cref="ArgumentException">If the server name is empty or just whitespace</exception>
        /// <param name="server_">The server name of the SMTP server to connect to</param>
        /// <param name="credential_">The network credentials to use to connect to the server</param>
        public SmtpConfig(string server_, NetworkCredential credential_) : this(server_, DefaultPort, credential_)
        {
                
        }

        /// <summary>
        /// Constructs an instance of the <code>SmtpConfig</code> with the specified server name,
        /// port, username and password.
        /// 
        /// If using this constructor all of the above must be provided and non null and non empty.
        /// </summary>
        /// <exception cref="ArgumentNullException">If any of the server name, username or password are null</exception>
        /// <exception cref="ArgumentException">If any of the server name, username, or password are empty or only whitespace</exception>
        /// <param name="server_">The server name of the SMTP server to connect to</param>
        /// <param name="port_">The port number the SMTP server is listening on</param>
        /// <param name="userName_">The username to provide to the SMTP server</param>
        /// <param name="password_">The password to provide to the SMTP server</param>
        /// <param name="enableSsl_">Whether or not SSL/TLS should be used. Defaults to false</param>
        public SmtpConfig(string server_, int port_, string userName_, string password_, bool enableSsl_ = false)
            : this(server_, port_, new NetworkCredential(userName_, password_), enableSsl_)
        {
            if (null == userName_)
                throw new ArgumentNullException(nameof(userName_));

            if (string.IsNullOrWhiteSpace(userName_))
                throw new ArgumentException("No username specified", nameof(userName_));

            if (null == password_)
                throw new ArgumentNullException(nameof(password_));

            if (string.IsNullOrWhiteSpace(password_))
                throw new ArgumentException("No password specified", nameof(password_));
        }

        /// <summary>
        /// Constuct an instance of a <code>SmtpConfig</code> using the specified server name,
        /// port and network credentials.
        /// </summary>
        /// <exception cref="ArgumentNullException">If the server name is null</exception>
        /// <exception cref="ArgumentException">If the server name is whitespace or emtpy</exception>
        /// <param name="server_">The name of the SMTP server to connect to</param>
        /// <param name="port_">The port number the STMP server is listening on</param>
        /// <param name="userCredentials_">The user credentials (username and password) to provide to the SMTP server</param>
        /// <param name="enableSsl_">Whether or not SSL/TLS should be used. Defaults to false</param>
        public SmtpConfig(string server_, int port_, NetworkCredential userCredentials_, bool enableSsl_ = false)
        {
            if (null == server_)
                throw new ArgumentNullException(nameof(server_));

            if (string.IsNullOrWhiteSpace(server_))
                throw new ArgumentException("No server specified", nameof(server_));

            ServerName = server_;
            Port = port_;
            UserCredentials = userCredentials_;
            EnableSsl = enableSsl_;
        }

        /// <summary>
        /// Constructs an instance of <code>SmtpConfig</code> and initialises all the necessary values
        /// by reading the configuration from the provided <code>IConfigProvider</code>.
        /// 
        /// The configuration keys it expects to find are in the nexted class <code>ConfigKeys</code>.
        /// 
        /// ServerName: "smtp.server"
        /// Port: "smtp.port"
        /// Username: "smtp.username"
        /// Password: "smtp.password"
        /// </summary>
        /// <exception cref="ArgumentNullException">If a <code>null</code> <code>IConfigProvider</code> is passed in</exception>
        /// <param name="configProvider_">The configuration provider containing the required config keyed as specified</param>
        public SmtpConfig(IConfigProvider configProvider_)
        {
            if (null == configProvider_)
                throw new ArgumentNullException(nameof(configProvider_));

            ServerName = GetServerName(configProvider_);
            var userName = GetUserName(configProvider_);
            var password = GetPassword(configProvider_);
            UserCredentials = new NetworkCredential(userName, password);
            EnableSsl = GetEnableSsl(configProvider_);

            if (!configProvider_.Contains(ConfigKeys.Port))
                return;

            Port = configProvider_.GetConfigAsInteger(ConfigKeys.Port);
        }

        /// <summary>
        /// Read only property detailing the server name to connect to
        /// </summary>
        public string ServerName { get; }

        /// <summary>
        /// Read only property specifying the port the SMTP server is listening on
        /// </summary>
        public int Port { get; } = DefaultPort;
        
        /// <summary>
        /// Read only property the credentials (username and password) to provide to the SMTP server
        /// </summary>
        public NetworkCredential UserCredentials { get; }
        
        /// <summary>
        /// Utility property that returns <code>UserCredentials.Username</code>
        /// </summary>
        public string Username => UserCredentials?.UserName;

        /// <summary>
        /// Utility property that returns <code>UserCredentals.Password</code>
        /// </summary>
        public string Password => UserCredentials?.Password;

        /// <summary>
        /// Read only property specifying if a secure connection should be used
        /// </summary>
        public bool EnableSsl { get; } = DefaultEnableSsl;

        private string GetServerName(IConfigProvider configProvider_)
        {
            if (!configProvider_.Contains(ConfigKeys.ServerName))
                throw new ArgumentException("The config provider does not have a server name specified: " + ConfigKeys.ServerName);

            var serverName = configProvider_.GetConfigAsString(ConfigKeys.ServerName);
            if (null == serverName)
                throw new ArgumentException("The config provider contains a null server name");

            if (string.IsNullOrWhiteSpace(serverName))
                throw new ArgumentException("The config provider containy an empty server name");

            return serverName;
        }

        private string GetUserName(IConfigProvider configProvider_)
        {
            if (!configProvider_.Contains(ConfigKeys.UserName))
                throw new ArgumentException("The config provider does not have a username specified: " + ConfigKeys.UserName);

            var userName = configProvider_.GetConfigAsString(ConfigKeys.UserName);
            if (null == userName)
                throw new ArgumentException("The config provider contains a null usernmae");

            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentException("The config provider contains an empty server name");

            return userName;
        }

        private string GetPassword(IConfigProvider configProvider_)
        {
            if (!configProvider_.Contains(ConfigKeys.Password))
                throw new ArgumentException("The config provider does not have a password specified: " + ConfigKeys.Password);

            var password = configProvider_.GetConfigAsString(ConfigKeys.Password);
            if (null == password)
                throw new ArgumentException("The config provider contains a null password");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("The config provider contains an empty password");

            return password;
        }

        private bool GetEnableSsl(IConfigProvider configProvider_)
        {
            if (!configProvider_.Contains(ConfigKeys.EnableSsl))
                return false;

            return configProvider_.GetConfigAsBool(ConfigKeys.EnableSsl);
        }
    }
}
