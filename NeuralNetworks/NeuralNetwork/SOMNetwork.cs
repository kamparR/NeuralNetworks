using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    public class SOMNetwork : INeuralNetwork
    {
        private List<INeuron> neurons;
        private int inputs;
        private int outputs;
        private WeightInitializer weightInitializer;
        private int iteration = 0;
        private int mapWidth = 0;
        private int maxEpoch;

        public SOMNetwork(INeuron baseNeuron, int inputs, int outputs, int maxEpoch)
        {
            this.inputs = inputs;
            this.outputs = outputs;
            this.weightInitializer = new WeightInitializer(0, 1);
            this.maxEpoch = maxEpoch;

            InitializeNeurons(baseNeuron);
            ReinitializeWeights();
        }

        public List<float> Compute(List<float> inputs)
        {



            return new List<float>();
        }

        public float Train(List<float> inputs, List<float> o)
        {
            iteration++;
            int bmuIndex = GetBMUNeuronIndex(inputs);
            float distance = neurons[bmuIndex].Compute(inputs);
            float neighbourhoodRadius = GetNeighbourhoodRadius();
            float neighbourhoodRadiusSq = neighbourhoodRadius*neighbourhoodRadius;

            for (int i = 0; i < neurons.Count; i++)
            {
                float distanceToBMUSq = GetNeuronsDistanceSq(i, bmuIndex);

                if (distanceToBMUSq < neighbourhoodRadiusSq)
                {
                    float influence = (float) Math.Exp(-distanceToBMUSq/(2*neighbourhoodRadiusSq));
                    neurons[i].Train(inputs, influence);
                }
            }

            return distance;
        }

        private int GetBMUNeuronIndex(List<float> inputs)
        {
            float minDistance = 0;
            int bmuIndex = 0;

            for (int i = 0; i < neurons.Count; i++)
            {
                float distance = neurons[i].Compute(inputs);

                if (i == 0 || distance < minDistance)
                {
                    bmuIndex = i;
                    minDistance = distance;
                }
            }

            return bmuIndex;
        }

        private float GetNeighbourhoodRadius()
        {
            float mapRadius = mapWidth/2f;

            float timeConstant = maxEpoch/(float)Math.Log(mapRadius);
            float neighbourhoodRadius = mapRadius*(float)Math.Exp(-iteration/timeConstant);

            return neighbourhoodRadius;
        }

        private float GetNeuronsDistanceSq(int neuronAIndex, int neuronBIndex)
        {
            float Ax = neuronAIndex % mapWidth;
            float Ay = neuronAIndex / mapWidth;
            float Bx = neuronBIndex % mapWidth;
            float By = neuronBIndex / mapWidth;

            return (Ax - Bx)*(Ax - Bx) + (Ay - By)*(Ay - By);
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
            iteration = 0;
            mapWidth = (int) Math.Sqrt(neurons.Count);

            foreach (var neuron in neurons)
            {
                var weights = new List<float>();
                for (int i = 0; i < inputs; i++)
                {
                    weights.Add(weightInitializer.NextWeight());
                }
                neuron.Weights = weights;
            }
        }

        public List<float> GetFeature(int layer, int neuron, List<float> inputs)
        {
            var result = Enumerable.Repeat(0f, this.inputs * this.outputs).ToList();
            int index = 0;

            for (int i = 0; i < this.outputs; i++)
            {
                for (int j = 0; j < this.inputs; j++)
                {
                    result[index++] = neurons[i].Weights[j];
                }
            }

            return result;
        }
    }
}
