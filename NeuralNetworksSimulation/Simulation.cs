using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NeuralNetworks;
using Utils;

namespace NeuralNetworksSimulation
{
    public class Simulation
    {
        private INeuralNetwork neuralNetwork;
        private bool logger;
        private Stopwatch stopwatch;
        private long millisecondsTimeLimit = 2000;

        public float ValidationData { get; set; }
        public float ImagesDisturbanceProbability { set; get; }
        public float ImageDisturbanceMaxDifference { set; get; }
        public int MaxEpoch { set; get; }

        public Simulation(INeuralNetwork neuralNetwork, bool logger = false)
        {
            this.neuralNetwork = neuralNetwork;
            this.logger = logger;
            stopwatch = new Stopwatch();
        }

        public List<float> TrainByThreshold(List<TrainData> trainData, float threshold)
        {
            var errors = new List<float>();
            float error = 0;
            stopwatch.Restart();

            do
            {
                error = Train(trainData) / trainData.Count;
                errors.Add(error);

                if (stopwatch.ElapsedMilliseconds > millisecondsTimeLimit)
                {
                    stopwatch.Stop();
                    return null;
                }

            } while (error > threshold);

            return errors;
        }

        public void TrainByLoops(List<TrainData> trainData, int loops)
        {
            for(int i = 0; i < loops; i++)
            {
                float error = Train(trainData);
            }
        }

        public List<float> Compute(List<float> inputs)
        {
            return neuralNetwork.Compute(inputs);
        }

        public void Reset()
        {
            neuralNetwork.ReinitializeWeights();
        }

        public float TestSoftMax(List<TrainData> trainData)
        {
            int correct = 0;

            foreach (var data in trainData)
            {
                var output = neuralNetwork.Compute(data.Inputs);

                if (SoftMax(output) == SoftMax(data.Outputs))
                {
                    correct++;
                }
            }

            return (float)correct / trainData.Count;
        }

        private int SoftMax(List<float> values)
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

        public float Train(List<TrainData> trainData)
        {
            List<int> indexList = Enumerable.Range(0, trainData.Count).ToList();
            indexList.Shuffle();
            float error = 0;

            for (int i = 0; i < indexList.Count; i++)
            {
                var inputs = trainData[indexList[i]].Inputs;
                var outputs = trainData[indexList[i]].Outputs;
                
                error += neuralNetwork.Train(inputs, outputs);
                
            }

            return error / trainData.Count;
        }


        private void Log(string message)
        {
            if (logger)
            {
                Console.WriteLine(message);
            }
        }
    }
}
