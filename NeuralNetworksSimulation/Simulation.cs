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

        public Simulation(INeuralNetwork neuralNetwork, bool logger = false, float validationData = 0f)
        {
            this.neuralNetwork = neuralNetwork;
            this.logger = logger;
            stopwatch = new Stopwatch();
            this.ValidationData = validationData;
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

            int validationCount = ((int)(trainData.Count*ValidationData)).Clamp(0, trainData.Count - 1);
            int testCount = trainData.Count - validationCount;

            for (int i = 0; i < indexList.Count; i++)
            {
                var inputs = trainData[indexList[i]].Inputs;
                var outputs = trainData[indexList[i]].Outputs;

                if (validationCount == 0)
                {
                    error += neuralNetwork.Train(inputs, outputs) / trainData.Count;
                }
                else
                {
                    if (indexList[i] < testCount)
                    {
                        neuralNetwork.Train(inputs, outputs);
                    }
                    else
                    {
                        var output = neuralNetwork.Compute(inputs);

                        if (SoftMax(output) != SoftMax(outputs))
                        {
                            error += 1f / validationCount;
                        }
                    }
                }
                
            }

            return error;
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
