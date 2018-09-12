using System;
using System.IO;
using System.Xml;

namespace HmxLabs.Core.Serialization.Xml
{
    /// <summary>
    /// A base (abstract) implementation of <code>IXmlSerializer</code> which provides much of the plumbing
    /// needed around creating the basic streams and xml readers and writers allowing derived classes to simply
    /// focus on the actual serialization code.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class XmlSerializer<T> : IXmlSerializer<T>, IXmlDeserializer<T> where T : class 
    {
        /// <summary>
        /// Constructs a memory stream to serialize into and then calls through to the Serialize(T, Stream) method
        /// to perform the actual serialization.
        /// </summary>
        /// <param name="target_"></param>
        /// <returns></returns>
        public byte[] Serialize(T target_)
        {
            if (null == target_)
                throw new ArgumentNullException(nameof(target_));

            var stream = new MemoryStream();
            byte[] bytes;
            using (stream)
            {
                Serialize(target_, stream);
                bytes = stream.ToArray();
                stream.Close();
            }

            return bytes;
        }

        /// <summary>
        /// Constructs an XmlWriter with the settings provided by the virtual method
        /// <code>GetXmlWriterSettings</code> and the calls through to the abstract method
        /// Serialize(T, XmlWriter) for a dervied class to do the actual serialization.
        /// </summary>
        /// <param name="target_">The object to serialize</param>
        /// <param name="stream_">The stream to which the object should be written</param>
        public void Serialize(T target_, Stream stream_)
        {
            if (null == target_)
                throw new ArgumentNullException(nameof(target_));

            if (null == stream_)
                throw new ArgumentNullException(nameof(stream_));

            var settings = GetXmlWriterSettings();
            var xmlWriter = XmlWriter.Create(stream_, settings);
            using (xmlWriter)
            {
                Serialize(target_, xmlWriter);
                xmlWriter.Close();
            }
        }

        /// <summary>
        /// Creates a memory stream wrapper around the provided byte array and the calls through to the 
        /// Deserialize(Stream) method.
        /// </summary>
        /// <param name="data_"></param>
        /// <returns></returns>
        public T Deserialize(byte[] data_)
        {
            if (null == data_)
                throw new ArgumentNullException(nameof(data_));

            if (0 == data_.Length)
                throw new ArgumentException("Empty byte array provided", nameof(data_));

            T serializable;
            var stream = new MemoryStream(data_, false); // we don't intend to modify the data so mark as not writable
            using (stream)
            {
                serializable = Deserialize(stream);
                stream.Close();
            }
            return serializable;
        }

        /// <summary>
        /// Creates an XmlReader using the setting created by the 
        /// <code>GetXmlReaderSettingds</code> virtual method and the calls through
        /// to the abstract <code>Deserialize(XmlReader)</code>  method for a derived class to do
        /// the actual work.
        /// </summary>
        /// <param name="stream_"></param>
        /// <returns></returns>
        public T Deserialize(Stream stream_)
        {
            if (null == stream_)
                throw new ArgumentNullException("stream_");

            T serializable;
            var settings = GetXmlReaderSettings();
            var xmlReader = XmlReader.Create(stream_, settings);
            using (xmlReader)
            {
                serializable = Deserialize(xmlReader);
                xmlReader.Close();
            }
            return serializable;
        }

        /// <summary>
        /// See <code>ISerializer.ForType</code>
        /// </summary>
        public virtual Type ForType => typeof(T);

        /// <summary>
        /// An abstract method expected to be implemented by derived classes to perform the actual serialization
        /// </summary>
        /// <param name="serializable_"></param>
        /// <param name="writer_"></param>
        public abstract void Serialize(T serializable_, XmlWriter writer_);

        /// <summary>
        /// An abstract metho expected to be implemented by derived classes to perform the actual deserialization
        /// </summary>
        /// <param name="reader_"></param>
        /// <returns></returns>
        public abstract T Deserialize(XmlReader reader_);

        /// <summary>
        /// See <code>IDeserializer.TypeKey</code>
        /// </summary>
        public abstract string TypeKey { get; }

        /// <summary>
        /// Virtual method providing the XmlWriter settings to be used.
        /// The default implementation in this class simply returns
        /// <code>XmlSerializerSettings.GetDefaultWriterSettings</code>
        /// </summary>
        /// <returns></returns>
        protected virtual XmlWriterSettings GetXmlWriterSettings()
        {
            return XmlSerializerSettings.GetDefaultWriterSettings();
        }

        /// <summary>
        /// Virtual method providing the XmlReader settings to be used.
        /// The default implementation in this class returns
        /// <code>XmlSerializerSettings.GetDefaultReaderSettings</code>
        /// </summary>
        /// <returns></returns>
        protected virtual XmlReaderSettings GetXmlReaderSettings()
        {
            return XmlSerializerSettings.GetDefaultReaderSettings();
        }
    }
}
