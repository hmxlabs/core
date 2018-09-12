using System;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using HmxLabs.Core.Config;
using HmxLabs.Core.Net.Mail;
using NUnit.Framework;
// ReSharper disable ObjectCreationAsStatement -- we create a lot of unassigned objects in argument guard tests

namespace HmxLabs.Core.Tests.Net.Mail
{
    [TestFixture]
    public class MailSenderConfigTests
    {
        [Test]
        public void TestMinimalConstructorArgumentGuards()
        {
            Assert.Throws<ArgumentNullException>(() => new MailSenderConfig(null, Sender));
            Assert.Throws<ArgumentException>(() => new MailSenderConfig("  ", Sender));
            Assert.Throws<ArgumentNullException>(() => new MailSenderConfig(ServerName, null));
        }

        [Test]
        public void TestNetworkCredentialConstructorArgumentGuards()
        {
            Assert.Throws<ArgumentNullException>(() => new MailSenderConfig(null, Credential, Sender));
            Assert.Throws<ArgumentException>(() => new MailSenderConfig("", Credential, Sender));
            Assert.Throws<ArgumentNullException>(() => new MailSenderConfig(ServerName, Credential, null));
        }

        [Test]
        public void TestFullyQualifiedConstructorArgumentGuards()
        {
            Assert.Throws<ArgumentNullException>(() => new MailSenderConfig(null, Port, Credential, Sender));
            Assert.Throws<ArgumentException>(() => new MailSenderConfig("  ", Port, Credential, Sender));
            Assert.Throws<ArgumentNullException>(() => new MailSenderConfig(ServerName, Port, Credential, null));
        }

        [Test]
        public void TestConfigProviderConstructorArgumentGuards()
        {
            Assert.Throws<ArgumentNullException>(() => new MailSenderConfig(null));
        }

        [Test]
        public void TestMinimalConstructor()
        {
            var config = new MailSenderConfig(ServerName, Sender);
            Assert.That(config.ServerName, Is.EqualTo(ServerName));
            Assert.That(config.Sender, Is.EqualTo(Sender));
            Assert.That(config.UserCredentials, Is.Null);
            Assert.That(config.Port, Is.EqualTo(SmtpConfig.DefaultPort));
        }

        [Test]
        public void TestNetworkCredentialConstructor()
        {
            var config = new MailSenderConfig(ServerName, Credential, Sender);
            Assert.That(config.ServerName, Is.EqualTo(ServerName));
            Assert.That(config.Sender, Is.EqualTo(Sender));
            Assert.That(config.UserCredentials, Is.EqualTo(Credential));
            Assert.That(config.Port, Is.EqualTo(SmtpConfig.DefaultPort));
        }

        [Test]
        public void TestFullyQualifiedConstructor()
        {
            var config = new MailSenderConfig(ServerName, Port, Credential, Sender);
            Assert.That(config.ServerName, Is.EqualTo(ServerName));
            Assert.That(config.Sender, Is.EqualTo(Sender));
            Assert.That(config.UserCredentials, Is.EqualTo(Credential));
            Assert.That(config.Port, Is.EqualTo(Port));
        }

        [Test]
        public void TestConfigProviderConstructor()
        {
            var configProvider = new FixedConfigProvider();
            configProvider.AddConfig(MailSenderConfig.MailSenderConfigKeys.SenderName, SenderName);
            configProvider.AddConfig(MailSenderConfig.MailSenderConfigKeys.SenderAddress, SenderAddress);
            configProvider.AddConfig(SmtpConfig.ConfigKeys.ServerName, ServerName);
            configProvider.AddConfig(SmtpConfig.ConfigKeys.Port, Port.ToString(CultureInfo.InvariantCulture));
            configProvider.AddConfig(SmtpConfig.ConfigKeys.UserName, Username);
            configProvider.AddConfig(SmtpConfig.ConfigKeys.Password, Password);
            configProvider.AddConfig(SmtpConfig.ConfigKeys.EnableSsl, EnableSsl.ToString());

            var config = new MailSenderConfig(configProvider);
            Assert.That(config.ServerName, Is.EqualTo(ServerName));
            Assert.That(config.Port, Is.EqualTo(Port));
            Assert.That(config.UserCredentials, Is.Not.Null);
            Assert.That(config.UserCredentials.UserName, Is.EqualTo(Username));
            Assert.That(config.UserCredentials.Password, Is.EqualTo(Password));
            Assert.That(config.Username, Is.EqualTo(Username));
            Assert.That(config.Password, Is.EqualTo(Password));
            Assert.That(config.Sender, Is.Not.Null);
            Assert.That(config.Sender.Address, Is.EqualTo(SenderAddress));
            Assert.That(config.Sender.DisplayName, Is.EqualTo(SenderName));
            Assert.That(config.SenderName, Is.EqualTo(SenderName));
            Assert.That(config.SenderAddress, Is.EqualTo(SenderAddress));
            Assert.That(config.EnableSsl, Is.EqualTo(EnableSsl));
        }

        private const string ServerName = "ServerName";
        private const int Port = 123;
        private const string Username = "username";
        private const string Password = "password";
        private static readonly NetworkCredential Credential = new NetworkCredential(Username, Password);
        private const string SenderAddress = "dummy@address.com";
        private const string SenderName = "Dummy Sender";
        private const bool EnableSsl = true;
        private static readonly MailAddress Sender = new MailAddress(SenderAddress, SenderName);
    }
}
