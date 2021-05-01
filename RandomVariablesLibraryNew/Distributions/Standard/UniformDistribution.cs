using RandomVariablesLibraryNew.Distributions.Base;
using RandomVariablesLibraryNew.Generators;
using RandomVariablesLibraryNew.Segments;
using System;

namespace RandomVariablesLibraryNew.Distributions.Standard
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

            InitPiecewisePDF();
        }

        protected void InitPiecewisePDF()
        {
            PiecewisePDF = new PiecewiseFunction();
            PiecewisePDF.AddSegment(new Segment(A, B, ProbabilityFunction));
        }

        public override double GetNewRandomValue()
        {
            return UniformGenerator.Next(A, B);
        }
    }
}
