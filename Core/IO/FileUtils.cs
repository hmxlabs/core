using System;
using System.IO;

namespace HmxLabs.Core.IO
{
    /// <summary>
    /// Utility class providing various operations on files.
    /// </summary>
    public static class FileUtils
    {
        /// <summary>
        /// Deprecated. Do not use. The .NET framework provides this functionality.
        /// </summary>
        /// <param name="uri_"></param>
        /// <param name="bufferSize_"></param>
        /// <returns></returns>
        public static byte[] ReadFile(string uri_, int bufferSize_)
        {
            CheckReadInputs(uri_, bufferSize_);

            byte[] bytes;
            var file = File.Open(uri_, FileMode.Open);
            using (file)
            {
                var bytesRead = 0;
                var totalBytes = file.Length;
                var bytesRemaining = (int)totalBytes;
                bytes = new byte[totalBytes];
                while (0 < bytesRemaining)
                {
                    var lastReadByteCount = file.Read(bytes, bytesRead, bytesRemaining);
                    bytesRead += lastReadByteCount;
                    bytesRemaining -= lastReadByteCount;
                    
                    if (0 == lastReadByteCount)
                        break;
                }
                file.Close();
            }            
            return bytes;
        }

        // ReSharper disable UnusedParameter.Local -- Done as this function only does pre-condition checks so this is expected
        private static void CheckReadInputs(string uri_, int bufferSize_)
        {
            if (string.IsNullOrWhiteSpace(uri_))
                throw new ArgumentException("The provided URI is empty", nameof(uri_));

            if (0 > bufferSize_)
                throw new ArgumentException("Invalid buffer size specified");

            if (!File.Exists(uri_))
                throw new ArgumentException("The specified URI does not exist", nameof(uri_));
        }
        // ReSharper restore UnusedParameter.Local
    }
}
