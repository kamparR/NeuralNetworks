using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.ActivationFunction
{
    public class BipolarBinaryFunction : IActivationFunction
    {
        public float Compute(float value)
        {
            return value >= 0 ? 1 : -1;
        }
    }
}
