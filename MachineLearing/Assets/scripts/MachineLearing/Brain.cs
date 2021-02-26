using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Brain
{
    public Perceptron FrontPerceptron;
    public Perceptron LeftPerceptron;
    public Perceptron RightPerceptron;

    public Perceptron CopyFrontPerceptron()
    {
        Perceptron perceptron = new Perceptron(3);
        perceptron.Weights = FrontPerceptron.Weights;
        return perceptron;
    }

    public Perceptron CopyLeftPerceptron()
    {
        Perceptron perceptron = new Perceptron(3);
        perceptron.Weights = LeftPerceptron.Weights;
        return perceptron;
    }

    public Perceptron CopyRightPerceptron()
    {
        Perceptron perceptron = new Perceptron(3);
        perceptron.Weights = RightPerceptron.Weights;
        return perceptron;
    }

    public void Train(float TrainRate)
    {
        FrontPerceptron.Adjust(TrainRate);
        LeftPerceptron.Adjust(TrainRate);
        RightPerceptron.Adjust(TrainRate);
    }
}