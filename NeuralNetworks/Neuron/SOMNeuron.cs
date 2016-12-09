using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetworks.ActivationFunction;

namespace NeuralNetworks
{
    public class SOMNeuron : Neuron
    {
        public SOMNeuron(IActivationFunction activationFunction, float learningAlpha) :
            base(activationFunction, learningAlpha)
        {
            
        }

        public SOMNeuron(SOMNeuron somNeuron) :
            base(somNeuron)
        {
            
        }

        public override INeuron Copy()
        {
            return new SOMNeuron(this);
        }

        public override float Compute(List<float> inputs)
        {
            float distance = 0;

            for (int i = 0; i < Weights.Count; i++)
            {
                float diff = inputs[i] - Weights[i];
                distance += diff*diff;
            }

            return (float)Math.Sqrt(distance);
        }

        public override float Train(List<float> inputs, float influence)
        {
            float errorSum = 0;

            for (int i = 0; i < Weights.Count; i++)
            {
                float error = inputs[i] - Weights[i];
                Weights[i] += learningAlpha*influence*error;

                if (Weights[i] < 0) Weights[i] = 0;
                else if (Weights[i] > 1) Weights[i] = 1;

                errorSum += error;
            }

            return errorSum / inputs.Count;
        }

        protected override float ComputeError(List<float> inputs, float errorDifferent)
        {
            throw new NotImplementedException();
        }

        public override List<float> GetFeature(int inputs)
        {
            throw new NotImplementedException();
        }
    }
}
