using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    public class SOMNeuron : INeuron
    {
        public List<float> Weights { get; set; }

        public float Compute(List<float> inputs)
        {
            throw new NotImplementedException();
        }

        public float Train(List<float> inputs, float correctOutput)
        {
            throw new NotImplementedException();
        }

        public INeuron Copy()
        {
            throw new NotImplementedException();
        }

        public List<float> GetFeature(int inputs)
        {
            throw new NotImplementedException();
        }
    }
}
