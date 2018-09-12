using System;

namespace HmxLabs.Core.Serialization
{
    /// <summary>
    /// A utility class that composes a serializer and deserializer pair together
    /// </summary>
    public interface ICompositeSerializer
    {
        /// <summary>
        /// Deserialize the byte array. This simply calls through to the underlying registered deserializer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data_"></param>
        /// <returns></returns>
        T Deserialize<T>(byte[] data_) where T : class;

        /// <summary>
        /// Serialize the object to a byte array. This simply calls through to the underyling registered serializer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data_"></param>
        /// <returns></returns>
        byte[] Serialize<T>(T data_) where T : class;

        /// <summary>
        /// Register a serializer with this composite serializer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializer_"></param>
        void RegisterSerializer<T>(ISerializer<T> serializer_) where T : class;

        /// <summary>
        /// Register a deserializer with this composite object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer_"></param>
        void RegisterDeserializer<T>(IDeserializer<T> deserializer_) where T : class;

        /// <summary>
        /// Returns <c>true</c> if this composite serializer is able to serialize the specified type
        /// </summary>
        /// <param name="type_"></param>
        /// <returns></returns>
        bool CanSerialize(Type type_);

        /// <summary>
        /// Returns <c>true</c> if this composite serializer can deserializer data with the specified type key.
        /// </summary>
        /// <param name="typeKey_"></param>
        /// <returns></returns>
        bool CanDeserialize(string typeKey_);
    }
}
