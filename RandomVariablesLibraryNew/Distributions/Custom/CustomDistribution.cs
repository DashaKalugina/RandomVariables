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
        private Point[] Probabilities { get; set; }

        public Point[] ProbabilityFunctionValues { get; set; }

        private Point[] DistributionFunctionValues { get; set; }

        private int NumberOfIntervals { get; }

        public double MinValue { get; }

        public double MaxValue { get; }

        private alglib.spline1dinterpolant InterpolantModel { get; set; }

        public CustomDistribution(double[] variableValues)
        {
            // Определяем кол-во интервалов для разбиения по формуле Стерджеса
            var dataCount = variableValues.Length;
            //NumberOfIntervals = (int)Math.Ceiling(1 + 3.322 * Math.Log10(dataCount));
            NumberOfIntervals = 100;

            MinValue = variableValues.Min();
            MaxValue = variableValues.Max();

            CalculateProbabilities(variableValues);

            CalculateDistributionFunctionValues();
            CalculateProbabilityFunctionValues();

            InitInterpolantModel();

            ConstructPiecewisePDF(variableValues);
        }

        private void CalculateProbabilities(double[] variableValues)
        {
            Probabilities = new Point[NumberOfIntervals + 1];

            var intervalLength = (MaxValue - MinValue) / NumberOfIntervals;

            var counts = new int[NumberOfIntervals + 1];
            foreach (var value in variableValues)
            {
                var index = (int)((value - MinValue) / intervalLength);

                counts[index]++;
            }

            for (int i = 0; i < Probabilities.Length; i++)
            {
                var variableValue = MinValue + i * intervalLength;
                var probability = (double)counts[i] / variableValues.Length;

                Probabilities[i] = new Point(variableValue, probability);
            }

            var probabilitiesSum = Probabilities.Select(p => p.Y).Sum();
            if (Math.Abs(1 - probabilitiesSum) > Math.Pow(10, -6))
            {
                throw new Exception("Сумма вероятностей должна быть равна единице!");
            }
        }

        private void CalculateProbabilityFunctionValues()
        {
            // пересмотреть вычисление на интервалах

            var length = NumberOfIntervals + 1;
            ProbabilityFunctionValues = new Point[length];

            for (var i = 0; i < length; i++)
            {
                var funcValue = i > 0 && i < length - 1 && Probabilities[i].Y != 0
                    ? Probabilities[i].Y / (Probabilities[i + 1].X - Probabilities[i].X)
                    : Probabilities[i].Y;
                ProbabilityFunctionValues[i] = new Point(Probabilities[i].X, funcValue);
            }
        }

        private void CalculateDistributionFunctionValues()
        {
            var length = NumberOfIntervals + 1;
            DistributionFunctionValues = new Point[length];

            var sum = 0.0;
            for (var i = 0; i < length; i++)
            {
                var distributionFuncValue = i == 0 ? 0 : i == length - 1 ? 1 : sum;

                DistributionFunctionValues[i] = new Point(Probabilities[i].X, distributionFuncValue);

                sum += Probabilities[i].Y;
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

        private void ConstructPiecewisePDF(double[] variableValues)
        {
            PiecewisePDF = new PiecewiseFunction();

            Func<double, double> probabilityFunction = (x) => GetProbabilityFunctionValueAtPoint(x);

            var minValue = variableValues.Min();
            var maxValue = variableValues.Max();

            if (minValue < 0)
            {
                if (maxValue <= 0)
                {
                    PiecewisePDF.AddSegment(new Segment(minValue, maxValue, probabilityFunction));
                }
                else
                {
                    PiecewisePDF.AddSegment(new Segment(minValue, 0, probabilityFunction));
                    PiecewisePDF.AddSegment(new Segment(0, maxValue, probabilityFunction));
                }
            }
            else
            {
                PiecewisePDF.AddSegment(new Segment(minValue, maxValue, probabilityFunction));
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
