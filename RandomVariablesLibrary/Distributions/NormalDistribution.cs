using RandomVariablesLibrary.Generators;
using System;

namespace RandomVariables
{
    /// <summary>
    /// Класс нормального распределения
    /// </summary>
    public class NormalDistribution : DistributionBase
    {
        public double Mu { get; set; }
        public double Sigma { get; set; }

        public override Func<double, double> ProbabilityFunction
        {
            get => (x) =>
            {
                return Math.Pow(Sigma * Math.Pow(2 * Math.PI, 0.5), -1) *
                Math.Pow(Math.E, (-1) * Math.Pow(x - Mu, 2) / (2 * Math.Pow(Sigma, 2)));
            };
        }

        public override double GetNewRandomValue()
        {
            return NormalGenerator.Next(Mu, Sigma);
        }
    }
}
