using RandomVariablesLibrary.Distributions.Custom;
using RandomVariablesLibrary.Distributions.Standard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RandomVariables.WebApplication.Models
{
    public static class DistributionNames
    {
        //public static readonly Dictionary<string, string> ShortDistrNamesByFullNames = new Dictionary<string, string>
        //{
        //    { nameof(UniformDistribution), "Uniform" },
        //    { nameof(NormalDistribution), "Normal" },
        //    { nameof(ExponentialDistribution), "Exponential" },
        //    { nameof(ChiSquareDistribution), "ChiSquare" },
        //    { nameof(GammaDistribution), "Gamma" },
        //    { nameof(CauchyDistribution), "Cauchy" },
        //    { nameof(FDistribution), "FDistr" }
        //};

        public static readonly Dictionary<string, string> FullDistrNamesByShortNames = new Dictionary<string, string>
        {
            { "Uniform", nameof(UniformDistribution)},
            { "Normal", nameof(NormalDistribution) },
            { "Exponential", nameof(ExponentialDistribution) },
            { "ChiSquare", nameof(ChiSquareDistribution) },
            { "Gamma", nameof(GammaDistribution) },
            { "Cauchy", nameof(CauchyDistribution) },
            { "FDistr", nameof(FDistribution) },
            { "WeibullDistr", nameof(WeibullDistribution)},
            { "CustomDistr", nameof(CustomDistribution)}
        };

        //public static readonly IEnumerable<string> FullDistrNames = new List<string>
        //{
        //        nameof(UniformDistribution),
        //        nameof(NormalDistribution),
        //        nameof(ExponentialDistribution),
        //        nameof(ChiSquareDistribution),
        //        nameof(GammaDistribution),
        //        nameof(CauchyDistribution),
        //        nameof(FDistribution),
        //        nameof(WeibullDistribution)
        //};

        public static readonly Dictionary<string, List<string>> ParametersByDistributionsName = new Dictionary<string, List<string>>
        {
                { nameof(UniformDistribution), new List<string> { "A (начало отрезка)", "B (конец отрезка)"} },
                { nameof(NormalDistribution), new List<string> { "μ (мат. ожидание)", "σ (среднеквадр. отклонение)" } },
                { nameof(ExponentialDistribution), new List<string> { "λ (обр. коэффициент масштаба)" } },
                { nameof(ChiSquareDistribution), new List<string> { "k (число степеней свободы)"} },
                { nameof(GammaDistribution), new List<string> { "k (параметр формы)", "θ (параметр масштаба)" } },
                { nameof(CauchyDistribution), new List<string> { "x0 (параметр сдвига)", "γ (параметр масштаба)" } },
                { nameof(FDistribution), new List<string> { "d1 (число степеней свободы)", "d2 (число степеней свободы)"} },
                { nameof(WeibullDistribution), new List<string> { "k (коэффициент формы)", "λ (коэффициент масштаба)" } }
        };
    }
}
