using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class ML : AbstractGame
    {
        private Random m_Random;
        private Vector2 m_ScreenSize = new Vector2(400, 400);
        private Perceptron m_Perceptron;
        private Point[] m_Points;
        private float[] m_Input = { -1f, 0.5f };
        private float m_Score = 0;
        private bool m_Stop = false;

        public override void GameInitialize()
        {
            GAME_ENGINE.SetScreenHeight(400);
            GAME_ENGINE.SetScreenWidth(400);
            GAME_ENGINE.SetTitle("MachineLearning");
            GAME_ENGINE.SetBackgroundColor(Color.AppelBlauwZeeGroen);
        }

        public override void GameStart()
        {
            m_Random = new Random();

            LoadPerceptron();
            m_Points = new Point[100];
            for (int i = 0; i < m_Points.Length; i++)
            {
                Vector2 v = new Vector2(m_Random.Next(50, m_ScreenSize.X - 49), m_Random.Next(50, m_ScreenSize.Y - 49));
                m_Points[i] = new Point(v.X, v.Y);
            }
            Task();
        }

        public override void GameEnd()
        {
            //Clean up unmanaged objects here (F.e. bitmaps & fonts)

            //For example:
            //m_Bitmap.Dispose();
            //m_Font.Dispose();
        }

        public override void Update()
        {
            if (GAME_ENGINE.GetMouseButtonDown(0) && m_Stop == false)
            {
                Test();
            }
        }

        public override void Paint()
        {
            foreach (Point pt in m_Points)
            {
                pt.Painter();
            }
            PaintLine();
        }

        private void PaintLine()
        {
            GAME_ENGINE.SetColor(Color.Black);
            GAME_ENGINE.DrawLine(0, 0, m_ScreenSize.X, m_ScreenSize.Y);
        }

        private void Test()
        {
            m_Score = 0;
            for (int i = 0; i < m_Points.Length; i++)
            {
                float[] input = { m_Points[i].Position.X, m_Points[i].Position.Y };
                if (m_Points[i].Label == m_Perceptron.Geuss(input))
                {
                    m_Points[i].Correct = true;
                    m_Score++;
                }
                else
                {
                    m_Points[i].Correct = false;
                }
            }
            if (m_Score >= 100f)
            {
                Console.WriteLine("Perfect");
                m_Stop = true;
                return;
            }
            else
            {
                Console.WriteLine("Reset weights");
                foreach (Point pt in m_Points)
                {
                    float[] input = { pt.Position.X, pt.Position.Y };
                    m_Perceptron.train(input, pt.Label);
                    //LoadPerceptron();
                }
            }
        }

        private void Task()
        {
            foreach (Point pt in m_Points)
            {
                Console.WriteLine(pt.Position.X + " = X " + pt.Position.Y + " = Y");
            }
            Console.WriteLine(m_Points.Count().ToString());
        }

        private void LoadPerceptron()
        {
            m_Perceptron = new Perceptron();
        }

        private float normalized(float value, float max) => (1f / max) * value;
    }
}