using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace RandomVariables
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = 0;
            var b = 5;
            var uniformDistributedVariable = new RandomVariable(new UniformDistribution { A = a, B = b });
            var variableValues = uniformDistributedVariable.GetVariableValues();
            //var probabilityValues = uniformDistributedVariable.GetProbabilityValues();

            var dataPoints = new List<DataPoint>();
            for (int i = 0; i < variableValues.Length; i++)
            {
                //points.Add(new DataPoint(variableValues[i], uniformDistributedVariable._distribution.DistributionFunction(variableValues[i])));
                dataPoints.Add(new DataPoint(variableValues[i], uniformDistributedVariable._distribution.ProbabilityFunction(variableValues[i])));
                //points.Add(new DataPoint(probabilityValues[i], variableValues[i]));
            }

			var series = new Series();
			dataPoints.ForEach(x => series.Points.Add(x));

			var chart = MakeChart(series, "Х", "f(x)", "График функции плотности");

			var forms = MakeFormsFromCharts(new List<Chart> { chart });

			Application.EnableVisualStyles();

			Application.Run(new MultiFormContext(forms.ToArray()));
			Console.ReadKey();

			//var series = new LineSeries();
			//series.Points.AddRange(points);

			//PlotModel = new PlotModel();
			//PlotModel.Series.Add(series);
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

			series.ChartType = SeriesChartType.FastLine;
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
			chart.ChartAreas[0].AxisX.Interval = 25;
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
