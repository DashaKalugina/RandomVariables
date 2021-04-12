using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomVariablesLibraryNew.Generators
{
    /// <summary>
    /// Генератор нормально распределенной СВ
    /// </summary>
    public static class NormalGenerator
    {
        private static readonly NormalRandom _random = new NormalRandom();

        public static double Next(double mu, double sigma)
        {
            return sigma * _random.NextDouble() + mu;
        }
    }
}
