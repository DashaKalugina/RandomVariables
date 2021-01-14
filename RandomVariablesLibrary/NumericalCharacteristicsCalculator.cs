using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomVariables
{
    static class NumericalCharacteristicsCalculator
    {
        /// <summary>
        /// Математическое ожидание
        /// </summary>
        /// <param name="variableValues"></param>
        /// <param name="probabilityValues"></param>
        /// <returns></returns>
        public static double GetMeanValue(Point[] probabilities)
        {
            double meanValue = default;
            foreach (var value in probabilities)
            {
                meanValue += value.X * value.Y;
            }
            return meanValue;
        }

        /// <summary>
        /// Несмещенная оценка дисперсии
        /// </summary>
        /// <param name="variableValues"></param>
        /// <param name="probabilityValues"></param>
        /// <returns></returns>
        public static double GetVariance(Point[] probabilities)
        {
            var meanValue = GetMeanValue(probabilities);

            double variance = default;
            foreach (var value in probabilities)
            {
                variance += value.Y * Math.Pow((value.X - meanValue), 2);
            }

            //return variance / (variableValues.Length - 1);
            return variance;
        }

        /// <summary>
        /// Среднее квадратическое отклонение
        /// </summary>
        /// <param name="variableValues"></param>
        /// <param name="probabilityValues"></param>
        /// <returns></returns>
        public static double GetStandardDeviation(Point[] probabilities)
        {
            var standardDeviation = Math.Pow(GetVariance(probabilities), 0.5);
            return standardDeviation;
        }

        /// <summary>
        /// Коэффициент асимметрии
        /// </summary>
        /// <param name="variableValues"></param>
        /// <param name="probabilityValues"></param>
        /// <returns></returns>
        public static double GetSkewness(Point[] probabilities)
        {
            var standardDeviation = GetStandardDeviation(probabilities);
            var skewness = GetCentralMoment(3, probabilities) / Math.Pow(standardDeviation, 3);

            return skewness;
        }

        /// <summary>
        /// Эксцесс
        /// </summary>
        /// <param name="variableValues"></param>
        /// <param name="probabilityValues"></param>
        /// <returns></returns>
        public static double GetKurtosis(Point[] probabilities)
        {
            var standardDeviation = GetStandardDeviation(probabilities);
            var skewness = GetCentralMoment(4, probabilities) / Math.Pow(standardDeviation, 4) - 3;

            return skewness;
        }

        /// <summary>
        /// Центральный момент q-го порядка СВ
        /// </summary>
        /// <param name="q"></param>
        /// <param name="variableValues"></param>
        /// <param name="probabilityValues"></param>
        /// <returns></returns>
        public static double GetCentralMoment(int q, Point[] probabilities)
        {
            double centralMoment = default;
            var meanValue = GetMeanValue(probabilities);

            foreach (var value in probabilities)
            {
                centralMoment += Math.Pow(value.X - meanValue, q) * value.Y;
            }

            return centralMoment;
        }
    }
}
