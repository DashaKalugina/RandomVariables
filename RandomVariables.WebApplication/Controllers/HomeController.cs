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
            var shortDistrNamesByFullNames = DistributionNames.FullDistrNamesByShortNames.ToDictionary(k => k.Value, v => v.Key);
            var viewModel = new CalculatorPageViewModel
            {
                ParametersByDistributionsName = DistributionNames.ParametersByDistributionsName,
                ShortDistrNamesByFullNames = shortDistrNamesByFullNames
            };

            return View(viewModel);
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
        public JsonResult EvaluateExpression(string expression)
        {
            var resultDistr = FormulaRecognitionService.GetResultFormula(expression);
            //var pdfData = resultDistr.GetPDFDataForPlot();
            //var x1 = pdfData.Select(p => p.X);
            //var y1 = pdfData.Select(p => p.Y);

            //var cdfData = resultDistr.GetCDFDataForPlot();
            //var x2 = cdfData.Select(p => p.X);
            //var y2 = cdfData.Select(p => p.Y);

            //return Json(new 
            //{
            //    pdf = new
            //    {
            //        x = JsonConvert.SerializeObject(x1),
            //        y = JsonConvert.SerializeObject(y1),
            //    },
            //    cdf = new
            //    {
            //        x = JsonConvert.SerializeObject(x2),
            //        y = JsonConvert.SerializeObject(y2)
            //    }          
            //});

            var pdfData = resultDistr.GetPDFDataForPlot();
          //  var x1 = pdfData.Select(p => p.X);
          //  var y1 = pdfData.Select(p => p.Y);

            var cdfData = resultDistr.GetCDFDataForPlot();
          //  var x2 = cdfData.Select(p => p.X);
           // var y2 = cdfData.Select(p => p.Y);

            return Json(new
            {
                pdf = new
                {
                    x = JsonConvert.SerializeObject(pdfData.Select(p => p.X)),
                    y = JsonConvert.SerializeObject(pdfData.Select(p => p.Y)),
                },
                cdf = new
                {
                    x = JsonConvert.SerializeObject(cdfData.Select(p => p.X)),
                    y = JsonConvert.SerializeObject(cdfData.Select(p => p.Y))
                }
            });
        }
    }
}
