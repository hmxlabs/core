using System.IO;
using NUnit.Framework;

namespace HmxLabs.Core.Tests.Ext
{
    public static class AssertFile
    {
        public static void Exists(string path_)
        {
            Assert.That(File.Exists(path_), $"The path {path_} does not exist");
        }

        public static void DoesNotExist(string path_)
        {
            Assert.That(File.Exists(path_), Is.False, $"The path {path_} exists but should not");
        }

        public static void Exists(string path_, string message_)
        {
            Assert.That(File.Exists(path_), string.Format(message_, path_));
        }

        public static void DoesNotExist(string path_, string message_)
        {
            Assert.That(File.Exists(path_), Is.False, string.Format(message_, path_));
        }
    }
}