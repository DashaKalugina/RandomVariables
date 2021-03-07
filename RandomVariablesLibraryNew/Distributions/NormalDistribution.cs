using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomVariablesLibraryNew.Distributions
{
    /// <summary>
    /// Класс нормального распределения
    /// </summary>
    public class NormalDistribution: Distribution
    {
        public double Mu { get; }
        public double Sigma { get; }

        public Func<double, double> ProbabilityFunction
        {
            get => (x) =>
            {
                return Math.Pow(Sigma * Math.Pow(2 * Math.PI, 0.5), -1) *
                Math.Pow(Math.E, (-1) * Math.Pow(x - Mu, 2) / (2 * Math.Pow(Sigma, 2)));
            };
        }

        public NormalDistribution(double mu, double sigma)
        {
            Mu = mu;
            Sigma = sigma;
        }
    }
}
