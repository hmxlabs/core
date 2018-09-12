using System;
using System.IO;

namespace HmxLabs.Core.Log
{
    /// <summary>
    /// An implementation of <code>ILogger</code> that write to file.
    /// 
    /// Derives from <code>StreamLogger</code> and just provides a file stream
    /// </summary>
	public class FileLogger: StreamLogger
	{
        /// <summary>
        /// The default file extension
        /// </summary>
        public const string LogFileExtension = "log.txt";

        /// <summary>
        /// The default period of time to keep log files before automatically deleting them
        /// </summary>
        public static readonly TimeSpan DefaultRetentionPeriod = new TimeSpan(7, 0, 0, 0);

        /// <summary>
        /// Constructs a new instance of the logger with the given name and writing to the
        /// specified output directory
        /// 
        /// The default retention period is used
        /// </summary>
        /// <param name="name_">A unique name for this logger</param>
        /// <param name="directory_">The output directory to write the logs to</param>
		public FileLogger(string name_, string directory_) : base(null, name_)
		{
		    _directory = ValidateDirectory(directory_);
		    RetentionPeriod = DefaultRetentionPeriod;
		}

        /// <summary>
        /// Constructor used by the logger factory to ensure the logger registers itself
        /// </summary>
        /// <param name="factory_"></param>
        /// <param name="name_">A unique name for this logger</param>
        /// <param name="directory_">The output directory to write the logs to</param>
		internal FileLogger(LoggerFactory factory_, string name_, string directory_) : base(factory_, name_)
		{
		    _directory = ValidateDirectory(directory_);
		    RetentionPeriod = DefaultRetentionPeriod;
		}

        /// <summary>
        /// Finalizer as part of the .NET dispose pattern.
        /// </summary>
	    ~FileLogger()
	    {
	        Dispose(false);
	    }

        /// <summary>
        /// The retention period to apply to log files
        /// </summary>
        public TimeSpan? RetentionPeriod { get; set; }

        /// <summary>
        /// Create a log file and remove any old logs
        /// </summary>
		public override void Open()
		{
            base.Open();
            CreateLogFile();
            CleanOldLogs();
		}

	    private void CleanOldLogs()
	    {
	        if (!RetentionPeriod.HasValue)
	            return;

	        var fileList = Directory.GetFiles(_directory);
	        var timeCutOff = TimeProvider.Now.Subtract(RetentionPeriod.Value);
	        foreach (var file in fileList)
	        {
	            var fileInfo = new FileInfo(file);
	            if (0 < fileInfo.LastWriteTime.CompareTo(timeCutOff))
	                continue;

                File.Delete(file);
	        }
	    }

	    private void CreateLogFile()
	    {
            lock (WriteStreamLock)
            {
                if (null != WriteStream)
                {
                    // Already created. Don't do it again
                    return;
                }

                var count = 0;
                var filename = CreateLogFilename(count);
                while (File.Exists(filename))
                {
                    count++;
                    filename = CreateLogFilename(count);
                }

                WriteStream = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
            }
	    }

	    private string CreateLogFilename(int count_)
	    {
            var filename = string.Format("{0}-{1}.{2}", TimeProvider.Now.ToString("yyyy-MM-dd HH-mm"), count_, LogFileExtension);
            var fullFilename = Path.Combine(_directory, filename);
	        return fullFilename;
	    }

	    private string ValidateDirectory(string directory_)
	    {
	        if (null == directory_)
                throw new ArgumentNullException("directory_");

            if (string.IsNullOrWhiteSpace(directory_))
                throw new ArgumentException("Empty or white space directory provided");

	        if (Directory.Exists(directory_))
                return Path.GetFullPath(directory_);
	        
	        
            var dirInfo = Directory.CreateDirectory(directory_);
	        return dirInfo.FullName;
	    }

		private readonly string _directory;
	}
}