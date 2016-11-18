using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetworksSimulation;
using Utils;

namespace DigitImageParser
{
    public class DigitImage
    {
        public int Digit { get; set; }
        public List<float> Pixels { get; } = new List<float>();

        public DigitImage()
        {
            
        }

        public DigitImage(DigitImage digitImage)
        {
            this.Digit = digitImage.Digit;
            this.Pixels.AddRange(digitImage.Pixels);
        }

        public TrainData ConvertToTrainData()
        {
            var trainData = new TrainData();
            trainData.Inputs = Pixels;
            trainData.Outputs = Enumerable.Repeat(0f, 10).ToList();
            trainData.Outputs[Digit] = 1f;
            return trainData;
        }

        public void Disturb(float probability, float maxDifference, Random random)
        {
            for (int i = 0; i < Pixels.Count; i++)
            {
                if (random.NextDouble() < probability)
                {
                    float difference = (float)(random.NextDouble()*2*maxDifference - maxDifference);
                    Pixels[i] += difference;
                    Pixels[i].Clamp(0f, 1f);
                }
            }
        }
    }
}
