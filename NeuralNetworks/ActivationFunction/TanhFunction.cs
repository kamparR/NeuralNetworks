using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.ActivationFunction
{
    public class TanhFunction : IActivationFunction
    {
        public float Compute(float value)
        {
            return (float)Math.Tanh(value);
        }

        public float ComputeDerivative(float value)
        {
            return 1 - (value * value);
        }
    }
}
