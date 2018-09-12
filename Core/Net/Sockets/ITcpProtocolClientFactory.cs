using System.Net.Sockets;

namespace HmxLabs.Core.Net.Sockets
{
    /// <summary>
    /// This factory is intended for use by instances of the <code>ITcpProtocolListener</code> class to in order
    /// to create the correct type of <code>ITcpProtocolClient</code> instances upon accepting incoming connections.
    /// </summary>
    public interface ITcpProtocolClientFactory
    {
        /// <summary>
        /// Create a new protocol client given an existing <code>TcpClient</code> connection.
        /// </summary>
        /// <param name="tcpClient_">The existing, already connected TcpClient</param>
        /// <returns></returns>
        ITcpProtocolClient CreateConnectedTcpClient(TcpClient tcpClient_);
    }
}
