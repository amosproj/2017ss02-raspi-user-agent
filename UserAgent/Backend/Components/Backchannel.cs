using CommonFiles.Networking;
using CommonFiles.TransferObjects;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RaspberryBackend.Components
{
    /// <summary>
    /// Socket, which can be used by multiple threads for sending.
    /// </summary>
    class BackChannel
    {
        private TcpClient socket;

        /// <summary>
        /// Sets the socket.
        /// </summary>
        /// <param name="socket">Socket, which will be used for sending.</param>
        public void setClient(TcpClient socket)
        {
            this.socket = socket;
        }

        /// <summary>
        /// Sends an Object over the socket.
        /// </summary>
        /// <param name="obj">Object which will be sent.</param>
        public void sendObject(Object obj)
        {
            lock (this)
            {
                Transfer.sendObject(socket.GetStream(), obj);
            }
        }
    }
}