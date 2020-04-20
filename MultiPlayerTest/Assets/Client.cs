using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Client : MonoBehaviour
{
    public static Client instance;
    public static int dataBufferSize = 4096;

    public string ip = "127.0.0.1";
    public int port = 26950;
    public int myId = 0;
    public TCP tcp;

    private delegate void PacketHandler(Packet packet);

    private static Dictionary<int, PacketHandler> packethandler;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instace already exists, Destroying object");
            Destroy(this);
        }
    }

    private void Start()
    {
        tcp = new TCP();
    }

    public void ConnectedToServer()
    {
        InitializeClientData();
        tcp.Connect();
    }

    public class TCP
    {
        public TcpClient socket;

        private NetworkStream stream;
        private Packet receivedData;
        private byte[] receivebuffer;

        public void Connect()
        {
            socket = new TcpClient()
            {
                ReceiveBufferSize = dataBufferSize,
                SendBufferSize = dataBufferSize,
            };
            receivebuffer = new byte[dataBufferSize];
            socket.BeginConnect(instance.ip, instance.port, ConnectCallBack, socket);
        }

        private void ConnectCallBack(IAsyncResult _result)
        {
            socket.EndConnect(_result);

            if (!socket.Connected)
            {
                return;
            }

            stream = socket.GetStream();

            receivedData = new Packet();

            stream.BeginRead(receivebuffer, 0, dataBufferSize, ReceiveCallBack, null);
        }

        public void SendToData(Packet packet)
        {
            try
            {
                if (socket != null)
                {
                    stream.BeginWrite(packet.ToArray(), 0, packet.Length(), null, null);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
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
                stream.BeginRead(receivebuffer, 0, dataBufferSize, ReceiveCallBack, null);
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
                        packethandler[packetId](packet);
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

    private void InitializeClientData()
    {
        packethandler = new Dictionary<int, PacketHandler>()
        {
            { (int)ServerPackets.welcome, ClientHandle.Welcome }
        };
        Debug.Log("Initialize packets");
    }
}