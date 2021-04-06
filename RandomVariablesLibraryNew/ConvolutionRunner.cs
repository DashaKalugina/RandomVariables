using RandomVariablesLibraryNew.Segments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomVariablesLibraryNew
{
    public class ConvolutionRunner
    {
        public List<Tuple<Segment, Segment>> SegmentTuplesList { get; set; }

        public ConvolutionRunner(List<Tuple<Segment, Segment>> segments)
        {
            SegmentTuplesList = segments;
        }

        public double GetConvolutionValueAtPoint(double x)
        {
            double integralValue = default;

            foreach(var tuple in SegmentTuplesList)
            {
                var segment1 = tuple.Item1;
                var segment2 = tuple.Item2;

                var fun1 = GetFunc1(segment1, segment2, x);
                var fun2 = GetFunc2(segment1, segment2, x);

                var minX = Math.Max(segment1.A, x - segment2.B);
                var maxX = Math.Min(segment1.B, x - segment2.A);

                var minY = Math.Max(segment2.A, x - segment1.B);
                var maxY = Math.Min(segment2.B, x - segment1.A);

                var isSegment1Finite = !double.IsInfinity(segment1.A) && !double.IsInfinity(segment1.A);
                var isSegment2Finite = !double.IsInfinity(segment2.A) && !double.IsInfinity(segment2.A);

                if (isSegment1Finite && isSegment2Finite)
                {
                    // здесь будет обработка полюсов

                    // Полюсов нет, интегрируем по х
                    integralValue += IntegralCalculator.Integrate(minX, maxX, fun1);
                } 
                else if (isSegment1Finite && (double.IsInfinity(segment2.A) || double.IsInfinity(segment2.B)))
                {
                    // Сегмент 1 конечный, интегрируем по х
                    integralValue += IntegralCalculator.Integrate(minX, maxX, fun1);
                }
                else if (isSegment2Finite && (double.IsInfinity(segment1.A) || double.IsInfinity(segment1.B)))
                {
                    // Сегмент 2 конечный, интегрируем по y
                    integralValue += IntegralCalculator.Integrate(minY, maxY, fun2);
                }
                else if (double.IsInfinity(segment1.A) && double.IsInfinity(segment2.B))
                {
                    if (Math.Abs(maxX) < Math.Abs(minY))
                    {
                        integralValue += IntegralCalculator.CalculateFromMinusInfinityIntegral(maxX, fun1);
                    } 
                    else
                    {
                        integralValue += IntegralCalculator.CalculateToPositiveInfinityIntegral(minY, fun2);
                    }
                }
                else if (double.IsInfinity(segment1.B) && double.IsInfinity(segment2.A))
                {
                    if (Math.Abs(minX) < Math.Abs(maxY))
                    {
                        integralValue += IntegralCalculator.CalculateFromMinusInfinityIntegral(minX, fun1);
                    }
                    else
                    {
                        integralValue += IntegralCalculator.CalculateToPositiveInfinityIntegral(maxY, fun2);
                    }
                }
                else if ((double.IsInfinity(segment1.A) && double.IsInfinity(segment2.A))
                    || double.IsInfinity(segment1.B) && double.IsInfinity(segment2.B))
                {

                }
            }

            //var func1 = GetFunc1(

            return integralValue;
        }

        private Func<double, double> GetFunc1(Segment segment1, Segment segment2, double x)
        {
            return (t) => segment1[t] * segment2[x - t];
        }

        private Func<double, double> GetFunc2(Segment segment1, Segment segment2, double x)
        {
            return (t) => segment1[x - t] * segment2[t];
        }
    }
}
