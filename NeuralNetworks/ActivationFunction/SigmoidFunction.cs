using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.ActivationFunction
{
    public class SigmoidFunction : IActivationFunction
    {
        public float Compute(float value)
        {
            return 1 / (1 + (float)Math.Exp(-value));
        }

        public float ComputeDerivative(float value)
        {
            float s = Compute(value);
            return s * (1 - s);
        }
    }
}
