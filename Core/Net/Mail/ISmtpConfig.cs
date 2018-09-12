using System.Net;

namespace HmxLabs.Core.Net.Mail
{
    /// <summary>
    /// Interface providing access to all the configuration required to connect to an SMTP server
    /// </summary>
    public interface ISmtpConfig
    {
        /// <summary>
        /// Read only property providing the server name to connect to
        /// </summary>
        string ServerName { get; }

        /// <summary>
        /// Read only property providing the port to use
        /// </summary>
        int Port { get; }

        /// <summary>
        /// Read only property providing the username to use.
        /// When provided this is analagous to <code>UserCredential.UserName</code> and this
        /// is simply a convenience property as a short hand
        /// </summary>
        string Username { get; }
        
        /// <summary>
        /// Read only property providing the password to use.
        /// When provided, this is analagous to <code>UserCredential.Password</code> and this
        /// is simply a convenience property as a short hand
        /// </summary>
        string Password { get; }

        /// <summary>
        /// Read only property specifying if SSL (or TLS) should be used.
        /// </summary>
        bool EnableSsl { get; }

        /// <summary>
        /// The username and passowrd credentials required to connect to the SMTP server.
        /// When provided these are analagous to the <code>Username</code> and
        /// <code>Passowrd</code> fields
        /// </summary>
        NetworkCredential UserCredentials { get; }
    }
}
