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
    public partial class Task3 : Window
    {
        const double eps = Constants.Eps;

        const double variant = Constants.Var;

        Random rand = new();
        MainViewModel model;
        Range x_range, y_range;
        ScatterSeries scatterSeries;

        List<Point> points = new();

        List<DataPoint> GetCirclePoints()
        {
            List<DataPoint> DP = new();

            for (double rad = 0; rad <= Math.PI * 2; rad += eps)
            {
                DP.Add(new DataPoint(variant * Math.Cos(rad), variant * Math.Sin(rad)));
            }

            return DP;
        }
            
        double ApproximateSquare(ICollection<Point> points)
        {
            if (points.Count() == 0)
                throw new EmptyListException();

            double inside_circle = points.Where(p => Math.Pow(p.x, 2) + Math.Pow(p.y, 2) < Math.Pow(variant, 2)).Count();

            return inside_circle / points.Count() * x_range.Diff * y_range.Diff;
        }

        static double ExactSquare()
        {
            return Math.PI * Math.Pow(variant, 2);
        }

        List<Point> GeneratePoints(int n, Range x_range, Range y_range)
        {
            List<Point> points = new();
            for (int i = 0; i < n; i++)
                points.Add(Point.RandomPoint(x_range, y_range, rand));

            return points;
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

        public Task3()
        {
            InitializeComponent();

            model = new MainViewModel("Вычисление площади окружности");

            model.AddLineSeries(GetCirclePoints());

            x_range = new(-variant, variant);
            y_range = new(-variant, variant);

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