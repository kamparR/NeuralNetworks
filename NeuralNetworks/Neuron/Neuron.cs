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
        protected List<float> weights;
        protected float learningAlpha;
        protected IActivationFunction activationFunction;

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

        public void SetWeights(List<float> weights)
        {
            this.weights = weights;
        }

        public float Compute(List<float> inputs)
        {
            Debug.Assert(weights != null);

            float sumOfProduct = SumOfProduct(inputs);
            return activationFunction.Compute(sumOfProduct);
        }

        public virtual float Train(List<float> inputs, float correctOutput)
        {
            Debug.Assert(weights != null);

            float error = ComputeError(inputs, correctOutput);

            for (int i = 0; i < weights.Count; i++)
            {
                float input = i < inputs.Count ? inputs[i] : 1;
                weights[i] += learningAlpha*error*input;
            }

            return error;
        }

        protected abstract float ComputeError(List<float> inputs, float correctOutput);

        protected float SumOfProduct(List<float> inputs)
        {
            float sum = 0;

            for (int i = 0; i < weights.Count; i++)
            {
                float input = i < inputs.Count ? inputs[i] : 1;
                sum += input * weights[i];
            }

            return sum;
        }
    }
}
