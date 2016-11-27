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

        public List<float> Train(List<float> inputs, List<float> error)
        {
            List<float> currentError = Enumerable.Repeat(0f, this.inputs).ToList();

            for (var i = 0; i < this.outputs; i++)
            {
                List<float> weightsCopy = new List<float>(neurons[i].Weights);
                float neuronError = neurons[i].Train(inputs, error[i]);

                for (int j = 0; j < this.inputs; j++)
                {
                    currentError[j] += weightsCopy[j] * neuronError;
                }
            }

            return currentError;
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
                neuron.Weights = weights;
            }
        }

        public List<float> GetFeature(int neuron)
        {
            return neurons[neuron].GetFeature(this.inputs);
        }
    }
}
