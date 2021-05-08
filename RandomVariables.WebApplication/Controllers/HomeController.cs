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
using System.IO;

namespace RandomVariables.WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private const string CUSTOM_DISTR_FOLDER_NAME = "CustomDistrFiles";

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Удалим папку с пользовательскими распределениями при загрузке главной страницы. Если ее не было то создадим.
            var customDistrFolderName = Path.Combine(Directory.GetCurrentDirectory(), CUSTOM_DISTR_FOLDER_NAME);
            if (Directory.Exists(customDistrFolderName))
            {
                Directory.Delete(customDistrFolderName, true);
            }

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

        [HttpPost]
        public async Task<JsonResult> AddFile()
        {
            var fileToUpload = Request.Form.Files.FirstOrDefault();
            var fileExtention = Path.GetExtension(fileToUpload.FileName);
            var fileName = $"{Request.Form.FirstOrDefault(x => x.Key == "fileName").Value}{fileExtention}";
            if (fileToUpload == null)
            {
                return Json(new { MessageError = "Произошла ошибка при добавлении файла" });
            }
            try
            {
                var customDistrFolderName = Path.Combine(Directory.GetCurrentDirectory(), CUSTOM_DISTR_FOLDER_NAME);
                if (!Directory.Exists(customDistrFolderName))
                {
                    Directory.CreateDirectory(customDistrFolderName);
                }
                var filePath = Path.Combine(customDistrFolderName, fileName);
                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await fileToUpload.CopyToAsync(fileStream);
                }
                //var currentUser = await _userManager.GetUserAsync(HttpContext.User);
                //// Уменьшаем баланс согласно размеру файла.
                //currentUser.Balance -= (decimal)fileToUpload.Length / 1024;

                //var filesContainerName = currentUser.Id;

                //await _filesManagementService.UploadFileToUserContainer(filesContainerName, fileToUpload);

                //_context.SaveChanges();
                return Json(new { Ok = true, DistrName = Path.GetFileNameWithoutExtension(filePath) });
            }
            catch (Exception ex)
            {
                return Json(new { MessageError = ex.Message });
            }
        }
    }
}
