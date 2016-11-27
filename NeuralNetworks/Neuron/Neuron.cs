using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetworks.ActivationFunction;

namespace NeuralNetworks
{
    public abstract class Neuron : INeuron
    {
        public List<float> Weights { get; set; }
        protected float learningAlpha;
        protected IActivationFunction activationFunction;
        protected float lastOutput;

        protected Neuron(IActivationFunction activationFunction, float learningAlpha)
        {
            this.activationFunction = activationFunction;
            this.learningAlpha = learningAlpha;

            Debug.Assert(learningAlpha > 0);
        }

        protected Neuron(Neuron neuron)
        {
            this.activationFunction = neuron.activationFunction;
            this.learningAlpha = neuron.learningAlpha;
        }

        public abstract INeuron Copy();

        public float Compute(List<float> inputs)
        {
            Debug.Assert(Weights != null);

            float sumOfProduct = SumOfProduct(inputs);
            lastOutput = activationFunction.Compute(sumOfProduct);
            return lastOutput;
        }

        public virtual float Train(List<float> inputs, float correctOutput)
        {
            Debug.Assert(Weights != null);

            float error = ComputeError(inputs, correctOutput);

            for (int i = 0; i < Weights.Count; i++)
            {
                float input = i < inputs.Count ? inputs[i] : 1;
                Weights[i] += learningAlpha * error * input;
            }

            return error;
        }

        protected abstract float ComputeError(List<float> inputs, float correctOutput);

        protected float SumOfProduct(List<float> inputs)
        {
            float sum = 0;

            for (int i = 0; i < Weights.Count; i++)
            {
                float input = i < inputs.Count ? inputs[i] : 1;
                sum += input * Weights[i];
            }

            return sum;
        }

        public virtual List<float> GetFeature(int inputs)
        {
            throw new NotImplementedException();
        }
    }
}
