using NUnit.Framework;
using RandomVariablesLibraryNew;
using RandomVariablesLibraryNew.Distributions.Standard;
using System;
using System.Collections.Generic;
using System.Text;

namespace RandomVariables.Tests
{
    public class ExponentialDistributionTests
    {
        [TestCase(0.5)]
        [TestCase(1.0)]
        [TestCase(1.5)]
        public void GenerateExponentialVariableAndCheckCharacteristics(double lambda)
        {
            var exponentialVariable = new ExponentialDistribution(lambda);

            var mean = Math.Pow(lambda, -1); // мат. ожидание
            var variance = Math.Pow(lambda, -2); // дисперсия
            var standardDeviation = Math.Sqrt(variance); // СКО
            var skewness = 2.0; // коэффициент асимметрии
            var kurtosis = 6.0; // эксцесс

            var delta = Math.Pow(10, -3);
            Assert.AreEqual(mean, exponentialVariable.Mean, delta);
            Assert.AreEqual(variance, exponentialVariable.Variance, delta);
            Assert.AreEqual(standardDeviation, exponentialVariable.StandardDeviation, delta);
            Assert.AreEqual(skewness, exponentialVariable.Skewness, delta);
            Assert.AreEqual(kurtosis, exponentialVariable.Kurtosis, delta);
        }

        [TestCase(0.5)]
        [TestCase(1.0)]
        [TestCase(1.5)]
        public void ExponentialDistributionChiSquareTest(double lambda)
        {
            var expDistribution = new ExponentialDistribution(lambda);

            var test = ChiSquareTest.Test(expDistribution);
            Assert.IsTrue(test);
        }

        [TestCase(0.5)]
        [TestCase(1.0)]
        [TestCase(1.5)]
        public void SumOfTwoExponentialDistributionsChiSquareTest(double lambda)
        {
            var distr1 = new ExponentialDistribution(lambda);
            var distr2 = new ExponentialDistribution(lambda);

            var sum = distr1 + distr2;

            var test = ChiSquareTest.Test(sum);
            Assert.IsTrue(test);
        }

        [TestCase(0.5)]
        [TestCase(1.0)]
        [TestCase(1.5)]
        public void DifferenceOfTwoExponentialDistributionsChiSquareTest(double lambda)
        {
            var distr1 = new ExponentialDistribution(lambda);
            var distr2 = new ExponentialDistribution(lambda);

            var diff = distr1 - distr2;

            var test = ChiSquareTest.Test(diff);
            Assert.IsTrue(test);
        }

        [TestCase(0.5)]
        [TestCase(1.0)]
        [TestCase(1.5)]
        public void ProductOfTwoExponentialDistributionsChiSquareTest(double lambda)
        {
            var distr1 = new ExponentialDistribution(lambda);
            var distr2 = new ExponentialDistribution(lambda);

            var product = distr1 * distr2;

            var test = ChiSquareTest.Test(product);
            Assert.IsTrue(test);
        }

        [TestCase(0.5)]
        [TestCase(1.0)]
        [TestCase(1.5)]
        public void QuotientOfTwoExponentialDistributionsChiSquareTest(double lambda)
        {
            var distr1 = new ExponentialDistribution(lambda);
            var distr2 = new ExponentialDistribution(lambda);

            var quotient = distr1 / distr2;

            var test = ChiSquareTest.Test(quotient);
            Assert.IsTrue(test);
        }

        [TestCase(0.5, 3)]
        [TestCase(0.5, 4)]
        [TestCase(0.5, 5)]
        [TestCase(1.0, 3)]
        [TestCase(1.0, 4)]
        [TestCase(1.0, 5)]
        [TestCase(1.5, 3)]
        [TestCase(1.5, 4)]
        [TestCase(1.5, 5)]
        public void SumOfSeveralExponentialsChiSquareTest(double lambda, int count)
        {
            var distr1 = new ExponentialDistribution(lambda);
            var distr2 = new ExponentialDistribution(lambda);

            var sum = distr1 + distr2;
            if (count > 2)
            {
                for (var i = 0; i < count - 2; i++)
                {
                    var distr = new ExponentialDistribution(lambda);
                    sum += distr;
                }
            }

            var test = ChiSquareTest.Test(sum);
            Assert.IsTrue(test);
        }

        [TestCase(0.5, 3)]
        [TestCase(0.5, 4)]
        [TestCase(0.5, 5)]
        [TestCase(1.0, 3)]
        [TestCase(1.0, 4)]
        [TestCase(1.0, 5)]
        [TestCase(1.5, 3)]
        [TestCase(1.5, 4)]
        [TestCase(1.5, 5)]
        public void DifferenceOfSeveralExponentialsChiSquareTest(double lambda, int count)
        {
            var distr1 = new ExponentialDistribution(lambda);
            var distr2 = new ExponentialDistribution(lambda);

            var diff = distr1 - distr2;
            if (count > 2)
            {
                for (var i = 0; i < count - 2; i++)
                {
                    var distr = new ExponentialDistribution(lambda);
                    diff -= distr;
                }
            }

            var test = ChiSquareTest.Test(diff);
            Assert.IsTrue(test);
        }

        [TestCase(0.5, 3)]
        [TestCase(0.5, 4)]
        [TestCase(0.5, 5)]
        [TestCase(1.0, 3)]
        [TestCase(1.0, 4)]
        [TestCase(1.0, 5)]
        [TestCase(1.5, 3)]
        [TestCase(1.5, 4)]
        [TestCase(1.5, 5)]
        public void ProductOfSeveralExponentialsChiSquareTest(double lambda, int count)
        {
            var distr1 = new ExponentialDistribution(lambda);
            var distr2 = new ExponentialDistribution(lambda);

            var product = distr1 * distr2;
            if (count > 2)
            {
                for (var i = 0; i < count - 2; i++)
                {
                    var distr = new ExponentialDistribution(lambda);
                    product *= distr;
                }
            }

            var test = ChiSquareTest.Test(product);
            Assert.IsTrue(test);
        }

        [TestCase(0.5, 3)]
        [TestCase(0.5, 4)]
        [TestCase(0.5, 5)]
        [TestCase(1.0, 3)]
        [TestCase(1.0, 4)]
        [TestCase(1.0, 5)]
        [TestCase(1.5, 3)]
        [TestCase(1.5, 4)]
        [TestCase(1.5, 5)]
        public void QuotientOfSeveralExponentialsChiSquareTest(double lambda, int count)
        {
            var distr1 = new ExponentialDistribution(lambda);
            var distr2 = new ExponentialDistribution(lambda);

            var quotient = distr1 / distr2;
            if (count > 2)
            {
                for (var i = 0; i < count - 2; i++)
                {
                    var distr = new ExponentialDistribution(lambda);
                    quotient /= distr;
                }
            }

            var test = ChiSquareTest.Test(quotient);
            Assert.IsTrue(test);
        }
    }
}
