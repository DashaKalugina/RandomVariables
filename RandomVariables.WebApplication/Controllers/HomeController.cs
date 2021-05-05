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
            var standardDistributions = new List<string>
            {
                nameof(UniformDistribution),
                nameof(NormalDistribution),
                nameof(ExponentialDistribution),
                nameof(ChiSquareDistribution),
                nameof(GammaDistribution),
                nameof(CauchyDistribution),
                nameof(FDistribution),
                nameof(WeibullDistribution)
            };

            return View(new CalculatorPageViewModel { StandardDistributions = standardDistributions});
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
    }
}
