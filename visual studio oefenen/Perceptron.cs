using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    internal class Perceptron : GameObject
    {
        private Random m_Random = new Random();
        public float[] Weights = new float[2];

        public Perceptron()
        {
            for (int i = 0; i < Weights.Length; i++)
            {
                Weights[i] = m_Random.Next(-1, 2);
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
    }
}