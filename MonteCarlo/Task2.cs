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
    public class Task2 : BasicTask
    {
        const double eps = Constants.ComputeEps;
        const double gEps = Constants.GraphicsEps;

        const double variant = Constants.Var;
        const double a = 0, b = 7;

        public override String TaskTitle
        {
            get
            {
                return "Вычисление интеграла";
            }
        }

        public override bool isInFigure(Point p)
        {
            return p.y <= f(p.x);
        }

        public override Range GetXRange()
        {
            return new(0, 7);
        }

        public override Range GetYRange()
        {
            return new(0, FindFuncMax(f, a, b));
        }

        static double f(double x)
        {
            return Math.Sqrt(29 - variant * Math.Pow(Math.Cos(x), 2));
        }

        static double FindFuncMax(Func<double, double> f, double a, double b)
        {
            double max = f(a);
            for (double i = a; i <= b; i += eps)
                max = Math.Max(f(i), max);
            return max;
        }

        override public double ExactSquare()
        {
            double sum = 0;
            for (double x = a; x < b - eps; x += eps)
                sum += eps * f(x);
            return sum;
        }

        public override List<List<Point>> GetFuncs()
        {
            return new() { GetFunctionTable(f, new(0,7), gEps) };
        }

    }
}