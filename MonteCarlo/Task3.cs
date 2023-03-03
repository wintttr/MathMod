using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace MonteCarlo
{
    public class Task3 : BasicTask
    {
        const double variant = Constants.Var;
        const double eps = Constants.ComputeEps;

        public double ApprPI(ICollection<Point> points)
        {
            return ApproximateSquare(points) / Math.Pow(variant, 2);
        }

        public override string TaskTitle
        {
            get
            {
                return "Вычисление числа ПИ";
            }
        }

        static List<Point> GetCirclePoints()
        {
            List<Point> points = new();

            for (double rad = 0; rad <= Math.PI * 2; rad += eps)
            {
                points.Add(new(variant * Math.Cos(rad), variant * Math.Sin(rad)));
            }

            return points;
        }

        public override List<List<Point>> GetFuncs()
        {
            return new() { GetCirclePoints() };
        }

        public override bool isInFigure(Point p)
        {
            return Math.Pow(p.x, 2) + Math.Pow(p.y, 2) < Math.Pow(variant, 2);
        }

        public override Range GetXRange()
        {
            return new(-variant, variant);
        }

        public override Range GetYRange()
        {
            return new(-variant, variant);
        }

        override public double ExactSquare()
        {
            return Math.PI * Math.Pow(variant, 2);
        }

        public List<String> AdditiveCalcs(ICollection<Point> points)
        {
            return new()
            {
                String.Format("PI = {0:F4}", ApprPI(points))
            };
        }

        public List<String> AdditiveErrors(ICollection<Point> points)
        {
            double absolute = Math.Abs(Math.PI - ApprPI(points));
            double relative = absolute / ApprPI(points) * 100;
            return new()
            {
                String.Format("Аболютная погрешность PI: {0:F4}", absolute),
                String.Format("Относительная погрешность PI: {0:F4}%", relative)
            };
        }

        public override List<String> GetCalcs(ICollection<Point> points)
        {
            return new(Enumerable.Concat(DefaultCalcs(points), AdditiveCalcs(points)));
        }

        public override List<string> GetErrors(ICollection<Point> points)
        {
            return new(Enumerable.Concat(DefaultErrors(points), AdditiveErrors(points)));
        }
    }
}
