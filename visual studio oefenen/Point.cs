using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class Point : GameObject
    {
        private Random m_Random = new Random();
        public Vector2 Position;
        public int Label;

        public Point(int Screenwitdh, int Screenheight)
        {
            Position = new Vector2(m_Random.Next(0, Screenwitdh + 1), m_Random.Next(0, Screenheight + 1));
            if (Position.X > Position.Y)
            {
                Label = 1;
            }
            else
            {
                Label = -1;
            }
        }

        public void Painter()
        {
            GAME_ENGINE.DrawEllipse(Position.X, Position.Y, 51, 51);
            if (Label == 1)
            {
                GAME_ENGINE.SetColor(Color.White);
            }
            else
            {
                GAME_ENGINE.SetColor(Color.Black);
            }
            GAME_ENGINE.DrawEllipse(Position.X, Position.Y, 50, 50);
        }
    }
}