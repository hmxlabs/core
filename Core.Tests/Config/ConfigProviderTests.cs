using HmxLabs.Core.Config;
using NUnit.Framework;

namespace HmxLabs.Core.Tests.Config
{
    [TestFixture]
    public abstract class ConfigProviderTests
    {
        public static class TestData
        {
            public static class Keys
            {
                public const string StringKey = "test.string.value";
                public const string StringKeyNull = "test.string.null.value";
                public const string StringKeyEmpty = "test.string.empty.value";
                public const string IntKey = "test.int.value";
                public const string DoubleKey = "test.double.value";
                public const string BoolKeyTrue = "test.bool.true";
                public const string BoolKeyFalse = "test.bool.false";
                public const string BoolKeyInvalid = "test.bool.invalid";
            }

            public static class Values
            {
                public const string StringValue = "This is a random string that won't parse as an int or a double";
                public const int IntValue = 123;
                public const double DoubleValue = 101.1;
                public const bool BoolValueTrue = true;
                public const bool BoolValueFalse = false;
                public const string BoolValueInvalid = "hello";
            }   
        }

        [Test]
        public void TestContains()
        {
            var configProvider = CreatePopulatedConfigProvider();
            Assert.That(configProvider.Contains(TestData.Keys.StringKey));
            Assert.That(configProvider.Contains(TestData.Keys.IntKey));
            Assert.That(configProvider.Contains(TestData.Keys.DoubleKey));
        }

        [Test]
        public void TestGetString()
        {
            var configProvider = CreatePopulatedConfigProvider();
            var returnedStringConfig = configProvider.GetConfigAsString(TestData.Keys.StringKey);
            Assert.That(returnedStringConfig, Is.EqualTo(TestData.Values.StringValue));
        }

        [Test]
        public void TestGetInt()
        {
            var configProvider = CreatePopulatedConfigProvider();
            var returnedIntConfig = configProvider.GetConfigAsInteger(TestData.Keys.IntKey);
            Assert.That(returnedIntConfig, Is.EqualTo(TestData.Values.IntValue));
        }

        [Test]
        public void TestGetDouble()
        {
            var configProvider = CreatePopulatedConfigProvider();
            var returnedDoubleConfig = configProvider.GetConfigAsDouble(TestData.Keys.DoubleKey);
            Assert.That(returnedDoubleConfig, Is.EqualTo(TestData.Values.DoubleValue));
        }

        [Test]
        public void TestGetBool()
        {
            var configProvider = CreatePopulatedConfigProvider();
            var returnedBoolConfig = configProvider.GetConfigAsBool(TestData.Keys.BoolKeyTrue);
            Assert.That(returnedBoolConfig, Is.True);

            returnedBoolConfig = configProvider.GetConfigAsBool(TestData.Keys.BoolKeyFalse);
            Assert.That(returnedBoolConfig, Is.False);
        }

        [Test]
        public void TestGetBoolThrowsWhenNotABool()
        {
            var configProvider = CreatePopulatedConfigProvider();
            Assert.Throws<ConfigException>(() => configProvider.GetConfigAsInteger(TestData.Keys.BoolKeyInvalid));
        }

        [Test]
        public void TestGetIntThrowsWhenNotAnInt()
        {
            var configProvider = CreatePopulatedConfigProvider();
            Assert.Throws<ConfigException>(() => configProvider.GetConfigAsInteger(TestData.Keys.StringKey));
        }

        [Test]
        public void TestGetDoubleThrowsWhenNotADouble()
        {
            var configProvider = CreatePopulatedConfigProvider();
            Assert.Throws<ConfigException>(() => configProvider.GetConfigAsDouble(TestData.Keys.StringKey));
        }

        protected abstract IConfigProvider CreatePopulatedConfigProvider();
    }
}
