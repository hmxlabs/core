using System;
using System.IO;
using System.Text;
using HmxLabs.Core.Log;
using HmxLabs.Core.Tests.Base;
using NUnit.Framework;

namespace HmxLabs.Core.Tests.Log
{
    public delegate void LogTestLineDelegate(ILogger logger_);

    [TestFixture]
    public class StreamLoggerTests
    {
        public const string LoggerName = "testLogger";
        public static readonly DateTime LogDate = new DateTime(2001, 1, 1, 0, 0, 0);

        [Test]
        public void TestDebugLogging()
        {
            var expectedLog = "2001-01-01T00:00:00 DEBUG: HELLO" + Environment.NewLine;
            PerformLogTest(expectedLog, logger_ => logger_.Debug("HELLO"));
        }

        [Test]
        public void TestInfoLogging()
        {
            var expectedLog = "2001-01-01T00:00:00 INFO: HELLO" + Environment.NewLine;
            PerformLogTest(expectedLog, logger_ => logger_.Info("HELLO"));
        }

        [Test]
        public void TestNoticeLogging()
        {
            var expectedLog = "2001-01-01T00:00:00 NOTICE: HELLO" + Environment.NewLine;
            PerformLogTest(expectedLog, logger_ => logger_.Notice("HELLO"));
        }

        [Test]
        public void TestWarningLogging()
        {
            var expectedLog = "2001-01-01T00:00:00 WARNING: HELLO" + Environment.NewLine;
            PerformLogTest(expectedLog, logger_ => logger_.Warning("HELLO"));
        }

        [Test]
        public void TestErrorLogging()
        {
            var expectedLog = "2001-01-01T00:00:00 ERROR: HELLO" + Environment.NewLine;
            PerformLogTest(expectedLog, logger_ => logger_.Error("HELLO"));
        }

        [Test]
        public void TestCriticalLogging()
        {
            var expectedLog = "2001-01-01T00:00:00 CRITICAL: HELLO" + Environment.NewLine;
            PerformLogTest(expectedLog, logger_ => logger_.Critical("HELLO"));
        }

        [Test]
        public void TestFatalLogging()
        {
            var expectedLog = "2001-01-01T00:00:00 FATAL: HELLO" + Environment.NewLine;
            PerformLogTest(expectedLog, logger_ => logger_.Fatal("HELLO"));
        }

        [Test]
        public void TestDebugWithArgsLogging()
        {
            var expectedLog = "2001-01-01T00:00:00 DEBUG: HELLO 1 2 3" + Environment.NewLine;
            PerformLogTest(expectedLog, logger_ => logger_.Debug("HELLO {0} {1} {2}", 1, 2, 3));
        }

        [Test]
        public void TestInfoWithArgsLogging()
        {
            var expectedLog = "2001-01-01T00:00:00 INFO: HELLO 1 2 3" + Environment.NewLine;
            PerformLogTest(expectedLog, logger_ => logger_.Info("HELLO {0} {1} {2}", 1, 2, 3));
        }

        [Test]
        public void TestNoticeWithArgsLogging()
        {
            var expectedLog = "2001-01-01T00:00:00 NOTICE: HELLO 1 2 3" + Environment.NewLine;
            PerformLogTest(expectedLog, logger_ => logger_.Notice("HELLO {0} {1} {2}", 1, 2, 3));
        }

        [Test]
        public void TestWarningWithArgsLogging()
        {
            var expectedLog = "2001-01-01T00:00:00 WARNING: HELLO 1 2 3" + Environment.NewLine;
            PerformLogTest(expectedLog, logger_ => logger_.Warning("HELLO {0} {1} {2}", 1, 2, 3));
        }

        [Test]
        public void TestErrorWithArgsLogging()
        {
            var expectedLog = "2001-01-01T00:00:00 ERROR: HELLO 1 2 3" + Environment.NewLine;
            PerformLogTest(expectedLog, logger_ => logger_.Error("HELLO {0} {1} {2}", 1, 2, 3));
        }

        [Test]
        public void TestCricitalWithArgsLogging()
        {
            var expectedLog = "2001-01-01T00:00:00 CRITICAL: HELLO 1 2 3" + Environment.NewLine;
            PerformLogTest(expectedLog, logger_ => logger_.Critical("HELLO {0} {1} {2}", 1, 2, 3));
        }

        [Test]
        public void TestFatalWithArgsLogging()
        {
            var expectedLog = "2001-01-01T00:00:00 FATAL: HELLO 1 2 3" + Environment.NewLine;
            PerformLogTest(expectedLog, logger_ => logger_.Fatal("HELLO {0} {1} {2}", 1, 2, 3));
        }

        [Test]
        public void TestDebugWithException()
        {
            var exception = new Exception();
            var expectedLog = "2001-01-01T00:00:00 DEBUG: HELLO" + Environment.NewLine + exception + Environment.NewLine;
            PerformLogTest(expectedLog, logger_ => logger_.Debug(exception, "HELLO"));
        }
        
        [Test]
        public void TestInfoWithException()
        {
            var exception = new Exception();
            var expectedLog = "2001-01-01T00:00:00 INFO: HELLO" + Environment.NewLine + exception + Environment.NewLine;
            PerformLogTest(expectedLog, logger_ => logger_.Info(exception, "HELLO"));
        }

        [Test]
        public void TestNoticeWithException()
        {
            var exception = new Exception();
            var expectedLog = "2001-01-01T00:00:00 NOTICE: HELLO" + Environment.NewLine + exception + Environment.NewLine;
            PerformLogTest(expectedLog, logger_ => logger_.Notice(exception, "HELLO"));
        }

        [Test]
        public void TestWarningWithException()
        {
            var exception = new Exception();
            var expectedLog = "2001-01-01T00:00:00 WARNING: HELLO" + Environment.NewLine + exception + Environment.NewLine;
            PerformLogTest(expectedLog, logger_ => logger_.Warning(exception, "HELLO"));
        }

        [Test]
        public void TestErrorWithException()
        {
            var exception = new Exception();
            var expectedLog = "2001-01-01T00:00:00 ERROR: HELLO" + Environment.NewLine + exception + Environment.NewLine;
            PerformLogTest(expectedLog, logger_ => logger_.Error(exception, "HELLO"));
        }

        [Test]
        public void TestCriticalWithException()
        {
            var exception = new Exception();
            var expectedLog = "2001-01-01T00:00:00 CRITICAL: HELLO" + Environment.NewLine + exception + Environment.NewLine;
            PerformLogTest(expectedLog, logger_ => logger_.Critical(exception, "HELLO"));
        }

        [Test]
        public void TestFatalWithException()
        {
            var exception = new Exception();
            var expectedLog = "2001-01-01T00:00:00 FATAL: HELLO" + Environment.NewLine + exception + Environment.NewLine;
            PerformLogTest(expectedLog, logger_ => logger_.Fatal(exception, "HELLO"));
        }

        [Test]
        public void TestDebugWithExceptionAndArgs()
        {
            var exception = new Exception();
            var expectedLog = "2001-01-01T00:00:00 DEBUG: HELLO 1 2 3" + Environment.NewLine + exception + Environment.NewLine;
            PerformLogTest(expectedLog, logger_ => logger_.Debug(exception, "HELLO {0} {1} {2}", 1, 2, 3));
        }
        
        [Test]
        public void TestInfoWithExceptionAndArgs()
        {
            var exception = new Exception();
            var expectedLog = "2001-01-01T00:00:00 INFO: HELLO 1 2 3" + Environment.NewLine + exception + Environment.NewLine;
            PerformLogTest(expectedLog, logger_ => logger_.Info(exception, "HELLO {0} {1} {2}", 1, 2, 3));
        }

        [Test]
        public void TestNoticeWithExceptionAndArgs()
        {
            var exception = new Exception();
            var expectedLog = "2001-01-01T00:00:00 NOTICE: HELLO 1 2 3" + Environment.NewLine + exception + Environment.NewLine;
            PerformLogTest(expectedLog, logger_ => logger_.Notice(exception, "HELLO {0} {1} {2}", 1, 2, 3));
        }

        [Test]
        public void TestWarningWithExceptionAndArgs()
        {
            var exception = new Exception();
            var expectedLog = "2001-01-01T00:00:00 WARNING: HELLO 1 2 3" + Environment.NewLine + exception + Environment.NewLine;
            PerformLogTest(expectedLog, logger_ => logger_.Warning(exception, "HELLO {0} {1} {2}", 1, 2, 3));
        }

        [Test]
        public void TestErrorWithExceptionAndArgs()
        {
            var exception = new Exception();
            var expectedLog = "2001-01-01T00:00:00 ERROR: HELLO 1 2 3" + Environment.NewLine + exception + Environment.NewLine;
            PerformLogTest(expectedLog, logger_ => logger_.Error(exception, "HELLO {0} {1} {2}", 1, 2, 3));
        }

        [Test]
        public void TestCriticalWithExceptionAndArgs()
        {
            var exception = new Exception();
            var expectedLog = "2001-01-01T00:00:00 CRITICAL: HELLO 1 2 3" + Environment.NewLine + exception + Environment.NewLine;
            PerformLogTest(expectedLog, logger_ => logger_.Critical(exception, "HELLO {0} {1} {2}", 1, 2, 3));
        }

        [Test]
        public void TestFatalWithExceptionAndArgs()
        {
            var exception = new Exception();
            var expectedLog = "2001-01-01T00:00:00 FATAL: HELLO 1 2 3" + Environment.NewLine + exception + Environment.NewLine;
            PerformLogTest(expectedLog, logger_ => logger_.Fatal(exception, "HELLO {0} {1} {2}", 1, 2, 3));
        }

        private void PerformLogTest(string expectedLog_, LogTestLineDelegate logTestLineDelegate_)
        {
            var logStream = new MemoryStream();
            var logger = CreateLogger(logStream);
            logTestLineDelegate_(logger);
            TestLogOutput(expectedLog_, logStream, logger.LogEncoding);
        }

        private ILogger CreateLogger(Stream logStream_)
        {
            var logger = new StreamLogger(LoggerName, logStream_);
            logger.TimeProvider = new FixedTimeProvider(LogDate);
            return logger;
        }

        private void TestLogOutput(string expectedOutput_, Stream logStream_, Encoding encoding_)
        {            
            logStream_.Seek(0, SeekOrigin.Begin);
            var loggedBytes = new byte[logStream_.Length];
            logStream_.Read(loggedBytes, 0, (int)logStream_.Length);
            var logStr = encoding_.GetString(loggedBytes);
            Assert.That(logStr, Is.EqualTo(expectedOutput_));
        }
    }
}
