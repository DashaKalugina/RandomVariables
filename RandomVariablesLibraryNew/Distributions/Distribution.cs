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

        public static ProductDistribution operator *(Distribution distr1, Distribution distr2)
        {
            return new ProductDistribution(distr1, distr2);
        }

        public string SummaryInfo => PiecewisePDF.SummaryInfo;

        public List<Point> GetPDFDataForPlot(double xMin, double xMax, int numberOfPoints = 1000)
        {
            var resultPoints = new List<Point>();

            foreach(var segment in PiecewisePDF.Segments)
            {
                var segmentPoints = segment.GetPoints(xMin, xMax, numberOfPoints);
                resultPoints.AddRange(segmentPoints);
            }

            return resultPoints;
        }

        public List<double> GetProbabilityFunctionValues(int numberOfPoints = 1000)
        {
            var result = new List<double>();

            foreach (var segment in PiecewisePDF.Segments)
            {
                var segmentPoints = segment.GetProbabilityFunctionValues(numberOfPoints);
                result.AddRange(segmentPoints);
            }

            return result;
        }

        public abstract double GetNewRandomValue();
    }
}
