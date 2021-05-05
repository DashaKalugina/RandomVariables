using RandomVariablesLibrary.Segments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomVariablesLibrary.ConvolutionCalculators
{
    public static class QuotientConvolutionCalculator
    {
        public static PiecewiseFunction Calculate(PiecewiseFunction f, PiecewiseFunction g)
        {
            var resultPiecewiseFunction = new PiecewiseFunction();

            var fSplitted = GetSplittedPiecewiseFunction(f);
            var gSplitted = GetSplittedPiecewiseFunction(g);

            var breaks = GetResultBreakPoints(fSplitted, gSplitted);

            if (double.IsInfinity(breaks[0]))
            {
                var appropriateSegments = FindSegments(fSplitted, gSplitted, breaks[1] - 1);
                var convRunner = new ConvolutionRunner(appropriateSegments);

                Func<double, double> probabilityFunction = (x) => convRunner.GetConvolutionValueAtPointQuotient(x);

                var minusInfSegment = new MinusInfinitySegment(breaks[1], probabilityFunction);
                resultPiecewiseFunction.AddSegment(minusInfSegment);

                breaks.RemoveAt(0);
            }

            if (double.IsInfinity(breaks[breaks.Count - 1]))
            {
                var appropriateSegments = FindSegments(fSplitted, gSplitted, breaks[breaks.Count - 2] + 1);
                var convRunner = new ConvolutionRunner(appropriateSegments);

                Func<double, double> probabilityFunction = (x) => convRunner.GetConvolutionValueAtPointQuotient(x);

                var plusInfSegment = new PlusInfinitySegment(breaks[breaks.Count - 2], probabilityFunction);
                resultPiecewiseFunction.AddSegment(plusInfSegment);

                breaks.RemoveAt(breaks.Count - 1);
            }

            for (var i = 0; i < breaks.Count - 1; i++)
            {
                var segments = FindSegments(fSplitted, gSplitted, (breaks[i] + breaks[i + 1]) / 2);
                var runner = new ConvolutionRunner(segments);

                Func<double, double> func = (x) => runner.GetConvolutionValueAtPointQuotient(x);

                //Segment newSegment = null;

                var newSegment = new Segment(breaks[i], breaks[i + 1], func);

                //if (breaks[i] == 0)
                //{
                //    if (poleAtZero)
                //    {
                //        newSegment = new SegmentWithPole(breaks[i], breaks[i + 1], func, true);
                //        //var segmentWithPole = new SegmentWithPole(breaks[i], breaks[i + 1], func, true);
                //        //newSegment = segmentWithPole.ToInterpolatedSegment();
                //    }
                //    else
                //    {
                //        newSegment = new Segment(breaks[i], breaks[i + 1], func);
                //    }
                //}
                //else if (breaks[i + 1] == 0)
                //{
                //    if (poleAtZero)
                //    {
                //        newSegment = new SegmentWithPole(breaks[i], breaks[i + 1], func, false);
                //    }
                //    else
                //    {
                //        newSegment = new Segment(breaks[i], breaks[i + 1], func);
                //    }
                //}
                //else
                //{
                //    newSegment = new Segment(breaks[i], breaks[i + 1], func);
                //}

                //var newSegment = new Segment(breaks[i], breaks[i + 1], func);

                resultPiecewiseFunction.AddSegment(newSegment);
            }

            return resultPiecewiseFunction;
        }

        private static List<Tuple<Segment, Segment>> FindSegments(PiecewiseFunction f, PiecewiseFunction g, double z)
        {
            var segmentTuplesList = new List<Tuple<Segment, Segment>>();

            foreach (var fseg in f.Segments)
            {
                foreach (var gseg in g.Segments)
                {
                    // does x*z intersect the rectangular segment?
                    var yl = z * gseg.A;
                    var yr = z * gseg.B;

                    if ((yl > fseg.A && yr < fseg.B) || (yl < fseg.B && yr > fseg.A))
                    {
                        segmentTuplesList.Add(new Tuple<Segment, Segment>(fseg, gseg));
                    }
                }
            }

            return segmentTuplesList;
        }

        private static List<double> GetResultBreakPoints(PiecewiseFunction f, PiecewiseFunction g)
        {
            var breakPointProducts = GetBreakPointsProducts(f, g);

            //breakPointProducts.Sort();

            var resultBreaks = new List<double>();
            resultBreaks.AddRange(breakPointProducts);

            var (fPos, fNeg) = DistrSigns(f);
            var (gZero, gZeroPos, gZeroNeg) = DistrZeroSigns(g);

            if (gZero)
            {
                if ((fPos && gZeroNeg) || (fNeg && gZeroPos))
                {
                    if (!resultBreaks.Contains(double.NegativeInfinity))
                    {
                        resultBreaks.Add(double.NegativeInfinity);
                    }
                }
                if ((fPos && gZeroPos) || (fNeg && gZeroNeg))
                {
                    if (!resultBreaks.Contains(double.PositiveInfinity))
                    {
                        resultBreaks.Add(double.PositiveInfinity);
                    }
                }
            }

            resultBreaks.Sort();

            var index = resultBreaks.IndexOf(0);
            if (index < resultBreaks.Count - 1)
            {
                if (double.IsInfinity(resultBreaks[index + 1]) && !resultBreaks.Contains(1))
                {
                    resultBreaks.Add(1);
                }
            }

            resultBreaks.Sort();

            return resultBreaks;
        }

        private static List<double> GetBreakPointsProducts(PiecewiseFunction f, PiecewiseFunction g)
        {
            var fBreaks = f.GetBreakPoints();
            var gBreaks = g.GetBreakPoints();

            // вычисляем частное точек разрыва, оставляем только уникальные
            var breakPointProducts = new List<double>();
            foreach (var fBreak in fBreaks)
            {
                foreach (var gBreak in gBreaks)
                {
                    var product = fBreak / gBreak;
                    if (!double.IsNaN(product) && !breakPointProducts.Contains(product)) // добавить проверку на мин. значение произведения
                    {
                        breakPointProducts.Add(product);
                    }
                }
            }

            return breakPointProducts;
        }

        private static (bool, bool) DistrSigns(PiecewiseFunction function)
        {
            var fNeg = function.Segments.Any(s => s.A < 0);
            var fPos = function.Segments.Any(s => s.B > 0);

            return (fPos, fNeg);
        }

        private static (bool, bool, bool) DistrZeroSigns(PiecewiseFunction function)
        {
            var gZero = false;
            var gZeroPos = false;
            var gZeroNeg= false;
            
            foreach(var segment in function.Segments)
            {
                if (segment.A <= 0 && 0 <= segment.B)
                {
                    gZero = true;
                    if (segment.A < 0)
                    {
                        gZeroNeg = true;
                    }
                    if (segment.B > 0)
                    {
                        gZeroPos = true;
                    }
                }
            }

            return (gZero, gZeroPos, gZeroNeg);
        }

        private static PiecewiseFunction GetSplittedPiecewiseFunction(PiecewiseFunction function)
        {
            var breaks = function.GetBreakPoints();
            // непонятное условие
            //if (breaks.First() > 0 || breaks.Last() < 0)
            //{
            //    return function;
            //}

            if (!breaks.Contains(0))
            {
                breaks.Add(0);
            }

            breaks.Sort();

            var index = breaks.IndexOf(0);
            if (index == breaks.Count - 2) // предпоследний
            {
                if (double.IsInfinity(breaks[index + 1]) && !breaks.Contains(-1))
                {
                    breaks.Add(-1);
                }
                else
                {
                    var valueToInsert = breaks[index + 1] / 2;
                    if (!breaks.Contains(valueToInsert))
                    {
                        breaks.Add(valueToInsert);
                    }
                }
            }

            if (index == 1) // второй
            {
                if (double.IsInfinity(breaks[index - 1]) && !breaks.Contains(-1))
                {
                    breaks.Add(-1);
                }
                else
                {
                    var valueToInsert = breaks[index - 1] / 2;
                    if (!breaks.Contains(valueToInsert))
                    {
                        breaks.Add(valueToInsert);
                    }
                }
            }

            breaks.Sort();

            return function.SplitByPoints(breaks);
        }
    }
}
