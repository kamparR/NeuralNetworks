using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using DigitImageParser;
using NeuralNetworksSimulation;

namespace NeuralNetworksGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<KeyValuePair<int, float>> source;
        private Random rand = new Random();
        private ImageParser imageParser;
        private List<TrainData> trainData;

        public MainWindow()
        {
            InitializeComponent();
            LoadLineChartData();

            imageParser = new ImageParser(@"D:\Studia\Semestr VII\Sieci neuronowe\NeuralNetworks\PngData\");
            imageParser.Parse();
            trainData = imageParser.GetTrainData();
        }

        private void StartSimulation()
        {
            
        }

        private void AddChartValue(float value)
        {
            source.Add(new KeyValuePair<int, float>(source.Count, value));
        }

        private void LoadLineChartData()
        {
            source = new ObservableCollection<KeyValuePair<int, float>>();
            ChartSeries.DataContext = source;
        }

        private void loadBtn_Click(object sender, RoutedEventArgs e)
        {
            AddChartValue((float)(rand.NextDouble() * 100));
        }
    }
}
