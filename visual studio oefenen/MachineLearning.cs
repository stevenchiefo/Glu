using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class MachineLearning : AbstractGame
    {
        private Vector2 m_ScreenSize = new Vector2(400, 400);
        private Perceptron m_Perceptron;
        private Point[] m_Points = new Point[100];
        private float[] m_Input = { -1f, 0.5f };

        //public override void GameInitialize()
        //{
        //    GAME_ENGINE.SetScreenHeight(400);
        //    GAME_ENGINE.SetScreenWidth(400);
        //    GAME_ENGINE.SetTitle("MachineLearning");
        //    GAME_ENGINE.SetBackgroundColor(Color.White);
        //}

        public override void GameStart()
        {
            LoadPerceptron();
            Task();
        }

        public override void GameEnd()
        {
        }

        public override void Update()
        {
        }

        public override void Paint()
        {
            foreach (Point pt in m_Points)
            {
                pt.Painter();
            }
        }

        private void Task()
        {
            Console.WriteLine(m_Perceptron.Geuss(m_Input));
            Console.ReadKey();
        }

        private void LoadPerceptron()
        {
            m_Perceptron = new Perceptron();
        }

        private float normalized(float value, float max) => (1f / max) * value;
    }
}