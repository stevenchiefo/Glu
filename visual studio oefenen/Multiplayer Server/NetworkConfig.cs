using System;
using System.Net;
using System.Net.Sockets;
using KaymakNetwork.Network;

namespace Multiplayer_Server
{
    internal static class NetworkConfig
    {
        private static Socket Serversv;
        private static Server m_Socket;

        internal static Server socket
        {
            get { return m_Socket; }
            set
            {
                if (m_Socket != null)
                {
                    m_Socket.ConnectionReceived -= Socket_ConnectionReceived;
                    m_Socket.ConnectionLost -= Socket_ConnectionLost;
                }
                m_Socket = value;
                if (m_Socket != null)
                {
                    m_Socket.ConnectionReceived += Socket_ConnectionReceived;
                    m_Socket.ConnectionLost += Socket_ConnectionLost;
                }
            }
        }

        internal static void InitNetwork()
        {
            if (!(m_Socket == null))
                return;

            m_Socket = new Server(100)
            {
                BufferLimit = 2048000,
                PacketAcceptLimit = 100,
                PacketDisconnectCount = 150,
            };

            NetworkReceive.PacketRouter();
            Console.WriteLine("Network Intitialized!");
        }

        internal static void Socket_ConnectionReceived(int connectionID)
        {
            Console.WriteLine("Connection received on index[" + connectionID + "]");
            NetworkSend.WelcomeMsg(connectionID, "Welcome to the Server");
        }

        internal static void Socket_ConnectionLost(int connectionID)
        {
            Console.WriteLine("Connection lost on index[" + connectionID + "]");
        }
    }
}