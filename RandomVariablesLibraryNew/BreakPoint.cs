using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomVariablesLibraryNew
{
    public class BreakPoint
    {
        public double X { get; }

        public bool NegativePole { get; set; }

        public bool PositivePole { get; set; }

        public bool Сontinuous { get; set; }

        public bool IsDirac { get; set; }

        public BreakPoint(double x, bool negPole, bool posPole, bool cont = true, bool isDirac = false)
        {
            X = x;
            NegativePole = negPole;
            PositivePole = posPole;
            Сontinuous = cont;
            IsDirac = isDirac;
        }
    }
}
