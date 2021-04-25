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

        //public AdaptiveBarycentricInterpolator(double[] xs, double[] ys, double[] weights = null)
        //{
        //    Xs = xs;
        //    Ys = ys;

        //    if (weights == null)
        //    {
        //        InitWeights();
        //    }
        //    else
        //    {
        //        Weights = weights;
        //    }
        //}

        public AdaptiveBarycentricInterpolator() { }

        public virtual double InterpolateAt(double x)
        {
            var xDiff = Xs.Select(xs => x - xs).ToList();
            for (var i = 0; i < xDiff.Count; i++)
            {
                if (xDiff[i] == 0)
                {
                    xDiff[i] = 1;
                }
            }

            var temp = new List<double>();
            for (var i = 0; i < xDiff.Count; i++)
            {
                temp.Add(Weights[i] / xDiff[i]);
            }

            var scalarProduct = 0.0; // num
            for (var i = 0; i < temp.Count; i++)
            {
                scalarProduct += temp[i] * Ys[i];
            }

            var tempSum = temp.Sum(); // den

            if (tempSum == 0)
            {
                // обработать
            }

            var result = scalarProduct / tempSum;

            return result;
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

            InitBarycentricInterpolator(Xs, Ys);
        }

        protected void AdaptiveInterpolate()
        {
            var maxN = 200;
            var oldError = default(double);

            while(n <= maxN)
            {
                var new_N = 2 * n - 1;
                var new_Xs = GetIncrementalNodes(new_N);
                var new_Ys = F(new_Xs); // AdaptiveInterpolator
            }
        }

        protected virtual double[] GetIncrementalNodes(int n)
        {
            return default;
        }

        private void InitBarycentricInterpolator(double[] xs, double[] ys)
        {
            if (Weights == null)
            {
                InitWeights();
            }
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
