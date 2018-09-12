using System;
using System.Net;
using System.Text;

namespace HmxLabs.Core.Net
{
    /// <summary>
    /// An implementation of <code>IIpEndPointResolved</code>
    /// that uses DNS to resolve machine names to IP addresses
    /// </summary>
    public class DnsEndPointResolver : IIpEndPointResolver
    {
        /// <summary>
        /// Take the given IP address and port and construct an <code>IPEndPoint</code>
        /// 
        /// No DNS resolution is actually required for this version of the function.
        /// </summary>
        /// <param name="address_">The IP adress</param>
        /// <param name="port_">The port</param>
        /// <returns>An IPEndPoint</returns>
        public IPEndPoint Resolve(IPAddress address_, int port_)
        {
            if (null == address_)
                throw new ArgumentNullException(nameof(address_));

            return new IPEndPoint(address_, port_);
        }

        /// <summary>
        /// Take the given string which may contain either an IP address or a hostname and resolve this
        /// to an <code>IPEndPoint</code>
        /// 
        /// The provided string will first be attempted to be parsed as an IP address. If this fails
        /// it is subsequently treated as a machine name and an attept made at DNS resolution to an 
        /// IP address.
        /// 
        /// In the instance that a machine name resolves to multiple IP addresses one will be 
        /// selected at random (using <code>System.Random</code>).
        /// </summary>
        /// <param name="hostnameOrIp_">THe IP adress or hostname</param>
        /// <param name="port_">the port number</param>
        /// <returns>An IPENdPoint</returns>
        public IPEndPoint Resolve(string hostnameOrIp_, int port_)
        {
            if (null == hostnameOrIp_)
                throw new ArgumentNullException(nameof(hostnameOrIp_));

            if (string.IsNullOrWhiteSpace(hostnameOrIp_))
                throw new ArgumentException("Empty or whitespace hostname/IP Address provided");

            IPAddress ipAddress = Resolve(hostnameOrIp_);
            return new IPEndPoint(ipAddress, port_);
        }

        private IPAddress Resolve(string hostnameOrIp_)
        {
            Exception ipParseException;
            try
            {
                return IPAddress.Parse(hostnameOrIp_);
            }
            catch (Exception exp)
            {
                // So not an IP Address. Try and resolve it as hostname instead
                ipParseException = exp;
            }

            IPAddress[] addresses;
            try
            {
                addresses = Dns.GetHostAddresses(hostnameOrIp_);
            }
            catch (Exception exp)
            {
                var message = new StringBuilder();
                message.AppendLine($"Unable to parse [{hostnameOrIp_}] either as an IP address or resolve it as a hostname");
                message.AppendLine($"IP Parse Exception: {ipParseException}");
                message.AppendLine($"DNS Resolve Exception: {exp}");
                
                throw new ArgumentException(message.ToString(), exp);
            }

            return PickIpAddressFromList(addresses, hostnameOrIp_);
        }

        private IPAddress PickIpAddressFromList(IPAddress[] addresses_, string hostnameOrIp_)
        {
            if (null == addresses_ || 0 == addresses_.Length)
                throw new ArgumentException($"The provided hostname [{hostnameOrIp_}] does not resolve to an IP address");

            if (1 == addresses_.Length)
                return addresses_[0];

            var rand = new Random((int)DateTime.UtcNow.Ticks);
            var index = rand.Next() % addresses_.Length;
            return addresses_[index];
        }
    }
}
