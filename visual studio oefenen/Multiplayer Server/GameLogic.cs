using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multiplayer_Server
{
    internal class GameLogic
    {
        public static void Update()
        {
            ThreadManager.UpdateMain();
        }
    }
}