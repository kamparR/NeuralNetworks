using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    public class NeuralNetwork : INeuralNetwork
    {
        private INeuron neuron;
        private WeightInitializer weightInitializer;
        private int inputs;
        private int outputs;

        public NeuralNetwork(INeuron baseNeuron, int inputs, int outputs, WeightInitializer weightInitializer)
        {
            this.inputs = inputs;
            this.outputs = outputs;
            this.weightInitializer = weightInitializer;

            InitializeNeurons(baseNeuron);
            ReinitializeWeights();
        }

        public List<float> Compute(List<float> inputs)
        {
            float output = neuron.Compute(inputs);
            return new List<float> { output };
        }

        public float Train(List<float> inputs, List<float> correctOutputs)
        {
            float error = 0;
            error = neuron.Train(inputs, correctOutputs[0]);
            return Math.Abs(error);
        }

        private void InitializeNeurons(INeuron baseNeuron)
        {
            this.neuron = baseNeuron.Copy();
        }

        public void ReinitializeWeights()
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
