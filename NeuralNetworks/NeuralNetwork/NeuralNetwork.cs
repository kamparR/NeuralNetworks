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

        public float Train(List<float> inputs, List<float> correctOutputs)
        {
            List<List<float>> layerInputs = new List<List<float>>();
            float error = 0;

            //Compute and remember inputs
            layerInputs.Add(inputs);
            for (var i = 0; i < layers.Count; i++)
            {
                layerInputs.Add(layers[i].Compute(layerInputs[i]));
            }

            //Error on last output
            int lastOutputIndex = layerInputs.Count - 1;
            int outputsNumber = correctOutputs.Count;
            List<float> layerError = Enumerable.Repeat(0f, outputsNumber).ToList();

            for (int i = 0; i < outputsNumber; i++)
            {
                layerError[i] = layerInputs[lastOutputIndex][i] - correctOutputs[i];
                error += Math.Abs(layerError[i]);
            }

            //Train
            for (int i = layers.Count - 1; i >= 0; i--)
            {
                layerError = layers[i].Train(layerInputs[i], layerError);
            }

            return error / correctOutputs.Count;
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

        public List<float> GetFeature(int layer, int neuron)
        {
            return layers[layer].GetFeature(neuron);
        }
    }
}
