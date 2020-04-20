using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Multiplayer_Server
{
    internal class Program
    {
        private static Thread m_ThreadConsole;

        private static void Main(string[] args)
        {
            m_ThreadConsole = new Thread(new ThreadStart(ConsoleThread));
            m_ThreadConsole.Start();

            NetworkConfig.InitNetwork();
            NetworkConfig.socket.StartListening(500, 5, 1);
        }

        private static void ConsoleThread()
        {
            while (true)
            {
            }
        }
    }
}