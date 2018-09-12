using System.Net;

namespace HmxLabs.Core.Net.Sockets
{
    /// <summary>
    /// Representation of an endpoint in a TCP connection. 
    /// 
    /// This interface provides the various types of data that may be required when using TCP connections
    /// and is essentially a utilty wrapper around the standard .NET framework constructs.
    /// </summary>
    public interface ITcpEndPoint
    {
        /// <summary>
        /// The IP endpoint object 
        /// </summary>
        IPEndPoint IpEndPoint { get; }

        /// <summary>
        /// Read only property specifying if a DnsEndPoint is available. 
        /// This is only available in the case where that end point was instatiated
        /// with a host name (i.e. not an IP address) and DNS resolution was required
        /// </summary>
        bool DnsEndPointAvailable { get; }

        /// <summary>
        /// Read only properly providing the DNS endpoint object, if available.
        /// 
        /// See also <code>DnsEndPointAvailable</code>
        /// </summary>
        DnsEndPoint DnsEndPoint { get; }

        /// <summary>
        /// Read only property indicating if the host name is available.
        /// 
        /// Similarly to the <code>DnsEndPointAvailable</code> property this is only
        /// avaialable if the end point was constructed with a hostname and not an
        /// IP address.
        /// </summary>
        bool HostnameAvailable { get; }

        /// <summary>
        /// Read only property providing the hostname that this end point is connected to
        /// </summary>
        string Hostname { get; }

        /// <summary>
        /// Read only property providing the IP address of this end point
        /// </summary>
        IPAddress IpAddress { get; }

        /// <summary>
        /// Read only property providing the port of this end point
        /// </summary>
        int Port { get; }
    }
}
