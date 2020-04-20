using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiplayer_Server
{
    internal class ServerSend
    {
        private static void SendToData(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            Server.m_Clients[_toClient].Tcp.SendData(_packet);
        }

        private static void SendToDataAll(Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 0; i <= Server.MaxPlayers; i++)
            {
                Server.m_Clients[i].Tcp.SendData(_packet);
            }
        }

        private static void SendToDataAll(int _exceptClient, Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 0; i <= Server.MaxPlayers; i++)
            {
                if (i != _exceptClient)
                {
                    Server.m_Clients[i].Tcp.SendData(_packet);
                }
            }
        }

        public static void Welcome(int _toClient, string msg)
        {
            using (Packet _packet = new Packet((int)ServerPackets.welcome))
            {
                _packet.Write(msg);
                _packet.Write(_toClient);

                SendToData(_toClient, _packet);
            }
        }
    }
}