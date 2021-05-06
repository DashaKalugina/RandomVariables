using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RandomVariables.WebApplication.ViewModels
{
    public class CalculatorPageViewModel
    {
        //public List<string> StandardDistributions { get; set; }
        public Dictionary<string, List<string>> ParametersByDistributionsName { get; set; }

        //public Dictionary<string, string> ParametersByDistributionsName { get; set; }
    }
}
