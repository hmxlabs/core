using System;
using System.Xml;

namespace HmxLabs.Core.Serialization.Xml
{
    /// <summary>
    /// Helper / utility methods (written as extensions) for XmlReader
    /// </summary>
    public static class XmlReaderExtensions
    {
        /// <summary>
        /// Treat the contents of the XmlReader's a text and return it as a string.
        /// 
        /// Validation is performed to ensure that the current node is in fact a text node,
        /// the. If the node is not a text node an <c>XmlException</c> is thrown
        /// 
        /// If the current node is an empty element a null string is returned.
        /// </summary>
        /// <param name="reader_"></param>
        /// <returns></returns>
        public static string ReadTextElementValue(this XmlReader reader_)
        {
            if (null == reader_)
                throw new ArgumentNullException(nameof(reader_));

            if (reader_.IsEmptyElement)
                return null;

            reader_.Read(); // This should advance us onto the element's text node
            if (XmlNodeType.Text != reader_.NodeType)
                throw new XmlException($"Expected text node but found {reader_.NodeType} node");

            return reader_.Value;
        }
    }
}
