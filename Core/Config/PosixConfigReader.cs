using System;
using System.IO;

namespace HmxLabs.Core.Config
{
    /// <summary>
    /// An implementation of the <code>IConfigProvider</code> interface that reads its configuration data from
    /// a *NIX style config file that contains one line of configuration per line in the file
    /// Each line take the form of a key value pair seperated by the equals sign and any line can be 
    /// commented out using the hash character.
    /// 
    /// For example
    /// # Provide the server name -- this line is a comment
    /// smtp.server.name=www.mailserver.com
    /// 
    /// The use of <code>#</code> as a comment and <code>=</code> as a seperator is the default, however this can be changed
    /// by using the overloaded constructor to explicitly specify the split character and comment string.
    /// </summary>
    public class PosixConfigReader : ConfigProvider
    {
        /// <summary>
        /// The default split character. <code>=</code>
        /// </summary>
        public const char DefaultSplitChar = '=';

        /// <summary>
        /// The default comment string. <code>#</code>
        /// </summary>
        public const string DefaultCommentString = "#";

        /// <summary>
        /// Default constructor. The filename for the config file to read must be provided.
        /// 
        /// Default values for the split character and comment string will be used.
        /// </summary>
        /// <exception cref="ArgumentNullException">If the provided filename is null</exception>
        /// <exception cref="ArgumentException">If the provided filename is an empty string or only contains whitespace</exception>
        /// <exception cref="FileNotFoundException">If the provided filename is not found</exception>
        /// <param name="filename_">The filename of the configuration file to read</param>
        public PosixConfigReader(string filename_) : this(filename_, DefaultSplitChar, DefaultCommentString)
        {
        }

        /// <summary>
        /// Constructor allowing explicit specification of the split character and comment string
        /// 
        /// If <code>null</code>, an empty string or only whitespace is provided as the comment
        /// string the default comment string value will be used instead.
        /// </summary>
        /// <exception cref="ArgumentNullException">If the provided filename is null</exception>
        /// <exception cref="ArgumentException">If the provided filename is an empty string or only contains whitespace</exception>
        /// <exception cref="FileNotFoundException">If the provided filename is not found</exception>
        /// <param name="filename_">The filename of the configuration file to read</param>
        /// <param name="splitChar_">The character to use to split keys and values on a line</param>
        /// <param name="commentString_">The string to use to treat the whole line as comment</param>
        public PosixConfigReader(string filename_, char splitChar_, string commentString_)
        {
            if (null == filename_)
                throw new ArgumentNullException(nameof(filename_));

            if (string.IsNullOrWhiteSpace(filename_))
                throw new ArgumentException("Invalid (empty) filename provided");

            if (!File.Exists(filename_))
                throw new FileNotFoundException($"The specified config file [{filename_}] could not be found");

            SplitChar = splitChar_;
            CommentString = string.IsNullOrWhiteSpace(commentString_) ? null : commentString_;

            Filename = filename_;
            ReadConfig();
        }

        /// <summary>
        /// Read only property providing the filename that was read to create this object
        /// </summary>
        public string Filename { get; }

        /// <summary>
        /// Read only property providing the split character that was used when reading the configuration
        /// </summary>
        public char SplitChar { get; }

        /// <summary>
        /// Read only property providing the comment string that was used when reading the configuration
        /// </summary>
        public string CommentString { get; }

        private void ReadConfig()
        {
            var contents = File.ReadAllLines(Filename);
            foreach (var line in contents)
            {
                ParseLine(line);
            }
        }

        private void ParseLine(string line_)
        {
            if (string.IsNullOrWhiteSpace(line_))
                return;

            if (null != CommentString && line_.StartsWith(CommentString, StringComparison.InvariantCulture))
                return;

            var splitCharIndex = line_.IndexOf(SplitChar);
            if (0 > splitCharIndex)
                throw new InvalidDataException($"The configuration file [{Filename}] contains invalid data (there is no value) on line [{line_}]");

            var key = line_.Substring(0, splitCharIndex);
            var value = line_.Substring(splitCharIndex+1); // Need to add 1 as we don't want to include the split char in the value

            if (Config.ContainsKey(key))
                throw new InvalidDataException($"The configuration file [{Filename}] contains multiple entries for key [{key}]");

            Config.Add(key, value);
        }
    }
}