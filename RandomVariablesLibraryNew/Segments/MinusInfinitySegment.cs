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

        protected override double FindLeftPoint()
        {
            var leftPoint = B - 1;
            var leftPointY = ProbabilityFunction(leftPoint);

            var startY = leftPointY;

            while(!(leftPointY/startY <= Math.Pow(10, -3)))
            {
                leftPoint = leftPoint - 1.2 * Math.Abs(leftPoint - B);
                leftPointY = ProbabilityFunction(leftPoint);

                if (Math.Abs(leftPoint) > Math.Pow(10, 20))
                {
                    return leftPoint;
                }
            }

            return leftPoint;
        }
    }
}
