using System.Collections.Generic;

namespace HmxLabs.Core.Net.Sockets
{
    /// <summary>
    /// A simple container used to store a list of <code>ITcpProtocolClient</code>s.
    /// 
    /// Used by the <code>TcpProtocolServer</code> to store its internal list of client
    /// connections
    /// 
    /// Implemented simply as a <code>List</code> with simple locking to provide thread safe access.
    /// </summary>
    public class TcpProtocolClientList
    {
        /// <summary>
        /// Add a client to the list.
        /// </summary>
        /// <param name="client_"></param>
        public void Add(ITcpProtocolClient client_)
        {
            lock(_lock)
            {
                _list.Add(client_);
            }
        }

        /// <summary>
        /// Remove a client from this list
        /// </summary>
        /// <param name="client_"></param>
        public void Remove(ITcpProtocolClient client_)
        {
            lock(_lock)
            {
                _list.Remove(client_);
            }
        }

        /// <summary>
        /// Get an enumeration of all the clients in this list
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ITcpProtocolClient> GetClients()
        {
            lock (_lock)
            {
                var clients = new ITcpProtocolClient[_list.Count];
                _list.CopyTo(clients);
                return clients;
            }
        }

        /// <summary>
        /// Clear out the contents of this list
        /// </summary>
        public void Clear()
        {
            lock(_lock)
            {
                _list.Clear();
            }
        }

        private readonly List<ITcpProtocolClient> _list = new List<ITcpProtocolClient>();
        private readonly object _lock = new object();
    }
}