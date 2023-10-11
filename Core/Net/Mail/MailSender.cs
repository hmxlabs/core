using System;
using System.ComponentModel;
using System.Net;
using System.Net.Mail;

namespace HmxLabs.Core.Net.Mail
{
    /// <summary>
    /// An implementation of the <code>IMailSender</code> interface. Please see <code>IMailSender</code>.
    /// 
    /// This is an encapsulation around an <code>ISmtpClient</code> instance, please also see this class
    /// for further information.
    /// </summary>
    public class MailSender : IMailSender
    {
        /// <summary>
        /// Constructs an instance of this class using the provided <code>ISmtpClient</code>
        /// and <code>MailSenderConfiguration</code>.
        /// </summary>
        /// <param name="config_">The configuration information. If an <code>ISmtpClient</code> is not provided this must contain the necessary config to create one</param>
        /// <param name="smtpClient_">The SMTP client to use or null to create and use an instance of <code>SmtpClient</code></param>
        public MailSender(IMailSenderConfig config_, ISmtpClient smtpClient_ = null)
        {
            if (null == config_)
                throw new ArgumentNullException(nameof(config_));

            _smtpClient = smtpClient_;
            _config = config_;

            if (null == _smtpClient)
                _smtpClient = new SmtpClient(config_);

            _smtpClient.SendCompleted += OnSendCompleted;
        }

        /// <summary>
        /// Destructor. Should never need to be called as this object should be disposed of. Will only be invoked
        /// if the Dispose method was not called when.
        /// </summary>
        ~MailSender()
        {
            Dispose(false);
        }

        /// <summary>
        /// Event raised when the sending of an email is complete
        /// </summary>
        public event SendCompletedEventHandler SendCompleted;

        /// <summary>
        /// The serve name this client is connected to
        /// </summary>
        public string ServerName => _config.ServerName;

        /// <summary>
        /// The port on which the SMTP server is listening.
        /// </summary>
        public int Port => _config.Port;

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
        /// The username and password credentials required to connect to the SMTP server.
        /// When provided these are analagous to the <code>Username</code> and
        /// <code>Password</code> fields
        /// </summary>
        public NetworkCredential UserCredentials => _config.UserCredentials;

        /// <summary>
        /// Ths <code>MailAddress</code> to send the email from.
        /// <code>Sender.Address</code> is identical to <code>SenderAddress</code> and
        /// <code>Sender.DisplayName</code> is identical to <code>SenderName</code>
        /// </summary>
        public MailAddress Sender => _config.Sender;

        /// <summary>
        /// Equivalent to <code>Sender.Address</code>. This is a convenience property
        /// for shorthand.
        /// </summary>
        public string SenderAddress => _config.SenderAddress;

        /// <summary>
        /// Equivalent to <code>Sender.DisplayName</code>. This is a convenience property
        /// for short hand
        /// </summary>
        public string SenderName => _config.SenderName;

        /// <summary>
        /// Whether to enable SSL/TLS for the connection to the SMTP server
        /// </summary>
        public bool EnableSsl => _config.EnableSsl;

        /// <summary>
        /// Read only property providing the <code>ISmtpClient</code> that is used to actually
        /// send the email
        /// </summary>
        public ISmtpClient SmtpClient => _smtpClient;

        /// <summary>
        /// Please see documentation for <code>IDisposable</code>
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Send the provided <code>MailMessage</code> using the
        /// SMTP server that this client object is connected to.
        /// 
        /// THe method will block till such time that the message has been transmitted
        /// to the SMTP server in its entirety and a response received.
        /// </summary>
        /// <exception cref="ArgumentNullException">If a <code>null</code> mail message is provided</exception>
        /// <param name="message_">The mail message to send</param>
        public void Send(MailMessage message_)
        {
            if (null == message_)
                throw new ArgumentNullException(nameof(message_));

            message_.Sender = Sender;
            message_.From = Sender; // From needs to be set... not sure what the difference between .From and .Sender is though...
            _smtpClient.Send(message_);
        }

        /// <summary>
        /// Sends the provided <code>MailMessage</code> asynchronously.
        /// This method call will return immediately and the object
        /// will subsequently continue to transmit the Mail Message
        /// to the SMTP server.
        /// </summary>
        /// <param name="message_">The mail message to send</param>
        /// <param name="userState_">Any state that the caller wishes to preserve to identify this call</param>
        public void SendAsync(MailMessage message_, object userState_)
        {
            if (null == message_)
                throw new ArgumentNullException(nameof(message_));

            message_.Sender = Sender;
            _smtpClient.SendAsync(message_, userState_);
        }

        /// <summary>
        /// Cancel sending the Mail Message that is currently in progress
        /// </summary>
        public void SendAsyncCancel()
        {
            _smtpClient.SendAsyncCancel();
        }

        /// <summary>
        /// Follows the standard .NET disposing pattern. THe bulk of the work to actually
        /// dispose of the object is done here.
        /// </summary>
        /// <param name="disposing_"></param>
        protected virtual void Dispose(bool disposing_)
        {
            if (!disposing_)
                return;

            if (null == _smtpClient)
                return;

            _smtpClient.SendCompleted -= OnSendCompleted;
            _smtpClient.Dispose();
            _smtpClient = null;
        }

        /// <summary>
        /// Invokes the SendCompleted event.
        /// </summary>
        /// <param name="sender_">The entity invoking the event</param>
        /// <param name="args_">The completed event args</param>
        protected virtual void OnSendCompleted(object sender_, AsyncCompletedEventArgs args_)
        {
            SendCompleted?.Invoke(sender_, args_);
        }

        private ISmtpClient _smtpClient;
        private readonly IMailSenderConfig _config;

    }
}
