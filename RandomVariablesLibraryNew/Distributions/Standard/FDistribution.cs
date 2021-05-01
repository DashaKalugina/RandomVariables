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
    public class FDistribution : Distribution
    {
        public double Df1 { get; set; }

        public double Df2 { get; set; }

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
                    //return Df1 < 2
                    //    ? double.PositiveInfinity
                    //    : Df1 == 2 ? 1 : 0;

                    if (Df1 == 2)
                    {
                        return 1;
                    }
                    else if (Df1 > 2)
                    {
                        return 0;
                    }
                }

                var numerator = Gamma.Function((Df1 + Df2) / 2) * Math.Pow(Df1, Df1 / 2) * Math.Pow(Df2, Df2 / 2) * Math.Pow(x, Df1 / 2 - 1);
                var denominator = Gamma.Function(Df1 / 2) * Gamma.Function(Df2 / 2) * Math.Pow(Df1 * x + Df2, (Df1 + Df2) / 2);
                var result = numerator / denominator;

                return result;
            };
        }

        public FDistribution(double df1, double df2)
        {
            if (df1 <= 0 || df2 <= 0)
            {
                throw new Exception("Количество степеней свободы должно быть положительным!");
            }

            Df1 = df1;
            Df2 = df2;

            InitPiecewisePDF();
        }

        protected void InitPiecewisePDF()
        {
            PiecewisePDF = new PiecewiseFunction();

            if (Df1 < 2)
            {
                PiecewisePDF.AddSegment(new SegmentWithPole(0, 1, ProbabilityFunction, true));
                PiecewisePDF.AddSegment(new PlusInfinitySegment(1, ProbabilityFunction));
            }
            else if (Df1 == 2)
            {
                PiecewisePDF.AddSegment(new Segment(0, 1, ProbabilityFunction));
                PiecewisePDF.AddSegment(new PlusInfinitySegment(1, ProbabilityFunction));
            }
            else
            {
                var mode = (Df1 - 2) / Df1 * Df2 / (Df2 + 2);
                PiecewisePDF.AddSegment(new SegmentWithPole(0, mode, ProbabilityFunction, true));
                PiecewisePDF.AddSegment(new Segment(mode, mode + 1, ProbabilityFunction));
                PiecewisePDF.AddSegment(new PlusInfinitySegment(mode + 1, ProbabilityFunction));
            }
        }

        public override double GetNewRandomValue()
        {
            throw new NotImplementedException();
        }
    }
}
