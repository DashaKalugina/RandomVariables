using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomVariablesLibraryNew.Segments
{
    public class Segment
    {
        public double A { get; }

        public double B { get; }

        public Func<double, double> ProbabilityFunction { get; }

        public Segment(double a, double b, Func<double, double> probabilityFunction)
        {
            A = a;
            B = b;
            ProbabilityFunction = probabilityFunction;
        }

        public double this[double x]
        {
            get
            {
                return ProbabilityFunction(x);
            }
        }
    }
}
