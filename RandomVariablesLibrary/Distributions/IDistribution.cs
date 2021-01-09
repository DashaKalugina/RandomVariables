using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomVariables
{
    interface IDistribution
    {
        //double distributionFunction(double x);
        //Func<double, double> distributionFunction { get; set; }
        Func<double, double> probabilityFunction { get; set; }

    }
}
