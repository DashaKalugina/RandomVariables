using System;

namespace RandomVariables
{
    /// <summary>
    /// Генератор равномерно распределенной СВ от a до b
    /// </summary>
    public static class UniformGenerator
    {
        //public double Mean => (_a + _b) / 2;

        //public double Variance => Math.Pow(_b - _a, 2) / 12;

        private static readonly Random _random = new Random();

        public static double Next(double a, double b)
        {
            return a + (b - a) * _random.NextDouble();
        }
    }
}
