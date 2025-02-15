﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomVariablesLibrary.Segments
{
    public class Segment
    {
        public double A { get; }

        public double B { get; }

        public virtual double SafeA => A;

        public virtual double SafeB => B;

        public Func<double, double> ProbabilityFunction { get; }

        public Segment(double a, double b, Func<double, double> probabilityFunction)
        {
            A = a;
            B = b;
            ProbabilityFunction = probabilityFunction;
        }

        /// <summary>
        /// Возвращает значение функции плотности в данной точке сегмента
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public double this[double x]
        {
            get
            {
                return ProbabilityFunction(x);
            }
        }

        public List<double> GetPoints(double? xMin, double? xMax, int numberOfPoints)
        {
            var leftPoint = FindLeftPoint();
            var rightPoint = FindRightPoint();

            xMin = xMin.HasValue
                ? Math.Max(xMin.Value, leftPoint)
                : leftPoint;

            xMax = xMax.HasValue
                ? Math.Min(xMax.Value, rightPoint)
                : rightPoint;

            if (xMin >= xMax)
            {
                return new List<double>();
            }

            var leftRightEpsilon = Math.Pow(10, -20);
            if (xMin == 0)
            {
                xMin = xMin + leftRightEpsilon;
            }
            if (xMax == 0)
            {
                xMax = xMax - leftRightEpsilon;
            }

            var args = new List<double>();
            if (this is SegmentWithPole segmentWithPole)
            {
                if (xMax / xMin > Math.Pow(10, 2) && segmentWithPole.LeftPole)
                {
                    args = LogSpace(Math.Log10(xMin.Value), Math.Log10(xMax.Value), numberOfPoints).ToList();
                }
                else if (xMin / xMax > Math.Pow(10, 2) && !segmentWithPole.LeftPole)
                {
                    args = LogSpace(Math.Log10(Math.Abs(xMin.Value)), Math.Log10(Math.Abs(xMax.Value)), numberOfPoints).ToList();
                    args = args.Select(arg => arg * (-1)).ToList();
                }
            }
            else
            {
                args = LinSpace(xMin.Value, xMax.Value, numberOfPoints).ToList();
            }

            return args;
        }

        public List<double> GetProbabilityFunctionValues(int numberOfPoints)
        {
            var pdfValues = new List<double>();

            var leftPoint = FindLeftPoint();
            var rightPoint = FindRightPoint();

            if (leftPoint >= rightPoint)
            {
                return new List<double>();
            }

            var step = (rightPoint - leftPoint) / (numberOfPoints - 1);

            for (var i = 0; i < numberOfPoints; i++)
            {
                var x = leftPoint + i * step;
                var y = ProbabilityFunction(x);
                pdfValues.Add(y);
            }

            return pdfValues;
        }

        public virtual double FindLeftPoint() => A;

        public virtual double FindRightPoint() => B;

        public virtual Segment ShiftAndScale(double shift, double scale)
        {
            var a = scale > 0 ? A : B;
            var b = scale > 0 ? B : A;

            var newA = a * scale + shift;
            var newB = b * scale + shift;

            Func<double, double> newProbabilityFunction = (x) =>
            {
                return Math.Abs(1 / scale) * ProbabilityFunction((x - shift) * 1 / scale);
            };

            var shiftedScaledSegment = new Segment(newA, newB, newProbabilityFunction);

            return shiftedScaledSegment;
        }

        private IEnumerable<double> LogSpace(double start, double stop, int num, bool endpoint = true, double numericBase = 10.0d)
        {
            var y = LinSpace(start, stop, num: num, endpoint: endpoint);
            return Power(y, numericBase);
        }

        private IEnumerable<double> Arange(double start, int count)
        {
            return Enumerable.Range((int)start, count).Select(v => (double)v);
        }

        private IEnumerable<double> Power(IEnumerable<double> exponents, double baseValue = 10.0d)
        {
            return exponents.Select(v => Math.Pow(baseValue, v));
        }

        private IEnumerable<double> LinSpace(double start, double stop, int num, bool endpoint = true)
        {
            var result = new List<double>();
            if (num <= 0)
            {
                return result;
            }

            if (endpoint)
            {
                if (num == 1)
                {
                    return new List<double>() { start };
                }

                var step = (stop - start) / ((double)num - 1.0d);
                result = Arange(0, num).Select(v => (v * step) + start).ToList();
            }
            else
            {
                var step = (stop - start) / (double)num;
                result = Arange(0, num).Select(v => (v * step) + start).ToList();
            }

            return result;
        }
    }
}
