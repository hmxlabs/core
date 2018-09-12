using System;
using System.IO;
using HmxLabs.Core.Log;
using HmxLabs.Core.Tests.Base;
using HmxLabs.Core.Tests.Ext;
using NUnit.Framework;
// ReSharper disable ObjectCreationAsStatement

namespace HmxLabs.Core.Tests.Log
{
    [TestFixture]
    public class FileLoggerTests
    {
        [Test]
        public void TestConstructorThrowsOnNullDirectory()
        {
            Assert.Throws<ArgumentNullException>(() => new FileLogger("testLogger", null));
        }

        [Test]
        public void TestConstructorThrowsOnEmptyDirectory()
        {
            Assert.Throws<ArgumentException>(() => new FileLogger("testLogger", string.Empty));
        }

        [Test]
        public void TestConstructorThrowsOnWhitespaceDirectory()
        {
            Assert.Throws<ArgumentException>(() => new FileLogger("testLogger", "    "));
        }

        [Test]
        public void TestDirectoryIsCreatedIfMissing()
        {
            const string newDirName = "NewLogDir";
            if (Directory.Exists(newDirName))
                Directory.Delete(newDirName, true);

            // Disable as we need to create the instance to see the directory created but don't need it after that
            // ReSharper disable once UnusedVariable
            var fileLogger = new FileLogger("testLogger", newDirName);
            AssertDirectory.Exists(newDirName);
        }

        [Test]
        public void NewFileIsCreatedWithExpectedName()
        {
            const string newDirName = "NewFileLogDir";
            if (Directory.Exists(newDirName))
                Directory.Delete(newDirName, true);

            var fileLogger = new FileLogger("testLogger", newDirName);
            var loggerDate = new DateTime(2001, 1, 1, 0, 0, 0);
            var fixedTimeProvider = new FixedTimeProvider(loggerDate);
            fileLogger.TimeProvider = fixedTimeProvider;
            fileLogger.Open();
            fileLogger.Dispose();
            var expectedFilename = string.Format("{0}-0.log.txt", loggerDate.ToString("yyyy-MM-dd HH-mm"));
            var expectedFilePath = Path.Combine(newDirName, expectedFilename);
            AssertFile.Exists(expectedFilePath);
        }

        [Test]
        public void FileIndexCounterIncrementsWhenFileExists()
        {
            const string newDirName = "IndexIncLogDir";
            if (Directory.Exists(newDirName))
                Directory.Delete(newDirName, true);

            var fileLogger = new FileLogger("testLogger", newDirName);
            var loggerDate = new DateTime(2001, 1, 1, 0, 0, 0);
            var fixedTimeProvider = new FixedTimeProvider(loggerDate);
            fileLogger.TimeProvider = fixedTimeProvider;
            fileLogger.Open();
            fileLogger.Dispose();
            fileLogger = new FileLogger("testLogger", newDirName);
            fileLogger.TimeProvider = fixedTimeProvider;
            fileLogger.Open();
            fileLogger.Dispose();
            var expectedFilename = string.Format("{0}-1.log.txt", loggerDate.ToString("yyyy-MM-dd HH-mm"));
            var expectedFilePath = Path.Combine(newDirName, expectedFilename);
            AssertFile.Exists(expectedFilePath);
        }

        [Test]
        public void TestOldFilesAreCleanedUp()
        {
            const string newDirName = "CleanUpLogDir";
            if (Directory.Exists(newDirName))
            {
                Directory.Delete(newDirName, true);
            }
                

            Directory.CreateDirectory(newDirName);
            AssertDirectory.Exists(newDirName);
            var retentionPeriod = new TimeSpan(7, 0, 0, 0);
            var loggerDate = DateTime.Now;
            var firstOldFile = string.Format("{0}-0.log.txt", loggerDate.ToString("yyyy-MM-dd HH-mm"));
            var firstOldFilePath = Path.Combine(newDirName, firstOldFile);
            var fileStream = File.Create(firstOldFilePath);
            fileStream.Close();
            var fileInfo = new FileInfo(firstOldFilePath);
            fileInfo.LastWriteTime = loggerDate.Subtract(retentionPeriod).Subtract(new TimeSpan(0, 0, 0, 1)); // one second older than cutoff

            var secondOldFile = string.Format("{0}-1.log.txt", loggerDate.ToString("yyyy-MM-dd HH-mm"));
            var secondOldFilePath = Path.Combine(newDirName, secondOldFile);
            fileStream = File.Create(secondOldFilePath);
            fileStream.Close();
            fileInfo = new FileInfo(secondOldFilePath);
            fileInfo.LastWriteTime = loggerDate.Subtract(retentionPeriod).Add(new TimeSpan(0, 0, 0, 1)); // one second newer than cutoff

            var fileLogger = new FileLogger("testLogger", newDirName);
            fileLogger.TimeProvider = new FixedTimeProvider(loggerDate);
            fileLogger.RetentionPeriod = retentionPeriod;
            fileLogger.Open();
            fileLogger.Dispose();
            
            // Check the first one is gone and the second one is still there
            AssertFile.DoesNotExist(firstOldFilePath);
            AssertFile.Exists(secondOldFilePath);
        }
    }
}
