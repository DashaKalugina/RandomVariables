using NUnit.Framework;
using RandomVariablesLibraryNew;
using RandomVariablesLibraryNew.Distributions.Standard;
using System;
using System.Collections.Generic;
using System.Text;

namespace RandomVariables.Tests
{
    public class NormalDistributionTests
    {
        [TestCase(0, 1)]
        [TestCase(0, 0.8)]
        [TestCase(1, 1)]
        [TestCase(1, 0.2)]
        [TestCase(-2, 0.5)]
        public void GenerateNormalVariableAndCheckCharacteristics(double mu, double sigma)
        {
            var normalDistributedVariable = new NormalDistribution(mu, sigma);

            var mean = mu; // мат. ожидание
            var variance = Math.Pow(sigma, 2); // дисперсия
            var standardDeviation = sigma; // СКО
            var skewness = 0; // коэффициент асимметрии
            var kurtosis = 0; // эксцесс

            var delta = Math.Pow(10, -4);
            Assert.AreEqual(mean, normalDistributedVariable.Mean, delta);
            Assert.AreEqual(variance, normalDistributedVariable.Variance, delta);
            Assert.AreEqual(standardDeviation, normalDistributedVariable.StandardDeviation, delta);
            Assert.AreEqual(skewness, normalDistributedVariable.Skewness, delta);
            Assert.AreEqual(kurtosis, normalDistributedVariable.Kurtosis, delta);
        }

        [TestCase(0, 1)]
        [TestCase(0, 0.8)]
        [TestCase(1, 1)]
        [TestCase(1, 0.2)]
        [TestCase(-2, 0.5)]
        public void NormalDistributionChiSquareTest(double mu, double sigma)
        {
            var normalDistributedVariable = new NormalDistribution(mu, sigma);

            var test = ChiSquareTest.Test(normalDistributedVariable);
            Assert.IsTrue(test);
        }

        [TestCase(0, 1)]
        [TestCase(0, 0.8)]
        [TestCase(1, 1)]
        [TestCase(1, 0.2)]
        [TestCase(-2, 0.5)]
        public void SumOfTwoNormalDistributionsChiSquareTest(double mu, double sigma)
        {
            var distr1 = new NormalDistribution(mu, sigma);
            var distr2 = new NormalDistribution(mu, sigma);

            var sum = distr1 + distr2;
            var test = ChiSquareTest.Test(sum);
            Assert.IsTrue(test);
        }

        [TestCase(0, 1)]
        [TestCase(0, 0.8)]
        [TestCase(1, 1)]
        [TestCase(1, 0.2)]
        [TestCase(-2, 0.5)]
        public void DifferenceOfTwoNormalDistributionsChiSquareTest(double mu, double sigma)
        {
            var distr1 = new NormalDistribution(mu, sigma);
            var distr2 = new NormalDistribution(mu, sigma);

            var sum = distr1 - distr2;
            var test = ChiSquareTest.Test(sum);
            Assert.IsTrue(test);
        }

        [TestCase(0, 1)]
        [TestCase(0, 0.8)]
        [TestCase(1, 1)]
        [TestCase(1, 0.2)]
        [TestCase(-2, 0.5)]
        public void ProductOfTwoNormalDistributionsChiSquareTest(double mu, double sigma)
        {
            var distr1 = new NormalDistribution(mu, sigma);
            var distr2 = new NormalDistribution(mu, sigma);

            var sum = distr1 * distr2;
            var test = ChiSquareTest.Test(sum);
            Assert.IsTrue(test);
        }

        [TestCase(0, 1)]
        [TestCase(0, 0.8)]
        [TestCase(1, 1)]
        [TestCase(1, 0.2)]
        [TestCase(-2, 0.5)]
        public void QuotientOfTwoNormalDistributionsChiSquareTest(double mu, double sigma)
        {
            var distr1 = new NormalDistribution(mu, sigma);
            var distr2 = new NormalDistribution(mu, sigma);

            var sum = distr1 / distr2;
            var test = ChiSquareTest.Test(sum);
            Assert.IsTrue(test);
        }

        [TestCase(0, 1, 3)]
        [TestCase(0, 1, 4)]
        [TestCase(0, 1, 5)]
        [TestCase(0, 1, 6)]
        [TestCase(0, 0.8, 3)]
        [TestCase(0, 0.8, 4)]
        [TestCase(0, 0.8, 5)]
        [TestCase(0, 0.8, 6)]
        [TestCase(1, 1, 3)]
        [TestCase(1, 1, 4)]
        [TestCase(1, 1, 5)]
        [TestCase(1, 1, 6)]
        [TestCase(1, 0.2, 3)]
        [TestCase(1, 0.2, 4)]
        [TestCase(1, 0.2, 5)]
        [TestCase(-2, 0.5, 5)]
        [TestCase(-2, 0.5, 6)]
        public void SumOfSeveralNormalsChiSquareTest(double mu, double sigma, int count)
        {
            var distr1 = new NormalDistribution(mu, sigma);
            var distr2 = new NormalDistribution(mu, sigma);

            var sum = distr1 + distr2;
            if (count > 2)
            {
                for (var i = 0; i < count - 2; i++)
                {
                    var distr = new NormalDistribution(mu, sigma);
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
        [TestCase(0, 0.8, 3)]
        [TestCase(0, 0.8, 4)]
        [TestCase(0, 0.8, 5)]
        [TestCase(0, 0.8, 6)]
        [TestCase(1, 1, 3)]
        [TestCase(1, 1, 4)]
        [TestCase(1, 1, 5)]
        [TestCase(1, 1, 6)]
        [TestCase(1, 0.2, 3)]
        [TestCase(1, 0.2, 4)]
        [TestCase(1, 0.2, 5)]
        [TestCase(-2, 0.5, 5)]
        [TestCase(-2, 0.5, 6)]
        public void DifferenceOfSeveralNormalsChiSquareTest(double mu, double sigma, int count)
        {
            var distr1 = new NormalDistribution(mu, sigma);
            var distr2 = new NormalDistribution(mu, sigma);

            var diff = distr1 - distr2;
            if (count > 2)
            {
                for (var i = 0; i < count - 2; i++)
                {
                    var distr = new NormalDistribution(mu, sigma);
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
        [TestCase(0, 0.8, 3)]
        [TestCase(0, 0.8, 4)]
        [TestCase(0, 0.8, 5)]
        [TestCase(0, 0.8, 6)]
        [TestCase(1, 1, 3)]
        [TestCase(1, 1, 4)]
        [TestCase(1, 1, 5)]
        [TestCase(1, 1, 6)]
        [TestCase(1, 0.2, 3)]
        [TestCase(1, 0.2, 4)]
        [TestCase(1, 0.2, 5)]
        [TestCase(-2, 0.5, 5)]
        [TestCase(-2, 0.5, 6)]
        public void ProductOfSeveralNormalsChiSquareTest(double mu, double sigma, int count)
        {
            var distr1 = new NormalDistribution(mu, sigma);
            var distr2 = new NormalDistribution(mu, sigma);

            var diff = distr1 * distr2;
            if (count > 2)
            {
                for (var i = 0; i < count - 2; i++)
                {
                    var distr = new NormalDistribution(mu, sigma);
                    diff *= distr;
                }
            }

            var test = ChiSquareTest.Test(diff);
            Assert.IsTrue(test);
        }

        [TestCase(0, 1, 3)]
        [TestCase(0, 1, 4)]
        [TestCase(0, 1, 5)]
        [TestCase(0, 1, 6)]
        [TestCase(0, 0.8, 3)]
        [TestCase(0, 0.8, 4)]
        [TestCase(0, 0.8, 5)]
        [TestCase(0, 0.8, 6)]
        [TestCase(1, 1, 3)]
        [TestCase(1, 1, 4)]
        [TestCase(1, 1, 5)]
        [TestCase(1, 1, 6)]
        [TestCase(1, 0.2, 3)]
        [TestCase(1, 0.2, 4)]
        [TestCase(1, 0.2, 5)]
        [TestCase(-2, 0.5, 5)]
        [TestCase(-2, 0.5, 6)]
        public void QuotientOfSeveralNormalsChiSquareTest(double mu, double sigma, int count)
        {
            var distr1 = new NormalDistribution(mu, sigma);
            var distr2 = new NormalDistribution(mu, sigma);

            var diff = distr1 / distr2;
            if (count > 2)
            {
                for (var i = 0; i < count - 2; i++)
                {
                    var distr = new NormalDistribution(mu, sigma);
                    diff /= distr;
                }
            }

            var test = ChiSquareTest.Test(diff);
            Assert.IsTrue(test);
        }
    }
}
