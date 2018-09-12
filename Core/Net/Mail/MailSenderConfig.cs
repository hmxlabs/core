using System;
using System.Net;
using System.Net.Mail;
using HmxLabs.Core.Config;

namespace HmxLabs.Core.Net.Mail
{
    /// <summary>
    /// A concrete implementation of the <code>IMailSender</code> interface.
    /// 
    /// This implementation derives from <code>SmtoConfig</code> and as such
    /// all the relevant documentation for that class also applies here
    /// 
    /// Similar to the <code>SmtpConfig</code> class, the <code>MailSenderConfig</code>
    /// can also read its configuration from an <code>IConfigProvider</code>. This is as
    /// per <code>SmtpConfig</code> for any items that are inherited and also the following
    /// additional keys
    /// 
    /// Sender Address: "mail.sender.address"
    /// Sender Name: "mail.sender.name"
    /// 
    /// These values can be found in the nested class <code>MailSenderConfigKeys</code>
    /// </summary>
    public class MailSenderConfig : SmtpConfig, IMailSenderConfig
    {
        /// <summary>
        /// The key values used when extracting the required configuration information
        /// to construct a <code>MailSenderConfig</code>
        /// </summary>
        public static class MailSenderConfigKeys
        {
            /// <summary>
            /// The key specifying the config for the email address of the sender
            /// </summary>
            public const string SenderAddress = "mail.sender.address";

            /// <summary>
            /// The key specifying the config for the sender's human readable name
            /// </summary>
            public const string SenderName = "mail.sender.name";
        }

        /// <summary>
        /// Construct an instance of this class with the specified server name and sender email address
        /// 
        /// A default value will be used for the Port, no network credentials will be used and no sender
        /// same will be set on the mail message
        /// </summary>
        /// <exception cref="ArgumentNullException">If the sender or server is null</exception>
        /// <exception cref="ArgumentException">If the server is empty or whitespace</exception>
        /// <param name="server_">The server name to connect to</param>
        /// <param name="sender_">The sender's mail address</param>
        public MailSenderConfig(string server_, MailAddress sender_) : base(server_)
        {
            if (null == sender_)
                throw new ArgumentNullException(nameof(sender_));

            _sender = sender_;
        }

        /// <summary>
        /// Construct an instance of this class with the specified server, network credential and sender data
        /// 
        /// A default port number will be used as specified in <code>SmtpConfig.DefaultPort</code>
        /// </summary>
        /// <param name="server_">The server name to connect to</param>
        /// <param name="credential_">The username and password to provide to the server</param>
        /// <param name="sender_">THe sender name and address to use</param>
        public MailSenderConfig(string server_, NetworkCredential credential_, MailAddress sender_) : base(server_, credential_)
        {
            if (null == sender_)
                throw new ArgumentNullException(nameof(sender_));

            _sender = sender_;
        }

        /// <summary>
        /// Fully qualified constructor.
        /// </summary>
        /// <param name="server_">The server name to connect to</param>
        /// <param name="userCredentials_">The username and password to provide to the server</param>
        /// <param name="sender_">THe sender name and address to use</param>
        /// <param name="port_">The port that the SMTP server is listening on</param>
        /// <param name="enableSsl_">Whether or not SSL/TLS should be used. Defaults to false</param>
        public MailSenderConfig(string server_, int port_, NetworkCredential userCredentials_, MailAddress sender_, bool enableSsl_ = false) : base(server_, port_, userCredentials_, enableSsl_)
        {
            if (null == sender_)
                throw new ArgumentNullException(nameof(sender_));

            _sender = sender_;
        }

        /// <summary>
        /// Constructs an instance of <code>SmtpConfig</code> and initialises all the necessary values
        /// by reading the configuration from the provided <code>IConfigProvider</code>.
        /// 
        /// The configuration keys it expects to find are in the nexted classes <code>SmtpConfig.ConfigKeys</code>
        /// and <code>MailSenderConfig.ConfigKeys</code>
        /// 
        /// ServerName: "smtp.server"
        /// Port: "smtp.port"
        /// Username: "smtp.username"
        /// Password: "smtp.password"
        /// Sender Address: "mail.sender.address"
        /// Sender Name: "mail.sender.name"
        /// </summary>
        /// <param name="configProvider_"></param>
        public MailSenderConfig(IConfigProvider configProvider_) : base(configProvider_)
        {
            if (null == configProvider_)
                throw new ArgumentNullException(nameof(configProvider_));

            var senderAddress = GetSenderAddress(configProvider_);
            var senderName = GetSenderName(configProvider_);
            _sender = new MailAddress(senderAddress, senderName);
        }

        /// <summary>
        /// Read only property providing the sender's email address and name via a <code>MailAddress</code>
        /// </summary>
        public MailAddress Sender => _sender;

        /// <summary>
        /// Read only property providing the sender's email address. This is equivalent to
        /// <code>Sender.Address</code>
        /// </summary>
        public string SenderAddress => _sender.Address;

        /// <summary>
        /// Read only property providing the sender's name. This is equivalent to
        /// <code>Sender.DisplayName</code>
        /// </summary>
        public string SenderName => _sender.DisplayName;

        private string GetSenderAddress(IConfigProvider configProvider_)
        {
            if (!configProvider_.Contains(MailSenderConfigKeys.SenderAddress))
                throw new ArgumentException("The config provider does not have a sender address specified: " + MailSenderConfigKeys.SenderAddress);

            var senderAddress = configProvider_.GetConfigAsString(MailSenderConfigKeys.SenderAddress);
            if (null == senderAddress)
                throw new ArgumentException("The config provider contains a null sender address");

            if (string.IsNullOrWhiteSpace(senderAddress))
                throw new ArgumentException("The config provider containy an empty sender address");

            return senderAddress;
        }

        private string GetSenderName(IConfigProvider configProvider_)
        {
            if (!configProvider_.Contains(MailSenderConfigKeys.SenderName))
                throw new ArgumentException("The config provider does not have a sender name specified: " + MailSenderConfigKeys.SenderName);

            var senderName = configProvider_.GetConfigAsString(MailSenderConfigKeys.SenderName);
            if (null == senderName)
                throw new ArgumentException("The config provider contains a null sender name");

            if (string.IsNullOrWhiteSpace(senderName))
                throw new ArgumentException("The config provider containy an empty sender name");

            return senderName;
        }

        private readonly MailAddress _sender;
    }
}
