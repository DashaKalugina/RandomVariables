﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomVariablesLibraryNew.Distributions
{
    public class ShiftedScaledDistribution : Distribution
    {
        public double Shift { get; }

        public double Scale { get; }

        public Distribution Distribution { get; }

        public ShiftedScaledDistribution(Distribution distribution, double shift = 0, double scale = 1)
        {
            if (scale == 0)
            {
                throw new Exception("Параметр scale не может быть равным нулю");
            }

            Shift = shift;
            Scale = scale;

            PiecewisePDF = distribution.PiecewisePDF.GetShiftedAndScaledCopy(shift, scale);
        }

        public override double GetNewRandomValue()
        {
            return Scale * Distribution.GetNewRandomValue() + Shift;
        }
    }
}
