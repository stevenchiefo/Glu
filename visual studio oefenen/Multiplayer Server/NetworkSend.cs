using System;
using KaymakNetwork;

namespace Multiplayer_Server
{
    internal static class NetworkSend
    {
        public static void WelcomeMsg(int connectionID, string msg)
        {
            ByteBuffer buffer = new ByteBuffer(4);
            buffer.WriteString(msg);
            NetworkConfig.socket.SendDataTo(connectionID, buffer.Data, buffer.Head);

            buffer.Dispose();
        }
    }
}