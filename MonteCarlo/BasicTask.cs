using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MonteCarlo
{
    abstract public class BasicTask
    {
        abstract public String TaskTitle { get; }

        abstract public bool isInFigure(Point p);

        abstract public double ExactSquare();

        abstract public Range GetXRange();
        abstract public Range GetYRange();
        abstract public List<List<Point>> GetFuncs();

        static protected List<Point> GetFunctionTable(Func<double, double> f, Range r, double eps)
        {
            List<Point> table = new();
            for (double x = r.Start; x <= r.End; x += eps)
                table.Add(new(x, f(x)));
            return table;
        }

        public List<String> DefaultErrors(ICollection<Point> points)
        {
            double appr = ApproximateSquare(points);
            double exact = ExactSquare();

            double absolute = Math.Abs(appr - exact);
            double relative = absolute / exact * 100;

            
            String absoluteError = String.Format("Абсолютная погрешность: {0:F4}", absolute);
            String relativeError = String.Format("Относительная погрешность: {0:F4}%", relative);

            return new() { absoluteError, relativeError };
        }

        public List<String> DefaultCalcs(ICollection<Point> points)
        {
            double appr = ApproximateSquare(points);
            double exact = ExactSquare();

            String absoluteError = String.Format("Точная площадь: {0:F4}", exact);
            String relativeError = String.Format("Приближённая площадь: {0:F4}", appr);

            return new() { absoluteError, relativeError };
        }

        abstract public List<String> GetErrors(ICollection<Point> points);
        abstract public List<String> GetCalcs(ICollection<Point> points);


        virtual public double ApproximateSquare(ICollection<Point> points)
        {
            if (points.Count() == 0)
                throw new EmptyListException();

            double in_triangle = points.Where(p => isInFigure(p)).Count();

            return in_triangle / points.Count() * GetXRange().Diff * GetYRange().Diff;
        }


        static public List<Point> GeneratePoints(int n, Range x_range, Range y_range, Random rand)
        {
            List<Point> points = new();
            for (int i = 0; i < n; i++)
                points.Add(Point.RandomPoint(x_range, y_range, rand));
            return points;
        }
    }
}
