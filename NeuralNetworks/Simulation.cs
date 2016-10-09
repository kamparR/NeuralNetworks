using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    public class Simulation
    {
        private INeuralNetwork neuralNetwork;

        public Simulation(INeuralNetwork neuralNetwork)
        {
            this.neuralNetwork = neuralNetwork;
        }

        public void TrainByThreshold(List<Tuple<List<float>, float>> trainData, float threshold)
        {
            float error = 0;

            do
            {
                error = Train(trainData);
            } while (error > threshold);
        }

        public void TrainByLoops(List<Tuple<List<float>, float>> trainData, int loops)
        {
            while (loops > 0)
            {
                Train(trainData);
                loops--;
            }
        }

        public List<float> Compute(List<float> inputs)
        {
            return neuralNetwork.Compute(inputs);
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

        private float Train(List<Tuple<List<float>, float>> trainData)
        {
            List<int> indexList = Enumerable.Range(0, trainData.Count).ToList();
            indexList.Shuffle();
            float error = 0;

            foreach (int index in indexList)
            {
                var inputs = trainData[index].Item1;
                var outputs = trainData[index].Item2;

                error += neuralNetwork.Train(inputs, outputs);
            }

            return error;
        }
    }
}
