using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HmxLabs.Core.Serialization
{
    /// <summary>
    /// An implementation of <code>ISerializerCache</code>. See the interfae specification for more details.
    /// </summary>
    public class SerializerCache : ISerializerCache
    {
        /// <summary>
        /// Add the specified serializer to this cache. Attempting to add a serializer for a type
        /// that has already been added will result in an <code>ArgumentException</code>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializer_"></param>
        public void AddSerializer<T>(ISerializer<T> serializer_)
        {
            if (null == serializer_)
                throw new ArgumentNullException();

            if (_serializers.ContainsKey(serializer_.ForType))
                throw new ArgumentException("The provided serializer's type has been previously registerd: " + serializer_.ForType);

            _serializers[serializer_.ForType] = serializer_;
        }

        /// <summary>
        /// Add the specified deserializer to this cache. Attempting to add a deserializer for a type key
        /// that has already been added will result in an <code>ArgumentException</code>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer_"></param>
        public void AddDeserializer<T>(IDeserializer<T> deserializer_)
        {
            if (null == deserializer_)
                throw new ArgumentNullException(nameof(deserializer_));

            if (_deserializers.ContainsKey(deserializer_.TypeKey))
                throw new ArgumentException("The provided serializer's type key has previously been registered: " + deserializer_.TypeKey);

            _deserializers[deserializer_.TypeKey] = deserializer_;
        }

        /// <summary>
        /// Obtain a serializer for the specified type key.
        /// 
        /// Throws a SerializationException if no deserializer for the requested type key is present.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="typeKey_"></param>
        /// <returns></returns>
        public IDeserializer<T> GetDeserializer<T>(string typeKey_)
        {
            if (null == typeKey_)
                throw new ArgumentNullException(nameof(typeKey_));

            if (string.IsNullOrWhiteSpace(typeKey_))
                throw new ArgumentException("Empty type key provided");

            if (!_deserializers.ContainsKey(typeKey_))
                throw new SerializationException("No Serializer available to handle type key: " + typeKey_);

            var serializer = _deserializers[typeKey_] as IDeserializer<T>;
            if (null == serializer)
                throw new SerializationException($"The serializer registered to handle type key {typeKey_} could not be cast to type {typeof(ISerializer<T>)}");

            return serializer;
        }

        /// <summary>
        /// Obtain a serializer for the specified type.
        /// 
        /// Throws a SerializationException if no serializer is available.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type_"></param>
        /// <returns></returns>
        public ISerializer<T> GetSerializer<T>(Type type_)
        {
            if (null == type_)
                throw new ArgumentNullException("type_");

            if (!_serializers.ContainsKey(type_))
                throw new SerializationException("No serializer available to handle type: " + type_);

            var serializer = _serializers[type_] as ISerializer<T>;
            if (null == serializer)
                throw new SerializationException(string.Format("The serializer registered to handle type {0} could not be cast to type {1}", type_, typeof(ISerializer<T>)));

            return serializer;
        }

        /// <summary>
        /// Utility method to check if this cache contains the specified deserialiser
        /// </summary>
        /// <param name="typeKey_">The string type key for the deserialiser</param>
        /// <returns></returns>
        public bool ContainsDeserializer(string typeKey_)
        {
            if (null == typeKey_)
                throw new ArgumentNullException("typeKey_");

            if (string.IsNullOrWhiteSpace(typeKey_))
                throw new ArgumentException("Empty type key provided");

            return _deserializers.ContainsKey(typeKey_);
        }

        
        /// /// <summary>
        /// Utility method to check if this cache contains the specified serializer.
        /// </summary>
        /// <param name="type_"></param>
        /// <returns></returns>
        public bool ContainsSerializer(Type type_)
        {
            if (null == type_)
                throw new ArgumentNullException("type_");

            return _serializers.ContainsKey(type_);
        }

        private readonly Dictionary<string, object> _deserializers = new Dictionary<string, object>();
        private readonly Dictionary<Type, object> _serializers = new Dictionary<Type, object>();
    }
}
