using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MonteCarlo
{
    abstract public class BasicTask
    {
        List<Point> points = new();

        abstract public String TaskTitle { get; }

        abstract public bool isInFigure(Point p);

        abstract public double ExactSquare();

        abstract public Range GetXRange();
        abstract public Range GetYRange();
        abstract public void AddFuncs(MainViewModel model);

        virtual public List<String> DefaultErrors()
        {
            double appr = ApproximateSquare();
            double exact = ExactSquare();

            double absolute = Math.Abs(appr - exact);
            double relative = absolute / exact * 100;

            
            String absoluteError = String.Format("Абсолютная погрешность: {0:F4}", absolute);
            String relativeError = String.Format("Относительная погрешность: {0:F4}%", relative);

            return new() { absoluteError, relativeError };
        }

        virtual public List<String> DefaultCalcs()
        {
            double appr = ApproximateSquare();
            double exact = ExactSquare();

            String absoluteError = String.Format("Точная площадь: {0:F4}", exact);
            String relativeError = String.Format("Приближённая площадь: {0:F4}", appr);

            return new() { absoluteError, relativeError };
        }

        virtual public List<String> AdditiveErrors()
        {
            return new();
        }

        virtual public List<String> AdditiveCalcs()
        {
            return new();
        }

        virtual public double ApproximateSquare()
        {
            if (points.Count() == 0)
                throw new EmptyListException();

            double in_triangle = points.Where(p => isInFigure(p)).Count();

            return in_triangle / points.Count() * GetXRange().Diff * GetYRange().Diff;
        }


        public List<Point> GeneratePoints(int n, Range x_range, Range y_range, Random rand)
        {
            points = new();
            for (int i = 0; i < n; i++)
                points.Add(Point.RandomPoint(x_range, y_range, rand));
            return points;
        }
    }
}
