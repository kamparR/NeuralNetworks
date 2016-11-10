using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetworks.ActivationFunction;

namespace NeuralNetworks
{
    public class BackpropagationNeuron : Neuron
    {
        public BackpropagationNeuron(IActivationFunction activationFunction, float learningAlpha) :
            base(activationFunction, learningAlpha)
        {
        }

        public BackpropagationNeuron(BackpropagationNeuron backpropagationNeuron) : 
            base(backpropagationNeuron)
        {
        }

        public override INeuron Copy()
        {
            return new BackpropagationNeuron(this);
        }

        public override float Train(List<float> inputs, float errorDifferent)
        {
            float error = ComputeError(inputs, errorDifferent);

            for (int i = 0; i < Weights.Count; i++)
            {
                float input = i < inputs.Count ? inputs[i] : 1;
                Weights[i] -= learningAlpha * error * input;
            }

            return error;
        }

        protected override float ComputeError(List<float> inputs, float errorDifferent)
        {
            float currentOutput = lastOutput;//Compute(inputs);
            return activationFunction.ComputeDerivative(currentOutput) * errorDifferent;
            //return activationFunction.ComputeDerivative(activationFunction.ComputeDerivative(currentOutput)) * errorDifferent;
        }
    }
}
