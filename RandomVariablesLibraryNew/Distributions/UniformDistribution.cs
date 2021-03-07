using RandomVariablesLibraryNew.Segments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomVariablesLibraryNew.Distributions
{
    /// <summary>
    /// Класс равномерного распределения
    /// </summary>
    public class UniformDistribution: Distribution
    {
        public double A { get; }
        public double B { get; }

        public Func<double, double> ProbabilityFunction
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

        public UniformDistribution(double a, double b)
        {
            A = a;
            B = b;

            PiecewisePDF = new PiecewiseFunction();
            PiecewisePDF.AddSegment(new Segment(A, B, ProbabilityFunction));
        }
    }
}
