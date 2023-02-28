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

    public partial class Task1 : Window
    {
        const double variant = Constants.Var;

        Random rand = new();
        MainViewModel model;
        Range x_range, y_range;
        ScatterSeries scatterSeries;

        List<Point> points = new();

        static double F1(double x)
        {
            return 10 * x / variant;
        }

        static double F2(double x)
        {
            return 10 * (x - 20) / (variant - 20) + 20;
        }

        static Point cross(double n)
        {
            double x = n - 0.1 * Math.Pow(n, 2) + 2 * n;
            double y = F1(x);
            return new Point(x, y);
        }

        double ApproximateSquare(ICollection<Point> points)
        {
            if (points.Count() == 0)
                throw new EmptyListException();

            double in_triangle = points.Where(p => (F1(p.x) >= p.y && p.y >= F2(p.x))).Count();

            return in_triangle / points.Count() * x_range.Diff * y_range.Diff;
        }

        static double ExactSquare()
        {
            double h = cross(variant).x;
            return 0.5 * h * Math.Abs(F1(0) - F2(0));
        }

        List<Point> GeneratePoints(int n, Range x_range, Range y_range)
        {
            List<Point> points = new();
            for (int i = 0; i < n; i++)
                points.Add(Point.RandomPoint(x_range, y_range, rand));

            return points;
        }

        public Task1()
        {
            InitializeComponent();

            model = new MainViewModel("Вычисление площади треугольника");

            model.AddFunc((x) => F1(x), 0, cross(variant).x);
            model.AddFunc((x) => F2(x), 0, cross(variant).x);

            var ys = new List<double>() { F1(0), F2(0), cross(variant).y };
            x_range = new(0, cross(variant).x);
            y_range = new(ys.Min(), ys.Max());

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

        private void infButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double appr = ApproximateSquare(points);
                double exact = ExactSquare();

                double absolute = Math.Abs(appr - exact);
                double relative = absolute / exact * 100;

                MessageBox.Show(String.Format("Абсолютная погрешность: {0:F4}\nОтносительная погрешность: {1:F4}%", absolute, relative));

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