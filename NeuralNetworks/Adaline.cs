﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    public class Adaline : Neuron
    {
        protected override float ActivationFunction(float value)
        {
            return value > 0 ? 1 : 0;
        }

        protected override float ComputeError(List<float> inputs, float correctOutput)
        {
            float sumOfProduct = SumOfProduct(inputs);
            return (correctOutput - sumOfProduct);// * (correctOutput - sumOfProduct);
        }
    }
}
