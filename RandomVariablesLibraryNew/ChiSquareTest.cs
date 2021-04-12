using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Statistics;
using Test = Accord.Statistics.Testing.ChiSquareTest;

namespace RandomVariablesLibraryNew
{
    public static class ChiSquareTest
    {
        private const int N = 1000;
        private const int degreesOfFreedom = N;

        public static bool Test(List<double> expected, List<double> observed)
        {
            //var degreesOfFreedom = observed.Length - 1;

            var numberOfHitsExpected = GetNumberOfHitsAtIntervals(expected);
            var numberOfHitsObserved = GetNumberOfHitsAtIntervals(observed);

            var chiSquareTest = new Test(numberOfHitsExpected, numberOfHitsObserved, degreesOfFreedom);

            var pValue = chiSquareTest.PValue;

            var significant = chiSquareTest.Significant;

            return !significant;
        }

        public static double[] GetNumberOfHitsAtIntervals(List<double> data)
        {
            var dict = new Dictionary<double, int>();

            // сортируем данные
            data.Sort();

            //const int N = 1001; // кол-во точек
            //var deegreesOfFreedom = N - 1; // кол-во интервалов

            var minValue = data[5];
            var maxValue = data[data.Count - 6];

            var step = (maxValue - minValue) / degreesOfFreedom;

            for (var i = 0; i < N - 1; i++)
            {
                dict.Add(minValue + i * step, 0);
            }

            dict.Add(double.PositiveInfinity, 0);

            foreach (var point in data)
            {
                if (point < minValue)
                {
                    dict[minValue]++;
                    continue;
                }

                if (point > maxValue)
                {
                    dict[double.PositiveInfinity]++;
                    continue;
                }

                var rightValueOfInterval = dict.Keys.FirstOrDefault(key => point < key);
                if (dict.ContainsKey(rightValueOfInterval))
                {
                    dict[rightValueOfInterval]++;
                }
            }

            if (!dict.Values.Sum().Equals(data.Count))
            {
                throw new Exception("Неверно подсчитано кол-во попаданий по интервалам.");
            }

            var hitProbabilities = dict.Values.Select(value => (double)value / data.Count);
            

            return hitProbabilities.ToArray();
        }
    }
}
