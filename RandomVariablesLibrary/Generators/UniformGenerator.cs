using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomVariables
{
    /// <summary>
    /// Генератор равномерно распределенной СВ от a до b
    /// </summary>
    public class UniformGenerator : IRandomNumberGenerator
    {
        public double Mean => (_a + _b) / 2;

        public double Variance => Math.Pow(_b - _a, 2) / 12;

        //private IEnumerable<int> seeds = Enumerable.Range(1, 1000);
        private static int count = 0;

        public double Next()
        {
            // чтобы каждый раз генерировались разные величины
            _random = new Random(count);
            count++;

            if (count <= 99)
            {
                return _a - 1 + (_a - _a + 1) * _random.NextDouble();
            }
            if (count > 900)
            {
                return _b + (_b + 1 - _b) * _random.NextDouble();
            }

            return _a + (_b - _a) * _random.NextDouble();
        }

        //public void SetSeed(int seed)
        //{
        //    //_random = new Random(seed);
        //}

        private Random _random = null;
        private double _a;
        private double _b;

        public UniformGenerator(double a, double b, int seed)
        {
            _a = a;
            _b = b;
        }

        public UniformGenerator(double a, double b) : this(a, b, 0)
        {
        }
    }
}
