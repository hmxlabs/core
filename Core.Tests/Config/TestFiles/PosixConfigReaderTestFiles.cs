using System.IO;

namespace HmxLabs.Core.Tests.Config.TestFiles
{
    public class PosixConfigReaderTestFiles
    {
        public static readonly string Directory = Path.Combine(".", "Config", "TestFiles");
        public static readonly string DuplicateKeyFile = Path.Combine(Directory, "DuplicateKey.txt");
        public static readonly string InvalidLineFile = Path.Combine(Directory, "InvalidLine.txt");
        public static readonly string ValidConfigFile = Path.Combine(Directory, "ValidConfig.txt");
        public static readonly string TestData = Path.Combine(Directory, "TestData.txt");
    }
}
