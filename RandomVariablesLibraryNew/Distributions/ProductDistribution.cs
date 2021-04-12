﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomVariablesLibraryNew.Distributions
{
    public class ProductDistribution: Distribution
    {
        public Distribution Distr1 { get; set; }

        public Distribution Distr2 { get; set; }

        public ProductDistribution(Distribution distr1, Distribution distr2)
        {
            Distr1 = distr1;
            Distr2 = distr2;

            PiecewisePDF = ProductConvolutionCalculator.Calculate(distr1.PiecewisePDF, distr2.PiecewisePDF);
        }

        public override double GetNewRandomValue()
        {
            return Distr1.GetNewRandomValue() * Distr2.GetNewRandomValue();
        }
    }
}
