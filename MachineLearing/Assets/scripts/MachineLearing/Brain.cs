using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Brain
{
    public Perceptron[] Perceptrons;

    public void CreatePerceptrons(int amount, int weights)
    {
        Perceptrons = new Perceptron[amount];
        for (int i = 0; i < Perceptrons.Length; i++)
        {
            Perceptrons[i] = new Perceptron(weights);
        }
    }

    public Perceptron[] Copy()
    {
        Perceptron[] newbrain = new Perceptron[Perceptrons.Length];
        for (int i = 0; i < newbrain.Length; i++)
        {
            newbrain[i] = new Perceptron(Perceptrons[0].Weights.Length);
        }
        for (int i = 0; i < Perceptrons.Length; i++)
        {
            newbrain[i].Weights = Perceptrons[i].Weights;
        }
        return newbrain;
    }

    public void Train(float TrainRate)
    {
        foreach (Perceptron perceptron in Perceptrons)
        {
            perceptron.Adjust(TrainRate);
        }
    }
}