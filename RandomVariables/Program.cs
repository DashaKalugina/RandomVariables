using RandomVariablesLibraryNew.Distributions.Custom;
using RandomVariablesLibraryNew.Distributions.Standard;
using RandomVariablesLibraryNew.Generators;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
            //var x = new double[]
            //{
            //    -1, 0, 1, 2, 3, 4
            //};
            //var y = new double[x.Length];

            //for (var i = 0; i < x.Length; i++)
            //{
            //    y[i] = Math.Pow(x[i], 2);
            //}

            //alglib.spline1dinterpolant splineInterpolant;
            //alglib.spline1dbuildcubic(x, y, out splineInterpolant);

            //var point = 0.5;
            //var result = alglib.spline1dcalc(splineInterpolant, point);




            // Произведение двух стандартных нормальных распределений
            //var normalVar1 = new NormalDistribution(0, 1);
            //var normalVar2 = new NormalDistribution(0, 1);

            //var productOfNormalVars = uniformVar1 * uniformVar2 + uniformVar4 * uniformVar5;

            //var L0 = new UniformDistribution(9, 10);
            //var L1 = new UniformDistribution(11, 12);
            //var dt = new NormalDistribution(1, 2);
            //var productOfNormalVars = (L1 / L0 - 1) / dt;

            //var productOfNormalVars = (normalVar1 / normalVar2);

            //Console.WriteLine("\nПроизведение двух стандартных нормальных распределений");
            //Console.WriteLine(productOfNormalVars.SummaryInfo);

            //string path = @"c:\RandomData\data.txt";
            //var readText = File.ReadAllLines(path);
            //var variableValues = readText.Select(text => double.Parse(text)).ToArray();

            var variableValues = new List<double>();
            for (var i = 0; i < 10000000; i++)
            {
                variableValues.Add(NormalGenerator.Next(0, 1));
                //variableValues.Add(UniformGenerator.Next(0, 1));
            }

            var minValue1 = variableValues.Min();
            var maxValue1 = variableValues.Max();

            var minValue2 = variableValues.Min();
            var maxValue2 = variableValues.Max();

            //var customDistribution = new CustomDistribution(variableValues.ToArray()) * new CustomDistribution(variableValues.ToArray());
            var customDistribution = new CustomDistribution(variableValues.ToArray()) * new NormalDistribution(0, 1);

            Console.WriteLine("\nПользовательское распределение");
            Console.WriteLine(customDistribution.SummaryInfo);

            var data = customDistribution.GetPDFDataForPlot(-14, 14).ToArray();
            //var data = customDistribution.ProbabilityFunctionValues;

            var series51 = MakeSeries(data);

            var chart51 = MakeChart(series51, "x", "f(x)", "График f(x) пользовательского распределения",
                SeriesChartType.Column, 2, -14, 14);

            //////////////////////////////////////////////////////
            ///
            var normalVar1 = new NormalDistribution(0, 1) * new NormalDistribution(0, 1);
            //var normalVar1 = new UniformDistribution(0, 1) + new UniformDistribution(0, 1);

            var series61 = MakeSeries(normalVar1.GetPDFDataForPlot(-14, 14).ToArray());

            var chart61 = MakeChart(series61, "x", "f(x)", "График f(x) нормального распределения",
                SeriesChartType.Column, 2, -14, 14);

            ////////////////////////////////////////////////////


            var forms = MakeFormsFromCharts(new List<Chart> { 
                chart51,
                chart61
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
            series.ChartType = SeriesChartType.Point; //
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
            //chart.ChartAreas[0].AxisY.Interval = 1;
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
