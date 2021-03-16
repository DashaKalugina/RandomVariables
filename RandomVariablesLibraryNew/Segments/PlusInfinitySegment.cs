using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomVariablesLibraryNew.Segments
{
    /// <summary>
    /// Используется для представления сегмента вида (a; +inf]
    /// </summary>
    public class PlusInfinitySegment: Segment
    {
        public PlusInfinitySegment(double a, Func<double, double> probabilityFunction)
            : base(a, double.PositiveInfinity, probabilityFunction)
        {

        }
    }
}
