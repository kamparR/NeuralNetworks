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
        private Random random = new Random();

        public float ValidationData { get; set; }
        public float ImagesDisturbanceProbability { set; get; }
        public float ImageDisturbanceMaxDifference { set; get; }
        public int MaxEpoch { set; get; }
        public Config Config { set; get; }

        public Simulation(INeuralNetwork neuralNetwork, bool logger = false)
        {
            this.neuralNetwork = neuralNetwork;
            this.logger = logger;
            stopwatch = new Stopwatch();
            Reset();
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

                if (TrainData.SoftMax(output) == data.SoftMax())
                {
                    correct++;
                }
            }

            return (float)correct / trainData.Count;
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

        public float TrainAutoencoder(List<TrainData> trainData)
        {
            List<int> indexList = Enumerable.Range(0, trainData.Count).ToList();
            indexList.Shuffle();
            float error = 0;

            for (int i = 0; i < indexList.Count; i++)
            {
                var inputs = trainData[indexList[i]].Inputs;

                error += neuralNetwork.Train(inputs, inputs);

            }

            return error / trainData.Count;
        }

        public float TrainSOM()
        {
            bool clear = false;
            if (Config.TrainData == null || Config.TrainData.Count == 0)
            {
                clear = true;
                Config.TrainData = new List<TrainData>();

                for (int i = 0; i < 20; i++)
                {
                    var trainData = new TrainData();
                    trainData.Inputs = new List<float>();

                    for (int j = 0; j < 3; j++)
                    {
                        trainData.Inputs.Add((float)random.NextDouble());
                    }

                    Config.TrainData.Add(trainData);
                }
            }

            List<int> indexList = Enumerable.Range(0, Config.TrainData.Count).ToList();
            indexList.Shuffle();

            float error = 0;

            for (int j = 0; j < indexList.Count; j++)
            {
                error += neuralNetwork.Train(Config.TrainData[j].Inputs, new List<float>());
            }

            if (clear)
            {
                Config.TrainData.Clear();
            }
            
            return error / indexList.Count;
        }

        private void Log(string message)
        {
            if (logger)
            {
                Console.WriteLine(message);
            }
        }

        public int GetHiddenNeuronsNumber()
        {
            return Config.HiddenNeurons;
        }

        public List<float> GetFeature(int hiddenNeuronNumber, List<float> inputs)
        {
            return neuralNetwork.GetFeature(0, hiddenNeuronNumber, inputs);
        }
    }
}
