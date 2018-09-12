using System;

namespace HmxLabs.Core.Serialization
{
    /// <summary>
    /// A cache of <code>ISerializer</code>s keyed by the type of which they operate.
    /// </summary>
    public interface ISerializerCache
    {
        /// <summary>
        /// Add a new <code>ISerializer</code>
        /// </summary>
        /// <typeparam name="T">The type on which the serializer operates</typeparam>
        /// <param name="serializer_">The serializer instance to add</param>
        void AddSerializer<T>(ISerializer<T> serializer_);

        /// <summary>
        /// Add a new <code>IDeserializer</code>
        /// </summary>
        /// <typeparam name="T">The type on which the deserializer operates</typeparam>
        /// <param name="deserializer_">The deserializer instance to add</param>
        void AddDeserializer<T>(IDeserializer<T> deserializer_);

        /// <summary>
        /// Get an serializer instance that operates on objects of the specified type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type_"></param>
        /// <returns></returns>
        ISerializer<T> GetSerializer<T>(Type type_);

        /// <summary>
        /// Get a deserializer instance that operates on the specified types of objects
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="typeKey_"></param>
        /// <returns></returns>
        IDeserializer<T> GetDeserializer<T>(string typeKey_);
        
        /// <summary>
        /// Utility method to check if this cache contains the specified serializer.
        /// </summary>
        /// <param name="type_"></param>
        /// <returns></returns>
        bool ContainsSerializer(Type type_);

        /// <summary>
        /// Utility method to check if this cache contains the specified deserialiser
        /// </summary>
        /// <param name="typeKey_">The string type key for the deserialiser</param>
        /// <returns></returns>
        bool ContainsDeserializer(string typeKey_);
    }
}
