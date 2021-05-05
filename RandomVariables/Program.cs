using Accord.Math;
using RandomVariablesLibraryNew.Distributions.Base;
using RandomVariablesLibraryNew.Distributions.Custom;
using RandomVariablesLibraryNew.Distributions.Standard;
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
            var path = @"C:\RandomData\data.txt";
            var textData = File.ReadAllLines(path);
            var distrData = textData.Select(t => double.Parse(t)).ToArray();

            var customDistribution = new CustomDistribution(distrData) + new NormalDistribution(0, 1);

            Console.WriteLine("\nПользовательское распределение");
            Console.WriteLine(customDistribution.SummaryInfo);

            var data = customDistribution.GetPDFDataForPlot().ToArray();

            var series51 = MakeSeries(data);

            var chart51 = MakeChart(series51, "x", "f(x)", "График f(x) пользовательского распределения",
                SeriesChartType.Column, 1, -10, 10);

            /////

            //var sum = new NormalDistribution(2, 1) + new NormalDistribution(0, 1);

            //Console.WriteLine("\nПользовательское распределение");
            //Console.WriteLine(sum.SummaryInfo);

            //var sumData = sum.GetPDFDataForPlot().ToArray();

            //var series52 = MakeSeries(sumData);

            //var chart52 = MakeChart(series52, "x", "f(x)", "График суммы двух нормальных",
            //    SeriesChartType.Column, 1, -10, 10);

            ////////////////////////////////////////////////////


            var x = 0.17;
            var gamma = Gamma.Function(x);

            //var x = new double[]
            //{
            //    -1, 0, 1, 2, 3, 4
            //};
            //var y = new double[x.Length];

            //for (var i = 0; i < x.Length; i++)
            //{
            //    y[i] = Math.Pow(x[i], 2);
            //}

            //var L0 = new UniformDistribution(9, 10);
            //var L1 = new UniformDistribution(11, 12);
            //var dt = new NormalDistribution(1, 2);
            //var productOfNormalVars = (L1 / L0 - 1) / dt;

            //////////////////////////////////////////////////////

            //var customDistribution = new CauchyDistribution(0, 0.5);

            //Console.WriteLine("\nПользовательское распределение");
            //Console.WriteLine(customDistribution.SummaryInfo);

            //var data = customDistribution.GetPDFDataForPlot(-5, 5).ToArray();

            //var series51 = MakeSeries(data);

            //var chart51 = MakeChart(series51, "x", "f(x)", "График f(x) пользовательского распределения",
            //    SeriesChartType.Column, 0.5, -5, 5);

            //////////////////////////////////////////////////////
            ///
            //var normalVar1 = new ExponentialDistribution(1.5);
            ////var normalVar1 = new UniformDistribution(0, 1) + new UniformDistribution(0, 1);

            //var series61 = MakeSeries(normalVar1.GetPDFDataForPlot().ToArray());

            //var chart61 = MakeChart(series61, "x", "f(x)", "График f(x) нормального распределения",
            //    SeriesChartType.Column, 1, -3, 5);

            //var series62 = MakeSeries(normalVar1.GetCDFDataForPlot().ToArray());

            //var chart62 = MakeChart(series62, "x", "F(x)", "График F(x) нормального распределения",
            //    SeriesChartType.Column, 1, -3, 5);

            //Console.WriteLine("\nНормальное распределение");
            //Console.WriteLine(normalVar1.SummaryInfo);

            ////////////////////////////////////////////////////

            //var customDistribution = new CauchyDistribution(0, 0.5);

            //var dataCount = 1000000;
            //var dataSampling = new List<double>();
            //for (var i = 0; i < dataCount; i++)
            //{
            //    dataSampling.Add(customDistribution.GetNewRandomValue());
            //}
            //dataSampling.Sort();

            //var (pdfValuesExpected, pdfValuesActual) = CalculateExpectedAndActualPDFValues(customDistribution, dataSampling.ToArray());

            //var chiSquareTest = new Test(pdfValuesExpected.Select(p => p.Y).ToArray(), pdfValuesActual.Select(p => p.Y).ToArray(), pdfValuesExpected.Count() - 1);

            //var series1 = MakeSeries(pdfValuesExpected);

            //var chart1 = MakeChart(series1, "x", "f(x)", "pdfValuesExpected",
            //    SeriesChartType.Column, 1, -1, 30);

            //var series2 = MakeSeries(pdfValuesActual);

            //var chart2 = MakeChart(series2, "x", "f(x)", "pdfValuesActual",
            //    SeriesChartType.Column, 1, -1, 30);


            ////////////////////////////////////////////////////


            var forms = MakeFormsFromCharts(new List<Chart> {
                chart51
            });

            Application.EnableVisualStyles();
            Application.Run(new MultiFormContext(forms.ToArray()));

            Console.ReadKey();
        }

        public static (Point[], Point[]) CalculateExpectedAndActualPDFValues(Distribution distribution, double[] variableValues)
        {
            //var numberOfIntervals = (int)Math.Ceiling(1 + 3.322 * Math.Log10(variableValues.Count()));
            var numberOfIntervals = 10;
            var numberOfPoints = numberOfIntervals;

            var min = variableValues.Min();
            var max = variableValues.Max();
            var intervalLength = (max - min) / numberOfIntervals;

            min = min + intervalLength;
            max = max - intervalLength;

            //var count1 = variableValues.Where(v => v < min + intervalLength).ToList();

            var breakPoints = new double[numberOfPoints];
            for (var i = 0; i < numberOfPoints; i++)
            {
                breakPoints[i] = min + i * intervalLength;
            }

            // считаем кол-во попаданий в интервалы
            var counts = new int[numberOfPoints];
            var FValues = new double[numberOfPoints];
            for (var i = 0; i < breakPoints.Length; i++)
            {
                var currentValue = breakPoints[i];

                var count = variableValues.Where(v => v <= currentValue).Count();
                counts[i] = count;
                FValues[i] = (double)count / variableValues.Length;
            }

            var pdfValuesActual = new Point[numberOfPoints - 1];
            for (int i = 0; i < pdfValuesActual.Length; i++)
            {
                var variableValue = breakPoints[i];
                pdfValuesActual[i] = new Point(variableValue, distribution.GetPdfValueAtPoint(variableValue));
            }

            var pdfValuesExpected = CalculateProbabilityFunctionValues(FValues, breakPoints, intervalLength);

            return (pdfValuesExpected, pdfValuesActual);
        }

        private static Point[] CalculateProbabilityFunctionValues(double[] FValues, double[] breakPoints, double intervalLength)
        {
            var probabilityFunctionValues = new Point[breakPoints.Length - 1];

            for (var i = 0; i < breakPoints.Length - 1; i++)
            {
                var funcValue = (FValues[i + 1] - FValues[i]) / intervalLength;
                probabilityFunctionValues[i] = new Point(breakPoints[i], funcValue);
            }

            return probabilityFunctionValues;
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
            //chart.ChartAreas[0].AxisY.Maximum = 3;
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
