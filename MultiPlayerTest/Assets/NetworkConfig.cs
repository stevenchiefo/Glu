using KaymakNetwork.Network;
using System;
using UnityEngine;

internal static class NetworkConfig
{
    internal static Client m_Socket;

    internal static void InitNetwork()
    {
        if (!ReferenceEquals(m_Socket, null)) return;
        m_Socket = new Client(100);
        NetworkReceive.PacketRouter();
    }

    internal static void ConnectToServer()
    {
        try
        {
            m_Socket.Connect("localhost", 500);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }

        if (m_Socket.IsConnected == false)
        {
            Debug.LogError("Did not connect");
        }
    }

    internal static void DisconnectFromServer()
    {
        m_Socket.Dispose();
    }
}