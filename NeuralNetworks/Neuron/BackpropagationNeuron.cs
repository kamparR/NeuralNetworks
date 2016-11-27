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
        private float momentum;
        private List<float> weightsDelta;

        public BackpropagationNeuron(IActivationFunction activationFunction, float learningAlpha, float momentum = 0f) :
            base(activationFunction, learningAlpha)
        {
            this.momentum = momentum;
        }

        public BackpropagationNeuron(BackpropagationNeuron backpropagationNeuron) : 
            base(backpropagationNeuron)
        {
            this.momentum = backpropagationNeuron.momentum;
        }

        public override INeuron Copy()
        {
            return new BackpropagationNeuron(this);
        }

        public override float Train(List<float> inputs, float errorDifferent)
        {
            if (weightsDelta == null || weightsDelta.Count != Weights.Count)
            {
                InitializeWeightsDelta();
            }

            float error = ComputeError(inputs, errorDifferent);

            for (int i = 0; i < Weights.Count; i++)
            {
                float input = i < inputs.Count ? inputs[i] : 1;
                float delta = learningAlpha*error*input - momentum * weightsDelta[i];
                Weights[i] -= delta;
                weightsDelta[i] = delta;
            }

            return error;
        }

        protected override float ComputeError(List<float> inputs, float errorDifferent)
        {
            float currentOutput = lastOutput;//Compute(inputs);
            return activationFunction.ComputeDerivative(currentOutput) * errorDifferent;
            //return activationFunction.ComputeDerivative(activationFunction.ComputeDerivative(currentOutput)) * errorDifferent;
        }

        private void InitializeWeightsDelta()
        {
            weightsDelta = Enumerable.Repeat(0f, Weights.Count).ToList();
        }

        public override List<float> GetFeature(int inputs)
        {
            var result = Enumerable.Repeat(0f, inputs).ToList();
            float sum = 0;

            for (int i = 0; i < inputs; i++)
            {
                sum += Weights[i]*Weights[i];
            }

            sum = (float)Math.Sqrt(sum);

            for (int i = 0; i < inputs; i++)
            {
                result[i] = Weights[i]/sum;
            }

            return result;
        }
    }
}
