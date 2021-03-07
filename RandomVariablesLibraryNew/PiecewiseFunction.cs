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

        public double Integral(double a, double b, Func<double, double> function)
        {
            return default;
        }

        /// <summary>
        /// Вычисляет дисперсию
        /// </summary>
        public double Variance
        {
            get => default;
        }

        /// <summary>
        /// Вычисляет среднее квадратическое отклонение
        /// </summary>
        public double StandardDeviation
        {
            get => default;
        }

        /// <summary>
        /// Вычисляет коэффициент асимметрии
        /// </summary>
        public double Skewness
        {
            get => default;
        }

        /// <summary>
        /// Вычисляет эксцесс
        /// </summary>
        public double Kurtosis
        {
            get => default;
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
    }
}
