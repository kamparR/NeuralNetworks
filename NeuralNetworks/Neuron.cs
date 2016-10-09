using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    public abstract class Neuron : INeuron
    {
        protected List<float> weights;
        protected float alpha;

        public void SetWeights(List<float> weights)
        {
            this.weights = weights;
        }

        public void SetParameters(float alpha)
        {
            this.alpha = alpha;
        }

        public float Compute(List<float> inputs)
        {
            float sumOfProduct = SumOfProduct(inputs);
            return ActivationFunction(sumOfProduct);
        }

        public virtual float Train(List<float> inputs, float correctOutput)
        {
            Debug.Assert(alpha > 0);

            float error = ComputeError(inputs, correctOutput);

            for (int i = 0; i < weights.Count; i++)
            {
                float input = i < inputs.Count ? inputs[i] : 1;
                weights[i] += alpha*error*input;
            }

            return error;
        }

        protected abstract float ActivationFunction(float value);
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
