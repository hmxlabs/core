using System.Net.Sockets;

namespace HmxLabs.Core.Net.Sockets
{
    /// <summary>
    /// An implementation of <code>ITcpProtocolClientFactory</code> that
    /// always returns <code>TcpProtocolClient</code> objects with
    /// a <code>LengthPrefixNetProtocol</code>
    /// </summary>
    public class LengthPrefixProtocolClientFactory : ITcpProtocolClientFactory
    {
        /// <summary>
        /// Contructs a new <code>TcpProtocolClient</code> using a 
        /// <code>LengthPrefixNetProtocol</code>.
        /// 
        /// See also <code>ITcpProtocolClientFactory.CreateConnectedTcpClient</code>
        /// </summary>
        /// <param name="tcpClient_"></param>
        /// <returns></returns>
        public ITcpProtocolClient CreateConnectedTcpClient(TcpClient tcpClient_)
        {
            return new TcpProtocolClient(new LengthPrefixNetProtocol(), tcpClient_);
        }
    }
}
