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
                    integralValue += Integral(segment.A, segment.B, integralFunc);
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
                    integralValue += Integral(segment.A, segment.B, integralFunc);
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
                centralMoment += Integral(segment.A, segment.B, integralFunc);
            }

            return centralMoment;
        }

        #endregion

        public string SummaryInfo
        {
            get
            {
                //var stringBuilder = new StringBuilder();

                //stringBuilder.AppendLine($"Мат. ожидание: {Mean}");
                //stringBuilder.AppendLine($"Дисперсия: {Variance}");
                //stringBuilder.AppendLine($"Среднее квадратическое отклонение: {StandardDeviation}");
                //stringBuilder.AppendLine($"Коэффициент асимметрии: {Skewness}");
                //stringBuilder.AppendLine($"Эксцесс: {Kurtosis}");

                //return stringBuilder.ToString();

                return string.Empty;
            }
        }

        public void AddSegment(Segment segment)
        {
            Segments.Add(segment);
        }

        /// <summary>
        /// Вычисляет интеграл
        /// </summary>
        /// <param name="a">Начало отрезка интегрирования</param>
        /// <param name="b">Конец отрезка интегрирования</param>
        /// <param name="function">Подынтегральная функция</param>
        /// <returns></returns>
        public double Integral(double a, double b, Func<double, double> function)
        {
            var n = 6; // количество точек
            var weights = new double[]
            {
                0.171324492,
                0.360761573,
                0.467913935,
                0.467913935,
                0.360761573,
                0.171324492
            };

            var arguments = new double[]
            {
                -0.932469514,
                -0.661209386,
                -0.238619186,
                0.238619186,
                0.661209386,
                0.932469514
            };

            double integralValue = default;

            for (var i = 0; i < n; i++)
            {
                var transformedArg = (b + a) / 2 + ((b - a) / 2) * arguments[i];
                integralValue += weights[i] * function(transformedArg);
            }

            integralValue = ((b - a) / 2) * integralValue;

            return integralValue;
        }
    }
}
