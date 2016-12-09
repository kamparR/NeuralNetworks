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

namespace NeuralNetworksGUI
{
    /// <summary>
    /// Interaction logic for ColorWindow.xaml
    /// </summary>
    public partial class ColorWindow : Window
    {
        public ColorWindow()
        {
            InitializeComponent();
        }

        public void Update(List<float> colors)
        {
            var pixels = new List<Tuple<float, float, float>>();

            for (int i = 0; i < colors.Count; i += 3)
            {
                var pixel = new Tuple<float, float, float>(colors[i], colors[i+1], colors[i+2]);
                pixels.Add(pixel);
            }

            Draw(pixels, (int)Math.Sqrt(pixels.Count));
        }

        private void Draw(List<Tuple<float, float, float>> pixels, int width)
        {
            ColorCanvas.Children.Clear();
            float pixelSize = (float)ColorCanvas.Width / width;
            int height = pixels.Count / width;
            ColorCanvas.Height = pixelSize * height;

            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    var pixelValue = pixels[j * width + i];

                    var rectangle = new Rectangle();
                    byte red = (byte)(Byte.MaxValue * pixelValue.Item1);
                    byte green = (byte)(Byte.MaxValue * pixelValue.Item2);
                    byte blue = (byte)(Byte.MaxValue * pixelValue.Item3);
                    var color = Color.FromRgb(red, green, blue);
                    var colorBrush = new SolidColorBrush(color);
                    rectangle.Stroke = colorBrush;
                    rectangle.Fill = colorBrush;
                    rectangle.Width = pixelSize;
                    rectangle.Height = pixelSize;
                    Canvas.SetLeft(rectangle, i * pixelSize);
                    Canvas.SetTop(rectangle, j * pixelSize);

                    ColorCanvas.Children.Add(rectangle);
                }
            }
        }
    }
}
