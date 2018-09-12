using System.Text;
using System.Xml;

namespace HmxLabs.Core.Serialization.Xml
{
    /// <summary>
    /// Default XML serialization settings
    /// </summary>
    public class XmlSerializerSettings
    {
        /// <summary>
        /// The default encoding to use. Currently set to UTF8
        /// </summary>
        public static readonly Encoding DefaultEncoding = Encoding.UTF8;

        /// <summary>
        /// The default new line character. Set to NewLine (\n) as per the
        /// Unix standard rather than the windows standard of CarriageReturn+NewLine
        /// </summary>
        public const string DefaultNewLine = "\n";

        /// <summary>
        /// The default indent character(s). Set to two spaces.
        /// </summary>
        public const string DefaultIndentChars = "  ";

        /// <summary>
        /// Returns the default Xml Reader settings
        /// </summary>
        /// <returns></returns>
        public static XmlReaderSettings GetDefaultReaderSettings()
        {
            var settings = new XmlReaderSettings();
            settings.CloseInput = false;
            settings.ConformanceLevel = ConformanceLevel.Auto;
            settings.IgnoreComments = true;
            settings.IgnoreProcessingInstructions = true;
            settings.IgnoreWhitespace = true;
            settings.ValidationType = ValidationType.None;
            return settings;
        }

        /// <summary>
        /// Returns the default Xml Writer settings
        /// </summary>
        /// <returns></returns>
        public static XmlWriterSettings GetDefaultWriterSettings()
        {
            var settings = new XmlWriterSettings();
            settings.CloseOutput = false;
            settings.ConformanceLevel = ConformanceLevel.Auto;
            settings.Encoding = DefaultEncoding;
            settings.Indent = true;
            settings.IndentChars = DefaultIndentChars;
            settings.NewLineChars = DefaultNewLine;
            settings.NewLineOnAttributes = false;
            settings.OmitXmlDeclaration = true;

            return settings;
        }
    }
}
