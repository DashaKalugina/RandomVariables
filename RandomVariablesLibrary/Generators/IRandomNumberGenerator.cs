using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomVariables
{
    public interface IRandomNumberGenerator
    {
        double Mean { get; }
        double Variance { get; }
        double Next();
        //void SetSeed(int seed);
    }
}
