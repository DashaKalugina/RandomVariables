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
        public static double GetMeanValue(double[] variableValues, double[] probabilityValues)
        {
            double meanValue = default;
            for (int i = 0; i < variableValues.Length; i++)
            {
                meanValue += variableValues[i] * probabilityValues[i];
            }
            return meanValue;
        }

        /// <summary>
        /// Несмещенная оценка дисперсии
        /// </summary>
        /// <param name="variableValues"></param>
        /// <param name="probabilityValues"></param>
        /// <returns></returns>
        public static double GetVariance(double[] variableValues, double[] probabilityValues)
        {
            var meanValue = GetMeanValue(variableValues, probabilityValues);
            double variance = default;
            for (int i = 0; i < variableValues.Length; i++)
            {
                variance += probabilityValues[i] * Math.Pow((variableValues[i] - meanValue), 2);
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
        public static double GetStandardDeviation(double[] variableValues, double[] probabilityValues)
        {
            var standardDeviation = Math.Pow(GetVariance(variableValues, probabilityValues), 0.5);
            return standardDeviation;
        }

        /// <summary>
        /// Коэффициент асимметрии
        /// </summary>
        /// <param name="variableValues"></param>
        /// <param name="probabilityValues"></param>
        /// <returns></returns>
        public static double GetSkewness(double[] variableValues, double[] probabilityValues)
        {
            var standardDeviation = GetStandardDeviation(variableValues, probabilityValues);
            var skewness = GetCentralMoment(3, variableValues, probabilityValues) / Math.Pow(standardDeviation, 3);

            return skewness;
        }

        /// <summary>
        /// Эксцесс
        /// </summary>
        /// <param name="variableValues"></param>
        /// <param name="probabilityValues"></param>
        /// <returns></returns>
        public static double GetKurtosis(double[] variableValues, double[] probabilityValues)
        {
            var standardDeviation = GetStandardDeviation(variableValues, probabilityValues);
            var skewness = GetCentralMoment(4, variableValues, probabilityValues) / Math.Pow(standardDeviation, 4) - 3;

            return skewness;
        }

        /// <summary>
        /// Центральный момент q-го порядка СВ
        /// </summary>
        /// <param name="q"></param>
        /// <param name="variableValues"></param>
        /// <param name="probabilityValues"></param>
        /// <returns></returns>
        public static double GetCentralMoment(int q, double[] variableValues, double[] probabilityValues)
        {
            double centralMoment = default;
            var meanValue = GetMeanValue(variableValues, probabilityValues);

            for (int i=0; i<variableValues.Length; i++)
            {
                centralMoment += Math.Pow(variableValues[i] - meanValue, q) * probabilityValues[i];
            }
            return centralMoment;
        }
    }
}
