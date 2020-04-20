using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiplayer_Server
{
    internal class ServerHandle
    {
        public static void WelcomeReceived(int fromclient, Packet packet)
        {
            int clientIdCheck = packet.ReadInt();
            string username = packet.ReadString();

            Console.WriteLine($"{Server.m_Clients[fromclient].Tcp.socket.Client.RemoteEndPoint} connected succesfully and is now player {fromclient}.");
            Console.WriteLine("Welcome " + username + " To the server");
            if (fromclient != clientIdCheck)
            {
                Console.WriteLine($"Player \"{username}\" (ID: { fromclient}) has assumed the wrong client ID ({clientIdCheck})|");
            }
        }
    }
}