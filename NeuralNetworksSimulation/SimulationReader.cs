using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NeuralNetworksSimulation
{
    public class SimulationReader
    {
        public List<Config> Configs { private set; get; }
        public string ImagesPath { set; get; }
        public float ImagesDisturbanceProbability { set; get; }
        public float ImageDisturbanceMaxDifference { set; get; }

        public void ReadConfigsFile(string path)
        {
            if (!File.Exists(path))
            {
                ThrowError($"File doesn't exist, path: {path}");
            }

            var file = new StreamReader(path);
            string json = file.ReadToEnd();
            var configsToReduce = JsonConvert.DeserializeObject<List<Config>>(json);
            Configs = new List<Config>();

            foreach (var config in configsToReduce)
            {
                if (config.CanBeReduced())
                {
                    List<Config> reducedConfigs = config.Reduce();
                    Configs.AddRange(reducedConfigs);
                }
                else
                {
                    Configs.Add(config);
                }
            }

            if (Configs.Count > 0)
            {
                var config = Configs[0];
                ImagesPath = config.ImagesPath;
                ImagesDisturbanceProbability = config.ImagesDisturbanceProbability;
                ImageDisturbanceMaxDifference = config.ImageDisturbanceMaxDifference;
            }
        }

        private void ThrowError(string message)
        {
            Console.WriteLine(message);
            throw new Exception(message);
        }
    }
}
