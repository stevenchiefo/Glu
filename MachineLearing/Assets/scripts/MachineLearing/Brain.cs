using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Brain
{
    public Perceptron Perceptron;

    public Perceptron CopyPerceptron()
    {
        Perceptron perceptron = new Perceptron(3);
        perceptron.Weights = Perceptron.Weights;
        return perceptron;
    }
}
