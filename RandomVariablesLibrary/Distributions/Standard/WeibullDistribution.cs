using RandomVariablesLibrary.Distributions.Base;
using RandomVariablesLibrary.Segments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeibullGenerator = Accord.Statistics.Distributions.Univariate.WeibullDistribution;

namespace RandomVariablesLibrary.Distributions.Standard
{
    public class WeibullDistribution : Distribution
    {
        public double K { get; set; }

        public double Lambda { get; set; }

        public Func<double, double> ProbabilityFunction
        {
            get => (x) =>
            {
                if (x < 0)
                {
                    return 0;
                }

                if (x == 0)
                {
                    //if (K < 1)
                    //{
                    //    return double.PositiveInfinity;
                    //}
                    if (K == 1)
                    {
                        return 1;
                    }
                    if (K > 1)
                    {
                        return 0;
                    }
                }

                return (K / Lambda)
                       * Math.Pow(x / Lambda, K - 1)
                       * Math.Exp((-1) * Math.Pow(x / Lambda, K));
            };
        }

        public WeibullDistribution(double k, double lambda)
        {
            K = k;
            Lambda = lambda;

            InitPiecewisePDF();
        }

        protected void InitPiecewisePDF()
        {
            PiecewisePDF = new PiecewiseFunction();

            if (K <= 1)
            {
                PiecewisePDF.AddSegment(new SegmentWithPole(0, K, ProbabilityFunction, true));
                PiecewisePDF.AddSegment(new PlusInfinitySegment(K, ProbabilityFunction));
            }
            else
            {
                var mode = Lambda * Math.Pow(((K - 1) / K), 1 / K);
                PiecewisePDF.AddSegment(new SegmentWithPole(0, mode, ProbabilityFunction, true));
                PiecewisePDF.AddSegment(new PlusInfinitySegment(mode, ProbabilityFunction));
            }
        }

        public override double GetNewRandomValue()
        {
            return WeibullGenerator.Random(K, Lambda);
        }
    }
}
