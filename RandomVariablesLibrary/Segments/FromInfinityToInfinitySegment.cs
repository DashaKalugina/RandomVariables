﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomVariablesLibrary.Segments
{
    public class FromInfinityToInfinitySegment : Segment
    {
        public FromInfinityToInfinitySegment(Func<double, double> probabilityFunction)
            : base(double.NegativeInfinity, double.PositiveInfinity, probabilityFunction)
        {

        }

        public override double FindLeftPoint()
        {
            return (-1) * Math.Pow(10, 20);
        }

        public override double FindRightPoint()
        {
            return Math.Pow(10, 20);
        }
    }
}
