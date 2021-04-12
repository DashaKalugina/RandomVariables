using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomVariablesLibraryNew.Generators
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
