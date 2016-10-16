using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NeuralNetworksConsole
{
    class SimulationReader
    {
        public List<Config> Configs { private set; get; }

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
        }

        private void ThrowError(string message)
        {
            Console.WriteLine(message);
            throw new Exception(message);
        }
    }
}
