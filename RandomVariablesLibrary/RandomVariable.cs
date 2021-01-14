using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomVariables
{
    public class RandomVariable
    {
        public DistributionBase _distribution;

        public Point[] Probabilities { get; private set; }

        public Point[] ProbabilityFunctionValues { get; private set; }

        public Point[] DistributionFunctionValues { get; private set; }

        private const int N = 1000000;
        private const int n = 1000;

        public RandomVariable(DistributionBase distribution)
        {
            _distribution = distribution;

            var variableValues = _distribution.GenerateRandomVariableValues(N);
            CalculateProbabilities(variableValues);

            CalculateDistributionFunctionValues();
            CalculateProbabilityFunctionValues();
        }

        public RandomVariable(CustomDistribution customDistribution, double[] variableValues)
        {
            CalculateProbabilities(variableValues);

            CalculateDistributionFunctionValues();
            CalculateProbabilityFunctionValues();
        }

        private void CalculateProbabilities(double[] variableValues)
        {
            Probabilities = new Point[n + 1];

            var min = variableValues.Min();
            var max = variableValues.Max();
            var intervalLength = (max - min) / n;

            var counts = new int[n + 1];
            foreach (var value in variableValues)
            {
                var index = (int)((value - min) / intervalLength);

                counts[index]++;
            }

            for (int i = 0; i < Probabilities.Length; i++)
            {
                var variableValue = min + i * intervalLength;
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

            var length = n + 1;
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
            var length = n + 1;
            DistributionFunctionValues = new Point[length];

            var sum = 0.0;
            for (var i = 0; i < length; i++)
            {
                var distributionFuncValue = i == 0 ? 0 : i == length - 1 ? 1 : sum;

                DistributionFunctionValues[i] = new Point(Probabilities[i].X, distributionFuncValue);

                sum += Probabilities[i].Y;
            }
        }

        public static RandomVariable operator +(RandomVariable variable1, RandomVariable variable2)
        {
            var probabilities1 = variable1.Probabilities;
            var probabilities2 = variable2.Probabilities;

            var newVariableValues = new HashSet<double>();

            foreach (var prob1 in probabilities1)
            {
                foreach(var prob2 in probabilities2)
                {
                    var newValue = prob1.X + prob2.X;
                    if (!newVariableValues.Contains(newValue))
                    {
                        newVariableValues.Add(newValue);
                    }
                }
            }

            var newVariable = new RandomVariable(new CustomDistribution(), newVariableValues.ToArray());

            return newVariable;
        }

        /// <summary>
        /// Вычисляет математическое ожидание
        /// </summary>
        public double Mean
        {
            get => NumericalCharacteristicsCalculator.GetMeanValue(Probabilities);
        }

        /// <summary>
        /// Вычисляет дисперсию
        /// </summary>
        public double Variance
        {
            get => NumericalCharacteristicsCalculator.GetVariance(Probabilities);
        }

        /// <summary>
        /// Вычисляет среднее квадратическое отклонение
        /// </summary>
        public double StandardDeviation
        {
            get => NumericalCharacteristicsCalculator.GetStandardDeviation(Probabilities);
        }

        /// <summary>
        /// Вычисляет коэффициент асимметрии
        /// </summary>
        public double Skewness
        {
            get => NumericalCharacteristicsCalculator.GetSkewness(Probabilities);
        }

        /// <summary>
        /// Вычисляет эксцесс
        /// </summary>
        public double Kurtosis
        {
            get => NumericalCharacteristicsCalculator.GetKurtosis(Probabilities);
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
