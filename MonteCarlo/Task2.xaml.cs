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
    public partial class Task2 : Window
    {
        const double eps = Constants.Eps;

        const double variant = Constants.Var;
        const double a = 0, b = 7;

        Random rand = new();
        MainViewModel model;
        Range x_range, y_range;
        ScatterSeries scatterSeries;

        List<Point> points = new();

        static double f(double x)
        {
            return Math.Sqrt(29 - variant*Math.Pow(Math.Cos(x), 2));
        }

        static double FindFuncMax(Func<double, double> f, double a, double b)
        {
            double max = f(a);
            for (double i = a; i <= b; i += eps)
                max = Math.Max(f(i), max);
            return max;
        }

        double ApproximateSquare(ICollection<Point> points)
        {
            if (points.Count() == 0)
                throw new EmptyListException();

            double under_function = points.Where(p => p.y <= f(p.x)).Count();

            return under_function / points.Count() * x_range.Diff * y_range.Diff;
        }

        static double ExactSquare()
        {
            double sum = 0;
            for (double x = a; x < b - eps; x += eps)
                sum += eps * f(x);
            return sum;
        }

        List<Point> GeneratePoints(int n, Range x_range, Range y_range)
        {
            List<Point> points = new();
            for (int i = 0; i < n; i++)
                points.Add(Point.RandomPoint(x_range, y_range, rand));

            return points;
        }

        public Task2()
        {
            InitializeComponent();

            model = new MainViewModel("Вычисление интеграла");

            model.AddFunc(f, 0, 7);

            x_range = new(0, 7);
            y_range = new(0, FindFuncMax(f, a, b));

            scatterSeries = model.AddScatterPoints();

            plot.Model = model.MyModel;
        }

        private void exactCalc_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(String.Format("Точная площадь: {0:F4}", ExactSquare()));
        }

        private void approxCalc_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBox.Show(String.Format("Приближённая площадь: {0:F4}", ApproximateSquare(points)));
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