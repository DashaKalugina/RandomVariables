using System;
using System.Collections.Generic;
using System.Linq;
using Test = Accord.Statistics.Testing.ChiSquareTest;
using RandomVariablesLibrary.Distributions.Base;

namespace RandomVariablesLibrary
{
    public static class ChiSquareTest
    {
        public static bool Test(Distribution distribution)
        {
            var dataCount = 10000000;
            var dataSampling = new List<double>();

            for (var i = 0; i < dataCount; i++)
            {
                dataSampling.Add(distribution.GetNewRandomValue());
            }
            dataSampling.Sort();

            // Определяем кол-во интервалов для разбиения по формуле Стерджеса
            var numberOfIntervals = (int)Math.Ceiling(1 + 3.322 * Math.Log10(dataCount));
            numberOfIntervals = 100;
            var (pdfValuesExpected, pdfValuesActual) = CalculateExpectedAndActualPDFValues(distribution, dataSampling.ToArray());

            var chiSquareTest = new Test(pdfValuesExpected, pdfValuesActual, numberOfIntervals-1);

            var significant = chiSquareTest.Significant;

            return !significant;
        }

        public static (double[], double[]) CalculateExpectedAndActualPDFValues(Distribution distribution, double[] variableValues)
        {
            var numberOfIntervals = (int)Math.Ceiling(1 + 3.322 * Math.Log10(variableValues.Count()));
            var numberOfPoints = numberOfIntervals;

            var min = variableValues.Min();
            var max = variableValues.Max();
            var intervalLength = (max - min) / numberOfIntervals;

            min = min + intervalLength;
            max = max - intervalLength;

            var breakPoints = new double[numberOfPoints];
            for (var i = 0; i < numberOfPoints; i++)
            {
                breakPoints[i] = min + i * intervalLength;
            }

            // считаем кол-во попаданий в интервалы
            var counts = new int[numberOfPoints];
            var FValues = new double[numberOfPoints];
            for (var i = 0; i < breakPoints.Length; i++)
            {
                var currentValue = breakPoints[i];

                var count = variableValues.Where(v => v <= currentValue).Count();
                counts[i] = count;
                FValues[i] = (double)count / variableValues.Length;
            }

            var pdfValuesActual = new double[numberOfPoints - 1];
            for (int i = 0; i < pdfValuesActual.Length; i++)
            {
                var variableValue = breakPoints[i];
                pdfValuesActual[i] = distribution.GetPdfValueAtPoint(variableValue);
            }

            var pdfValuesExpected = CalculateProbabilityFunctionValues(FValues, breakPoints, intervalLength);

            return (pdfValuesExpected, pdfValuesActual);
        }

        private static double[] CalculateProbabilityFunctionValues(double[] FValues, double[] breakPoints, double intervalLength)
        {
            var probabilityFunctionValues = new double[breakPoints.Length - 1];

            for (var i = 0; i < breakPoints.Length - 1; i++)
            {
                var funcValue = (FValues[i + 1] - FValues[i]) / intervalLength;
                probabilityFunctionValues[i] = funcValue;
            }

            return probabilityFunctionValues;
        }
    }
}
