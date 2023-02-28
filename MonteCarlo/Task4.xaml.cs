using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MonteCarlo
{
    public partial class Task4 : Window
    {
        const double eps = Constants.Eps;

        const double variant = Constants.Var;

        const double A = variant + 10;
        const double B = variant - 10;

        Random rand = new();
        MainViewModel model;
        Range x_range, y_range;
        ScatterSeries scatterSeries;

        List<Point> points = new();

        static double phi(double x)
        {
            return Math.Sqrt(A*Math.Pow(Math.Cos(x), 2) + B*Math.Pow(Math.Sin(x), 2));
        }

        List<DataPoint> GetFigurePoints()
        {
            List<DataPoint> DP = new();

            for (double rad = 0; rad <= Math.PI * 2; rad += eps)
            {
                DP.Add(new DataPoint(phi(rad)*Math.Cos(rad), phi(rad)*Math.Sin(rad)));
            }

            return DP;
        }

        int PointCount(ICollection<Point> points)
        {
            if (points.Count() == 0)
                throw new EmptyListException();

            int inside_circle = 0;
            foreach (var p in points) {
                double r = Math.Sqrt(Math.Pow(p.x, 2) + Math.Pow(p.y, 2));
                double rad = 0;

                if (p.x > 0) rad = Math.Atan(p.y / p.x);
                else if (p.x < 0) rad = Math.PI + Math.Atan(p.y / p.x);
                else if (p.y > 0) rad = Math.PI / 2;
                else if (p.y < 0) rad = -Math.PI / 2;
                else rad = 0;

                if (r < phi(rad))
                    inside_circle += 1;
            }

            return inside_circle;
        }

        List<Point> GeneratePoints(int n, Range x_range, Range y_range)
        {
            List<Point> points = new();
            for (int i = 0; i < n; i++)
                points.Add(Point.RandomPoint(x_range, y_range, rand));

            return points;
        }

        public Task4()
        {
            InitializeComponent();

            model = new MainViewModel("");

            model.AddLineSeries(GetFigurePoints());

            double a = phi(0) * Math.Cos(0), b = phi(0) * Math.Sin(0);
            for(double i = 0; i < Math.PI / 2; i += eps)
            {
                a = Math.Max(a, phi(i) * Math.Cos(i));
                b = Math.Max(b, phi(i) * Math.Sin(i));
            }

            x_range = new(-a, a);
            y_range = new(-b, b);

            scatterSeries = model.AddScatterPoints();

            plot.Model = model.MyModel;
        }

        private void pointcountCalc_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBox.Show(String.Format("Количество точек внутри фигуры: {0}", PointCount(points)));
            }
            catch (EmptyListException)
            {
                MessageBox.Show("Ашипка! Сгенерируйте точки.");
            }
        }

        private void generateButton_Click(object sender, RoutedEventArgs e)
        {
            int points_count;
            try
            {
                points_count = Int32.Parse(pointsCount.Text);

                points = GeneratePoints(points_count, x_range, y_range);

                List<ScatterPoint> scatterPoints = new(from p in points select new ScatterPoint(p.x, p.y, 5));
                model.UpdateScatterPoints(scatterPoints, scatterSeries);

                plot.Model = model.MyModel;
                plot.InvalidatePlot();
            }
            catch (FormatException)
            {
                MessageBox.Show("Число имеет неверный формат.");
            }
        }
    }
}