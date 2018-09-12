using System;
using HmxLabs.Core.Net.Mail;
using NUnit.Framework;

namespace HmxLabs.Core.Tests.Net.Mail
{
    [TestFixture]
    public class SmtpClientTests
    {
        [Test]
        public void TestConstructorArgumentGuards()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new SmtpClient(null));
        }

        [Test]
        public void TestConstructor()
        {
            var smtpConfig = new SmtpConfig(ServerName, Port, Username, Password, EnableSsl);
            var smtpClient = new SmtpClient(smtpConfig);
            
            Assert.That(smtpClient.ServerName, Is.EqualTo(ServerName));
            Assert.That(smtpClient.Port, Is.EqualTo(Port));
            Assert.That(smtpClient.Username, Is.EqualTo(Username));
            Assert.That(smtpClient.Password, Is.EqualTo(Password));
            Assert.That(smtpClient.UserCredentials.UserName, Is.EqualTo(Username));
            Assert.That(smtpClient.UserCredentials.Password, Is.EqualTo(Password));
            Assert.That(smtpClient.EnableSsl, Is.EqualTo(EnableSsl));
        }

        private const string ServerName = "serverName";
        private const int Port = 100;
        private const string Username = "username";
        private const string Password = "password";
        private const bool EnableSsl = true;
    }
}
