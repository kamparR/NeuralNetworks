using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetworksSimulation;

namespace DigitImageParser
{
    public class DigitImage
    {
        public int Digit { get; set; }
        public List<float> Pixels { get; } = new List<float>();

        public TrainData ConvertToTrainData()
        {
            var trainData = new TrainData();
            trainData.Inputs = Pixels;
            trainData.Outputs = Enumerable.Repeat(0f, 10).ToList();
            trainData.Outputs[Digit] = 1f;
            return trainData;
        }
    }
}
