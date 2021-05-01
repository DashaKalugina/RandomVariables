using Accord.Math;
using RandomVariablesLibraryNew.Distributions.Base;
using RandomVariablesLibraryNew.Segments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomVariablesLibraryNew.Distributions.Standard
{
    public class GammaDistribution : Distribution
    {
        public double K { get; }
        public double Theta { get; }

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
                    if (K < 1)
                    {
                        return 1;
                    }
                    if (K == 1)
                    {
                        return 1 / Theta;
                    }
                    return 0;
                }

                return Math.Exp((-1) * x / Theta) * Math.Pow(x, K - 1) / (Math.Pow(Theta, K) * Gamma.Function(K));
            };
        }

        public GammaDistribution(double k, double theta)
        {
            if (k <= 0 || theta <= 0)
            {
                throw new Exception("Параметры распределения должны быть положительными!");
            }

            K = k;
            Theta = theta;

            InitPiecewisePDF();
        }

        protected void InitPiecewisePDF()
        {
            PiecewisePDF = new PiecewiseFunction();

            if (K < 1)
            {
                PiecewisePDF.AddSegment(new SegmentWithPole(0, 1, ProbabilityFunction, true));
                PiecewisePDF.AddSegment(new PlusInfinitySegment(1, ProbabilityFunction));
            }
            else if (K == 1)
            {
                PiecewisePDF.AddSegment(new Segment(0, 1, ProbabilityFunction));
                PiecewisePDF.AddSegment(new PlusInfinitySegment(1, ProbabilityFunction));
            }
            else
            {
                var mode = (K - 1) * Theta;
                PiecewisePDF.AddSegment(new Segment(0, mode / 2, ProbabilityFunction));
                PiecewisePDF.AddSegment(new Segment(mode / 2, mode, ProbabilityFunction));
                PiecewisePDF.AddSegment(new Segment(mode, 2 * mode, ProbabilityFunction));
                PiecewisePDF.AddSegment(new PlusInfinitySegment(2 * mode, ProbabilityFunction));
            }
        }

        public override double GetNewRandomValue()
        {
            throw new NotImplementedException();
        }
    }
}
