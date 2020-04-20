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

                receivebuffer = new byte[databuffersize];

                stream.BeginRead(receivebuffer, 0, databuffersize, ReceiveCallBack, null);
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

                    stream.BeginRead(receivebuffer, 0, databuffersize, ReceiveCallBack, null);
                }
                catch (Exception _ex)
                {
                    Console.WriteLine("Error = " + _ex);
                }
            }
        }
    }
}