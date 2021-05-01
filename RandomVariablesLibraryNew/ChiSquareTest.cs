using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Statistics;
using Test = Accord.Statistics.Testing.ChiSquareTest;
using CenterSpace.NMath.Stats;
using RandomVariablesLibraryNew.Distributions;
using RandomVariablesLibraryNew.Distributions.Base;

namespace RandomVariablesLibraryNew
{
    public static class ChiSquareTest
    {
        public static bool Test(Distribution distribution)
        {
            var dataCount = 100000;
            var dataSampling = new List<double>();

            for (var i = 0; i < dataCount; i++)
            {
                dataSampling.Add(distribution.GetNewRandomValue());
            }
            dataSampling.Sort();

            // Определяем кол-во интервалов для разбиения по формуле Стерджеса
            var numberOfIntervals = (int)Math.Ceiling(1 + 3.322 * Math.Log10(dataCount));
            var (pdfValuesExpected, pdfValuesActual) = CalculateExpectedAndActualPDFValues(distribution, dataSampling.ToArray(), numberOfIntervals);

            var chiSquareTest = new Test(pdfValuesExpected, pdfValuesActual, numberOfIntervals);

            var significant = chiSquareTest.Significant;

            return !significant;
        }

        private static (double[], double[]) CalculateExpectedAndActualPDFValues(Distribution distribution, double[] variableValues, int numberOfIntervals)
        {
            var probabilities = new double[numberOfIntervals + 1];
            var pdfValuesExpected = new double[numberOfIntervals + 1];
            //var pdfValuesActual = new double[numberOfIntervals + 1];

            var min = variableValues.Min();
            var max = variableValues.Max();
            var intervalLength = (max - min) / numberOfIntervals;

            var counts = new int[numberOfIntervals + 1];
            foreach (var value in variableValues)
            {
                var index = (int)((value - min) / intervalLength);

                counts[index]++;
            }

            var variableValues1 = new double[numberOfIntervals + 1];
            for (int i = 0; i < probabilities.Length; i++)
            {
                var variableValue = min + i * intervalLength;
                variableValues1[i] = variableValue;
                probabilities[i] = (double)counts[i] / variableValues.Length;

                pdfValuesExpected[i] = distribution.GetPdfValueAtPoint(variableValue);
            }

            var probabilitiesSum = probabilities.Sum();
            if (Math.Abs(1 - probabilitiesSum) > Math.Pow(10, -6))
            {
                throw new Exception("Сумма вероятностей должна быть равна единице!");
            }

            var pdfValuesActual = CalculateProbabilityFunctionValues(probabilities, variableValues1, numberOfIntervals);

            return (pdfValuesExpected, pdfValuesActual);
        }

        private static double[] CalculateProbabilityFunctionValues(double[] probabilities, double[] variableValues, int numberOfIntervals)
        {
            var length = numberOfIntervals + 1;
            var probabilityFunctionValues = new double[length];

            for (var i = 0; i < length; i++)
            {
                var funcValue = i > 0 && i < length - 1 && probabilities[i] != 0
                    ? probabilities[i] / (variableValues[i + 1] - variableValues[i])
                    : probabilities[i];
                probabilityFunctionValues[i] = funcValue;
            }

            return probabilityFunctionValues;
        }
    }
}
