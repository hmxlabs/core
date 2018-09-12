using System;
using System.IO;

namespace HmxLabs.Core.IO
{
    /// <summary>
    /// This class gets the location of a users Dropbox folder. It based on the information
    /// provided here https://www.dropbox.com/en/help/4584
    /// 
    /// This is currently implemented only for Windows and will not work on other platforms.
    /// </summary>
    public class DropboxUtils
    {
        /// <summary>
        /// This function will attempt to retreve any possible dropbox location and will use the following priority
        /// 1. Business
        /// 2. Personal
        /// </summary>
        /// <returns>The location of the user's dropbox folder</returns>
        public static string GetDropboxLocation()
        {
            if (IsWindowsOs())
                return GetWindowsDropboxLocation();

            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the locations of the user's personal DropBox folder
        /// </summary>
        /// <returns></returns>
        public static string GetPersonalDropboxLocation()
        {
            if (IsWindowsOs())
                return GetWindowsDropboxLocation(DropboxInfoParser.DropboxTypes.Personal);

            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the location of the user's business DropBox folder
        /// </summary>
        /// <returns></returns>
        public static string GetBusinessDropboxLocation()
        {
            if (IsWindowsOs())
                return GetWindowsDropboxLocation(DropboxInfoParser.DropboxTypes.Business);

            throw new NotImplementedException();
        }

        private static string GetWindowsDropboxLocation()
        {
            var infoFile = GetWindowsDropboxInfoFileLocation();
            var infoJson = File.ReadAllText(infoFile);

            var dropboxLocation = DropboxInfoParser.GetDropboxDirectoryLocationFromJson(infoJson, DropboxInfoParser.DropboxTypes.Business);
            if (!string.IsNullOrWhiteSpace(dropboxLocation))
                return dropboxLocation;

            return DropboxInfoParser.GetDropboxDirectoryLocationFromJson(infoJson, DropboxInfoParser.DropboxTypes.Personal);
        }

        private static string GetWindowsDropboxLocation(string type_)
        {
            var infoFile = GetWindowsDropboxInfoFileLocation();
            return DropboxInfoParser.GetDropboxDirectoryLocationFromFile(infoFile, type_);
        }

        private static bool IsWindowsOs()
        {
            if (PlatformID.Win32NT == Environment.OSVersion.Platform ||
                PlatformID.Win32Windows == Environment.OSVersion.Platform)
                return true;

            return false;
        }

        private static string GetWindowsDropboxInfoFileLocation()
        {
            var infoFile = GetAppDataDropboxInfoFile();
            if (null != infoFile)
                return infoFile;

            infoFile = GetLocalAppDataDropboxInfoFile();
            if (null != infoFile)
                return infoFile;

            throw new FileNotFoundException("Unable to locate Dropbox info.json file");
        }

        private static string GetAppDataDropboxInfoFile()
        {
            return GetDropboxInfoFile(Environment.SpecialFolder.ApplicationData);
        }

        private static string GetLocalAppDataDropboxInfoFile()
        {
            return GetDropboxInfoFile(Environment.SpecialFolder.LocalApplicationData);
        }

        private static string GetDropboxInfoFile(Environment.SpecialFolder folder_)
        {
            var baseDir = Environment.GetFolderPath(folder_);
            var infoDir = Path.Combine(baseDir, InfoDirectoryName);
            if (!Directory.Exists(infoDir))
                return null;

            var infoFile = Path.Combine(infoDir, InfoFileName);
            return !File.Exists(infoFile) ? null : infoFile;
        }

        private const string InfoDirectoryName = "Dropbox";
        private const string InfoFileName = "info.json";
    }
}
