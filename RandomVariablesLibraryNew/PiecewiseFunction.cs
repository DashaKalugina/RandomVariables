using RandomVariablesLibraryNew.Segments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomVariablesLibraryNew
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
            Segments.Add(segment);
        }

        public List<BreakPoint> GetBreakPointsExtended()
        {
            if (Segments.Count == 0)
            {
                return new List<BreakPoint>();
            }

            var breakPoints = new List<BreakPoint>();

            breakPoints.Add(new BreakPoint(Segments[0].A, false, false));
            foreach(var segment in Segments)
            {
                // сюда будет добавлена обработка полюсов и бесконечностей
                breakPoints.Add(new BreakPoint(segment.B, false, false));
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
                else
                {
                    splittedFunction.AddSegment(new Segment(a, b, segment.ProbabilityFunction));
                }
            }

            return splittedFunction;
        }

        #region Characteristics

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

        #endregion

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

    }
}
