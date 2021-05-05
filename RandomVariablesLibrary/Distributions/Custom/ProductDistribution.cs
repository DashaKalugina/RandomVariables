using RandomVariablesLibrary.ConvolutionCalculators;
using RandomVariablesLibrary.Distributions.Base;

namespace RandomVariablesLibrary.Distributions.Custom
{
    public class ProductDistribution: Distribution
    {
        public Distribution Distr1 { get; set; }

        public Distribution Distr2 { get; set; }

        public ProductDistribution(Distribution distr1, Distribution distr2)
        {
            Distr1 = distr1;
            Distr2 = distr2;

            InitPiecewisePDF();
        }

        protected void InitPiecewisePDF()
        {
            PiecewisePDF = ProductConvolutionCalculator.Calculate(Distr1.PiecewisePDF, Distr2.PiecewisePDF);
        }

        public override double GetNewRandomValue()
        {
            return Distr1.GetNewRandomValue() * Distr2.GetNewRandomValue();
        }
    }
}
