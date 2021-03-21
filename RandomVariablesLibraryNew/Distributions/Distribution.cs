using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomVariablesLibraryNew.Distributions
{
    public abstract class Distribution
    {
        /// <summary>
        /// Кусочно-заданная функция плотности распределения
        /// </summary>
        public PiecewiseFunction PiecewisePDF;

        public double Mean => PiecewisePDF.Mean;

        public double Variance => PiecewisePDF.Variance;

        public double StandardDeviation => PiecewisePDF.StandardDeviation;

        public double Skewness => PiecewisePDF.Skewness;

        public double Kurtosis => PiecewisePDF.Kurtosis;

        public static SumDistribution operator +(Distribution distr1, Distribution distr2)
        {

            return new SumDistribution(distr1, distr2);
        }
    }
}
