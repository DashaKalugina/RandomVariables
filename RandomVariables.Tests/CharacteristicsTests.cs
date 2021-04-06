using NUnit.Framework;
using RandomVariablesLibraryNew;
using RandomVariablesLibraryNew.Distributions;
//using RandomVariablesLibrary.Distributions;

using System;

namespace RandomVariables.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

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

            var delta = Math.Pow(10, -3);
            Assert.AreEqual(mean, normalDistributedVariable.Mean, delta);
            Assert.AreEqual(variance, normalDistributedVariable.Variance, delta);
            Assert.AreEqual(standardDeviation, normalDistributedVariable.StandardDeviation, delta);
            Assert.AreEqual(skewness, normalDistributedVariable.Skewness, delta);
            Assert.AreEqual(kurtosis, normalDistributedVariable.Kurtosis, delta);
        }

        /// <summary>
        /// Тест вычисления несобственного интеграла к +inf.
        /// Три случая: 1) [a, +inf) (a < 0); 2) [a, +inf) (a = 0); 3) [a, +inf) (a > 0); 
        /// </summary>
        [Test]
        public void ToPositiveInfinityIntegralTest()
        {
            var abs = Math.Pow(10, -4);

            Func<double, double> func1 = (x) => 1 / Math.Pow(x, 2);
            var res1 = IntegralCalculator.Integrate(1, double.PositiveInfinity, func1);
            Assert.AreEqual(1.0, res1, abs);

            Func<double, double> func2 = (x) => x / (Math.Pow(x, 4) + 1);
            var res2 = IntegralCalculator.Integrate(0, double.PositiveInfinity, func2);
            Assert.AreEqual(Math.PI/4, res2, abs);

            Func<double, double> func3 = (x) => 1 / (Math.Pow(x, 2) + 4 * x + 5);
            var res3 = IntegralCalculator.Integrate(- 1, double.PositiveInfinity, func3);
            Assert.AreEqual(Math.PI / 4, res3, abs);

            Func<double, double> func4 = (x) => x * Math.Pow(Math.E, (-1) * x * x);
            var res4 = IntegralCalculator.Integrate(0, double.PositiveInfinity, func4);
            Assert.AreEqual(0.5, res4, abs);
        }

        [Test]
        public void FromNegativeInfinityIntegralTest()
        {
            var abs = Math.Pow(10, -4);

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
            var abs = Math.Pow(10, -4);

            Func<double, double> func1 = (x) => x * Math.Pow(Math.E, (-1) * x * x);
            var res1 = IntegralCalculator.Integrate(double.NegativeInfinity, double.PositiveInfinity, func1);
            Assert.AreEqual(0, res1, abs);

            Func<double, double> func2 = (x) => 1 / (x * x + 2 * x + 8);
            var res2 = IntegralCalculator.Integrate(double.NegativeInfinity, double.PositiveInfinity, func2);
            Assert.AreEqual(Math.PI/Math.Sqrt(7), res2, abs);
        }

        //[TestCase(0.5)]
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

            var delta = Math.Pow(10, -2);
            Assert.AreEqual(mean, exponentialVariable.Mean, delta);
            Assert.AreEqual(variance, exponentialVariable.Variance, delta);
            Assert.AreEqual(standardDeviation, exponentialVariable.StandardDeviation, delta);
            Assert.AreEqual(skewness, exponentialVariable.Skewness, delta);
            Assert.AreEqual(kurtosis, exponentialVariable.Kurtosis, delta);
        }

        [Test]
        public void TestSumDistributionOfUniformVariables()
        {
            var var1 = new UniformDistribution(0, 1);
            var var2 = new UniformDistribution(0, 1);

            var sum = var1 + var2;

            var delta = Math.Pow(10, -3);
        }

        [Test]
        public void TestSumDistributionOfNormalVariables()
        {
            var var1 = new NormalDistribution(0, 1);
            var var2 = new NormalDistribution(0, 1);

            var sum = var1 + var2;

            var delta = Math.Pow(10, -3);
        }
    }
}