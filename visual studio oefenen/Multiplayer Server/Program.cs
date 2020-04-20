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
        private static bool isRunning = false;

        private static void Main(string[] args)
        {
            Console.Title = "Game Server";
            isRunning = true;

            Thread mainThread = new Thread(new ThreadStart(MainThread));
            mainThread.Start();
            Server.Start(5, 26950);
        }

        private static void MainThread()
        {
            Console.WriteLine($"Main thread started. running at {Constants.TICKS_PER_SEC}");
            DateTime nextloop = DateTime.Now;

            while (isRunning)
            {
                while (nextloop < DateTime.Now)
                {
                    GameLogic.Update();

                    nextloop = nextloop.AddMilliseconds(Constants.MS_PER_TICK);

                    if (nextloop > DateTime.Now)
                    {
                        Thread.Sleep(nextloop - DateTime.Now);
                    }
                }
            }
        }
    }
}