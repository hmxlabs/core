using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace HmxLabs.Core.Net.Sockets
{
    /// <summary>
    /// Implementation of <code>ITcpProtocolServer</code>.
    /// 
    /// See <code>ITcpProtocolServer</code> for more details
    /// </summary>
    public class TcpProtocolServer : ITcpProtocolServer
    {
        /// <summary>
        /// Constructor specifying the host and port to listen on. This constructor utilises
        /// the <code>DnsEndPointResolver</code> to resolve the host name to an IP address
        /// </summary>
        /// <param name="host_">The host name to listen on</param>
        /// <param name="port_">The port to listen on</param>
        public TcpProtocolServer(string host_, int port_)
        {
            var resolver = new DnsEndPointResolver();
            var ipEndPoint = resolver.Resolve(host_, port_);
            _tcpListener = new TcpListener(ipEndPoint);
        }

        /// <summary>
        /// Construct specifying the IP Endpoint to listen on. 
        /// </summary>
        /// <param name="endpoint_">The precise IP end pont to listen on</param>
        public TcpProtocolServer(IPEndPoint endpoint_)
        {
            _tcpListener = new TcpListener(endpoint_);
        }

        /// <summary>
        /// Construct the server specifying only a port. This will result in the server listening
        /// on ALL interfaces on the machine including both IPv4 and IPv6 addresses.
        /// </summary>
        /// <param name="port_"></param>
        public TcpProtocolServer(int port_)
        {
            _tcpListener = TcpListener.Create(port_); // Create the listener this way to ensure we listen on all addresses IPv4 and IPv6
        }

        /// <summary>
        /// Destructor as part of Disposable pattern.
        /// </summary>
        ~TcpProtocolServer()
        {
            Dispose(false);
        }

        /// <summary>
        /// Dispose of all open resources. If the 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// See <code>ITcpProtocolServer.ProtocolClientFactory</code>
        /// </summary>
        public ITcpProtocolClientFactory ProtocolClientFactory { get; set; }

        /// <summary>
        /// See <code>ITcpProtocolServer.ClientConnected</code>
        /// </summary>
        public event ClientChangeAction ClientConnected;

        /// <summary>
        /// See <code>ITcpProtocolServer.ClientDisconnected</code>
        /// </summary>
        public event ClientChangeAction ClientDisconnected;

        /// <summary>
        /// See <code>ITcpProtocolServer.ServerError</code>
        /// </summary>
        public event NetworkInterfaceServerErrorAction ServerError;

        /// <summary>
        /// See <code>ITcpProtocolServer.MessageReceived</code>
        /// </summary>
        public event MessageReceivedAction MessageReceived;

        /// <summary>
        /// See <code>ITcpProtocolServer.KeepAliveReceived</code>
        /// </summary>
        public event NetworkInterfaceAction KeepAliveReceived;

        /// <summary>
        /// See <code>ITcpProtocolServer.ClientConnectionError</code>
        /// </summary>
        public event NetworkInterfaceErrorAction ClientConnectionError;

        /// <summary>
        /// See <code>ITcpProtocolServer.ClientReceiveError</code>
        /// </summary>
        public event NetworkInterfaceErrorAction ClientReceiveError;

        /// <summary>
        /// See <code>ITcpProtocolServer.Start</code>
        /// </summary>
        public void Start()
        {
            if (null == ProtocolClientFactory)
                throw new InvalidOperationException("No protocol client factory specified.");

            lock (_lock)
            {
                _tcpListener.Start();
                _tcpListener.BeginAcceptTcpClient(OnClientAccepted, null);
            }
        }

        /// <summary>
        /// See <code>ITcpProtocolServer.Stop</code>
        /// </summary>
        public void Stop()
        {
            lock (_lock)
            {
                _stopped = true;
               _tcpListener.Stop();
            }

            DisconnectAllClients();
        }

        /// <summary>
        /// See <code>ITcpProtocolServer.Clients</code>
        /// </summary>
        public IEnumerable<INetworkInterface> Clients => _clientList.GetClients();

        private void Dispose(bool disposing_)
        {
            if (!disposing_)
                return; // No unmanaged resources to dispose

            if (null == _tcpListener)
                return;

            Stop();
        }

        private void OnClientConnected(ITcpProtocolClient client_)
        {
            _clientList.Add(client_);
            SubscribeToClientEvents(client_);

            ClientConnected?.Invoke(this, client_);
        }

        private void SubscribeToClientEvents(INetworkInterface client_)
        {
            client_.Disconnected += ClientDisconnectEventHandler;
            client_.ConnectionError += ClientConnectionErrorEventHandler;
            client_.MessageReceived += ClientReceiveMessageEventHandler;
            client_.KeepAliveReceived += ClientReceiveKeepAliveEventHandler;
            client_.ReceiveError += ClientReceiveErrorEventHandler;
        }

        private void UnsubscribeFromClientEvents(INetworkInterface client_)
        {
            client_.Disconnected -= ClientDisconnectEventHandler;
            client_.ConnectionError -= ClientConnectionErrorEventHandler;
            client_.MessageReceived -= ClientReceiveMessageEventHandler;
            client_.KeepAliveReceived -= ClientReceiveKeepAliveEventHandler;
            client_.ReceiveError -= ClientReceiveErrorEventHandler;
        }

        private void OnClientDisconnected(ITcpProtocolClient client_)
        {
            UnsubscribeFromClientEvents(client_);
            _clientList.Remove(client_);

            ClientDisconnected?.Invoke(this, client_);
        }

        private void OnConnectionError(Exception exception_)
        {
            ServerError?.Invoke(this, exception_);
        }

        private void OnMessageReceived(INetworkInterface client_, byte[] message_)
        {
            MessageReceived?.Invoke(client_, message_);
        }

        private void OnKeepAliveReceived(INetworkInterface client_)
        {
            KeepAliveReceived?.Invoke(client_);
        }

        private void OnClientConnectionError(INetworkInterface client_, Exception exception_)
        {
            ClientConnectionError?.Invoke(client_, exception_);
        }

        private void OnClientReceiveError(INetworkInterface client_, Exception exception_)
        {
            ClientReceiveError?.Invoke(client_, exception_);
        }

        private void OnClientAccepted(IAsyncResult asyncResult_)
        {
            try
            {
                TcpClient client;
                lock(_lock)
                {
                    if (_stopped) return; // Server has been stopped. Get out here.
                    client = _tcpListener.EndAcceptTcpClient(asyncResult_);
                }

                if (!client.Connected)
                    return;

                var protocolClient = ProtocolClientFactory.CreateConnectedTcpClient(client);
                OnClientConnected(protocolClient);
                protocolClient.InitialisePreConnectedClient();
                lock(_lock){_tcpListener.BeginAcceptTcpClient(OnClientAccepted, null);}
            }
            catch (Exception ex)
            {
                OnConnectionError(ex);
            }
        }

        private void ClientDisconnectEventHandler(INetworkInterface client_, bool connected_)
        {
            var client = client_ as ITcpProtocolClient;
            if (null == client_)
            {
                OnConnectionError(new InvalidOperationException("Unknown type of INetworkInterface in disconnect handler"));
                return;
            }

            OnClientDisconnected(client);
        }

        private void ClientConnectionErrorEventHandler(INetworkInterface client_, Exception exception_)
        {
            OnClientConnectionError(client_, exception_);
        }

        private void ClientReceiveErrorEventHandler(INetworkInterface client_, Exception exception_)
        {
            OnClientReceiveError(client_, exception_);
        }

        private void ClientReceiveMessageEventHandler(INetworkInterface client_, byte[] message_)
        {
            OnMessageReceived(client_, message_);
        }

        private void ClientReceiveKeepAliveEventHandler(INetworkInterface client_)
        {
            OnKeepAliveReceived(client_);
        }

        private void DisconnectAllClients()
        {
            var clients = _clientList.GetClients();
            foreach (var client in clients)
            {
                try
                {
                    if (client.IsConnected)
                        client.Close();
                }
                catch (Exception ex)
                {
                    OnConnectionError(ex);
                }
            }
            _clientList.Clear();
        }

        private readonly TcpListener _tcpListener;
        private readonly object _lock = new object();
        private readonly TcpProtocolClientList _clientList = new TcpProtocolClientList();
        private bool _stopped;
    }
}