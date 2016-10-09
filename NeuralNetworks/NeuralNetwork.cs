using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    public class NeuralNetwork<T> : INeuralNetwork 
        where T : INeuron, new()
    {
        private INeuron neuron;
        private WeightInitializer weightInitializer;
        private int inputs;
        private int outputs;

        public NeuralNetwork(WeightInitializer weightInitializer, int inputs, int outputs, float alpha)
        {
            this.weightInitializer = weightInitializer;
            this.inputs = inputs;
            this.outputs = outputs;
            neuron = new T();
            InitializeWeights();
            neuron.SetParameters(alpha);
        }

        public List<float> Compute(List<float> inputs)
        {
            float output = neuron.Compute(inputs);
            return new List<float> { output };
        }

        public float Train(List<float> inputs, float correctOutput)
        {
            float error = 0;
            error = Math.Abs(neuron.Train(inputs, correctOutput));
            return error;
        }

        private void InitializeWeights()
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
