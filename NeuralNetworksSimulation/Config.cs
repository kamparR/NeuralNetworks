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
        public int HiddenNeurons = 0;
        public float ValidationData = 0;
        public float Momentum = 0;
        public string ImagesPath = @"D:\Studia\Semestr VII\Sieci neuronowe\NeuralNetworks\PngData\";
        public float ImagesDisturbanceProbability = 0;
        public float ImageDisturbanceMaxDifference = 0;

        public static string GetCsvHeaders()
        {
            var headers = new[] { "Neuron", "ActivationFunction", "MinWeights", "MaxWeights", "Alpha", "Inputs", "Outputs", "TrainThreshold", "Repeat", "HiddenNeurons" };
            return string.Join(";", headers);
        }

        public string GetCsvData()
        {
            var data = new string[] { Neuron, ActivationFunction, Weights.Min.ToString(), Weights.Max.ToString(), Alpha.ToString(), Inputs.ToString(), Outputs.ToString(), TrainThreshold.ToString(), Repeat.ToString(), HiddenNeurons.ToString() };
            return string.Join(";", data);
        }

        public Simulation CreateSimulation()
        {
            var weightInitializer = new WeightInitializer(Weights.Min, Weights.Max);
            var activationFunction = CreateActivationFunction();
            var neuron = CreateNeuron(activationFunction);
            var neuralNetwork = new NeuralNetwork(neuron, Inputs, Outputs, weightInitializer, HiddenNeurons);
            var simulation = new Simulation(neuralNetwork, true, ValidationData);
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
            else if (equals("SigmoidFunction"))
            {
                activationFunction = new SigmoidFunction();
            }
            else if (equals("TanhFunction"))
            {
                activationFunction = new TanhFunction();
            }
            else
            {
                Console.WriteLine($"Wrong activation function: {ActivationFunction}");
            }

            return activationFunction;
        }

        public INeuron CreateNeuron(IActivationFunction activationFunction)
        {
            INeuron neuron = null;
            Func<string, bool> equals = value => string.Equals(Neuron, value, StringComparison.InvariantCultureIgnoreCase);

            if (equals("Perceptron"))
            {
                neuron = new Perceptron(activationFunction, Alpha);
            }
            else if (equals("Adaline"))
            {
                neuron = new Adaline(activationFunction, Alpha);
            }
            else if (equals("BackpropagationNeuron"))
            {
                neuron = new BackpropagationNeuron(activationFunction, Alpha, Momentum);
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
