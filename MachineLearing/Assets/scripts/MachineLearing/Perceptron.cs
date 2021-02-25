
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class Perceptron
{
    
    public float[] Weights;

    public Perceptron(int n)
    {
        Weights = new float[n];
        for (int i = 0; i < Weights.Length; i++)
        {
            int weight = Random.Range(0, 101);
            if (weight > 50)
            {
                Weights[i] = normalized((float)Random.Range(0f, 1f),1f);
            }
            else
            {
                Weights[i] = -normalized((float)Random.Range(0f, 1f),1f);
            }
        }
    }

    public float Geuss(float[] input)
    {
        float sum = 0;
        for (int i = 0; i < Weights.Length; i++)
        {
            sum += input[i] * Weights[i];
        }
        float outpust = Sign(sum);
        return outpust;
    }

    public float Sign(float n)
    {
        return n * 100f;
    }



    private float normalized(float value, float max) => (-1f / max) * value;
}
