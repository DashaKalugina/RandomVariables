using System;
using System.Linq;
using System.Text;

namespace RandomVariables
{
    public class RandomVariable
    {
        public DistributionBase _distribution;

        private double[] _variableValues;
        private double[] _probabilityValues;

        private const int N = 100000;
        private const int n = 1000;

        public double[] GetVariableValues() => _variableValues;
        public double[] GetProbabilityValues() => _probabilityValues;

        public RandomVariable(DistributionBase distribution)
        {
            _distribution = distribution;

            CalculateVariableAndProbabilityValues();
        }

        public void CalculateVariableAndProbabilityValues()
        {
            var counts = new int[n+1];
            var variableValues = _distribution.GenerateRandomVariableValues(N);
            
            _probabilityValues = new double[n+1];
            _variableValues = new double[n+1];
            
            var min = variableValues.Min();
            var max = variableValues.Max();
            var intervalLength = (max - min) / n;

            foreach (var value in variableValues)
            {
                var index = (int) ((value - min) / intervalLength);

                counts[index]++;

                if (_variableValues[index] == 0)
                {
                    _variableValues[index] = min + index * intervalLength;
                }
            }
            
            for (int i=0; i<_probabilityValues.Length; i++)
            {
                _probabilityValues[i] = (double)counts[i] / N;
            }

            var probabilitiesSum = _probabilityValues.Sum();
            if (Math.Abs(1 - probabilitiesSum) > Math.Pow(10,-6))
            {
                throw new Exception("Сумма вероятностей должна быть равна единице!");
            }
        }

        public static RandomVariable operator +(RandomVariable variable1, RandomVariable variable2)
        {
            return null;
        }

        /// <summary>
        /// Вычисляет математическое ожидание
        /// </summary>
        public double Mean
        {
            get => NumericalCharacteristicsCalculator.GetMeanValue(_variableValues, _probabilityValues);
        }

        /// <summary>
        /// Вычисляет дисперсию
        /// </summary>
        public double Variance
        {
            get => NumericalCharacteristicsCalculator.GetVariance(_variableValues, _probabilityValues);
        }

        /// <summary>
        /// Вычисляет среднее квадратическое отклонение
        /// </summary>
        public double StandardDeviation
        {
            get => NumericalCharacteristicsCalculator.GetStandardDeviation(_variableValues, _probabilityValues);
        }

        /// <summary>
        /// Вычисляет коэффициент асимметрии
        /// </summary>
        public double Skewness
        {
            get => NumericalCharacteristicsCalculator.GetSkewness(_variableValues, _probabilityValues);
        }

        /// <summary>
        /// Вычисляет эксцесс
        /// </summary>
        public double Kurtosis
        {
            get => NumericalCharacteristicsCalculator.GetKurtosis(_variableValues, _probabilityValues);
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
    }
}
