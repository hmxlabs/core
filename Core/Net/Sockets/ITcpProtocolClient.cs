namespace HmxLabs.Core.Net.Sockets
{
    /// <summary>
    /// A network connection (<code>INetworkInterface</code>) that operate over TCP IP
    /// and includes a protocol for framing the data over the wire.
    /// </summary>
    public interface ITcpProtocolClient : INetworkInterface
    {
        /// <summary>
        /// Read only property providing the network protocol that should be used to stream
        /// the data over the wire
        /// </summary>
        INetProtocol Protocol { get; }

        /// <summary>
        /// Provides a mechanism to initialise the connection when it is constructed with a 
        /// <code>TcpClient</code> that is already connected rather than allowing it to construct
        /// a new client connection object.
        /// 
        /// This is the case generally when a TCP listener has accepted an incoming connection
        /// and hands off a now connected client end point.
        /// </summary>
        void InitialisePreConnectedClient();
    }
}