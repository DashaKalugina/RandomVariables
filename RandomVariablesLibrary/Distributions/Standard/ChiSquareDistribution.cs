using Accord.Math;
using RandomVariablesLibrary.Distributions.Base;
using RandomVariablesLibrary.Segments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomVariablesLibrary.Distributions.Standard
{
    public class ChiSquareDistribution : Distribution
    {
        /// <summary>
        /// Число степеней свободы
        /// </summary>
        public double DegreesOfFreedom { get; }

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
                    if (DegreesOfFreedom == 1)
                    {
                        return double.PositiveInfinity;
                    }
                    if (DegreesOfFreedom == 2)
                    {
                        return 0.5;
                    }

                    return 0;
                }

                var product = Math.Pow(0.5, DegreesOfFreedom / 2) * Math.Pow(x, DegreesOfFreedom / 2 - 1) * Math.Exp((-1) * x / 2);
                return product / Gamma.Function(DegreesOfFreedom / 2);
            };
        }

        public ChiSquareDistribution(double degreesOfFreedom)
        {
            if (degreesOfFreedom <= 0)
            {
                throw new Exception("Количество степеней свободы должно быть положительным!");
            }

            DegreesOfFreedom = degreesOfFreedom;

            InitPiecewisePDF();
        }

        protected void InitPiecewisePDF()
        {
            PiecewisePDF = new PiecewiseFunction();

            if (DegreesOfFreedom >= 1 && DegreesOfFreedom <= 20)
            {
                PiecewisePDF.AddSegment(new SegmentWithPole(0, DegreesOfFreedom / 2, ProbabilityFunction, true));
                PiecewisePDF.AddSegment(new Segment(DegreesOfFreedom / 2, DegreesOfFreedom * 2, ProbabilityFunction));
                PiecewisePDF.AddSegment(new PlusInfinitySegment(DegreesOfFreedom * 2, ProbabilityFunction));
            }
            else
            {
                PiecewisePDF.AddSegment(new SegmentWithPole(0, DegreesOfFreedom * 0.75, ProbabilityFunction, true));
                PiecewisePDF.AddSegment(new Segment(DegreesOfFreedom * 0.75, DegreesOfFreedom * 4 / 3, ProbabilityFunction));
                PiecewisePDF.AddSegment(new PlusInfinitySegment(DegreesOfFreedom * 4 / 3, ProbabilityFunction));
            }
        }

        public override double GetNewRandomValue()
        {
            throw new NotImplementedException();
        }
    }
}
