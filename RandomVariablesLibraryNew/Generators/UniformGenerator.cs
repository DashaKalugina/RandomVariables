using System;

namespace RandomVariablesLibraryNew.Generators
{
    /// <summary>
    /// Генератор равномерно распределенной СВ от a до b
    /// </summary>
    public static class UniformGenerator
    {
        private static readonly Random _random = new Random();

        public static double Next(double a, double b)
        {
            return a + (b - a) * _random.NextDouble();
        }
    }
}
