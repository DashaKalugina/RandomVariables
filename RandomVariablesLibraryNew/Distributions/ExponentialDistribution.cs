using RandomVariablesLibraryNew.Generators;
using RandomVariablesLibraryNew.Segments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomVariablesLibraryNew.Distributions
{
    public class ExponentialDistribution: Distribution
    {
        public double Lambda { get; }

        public Func<double, double> ProbabilityFunction
        {
            get => (x) =>
            {
                if (x < 0)
                {
                    return 0;
                }

                return Lambda * Math.Pow(Math.E, (-1) * Lambda * x);
            };
        }

        public ExponentialDistribution(double lambda)
        {
            Lambda = lambda;

            PiecewisePDF = new PiecewiseFunction();

            PiecewisePDF.AddSegment(new Segment(0, 1, ProbabilityFunction));
            PiecewisePDF.AddSegment(new PlusInfinitySegment(1, ProbabilityFunction));
        }

        public override double GetNewRandomValue()
        {
            return ExponentialGenerator.Next(Lambda);
        }
    }
}
