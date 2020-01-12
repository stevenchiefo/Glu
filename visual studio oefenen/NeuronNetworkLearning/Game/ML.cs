using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace GameEngine
{
    public class ML : AbstractGame
    {
        private int m_MaxScore = 0;
        private Dictionary<string, int> m_TestOutCome = new Dictionary<string, int>();
        private string m_FilePath = "Saved.json";
        private Saved m_Saved;
        private Random m_Random;
        private Vector2 m_ScreenSize = new Vector2(400, 400);
        private Perceptron m_Perceptron;
        private Point[] m_Points;
        private float[] m_Input = { -1f, 0.5f };
        private float m_Score = 0;
        private bool m_Stop = false;
        private bool m_AutoTest = false;

        public override void GameInitialize()
        {
            GAME_ENGINE.SetScreenHeight(400);
            GAME_ENGINE.SetScreenWidth(400);
            GAME_ENGINE.SetTitle("MachineLearning");
            GAME_ENGINE.SetBackgroundColor(Color.AppelBlauwZeeGroen);
        }

        public override void GameStart()
        {
            m_Saved = new Saved();
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
        }

        public override void Update()
        {
            if (GAME_ENGINE.GetMouseButtonDown(0))
            {
                DoTest();
            }
            if (GAME_ENGINE.GetKeyDown(Key.Q))
            {
                Test();
            }
            if (GAME_ENGINE.GetMouseButton(1))
            {
                m_AutoTest = true;
            }
            if (m_AutoTest == true && m_Stop == false)
            {
                DoTest();
                Test();
            }
            if (GAME_ENGINE.GetKeyDown(Key.Space))
            {
                LoadSavedPerceptron();
            }
            if (GAME_ENGINE.GetKeyDown(Key.E))
            {
                NewPoints(m_Random.Next(100, 1000));
            }
            if (GAME_ENGINE.GetKeyDown(Key.Z))
            {
                bool valid = false;
                int TestCount = 0;
                while (valid == false)
                {
                    string anwer = Console.ReadLine();
                    TestCount = Convert.ToInt32(anwer);
                    if (TestCount <= 10000)
                    {
                        valid = true;
                    }
                }

                m_MaxScore = 0;
                Console.Clear();
                m_TestOutCome.Clear();
                for (int i = 0; i < TestCount; i++)
                {
                    DoTest();
                    Test();
                    int score = (int)Math.Round(m_Score);
                    m_TestOutCome.Add(i.ToString(), (score));
                    NewPoints(m_Random.Next(100, 1000));
                }
                Console.WriteLine(m_MaxScore + " / " + GetAverage().ToString());
                int didnotget = m_MaxScore - GetAverage();
                Console.WriteLine("Did not get = " + didnotget);
                Console.WriteLine("That is " + GetPercentage(m_MaxScore, didnotget) + "%");
            }
            if (GAME_ENGINE.GetKeyDown(Key.X))
            {
                SavePerceptron();
            }
        }

        private float GetPercentage(int Max, int Num)
        {
            float max = Max;
            float num = Num;

            return (num / max) * 100f;
        }

        private int GetAverage()
        {
            int average = 0;
            foreach (KeyValuePair<string, int> item in m_TestOutCome)
            {
                average += item.Value;
            }

            return average;
        }

        private void NewPoints(int amount)
        {
            m_Stop = false;
            m_Points = new Point[amount];

            for (int i = 0; i < m_Points.Length; i++)
            {
                Vector2 v = new Vector2(m_Random.Next(50, m_ScreenSize.X - 49), m_Random.Next(50, m_ScreenSize.Y - 49));
                m_Points[i] = new Point(v.X, v.Y);
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

        private void DoTest()
        {
            m_Score = 0;
            for (int i = 0; i < m_Points.Length; i++)
            {
                float[] input = { m_Points[i].Position.X, m_Points[i].Position.Y, m_Points[i].Bias };
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
            m_MaxScore += m_Points.Length;
        }

        private void Test()
        {
            if (m_Score >= m_Points.Length)
            {
                Console.WriteLine(m_Points.Length + " / " + m_Score);
                m_Stop = true;
                return;
            }
            else
            {
                Console.WriteLine("Reset weights");
                foreach (Point pt in m_Points)
                {
                    float[] input = { pt.Position.X, pt.Position.Y, pt.Bias };
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
            m_Perceptron = new Perceptron(3);
        }

        private void SavePerceptron()
        {
            m_Saved.Perceptron = m_Perceptron;

            using (StreamWriter file = File.CreateText(m_FilePath))
            {
                string content = JsonConvert.SerializeObject(m_Saved);
                file.Write(content);
                Console.WriteLine("Perceptron saved");
            }
        }

        private void LoadSavedPerceptron()
        {
            if (File.Exists(m_FilePath))
            {
                StreamReader stream = new StreamReader(m_FilePath);
                string content = stream.ReadToEnd();
                m_Saved = JsonConvert.DeserializeObject<Saved>(content);
                m_Perceptron = m_Saved.Perceptron;
                Console.WriteLine("Perception found");
            }
            else
            {
                Console.WriteLine("No file found");
            }
        }

        private float normalized(float value, float max) => (1f / max) * value;
    }
}