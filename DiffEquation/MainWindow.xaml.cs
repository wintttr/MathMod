using OxyPlot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DiffEquation
{
    using func2 = Func<double, double, double>;
    using func1 = Func<double, double>;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static double eps = 0.001;

        (func2 x, func2 y, func1 x_s, func1 y_s, Point x0y0) get_test1()
        {
            static double f_x(double x, double y)
            {
                return -2 * x + 4 * y;
            }

            static double f_y(double x, double y)
            {
                return -x + 3 * y;
            }

            static double f_x_s(double t)
            {
                return 4 * Math.Exp(-t) - Math.Exp(2 * t);
            }

            static double f_y_s(double t)
            {
                return Math.Exp(-t) - Math.Exp(2 * t);
            }

            return (f_x, f_y, f_x_s, f_y_s, new(3,0));
        }

        (func2 x, func2 y, func1 x_s, func1 y_s, Point x0y0) get_test2()
        {
            static double f_x(double x, double y)
            {
                return y;
            }

            static double f_y(double x, double y)
            {
                return 2 * y;
            }

            static double f_x_s(double t)
            {
                return Math.Exp(2 * t) + 1;
            }

            static double f_y_s(double t)
            {
                return 2 * Math.Exp(2 * t);
            }

            return (f_x, f_y, f_x_s, f_y_s, new(2, 2));
        }

        public MainWindow()
        {
            InitializeComponent();
            SolveAndShow(1);
        }

        public void SolveAndShow(int n)
        {
            (func2 f_x, func2 f_y, func1 x_s, func1 y_s, Point x0y0) = get_test2();

            Range r = new(0, 1);

            List<Point> s = RungeKutta.Solve2EquationSystem(f_x, f_y, r, n, x0y0);

            List<double> x = new(), y = new();

            for (double t = r.Start; t <= r.End; t += eps)
            {
                x.Add(x_s(t));
                y.Add(y_s(t));
            }

            MainViewModel m = new("Метод Рунге-Кутта");

            m.AddLineSeries(new List<DataPoint>(s.Select(p => new DataPoint(p.x, p.y)).OrderByDescending(p => p.X)), "Приближённое решение");
            m.AddLineSeries(new List<DataPoint>(from p in x.Zip(y) orderby p.First descending select new DataPoint(p.First, p.Second)), "Точное решение");

            plot.Model = m.Model;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int n = Int32.Parse(n_num.Text);

                SolveAndShow(n);
            }
            catch (Exception)
            {
                MessageBox.Show("ERROR ERROR ERROR");
            }
        }
    }
}
