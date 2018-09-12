using System;
using System.Net.Mail;

namespace HmxLabs.Core.Net.Mail
{
    /// <summary>
    /// An interface that represents a connection to an SMTP server
    /// and exposes methods to send an email.
    /// 
    /// This code is old and pre-dates the TPL and should really
    /// be refactored to return a <code>Task</code> from the
    /// async mathod and use the TPL under the covers to peform
    /// the actions in an async manner.
    /// 
    /// The primary value of this interface however is to facilitate
    /// testability of the code.
    /// </summary>
    public interface ISmtpClient : ISmtpConfig, IDisposable
    {
        /// <summary>
        /// Send the provided <code>MailMessage</code> using the
        /// SMTP server that this client object is connected to.
        /// 
        /// THe method will block till such time that the message has been transmitted
        /// to the SMTP server in its entirety and a response received.
        /// </summary>
        /// <exception cref="ArgumentNullException">If a <code>null</code> mail message is provided</exception>
        /// <param name="message_">The mail message to send</param>
        void Send(MailMessage message_);

        /// <summary>
        /// Sends the provided <code>MailMessage</code> asynchronously.
        /// This method call will return immediately and the object
        /// will subsequently continue to transmit the Mail Message
        /// to the SMTP server.
        /// </summary>
        /// <param name="message_">The mail message to send</param>
        /// <param name="userState_">Any state that the caller wishes to preserve to identify this call</param>
        void SendAsync(MailMessage message_, object userState_);

        /// <summary>
        /// Cancel sending the Mail Message that is currently in progress
        /// </summary>
        void SendAsyncCancel();

        /// <summary>
        /// An event raised after a call to <code>SendAsync</code> completes.
        /// Register with this event to receive notification of message
        /// sending completion.
        /// </summary>
        event SendCompletedEventHandler SendCompleted;
    }
}
