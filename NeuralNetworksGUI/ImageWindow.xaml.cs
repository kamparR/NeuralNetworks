using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DigitImageParser;
using NeuralNetworksSimulation;

namespace NeuralNetworksGUI
{
    /// <summary>
    /// Interaction logic for Image.xaml
    /// </summary>
    public partial class ImageWindow : Window
    {
        private List<DigitImage> data;
        private Simulation simulation;
        private Random random;
        private int currentImageNumber;
        private int currentFeatureNumber;

        public ImageWindow(List<DigitImage> data, Simulation simulation)
        {
            InitializeComponent();
            this.data = data;
            this.simulation = simulation;
            random = new Random();
            currentImageNumber = 0;
            currentFeatureNumber = 0;

            RandNextImage();
        }

        private void RandNextImage()
        {
            currentImageNumber = random.Next(data.Count);
            RefreshImage();
        }

        private void RandNextFeature()
        {
            currentFeatureNumber = random.Next(simulation.GetHiddenNeuronsNumber());
            RefreshFeatureImage();
        }

        private void RefreshImage()
        {
            FirstCanvas.Children.Clear();
            SecondCanvas.Children.Clear();
            ThirdCanvas.Children.Clear();

            var currentImage = data[currentImageNumber];
            var width = currentImage.Width;
            var inputs = currentImage.Pixels;
            var outputs = simulation.Compute(inputs);
            var feature = simulation.GetFeature(currentFeatureNumber, inputs);

            Draw(FirstCanvas, inputs, width, true);
            Draw(SecondCanvas, outputs, width, true);
            Draw(ThirdCanvas, feature, width, true);
        }

        private void RefreshFeatureImage()
        {
            ThirdCanvas.Children.Clear();
            var width = data[currentImageNumber].Width;
            var values = simulation.GetFeature(currentFeatureNumber, null);

            Draw(ThirdCanvas, values, width, true);
        }

        private void Draw(Canvas canvas, List<float> pixels, int width, bool reverse = false)
        {
            float pixelSize = (float) FirstCanvas.Width/width;
            int height = pixels.Count/width;
            canvas.Height = pixelSize*height;

            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    float pixelValue = pixels[j*width + i];

                    if (reverse)
                    {
                        pixelValue = 1 - pixelValue;
                    }

                    var rectangle = new Rectangle();
                    byte colorPart = (byte) (Byte.MaxValue*pixelValue);
                    var color = Color.FromRgb(colorPart, colorPart, colorPart);
                    var colorBrush = new SolidColorBrush(color);
                    rectangle.Stroke = colorBrush;
                    rectangle.Fill = colorBrush;
                    rectangle.Width = pixelSize;
                    rectangle.Height = pixelSize;
                    Canvas.SetLeft(rectangle, i * pixelSize);
                    Canvas.SetTop(rectangle, j * pixelSize);

                    canvas.Children.Add(rectangle);
                }
            } 
        }

        private void NextImageBtn_Click(object sender, RoutedEventArgs e)
        {
            RandNextImage();
        }

        private void NextFeatureBtn_Click(object sender, RoutedEventArgs e)
        {
            RandNextFeature();
        }
    }
}
