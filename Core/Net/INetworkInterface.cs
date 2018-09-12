using System;

namespace HmxLabs.Core.Net
{
    /// <summary>
    /// Delegate used to notify receipt of a new message on the network interface
    /// </summary>
    /// <param name="networkInterface_"></param>
    /// <param name="message_"></param>
    public delegate void MessageReceivedAction(INetworkInterface networkInterface_, byte[] message_);

    /// <summary>
    /// Delegate used to notify of some action (for example a keep alive ping) taking place on the network interface
    /// </summary>
    /// <param name="networkInterface_"></param>
    public delegate void NetworkInterfaceAction(INetworkInterface networkInterface_);

    /// <summary>
    /// Delegate used to notify of some error condition taking place on the network interface
    /// </summary>
    /// <param name="networkInterface_"></param>
    /// <param name="exception_"></param>
    public delegate void NetworkInterfaceErrorAction(INetworkInterface networkInterface_, Exception exception_);

    /// <summary>
    /// Delegate used to notify of a connection or disconnection event on the network interface
    /// </summary>
    /// <param name="networkInterface_"></param>
    /// <param name="connected_"></param>
    public delegate void ConnectionStatusAction(INetworkInterface networkInterface_, bool connected_);

    /// <summary>
    /// This interface represents an abstraction of a network interface. The idea being that all network
    /// interfaces, regardless of whether they are HTTP, REST, TCP or other messaging technologies should
    /// always just be an implementaton of this interface.
    /// 
    /// Doing so allows the application developer to swap out one type of network connection for another
    /// without impacting large areas of the code and ensure that the logic of the application remains
    /// agnostic of the communications technology with which it is currently implemented.
    /// 
    /// It also aids testing greatly as all network interfaces are therefore interfaced and may be mocked
    /// or replaced with alternative implementations to allow automated testing.
    /// 
    /// The code pred-dates the TPL and could do with being updated to support the new Async
    /// programming paradigms now available in the .NET framework.
    /// </summary>
    public interface INetworkInterface : IDisposable
    {
        /// <summary>
        /// Synchronously connect this network interface to its endpoint
        /// </summary>
        void Connect();

        /// <summary>
        /// Async connections request to the endpoint
        /// </summary>
        /// <param name="callback_">The delegate to be called back on once the connection completes</param>
        /// <param name="state_">Arbitarty client state to maintain</param>
        /// <returns></returns>
        IAsyncResult BeginConnect(AsyncCallback callback_, object state_);

        /// <summary>
        /// End the connection operation. This must be called once the async connection is complete in order to
        /// use the connection object.
        /// </summary>
        /// <param name="asyncResult_"></param>
        void EndConnect(IAsyncResult asyncResult_);

        /// <summary>
        /// Read only property indicating if this network connection is connected to its endpoint
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// Close the network interface. Calling this method will also dispose of any open resources
        /// and remove all callbacks to ensure that no references are held and that no further notifications
        /// are received from this <code>INetworkInterface</code> instance.
        /// </summary>
        void Close();

        /// <summary>
        /// Send the specified byte array
        /// </summary>
        /// <param name="message_">The message to send</param>
        void Send(byte[] message_);

        /// <summary>
        /// Asynchrounously send the specified message
        /// </summary>
        /// <param name="message_"></param>
        /// <param name="callback_">The delegate to be called back on once the message sending completes</param>
        /// <param name="state_">Arbitarty client state to maintain</param>
        /// <returns></returns>
        IAsyncResult BeginSend(byte[] message_, AsyncCallback callback_, object state_);

        /// <summary>
        /// End the send message operation. This method must be called on completion of the async send.
        /// </summary>
        /// <param name="asyncResult_"></param>
        void EndSend(IAsyncResult asyncResult_);

        /// <summary>
        /// Event that is invoked when a new message is received on this interface
        /// </summary>
        event MessageReceivedAction MessageReceived;

        /// <summary>
        /// Event that is invoked when a keep alive message is received on this interface
        /// </summary>
        event NetworkInterfaceAction KeepAliveReceived;

        /// <summary>
        /// Event that is invoked when a connection error occurs on this interface
        /// </summary>
        event NetworkInterfaceErrorAction ConnectionError;

        /// <summary>
        /// Event that is invoked when an errors occurs when receiving data
        /// </summary>
        event NetworkInterfaceErrorAction ReceiveError;

        /// <summary>
        /// Event that is invoked when the connection is established
        /// </summary>
        event ConnectionStatusAction Connected;

        /// <summary>
        /// The delegate to be called back on if the network interface connection is lost.
        /// Once this delegate has been called, no further callbacks from this network
        /// interface will be made and all delegate properties will be set to null
        /// to ensure no references are held. Effectively being called on this delegate
        /// is equivalent to the <code>Close</code> method having been called.
        /// </summary>
        event ConnectionStatusAction Disconnected;
    }
}