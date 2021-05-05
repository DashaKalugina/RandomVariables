using RandomVariablesLibrary.Distributions.Custom;
using System.Collections.Generic;
using System.Linq;

namespace RandomVariablesLibrary.Distributions.Base
{
    public abstract class Distribution
    {
        /// <summary>
        /// Кусочно-заданная функция плотности распределения
        /// </summary>
        public PiecewiseFunction PiecewisePDF;

        #region Characteristics and Summary

        public double Mean => PiecewisePDF.Mean;

        public double Variance => PiecewisePDF.Variance;

        public double StandardDeviation => PiecewisePDF.StandardDeviation;

        public double Skewness => PiecewisePDF.Skewness;

        public double Kurtosis => PiecewisePDF.Kurtosis;

        public string SummaryInfo => PiecewisePDF.SummaryInfo;

        #endregion

        public Distribution()
        {
            //InitPiecewisePDF();
        }

        public List<Point> GetPDFDataForPlot(double? xMin = null, double? xMax = null, int numberOfPoints = 1000)
        {
            var resultPoints = new List<Point>();

            if (!xMin.HasValue)
            {
                xMin = PiecewisePDF.Segments.First().FindLeftPoint();
            }

            if (!xMax.HasValue)
            {
                xMax = PiecewisePDF.Segments.Last().FindRightPoint();
            }

            //var segments = PiecewisePDF.Segments.OrderBy(s => s.A);
            foreach(var segment in PiecewisePDF.Segments)
            {
                var args = segment.GetPoints(xMin, xMax, numberOfPoints);

                var points = new List<Point>();
                foreach (var x in args)
                {
                    var y = segment[x];
                    points.Add(new Point(x, y));
                }

                resultPoints.AddRange(points);
            }

            return resultPoints;
        }

        public List<Point> GetCDFDataForPlot(double? xMin = null, double? xMax = null, int numberOfPoints = 1000)
        {
            var resultPoints = new List<Point>();

            if (!xMin.HasValue)
            {
                xMin = PiecewisePDF.Segments.First().FindLeftPoint();
            }

            if (!xMax.HasValue)
            {
                xMax = PiecewisePDF.Segments.Last().FindRightPoint();
            }

            //var segments = PiecewisePDF.Segments.OrderBy(s => s.A);
            var integralValue = default(double);
            var leftValue = double.NegativeInfinity;
            foreach (var segment in PiecewisePDF.Segments)
            {
                var args = segment.GetPoints(xMin, xMax, numberOfPoints);

                var points = new List<Point>();
                foreach (var arg in args)
                {
                    integralValue += IntegralCalculator.Integrate(leftValue, arg, (x) => segment[x]);
                    points.Add(new Point(arg, integralValue));

                    leftValue = arg;
                }

                resultPoints.AddRange(points);
            }

            return resultPoints;
        }

        public List<double> GetProbabilityFunctionValues(int numberOfPointsBySegment = 100000)
        {
            var result = new List<double>();

            foreach (var segment in PiecewisePDF.Segments)
            {
                var segmentPoints = segment.GetProbabilityFunctionValues(numberOfPointsBySegment);
                result.AddRange(segmentPoints);
            }

            return result;
        }

        public double GetPdfValueAtPoint(double point)
        {
            var segment = PiecewisePDF.Segments.SingleOrDefault(s => point >= s.A && point <= s.B);
            if (segment!= null)
            {
                return segment[point];
            }

            return default;
        }

        public abstract double GetNewRandomValue();

        //protected abstract void InitPiecewisePDF();

        #region Override Operators

        public static SumDistribution operator +(Distribution distr1, Distribution distr2)
        {
            return new SumDistribution(distr1, distr2);
        }

        public static DifferenceDistribution operator -(Distribution distr1, Distribution distr2)
        {
            return new DifferenceDistribution(distr1, distr2);
        }

        public static ProductDistribution operator *(Distribution distr1, Distribution distr2)
        {
            return new ProductDistribution(distr1, distr2);
        }

        public static QuotientDistribution operator /(Distribution distr1, Distribution distr2)
        {
            return new QuotientDistribution(distr1, distr2);
        }

        public static ShiftedScaledDistribution operator +(Distribution distribution, double number)
        {
            return new ShiftedScaledDistribution(distribution, shift: number);
        }

        public static ShiftedScaledDistribution operator -(Distribution distribution, double number)
        {
            return new ShiftedScaledDistribution(distribution, shift: (-1) * number);
        }

        public static Distribution operator *(Distribution distribution, double number)
        {
            if (number == 1)
            {
                return distribution;
            }

            return new ShiftedScaledDistribution(distribution, scale: number);
        }

        public static Distribution operator /(Distribution distribution, double number)
        {
            if (number == 1)
            {
                return distribution;
            }

            return new ShiftedScaledDistribution(distribution, scale: 1 / number);
        }

        #endregion
    }
}
