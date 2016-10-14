using System.Collections.Generic;
using NeuralNetworks.ActivationFunction;

namespace NeuralNetworks
{
    public class Adaline : Neuron
    {
        public Adaline(IActivationFunction activationFunction, float learningAlpha) :
            base(activationFunction, learningAlpha)
        {
        }

        public Adaline(Adaline adaline) : 
            base(adaline)
        {
        }

        public override INeuron Copy()
        {
            return new Adaline(this);
        }

        protected override float ComputeError(List<float> inputs, float correctOutput)
        {
            float sumOfProduct = SumOfProduct(inputs);
            return (correctOutput - sumOfProduct);
        }
    }
}
