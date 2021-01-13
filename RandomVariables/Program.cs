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
            var b = 5.0;
            var uniformDistributedVariable = new RandomVariable(new UniformDistribution { A = a, B = b });

            var variableValues1 = uniformDistributedVariable.GetVariableValues();
            //var probabilityValues = uniformDistributedVariable.GetProbabilityValues();

            Console.WriteLine("\nРавномерное распределение");
            Console.WriteLine($"{uniformDistributedVariable.SummaryInfo}");

            var dataPoints1 = new List<DataPoint>();
            for (int i = 0; i < variableValues1.Length; i++)
            {
                dataPoints1.Add(new DataPoint(variableValues1[i], uniformDistributedVariable._distribution.ProbabilityFunction(variableValues1[i])));
            }

            var series1 = new Series();
            dataPoints1.ForEach(x => series1.Points.Add(x));

            var chart1 = MakeChart(series1, "Х", "f(x)", "График функции плотности равномерного распределения");

            // Нормальное распределение
            var mu = 0.0;
            var sigma = 1.0;
            var normalVariable = new RandomVariable(new NormalDistribution { Mu = mu, Sigma = sigma });
            var variableValues2 = normalVariable.GetVariableValues();
            var probabilityValues2 = normalVariable.GetProbabilityValues();

            Console.WriteLine("\nНормальное распределение");
            Console.WriteLine($"{normalVariable.SummaryInfo}");

            var dataPoints2 = new List<DataPoint>();
            for (int i = 0; i < variableValues2.Length; i++)
            {
				// правильно отображается
                dataPoints2.Add(new DataPoint(variableValues2[i], normalVariable._distribution.ProbabilityFunction(variableValues2[i])));

				//dataPoints2.Add(new DataPoint(variableValues2[i], probabilityValues2[i]));

				// доработать в этом направлении
				//var probValue = i > 0 && i < variableValues2.Length - 1 && probabilityValues2[i] != 0
				//	? probabilityValues2[i] / (variableValues2[i + 1] - variableValues2[i])
				//	: probabilityValues2[i];
				//dataPoints2.Add(new DataPoint(variableValues2[i], probValue));
			}

            var series2 = new Series();
            dataPoints2.ForEach(x => series2.Points.Add(x));

            var chart2 = MakeChart(series2, "Х", "f(x)", "График функции плотности нормального распределения");

			////////////////////////


			var a1 = 0.0;
			var b1 = 1.0;
			var var1 = new RandomVariable(new UniformDistribution { A = a1, B = b1 });

			var a2 = 0.0;
			var b2 = 1.0;
			var var2 = new RandomVariable(new UniformDistribution { A = a2, B = b2 });

			var var3 = var1 + var1;
			var var3Values = var3.GetVariableValues();
			var var3Probs = var3.GetProbabilityValues();

			var dataPoints3 = new List<DataPoint>();
			for (int i = 0; i < var3Values.Length; i++)
			{

				//dataPoints3.Add(new DataPoint(var3Values[i], var3Probs[i]));

                // доработать в этом направлении
                var probValue = i > 0 && i < var3Values.Length - 1 && var3Probs[i] != 0
                    ? var3Probs[i] / (var3Values[i + 1] - var3Values[i])
                    : var3Probs[i];
                dataPoints3.Add(new DataPoint(var3Values[i], probValue));
            }

			var series3 = new Series();
			dataPoints3.ForEach(x => series3.Points.Add(x));

			var chart3 = MakeChart(series3, "Х", "f(x)", "График суммы равномерных от 1 до 2");


			////////////////////////////////////////////////////////////
			var forms = MakeFormsFromCharts(new List<Chart> { chart1, chart2, chart3 });

			Application.EnableVisualStyles();
			Application.Run(new MultiFormContext(forms.ToArray()));

			Console.ReadKey();
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
			//chart.ChartAreas[0].AxisX.Interval = 25;
			//chart.ChartAreas[0].AxisX.Minimum = 0;
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
