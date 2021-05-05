using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomVariablesLibrary.Segments
{
    /// <summary>
    /// Класс используется для представления сегмента с полюсом на интервале (a, b)
    /// </summary>
    public class SegmentWithPole: Segment
    {
        public SegmentWithPole(double a, double b, Func<double, double> probabilityFunction, bool leftPole)
            : base(a, b, probabilityFunction)
        {
            LeftPole = leftPole;
        }

        public bool LeftPole { get; }

        public override double SafeA
        {
            get
            {
                if (LeftPole)
                {
                    return A == 0 ? 0 : A + Math.Abs(A) * double.Epsilon;
                }

                return A;
            }
        }

        public override double SafeB
        {
            get
            {
                if (LeftPole)
                {
                    return B;
                }

                return B == 0 ? 0 : B - Math.Abs(B) * double.Epsilon;
            }
        }

        public override Segment ShiftAndScale(double shift, double scale)
        {
            Func<double, double> newProbabilityFunction = (x) =>
            {
                return Math.Abs(1 / scale) * ProbabilityFunction((x - shift) * 1 / scale);
            };

            if (scale > 0)
            {
                var newA = A * scale + shift;
                var newB = B * scale + shift;
                return new SegmentWithPole(newA, newB, newProbabilityFunction, LeftPole);
            }
            else
            {
                var newA = B * scale + shift;
                var newB = A * scale + shift;
                return new SegmentWithPole(newA, newB, newProbabilityFunction, !LeftPole);
            }
        }
    }
}
