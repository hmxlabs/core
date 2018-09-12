using System;
using System.IO;

namespace HmxLabs.Core.IO
{
    /// <summary>
    /// A utility class to parse the DropBox JSON configuration object
    /// </summary>
    public static class DropboxInfoParser
    {
        /// <summary>
        /// The types of DropBox installation possible on a machine. Both may be present
        /// </summary>
        public static class DropboxTypes
        {
            /// <summary>
            /// A personal DropBox account, the most common type
            /// </summary>
            public const string Personal = "personal";

            /// <summary>
            /// A paid for business DropBox account.
            /// </summary>
            public const string Business = "business";
        }

        /// <summary>
        /// Retrieve the location of the DropBox directory from the specified file and for the specified type of DropBox
        /// installation.
        /// </summary>
        /// <param name="dropboxInfoFilename_">The location of the DropBox configuration file</param>
        /// <param name="section_">The <code>DropBoxTypes</code> string representation of the account type to look for</param>
        /// <returns></returns>
        public static string GetDropboxDirectoryLocationFromFile(string dropboxInfoFilename_, string section_)
        {
            if (null == dropboxInfoFilename_)
                throw new ArgumentNullException(nameof(dropboxInfoFilename_));

            if (null == section_)
                throw new ArgumentNullException(nameof(section_));

            if (string.IsNullOrEmpty(section_))
                throw new ArgumentException("Invalid section provided. Value is empty");

            var dropBoxInfo = File.ReadAllText(dropboxInfoFilename_);
            return GetDropboxDirectoryLocationFromJson(dropBoxInfo, section_);
        }

        /// <summary>
        /// Retrieve the location of the DropBox directory from the specified JSON string and for the specified type of DropBox
        /// installation.
        /// </summary>
        /// <param name="infoJson_">The JSON string within the DropBox config file</param>
        /// <param name="section_">The <code>DropBoxTypes</code> string representation of the account type to look for</param>
        /// <returns></returns>
        public static string GetDropboxDirectoryLocationFromJson(string infoJson_, string section_)
        {
            if (null == infoJson_)
                throw new ArgumentNullException(nameof(infoJson_));

            if (null == section_)
                throw new ArgumentNullException(nameof(section_));

            if (string.IsNullOrEmpty(section_))
                throw new ArgumentException("Invalid section provided. Value is empty");

            var personalSectionStart = infoJson_.IndexOf(section_, StringComparison.InvariantCulture);
            if (0 > personalSectionStart)
                return null;

            var pathStart = infoJson_.IndexOf(PathHeader, personalSectionStart, StringComparison.InvariantCulture);
            pathStart = infoJson_.IndexOf(":", pathStart, StringComparison.InvariantCulture);
            var pathStr = infoJson_.Substring(pathStart);

            pathStr = pathStr.Substring(pathStr.IndexOf("\"", StringComparison.InvariantCulture) + 1);
            pathStr = pathStr.Substring(0, pathStr.IndexOf("\"", StringComparison.InvariantCulture));

            // The file often has a \\ instead of \ as a path separator..
            pathStr = pathStr.Replace("\\\\", "\\");

            return pathStr;
        }

        private const string PathHeader = "path";
    }
}
