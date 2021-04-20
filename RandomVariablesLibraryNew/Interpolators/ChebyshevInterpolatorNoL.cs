using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomVariablesLibraryNew.Interpolators
{
    public class ChebyshevInterpolatorNoL : ChebyshevInterpolator
    {
        public ChebyshevInterpolatorNoL(Func<double, double> f, double a, double b)
        {
            
        }

        public override void InitWeights()
        {
            Weights = new double[Xs.Length];
            for (var i=0; i<Weights.Length; i++)
            {
                // четные делаем -1, нечетные 1
                Weights[i] = i % 2 == 0 ? -1 : 1;
            }

            var n = Xs.Length + 1;

            var res1 = new List<double>(); // промежут. рез-т
            for (var i = 0; i < n; i++)
            {
                res1.Add(Math.Sin(i * Math.PI / (n - 1) / 2));
            }
            res1.RemoveAt(0);
            res1.Reverse();

            for (var i = 0; i < n; i++)
            {
                Weights[i] = Weights[i] * 2 * Math.Pow(res1[i], 2);
                if (i % 2 == 0)
                {
                    Weights[i] = Weights[i] * (-1);
                }
            }
        }

        public override double[] GetNodes(int n)
        {
            var nodes = base.GetNodes(n);
            var res = nodes.ToList();
            res.RemoveAt(0);

            return res.ToArray();
        }

        

    }
}
