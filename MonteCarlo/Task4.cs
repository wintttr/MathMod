using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonteCarlo
{
    public class Task4 : BasicTask
    {
        const double eps = Constants.ComputeEps;

        const double variant = Constants.Var;

        const double A = variant + 10;
        const double B = variant - 10;

        double a, b;

        public override string TaskTitle 
        {
            get
            {
                return "Вычисление площади фигуры";
            }
        }

        public override double ExactSquare()
        {
            double sum = 0;
            for (double x = 0; x < Math.PI * 2 - eps; x += eps)
                sum += eps * phi2(x);
            return sum / 2;
        }

        public Task4()
        {
            a = Math.Abs(phi(0) * Math.Cos(0));
            b = Math.Abs(phi(0) * Math.Sin(0));
            for (double i = 0; i < Math.PI / 2; i += eps)
            {
                a = Math.Max(a, Math.Abs(phi(i) * Math.Cos(i)));
                b = Math.Max(b, Math.Abs(phi(i) * Math.Sin(i)));
            }
        }

        static double phi(double x)
        {
            return Math.Sqrt(phi2(x));
        }
        static double phi2(double x)
        {
            return A * Math.Pow(Math.Cos(x), 2) + B * Math.Pow(Math.Sin(x), 2);
        }

        static List<Point> GetFigurePoints()
        {
            List<Point> DP = new();

            for (double rad = 0; rad <= Math.PI * 2; rad += eps)
            {
                DP.Add(new Point(phi(rad) * Math.Cos(rad), phi(rad) * Math.Sin(rad)));
            }

            return DP;
        }

        public override bool isInFigure(Point p)
        {
            double r = Math.Sqrt(Math.Pow(p.x, 2) + Math.Pow(p.y, 2));

            double rad;

            if (p.x > 0) rad = Math.Atan(p.y / p.x);
            else if (p.x < 0) rad = Math.PI + Math.Atan(p.y / p.x);
            else if (p.y > 0) rad = Math.PI / 2;
            else if (p.y < 0) rad = -Math.PI / 2;
            else rad = 0;

            if (r < phi(rad))
                return true;

            return false;
        }

        public override List<List<Point>> GetFuncs()
        {
            return new() { GetFigurePoints() };
        }

        public override Range GetXRange()
        {
            
            return new(-a, a);
        }

        public override Range GetYRange()
        {
            return new(-b, b);
        }

        public override List<string> GetCalcs(ICollection<Point> points)
        {
            return DefaultCalcs(points);
        }

        public override List<string> GetErrors(ICollection<Point> points)
        {
            return DefaultErrors(points);
        }
    }
}
