using System;
using HmxLabs.Core.Config;
using HmxLabs.Core.Log;
using NUnit.Framework;
// ReSharper disable ObjectCreationAsStatement -- expected due to the argument guard tests

namespace HmxLabs.Core.Tests.Log
{
    [TestFixture]
    public class LogConfigTests
    {
        [Test]
        public void TestConstructorArgumentGuards()
        {
            Assert.Throws<ArgumentNullException>(() => new LogConfig(null, null, LoggerType.Console));
            Assert.Throws<ArgumentNullException>(() => new LogConfig("sdfds", "sdfsdf", null));
        }

        [Test]
        public void TestNullLocationIsOk()
        {
            const string name = "myName";
            var logConfig = new LogConfig(name, null, LoggerType.File);
            Assert.That(logConfig, Is.Not.Null);
            Assert.That(logConfig.Type, Is.EqualTo(LoggerType.File));
            Assert.That(logConfig.Name, Is.EqualTo(name));
        }

        [Test]
        public void TestConstructionWithConfigProvider()
        {
            const string name = "myLoggerName";
            const string location = "myLogLocation";
            const string type = "myLogType";
            var configProvider = new FixedConfigProvider();
            configProvider.AddConfig(LogConfig.ConfigKeys.Name, name);
            configProvider.AddConfig(LogConfig.ConfigKeys.Location, location);
            configProvider.AddConfig(LogConfig.ConfigKeys.Type, type);

            var logConfig = new LogConfig(configProvider);
            Assert.That(logConfig.Name, Is.EqualTo(name));
            Assert.That(logConfig.Type, Is.EqualTo(type));
            Assert.That(logConfig.Location, Is.EqualTo(location));
        }
    }
}
