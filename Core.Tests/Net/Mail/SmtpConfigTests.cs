using System;
using System.Globalization;
using System.Net;
using HmxLabs.Core.Config;
using HmxLabs.Core.Net.Mail;
using NUnit.Framework;
// ReSharper disable ObjectCreationAsStatement -- we create a lot of unassigned objects in argument guard tests

namespace HmxLabs.Core.Tests.Net.Mail
{
    [TestFixture]
    public class SmtpConfigTests
    {
        [Test]
        public void TestFullyQualifiedConstructorWithNetworkCredentialArgumentGuards()
        {
            Assert.Throws<ArgumentNullException>(() => new SmtpConfig(null, Port, Credential));
            Assert.Throws<ArgumentException>(() => new SmtpConfig("", Port, Credential));
        }

        [Test]
        public void TestMinimalConstructorArgumentGuards()
        {
            Assert.Throws<ArgumentNullException>(() => new SmtpConfig((string)null));
            Assert.Throws<ArgumentException>(() => new SmtpConfig(""));
        }

        [Test]
        public void TestConfigProviderConstructorArgumentGuards()
        {
            Assert.Throws<ArgumentNullException>(() => new SmtpConfig((IConfigProvider)null));
            var configProvider = new FixedConfigProvider();
            Assert.Throws<ArgumentException>(() => new SmtpConfig(configProvider));
        }

        [Test]
        public void TestMinimalConstructorWithNetworkCredentialArgumentGuards()
        {
            Assert.Throws<ArgumentNullException>(() => new SmtpConfig(null, Credential));
            Assert.Throws<ArgumentException>(() => new SmtpConfig("", Credential));
        }

        [Test]
        public void TestConstructorWithUsernamePasswordArgumentGuards()
        {
            Assert.Throws<ArgumentNullException>(() => new SmtpConfig(null, Port, Username, Password));
            Assert.Throws<ArgumentNullException>(() => new SmtpConfig(ServerName, Port, null, Password));
            Assert.Throws<ArgumentNullException>(() => new SmtpConfig(ServerName, Port, Username, null));

            Assert.Throws<ArgumentException>(() => new SmtpConfig("", Port, Username, Password));
            Assert.Throws<ArgumentException>(() => new SmtpConfig(ServerName, Port, "", Password));
            Assert.Throws<ArgumentException>(() => new SmtpConfig(ServerName, Port, Username, ""));
        }

        [Test]
        public void TestConstructor()
        {
            var smtpConfig = new SmtpConfig(ServerName, Port, Username, Password, true);
            CheckSmtpConfig(smtpConfig);

            smtpConfig = new SmtpConfig(ServerName, Port, Credential, true);
            CheckSmtpConfig(smtpConfig);
        }

        [Test]
        public void TestConstructorWithNullCredential()
        {
            var smtpConfig = new SmtpConfig(ServerName, Port, null);
            Assert.That(smtpConfig.UserCredentials, Is.Null);
            Assert.That(smtpConfig.Username, Is.Null);
            Assert.That(smtpConfig.Password, Is.Null);
        }

        [Test]
        public void TestConstructorWithConfigProvider()
        {
            var configProvider = new FixedConfigProvider();
            configProvider.AddConfig(SmtpConfig.ConfigKeys.ServerName, ServerName);
            configProvider.AddConfig(SmtpConfig.ConfigKeys.Port, Port.ToString(CultureInfo.InvariantCulture));
            configProvider.AddConfig(SmtpConfig.ConfigKeys.UserName, Username);
            configProvider.AddConfig(SmtpConfig.ConfigKeys.Password, Password);
            configProvider.AddConfig(SmtpConfig.ConfigKeys.EnableSsl, EnableSsl.ToString());
            var smtpConfig = new SmtpConfig(configProvider);
            CheckSmtpConfig(smtpConfig);
        }

        private void CheckSmtpConfig(SmtpConfig config_)
        {
            Assert.That(config_.ServerName, Is.EqualTo(ServerName));
            Assert.That(config_.Port, Is.EqualTo(Port));
            Assert.That(config_.UserCredentials, Is.Not.Null);
            Assert.That(config_.UserCredentials.UserName, Is.EqualTo(Username));
            Assert.That(config_.UserCredentials.Password, Is.EqualTo(Password));
            Assert.That(config_.Username, Is.EqualTo(Username));
            Assert.That(config_.Password, Is.EqualTo(Password));
            Assert.That(config_.EnableSsl, Is.EqualTo(EnableSsl));
        }

        private const string ServerName = "serverName";
        private const int Port = 100;
        private const string Username = "username";
        private const string Password = "password";
        private const bool EnableSsl = true;
        private static readonly NetworkCredential Credential = new NetworkCredential(Username, Password);
    }
}
