using System;
using HmxLabs.Core.DateTIme;
using NUnit.Framework;

namespace HmxLabs.Core.Tests.Base
{
    public class FsDateTimeTests
    {
        public const string StringDateOne = "1982-03-12T15:35:10";
        public const string StringDateTwo = "2010-01-01T02:00:05";
        public static readonly DateTime DateTimeOne = new DateTime(1982, 3, 12, 15, 35, 10);
        public static readonly DateTime DateTimeTwo = new DateTime(2010, 1, 1, 2, 0, 5);

        [Test]
        public void TestIsoDateToString()
        {
            Assert.AreEqual(StringDateOne, DateTimeOne.ToIsoDateTimeString());
            Assert.AreEqual(StringDateTwo, DateTimeTwo.ToIsoDateTimeString());
        }

        [Test]
        public static void TestIsoDateFromString()
        {
            Assert.AreEqual(DateTimeOne, HmxDateTime.ParseIsoDateTimeString(StringDateOne));
            Assert.AreEqual(DateTimeTwo, HmxDateTime.ParseIsoDateTimeString(StringDateTwo));
        }
    }
}
