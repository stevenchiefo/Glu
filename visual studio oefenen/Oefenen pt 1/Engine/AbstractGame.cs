using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    //Hides the basic setup from the oefenen_pt_1 class.
    public class AbstractGame : GameObject
    {
        public override void GameInitialize()
        {
            // Set the required values
            GAME_ENGINE.SetTitle("Canvas v1.1.1");
            GAME_ENGINE.SetIcon("icon.ico");

            // Set the optional values
            GAME_ENGINE.SetScreenWidth(1960);
            GAME_ENGINE.SetScreenHeight(1060);
            GAME_ENGINE.SetBackgroundColor(0, 167, 141); //Appelblauwzeegroen
            //GAME_ENGINE.SetBackgroundColor(49, 77, 121); //The Unity background color
        }
    }
}