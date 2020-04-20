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
        private static void Main(string[] args)
        {
            Server.Start(5, 26950);

            Console.ReadKey();
        }
    }
}