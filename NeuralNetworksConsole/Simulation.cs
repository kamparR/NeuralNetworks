using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NeuralNetworksConsole;

namespace NeuralNetworks
{
    class Simulation
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

        public int TrainByThreshold(List<Config.Data> trainData, float threshold)
        {
            float error = 0;
            int loops = 0;
            stopwatch.Restart();

            do
            {
                error = Train(trainData);
                //Console.WriteLine(error);
                loops++;

                if (stopwatch.ElapsedMilliseconds > millisecondsTimeLimit)
                {
                    stopwatch.Stop();
                    return -1;
                }

            } while (error > threshold);

            return loops;
        }

        public void TrainByLoops(List<Config.Data> trainData, int loops)
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

        private float Train(List<Config.Data> trainData)
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
