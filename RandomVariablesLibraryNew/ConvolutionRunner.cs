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

        /// <summary>
        /// Метод вычисляет значение свертки функций плотности в точке x для операции сложения
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public double GetConvolutionValueAtPoint(double x)
        {
            double integralValue = default;

            foreach(var tuple in SegmentTuplesList)
            {
                var segment1 = tuple.Item1;
                var segment2 = tuple.Item2;

                var fun1 = GetSumFunc1(segment1, segment2, x);
                var fun2 = GetSumFunc2(segment1, segment2, x);

                var minX = Math.Max(segment1.SafeA, x - segment2.SafeB);
                var maxX = Math.Min(segment1.SafeB, x - segment2.SafeA);

                var minY = Math.Max(segment2.SafeA, x - segment1.SafeB);
                var maxY = Math.Min(segment2.SafeB, x - segment1.SafeA);

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

        /// <summary>
        /// Метод вычисляет значение свертки функций плотности в точке x для операции умножения
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public double GetConvolutionValueAtPointProduct(double x)
        {
            double integralValue = default;

            foreach (var tuple in SegmentTuplesList)
            {
                var segment1 = tuple.Item1;
                var segment2 = tuple.Item2;

                var func1 = GetProductFunc1(segment1, segment2, x);
                var func2 = GetProductFunc2(segment1, segment2, x);

                double min;
                double max;
                double integral1 = default;
                var sign = Math.Sign(segment1.A + segment1.B);
                var convolution = (segment1.A + segment1.B) * (segment2.A + segment2.B);
                if (convolution > 0)
                {
                    if (x > 0)
                    {
                        min = Math.Max(segment1.A, sign * x / Math.Abs(segment2.B));
                        max = Math.Min(segment1.B, sign * x / Math.Abs(segment2.A));
                    }
                    else
                    {
                        min = segment1.A;
                        max = segment1.B;
                        integral1 = IntegralCalculator.Integrate(segment2.A, segment2.B, func2);
                    }
                }
                else
                {
                    if (x < 0)
                    {
                        min = Math.Max(segment1.A, (-1) * sign * x / Math.Abs(segment2.A));
                        max = Math.Min(segment1.B, (-1) * sign * x / Math.Abs(segment2.B));
                    }
                    else
                    {
                        min = segment1.A;
                        max = segment1.B;
                        integral1 = IntegralCalculator.Integrate(segment2.A, segment2.B, func2);
                    }
                }

                integralValue += IntegralCalculator.Integrate(min, max, func1);
                integralValue += integral1;
            }

            //var func1 = GetFunc1(

            return integralValue;
        }

        private Func<double, double> GetProductFunc1(Segment segment1, Segment segment2, double x)
        {
            return (t) => t == 0 ? 0 : segment1[t] * segment2[x / t] * 1.0 / Math.Abs(t);
            //return (t) => segment1[t] * segment2[x / t] * 1.0 / Math.Abs(t);
        }

        private Func<double, double> GetProductFunc2(Segment segment1, Segment segment2, double x)
        {
            return (t) => t == 0 ? 0 : segment1[x / t] * segment2[t] * 1.0 / Math.Abs(t);
            //return (t) => segment1[x / t] * segment2[t] * 1.0 / Math.Abs(t);
        }

        private Func<double, double> GetSumFunc1(Segment segment1, Segment segment2, double x)
        {
            return (t) => segment1[t] * segment2[x - t];
        }

        private Func<double, double> GetSumFunc2(Segment segment1, Segment segment2, double x)
        {
            return (t) => segment1[x - t] * segment2[t];
        }
    }
}
