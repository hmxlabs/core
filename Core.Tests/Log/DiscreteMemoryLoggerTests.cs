using System;
using System.Collections.Generic;
using System.Linq;
using HmxLabs.Core.Log;
using NUnit.Framework;

namespace HmxLabs.Core.Tests.Log
{
    [TestFixture]
    public class DiscreteMemoryLoggerTests
    {
        [Test]
        public void TestAllLogMessagesAreEmptyOnConstruction()
        {
            var logger = new DiscreteMemoryLogger("test");

            foreach (var level in GetLogLevels())
            {
                Assert.That(logger.LogMessages[level], Is.Empty);
                Assert.That(logger.LogExceptions[level], Is.Empty);
            }
        }

        [Test]
        public void TestDebugLogging()
        {
            var logger = new DiscreteMemoryLogger("test");
            logger.Debug("test log");
            logger.Debug("test log with param {0}", 1000);
            logger.Debug(new Exception("test exception"), "test log");
            logger.Debug(new Exception("test exception"), "test log with params {0}", 9999);
            CheckLoggerContainsExpectedMessages(logger, LogLevel.Debug);
        }
        
        [Test]
        public void TestInfoLogging()
        {
            var logger = new DiscreteMemoryLogger("test");
            logger.Info("test log");
            logger.Info("test log with param {0}", 1000);
            logger.Info(new Exception("test exception"), "test log");
            logger.Info(new Exception("test exception"), "test log with params {0}", 9999);
            CheckLoggerContainsExpectedMessages(logger, LogLevel.Information);
        }

        [Test]
        public void TestNoticeLogging()
        {
            var logger = new DiscreteMemoryLogger("test");
            logger.Notice("test log");
            logger.Notice("test log with param {0}", 1000);
            logger.Notice(new Exception("test exception"), "test log");
            logger.Notice(new Exception("test exception"), "test log with params {0}", 9999);
            CheckLoggerContainsExpectedMessages(logger, LogLevel.Notice);
        }

        [Test]
        public void TestWarningLogging()
        {
            var logger = new DiscreteMemoryLogger("test");
            logger.Warning("test log");
            logger.Warning("test log with param {0}", 1000);
            logger.Warning(new Exception("test exception"), "test log");
            logger.Warning(new Exception("test exception"), "test log with params {0}", 9999);
            CheckLoggerContainsExpectedMessages(logger, LogLevel.Warning);
        }

        [Test]
        public void TestErrorLogging()
        {
            var logger = new DiscreteMemoryLogger("test");
            logger.Error("test log");
            logger.Error("test log with param {0}", 1000);
            logger.Error(new Exception("test exception"), "test log");
            logger.Error(new Exception("test exception"), "test log with params {0}", 9999);
            CheckLoggerContainsExpectedMessages(logger, LogLevel.Error);
        }

        [Test]
        public void TestCriticalLogging()
        {
            var logger = new DiscreteMemoryLogger("test");
            logger.Critical("test log");
            logger.Critical("test log with param {0}", 1000);
            logger.Critical(new Exception("test exception"), "test log");
            logger.Critical(new Exception("test exception"), "test log with params {0}", 9999);
            CheckLoggerContainsExpectedMessages(logger, LogLevel.Critical);
        }

        [Test]
        public void TestFataLogging()
        {
            var logger = new DiscreteMemoryLogger("test");
            logger.Fatal("test log");
            logger.Fatal("test log with param {0}", 1000);
            logger.Fatal(new Exception("test exception"), "test log");
            logger.Fatal(new Exception("test exception"), "test log with params {0}", 9999);
            CheckLoggerContainsExpectedMessages(logger, LogLevel.Fatal);
        }

        private static void CheckLoggerContainsExpectedMessages(DiscreteMemoryLogger logger_, LogLevel level_)
        {
            Assert.That(logger_.LogMessages[level_], Has.Count.EqualTo(4));
            Assert.That(logger_.LogExceptions[level_], Has.Count.EqualTo(2));

            logger_.Clear();
            Assert.That(logger_.LogMessages[level_], Is.Empty);
            Assert.That(logger_.LogExceptions[level_], Is.Empty);
        }

        private static IEnumerable<LogLevel> GetLogLevels()
        {
            return Enum.GetValues(typeof (LogLevel)).Cast<LogLevel>();
        }
    }
}
