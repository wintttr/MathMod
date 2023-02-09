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

namespace MinQ
{
    public class MainViewModel
    {
        public MainViewModel(String title, double MinX, double MaxX, double step = 0.1)
        {
            _step = step;
            _MinX = MinX;
            _MaxX = MaxX;

            MyModel = new PlotModel { Title = title };

            MyModel.Legends.Add(new Legend
            {
                LegendPosition = LegendPosition.RightTop,
            });
        }

        public Series AddFunc(Func<double, double> f, String title)
        {
            Series s = new FunctionSeries(f, _MinX, _MaxX, _step, title);
            MyModel.Series.Add(s);
            s.Tag = f;
            return s;
        }

        public Series AddScatterPoints(ICollection<ScatterPoint> Points, String title)
        {
            var scatterSeries = new ScatterSeries { MarkerType = MarkerType.Circle, Title = title};

            scatterSeries.Points.AddRange(Points);

            MyModel.Series.Add(scatterSeries);

            return scatterSeries;
        }

        public PlotModel MyModel { get; private set; }
        private readonly double _step;
        private readonly double _MinX, _MaxX;
    }
}
