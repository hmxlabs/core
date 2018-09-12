using System;

namespace HmxLabs.Core.Serialization
{
    /// <summary>
    /// Implemation of <code>ICompositeSerializer</code>
    /// </summary>
    public abstract class CompositeSerializer : ICompositeSerializer
    {
        /// <summary>
        /// See <code>ICompositeDeserializer</code>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data_"></param>
        /// <returns></returns>
        public abstract T Deserialize<T>(byte[] data_) where T : class;

        /// <summary>
        /// See <code>ICompositeDeserializer</code>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data_"></param>
        /// <returns></returns>
        public byte[] Serialize<T>(T data_) where T : class
        {
            if (null == data_)
                throw new ArgumentNullException(nameof(data_));

            var serializer = _cache.GetSerializer<T>(typeof(T));
            return serializer.Serialize(data_);
        }

        /// <summary>
        /// See <code>ICompositeDeserializer</code>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializer_"></param>
        public void RegisterSerializer<T>(ISerializer<T> serializer_) where T : class
        {
            if (null == serializer_)
                throw new ArgumentNullException(nameof(serializer_));

            _cache.AddSerializer(serializer_);
        }

        /// <summary>
        /// See <code>ICompositeDeserializer</code>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deserializer_"></param>
        public void RegisterDeserializer<T>(IDeserializer<T> deserializer_) where T : class
        {
            if (null == deserializer_)
                throw new ArgumentNullException(nameof(deserializer_));

            _cache.AddDeserializer(deserializer_);
        }

        /// <summary>
        /// See <code>ICompositeDeserializer</code>
        /// </summary>
        /// <param name="type_"></param>
        /// <returns></returns>
        public bool CanSerialize(Type type_)
        {
            if (null == type_)
                throw new ArgumentNullException(nameof(type_));

            return _cache.ContainsSerializer(type_);
        }

        /// <summary>
        /// See <code>ICompositeDeserializer</code>
        /// </summary>
        /// <param name="typeKey_"></param>
        /// <returns></returns>
        public bool CanDeserialize(string typeKey_)
        {
            if (null == typeKey_)
                throw new ArgumentNullException(nameof(typeKey_));

            if (string.IsNullOrWhiteSpace(typeKey_))
                throw new ArgumentException("The provided type key is empty");

            return _cache.ContainsDeserializer(typeKey_);
        }

        /// <summary>
        /// The backing cache that is used to store the registered serializers
        /// </summary>
        protected ISerializerCache Cache => _cache;

        private readonly ISerializerCache _cache = new SerializerCache();
    }
}
