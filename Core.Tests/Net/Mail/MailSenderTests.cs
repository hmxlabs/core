using System;
using System.Net;
using System.Net.Mail;
using HmxLabs.Core.Net.Mail;
using NSubstitute;
using NUnit.Framework;

namespace HmxLabs.Core.Tests.Net.Mail
{
    [TestFixture]
    public class MailSenderTests
    {
        [Test]
        public void TestConstructorArgumentGuards()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new MailSender(null));
        }

        [Test]
        public void TestSendArgumentGuards()
        {
            var smtpClient = Substitute.For<ISmtpClient>();
            var mailSender = new MailSender(MailSenderConfig, smtpClient);
            Assert.Throws<ArgumentNullException>(() => mailSender.Send(null));
            Assert.Throws<ArgumentNullException>(() => mailSender.SendAsync(null, new object()));
        }

        [Test]
        public void TestSendSetsSenderandFromOnMessage()
        {
            var smtpClient = Substitute.For<ISmtpClient>();
            var mailSender = new MailSender(MailSenderConfig, smtpClient);
            var message = new MailMessage();
            message.To.Add(new MailAddress("person@somehwere.com"));
            Assert.That(message.From, Is.Null);
            Assert.That(message.Sender, Is.Null);

            mailSender.Send(message);

            Assert.That(message.From, Is.Not.Null);
            Assert.That(message.Sender, Is.Not.Null);

            Assert.That(message.From.Address, Is.EqualTo(MailSenderConfig.SenderAddress));
            Assert.That(message.Sender.Address, Is.EqualTo(MailSenderConfig.SenderAddress));
            Assert.That(message.From.DisplayName, Is.EqualTo(MailSenderConfig.SenderName));
            Assert.That(message.Sender.DisplayName, Is.EqualTo(MailSenderConfig.SenderName));
        }

        [Test]
        public void TestSendCallsThroughToSmtpClient()
        {
            var smtpClient = Substitute.For<ISmtpClient>();
            var mailSender = new MailSender(MailSenderConfig, smtpClient);
            var message = new MailMessage();
            message.To.Add(new MailAddress("person@somehwere.com"));
            mailSender.Send(message);
            smtpClient.Received(1).Send(message);

            mailSender.SendAsync(message, null);
            smtpClient.Received(1).SendAsync(message, null);
        }

        [Test]
        public void TestConstructor()
        {
            var smtpClient = Substitute.For<ISmtpClient>();
            var mailSender = new MailSender(MailSenderConfig, smtpClient);
            Assert.That(mailSender.SmtpClient, Is.EqualTo(smtpClient));
            Assert.That(mailSender.Sender, Is.EqualTo(Sender));

            mailSender = new MailSender(MailSenderConfig);
            Assert.That(mailSender.Sender, Is.EqualTo(Sender));
            Assert.That(mailSender.SmtpClient, Is.Not.Null);
            Assert.That(mailSender.ServerName, Is.EqualTo(Server));
            Assert.That(mailSender.Port, Is.EqualTo(Port));
            Assert.That(mailSender.Username, Is.EqualTo(Username));
            Assert.That(mailSender.Password, Is.EqualTo(Password));
            Assert.That(mailSender.UserCredentials.UserName, Is.EqualTo(Username));
            Assert.That(mailSender.UserCredentials.Password, Is.EqualTo(Password));
            Assert.That(mailSender.EnableSsl, Is.EqualTo(EnableSsl));
        }

        private const string Server = "smtp.server.com";
        private const string SenderAddress = "dummy@address.com";
        private const string SenderName = "Dummy Sender";
        private const int Port = 100;
        private const string Username = "username";
        private const string Password = "password";
        private const bool EnableSsl = true;
        private static readonly NetworkCredential Credential = new NetworkCredential(Username, Password);
        private static readonly MailAddress Sender = new MailAddress(SenderAddress, SenderName);
        private static readonly IMailSenderConfig MailSenderConfig = new MailSenderConfig(Server, Port, Credential, Sender, EnableSsl);
    }
}
