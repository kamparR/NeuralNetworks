using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    public class Neuron : INeuron
    {
        private List<float> weights;
        private float alpha;
        private float threshold;

        public Neuron(List<float> weights, float alpha, float threshold)
        {
            this.weights = weights;
            this.alpha = alpha;
            this.threshold = threshold;
        }

        public float Compute(List<float> inputs)
        {
            float sumOfProduct = SumOfProduct(inputs);
            return ActivationFunction(sumOfProduct);
        }

        public void Learn(List<float> inputs, float correctOutput)
        {
            Debug.Assert(inputs.Count == weights.Count);
            float currentOutput = Compute(inputs);
            float error = correctOutput - currentOutput;

            for (int i = 0; i < inputs.Count && i < weights.Count; i++)
            {
                weights[i] += alpha*weights[i]*inputs[i];
            }
        }

        protected virtual float ActivationFunction(float value)
        {
            return value >= threshold ? 1 : 0;
        }

        private float SumOfProduct(List<float> inputs)
        {
            Debug.Assert(inputs.Count == weights.Count);
            float sum = 0;

            for (int i = 0; i < inputs.Count && i < weights.Count; i++)
            {
                sum += inputs[i]*weights[i];
            }

            return sum;
        }


    }
}
