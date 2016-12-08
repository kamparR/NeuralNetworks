using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    public class SOMNetwork : INeuralNetwork
    {
        public List<float> Compute(List<float> inputs)
        {
            throw new NotImplementedException();
        }

        public float Train(List<float> inputs, List<float> correctOutputs)
        {
            throw new NotImplementedException();
        }

        public void ReinitializeWeights()
        {
            
        }

        public List<float> GetFeature(int layer, int neuron, List<float> inputs)
        {
            throw new NotImplementedException();
        }
    }
}
