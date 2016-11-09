using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    public class NeuralNetwork : INeuralNetwork
    {
        private List<Layer> layers;

        public NeuralNetwork(INeuron baseNeuron, int inputs, int outputs, WeightInitializer weightInitializer, int hiddenNeurons)
        {
            CreateLayers(baseNeuron, inputs, outputs, weightInitializer, hiddenNeurons);
        }

        public List<float> Compute(List<float> inputs)
        {
            List<float> output = inputs;

            foreach (var layer in layers)
            {
                output = layer.Compute(output);
            }

            return output;
        }

        public float Train(List<float> inputs, List<float> correctOutputs)//TODO
        {
            float error = 0;
            //error = neuron.Train(inputs, correctOutputs[0]);
            return Math.Abs(error);
        }

        private void CreateLayers(INeuron baseNeuron, int inputs, int outputs, WeightInitializer weightInitializer, int hiddenNeurons)//TODO multi hidden layer support
        {
            layers = new List<Layer>();

            if (hiddenNeurons > 0)
            {
                layers.Add(new Layer(inputs, hiddenNeurons, baseNeuron, weightInitializer));
                layers.Add(new Layer(hiddenNeurons, outputs, baseNeuron, weightInitializer));
            }
            else
            {
                layers.Add(new Layer(inputs, outputs, baseNeuron, weightInitializer));
            }
        }

        public void ReinitializeWeights()
        {
            foreach (var layer in layers)
            {
                layer.ReinitializeWeights();
            }
        }
    }
}
