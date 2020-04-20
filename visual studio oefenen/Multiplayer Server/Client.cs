using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Multiplayer_Server
{
    internal class Client
    {
        public static int databuffersize = 4096;

        public int Id;
        public TCP Tcp;

        public Client(int _clientID)
        {
            Id = _clientID;
            Tcp = new TCP(_clientID);
        }

        public class TCP
        {
            public TcpClient socket;
            private Packet receivedData;
            private readonly int id;
            private NetworkStream stream;
            private byte[] receivebuffer;

            public TCP(int _id)
            {
                id = _id;
            }

            public void Connect(TcpClient _socket)
            {
                socket = _socket;
                socket.ReceiveBufferSize = databuffersize;
                socket.SendBufferSize = databuffersize;

                stream = socket.GetStream();

                receivedData = new Packet();
                receivebuffer = new byte[databuffersize];

                stream.BeginRead(receivebuffer, 0, databuffersize, ReceiveCallBack, null);

                ServerSend.Welcome(id, "Welcome client[" + id + "]");
            }

            public void SendData(Packet _packet)
            {
                stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null);
            }

            private void ReceiveCallBack(IAsyncResult _result)
            {
                try
                {
                    int byteLenght = stream.EndRead(_result);
                    if (byteLenght <= 0)
                    {
                        return;
                    }

                    byte[] _data = new byte[byteLenght];
                    Array.Copy(receivebuffer, _data, byteLenght);

                    receivedData.Reset(HandleData(_data));
                    stream.BeginRead(receivebuffer, 0, databuffersize, ReceiveCallBack, null);
                }
                catch (Exception _ex)
                {
                    Console.WriteLine("Error = " + _ex);
                }
            }

            private bool HandleData(byte[] _data)
            {
                int packetlenght = 0;

                receivedData.SetBytes(_data);

                if (receivedData.UnreadLength() >= 4)
                {
                    packetlenght = receivedData.ReadInt();
                    if (packetlenght <= 0)
                    {
                        return true;
                    }
                }
                while (packetlenght > 0 && packetlenght <= receivedData.UnreadLength())
                {
                    byte[] packetBytes = receivedData.ReadBytes(packetlenght);
                    ThreadManager.ExecuteOnMainThread(() =>
                    {
                        using (Packet packet = new Packet(packetBytes))
                        {
                            int packetId = packet.ReadInt();
                            Server.packetHandler[packetId](id, packet);
                        }
                    });

                    packetlenght = 0;
                    if (receivedData.UnreadLength() >= 4)
                    {
                        packetlenght = receivedData.ReadInt();
                        if (packetlenght <= 0)
                        {
                            return true;
                        }
                    }
                }
                if (packetlenght <= 1)
                {
                    return true;
                }

                return false;
            }
        }
    }
}