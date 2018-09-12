using System;
using System.IO;

namespace HmxLabs.Core.IO
{
    /// <summary>
    /// Utility methods for manipulating directories and the files within them
    /// </summary>
    public static class DirectoryExt
    {
        /// <summary>
        /// Copy all the files located in the source directory to the destination directory. The function will recurse
        /// any subdirectories and copy their contents also.
        /// 
        /// If the destination directory does not exist it will be created. If a file exists in
        /// the destination directory it will not be overwritten.
        /// </summary>
        /// <param name="source_">The source directory to copy from</param>
        /// <param name="destination_">The target directory to copy to</param>
        public static void CopyContents(string source_, string destination_)
        {
            CopyContents(source_, destination_, true, true, false);
        }

        /// <summary>
        /// Copy all the files located in the source directory to the destinateion directory, optionally
        /// creating the destination directory if it does not exist and recursing into sub directories.
        /// </summary>
        /// <param name="source_">The source directory to copy from</param>
        /// <param name="destination_">The target directory to copy to</param>
        /// <param name="createDestination_"><code>true</code> if the target directory should be created if it does not exist. <code>false</code> if the call should fail instead</param>
        /// <param name="recursive_"><code>true</code> if the contents of subdirectories should also be copied. <code>false</code> if only the files in the source directory should be copied</param>
        public static void CopyContents(string source_, string destination_, bool createDestination_, bool recursive_)
        {
            CopyContents(source_, destination_, createDestination_, recursive_, false);
        }

        /// <summary>
        /// Copy all the files located in the source directory to the destinateion directory, optionally
        /// creating the destination directory if it does not exist and recursing into sub directories.
        /// 
        /// If the overwrite flag is set to <code>true</code> this will also overwrite any existing files
        /// in the destination directory.
        /// </summary>
        /// <param name="source_">The source directory to copy from</param>
        /// <param name="destination_">The target directory to copy to</param>
        /// <param name="createDestination_"><code>true</code> if the target directory should be created if it does not exist. <code>false</code> if the call should fail instead</param>
        /// <param name="recursive_"><code>true</code> if the contents of subdirectories should also be copied. <code>false</code> if only the files in the source directory should be copied</param>
        /// <param name="overwrite_"><code>true</code> to overwrite existing files in the destination</param>
        public static void CopyContents(string source_, string destination_, bool createDestination_, bool recursive_, bool overwrite_)
        {
            ValidateCopyContentsArguments(source_, destination_, createDestination_);
            CopyFiles(source_, destination_, overwrite_);

            if (!recursive_)
                return;

            CopyDirectories(source_, destination_);
        }

        /// <summary>
        /// Delete the contents of the specified directory (but leave the directory).
        /// </summary>
        /// <param name="directory_">The directory to delete the contents of</param>
        public static void DeleteContents(string directory_)
        {
            ValidateDeleteContentsArguments(directory_);

            DeleteFiles(directory_);
            DeleteDirectories(directory_);
        }

        private static void DeleteFiles(string directory_)
        {
            var files = Directory.GetFiles(directory_);
            foreach (var file in files)
            {
                File.Delete(file);
            }
        }

        private static void DeleteDirectories(string directory_)
        {
            var directories = Directory.GetDirectories(directory_);
            foreach (var directory in directories)
            {
                Directory.Delete(directory, true);
            }
        }

        private static void CopyDirectories(string source_, string destination_)
        {
            var directoryList = Directory.GetDirectories(source_);
            foreach (var directory in directoryList)
            {
                var localDirName = Path.GetFileName(directory);
                if (null == localDirName)
                    throw new IOException("Unable to determine directory name for: " + directory);
                var destDir = Path.Combine(destination_, localDirName);
                CopyContents(directory, destDir, true, true, true);
            }
        }

        private static void CopyFiles(string source_, string destination_, bool overwrite_)
        {
            var fileList = Directory.GetFiles(source_);
            foreach (var file in fileList)
            {
                var localFilename = Path.GetFileName(file);
                if (null == localFilename)
                    throw new IOException("Unable to determine filename for: " + file);
                var destFilename = Path.Combine(destination_, localFilename);
                File.Copy(file, destFilename, overwrite_);
            }
        }

        private static void ValidateDeleteContentsArguments(string directory_)
        {
            if (null == directory_)
                throw new ArgumentNullException(nameof(directory_));

            if (string.IsNullOrWhiteSpace(directory_))
                throw new ArgumentException("Null or whitespace value provided for directory to delete", nameof(directory_));

            if (!Directory.Exists(directory_))
                throw new DirectoryNotFoundException($"The directory {directory_} does not exist");
        }

        // ReSharper disable once UnusedParameter.Local -- The function is only supposed to do paramater checking!
        private static void ValidateCopyContentsArguments(string source_, string destination_, bool createDestination_)
        {
            if (null == source_)
                throw new ArgumentNullException(nameof(source_));

            if (string.IsNullOrWhiteSpace(source_))
                throw new ArgumentException("Null or whitespace value provided for source directory", nameof(source_));

            if (null == destination_)
                throw new ArgumentNullException(nameof(destination_));

            if (string.IsNullOrWhiteSpace(destination_))
                throw new ArgumentException("Null or whitespace value provided for destination directory", nameof(destination_));

            if (!Directory.Exists(source_))
                throw new DirectoryNotFoundException($"The source directory {source_} does not exist");

            if (Directory.Exists(destination_))
                return;

            if (!createDestination_)
                throw new DirectoryNotFoundException(
                    $"The destination directory {destination_} does not exist and was not requested to be created");
            
            Directory.CreateDirectory(destination_);
        }
    }
}