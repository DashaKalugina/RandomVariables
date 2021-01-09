using RandomVariablesLibrary.Generators;
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
    public static class UniformGenerator
    {
        //public double Mean => (_a + _b) / 2;

        //public double Variance => Math.Pow(_b - _a, 2) / 12;

        //private IEnumerable<int> seeds = Enumerable.Range(1, 1000);
        //private static int count = 0;

        private static readonly Random _random = new Random();

        public static double Next(double a, double b)
        {
            return a + (b - a) * _random.NextDouble();
        }
    }
}
