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
        public List<int> HiddenNeuronsRepeat;
        public float ValidationData = 0;
        public List<float> ValidationDataRepeat;
        public float Momentum = 0;
        public List<float> MomentumRepeat;
        public float Regularization = 0;
        public List<float> RegularizationRepeat;
        public string ImagesPath = @"D:\Studia\Semestr VII\Sieci neuronowe\NeuralNetworks\PngData\";
        public float ImagesDisturbanceProbability = 0;
        public List<float> ImagesDisturbanceProbabilityRepeat;
        public float ImageDisturbanceMaxDifference = 0;
        public List<float> ImageDisturbanceMaxDifferenceRepeat;
        public int MaxEpoch = 100;

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
            var simulation = new Simulation(neuralNetwork, true);
            simulation.ValidationData = ValidationData;
            simulation.ImagesDisturbanceProbability = ImagesDisturbanceProbability;
            simulation.ImageDisturbanceMaxDifference = ImageDisturbanceMaxDifference;
            simulation.MaxEpoch = MaxEpoch;
            simulation.Config = this;
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
                neuron = new BackpropagationNeuron(activationFunction, Alpha, Momentum, Regularization);
            }
            else
            {
                Console.WriteLine($"Wrong neuron: {Neuron}");
            }

            return neuron;
        }

        public bool CanBeReduced()
        {
            return CanBeReduced(WeightsRepeat) || CanBeReduced(AlphaRepeat) || CanBeReduced(HiddenNeuronsRepeat) || CanBeReduced(ValidationDataRepeat) 
                || CanBeReduced(MomentumRepeat) || CanBeReduced(ImagesDisturbanceProbabilityRepeat) || CanBeReduced(ImageDisturbanceMaxDifferenceRepeat)
                || CanBeReduced(RegularizationRepeat);
        }

        private bool CanBeReduced<T>(List<T> fieldRepeat)
        {
            return fieldRepeat != null && fieldRepeat.Count > 0;
        }

        public List<Config> Reduce()
        {
            List<Config> reducedConfigs = new List<Config>();

            if (CanBeReduced())
            {
                InitReduce(ref WeightsRepeat, Weights);
                InitReduce(ref AlphaRepeat, Alpha);
                InitReduce(ref HiddenNeuronsRepeat, HiddenNeurons);
                InitReduce(ref ValidationDataRepeat, ValidationData);
                InitReduce(ref MomentumRepeat, Momentum);
                InitReduce(ref ImagesDisturbanceProbabilityRepeat, ImagesDisturbanceProbability);
                InitReduce(ref ImageDisturbanceMaxDifferenceRepeat, ImageDisturbanceMaxDifference);
                InitReduce(ref RegularizationRepeat, Regularization);

                foreach (var weight in WeightsRepeat)
                {
                    foreach (var alpha in AlphaRepeat)
                    {
                        foreach (var hiddenNeurons in HiddenNeuronsRepeat)
                        {
                            foreach (var validationData in ValidationDataRepeat)
                            {
                                foreach (var momentum in MomentumRepeat)
                                {
                                    foreach (var imageDisturbanceProbability in ImagesDisturbanceProbabilityRepeat)
                                    {
                                        foreach (var imageDisturbanceMaxDifference in ImageDisturbanceMaxDifferenceRepeat)
                                        {
                                            foreach (var regularization in RegularizationRepeat)
                                            {
                                                Config clone = (Config)this.MemberwiseClone();
                                                clone.Weights = weight;
                                                clone.Alpha = alpha;
                                                clone.HiddenNeurons = hiddenNeurons;
                                                clone.ValidationData = validationData;
                                                clone.Momentum = momentum;
                                                clone.ImagesDisturbanceProbability = imageDisturbanceProbability;
                                                clone.ImageDisturbanceMaxDifference = imageDisturbanceMaxDifference;
                                                clone.Regularization = regularization;
                                                reducedConfigs.Add(clone);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                reducedConfigs.Add(this);
            }

            return reducedConfigs;
        }

        private void InitReduce<T>(ref List<T> fieldRepeat, T field)
        {
            if (fieldRepeat == null)
            {
                fieldRepeat = new List<T>();
                fieldRepeat.Add(field);
            }
        }

        public class Range
        {
            public float Min;
            public float Max;
        }
    }
}
