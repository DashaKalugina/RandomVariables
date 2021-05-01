using RandomVariablesLibraryNew.Distributions.Base;
using System;

namespace RandomVariablesLibraryNew.Distributions.Custom
{
    public class ShiftedScaledDistribution : Distribution
    {
        public double Shift { get; }

        public double Scale { get; }

        public Distribution Distribution { get; }

        public ShiftedScaledDistribution(Distribution distribution, double shift = 0, double scale = 1)
        {
            if (scale == 0)
            {
                throw new Exception("Параметр scale не может быть равным нулю");
            }

            Distribution = distribution;

            Shift = shift;
            Scale = scale;

            InitPiecewisePDF();
        }

        protected void InitPiecewisePDF()
        {
            PiecewisePDF = Distribution.PiecewisePDF.GetShiftedAndScaledCopy(Shift, Scale);
        }

        public override double GetNewRandomValue()
        {
            return Scale * Distribution.GetNewRandomValue() + Shift;
        }
    }
}
