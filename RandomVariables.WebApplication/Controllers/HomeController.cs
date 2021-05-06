using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RandomVariables.WebApplication.Models;
using RandomVariablesLibrary.Distributions.Standard;
using RandomVariables.WebApplication.ViewModels;
using Newtonsoft.Json;

namespace RandomVariables.WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            //var standardDistributions = new List<string>
            //{
            //    nameof(UniformDistribution),
            //    nameof(NormalDistribution),
            //    nameof(ExponentialDistribution),
            //    nameof(ChiSquareDistribution),
            //    nameof(GammaDistribution),
            //    nameof(CauchyDistribution),
            //    nameof(FDistribution),
            //    nameof(WeibullDistribution)
            //};

            var parametersByDistributionsName = new Dictionary<string, List<string>>
            {
                { nameof(UniformDistribution), new List<string> { "A (начало отрезка)", "B (конец отрезка)"} },
                { nameof(NormalDistribution), new List<string> { "μ (мат. ожидание)", "σ (среднеквадр. отклонение)" } },
                { nameof(ExponentialDistribution), new List<string> { "λ (обр. коэффициент масштаба)" } },
                { nameof(ChiSquareDistribution), new List<string> { "k (число степеней свободы)"} },
                { nameof(GammaDistribution), new List<string> { "k (параметр формы)", "θ (параметр масштаба)" } },
                { nameof(CauchyDistribution), new List<string> { "x0 (параметр сдвига)", "γ (параметр масштаба)" } },
                { nameof(FDistribution), new List<string> { "d1 (число степеней свободы)", "d2 (число степеней свободы)"} }
            };

            return View(new CalculatorPageViewModel { ParametersByDistributionsName = parametersByDistributionsName });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<JsonResult> EvaluateExpression(string expression)
        {
            var normalDistr = new NormalDistribution(0, 1);
            var data = normalDistr.GetPDFDataForPlot();
            var x = data.Select(p => p.X);
            var y = data.Select(p => p.Y);

            return Json(new { x = JsonConvert.SerializeObject(x), y = JsonConvert.SerializeObject(y) });
        }
    }
}
