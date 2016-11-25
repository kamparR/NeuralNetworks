using NeuralNetworksSimulation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatexBuilder
{
    public class LatexFile
    {
        private string fileName;
        private Dictionary<Simulation, List<float>> simulationCharts;
        private List<TrainData> testData;

        public LatexFile(string fileName)
        {
            this.fileName = fileName;
            simulationCharts = new Dictionary<Simulation, List<float>>();

            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
        }

        public void AddChartValue(Simulation simulation, float value)
        {
            if (!simulationCharts.ContainsKey(simulation))
            {
                simulationCharts.Add(simulation, new List<float>());
            }

            simulationCharts[simulation].Add(value);
        }

        public void Generate(List<TrainData> testData)
        {
            this.testData = testData;
            var file = new StreamWriter($"{fileName}.tex");

            file.Write($@"
{GenerateDocumentBegin()}

{GenerateCharts()}

{GenerateDocumentEnd()}
");

            file.Close();
        }

        private string GenerateDocumentBegin()
        {
            return @"\documentclass{article}
\usepackage{graphicx}
\usepackage[polish]{babel}
\usepackage{polski}
\usepackage[utf8]{inputenc}
\usepackage[a4paper,left=2.5cm,right=2.5cm,top=2.5cm,bottom=2.5cm]{geometry}
\usepackage{here}
\usepackage{enumitem}
\usepackage{pgfplots}
\usepackage{subcaption}
\usepackage[backend=bibtex]{biblatex}

\begin{document}

\title{Sprawozdanie 2}
\author{Kamil Paruszkiewicz}

\maketitle
";
        }

        private string GenerateCharts()
        {
            string charts = "";

            foreach (var chart in simulationCharts)
            {
                charts += GenerateChart(chart.Key, chart.Value);
            }

            return charts;
        }

        private string GenerateChart(Simulation simulation, List<float> values)
        {
            string chart = "";
            
            chart += GenerateSimulationData(simulation);

            chart += @"
\begin{figure}[H]
\centering
\begin{tikzpicture}
\begin{axis}[
    %title={},
    xlabel={Epoch},
    ylabel={Error \%},
    xmin=0,
    ymin=0, ymax=100,
]
 
\addplot[color=blue]
    coordinates {";

            for (int i = 0; i < values.Count; i++)
            {
                chart += $"({i},{values[i]})";
            }

            chart += @"};
 
\end{axis}
\end{tikzpicture}
\end{figure}
";
            return chart;
        }

        private string GenerateSimulationData(Simulation simulation)
        {
            var config = simulation.Config;
            string data = "";

            data += $@"Min, Max Weight: {config.Weights.Min}, {config.Weights.Max}
Alpha: {config.Alpha}
HiddenNeurons: {config.HiddenNeurons}
ValidationData: {config.ValidationData}
ImagesDisturbanceProbability: {config.ImagesDisturbanceProbability}
ImageDisturbanceMaxDifference: {config.ImageDisturbanceMaxDifference}
Momentum: {config.Momentum} \\
TestCorrect: {simulation.TestSoftMax(testData) * 10000 / 100}%";

            return data;
        }

        private string GenerateDocumentEnd()
        {
            return @"
\end{document}";
        }

        public void Clear()
        {
            
        }
    }
}
