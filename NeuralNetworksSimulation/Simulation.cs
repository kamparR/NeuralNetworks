using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NeuralNetworks;

namespace NeuralNetworksSimulation
{
    public class Simulation
    {
        private INeuralNetwork neuralNetwork;
        private bool logger;
        private Stopwatch stopwatch;
        private long millisecondsTimeLimit = 2000;

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

        //public float ComputeError(List<Tuple<List<float>, float>> trainData)
        //{
        //    float error = 0;

        //    foreach (var data in trainData)
        //    {
        //        neuralNetwork.ComputeError();
        //    }

        //    return error;
        //}

        private float Train(List<TrainData> trainData)
        {
            List<int> indexList = Enumerable.Range(0, trainData.Count).ToList();
            indexList.Shuffle();
            float error = 0;

            foreach (int index in indexList)
            {
                var inputs = trainData[index].Inputs;
                var outputs = trainData[index].Outputs;

                error += neuralNetwork.Train(inputs, outputs);
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
