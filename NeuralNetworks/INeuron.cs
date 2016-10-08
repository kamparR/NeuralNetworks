using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    public interface INeuron
    {
        float Compute(List<float> inputs);
        void Learn(List<float> inputs, float correctOutput);
    }
}
