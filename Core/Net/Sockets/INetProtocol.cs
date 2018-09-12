using System;
using System.IO;

namespace HmxLabs.Core.Net.Sockets
{
    /// <summary>
    /// This is the implementation of a protocol over the raw stream (in the case of sockets the bytes sent over the wire).
    /// 
    /// This protocol should encompass features such as the framing of the data over the wire, the ability to handle
    /// keep alive messages if required and any other features of the protocol used to communicate between two
    /// endpoints.
    /// </summary>
    public interface INetProtocol
    {
        /// <summary>
        /// The underlying stream that this protocol will operate over
        /// </summary>
        Stream Stream { get; set; }

        /// <summary>
        /// Synchronous blocking read from the underlying data stream of the socket
        /// </summary>
        /// <returns></returns>
        INetProtocolReadOutput Read();

        /// <summary>
        /// Invokes the Read method async. 
        /// </summary>
        /// <param name="callback_">Callback to be notified on upon completion</param>
        /// <param name="state_">Client state to be maintained</param>
        /// <returns></returns>
        IAsyncResult BeginRead(AsyncCallback callback_, object state_);

        /// <summary>
        /// Ends the async read operation. This must ALWAYS be called once the read operation is complete
        /// and is necessary to obtain the output of the read operation when executed async.
        /// </summary>
        /// <param name="asyncResult_">The original IAsyncResult that was provided by the BeginRead operation</param>
        /// <returns></returns>
        INetProtocolReadOutput EndRead(IAsyncResult asyncResult_);

        /// <summary>
        /// Synchronous blocking write operation to the underlying stream of data
        /// </summary>
        /// <param name="data_">The raw message data to be written not including any protocol information such as end of frame characters etc</param>
        void Write(byte[] data_);

        /// <summary>
        /// Invokes the Write method async.
        /// </summary>
        /// <param name="data_">The raw message data to be written not including any protocol information such as end of frame characters etc</param>
        /// <param name="callback_">Callback to be notified on upon completion</param>
        /// <param name="state_">Client state to be maintained</param>
        /// <returns></returns>
        IAsyncResult BeginWrite(byte[] data_, AsyncCallback callback_, object state_);

        /// <summary>
        /// End the write operation. This must always be called once the async write completes
        /// </summary>
        /// <param name="asyncResult_"></param>
        void EndWrite(IAsyncResult asyncResult_);

        /// <summary>
        /// Read only properly providing what a KeepAlive message on this protocol looks like
        /// </summary>
        byte[] KeepAliveMessage { get; }

        /// <summary>
        /// Reset the current state of this protocol to as though it has just connected and no data has been
        /// exchanged
        /// </summary>
        void Reset();
    }
}