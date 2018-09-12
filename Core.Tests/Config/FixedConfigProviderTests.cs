using System;
using System.Globalization;
using HmxLabs.Core.Config;
using NUnit.Framework;

namespace HmxLabs.Core.Tests.Config
{
    [TestFixture]
    public class FixedConfigProviderTests : ConfigProviderTests
    {
        [Test]
        public void TestAddConfigArgumentGuards()
        {
            var configProvider = new FixedConfigProvider();
            Assert.Throws<ArgumentNullException>(() => configProvider.AddConfig(null, "abc"));
        }

        [Test]
        public void TestAddingDuplicateKeyThrows()
        {
            var configProvider = new FixedConfigProvider();
            const string key = "test key";
            configProvider.AddConfig(key, "abc");
            Assert.Throws<ArgumentException>(() => configProvider.AddConfig(key, "def"));
        }

        [Test]
        public void TestGetStringStrict()
        {
            var configProvider = CreatePopulatedConfigProvider();
            var returnedStringConfig = configProvider.GetConifgAsStringStrict(TestData.Keys.StringKey);
            Assert.That(returnedStringConfig, Is.EqualTo(TestData.Values.StringValue));

            Assert.Throws<ConfigException>(() => configProvider.GetConifgAsStringStrict(TestData.Keys.StringKeyNull));
            Assert.Throws<ConfigException>(() => configProvider.GetConifgAsStringStrict(TestData.Keys.StringKeyEmpty));
        }

        protected override IConfigProvider CreatePopulatedConfigProvider()
        {
            var configProvider = new FixedConfigProvider();
            configProvider.AddConfig(TestData.Keys.StringKey, TestData.Values.StringValue);
            configProvider.AddConfig(TestData.Keys.StringKeyNull, null);
            configProvider.AddConfig(TestData.Keys.StringKeyEmpty, string.Empty);
            configProvider.AddConfig(TestData.Keys.IntKey, TestData.Values.IntValue.ToString(CultureInfo.InvariantCulture));
            configProvider.AddConfig(TestData.Keys.DoubleKey, TestData.Values.DoubleValue.ToString(CultureInfo.InvariantCulture));
            configProvider.AddConfig(TestData.Keys.BoolKeyFalse, TestData.Values.BoolValueFalse.ToString(CultureInfo.InvariantCulture));
            configProvider.AddConfig(TestData.Keys.BoolKeyTrue, TestData.Values.BoolValueTrue.ToString(CultureInfo.InvariantCulture));
            configProvider.AddConfig(TestData.Keys.BoolKeyInvalid, TestData.Values.BoolValueInvalid);
            return configProvider;
        }
    }
}
