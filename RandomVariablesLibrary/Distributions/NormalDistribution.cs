using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomVariables
{
    /// <summary>
    /// Класс нормального распределения
    /// </summary>
    public class NormalDistribution : DistributionBase
    {
        public override Func<double, double> ProbabilityFunction
        {
            get => (x) =>
            {
                return Math.Pow(Sigma * Math.Pow(2 * Math.PI, 0.5), -1) *
                Math.Pow(Math.E, (-1) * Math.Pow(x - Mu, 2) / (2 * Math.Pow(Sigma, 2)));
            };
        }

        //public override Func<double, double> DistributionFunction
        //{
        //    get => (x) =>
        //    {

        //    };
        //}

        public double Mu { get; set; }
        public double Sigma { get; set; }
        public override IRandomNumberGenerator RandomNumberGenerator => throw new NotImplementedException();
    }
}
