using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace GameEngine
{
    internal class Perceptron : GameObject
    {
        private Random m_Random = new Random();
        public float[] Weights = new float[2];
        private float m_LearningRate = 0.1f;

        public Perceptron()
        {
            for (int i = 0; i < Weights.Length; i++)
            {
                int weight = m_Random.Next(0, 101);
                if (weight > 50)
                {
                    Weights[i] = normalized((float)-m_Random.NextDouble(), 1);
                }
                else
                {
                    Weights[i] = normalized((float)m_Random.NextDouble(), 1);
                }
            }
        }

        public int Geuss(float[] input)
        {
            float sum = 0;
            for (int i = 0; i < Weights.Length; i++)
            {
                sum += input[i] * Weights[i];
            }
            int outpust = Sign(sum);
            return outpust;
        }

        public int Sign(float n)
        {
            if (n >= 0)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }

        public void train(float[] input, int traget)
        {
            int geuss = Geuss(input);
            int error = geuss - traget;
            for (int i = 0; i < Weights.Length; i++)
            {
                Weights[i] -= error * input[i] * m_LearningRate;
            }
        }

        private float normalized(float value, float max) => (-1f / max) * value;
    }
}