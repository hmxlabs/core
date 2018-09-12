using HmxLabs.Core.Log;
using NUnit.Framework;

namespace HmxLabs.Core.Tests.Log
{
    [TestFixture]
    public class LogLevelTests
    {
        [Test]
        public void TestDebugLevelConversion()
        {
            Assert.That(LogLevel.Debug.ToLogString(), Is.EqualTo(LogLevelStrings.Debug));
        }

        [Test]
        public void TestInformationLevelConversion()
        {
            Assert.That(LogLevel.Information.ToLogString(), Is.EqualTo(LogLevelStrings.Information));
        }

        [Test]
        public void TestNoticeLevelConverstion()
        {
            Assert.That(LogLevel.Notice.ToLogString(), Is.EqualTo(LogLevelStrings.Notice));
        }

        [Test]
        public void TestWarningLevelConverstion()
        {
            Assert.That(LogLevel.Warning.ToLogString(), Is.EqualTo(LogLevelStrings.Warning));
        }

        [Test]
        public void TestErrorLevelConversion()
        {
            Assert.That(LogLevel.Error.ToLogString(), Is.EqualTo(LogLevelStrings.Error));
        }

        [Test]
        public void TestCriticalLevelConversion()
        {
            Assert.That(LogLevel.Critical.ToLogString(), Is.EqualTo(LogLevelStrings.Critical));
        }

        [Test]
        public void TestFatalLevelConversion()
        {
            Assert.That(LogLevel.Fatal.ToLogString(), Is.EqualTo(LogLevelStrings.Fatal));
        }
    }
}
