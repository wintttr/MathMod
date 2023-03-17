using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Legends;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace DiffEquation
{
    public class MainViewModel
    {
        public MainViewModel(String title)
        {
            Model = new PlotModel { Title = title };

            Model.Legends.Add(new Legend
            {
                LegendPosition = LegendPosition.RightTop,
            });
        }

        public FunctionSeries AddFunc(Func<double, double> f, Range r, double step = 0.001, String title = "")
        {
            FunctionSeries s = new FunctionSeries(f, r.Start, r.End, step, title);
            Model.Series.Add(s);

            return s;
        }

        public LineSeries AddLineSeries(ICollection<DataPoint> Points, String title = "")
        {
            var lineSeries = new LineSeries {  Title = title };

            lineSeries.Points.AddRange(Points);

            Model.Series.Add(lineSeries);

            return lineSeries;
        }

        public PlotModel Model { get; private set; }
    }
}
