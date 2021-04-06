using RandomVariablesLibraryNew.Distributions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Point = RandomVariablesLibraryNew.Point;

namespace RandomVariables
{
    class Program
    {
        static void Main(string[] args)
        {
            // Равномерное распределение
            var uniformVariable = new UniformDistribution(0, 1);

            Console.WriteLine("\nРавномерное распределение");
            Console.WriteLine($"{uniformVariable.SummaryInfo}");

            var series11 = MakeSeries(uniformVariable.GetPDFDataForPlot(-0.1, 1.1).ToArray());
            var chart11 = MakeChart(series11, "x", "f(x)", "График f(x) равномерного распределения (A = 0, B = 1)", 
                SeriesChartType.Column, 0.1, -0.1, 1.1);

            //var series12 = MakeSeries(uniformVariable.DistributionFunctionValues);
            //var chart12 = MakeChart(series12, "x", "f(x)", "График F(x) равномерного распределения (A = 0, B = 1)", 
            //    SeriesChartType.Point, 0.1, -0.1, 1.1);

            // Нормальное распределение
            var normalVariable = new NormalDistribution(0, 1);

            Console.WriteLine("\nНормальное распределение");
            Console.WriteLine($"{normalVariable.SummaryInfo}");

            var series21 = MakeSeries(normalVariable.GetPDFDataForPlot(-5, 5).ToArray());
            var chart21 = MakeChart(series21, "x", "f(x)", 
                "График f(x) стандартного нормального распределения (Mu = 0, Sigma = 1)", 
                SeriesChartType.Column, 1, -5, 5);

            //var series22 = MakeSeries(normalVariable.DistributionFunctionValues);
            //var chart22 = MakeChart(series22, "x", "f(x)", 
            //    "График F(x) стандартного нормального распределения (Mu = 0, Sigma = 1)", 
            //    SeriesChartType.Point, 1, -6, 6);

            // Экспоненциальное распределение
            var exponentialVariable = new ExponentialDistribution(0.5);

            Console.WriteLine("\nЭкспоненциальное распределение");
            Console.WriteLine($"{exponentialVariable.SummaryInfo}");

            var series31 = MakeSeries(exponentialVariable.GetPDFDataForPlot(0, 28).ToArray());
            var chart31 = MakeChart(series31, "x", "f(x)", "График f(x) экспоненциального распределения (Lambda = 0.5)",
                SeriesChartType.Column, 2, 0, 28);

            //var series32 = MakeSeries(exponentialVariable.DistributionFunctionValues);
            //var chart32 = MakeChart(series32, "x", "F(x)", "График F(x) экспоненциального распределения (Lambda = 0.5)",
            //    SeriesChartType.Point, 2, 0, 28);

            //Сумма равномерных от 0 до 1
            var uniformVar1 = new UniformDistribution(0, 1);
            var uniformVar2 = new UniformDistribution(0, 1);

            var sumOfUniformVars = uniformVar1 + uniformVar2;

            Console.WriteLine("\nСумма двух равномерных СВ от 0 до 1");
            Console.WriteLine(sumOfUniformVars.SummaryInfo);

            var series41 = MakeSeries(sumOfUniformVars.GetPDFDataForPlot(-1, 3).ToArray());
            var chart41 = MakeChart(series41, "x", "f(x)", "График f(x) суммы равномерных СВ от 0 до 1",
                SeriesChartType.Column, 0.2, -0.2, 2.2);

            //var series42 = MakeSeries(sumOfUniformVars.DistributionFunctionValues);
            //var chart42 = MakeChart(series42, "x", "F(x)", "График F(x) суммы равномерных СВ от 0 до 1",
            //    SeriesChartType.Point, 0.2, -0.2, 2.2);

            // Произведение двух стандартных нормальных распределений
            var normalVar1 = new NormalDistribution(0, 1);
            var normalVar2 = new NormalDistribution(0, 1);

            var productOfNormalVars = normalVar1 + normalVar2;

            Console.WriteLine("\nПроизведение двух стандартных нормальных распределений");
            Console.WriteLine(productOfNormalVars.SummaryInfo);

            var series51 = MakeSeries(productOfNormalVars.GetPDFDataForPlot(-26, 26).ToArray());
            var chart51 = MakeChart(series51, "x", "f(x)", "График f(x) произведения двух норм. СВ с Mu = 0, Sigma = 1",
                SeriesChartType.Column, 2, -26, 26);

            //var series52 = MakeSeries(productOfNormalVars.DistributionFunctionValues);
            //var chart52 = MakeChart(series52, "x", "F(x)", "График F(x) произведения двух норм. СВ с Mu = 0, Sigma = 1",
            //    SeriesChartType.Point, 2, -26, 26);

            var forms = MakeFormsFromCharts(new List<Chart> { 
                chart11, 
                //chart12, 
                chart21, 
                //chart22, 
                chart31, 
                //chart32, 
                chart41, 
                //chart42, 
                chart51, 
                //chart52 
            });

			Application.EnableVisualStyles();
			Application.Run(new MultiFormContext(forms.ToArray()));

			Console.ReadKey();
		}

        private static Series MakeSeries(Point[] values)
        {
            var dataPoints = new List<DataPoint>();

            for (int i = 0; i < values.Length; i++)
            {
                dataPoints.Add(new DataPoint(values[i].X, values[i].Y));
            }

            var series = new Series();
            dataPoints.ForEach(x => series.Points.Add(x));

            return series;
        }

		private static Chart MakeChart(Series series, string axisXTitle, string axisYTitle, 
            string chartTitle, SeriesChartType chartType, double interval, double min, double max)
		{
			var chart = new Chart();
			chart.ChartAreas.Add(new ChartArea());

            series.ChartType = chartType;
            //series.ChartType = SeriesChartType.Point;
            series.Color = Color.RoyalBlue;
			series.BorderWidth = 3;

			chart.Series.Add(series);
			chart.Dock = DockStyle.Fill;
			Axis ax = new Axis();
			ax.Title = axisXTitle;
			chart.ChartAreas[0].AxisX = ax;
			Axis ay = new Axis();
			ay.Title = axisYTitle;
			chart.ChartAreas[0].AxisY = ay;
            chart.ChartAreas[0].AxisX.Interval = interval;
            chart.ChartAreas[0].AxisX.Minimum = min;
            chart.ChartAreas[0].AxisX.Maximum = max;
            chart.Titles.Add(chartTitle);
			return chart;
		}

		private static IEnumerable<Form> MakeFormsFromCharts(IEnumerable<Chart> charts)
		{
			var forms = new List<Form>();
			foreach (var chart in charts)
			{
				var form = new Form();
				form.ClientSize = new Size(800, 600);
				form.Controls.Add(chart);
				forms.Add(form);
			}

			return forms;
		}
	}
}
