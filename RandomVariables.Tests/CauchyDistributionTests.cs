using NUnit.Framework;
using RandomVariablesLibraryNew;
using RandomVariablesLibraryNew.Distributions.Standard;
using System;
using System.Collections.Generic;
using System.Text;

namespace RandomVariables.Tests
{
    public class CauchyDistributionTests
    {
        [TestCase(0, 0.5)]
        [TestCase(0, 1)]
        [TestCase(0, 2)]
        [TestCase(-2, 1)]
        public void CauchyDistributionChiSquareTest(double center, double gamma)
        {
            var cauchyDistribution = new CauchyDistribution(center, gamma);

            var test = ChiSquareTest.Test(cauchyDistribution);
            Assert.IsTrue(test);
        }

        [TestCase(0, 0.5, 2)]
        [TestCase(0, 0.5, 3)]
        [TestCase(0, 0.5, 4)]
        [TestCase(0, 0.5, 5)]
        [TestCase(0, 1, 2)]
        [TestCase(0, 1, 3)]
        [TestCase(0, 1, 4)]
        [TestCase(0, 1, 5)]
        [TestCase(0, 2, 2)]
        [TestCase(0, 2, 3)]
        [TestCase(0, 2, 4)]
        [TestCase(0, 2, 5)]
        [TestCase(-2, 1, 2)]
        [TestCase(-2, 1, 3)]
        [TestCase(-2, 1, 4)]
        [TestCase(-2, 1, 5)]
        public void SumOfSeveralCauchysChiSquareTest(double center, double gamma, int count)
        {
            var distr1 = new CauchyDistribution(center, gamma);
            var distr2 = new CauchyDistribution(center, gamma);

            var sum = distr1 + distr2;
            if (count > 2)
            {
                for (var i = 0; i < count - 2; i++)
                {
                    var distr = new CauchyDistribution(center, gamma);
                    sum += distr;
                }
            }

            var test = ChiSquareTest.Test(sum);
            Assert.IsTrue(test);
        }

        [TestCase(0, 0.5, 2)]
        [TestCase(0, 0.5, 3)]
        [TestCase(0, 0.5, 4)]
        [TestCase(0, 0.5, 5)]
        [TestCase(0, 1, 2)]
        [TestCase(0, 1, 3)]
        [TestCase(0, 1, 4)]
        [TestCase(0, 1, 5)]
        [TestCase(0, 2, 2)]
        [TestCase(0, 2, 3)]
        [TestCase(0, 2, 4)]
        [TestCase(0, 2, 5)]
        [TestCase(-2, 1, 2)]
        [TestCase(-2, 1, 3)]
        [TestCase(-2, 1, 4)]
        [TestCase(-2, 1, 5)]
        public void DifferenceOfSeveralCauchysChiSquareTest(double center, double gamma, int count)
        {
            var distr1 = new CauchyDistribution(center, gamma);
            var distr2 = new CauchyDistribution(center, gamma);

            var diff = distr1 - distr2;
            if (count > 2)
            {
                for (var i = 0; i < count - 2; i++)
                {
                    var distr = new CauchyDistribution(center, gamma);
                    diff -= distr;
                }
            }

            var test = ChiSquareTest.Test(diff);
            Assert.IsTrue(test);
        }

        [TestCase(0, 0.5, 2)]
        [TestCase(0, 0.5, 3)]
        [TestCase(0, 0.5, 4)]
        [TestCase(0, 0.5, 5)]
        [TestCase(0, 1, 2)]
        [TestCase(0, 1, 3)]
        [TestCase(0, 1, 4)]
        [TestCase(0, 1, 5)]
        [TestCase(0, 2, 2)]
        [TestCase(0, 2, 3)]
        [TestCase(0, 2, 4)]
        [TestCase(0, 2, 5)]
        [TestCase(-2, 1, 2)]
        [TestCase(-2, 1, 3)]
        [TestCase(-2, 1, 4)]
        [TestCase(-2, 1, 5)]
        public void ProductOfSeveralCauchysChiSquareTest(double center, double gamma, int count)
        {
            var distr1 = new CauchyDistribution(center, gamma);
            var distr2 = new CauchyDistribution(center, gamma);

            var product = distr1 * distr2;
            if (count > 2)
            {
                for (var i = 0; i < count - 2; i++)
                {
                    var distr = new CauchyDistribution(center, gamma);
                    product *= distr;
                }
            }

            var test = ChiSquareTest.Test(product);
            Assert.IsTrue(test);
        }

        [TestCase(0, 0.5, 2)]
        [TestCase(0, 0.5, 3)]
        [TestCase(0, 0.5, 4)]
        [TestCase(0, 0.5, 5)]
        [TestCase(0, 1, 2)]
        [TestCase(0, 1, 3)]
        [TestCase(0, 1, 4)]
        [TestCase(0, 1, 5)]
        [TestCase(0, 2, 2)]
        [TestCase(0, 2, 3)]
        [TestCase(0, 2, 4)]
        [TestCase(0, 2, 5)]
        [TestCase(-2, 1, 2)]
        [TestCase(-2, 1, 3)]
        [TestCase(-2, 1, 4)]
        [TestCase(-2, 1, 5)]
        public void QuotientOfSeveralCauchysChiSquareTest(double center, double gamma, int count)
        {
            var distr1 = new CauchyDistribution(center, gamma);
            var distr2 = new CauchyDistribution(center, gamma);

            var quotient = distr1 / distr2;
            if (count > 2)
            {
                for (var i = 0; i < count - 2; i++)
                {
                    var distr = new CauchyDistribution(center, gamma);
                    quotient /= distr;
                }
            }

            var test = ChiSquareTest.Test(quotient);
            Assert.IsTrue(test);
        }
    }
}
