namespace HmxLabs.Core.Net.Sockets
{
    /// <summary>
    /// Implementation of a <code>INetProtocolReadOutput</code>.
    /// 
    /// See <code>INetProtocolReadOutput</code> for more details
    /// </summary>
    public sealed class NetProtocolReadOutput : INetProtocolReadOutput
    {
        /// <summary>
        /// Constructor.
        /// 
        /// Though not checked, normally both the readType and message should be set
        /// to valid non null objects
        /// </summary>
        /// <param name="readType_">The type of read that occurred from the stream</param>
        /// <param name="message_">The message payload</param>
        public NetProtocolReadOutput(NetProtocolReadType readType_, byte[] message_)
        {
            ReadType = readType_;
            Message = message_;
        }

        /// <summary>
        /// See <code>INetProtocolReadOutput.Message</code>.
        /// </summary>
        public byte[] Message { get; }

        /// <summary>
        /// See <code>INetProtocolReadOutput.ReadType</code>
        /// </summary>
        public NetProtocolReadType ReadType { get; }
    }
}
