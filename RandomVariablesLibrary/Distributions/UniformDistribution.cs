using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomVariables
{
    /// <summary>
    /// Класс равномерного распределения
    /// </summary>
    public class UniformDistribution : DistributionBase
    {
        public double A { get; set; }
        public double B { get; set; }

        public override Func<double, double> ProbabilityFunction
        {
            get => (x) =>
            {
                if (x >= A && x <= B)
                {
                    return 1.0 / (B - A);
                }
                return 0;
            };
        }

        public override Func<double, double> DistributionFunction
        {
            get => (x) =>
            {
                if (x >= A && x < B)
                {
                    return (x - A) / (B - A);
                }
                if (x >= B)
                {
                    return 1;
                }
                return 0;
            };
        }

        public override double GetNewRandomValue()
        {
            return UniformGenerator.Next(A, B);
        }

        //public UniformGenerator uniformGenerator
        //{
        //    get => new UniformGenerator(a, b);
        //}

        //public override IRandomNumberGenerator RandomNumberGenerator => UniformGenerator.Next(A, B);
    }
}
