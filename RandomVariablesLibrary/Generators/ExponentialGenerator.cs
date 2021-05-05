using System;

namespace RandomVariablesLibrary.Generators
{
    public static class ExponentialGenerator
    {
        private static readonly Random _random = new Random();

        public static double Next(double lambda)
        {
            return (-1) * Math.Log(_random.NextDouble()) / lambda;
        }
    }
}
