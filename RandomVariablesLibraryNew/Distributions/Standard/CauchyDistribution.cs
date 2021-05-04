using RandomVariablesLibraryNew.Distributions.Base;
using RandomVariablesLibraryNew.Segments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CauchyGenerator = Accord.Statistics.Distributions.Univariate.CauchyDistribution;

namespace RandomVariablesLibraryNew.Distributions.Standard
{
    public class CauchyDistribution : Distribution
    {
        /// <summary>
        /// Параметр масштаба, Gamma > 0
        /// </summary>
        public double Gamma { get; }

        /// <summary>
        /// Параметр сдвига
        /// </summary>
        public double Center { get; }

        public Func<double, double> ProbabilityFunction
        {
            get => (x) =>
            {
                return Gamma / (Math.PI * (Math.Pow(x - Center, 2) + Math.Pow(Gamma, 2)));
            };
        }

        public CauchyDistribution(double center, double gamma)
        {
            if (gamma <= 0)
            {
                throw new Exception("Значение параметра сдвига должно быть строго больше нуля!");
            }

            Gamma = gamma;
            Center = center;

            InitPiecewisePDF();
        }

        protected void InitPiecewisePDF()
        {
            PiecewisePDF = new PiecewiseFunction();

            var b1 = Center - Gamma;
            var b2 = Center + Gamma;

            PiecewisePDF.AddSegment(new MinusInfinitySegment(b1, ProbabilityFunction));
            PiecewisePDF.AddSegment(new Segment(b1, b2, ProbabilityFunction));
            PiecewisePDF.AddSegment(new PlusInfinitySegment(b2, ProbabilityFunction));
        }

        public override double GetNewRandomValue()
        {
            return CauchyGenerator.Random(Center, Gamma);
        }
    }
}
