using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomVariablesLibraryNew.Interpolators
{
    public class ChebyshevInterpolator: AdaptiveBarycentricInterpolator
    {
        public double A { get; set; }

        public double B { get; set; }

        public ChebyshevInterpolator()
        {
            //A = a;
            //B = b;
        }

        public void Init(Func<double[], double[]> f, double a, double b)
        {
            A = a;
            B = b;
            AdaptiveInit(f);
            AdaptiveInterpolate();
        }

        public override void InitWeights()
        {

        }

        public override double[] GetNodes(int n)
        {
            // Chebyshev nodes for given degree n
            if (n == 1)
            {
                return new double[] { 0.5 * (A + B) };
            }

            var nodes = new List<double>();
            for (var i=0; i<n; i++)
            {
                var node = 0.5 * (A + B) - 0.5 * (B - A) * Math.Cos(i * Math.PI / (n - 1));
                nodes.Add(node);
            }

            nodes[0] = A;
            nodes[nodes.Count - 1] = B; 

            return nodes.ToArray();
        }

        protected override double[] GetIncrementalNodes(int n)
        {
            var nodes = new List<double>();

            var current = 1;

            while(current < n - 1)
            {
                var node = 0.5 * (A + B) - 0.5 * (B - A) * Math.Cos(current * Math.PI / (n - 1));
                nodes.Add(node);

                current += 2;
            }

            return nodes.ToArray();
        }
    }
}
