namespace HmxLabs.Core.Net.Sockets
{
    /// <summary>
    /// An interface defining the output of a read operation from a socket
    /// </summary>
    public interface INetProtocolReadOutput
    {
        /// <summary>
        /// The actual data that was read from the socket
        /// </summary>
        byte[] Message { get; }

        /// <summary>
        /// A classification of the data that was read.
        /// </summary>
        NetProtocolReadType ReadType { get; }
    }
}