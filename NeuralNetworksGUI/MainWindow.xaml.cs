using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
        private List<Simulation> simulations;

        public MainWindow()
        {
            InitializeComponent();
            LoadLineChartData();
        }

        private void EvaluateSimulations()
        {
            foreach (var simulation in simulations)
            {
                var correct = simulation.TestSoftMax(trainData);

                MessageBox.Show($"Result: {(correct*10000)/100}%");
            }

            EnableButtons();
        }

        private void TrainSimulations()
        {
            foreach (var simulation in simulations)
            {
                for (int i = 0; i < 100; i++)
                {
                    float error = simulation.Train(trainData);
                    AddChartValue(error);
                }
            }

            EnableButtons();
        }

        private void ResetSimulations()
        {
            foreach (var simulation in simulations)
            {
                simulation.Reset();
            }

            EnableButtons();
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

        private void LoadBtn_Click(object sender, RoutedEventArgs e)
        {
            string filePath = $"../../../Simulations/{ConfigTextBox.Text}.json";

            if (!File.Exists(filePath))
            {
                DisableButtons();
                return;
            }

            var simulationReader = new SimulationReader();
            simulationReader.ReadConfigsFile(filePath);

            simulations = simulationReader.Configs.ConvertAll(x => x.CreateSimulation());

            imageParser = new ImageParser(@"D:\Studia\Semestr VII\Sieci neuronowe\NeuralNetworks\PngData\");
            imageParser.Parse();
            trainData = imageParser.GetTrainData();

            EnableButtons();
        }

        private void DisableButtons()
        {
            TrainBtn.IsEnabled = false;
            EvaluateBtn.IsEnabled = false;
            ResetBtn.IsEnabled = false;
        }

        private void EnableButtons()
        {
            TrainBtn.IsEnabled = true;
            EvaluateBtn.IsEnabled = true;
            ResetBtn.IsEnabled = true;
        }

        private void EvaluateBtn_Click(object sender, RoutedEventArgs e)
        {
            DisableButtons();
            EvaluateSimulations();
        }

        private void TrainBtn_Click(object sender, RoutedEventArgs e)
        {
            DisableButtons();
            TrainSimulations();
        }

        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            DisableButtons();
            ResetSimulations();
        }
    }
}
