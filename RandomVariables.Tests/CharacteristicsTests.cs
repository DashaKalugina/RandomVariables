using System;
using NUnit.Framework;
using RandomVariablesLibrary;
using RandomVariablesLibrary.Distributions.Standard;

namespace RandomVariables.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        

        

        /// <summary>
        /// Тест вычисления несобственного интеграла к +inf.
        /// Три случая: 1) [a, +inf) (a < 0); 2) [a, +inf) (a = 0); 3) [a, +inf) (a > 0); 
        /// </summary>
        [Test]
        public void ToPositiveInfinityIntegralTest()
        {
            var abs = Math.Pow(10, -8);

            Func<double, double> func1 = (x) => 1 / Math.Pow(x, 2);
            var res1 = IntegralCalculator.Integrate(1, double.PositiveInfinity, func1);
            Assert.AreEqual(1.0, res1, abs);

            Func<double, double> func2 = (x) => x / (Math.Pow(x, 4) + 1);
            var res2 = IntegralCalculator.Integrate(0, double.PositiveInfinity, func2);
            Assert.AreEqual(Math.PI / 4, res2, abs);

            Func<double, double> func3 = (x) => 1 / (Math.Pow(x, 2) + 4 * x + 5);
            var res3 = IntegralCalculator.Integrate(-1, double.PositiveInfinity, func3);
            Assert.AreEqual(Math.PI / 4, res3, abs);

            Func<double, double> func4 = (x) => x * Math.Pow(Math.E, (-1) * x * x);
            var res4 = IntegralCalculator.Integrate(0, double.PositiveInfinity, func4);
            Assert.AreEqual(0.5, res4, abs);
        }

        [Test]
        public void FromNegativeInfinityIntegralTest()
        {
            var abs = Math.Pow(10, -8);

            Func<double, double> func1 = (x) => 2.0 / (Math.Pow(x, 2) + 9);
            var res1 = IntegralCalculator.Integrate(double.NegativeInfinity, -3.0, func1);
            Assert.AreEqual(Math.PI / 6, res1, abs);

            Func<double, double> func2 = (x) => x * Math.Pow(Math.E, (-1) * x * x);
            var res2 = IntegralCalculator.Integrate(double.NegativeInfinity, 0, func2);
            Assert.AreEqual(-0.5, res2, abs);
        }

        [Test]
        public void FromNegativeToPositiveInfinityIntegralTest()
        {
            var abs = Math.Pow(10, -8);

            Func<double, double> func1 = (x) => x * Math.Pow(Math.E, (-1) * x * x);
            var res1 = IntegralCalculator.Integrate(double.NegativeInfinity, double.PositiveInfinity, func1);
            Assert.AreEqual(0, res1, abs);

            Func<double, double> func2 = (x) => 1 / (x * x + 2 * x + 8);
            var res2 = IntegralCalculator.Integrate(double.NegativeInfinity, double.PositiveInfinity, func2);
            Assert.AreEqual(Math.PI / Math.Sqrt(7), res2, abs);
        }


        //[TestCase(1, 1)]
        //public void WeibullDistributionChiSquareTest(double k, double lambda)
        //{
        //    var count = default(int);
        //    var pValue = 0.05;
        //    var experimentsCount = 20;

        //    for (var i = 0; i < experimentsCount; i++)
        //    {
        //        try
        //        {
        //            var weibullDistribution = new WeibullDistribution(k, lambda);
        //            var test = ChiSquareTest.Test(weibullDistribution);
        //            if (!test)
        //            {
        //                throw new Exception();
        //            }
        //        }
        //        catch
        //        {
        //            count++;
        //            Assert.IsTrue(count <= pValue * experimentsCount, $"Упало на попытке {i+1}");
        //        }
        //    }
        //}

        [TestCase(0, 0.5)]
        [TestCase(0, 1)]
        [TestCase(0, 2)]
        [TestCase(-2, 1)]
        public void CauchyDistributionChiSquareTest(double center, double gamma)
        {
            var cauchyDistribution = new CauchyDistribution(gamma, center);

            var test = ChiSquareTest.Test(cauchyDistribution);
            Assert.IsTrue(test);
        }

        [TestCase(0.5, 1)]
        [TestCase(1, 1)]
        [TestCase(1.5, 1)]
        [TestCase(5, 1)]
        public void WeibullDistributionChiSquareTest(double k, double lambda)
        {
            var weibullDistribution = new WeibullDistribution(k, lambda);
            var test = ChiSquareTest.Test(weibullDistribution);

            Assert.IsTrue(test);
        }
    }
}