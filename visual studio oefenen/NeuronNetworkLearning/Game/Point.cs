using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class Point : GameObject
    {
        private int m_Scale = 10;
        private Random m_Random = new Random();
        public Vector2 Position;
        public int Label;
        public bool Correct = false;

        public Point(int X, int Y)
        {
            Position = new Vector2(X, Y);

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
            GAME_ENGINE.SetColor(Color.Black);
            GAME_ENGINE.DrawEllipse(Position.X, Position.Y, m_Scale + 1, m_Scale + 1);
            if (Label == 1)
            {
                GAME_ENGINE.SetColor(Color.White);
            }
            else
            {
                GAME_ENGINE.SetColor(Color.Black);
            }
            GAME_ENGINE.FillEllipse(Position.X, Position.Y, m_Scale, m_Scale);

            if (Correct == true)
            {
                GAME_ENGINE.SetColor(Color.Green);
                GAME_ENGINE.FillEllipse(Position.X, Position.Y, m_Scale / 2, m_Scale / 2);
            }
            else
            {
                GAME_ENGINE.SetColor(Color.Red);
                GAME_ENGINE.FillEllipse(Position.X, Position.Y, m_Scale / 2, m_Scale / 2);
            }
        }
    }
}