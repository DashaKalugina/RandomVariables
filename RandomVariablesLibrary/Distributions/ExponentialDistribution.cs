using RandomVariables;
using RandomVariablesLibrary.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomVariablesLibrary.Distributions
{
    public class ExponentialDistribution: DistributionBase
    {
        public double Lambda { get; set; }

        public override double GetNewRandomValue()
        {
            return ExponentialGenerator.Next(Lambda);
        }
    }
}
