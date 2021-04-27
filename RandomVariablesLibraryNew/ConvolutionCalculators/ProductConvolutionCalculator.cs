using RandomVariablesLibraryNew.Segments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomVariablesLibraryNew.ConvolutionCalculators
{
    public static class ProductConvolutionCalculator
    {
        public static PiecewiseFunction Calculate(PiecewiseFunction f, PiecewiseFunction g)
        {
            //var breaks = GetResultBreaks(f, g);

            var fSplitted = f.SplitByPoints(new List<double> { -1, 0, 1 });
            var gSplitted = g.SplitByPoints(new List<double> { -1, 0, 1 });

            var f_0 = fSplitted[0];
            var g_0 = gSplitted[0];
            var poleAtZero = f_0 > 0 && g_0 > 0;

            var breaks = GetResultBreakPoints(fSplitted, gSplitted);
            
            Func<double, double, double> operation = (a, b) => a * b;

            var resultPiecewiseFunction = new PiecewiseFunction();

            //if (breaks.Count > 1 && double.IsNegativeInfinity(breaks[0]))
            if (double.IsInfinity(breaks[0]))
            {
                var appropriateSegments = FindSegments(fSplitted, gSplitted, breaks[1] - 1);
                var convRunner = new ConvolutionRunner(appropriateSegments);

                Func<double, double> probabilityFunction = (x) => convRunner.GetConvolutionValueAtPointProduct(x);

                var minusInfSegment = new MinusInfinitySegment(breaks[1], probabilityFunction);
                resultPiecewiseFunction.AddSegment(minusInfSegment);

                breaks.RemoveAt(0);
            }

            //if (breaks.Count > 1 && double.IsPositiveInfinity(breaks[breaks.Count - 1]))
            if (double.IsInfinity(breaks[breaks.Count - 1]))
            {
                var appropriateSegments = FindSegments(fSplitted, gSplitted, breaks[breaks.Count - 2] + 1);
                var convRunner = new ConvolutionRunner(appropriateSegments);

                Func<double, double> probabilityFunction = (x) => convRunner.GetConvolutionValueAtPointProduct(x);

                var plusInfSegment = new PlusInfinitySegment(breaks[breaks.Count - 2], probabilityFunction);
                resultPiecewiseFunction.AddSegment(plusInfSegment);

                breaks.RemoveAt(breaks.Count - 1);
            }

            for (var i = 0; i < breaks.Count - 1; i++)
            {
                var segments = FindSegments(fSplitted, gSplitted, (breaks[i] + breaks[i + 1]) / 2);
                var runner = new ConvolutionRunner(segments);
                Func<double, double> func = (x) => runner.GetConvolutionValueAtPointProduct(x);

                Segment newSegment = null;

                if (breaks[i] == 0)
                {
                    if (poleAtZero)
                    {
                        newSegment = new SegmentWithPole(breaks[i], breaks[i + 1], func, true);
                        //var segmentWithPole = new SegmentWithPole(breaks[i], breaks[i + 1], func, true);
                        //newSegment = segmentWithPole.ToInterpolatedSegment();
                    }
                    else
                    {
                        newSegment = new Segment(breaks[i], breaks[i + 1], func);
                    }
                }
                else if (breaks[i + 1] == 0)
                {
                    if (poleAtZero)
                    {
                        newSegment = new SegmentWithPole(breaks[i], breaks[i + 1], func, false);
                    }
                    else
                    {
                        newSegment = new Segment(breaks[i], breaks[i + 1], func);
                    }
                }
                else
                {
                    newSegment = new Segment(breaks[i], breaks[i + 1], func);
                }

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
                    //var products = new List<double>
                    //{
                    //    fseg.A * gseg.A,
                    //    fseg.A * gseg.B,
                    //    fseg.B * gseg.A,
                    //    fseg.B * gseg.B,
                    //};

                    var products = new List<double>
                    {
                        fseg.A * gseg.A,
                        fseg.B * gseg.B,
                        fseg.A * gseg.B,
                        fseg.B * gseg.A,
                    };
                    var uniqueProducts = products.Where(p => !double.IsNaN(p)).Distinct();
                    var minP = uniqueProducts.Min();
                    var maxP = uniqueProducts.Max();

                    if (minP < z && z < maxP)
                    {
                        segmentTuplesList.Add(new Tuple<Segment, Segment>(fseg, gseg));
                    }
                    //if (fseg.A + gseg.A < z && fseg.B + gseg.B > z)
                    //{
                    //    segmentTuplesList.Add(new Tuple<Segment, Segment>(fseg, gseg));
                    //}
                }
            }

            return segmentTuplesList;
        }

        private static List<double> GetResultBreakPoints(PiecewiseFunction f, PiecewiseFunction g)
        {
            // добавить обработку полюсов
            var fBreaks = f.GetBreakPoints();
            var gBreaks = g.GetBreakPoints();

            // вычисляем произведения точек разрыва, оставляем только уникальные
            var breakPointProducts = new List<double>();
            foreach (var fBreak in fBreaks)
            {
                foreach (var gBreak in gBreaks)
                {
                    var product = fBreak * gBreak;
                    if (!double.IsNaN(product) && !breakPointProducts.Contains(product)) // добавить проверку на мин. значение произведения
                    {
                        breakPointProducts.Add(product);
                    }
                }
            }

            breakPointProducts.Sort();

            return breakPointProducts;
        }
    }
}
