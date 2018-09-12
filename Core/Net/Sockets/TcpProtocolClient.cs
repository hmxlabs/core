using System;
using System.Net;
using System.Net.Sockets;
using HmxLabs.Core.Threading;

namespace HmxLabs.Core.Net.Sockets
{
    /// <summary>
    /// Implementation of <code>ITcpProtocolClient</code>.
    /// 
    /// See <code>ITcpProtocolClient</code> for more details.
    /// </summary>
    public class TcpProtocolClient : ITcpProtocolClient
    {
        /// <summary>
        /// This constructor should only be used when an established TCP connection is present. The provided TCP client should
        /// already be in a connected state.
        /// </summary>
        /// <param name="protocol_"></param>
        /// <param name="client_"></param>
        public TcpProtocolClient(INetProtocol protocol_, TcpClient client_)
        {
            if (null == protocol_)
                throw new ArgumentNullException(nameof(protocol_));

            if (null == client_)
                throw new ArgumentNullException(nameof(client_));

            try
            {
                if (!client_.Connected)
                    throw new ArgumentException("Non connected TcpClient provided");
            }
            catch (Exception exp)
            {
                throw new ArgumentException("Non connected TcpClient provided", exp);
            }

            Protocol = protocol_;
            Client = client_;
            Protocol.Stream = Client.GetStream(); // Need to do this upfront too to ensure IsConnected works

            // Bit odd to throw parameter exceptions after accepting and assigning them...
            // just want to double check though that everything worked and we are connected as per this objects
            // definition of being connected.
            if (!IsConnected)
                throw new ArgumentException("The provided TcpClient object is not connected.");
        }

        /// <summary>
        /// This constructor should be used if a new connection is being created, for example in a client side application.
        /// The <code>TcpProtocolListener</code> should not attempt to use this constructor.
        /// </summary>
        /// <param name="protocol_"></param>
        /// <param name="hostnameOrIpAddress_"></param>
        /// <param name="port_"></param>
        /// <param name="resolver_"></param>
        public TcpProtocolClient(INetProtocol protocol_, string hostnameOrIpAddress_, int port_, IIpEndPointResolver resolver_ = null)
        {
            if (null == protocol_)
                throw new ArgumentNullException(nameof(protocol_));

            if (null == hostnameOrIpAddress_)
                throw new ArgumentNullException(nameof(hostnameOrIpAddress_));

            if (string.IsNullOrWhiteSpace(hostnameOrIpAddress_))
                throw new ArgumentException("Empty hostname provided");

            Protocol = protocol_;
            Hostname = hostnameOrIpAddress_;
            Port = port_;
            EndPointResolver = resolver_ ?? new DnsEndPointResolver();

            // Don't create the tcp client yet... we need the IPAddressFamily so create it at the point Connect is called
            // to allow the caller to have a chance to change the EndPointResolver.
        }

        /// <summary>
        /// Destructor implemented as part of standard .NET disposable pattern
        /// </summary>
        ~TcpProtocolClient()
        {
            Dispose(false);
        }
        
        /// <summary>
        /// Clean up all resources used by this object. Calling this will also
        /// disconnect / close the connection if this hasn't already been done.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Property indicatin if this client is connected. Relies on the underlying
        /// <code>TcpClient.Connected</code> property but also takes account of 
        /// other factors including the presence of a Protocol Stream to ensure
        /// that we are genuinely able to transfer data.
        /// 
        /// See also <code>ITcpProtocolClient.IsConnection</code>.
        /// </summary>
        public bool IsConnected
        {
            get
            { 
                if (null == Client) return false;
                try
                {
                    return Client.Client.Connected &&  null != Protocol.Stream;
                }
                catch (Exception)
                {
                    // This can happen if we have already called close on the underlying connection.
                    // For some obtuse reason, instead of returning false it will throw an NullReferenceException!
                    // This isn't particularly meaningful to caller, really all the caller cares abouts is whether
                    // or not the connection is established. return that information!

                    // If this caused an exception, pretty sure we're not connected anymore!
                    return false;
                }
            }
        }

        /// <summary>
        /// See <code>INetworkInterface.MessageReceived</code>.
        /// </summary>
        public event MessageReceivedAction MessageReceived;

        /// <summary>
        /// See <code>INetworkInterface.KeepAliveReceived</code>.
        /// </summary>
        public event NetworkInterfaceAction KeepAliveReceived;

        /// <summary>
        /// See <code>INetworkInterface.ReceiveError</code>.
        /// </summary>
        public event NetworkInterfaceErrorAction ReceiveError;

        /// <summary>
        /// See <code>INetworkInterface.ConnectionError</code>.
        /// </summary>
        public event NetworkInterfaceErrorAction ConnectionError;

        /// <summary>
        /// See <code>INetworkInterface.Connected</code>.
        /// </summary>
        public event ConnectionStatusAction Connected;

        /// <summary>
        /// See <code>INetworkInterface.Disconnected</code>.
        /// </summary>
        public event ConnectionStatusAction Disconnected;

        /// <summary>
        /// See <code>ITcpProtocolClient.Protocol</code>.
        /// </summary>
        public INetProtocol Protocol { get; protected set; }

        /// <summary>
        /// Property providing access to the <code>IIpEndPointResolver</code> to allow other modes
        /// of IP address resolution (or testing).
        /// </summary>
        public IIpEndPointResolver EndPointResolver
        {
            get => _endPointResolver;
            set { _endPointResolver = value ?? new DnsEndPointResolver(); }
        }
        
        /// <summary>
        /// The hostname this client is connected to
        /// </summary>
        public string Hostname { get; protected set; }

        /// <summary>
        /// The port this client is connected to
        /// </summary>
        public int Port { get; protected set; }

        /// <summary>
        /// The remote IPEndPoint, this is determined by using the
        /// <code>EndPointResolver</code>.to resolve the provided <code>Hostname</code>
        /// and then combined with the <code>Port</code> specified
        /// </summary>
        public IPEndPoint RemoteEndPoint => EndPointResolver.Resolve(Hostname, Port);

        /// <summary>
        /// If (and only if!) the connection is created with a pre-connected TcpClient, it will need to be initialised.
        /// This is done as a two step process rather than immediately in construction in order to allow users of
        /// this class to subscribe to any events before the thread reading from the connection is started in order
        /// to ensure that no messages are missed.
        /// </summary>
        public void InitialisePreConnectedClient()
        {
            DoPostConnectActions();
        }

        /// <summary>
        /// Connect this client to its remote end pont synchronously.
        /// 
        /// See also <code>INetworkInterface.Connect</code>
        /// </summary>
        public virtual void Connect()
        {
            DoConnect();
        }

        /// <summary>
        /// Connect this client to its remote end point async.
        /// 
        /// See also <code>INetworkInterface.BeginConnect</code>.
        /// </summary>
        /// <param name="callback_">Callback to be notified on once the connection completes</param>
        /// <param name="state_">User state to be maintained</param>
        /// <returns></returns>
        public virtual IAsyncResult BeginConnect(AsyncCallback callback_, object state_)
        {
            return DoBeginConnect(callback_, state_);
        }

        /// <summary>
        /// End the async connection after it has completed. This must always be called
        /// 
        /// See also <code>INetworkInterface.EndConnect</code>
        /// </summary>
        /// <param name="asyncResult_"></param>
        public virtual void EndConnect(IAsyncResult asyncResult_)
        {
            DoEndConnect(asyncResult_);
        }

        /// <summary>
        /// Close this connection and disconnect from the remote endpoint.
        /// 
        /// See also <code>INetworkInterface.Close</code>
        /// </summary>
        public virtual void Close()
        {
            DoClose();
        }

        /// <summary>
        /// See <code>INetworkInterface.Send</code>
        /// </summary>
        /// <param name="message_"></param>
        public virtual void Send(byte[] message_)
        {
            DoSend(message_);
        }

        /// <summary>
        /// See <code>INetworkInterface.BeginSend</code>
        /// </summary>
        /// <param name="message_"></param>
        /// <param name="callback_"></param>
        /// <param name="state_"></param>
        /// <returns></returns>
        public virtual IAsyncResult BeginSend(byte[] message_, AsyncCallback callback_, object state_)
        {
            return DoBeginSend(message_, callback_, state_);
        }

        /// <summary>
        /// See <code>INetworkInterface.EndSend</code>
        /// </summary>
        /// <param name="asyncResult_"></param>
        public virtual void EndSend(IAsyncResult asyncResult_)
        {
            DoEndSend(asyncResult_);
        }

        /// <summary>
        /// Folloeing .NET disposable pattern, implementation of dispose functionality.
        /// </summary>
        /// <param name="disposing_"></param>
        protected virtual void Dispose(bool disposing_)
        {
            if (!disposing_)
                return; // no unmanaged resources to dispose.

            if (null == Client)
                return;

            Close();
            Client = null;
        }

        /// <summary>
        /// The underlying TcpClient that used for the connection
        /// </summary>
        protected TcpClient Client { get; set; }

        /// <summary>
        /// Invokes the Connected event provided there are regitered listeners.
        /// </summary>
        protected virtual void OnConnected()
        {
            Connected?.Invoke(this, Client.Connected);
        }

        /// <summary>
        /// Invokes the Disconnected event and resets the state of this connection
        /// </summary>
        protected virtual void OnDisconnected()
        {
            lock(_keepReadingLock)
            {
                _keepReading = false;
            }
            Protocol.Reset();

            Disconnected?.Invoke(this, IsConnected);
        }

        /// <summary>
        /// Invokes the ReceiveError event.
        /// </summary>
        /// <param name="exception_"></param>
        protected virtual void OnReceiveError(Exception exception_)
        {
            ReceiveError?.Invoke(this, exception_);
        }

        /// <summary>
        /// Invokes the ConnectionError event.
        /// </summary>
        /// <param name="exception_"></param>
        protected virtual void OnConnectionError(Exception exception_)
        {
            ConnectionError?.Invoke(this, exception_);
        }

        /// <summary>
        /// Invokes the MessageReceived event
        /// </summary>
        /// <param name="message_"></param>
        protected virtual void OnMessageReceived(byte[] message_)
        {
            MessageReceived?.Invoke(this, message_);
        }

        /// <summary>
        /// Invokes the KeepAliveReceived event
        /// </summary>
        protected virtual void OnKeepAliveReceived()
        {
            KeepAliveReceived?.Invoke(this);
        }

        /// <summary>
        /// The underlyning method called by <code>Connect</code>. Does the actual work
        /// of establishing the remote endpont and creating a TcpClient to connect to it
        /// </summary>
        protected void DoConnect()
        {
            var endPoint = RemoteEndPoint; // Get this once to ensure we keep the same address between construction and connection
            Client = new TcpClient(endPoint.AddressFamily);
            Client.Connect(endPoint);
            DoPostConnectActions();
        }

        /// <summary>
        /// Carries out the actions required once a connection is established such as
        /// setting the stream on the underlying protocol and notifying listeners as
        /// well actually starting to listen on the socket
        /// </summary>
        protected void DoPostConnectActions()
        {
            Protocol.Stream = Client.GetStream();
            OnConnected();
            DoStartReceiving();
        }

        /// <summary>
        /// Underlying method called by <code>BeginConnect</code>. 
        /// 
        /// The end point is established synchronously, however the connection to the remote endpoint
        /// is then handled async
        /// </summary>
        /// <param name="callback_"></param>
        /// <param name="state_"></param>
        /// <returns></returns>
        protected IAsyncResult DoBeginConnect(AsyncCallback callback_, object state_)
        {
            var endPoint = RemoteEndPoint; // Need to get this once to ensure we keep the same one between construction and connection
            Client = new TcpClient(endPoint.AddressFamily);
            var outerAsyncResult = new AsyncResult(callback_, state_);
            Client.BeginConnect(endPoint.Address, endPoint.Port, ConnectComplete, outerAsyncResult);
            return outerAsyncResult;
        }

        /// <summary>
        /// Method invoked when the internal TcpClient connects.
        /// </summary>
        /// <param name="asyncResult_"></param>
        protected void ConnectComplete(IAsyncResult asyncResult_)
        {
            var outerAsyncResult = asyncResult_.AsyncState as AsyncResult;
            if (null == outerAsyncResult)
                throw new InvalidOperationException("Unable to retrieve state from async result.");

            try
            {
                Client.EndConnect(asyncResult_);
                Protocol.Stream = Client.GetStream();
                outerAsyncResult.CompleteOperation();
                OnConnected();
                DoStartReceiving();
            }
            catch (Exception exp)
            {
                outerAsyncResult.CompleteOperation(exp);
                OnConnectionError(exp);
            }
        }

        /// <summary>
        /// Do the actual work of ending the connect operation
        /// </summary>
        /// <param name="asyncResult_"></param>
        protected void DoEndConnect(IAsyncResult asyncResult_)
        {
            var asyncResult = asyncResult_ as AsyncResult;
            if (null == asyncResult)
                throw new ArgumentException("Invalid IAsyncResult provided.");

            asyncResult.EndOperation();
        }

        /// <summary>
        /// Do the work of closing the connection.
        /// </summary>
        protected void DoClose()
        {
            DoStopReceiving();
            
            if (IsConnected)
                Client.Close();

            Protocol.Reset();
            OnDisconnected();
            MessageReceived = null;
            KeepAliveReceived = null;
            ConnectionError = null;
            ReceiveError = null;
            Connected = null;
            Disconnected = null;
        }
        
        /// <summary>
        /// Do the actual work of sending the message.
        /// In reality this just a pass through to the protocol which
        /// knows how to frame the data.
        /// </summary>
        /// <param name="message_"></param>
        protected void DoSend(byte[] message_)
        {
            if (!IsConnected)
                throw new InvalidOperationException("Not connected. Can not send");

            Protocol.Write(message_);
        }

        /// <summary>
        /// Do the work of sending the data async.
        /// </summary>
        /// <param name="message_"></param>
        /// <param name="callback_"></param>
        /// <param name="state_"></param>
        /// <returns></returns>
        protected IAsyncResult DoBeginSend(byte[] message_, AsyncCallback callback_, object state_)
        {
            if (!IsConnected)
                throw new InvalidOperationException("Not connected. Can not send");

            var asyncResult = new AsyncResult(callback_, state_);
            Protocol.BeginWrite(message_, SendComplete, asyncResult);
            return asyncResult;
        }

        /// <summary>
        /// Callback invoked when the protocol completes its write of the data to be sent
        /// </summary>
        /// <param name="asyncResult_"></param>
        protected void SendComplete(IAsyncResult asyncResult_)
        {
            var outerAsyncResult = asyncResult_.AsyncState as AsyncResult;
            if (null == outerAsyncResult)
                throw new InvalidOperationException("Unexpected value for async state");

            try
            {
                Protocol.EndWrite(asyncResult_);
                outerAsyncResult.CompleteOperation();
            }
            catch (Exception exp)
            {
                outerAsyncResult.CompleteOperation(exp);
                OnDisconnected();
            }
        }

        /// <summary>
        /// Does the actual work of ending the send async operation
        /// </summary>
        /// <param name="asyncResult_"></param>
        protected void DoEndSend(IAsyncResult asyncResult_)
        {
            var asyncResult = asyncResult_ as AsyncResult;
            if (null == asyncResult)
                throw new ArgumentException("Invalid IAsyncResult provided.");

            asyncResult.EndOperation();
        }

        /// <summary>
        /// Start listening on the stream underlying the TcpClient.
        /// In reality this actually delegated to the Protocol which
        /// knows how the data is framed on the wire and how to deserialise it
        /// </summary>
        protected void DoStartReceiving()
        {
            lock (_keepReadingLock)
            {
                _keepReading = true;
            }

            Protocol.BeginRead(OnReadComplete, null);
        }

        /// <summary>
        /// Called when the protocol completes the read of a message from the stream.
        /// 
        /// Several edge cases need to be handled here in addition to the obviou use case
        /// of passing the received data to the application. These include dealing with
        /// KeepAlive messages and disconnections.
        /// </summary>
        /// <param name="asyncResult_"></param>
        protected void OnReadComplete(IAsyncResult asyncResult_)
        {
            lock(_keepReadingLock)
            {
                if (!_keepReading)
                    return;
            }

            try
            {
                var readResult = Protocol.EndRead(asyncResult_);
                if (NetProtocolReadType.EmptyRead == readResult.ReadType)
                {
                    // It would appear that the connection is not always closed but can sometimes 
                    // hang in a CLOSE_WAIT state. To ensure a proper clean up, call close here
                    // explicitly to ensure that this doesn't happen.
                    Close();
                    return;
                }

                if (NetProtocolReadType.Message == readResult.ReadType)
                {
                    OnMessageReceived(readResult.Message);
                }

                if (NetProtocolReadType.KeepAlive == readResult.ReadType)
                {
                    OnKeepAliveReceived();
                }

                Protocol.BeginRead(OnReadComplete, null);
            }
            catch (Exception exception)
            {
                if (!IsConnected)
                    OnDisconnected();

                OnReceiveError(exception);
            }
        }

        /// <summary>
        /// Does the actual work stopping listening.
        /// </summary>
        protected void DoStopReceiving()
        {
            lock(_keepReadingLock)
            {
                _keepReading = false;
            }
        }

        private readonly object _keepReadingLock = new object();
        private bool _keepReading;
        private IIpEndPointResolver _endPointResolver;
    }
}