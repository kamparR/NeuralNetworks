using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    public class WeightInitializer
    {
        private Random random;
        private float min;
        private float max;

        public WeightInitializer(float min = -1f, float max = 1f)
        {
            random = new Random();
            this.min = min;
            this.max = max;
        }

        public WeightInitializer(float max) :
            this(-max, max)
        {
        }

        public float NextWeight()
        {
            return (float)(min + (random.NextDouble() * (max - min)));
        }
    }
}
