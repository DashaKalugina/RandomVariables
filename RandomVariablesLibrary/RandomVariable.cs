using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomVariables
{
    public class RandomVariable
    {
        public DistributionBase _distribution;

        private double[] _variableValues;
        private double[] _probabilityValues;

        public Point[] Probabilities { get; private set; }

        public Point[] ProbabilityFunctionValues { get; private set; }

        public Point[] DistributionFunctionValues { get; private set; }

        private const int N = 100000;
        private const int n = 1000;

        public double[] GetVariableValues() => _variableValues;
        public double[] GetProbabilityValues() => _probabilityValues;

        public void SetVariableValues(double[] varValues)
        {
            _variableValues = varValues;
        }

        public void SetProbValues(double[] probValues)
        {
            _probabilityValues = probValues;
        }

        public RandomVariable(DistributionBase distribution)
        {
            _distribution = distribution;

            CalculateProbabilities();
            CalculateDistributionFunctionValues();
            CalculateProbabilityFunctionValues();
        }

        public RandomVariable(CustomDistribution customDistribution)
        {

        }

        //private void CalculateVariableAndProbabilityValues()
        //{
        //    var counts = new int[n+1];
        //    var variableValues = _distribution.GenerateRandomVariableValues(N);

        //    _probabilityValues = new double[n+1];
        //    _variableValues = new double[n+1];

        //    var min = variableValues.Min();
        //    var max = variableValues.Max();
        //    var intervalLength = (max - min) / n;

        //    foreach (var value in variableValues)
        //    {
        //        var index = (int) ((value - min) / intervalLength);

        //        counts[index]++;

        //        if (_variableValues[index] == 0)
        //        {
        //            _variableValues[index] = min + index * intervalLength;
        //        }
        //    }

        //    for (int i=0; i<_probabilityValues.Length; i++)
        //    {
        //        _probabilityValues[i] = (double)counts[i] / N;
        //    }

        //    var probabilitiesSum = _probabilityValues.Sum();
        //    if (Math.Abs(1 - probabilitiesSum) > Math.Pow(10,-6))
        //    {
        //        throw new Exception("Сумма вероятностей должна быть равна единице!");
        //    }
        //}

        private void CalculateProbabilities()
        {
            //var counts = new int[n + 1];
            

            _probabilityValues = new double[n + 1];
            _variableValues = new double[n + 1];

            Probabilities = new Point[n + 1];

            var variableValues = _distribution.GenerateRandomVariableValues(N);
            var min = variableValues.Min();
            var max = variableValues.Max();
            var intervalLength = (max - min) / n;

            var counts = new int[n + 1];
            foreach (var value in variableValues)
            {
                var index = (int)((value - min) / intervalLength);

                counts[index]++;
            }

            // убрать
            for (int i = 0; i < _probabilityValues.Length; i++)
            {
                _probabilityValues[i] = (double)counts[i] / N;
                if (_variableValues[i] == default)
                {
                    _variableValues[i] = min + i * intervalLength;
                }
            }

            for (int i = 0; i < Probabilities.Length; i++)
            {
                var variableValue = min + i * intervalLength;
                var probability = (double)counts[i] / N;

                Probabilities[i] = new Point(variableValue, probability);
            }

            //var probabilitiesSum = _probabilityValues.Sum();
            var probabilitiesSum = Probabilities.Select(p => p.Y).Sum();
            if (Math.Abs(1 - probabilitiesSum) > Math.Pow(10, -6))
            {
                throw new Exception("Сумма вероятностей должна быть равна единице!");
            }
        }

        private void CalculateProbabilityFunctionValues()
        {
            //var probValue = i > 0 && i < variableValues.Length - 1 && probabilities[i] != 0
            //        ? probabilities[i] / (variableValues[i + 1] - variableValues[i])
            //        : probabilities[i];

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
            var varValues1 = variable1.GetVariableValues();
            var varValues2 = variable2.GetVariableValues();

            var probValues1 = variable1.GetProbabilityValues();
            var probValues2 = variable2.GetProbabilityValues();

            var length1 = varValues1.Length;
            var length2 = varValues2.Length;

            var dict = new Dictionary<double, double>();

            for (var i=0; i<length1; i++)
            {
                for (var j=0; j<length2; j++)
                {
                    var newVariableValue = varValues1[i] + varValues2[j];
                    var probability = probValues1[i] * probValues2[j];

                    if (dict.ContainsKey(newVariableValue))
                    {
                        dict[newVariableValue] += probability;
                    } 
                    else
                    {
                        dict.Add(newVariableValue, probability);
                    }
                }
            }

            var (newVarValues, probabilityValues) = CalculateVariableAndProbabilityValues(dict.Keys.ToArray());

            var newVariable = new RandomVariable(new CustomDistribution());
            newVariable.SetVariableValues(newVarValues);
            newVariable.SetProbValues(probabilityValues);
            //newVariable.SetVariableValues(dict.Keys.ToArray());
            //newVariable.SetProbValues(dict.Values.ToArray());

            return newVariable;
        }

        private static (double[], double[]) CalculateVariableAndProbabilityValues(double[] variableValues)
        {
            var counts = new int[n + 1];

            var newVarValues = new double[n + 1];
            var probabilityValues = new double[n + 1];
            
            var min = variableValues.Min();
            var max = variableValues.Max();
            var intervalLength = (max - min) / n;

            foreach (var value in variableValues)
            {
                var index = (int)((value - min) / intervalLength);

                counts[index]++;

                if (newVarValues[index] == 0)
                {
                    newVarValues[index] = min + index * intervalLength;
                }
            }

            for (int i = 0; i < probabilityValues.Length; i++)
            {
                probabilityValues[i] = (double)counts[i] / variableValues.Length;
            }

            var probabilitiesSum = probabilityValues.Sum();
            if (Math.Abs(1 - probabilitiesSum) > Math.Pow(10, -6))
            {
                throw new Exception("Сумма вероятностей должна быть равна единице!");
            }

            return (newVarValues, probabilityValues);
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
