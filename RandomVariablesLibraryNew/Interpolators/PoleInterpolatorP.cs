using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomVariablesLibraryNew.Interpolators
{
    public class PoleInterpolatorP: ChebyshevInterpolatorNoL
    {
        public double Orig_A { get; set; }

        public double Orig_B { get; set; }

        public Func<double, double> WrappedF { get; set; }

        public int Sign { get; set; }

        public double Offset { get; set; }

        public PoleInterpolatorP(Func<double, double> f, double a, double b) : base(f, a, b)
        {
            if (f != null)
            {
                WrappedF = f;
            }

            Orig_A = a;
            Orig_B = b;

            Sign = Math.Sign(WrappedF((a + b) / 2));
            if (Sign == 0)
            {
                Sign = 1;
            }

            Offset = a == 0
                ? Math.Pow(10, -50)
                : Math.Abs(a) * double.Epsilon;

            Func<double[], double[]> wrappedF = (x) => Spec_F(x);
            Init(wrappedF, XtInv(Orig_A), XtInv(Orig_B));
        }

        public override double InterpolateAt(double x)
        {
            var result = Sign * (Math.Exp(Math.Abs(base.InterpolateAt(XtInv(x)))) - 1);

            return result;
        }

        public double[] GetNodes()
        {
            return default;
        }

        public double[] Spec_F(double[] args)
        {
            var res = args.Select(x => Math.Log(1 + Sign * WrappedF(Xt(x)))).ToArray();
            return res;
            //return Math.Log(1 + Sign * WrappedF(Xt(x)));
        }

        public double Spec_F(double x)
        {
            return Math.Log(1 + Sign * WrappedF(Xt(x)));
        }

        public double[] Xt(double[] args)
        {
            var res = args.Select(x => Math.Exp(x) + Orig_A + Offset).ToArray();
            return res;
            //return Math.Exp(x) + Orig_A + Offset;
        }

        public double Xt(double x)
        {
            return Math.Exp(x) + Orig_A + Offset;
        }

        public double[] XtInv(double[] args)
        {
            var res = args.Select(x => Math.Log(x - Orig_A + Offset)).ToArray();
            return res;
            //return Math.Log(x - Orig_A + Offset);
        }

        public double XtInv(double x)
        {
            return Math.Log(x - Orig_A + Offset);
        }
    }
}
