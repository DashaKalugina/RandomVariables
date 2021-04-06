using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomVariablesLibraryNew.Segments
{
    public class Segment
    {
        public double A { get; }

        public double B { get; }

        public Func<double, double> ProbabilityFunction { get; }

        public Segment(double a, double b, Func<double, double> probabilityFunction)
        {
            A = a;
            B = b;
            ProbabilityFunction = probabilityFunction;
        }

        /// <summary>
        /// Возвращает значение функции плотности в данной точке сегмента
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public double this[double x]
        {
            get
            {
                return ProbabilityFunction(x);
            }
        }

        public List<Point> GetPoints(double xMin, double xMax, int numberOfPoints)
        {
            var points = new List<Point>();

            var leftPoint = FindLeftPoint();
            var rightPoint = FindRightPoint();

            xMin = Math.Max(xMin, leftPoint);
            xMax = Math.Min(xMax, rightPoint);

            if (xMin >= xMax)
            {
                return new List<Point>();
            }

            var step = (xMax - xMin) / (numberOfPoints - 1);

            for (var i = 0; i < numberOfPoints; i++)
            {
                var x = xMin + i * step;
                var y = ProbabilityFunction(x);
                points.Add(new Point(x, y));
            }

            //var args = Enumerable.Range(0, numberOfPoints)
            //    .Select(i => xMin + (xMax - xMin) * ((double)i / (numberOfPoints - 1)));

            return points;
        }

        protected virtual double FindLeftPoint() => A;

        protected virtual double FindRightPoint() => B;
    }
}
