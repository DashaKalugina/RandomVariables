using RandomVariablesLibrary.Segments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomVariablesLibrary
{
    /// <summary>
    /// Класс используется для представления функции плотности 
    /// в виде кусочно-заданной функции (набора сегментов).
    /// </summary>
    public class PiecewiseFunction
    {
        public List<Segment> Segments { get; private set; }

        public PiecewiseFunction()
        {
            Segments = new List<Segment>();
        }

        public void AddSegment(Segment segment)
        {
            var index = Segments.FindIndex(s => segment.B <= s.A);
            if (index == -1)
            {
                Segments.Add(segment);
            }
            else
            {
                Segments.Insert(index, segment);
            }
        }

        public double this[double x]
        {
            get
            {
                var segment = Segments.SingleOrDefault(s => x >= s.A && x < s.B);
                if (segment != null)
                {
                    return segment[x];
                }

                return default;
            }
        }

        public PiecewiseFunction GetShiftedAndScaledCopy(double shift = 0, double scale = 1)
        {
            var shiftedScaledFunction = new PiecewiseFunction();

            foreach(var segment in Segments)
            {
                shiftedScaledFunction.AddSegment(segment.ShiftAndScale(shift, scale));
            }

            return shiftedScaledFunction;
        }

        public List<BreakPoint> GetBreakPointsExtended()
        {
            if (Segments.Count == 0)
            {
                return new List<BreakPoint>();
            }

            var breakPoints = new List<BreakPoint>();

            var firstBreak = new BreakPoint(Segments[0].A, false, false);
            breakPoints.Add(firstBreak);

            foreach(var segment in Segments)
            {
                // сюда будет добавлена обработка полюсов и бесконечностей
                if (segment is SegmentWithPole segmentWithPole1 && segmentWithPole1.LeftPole)
                {
                    firstBreak.PositivePole = true;
                }
                if (segment is SegmentWithPole segmentWithPole2 && !segmentWithPole2.LeftPole)
                {
                    breakPoints.Add(new BreakPoint(segment.B, true, false));
                }
                else
                {
                    breakPoints.Add(new BreakPoint(segment.B, false, false));
                }
            }

            var firstSegment = Segments.First();
            var isFirstSegmentWithLeftPole = firstSegment is SegmentWithPole firstSegmentWithPole
                                        && firstSegmentWithPole.LeftPole;

            var continuityEps = Math.Pow(10, 2) * double.Epsilon; // params.pole_detection.continuity_eps

            if (!double.IsInfinity(firstSegment.A) && (isFirstSegmentWithLeftPole || firstSegment[firstSegment.A] > continuityEps))
            {
                breakPoints[0].Сontinuous = false;
            }

            var lastSegment = Segments.Last();
            var isLastSegmentWithRightPole = lastSegment is SegmentWithPole lastSegmentWithPole
                                        && !lastSegmentWithPole.LeftPole;
            if (!double.IsInfinity(lastSegment.B) && (isLastSegmentWithRightPole || lastSegment[lastSegment.B] > continuityEps))
            {
                breakPoints[0].Сontinuous = false;
            }

            var segmentsWithoutFirst = Segments.GetRange(1, Segments.Count - 1);
            for (var i = 1; i < segmentsWithoutFirst.Count; i++)
            {
                var segi = segmentsWithoutFirst[i];

                var isSegmentWithRightPole = Segments[i] is SegmentWithPole segWithPole1 && !segWithPole1.LeftPole;
                var isCurrentSegmentWithLeftPole = segi is SegmentWithPole segWithPole2 && segWithPole2.LeftPole;

                if (!isSegmentWithRightPole && !isCurrentSegmentWithLeftPole)
                {
                    if (Math.Abs(Segments[i][Segments[i].B] - segi[segi.A]) > continuityEps)
                    {
                        breakPoints[i + 1].Сontinuous = false;
                    }
                }
            }

            return breakPoints;
        }

        public List<double> GetBreakPoints()
        {
            if (Segments.Count == 0)
            {
                return new List<double>();
            }

            var breakPoints = new List<double>();

            foreach(var segment in Segments)
            {
                if (!breakPoints.Contains(segment.A))
                {
                    breakPoints.Add(segment.A);
                }

                if (!breakPoints.Contains(segment.B))
                {
                    breakPoints.Add(segment.B);
                }
            }

            return breakPoints;
        }

        public PiecewiseFunction SplitByPoints(List<double> points)
        {
            var splittedFunction = new PiecewiseFunction();

            foreach(var segment in Segments)
            {
                var inds = points.Where(p => p > segment.A && p < segment.B);

                var a = segment.A;
                double b;

                foreach(var ind in inds)
                {
                    b = ind;
                    if (segment is MinusInfinitySegment && double.IsInfinity(a))
                    {
                        splittedFunction.AddSegment(new MinusInfinitySegment(b, segment.ProbabilityFunction));
                    }
                    else if (segment is PlusInfinitySegment && double.IsInfinity(b))
                    {
                        splittedFunction.AddSegment(new PlusInfinitySegment(a, segment.ProbabilityFunction));
                    }
                    // добавить обработку полюсов
                    else if (segment is SegmentWithPole segmentWithPole)
                    {
                        if (segmentWithPole.LeftPole && a == segment.A)
                        {
                            splittedFunction.AddSegment(new SegmentWithPole(a, b, segment.ProbabilityFunction, true));
                        }
                        else if (!segmentWithPole.LeftPole && b == segment.B)
                        {
                            splittedFunction.AddSegment(new SegmentWithPole(a, b, segment.ProbabilityFunction, false));
                        }
                    }
                    else
                    {
                        splittedFunction.AddSegment(new Segment(a, b, segment.ProbabilityFunction));
                    }

                    a = b;
                }

                b = segment.B;

                if (segment is MinusInfinitySegment && double.IsInfinity(a))
                {
                    splittedFunction.AddSegment(new MinusInfinitySegment(b, segment.ProbabilityFunction));
                }
                else if (segment is PlusInfinitySegment && double.IsInfinity(b))
                {
                    splittedFunction.AddSegment(new PlusInfinitySegment(a, segment.ProbabilityFunction));
                }
                // добавить обработку полюсов
                else if (segment is SegmentWithPole segmentWithPole)
                {
                    if (segmentWithPole.LeftPole && a == segment.A)
                    {
                        splittedFunction.AddSegment(new SegmentWithPole(a, b, segment.ProbabilityFunction, true));
                    }
                    else if (!segmentWithPole.LeftPole && b == segment.B)
                    {
                        splittedFunction.AddSegment(new SegmentWithPole(a, b, segment.ProbabilityFunction, false));
                    }
                }
                else
                {
                    splittedFunction.AddSegment(new Segment(a, b, segment.ProbabilityFunction));
                }
            }

            return splittedFunction;
        }

        #region Characteristics and Summary Info

        /// <summary>
        /// Вычисляет математическое ожидание
        /// </summary>
        public double Mean
        {
            get
            {
                double integralValue = default;

                foreach (var segment in Segments)
                {
                    Func<double, double> integralFunc = (x) => x * segment[x];
                    integralValue += IntegralCalculator.Integrate(segment.A, segment.B, integralFunc);
                }

                return integralValue;
            }
        }

        /// <summary>
        /// Вычисляет дисперсию
        /// </summary>
        public double Variance
        {
            get
            {
                var mean = Mean;

                double integralValue = default;
                foreach (var segment in Segments)
                {
                    Func<double, double> integralFunc = (x) => Math.Pow(x - mean, 2) * segment[x];
                    integralValue += IntegralCalculator.Integrate(segment.A, segment.B, integralFunc);
                }

                return integralValue;
            }
        }

        /// <summary>
        /// Вычисляет среднее квадратическое отклонение
        /// </summary>
        public double StandardDeviation
        {
            get => Math.Sqrt(Variance);
        }

        /// <summary>
        /// Вычисляет коэффициент асимметрии
        /// </summary>
        public double Skewness
        {
            get => GetCentralMoment(3) / Math.Pow(StandardDeviation, 3);
        }

        /// <summary>
        /// Вычисляет эксцесс
        /// </summary>
        public double Kurtosis
        {
            get => GetCentralMoment(4) / Math.Pow(StandardDeviation, 4) - 3;
        }

        public double GetCentralMoment(int q)
        {
            var mean = Mean;

            double centralMoment = default;
            foreach (var segment in Segments)
            {
                Func<double, double> integralFunc = (x) => Math.Pow(x - mean, q) * segment[x];
                centralMoment += IntegralCalculator.Integrate(segment.A, segment.B, integralFunc);
            }

            return centralMoment;
        }

        public string SummaryInfo
        {
            get
            {
                var stringBuilder = new StringBuilder();

                stringBuilder.AppendLine($"Мат. ожидание: {Mean}");
                stringBuilder.AppendLine($"Дисперсия: {Variance}");
                stringBuilder.AppendLine($"Среднее квадратическое отклонение: {StandardDeviation}");
                stringBuilder.AppendLine($"Коэффициент асимметрии: {Skewness}");
                stringBuilder.AppendLine($"Эксцесс: {Kurtosis}");

                return stringBuilder.ToString();
            }
        }

        #endregion

    }
}
