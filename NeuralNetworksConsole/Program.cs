using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetworks;

namespace NeuralNetworksConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            PerceptronOr();

            Console.ReadKey();
        }

        static void PerceptronOr()
        {
            var testData = new List<Tuple<List<float>, float>>
            {
                Tuple.Create(new List<float> {0, 0}, 0f),
                Tuple.Create(new List<float> {1, 0}, 1f),
                Tuple.Create(new List<float> {0, 1}, 1f),
                Tuple.Create(new List<float> {1, 1}, 1f)
            };

            var weightInitializer = new WeightInitializer(-1.0f, 1.0f);
            var neuralNetwork = new NeuralNetwork<Perceptron>(weightInitializer, 2, 1, 0.5f);
            var simulation = new Simulation(neuralNetwork);
            simulation.TrainByLoops(testData, 10);

        }
    }
}
