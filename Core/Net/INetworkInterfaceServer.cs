using System;
using System.Collections.Generic;

namespace HmxLabs.Core.Net
{
    /// <summary>
    /// Delegate used to notify when a network interface (a client to the <code>INetworkInterfaceServer</code>) undergoes a state change
    /// </summary>
    /// <param name="server_">The server related to the state change</param>
    /// <param name="client_">The client where the state has changed</param>
    public delegate void ClientChangeAction(INetworkInterfaceServer server_, INetworkInterface client_);

    /// <summary>
    /// Delegate used to notify when a network interface server has an error
    /// </summary>
    /// <param name="server_">The server related to the state change</param>
    /// <param name="error_">The exception that was thrown</param>
    public delegate void NetworkInterfaceServerErrorAction(INetworkInterfaceServer server_, Exception error_);

    /// <summary>
    /// This interface represents the abstraction of a network server and compliements 
    /// <code>INetworkInterface</code>. As such it follows similar design principles
    /// 
    /// See the documentation on <code>INetworkInterface</code> for further details
    /// </summary>
    public interface INetworkInterfaceServer : IDisposable
    {
        /// <summary>
        /// Event invoked when a new client connects to this server
        /// </summary>
        event ClientChangeAction ClientConnected;

        /// <summary>
        /// Event invoked when a client disconnects from this server
        /// </summary>
        event ClientChangeAction ClientDisconnected;

        /// <summary>
        /// Event invoked when this server encounters an error. This is mostly just inteded
        /// for information / logging purposes as supposed to providing a mechanism for the
        /// parent code to attempt some form of error/exception handling.
        /// </summary>
        event NetworkInterfaceServerErrorAction ServerError;

        /// <summary>
        /// An enumeration of all the clients currently connected to this server
        /// </summary>
        IEnumerable<INetworkInterface> Clients { get; }

        /// <summary>
        /// Start the server and accept incoming connection
        /// </summary>
        void Start();

        /// <summary>
        /// Stop the server, dispose of any resources and stop accepting new connections and disconnect
        /// any remaining connected clients
        /// </summary>
        void Stop();

        // The following are just pass through events really to facilitate application level code
        /// <summary>
        /// A pass through event that to facilite application code. This is raised by a <code>INetworkConnection</code>
        /// that is connected to this server.
        /// 
        /// See <code>INetworkInterface.MessageReceived</code> for more details
        /// </summary>
        event MessageReceivedAction MessageReceived;

        /// <summary>
        /// A pass through event that to facilite application code. This is raised by a <code>INetworkConnection</code>
        /// that is connected to this server.
        /// 
        /// See <code>INetworkInterface.KeepAliveReceived</code> for more details
        /// </summary>
        event NetworkInterfaceAction KeepAliveReceived;

        /// <summary>
        /// A pass through event that to facilite application code. This is raised by a <code>INetworkConnection</code>
        /// that is connected to this server.
        /// 
        /// See <code>INetworkInterface.ClientConnectionError</code> for more details
        /// </summary>
        event NetworkInterfaceErrorAction ClientConnectionError;

        /// <summary>
        /// A pass through event that to facilite application code. This is raised by a <code>INetworkConnection</code>
        /// that is connected to this server.
        /// 
        /// See <code>INetworkInterface.ClientReceiveError</code> for more details
        /// </summary>
        event NetworkInterfaceErrorAction ClientReceiveError;
    }
}
