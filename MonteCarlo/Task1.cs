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
    public class Task1 : BasicTask
    {
        const double variant = Constants.Var;
        const double eps = Constants.GraphicsEps;

        public override string TaskTitle
        {
            get
            {
                return "Вычисление площади треугольника";
            }
        }

        static public double F1(double x)
        {
            return 10 * x / variant;
        }

        static public double F2(double x)
        {
            return 10 * (x - 20) / (variant - 20) + 20;
        }
        override public bool isInFigure(Point p)
        {
            return F1(p.x) >= p.y && p.y >= F2(p.x);
        }

        static public Point cross()
        {
            double x = variant - 0.1 * Math.Pow(variant, 2) + 2 * variant;
            double y = F1(x);
            return new Point(x, y);
        }

        public override Range GetXRange()
        {
            return new(0, cross().x);
        }

        public override Range GetYRange()
        {
            var ys = new List<double>() { Task1.F1(0), Task1.F2(0), Task1.cross().y };

            return new(ys.Min(), ys.Max());
        }

        override public double ExactSquare()
        {
            double h = cross().x;
            return 0.5 * h * Math.Abs(F1(0) - F2(0));
        }
            
        override public List<List<Point>> GetFuncs()
        {
            Range x_range = GetXRange();

            return new() 
            { 
                GetFunctionTable(F1, x_range, eps), 
                GetFunctionTable(F2, x_range, eps) 
            };
        }
    }
}