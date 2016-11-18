using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using DigitImageParser;
using System.Windows.Forms;
using NeuralNetworksSimulation;
using MessageBox = System.Windows.MessageBox;
using Utils;

namespace NeuralNetworksGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MTObservableCollection<KeyValuePair<int, float>> TrainSeriesSource { get; }
        public MTObservableCollection<KeyValuePair<int, float>> TestSeriesSource { get; }
        private Random rand = new Random();
        private ImageParser imageParser;
        private List<TrainData> trainData;
        private List<Simulation> simulations;
        private Timer trainTimer;

        public MainWindow()
        {
            InitializeComponent();
            TrainSeriesSource = new MTObservableCollection<KeyValuePair<int, float>>();
            TestSeriesSource = new MTObservableCollection<KeyValuePair<int, float>>();
            TrainSeries.DataContext = TrainSeriesSource;
            TestSeries.DataContext = TestSeriesSource;
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
            trainTimer = new Timer();
            trainTimer.Interval = 1;
            trainTimer.Tick += (s, args) =>
            {
                float error = simulations[0].Train(trainData);
                float correct = simulations[0].TestSoftMax(trainData);
                
                AddTrainSeriesValue(error * 100f);
                AddTestSeriesValue((1f - correct) * 100f);
            };
            trainTimer.Start();
        }

        private void ResetSimulations()
        {
            foreach (var simulation in simulations)
            {
                simulation.Reset();
            }

            trainData.Shuffle();
            TrainSeriesSource.Clear();
            TestSeriesSource.Clear();
            EnableButtons();
        }

        private void AddTrainSeriesValue(float value)
        {
            TrainSeriesSource.Add(new KeyValuePair<int, float>(TrainSeriesSource.Count, value));
        }

        private void AddTestSeriesValue(float value)
        {
            TestSeriesSource.Add(new KeyValuePair<int, float>(TestSeriesSource.Count, value));
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

            imageParser = new ImageParser(simulationReader.ImagesPath);
            imageParser.Parse();
            imageParser.AddDisturbance(simulationReader.ImagesDisturbanceProbability, simulationReader.ImageDisturbanceMaxDifference);
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
            StopBtn.IsEnabled = true;
            LoadBtn.IsEnabled = false;
            TrainSimulations();
        }

        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            DisableButtons();
            ResetSimulations();
        }

        private void StopBtn_Click(object sender, RoutedEventArgs e)
        {
            StopBtn.IsEnabled = false;
            LoadBtn.IsEnabled = true;
            trainTimer.Stop();
            EvaluateSimulations();
        }
    }
}
