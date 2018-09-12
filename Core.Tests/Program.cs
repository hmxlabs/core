using System.Reflection;
using NUnit.ConsoleRunner;

namespace HmxLabs.Core.Tests
{
    class Program
    {
        static void Main(string[] args_)
        {
            Runner.Main(new[] { Assembly.GetExecutingAssembly().Location });
        }
    }
}
