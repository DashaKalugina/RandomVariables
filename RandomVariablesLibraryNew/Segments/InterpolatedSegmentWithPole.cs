using RandomVariablesLibraryNew.Interpolators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomVariablesLibraryNew.Segments
{
    public class InterpolatedSegmentWithPole: SegmentWithPole
    {
        public InterpolatedSegmentWithPole(double a, double b, Func<double, double> func, bool leftPole = true)
            //: base(a, b, (x) => (new PoleInterpolatorP(func, a, b)).InterpolateAt(x), true)
            : base(a, b, null, true)
        {
            if (leftPole)
            {
                var poleInterpolatorP = new PoleInterpolatorP(func, a, b);
                ProbabilityFunction = (x) => poleInterpolatorP.InterpolateAt(x);
            }
        }
    }
}
