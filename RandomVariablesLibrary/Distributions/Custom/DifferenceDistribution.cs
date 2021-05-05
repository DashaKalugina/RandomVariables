using RandomVariablesLibrary.ConvolutionCalculators;
using RandomVariablesLibrary.Distributions.Base;

namespace RandomVariablesLibrary.Distributions.Custom
{
    public class DifferenceDistribution: Distribution
    {
        public Distribution Distr1 { get; set; }

        public Distribution Distr2 { get; set; }

        public DifferenceDistribution(Distribution distr1, Distribution distr2)
        {
            Distr1 = distr1;
            Distr2 = distr2;

            InitPiecewisePDF();
        }

        protected void InitPiecewisePDF()
        {
            PiecewisePDF = SumConvolutionCalculator.Calculate(Distr1.PiecewisePDF,
                                                              Distr2.PiecewisePDF.GetShiftedAndScaledCopy(scale: -1));
        }

        public override double GetNewRandomValue()
        {
            return Distr1.GetNewRandomValue() - Distr2.GetNewRandomValue();
        }
    }
}
