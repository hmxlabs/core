namespace HmxLabs.Core.Net.Sockets
{
    /// <summary>
    /// Enumeration of the different types of data that can be read from a socket
    /// </summary>
    public enum NetProtocolReadType
    {
        /// <summary>
        /// No data wa read from the socket but the read operation completed normally
        /// </summary>
        EmptyRead,
        /// <summary>
        /// A keep alive message was received
        /// </summary>
        KeepAlive,
        /// <summary>
        /// An actual message was read (or part thereof)
        /// </summary>
        Message
    }
}
