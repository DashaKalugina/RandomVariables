using RandomVariablesLibraryNew.Segments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomVariablesLibraryNew.ConvolutionCalculators
{
    public static class SumConvolutionCalculator
    {
        public static PiecewiseFunction Calculate(PiecewiseFunction f, PiecewiseFunction g)
        {
            var breaks = GetResultBreaks(f, g);

            var resultPiecewiseFunction = new PiecewiseFunction();

            if (breaks.Count > 1 && double.IsNegativeInfinity(breaks[0].X))
            {
                var appropriateSegments = FindSegments(f, g, breaks[1].X - 1);
                var convRunner = new ConvolutionRunner(appropriateSegments);

                Func<double, double> probabilityFunction = (x) => convRunner.GetConvolutionValueAtPoint(x);

                var minusInfSegment = new MinusInfinitySegment(breaks[1].X, probabilityFunction);
                resultPiecewiseFunction.AddSegment(minusInfSegment);

                breaks.RemoveAt(0);
            }

            if (breaks.Count > 1 && double.IsPositiveInfinity(breaks[breaks.Count - 1].X))
            {
                var appropriateSegments = FindSegments(f, g, breaks[breaks.Count - 2].X + 1);
                var convRunner = new ConvolutionRunner(appropriateSegments);

                Func<double, double> probabilityFunction = (x) => convRunner.GetConvolutionValueAtPoint(x);

                var plusInfSegment = new PlusInfinitySegment(breaks[breaks.Count - 2].X, probabilityFunction);
                resultPiecewiseFunction.AddSegment(plusInfSegment);

                breaks.RemoveAt(breaks.Count - 1);
            }

            for (var i = 0; i < breaks.Count - 1; i++)
            {
                #region Comment
                //if (i == 0 && double.IsNegativeInfinity(breaks[0].X))
                //{
                //    var appropriateSegments = FindSegments(f, g, breaks[1].X - 1);
                //    var convRunner = new ConvolutionRunner(appropriateSegments);

                //    Func<double, double> probabilityFunction = (x) => convRunner.GetConvolutionValueAtPoint(x);

                //    var minusInfSegment = new MinusInfinitySegment(breaks[1].X, probabilityFunction);
                //    resultPiecewiseFunction.AddSegment(minusInfSegment);

                //    continue;
                //}

                //if (i == breaksCount - 1 && double.IsPositiveInfinity(breaks[breaksCount - 1].X))
                //{
                //    var appropriateSegments = FindSegments(f, g, breaks[breaksCount - 2].X + 1);
                //    var convRunner = new ConvolutionRunner(appropriateSegments);

                //    Func<double, double> probabilityFunction = (x) => convRunner.GetConvolutionValueAtPoint(x);

                //    var plusInfSegment = new PlusInfinitySegment(breaks[breaksCount - 2].X, probabilityFunction);
                //    resultPiecewiseFunction.AddSegment(plusInfSegment);

                //    continue;
                //}
                #endregion

                var segments = FindSegments(f, g, (breaks[i].X + breaks[i + 1].X) / 2);
                var runner = new ConvolutionRunner(segments);

                Func<double, double> func = (x) => runner.GetConvolutionValueAtPoint(x);
                var newSegment = new Segment(breaks[i].X, breaks[i + 1].X, func);

                resultPiecewiseFunction.AddSegment(newSegment);

            }

            return resultPiecewiseFunction;
        }

        private static List<BreakPoint> GetResultBreaks(PiecewiseFunction f, PiecewiseFunction g)
        {
            var resultBreaks = new List<BreakPoint>();

            var fbreaks = f.GetBreakPointsExtended();
            var gbreaks = g.GetBreakPointsExtended();

            var hasMinusInfinity = false;
            var hasPlusInfinity = false;

            // Проходим по точкам функции f
            foreach (var fbreak in fbreaks)
            {
                if (double.IsNegativeInfinity(fbreak.X))
                {
                    hasMinusInfinity = true;
                    continue;
                }
                if (double.IsPositiveInfinity(fbreak.X))
                {
                    hasPlusInfinity = true;
                    continue;
                }

                // Проходим по точкам функции g
                foreach (var gbreak in gbreaks)
                {
                    if (double.IsNegativeInfinity(gbreak.X))
                    {
                        hasMinusInfinity = true;
                        continue;
                    }
                    if (double.IsPositiveInfinity(gbreak.X))
                    {
                        hasPlusInfinity = true;
                        continue;
                    }
                    // здесь будет добавлена обработка функций Дирака

                    var newBreak = new BreakPoint(fbreak.X + gbreak.X, false, false);
                    resultBreaks.Add(newBreak);
                }
            }

            // нужно возвращать уникальные и отсортированные точки
            resultBreaks.Sort(delegate (BreakPoint b1, BreakPoint b2)
            {
                return b1.X.CompareTo(b2.X);
            });

            if (hasMinusInfinity)
            {
                resultBreaks = resultBreaks.Prepend(new BreakPoint(double.NegativeInfinity, false, false)).ToList();
            }

            if (hasPlusInfinity)
            {
                resultBreaks = resultBreaks.Append(new BreakPoint(double.PositiveInfinity, false, false)).ToList();
            }

            var uniqueBreakPoints = GetUniqueBreakPoints(resultBreaks);

            return uniqueBreakPoints;
        }

        private static List<BreakPoint> GetUniqueBreakPoints(List<BreakPoint> breakPoints)
        {
            var uniqueBreakPoints = new List<BreakPoint>();

            foreach(var breakPoint in breakPoints)
            {
                if (!uniqueBreakPoints.Exists(b => b.X == breakPoint.X))
                {
                    uniqueBreakPoints.Add(breakPoint);
                }
            }

            return uniqueBreakPoints;
        }

        private static List<Tuple<Segment, Segment>> FindSegments(PiecewiseFunction f, PiecewiseFunction g, double z)
        {
            var segmentTuplesList = new List<Tuple<Segment, Segment>>();

            foreach(var fseg in f.Segments)
            {
                foreach(var gseg in g.Segments)
                {
                    if (fseg.A + gseg.A < z && fseg.B + gseg.B > z)
                    {
                        segmentTuplesList.Add(new Tuple<Segment, Segment>(fseg, gseg));
                    }
                }
            }

            return segmentTuplesList;
        }
    }
}
