using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffEquation
{
    class RungeKutta
    {
        static public List<Point> Solve2EquationSystem(Func<double, double, double> f, Func<double, double, double> g, Range t_range, int n, Point x0y0)
        {
            List<double> x = new();
            List<double> y = new();

            double h = t_range.Diff / n;

            x.Add(x0y0.x);
            y.Add(x0y0.y);

            for (int k = 0; k < n; k++)
            {
                double K1 = h * f(x[k], y[k]);
                double L1 = h * g(x[k], y[k]);
                double K2 = h * f(x[k] + K1 / 2.0, y[k] + L1 / 2.0);
                double L2 = h * g(x[k] + K1 / 2.0, y[k] + L1 / 2.0);
                double K3 = h * f(x[k] + K2 / 2.0, y[k] + L2 / 2.0);
                double L3 = h * g(x[k] + K2 / 2.0, y[k] + L2 / 2.0);
                double K4 = h * f(x[k] + K3, y[k] + L3);
                double L4 = h * g(x[k] + K3, y[k] + L3);

                double delta_x = (K1 + 2 * K2 + 2 * K3 + K4) / 6.0;
                double next_x = x[k] + delta_x;

                double delta_y = (L1 + 2 * L2 + 2 * L3 + L4) / 6.0;
                double next_y = y[k] + delta_y;

                x.Add(next_x);
                y.Add(next_y);
            }

            return new(x.Zip(y).Select(p => new Point(p.First, p.Second)));
        }
    }
}
