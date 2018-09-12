using System;
using System.IO;

namespace HmxLabs.Core.Serialization
{
    /// <summary>
    /// Generic serializing interface used to enable more generic code, especially test cases, to be written
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISerializer<in T>
    {
        /// <summary>
        /// Serialize the provided object to a byte array
        /// </summary>
        /// <param name="target_">The object to be serialized</param>
        /// <returns></returns>
        byte[] Serialize(T target_);

        /// <summary>
        /// Serialize the provided object to the provided stream
        /// </summary>
        /// <param name="target_">The object to be serialized</param>
        /// <param name="stream_">The stream to which the serialized object should be written</param>
        void Serialize(T target_, Stream stream_);

        /// <summary>
        /// The type on which this serializer operates. This function is typlically used to be able to 
        /// cache these objects in a map and retrieve them by the type on which they operate i.e. the return
        /// value of this property.
        /// 
        /// The <code>ISerializerCache</code> relies on this for example
        /// </summary>
        Type ForType { get; }
    }
}
