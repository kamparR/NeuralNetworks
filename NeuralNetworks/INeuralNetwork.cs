using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    public interface INeuralNetwork
    {
        List<float> Compute(List<float> inputs);
        float Train(List<float> inputs, float correctOutput);
    }
}
