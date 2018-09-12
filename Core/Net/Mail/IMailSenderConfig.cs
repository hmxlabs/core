using System.Net.Mail;

namespace HmxLabs.Core.Net.Mail
{
    /// <summary>
    /// An extension to <code>ISmtpConfig</code> that in addition to providing
    /// the configuration required to connect to an SMTP server provides
    /// the additional information required to send an email via the STMP server
    /// </summary>
    public interface IMailSenderConfig : ISmtpConfig
    {
        /// <summary>
        /// Ths <code>MailAddress</code> to send the email from.
        /// <code>Sender.Address</code> is identical to <code>SenderAddress</code> and
        /// <code>Sender.DisplayName</code> is identical to <code>SenderName</code>
        /// </summary>
        MailAddress Sender { get; }

        /// <summary>
        /// Equivalent to <code>Sender.Address</code>. This is a convencience property
        /// for shorthand.
        /// </summary>
        string SenderAddress { get; }

        /// <summary>
        /// Equivalent to <code>Sender.DisplayName</code>. This is a convenience property
        /// for short hand
        /// </summary>
        string SenderName { get; }
    }
}
