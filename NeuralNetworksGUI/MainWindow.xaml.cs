﻿using System;
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
using LatexBuilder;

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
        private List<Simulation> simulations;
        private Timer trainTimer;
        private int currentSimulationNumber = 0;
        private LatexFile latexFile;
        private ColorWindow colorWindow;

        public MainWindow()
        {
            InitializeComponent();
            TrainSeriesSource = new MTObservableCollection<KeyValuePair<int, float>>();
            TestSeriesSource = new MTObservableCollection<KeyValuePair<int, float>>();
            TrainSeries.DataContext = TrainSeriesSource;
            TestSeries.DataContext = TestSeriesSource;
            latexFile = new LatexFile(@"D:\Studia\Semestr VII\Sieci neuronowe\Sprawozdanie\sprawozdanie-3");
        }

        private void EvaluateSimulation()
        {
            //var correct = simulations[currentSimulationNumber].TestSoftMax(imageParser.TestData);
            //MessageBox.Show($"Result: {(correct*10000)/100}%");


            if (simulations?[currentSimulationNumber]?.Config.Network == "SOMNetwork")
            {
                ShowColorWindow();
                colorWindow?.Update(simulations?[currentSimulationNumber]?.GetFeature(0, new List<float>()));
            }
            else
            {
                GenerateNetworkImages();
            }

            EnableButtons();
        }

        private void TrainSimulation(int simulationNumber, bool allSimlations = false)
        {
            if (trainTimer != null)
            {
                trainTimer.Stop();
                trainTimer = null;
            }

            int epoch = 0;
            var simulation = simulations[simulationNumber];
            int maxEpoch = allSimlations ? simulation.MaxEpoch : -1;
            currentSimulationNumber = simulationNumber;

            if (simulation.Config.Network == "SOMNetwork")
            {
                ShowColorWindow();
                colorWindow?.Update(simulations?[currentSimulationNumber]?.GetFeature(0, new List<float>()));
            }

            trainTimer = new Timer();
            trainTimer.Interval = 1;
            trainTimer.Tick += (s, args) =>
            {
                float error = 0;

                if (simulations?[currentSimulationNumber]?.Config.Network == "SOMNetwork")
                {
                    error = simulation.TrainSOM();

                    if (true || epoch%10 == 0)
                    {
                        colorWindow?.Update(simulations?[currentSimulationNumber]?.GetFeature(0, new List<float>()));
                    }
                }
                else
                {
                    error = simulation.TrainAutoencoder(imageParser.TrainData);
                    //float correctTrain = simulation.TestSoftMax(imageParser.TrainData);
                    //float correctTest = simulation.TestSoftMax(imageParser.TestData);

                    //AddTrainSeriesValue(correctTrain);
                    //AddTestSeriesValue(correctTest);
                }

                AddTrainSeriesValue(error);

                //LogSimulationTrainStep(simulation, simulationNumber, epoch, correctTrain, correctTest);

                epoch++;
                if (maxEpoch > 0 && epoch >= maxEpoch)
                {
                    trainTimer.Dispose();
                }
            };

            trainTimer.Start();
            trainTimer.Disposed += (sender, args) =>
            {
                if (simulationNumber + 1 < simulations.Count)
                {
                    TrainSeriesSource.Clear();
                    TestSeriesSource.Clear();
                    epoch = 0;
                    TrainSimulation(simulationNumber + 1, allSimlations);
                }
                else
                {
                    latexFile.Generate(imageParser.TestData);
                    StopBtn.IsEnabled = false;
                    LoadBtn.IsEnabled = true;
                    EnableButtons();
                    MessageBox.Show("Success");
                }
            };
        }

        private void ResetSimulations()
        {
            foreach (var simulation in simulations)
            {
                simulation.Reset();
            }

            TrainSeriesSource.Clear();
            TestSeriesSource.Clear();
            latexFile.Clear();
            EnableButtons();
        }

        private void LogSimulationTrainStep(Simulation simulation, int simulationNumber, int epoch, float correctTrain, float correctTest)
        {
            float value = (1f - correctTest) *100f;
            latexFile.AddChartValue(simulation, value);
        }

        private void AddTrainSeriesValue(float corrects)
        {
            float value = corrects;
            //float value = (1f - corrects)*100f;
            TrainSeriesSource.Add(new KeyValuePair<int, float>(TrainSeriesSource.Count, value));
        }

        private void AddTestSeriesValue(float corrects)
        {
            float value = (1f - corrects) * 100f;
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

            if (simulations?[currentSimulationNumber]?.Config.Network != "SOMNetwork")
            {
                imageParser = new ImageParser(simulationReader.ImagesPath);
                imageParser.Parse();
                imageParser.GenerateDataSets(simulations[0].ValidationData, simulations[0].ImagesDisturbanceProbability, simulations[0].ImageDisturbanceMaxDifference);
            }

            EnableButtons();
        }

        private void DisableButtons()
        {
            TrainBtn.IsEnabled = false;
            EvaluateBtn.IsEnabled = false;
            ResetBtn.IsEnabled = false;
            RunAllBtn.IsEnabled = false;
        }

        private void EnableButtons()
        {
            TrainBtn.IsEnabled = true;
            EvaluateBtn.IsEnabled = true;
            ResetBtn.IsEnabled = true;
            RunAllBtn.IsEnabled = true;
        }

        private void EvaluateBtn_Click(object sender, RoutedEventArgs e)
        {
            DisableButtons();
            EvaluateSimulation();
        }

        private void TrainBtn_Click(object sender, RoutedEventArgs e)
        {
            DisableButtons();
            StopBtn.IsEnabled = true;
            LoadBtn.IsEnabled = false;
            TrainSimulation(0);
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
            EvaluateSimulation();
        }

        private void RunAllBtn_Click(object sender, RoutedEventArgs e)
        {
            DisableButtons();
            StopBtn.IsEnabled = true;
            LoadBtn.IsEnabled = false;

            TrainSimulation(0, true);
        }

        private void GenerateNetworkImages()
        {
            var imageWindow = new ImageWindow(imageParser.DigitImages, simulations[0]);
            imageWindow.ShowDialog();
        }

        private void ShowColorWindow()
        {
            colorWindow = new ColorWindow();
            colorWindow.Show();
        }
    }
}
