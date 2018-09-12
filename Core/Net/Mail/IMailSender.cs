namespace HmxLabs.Core.Net.Mail
{
    /// <summary>
    /// An extension to the ISmtpClient interface that also
    /// provides access to the SMTP client used and the
    /// <code>IMailSenderConfig</code> in use to send the mail messages
    /// </summary>
    public interface IMailSender : ISmtpClient, IMailSenderConfig
    {
        /// <summary>
        /// The <code>ISmtpClient</code> that will be used to send the message
        /// </summary>
        ISmtpClient SmtpClient { get; }
    }
}