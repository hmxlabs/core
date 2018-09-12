using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace HmxLabs.Core.Serialization.Xml
{
    /// <summary>
    /// An Xml specific implementation of <code>ICompositeSerializer</code>. Derives from
    /// <code>CompositeSerializer</code> and provides XML specific implementations and overrides
    /// </summary>
    public class XmlCompositeSerializer : CompositeSerializer
    {
        /// <summary>
        /// Overrides the (abstract) base class implementation to create a memory stream and 
        /// the call Deserialize(Stream).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data_"></param>
        /// <returns></returns>
        public override T Deserialize<T>(byte[] data_)
        {
            if (null == data_)
                throw new ArgumentNullException(nameof(data_));

            if (0 == data_.Length)
                throw new ArgumentException("The provided data is of zero length");

            T serializable;
            var stream = new MemoryStream(data_, false); // we don't intend to modify the data to mark as not writable
            using (stream)
            {
                serializable = Deserialize<T>(stream);
                stream.Close();
            }
            return serializable;
        }
        
        /// <summary>
        /// Constructs an XmlReader using the <code>GetXmlReaderSettings</code> method
        /// and calls Deserialize(XmlReader).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream_"></param>
        /// <returns></returns>
        private T Deserialize<T>(Stream stream_)
        {
            T serializable;
            var settings = GetXmlReaderSettings();
            var xmlReader = XmlReader.Create(stream_, settings);
            using (xmlReader)
            {
                serializable = Deserialize<T>(xmlReader);
                xmlReader.Close();
            }
            return serializable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlReader_"></param>
        /// <returns></returns>
        private T Deserialize<T>(XmlReader xmlReader_)
        {
            if (null == xmlReader_)
                throw new ArgumentNullException("xmlReader_");

            xmlReader_.MoveToContent();
            if (!xmlReader_.IsStartElement())
                throw new SerializationException("Unable to find XML start element in data. Can not deserialize");

            var rootTag = xmlReader_.LocalName;
            if (!CanDeserialize(rootTag))
                throw new SerializationException("Unable to parse data with root tag: " + rootTag);

            var serializer = Cache.GetDeserializer<T>(rootTag) as IXmlDeserializer<T>;
            if (null == serializer)
                throw new SerializationException("Unable to retrieve serializer for root tag: " + rootTag);

            return serializer.Deserialize(xmlReader_);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual XmlReaderSettings GetXmlReaderSettings()
        {
            return XmlSerializerSettings.GetDefaultReaderSettings();
        }
    }
}
