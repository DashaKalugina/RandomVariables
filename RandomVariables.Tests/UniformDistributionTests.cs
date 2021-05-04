using NUnit.Framework;
using RandomVariablesLibraryNew;
using RandomVariablesLibraryNew.Distributions.Standard;
using System;
using System.Collections.Generic;
using System.Text;

namespace RandomVariables.Tests
{
    public class UniformDistributionTests
    {
        [TestCase(0.0, 1.0)]
        [TestCase(-1.0, 10.0)]
        [TestCase(1, 5)]
        [TestCase(-100.4, 76)]
        [TestCase(-1728.5, -18.5)]
        public void GenerateUniformVariableAndCheckCharacteristics(double a, double b)
        {
            var uniformDistributedVariable = new UniformDistribution(a, b);

            var mean = (a + b) / 2; // мат. ожидание
            var variance = Math.Pow(b - a, 2) / 12; // дисперсия
            var standardDeviation = Math.Sqrt(variance); // СКО
            var skewness = 0; // коэффициент асимметрии
            var kurtosis = (double)(-1) * 6 / 5; // эксцесс

            var delta = Math.Pow(10, -3);
            Assert.AreEqual(mean, uniformDistributedVariable.Mean, delta);
            Assert.AreEqual(variance, uniformDistributedVariable.Variance, delta);
            Assert.AreEqual(standardDeviation, uniformDistributedVariable.StandardDeviation, delta);
            Assert.AreEqual(skewness, uniformDistributedVariable.Skewness, delta);
            Assert.AreEqual(kurtosis, uniformDistributedVariable.Kurtosis, delta);
        }

        [TestCase(0.0, 1.0)]
        [TestCase(-1.0, 10.0)]
        [TestCase(1, 5)]
        [TestCase(-100.4, 76)]
        [TestCase(-1728.5, -18.5)]
        public void UniformDistributionChiSquareTest(double a, double b)
        {
            var uniformDistribution = new UniformDistribution(a, b);

            var test = ChiSquareTest.Test(uniformDistribution);
            Assert.IsTrue(test);
        }

        [TestCase(0.0, 1.0)]
        [TestCase(-1.0, 10.0)]
        [TestCase(1, 5)]
        [TestCase(-100.4, 76)]
        [TestCase(-1728.5, -18.5)]
        public void SumOfTwoUniformDistributionsChiSquareTest(double a, double b)
        {
            var distr1 = new UniformDistribution(a, b);
            var distr2 = new UniformDistribution(a, b);

            var sum = distr1 + distr2;

            var test = ChiSquareTest.Test(sum);
            Assert.IsTrue(test);
        }

        [TestCase(0.0, 1.0)]
        [TestCase(-1.0, 10.0)]
        [TestCase(1, 5)]
        [TestCase(-100.4, 76)]
        [TestCase(-1728.5, -18.5)]
        public void DifferenceOfTwoUniformDistributionsChiSquareTest(double a, double b)
        {
            var distr1 = new UniformDistribution(a, b);
            var distr2 = new UniformDistribution(a, b);

            var sum = distr1 - distr2;

            var test = ChiSquareTest.Test(sum);
            Assert.IsTrue(test);
        }

        [TestCase(0.0, 1.0)]
        [TestCase(-1.0, 10.0)]
        [TestCase(1, 5)]
        [TestCase(-100.4, 76)]
        [TestCase(-1728.5, -18.5)]
        public void ProductOfTwoUniformDistributionsChiSquareTest(double a, double b)
        {
            var distr1 = new UniformDistribution(a, b);
            var distr2 = new UniformDistribution(a, b);

            var sum = distr1 * distr2;

            var test = ChiSquareTest.Test(sum);
            Assert.IsTrue(test);
        }

        [TestCase(0.0, 1.0)]
        [TestCase(-1.0, 10.0)]
        [TestCase(1, 5)]
        [TestCase(-100.4, 76)]
        [TestCase(-1728.5, -18.5)]
        public void QuotientOfTwoUniformDistributionsChiSquareTest(double a, double b)
        {
            var distr1 = new UniformDistribution(a, b);
            var distr2 = new UniformDistribution(a, b);

            var quotient = distr1 / distr2;

            var test = ChiSquareTest.Test(quotient);
            Assert.IsTrue(test);
        }

        [TestCase(0, 1, 3)]
        [TestCase(0, 1, 4)]
        [TestCase(0, 1, 5)]
        [TestCase(0, 1, 6)]
        [TestCase(-1, 3.6, 3)]
        [TestCase(-1, 3.6, 4)]
        [TestCase(-1, 3.6, 5)]
        [TestCase(3, 6, 3)]
        [TestCase(-5, 0, 4)]
        [TestCase(-1, 5.48, 5)]
        public void SumOfSeveralUniformsChiSquareTest(double a, double b, int count)
        {
            var distr1 = new UniformDistribution(a, b);
            var distr2 = new UniformDistribution(a, b);

            var sum = distr1 + distr2;
            if (count > 2)
            {
                for (var i = 0; i < count - 2; i++)
                {
                    var distr = new UniformDistribution(a, b);
                    sum += distr;
                }
            }

            var test = ChiSquareTest.Test(sum);
            Assert.IsTrue(test);
        }

        [TestCase(0, 1, 3)]
        [TestCase(0, 1, 4)]
        [TestCase(0, 1, 5)]
        [TestCase(0, 1, 6)]
        [TestCase(-1, 3.6, 3)]
        [TestCase(-1, 3.6, 4)]
        [TestCase(-1, 3.6, 5)]
        [TestCase(3, 6, 3)]
        [TestCase(-5, 0, 4)]
        [TestCase(-1, 5.48, 5)]
        public void DifferenceOfSeveralUniformsChiSquareTest(double a, double b, int count)
        {
            var distr1 = new UniformDistribution(a, b);
            var distr2 = new UniformDistribution(a, b);

            var diff = distr1 - distr2;
            if (count > 2)
            {
                for (var i = 0; i < count - 2; i++)
                {
                    var distr = new UniformDistribution(a, b);
                    diff -= distr;
                }
            }

            var test = ChiSquareTest.Test(diff);
            Assert.IsTrue(test);
        }

        [TestCase(0, 1, 3)]
        [TestCase(0, 1, 4)]
        [TestCase(0, 1, 5)]
        [TestCase(0, 1, 6)]
        [TestCase(-1, 3.6, 3)]
        [TestCase(-1, 3.6, 4)]
        [TestCase(-1, 3.6, 5)]
        [TestCase(3, 6, 3)]
        [TestCase(-5, 0, 4)]
        [TestCase(-1, 5.48, 5)]
        public void ProductOfSeveralUniformsChiSquareTest(double a, double b, int count)
        {
            var distr1 = new UniformDistribution(a, b);
            var distr2 = new UniformDistribution(a, b);

            var product = distr1 * distr2;
            if (count > 2)
            {
                for (var i = 0; i < count - 2; i++)
                {
                    var distr = new UniformDistribution(a, b);
                    product *= distr;
                }
            }

            var test = ChiSquareTest.Test(product);
            Assert.IsTrue(test);
        }

        [TestCase(0, 1, 3)]
        [TestCase(0, 1, 4)]
        [TestCase(0, 1, 5)]
        [TestCase(0, 1, 6)]
        [TestCase(-1, 3.6, 3)]
        [TestCase(-1, 3.6, 4)]
        [TestCase(-1, 3.6, 5)]
        [TestCase(3, 6, 3)]
        [TestCase(-5, 0, 4)]
        [TestCase(-1, 5.48, 5)]
        public void QuotientOfSeveralUniformsChiSquareTest(double a, double b, int count)
        {
            var distr1 = new UniformDistribution(a, b);
            var distr2 = new UniformDistribution(a, b);

            var quotient = distr1 / distr2;
            if (count > 2)
            {
                for (var i = 0; i < count - 2; i++)
                {
                    var distr = new UniformDistribution(a, b);
                    quotient /= distr;
                }
            }

            var test = ChiSquareTest.Test(quotient);
            Assert.IsTrue(test);
        }
    }
}
