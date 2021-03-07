using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomVariablesLibraryNew.Distributions
{
    public abstract class Distribution
    {
        /// <summary>
        /// Кусочно-заданная функция плотности распределения
        /// </summary>
        public PiecewiseFunction PiecewisePDF;

        
    }
}
