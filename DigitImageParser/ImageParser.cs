using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetworksSimulation;
using Utils;

namespace DigitImageParser
{
    public class ImageParser
    {
        private string path;
        private Random random;
        private int originalCount;
        public List<DigitImage> DigitImages { get; }
        public List<TrainData> TrainData { private set; get; }
        public List<TrainData> TestData { private set; get; }


        public ImageParser(string path)
        {
            this.path = path;
            DigitImages = new List<DigitImage>();
            random = new Random();
        }

        public void Parse()
        {
            foreach (string filePath in Directory.GetFiles(path))
            {
                int digit = int.Parse(Path.GetFileName(filePath)[0].ToString());
                Bitmap bitmap = new Bitmap(filePath);
                var digitImage = new DigitImage();
                digitImage.Digit = digit;
                digitImage.Width = bitmap.Width;
                digitImage.Height = bitmap.Height;

                for (int y = 0; y < bitmap.Height; y++)
                {
                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        Color color = bitmap.GetPixel(x, y);
                        digitImage.Pixels.Add(color.GetBrightness());
                    }
                }

                DigitImages.Add(digitImage);
            }
            
            DigitImages.Shuffle();
            originalCount = DigitImages.Count;
        }

        public void GenerateDataSets(float validationData, float disturbanceProbability = 0.1f, float disturbanceMaxDifference = 1)
        {
            int testDataCount = ((int)(DigitImages.Count * validationData)).Clamp(0, DigitImages.Count - 1);
            var testImages = DigitImages.GetRange(0, testDataCount);
            var trainImages = DigitImages.GetRange(testDataCount, DigitImages.Count - testDataCount);

            TestData = testImages.ConvertAll(x => x.ConvertToTrainData());

            if (disturbanceProbability > 0)
            {
                AddDisturbance(trainImages, disturbanceProbability, disturbanceMaxDifference);
            }

            TrainData = trainImages.ConvertAll(x => x.ConvertToTrainData());
        }

        private void AddDisturbance(List<DigitImage> dataSet, float probability = 0.1f, float maxDifference = 1)
        {
            int count = dataSet.Count;
            for (int i = 0; i < count; i++)
            {
                var imageCopy = new DigitImage(dataSet[i]);
                imageCopy.Disturb(probability, maxDifference, random);
                dataSet.Add(imageCopy);
            }
        }
    }
}
