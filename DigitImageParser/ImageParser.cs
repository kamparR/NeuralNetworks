using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetworksSimulation;

namespace DigitImageParser
{
    public class ImageParser
    {
        private string path;
        public List<DigitImage> DigitImages { get; }

        public ImageParser(string path)
        {
            this.path = path;
            DigitImages = new List<DigitImage>();
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
        }

        public List<TrainData> GetTrainData()
        {
            return DigitImages.ConvertAll(x => x.ConvertToTrainData());
        }
    }
}
