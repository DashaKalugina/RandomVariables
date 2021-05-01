using RandomVariablesLibraryNew.Distributions.Base;
using RandomVariablesLibraryNew.Generators;
using RandomVariablesLibraryNew.Segments;
using System;

namespace RandomVariablesLibraryNew.Distributions.Standard
{
    /// <summary>
    /// Класс нормального распределения
    /// </summary>
    public class NormalDistribution: Distribution
    {
        public double Mu { get; }
        public double Sigma { get; }

        public Func<double, double> ProbabilityFunction
        {
            get => (x) =>
            {
                return Math.Pow(Sigma * Math.Pow(2 * Math.PI, 0.5), -1) *
                Math.Pow(Math.E, (-1) * Math.Pow(x - Mu, 2) / (2 * Math.Pow(Sigma, 2)));
            };
        }

        public NormalDistribution(double mu, double sigma)
        {
            Mu = mu;
            Sigma = sigma;

            InitPiecewisePDF();
        }

        protected void InitPiecewisePDF()
        {
            PiecewisePDF = new PiecewiseFunction();

            var b1 = Mu - Sigma;
            var b2 = Mu + Sigma;

            PiecewisePDF.AddSegment(new MinusInfinitySegment(b1, ProbabilityFunction));
            PiecewisePDF.AddSegment(new Segment(b1, b2, ProbabilityFunction));
            PiecewisePDF.AddSegment(new PlusInfinitySegment(b2, ProbabilityFunction));
        }

        public override double GetNewRandomValue()
        {
            return NormalGenerator.Next(Mu, Sigma);
        }
    }
}
