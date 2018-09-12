using System.IO;

namespace HmxLabs.Core.Serialization
{
    /// <summary>
    /// Provides a generic interface for deserialization to allow less code, especially test code, to be be written
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDeserializer<out T>
    {
        /// <summary>
        /// Deserialize the provided byte array to the an object of type T
        /// </summary>
        /// <param name="data_">The byte array of data to be deserialized</param>
        /// <returns></returns>
        T Deserialize(byte[] data_);

        /// <summary>
        /// Read data from the provided stream and deserialize an object of type T
        /// </summary>
        /// <param name="stream_">The stream from which the data should be read</param>
        /// <returns></returns>
        T Deserialize(Stream stream_);

        /// <summary>
        /// A proxy for the type on which this serializer operates. This function is typlically used to be able to 
        /// cache these objects in a map and retrieve them by the type on which they operate i.e. the return
        /// value of this property.
        /// 
        /// This type key is just a string value and simply needs to be unique for each type that will be handled
        /// across the serializers present in any given application, or at least shared within one instance of a 
        /// SerializerCache.
        /// 
        /// Generally speaking it is a string that is easily derived from the serialized data to ensable the correct
        /// XmlDeserializer type to be used, so for example the XML Root Tag name may be used.
        /// 
        /// The <code>ISerializerCache</code> relies on this for example
        /// </summary>
        string TypeKey { get; }
    }
}
