using RandomVariablesLibraryNew.ConvolutionCalculators;
using RandomVariablesLibraryNew.Distributions.Base;

namespace RandomVariablesLibraryNew.Distributions.Custom
{
    public class SumDistribution: Distribution
    {
        public Distribution Distr1 { get; set; }

        public Distribution Distr2 { get; set; }

        public SumDistribution(Distribution distr1, Distribution distr2)
        {
            Distr1 = distr1;
            Distr2 = distr2;

            InitPiecewisePDF();
        }

        protected void InitPiecewisePDF()
        {
            PiecewisePDF = SumConvolutionCalculator.Calculate(Distr1.PiecewisePDF, Distr2.PiecewisePDF);
        }

        public override double GetNewRandomValue()
        {
            return Distr1.GetNewRandomValue() + Distr2.GetNewRandomValue();
        }
    }
}
