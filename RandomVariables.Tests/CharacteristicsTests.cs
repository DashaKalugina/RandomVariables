using NUnit.Framework;
using RandomVariablesLibrary.Distributions;
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
        public void GenerateUniformVariableAndCheckCharacteristics(double a, double b)
        {
            var uniformDistributedVariable = new RandomVariable(new UniformDistribution { A = a, B = b });

            var mean = (a + b) / 2; // мат. ожидание
            var variance = Math.Pow(b - a, 2) / 12; // дисперсия
            var standardDeviation = Math.Sqrt(variance); // СКО
            var skewness = 0; // коэффициент асимметрии
            var kurtosis = (double)(-1) * 6 / 5; // эксцесс

            var delta = Math.Pow(10, -2);
            Assert.AreEqual(mean, uniformDistributedVariable.Mean, delta);
            Assert.AreEqual(variance, uniformDistributedVariable.Variance, delta);
            Assert.AreEqual(standardDeviation, uniformDistributedVariable.StandardDeviation, delta);
            Assert.AreEqual(skewness, uniformDistributedVariable.Skewness, delta);
            Assert.AreEqual(kurtosis, uniformDistributedVariable.Kurtosis, delta);
        }

        [TestCase(0, 1)]
        [TestCase(1, 0.2)]
        [TestCase(-2, 0.5)]
        public void GenerateNormalVariableAndCheckCharacteristics(double mu, double sigma)
        {
            var normalDistributedVariable = new RandomVariable(new NormalDistribution { Mu = mu, Sigma = sigma });

            var mean = mu; // мат. ожидание
            var variance = Math.Pow(sigma, 2); // дисперсия
            var standardDeviation = sigma; // СКО
            var skewness = 0; // коэффициент асимметрии
            var kurtosis = 0; // эксцесс

            var delta = Math.Pow(10, -2);
            Assert.AreEqual(mean, normalDistributedVariable.Mean, delta);
            Assert.AreEqual(variance, normalDistributedVariable.Variance, delta);
            Assert.AreEqual(standardDeviation, normalDistributedVariable.StandardDeviation, delta);
            Assert.AreEqual(skewness, normalDistributedVariable.Skewness, delta);
            Assert.AreEqual(kurtosis, normalDistributedVariable.Kurtosis, delta);
        }

        [TestCase(0.5)]
        [TestCase(1.0)]
        [TestCase(1.5)]
        public void GenerateExponentialVariableAndCheckCharacteristics(double lambda)
        {
            var exponentialVariable = new RandomVariable(new ExponentialDistribution { Lambda = lambda });

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
    }
}