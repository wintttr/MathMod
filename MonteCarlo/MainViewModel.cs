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

namespace MonteCarlo
{

    public class MainViewModel
    {
        Dictionary<String, ScatterSeries> _points = new();

        public MainViewModel(String title)
        {
            MyModel = new PlotModel { Title = title };

            MyModel.Legends.Add(new Legend
            {
                LegendPosition = LegendPosition.RightTop,
            });
        }

        public FunctionSeries AddFunc(Func<double, double> f, double MinX, double MaxX, double step = 0.001, String title = "")
        {
            FunctionSeries s = new FunctionSeries(f, MinX, MaxX, step, title);
            MyModel.Series.Add(s);

            return s;
        }

        public ScatterSeries AddScatterSeries(String Tag, String title = "")
        {
            ScatterSeries scatterSeries = new ScatterSeries { MarkerType = MarkerType.Circle, Title = title};

            MyModel.Series.Add(scatterSeries);

            _points.Add(Tag, scatterSeries);

            return scatterSeries;
        }

        public ScatterSeries AddScatterPoints(ICollection<ScatterPoint> Points, String Tag, String title = "")
        {
            ScatterSeries scatterSeries = _points[Tag];

            scatterSeries.Points.AddRange(Points);

            return scatterSeries;
        }

        public ScatterSeries UpdateScatterPoints(ICollection<ScatterPoint> Points, String Tag)
        {
            ScatterSeries scatterSeries = _points[Tag];

            scatterSeries.Points.Clear();

            scatterSeries.Points.AddRange(Points);

            return _points[Tag];
        }

        public LineSeries AddLineSeries(ICollection<DataPoint> Points, String title = "")
        {
            var lineSeries = new LineSeries {  Title = title };

            lineSeries.Points.AddRange(Points);

            MyModel.Series.Add(lineSeries);

            return lineSeries;
        }

        public PlotModel MyModel { get; private set; }
    }
}
