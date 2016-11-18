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

            originalCount = DigitImages.Count;
        }

        public void AddDisturbance(float probability = 0.1f, float maxDifference = 1)
        {
            for (int i = 0; i < originalCount; i++)
            {
                var imageCopy = new DigitImage(DigitImages[i]);
                imageCopy.Disturb(probability, maxDifference, random);
                DigitImages.Add(imageCopy);
            }
        }

        public List<TrainData> GetTrainData()
        {
            List<TrainData> trainData = DigitImages.ConvertAll(x => x.ConvertToTrainData());
            trainData.Shuffle();
            return trainData;
        }
    }
}
