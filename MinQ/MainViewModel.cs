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
        public delegate double func(double x);

        Func<double, double> GetLinear(ICollection<double> Xs, ICollection<double> Ys)
        {
            (double a, double b) = Compute.LinearInterp(Xs, Ys);
            return (x) => Compute.Linear(x, a, b);
        }

        Func<double, double> GetPower(ICollection<double> Xs, ICollection<double> Ys)
        {
            (double a, double b) = Compute.PowerInterp(Xs, Ys);
            return (x) => Compute.Power(x, a, b);
        }

        Func<double, double> GetExponential(ICollection<double> Xs, ICollection<double> Ys)
        {
            (double a, double b) = Compute.ExponentialInterp(Xs, Ys);
            return (x) => Compute.Exponential(x, a, b);
        }

        Func<double, double> GetQuadratic(ICollection<double> Xs, ICollection<double> Ys)
        {
            (double a, double b, double c) = Compute.QuadraticInterp(Xs, Ys);
            return (x) => Compute.Quadratic(x, a, b, c);
        }

        public MainViewModel()
        {
            List<double> X = new() { 10, 15, 20, 25, 30, 35 };
            List<double> Y = new() { 4.3, 3.3, 2.68, 2.25, 1.9, 1.7 };

            MyModel = new PlotModel { Title = "Метод минимальных квадратов", AxisTierDistance = 0.1 };

            MyModel.Legends.Add(new Legend
            {
                LegendPosition = LegendPosition.RightTop,
            });

            var scatterSeries = new ScatterSeries { MarkerType = MarkerType.Circle, Title = "Original"};
            foreach((double x, double y) in X.Zip(Y))
            {
                scatterSeries.Points.Add(new ScatterPoint(x, y, 4));
            }

            MyModel.Series.Add(scatterSeries);

            MyModel.Series.Add(new FunctionSeries(GetLinear(X, Y), X.Min(), X.Max(), 0.1, "Linear"));
            MyModel.Series.Add(new FunctionSeries(GetPower(X, Y), X.Min(), X.Max(), 0.1, "Power"));
            MyModel.Series.Add(new FunctionSeries(GetExponential(X, Y), X.Min(), X.Max(), 0.1, "Exp"));
            MyModel.Series.Add(new FunctionSeries(GetQuadratic(X, Y), X.Min(), X.Max(), 0.1, "Quadratic"));
        }

        public PlotModel MyModel { get; private set; }
    }
}
