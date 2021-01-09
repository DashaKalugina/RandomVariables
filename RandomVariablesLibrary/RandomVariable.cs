using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomVariables
{
    public class RandomVariable
    {
        public DistributionBase _distribution;
        //private RandomVariableRange _variableRange;

        private double[] _variableValues;
        private double[] _probabilityValues;

        private const int N = 1000;

        public double[] GetVariableValues() => _variableValues;
        public double[] GetProbabilityValues() => _probabilityValues;

        public RandomVariable(DistributionBase distribution)
        {
            _distribution = distribution;
            //_variableRange = variableRange;

            CalculateVariableAndProbabilityValues();
        }

        public void CalculateVariableAndProbabilityValues()
        {
            //var count = (long)((_variableRange.To - _variableRange.From) / _variableRange.H);
            //_variableValues = new double[count];
            //_probabilityValues = new double[count];

            //for(int i=0; i<count; i++)
            //{
            //    var x = _variableRange.From + i * _variableRange.H;
            //    _variableValues[i] = x;
            //    _probabilityValues[i] = _distribution.ProbabilityFunction(x);
            //}

            _variableValues = _distribution.GenerateRandomVariableValues(N);
            _probabilityValues = new double[N];

            //var list = _variableValues.ToList();
            //list.Sort();
            //_variableValues = list.ToArray();

            for (int i=0; i<N-1; i++)
            {
                var Fx1 = _distribution.DistributionFunction(_variableValues[i]);
                var Fx2 = _distribution.DistributionFunction(_variableValues[i + 1]);
                _probabilityValues[i] = Fx2 - Fx1;
            }
            _probabilityValues[N - 1] = 1 - _probabilityValues.Sum();

            if (_probabilityValues.Sum() != 1)
            {
                throw new Exception("Сумма вероятностей не может быть больше единицы!");
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
