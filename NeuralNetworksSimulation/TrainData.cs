using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworksSimulation
{
    public class TrainData
    {
        public List<float> Inputs { get; set; }
        public List<float> Outputs { get; set; }

        public int SoftMax()
        {
            return SoftMax(Outputs);
        }

        public static int SoftMax(List<float> values)
        {
            int result = -1;

            for (int i = 0; i < values.Count; i++)
            {
                if (result < 0 || values[i] > values[result])
                {
                    result = i;
                }
            }

            return result;
        }
    }
}
