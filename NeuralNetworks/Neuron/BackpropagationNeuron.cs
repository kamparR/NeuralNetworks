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

        public override float Train(List<float> inputs, float correctOutput)
        {
            float error = ComputeError(inputs, correctOutput);

            for (int i = 0; i < weights.Count; i++)
            {
                float input = i < inputs.Count ? inputs[i] : 1;
                weights[i] += learningAlpha * error * input;
            }

            return error;
        }

        protected override float ComputeError(List<float> inputs, float correctOutput)
        {
            float currentOutput = Compute(inputs);
            return correctOutput - currentOutput;
        }
    }
}
