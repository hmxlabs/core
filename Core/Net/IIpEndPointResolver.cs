using System.Net;

namespace HmxLabs.Core.Net
{
    /// <summary>
    /// Utility class used to resolve from an IP address and port to an <code>IPEndPoint</code>
    /// </summary>
    public interface IIpEndPointResolver
    {
        /// <summary>
        /// Take the given IP address and port and construct an <code>IPEndPoint</code>
        /// </summary>
        /// <param name="address_">The IP adress</param>
        /// <param name="port_">The port</param>
        /// <returns>An IPEndPoint</returns>
        IPEndPoint Resolve(IPAddress address_, int port_);

        /// <summary>
        /// Take the given string which may contain either an IP address or a hostname and resolve this
        /// to an <code>IPEndPoint</code>
        /// </summary>
        /// <param name="hostnameOrIp_">THe IP adress or hostname</param>
        /// <param name="port_">the port number</param>
        /// <returns>An IPENdPoint</returns>
        IPEndPoint Resolve(string hostnameOrIp_, int port_);
    }
}
