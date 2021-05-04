using RandomVariablesLibraryNew.Distributions.Base;
using RandomVariablesLibraryNew.Segments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomVariablesLibraryNew.Distributions.Custom
{
    public class CustomDistribution : Distribution
    {
        public Point[] ProbabilityFunctionValues { get; set; }

        private Point[] DistributionFunctionValues { get; set; }

        private int NumberOfIntervals { get; }

        private double IntervalLength { get; }

        public double MinValue { get; }

        public double MaxValue { get; }

        private alglib.spline1dinterpolant InterpolantModel { get; set; }

        public CustomDistribution(double[] variableValues)
        {
            // Определяем кол-во интервалов для разбиения по формуле Стерджеса
            var dataCount = variableValues.Length;
            NumberOfIntervals = (int)Math.Ceiling(1 + 3.322 * Math.Log10(dataCount));
            //NumberOfIntervals = 100;

            MinValue = variableValues.Min();
            MaxValue = variableValues.Max();

            IntervalLength = (MaxValue - MinValue) / NumberOfIntervals;

            MinValue = MinValue + IntervalLength;
            MaxValue = MaxValue - IntervalLength;

            var breakPoints = GetBreakPoints();
            CalculateDistributionFunctionValues(variableValues, breakPoints);
            CalculateProbabilityFunctionValues(breakPoints);

            InitInterpolantModel();

            InitPiecewisePDF();
        }

        private double[] GetBreakPoints()
        {
            //var min = MinValue + IntervalLength;

            var breakPoints = new double[NumberOfIntervals];
            for (var i = 0; i < NumberOfIntervals; i++)
            {
                breakPoints[i] = MinValue + i * IntervalLength;
            }

            return breakPoints;
        }

        private void CalculateDistributionFunctionValues(double[] variableValues, double[] breakPoints)
        {
            var counts = new int[NumberOfIntervals];
            DistributionFunctionValues = new Point[NumberOfIntervals];
            for (var i = 0; i < breakPoints.Length; i++)
            {
                var currentValue = breakPoints[i];

                var count = variableValues.Where(v => v <= currentValue).Count();
                counts[i] = count;
                DistributionFunctionValues[i] = new Point(currentValue, (double)count / variableValues.Length);
            }

            //if (counts.Sum() != variableValues.Length)
            //{
            //    throw new Exception("Неверное вычисление количества попаданий СВ в интервалы!");
            //}
        }

        private void CalculateProbabilityFunctionValues(double[] breakPoints)
        {
            ProbabilityFunctionValues = new Point[breakPoints.Length - 1];

            for (var i = 0; i < breakPoints.Length - 1; i++)
            {
                var funcValue = (DistributionFunctionValues[i + 1].Y - DistributionFunctionValues[i].Y) / IntervalLength;
                ProbabilityFunctionValues[i] = new Point(breakPoints[i], funcValue);
            }
        }

        private void InitInterpolantModel()
        {
            var xData = ProbabilityFunctionValues.Select(p => p.X).ToArray();
            var yData = ProbabilityFunctionValues.Select(p => p.Y).ToArray();

            alglib.spline1dinterpolant spline1Dinterpolant;
            alglib.spline1dbuildcubic(xData, yData, out spline1Dinterpolant);
            InterpolantModel = spline1Dinterpolant;
        }

        private void InitPiecewisePDF()
        {
            PiecewisePDF = new PiecewiseFunction();

            Func<double, double> probabilityFunction = (x) => GetProbabilityFunctionValueAtPoint(x);

            if (MinValue < 0)
            {
                if (MaxValue <= 0)
                {
                    PiecewisePDF.AddSegment(new Segment(MinValue, MaxValue, probabilityFunction));
                }
                else
                {
                    PiecewisePDF.AddSegment(new Segment(MinValue, 0, probabilityFunction));
                    PiecewisePDF.AddSegment(new Segment(0, MaxValue, probabilityFunction));
                }
            }
            else
            {
                PiecewisePDF.AddSegment(new Segment(MinValue, MaxValue, probabilityFunction));
            }
        }

        private double GetProbabilityFunctionValueAtPoint(double x)
        {
            var existingPoint = ProbabilityFunctionValues.SingleOrDefault(p => p.X.Equals(x));
            if (existingPoint != null)
            {
                return existingPoint.Y;
            }

            var interpolatedValue = alglib.spline1dcalc(InterpolantModel, x);
            return interpolatedValue;
        }

        public override double GetNewRandomValue()
        {
            throw new NotImplementedException();
        }
    }
}
