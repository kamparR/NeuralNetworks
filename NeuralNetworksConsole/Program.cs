using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetworksSimulation;

namespace NeuralNetworksConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            var simulationReader = new SimulationReader();
            Console.Write("Simulation config file name (Simulations/XXX.json): ");
            string fileName = Console.ReadLine();

            simulationReader.ReadConfigsFile($"../Simulations/{fileName}.json");
            var file = new StreamWriter($"../Simulations/{fileName}-results.csv");
            Console.WriteLine("Loaded configs");

            var headers = $"{Config.GetCsvHeaders()};LoopsMean;";
            file.WriteLine(headers);
            Console.WriteLine(headers);
            var simulations = new List<Simulation>();

            foreach (var config in simulationReader.Configs)
            {
                var simulation = config.CreateSimulation();
                simulations.Add(simulation);
                var loopsResults = new List<int>();
                List<float> errors = null;

                for (int j = 0; j < config.Repeat; j++)
                {
                    simulation.Reset();
                    errors = simulation.TrainByThreshold(config.TrainData, config.TrainThreshold);

                    if (errors == null)
                    {
                        break;
                    }

                    loopsResults.Add(errors.Count);
                }

                string data = $"{config.GetCsvData()};";

                if (errors == null)
                {
                    data += "timeout";
                }
                else
                {
                    data += loopsResults.Sum()/(float) loopsResults.Count;

                    foreach (float error in errors)
                    {
                        data += $";{error}";
                    }
                }

                file.WriteLine(data);
                Console.WriteLine($"{simulations.Count}: {data}");
            }

            file.Close();
            Console.WriteLine("Ended");

            ConsoleMenu(simulations);
        }

        static void ConsoleMenu(List<Simulation> simulations)
        {
            Console.WriteLine("Type 'q' to exit");
            string typed = "";

            Func<string, bool> isExitString = value => value == "q" || value == "exit" || value == "quit" || string.IsNullOrEmpty(value);

            do
            {
                Console.Write("Select simulation number to compute values: ");
                typed = Console.ReadLine();

                if (isExitString(typed))
                {
                    continue;
                }

                int simulationNumber;
                if (int.TryParse(typed, out simulationNumber) && simulationNumber > 0 && simulationNumber <= simulations.Count)
                {
                    var simulation = simulations[simulationNumber - 1];

                    do
                    {
                        Console.Write("Write input values separated by ',': ");
                        typed = Console.ReadLine();

                        if (isExitString(typed))
                        {
                            continue;
                        }

                        string[] inputValues = typed.Split(',', ';');
                        var inputs = new List<float>();

                        foreach (var inputValue in inputValues)
                        {
                            inputs.Add(float.Parse(inputValue.Trim()));
                        }

                        var result = simulation.Compute(inputs);

                        var resultMessage = "Result: {";
                        for (int k = 0; k < result.Count; k++)
                        {
                            if (k > 0)
                            {
                                resultMessage += ", ";
                            }

                            resultMessage += result[k];
                        }
                        Console.WriteLine(resultMessage + "}");

                    } while (!isExitString(typed));
                }
            } while (!isExitString(typed));


            Console.WriteLine("End");
        }
    }
}
