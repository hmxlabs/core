using System;
using System.Net;

namespace HmxLabs.Core.Net.Mail
{
    /// <summary>
    /// An implementation of the the <code>ISmtpClient</code> interface
    /// that actually is just the <code>System.Net.Mail.SmtpClient</code>
    /// but exposes the additional properties required to fulfull <code>ISmtpClient</code>
    /// </summary>
    public class SmtpClient : System.Net.Mail.SmtpClient, ISmtpClient
    {
        /// <summary>
        /// Constuct an instance of the <code>ISmtpClient</code> using the
        /// provided configuration.
        /// </summary>
        /// <exception cref="ArgumentNullException">If the provided configuration is <code>null</code></exception>
        /// <param name="config_">The configuration information required</param>
        public SmtpClient(ISmtpConfig config_)
        {
            if (null == config_)
                throw new ArgumentNullException(nameof(config_));

            Host = config_.ServerName;
            Port = config_.Port;
            Credentials = config_.UserCredentials;
            EnableSsl = config_.EnableSsl;
            _config = config_;
        }

        /// <summary>
        /// The serve name this client is connected to
        /// </summary>
        public string ServerName => _config.ServerName;

        /// <summary>
        /// The username this client provided to the SMTP server. This is a convenience method that
        /// equates to <code>UserCredentials.Username</code>
        /// </summary>
        public string Username => _config.Username;

        /// <summary>
        /// The password this client presented to the SMTP server. This is a convenience methods that
        /// equates to <code>UserCredential.Password</code>
        /// </summary>
        public string Password => _config.Password;

        /// <summary>
        /// Ths user credentials that were provided to the SMTP server
        /// </summary>
        public NetworkCredential UserCredentials => _config.UserCredentials;

        private readonly ISmtpConfig _config;
    }
}
