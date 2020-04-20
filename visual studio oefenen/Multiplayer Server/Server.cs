using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Multiplayer_Server
{
    internal static class Server
    {
        public static int MaxPlayers { get; private set; }
        public static int Port { get; private set; }

        public static Dictionary<int, Client> m_Clients = new Dictionary<int, Client>();

        private static TcpListener m_TcpListener;

        internal static void Start(int _maxPlayers, int _port)
        {
            MaxPlayers = _maxPlayers;
            Port = _port;

            Console.WriteLine("Starting server....");
            InitializeServerData();

            m_TcpListener = new TcpListener(IPAddress.Any, Port);
            m_TcpListener.Start();
            m_TcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);

            Console.WriteLine($"Server stated on {Port}.");
        }

        private static void TCPConnectCallback(IAsyncResult _result)
        {
            TcpClient _client = m_TcpListener.EndAcceptTcpClient(_result);
            m_TcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);
            Console.WriteLine($"Incoming connection from {_client.Client.RemoteEndPoint}.....");

            for (int i = 1; i <= MaxPlayers; i++)
            {
                if (m_Clients[i].Tcp.socket == null)
                {
                    m_Clients[i].Tcp.Connect(_client);
                    return;
                }
            }

            Console.WriteLine($"{_client.Client.RemoteEndPoint} failed to connect: Server full!");
        }

        private static void InitializeServerData()
        {
            for (int i = 1; i <= MaxPlayers; i++)
            {
                m_Clients.Add(i, new Client(i));
            }
        }
    }
}