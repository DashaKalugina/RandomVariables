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

        public override double FindRightPoint()
        {
            var rightPoint = A + 1;
            var rightPointY = ProbabilityFunction(rightPoint);

            var startY = rightPointY;

            while (!(rightPointY / startY <= Math.Pow(10, -3)))
            {
                rightPoint = rightPoint + 1.2 * Math.Abs(rightPoint - A);
                rightPointY = ProbabilityFunction(rightPoint);

                if (Math.Abs(rightPoint) > Math.Pow(10, 20))
                {
                    return rightPoint;
                }
            }

            return rightPoint;
        }

        public override Segment ShiftAndScale(double shift, double scale)
        {
            Func<double, double> newProbabilityFunction = (x) =>
            {
                return Math.Abs(1 / scale) * ProbabilityFunction((x - shift) * 1 / scale);
            };

            if (scale > 0)
            {
                return new PlusInfinitySegment(A * scale + shift, newProbabilityFunction);
            }

            return new MinusInfinitySegment(A * scale + shift, newProbabilityFunction);
        }
    }
}
