using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.ActivationFunction
{
    public class UnipolarBinaryFunction : IActivationFunction
    {
        public float Compute(float value)
        {
            return value >= 0 ? 1 : 0;
        }

        public float ComputeDerivative(float value)
        {
            return 0;
        }
    }
}
