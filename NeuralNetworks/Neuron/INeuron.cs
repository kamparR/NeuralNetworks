using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetworks.ActivationFunction;

namespace NeuralNetworks
{
    public interface INeuron
    {
        List<float> Weights { get; set; }

        float Compute(List<float> inputs);
        float Train(List<float> inputs, float correctOutput);
        INeuron Copy();
        List<float> GetFeature(int inputs);
    }
}
