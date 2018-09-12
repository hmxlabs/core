using System;
using System.IO;
using HmxLabs.Core.Config;
using HmxLabs.Core.Tests.Config.TestFiles;
using NUnit.Framework;
// ReSharper disable ObjectCreationAsStatement -- we create a lot of unassigned objects testing the contructor here and this is deliberate so disable this

namespace HmxLabs.Core.Tests.Config
{
    [TestFixture]
    public class PosixConfigReaderTests : ConfigProviderTests
    {
        [Test]
        public void TestConstructorThrowsOnBadArguments()
        {
            Assert.Throws<ArgumentNullException>(() => new PosixConfigReader(null));
            Assert.Throws<ArgumentException>(() => new PosixConfigReader(string.Empty));
            Assert.Throws<ArgumentException>(() => new PosixConfigReader("   "));
        }

        [Test]
        public void TestConstructorThrowsOnMissingFile()
        {
            Assert.Throws<FileNotFoundException>(() => new PosixConfigReader("thisfiledoesnotexist.nothing"));
        }

        [Test]
        public void TestThrowsOnBadDataInFile()
        {
            Assert.Throws<InvalidDataException>(() => new PosixConfigReader(PosixConfigReaderTestFiles.InvalidLineFile));
        }

        [Test]
        public void TestThrowsOnDuplicateKey()
        {
            Assert.Throws<InvalidDataException>(() => new PosixConfigReader(PosixConfigReaderTestFiles.DuplicateKeyFile));
        }

        [Test]
        public void TestReadGoodFile()
        {
            var reader = new PosixConfigReader(PosixConfigReaderTestFiles.ValidConfigFile);

            Assert.That(reader.GetConfigAsString(ValidStringKey), Is.EqualTo(ValidStringValue));
            Assert.That(reader.GetConfigAsInteger(ValidIntKey), Is.EqualTo(ValidIntValue));
            Assert.That(reader.GetConfigAsDouble(ValidDoubleKey), Is.EqualTo(ValidDoubleValue));
            Assert.That(reader.GetConfigAsString(ValidPosixPathKey), Is.EqualTo(ValidPosixPathValue));
            Assert.That(reader.GetConfigAsString(ValidWindowsPathKey), Is.EqualTo(ValidWindowPathValue));
            Assert.That(reader.GetConfigAsString(ValidConnStrKey), Is.EqualTo(ValidConnStrValue));
        }

        [Test]
        public void TestThrowsOnInvalidTypeRequest()
        {
            var reader = new PosixConfigReader(PosixConfigReaderTestFiles.ValidConfigFile);
            Assert.Throws<ConfigException>(() => reader.GetConfigAsInteger(ValidStringKey));
            Assert.Throws<ConfigException>(() => reader.GetConfigAsDouble(ValidPosixPathKey));
        }

        protected override IConfigProvider CreatePopulatedConfigProvider()
        {
            return new PosixConfigReader(PosixConfigReaderTestFiles.TestData);
        }

        private const string ValidStringKey = "Server";
        private const string ValidIntKey = "Port";
        private const string ValidDoubleKey = "Tolerance";
        private const string ValidPosixPathKey = "Path";
        private const string ValidWindowsPathKey = "AnotherPath";
        private const string ValidConnStrKey = "ConnectionString";

        private const string ValidStringValue = "AServerName";
        private const int ValidIntValue = 1234;
        private const double ValidDoubleValue = 0.05;
        private const string ValidPosixPathValue = "/etc/fst/sample.conf";
        private const string ValidWindowPathValue = @"C:\Program Files (x86)\FST\sample.conf";
        private const string ValidConnStrValue = @"Microsoft.ACE.OLEDB.12.0;Data Source=C:\Data\Db\TestData.accdb";
    }
}
