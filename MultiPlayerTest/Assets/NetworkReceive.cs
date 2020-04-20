using System;
using KaymakNetwork;
using UnityEngine;

internal enum ServerPackets
{
    WelcomeMsg = 1,
}

internal static class NetworkReceive
{
    internal static void PacketRouter()
    {
        NetworkConfig.m_Socket.PacketId[(int)ServerPackets.WelcomeMsg] = new KaymakNetwork.Network.Client.DataArgs(Packet_WelcomeMsg);
    }

    private static void Packet_WelcomeMsg(ref byte[] data)
    {
        ByteBuffer buffer = new ByteBuffer(data);
        string msg = buffer.ReadString();
        Debug.Log(msg);

        buffer.Dispose();

        NetworkSend.SendPing();
    }
}