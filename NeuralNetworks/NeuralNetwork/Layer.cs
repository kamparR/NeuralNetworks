using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    public class Layer
    {
        private List<INeuron> neurons;
        private int inputs;
        private int outputs;
        private WeightInitializer weightInitializer;

        public Layer(int inputs, int outputs, INeuron baseNeuron, WeightInitializer weightInitializer)
        {
            this.inputs = inputs;
            this.outputs = outputs;
            this.weightInitializer = weightInitializer;

            InitializeNeurons(baseNeuron);
            ReinitializeWeights();
        }

        public List<float> Compute(List<float> inputs)
        {
            var output = new List<float>();

            foreach (var neuron in neurons)
            {
                output.Add(neuron.Compute(inputs));
            }

            return output;
        }

        private void InitializeNeurons(INeuron baseNeuron)
        {
            neurons = new List<INeuron>();

            for (int i = 0; i < outputs; i++)
            {
                neurons.Add(baseNeuron.Copy());
            }
        }

        public void ReinitializeWeights()
        {
            foreach (var neuron in neurons)
            {
                var weights = new List<float>();
                for (int i = 0; i < inputs + 1; i++)
                {
                    weights.Add(weightInitializer.NextWeight());
                }
                neuron.SetWeights(weights);
            }
        }
    }
}
