using RandomVariablesLibrary.Distributions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace RandomVariables
{
    class Program
    {
        static void Main(string[] args)
        {
            // Равномерное распределение
            var a = 0.0;
            var b = 1.0;
            var uniformDistributedVariable = new RandomVariable(new UniformDistribution { A = a, B = b });

            Console.WriteLine("\nРавномерное распределение");
            Console.WriteLine($"{uniformDistributedVariable.SummaryInfo}");

            var series1 = MakeSeries(uniformDistributedVariable);
            var chart1 = MakeChart(series1, "Х", "f(x)", "График функции плотности равномерного распределения");

            // Нормальное распределение
            var mu = 0.0;
            var sigma = 1.0;
            var normalVariable = new RandomVariable(new NormalDistribution { Mu = mu, Sigma = sigma });

            Console.WriteLine("\nНормальное распределение");
            Console.WriteLine($"{normalVariable.SummaryInfo}");

            var series2 = MakeSeries(normalVariable);
            var chart2 = MakeChart(series2, "Х", "f(x)", "График функции плотности нормального распределения");

            //////////////////////////


            var a1 = 0.0;
            var b1 = 1.0;
            var var1 = new RandomVariable(new UniformDistribution { A = a1, B = b1 });

            var a2 = 0.0;
            var b2 = 1.0;
            var var2 = new RandomVariable(new UniformDistribution { A = a2, B = b2 });

            //var var3 = var1 + var1;
            //var var3Values = var3.GetVariableValues();
            //var var3Probs = var3.GetProbabilityValues();

            //var var3 = new RandomVariable(new ExponentialDistribution { Lambda = 0.5 });
            var var3 = var1 + var2;
            var series3 = MakeSeries(var3);
            var chart3 = MakeChart(series3, "Х", "f(x)", "График суммы равномерных от 0 до 1");
            Console.WriteLine(var3.SummaryInfo);

            ////////////////////////////////////////////////////////////
            var forms = MakeFormsFromCharts(new List<Chart> { chart1, chart2, chart3 });

			Application.EnableVisualStyles();
			Application.Run(new MultiFormContext(forms.ToArray()));

			Console.ReadKey();
		}

        private static Series MakeSeries(RandomVariable randomVariable)
        {
            var probabilities = randomVariable.ProbabilityFunctionValues;

            var dataPoints = new List<DataPoint>();

            for (int i = 0; i < probabilities.Length; i++)
            {
                dataPoints.Add(new DataPoint(probabilities[i].X, probabilities[i].Y));
            }

            var series = new Series();
            dataPoints.ForEach(x => series.Points.Add(x));

            return series;
        }

        private static Series MakeSeries(IEnumerable<double> dataset)
		{
			var dataPoints = dataset
				.Select((x, index) => new DataPoint(index, x))
				.ToList();
			var series = new Series();
			dataPoints.ForEach(x => series.Points.Add(x));
			return series;
		}

		private static Chart MakeChart(Series series, string axisXTitle, string axisYTitle, string chartTitle)
		{
			var chart = new Chart();
			chart.ChartAreas.Add(new ChartArea());

            //series.ChartType = SeriesChartType.FastLine;
            //series.ChartType = SeriesChartType.Column;
            series.ChartType = SeriesChartType.Point;
            series.Color = Color.Green;
			series.BorderWidth = 3;

			chart.Series.Add(series);
			chart.Dock = DockStyle.Fill;
			Axis ax = new Axis();
			ax.Title = axisXTitle;
			chart.ChartAreas[0].AxisX = ax;
			Axis ay = new Axis();
			ay.Title = axisYTitle;
			chart.ChartAreas[0].AxisY = ay;
			chart.ChartAreas[0].AxisX.Interval = 1;
            //chart.ChartAreas[0].AxisX.Minimum = -1;
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
