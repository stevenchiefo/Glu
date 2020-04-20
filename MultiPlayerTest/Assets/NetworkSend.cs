using System;
using KaymakNetwork;

internal enum ClientPackets
{
    CPing = 1,
}

internal static class NetworkSend
{
    public static void SendPing()
    {
        ByteBuffer buffer = new ByteBuffer(4);
        buffer.WriteInt32((int)ClientPackets.CPing);
        buffer.WriteString("Hello i'm the client thank you for letting me connect to the server!");
        NetworkConfig.m_Socket.SendData(buffer.Data, buffer.Head);
    }
}