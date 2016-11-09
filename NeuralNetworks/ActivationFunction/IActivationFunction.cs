using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks.ActivationFunction
{
    public interface IActivationFunction
    {
        float Compute(float value);
        float ComputeDerivative(float value);
    }
}
