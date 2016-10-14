using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetworks.ActivationFunction;

namespace NeuralNetworks
{
    public class Perceptron : Neuron
    {
        public Perceptron(IActivationFunction activationFunction, float learningAlpha) :
            base(activationFunction, learningAlpha)
        {
        }

        public Perceptron(Perceptron perceptron) : 
            base(perceptron)
        {
        }

        public override INeuron Copy()
        {
            return new Perceptron(this);
        }

        protected override float ComputeError(List<float> inputs, float correctOutput)
        {
            float currentOutput = Compute(inputs);
            return correctOutput - currentOutput;
        }
    }
}
