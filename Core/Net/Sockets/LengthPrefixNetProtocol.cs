using System;
using System.IO;
using System.Net;
using HmxLabs.Core.Threading;

namespace HmxLabs.Core.Net.Sockets
{
    /// <summary>
    /// An implementation of a <code>INetProtocol</code> that frames the data over the wire by always 
    /// prefixing the payload with a 32 bit integer specifying the length of the byte array that forms
    /// the message payload.
    /// 
    /// The protocol supports the use of a keep alive message and can limit the maximum message size
    /// </summary>
    public class LengthPrefixNetProtocol : INetProtocol
    {
        /// <summary>
        /// The default maximum message size to allow. Set to 256KB.
        /// </summary>
        public const int DefaultMaximumMessageLength = 256*1024; // 256KB

        /// <summary>
        /// Default constructor. Properties such as the <code>Stream</code> must be set
        /// prior to using the constructed object.
        /// </summary>
        public LengthPrefixNetProtocol()
        {
            MaximumMessageLength = DefaultMaximumMessageLength;
        }

        /// <summary>
        /// See <code>INetProtocol.Reset</code>
        /// </summary>
        public void Reset()
        {
            Stream = null;
            _outgoingMessage = null;
            _readBuffer = null;
            _bytesRead = 0;
            _readOutput = null;
            _writeAsyncResult = null;
            _readAsyncResult = null;
        }

        /// <summary>
        /// Property specifying the maximum message length to allow
        /// </summary>
        public int MaximumMessageLength { get; set; }

        /// <summary>
        /// See <code>INetProtocol.Stream</code>.
        /// </summary>
        public Stream Stream { get; set; }

        /// <summary>
        /// See <code>INetProtocol.Read</code>.
        /// </summary>
        /// <returns></returns>
        public INetProtocolReadOutput Read()
        {
            if (null == Stream)
                throw new InvalidOperationException("No Stream to operate on has been specified");

            if (null != _readAsyncResult)
                throw new InvalidOperationException("The current read operation is currently pending");

            _readOutput = null;
            var messageLength = ReadMessageLength();
            if (0 == messageLength)
            {
                return _readOutput;
            }

            _readBuffer = new byte[messageLength];
            _bytesRead = 0;
            FillBufferFromStreamSync();
            return new NetProtocolReadOutput(NetProtocolReadType.Message, _readBuffer);
        }

        /// <summary>
        /// See <code>INetProtocol.BeginRead</code>.
        /// </summary>
        /// <returns></returns>
        public IAsyncResult BeginRead(AsyncCallback callback_, object state_)
        {
            if (null != _readAsyncResult)
                throw new InvalidOperationException("The current read operation is still pending.");

            if (null == Stream)
                throw new InvalidOperationException("No Stream to operate on has been specified");

            _readAsyncResult = new AsyncResult(callback_, state_);
            _readOutput = null;
            StartReadingMessageAsync();
            return _readAsyncResult;
        }

        /// <summary>
        /// See <code>INetProtocol.EndRead</code>.
        /// </summary>
        /// <returns></returns>
        public INetProtocolReadOutput EndRead(IAsyncResult asyncResult_)
        {
            if (null == asyncResult_)
                throw new ArgumentNullException(nameof(asyncResult_));

            if (!ReferenceEquals(asyncResult_, _readAsyncResult))
                throw new ArgumentException("Invalid async result provided");

            try
            {
                _readAsyncResult.EndOperation();
                return _readOutput;
            }
            finally
            {
                _readAsyncResult = null;
            }
        }


        /// <summary>
        /// See <code>INetProtocol.Write</code>.
        /// </summary>
        /// <returns></returns>
        public void Write(byte[] message_)
        {
            if (null != _writeAsyncResult)
                throw new InvalidOperationException("The current write operation is still pending.");

            if (null == Stream)
                throw new InvalidOperationException("No Stream to operate on has been specified");

            if (null == message_)
                throw new ArgumentNullException(nameof(message_));

            var messageLength = BitConverter.GetBytes(message_.Length);
            Stream.Write(messageLength, 0, messageLength.Length);
            
            if (0 == message_.Length) return; // we always write the message length, even for 0 length messages as this is the keepalive message
            Stream.Write(message_, 0, message_.Length);
        }

        /// <summary>
        /// See <code>INetProtocol.BeginWrite</code>.
        /// </summary>
        /// <returns></returns>
        public IAsyncResult BeginWrite(byte[] message_, AsyncCallback callback_, object state_)
        {
            if (null != _writeAsyncResult)
                throw new InvalidOperationException("The current write operation is still pending.");

            if (null == Stream)
                throw new InvalidOperationException("No Stream to operate on has been specified");

            if (null == message_)
                throw new ArgumentNullException(nameof(message_));
            
            _outgoingMessage = message_;
            _writeAsyncResult = new AsyncResult(callback_, state_);
            var messageLength = BitConverter.GetBytes(message_.Length);
            Stream.BeginWrite(messageLength, 0, messageLength.Length, OnWriteMessageLengthCompleted, null);
            return _writeAsyncResult;
        }

        /// <summary>
        /// See <code>INetProtocol.EndWrite</code>.
        /// </summary>
        /// <returns></returns>
        public void EndWrite(IAsyncResult asyncResult_)
        {
            if (null == asyncResult_)
                throw new ArgumentNullException(nameof(asyncResult_));

            if (!ReferenceEquals(asyncResult_, _writeAsyncResult))
                throw new ArgumentException("Invalid async result provided");

            try
            {
                _writeAsyncResult.EndOperation();
            }
            finally
            {
                _writeAsyncResult = null;
            }
        }

        /// <summary>
        /// See <code>INetProtocol.KeepAliveMessage</code>.
        /// </summary>
        public byte[] KeepAliveMessage => KeepAliveMessageData;

        private int BytesRemainingToBeRead => _readBuffer.Length - _bytesRead;

        private int ReadMessageLength()
        {
            _readBuffer = new byte[sizeof(int)];
            _bytesRead = 0;
            FillBufferFromStreamSync();
            if (0 == _bytesRead)
            {
                _readOutput = new NetProtocolReadOutput(NetProtocolReadType.EmptyRead, null);
                return 0;
            }

            var messageLength = BitConverter.ToInt32(_readBuffer, 0);
            ValidateReadMessageLength(messageLength);
            if (0 == messageLength)
            {
                _readOutput = new NetProtocolReadOutput(NetProtocolReadType.KeepAlive, new byte[0]);
            }

            return messageLength;
        }

        private void FillBufferFromStreamSync()
        {
            do
            {
                var bytesRead = Stream.Read(_readBuffer, _bytesRead, BytesRemainingToBeRead);
                if (0 == bytesRead)
                {
                    return;
                }
                _bytesRead += bytesRead;
            } while (_bytesRead < _readBuffer.Length);
        }

        private void StartReadingMessageAsync()
        {
            try
            {
                _bytesRead = 0;
                _readBuffer = new byte[sizeof(int)];
                Stream.BeginRead(_readBuffer, _bytesRead, BytesRemainingToBeRead, OnReadMessageLengthCompleted, null);
            }
            catch (Exception ex)
            {
                _readAsyncResult.CompleteOperation(ex);
            }
        }

        private void OnReadMessageLengthCompleted(IAsyncResult asyncResult_)
        {
            if (null == _readAsyncResult)
                return; // Protocol has been reset, no further read/write data is of interest.

            try
            {
                var bytesRead = Stream.EndRead(asyncResult_);
                if (0 == bytesRead)
                {
                    // 0 bytes read. Connection has been closed.
                    _readOutput = new NetProtocolReadOutput(NetProtocolReadType.EmptyRead, null);
                    _readAsyncResult.CompleteOperation();
                    return;
                }
                    
                _bytesRead += bytesRead;
                if (0 == BytesRemainingToBeRead)
                {
                    BeginReadMessageContents();
                }
                else
                {
                    Stream.BeginRead(_readBuffer, _bytesRead, BytesRemainingToBeRead, OnReadMessageLengthCompleted, null);
                }
            }
            catch(Exception ex)
            {
                _readAsyncResult.CompleteOperation(ex);
            }
        }

        private void BeginReadMessageContents()
        {
            var messageLength = BitConverter.ToInt32(_readBuffer, 0);
            if (0 == messageLength)
            {
                // Keep alive message. Nothing to do.
                _bytesRead = 0;
                _readBuffer = new byte[0];
                _readOutput = new NetProtocolReadOutput(NetProtocolReadType.KeepAlive, _readBuffer);
                _readAsyncResult.CompleteOperation();
                return;
            }
            
            ValidateReadMessageLength(messageLength);
            _readBuffer = new byte[messageLength];
            _bytesRead = 0;
            Stream.BeginRead(_readBuffer, _bytesRead, BytesRemainingToBeRead, OnReadMessageCompleted, null);
        }

        private void ValidateReadMessageLength(int messageLength_)
        {
            if (0 > messageLength_)
            {
                throw new ProtocolViolationException("Received a negative message length of: " + messageLength_);
            }

            if (MaximumMessageLength < messageLength_)
            {
                throw new ProtocolViolationException("Received a message larger than the maximum permitted size of: " + messageLength_);
            }
        }

        private void OnReadMessageCompleted(IAsyncResult asyncResult_)
        {
            if (null == _readAsyncResult)
                return; // Protocol has been reset, no further read/write data is of interest.

            try
            {
                _bytesRead += Stream.EndRead(asyncResult_);
                if (0 == BytesRemainingToBeRead)
                {
                    _readOutput = new NetProtocolReadOutput(NetProtocolReadType.Message, _readBuffer);
                    _readAsyncResult.CompleteOperation();
                }
                else
                {
                    Stream.BeginRead(_readBuffer, _bytesRead, BytesRemainingToBeRead, OnReadMessageCompleted, null);
                }
            }
            catch (Exception ex)
            {
                _readAsyncResult.CompleteOperation(ex);
            }
        }

        private void OnWriteMessageLengthCompleted(IAsyncResult asyncResult_)
        {
            if (null == _writeAsyncResult)
                return; // Protocol has been reset. No further information is of interest.

            try
            {
                Stream.EndWrite(asyncResult_);
                if (0 == _outgoingMessage.Length)
                {
                    // A keepalive message. It has no body so just end the operation now
                    _writeAsyncResult.CompleteOperation();
                    return;
                }
                Stream.BeginWrite(_outgoingMessage, 0, _outgoingMessage.Length, OnWriteMessageCompleted, null);
            }
            catch (Exception ex)
            {
                _writeAsyncResult.CompleteOperation(ex);
            }
        }

        private void OnWriteMessageCompleted(IAsyncResult asyncResult_)
        {
            if (null == _writeAsyncResult)
                return; // Protocol has been reset. No further information is of interest.

            try
            {
                Stream.EndWrite(asyncResult_);
                _writeAsyncResult.CompleteOperation();
            }
            catch (Exception ex)
            {
                _writeAsyncResult.CompleteOperation(ex);
            }
        }

        private byte[] _outgoingMessage;
        private byte[] _readBuffer;
        private int _bytesRead;
        private INetProtocolReadOutput _readOutput;
        private AsyncResult _writeAsyncResult;
        private AsyncResult _readAsyncResult;
        private static readonly byte[] KeepAliveMessageData = new byte[0];
    }
}