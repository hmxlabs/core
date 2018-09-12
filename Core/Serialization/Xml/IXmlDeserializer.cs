using System.Xml;

namespace HmxLabs.Core.Serialization.Xml
{
    /// <summary>
    /// An extension of the deserializer interface specific to XML serialization.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IXmlDeserializer<out T> : IDeserializer<T>
    {
        /// <summary>
        /// Deserialize data from the XmlReader to contruct object.
        /// </summary>
        /// <param name="reader_">The XmlReader to operate on</param>
        /// <returns></returns>
        T Deserialize(XmlReader reader_);
    }
}
