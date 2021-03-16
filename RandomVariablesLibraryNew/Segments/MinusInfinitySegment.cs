using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomVariablesLibraryNew.Segments
{
    /// <summary>
    /// Используется для представления сегмента вида [-inf; b)
    /// </summary>
    public class MinusInfinitySegment : Segment
    {
        public MinusInfinitySegment(double b, Func<double, double> probabilityFunction) 
            : base(double.NegativeInfinity, b, probabilityFunction)
        {

        }
    }
}
