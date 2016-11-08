using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetworks;
using NeuralNetworks.ActivationFunction;

namespace NeuralNetworksSimulation
{
    public class Config
    {
        public string Neuron;
        public string ActivationFunction;
        public Range Weights;
        public List<Range> WeightsRepeat;
        public float Alpha;
        public List<float> AlphaRepeat;
        public int Inputs;
        public int Outputs;
        public List<TrainData> TrainData;
        public float TrainThreshold;
        public List<TrainData> TestData;
        public int Repeat = 1;

        public static string GetCsvHeaders()
        {
            var headers = new[] { "Neuron", "ActivationFunction", "MinWeights", "MaxWeights", "Alpha", "Inputs", "Outputs", "TrainThreshold", "Repeat" };
            return string.Join(";", headers);
        }

        public string GetCsvData()
        {
            var data = new string[] { Neuron, ActivationFunction, Weights.Min.ToString(), Weights.Max.ToString(), Alpha.ToString(), Inputs.ToString(), Outputs.ToString(), TrainThreshold.ToString(), Repeat.ToString() };
            return string.Join(";", data);
        }

        public Simulation CreateSimulation()
        {
            var weightInitializer = new WeightInitializer(Weights.Min, Weights.Max);
            var activationFunction = CreateActivationFunction();
            var neuron = CreateNeuron(activationFunction, Alpha);
            var neuralNetwork = new NeuralNetwork(neuron, Inputs, Outputs, weightInitializer);
            var simulation = new Simulation(neuralNetwork, true);
            return simulation;
        }

        public IActivationFunction CreateActivationFunction()
        {
            IActivationFunction activationFunction = null;

            Func<string, bool> equals = value => string.Equals(ActivationFunction, value, StringComparison.InvariantCultureIgnoreCase);

            if (equals("UnipolarBinaryFunction"))
            {
                activationFunction = new UnipolarBinaryFunction();
            }
            else if (equals("BipolarBinaryFunction"))
            {
                activationFunction = new BipolarBinaryFunction();
            }
            else
            {
                Console.WriteLine($"Wrong activation function: {ActivationFunction}");
            }

            return activationFunction;
        }

        public INeuron CreateNeuron(IActivationFunction activationFunction, float alpha)
        {
            INeuron neuron = null;
            Func<string, bool> equals = value => string.Equals(Neuron, value, StringComparison.InvariantCultureIgnoreCase);

            if (equals("Perceptron"))
            {
                neuron = new Perceptron(activationFunction, alpha);
            }
            else if (equals("Adaline"))
            {
                neuron = new Adaline(activationFunction, alpha);
            }
            else
            {
                Console.WriteLine($"Wrong neuron: {Neuron}");
            }

            return neuron;
        }

        public bool CanBeReduced()
        {
            return (WeightsRepeat != null && WeightsRepeat.Count > 0) || (AlphaRepeat != null && AlphaRepeat.Count > 0);
        }

        public List<Config> Reduce()
        {
            List<Config> reducedConfigs = new List<Config>();

            if (CanBeReduced())
            {
                if (WeightsRepeat == null)
                {
                    WeightsRepeat = new List<Range>();
                }

                if (AlphaRepeat == null)
                {
                    AlphaRepeat = new List<float>();
                }

                if (Weights != null)
                {
                    WeightsRepeat.Add(Weights);
                }

                if (Math.Abs(Alpha) > float.Epsilon)
                {
                    AlphaRepeat.Add(Alpha);
                }

                foreach (var weight in WeightsRepeat)
                {
                    foreach (var alpha in AlphaRepeat)
                    {
                        Config clone = (Config)this.MemberwiseClone();
                        clone.Weights = weight;
                        clone.Alpha = alpha;
                        reducedConfigs.Add(clone);
                    }
                }
            }
            else
            {
                reducedConfigs.Add(this);
            }

            return reducedConfigs;
        }

        public class Range
        {
            public float Min;
            public float Max;
        }
    }
}
