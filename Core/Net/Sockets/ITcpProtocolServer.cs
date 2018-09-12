namespace HmxLabs.Core.Net.Sockets
{
    /// <summary>
    /// An extension to a <code>INetworkInterfaceServer</code> which operates on TCP connections including
    /// a protocol to frame the data.
    /// </summary>
    public interface ITcpProtocolServer : INetworkInterfaceServer
    {
        /// <summary>
        /// Read/ Write property providing access to the factory object used to construct
        /// new TCP Protocol Clients.
        /// 
        /// This must be set to a valid non null instance of such a factory.
        /// </summary>
        ITcpProtocolClientFactory ProtocolClientFactory { get; set; }
    }
}
