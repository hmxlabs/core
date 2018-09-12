using System.Xml;

namespace HmxLabs.Core.Serialization.Xml
{
    /// <summary>
    /// XML specific extension to the serialization interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IXmlSerializer<in T> : ISerializer<T>
    {
        /// <summary>
        /// Serialize the provided data to the provided XmlWriter
        /// </summary>
        /// <param name="serializable_"></param>
        /// <param name="writer_"></param>
        void Serialize(T serializable_, XmlWriter writer_);
    }
}