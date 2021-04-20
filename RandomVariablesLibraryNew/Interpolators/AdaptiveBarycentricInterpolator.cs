using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomVariablesLibraryNew.Interpolators
{
    public abstract class AdaptiveBarycentricInterpolator
    {
        public double[] Xs { get; set; }

        public double[] Ys { get; set; }

        public double[] Weights { get; set; }

        public Func<double[], double[]> F { get; set; }

        public int n { get; set; }

        public AdaptiveBarycentricInterpolator(double[] xs, double[] ys, double[] weights = null)
        {
            Xs = xs;
            Ys = ys;

            if (weights == null)
            {
                InitWeights();
            }
            else
            {
                Weights = weights;
            }
        }

        public abstract double[] GetNodes(int n);

        protected void AdaptiveInit(Func<double[], double[]> f)
        {
            if (f != null)
            {
                F = f;
            }

            n = 3;
            Xs = GetNodes(n);
            Ys = F(Xs);

            InitBarycentricInterpolator();
        }

        protected void AdaptiveInterpolate()
        {
            var maxN = 200;
            var oldError = default(double);

            while(n <= maxN)
            {
                var new_N = 2 * n - 1;
                var new_Xs = 
            }
        }

        protected virtual double[] GetIncrementalNodes(int n)
        {
            return default;
        }

        private void InitBarycentricInterpolator()
        {
            InitWeights();
        }

        public virtual void InitWeights()
        {
            // Using Lagrange method
            var weights = new List<double>();

            for (var i = 0; i < Xs.Length; i++)
            {
                var xi = Xs[i];
                var weight = 1.0;
                for (var j = 0; j < Xs.Length; j++)
                {
                    var xj = Xs[j];
                    if (j != i)
                    {
                        weight = weight / (xi - xj);
                    }
                }
                weights.Add(weight);
            }

            Weights = weights.ToArray();
        }
    }
}
